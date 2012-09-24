namespace TDPPM
{
    partial class SheetEditPanel
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SheetEditPanel));
            this.imgSheetTree = new System.Windows.Forms.ImageList(this.components);
            this.cmsModel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsCreateSheet = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSheet = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsOpenSheet = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDelSheet = new System.Windows.Forms.ToolStripMenuItem();
            this.splMain = new System.Windows.Forms.SplitContainer();
            this.tvwSheet = new System.Windows.Forms.TreeView();
            this.tsrSheetTree = new System.Windows.Forms.ToolStrip();
            this.tsrbtnAdd = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnDel = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnOpen = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnRefresh = new System.Windows.Forms.ToolStripButton();
            this.tabSheetTool = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.CharEdit = new TDPPM.ChartEditPanel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.LabelEdit = new TDPPM.LabelEditPanel();
            this.cmsModel.SuspendLayout();
            this.cmsSheet.SuspendLayout();
            this.splMain.Panel1.SuspendLayout();
            this.splMain.Panel2.SuspendLayout();
            this.splMain.SuspendLayout();
            this.tsrSheetTree.SuspendLayout();
            this.tabSheetTool.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgSheetTree
            // 
            this.imgSheetTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgSheetTree.ImageStream")));
            this.imgSheetTree.TransparentColor = System.Drawing.Color.Magenta;
            this.imgSheetTree.Images.SetKeyName(0, "modelExist.png");
            this.imgSheetTree.Images.SetKeyName(1, "modelNone.png");
            this.imgSheetTree.Images.SetKeyName(2, "sheet.png");
            this.imgSheetTree.Images.SetKeyName(3, "empty.png");
            // 
            // cmsModel
            // 
            this.cmsModel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsCreateSheet});
            this.cmsModel.Name = "cmsModel";
            this.cmsModel.Size = new System.Drawing.Size(131, 26);
            // 
            // cmsCreateSheet
            // 
            this.cmsCreateSheet.Name = "cmsCreateSheet";
            this.cmsCreateSheet.Size = new System.Drawing.Size(130, 22);
            this.cmsCreateSheet.Text = "新建图纸页";
            this.cmsCreateSheet.Click += new System.EventHandler(this.cmsCreateSheet_Click);
            // 
            // cmsSheet
            // 
            this.cmsSheet.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsOpenSheet,
            this.cmsDelSheet});
            this.cmsSheet.Name = "cmsModel";
            this.cmsSheet.Size = new System.Drawing.Size(131, 48);
            // 
            // cmsOpenSheet
            // 
            this.cmsOpenSheet.Name = "cmsOpenSheet";
            this.cmsOpenSheet.Size = new System.Drawing.Size(130, 22);
            this.cmsOpenSheet.Text = "打开图纸页";
            this.cmsOpenSheet.Click += new System.EventHandler(this.cmsOpenSheet_Click);
            // 
            // cmsDelSheet
            // 
            this.cmsDelSheet.Name = "cmsDelSheet";
            this.cmsDelSheet.Size = new System.Drawing.Size(130, 22);
            this.cmsDelSheet.Text = "删除图纸页";
            this.cmsDelSheet.Click += new System.EventHandler(this.cmsDelSheet_Click);
            // 
            // splMain
            // 
            this.splMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splMain.Location = new System.Drawing.Point(0, 0);
            this.splMain.Name = "splMain";
            // 
            // splMain.Panel1
            // 
            this.splMain.Panel1.Controls.Add(this.tvwSheet);
            this.splMain.Panel1.Controls.Add(this.tsrSheetTree);
            // 
            // splMain.Panel2
            // 
            this.splMain.Panel2.Controls.Add(this.tabSheetTool);
            this.splMain.Size = new System.Drawing.Size(698, 420);
            this.splMain.SplitterDistance = 205;
            this.splMain.TabIndex = 0;
            // 
            // tvwSheet
            // 
            this.tvwSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwSheet.HideSelection = false;
            this.tvwSheet.ImageIndex = 0;
            this.tvwSheet.ImageList = this.imgSheetTree;
            this.tvwSheet.Location = new System.Drawing.Point(0, 25);
            this.tvwSheet.Name = "tvwSheet";
            this.tvwSheet.SelectedImageIndex = 0;
            this.tvwSheet.Size = new System.Drawing.Size(205, 395);
            this.tvwSheet.TabIndex = 1;
            this.tvwSheet.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwSheet_AfterSelect);
            this.tvwSheet.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tvwSheet_MouseClick);
            this.tvwSheet.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tvwSheet_MouseDoubleClick);
            // 
            // tsrSheetTree
            // 
            this.tsrSheetTree.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsrSheetTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsrbtnAdd,
            this.tsrbtnDel,
            this.tsrbtnOpen,
            this.tsrbtnRefresh});
            this.tsrSheetTree.Location = new System.Drawing.Point(0, 0);
            this.tsrSheetTree.Name = "tsrSheetTree";
            this.tsrSheetTree.Size = new System.Drawing.Size(205, 25);
            this.tsrSheetTree.TabIndex = 0;
            this.tsrSheetTree.Text = "toolStrip1";
            // 
            // tsrbtnAdd
            // 
            this.tsrbtnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsrbtnAdd.Image = global::TDPPM.Properties.Resources.addProcedure;
            this.tsrbtnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnAdd.Name = "tsrbtnAdd";
            this.tsrbtnAdd.Size = new System.Drawing.Size(23, 22);
            this.tsrbtnAdd.Text = "新建图纸页";
            this.tsrbtnAdd.Click += new System.EventHandler(this.tsrbtnAdd_Click);
            // 
            // tsrbtnDel
            // 
            this.tsrbtnDel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsrbtnDel.Image = global::TDPPM.Properties.Resources.deleteProcedure;
            this.tsrbtnDel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnDel.Name = "tsrbtnDel";
            this.tsrbtnDel.Size = new System.Drawing.Size(23, 22);
            this.tsrbtnDel.Text = "删除图纸页";
            this.tsrbtnDel.Click += new System.EventHandler(this.tsrbtnDel_Click);
            // 
            // tsrbtnOpen
            // 
            this.tsrbtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsrbtnOpen.Image = global::TDPPM.Properties.Resources.open;
            this.tsrbtnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnOpen.Name = "tsrbtnOpen";
            this.tsrbtnOpen.Size = new System.Drawing.Size(23, 22);
            this.tsrbtnOpen.Text = "打开图纸页";
            this.tsrbtnOpen.Click += new System.EventHandler(this.tsrbtnOpen_Click);
            // 
            // tsrbtnRefresh
            // 
            this.tsrbtnRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsrbtnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsrbtnRefresh.Image = global::TDPPM.Properties.Resources.refresh;
            this.tsrbtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnRefresh.Name = "tsrbtnRefresh";
            this.tsrbtnRefresh.Size = new System.Drawing.Size(23, 22);
            this.tsrbtnRefresh.Text = "刷新";
            this.tsrbtnRefresh.Click += new System.EventHandler(this.tsrbtnRefresh_Click);
            // 
            // tabSheetTool
            // 
            this.tabSheetTool.Controls.Add(this.tabPage1);
            this.tabSheetTool.Controls.Add(this.tabPage2);
            this.tabSheetTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSheetTool.Location = new System.Drawing.Point(0, 0);
            this.tabSheetTool.Name = "tabSheetTool";
            this.tabSheetTool.SelectedIndex = 0;
            this.tabSheetTool.Size = new System.Drawing.Size(489, 420);
            this.tabSheetTool.TabIndex = 0;
            this.tabSheetTool.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabSheetTool_Selecting);
            this.tabSheetTool.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tabSheetTool_MouseDoubleClick);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.CharEdit);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(481, 394);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "二维图表";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // CharEdit
            // 
            this.CharEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CharEdit.Location = new System.Drawing.Point(3, 3);
            this.CharEdit.Name = "CharEdit";
            this.CharEdit.Size = new System.Drawing.Size(475, 388);
            this.CharEdit.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.LabelEdit);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(481, 394);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "打标号";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // LabelEdit
            // 
            this.LabelEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabelEdit.Location = new System.Drawing.Point(3, 3);
            this.LabelEdit.Name = "LabelEdit";
            this.LabelEdit.Size = new System.Drawing.Size(475, 388);
            this.LabelEdit.TabIndex = 0;
            // 
            // SheetEditPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splMain);
            this.Name = "SheetEditPanel";
            this.Size = new System.Drawing.Size(698, 420);
            this.cmsModel.ResumeLayout(false);
            this.cmsSheet.ResumeLayout(false);
            this.splMain.Panel1.ResumeLayout(false);
            this.splMain.Panel1.PerformLayout();
            this.splMain.Panel2.ResumeLayout(false);
            this.splMain.ResumeLayout(false);
            this.tsrSheetTree.ResumeLayout(false);
            this.tsrSheetTree.PerformLayout();
            this.tabSheetTool.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splMain;
        private System.Windows.Forms.ToolStrip tsrSheetTree;
        private System.Windows.Forms.ToolStripButton tsrbtnAdd;
        private System.Windows.Forms.ToolStripButton tsrbtnDel;
        private System.Windows.Forms.ToolStripButton tsrbtnOpen;
        private System.Windows.Forms.TreeView tvwSheet;
        private System.Windows.Forms.TabControl tabSheetTool;
        private System.Windows.Forms.TabPage tabPage1;
        private ChartEditPanel CharEdit;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ImageList imgSheetTree;
        private System.Windows.Forms.ToolStripButton tsrbtnRefresh;
        private System.Windows.Forms.ContextMenuStrip cmsModel;
        private System.Windows.Forms.ContextMenuStrip cmsSheet;
        private System.Windows.Forms.ToolStripMenuItem cmsCreateSheet;
        private System.Windows.Forms.ToolStripMenuItem cmsDelSheet;
        private System.Windows.Forms.ToolStripMenuItem cmsOpenSheet;
        public LabelEditPanel LabelEdit;
    }
}
