using System;
using System.Collections.Generic;
using System.Text;
using NXOpen;
using NXOpenUI;

namespace ClassLibrary1test
{
    public class Class1
    {
        // class members
        public static int Main(string[] args)
        {
            int retValue = 0;
            try
            {
                

                //TODO: Add your application code here 
                if (args[0] == "sss")
                {
                    Form1 s=new Form1();
                    s.Show();
                }
               
            }
            catch (NXOpen.NXException ex)
            {
                // ---- Enter your exception handling code here -----
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            }
            return retValue;
        }

    }
}
