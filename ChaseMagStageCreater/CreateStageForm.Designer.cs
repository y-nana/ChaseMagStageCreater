
namespace ChaseMagStageCreater
{
    partial class CreateStageForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.stagePartsGroupBox = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.scaffoldButton = new System.Windows.Forms.RadioButton();
            this.jumpRampButton = new System.Windows.Forms.RadioButton();
            this.wallButton = new System.Windows.Forms.RadioButton();
            this.NormalWallButton = new System.Windows.Forms.RadioButton();
            this.itemBoxButton = new System.Windows.Forms.RadioButton();
            this.partsListBox = new System.Windows.Forms.ListBox();
            this.positionX = new System.Windows.Forms.NumericUpDown();
            this.positionY = new System.Windows.Forms.NumericUpDown();
            this.sizeX = new System.Windows.Forms.NumericUpDown();
            this.polePanel = new System.Windows.Forms.Panel();
            this.northButton = new System.Windows.Forms.RadioButton();
            this.southButton = new System.Windows.Forms.RadioButton();
            this.poleLabel = new System.Windows.Forms.Label();
            this.positionLabel = new System.Windows.Forms.Label();
            this.xPosLabel = new System.Windows.Forms.Label();
            this.yPosLabel = new System.Windows.Forms.Label();
            this.sizeLabel = new System.Windows.Forms.Label();
            this.partDataGroup = new System.Windows.Forms.GroupBox();
            this.ySizeLabel = new System.Windows.Forms.Label();
            this.xSizeLabel = new System.Windows.Forms.Label();
            this.sizeY = new System.Windows.Forms.NumericUpDown();
            this.heightSize = new System.Windows.Forms.NumericUpDown();
            this.stageSizeGroup = new System.Windows.Forms.GroupBox();
            this.widthLabel = new System.Windows.Forms.Label();
            this.heightLabel = new System.Windows.Forms.Label();
            this.widthSize = new System.Windows.Forms.NumericUpDown();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateNewFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OverWriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitAppToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.AllPartsLabel = new System.Windows.Forms.Label();
            this.msgText = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.InStagePicture = new System.Windows.Forms.PictureBox();
            this.stageBox = new System.Windows.Forms.PictureBox();
            this.playerViewCheckBox = new System.Windows.Forms.CheckBox();
            this.bottomLeftLabel = new System.Windows.Forms.Label();
            this.bottomRightLabel = new System.Windows.Forms.Label();
            this.topRightLabel = new System.Windows.Forms.Label();
            this.topleftLabel = new System.Windows.Forms.Label();
            this.addModeButtom = new System.Windows.Forms.RadioButton();
            this.viewMoveModeButton = new System.Windows.Forms.RadioButton();
            this.mode = new System.Windows.Forms.GroupBox();
            this.stagePartsGroupBox.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeX)).BeginInit();
            this.polePanel.SuspendLayout();
            this.partDataGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightSize)).BeginInit();
            this.stageSizeGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.widthSize)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InStagePicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stageBox)).BeginInit();
            this.mode.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // stagePartsGroupBox
            // 
            this.stagePartsGroupBox.Controls.Add(this.flowLayoutPanel1);
            this.stagePartsGroupBox.Location = new System.Drawing.Point(12, 174);
            this.stagePartsGroupBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.stagePartsGroupBox.Name = "stagePartsGroupBox";
            this.stagePartsGroupBox.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.stagePartsGroupBox.Size = new System.Drawing.Size(176, 249);
            this.stagePartsGroupBox.TabIndex = 14;
            this.stagePartsGroupBox.TabStop = false;
            this.stagePartsGroupBox.Text = "ステージパーツ";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.scaffoldButton);
            this.flowLayoutPanel1.Controls.Add(this.jumpRampButton);
            this.flowLayoutPanel1.Controls.Add(this.wallButton);
            this.flowLayoutPanel1.Controls.Add(this.NormalWallButton);
            this.flowLayoutPanel1.Controls.Add(this.itemBoxButton);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(7, 36);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(147, 201);
            this.flowLayoutPanel1.TabIndex = 15;
            // 
            // scaffoldButton
            // 
            this.scaffoldButton.AutoSize = true;
            this.scaffoldButton.Location = new System.Drawing.Point(3, 4);
            this.scaffoldButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.scaffoldButton.Name = "scaffoldButton";
            this.scaffoldButton.Size = new System.Drawing.Size(73, 31);
            this.scaffoldButton.TabIndex = 0;
            this.scaffoldButton.TabStop = true;
            this.scaffoldButton.Text = "足場";
            this.scaffoldButton.UseVisualStyleBackColor = true;
            // 
            // jumpRampButton
            // 
            this.jumpRampButton.AutoSize = true;
            this.jumpRampButton.Location = new System.Drawing.Point(3, 43);
            this.jumpRampButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.jumpRampButton.Name = "jumpRampButton";
            this.jumpRampButton.Size = new System.Drawing.Size(91, 31);
            this.jumpRampButton.TabIndex = 1;
            this.jumpRampButton.TabStop = true;
            this.jumpRampButton.Text = "磁力台";
            this.jumpRampButton.UseVisualStyleBackColor = true;
            // 
            // wallButton
            // 
            this.wallButton.AutoSize = true;
            this.wallButton.Location = new System.Drawing.Point(3, 82);
            this.wallButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.wallButton.Name = "wallButton";
            this.wallButton.Size = new System.Drawing.Size(91, 31);
            this.wallButton.TabIndex = 2;
            this.wallButton.TabStop = true;
            this.wallButton.Text = "磁力壁";
            this.wallButton.UseVisualStyleBackColor = true;
            // 
            // NormalWallButton
            // 
            this.NormalWallButton.AutoSize = true;
            this.NormalWallButton.Location = new System.Drawing.Point(3, 121);
            this.NormalWallButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.NormalWallButton.Name = "NormalWallButton";
            this.NormalWallButton.Size = new System.Drawing.Size(109, 31);
            this.NormalWallButton.TabIndex = 3;
            this.NormalWallButton.TabStop = true;
            this.NormalWallButton.Text = "普通の壁";
            this.NormalWallButton.UseVisualStyleBackColor = true;
            // 
            // itemBoxButton
            // 
            this.itemBoxButton.AutoSize = true;
            this.itemBoxButton.Location = new System.Drawing.Point(3, 160);
            this.itemBoxButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.itemBoxButton.Name = "itemBoxButton";
            this.itemBoxButton.Size = new System.Drawing.Size(127, 31);
            this.itemBoxButton.TabIndex = 4;
            this.itemBoxButton.TabStop = true;
            this.itemBoxButton.Text = "アイテム箱";
            this.itemBoxButton.UseVisualStyleBackColor = true;
            // 
            // partsListBox
            // 
            this.partsListBox.FormattingEnabled = true;
            this.partsListBox.ItemHeight = 27;
            this.partsListBox.Location = new System.Drawing.Point(950, 93);
            this.partsListBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.partsListBox.Name = "partsListBox";
            this.partsListBox.Size = new System.Drawing.Size(223, 220);
            this.partsListBox.TabIndex = 16;
            this.partsListBox.SelectedIndexChanged += new System.EventHandler(this.partsListBox_SelectedIndexChanged);
            // 
            // positionX
            // 
            this.positionX.DecimalPlaces = 1;
            this.positionX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.positionX.Location = new System.Drawing.Point(115, 38);
            this.positionX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.positionX.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.positionX.Name = "positionX";
            this.positionX.Size = new System.Drawing.Size(100, 34);
            this.positionX.TabIndex = 17;
            this.positionX.ValueChanged += new System.EventHandler(this.position_ValueChanged_X);
            // 
            // positionY
            // 
            this.positionY.DecimalPlaces = 1;
            this.positionY.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.positionY.Location = new System.Drawing.Point(115, 80);
            this.positionY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.positionY.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.positionY.Name = "positionY";
            this.positionY.Size = new System.Drawing.Size(100, 34);
            this.positionY.TabIndex = 18;
            this.positionY.ValueChanged += new System.EventHandler(this.position_ValueChanged_Y);
            // 
            // sizeX
            // 
            this.sizeX.DecimalPlaces = 1;
            this.sizeX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.sizeX.Location = new System.Drawing.Point(115, 122);
            this.sizeX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sizeX.Name = "sizeX";
            this.sizeX.Size = new System.Drawing.Size(100, 34);
            this.sizeX.TabIndex = 19;
            this.sizeX.ValueChanged += new System.EventHandler(this.size_ValueChanged_X);
            // 
            // polePanel
            // 
            this.polePanel.Controls.Add(this.northButton);
            this.polePanel.Controls.Add(this.southButton);
            this.polePanel.Controls.Add(this.poleLabel);
            this.polePanel.Location = new System.Drawing.Point(6, 206);
            this.polePanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.polePanel.Name = "polePanel";
            this.polePanel.Size = new System.Drawing.Size(209, 72);
            this.polePanel.TabIndex = 20;
            // 
            // northButton
            // 
            this.northButton.AutoSize = true;
            this.northButton.Location = new System.Drawing.Point(130, 37);
            this.northButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.northButton.Name = "northButton";
            this.northButton.Size = new System.Drawing.Size(68, 31);
            this.northButton.TabIndex = 1;
            this.northButton.TabStop = true;
            this.northButton.Text = "N極";
            this.northButton.UseVisualStyleBackColor = true;
            // 
            // southButton
            // 
            this.southButton.AutoSize = true;
            this.southButton.Location = new System.Drawing.Point(43, 37);
            this.southButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.southButton.Name = "southButton";
            this.southButton.Size = new System.Drawing.Size(66, 31);
            this.southButton.TabIndex = 0;
            this.southButton.TabStop = true;
            this.southButton.Text = "S極";
            this.southButton.UseVisualStyleBackColor = true;
            this.southButton.CheckedChanged += new System.EventHandler(this.poleRadioButton_CheckedChanged);
            // 
            // poleLabel
            // 
            this.poleLabel.AutoSize = true;
            this.poleLabel.Location = new System.Drawing.Point(-1, 6);
            this.poleLabel.Name = "poleLabel";
            this.poleLabel.Size = new System.Drawing.Size(84, 27);
            this.poleLabel.TabIndex = 25;
            this.poleLabel.Text = "磁力の極";
            // 
            // positionLabel
            // 
            this.positionLabel.AutoSize = true;
            this.positionLabel.Location = new System.Drawing.Point(6, 45);
            this.positionLabel.Name = "positionLabel";
            this.positionLabel.Size = new System.Drawing.Size(48, 27);
            this.positionLabel.TabIndex = 21;
            this.positionLabel.Text = "位置";
            // 
            // xPosLabel
            // 
            this.xPosLabel.AutoSize = true;
            this.xPosLabel.Location = new System.Drawing.Point(87, 45);
            this.xPosLabel.Name = "xPosLabel";
            this.xPosLabel.Size = new System.Drawing.Size(22, 27);
            this.xPosLabel.TabIndex = 22;
            this.xPosLabel.Text = "x";
            // 
            // yPosLabel
            // 
            this.yPosLabel.AutoSize = true;
            this.yPosLabel.Location = new System.Drawing.Point(87, 82);
            this.yPosLabel.Name = "yPosLabel";
            this.yPosLabel.Size = new System.Drawing.Size(22, 27);
            this.yPosLabel.TabIndex = 23;
            this.yPosLabel.Text = "y";
            // 
            // sizeLabel
            // 
            this.sizeLabel.AutoSize = true;
            this.sizeLabel.Location = new System.Drawing.Point(5, 129);
            this.sizeLabel.Name = "sizeLabel";
            this.sizeLabel.Size = new System.Drawing.Size(66, 27);
            this.sizeLabel.TabIndex = 24;
            this.sizeLabel.Text = "サイズ";
            // 
            // partDataGroup
            // 
            this.partDataGroup.Controls.Add(this.ySizeLabel);
            this.partDataGroup.Controls.Add(this.xSizeLabel);
            this.partDataGroup.Controls.Add(this.sizeY);
            this.partDataGroup.Controls.Add(this.positionX);
            this.partDataGroup.Controls.Add(this.positionY);
            this.partDataGroup.Controls.Add(this.sizeLabel);
            this.partDataGroup.Controls.Add(this.sizeX);
            this.partDataGroup.Controls.Add(this.yPosLabel);
            this.partDataGroup.Controls.Add(this.polePanel);
            this.partDataGroup.Controls.Add(this.xPosLabel);
            this.partDataGroup.Controls.Add(this.positionLabel);
            this.partDataGroup.Location = new System.Drawing.Point(952, 326);
            this.partDataGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.partDataGroup.Name = "partDataGroup";
            this.partDataGroup.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.partDataGroup.Size = new System.Drawing.Size(221, 292);
            this.partDataGroup.TabIndex = 26;
            this.partDataGroup.TabStop = false;
            this.partDataGroup.Text = "パーツのデータ";
            // 
            // ySizeLabel
            // 
            this.ySizeLabel.AutoSize = true;
            this.ySizeLabel.Location = new System.Drawing.Point(87, 166);
            this.ySizeLabel.Name = "ySizeLabel";
            this.ySizeLabel.Size = new System.Drawing.Size(22, 27);
            this.ySizeLabel.TabIndex = 28;
            this.ySizeLabel.Text = "y";
            // 
            // xSizeLabel
            // 
            this.xSizeLabel.AutoSize = true;
            this.xSizeLabel.Location = new System.Drawing.Point(87, 124);
            this.xSizeLabel.Name = "xSizeLabel";
            this.xSizeLabel.Size = new System.Drawing.Size(22, 27);
            this.xSizeLabel.TabIndex = 27;
            this.xSizeLabel.Text = "x";
            // 
            // sizeY
            // 
            this.sizeY.DecimalPlaces = 1;
            this.sizeY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.sizeY.Location = new System.Drawing.Point(115, 164);
            this.sizeY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sizeY.Name = "sizeY";
            this.sizeY.Size = new System.Drawing.Size(100, 34);
            this.sizeY.TabIndex = 26;
            this.sizeY.ValueChanged += new System.EventHandler(this.size_ValueChanged_Y);
            // 
            // heightSize
            // 
            this.heightSize.DecimalPlaces = 1;
            this.heightSize.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.heightSize.Location = new System.Drawing.Point(55, 58);
            this.heightSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.heightSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.heightSize.Name = "heightSize";
            this.heightSize.Size = new System.Drawing.Size(100, 34);
            this.heightSize.TabIndex = 27;
            this.heightSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.heightSize.ValueChanged += new System.EventHandler(this.Size_ValueChanged);
            // 
            // stageSizeGroup
            // 
            this.stageSizeGroup.Controls.Add(this.widthLabel);
            this.stageSizeGroup.Controls.Add(this.heightLabel);
            this.stageSizeGroup.Controls.Add(this.widthSize);
            this.stageSizeGroup.Controls.Add(this.heightSize);
            this.stageSizeGroup.Location = new System.Drawing.Point(12, 431);
            this.stageSizeGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.stageSizeGroup.Name = "stageSizeGroup";
            this.stageSizeGroup.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.stageSizeGroup.Size = new System.Drawing.Size(176, 173);
            this.stageSizeGroup.TabIndex = 28;
            this.stageSizeGroup.TabStop = false;
            this.stageSizeGroup.Text = "ステージサイズ";
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Location = new System.Drawing.Point(17, 111);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(30, 27);
            this.widthLabel.TabIndex = 30;
            this.widthLabel.Text = "横";
            // 
            // heightLabel
            // 
            this.heightLabel.AutoSize = true;
            this.heightLabel.Location = new System.Drawing.Point(19, 60);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(30, 27);
            this.heightLabel.TabIndex = 29;
            this.heightLabel.Text = "縦";
            // 
            // widthSize
            // 
            this.widthSize.DecimalPlaces = 1;
            this.widthSize.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.widthSize.Location = new System.Drawing.Point(55, 111);
            this.widthSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.widthSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.widthSize.Name = "widthSize";
            this.widthSize.Size = new System.Drawing.Size(100, 34);
            this.widthSize.TabIndex = 28;
            this.widthSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.widthSize.ValueChanged += new System.EventHandler(this.Size_ValueChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(10, 4, 0, 4);
            this.menuStrip1.Size = new System.Drawing.Size(1200, 37);
            this.menuStrip1.TabIndex = 30;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルFToolStripMenuItem
            // 
            this.ファイルFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateNewFileToolStripMenuItem,
            this.OpenFileToolStripMenuItem,
            this.OverWriteToolStripMenuItem,
            this.SaveFileToolStripMenuItem,
            this.toolStripMenuItem1,
            this.ExitAppToolStripMenuItem});
            this.ファイルFToolStripMenuItem.Name = "ファイルFToolStripMenuItem";
            this.ファイルFToolStripMenuItem.Size = new System.Drawing.Size(98, 29);
            this.ファイルFToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // CreateNewFileToolStripMenuItem
            // 
            this.CreateNewFileToolStripMenuItem.Name = "CreateNewFileToolStripMenuItem";
            this.CreateNewFileToolStripMenuItem.Size = new System.Drawing.Size(279, 34);
            this.CreateNewFileToolStripMenuItem.Text = "新規作成(&N)...";
            this.CreateNewFileToolStripMenuItem.Click += new System.EventHandler(this.CreateNewFileToolStripMenuItem_Click);
            // 
            // OpenFileToolStripMenuItem
            // 
            this.OpenFileToolStripMenuItem.Name = "OpenFileToolStripMenuItem";
            this.OpenFileToolStripMenuItem.Size = new System.Drawing.Size(279, 34);
            this.OpenFileToolStripMenuItem.Text = "開く(&O)...";
            this.OpenFileToolStripMenuItem.Click += new System.EventHandler(this.OpenFileToolStripMenuItem_Click);
            // 
            // OverWriteToolStripMenuItem
            // 
            this.OverWriteToolStripMenuItem.Name = "OverWriteToolStripMenuItem";
            this.OverWriteToolStripMenuItem.Size = new System.Drawing.Size(279, 34);
            this.OverWriteToolStripMenuItem.Text = "上書き保存(&S)";
            this.OverWriteToolStripMenuItem.Click += new System.EventHandler(this.OverWriteToolStripMenuItem_Click);
            // 
            // SaveFileToolStripMenuItem
            // 
            this.SaveFileToolStripMenuItem.Name = "SaveFileToolStripMenuItem";
            this.SaveFileToolStripMenuItem.Size = new System.Drawing.Size(279, 34);
            this.SaveFileToolStripMenuItem.Text = "名前を付けて保存(&A)...";
            this.SaveFileToolStripMenuItem.Click += new System.EventHandler(this.SaveFileToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(276, 6);
            // 
            // ExitAppToolStripMenuItem
            // 
            this.ExitAppToolStripMenuItem.Name = "ExitAppToolStripMenuItem";
            this.ExitAppToolStripMenuItem.Size = new System.Drawing.Size(279, 34);
            this.ExitAppToolStripMenuItem.Text = "終了(&X)";
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(1082, 626);
            this.deleteButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(88, 30);
            this.deleteButton.TabIndex = 31;
            this.deleteButton.Text = "削除";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(950, 626);
            this.addButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(92, 30);
            this.addButton.TabIndex = 32;
            this.addButton.Text = "追加";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // AllPartsLabel
            // 
            this.AllPartsLabel.AutoSize = true;
            this.AllPartsLabel.Location = new System.Drawing.Point(947, 62);
            this.AllPartsLabel.Name = "AllPartsLabel";
            this.AllPartsLabel.Size = new System.Drawing.Size(138, 27);
            this.AllPartsLabel.TabIndex = 33;
            this.AllPartsLabel.Text = "すべてのパーツ";
            // 
            // msgText
            // 
            this.msgText.AutoSize = true;
            this.msgText.ForeColor = System.Drawing.SystemColors.ControlText;
            this.msgText.Location = new System.Drawing.Point(9, 658);
            this.msgText.Name = "msgText";
            this.msgText.Size = new System.Drawing.Size(46, 27);
            this.msgText.TabIndex = 34;
            this.msgText.Text = "text";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // InStagePicture
            // 
            this.InStagePicture.BackColor = System.Drawing.Color.White;
            this.InStagePicture.Location = new System.Drawing.Point(207, 217);
            this.InStagePicture.Name = "InStagePicture";
            this.InStagePicture.Size = new System.Drawing.Size(700, 280);
            this.InStagePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.InStagePicture.TabIndex = 35;
            this.InStagePicture.TabStop = false;
            this.InStagePicture.Click += new System.EventHandler(this.InStagePicture_Click);
            this.InStagePicture.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.InStagePicture_MouseWheel);
            // 
            // stageBox
            // 
            this.stageBox.BackColor = System.Drawing.SystemColors.GrayText;
            this.stageBox.Location = new System.Drawing.Point(194, 56);
            this.stageBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.stageBox.Name = "stageBox";
            this.stageBox.Size = new System.Drawing.Size(725, 600);
            this.stageBox.TabIndex = 15;
            this.stageBox.TabStop = false;
            // 
            // playerViewCheckBox
            // 
            this.playerViewCheckBox.AutoSize = true;
            this.playerViewCheckBox.Location = new System.Drawing.Point(12, 614);
            this.playerViewCheckBox.Name = "playerViewCheckBox";
            this.playerViewCheckBox.Size = new System.Drawing.Size(131, 31);
            this.playerViewCheckBox.TabIndex = 36;
            this.playerViewCheckBox.Text = "checkBox1";
            this.playerViewCheckBox.UseVisualStyleBackColor = true;
            this.playerViewCheckBox.CheckedChanged += new System.EventHandler(this.playerViewCheckBox_CheckedChanged);
            // 
            // bottomLeftLabel
            // 
            this.bottomLeftLabel.AutoSize = true;
            this.bottomLeftLabel.Location = new System.Drawing.Point(207, 504);
            this.bottomLeftLabel.Name = "bottomLeftLabel";
            this.bottomLeftLabel.Size = new System.Drawing.Size(64, 27);
            this.bottomLeftLabel.TabIndex = 38;
            this.bottomLeftLabel.Text = "label1";
            // 
            // bottomRightLabel
            // 
            this.bottomRightLabel.AutoSize = true;
            this.bottomRightLabel.Location = new System.Drawing.Point(843, 504);
            this.bottomRightLabel.Name = "bottomRightLabel";
            this.bottomRightLabel.Size = new System.Drawing.Size(64, 27);
            this.bottomRightLabel.TabIndex = 39;
            this.bottomRightLabel.Text = "label1";
            // 
            // topRightLabel
            // 
            this.topRightLabel.AutoSize = true;
            this.topRightLabel.Location = new System.Drawing.Point(843, 187);
            this.topRightLabel.Name = "topRightLabel";
            this.topRightLabel.Size = new System.Drawing.Size(64, 27);
            this.topRightLabel.TabIndex = 40;
            this.topRightLabel.Text = "label2";
            // 
            // topleftLabel
            // 
            this.topleftLabel.AutoSize = true;
            this.topleftLabel.Location = new System.Drawing.Point(207, 187);
            this.topleftLabel.Name = "topleftLabel";
            this.topleftLabel.Size = new System.Drawing.Size(64, 27);
            this.topleftLabel.TabIndex = 41;
            this.topleftLabel.Text = "label3";
            // 
            // addModeButtom
            // 
            this.addModeButtom.AutoSize = true;
            this.addModeButtom.Location = new System.Drawing.Point(10, 33);
            this.addModeButtom.Name = "addModeButtom";
            this.addModeButtom.Size = new System.Drawing.Size(73, 31);
            this.addModeButtom.TabIndex = 42;
            this.addModeButtom.TabStop = true;
            this.addModeButtom.Text = "追加";
            this.addModeButtom.UseVisualStyleBackColor = true;
            // 
            // viewMoveModeButton
            // 
            this.viewMoveModeButton.AutoSize = true;
            this.viewMoveModeButton.Location = new System.Drawing.Point(10, 66);
            this.viewMoveModeButton.Name = "viewMoveModeButton";
            this.viewMoveModeButton.Size = new System.Drawing.Size(109, 31);
            this.viewMoveModeButton.TabIndex = 43;
            this.viewMoveModeButton.Text = "視点移動";
            this.viewMoveModeButton.UseVisualStyleBackColor = true;
            // 
            // mode
            // 
            this.mode.Controls.Add(this.viewMoveModeButton);
            this.mode.Controls.Add(this.addModeButtom);
            this.mode.Location = new System.Drawing.Point(12, 56);
            this.mode.Name = "mode";
            this.mode.Size = new System.Drawing.Size(155, 103);
            this.mode.TabIndex = 44;
            this.mode.TabStop = false;
            this.mode.Text = "モード";
            // 
            // CreateStageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 760);
            this.Controls.Add(this.mode);
            this.Controls.Add(this.topleftLabel);
            this.Controls.Add(this.topRightLabel);
            this.Controls.Add(this.bottomRightLabel);
            this.Controls.Add(this.bottomLeftLabel);
            this.Controls.Add(this.playerViewCheckBox);
            this.Controls.Add(this.InStagePicture);
            this.Controls.Add(this.msgText);
            this.Controls.Add(this.AllPartsLabel);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.stageSizeGroup);
            this.Controls.Add(this.partDataGroup);
            this.Controls.Add(this.partsListBox);
            this.Controls.Add(this.stageBox);
            this.Controls.Add(this.stagePartsGroupBox);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "CreateStageForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.CreateStageForm_Load);
            this.stagePartsGroupBox.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeX)).EndInit();
            this.polePanel.ResumeLayout(false);
            this.polePanel.PerformLayout();
            this.partDataGroup.ResumeLayout(false);
            this.partDataGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightSize)).EndInit();
            this.stageSizeGroup.ResumeLayout(false);
            this.stageSizeGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.widthSize)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InStagePicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stageBox)).EndInit();
            this.mode.ResumeLayout(false);
            this.mode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox stagePartsGroupBox;
        private System.Windows.Forms.RadioButton itemBoxButton;
        private System.Windows.Forms.RadioButton NormalWallButton;
        private System.Windows.Forms.RadioButton wallButton;
        private System.Windows.Forms.RadioButton jumpRampButton;
        private System.Windows.Forms.RadioButton scaffoldButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.PictureBox stageBox;
        private System.Windows.Forms.ListBox partsListBox;
        private System.Windows.Forms.NumericUpDown positionX;
        private System.Windows.Forms.NumericUpDown positionY;
        private System.Windows.Forms.NumericUpDown sizeX;
        private System.Windows.Forms.Panel polePanel;
        private System.Windows.Forms.RadioButton northButton;
        private System.Windows.Forms.RadioButton southButton;
        private System.Windows.Forms.Label positionLabel;
        private System.Windows.Forms.Label xPosLabel;
        private System.Windows.Forms.Label yPosLabel;
        private System.Windows.Forms.Label sizeLabel;
        private System.Windows.Forms.Label poleLabel;
        private System.Windows.Forms.GroupBox partDataGroup;
        private System.Windows.Forms.NumericUpDown heightSize;
        private System.Windows.Forms.GroupBox stageSizeGroup;
        private System.Windows.Forms.Label widthLabel;
        private System.Windows.Forms.Label heightLabel;
        private System.Windows.Forms.NumericUpDown widthSize;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CreateNewFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ExitAppToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OverWriteToolStripMenuItem;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Label AllPartsLabel;
        private System.Windows.Forms.Label msgText;
        private System.Windows.Forms.Label ySizeLabel;
        private System.Windows.Forms.Label xSizeLabel;
        private System.Windows.Forms.NumericUpDown sizeY;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.PictureBox InStagePicture;
        private System.Windows.Forms.CheckBox playerViewCheckBox;
        private System.Windows.Forms.Label bottomLeftLabel;
        private System.Windows.Forms.Label bottomRightLabel;
        private System.Windows.Forms.Label topRightLabel;
        private System.Windows.Forms.Label topleftLabel;
        private System.Windows.Forms.RadioButton addModeButtom;
        private System.Windows.Forms.RadioButton viewMoveModeButton;
        private System.Windows.Forms.GroupBox mode;
    }
}

