using CodeWalker.GameFiles;
using CodeWalker.Properties;
using CodeWalker.Rendering;
using CodeWalker.World;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Device = SharpDX.Direct3D11.Device;
using DeviceContext = SharpDX.Direct3D11.DeviceContext;

namespace CodeWalker
{
    public partial class RealMapForm : Form, DXForm
    {
        public Form Form { get { return this; } }

        public Renderer Renderer = null;
        public object RenderSyncRoot { get { return Renderer.RenderSyncRoot; } }

        public const float WorldXMin = -5355.0f;
        public const float WorldXMax = 6167.0f;
        public const float WorldYMin = -3833.0f;
        public const float WorldYMax = 7688.0f;
        public static readonly float SpanX = WorldXMax - WorldXMin;
        public static readonly float SpanY = WorldYMax - WorldYMin;

        const float MapLightDirX = 2.25f;
        const float MapLightDirY = 1.15f;
        const float MapLookTimeOfDay = 0.0f;
        const int OutputTileSize4K = 4096;

        static readonly SharpDX.Color OceanClearColour = new SharpDX.Color(0 / 255f, 24 / 255f, 27 / 255f, 1f);

        bool mapStreetLightsEnabled = false;
        bool toolsHiddenForCapture = false;
        bool toolsVisibleBeforeCapture = true;
        bool worldPanelVisibleBeforeCapture = true;
        bool showMainMap = true;
        bool showCayoPerico = false;
        bool showNorthYankton = false;
        bool suppressDlcEvents = false;

        volatile bool formopen = false;
        volatile bool running = false;
        volatile bool pauserendering = false;
        volatile bool worldReady = false;

        Stopwatch frametimer = new Stopwatch();
        Space space = new Space();
        Camera camera;
        Timecycle timecycle;
        Weather weather;
        Clouds clouds;
        Water water = new Water();

        Entity camEntity = new Entity();

        bool MouseLButtonDown = false;
        bool MouseRButtonDown = false;
        int MouseX;
        int MouseY;
        System.Drawing.Point MouseDownPoint;
        System.Drawing.Point MouseLastPoint;
        int MapViewDragX = 0;
        int MapViewDragY = 0;

        public GameFileCache GameFileCache { get; } = GameFileCacheFactory.Create();
        InputManager Input = new InputManager();
        bool initedOk = false;

        Dictionary<MetaHash, YmapFile> renderworldVisibleYmapDict = new Dictionary<MetaHash, YmapFile>();

        volatile bool captureRequested = false;
        volatile bool batchRunning = false;
        volatile bool batchCancel = false;
        System.Drawing.Rectangle pendingCaptureRect;
        int pendingCaptureZ, pendingCaptureX, pendingCaptureY;
        int pendingOutputSize;
        string pendingOutputPath;

        enum CapturePhase { Idle, Settling, Capture }
        CapturePhase capturePhase = CapturePhase.Idle;
        int settleElapsedFrames = 0;
        int settleMinFrames = 60;
        int settleStableFrames = 0;
        int settleLastLoadedCount = -1;
        const int SettleStableRequired = 24;
        const int SettleMaxFrames = 3600;
        readonly Queue<TileJob> tileQueue = new Queue<TileJob>();
        int batchTotal = 0;
        int batchDone = 0;
        int batchSkipped = 0;

        struct TileJob
        {
            public int Z, X, Y;
            public string Path;
        }

        struct TileBounds
        {
            public float X0, X1, YTop, YBottom;
            public float CenterX { get { return (X0 + X1) * 0.5f; } }
            public float CenterY { get { return (YTop + YBottom) * 0.5f; } }
            public float Width { get { return X1 - X0; } }
            public float Height { get { return YTop - YBottom; } }
        }

        public RealMapForm()
        {
            InitializeComponent();

            Renderer = new Renderer(this, GameFileCache);
            camera = Renderer.camera;
            timecycle = Renderer.timecycle;
            weather = Renderer.weather;
            clouds = Renderer.clouds;

            initedOk = Renderer.Init();

            Renderer.controllightdir = true;
            Renderer.controltimeofday = false;
            Renderer.timerunning = false;
            Renderer.lightdirx = MapLightDirX;
            Renderer.lightdiry = MapLightDirY;
            Renderer.rendercollisionmeshes = false;
            Renderer.renderclouds = false;
            Renderer.rendermoon = false;
            Renderer.renderskeletons = false;
            Renderer.renderfragwindows = false;
            Renderer.renderskydome = false;
            Renderer.renderlights = false;
            Renderer.renderlodlights = false;
            Renderer.renderdistlodlights = false;
            Renderer.renderartificialambientlight = false;
            Renderer.rendernaturalambientlight = true;
            Renderer.rendertimedents = true;
            Renderer.rendertimedentsalways = false;
            Renderer.timeofday = MapLookTimeOfDay;

            showMainMap = true;
            showCayoPerico = false;
            showNorthYankton = false;
            Renderer.ShowGTAVMap = showMainMap;
            Renderer.ShowCayoPerico = showCayoPerico;
            Renderer.ShowNorthYankton = showNorthYankton;

            string defaultOut = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                "CodeWalker RealMap", "tiles", "realmap");
            OutputFolderTextBox.Text = defaultOut;
            OutputTileSizeNumeric.Value = OutputTileSize4K;

