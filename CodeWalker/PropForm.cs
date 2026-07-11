using CodeWalker.GameFiles;
using CodeWalker.Properties;
using CodeWalker.Rendering;
using CodeWalker.World;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = SharpDX.Color;

namespace CodeWalker
{
    public partial class PropForm : Form, DXForm
    {
        public Form Form { get { return this; } } //for DXForm/DXManager use

        public Renderer Renderer = null;
        public object RenderSyncRoot { get { return Renderer.RenderSyncRoot; } }

        volatile bool formopen = false;
        volatile bool running = false;
        volatile bool pauserendering = false;
        //volatile bool initialised = false;

        Stopwatch frametimer = new Stopwatch();
        Camera camera;
        Timecycle timecycle;
        Weather weather;
        Clouds clouds;

        Entity camEntity = new Entity();


        bool MouseLButtonDown = false;
        bool MouseRButtonDown = false;
        int MouseX;
        int MouseY;
        System.Drawing.Point MouseDownPoint;
        System.Drawing.Point MouseLastPoint;


        public GameFileCache GameFileCache { get; } = GameFileCacheFactory.Create();


        InputManager Input = new InputManager();


        bool initedOk = false;



        bool toolsPanelResizing = false;
        int toolsPanelResizeStartX = 0;
        int toolsPanelResizeStartLeft = 0;
        int toolsPanelResizeStartRight = 0;

        Dictionary<DrawableBase, bool> DrawableDrawFlags = new Dictionary<DrawableBase, bool>();

        bool enableGrid = false;
        float gridSize = 1.0f;
        int gridCount = 40;
        List<VertexTypePC> gridVerts = new List<VertexTypePC>();
        object gridSyncRoot = new object();




        YdrFile Ydr = null;
        YftFile Yft = null;
        YddFile Ydd = null;
        Archetype ModelArchetype = null;
        uint ModelHash = 0;

        List<string> PropNames = new List<string>();
        int CurrentPropIndex = -1;

        Dictionary<string, CustomPropInfo> customProps = new Dictionary<string, CustomPropInfo>(StringComparer.OrdinalIgnoreCase);
        HashSet<string> registeredCustomYtds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        int propLoadGeneration = 0;
        int pendingPropIndex = -1;
        bool pendingPropLoadScheduled = false;
        bool pendingPropMoveCamera = true;

        class CustomPropInfo
        {
            public string Name;
            public string ModelPath;
            public string ModelExtension;
            public uint TxdHash;
            public List<string> YtdPaths = new List<string>();
            public YdrFile Ydr;
            public YftFile Yft;
            public YddFile Ydd;
            public bool ModelRegistered;
        }

        uint customFolderDefaultTxdHash = 0;
        readonly List<uint> customFolderYtdHashes = new List<uint>();
        readonly Dictionary<string, string> customGtxdChildToParent = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        volatile bool capturePhotoRequested = false;
        string capturePhotoPropName = null;
        System.Drawing.Rectangle pendingCaptureRect;
        bool pendingCaptureGreenScreen;
        bool pendingCaptureStudioLighting;

        Panel CaptureOverlayPanel;
        Label CaptureOverlayLabel;
        System.Windows.Forms.Timer CaptureOverlayHideTimer;
        readonly List<string> captureOverlayLines = new List<string>();

        const float StudioLightDirX = 2.25f;
        const float StudioLightDirY = 0.25f;

        WorldRenderMode renderModeBeforeStudio = WorldRenderMode.Default;




        public PropForm()
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
            Renderer.lightdirx = StudioLightDirX;
            Renderer.lightdiry = StudioLightDirY;
            Renderer.rendercollisionmeshes = false;
            Renderer.renderclouds = false;
            //Renderer.renderclouds = true;
            //Renderer.individualcloudfrag = "Contrails";
            Renderer.rendermoon = false;
            Renderer.renderskeletons = false;
            Renderer.renderfragwindows = false;
            Renderer.SelectionFlagsTestAll = true;

            GTAFolder.UpdateEnhancedFormTitle(this);

            InitCaptureOverlay();
        }

        private void InitCaptureOverlay()
        {
            CaptureOverlayLabel = new Label
            {
                AutoSize = true,
                ForeColor = System.Drawing.Color.White,
                BackColor = System.Drawing.Color.Transparent,
                Font = new System.Drawing.Font("Segoe UI", 9.75F),
                Location = new System.Drawing.Point(8, 6),
                MaximumSize = new System.Drawing.Size(420, 0)
            };

            CaptureOverlayPanel = new Panel
            {
                BackColor = System.Drawing.Color.FromArgb(210, 32, 32, 36),
                Padding = new Padding(8, 6, 8, 6),
                Visible = false
            };
            CaptureOverlayPanel.Controls.Add(CaptureOverlayLabel);
            CaptureOverlayPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            CaptureOverlayHideTimer = new System.Windows.Forms.Timer { Interval = 6000 };
            CaptureOverlayHideTimer.Tick += CaptureOverlayHideTimer_Tick;

            Controls.Add(CaptureOverlayPanel);
            CaptureOverlayPanel.BringToFront();

            Resize += PropForm_Resize;
        }

        private void PropForm_Resize(object sender, EventArgs e)
        {
            UpdateCaptureOverlayPosition();
        }

        private void CaptureOverlayHideTimer_Tick(object sender, EventArgs e)
        {
            CaptureOverlayHideTimer.Stop();
            CaptureOverlayPanel.Visible = false;
            captureOverlayLines.Clear();
        }

