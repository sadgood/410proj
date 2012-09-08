using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NXOpen;
using NXOpenUI;

namespace ClassLibrary1test
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        public void DrawModel(Graphics g)
        {

            SolidBrush drawBrush = new SolidBrush(Color.Red);
            //Pen CopyLinePen = new Pen(Color.Red, 2);
            System.Drawing.Point SeqLocation = new System.Drawing.Point(200, 200);
             Font drawFont = new Font("Arial", 8);
             try
             {                
                 //g.DrawString("wda", drawFont, drawBrush, SeqLocation);
                 g.FillRectangle(drawBrush, new Rectangle(20, 20, 20, 20));
             }
             catch (System.Exception ex)
             {
                 UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
             }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = null;
            ModelClass modelClass;
            DrawModel(g);

        }
    }
}