            GTAFolder.UpdateEnhancedFormTitle(this);
        }

        public void InitScene(Device device)
        {
            int width = ClientSize.Width;
            int height = ClientSize.Height;

            try
            {
                Renderer.DeviceCreated(device, width, height);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading shaders!\n" + ex.ToString());
                return;
            }

            camera.FollowEntity = camEntity;
            camEntity.Position = new Vector3(0, 0, 0);
            camEntity.Orientation = Quaternion.LookAtLH(Vector3.Zero, Vector3.Up, Vector3.ForwardLH);

            Renderer.SetCameraMode("2D Map");
            Renderer.ShowGTAVMap = showMainMap;
            Renderer.ShowCayoPerico = showCayoPerico;
            Renderer.ShowNorthYankton = showNorthYankton;
            Renderer.HideShipsAndMloShells = true;
            Renderer.ShowScriptedYmaps = false;
            Renderer.renderinteriors = false;
            Renderer.waitforchildrentoload = true;
            ApplyFlatMapLighting();

            GoToWorldPosition(200f, -900f, SpanY / 8f);
            WindowState = FormWindowState.Maximized;

            formopen = true;
            new Thread(new ThreadStart(ContentThread)).Start();
            frametimer.Start();
        }

        public void CleanupScene()
        {
            formopen = false;
            batchCancel = true;
            Renderer.DeviceDestroyed();

            int count = 0;
            while (running && (count < 5000))
            {
                Thread.Sleep(1);
                count++;
            }
        }

        public void RenderScene(DeviceContext context)
        {
            float elapsed = (float)frametimer.Elapsed.TotalSeconds;
            frametimer.Restart();

            if (pauserendering) return;

            GameFileCache.BeginFrame();

            if (!Monitor.TryEnter(Renderer.RenderSyncRoot, 50))
            {
                return;
            }

            UpdateControlInputs(elapsed);
            space.Update(elapsed);
            Renderer.Update(elapsed, MouseLastPoint.X, MouseLastPoint.Y);

            if (ShadowsOffCheckBox.Checked || batchRunning || capturePhase != CapturePhase.Idle)
            {
                ApplyFlatMapLightingLocked();
            }

            TickCaptureState();

            Renderer.BeginRender(context);
            Renderer.RenderSkyAndClouds();
            Renderer.SelectedDrawable = null;

            if (worldReady)
            {
                RenderWorld();
            }

            Renderer.RenderQueued();
            Renderer.RenderFinalPass();

            if (captureRequested)
            {
                captureRequested = false;
                Bitmap capturedBitmap = null;

                try
                {
                    using (var bmp = Renderer.DXMan.CaptureBackbufferRectangle(
                        pendingCaptureRect.X, pendingCaptureRect.Y, pendingCaptureRect.Width, pendingCaptureRect.Height))
                    {
                        if (bmp == null)
                        {
                            throw new Exception("Could not read the render buffer.");
                        }
                        capturedBitmap = new Bitmap(bmp);
                    }
                }
                catch (Exception ex)
                {
                    capturedBitmap?.Dispose();
                    BeginInvoke(new Action(() =>
                    {
                        UpdateStatus("Capture failed: " + ex.Message);
                        if (batchRunning)
                        {
                            StopBatch("Capture failed");
                        }
                    }));
                }

                Renderer.EndRender();
                Monitor.Exit(Renderer.RenderSyncRoot);

                if (capturedBitmap != null)
                {
                    int z = pendingCaptureZ, x = pendingCaptureX, y = pendingCaptureY;
                    int outSize = pendingOutputSize;
                    string path = pendingOutputPath;
                    Task.Run(() => FinishCapturedTile(capturedBitmap, z, x, y, outSize, path));
                }
                else if (batchRunning)
                {
                    BeginInvoke(new Action(AdvanceBatchAfterSave));
                }

                return;
            }

            Renderer.EndRender();
            Monitor.Exit(Renderer.RenderSyncRoot);
        }

        public void BuffersResized(int w, int h)
        {
            Renderer.BuffersResized(w, h);
        }