        private void ResetCaptureOverlay()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(ResetCaptureOverlay));
                return;
            }

            captureOverlayLines.Clear();
            CaptureOverlayHideTimer.Stop();
            CaptureOverlayPanel.Visible = true;
            CaptureOverlayPanel.BringToFront();
            UpdateCaptureOverlayText();
        }

        private void AddCaptureOverlayLine(string line)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => AddCaptureOverlayLine(line)));
                return;
            }

            captureOverlayLines.Add(line);
            CaptureOverlayPanel.Visible = true;
            CaptureOverlayPanel.BringToFront();
            UpdateCaptureOverlayText();
        }

        private void UpdateCaptureOverlayText()
        {
            CaptureOverlayLabel.Text = string.Join(Environment.NewLine, captureOverlayLines);
            UpdateCaptureOverlayPosition();
        }

        private void UpdateCaptureOverlayPosition()
        {
            if (CaptureOverlayPanel == null) return;

            int margin = 12;
            int maxWidth = Math.Max(200, ClientSize.Width - (ToolsPanel.Visible ? ToolsPanel.Right : 0) - margin * 2);
            CaptureOverlayLabel.MaximumSize = new System.Drawing.Size(Math.Min(420, maxWidth), 0);

            var labelSize = CaptureOverlayLabel.PreferredSize;
            CaptureOverlayLabel.Size = labelSize;
            CaptureOverlayPanel.Size = new System.Drawing.Size(labelSize.Width + 16, labelSize.Height + 12);
            CaptureOverlayPanel.Location = new System.Drawing.Point(ClientSize.Width - CaptureOverlayPanel.Width - margin, margin);
        }

        private void ScheduleCaptureOverlayHide(int ms = 6000)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => ScheduleCaptureOverlayHide(ms)));
                return;
            }

            CaptureOverlayHideTimer.Stop();
            CaptureOverlayHideTimer.Interval = ms;
            CaptureOverlayHideTimer.Start();
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
            camera.FollowEntity.Position = Vector3.Zero;// prevworldpos;
            camera.FollowEntity.Orientation = Quaternion.LookAtLH(Vector3.Zero, Vector3.Up, Vector3.ForwardLH);
            camera.TargetDistance = 2.0f;
            camera.CurrentDistance = 2.0f;
            camera.TargetRotation.Y = 0.2f;
            camera.CurrentRotation.Y = 0.2f;
            camera.TargetRotation.X = 0.5f * (float)Math.PI;
            camera.CurrentRotation.X = 0.5f * (float)Math.PI;

            Renderer.shaders.deferred = false; //no point using this here yet


            LoadSettings();


            formopen = true;
            new Thread(new ThreadStart(ContentThread)).Start();

            frametimer.Start();

        }
        public void CleanupScene()
        {
            formopen = false;

            Renderer.DeviceDestroyed();

            int count = 0;
            while (running && (count < 5000)) //wait for the content thread to exit gracefully
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
            { return; } //couldn't get a lock, try again next time

            if (StudioLightingCheckBox.Checked)
            {
                Renderer.StudioLightingOverride = true;
                Renderer.lightdirx = StudioLightDirX;
                Renderer.lightdiry = StudioLightDirY;
            }

            UpdateControlInputs(elapsed);
            //space.Update(elapsed);

            Renderer.Update(elapsed, MouseLastPoint.X, MouseLastPoint.Y);



            //UpdateWidgets();
            //BeginMouseHitTest();




            Renderer.BeginRender(context);

            Renderer.RenderSkyAndClouds();

            Renderer.SelectedDrawable = null;// SelectedItem.Drawable;


            RenderProp();

            //UpdateMouseHitsFromRenderer();
            //RenderSelection();


            RenderGrid(context);


            Renderer.RenderQueued();

            //Renderer.RenderBounds(MapSelectionMode.Entity);

            Renderer.RenderSelectionGeometry(MapSelectionMode.Entity);

            //RenderMoused();

            Renderer.RenderFinalPass();

            if (capturePhotoRequested)
            {
                capturePhotoRequested = false;
                string propName = capturePhotoPropName;
                capturePhotoPropName = null;
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

                    BeginInvoke(new Action(() => AddCaptureOverlayLine("Captured")));
                }
                catch (Exception ex)
                {
                    capturedBitmap?.Dispose();
                    BeginInvoke(new Action(() =>
                    {
                        AddCaptureOverlayLine("Capture failed");
                        AddCaptureOverlayLine(ex.Message);
                        ScheduleCaptureOverlayHide(8000);
                        MessageBox.Show("Error saving capture:\n" + ex.Message, "Capture Photo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }

                Renderer.EndRender();

                Monitor.Exit(Renderer.RenderSyncRoot);

                if (capturedBitmap != null)
                {
                    bool greenScreen = pendingCaptureGreenScreen;
                    bool studioLighting = pendingCaptureStudioLighting;
                    Task.Run(() => FinishCapturedPhoto(capturedBitmap, propName, greenScreen, studioLighting));
                }

                return;
            }

            //RenderMarkers();
            //RenderWidgets();

            Renderer.EndRender();

            Monitor.Exit(Renderer.RenderSyncRoot);

            //UpdateMarkerSelectionPanelInvoke();
        }
        public void BuffersResized(int w, int h)
        {
            Renderer.BuffersResized(w, h);
        }
        public bool ConfirmQuit()
        {
            return true;
        }




        private void Init()
        {
            //called from PropForm_Load

            if (!initedOk)
            {
                Close();
                return;
            }


            MouseWheel += PropForm_MouseWheel;

            if (!GTAFolder.UpdateGTAFolder(true))
            {
                Close();
                return;
            }



            ShaderParamNames[] texsamplers = RenderableGeometry.GetTextureSamplerList();
            foreach (var texsampler in texsamplers)
            {
                TextureSamplerComboBox.Items.Add(texsampler);
            }
            //TextureSamplerComboBox.SelectedIndex = 0;//LoadSettings will do this..


            UpdateGridVerts();
            GridSizeComboBox.SelectedIndex = 1;
            GridCountComboBox.SelectedIndex = 1;



            Input.Init();


            Renderer.Start();
        }


        private void ContentThread()
        {
            //main content loading thread.
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

            GameFileCache.EnableDlc = true;
            GameFileCache.EnableMods = true;
            GameFileCache.LoadPeds = false;
            GameFileCache.LoadVehicles = false;
            GameFileCache.LoadArchetypes = true;
            GameFileCache.BuildExtendedJenkIndex = false;//to speed things up a little
            GameFileCache.DoFullStringIndex = true;//to get all global text from DLC...
            GameFileCache.Init(UpdateStatus, LogError);

            //UpdateDlcListComboBox(gameFileCache.DlcNameList);

            //EnableCacheDependentUI();

            LoadWorld();

            UpdateStatus("Ready - enter prop name(s) to view");



            //initialised = true;

            //EnableDLCModsUI();

            //UpdateStatus("Ready");


            Task.Run(() => {
                while (formopen && !IsDisposed) //renderer content loop
                {
                    bool rcItemsPending = Renderer.ContentThreadProc();

                    if (!rcItemsPending)
                    {
                        Thread.Sleep(1); //sleep if there's nothing to do
                    }
                }
            });

            while (formopen && !IsDisposed) //main asset loop
            {
                bool fcItemsPending = GameFileCache.ContentThreadProc();

                if (!fcItemsPending)
                {
                    Thread.Sleep(1); //sleep if there's nothing to do
                }
            }

            GameFileCache.Clear();

            running = false;
        }




        private void LoadSettings()
        {
            var s = Settings.Default;
            //WindowState = s.WindowMaximized ? FormWindowState.Maximized : WindowState;
            //FullScreenCheckBox.Checked = s.FullScreen;
            WireframeCheckBox.Checked = s.Wireframe;
            HDRRenderingCheckBox.Checked = s.HDR;
            ShadowsCheckBox.Checked = s.Shadows;
            SkydomeCheckBox.Checked = s.Skydome;
            RenderModeComboBox.SelectedIndex = Math.Max(RenderModeComboBox.FindString(s.RenderMode), 0);
            TextureSamplerComboBox.SelectedIndex = Math.Max(TextureSamplerComboBox.FindString(s.RenderTextureSampler), 0);
            TextureCoordsComboBox.SelectedIndex = Math.Max(TextureCoordsComboBox.FindString(s.RenderTextureSamplerCoord), 0);
            AnisotropicFilteringCheckBox.Checked = s.AnisotropicFiltering;
            //ErrorConsoleCheckBox.Checked = s.ShowErrorConsole;
            //StatusBarCheckBox.Checked = s.ShowStatusBar;

            ApplyStudioLighting(StudioLightingCheckBox.Checked);
        }

        private void ApplyStudioLighting(bool studio)
        {
            lock (Renderer.RenderSyncRoot)
            {
                Renderer.controltimeofday = !studio;
                Renderer.controllightdir = studio || ControlLightDirCheckBox.Checked;
                Renderer.timerunning = false;

                if (studio)
                {
                    Renderer.StudioLightingOverride = true;
                    Renderer.lightdirx = StudioLightDirX;
                    Renderer.lightdiry = StudioLightDirY;
                    Renderer.renderskydome = false;
                    Renderer.renderclouds = false;
                    Renderer.rendernaturalambientlight = true;
                    Renderer.renderartificialambientlight = false;
                    Renderer.renderlights = false;
                    Renderer.shaders.shadows = false;
                    if (Renderer.shaders != null)
                    {
                        if (Renderer.shaders.RenderMode != WorldRenderMode.SingleTexture)
                        {
                            renderModeBeforeStudio = Renderer.shaders.RenderMode;
                        }
                        Renderer.shaders.RenderMode = WorldRenderMode.SingleTexture;
                    }
                    if (!GreenScreenCheckBox.Checked)
                    {
                        Renderer.shaders.hdr = false;
                    }
                }
                else
                {
                    Renderer.StudioLightingOverride = false;
                    Renderer.renderskydome = SkydomeCheckBox.Checked;
                    Renderer.rendernaturalambientlight = true;
                    Renderer.renderartificialambientlight = true;
                    Renderer.renderlights = true;
                    Renderer.shaders.shadows = ShadowsCheckBox.Checked;
                    ApplyRenderModeFromCombo();
                    if (!GreenScreenCheckBox.Checked)
                    {
                        Renderer.shaders.hdr = HDRRenderingCheckBox.Checked;
                    }
                }
            }

            if (studio)
            {
                ControlLightDirCheckBox.Checked = true;
            }
            ControlLightDirCheckBox.Enabled = !studio;
            RenderModeComboBox.Enabled = !studio;
            SkydomeCheckBox.Enabled = !studio && !GreenScreenCheckBox.Checked;
            ShadowsCheckBox.Enabled = !studio;
            TimeOfDayTrackBar.Enabled = !studio;
            label19.Enabled = !studio;
            TimeOfDayLabel.Enabled = !studio;
            HDRRenderingCheckBox.Enabled = !studio && !GreenScreenCheckBox.Checked;
        }

        private void StudioLightingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ApplyStudioLighting(StudioLightingCheckBox.Checked);
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
            //UpdateWeatherTypesComboBox(weather);

            UpdateStatus("Loading clouds...");
            clouds.Init(GameFileCache, UpdateStatus, weather);
            //UpdateCloudTypesComboBox(clouds);

        }






        private void UpdateStatus(string text)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => { UpdateStatus(text); }));
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
                    Invoke(new Action(() => { LogError(text); }));
                }
                else
                {
                    //TODO: error logging..
                    ConsoleTextBox.AppendText(text + "\r\n");
                    //StatusLabel.Text = text;
                    //MessageBox.Show(text);
                }
            }
            catch { }
        }




        private void UpdateMousePosition(MouseEventArgs e)
        {
            MouseX = e.X;
            MouseY = e.Y;
            MouseLastPoint = e.Location;
        }

        private void RotateCam(int dx, int dy)
        {
            camera.MouseRotate(dx, dy);
        }

        private void MoveCameraToView(Vector3 pos, float rad)
        {
            //move the camera to a default place where the given sphere is fully visible.

            rad = Math.Max(0.01f, rad);

            camera.FollowEntity.Position = pos;
            camera.TargetDistance = rad * 1.6f;
            camera.CurrentDistance = rad * 1.6f;

            camera.TargetRotation.X = 0.5f * (float)Math.PI;
            camera.CurrentRotation.X = 0.5f * (float)Math.PI;
            camera.TargetRotation.Y = 0.2f;
            camera.CurrentRotation.Y = 0.2f;
            camera.TargetRotation.Z = 0.0f;
            camera.CurrentRotation.Z = 0.0f;

            camera.UpdateProj = true;

        }

        private void CenterCameraOnProp()
        {
            GetCurrentPropFiles(out YdrFile ydr, out YftFile yft, out YddFile ydd, out uint modelHash, out Archetype archetype);

            DrawableBase drawable = null;
            if ((ydr != null) && ydr.Loaded)
            {
                drawable = ydr.Drawable;
            }
            else if ((yft != null) && yft.Loaded)
            {
                drawable = yft.Fragment?.Drawable;
            }
            else if ((ydd != null) && ydd.Loaded && (ydd.Drawables != null) && (ydd.Drawables.Length > 0))
            {
                float maxrad = 0.01f;
                foreach (var d in ydd.Drawables)
                {
                    maxrad = Math.Max(maxrad, d.BoundingSphereRadius);
                }
                MoveCameraToView(Vector3.Zero, maxrad);
                return;
            }

            if (drawable != null)
            {
                LoadDrawable(drawable, true);
            }
        }


        private void AddDrawableTreeNode(DrawableBase drawable, uint hash, bool check)
        {
            MetaHash mhash = new MetaHash(hash);

            var dnode = ModelsTreeView.Nodes.Add(mhash.ToString());
            dnode.Tag = drawable;
            dnode.Checked = check;

            AddDrawableModelsTreeNodes(drawable.DrawableModels?.High, "High Detail", true, dnode);
            AddDrawableModelsTreeNodes(drawable.DrawableModels?.Med, "Medium Detail", false, dnode);
            AddDrawableModelsTreeNodes(drawable.DrawableModels?.Low, "Low Detail", false, dnode);
            AddDrawableModelsTreeNodes(drawable.DrawableModels?.VLow, "Very Low Detail", false, dnode);
            //AddDrawableModelsTreeNodes(drawable.DrawableModels?.Extra, "X Detail", false, dnode);

        }
        private void AddDrawableModelsTreeNodes(DrawableModel[] models, string prefix, bool check, TreeNode parentDrawableNode = null)
        {
            if (models == null) return;

            for (int mi = 0; mi < models.Length; mi++)
            {
                var tnc = (parentDrawableNode != null) ? parentDrawableNode.Nodes : ModelsTreeView.Nodes;

                var model = models[mi];
                string mprefix = prefix + " " + (mi + 1).ToString();
                var mnode = tnc.Add(mprefix + " " + model.ToString());
                mnode.Tag = model;
                mnode.Checked = check;

                var tmnode = TexturesTreeView.Nodes.Add(mprefix + " " + model.ToString());
                tmnode.Tag = model;

                if (!check)
                {
                    lock (Renderer.RenderSyncRoot)
                    {
                        Renderer.SelectionModelDrawFlags[model] = false;
                    }
                }

                if (model.Geometries == null) continue;

                foreach (var geom in model.Geometries)
                {
                    var gname = geom.ToString();
                    var gnode = mnode.Nodes.Add(gname);
                    gnode.Tag = geom;
                    gnode.Checked = true;// check;

                    var tgnode = tmnode.Nodes.Add(gname);
                    tgnode.Tag = geom;

                    if ((geom.Shader != null) && (geom.Shader.ParametersList != null) && (geom.Shader.ParametersList.Hashes != null))
                    {
                        var pl = geom.Shader.ParametersList;
                        var h = pl.Hashes;
                        var p = pl.Parameters;
                        for (int ip = 0; ip < h.Length; ip++)
                        {
                            var hash = pl.Hashes[ip];
                            var parm = pl.Parameters[ip];
                            var tex = parm.Data as TextureBase;
                            if (tex != null)
                            {
                                var t = tex as Texture;
                                var tstr = tex.Name.Trim();
                                if (t != null)
                                {
                                    tstr = string.Format("{0} ({1}x{2}, embedded)", tex.Name, t.Width, t.Height);
                                }
                                var tnode = tgnode.Nodes.Add(hash.ToString().Trim() + ": " + tstr);
                                tnode.Tag = tex;
                            }
                        }
                        tgnode.Expand();
                    }

                }

                mnode.Expand();
                tmnode.Expand();
            }
        }
        private void UpdateSelectionDrawFlags(TreeNode node)
        {
            //update the selection draw flags depending on tag and checked/unchecked
            var drwbl = node.Tag as DrawableBase;
            var model = node.Tag as DrawableModel;
            var geom = node.Tag as DrawableGeometry;
            bool rem = node.Checked;
            lock (Renderer.RenderSyncRoot)
            {
                if (drwbl != null)
                {
                    if (rem)
                    {
                        if (DrawableDrawFlags.ContainsKey(drwbl))
                        {
                            DrawableDrawFlags.Remove(drwbl);
                        }
                    }
                    else
                    {
                        DrawableDrawFlags[drwbl] = false;
                    }
                }
                if (model != null)
                {
                    if (rem)
                    {
                        if (Renderer.SelectionModelDrawFlags.ContainsKey(model))
                        {
                            Renderer.SelectionModelDrawFlags.Remove(model);
                        }
                    }
                    else
                    {
                        Renderer.SelectionModelDrawFlags[model] = false;
                    }
                }
                if (geom != null)
                {
                    if (rem)
                    {
                        if (Renderer.SelectionGeometryDrawFlags.ContainsKey(geom))
                        {
                            Renderer.SelectionGeometryDrawFlags.Remove(geom);
                        }
                    }
                    else
                    {
                        Renderer.SelectionGeometryDrawFlags[geom] = false;
                    }
                }
                //updateArchetypeStatus = true;
            }
        }


        private Archetype TryGetArchetype(uint hash)
        {
            if (!GameFileCache.IsInited) return null;
            return GameFileCache.GetArchetype(hash);
        }

        private const int EM_GETFIRSTVISIBLELINE = 0x00CE;
        private const int EM_LINESCROLL = 0x00B6;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private int GetPropListFirstVisibleLine()
        {
            if (!PropListTextBox.IsHandleCreated) return 0;
            return SendMessage(PropListTextBox.Handle, EM_GETFIRSTVISIBLELINE, 0, 0);
        }

        private void SetPropListFirstVisibleLine(int line)
        {
            if (!PropListTextBox.IsHandleCreated) return;
            int delta = line - GetPropListFirstVisibleLine();
            if (delta != 0)
            {
                SendMessage(PropListTextBox.Handle, EM_LINESCROLL, 0, delta);
            }
        }

        private void ParsePropList()
        {
            PropNames.Clear();
            var lines = PropListTextBox.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var name = line.Trim();
                if (string.IsNullOrEmpty(name) || name.StartsWith("#")) continue;

                var tab = name.IndexOf('\t');
                if (tab > 0) name = name.Substring(0, tab).Trim();

                PropNames.Add(name);
            }
            RefreshPropListTextBox();
        }

        private static string GetCaptureOutputFolder()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "CodeWalker Props");
        }

        private bool HasCapturedPhoto(string propName)
        {
            if (string.IsNullOrWhiteSpace(propName)) return false;

            string folder = GetCaptureOutputFolder();
            if (!Directory.Exists(folder)) return false;

            string safeName = SanitizeFileName(propName);
            foreach (var file in Directory.GetFiles(folder, "*.png"))
            {
                string baseName = Path.GetFileNameWithoutExtension(file);
                if (string.Equals(baseName, safeName, StringComparison.OrdinalIgnoreCase)) return true;
                if (baseName.StartsWith(safeName + "_", StringComparison.OrdinalIgnoreCase)) return true;
            }

            return false;
        }

        private string GetPropListLine(string name)
        {
            if (customProps.TryGetValue(name, out CustomPropInfo info) && (info.YtdPaths.Count > 0))
            {
                return name + "\t" + info.YtdPaths[0];
            }
            return name;
        }

        private void RefreshPropListTextBox()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(RefreshPropListTextBox));
                return;
            }

            int firstVisibleLine = GetPropListFirstVisibleLine();
            PropListTextBox.Text = string.Join(Environment.NewLine, PropNames.Select(GetPropListLine));
            ApplyPropListCaptureHighlight();
            SetPropListFirstVisibleLine(firstVisibleLine);
        }

        private void UpdatePropCaptureHighlightForName(string propName)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdatePropCaptureHighlightForName(propName)));
                return;
            }

            for (int i = 0; i < PropNames.Count; i++)
            {
                if (string.Equals(PropNames[i], propName, StringComparison.OrdinalIgnoreCase))
                {
                    int firstVisibleLine = GetPropListFirstVisibleLine();
                    ColorizePropListLine(i);
                    SetPropListFirstVisibleLine(firstVisibleLine);
                    return;
                }
            }
        }

        private void ApplyPropListCaptureHighlight()
        {
            if (PropNames.Count == 0) return;

            for (int i = 0; i < PropNames.Count; i++)
            {
                ColorizePropListLine(i);
            }
        }

        private void ColorizePropListLine(int i)
        {
            if ((i < 0) || (i >= PropNames.Count)) return;

            string line = GetPropListLine(PropNames[i]);
            int lineStart = PropListTextBox.GetFirstCharIndexFromLine(i);
            if (lineStart < 0) return;

            int lineEnd = lineStart + line.Length;
            if (i + 1 < PropNames.Count)
            {
                int nextStart = PropListTextBox.GetFirstCharIndexFromLine(i + 1);
                if (nextStart > lineStart)
                {
                    lineEnd = nextStart - 1;
                }
            }
            else
            {
                lineEnd = PropListTextBox.TextLength;
            }

            int lineLen = Math.Max(0, lineEnd - lineStart);
            if (lineLen == 0) return;

            PropListTextBox.Select(lineStart, lineLen);
            if (HasCapturedPhoto(PropNames[i]))
            {
                PropListTextBox.SelectionBackColor = System.Drawing.Color.FromArgb(210, 255, 210);
                PropListTextBox.SelectionColor = System.Drawing.Color.FromArgb(20, 100, 20);
            }
            else
            {
                PropListTextBox.SelectionBackColor = System.Drawing.Color.FromArgb(255, 220, 170);
                PropListTextBox.SelectionColor = System.Drawing.Color.FromArgb(140, 60, 0);
            }
        }

        private void HighlightCurrentPropInList()
        {
            if ((CurrentPropIndex < 0) || (CurrentPropIndex >= PropNames.Count)) return;

            int lineStart = PropListTextBox.GetFirstCharIndexFromLine(CurrentPropIndex);
            if (lineStart < 0) return;

            string line = GetPropListLine(PropNames[CurrentPropIndex]);
            PropListTextBox.Select(lineStart, Math.Min(line.Length, PropListTextBox.TextLength - lineStart));
            PropListTextBox.ScrollToCaret();
        }

        private int CountMissingCaptures()
        {
            int missing = 0;
            foreach (var name in PropNames)
            {
                if (!HasCapturedPhoto(name)) missing++;
            }
            return missing;
        }

        private void SetCurrentPropFiles(YdrFile ydr, YftFile yft, YddFile ydd, uint hash, Archetype archetype)
        {
            lock (Renderer.RenderSyncRoot)
            {
                Ydr = ydr;
                Yft = yft;
                Ydd = ydd;
                ModelHash = hash;
                ModelArchetype = archetype;
            }
        }

        private void GetCurrentPropFiles(out YdrFile ydr, out YftFile yft, out YddFile ydd, out uint hash, out Archetype archetype)
        {
            lock (Renderer.RenderSyncRoot)
            {
                ydr = Ydr;
                yft = Yft;
                ydd = Ydd;
                hash = ModelHash;
                archetype = ModelArchetype;
            }
        }

        private void ClearDisplayedProp()
        {
            SetCurrentPropFiles(null, null, null, 0, null);
            CurrentPropNameLabel.Text = "-";
            PropTypeLabel.Text = "-";
            UpdateModelsUI(null);
        }

        private void UpdatePropIndexLabel()
        {
            if (PropNames.Count > 0)
            {
                int missing = CountMissingCaptures();
                if (missing > 0)
                {
                    PropIndexLabel.Text = string.Format("{0} / {1} ({2} missing)", CurrentPropIndex + 1, PropNames.Count, missing);
                }
                else
                {
                    PropIndexLabel.Text = string.Format("{0} / {1} (all captured)", CurrentPropIndex + 1, PropNames.Count);
                }
            }
            else
            {
                PropIndexLabel.Text = "- / -";
            }
        }

        public void LoadPropList()
        {
            ParsePropList();
            if (PropNames.Count > 0)
            {
                ShowPropAtIndex(0);
            }
            else
            {
                CurrentPropIndex = -1;
                UpdatePropIndexLabel();
            }
        }

        public void ShowPropAtIndex(int index, bool movecamera = true)
        {
            if (PropNames.Count == 0) return;

            index = Math.Max(0, Math.Min(index, PropNames.Count - 1));
            CurrentPropIndex = index;
            var name = PropNames[index];
            PropNameTextBox.Text = name;
            LoadProp(name, movecamera);
            UpdatePropIndexLabel();
            HighlightCurrentPropInList();
        }

        private void RequestShowPropAtIndex(int index)
        {
            if (PropNames.Count == 0) return;

            pendingPropIndex = Math.Max(0, Math.Min(index, PropNames.Count - 1));
            if (!pendingPropLoadScheduled)
            {
                pendingPropLoadScheduled = true;
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(ProcessPendingPropLoad));
                }
                else
                {
                    ProcessPendingPropLoad();
                }
            }
        }

        private void ProcessPendingPropLoad()
        {
            pendingPropLoadScheduled = false;
            int idx = pendingPropIndex;
            pendingPropIndex = -1;
            if (idx >= 0 && idx < PropNames.Count)
            {
                ShowPropAtIndex(idx);
            }
        }

        public void ShowNextProp()
        {
            if (PropNames.Count == 0) return;
            int idx = (pendingPropIndex >= 0) ? pendingPropIndex : CurrentPropIndex;
            RequestShowPropAtIndex(idx + 1);
        }

        public void ShowPreviousProp()
        {
            if (PropNames.Count == 0) return;
            int idx = (pendingPropIndex >= 0) ? pendingPropIndex : CurrentPropIndex;
            RequestShowPropAtIndex(idx - 1);
        }

        public void LoadProp(string name, bool movecamera = true)
        {
            if (string.IsNullOrWhiteSpace(name)) return;

            pendingPropMoveCamera = movecamera;
            name = name.Trim();
            var nameLower = name.ToLowerInvariant();
            var hash = JenkHash.GenHash(nameLower);
            int generation = ++propLoadGeneration;

            if (customProps.TryGetValue(nameLower, out CustomPropInfo custom))
            {
                LoadCustomPropModel(custom, movecamera, generation);
                return;
            }

            SetCurrentPropFiles(null, null, null, hash, TryGetArchetype(hash));

            YdrFile ydr = GameFileCache.GetYdr(hash);
            if (ydr != null)
            {
                if (generation != propLoadGeneration) return;

                SetCurrentPropFiles(ydr, null, null, hash, TryGetArchetype(hash));
                if (ydr.Loaded && ydr.Drawable != null)
                {
                    LoadDrawable(ydr.Drawable, movecamera);
                    PropTypeLabel.Text = "YDR";
                    CurrentPropNameLabel.Text = name;
                    return;
                }
                PropTypeLabel.Text = "Loading YDR...";
                CurrentPropNameLabel.Text = name;
                return;
            }

            YftFile yft = GameFileCache.GetYft(hash);
            if (yft != null)
            {
                if (generation != propLoadGeneration) return;

                SetCurrentPropFiles(null, yft, null, hash, TryGetArchetype(hash));
                if (yft.Loaded && yft.Fragment?.Drawable != null)
                {
                    LoadDrawable(yft.Fragment.Drawable, movecamera);
                    PropTypeLabel.Text = "YFT";
                    CurrentPropNameLabel.Text = name;
                    return;
                }
                PropTypeLabel.Text = "Loading YFT...";
                CurrentPropNameLabel.Text = name;
                return;
            }

            PropTypeLabel.Text = "Not found";
            CurrentPropNameLabel.Text = name;
            UpdateModelsUI(null);
        }

        private void RegisterCustomYtdFile(string filename)
        {
            if (string.IsNullOrEmpty(filename) || !File.Exists(filename)) return;
            if (!registeredCustomYtds.Add(filename)) return;

            var nameLower = Path.GetFileNameWithoutExtension(filename).ToLowerInvariant();
            JenkIndex.Ensure(nameLower);
            uint hash = JenkHash.GenHash(nameLower);

            var ytd = new YtdFile();
            ytd.RpfFileEntry = new RpfResourceFileEntry();
            ytd.RpfFileEntry.Name = Path.GetFileName(filename);
            ytd.RpfFileEntry.ShortNameHash = hash;
            ytd.FilePath = filename;
            ytd.Name = ytd.RpfFileEntry.Name;
            ytd.Load(File.ReadAllBytes(filename));
            GameFileCache.AddProjectFile(ytd);
            GameFileCache.RegisterProjectTextureLookups(ytd);
        }

        private static uint DetectDefaultGtxdTxdHash(Dictionary<string, string> childToParent, string[] ytdFiles)
        {
            var ytdNames = new HashSet<string>(ytdFiles.Select(f => Path.GetFileNameWithoutExtension(f).ToLowerInvariant()));
            if (childToParent.Count == 0) return 0;

            var parents = new HashSet<string>(childToParent.Values, StringComparer.OrdinalIgnoreCase);
            foreach (var leaf in childToParent.Keys.Where(c => !parents.Contains(c)))
            {
                if (ytdNames.Contains(leaf.ToLowerInvariant()))
                {
                    return JenkHash.GenHash(leaf.ToLowerInvariant());
                }
            }

            foreach (var child in childToParent.Keys)
            {
                if (ytdNames.Contains(child.ToLowerInvariant()))
                {
                    return JenkHash.GenHash(child.ToLowerInvariant());
                }
            }

            return 0;
        }

        private void EnsureTxdChainLoaded(uint txdHash)
        {
            uint hash = txdHash;
            while (hash != 0)
            {
                GameFileCache.GetYtd(hash);
                hash = GameFileCache.TryGetParentYtdHash(hash);
            }
        }

        private uint ResolveCustomPropTxdHash(CustomPropInfo info, uint modelHash)
        {
            if (info != null)
            {
                if (info.TxdHash != 0) return info.TxdHash;
                if (info.YtdPaths.Count > 0)
                {
                    var ytdName = Path.GetFileNameWithoutExtension(info.YtdPaths[0]).ToLowerInvariant();
                    return JenkHash.GenHash(ytdName);
                }
            }

            if (customFolderDefaultTxdHash != 0) return customFolderDefaultTxdHash;
            return modelHash;
        }

        private static bool IsGtxdRelationshipFile(string filename)
        {
            var nameLower = Path.GetFileName(filename).ToLowerInvariant();
            if (nameLower == "gtxd.meta" || nameLower == "gtxd.ymt") return true;
            if (nameLower.EndsWith(".meta") && nameLower.Contains("gtxd")) return true;
            if (nameLower.EndsWith(".ymt") && nameLower.Contains("gtxd")) return true;
            return false;
        }

        private int RegisterCustomGtxdFile(string filename)
        {
            if (string.IsNullOrEmpty(filename) || !File.Exists(filename)) return 0;

            var entry = new RpfResourceFileEntry();
            entry.Name = Path.GetFileName(filename);
            entry.NameLower = entry.Name.ToLowerInvariant();

            var gtxd = new GtxdFile();
            gtxd.Load(File.ReadAllBytes(filename), entry);
            if ((gtxd.TxdRelationships == null) || (gtxd.TxdRelationships.Count == 0)) return 0;

            foreach (var kvp in gtxd.TxdRelationships)
            {
                JenkIndex.Ensure(kvp.Key);
                JenkIndex.Ensure(kvp.Value);
                if (!customGtxdChildToParent.ContainsKey(kvp.Key))
                {
                    customGtxdChildToParent.Add(kvp.Key, kvp.Value);
                }
            }

            return GameFileCache.AddTxdRelationships(gtxd.TxdRelationships);
        }

        private void LoadCustomYtypFiles(string folder)
        {
            var ytypFiles = Directory.GetFiles(folder, "*.ytyp", SearchOption.AllDirectories);
            foreach (var ytypPath in ytypFiles)
            {
                try
                {
                    var entry = new RpfResourceFileEntry();
                    entry.Name = Path.GetFileName(ytypPath);
                    entry.NameLower = entry.Name.ToLowerInvariant();

                    var ytyp = new YtypFile();
                    ytyp.Load(File.ReadAllBytes(ytypPath), entry);
                    if (ytyp.AllArchetypes == null) continue;

                    foreach (var arch in ytyp.AllArchetypes)
                    {
                        if (arch == null) continue;
                        JenkIndex.Ensure(arch.Name);
                        GameFileCache.AddProjectArchetype(arch);
                    }
                }
                catch (Exception ex)
                {
                    LogError("Error loading YTYP " + ytypPath + ": " + ex.Message);
                }
            }
        }

        private static DrawableBase PeekCustomDrawable(string modelPath, string modelExtension)
        {
            if (string.IsNullOrEmpty(modelPath) || !File.Exists(modelPath)) return null;

            byte[] data = File.ReadAllBytes(modelPath);
            switch (modelExtension)
            {
                case ".ydr":
                {
                    var ydr = new YdrFile();
                    ydr.Load(data);
                    return ydr.Drawable;
                }
                case ".yft":
                {
                    var yft = new YftFile();
                    yft.Load(data);
                    return yft.Fragment?.Drawable;
                }
                case ".ydd":
                {
                    var ydd = new YddFile();
                    ydd.Load(data);
                    if ((ydd.Drawables != null) && (ydd.Drawables.Length > 0))
                    {
                        return ydd.Drawables[0];
                    }
                    break;
                }
            }

            return null;
        }

        private static HashSet<uint> CollectExternalTextureHashes(DrawableBase drawable)
        {
            var hashes = new HashSet<uint>();
            if (drawable?.ShaderGroup?.Shaders?.data_items == null) return hashes;

            foreach (var shader in drawable.ShaderGroup.Shaders.data_items)
            {
                if (shader?.ParametersList?.Parameters == null) continue;
                foreach (var param in shader.ParametersList.Parameters)
                {
                    if (param.Data is Texture)
                    {
                        continue;
                    }

                    if (param.Data is TextureBase texBase)
                    {
                        hashes.Add(texBase.NameHash);
                    }
                }
            }

            return hashes;
        }

        private Dictionary<uint, HashSet<string>> BuildCustomTextureIndex(string[] ytdFiles)
        {
            var index = new Dictionary<uint, HashSet<string>>();
            foreach (var ytdFile in ytdFiles)
            {
                var ytdName = Path.GetFileNameWithoutExtension(ytdFile).ToLowerInvariant();
                var ytdHash = JenkHash.GenHash(ytdName);
                var ytd = GameFileCache.GetYtd(ytdHash);
                if ((ytd == null) || !ytd.Loaded || (ytd.TextureDict?.TextureNameHashes?.data_items == null)) continue;

                foreach (uint texHash in ytd.TextureDict.TextureNameHashes.data_items)
                {
                    if (!index.TryGetValue(texHash, out HashSet<string> ytdNames))
                    {
                        ytdNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        index[texHash] = ytdNames;
                    }
                    ytdNames.Add(ytdName);
                }
            }

            return index;
        }

        private static int ScoreDirectTxdTextures(string ytdName, HashSet<uint> requiredTextures, Dictionary<uint, HashSet<string>> textureIndex)
        {
            if ((requiredTextures == null) || (requiredTextures.Count == 0)) return 0;

            int found = 0;
            foreach (uint texHash in requiredTextures)
            {
                if (textureIndex.TryGetValue(texHash, out HashSet<string> ytdNames) && ytdNames.Contains(ytdName))
                {
                    found++;
                }
            }

            return found;
        }

        private static bool TextureYtdsIntersectChain(HashSet<string> ytdNames, HashSet<string> chainYtds)
        {
            foreach (var ytdName in ytdNames)
            {
                if (chainYtds.Contains(ytdName)) return true;
            }
            return false;
        }

        private static int ScoreTxdChainForTextures(string startYtd, HashSet<uint> requiredTextures, Dictionary<uint, HashSet<string>> textureIndex, Dictionary<string, string> gtxdChildToParent)
        {
            if ((requiredTextures == null) || (requiredTextures.Count == 0)) return 0;

            var chainYtds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string current = startYtd;
            while (!string.IsNullOrEmpty(current) && chainYtds.Add(current))
            {
                if (!gtxdChildToParent.TryGetValue(current, out current)) break;
            }

            int found = 0;
            foreach (uint texHash in requiredTextures)
            {
                if (textureIndex.TryGetValue(texHash, out HashSet<string> ytdNames) && TextureYtdsIntersectChain(ytdNames, chainYtds))
                {
                    found++;
                }
            }

            return found;
        }

        private static bool IsGtxdDescendant(string child, string ancestor, Dictionary<string, string> gtxdChildToParent)
        {
            string current = child;
            while (gtxdChildToParent.TryGetValue(current, out current))
            {
                if (string.Equals(current, ancestor, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        private static string ProbeBestTxdName(HashSet<uint> requiredTextures, string[] ytdFiles, Dictionary<string, string> gtxdChildToParent, Dictionary<uint, HashSet<string>> textureIndex)
        {
            if ((requiredTextures == null) || (requiredTextures.Count == 0)) return null;

            string best = null;
            int bestDirect = 0;
            int bestChain = 0;
            foreach (var ytdFile in ytdFiles)
            {
                var ytdName = Path.GetFileNameWithoutExtension(ytdFile);
                int direct = ScoreDirectTxdTextures(ytdName, requiredTextures, textureIndex);
                int chain = ScoreTxdChainForTextures(ytdName, requiredTextures, textureIndex, gtxdChildToParent);
                if ((direct > bestDirect) ||
                    ((direct == bestDirect) && (chain > bestChain)) ||
                    ((direct == bestDirect) && (chain == bestChain) && (best != null) && IsGtxdDescendant(ytdName, best, gtxdChildToParent)))
                {
                    bestDirect = direct;
                    bestChain = chain;
                    best = ytdName;
                }
            }

            return ((bestDirect > 0) || (bestChain > 0)) ? best : null;
        }

        private static void AssignYtdPathForTxdHash(CustomPropInfo info, uint txdHash, string[] ytdFiles)
        {
            foreach (var ytdFile in ytdFiles)
            {
                if (JenkHash.GenHash(Path.GetFileNameWithoutExtension(ytdFile).ToLowerInvariant()) == txdHash)
                {
                    if (!info.YtdPaths.Any(p => string.Equals(p, ytdFile, StringComparison.OrdinalIgnoreCase)))
                    {
                        info.YtdPaths.Add(ytdFile);
                    }
                    return;
                }
            }
        }

        private uint ResolveCustomPropTxdForModel(string nameLower, string modelPath, string modelExtension, string[] ytdFiles, Dictionary<uint, HashSet<string>> textureIndex)
        {
            var modelHash = JenkHash.GenHash(nameLower);
            var archetype = GameFileCache.GetArchetype(modelHash);
            if ((archetype != null) && (archetype.TextureDict.Hash != 0))
            {
                return archetype.TextureDict.Hash;
            }

            if (customFolderDefaultTxdHash != 0)
            {
                return customFolderDefaultTxdHash;
            }

            var drawable = PeekCustomDrawable(modelPath, modelExtension);
            var requiredTextures = CollectExternalTextureHashes(drawable);
            var probedTxdName = ProbeBestTxdName(requiredTextures, ytdFiles, customGtxdChildToParent, textureIndex);
            if (!string.IsNullOrEmpty(probedTxdName))
            {
                return JenkHash.GenHash(probedTxdName.ToLowerInvariant());
            }

            var ytdPath = FindYtdForModel(modelPath, ytdFiles);
            if (!string.IsNullOrEmpty(ytdPath))
            {
                return JenkHash.GenHash(Path.GetFileNameWithoutExtension(ytdPath).ToLowerInvariant());
            }

            if (customFolderDefaultTxdHash != 0) return customFolderDefaultTxdHash;
            return modelHash;
        }

        private static void UpdateEmbeddedTextures(DrawableBase dwbl)
        {
            if (dwbl == null) return;

            var td = dwbl.ShaderGroup?.TextureDictionary;
            var shaders = dwbl.ShaderGroup?.Shaders?.data_items;
            if ((td == null) || (shaders == null)) return;

            var updated = false;
            foreach (var shader in shaders)
            {
                if (shader?.ParametersList?.Parameters == null) continue;
                foreach (var param in shader.ParametersList.Parameters)
                {
                    if (param.Data is TextureBase tex)
                    {
                        var resolved = td.Lookup(tex.NameHash);
                        if ((resolved != null) && (tex != resolved))
                        {
                            param.Data = resolved;
                            updated = true;
                        }
                    }
                }
            }

            if (!updated) return;

            foreach (var model in dwbl.AllModels)
            {
                if (model?.Geometries == null) continue;
                foreach (var geom in model.Geometries)
                {
                    geom.UpdateRenderableParameters = true;
                }
            }
        }

        private void PrepareCustomPropRenderable(DrawableBase drawable, uint primaryTxdHash)
        {
            if (drawable == null) return;

            lock (Renderer.RenderSyncRoot)
            {
                var rndbl = Renderer.RenderableCache.GetRenderable(drawable);
                if (rndbl == null) return;

                var txds = new List<YtdFile>();
                var seen = new HashSet<uint>();

                void addYtd(uint hash)
                {
                    if ((hash == 0) || !seen.Add(hash)) return;
                    var ytd = GameFileCache.GetYtd(hash);
                    if (ytd != null) txds.Add(ytd);
                }

                addYtd(primaryTxdHash);
                uint parentHash = primaryTxdHash;
                while ((parentHash = GameFileCache.TryGetParentYtdHash(parentHash)) != 0)
                {
                    addYtd(parentHash);
                }

                if ((customFolderDefaultTxdHash != 0) && (customFolderDefaultTxdHash != primaryTxdHash))
                {
                    addYtd(customFolderDefaultTxdHash);
                    parentHash = customFolderDefaultTxdHash;
                    while ((parentHash = GameFileCache.TryGetParentYtdHash(parentHash)) != 0)
                    {
                        addYtd(parentHash);
                    }
                }

                foreach (uint hash in customFolderYtdHashes)
                {
                    addYtd(hash);
                }

                rndbl.SDtxds = txds.ToArray();
                rndbl.HDtxds = null;

                if (rndbl.AllModels == null) return;
                foreach (var model in rndbl.AllModels)
                {
                    if (model?.Geometries == null) continue;
                    foreach (var geom in model.Geometries)
                    {
                        if (geom?.DrawableGeom != null)
                        {
                            geom.Init(geom.DrawableGeom);
                        }
                    }
                }
            }
        }

        private void InvalidateCustomPropRenderable(DrawableBase drawable)
        {
            if (drawable == null) return;

            lock (Renderer.RenderSyncRoot)
            {
                var rndbl = Renderer.RenderableCache.GetRenderable(drawable);
                if (rndbl == null) return;

                rndbl.SDtxds = null;
                rndbl.HDtxds = null;
                if (rndbl.AllModels == null) return;

                foreach (var model in rndbl.AllModels)
                {
                    if (model?.Geometries == null) continue;
                    foreach (var geom in model.Geometries)
                    {
                        if (geom?.DrawableGeom != null)
                        {
                            geom.Init(geom.DrawableGeom);
                        }
                    }
                }
            }
        }

        private static string FindYtdForModel(string modelPath, string[] ytdFiles)
        {
            var modelName = Path.GetFileNameWithoutExtension(modelPath).ToLowerInvariant();
            var modelDir = Path.GetDirectoryName(modelPath);

            var sameDir = Path.Combine(modelDir, modelName + ".ytd");
            if (File.Exists(sameDir)) return sameDir;

            foreach (var ytd in ytdFiles)
            {
                if (string.Equals(Path.GetFileNameWithoutExtension(ytd), modelName, StringComparison.OrdinalIgnoreCase))
                {
                    return ytd;
                }
            }

            return null;
        }

        private void LoadCustomPropsFolder(string folder)
        {
            if (!GameFileCache.IsInited) return;
            if (string.IsNullOrEmpty(folder) || !Directory.Exists(folder)) return;

            UpdateStatus("Scanning custom props folder...");

            customGtxdChildToParent.Clear();
            customFolderDefaultTxdHash = 0;
            customFolderYtdHashes.Clear();

            int gtxdLinks = 0;
            var gtxdFileList = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories)
                .Where(IsGtxdRelationshipFile)
                .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
                .ToList();
            foreach (var gtxdFile in gtxdFileList)
            {
                try
                {
                    gtxdLinks += RegisterCustomGtxdFile(gtxdFile);
                }
                catch (Exception ex)
                {
                    LogError("Error loading GTXD " + gtxdFile + ": " + ex.Message);
                }
            }

            var ytdFiles = Directory.GetFiles(folder, "*.ytd", SearchOption.AllDirectories);
            customFolderDefaultTxdHash = DetectDefaultGtxdTxdHash(customGtxdChildToParent, ytdFiles);
            foreach (var ytdFile in ytdFiles)
            {
                try
                {
                    RegisterCustomYtdFile(ytdFile);
                    customFolderYtdHashes.Add(JenkHash.GenHash(Path.GetFileNameWithoutExtension(ytdFile).ToLowerInvariant()));
                }
                catch (Exception ex)
                {
                    LogError("Error loading YTD " + ytdFile + ": " + ex.Message);
                }
            }

            var textureIndex = BuildCustomTextureIndex(ytdFiles);
            LoadCustomYtypFiles(folder);

            var modelFiles = new List<string>();
            modelFiles.AddRange(Directory.GetFiles(folder, "*.ydr", SearchOption.AllDirectories));
            modelFiles.AddRange(Directory.GetFiles(folder, "*.yft", SearchOption.AllDirectories));
            modelFiles.AddRange(Directory.GetFiles(folder, "*.ydd", SearchOption.AllDirectories));
            modelFiles.Sort(StringComparer.OrdinalIgnoreCase);

            int added = 0;
            foreach (var modelFile in modelFiles)
            {
                var name = Path.GetFileNameWithoutExtension(modelFile);
                var nameLower = name.ToLowerInvariant();
                JenkIndex.Ensure(nameLower);

                var info = new CustomPropInfo()
                {
                    Name = name,
                    ModelPath = modelFile,
                    ModelExtension = Path.GetExtension(modelFile).ToLowerInvariant(),
                };

                info.TxdHash = ResolveCustomPropTxdForModel(nameLower, modelFile, info.ModelExtension, ytdFiles, textureIndex);
                AssignYtdPathForTxdHash(info, info.TxdHash, ytdFiles);

                customProps[nameLower] = info;

                if (!PropNames.Any(p => string.Equals(p, name, StringComparison.OrdinalIgnoreCase)))
                {
                    PropNames.Add(name);
                    added++;
                }
            }

            RefreshPropListTextBox();
            ClearDisplayedProp();
            CurrentPropIndex = -1;
            UpdatePropIndexLabel();
            UpdateStatus(string.Format("Added {0} custom props ({1} YTD, {2} GTXD links) — select one to view", added, ytdFiles.Length, gtxdLinks));
        }

        private void LoadCustomPropModel(CustomPropInfo info, bool movecamera = true, int generation = -1)
        {
            if ((info == null) || string.IsNullOrEmpty(info.ModelPath) || !File.Exists(info.ModelPath)) return;
            if (generation < 0) generation = ++propLoadGeneration;

            var displayName = info.Name;
            var nameLower = displayName.ToLowerInvariant();
            uint hash = JenkHash.GenHash(nameLower);

            foreach (var ytdPath in info.YtdPaths)
            {
                try
                {
                    RegisterCustomYtdFile(ytdPath);
                }
                catch { }
            }

            EnsureTxdChainLoaded(ResolveCustomPropTxdHash(info, hash));

            if (!info.ModelRegistered)
            {
                byte[] data = File.ReadAllBytes(info.ModelPath);

                switch (info.ModelExtension)
                {
                    case ".ydr":
                    {
                        var ydr = new YdrFile();
                        ydr.RpfFileEntry = new RpfResourceFileEntry();
                        ydr.RpfFileEntry.Name = Path.GetFileName(info.ModelPath);
                        ydr.RpfFileEntry.ShortNameHash = hash;
                        ydr.FilePath = info.ModelPath;
                        ydr.Name = ydr.RpfFileEntry.Name;
                        ydr.Load(data);
                        GameFileCache.AddProjectFile(ydr);
                        info.Ydr = ydr;
                        break;
                    }
                    case ".yft":
                    {
                        var yft = new YftFile();
                        yft.RpfFileEntry = new RpfResourceFileEntry();
                        yft.RpfFileEntry.Name = Path.GetFileName(info.ModelPath);
                        yft.RpfFileEntry.ShortNameHash = hash;
                        yft.FilePath = info.ModelPath;
                        yft.Name = yft.RpfFileEntry.Name;
                        yft.Load(data);
                        GameFileCache.AddProjectFile(yft);
                        info.Yft = yft;
                        break;
                    }
                    case ".ydd":
                    {
                        var ydd = new YddFile();
                        ydd.RpfFileEntry = new RpfResourceFileEntry();
                        ydd.RpfFileEntry.Name = Path.GetFileName(info.ModelPath);
                        ydd.RpfFileEntry.ShortNameHash = hash;
                        ydd.FilePath = info.ModelPath;
                        ydd.Name = ydd.RpfFileEntry.Name;
                        ydd.Load(data);
                        GameFileCache.AddProjectFile(ydd);
                        info.Ydd = ydd;
                        break;
                    }
                    default:
                        return;
                }

                info.ModelRegistered = true;
            }

            uint propTxdHash = ResolveCustomPropTxdHash(info, hash);
            switch (info.ModelExtension)
            {
                case ".ydr":
                    UpdateEmbeddedTextures(info.Ydr?.Drawable);
                    PrepareCustomPropRenderable(info.Ydr?.Drawable, propTxdHash);
                    break;
                case ".yft":
                    UpdateEmbeddedTextures(info.Yft?.Fragment?.Drawable);
                    UpdateEmbeddedTextures(info.Yft?.Fragment?.DrawableCloth);
                    PrepareCustomPropRenderable(info.Yft?.Fragment?.Drawable, propTxdHash);
                    PrepareCustomPropRenderable(info.Yft?.Fragment?.DrawableCloth, propTxdHash);
                    break;
                case ".ydd":
                    if (info.Ydd?.Drawables != null)
                    {
                        foreach (var d in info.Ydd.Drawables)
                        {
                            UpdateEmbeddedTextures(d);
                            PrepareCustomPropRenderable(d, propTxdHash);
                        }
                    }
                    break;
            }

            if (generation != propLoadGeneration) return;

            var archetype = TryGetArchetype(hash);
            SetCurrentPropFiles(info.Ydr, info.Yft, info.Ydd, hash, archetype);

            switch (info.ModelExtension)
            {
                case ".ydr":
                    LoadDrawable(info.Ydr?.Drawable, movecamera);
                    PropTypeLabel.Text = "YDR (custom)";
                    break;
                case ".yft":
                    LoadDrawable(info.Yft?.Fragment?.Drawable, movecamera);
                    PropTypeLabel.Text = "YFT (custom)";
                    break;
                case ".ydd":
                    if (info.Ydd?.Drawables != null)
                    {
                        float maxrad = 0.01f;
                        foreach (var d in info.Ydd.Drawables)
                        {
                            maxrad = Math.Max(maxrad, d.BoundingSphereRadius);
                        }
                        if (movecamera) MoveCameraToView(Vector3.Zero, maxrad);
                    }
                    if (info.Ydd?.Dict != null)
                    {
                        lock (Renderer.RenderSyncRoot)
                        {
                            Renderer.SelectionModelDrawFlags.Clear();
                            Renderer.SelectionGeometryDrawFlags.Clear();
                        }
                        ModelsTreeView.Nodes.Clear();
                        ModelsTreeView.ShowRootLines = false;
                        TexturesTreeView.Nodes.Clear();
                        foreach (var kvp in info.Ydd.Dict)
                        {
                            AddDrawableTreeNode(kvp.Value, kvp.Key, true);
                        }
                        DetailsPropertyGrid.SelectedObject = info.Ydd;
                    }
                    PropTypeLabel.Text = "YDD (custom)";
                    break;
            }

            PropNameTextBox.Text = displayName;
            CurrentPropNameLabel.Text = displayName;
            var txdName = JenkIndex.TryGetString(info.TxdHash);
            if (!string.IsNullOrEmpty(txdName))
            {
                UpdateStatus("TXD: " + txdName);
            }
            else if (info.YtdPaths.Count > 0)
            {
                UpdateStatus("YTD: " + info.YtdPaths[0]);
            }
        }

        public void LoadDrawable(DrawableBase drawable, bool movecamera = true)
        {
            if (drawable == null) return;

            if (movecamera)
            {
                var cen = drawable.BoundingCenter;
                var rad = drawable.BoundingSphereRadius;
                if (ModelArchetype != null)
                {
                    cen = ModelArchetype.BSCenter;
                    rad = ModelArchetype.BSRadius;
                }
                MoveCameraToView(cen, rad);
            }

            UpdateModelsUI(drawable);
        }





        private void UpdateModelsUI(DrawableBase drawable)
        {
            DetailsPropertyGrid.SelectedObject = drawable;

            DrawableDrawFlags.Clear();
            lock (Renderer.RenderSyncRoot)
            {
                Renderer.SelectionModelDrawFlags.Clear();
                Renderer.SelectionGeometryDrawFlags.Clear();
            }
            ModelsTreeView.Nodes.Clear();
            ModelsTreeView.ShowRootLines = false;
            TexturesTreeView.Nodes.Clear();
            if (drawable != null)
            {
                AddDrawableModelsTreeNodes(drawable.DrawableModels?.High, "High Detail", true);
                AddDrawableModelsTreeNodes(drawable.DrawableModels?.Med, "Medium Detail", false);
                AddDrawableModelsTreeNodes(drawable.DrawableModels?.Low, "Low Detail", false);
                AddDrawableModelsTreeNodes(drawable.DrawableModels?.VLow, "Very Low Detail", false);
                //AddDrawableModelsTreeNodes(drawable.DrawableModels?.Extra, "X Detail", false);


                var fdrawable = drawable as FragDrawable;
                if (fdrawable != null)
                {
                    var plod1 = fdrawable.OwnerFragment?.PhysicsLODGroup?.PhysicsLOD1;
                    if ((plod1 != null) && (plod1.Children?.data_items != null))
                    {
                        foreach (var child in plod1.Children.data_items)
                        {
                            var cdrwbl = child.Drawable1;
                            if ((cdrwbl != null) && (cdrwbl.AllModels?.Length > 0))
                            {
                                if (cdrwbl.Owner is FragDrawable) continue; //it's a copied drawable... eg a wheel

                                var dname = child.GroupName;
                                AddDrawableModelsTreeNodes(cdrwbl.DrawableModels?.High, dname + " - High Detail", true);
                                AddDrawableModelsTreeNodes(cdrwbl.DrawableModels?.Med, dname + " - Medium Detail", false);
                                AddDrawableModelsTreeNodes(cdrwbl.DrawableModels?.Low, dname + " - Low Detail", false);
                                AddDrawableModelsTreeNodes(cdrwbl.DrawableModels?.VLow, dname + " - Very Low Detail", false);
                            }
                        }
                    }

                    var fdarr = fdrawable.OwnerFragment?.DrawableArray?.data_items;
                    if (fdarr != null)
                    {
                        var fdnames = fdrawable.OwnerFragment?.DrawableArrayNames?.data_items;
                        for (int i = 0; i < fdarr.Length; i++)
                        {
                            var arrd = fdarr[i];
                            if ((arrd != null) && (arrd.AllModels?.Length > 0))
                            {
                                var dname = ((fdnames != null) && (i < fdnames.Length)) ? fdnames[i]?.Value : arrd.Name;
                                if (string.IsNullOrEmpty(dname)) dname = "(No name)";
                                AddDrawableModelsTreeNodes(arrd.DrawableModels?.High, dname + " - High Detail", false);
                                AddDrawableModelsTreeNodes(arrd.DrawableModels?.Med, dname + " - Medium Detail", false);
                                AddDrawableModelsTreeNodes(arrd.DrawableModels?.Low, dname + " - Low Detail", false);
                                AddDrawableModelsTreeNodes(arrd.DrawableModels?.VLow, dname + " - Very Low Detail", false);
                            }
                        }
                    }

                }

            }
        }



        private void RenderProp()
        {
            GetCurrentPropFiles(out YdrFile ydr, out YftFile yft, out YddFile ydd, out uint modelHash, out Archetype archetype);

            CustomPropInfo customInfo = null;
            if ((CurrentPropIndex >= 0) && (CurrentPropIndex < PropNames.Count))
            {
                customProps.TryGetValue(PropNames[CurrentPropIndex].ToLowerInvariant(), out customInfo);
            }

            uint txdHash = (archetype != null) ? archetype.TextureDict.Hash : ResolveCustomPropTxdHash(customInfo, modelHash);

            if (ydr != null)
            {
                if (ydr.Loaded && ydr.Drawable != null)
                {
                    if (archetype == null) archetype = TryGetArchetype(modelHash);
                    var txdExtra = ydr.Drawable?.ShaderGroup?.TextureDictionary;
                    Renderer.RenderDrawable(ydr.Drawable, archetype, null, txdHash, txdExtra, null, null);
                }
            }
            else if (yft != null)
            {
                if (yft.Loaded && yft.Fragment != null)
                {
                    if (archetype == null) archetype = TryGetArchetype(modelHash);
                    Renderer.RenderFragment(archetype, null, yft.Fragment, txdHash, null);
                }
            }
            else if (ydd != null)
            {
                if (ydd.Loaded && ydd.Dict != null)
                {
                    foreach (var kvp in ydd.Dict)
                    {
                        var arch = TryGetArchetype(kvp.Key);
                        var propTxdHash = (arch != null) ? arch.TextureDict.Hash : txdHash;
                        var txdExtra = kvp.Value?.ShaderGroup?.TextureDictionary;
                        Renderer.RenderDrawable(kvp.Value, arch, null, propTxdHash, txdExtra, null, null);
                    }
                }
            }
        }



        private void UpdateTimeOfDayLabel()
        {
            int v = TimeOfDayTrackBar.Value;
            float fh = v / 60.0f;
            int ih = (int)fh;
            int im = v - (ih * 60);
            if (ih == 24) ih = 0;
            TimeOfDayLabel.Text = string.Format("{0:00}:{1:00}", ih, im);
        }


        private void UpdateControlInputs(float elapsed)
        {
            if (elapsed > 0.1f) elapsed = 0.1f;

            var s = Settings.Default;

            float moveSpeed = 2.0f;


            Input.Update();

            if (Input.xbenable)
            {
                //if (ControllerButtonJustPressed(GamepadButtonFlags.Start))
                //{
                //    SetControlMode(ControlMode == WorldControlMode.Free ? WorldControlMode.Ped : WorldControlMode.Free);
                //}
            }



            if (Input.ShiftPressed)
            {
                moveSpeed *= 5.0f;
            }
            if (Input.CtrlPressed)
            {
                moveSpeed *= 0.2f;
            }

            Vector3 movevec = Input.KeyboardMoveVec(false);

            if (Input.xbenable)
            {
                movevec.X += Input.xblx;
                movevec.Z -= Input.xbly;
                moveSpeed *= (1.0f + (Math.Min(Math.Max(Input.xblt, 0.0f), 1.0f) * 15.0f)); //boost with left trigger
                if (Input.ControllerButtonPressed(GamepadButtonFlags.A | GamepadButtonFlags.RightShoulder | GamepadButtonFlags.LeftShoulder))
                {
                    moveSpeed *= 5.0f;
                }
            }


            //if (MapViewEnabled == true)
            //{
            //    movevec *= elapsed * 100.0f * Math.Min(camera.OrthographicTargetSize * 0.01f, 30.0f);
            //    float mapviewscale = 1.0f / camera.Height;
            //    float fdx = MapViewDragX * mapviewscale;
            //    float fdy = MapViewDragY * mapviewscale;
            //    movevec.X -= fdx * camera.OrthographicSize;
            //    movevec.Y += fdy * camera.OrthographicSize;
            //}
            //else
            {
                //normal movement
                movevec *= elapsed * moveSpeed * Math.Min(camera.TargetDistance, 50.0f);
            }


            Vector3 movewvec = camera.ViewInvQuaternion.Multiply(movevec);
            camEntity.Position += movewvec;

            //MapViewDragX = 0;
            //MapViewDragY = 0;




            if (Input.xbenable)
            {
                camera.ControllerRotate(Input.xbrx, Input.xbry, elapsed);

                float zoom = 0.0f;
                float zoomspd = s.XInputZoomSpeed;
                float zoomamt = zoomspd * elapsed;
                if (Input.ControllerButtonPressed(GamepadButtonFlags.DPadUp)) zoom += zoomamt;
                if (Input.ControllerButtonPressed(GamepadButtonFlags.DPadDown)) zoom -= zoomamt;

                camera.ControllerZoom(zoom);

            }



        }



        private void UpdateGridVerts()
        {
            lock (gridSyncRoot)
            {
                gridVerts.Clear();

                float s = gridSize * gridCount * 0.5f;
                uint cblack = (uint)Color.Black.ToRgba();
                uint cgray = (uint)Color.DimGray.ToRgba();
                uint cred = (uint)Color.DarkRed.ToRgba();
                uint cgrn = (uint)Color.DarkGreen.ToRgba();
                int interval = 10;

                for (int i = 0; i <= gridCount; i++)
                {
                    float o = (gridSize * i) - s;
                    if ((i % interval) != 0)
                    {
                        gridVerts.Add(new VertexTypePC() { Position = new Vector3(o, -s, 0), Colour = cgray });
                        gridVerts.Add(new VertexTypePC() { Position = new Vector3(o, s, 0), Colour = cgray });
                        gridVerts.Add(new VertexTypePC() { Position = new Vector3(-s, o, 0), Colour = cgray });
                        gridVerts.Add(new VertexTypePC() { Position = new Vector3(s, o, 0), Colour = cgray });
                    }
                }
                for (int i = 0; i <= gridCount; i++) //draw main lines last, so they are on top
                {
                    float o = (gridSize * i) - s;
                    if ((i % interval) == 0)
                    {
                        var cx = (o == 0) ? cred : cblack;
                        var cy = (o == 0) ? cgrn : cblack;
                        gridVerts.Add(new VertexTypePC() { Position = new Vector3(o, -s, 0), Colour = cy });
                        gridVerts.Add(new VertexTypePC() { Position = new Vector3(o, s, 0), Colour = cy });
                        gridVerts.Add(new VertexTypePC() { Position = new Vector3(-s, o, 0), Colour = cx });
                        gridVerts.Add(new VertexTypePC() { Position = new Vector3(s, o, 0), Colour = cx });
                    }
                }

            }
        }

        private void RenderGrid(DeviceContext context)
        {
            if (!enableGrid) return;

            lock (gridSyncRoot)
            {
                if (gridVerts.Count > 0)
                {
                    Renderer.RenderLines(gridVerts);
                }
            }
        }















        private void PropForm_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void PropForm_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left: MouseLButtonDown = true; break;
                case MouseButtons.Right: MouseRButtonDown = true; break;
            }

            if (!ToolsPanelShowButton.Focused)
            {
                ToolsPanelShowButton.Focus(); //make sure no textboxes etc are focused!
            }

            MouseDownPoint = e.Location;
            MouseLastPoint = MouseDownPoint;

            if (MouseLButtonDown)
            {
            }

            if (MouseRButtonDown)
            {
                //SelectMousedItem();
            }

            MouseX = e.X; //to stop jumps happening on mousedown, sometimes the last MouseMove event was somewhere else... (eg after clicked a menu)
            MouseY = e.Y;
        }

        private void PropForm_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left: MouseLButtonDown = false; break;
                case MouseButtons.Right: MouseRButtonDown = false; break;
            }



            if (e.Button == MouseButtons.Left)
            {
            }
        }

        private void PropForm_MouseMove(object sender, MouseEventArgs e)
        {
            int dx = e.X - MouseX;
            int dy = e.Y - MouseY;

            //if (MouseInvert)
            //{
            //    dy = -dy;
            //}

            //if (ControlMode == WorldControlMode.Free && !ControlBrushEnabled)
            {
                if (MouseLButtonDown)
                {
                    RotateCam(dx, dy);
                }
                if (MouseRButtonDown && !StudioLightingCheckBox.Checked)
                {
                    if (Renderer.controllightdir)
                    {
                        Renderer.lightdirx += (dx * camera.Sensitivity);
                        Renderer.lightdiry += (dy * camera.Sensitivity);
                    }
                    else if (Renderer.controltimeofday)
                    {
                        float tod = Renderer.timeofday;
                        tod += (dx - dy) / 30.0f;
                        while (tod >= 24.0f) tod -= 24.0f;
                        while (tod < 0.0f) tod += 24.0f;
                        timecycle.SetTime(tod);
                        Renderer.timeofday = tod;

                        float fv = tod * 60.0f;
                        TimeOfDayTrackBar.Value = (int)fv;
                        UpdateTimeOfDayLabel();
                    }
                }

                UpdateMousePosition(e);

            }



        }

        private void PropForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                camera.MouseZoom(e.Delta);
            }
        }

        private void PropForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (ActiveControl is TextBox)
            {
                var tb = ActiveControl as TextBox;
                if (!tb.ReadOnly) return; //don't move the camera when typing!
            }
            if (ActiveControl is ComboBox)
            {
                var cb = ActiveControl as ComboBox;
                if (cb.DropDownStyle != ComboBoxStyle.DropDownList) return; //nontypable combobox
            }

            bool enablemove = true;// (!iseditmode) || (MouseLButtonDown && (GrabbedMarker == null) && (GrabbedWidget == null));

            Input.KeyDown(e, enablemove);

            var k = e.KeyCode;
            var kb = Input.keyBindings;
            bool ctrl = Input.CtrlPressed;
            bool shift = Input.ShiftPressed;


            if (!ctrl)
            {
                if (k == kb.MoveSlowerZoomIn)
                {
                    camera.MouseZoom(1);
                }
                if (k == kb.MoveFasterZoomOut)
                {
                    camera.MouseZoom(-1);
                }
            }


            if (!Input.kbmoving) //don't trigger further actions if moving.
            {
                if (!ctrl)
                {
                    if (k == Keys.Left)
                    {
                        ShowPreviousProp();
                    }
                    if (k == Keys.Right)
                    {
                        ShowNextProp();
                    }
                    if (k == Keys.F || k == Keys.Home)
                    {
                        CenterCameraOnProp();
                    }
                }
                else
                {
                    //switch (k)
                    //{
                    //    //case Keys.N:
                    //    //    New();
                    //    //    break;
                    //    //case Keys.O:
                    //    //    Open();
                    //    //    break;
                    //    //case Keys.S:
                    //    //    if (shift) SaveAll();
                    //    //    else Save();
                    //    //    break;
                    //    //case Keys.Z:
                    //    //    Undo();
                    //    //    break;
                    //    //case Keys.Y:
                    //    //    Redo();
                    //    //    break;
                    //    //case Keys.C:
                    //    //    CopyItem();
                    //    //    break;
                    //    //case Keys.V:
                    //    //    PasteItem();
                    //    //    break;
                    //    //case Keys.U:
                    //    //    ToolsPanelShowButton.Visible = !ToolsPanelShowButton.Visible;
                    //    //    break;
                    //}
                }
            }

            //if (ControlMode != WorldControlMode.Free || ControlBrushEnabled)
            //{
            //    e.Handled = true;
            //}
        }

        private void PropForm_KeyUp(object sender, KeyEventArgs e)
        {
            Input.KeyUp(e);

            if (ActiveControl is TextBox)
            {
                var tb = ActiveControl as TextBox;
                if (!tb.ReadOnly) return; //don't move the camera when typing!
            }
            if (ActiveControl is ComboBox)
            {
                var cb = ActiveControl as ComboBox;
                if (cb.DropDownStyle != ComboBoxStyle.DropDownList) return; //non-typable combobox
            }

            //if (ControlMode != WorldControlMode.Free)
            //{
            //    e.Handled = true;
            //}
        }

        private void PropForm_Deactivate(object sender, EventArgs e)
        {
            //try not to lock keyboard movement if the form loses focus.
            Input.KeyboardStop();
        }

        private void StatsUpdateTimer_Tick(object sender, EventArgs e)
        {
            StatsLabel.Text = Renderer.GetStatusText();

            if (Renderer.timerunning)
            {
                float fv = Renderer.timeofday * 60.0f;
                //TimeOfDayTrackBar.Value = (int)fv;
                UpdateTimeOfDayLabel();
            }

            GetCurrentPropFiles(out YdrFile ydr, out YftFile yft, out YddFile ydd, out uint modelHash, out Archetype archetype);

            if (ydr != null && ydr.Loaded && ydr.Drawable != null && PropTypeLabel.Text.StartsWith("Loading"))
            {
                LoadDrawable(ydr.Drawable, pendingPropMoveCamera);
                PropTypeLabel.Text = "YDR";
            }
            else if (yft != null && yft.Loaded && yft.Fragment?.Drawable != null && PropTypeLabel.Text.StartsWith("Loading"))
            {
                LoadDrawable(yft.Fragment.Drawable, pendingPropMoveCamera);
                PropTypeLabel.Text = "YFT";
            }

            //CameraPositionTextBox.Text = FloatUtil.GetVector3String(camera.Position, "0.##");
        }

        private void ToolsPanelShowButton_Click(object sender, EventArgs e)
        {
            ToolsPanel.Visible = true;
        }

        private void ToolsPanelHideButton_Click(object sender, EventArgs e)
        {
            ToolsPanel.Visible = false;
        }

        private void ToolsDragPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                toolsPanelResizing = true;
                toolsPanelResizeStartX = e.X + ToolsPanel.Left + ToolsDragPanel.Left;
                toolsPanelResizeStartLeft = ToolsPanel.Left;
                toolsPanelResizeStartRight = ToolsPanel.Right;
            }
        }

        private void ToolsDragPanel_MouseUp(object sender, MouseEventArgs e)
        {
            toolsPanelResizing = false;
        }

        private void ToolsDragPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (toolsPanelResizing)
            {
                int rx = e.X + ToolsPanel.Left + ToolsDragPanel.Left;
                int dx = rx - toolsPanelResizeStartX;
                ToolsPanel.Width = toolsPanelResizeStartRight - toolsPanelResizeStartLeft + dx;
            }
        }

        private void ModelsTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                UpdateSelectionDrawFlags(e.Node);
            }
        }

        private void ModelsTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
            {
                e.Node.Checked = !e.Node.Checked;
                //UpdateSelectionDrawFlags(e.Node);
            }
        }

        private void ModelsTreeView_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true; //stops annoying ding sound...
        }

        private void HDRRenderingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (GreenScreenCheckBox.Checked || StudioLightingCheckBox.Checked) return;

            lock (Renderer.RenderSyncRoot)
            {
                Renderer.shaders.hdr = HDRRenderingCheckBox.Checked;
            }
        }

        private void ShadowsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (StudioLightingCheckBox.Checked) return;

            lock (Renderer.RenderSyncRoot)
            {
                Renderer.shaders.shadows = ShadowsCheckBox.Checked;
            }
        }

        private void SkydomeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (GreenScreenCheckBox.Checked || StudioLightingCheckBox.Checked) return;

            Renderer.renderskydome = SkydomeCheckBox.Checked;
            //Renderer.controllightdir = !Renderer.renderskydome;
        }

        private void GreenScreenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (GreenScreenCheckBox.Checked && !StudioLightingCheckBox.Checked)
            {
                StudioLightingCheckBox.Checked = true;
            }
            UpdateGreenScreenBackground();
        }

        private void UpdateGreenScreenBackground()
        {
            bool green = GreenScreenCheckBox.Checked;
            lock (Renderer.RenderSyncRoot)
            {
                if (green)
                {
                    Renderer.DXMan.SetClearColour(DXManager.GreenScreenClearColour);
                    Renderer.renderskydome = false;
                    Renderer.shaders.hdr = false;
                    BackColor = System.Drawing.Color.Lime;
                }
                else
                {
                    Renderer.DXMan.SetClearColour(DXManager.DefaultClearColour);
                    Renderer.renderskydome = StudioLightingCheckBox.Checked ? false : SkydomeCheckBox.Checked;
                    Renderer.shaders.hdr = StudioLightingCheckBox.Checked ? false : HDRRenderingCheckBox.Checked;
                    BackColor = System.Drawing.Color.MidnightBlue;
                }
            }
            SkydomeCheckBox.Enabled = !green && !StudioLightingCheckBox.Checked;
            HDRRenderingCheckBox.Enabled = !green && !StudioLightingCheckBox.Checked;
        }

        private void ControlLightDirCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (StudioLightingCheckBox.Checked) return;
            Renderer.controllightdir = ControlLightDirCheckBox.Checked;
        }

        private void TimeOfDayTrackBar_Scroll(object sender, EventArgs e)
        {
            if (StudioLightingCheckBox.Checked) return;
            int v = TimeOfDayTrackBar.Value;
            float fh = v / 60.0f;
            UpdateTimeOfDayLabel();
            lock (Renderer.RenderSyncRoot)
            {
                Renderer.timeofday = fh;
                timecycle.SetTime(Renderer.timeofday);
            }
        }

        private void ShowCollisionMeshesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Renderer.rendercollisionmeshes = ShowCollisionMeshesCheckBox.Checked;
            Renderer.rendercollisionmeshlayerdrawable = ShowCollisionMeshesCheckBox.Checked;
        }

        private void WireframeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Renderer.shaders.wireframe = WireframeCheckBox.Checked;
        }

        private void AnisotropicFilteringCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Renderer.shaders.AnisotropicFiltering = AnisotropicFilteringCheckBox.Checked;
        }

        private void HDTexturesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Renderer.renderhdtextures = HDTexturesCheckBox.Checked;
        }

        private void RenderModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StudioLightingCheckBox.Checked) return;
            ApplyRenderModeFromCombo();
        }

        private void ApplyRenderModeFromCombo()
        {
            if (Renderer?.shaders == null) return;

            TextureSamplerComboBox.Enabled = false;
            TextureCoordsComboBox.Enabled = false;
            lock (Renderer.RenderSyncRoot)
            {
                switch (RenderModeComboBox.Text)
                {
                    default:
                    case "Default":
                        Renderer.shaders.RenderMode = WorldRenderMode.Default;
                        break;
                    case "Single texture":
                        Renderer.shaders.RenderMode = WorldRenderMode.SingleTexture;
                        TextureSamplerComboBox.Enabled = true;
                        TextureCoordsComboBox.Enabled = true;
                        break;
                    case "Vertex normals":
                        Renderer.shaders.RenderMode = WorldRenderMode.VertexNormals;
                        break;
                    case "Vertex tangents":
                        Renderer.shaders.RenderMode = WorldRenderMode.VertexTangents;
                        break;
                    case "Vertex colour 1":
                        Renderer.shaders.RenderMode = WorldRenderMode.VertexColour;
                        Renderer.shaders.RenderVertexColourIndex = 1;
                        break;
                    case "Vertex colour 2":
                        Renderer.shaders.RenderMode = WorldRenderMode.VertexColour;
                        Renderer.shaders.RenderVertexColourIndex = 2;
                        break;
                    case "Vertex colour 3":
                        Renderer.shaders.RenderMode = WorldRenderMode.VertexColour;
                        Renderer.shaders.RenderVertexColourIndex = 3;
                        break;
                    case "Texture coord 1":
                        Renderer.shaders.RenderMode = WorldRenderMode.TextureCoord;
                        Renderer.shaders.RenderTextureCoordIndex = 1;
                        break;
                    case "Texture coord 2":
                        Renderer.shaders.RenderMode = WorldRenderMode.TextureCoord;
                        Renderer.shaders.RenderTextureCoordIndex = 2;
                        break;
                    case "Texture coord 3":
                        Renderer.shaders.RenderMode = WorldRenderMode.TextureCoord;
                        Renderer.shaders.RenderTextureCoordIndex = 3;
                        break;
                }
            }
        }

        private void TextureSamplerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TextureSamplerComboBox.SelectedItem is ShaderParamNames)
            {
                Renderer.shaders.RenderTextureSampler = (ShaderParamNames)TextureSamplerComboBox.SelectedItem;
            }
        }

        private void TextureCoordsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (TextureCoordsComboBox.Text)
            {
                default:
                case "Texture coord 1":
                    Renderer.shaders.RenderTextureSamplerCoord = 1;
                    break;
                case "Texture coord 2":
                    Renderer.shaders.RenderTextureSamplerCoord = 2;
                    break;
                case "Texture coord 3":
                    Renderer.shaders.RenderTextureSamplerCoord = 3;
                    break;
            }
        }

        private void GridCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            enableGrid = GridCheckBox.Checked;
        }

        private void GridSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            float newgs;
            float.TryParse(GridSizeComboBox.Text, out newgs);
            if (newgs != gridSize)
            {
                gridSize = newgs;
                UpdateGridVerts();
            }
        }

        private void GridCountComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int newgc;
            int.TryParse(GridCountComboBox.Text, out newgc);
            if (newgc != gridCount)
            {
                gridCount = newgc;
                UpdateGridVerts();
            }
        }

        private void SkeletonsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Renderer.renderskeletons = SkeletonsCheckBox.Checked;
        }

        private void ShatterMapsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Renderer.renderfragwindows = ShatterMapsCheckBox.Checked;
        }

        private void ErrorConsoleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ConsolePanel.Visible = ErrorConsoleCheckBox.Checked;
        }

        private void StatusBarCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            StatusStrip.Visible = StatusBarCheckBox.Checked;
        }

        private void TextureViewerButton_Click(object sender, EventArgs e)
        {
            //TextureDictionary td = null;

            //if ((Ydr != null) && (Ydr.Loaded))
            //{
            //    td = Ydr.Drawable?.ShaderGroup?.TextureDictionary;
            //}
            //else if ((Yft != null) && (Yft.Loaded))
            //{
            //    td = Yft.Fragment?.Drawable?.ShaderGroup?.TextureDictionary;
            //}

            //if (td != null)
            //{
            //    YtdForm f = new YtdForm();
            //    f.Show();
            //    f.LoadTexDict(td, fileName);
            //    //f.LoadYtd(ytd);
            //}
            //else
            //{
            //    MessageBox.Show("Couldn't find embedded texture dict.");
            //}
        }



        private void PropNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                LoadProp(PropNameTextBox.Text);
            }
        }

        private void LoadPropButton_Click(object sender, EventArgs e)
        {
            if (!GameFileCache.IsInited) return;
            var name = PropNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(name)) return;

            if (PropNames.Count == 0)
            {
                PropNames.Add(name);
                CurrentPropIndex = 0;
                UpdatePropIndexLabel();
            }

            LoadProp(name);
        }

        private void LoadCustomPropButton_Click(object sender, EventArgs e)
        {
            if (!GameFileCache.IsInited) return;

            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select folder containing custom prop models (.ydr, .yft, .ydd) and textures (.ytd)";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    LoadCustomPropsFolder(fbd.SelectedPath);
                }
            }
        }

        private void LoadPropListButton_Click(object sender, EventArgs e)
        {
            if (!GameFileCache.IsInited) return;
            LoadPropList();
        }

        private void PrevPropButton_Click(object sender, EventArgs e)
        {
            if (!GameFileCache.IsInited) return;
            ShowPreviousProp();
        }

        private void NextPropButton_Click(object sender, EventArgs e)
        {
            if (!GameFileCache.IsInited) return;
            ShowNextProp();
        }

        private void OpenPropListButton_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text files|*.txt|All files|*.*";
                ofd.Title = "Open prop list";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        PropListTextBox.Text = File.ReadAllText(ofd.FileName);
                        if (GameFileCache.IsInited)
                        {
                            LoadPropList();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading file:\n" + ex.Message);
                    }
                }
            }
        }

        private void CapturePhotoButton_Click(object sender, EventArgs e)
        {
            var name = PropNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                name = CurrentPropNameLabel.Text?.Trim();
            }
            if (string.IsNullOrEmpty(name) || name == "-")
            {
                MessageBox.Show("Load a prop first before capturing.", "Capture Photo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            capturePhotoPropName = name;
            pendingCaptureRect = GetCaptureViewportRect();
            pendingCaptureGreenScreen = GreenScreenCheckBox.Checked;
            pendingCaptureStudioLighting = StudioLightingCheckBox.Checked;
            capturePhotoRequested = true;
            ResetCaptureOverlay();
            AddCaptureOverlayLine("Capturing photo...");
            UpdateStatus("Capturing...");
        }

        private void CenterPropButton_Click(object sender, EventArgs e)
        {
            CenterCameraOnProp();
        }

        private void PropListTextBox_DoubleClick(object sender, EventArgs e)
        {
            int lineIndex = PropListTextBox.GetLineFromCharIndex(PropListTextBox.SelectionStart);
            if ((lineIndex >= 0) && (lineIndex < PropNames.Count))
            {
                ShowPropAtIndex(lineIndex);
            }
        }

        private System.Drawing.Rectangle GetCaptureViewportRect()
        {
            int x = 0;
            if (ToolsPanel.Visible)
            {
                x = ToolsPanel.Right;
            }
            int statusH = StatusStrip.Visible ? StatusStrip.Height : 0;
            int w = Math.Max(1, ClientSize.Width - x);
            int h = Math.Max(1, ClientSize.Height - statusH);
            return new System.Drawing.Rectangle(x, 0, w, h);
        }

        private void FinishCapturedPhoto(Bitmap bmp, string propName, bool greenScreen, bool studioLighting)
        {
            try
            {
                using (bmp)
                {
                    if (greenScreen)
                    {
                        ApplyGreenScreenChromaKey(bmp);
                    }

                    if (studioLighting)
                    {
                        ApplyStudioCaptureCorrection(bmp);
                    }

                    string picturesFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    string outputFolder = Path.Combine(picturesFolder, "CodeWalker Props");
                    Directory.CreateDirectory(outputFolder);

                    string safeName = SanitizeFileName(propName);
                    string defaultPath = Path.Combine(outputFolder, safeName + ".png");
                    bool duplicateFound = File.Exists(defaultPath);
                    string filePath = GetUniqueFilePath(outputFolder, safeName, ".png");
                    bmp.Save(filePath, ImageFormat.Png);

                    string savedPath = filePath;
                    bool wasDuplicate = duplicateFound;
                    BeginInvoke(new Action(() =>
                    {
                        if (wasDuplicate)
                        {
                            AddCaptureOverlayLine("Duplicate image found");
                        }
                        else
                        {
                            AddCaptureOverlayLine("Saved the image");
                        }
                        AddCaptureOverlayLine("Saved at: " + savedPath);
                        ScheduleCaptureOverlayHide();

                        UpdateStatus("Saved " + Path.GetFileName(savedPath));
                        UpdatePropCaptureHighlightForName(propName);
                        UpdatePropIndexLabel();

                        if ((CurrentPropIndex >= 0) && (CurrentPropIndex < PropNames.Count - 1))
                        {
                            ShowPropAtIndex(CurrentPropIndex + 1, movecamera: false);
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                BeginInvoke(new Action(() =>
                {
                    AddCaptureOverlayLine("Capture failed");
                    AddCaptureOverlayLine(ex.Message);
                    ScheduleCaptureOverlayHide(8000);
                    MessageBox.Show("Error saving capture:\n" + ex.Message, "Capture Photo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
        }

        private static void ApplyStudioCaptureCorrection(Bitmap bmp)
        {
            // Only tame near-clipped highlights in saved captures.
            const float exposure = 0.98f;
            const float shoulder = 0.93f;
            const float compression = 0.72f;

            var bounds = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
            var data = bmp.LockBits(bounds, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            try
            {
                int stride = data.Stride;
                for (int y = 0; y < bmp.Height; y++)
                {
                    int row = y * stride;
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        int i = row + (x * 4);
                        IntPtr p = IntPtr.Add(data.Scan0, i);
                        byte b = Marshal.ReadByte(p, 0);
                        byte g = Marshal.ReadByte(p, 1);
                        byte r = Marshal.ReadByte(p, 2);
                        byte a = Marshal.ReadByte(p, 3);
                        if (a == 0) continue;

                        float rf = (r / 255.0f) * exposure;
                        float gf = (g / 255.0f) * exposure;
                        float bf = (b / 255.0f) * exposure;
                        float lum = Math.Max(rf, Math.Max(gf, bf));
                        if (lum > shoulder)
                        {
                            float t = (lum - shoulder) / Math.Max(1.0f - shoulder, 0.001f);
                            float scale = (shoulder + (t * compression * (1.0f - shoulder))) / lum;
                            rf *= scale;
                            gf *= scale;
                            bf *= scale;
                        }

                        Marshal.WriteByte(p, 0, (byte)Math.Max(0, Math.Min(255, (int)(bf * 255.0f + 0.5f))));
                        Marshal.WriteByte(p, 1, (byte)Math.Max(0, Math.Min(255, (int)(gf * 255.0f + 0.5f))));
                        Marshal.WriteByte(p, 2, (byte)Math.Max(0, Math.Min(255, (int)(rf * 255.0f + 0.5f))));
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(data);
            }
        }

        private static void ApplyGreenScreenChromaKey(Bitmap bmp)
        {
            const byte keyR = 0;
            const byte keyG = 255;
            const byte keyB = 0;
            const int tolerance = 90;
            const int feather = 50;

            var bounds = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
            var data = bmp.LockBits(bounds, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            try
            {
                int stride = data.Stride;
                int height = bmp.Height;
                int width = bmp.Width;
                for (int y = 0; y < height; y++)
                {
                    int row = y * stride;
                    for (int x = 0; x < width; x++)
                    {
                        int i = row + (x * 4);
                        IntPtr p = IntPtr.Add(data.Scan0, i);
                        byte b = Marshal.ReadByte(p, 0);
                        byte g = Marshal.ReadByte(p, 1);
                        byte r = Marshal.ReadByte(p, 2);

                        int dr = r - keyR;
                        int dg = g - keyG;
                        int db = b - keyB;
                        float dist = (float)Math.Sqrt((dr * dr) + (dg * dg) + (db * db));

                        byte alpha;
                        if (dist <= tolerance)
                        {
                            alpha = 0;
                        }
                        else if (dist <= tolerance + feather)
                        {
                            float t = (dist - tolerance) / feather;
                            alpha = (byte)(t * 255.0f);
                        }
                        else
                        {
                            alpha = 255;
                        }

                        Marshal.WriteByte(p, 3, alpha);
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(data);
            }
        }

        private static string SanitizeFileName(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            return name.Trim();
        }

        private static string GetUniqueFilePath(string folder, string baseName, string extension)
        {
            string path = Path.Combine(folder, baseName + extension);
            if (!File.Exists(path)) return path;

            for (int i = 1; i < 10000; i++)
            {
                path = Path.Combine(folder, baseName + "_" + i.ToString() + extension);
                if (!File.Exists(path)) return path;
            }

            return Path.Combine(folder, baseName + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + extension);
        }
    }
}
