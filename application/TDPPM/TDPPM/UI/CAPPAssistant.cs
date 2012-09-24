using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NXOpen;
using System.Runtime.InteropServices;

namespace TDPPM
{
    public partial class CAPPAssistant : Form
    {
        public CAPPAssistant()
        {
            InitializeComponent();
            NXOpenUI.FormUtilities.ReparentForm(this);
            NXOpenUI.FormUtilities.SetApplicationIcon(this);
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}