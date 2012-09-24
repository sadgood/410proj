using WaveRelationControl;
namespace TDPPM
{
    partial class WavePreview
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.WRC = new WaveRelationControl.WaveRelationCtl();
            this.SuspendLayout();
            // 
            // WRC
            // 
            this.WRC.AutoScroll = true;
            this.WRC.BackColor = System.Drawing.Color.White;
            this.WRC.Location = new System.Drawing.Point(12, 12);
            this.WRC.Name = "WRC";
            this.WRC.Size = new System.Drawing.Size(379, 292);
            this.WRC.TabIndex = 0;
            // 
            // WavePreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 316);
            this.Controls.Add(this.WRC);
            this.Name = "WavePreview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "WavePreview";
            this.ResumeLayout(false);

        }

        #endregion

        private WaveRelationCtl WRC;
    }
}