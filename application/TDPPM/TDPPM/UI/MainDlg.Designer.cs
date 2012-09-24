namespace TDPPM
{
    partial class MainDlg
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDlg));
            this.mnsMain = new System.Windows.Forms.MenuStrip();
            this.tsmFileRoot = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmNew = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmToolRoot = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmRecover = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmChangeEnv = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmModeling = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmDrafting = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmHelpRoot = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.strMain = new System.Windows.Forms.ToolStrip();
            this.tsrbtnNew = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnOpen = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnSave = new System.Windows.Forms.ToolStripButton();
            this.tssSep = new System.Windows.Forms.ToolStripSeparator();
            this.tsrbtnModeling = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnDrafting = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnHelp = new System.Windows.Forms.ToolStripDropDownButton();
            this.用户使用说明书ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsrbtnTools = new System.Windows.Forms.ToolStripDropDownButton();
            this.恢复工程文件夹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开所有模型ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.环境切换ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.建模ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.制图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.定制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsrFile = new System.Windows.Forms.ToolStripDropDownButton();
            this.新建ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ssrMain = new System.Windows.Forms.StatusStrip();
            this.tslMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.pnlWelcome = new System.Windows.Forms.Panel();
            this.picWelcome = new System.Windows.Forms.PictureBox();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tsmJiagongmian = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmGongcha = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmHuizhuan = new System.Windows.Forms.ToolStripMenuItem();
            this.ProcessEdit = new TDPPM.ProcessEditPanel();
            this.ModelEdit = new TDPPM.ModelEditPanel();
            this.SheetEdit = new TDPPM.SheetEditPanel();
            this.tsmCAPP = new System.Windows.Forms.ToolStripMenuItem();
            this.mnsMain.SuspendLayout();
            this.strMain.SuspendLayout();
            this.ssrMain.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.pnlWelcome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWelcome)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnsMain
            // 
            this.mnsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmFileRoot,
            this.tsmToolRoot,
            this.tsmHelpRoot});
            this.mnsMain.Location = new System.Drawing.Point(0, 0);
            this.mnsMain.Name = "mnsMain";
            this.mnsMain.Size = new System.Drawing.Size(655, 24);
            this.mnsMain.TabIndex = 0;
            this.mnsMain.Text = "menuStrip1";
            // 
            // tsmFileRoot
            // 
            this.tsmFileRoot.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmNew,
            this.tsmOpen,
            this.tsmSave,
            this.tsmExit});
            this.tsmFileRoot.Name = "tsmFileRoot";
            this.tsmFileRoot.Size = new System.Drawing.Size(41, 20);
            this.tsmFileRoot.Text = "文件";
            // 
            // tsmNew
            // 
            this.tsmNew.Name = "tsmNew";
            this.tsmNew.Size = new System.Drawing.Size(94, 22);
            this.tsmNew.Text = "新建";
            this.tsmNew.Click += new System.EventHandler(this.tsmNew_Click);
            // 
            // tsmOpen
            // 
            this.tsmOpen.Name = "tsmOpen";
            this.tsmOpen.Size = new System.Drawing.Size(94, 22);
            this.tsmOpen.Text = "打开";
            this.tsmOpen.Click += new System.EventHandler(this.tsmOpen_Click);
            // 
            // tsmSave
            // 
            this.tsmSave.Name = "tsmSave";
            this.tsmSave.Size = new System.Drawing.Size(94, 22);
            this.tsmSave.Text = "保存";
            this.tsmSave.Click += new System.EventHandler(this.tsmSave_Click);
            // 
            // tsmExit
            // 
            this.tsmExit.Name = "tsmExit";
            this.tsmExit.Size = new System.Drawing.Size(94, 22);
            this.tsmExit.Text = "退出";
            this.tsmExit.Click += new System.EventHandler(this.tsmExit_Click);
            // 
            // tsmToolRoot
            // 
            this.tsmToolRoot.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmRecover,
            this.tsmChangeEnv,
            this.tsmJiagongmian,
            this.tsmGongcha,
            this.tsmHuizhuan,
            this.tsmCAPP});
            this.tsmToolRoot.Name = "tsmToolRoot";
            this.tsmToolRoot.Size = new System.Drawing.Size(41, 20);
            this.tsmToolRoot.Text = "工具";
            // 
            // tsmRecover
            // 
            this.tsmRecover.Name = "tsmRecover";
            this.tsmRecover.Size = new System.Drawing.Size(154, 22);
            this.tsmRecover.Text = "恢复工艺文件夹";
            this.tsmRecover.Click += new System.EventHandler(this.tsmRecover_Click);
            // 
            // tsmChangeEnv
            // 
            this.tsmChangeEnv.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmModeling,
            this.tsmDrafting});
            this.tsmChangeEnv.Name = "tsmChangeEnv";
            this.tsmChangeEnv.Size = new System.Drawing.Size(154, 22);
            this.tsmChangeEnv.Text = "环境切换";
            this.tsmChangeEnv.Click += new System.EventHandler(this.tsmChangeEnv_Click);
            // 
            // tsmModeling
            // 
            this.tsmModeling.Name = "tsmModeling";
            this.tsmModeling.Size = new System.Drawing.Size(118, 22);
            this.tsmModeling.Text = "建模环境";
            this.tsmModeling.Click += new System.EventHandler(this.tsmModeling_Click);
            // 
            // tsmDrafting
            // 
            this.tsmDrafting.Name = "tsmDrafting";
            this.tsmDrafting.Size = new System.Drawing.Size(118, 22);
            this.tsmDrafting.Text = "制图环境";
            this.tsmDrafting.Click += new System.EventHandler(this.tsmDrafting_Click);
            // 
            // tsmHelpRoot
            // 
            this.tsmHelpRoot.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmHelp,
            this.tsmAbout});
            this.tsmHelpRoot.Name = "tsmHelpRoot";
            this.tsmHelpRoot.Size = new System.Drawing.Size(41, 20);
            this.tsmHelpRoot.Text = "帮助";
            // 
            // tsmHelp
            // 
            this.tsmHelp.Name = "tsmHelp";
            this.tsmHelp.Size = new System.Drawing.Size(94, 22);
            this.tsmHelp.Text = "帮助";
            this.tsmHelp.Click += new System.EventHandler(this.tsmHelp_Click);
            // 
            // tsmAbout
            // 
            this.tsmAbout.Name = "tsmAbout";
            this.tsmAbout.Size = new System.Drawing.Size(94, 22);
            this.tsmAbout.Text = "关于";
            this.tsmAbout.Click += new System.EventHandler(this.tsmAbout_Click);
            // 
            // strMain
            // 
            this.strMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.strMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsrbtnNew,
            this.tsrbtnOpen,
            this.tsrbtnSave,
            this.tssSep,
            this.tsrbtnModeling,
            this.tsrbtnDrafting,
            this.tsrbtnHelp,
            this.tsrbtnTools,
            this.tsrFile});
            this.strMain.Location = new System.Drawing.Point(0, 24);
            this.strMain.Name = "strMain";
            this.strMain.Size = new System.Drawing.Size(655, 25);
            this.strMain.TabIndex = 1;
            this.strMain.Text = "toolStrip1";
            // 
            // tsrbtnNew
            // 
            this.tsrbtnNew.Image = global::TDPPM.Properties.Resources.create;
            this.tsrbtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnNew.Name = "tsrbtnNew";
            this.tsrbtnNew.Size = new System.Drawing.Size(49, 22);
            this.tsrbtnNew.Text = "新建";
            this.tsrbtnNew.Click += new System.EventHandler(this.tsmNew_Click);
            // 
            // tsrbtnOpen
            // 
            this.tsrbtnOpen.Image = global::TDPPM.Properties.Resources.open;
            this.tsrbtnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnOpen.Name = "tsrbtnOpen";
            this.tsrbtnOpen.Size = new System.Drawing.Size(49, 22);
            this.tsrbtnOpen.Text = "打开";
            this.tsrbtnOpen.Click += new System.EventHandler(this.tsmOpen_Click);
            // 
            // tsrbtnSave
            // 
            this.tsrbtnSave.Image = global::TDPPM.Properties.Resources.save;
            this.tsrbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnSave.Name = "tsrbtnSave";
            this.tsrbtnSave.Size = new System.Drawing.Size(49, 22);
            this.tsrbtnSave.Text = "保存";
            this.tsrbtnSave.Click += new System.EventHandler(this.tsmSave_Click);
            // 
            // tssSep
            // 
            this.tssSep.Name = "tssSep";
            this.tssSep.Size = new System.Drawing.Size(6, 25);
            // 
            // tsrbtnModeling
            // 
            this.tsrbtnModeling.Image = global::TDPPM.Properties.Resources.modeling;
            this.tsrbtnModeling.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnModeling.Name = "tsrbtnModeling";
            this.tsrbtnModeling.Size = new System.Drawing.Size(49, 22);
            this.tsrbtnModeling.Text = "建模";
            this.tsrbtnModeling.Click += new System.EventHandler(this.tsmModeling_Click);
            // 
            // tsrbtnDrafting
            // 
            this.tsrbtnDrafting.Image = global::TDPPM.Properties.Resources.drafting;
            this.tsrbtnDrafting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnDrafting.Name = "tsrbtnDrafting";
            this.tsrbtnDrafting.Size = new System.Drawing.Size(49, 22);
            this.tsrbtnDrafting.Text = "制图";
            this.tsrbtnDrafting.Click += new System.EventHandler(this.tsmDrafting_Click);
            // 
            // tsrbtnHelp
            // 
            this.tsrbtnHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsrbtnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsrbtnHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.用户使用说明书ToolStripMenuItem,
            this.关于ToolStripMenuItem});
            this.tsrbtnHelp.Image = ((System.Drawing.Image)(resources.GetObject("tsrbtnHelp.Image")));
            this.tsrbtnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnHelp.Name = "tsrbtnHelp";
            this.tsrbtnHelp.Size = new System.Drawing.Size(42, 22);
            this.tsrbtnHelp.Text = "帮助";
            this.tsrbtnHelp.Visible = false;
            // 
            // 用户使用说明书ToolStripMenuItem
            // 
            this.用户使用说明书ToolStripMenuItem.Name = "用户使用说明书ToolStripMenuItem";
            this.用户使用说明书ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.用户使用说明书ToolStripMenuItem.Text = "用户使用说明书";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.关于ToolStripMenuItem.Text = "关于";
            // 
            // tsrbtnTools
            // 
            this.tsrbtnTools.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsrbtnTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsrbtnTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.恢复工程文件夹ToolStripMenuItem,
            this.打开所有模型ToolStripMenuItem,
            this.环境切换ToolStripMenuItem,
            this.定制ToolStripMenuItem});
            this.tsrbtnTools.Image = ((System.Drawing.Image)(resources.GetObject("tsrbtnTools.Image")));
            this.tsrbtnTools.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnTools.Name = "tsrbtnTools";
            this.tsrbtnTools.Size = new System.Drawing.Size(42, 22);
            this.tsrbtnTools.Text = "工具";
            this.tsrbtnTools.Visible = false;
            // 
            // 恢复工程文件夹ToolStripMenuItem
            // 
            this.恢复工程文件夹ToolStripMenuItem.Name = "恢复工程文件夹ToolStripMenuItem";
            this.恢复工程文件夹ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.恢复工程文件夹ToolStripMenuItem.Text = "恢复工程文件夹";
            // 
            // 打开所有模型ToolStripMenuItem
            // 
            this.打开所有模型ToolStripMenuItem.Name = "打开所有模型ToolStripMenuItem";
            this.打开所有模型ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.打开所有模型ToolStripMenuItem.Text = "打开所有模型";
            // 
            // 环境切换ToolStripMenuItem
            // 
            this.环境切换ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.建模ToolStripMenuItem,
            this.制图ToolStripMenuItem});
            this.环境切换ToolStripMenuItem.Name = "环境切换ToolStripMenuItem";
            this.环境切换ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.环境切换ToolStripMenuItem.Text = "环境切换";
            // 
            // 建模ToolStripMenuItem
            // 
            this.建模ToolStripMenuItem.Name = "建模ToolStripMenuItem";
            this.建模ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.建模ToolStripMenuItem.Text = "建模";
            // 
            // 制图ToolStripMenuItem
            // 
            this.制图ToolStripMenuItem.Name = "制图ToolStripMenuItem";
            this.制图ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.制图ToolStripMenuItem.Text = "制图";
            // 
            // 定制ToolStripMenuItem
            // 
            this.定制ToolStripMenuItem.Name = "定制ToolStripMenuItem";
            this.定制ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.定制ToolStripMenuItem.Text = "定制";
            // 
            // tsrFile
            // 
            this.tsrFile.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsrFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsrFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建ToolStripMenuItem,
            this.打开ToolStripMenuItem,
            this.保存ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.tsrFile.Image = ((System.Drawing.Image)(resources.GetObject("tsrFile.Image")));
            this.tsrFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrFile.Name = "tsrFile";
            this.tsrFile.Size = new System.Drawing.Size(42, 22);
            this.tsrFile.Text = "文件";
            this.tsrFile.Visible = false;
            // 
            // 新建ToolStripMenuItem
            // 
            this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
            this.新建ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.新建ToolStripMenuItem.Text = "新建";
            // 
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.打开ToolStripMenuItem.Text = "打开";
            // 
            // 保存ToolStripMenuItem
            // 
            this.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            this.保存ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.保存ToolStripMenuItem.Text = "保存";
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            // 
            // ssrMain
            // 
            this.ssrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslMessage});
            this.ssrMain.Location = new System.Drawing.Point(0, 409);
            this.ssrMain.Name = "ssrMain";
            this.ssrMain.Size = new System.Drawing.Size(655, 22);
            this.ssrMain.TabIndex = 2;
            this.ssrMain.Text = "statusStrip1";
            // 
            // tslMessage
            // 
            this.tslMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tslMessage.Name = "tslMessage";
            this.tslMessage.Size = new System.Drawing.Size(640, 17);
            this.tslMessage.Spring = true;
            this.tslMessage.Text = "欢迎使用三维机加工艺设计系统";
            this.tslMessage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ModelEdit);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(647, 334);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "编辑模型";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabMain
            // 
            this.tabMain.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabMain.Controls.Add(this.tabPage10);
            this.tabMain.Controls.Add(this.tabPage2);
            this.tabMain.Controls.Add(this.tabPage9);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Multiline = true;
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(655, 360);
            this.tabMain.TabIndex = 1;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
            this.tabMain.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabMain_Selecting);
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.ProcessEdit);
            this.tabPage10.Location = new System.Drawing.Point(4, 4);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(647, 334);
            this.tabPage10.TabIndex = 3;
            this.tabPage10.Text = "编辑工艺";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.SheetEdit);
            this.tabPage9.Location = new System.Drawing.Point(4, 4);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(647, 334);
            this.tabPage9.TabIndex = 2;
            this.tabPage9.Text = "编辑图表";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // pnlWelcome
            // 
            this.pnlWelcome.Controls.Add(this.picWelcome);
            this.pnlWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWelcome.Location = new System.Drawing.Point(0, 49);
            this.pnlWelcome.Name = "pnlWelcome";
            this.pnlWelcome.Size = new System.Drawing.Size(655, 382);
            this.pnlWelcome.TabIndex = 4;
            // 
            // picWelcome
            // 
            this.picWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picWelcome.Image = global::TDPPM.Properties.Resources.welcome;
            this.picWelcome.Location = new System.Drawing.Point(0, 0);
            this.picWelcome.Name = "picWelcome";
            this.picWelcome.Size = new System.Drawing.Size(655, 382);
            this.picWelcome.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picWelcome.TabIndex = 3;
            this.picWelcome.TabStop = false;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.tabMain);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 49);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(655, 360);
            this.pnlMain.TabIndex = 5;
            // 
            // tsmJiagongmian
            // 
            this.tsmJiagongmian.Name = "tsmJiagongmian";
            this.tsmJiagongmian.Size = new System.Drawing.Size(154, 22);
            this.tsmJiagongmian.Text = "加工面标注";
            this.tsmJiagongmian.Click += new System.EventHandler(this.tsmJiagongmian_Click);
            // 
            // tsmGongcha
            // 
            this.tsmGongcha.Name = "tsmGongcha";
            this.tsmGongcha.Size = new System.Drawing.Size(154, 22);
            this.tsmGongcha.Text = "公差标注";
            this.tsmGongcha.Click += new System.EventHandler(this.tsmGongcha_Click);
            // 
            // tsmHuizhuan
            // 
            this.tsmHuizhuan.Name = "tsmHuizhuan";
            this.tsmHuizhuan.Size = new System.Drawing.Size(154, 22);
            this.tsmHuizhuan.Text = "回转裁剪工具";
            this.tsmHuizhuan.Click += new System.EventHandler(this.tsmHuizhuan_Click);
            // 
            // ProcessEdit
            // 
            this.ProcessEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProcessEdit.Location = new System.Drawing.Point(3, 3);
            this.ProcessEdit.Name = "ProcessEdit";
            this.ProcessEdit.Size = new System.Drawing.Size(641, 328);
            this.ProcessEdit.TabIndex = 0;
            // 
            // ModelEdit
            // 
            this.ModelEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ModelEdit.Location = new System.Drawing.Point(3, 3);
            this.ModelEdit.Name = "ModelEdit";
            this.ModelEdit.Size = new System.Drawing.Size(641, 328);
            this.ModelEdit.TabIndex = 0;
            // 
            // SheetEdit
            // 
            this.SheetEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SheetEdit.Location = new System.Drawing.Point(3, 3);
            this.SheetEdit.Name = "SheetEdit";
            this.SheetEdit.Size = new System.Drawing.Size(641, 328);
            this.SheetEdit.TabIndex = 0;
            // 
            // tsmCAPP
            // 
            this.tsmCAPP.Name = "tsmCAPP";
            this.tsmCAPP.Size = new System.Drawing.Size(154, 22);
            this.tsmCAPP.Text = "CAPP助手";
            this.tsmCAPP.Click += new System.EventHandler(this.tsmCAPP_Click);
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 431);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.ssrMain);
            this.Controls.Add(this.pnlWelcome);
            this.Controls.Add(this.strMain);
            this.Controls.Add(this.mnsMain);
            this.MainMenuStrip = this.mnsMain;
            this.Name = "MainDlg";
            this.Text = "3DPPM";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDlg_FormClosing);
            this.mnsMain.ResumeLayout(false);
            this.mnsMain.PerformLayout();
            this.strMain.ResumeLayout(false);
            this.strMain.PerformLayout();
            this.ssrMain.ResumeLayout(false);
            this.ssrMain.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabPage10.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.pnlWelcome.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picWelcome)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnsMain;
        private System.Windows.Forms.ToolStripMenuItem tsmFileRoot;
        private System.Windows.Forms.ToolStripMenuItem tsmOpen;
        private System.Windows.Forms.ToolStripMenuItem tsmSave;
        private System.Windows.Forms.ToolStripMenuItem tsmExit;
        private System.Windows.Forms.ToolStripMenuItem tsmToolRoot;
        private System.Windows.Forms.ToolStripMenuItem tsmHelpRoot;
        private System.Windows.Forms.ToolStripMenuItem tsmHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmAbout;
        private System.Windows.Forms.ToolStrip strMain;
        private System.Windows.Forms.ToolStripButton tsrbtnOpen;
        private System.Windows.Forms.ToolStripButton tsrbtnSave;
        private System.Windows.Forms.ToolStripButton tsrbtnNew;
        private System.Windows.Forms.StatusStrip ssrMain;
        private System.Windows.Forms.ToolStripStatusLabel tslMessage;
        private System.Windows.Forms.ToolStripSeparator tssSep;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.ToolStripMenuItem tsmNew;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem tsmRecover;
        private System.Windows.Forms.PictureBox picWelcome;
        private System.Windows.Forms.Panel pnlWelcome;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ToolStripButton tsrbtnModeling;
        private System.Windows.Forms.ToolStripButton tsrbtnDrafting;
        private System.Windows.Forms.ToolStripMenuItem tsmChangeEnv;
        private System.Windows.Forms.ToolStripMenuItem tsmModeling;
        private System.Windows.Forms.ToolStripMenuItem tsmDrafting;
        private System.Windows.Forms.TabPage tabPage9;
        SheetEditPanel SheetEdit;
        private System.Windows.Forms.TabPage tabPage10;
        ProcessEditPanel ProcessEdit;
        ModelEditPanel ModelEdit;
        private System.Windows.Forms.ToolStripDropDownButton tsrbtnTools;
        private System.Windows.Forms.ToolStripMenuItem 恢复工程文件夹ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开所有模型ToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton tsrbtnHelp;
        private System.Windows.Forms.ToolStripMenuItem 用户使用说明书ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 环境切换ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 建模ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 制图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 定制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton tsrFile;
        private System.Windows.Forms.ToolStripMenuItem 新建ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ToolStripMenuItem tsmJiagongmian;
        private System.Windows.Forms.ToolStripMenuItem tsmGongcha;
        private System.Windows.Forms.ToolStripMenuItem tsmHuizhuan;
        private System.Windows.Forms.ToolStripMenuItem tsmCAPP;
    }
}