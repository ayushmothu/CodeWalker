namespace CodeWalker
{
    partial class RealMapForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatsUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.ToolsPanel = new System.Windows.Forms.Panel();
            this.ToolsScrollPanel = new System.Windows.Forms.Panel();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.SettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.labelOutput = new System.Windows.Forms.Label();
            this.OutputFolderTextBox = new System.Windows.Forms.TextBox();
            this.BrowseOutputButton = new System.Windows.Forms.Button();
            this.labelMinZ = new System.Windows.Forms.Label();
            this.MinZoomNumeric = new System.Windows.Forms.NumericUpDown();
            this.labelMaxZ = new System.Windows.Forms.Label();
            this.MaxZoomNumeric = new System.Windows.Forms.NumericUpDown();
            this.labelTileSize = new System.Windows.Forms.Label();
            this.OutputTileSizeNumeric = new System.Windows.Forms.NumericUpDown();
            this.labelSettle = new System.Windows.Forms.Label();
            this.SettleFramesNumeric = new System.Windows.Forms.NumericUpDown();
            this.labelMapDetail = new System.Windows.Forms.Label();
            this.MapDetailNumeric = new System.Windows.Forms.NumericUpDown();
            this.ShadowsOffCheckBox = new System.Windows.Forms.CheckBox();
            this.SkipExistingCheckBox = new System.Windows.Forms.CheckBox();
            this.LightingGroupBox = new System.Windows.Forms.GroupBox();
            this.DayButton = new System.Windows.Forms.Button();
            this.NightButton = new System.Windows.Forms.Button();
            this.TimeOfDayModeLabel = new System.Windows.Forms.Label();
            this.PreviewGroupBox = new System.Windows.Forms.GroupBox();
            this.labelTileZ = new System.Windows.Forms.Label();
            this.TileZNumeric = new System.Windows.Forms.NumericUpDown();
            this.labelTileX = new System.Windows.Forms.Label();
            this.TileXNumeric = new System.Windows.Forms.NumericUpDown();
            this.labelTileY = new System.Windows.Forms.Label();
            this.TileYNumeric = new System.Windows.Forms.NumericUpDown();
            this.GoToTileButton = new System.Windows.Forms.Button();
            this.CoordsLabel = new System.Windows.Forms.Label();
            this.ProgressGroupBox = new System.Windows.Forms.GroupBox();
            this.CaptureCurrentButton = new System.Windows.Forms.Button();
            this.StartCaptureButton = new System.Windows.Forms.Button();
            this.StopCaptureButton = new System.Windows.Forms.Button();
            this.CaptureProgressBar = new System.Windows.Forms.ProgressBar();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.ToolsPanelHideButton = new System.Windows.Forms.Button();
            this.ToolsPanelShowButton = new System.Windows.Forms.Button();
            this.StatusStrip.SuspendLayout();
            this.ToolsPanel.SuspendLayout();
            this.ToolsScrollPanel.SuspendLayout();
            this.SettingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinZoomNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxZoomNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputTileSizeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SettleFramesNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapDetailNumeric)).BeginInit();
            this.LightingGroupBox.SuspendLayout();
            this.PreviewGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TileZNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TileXNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TileYNumeric)).BeginInit();
            this.ProgressGroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // StatusStrip
            //
            this.StatusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(34)))), ((int)(((byte)(38)))));
            this.StatusStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.StatusStrip.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.StatsLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 728);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);
            this.StatusStrip.Size = new System.Drawing.Size(1280, 22);
            this.StatusStrip.SizingGrip = false;
            this.StatusStrip.TabIndex = 0;
            //
            // StatusLabel
            //
            this.StatusLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(39, 17);
            this.StatusLabel.Spring = true;
            this.StatusLabel.Text = "Ready";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // StatsLabel
            //
            this.StatsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.StatsLabel.Name = "StatsLabel";
            this.StatsLabel.Size = new System.Drawing.Size(50, 17);
            this.StatsLabel.Text = "Drawn: -";
            //
            // StatsUpdateTimer
            //
            this.StatsUpdateTimer.Enabled = true;
            this.StatsUpdateTimer.Interval = 500;
            this.StatsUpdateTimer.Tick += new System.EventHandler(this.StatsUpdateTimer_Tick);
            //
            // ToolsPanel
            //
            this.ToolsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(46)))));
            this.ToolsPanel.Controls.Add(this.ToolsScrollPanel);
            this.ToolsPanel.Controls.Add(this.ToolsPanelHideButton);
            this.ToolsPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.ToolsPanel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ToolsPanel.Location = new System.Drawing.Point(0, 0);
            this.ToolsPanel.Name = "ToolsPanel";
            this.ToolsPanel.Size = new System.Drawing.Size(360, 706);
            this.ToolsPanel.TabIndex = 1;
            //
            // ToolsScrollPanel
            //
            this.ToolsScrollPanel.AutoScroll = true;
            this.ToolsScrollPanel.Controls.Add(this.InfoLabel);
            this.ToolsScrollPanel.Controls.Add(this.SettingsGroupBox);
            this.ToolsScrollPanel.Controls.Add(this.LightingGroupBox);
            this.ToolsScrollPanel.Controls.Add(this.PreviewGroupBox);
            this.ToolsScrollPanel.Controls.Add(this.ProgressGroupBox);
            this.ToolsScrollPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolsScrollPanel.Location = new System.Drawing.Point(0, 0);
            this.ToolsScrollPanel.Name = "ToolsScrollPanel";
            this.ToolsScrollPanel.Padding = new System.Windows.Forms.Padding(12, 36, 12, 12);
            this.ToolsScrollPanel.Size = new System.Drawing.Size(360, 728);
            this.ToolsScrollPanel.TabIndex = 0;
            //
            // InfoLabel
            //
            this.InfoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.InfoLabel.Location = new System.Drawing.Point(12, 36);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(318, 34);
            this.InfoLabel.TabIndex = 0;
            this.InfoLabel.Text = "LS mainland only · Cayo & Yankton off · 4K PNG tiles";
            //
            // SettingsGroupBox
            //
            this.SettingsGroupBox.Controls.Add(this.labelOutput);
            this.SettingsGroupBox.Controls.Add(this.OutputFolderTextBox);
            this.SettingsGroupBox.Controls.Add(this.BrowseOutputButton);
            this.SettingsGroupBox.Controls.Add(this.labelMinZ);
            this.SettingsGroupBox.Controls.Add(this.MinZoomNumeric);
            this.SettingsGroupBox.Controls.Add(this.labelMaxZ);
            this.SettingsGroupBox.Controls.Add(this.MaxZoomNumeric);
            this.SettingsGroupBox.Controls.Add(this.labelTileSize);
            this.SettingsGroupBox.Controls.Add(this.OutputTileSizeNumeric);
            this.SettingsGroupBox.Controls.Add(this.labelSettle);
            this.SettingsGroupBox.Controls.Add(this.SettleFramesNumeric);
            this.SettingsGroupBox.Controls.Add(this.labelMapDetail);
            this.SettingsGroupBox.Controls.Add(this.MapDetailNumeric);
            this.SettingsGroupBox.Controls.Add(this.ShadowsOffCheckBox);
            this.SettingsGroupBox.Controls.Add(this.SkipExistingCheckBox);
            this.SettingsGroupBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.SettingsGroupBox.Location = new System.Drawing.Point(12, 74);
            this.SettingsGroupBox.Name = "SettingsGroupBox";
            this.SettingsGroupBox.Size = new System.Drawing.Size(318, 250);
            this.SettingsGroupBox.TabIndex = 1;
            this.SettingsGroupBox.TabStop = false;
            this.SettingsGroupBox.Text = "Settings";
            //
            // labelOutput
            //
            this.labelOutput.AutoSize = true;
            this.labelOutput.Location = new System.Drawing.Point(12, 22);
            this.labelOutput.Name = "labelOutput";
            this.labelOutput.Size = new System.Drawing.Size(71, 13);
            this.labelOutput.TabIndex = 0;
            this.labelOutput.Text = "Output folder";
            //
            // OutputFolderTextBox
            //
            this.OutputFolderTextBox.Location = new System.Drawing.Point(12, 40);
            this.OutputFolderTextBox.Name = "OutputFolderTextBox";
            this.OutputFolderTextBox.Size = new System.Drawing.Size(252, 20);
            this.OutputFolderTextBox.TabIndex = 1;
            //
            // BrowseOutputButton
            //
            this.BrowseOutputButton.ForeColor = System.Drawing.Color.Black;
            this.BrowseOutputButton.Location = new System.Drawing.Point(270, 38);
            this.BrowseOutputButton.Name = "BrowseOutputButton";
            this.BrowseOutputButton.Size = new System.Drawing.Size(34, 24);
            this.BrowseOutputButton.TabIndex = 2;
            this.BrowseOutputButton.Text = "...";
            this.BrowseOutputButton.UseVisualStyleBackColor = true;
            this.BrowseOutputButton.Click += new System.EventHandler(this.BrowseOutputButton_Click);
            //
            // labelMinZ
            //
            this.labelMinZ.AutoSize = true;
            this.labelMinZ.Location = new System.Drawing.Point(12, 76);
            this.labelMinZ.Name = "labelMinZ";
            this.labelMinZ.Size = new System.Drawing.Size(36, 13);
            this.labelMinZ.TabIndex = 3;
            this.labelMinZ.Text = "Min Z";
            //
            // MinZoomNumeric
            //
            this.MinZoomNumeric.Location = new System.Drawing.Point(70, 74);
            this.MinZoomNumeric.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
            this.MinZoomNumeric.Name = "MinZoomNumeric";
            this.MinZoomNumeric.Size = new System.Drawing.Size(56, 20);
            this.MinZoomNumeric.TabIndex = 4;
            //
            // labelMaxZ
            //
            this.labelMaxZ.AutoSize = true;
            this.labelMaxZ.Location = new System.Drawing.Point(150, 76);
            this.labelMaxZ.Name = "labelMaxZ";
            this.labelMaxZ.Size = new System.Drawing.Size(39, 13);
            this.labelMaxZ.TabIndex = 5;
            this.labelMaxZ.Text = "Max Z";
            //
            // MaxZoomNumeric
            //
            this.MaxZoomNumeric.Location = new System.Drawing.Point(210, 74);
            this.MaxZoomNumeric.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
            this.MaxZoomNumeric.Name = "MaxZoomNumeric";
            this.MaxZoomNumeric.Size = new System.Drawing.Size(56, 20);
            this.MaxZoomNumeric.TabIndex = 6;
            this.MaxZoomNumeric.Value = new decimal(new int[] { 5, 0, 0, 0 });
            //
            // labelTileSize
            //
            this.labelTileSize.AutoSize = true;
            this.labelTileSize.Location = new System.Drawing.Point(12, 110);
            this.labelTileSize.Name = "labelTileSize";
            this.labelTileSize.Size = new System.Drawing.Size(78, 13);
            this.labelTileSize.TabIndex = 7;
            this.labelTileSize.Text = "Tile size (4K)";
            //
            // OutputTileSizeNumeric
            //
            this.OutputTileSizeNumeric.Location = new System.Drawing.Point(150, 108);
            this.OutputTileSizeNumeric.Maximum = new decimal(new int[] { 8192, 0, 0, 0 });
            this.OutputTileSizeNumeric.Minimum = new decimal(new int[] { 4096, 0, 0, 0 });
            this.OutputTileSizeNumeric.Name = "OutputTileSizeNumeric";
            this.OutputTileSizeNumeric.Size = new System.Drawing.Size(72, 20);
            this.OutputTileSizeNumeric.TabIndex = 8;
            this.OutputTileSizeNumeric.Value = new decimal(new int[] { 4096, 0, 0, 0 });
            //
            // labelSettle
            //
            this.labelSettle.AutoSize = true;
            this.labelSettle.Location = new System.Drawing.Point(12, 144);
            this.labelSettle.Name = "labelSettle";
            this.labelSettle.Size = new System.Drawing.Size(70, 13);
            this.labelSettle.TabIndex = 9;
            this.labelSettle.Text = "Settle frames";
            //
            // SettleFramesNumeric
            //
            this.SettleFramesNumeric.Location = new System.Drawing.Point(150, 142);
            this.SettleFramesNumeric.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
            this.SettleFramesNumeric.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            this.SettleFramesNumeric.Name = "SettleFramesNumeric";
            this.SettleFramesNumeric.Size = new System.Drawing.Size(72, 20);
            this.SettleFramesNumeric.TabIndex = 10;
            this.SettleFramesNumeric.Value = new decimal(new int[] { 60, 0, 0, 0 });
            //
            // labelMapDetail
            //
            this.labelMapDetail.AutoSize = true;
            this.labelMapDetail.Location = new System.Drawing.Point(12, 178);
            this.labelMapDetail.Name = "labelMapDetail";
            this.labelMapDetail.Size = new System.Drawing.Size(85, 13);
            this.labelMapDetail.TabIndex = 11;
            this.labelMapDetail.Text = "Map view detail";
            //
            // MapDetailNumeric
            //
            this.MapDetailNumeric.DecimalPlaces = 1;
            this.MapDetailNumeric.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            this.MapDetailNumeric.Location = new System.Drawing.Point(150, 176);
            this.MapDetailNumeric.Maximum = new decimal(new int[] { 50, 0, 0, 65536 });
            this.MapDetailNumeric.Minimum = new decimal(new int[] { 5, 0, 0, 65536 });
            this.MapDetailNumeric.Name = "MapDetailNumeric";
            this.MapDetailNumeric.Size = new System.Drawing.Size(72, 20);
            this.MapDetailNumeric.TabIndex = 12;
            this.MapDetailNumeric.Value = new decimal(new int[] { 30, 0, 0, 65536 });
            this.MapDetailNumeric.ValueChanged += new System.EventHandler(this.MapDetailNumeric_ValueChanged);
            //
            // ShadowsOffCheckBox
            //
            this.ShadowsOffCheckBox.AutoSize = true;
            this.ShadowsOffCheckBox.Checked = true;
            this.ShadowsOffCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShadowsOffCheckBox.Location = new System.Drawing.Point(12, 214);
            this.ShadowsOffCheckBox.Name = "ShadowsOffCheckBox";
            this.ShadowsOffCheckBox.Size = new System.Drawing.Size(108, 17);
            this.ShadowsOffCheckBox.TabIndex = 13;
            this.ShadowsOffCheckBox.Text = "Disable shadows";
            this.ShadowsOffCheckBox.UseVisualStyleBackColor = true;
            this.ShadowsOffCheckBox.CheckedChanged += new System.EventHandler(this.ShadowsOffCheckBox_CheckedChanged);
            //
            // SkipExistingCheckBox
            //
            this.SkipExistingCheckBox.AutoSize = true;
            this.SkipExistingCheckBox.Checked = true;
            this.SkipExistingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SkipExistingCheckBox.Location = new System.Drawing.Point(150, 214);
            this.SkipExistingCheckBox.Name = "SkipExistingCheckBox";
            this.SkipExistingCheckBox.Size = new System.Drawing.Size(112, 17);
            this.SkipExistingCheckBox.TabIndex = 14;
            this.SkipExistingCheckBox.Text = "Skip existing tiles";
            this.SkipExistingCheckBox.UseVisualStyleBackColor = true;
            //
            // LightingGroupBox
            //
            this.LightingGroupBox.Controls.Add(this.DayButton);
            this.LightingGroupBox.Controls.Add(this.NightButton);
            this.LightingGroupBox.Controls.Add(this.TimeOfDayModeLabel);
            this.LightingGroupBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.LightingGroupBox.Location = new System.Drawing.Point(12, 336);
            this.LightingGroupBox.Name = "LightingGroupBox";
            this.LightingGroupBox.Size = new System.Drawing.Size(318, 88);
            this.LightingGroupBox.TabIndex = 2;
            this.LightingGroupBox.TabStop = false;
            this.LightingGroupBox.Text = "Lighting";
            //
            // DayButton
            //
            this.DayButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.DayButton.ForeColor = System.Drawing.Color.Black;
            this.DayButton.Location = new System.Drawing.Point(12, 28);
            this.DayButton.Name = "DayButton";
            this.DayButton.Size = new System.Drawing.Size(142, 28);
            this.DayButton.TabIndex = 0;
            this.DayButton.Text = "Set Day";
            this.DayButton.UseVisualStyleBackColor = true;
            this.DayButton.Click += new System.EventHandler(this.DayButton_Click);
            //
            // NightButton
            //
            this.NightButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.NightButton.ForeColor = System.Drawing.Color.Black;
            this.NightButton.Location = new System.Drawing.Point(164, 28);
            this.NightButton.Name = "NightButton";
            this.NightButton.Size = new System.Drawing.Size(140, 28);
            this.NightButton.TabIndex = 1;
            this.NightButton.Text = "Set Night";
            this.NightButton.UseVisualStyleBackColor = true;
            this.NightButton.Click += new System.EventHandler(this.NightButton_Click);
            //
            // TimeOfDayModeLabel
            //
            this.TimeOfDayModeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.TimeOfDayModeLabel.Location = new System.Drawing.Point(12, 62);
            this.TimeOfDayModeLabel.Name = "TimeOfDayModeLabel";
            this.TimeOfDayModeLabel.Size = new System.Drawing.Size(292, 18);
            this.TimeOfDayModeLabel.TabIndex = 2;
            this.TimeOfDayModeLabel.Text = "Mode: DAY (12:00)";
            //
            // PreviewGroupBox
            //
            this.PreviewGroupBox.Controls.Add(this.labelTileZ);
            this.PreviewGroupBox.Controls.Add(this.TileZNumeric);
            this.PreviewGroupBox.Controls.Add(this.labelTileX);
            this.PreviewGroupBox.Controls.Add(this.TileXNumeric);
            this.PreviewGroupBox.Controls.Add(this.labelTileY);
            this.PreviewGroupBox.Controls.Add(this.TileYNumeric);
            this.PreviewGroupBox.Controls.Add(this.GoToTileButton);
            this.PreviewGroupBox.Controls.Add(this.CoordsLabel);
            this.PreviewGroupBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.PreviewGroupBox.Location = new System.Drawing.Point(12, 436);
            this.PreviewGroupBox.Name = "PreviewGroupBox";
            this.PreviewGroupBox.Size = new System.Drawing.Size(318, 110);
            this.PreviewGroupBox.TabIndex = 3;
            this.PreviewGroupBox.TabStop = false;
            this.PreviewGroupBox.Text = "Preview Tile";
            //
            // labelTileZ
            //
            this.labelTileZ.AutoSize = true;
            this.labelTileZ.Location = new System.Drawing.Point(12, 28);
            this.labelTileZ.Name = "labelTileZ";
            this.labelTileZ.Size = new System.Drawing.Size(14, 13);
            this.labelTileZ.TabIndex = 0;
            this.labelTileZ.Text = "Z";
            //
            // TileZNumeric
            //
            this.TileZNumeric.Location = new System.Drawing.Point(32, 26);
            this.TileZNumeric.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
            this.TileZNumeric.Name = "TileZNumeric";
            this.TileZNumeric.Size = new System.Drawing.Size(48, 20);
            this.TileZNumeric.TabIndex = 1;
            this.TileZNumeric.Value = new decimal(new int[] { 3, 0, 0, 0 });
            //
            // labelTileX
            //
            this.labelTileX.AutoSize = true;
            this.labelTileX.Location = new System.Drawing.Point(92, 28);
            this.labelTileX.Name = "labelTileX";
            this.labelTileX.Size = new System.Drawing.Size(14, 13);
            this.labelTileX.TabIndex = 2;
            this.labelTileX.Text = "X";
            //
            // TileXNumeric
            //
            this.TileXNumeric.Location = new System.Drawing.Point(112, 26);
            this.TileXNumeric.Maximum = new decimal(new int[] { 1023, 0, 0, 0 });
            this.TileXNumeric.Name = "TileXNumeric";
            this.TileXNumeric.Size = new System.Drawing.Size(56, 20);
            this.TileXNumeric.TabIndex = 3;
            //
            // labelTileY
            //
            this.labelTileY.AutoSize = true;
            this.labelTileY.Location = new System.Drawing.Point(180, 28);
            this.labelTileY.Name = "labelTileY";
            this.labelTileY.Size = new System.Drawing.Size(14, 13);
            this.labelTileY.TabIndex = 4;
            this.labelTileY.Text = "Y";
            //
            // TileYNumeric
            //
            this.TileYNumeric.Location = new System.Drawing.Point(200, 26);
            this.TileYNumeric.Maximum = new decimal(new int[] { 1023, 0, 0, 0 });
            this.TileYNumeric.Name = "TileYNumeric";
            this.TileYNumeric.Size = new System.Drawing.Size(56, 20);
            this.TileYNumeric.TabIndex = 5;
            //
            // GoToTileButton
            //
            this.GoToTileButton.ForeColor = System.Drawing.Color.Black;
            this.GoToTileButton.Location = new System.Drawing.Point(12, 56);
            this.GoToTileButton.Name = "GoToTileButton";
            this.GoToTileButton.Size = new System.Drawing.Size(100, 26);
            this.GoToTileButton.TabIndex = 6;
            this.GoToTileButton.Text = "Go To Tile";
            this.GoToTileButton.UseVisualStyleBackColor = true;
            this.GoToTileButton.Click += new System.EventHandler(this.GoToTileButton_Click);
            //
            // CoordsLabel
            //
            this.CoordsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.CoordsLabel.Location = new System.Drawing.Point(122, 56);
            this.CoordsLabel.Name = "CoordsLabel";
            this.CoordsLabel.Size = new System.Drawing.Size(182, 42);
            this.CoordsLabel.TabIndex = 7;
            this.CoordsLabel.Text = "World: -";
            //
            // ProgressGroupBox
            //
            this.ProgressGroupBox.Controls.Add(this.CaptureCurrentButton);
            this.ProgressGroupBox.Controls.Add(this.StartCaptureButton);
            this.ProgressGroupBox.Controls.Add(this.StopCaptureButton);
            this.ProgressGroupBox.Controls.Add(this.CaptureProgressBar);
            this.ProgressGroupBox.Controls.Add(this.ProgressLabel);
            this.ProgressGroupBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ProgressGroupBox.Location = new System.Drawing.Point(12, 558);
            this.ProgressGroupBox.Name = "ProgressGroupBox";
            this.ProgressGroupBox.Size = new System.Drawing.Size(318, 168);
            this.ProgressGroupBox.TabIndex = 4;
            this.ProgressGroupBox.TabStop = false;
            this.ProgressGroupBox.Text = "Capture";
            //
            // CaptureCurrentButton
            //
            this.CaptureCurrentButton.ForeColor = System.Drawing.Color.Black;
            this.CaptureCurrentButton.Location = new System.Drawing.Point(12, 26);
            this.CaptureCurrentButton.Name = "CaptureCurrentButton";
            this.CaptureCurrentButton.Size = new System.Drawing.Size(292, 30);
            this.CaptureCurrentButton.TabIndex = 0;
            this.CaptureCurrentButton.Text = "Capture Current Tile";
            this.CaptureCurrentButton.UseVisualStyleBackColor = true;
            this.CaptureCurrentButton.Click += new System.EventHandler(this.CaptureCurrentButton_Click);
            //
            // StartCaptureButton
            //
            this.StartCaptureButton.ForeColor = System.Drawing.Color.Black;
            this.StartCaptureButton.Location = new System.Drawing.Point(12, 64);
            this.StartCaptureButton.Name = "StartCaptureButton";
            this.StartCaptureButton.Size = new System.Drawing.Size(142, 28);
            this.StartCaptureButton.TabIndex = 1;
            this.StartCaptureButton.Text = "Start Batch";
            this.StartCaptureButton.UseVisualStyleBackColor = true;
            this.StartCaptureButton.Click += new System.EventHandler(this.StartCaptureButton_Click);
            //
            // StopCaptureButton
            //
            this.StopCaptureButton.Enabled = false;
            this.StopCaptureButton.ForeColor = System.Drawing.Color.Black;
            this.StopCaptureButton.Location = new System.Drawing.Point(164, 64);
            this.StopCaptureButton.Name = "StopCaptureButton";
            this.StopCaptureButton.Size = new System.Drawing.Size(140, 28);
            this.StopCaptureButton.TabIndex = 2;
            this.StopCaptureButton.Text = "Stop";
            this.StopCaptureButton.UseVisualStyleBackColor = true;
            this.StopCaptureButton.Click += new System.EventHandler(this.StopCaptureButton_Click);
            //
            // CaptureProgressBar
            //
            this.CaptureProgressBar.Location = new System.Drawing.Point(12, 104);
            this.CaptureProgressBar.Name = "CaptureProgressBar";
            this.CaptureProgressBar.Size = new System.Drawing.Size(292, 16);
            this.CaptureProgressBar.TabIndex = 3;
            //
            // ProgressLabel
            //
            this.ProgressLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.ProgressLabel.Location = new System.Drawing.Point(12, 128);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(292, 30);
            this.ProgressLabel.TabIndex = 4;
            this.ProgressLabel.Text = "Idle";
            //
            // ToolsPanelHideButton
            //
            this.ToolsPanelHideButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ToolsPanelHideButton.ForeColor = System.Drawing.Color.Black;
            this.ToolsPanelHideButton.Location = new System.Drawing.Point(326, 6);
            this.ToolsPanelHideButton.Name = "ToolsPanelHideButton";
            this.ToolsPanelHideButton.Size = new System.Drawing.Size(28, 24);
            this.ToolsPanelHideButton.TabIndex = 5;
            this.ToolsPanelHideButton.Text = "<<";
            this.ToolsPanelHideButton.UseVisualStyleBackColor = true;
            this.ToolsPanelHideButton.Click += new System.EventHandler(this.ToolsPanelHideButton_Click);
            //
            // ToolsPanelShowButton
            //
            this.ToolsPanelShowButton.ForeColor = System.Drawing.Color.Black;
            this.ToolsPanelShowButton.Location = new System.Drawing.Point(0, 0);
            this.ToolsPanelShowButton.Name = "ToolsPanelShowButton";
            this.ToolsPanelShowButton.Size = new System.Drawing.Size(28, 24);
            this.ToolsPanelShowButton.TabIndex = 2;
            this.ToolsPanelShowButton.Text = ">>";
            this.ToolsPanelShowButton.UseVisualStyleBackColor = true;
            this.ToolsPanelShowButton.Visible = false;
            this.ToolsPanelShowButton.Click += new System.EventHandler(this.ToolsPanelShowButton_Click);
            //
            // RealMapForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1280, 750);
            // Dock order: StatusStrip bottom first, then left tools panel
            this.Controls.Add(this.ToolsPanelShowButton);
            this.Controls.Add(this.ToolsPanel);
            this.Controls.Add(this.StatusStrip);
            this.StatusStrip.SendToBack();
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1100, 720);
            this.Name = "RealMapForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CodeWalker RealMap Capture";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RealMapForm_FormClosing);
            this.Load += new System.EventHandler(this.RealMapForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RealMapForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.RealMapForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RealMapForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RealMapForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RealMapForm_MouseUp);
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ToolsPanel.ResumeLayout(false);
            this.ToolsScrollPanel.ResumeLayout(false);
            this.SettingsGroupBox.ResumeLayout(false);
            this.SettingsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinZoomNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxZoomNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputTileSizeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SettleFramesNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapDetailNumeric)).EndInit();
            this.LightingGroupBox.ResumeLayout(false);
            this.PreviewGroupBox.ResumeLayout(false);
            this.PreviewGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TileZNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TileXNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TileYNumeric)).EndInit();
            this.ProgressGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel StatsLabel;
        private System.Windows.Forms.Timer StatsUpdateTimer;
        private System.Windows.Forms.Panel ToolsPanel;
        private System.Windows.Forms.Panel ToolsScrollPanel;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.GroupBox SettingsGroupBox;
        private System.Windows.Forms.Label labelOutput;
        private System.Windows.Forms.TextBox OutputFolderTextBox;
        private System.Windows.Forms.Button BrowseOutputButton;
        private System.Windows.Forms.Label labelMinZ;
        private System.Windows.Forms.NumericUpDown MinZoomNumeric;
        private System.Windows.Forms.Label labelMaxZ;
        private System.Windows.Forms.NumericUpDown MaxZoomNumeric;
        private System.Windows.Forms.Label labelTileSize;
        private System.Windows.Forms.NumericUpDown OutputTileSizeNumeric;
        private System.Windows.Forms.Label labelSettle;
        private System.Windows.Forms.NumericUpDown SettleFramesNumeric;
        private System.Windows.Forms.Label labelMapDetail;
        private System.Windows.Forms.NumericUpDown MapDetailNumeric;
        private System.Windows.Forms.CheckBox ShadowsOffCheckBox;
        private System.Windows.Forms.CheckBox SkipExistingCheckBox;
        private System.Windows.Forms.GroupBox LightingGroupBox;
        private System.Windows.Forms.Button DayButton;
        private System.Windows.Forms.Button NightButton;
        private System.Windows.Forms.Label TimeOfDayModeLabel;
        private System.Windows.Forms.GroupBox PreviewGroupBox;
        private System.Windows.Forms.Label labelTileZ;
        private System.Windows.Forms.NumericUpDown TileZNumeric;
        private System.Windows.Forms.Label labelTileX;
        private System.Windows.Forms.NumericUpDown TileXNumeric;
        private System.Windows.Forms.Label labelTileY;
        private System.Windows.Forms.NumericUpDown TileYNumeric;
        private System.Windows.Forms.Button GoToTileButton;
        private System.Windows.Forms.Label CoordsLabel;
        private System.Windows.Forms.GroupBox ProgressGroupBox;
        private System.Windows.Forms.Button CaptureCurrentButton;
        private System.Windows.Forms.Button StartCaptureButton;
        private System.Windows.Forms.Button StopCaptureButton;
        private System.Windows.Forms.ProgressBar CaptureProgressBar;
        private System.Windows.Forms.Label ProgressLabel;
        private System.Windows.Forms.Button ToolsPanelHideButton;
        private System.Windows.Forms.Button ToolsPanelShowButton;
    }
}
