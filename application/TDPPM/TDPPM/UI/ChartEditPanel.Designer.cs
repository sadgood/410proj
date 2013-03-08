namespace TDPPM
{
    partial class ChartEditPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pnlList = new System.Windows.Forms.Panel();
            this.dgvChart = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tsrPDF = new System.Windows.Forms.ToolStrip();
            this.tsrbtnPDFPreviewOne = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnPDFCreateAll = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnPDFPreviewAll = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnPDFOutputAll = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsrFill = new System.Windows.Forms.ToolStrip();
            this.tsrbtnFillShow = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnFillAll = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnFillTitle = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnClearShow = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnMoveUp = new System.Windows.Forms.ToolStripButton();
            this.tsrbtnMoveDown = new System.Windows.Forms.ToolStripButton();
            this.strbtnHelp = new System.Windows.Forms.ToolStripButton();
            this.pnlList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChart)).BeginInit();
            this.tsrPDF.SuspendLayout();
            this.tsrFill.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlList
            // 
            this.pnlList.Controls.Add(this.dgvChart);
            this.pnlList.Controls.Add(this.tsrPDF);
            this.pnlList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlList.Location = new System.Drawing.Point(0, 25);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(428, 361);
            this.pnlList.TabIndex = 5;
            // 
            // dgvChart
            // 
            this.dgvChart.AllowUserToAddRows = false;
            this.dgvChart.AllowUserToDeleteRows = false;
            this.dgvChart.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvChart.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvChart.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvChart.BackgroundColor = System.Drawing.Color.White;
            this.dgvChart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChart.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.dgvChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChart.Location = new System.Drawing.Point(0, 0);
            this.dgvChart.MultiSelect = false;
            this.dgvChart.Name = "dgvChart";
            this.dgvChart.RowHeadersVisible = false;
            this.dgvChart.RowTemplate.Height = 23;
            this.dgvChart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChart.Size = new System.Drawing.Size(428, 336);
            this.dgvChart.TabIndex = 3;
            this.dgvChart.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChart_CellDoubleClick);
            this.dgvChart.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChart_CellValueChanged);
            this.dgvChart.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvChart_CurrentCellDirtyStateChanged);
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column1.HeaderText = "名称";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column2.HeaderText = "所在模型";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column3.FillWeight = 200F;
            this.Column3.HeaderText = "备注";
            this.Column3.Name = "Column3";
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column4
            // 
            this.Column4.FillWeight = 50F;
            this.Column4.HeaderText = "打印";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.FillWeight = 50F;
            this.Column5.HeaderText = "通过";
            this.Column5.Name = "Column5";
            // 
            // tsrPDF
            // 
            this.tsrPDF.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tsrPDF.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsrPDF.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsrbtnPDFPreviewOne,
            this.tsrbtnPDFCreateAll,
            this.tsrbtnPDFPreviewAll,
            this.tsrbtnPDFOutputAll,
            this.tsrbtnRefresh});
            this.tsrPDF.Location = new System.Drawing.Point(0, 336);
            this.tsrPDF.Name = "tsrPDF";
            this.tsrPDF.Size = new System.Drawing.Size(428, 25);
            this.tsrPDF.TabIndex = 4;
            this.tsrPDF.Text = "toolStrip2";
            // 
            // tsrbtnPDFPreviewOne
            // 
            this.tsrbtnPDFPreviewOne.Image = global::TDPPM.Properties.Resources.preview1;
            this.tsrbtnPDFPreviewOne.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnPDFPreviewOne.Name = "tsrbtnPDFPreviewOne";
            this.tsrbtnPDFPreviewOne.Size = new System.Drawing.Size(73, 22);
            this.tsrbtnPDFPreviewOne.Text = "本页预览";
            this.tsrbtnPDFPreviewOne.Click += new System.EventHandler(this.tsrbtnPDFPreviewOne_Click);
            // 
            // tsrbtnPDFCreateAll
            // 
            this.tsrbtnPDFCreateAll.Image = global::TDPPM.Properties.Resources.fillall;
            this.tsrbtnPDFCreateAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnPDFCreateAll.Name = "tsrbtnPDFCreateAll";
            this.tsrbtnPDFCreateAll.Size = new System.Drawing.Size(73, 22);
            this.tsrbtnPDFCreateAll.Text = "生成工艺";
            this.tsrbtnPDFCreateAll.Click += new System.EventHandler(this.tsrbtnPDFCreateAll_Click);
            // 
            // tsrbtnPDFPreviewAll
            // 
            this.tsrbtnPDFPreviewAll.Image = global::TDPPM.Properties.Resources.preview2;
            this.tsrbtnPDFPreviewAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnPDFPreviewAll.Name = "tsrbtnPDFPreviewAll";
            this.tsrbtnPDFPreviewAll.Size = new System.Drawing.Size(73, 22);
            this.tsrbtnPDFPreviewAll.Text = "预览工艺";
            this.tsrbtnPDFPreviewAll.Click += new System.EventHandler(this.tsrbtnPDFPreviewAll_Click);
            // 
            // tsrbtnPDFOutputAll
            // 
            this.tsrbtnPDFOutputAll.Image = global::TDPPM.Properties.Resources.save;
            this.tsrbtnPDFOutputAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnPDFOutputAll.Name = "tsrbtnPDFOutputAll";
            this.tsrbtnPDFOutputAll.Size = new System.Drawing.Size(73, 22);
            this.tsrbtnPDFOutputAll.Text = "工艺导出";
            this.tsrbtnPDFOutputAll.Click += new System.EventHandler(this.tsrbtnPDFOutputAll_Click);
            // 
            // tsrbtnRefresh
            // 
            this.tsrbtnRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsrbtnRefresh.Image = global::TDPPM.Properties.Resources.refresh;
            this.tsrbtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnRefresh.Name = "tsrbtnRefresh";
            this.tsrbtnRefresh.Size = new System.Drawing.Size(73, 22);
            this.tsrbtnRefresh.Text = "刷新图表";
            this.tsrbtnRefresh.Click += new System.EventHandler(this.tsrbtnRefresh_Click);
            // 
            // tsrFill
            // 
            this.tsrFill.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsrFill.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsrbtnFillShow,
            this.tsrbtnFillAll,
            this.tsrbtnFillTitle,
            this.tsrbtnClearShow,
            this.tsrbtnMoveUp,
            this.tsrbtnMoveDown,
            this.strbtnHelp});
            this.tsrFill.Location = new System.Drawing.Point(0, 0);
            this.tsrFill.Name = "tsrFill";
            this.tsrFill.Size = new System.Drawing.Size(428, 25);
            this.tsrFill.TabIndex = 1;
            this.tsrFill.Text = "toolStrip1";
            // 
            // tsrbtnFillShow
            // 
            this.tsrbtnFillShow.Image = global::TDPPM.Properties.Resources.fillone;
            this.tsrbtnFillShow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnFillShow.Name = "tsrbtnFillShow";
            this.tsrbtnFillShow.Size = new System.Drawing.Size(73, 22);
            this.tsrbtnFillShow.Text = "填写显示";
            this.tsrbtnFillShow.Click += new System.EventHandler(this.tsrbtnFillShow_Click);
            // 
            // tsrbtnFillAll
            // 
            this.tsrbtnFillAll.Image = global::TDPPM.Properties.Resources.fillall;
            this.tsrbtnFillAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnFillAll.Name = "tsrbtnFillAll";
            this.tsrbtnFillAll.Size = new System.Drawing.Size(73, 22);
            this.tsrbtnFillAll.Text = "填写所有";
            this.tsrbtnFillAll.Click += new System.EventHandler(this.tsrbtnFillAll_Click);
            // 
            // tsrbtnFillTitle
            // 
            this.tsrbtnFillTitle.Image = global::TDPPM.Properties.Resources.refresh3;
            this.tsrbtnFillTitle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnFillTitle.Name = "tsrbtnFillTitle";
            this.tsrbtnFillTitle.Size = new System.Drawing.Size(73, 22);
            this.tsrbtnFillTitle.Text = "更新表头";
            this.tsrbtnFillTitle.Click += new System.EventHandler(this.tsrbtnFillTitle_Click);
            // 
            // tsrbtnClearShow
            // 
            this.tsrbtnClearShow.Image = global::TDPPM.Properties.Resources.clear;
            this.tsrbtnClearShow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnClearShow.Name = "tsrbtnClearShow";
            this.tsrbtnClearShow.Size = new System.Drawing.Size(73, 22);
            this.tsrbtnClearShow.Text = "清空显示";
            this.tsrbtnClearShow.Click += new System.EventHandler(this.tsrbtnClearShow_Click);
            // 
            // tsrbtnMoveUp
            // 
            this.tsrbtnMoveUp.Image = global::TDPPM.Properties.Resources.upWards;
            this.tsrbtnMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnMoveUp.Name = "tsrbtnMoveUp";
            this.tsrbtnMoveUp.Size = new System.Drawing.Size(49, 22);
            this.tsrbtnMoveUp.Text = "上移";
            this.tsrbtnMoveUp.Click += new System.EventHandler(this.tsrbtnMoveUp_Click);
            // 
            // tsrbtnMoveDown
            // 
            this.tsrbtnMoveDown.Image = global::TDPPM.Properties.Resources.downWards;
            this.tsrbtnMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsrbtnMoveDown.Name = "tsrbtnMoveDown";
            this.tsrbtnMoveDown.Size = new System.Drawing.Size(49, 22);
            this.tsrbtnMoveDown.Text = "下移";
            this.tsrbtnMoveDown.Click += new System.EventHandler(this.tsrbtnMoveDown_Click);
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
            this.strbtnHelp.Click += new System.EventHandler(this.strbtnHelp_Click);
            // 
            // ChartEditPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlList);
            this.Controls.Add(this.tsrFill);
            this.Name = "ChartEditPanel";
            this.Size = new System.Drawing.Size(428, 386);
            this.pnlList.ResumeLayout(false);
            this.pnlList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChart)).EndInit();
            this.tsrPDF.ResumeLayout(false);
            this.tsrPDF.PerformLayout();
            this.tsrFill.ResumeLayout(false);
            this.tsrFill.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsrFill;
        private System.Windows.Forms.ToolStripButton tsrbtnFillShow;
        private System.Windows.Forms.ToolStripButton tsrbtnFillAll;
        private System.Windows.Forms.ToolStripButton tsrbtnFillTitle;
        private System.Windows.Forms.ToolStripButton tsrbtnClearShow;
        private System.Windows.Forms.ToolStripButton tsrbtnMoveUp;
        private System.Windows.Forms.ToolStripButton tsrbtnMoveDown;
        private System.Windows.Forms.DataGridView dgvChart;
        private System.Windows.Forms.ToolStrip tsrPDF;
        private System.Windows.Forms.ToolStripButton tsrbtnPDFPreviewOne;
        private System.Windows.Forms.ToolStripButton tsrbtnPDFCreateAll;
        private System.Windows.Forms.ToolStripButton tsrbtnPDFPreviewAll;
        private System.Windows.Forms.ToolStripButton tsrbtnPDFOutputAll;
        private System.Windows.Forms.ToolStripButton tsrbtnRefresh;
        private System.Windows.Forms.Panel pnlList;
        private System.Windows.Forms.ToolStripButton strbtnHelp;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column5;

    }
}
