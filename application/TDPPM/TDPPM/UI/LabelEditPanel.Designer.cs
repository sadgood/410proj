namespace TDPPM
{
    partial class LabelEditPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LabelEditPanel));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlMain = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvChicun = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvXingwei = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tsrDown = new System.Windows.Forms.ToolStrip();
            this.tsrbtnCancel = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnOk = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnSearch = new System.Windows.Forms.ToolStripButton();
            this.tsrUp = new System.Windows.Forms.ToolStrip();
            this.tsrlblChicun = new System.Windows.Forms.ToolStripLabel();
            this.tsrtxtChicun = new System.Windows.Forms.ToolStripTextBox();
            this.tsrlblXingwei = new System.Windows.Forms.ToolStripLabel();
            this.tsrtxtXingwei = new System.Windows.Forms.ToolStripTextBox();
            this.tsrbtnUp = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnDown = new System.Windows.Forms.ToolStripButton();
            this.strbtnHelp = new System.Windows.Forms.ToolStripButton();
            this.pnlMain.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChicun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvXingwei)).BeginInit();
            this.tsrDown.SuspendLayout();
            this.tsrUp.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.splitContainer1);
            this.pnlMain.Controls.Add(this.tsrDown);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 25);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(439, 331);
            this.pnlMain.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvChicun);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvXingwei);
            this.splitContainer1.Size = new System.Drawing.Size(439, 306);
            this.splitContainer1.SplitterDistance = 141;
            this.splitContainer1.TabIndex = 1;
            // 
            // dgvChicun
            // 
            this.dgvChicun.AllowUserToAddRows = false;
            this.dgvChicun.AllowUserToDeleteRows = false;
            this.dgvChicun.AllowUserToResizeRows = false;
            this.dgvChicun.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvChicun.BackgroundColor = System.Drawing.Color.White;
            this.dgvChicun.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChicun.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            this.dgvChicun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChicun.Location = new System.Drawing.Point(0, 0);
            this.dgvChicun.MultiSelect = false;
            this.dgvChicun.Name = "dgvChicun";
            this.dgvChicun.RowHeadersVisible = false;
            this.dgvChicun.RowTemplate.Height = 23;
            this.dgvChicun.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChicun.Size = new System.Drawing.Size(439, 141);
            this.dgvChicun.TabIndex = 0;
            this.dgvChicun.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChicun_CellValueChanged);
            this.dgvChicun.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvChicun_CurrentCellDirtyStateChanged);
            this.dgvChicun.SelectionChanged += new System.EventHandler(this.dgvChicun_SelectionChanged);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.FillWeight = 80F;
            this.Column1.HeaderText = "编号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 54;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.FillWeight = 200F;
            this.Column2.HeaderText = "类型";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column3.HeaderText = "尺寸";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 54;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column4.HeaderText = "上差";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 54;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column5.HeaderText = "下差";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 54;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column6.FillWeight = 50F;
            this.Column6.HeaderText = "标号";
            this.Column6.Name = "Column6";
            this.Column6.Width = 40;
            // 
            // dgvXingwei
            // 
            this.dgvXingwei.AllowUserToAddRows = false;
            this.dgvXingwei.AllowUserToDeleteRows = false;
            this.dgvXingwei.AllowUserToResizeRows = false;
            this.dgvXingwei.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvXingwei.BackgroundColor = System.Drawing.Color.White;
            this.dgvXingwei.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvXingwei.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewCheckBoxColumn1});
            this.dgvXingwei.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvXingwei.Location = new System.Drawing.Point(0, 0);
            this.dgvXingwei.MultiSelect = false;
            this.dgvXingwei.Name = "dgvXingwei";
            this.dgvXingwei.RowHeadersVisible = false;
            this.dgvXingwei.RowTemplate.Height = 23;
            this.dgvXingwei.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvXingwei.Size = new System.Drawing.Size(439, 161);
            this.dgvXingwei.TabIndex = 1;
            this.dgvXingwei.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvXingwei_CellValueChanged);
            this.dgvXingwei.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvXingwei_CurrentCellDirtyStateChanged);
            this.dgvXingwei.SelectionChanged += new System.EventHandler(this.dgvXingwei_SelectionChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.FillWeight = 80F;
            this.dataGridViewTextBoxColumn1.HeaderText = "编号";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 60;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.FillWeight = 200F;
            this.dataGridViewTextBoxColumn2.HeaderText = "类型";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn3.HeaderText = "尺寸";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 54;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewCheckBoxColumn1.FillWeight = 50F;
            this.dataGridViewCheckBoxColumn1.HeaderText = "标号";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 40;
            // 
            // tsrDown
            // 
            this.tsrDown.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tsrDown.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsrDown.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsrbtnCancel,
            this.tsrbtnOk,
            this.tsrbtnSearch});
            this.tsrDown.Location = new System.Drawing.Point(0, 306);
            this.tsrDown.Name = "tsrDown";
            this.tsrDown.Size = new System.Drawing.Size(439, 25);
            this.tsrDown.TabIndex = 0;
            this.tsrDown.Text = "toolStrip2";
            // 
            // tsrbtnCancel
            // 
            this.tsrbtnCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsrbtnCancel.Image = global::TDPPM.Properties.Resources.cancel;
            this.tsrbtnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnCancel.Name = "tsrbtnCancel";
            this.tsrbtnCancel.Size = new System.Drawing.Size(49, 22);
            this.tsrbtnCancel.Text = "取消";
            this.tsrbtnCancel.Click += new System.EventHandler(this.tsrbtnCancel_Click);
            // 
            // tsrbtnOk
            // 
            this.tsrbtnOk.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsrbtnOk.Image = global::TDPPM.Properties.Resources.ok;
            this.tsrbtnOk.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnOk.Name = "tsrbtnOk";
            this.tsrbtnOk.Size = new System.Drawing.Size(49, 22);
            this.tsrbtnOk.Text = "应用";
            this.tsrbtnOk.Click += new System.EventHandler(this.tsrbtnOk_Click);
            // 
            // tsrbtnSearch
            // 
            this.tsrbtnSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsrbtnSearch.Image = global::TDPPM.Properties.Resources.search;
            this.tsrbtnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnSearch.Name = "tsrbtnSearch";
            this.tsrbtnSearch.Size = new System.Drawing.Size(97, 22);
            this.tsrbtnSearch.Text = "查询尺寸标注";
            this.tsrbtnSearch.Click += new System.EventHandler(this.tsrbtnSearch_Click);
            // 
            // tsrUp
            // 
            this.tsrUp.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsrUp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsrlblChicun,
            this.tsrtxtChicun,
            this.tsrlblXingwei,
            this.tsrtxtXingwei,
            this.tsrbtnUp,
            this.tsrbtnDown,
            this.strbtnHelp});
            this.tsrUp.Location = new System.Drawing.Point(0, 0);
            this.tsrUp.Name = "tsrUp";
            this.tsrUp.Size = new System.Drawing.Size(439, 25);
            this.tsrUp.TabIndex = 0;
            this.tsrUp.Text = "toolStrip1";
            // 
            // tsrlblChicun
            // 
            this.tsrlblChicun.Name = "tsrlblChicun";
            this.tsrlblChicun.Size = new System.Drawing.Size(77, 22);
            this.tsrlblChicun.Text = "尺寸起始编号";
            // 
            // tsrtxtChicun
            // 
            this.tsrtxtChicun.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tsrtxtChicun.Name = "tsrtxtChicun";
            this.tsrtxtChicun.Size = new System.Drawing.Size(40, 25);
            this.tsrtxtChicun.Text = "1";
            this.tsrtxtChicun.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tsrtxtChicun.TextChanged += new System.EventHandler(this.tsrtxtChicun_TextChanged);
            // 
            // tsrlblXingwei
            // 
            this.tsrlblXingwei.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsrlblXingwei.Image = ((System.Drawing.Image)(resources.GetObject("tsrlblXingwei.Image")));
            this.tsrlblXingwei.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrlblXingwei.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.tsrlblXingwei.Name = "tsrlblXingwei";
            this.tsrlblXingwei.Size = new System.Drawing.Size(77, 22);
            this.tsrlblXingwei.Text = "形位起始编号";
            // 
            // tsrtxtXingwei
            // 
            this.tsrtxtXingwei.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tsrtxtXingwei.Name = "tsrtxtXingwei";
            this.tsrtxtXingwei.Size = new System.Drawing.Size(40, 25);
            this.tsrtxtXingwei.Text = "1";
            this.tsrtxtXingwei.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tsrtxtXingwei.TextChanged += new System.EventHandler(this.tsrtxtXingwei_TextChanged);
            // 
            // tsrbtnUp
            // 
            this.tsrbtnUp.Image = global::TDPPM.Properties.Resources.upWards;
            this.tsrbtnUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnUp.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.tsrbtnUp.Name = "tsrbtnUp";
            this.tsrbtnUp.Size = new System.Drawing.Size(49, 22);
            this.tsrbtnUp.Text = "上移";
            this.tsrbtnUp.Click += new System.EventHandler(this.tsrbtnUp_Click);
            // 
            // tsrbtnDown
            // 
            this.tsrbtnDown.Image = global::TDPPM.Properties.Resources.downWards;
            this.tsrbtnDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnDown.Name = "tsrbtnDown";
            this.tsrbtnDown.Size = new System.Drawing.Size(49, 22);
            this.tsrbtnDown.Text = "下移";
            this.tsrbtnDown.Click += new System.EventHandler(this.tsrbtnDown_Click);
            // 
            // strbtnHelp
            // 
            this.strbtnHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.strbtnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.strbtnHelp.Image = global::TDPPM.Properties.Resources.help;
            this.strbtnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.strbtnHelp.Name = "strbtnHelp";
            this.strbtnHelp.Size = new System.Drawing.Size(23, 22);
            this.strbtnHelp.Text = "帮助";
            this.strbtnHelp.Click += new System.EventHandler(this.tsrbtnHelp_Click);
            // 
            // LabelEditPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.tsrUp);
            this.Name = "LabelEditPanel";
            this.Size = new System.Drawing.Size(439, 356);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChicun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvXingwei)).EndInit();
            this.tsrDown.ResumeLayout(false);
            this.tsrDown.PerformLayout();
            this.tsrUp.ResumeLayout(false);
            this.tsrUp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsrUp;
        private System.Windows.Forms.ToolStripLabel tsrlblChicun;
        private System.Windows.Forms.ToolStripTextBox tsrtxtChicun;
        private System.Windows.Forms.ToolStripLabel tsrlblXingwei;
        private System.Windows.Forms.ToolStripTextBox tsrtxtXingwei;
        private System.Windows.Forms.ToolStripButton tsrbtnUp;
        private System.Windows.Forms.ToolStripButton tsrbtnDown;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ToolStrip tsrDown;
        private System.Windows.Forms.ToolStripButton tsrbtnCancel;
        private System.Windows.Forms.ToolStripButton tsrbtnOk;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvChicun;
        private System.Windows.Forms.ToolStripButton strbtnHelp;
        private System.Windows.Forms.ToolStripButton tsrbtnSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column6;
        private System.Windows.Forms.DataGridView dgvXingwei;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.Timer timer1;

    }
}