        public bool ConfirmQuit()
        {
            if (batchRunning)
            {
                return MessageBox.Show("A RealMap batch is running. Quit anyway?", "Confirm quit",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
            }
            return true;
        }

        private bool IsNightMode
        {
            get { return mapStreetLightsEnabled; }
        }

        private void RenderWorld()
        {
            renderworldVisibleYmapDict.Clear();

            int hour = ((int)MapLookTimeOfDay) % 24;
            MetaHash weathertype = new MetaHash(0);

            space.GetVisibleYmaps(camera, hour, weathertype, renderworldVisibleYmapDict);
            Renderer.RenderWorld(renderworldVisibleYmapDict, space.TemporaryEntities);

            if (water.Inited)
            {
                var quads = water.GetVisibleQuads(camera, water.WaterQuads);
                Renderer.RenderWaterQuads(quads);
            }
        }

        private void ApplyFlatMapLighting()
        {
            lock (Renderer.RenderSyncRoot)
            {
                ApplyFlatMapLightingLocked();
            }
        }

        private void ApplyFlatMapLightingLocked()
        {
            bool lightsOn = mapStreetLightsEnabled;

            Renderer.StudioLightingOverride = false;
            Renderer.controllightdir = true;
            Renderer.controltimeofday = false;
            Renderer.timerunning = false;
            Renderer.lightdirx = MapLightDirX;
            Renderer.lightdiry = MapLightDirY;
            Renderer.SetTimeOfDay(MapLookTimeOfDay);

            Renderer.renderskydome = false;
            Renderer.renderclouds = false;
            Renderer.rendernaturalambientlight = true;
            Renderer.renderartificialambientlight = true;
            Renderer.rendertimedents = true;
            Renderer.rendertimedentsalways = false;

            Renderer.renderdistlodlights = lightsOn;
            Renderer.renderlodlights = lightsOn;
            Renderer.renderlights = lightsOn;

            Renderer.ShowGTAVMap = showMainMap;
            Renderer.ShowCayoPerico = showCayoPerico;
            Renderer.ShowNorthYankton = showNorthYankton;

            bool hideShips = HideShipsCheckBox == null || HideShipsCheckBox.Checked;
            Renderer.HideShipsAndMloShells = hideShips;
            Renderer.ShowScriptedYmaps = !hideShips;
            Renderer.renderinteriors = !hideShips;

            if (Renderer.shaders != null)
            {
                Renderer.shaders.shadows = !ShadowsOffCheckBox.Checked;
                Renderer.shaders.hdr = false;
            }

            Renderer.DXMan?.SetClearColour(OceanClearColour);
            Renderer.MapViewDetail = (float)MapDetailNumeric.Value;
        }

        private void HidePanelsForCapture()
        {
            if (toolsHiddenForCapture) return;
            toolsVisibleBeforeCapture = ToolsPanel.Visible;
            worldPanelVisibleBeforeCapture = WorldPanel.Visible;
            toolsHiddenForCapture = true;
            ToolsPanel.Visible = false;
            ToolsPanelShowButton.Visible = false;
            WorldPanel.Visible = false;
            WorldPanelShowButton.Visible = false;
        }

        private void RestorePanelsAfterCapture()
        {
            if (!toolsHiddenForCapture) return;
            toolsHiddenForCapture = false;
            ToolsPanel.Visible = toolsVisibleBeforeCapture;
            ToolsPanelShowButton.Visible = !toolsVisibleBeforeCapture;
            WorldPanel.Visible = worldPanelVisibleBeforeCapture;
            WorldPanelShowButton.Visible = !worldPanelVisibleBeforeCapture;
        }

        private void SetMapStreetLights(bool lightsOn)
        {
            mapStreetLightsEnabled = lightsOn;
            ApplyFlatMapLighting();
            UpdateTimeOfDayLabel();
            UpdateStatus(lightsOn ? "Night (lights on)" : "Day (lights off)");
        }

        private void UpdateTimeOfDayLabel()
        {
            if (TimeOfDayModeLabel == null) return;
            TimeOfDayModeLabel.Text = mapStreetLightsEnabled ? "Time: Night" : "Time: Day";
        }

        private void Init()
        {
            if (!initedOk)
            {
                Close();
                return;
            }

            MouseWheel += RealMapForm_MouseWheel;

            if (!GTAFolder.UpdateGTAFolder(true))
            {
                Close();
                return;
            }

            Input.Init();
            Renderer.Start();
        }

        private void ContentThread()
        {
            running = true;
            UpdateStatus("Scanning...");

            try
            {
                GTA5Keys.LoadFromPath(GTAFolder.CurrentGTAFolder, GTAFolder.IsGen9, Settings.Default.Key);
            }
            catch
            {
                MessageBox.Show("Keys not found! This shouldn't happen.");
                Close();
                return;
            }

            var s = Settings.Default;
            GameFileCache.EnableMods = s.EnableMods;
            GameFileCache.SelectedDlc = s.DLC;
            GameFileCache.EnableDlc = !string.IsNullOrEmpty(s.DLC);
            GameFileCache.LoadPeds = false;
            GameFileCache.LoadVehicles = false;
            GameFileCache.LoadArchetypes = true;
            GameFileCache.BuildExtendedJenkIndex = false;
            GameFileCache.DoFullStringIndex = false;
            GameFileCache.Init(UpdateStatus, LogError);

            UpdateDlcListComboBox(GameFileCache.DlcNameList);
            SyncDlcModsUiFromCache();
            EnableDLCModsUI();

            LoadWorld();
            worldReady = true;
            UpdateStatus("Ready");

            Task.Run(() =>
            {
                while (formopen && !IsDisposed)
                {
                    bool rcItemsPending = Renderer.ContentThreadProc();
                    if (!rcItemsPending)
                    {
                        Thread.Sleep(1);
                    }
                }
            });

            while (formopen && !IsDisposed)
            {
                bool fcItemsPending = GameFileCache.ContentThreadProc();
                if (!fcItemsPending)
                {
                    Thread.Sleep(1);
                }
            }

            GameFileCache.Clear();
            running = false;
        }

        private void LoadWorld()
        {
            UpdateStatus("Loading timecycles...");
            timecycle.Init(GameFileCache, UpdateStatus);
            timecycle.SetTime(Renderer.timeofday);

            UpdateStatus("Loading materials...");
            BoundsMaterialTypes.Init(GameFileCache);

            UpdateStatus("Loading weather...");
            weather.Init(GameFileCache, UpdateStatus, timecycle);

            UpdateStatus("Loading clouds...");
            clouds.Init(GameFileCache, UpdateStatus, weather);

            UpdateStatus("Loading water...");
            water.Init(GameFileCache, UpdateStatus, loadHeistIsland: showCayoPerico);

            UpdateStatus("Loading world...");
            space.Init(GameFileCache, UpdateStatus);

            UpdateStatus("World loaded");
        }

        private void UpdateDlcListComboBox(List<string> dlcnames)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => UpdateDlcListComboBox(dlcnames)));
                    return;
                }

                suppressDlcEvents = true;
                DlcLevelComboBox.Items.Clear();
                foreach (var dlcname in dlcnames)
                {
                    DlcLevelComboBox.Items.Add(dlcname);
                }
                if (DlcLevelComboBox.Items.Count == 0)
                {
                    DlcLevelComboBox.Items.Add("<none>");
                    DlcLevelComboBox.SelectedIndex = 0;
                }
                else if (string.IsNullOrEmpty(GameFileCache.SelectedDlc))
                {
                    DlcLevelComboBox.SelectedIndex = dlcnames.Count - 1;
                }
                else
                {
                    int idx = DlcLevelComboBox.FindString(GameFileCache.SelectedDlc);
                    DlcLevelComboBox.SelectedIndex = (idx >= 0) ? idx : (dlcnames.Count - 1);
                }
                suppressDlcEvents = false;
            }
            catch { suppressDlcEvents = false; }
        }

        private void SyncDlcModsUiFromCache()
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(SyncDlcModsUiFromCache));
                    return;
                }

                suppressDlcEvents = true;
                EnableModsCheckBox.Checked = GameFileCache.EnableMods;
                EnableDlcCheckBox.Checked = GameFileCache.EnableDlc;
                suppressDlcEvents = false;
            }
            catch { suppressDlcEvents = false; }
        }

        private void EnableDLCModsUI()
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(EnableDLCModsUI));
                    return;
                }

                EnableModsCheckBox.Enabled = true;
                EnableDlcCheckBox.Enabled = true;
                DlcLevelComboBox.Enabled = true;
            }
            catch { }
        }

        private void SetDlcLevel(string dlc, bool enable)
        {
            if (!worldReady) return;
            if (batchRunning || capturePhase != CapturePhase.Idle)
            {
                MessageBox.Show("Stop the current capture before changing DLC.", "RealMap",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                SyncDlcModsUiFromCache();
                return;
            }

            Cursor = Cursors.WaitCursor;
            UpdateStatus("Applying DLC level...");
            Task.Run(() =>
            {
                try
                {
                    lock (Renderer.RenderSyncRoot)
                    {
                        if (GameFileCache.SetDlcLevel(dlc, enable))
                        {
                            LoadWorld();
                        }
                    }
                    try
                    {
                        Settings.Default.DLC = enable ? dlc : "";
                        Settings.Default.Save();
                    }
                    catch { }
                }
                finally
                {
                    BeginInvoke(new Action(() =>
                    {
                        Cursor = Cursors.Default;
                        UpdateStatus("DLC updated");
                    }));
                }
            });
        }

        private void SetModsEnabled(bool enable)
        {
            if (!worldReady) return;
            if (batchRunning || capturePhase != CapturePhase.Idle)
            {
                MessageBox.Show("Stop the current capture before changing mods.", "RealMap",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                SyncDlcModsUiFromCache();
                return;
            }

            Cursor = Cursors.WaitCursor;
            UpdateStatus("Applying mods setting...");
            Task.Run(() =>
            {
                try
                {
                    lock (Renderer.RenderSyncRoot)
                    {
                        if (GameFileCache.SetModsEnabled(enable))
                        {
                            UpdateDlcListComboBox(GameFileCache.DlcNameList);
                            LoadWorld();
                        }
                    }
                    try
                    {
                        Settings.Default.EnableMods = enable;
                        Settings.Default.Save();
                    }
                    catch { }
                }
                finally
                {
                    BeginInvoke(new Action(() =>
                    {
                        Cursor = Cursors.Default;
                        UpdateStatus("Mods setting updated");
                    }));
                }
            });
        }

        private void UpdateStatus(string text)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => UpdateStatus(text)));
                }
                else
                {
                    StatusLabel.Text = text;
                }
            }
            catch { }
        }

        private void LogError(string text)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => LogError(text)));
                }
                else
                {
                    UpdateStatus("Error: " + text);
                }
            }
            catch { }
        }

        static TileBounds GetTileBounds(int z, int x, int y)
        {
            int n = 1 << z;
            float x0 = WorldXMin + (x / (float)n) * SpanX;
            float x1 = WorldXMin + ((x + 1) / (float)n) * SpanX;
            float yTop = WorldYMax - (y / (float)n) * SpanY;
            float yBottom = WorldYMax - ((y + 1) / (float)n) * SpanY;
            return new TileBounds { X0 = x0, X1 = x1, YTop = yTop, YBottom = yBottom };
        }

        private void GoToTile(int z, int x, int y)
        {
            var b = GetTileBounds(z, x, y);
            float side = Math.Max(b.Width, b.Height);
            GoToWorldPosition(b.CenterX, b.CenterY, side);
            UpdateCoordsLabel(z, x, y, b);
        }

        private void GoToWorldPosition(float worldX, float worldY, float orthoHeight)
        {
            lock (Renderer.RenderSyncRoot)
            {
                camEntity.Position = new Vector3(worldX, worldY, 0);
                camera.OrthographicTargetSize = orthoHeight;
                camera.OrthographicSize = orthoHeight;
                camera.UpdateProj = true;
                Renderer.SetCameraMode("2D Map");
            }
        }

        private void UpdateCoordsLabel(int z, int x, int y, TileBounds b)
        {
            CoordsLabel.Text = string.Format(
                "Tile {0}/{1}/{2}\nWorld center ({3:0.##}, {4:0.##})  size {5:0.#}×{6:0.#}",
                z, x, y, b.CenterX, b.CenterY, b.Width, b.Height);
        }

        private System.Drawing.Rectangle GetCaptureViewportRect()
        {
            int left = (!toolsHiddenForCapture && ToolsPanel.Visible) ? ToolsPanel.Right : 0;
            int right = (!toolsHiddenForCapture && WorldPanel.Visible) ? WorldPanel.Width : 0;
            int statusH = StatusStrip.Visible ? StatusStrip.Height : 0;
            int availW = Math.Max(1, ClientSize.Width - left - right);
            int availH = Math.Max(1, ClientSize.Height - statusH);
            int side = Math.Min(availW, availH);
            int x = left + (availW - side) / 2;
            int y = (availH - side) / 2;
            return new System.Drawing.Rectangle(x, y, side, side);
        }

        private void PrepareCameraForTile(int z, int x, int y)
        {
            var b = GetTileBounds(z, x, y);
            float tileH = b.Height;

            var rect = GetCaptureViewportRect();
            float camW = camera.Width > 1 ? camera.Width : Math.Max(1, ClientSize.Width);
            float camH = camera.Height > 1 ? camera.Height : Math.Max(1, ClientSize.Height);

            float ortho = tileH * (camH / Math.Max(1, rect.Height));
            float orthoW = ortho * (camW / Math.Max(1f, camH));
            float capCx = rect.X + rect.Width * 0.5f;
            float capCy = rect.Y + rect.Height * 0.5f;
            float midX = camW * 0.5f;
            float midY = camH * 0.5f;
            float shiftX = ((midX - capCx) / camW) * orthoW;
            float shiftY = ((capCy - midY) / camH) * ortho;

            lock (Renderer.RenderSyncRoot)
            {
                camEntity.Position = new Vector3(b.CenterX + shiftX, b.CenterY + shiftY, 0);
                camera.OrthographicTargetSize = ortho;
                camera.OrthographicSize = ortho;
                camera.UpdateProj = true;
                Renderer.SetCameraMode("2D Map");
                ApplyFlatMapLightingLocked();
            }

            pendingCaptureRect = rect;
            pendingCaptureZ = z;
            pendingCaptureX = x;
            pendingCaptureY = y;
            pendingOutputSize = Math.Max(OutputTileSize4K, (int)OutputTileSizeNumeric.Value);
            pendingOutputPath = Path.Combine(OutputFolderTextBox.Text.Trim(), z.ToString(), x.ToString(), y.ToString() + ".png");
        }

        private void BeginSettleForCurrentTile()
        {
            HidePanelsForCapture();
            PrepareCameraForTile(pendingCaptureZ, pendingCaptureX, pendingCaptureY);

            settleMinFrames = Math.Max(5, (int)SettleFramesNumeric.Value);
            settleElapsedFrames = 0;
            settleStableFrames = 0;
            settleLastLoadedCount = -1;
            capturePhase = CapturePhase.Settling;
            UpdateStatus(string.Format("Loading {0}/{1}/{2}...", pendingCaptureZ, pendingCaptureX, pendingCaptureY));
        }

        private bool IsTileContentReady(out int fileQueue, out int gpuQueue, out int loadedCount)
        {
            fileQueue = GameFileCache.QueueLength;
            gpuQueue = Renderer.RenderableCache != null ? Renderer.RenderableCache.TotalQueueLength : 0;
            loadedCount = Renderer.RenderableCache != null ? Renderer.RenderableCache.TotalItemCount : 0;
            return fileQueue == 0 && gpuQueue == 0;
        }

        private void TickCaptureState()
        {
            if (capturePhase != CapturePhase.Settling) return;

            settleElapsedFrames++;

            if ((settleElapsedFrames % 15) == 1)
            {
                PrepareCameraForTile(pendingCaptureZ, pendingCaptureX, pendingCaptureY);
            }

            bool queuesEmpty = IsTileContentReady(out int fileQ, out int gpuQ, out int loaded);

            if (settleElapsedFrames < settleMinFrames)
            {
                settleStableFrames = 0;
                settleLastLoadedCount = loaded;
                if ((settleElapsedFrames % 10) == 0)
                {
                    UpdateStatus(string.Format(
                        "{0}/{1}/{2}: wait {3}/{4}  files={5} gpu={6}",
                        pendingCaptureZ, pendingCaptureX, pendingCaptureY,
                        settleElapsedFrames, settleMinFrames, fileQ, gpuQ));
                }
                return;
            }

            if (queuesEmpty)
            {
                if (loaded == settleLastLoadedCount)
                {
                    settleStableFrames++;
                }
                else
                {
                    settleStableFrames = 0;
                    settleLastLoadedCount = loaded;
                }
            }
            else
            {
                settleStableFrames = 0;
                settleLastLoadedCount = loaded;
            }

            if ((settleElapsedFrames % 10) == 0)
            {
                UpdateStatus(string.Format(
                    "{0}/{1}/{2}: files={3} gpu={4} loaded={5} stable={6}/{7}",
                    pendingCaptureZ, pendingCaptureX, pendingCaptureY,
                    fileQ, gpuQ, loaded, settleStableFrames, SettleStableRequired));
            }

            bool ready = queuesEmpty && settleStableFrames >= SettleStableRequired;
            bool timedOut = settleElapsedFrames >= SettleMaxFrames;

            if (ready || timedOut)
            {
                if (timedOut && !ready)
                {
                    UpdateStatus(string.Format(
                        "{0}/{1}/{2}: timeout, capturing (files={3} gpu={4})",
                        pendingCaptureZ, pendingCaptureX, pendingCaptureY, fileQ, gpuQ));
                }
                else
                {
                    UpdateStatus(string.Format("{0}/{1}/{2}: capturing...", pendingCaptureZ, pendingCaptureX, pendingCaptureY));
                }

                PrepareCameraForTile(pendingCaptureZ, pendingCaptureX, pendingCaptureY);
                capturePhase = CapturePhase.Capture;
                captureRequested = true;
            }
        }

        private void FinishCapturedTile(Bitmap bmp, int z, int x, int y, int outSize, string path)
        {
            try
            {
                using (bmp)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));

                    if (bmp.Width != outSize || bmp.Height != outSize)
                    {
                        using (var resized = new Bitmap(outSize, outSize, PixelFormat.Format32bppArgb))
                        using (var g = Graphics.FromImage(resized))
                        {
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            g.DrawImage(bmp, 0, 0, outSize, outSize);
                            resized.Save(path, ImageFormat.Png);
                        }
                    }
                    else
                    {
                        bmp.Save(path, ImageFormat.Png);
                    }
                }

                BeginInvoke(new Action(() =>
                {
                    UpdateStatus("Saved " + path);
                    AdvanceBatchAfterSave();
                }));
            }
            catch (Exception ex)
            {
                BeginInvoke(new Action(() =>
                {
                    UpdateStatus("Save failed: " + ex.Message);
                    MessageBox.Show("Error saving tile:\n" + ex.Message, "RealMap Capture",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    StopBatch("Save failed");
                }));
            }
        }

        private void AdvanceBatchAfterSave()
        {
            capturePhase = CapturePhase.Idle;

            if (!batchRunning)
            {
                RestorePanelsAfterCapture();
                ProgressLabel.Text = string.Format("Saved tile {0}/{1}/{2}", pendingCaptureZ, pendingCaptureX, pendingCaptureY);
                return;
            }

            batchDone++;
            UpdateBatchProgress();
            QueueNextBatchTile();
        }

        private void QueueNextBatchTile()
        {
            if (batchCancel || tileQueue.Count == 0)
            {
                StopBatch(batchCancel ? "Cancelled" : "Complete");
                return;
            }

            var job = tileQueue.Dequeue();
            pendingCaptureZ = job.Z;
            pendingCaptureX = job.X;
            pendingCaptureY = job.Y;
            pendingOutputPath = job.Path;
            BeginSettleForCurrentTile();
            pendingOutputPath = job.Path;
            UpdateBatchProgress();
        }

        private void UpdateBatchProgress()
        {
            int processed = batchDone + batchSkipped;
            CaptureProgressBar.Maximum = Math.Max(1, batchTotal);
            CaptureProgressBar.Value = Math.Min(processed, CaptureProgressBar.Maximum);
            ProgressLabel.Text = string.Format(
                "Done {0}  skipped {1}  remaining {2}  / total {3}\nCurrent {4}/{5}/{6}",
                batchDone, batchSkipped, tileQueue.Count, batchTotal,
                pendingCaptureZ, pendingCaptureX, pendingCaptureY);
        }

        private void StopBatch(string reason)
        {
            batchRunning = false;
            batchCancel = false;
            capturePhase = CapturePhase.Idle;
            captureRequested = false;
            tileQueue.Clear();
            RestorePanelsAfterCapture();
            StartCaptureButton.Enabled = true;
            StopCaptureButton.Enabled = false;
            CaptureCurrentButton.Enabled = true;
            ProgressLabel.Text = reason + " - saved " + batchDone + ", skipped " + batchSkipped;
            UpdateStatus(reason);
        }

        private void UpdateControlInputs(float elapsed)
        {
            if (elapsed > 0.1f) elapsed = 0.1f;
            if (batchRunning || capturePhase != CapturePhase.Idle) return;

            float moveSpeed = 50.0f;
            Input.Update();

            if (Input.ShiftPressed) moveSpeed *= 5.0f;
            if (Input.CtrlPressed) moveSpeed *= 0.2f;

            Vector3 movevec = Input.KeyboardMoveVec(true);
            if (Input.xbenable)
            {
                movevec.X += Input.xblx;
                movevec.Y += Input.xbly;
            }

            movevec *= elapsed * moveSpeed * Math.Min(camera.OrthographicTargetSize * 0.01f, 50.0f);

            float mapviewscale = 1.0f / Math.Max(1.0f, camera.Height);
            float fdx = MapViewDragX * mapviewscale;
            float fdy = MapViewDragY * mapviewscale;
            movevec.X -= fdx * camera.OrthographicSize;
            movevec.Y += fdy * camera.OrthographicSize;

            Vector3 movewvec = camera.ViewInvQuaternion.Multiply(movevec);
            camEntity.Position += movewvec;

            MapViewDragX = 0;
            MapViewDragY = 0;
        }

        #region UI events

        private void RealMapForm_Load(object sender, EventArgs e)
        {
            DarkTheme.Apply(this);
            WorldPanel.Visible = true;
            WorldPanelShowButton.Visible = false;
            WorldPanel.BringToFront();
            WorldPanelHideButton.BringToFront();
            ToolsPanel.BringToFront();
            ToolsPanelShowButton.BringToFront();
            WorldPanelShowButton.BringToFront();
            Init();
        }

        private void RealMapForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ConfirmQuit())
            {
                e.Cancel = true;
                return;
            }
            batchCancel = true;
            formopen = false;
        }

        private void StatsUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                StatsLabel.Text = Renderer.GetStatusText();
            }
            catch { }
        }

        private void ToolsPanelHideButton_Click(object sender, EventArgs e)
        {
            ToolsPanel.Visible = false;
            ToolsPanelShowButton.Visible = true;
        }

        private void ToolsPanelShowButton_Click(object sender, EventArgs e)
        {
            ToolsPanel.Visible = true;
            ToolsPanelShowButton.Visible = false;
        }

        private void WorldPanelHideButton_Click(object sender, EventArgs e)
        {
            WorldPanel.Visible = false;
            WorldPanelShowButton.Visible = true;
        }

        private void WorldPanelShowButton_Click(object sender, EventArgs e)
        {
            WorldPanel.Visible = true;
            WorldPanelShowButton.Visible = false;
        }

        private void EnableGTAVMapCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            showMainMap = EnableGTAVMapCheckBox.Checked;
            Renderer.ShowGTAVMap = showMainMap;
        }

        private void EnableCayoPericoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            showCayoPerico = EnableCayoPericoCheckBox.Checked;
            Renderer.ShowCayoPerico = showCayoPerico;
            if (!worldReady) return;
            Task.Run(() =>
            {
                lock (Renderer.RenderSyncRoot)
                {
                    water.Init(GameFileCache, UpdateStatus, loadHeistIsland: showCayoPerico);
                }
            });
        }

        private void EnableNorthYanktonCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            showNorthYankton = EnableNorthYanktonCheckBox.Checked;
            Renderer.ShowNorthYankton = showNorthYankton;
        }

        private void HideShipsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFlatMapLighting();
        }

        private void EnableModsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressDlcEvents || !worldReady) return;
            SetModsEnabled(EnableModsCheckBox.Checked);
        }

        private void EnableDlcCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (suppressDlcEvents || !worldReady) return;
            SetDlcLevel(DlcLevelComboBox.Text, EnableDlcCheckBox.Checked);
        }

        private void DlcLevelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressDlcEvents || !worldReady) return;
            if (!EnableDlcCheckBox.Checked) return;
            SetDlcLevel(DlcLevelComboBox.Text, true);
        }

        private void BrowseOutputButton_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "RealMap output folder";
                if (Directory.Exists(OutputFolderTextBox.Text))
                {
                    dlg.SelectedPath = OutputFolderTextBox.Text;
                }
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    OutputFolderTextBox.Text = dlg.SelectedPath;
                }
            }
        }

        private void GoToTileButton_Click(object sender, EventArgs e)
        {
            int z = (int)TileZNumeric.Value;
            int n = 1 << z;
            int x = Math.Min((int)TileXNumeric.Value, n - 1);
            int y = Math.Min((int)TileYNumeric.Value, n - 1);
            TileXNumeric.Value = x;
            TileYNumeric.Value = y;
            GoToTile(z, x, y);
        }

        private void WarnIfCaptureBelow4K()
        {
            var rect = GetCaptureViewportRect();
            if (rect.Width < OutputTileSize4K || rect.Height < OutputTileSize4K)
            {
                MessageBox.Show(
                    "Capture viewport is " + rect.Width + "x" + rect.Height + ".\n" +
                    "Tiles will be saved at 4096x4096 (upscaled if smaller).",
                    "RealMap",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void CaptureCurrentButton_Click(object sender, EventArgs e)
        {
            if (!worldReady)
            {
                MessageBox.Show("World is still loading.", "RealMap Capture", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (batchRunning || capturePhase != CapturePhase.Idle)
            {
                MessageBox.Show("A capture is already in progress.", "RealMap Capture", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            WarnIfCaptureBelow4K();

            int z = (int)TileZNumeric.Value;
            int n = 1 << z;
            int x = Math.Min((int)TileXNumeric.Value, n - 1);
            int y = Math.Min((int)TileYNumeric.Value, n - 1);

            string folder = OutputFolderTextBox.Text.Trim();
            if (string.IsNullOrEmpty(folder))
            {
                MessageBox.Show("Choose an output folder first.", "RealMap Capture", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            pendingCaptureZ = z;
            pendingCaptureX = x;
            pendingCaptureY = y;
            BeginSettleForCurrentTile();
        }

        private void StartCaptureButton_Click(object sender, EventArgs e)
        {
            if (!worldReady)
            {
                MessageBox.Show("World is still loading.", "RealMap Capture", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (batchRunning || capturePhase != CapturePhase.Idle)
            {
                return;
            }

            int minZ = (int)MinZoomNumeric.Value;
            int maxZ = (int)MaxZoomNumeric.Value;
            if (maxZ < minZ)
            {
                MessageBox.Show("Max Z must be >= Min Z.", "RealMap Capture", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string folder = OutputFolderTextBox.Text.Trim();
            if (string.IsNullOrEmpty(folder))
            {
                MessageBox.Show("Choose an output folder first.", "RealMap Capture", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Directory.CreateDirectory(folder);
            WarnIfCaptureBelow4K();
            tileQueue.Clear();
            batchTotal = 0;
            batchDone = 0;
            batchSkipped = 0;
            bool skipExisting = SkipExistingCheckBox.Checked;

            for (int z = minZ; z <= maxZ; z++)
            {
                int n = 1 << z;
                for (int x = 0; x < n; x++)
                {
                    for (int y = 0; y < n; y++)
                    {
                        string path = Path.Combine(folder, z.ToString(), x.ToString(), y.ToString() + ".png");
                        batchTotal++;
                        if (skipExisting && File.Exists(path))
                        {
                            batchSkipped++;
                            continue;
                        }
                        tileQueue.Enqueue(new TileJob { Z = z, X = x, Y = y, Path = path });
                    }
                }
            }

            if (tileQueue.Count == 0)
            {
                ProgressLabel.Text = "Nothing to do - all tiles already exist.";
                UpdateStatus("Batch skipped (all exist)");
                return;
            }

            batchRunning = true;
            batchCancel = false;
            StartCaptureButton.Enabled = false;
            StopCaptureButton.Enabled = true;
            CaptureCurrentButton.Enabled = false;
            UpdateBatchProgress();
            QueueNextBatchTile();
        }

        private void StopCaptureButton_Click(object sender, EventArgs e)
        {
            batchCancel = true;
            UpdateStatus("Stopping after current tile...");
        }

        private void ShadowsOffCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFlatMapLighting();
        }

        private void MapDetailNumeric_ValueChanged(object sender, EventArgs e)
        {
            Renderer.MapViewDetail = (float)MapDetailNumeric.Value;
        }

        private void DayButton_Click(object sender, EventArgs e)
        {
            SetMapStreetLights(false);
        }

        private void NightButton_Click(object sender, EventArgs e)
        {
            SetMapStreetLights(true);
        }

        private void RealMapForm_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left: MouseLButtonDown = true; break;
                case MouseButtons.Right: MouseRButtonDown = true; break;
            }
            if (!ToolsPanelShowButton.Focused)
            {
                ToolsPanelShowButton.Focus();
            }
            MouseDownPoint = e.Location;
            MouseLastPoint = MouseDownPoint;
            MouseX = e.X;
            MouseY = e.Y;
        }

        private void RealMapForm_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left: MouseLButtonDown = false; break;
                case MouseButtons.Right: MouseRButtonDown = false; break;
            }
        }

        private void RealMapForm_MouseMove(object sender, MouseEventArgs e)
        {
            int dx = e.X - MouseX;
            int dy = e.Y - MouseY;

            if (MouseLButtonDown && capturePhase == CapturePhase.Idle && !batchRunning)
            {
                MapViewDragX += dx;
                MapViewDragY += dy;
            }
            if (MouseRButtonDown && Renderer.controllightdir)
            {
                Renderer.lightdirx += (dx * camera.Sensitivity);
                Renderer.lightdiry += (dy * camera.Sensitivity);
            }

            MouseX = e.X;
            MouseY = e.Y;
            MouseLastPoint = e.Location;
        }

        private void RealMapForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta != 0 && capturePhase == CapturePhase.Idle && !batchRunning)
            {
                camera.MouseZoom(e.Delta);
            }
        }

        private void RealMapForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (ActiveControl is TextBox)
            {
                var tb = ActiveControl as TextBox;
                if (!tb.ReadOnly) return;
            }
            if (ActiveControl is ComboBox)
            {
                var cb = ActiveControl as ComboBox;
                if (cb.DropDownStyle != ComboBoxStyle.DropDownList) return;
            }

            Input.KeyDown(e, true);
        }

        private void RealMapForm_KeyUp(object sender, KeyEventArgs e)
        {
            Input.KeyUp(e);
        }

        #endregion
    }
}
