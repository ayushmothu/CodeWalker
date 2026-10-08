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
            this.WorldPanel = new System.Windows.Forms.Panel();
            this.WorldScrollPanel = new System.Windows.Forms.Panel();
            this.WorldGroupBox = new System.Windows.Forms.GroupBox();
            this.EnableGTAVMapCheckBox = new System.Windows.Forms.CheckBox();
            this.EnableCayoPericoCheckBox = new System.Windows.Forms.CheckBox();
            this.EnableNorthYanktonCheckBox = new System.Windows.Forms.CheckBox();
            this.HideShipsCheckBox = new System.Windows.Forms.CheckBox();
            this.EnableModsCheckBox = new System.Windows.Forms.CheckBox();
            this.EnableDlcCheckBox = new System.Windows.Forms.CheckBox();
            this.labelDlcLevel = new System.Windows.Forms.Label();
            this.DlcLevelComboBox = new System.Windows.Forms.ComboBox();
            this.WorldPanelHideButton = new System.Windows.Forms.Button();
            this.WorldPanelShowButton = new System.Windows.Forms.Button();
            this.StatusStrip.SuspendLayout();
            this.ToolsPanel.SuspendLayout();
            this.ToolsScrollPanel.SuspendLayout();
            this.SettingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinZoomNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxZoomNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputTileSizeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SettleFramesNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapDetailNumeric)).BeginInit();
            this.PreviewGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TileZNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TileXNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TileYNumeric)).BeginInit();
            this.ProgressGroupBox.SuspendLayout();
            this.WorldPanel.SuspendLayout();
            this.WorldScrollPanel.SuspendLayout();
            this.WorldGroupBox.SuspendLayout();
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
            this.ToolsPanel.Size = new System.Drawing.Size(300, 728);
            this.ToolsPanel.TabIndex = 1;
            //
            // ToolsScrollPanel
            //
            this.ToolsScrollPanel.AutoScroll = true;
            this.ToolsScrollPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(46)))));
            this.ToolsScrollPanel.Controls.Add(this.SettingsGroupBox);
            this.ToolsScrollPanel.Controls.Add(this.PreviewGroupBox);
            this.ToolsScrollPanel.Controls.Add(this.ProgressGroupBox);
            this.ToolsScrollPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolsScrollPanel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ToolsScrollPanel.Location = new System.Drawing.Point(0, 0);
            this.ToolsScrollPanel.Name = "ToolsScrollPanel";
            this.ToolsScrollPanel.Padding = new System.Windows.Forms.Padding(8, 30, 8, 8);
            this.ToolsScrollPanel.Size = new System.Drawing.Size(300, 728);
            this.ToolsScrollPanel.TabIndex = 0;
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
            this.SettingsGroupBox.Controls.Add(this.DayButton);
            this.SettingsGroupBox.Controls.Add(this.NightButton);
            this.SettingsGroupBox.Controls.Add(this.TimeOfDayModeLabel);
            this.SettingsGroupBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.SettingsGroupBox.Location = new System.Drawing.Point(10, 32);
            this.SettingsGroupBox.Name = "SettingsGroupBox";
            this.SettingsGroupBox.Size = new System.Drawing.Size(262, 300);
            this.SettingsGroupBox.TabIndex = 0;
            this.SettingsGroupBox.TabStop = false;
            this.SettingsGroupBox.Text = "Settings";
            //
            // labelOutput
            //
            this.labelOutput.AutoSize = true;
            this.labelOutput.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.labelOutput.Location = new System.Drawing.Point(12, 22);
            this.labelOutput.Name = "labelOutput";
            this.labelOutput.Size = new System.Drawing.Size(71, 13);
            this.labelOutput.TabIndex = 0;
            this.labelOutput.Text = "Output folder";
            //
            // OutputFolderTextBox
            //
            this.OutputFolderTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(30)))), ((int)(((byte)(34)))));
            this.OutputFolderTextBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.OutputFolderTextBox.Location = new System.Drawing.Point(12, 38);
            this.OutputFolderTextBox.Name = "OutputFolderTextBox";
            this.OutputFolderTextBox.Size = new System.Drawing.Size(208, 20);
            this.OutputFolderTextBox.TabIndex = 1;
            //
            // BrowseOutputButton
            //
            this.BrowseOutputButton.ForeColor = System.Drawing.Color.Black;
            this.BrowseOutputButton.Location = new System.Drawing.Point(224, 36);
            this.BrowseOutputButton.Name = "BrowseOutputButton";
            this.BrowseOutputButton.Size = new System.Drawing.Size(28, 23);
            this.BrowseOutputButton.TabIndex = 2;
            this.BrowseOutputButton.Text = "...";
            this.BrowseOutputButton.UseVisualStyleBackColor = true;
            this.BrowseOutputButton.Click += new System.EventHandler(this.BrowseOutputButton_Click);
            //
            // labelMinZ
            //
            this.labelMinZ.AutoSize = true;
            this.labelMinZ.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.labelMinZ.Location = new System.Drawing.Point(12, 72);
            this.labelMinZ.Name = "labelMinZ";
            this.labelMinZ.Size = new System.Drawing.Size(36, 13);
            this.labelMinZ.TabIndex = 3;
            this.labelMinZ.Text = "Min Z";
            //
            // MinZoomNumeric
            //
            this.MinZoomNumeric.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(30)))), ((int)(((byte)(34)))));
            this.MinZoomNumeric.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.MinZoomNumeric.Location = new System.Drawing.Point(54, 70);
            this.MinZoomNumeric.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
            this.MinZoomNumeric.Name = "MinZoomNumeric";
            this.MinZoomNumeric.Size = new System.Drawing.Size(50, 20);
            this.MinZoomNumeric.TabIndex = 4;
            //
            // labelMaxZ
            //
            this.labelMaxZ.AutoSize = true;
            this.labelMaxZ.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.labelMaxZ.Location = new System.Drawing.Point(130, 72);
            this.labelMaxZ.Name = "labelMaxZ";
            this.labelMaxZ.Size = new System.Drawing.Size(39, 13);
            this.labelMaxZ.TabIndex = 5;
            this.labelMaxZ.Text = "Max Z";
            //
            // MaxZoomNumeric
            //
            this.MaxZoomNumeric.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(30)))), ((int)(((byte)(34)))));
            this.MaxZoomNumeric.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.MaxZoomNumeric.Location = new System.Drawing.Point(175, 70);
            this.MaxZoomNumeric.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
            this.MaxZoomNumeric.Name = "MaxZoomNumeric";
            this.MaxZoomNumeric.Size = new System.Drawing.Size(50, 20);
            this.MaxZoomNumeric.TabIndex = 6;
            this.MaxZoomNumeric.Value = new decimal(new int[] { 5, 0, 0, 0 });
            //
            // labelTileSize
            //
            this.labelTileSize.AutoSize = true;
            this.labelTileSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.labelTileSize.Location = new System.Drawing.Point(12, 102);
            this.labelTileSize.Name = "labelTileSize";
            this.labelTileSize.Size = new System.Drawing.Size(72, 13);
            this.labelTileSize.TabIndex = 7;
            this.labelTileSize.Text = "Tile size (px)";
            //
            // OutputTileSizeNumeric
            //
            this.OutputTileSizeNumeric.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(30)))), ((int)(((byte)(34)))));
            this.OutputTileSizeNumeric.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.OutputTileSizeNumeric.Location = new System.Drawing.Point(150, 100);
            this.OutputTileSizeNumeric.Maximum = new decimal(new int[] { 8192, 0, 0, 0 });
            this.OutputTileSizeNumeric.Minimum = new decimal(new int[] { 4096, 0, 0, 0 });
            this.OutputTileSizeNumeric.Name = "OutputTileSizeNumeric";
            this.OutputTileSizeNumeric.Size = new System.Drawing.Size(75, 20);
            this.OutputTileSizeNumeric.TabIndex = 8;
            this.OutputTileSizeNumeric.Value = new decimal(new int[] { 4096, 0, 0, 0 });
            //
            // labelSettle
            //
            this.labelSettle.AutoSize = true;
            this.labelSettle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.labelSettle.Location = new System.Drawing.Point(12, 132);
            this.labelSettle.Name = "labelSettle";
            this.labelSettle.Size = new System.Drawing.Size(88, 13);
            this.labelSettle.TabIndex = 9;
            this.labelSettle.Text = "Min wait frames";
            //
            // SettleFramesNumeric
            //
            this.SettleFramesNumeric.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(30)))), ((int)(((byte)(34)))));
            this.SettleFramesNumeric.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.SettleFramesNumeric.Location = new System.Drawing.Point(150, 130);
            this.SettleFramesNumeric.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
            this.SettleFramesNumeric.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            this.SettleFramesNumeric.Name = "SettleFramesNumeric";
            this.SettleFramesNumeric.Size = new System.Drawing.Size(75, 20);
            this.SettleFramesNumeric.TabIndex = 10;
            this.SettleFramesNumeric.Value = new decimal(new int[] { 60, 0, 0, 0 });
            //
            // labelMapDetail
            //
            this.labelMapDetail.AutoSize = true;
            this.labelMapDetail.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.labelMapDetail.Location = new System.Drawing.Point(12, 162);
            this.labelMapDetail.Name = "labelMapDetail";
            this.labelMapDetail.Size = new System.Drawing.Size(85, 13);
            this.labelMapDetail.TabIndex = 11;
            this.labelMapDetail.Text = "Map view detail";
            //
            // MapDetailNumeric
            //
            this.MapDetailNumeric.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(30)))), ((int)(((byte)(34)))));
            this.MapDetailNumeric.DecimalPlaces = 1;
            this.MapDetailNumeric.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.MapDetailNumeric.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            this.MapDetailNumeric.Location = new System.Drawing.Point(150, 160);
            this.MapDetailNumeric.Maximum = new decimal(new int[] { 50, 0, 0, 65536 });
            this.MapDetailNumeric.Minimum = new decimal(new int[] { 5, 0, 0, 65536 });
            this.MapDetailNumeric.Name = "MapDetailNumeric";
            this.MapDetailNumeric.Size = new System.Drawing.Size(75, 20);
            this.MapDetailNumeric.TabIndex = 12;
            this.MapDetailNumeric.Value = new decimal(new int[] { 30, 0, 0, 65536 });
            this.MapDetailNumeric.ValueChanged += new System.EventHandler(this.MapDetailNumeric_ValueChanged);
            //
            // ShadowsOffCheckBox
            //
            this.ShadowsOffCheckBox.AutoSize = true;
            this.ShadowsOffCheckBox.Checked = true;
            this.ShadowsOffCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShadowsOffCheckBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ShadowsOffCheckBox.Location = new System.Drawing.Point(12, 194);
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
            this.SkipExistingCheckBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.SkipExistingCheckBox.Location = new System.Drawing.Point(140, 194);
            this.SkipExistingCheckBox.Name = "SkipExistingCheckBox";
            this.SkipExistingCheckBox.Size = new System.Drawing.Size(112, 17);
            this.SkipExistingCheckBox.TabIndex = 14;
            this.SkipExistingCheckBox.Text = "Skip existing tiles";
            this.SkipExistingCheckBox.UseVisualStyleBackColor = true;
            //
            // DayButton
            //
            this.DayButton.ForeColor = System.Drawing.Color.Black;
            this.DayButton.Location = new System.Drawing.Point(12, 228);
            this.DayButton.Name = "DayButton";
            this.DayButton.Size = new System.Drawing.Size(110, 26);
            this.DayButton.TabIndex = 15;
            this.DayButton.Text = "Day";
            this.DayButton.UseVisualStyleBackColor = true;
            this.DayButton.Click += new System.EventHandler(this.DayButton_Click);
            //
            // NightButton
            //
            this.NightButton.ForeColor = System.Drawing.Color.Black;
            this.NightButton.Location = new System.Drawing.Point(132, 228);
            this.NightButton.Name = "NightButton";
            this.NightButton.Size = new System.Drawing.Size(110, 26);
            this.NightButton.TabIndex = 16;
            this.NightButton.Text = "Night";
            this.NightButton.UseVisualStyleBackColor = true;
            this.NightButton.Click += new System.EventHandler(this.NightButton_Click);
            //
            // TimeOfDayModeLabel
            //
            this.TimeOfDayModeLabel.AutoSize = true;
            this.TimeOfDayModeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.TimeOfDayModeLabel.Location = new System.Drawing.Point(12, 266);
            this.TimeOfDayModeLabel.Name = "TimeOfDayModeLabel";
            this.TimeOfDayModeLabel.Size = new System.Drawing.Size(100, 13);
            this.TimeOfDayModeLabel.TabIndex = 17;
            this.TimeOfDayModeLabel.Text = "Time: Day (12:00)";
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
            this.PreviewGroupBox.Location = new System.Drawing.Point(10, 344);
            this.PreviewGroupBox.Name = "PreviewGroupBox";
            this.PreviewGroupBox.Size = new System.Drawing.Size(262, 108);
            this.PreviewGroupBox.TabIndex = 1;
            this.PreviewGroupBox.TabStop = false;
            this.PreviewGroupBox.Text = "Preview";
            //
            // labelTileZ
            //
            this.labelTileZ.AutoSize = true;
            this.labelTileZ.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.labelTileZ.Location = new System.Drawing.Point(12, 26);
            this.labelTileZ.Name = "labelTileZ";
            this.labelTileZ.Size = new System.Drawing.Size(14, 13);
            this.labelTileZ.TabIndex = 0;
            this.labelTileZ.Text = "Z";
            //
            // TileZNumeric
            //
            this.TileZNumeric.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(30)))), ((int)(((byte)(34)))));
            this.TileZNumeric.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.TileZNumeric.Location = new System.Drawing.Point(30, 24);
            this.TileZNumeric.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
            this.TileZNumeric.Name = "TileZNumeric";
            this.TileZNumeric.Size = new System.Drawing.Size(42, 20);
            this.TileZNumeric.TabIndex = 1;
            this.TileZNumeric.Value = new decimal(new int[] { 3, 0, 0, 0 });
            //
            // labelTileX
            //
            this.labelTileX.AutoSize = true;
            this.labelTileX.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.labelTileX.Location = new System.Drawing.Point(86, 26);
            this.labelTileX.Name = "labelTileX";
            this.labelTileX.Size = new System.Drawing.Size(14, 13);
            this.labelTileX.TabIndex = 2;
            this.labelTileX.Text = "X";
            //
            // TileXNumeric
            //
            this.TileXNumeric.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(30)))), ((int)(((byte)(34)))));
            this.TileXNumeric.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.TileXNumeric.Location = new System.Drawing.Point(104, 24);
            this.TileXNumeric.Maximum = new decimal(new int[] { 1023, 0, 0, 0 });
            this.TileXNumeric.Name = "TileXNumeric";
            this.TileXNumeric.Size = new System.Drawing.Size(50, 20);
            this.TileXNumeric.TabIndex = 3;
            //
            // labelTileY
            //
            this.labelTileY.AutoSize = true;
            this.labelTileY.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.labelTileY.Location = new System.Drawing.Point(168, 26);
            this.labelTileY.Name = "labelTileY";
            this.labelTileY.Size = new System.Drawing.Size(14, 13);
            this.labelTileY.TabIndex = 4;
            this.labelTileY.Text = "Y";
            //
            // TileYNumeric
            //
            this.TileYNumeric.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(30)))), ((int)(((byte)(34)))));
            this.TileYNumeric.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.TileYNumeric.Location = new System.Drawing.Point(186, 24);
            this.TileYNumeric.Maximum = new decimal(new int[] { 1023, 0, 0, 0 });
            this.TileYNumeric.Name = "TileYNumeric";
            this.TileYNumeric.Size = new System.Drawing.Size(50, 20);
            this.TileYNumeric.TabIndex = 5;
            //
            // GoToTileButton
            //
            this.GoToTileButton.ForeColor = System.Drawing.Color.Black;
            this.GoToTileButton.Location = new System.Drawing.Point(12, 56);
            this.GoToTileButton.Name = "GoToTileButton";
            this.GoToTileButton.Size = new System.Drawing.Size(75, 24);
            this.GoToTileButton.TabIndex = 6;
            this.GoToTileButton.Text = "Go To";
            this.GoToTileButton.UseVisualStyleBackColor = true;
            this.GoToTileButton.Click += new System.EventHandler(this.GoToTileButton_Click);
            //
            // CoordsLabel
            //
            this.CoordsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.CoordsLabel.Location = new System.Drawing.Point(96, 56);
            this.CoordsLabel.Name = "CoordsLabel";
            this.CoordsLabel.Size = new System.Drawing.Size(152, 40);
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
            this.ProgressGroupBox.Location = new System.Drawing.Point(10, 464);
            this.ProgressGroupBox.Name = "ProgressGroupBox";
            this.ProgressGroupBox.Size = new System.Drawing.Size(262, 156);
            this.ProgressGroupBox.TabIndex = 2;
            this.ProgressGroupBox.TabStop = false;
            this.ProgressGroupBox.Text = "Capture";
            //
            // CaptureCurrentButton
            //
            this.CaptureCurrentButton.ForeColor = System.Drawing.Color.Black;
            this.CaptureCurrentButton.Location = new System.Drawing.Point(12, 24);
            this.CaptureCurrentButton.Name = "CaptureCurrentButton";
            this.CaptureCurrentButton.Size = new System.Drawing.Size(236, 28);
            this.CaptureCurrentButton.TabIndex = 0;
            this.CaptureCurrentButton.Text = "Capture Current Tile";
            this.CaptureCurrentButton.UseVisualStyleBackColor = true;
            this.CaptureCurrentButton.Click += new System.EventHandler(this.CaptureCurrentButton_Click);
            //
            // StartCaptureButton
            //
            this.StartCaptureButton.ForeColor = System.Drawing.Color.Black;
            this.StartCaptureButton.Location = new System.Drawing.Point(12, 60);
            this.StartCaptureButton.Name = "StartCaptureButton";
            this.StartCaptureButton.Size = new System.Drawing.Size(114, 26);
            this.StartCaptureButton.TabIndex = 1;
            this.StartCaptureButton.Text = "Start Batch";
            this.StartCaptureButton.UseVisualStyleBackColor = true;
            this.StartCaptureButton.Click += new System.EventHandler(this.StartCaptureButton_Click);
            //
            // StopCaptureButton
            //
            this.StopCaptureButton.Enabled = false;
            this.StopCaptureButton.ForeColor = System.Drawing.Color.Black;
            this.StopCaptureButton.Location = new System.Drawing.Point(134, 60);
            this.StopCaptureButton.Name = "StopCaptureButton";
            this.StopCaptureButton.Size = new System.Drawing.Size(114, 26);
            this.StopCaptureButton.TabIndex = 2;
            this.StopCaptureButton.Text = "Stop";
            this.StopCaptureButton.UseVisualStyleBackColor = true;
            this.StopCaptureButton.Click += new System.EventHandler(this.StopCaptureButton_Click);
            //
            // CaptureProgressBar
            //
            this.CaptureProgressBar.Location = new System.Drawing.Point(12, 98);
            this.CaptureProgressBar.Name = "CaptureProgressBar";
            this.CaptureProgressBar.Size = new System.Drawing.Size(236, 16);
            this.CaptureProgressBar.TabIndex = 3;
            //
            // ProgressLabel
            //
            this.ProgressLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.ProgressLabel.Location = new System.Drawing.Point(12, 120);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(236, 28);
            this.ProgressLabel.TabIndex = 4;
            this.ProgressLabel.Text = "Idle";
            //
            // ToolsPanelHideButton
            //
            this.ToolsPanelHideButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ToolsPanelHideButton.ForeColor = System.Drawing.Color.Black;
            this.ToolsPanelHideButton.Location = new System.Drawing.Point(268, 4);
            this.ToolsPanelHideButton.Name = "ToolsPanelHideButton";
            this.ToolsPanelHideButton.Size = new System.Drawing.Size(28, 22);
            this.ToolsPanelHideButton.TabIndex = 3;
            this.ToolsPanelHideButton.Text = "<<";
            this.ToolsPanelHideButton.UseVisualStyleBackColor = true;
            this.ToolsPanelHideButton.Click += new System.EventHandler(this.ToolsPanelHideButton_Click);
            //
            // ToolsPanelShowButton
            //
            this.ToolsPanelShowButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.ToolsPanelShowButton.ForeColor = System.Drawing.Color.Black;
            this.ToolsPanelShowButton.Location = new System.Drawing.Point(4, 4);
            this.ToolsPanelShowButton.Name = "ToolsPanelShowButton";
            this.ToolsPanelShowButton.Size = new System.Drawing.Size(28, 22);
            this.ToolsPanelShowButton.TabIndex = 2;
            this.ToolsPanelShowButton.Text = ">>";
            this.ToolsPanelShowButton.UseVisualStyleBackColor = true;
            this.ToolsPanelShowButton.Visible = false;
            this.ToolsPanelShowButton.Click += new System.EventHandler(this.ToolsPanelShowButton_Click);
            //
            // WorldPanel
            //
            this.WorldPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(46)))));
            this.WorldPanel.Controls.Add(this.WorldScrollPanel);
            this.WorldPanel.Controls.Add(this.WorldPanelHideButton);
            this.WorldPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.WorldPanel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.WorldPanel.Location = new System.Drawing.Point(1040, 0);
            this.WorldPanel.Name = "WorldPanel";
            this.WorldPanel.Size = new System.Drawing.Size(240, 728);
            this.WorldPanel.TabIndex = 3;
            //
            // WorldScrollPanel
            //
            this.WorldScrollPanel.AutoScroll = true;
            this.WorldScrollPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(42)))), ((int)(((byte)(46)))));
            this.WorldScrollPanel.Controls.Add(this.WorldGroupBox);
            this.WorldScrollPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WorldScrollPanel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.WorldScrollPanel.Location = new System.Drawing.Point(0, 0);
            this.WorldScrollPanel.Name = "WorldScrollPanel";
            this.WorldScrollPanel.Padding = new System.Windows.Forms.Padding(8, 30, 8, 8);
            this.WorldScrollPanel.Size = new System.Drawing.Size(240, 728);
            this.WorldScrollPanel.TabIndex = 0;
            //
            // WorldGroupBox
            //
            this.WorldGroupBox.Controls.Add(this.EnableGTAVMapCheckBox);
            this.WorldGroupBox.Controls.Add(this.EnableCayoPericoCheckBox);
            this.WorldGroupBox.Controls.Add(this.EnableNorthYanktonCheckBox);
            this.WorldGroupBox.Controls.Add(this.HideShipsCheckBox);
            this.WorldGroupBox.Controls.Add(this.EnableModsCheckBox);
            this.WorldGroupBox.Controls.Add(this.EnableDlcCheckBox);
            this.WorldGroupBox.Controls.Add(this.labelDlcLevel);
            this.WorldGroupBox.Controls.Add(this.DlcLevelComboBox);
            this.WorldGroupBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.WorldGroupBox.Location = new System.Drawing.Point(8, 32);
            this.WorldGroupBox.Name = "WorldGroupBox";
            this.WorldGroupBox.Size = new System.Drawing.Size(208, 300);
            this.WorldGroupBox.TabIndex = 0;
            this.WorldGroupBox.TabStop = false;
            this.WorldGroupBox.Text = "World / DLC";
            //
            // EnableGTAVMapCheckBox
            //
            this.EnableGTAVMapCheckBox.AutoSize = true;
            this.EnableGTAVMapCheckBox.Checked = true;
            this.EnableGTAVMapCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnableGTAVMapCheckBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.EnableGTAVMapCheckBox.Location = new System.Drawing.Point(12, 28);
            this.EnableGTAVMapCheckBox.Name = "EnableGTAVMapCheckBox";
            this.EnableGTAVMapCheckBox.Size = new System.Drawing.Size(118, 17);
            this.EnableGTAVMapCheckBox.TabIndex = 0;
            this.EnableGTAVMapCheckBox.Text = "Display Main Map";
            this.EnableGTAVMapCheckBox.UseVisualStyleBackColor = true;
            this.EnableGTAVMapCheckBox.CheckedChanged += new System.EventHandler(this.EnableGTAVMapCheckBox_CheckedChanged);
            //
            // EnableCayoPericoCheckBox
            //
            this.EnableCayoPericoCheckBox.AutoSize = true;
            this.EnableCayoPericoCheckBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.EnableCayoPericoCheckBox.Location = new System.Drawing.Point(12, 54);
            this.EnableCayoPericoCheckBox.Name = "EnableCayoPericoCheckBox";
            this.EnableCayoPericoCheckBox.Size = new System.Drawing.Size(140, 17);
            this.EnableCayoPericoCheckBox.TabIndex = 1;
            this.EnableCayoPericoCheckBox.Text = "Display Cayo Perico Map";
            this.EnableCayoPericoCheckBox.UseVisualStyleBackColor = true;
            this.EnableCayoPericoCheckBox.CheckedChanged += new System.EventHandler(this.EnableCayoPericoCheckBox_CheckedChanged);
            //
            // EnableNorthYanktonCheckBox
            //
            this.EnableNorthYanktonCheckBox.AutoSize = true;
            this.EnableNorthYanktonCheckBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.EnableNorthYanktonCheckBox.Location = new System.Drawing.Point(12, 80);
            this.EnableNorthYanktonCheckBox.Name = "EnableNorthYanktonCheckBox";
            this.EnableNorthYanktonCheckBox.Size = new System.Drawing.Size(158, 17);
            this.EnableNorthYanktonCheckBox.TabIndex = 2;
            this.EnableNorthYanktonCheckBox.Text = "Display North Yankton Map";
            this.EnableNorthYanktonCheckBox.UseVisualStyleBackColor = true;
            this.EnableNorthYanktonCheckBox.CheckedChanged += new System.EventHandler(this.EnableNorthYanktonCheckBox_CheckedChanged);
            //
            // HideShipsCheckBox
            //
            this.HideShipsCheckBox.AutoSize = true;
            this.HideShipsCheckBox.Checked = true;
            this.HideShipsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HideShipsCheckBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.HideShipsCheckBox.Location = new System.Drawing.Point(12, 106);
            this.HideShipsCheckBox.Name = "HideShipsCheckBox";
            this.HideShipsCheckBox.Size = new System.Drawing.Size(178, 17);
            this.HideShipsCheckBox.TabIndex = 3;
            this.HideShipsCheckBox.Text = "Hide ships / yachts / boxes";
            this.HideShipsCheckBox.UseVisualStyleBackColor = true;
            this.HideShipsCheckBox.CheckedChanged += new System.EventHandler(this.HideShipsCheckBox_CheckedChanged);
            //
            // EnableModsCheckBox
            //
            this.EnableModsCheckBox.AutoSize = true;
            this.EnableModsCheckBox.Enabled = false;
            this.EnableModsCheckBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.EnableModsCheckBox.Location = new System.Drawing.Point(12, 148);
            this.EnableModsCheckBox.Name = "EnableModsCheckBox";
            this.EnableModsCheckBox.Size = new System.Drawing.Size(88, 17);
            this.EnableModsCheckBox.TabIndex = 4;
            this.EnableModsCheckBox.Text = "Enable Mods";
            this.EnableModsCheckBox.UseVisualStyleBackColor = true;
            this.EnableModsCheckBox.CheckedChanged += new System.EventHandler(this.EnableModsCheckBox_CheckedChanged);
            //
            // EnableDlcCheckBox
            //
            this.EnableDlcCheckBox.AutoSize = true;
            this.EnableDlcCheckBox.Checked = true;
            this.EnableDlcCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnableDlcCheckBox.Enabled = false;
            this.EnableDlcCheckBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.EnableDlcCheckBox.Location = new System.Drawing.Point(12, 174);
            this.EnableDlcCheckBox.Name = "EnableDlcCheckBox";
            this.EnableDlcCheckBox.Size = new System.Drawing.Size(83, 17);
            this.EnableDlcCheckBox.TabIndex = 5;
            this.EnableDlcCheckBox.Text = "Enable DLC";
            this.EnableDlcCheckBox.UseVisualStyleBackColor = true;
            this.EnableDlcCheckBox.CheckedChanged += new System.EventHandler(this.EnableDlcCheckBox_CheckedChanged);
            //
            // labelDlcLevel
            //
            this.labelDlcLevel.AutoSize = true;
            this.labelDlcLevel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.labelDlcLevel.Location = new System.Drawing.Point(10, 208);
            this.labelDlcLevel.Name = "labelDlcLevel";
            this.labelDlcLevel.Size = new System.Drawing.Size(60, 13);
            this.labelDlcLevel.TabIndex = 6;
            this.labelDlcLevel.Text = "DLC Level:";
            //
            // DlcLevelComboBox
            //
            this.DlcLevelComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(30)))), ((int)(((byte)(34)))));
            this.DlcLevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DlcLevelComboBox.Enabled = false;
            this.DlcLevelComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DlcLevelComboBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.DlcLevelComboBox.FormattingEnabled = true;
            this.DlcLevelComboBox.Items.AddRange(new object[] {
            "<Loading...>"});
            this.DlcLevelComboBox.Location = new System.Drawing.Point(12, 226);
            this.DlcLevelComboBox.Name = "DlcLevelComboBox";
            this.DlcLevelComboBox.Size = new System.Drawing.Size(180, 21);
            this.DlcLevelComboBox.TabIndex = 7;
            this.DlcLevelComboBox.SelectedIndexChanged += new System.EventHandler(this.DlcLevelComboBox_SelectedIndexChanged);
            //
            // WorldPanelHideButton
            //
            this.WorldPanelHideButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.WorldPanelHideButton.ForeColor = System.Drawing.Color.Black;
            this.WorldPanelHideButton.Location = new System.Drawing.Point(4, 4);
            this.WorldPanelHideButton.Name = "WorldPanelHideButton";
            this.WorldPanelHideButton.Size = new System.Drawing.Size(28, 22);
            this.WorldPanelHideButton.TabIndex = 1;
            this.WorldPanelHideButton.Text = ">>";
            this.WorldPanelHideButton.UseVisualStyleBackColor = true;
            this.WorldPanelHideButton.Click += new System.EventHandler(this.WorldPanelHideButton_Click);
            //
            // WorldPanelShowButton
            //
            this.WorldPanelShowButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.WorldPanelShowButton.ForeColor = System.Drawing.Color.Black;
            this.WorldPanelShowButton.Location = new System.Drawing.Point(1248, 4);
            this.WorldPanelShowButton.Name = "WorldPanelShowButton";
            this.WorldPanelShowButton.Size = new System.Drawing.Size(28, 22);
            this.WorldPanelShowButton.TabIndex = 4;
            this.WorldPanelShowButton.Text = "<<";
            this.WorldPanelShowButton.UseVisualStyleBackColor = true;
            this.WorldPanelShowButton.Visible = false;
            this.WorldPanelShowButton.Click += new System.EventHandler(this.WorldPanelShowButton_Click);
            //
            // RealMapForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1280, 750);
            this.Controls.Add(this.ToolsPanelShowButton);
            this.Controls.Add(this.WorldPanelShowButton);
            this.Controls.Add(this.ToolsPanel);
            this.Controls.Add(this.WorldPanel);
            this.Controls.Add(this.StatusStrip);
            this.StatusStrip.SendToBack();
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1024, 700);
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
            this.PreviewGroupBox.ResumeLayout(false);
            this.PreviewGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TileZNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TileXNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TileYNumeric)).EndInit();
            this.ProgressGroupBox.ResumeLayout(false);
            this.WorldPanel.ResumeLayout(false);
            this.WorldScrollPanel.ResumeLayout(false);
            this.WorldGroupBox.ResumeLayout(false);
            this.WorldGroupBox.PerformLayout();
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
        private System.Windows.Forms.Panel WorldPanel;
        private System.Windows.Forms.Panel WorldScrollPanel;
        private System.Windows.Forms.GroupBox WorldGroupBox;
        private System.Windows.Forms.CheckBox EnableGTAVMapCheckBox;
        private System.Windows.Forms.CheckBox EnableCayoPericoCheckBox;
        private System.Windows.Forms.CheckBox EnableNorthYanktonCheckBox;
        private System.Windows.Forms.CheckBox HideShipsCheckBox;
        private System.Windows.Forms.CheckBox EnableModsCheckBox;
        private System.Windows.Forms.CheckBox EnableDlcCheckBox;
        private System.Windows.Forms.Label labelDlcLevel;
        private System.Windows.Forms.ComboBox DlcLevelComboBox;
        private System.Windows.Forms.Button WorldPanelHideButton;
        private System.Windows.Forms.Button WorldPanelShowButton;
    }
}
