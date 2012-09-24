
using System;
using NXOpen;
using NXOpen.UF;
using TDPPM;
using System.Windows.Forms;

namespace TDPPM
{

    public class Program
    {
        // class members
        public static Session theSession;
        public static UI theUI;
        public static UFSession theUfSession;
        public static Program theProgram;
        public static bool isDisposeCalled;



        //------------------------------------------------------------------------------
        // Constructor
        //------------------------------------------------------------------------------
        public Program()
        {
            try
            {
                theSession = Session.GetSession();
                theUI = UI.GetUI();
                theUfSession = UFSession.GetUFSession();
                isDisposeCalled = false;
            }
            catch (NXOpen.NXException ex)
            {
                // ---- Enter your exception handling code here -----
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            }
        }

        //------------------------------------------------------------------------------
        //  Explicit Activation
        //      This entry point is used to activate the application explicitly
        //------------------------------------------------------------------------------
        public static int Main(string[] args)
        {
            int retValue = 0;
            try
            {
                theProgram = new Program();

                //TODO: Add your application code here 
                if (args[0] == "3DPPM_RevolveTrim")
                {
                    NXFun.ShowHuizhuan();
                }
                else if (args[0] == "3DPPM_AddTolerance")
                {
                    NXFun.ShowGongcha();              
                }
                else if (args[0] == "3DPPM_MachinedSurface")
                {
                    NXFun.ShowJiagongmian();
                }
                else if (args[0] == "3DPPM_CAPPAssistant")
                {
                    //                 CAPPAssistant assistant = new CAPPAssistant();
                    //                 assistant.Show();
                    NXFun.ShowCAPP();
                    
                }
                else if (args[0] == "3DPPM_Help")
                {
                    NXFun.ShowHelp();
                }
                else if (args[0] == "3DPPM_3DPPM")
                {
                    MainDlg md = new MainDlg();
                    md.Show();
                }
                else if (args[0] == "3DPPM_About")
                {
                    NXFun.ShowAbout();
                }
                theProgram.Dispose();
            }
            catch (NXOpen.NXException ex)
            {
                // ---- Enter your exception handling code here -----
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            }
            return retValue;
        }
        
        //------------------------------------------------------------------------------
        // Following method disposes all the class members
        //------------------------------------------------------------------------------
        public void Dispose()
        {
            try
            {
                if (isDisposeCalled == false)
                {
                    //TODO: Add your application code here 
                    //int a = 1;
                }
                isDisposeCalled = true;
            }
            catch (NXOpen.NXException ex)
            {
                // ---- Enter your exception handling code here -----
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);

            }
        }

        public static int GetUnloadOption(string arg)
        {
            //Unloads the image explicitly, via an unload dialog
            //UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, "GetUnloadOption");
            return System.Convert.ToInt32(Session.LibraryUnloadOption.Explicitly);

            //Unloads the image immediately after execution within NX
            // return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);

            //Unloads the image when the NX session terminates
            //return System.Convert.ToInt32(Session.LibraryUnloadOption.AtTermination);
        }
        public static int UnloadLibrary(string arg)
        {
            //UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error,"bb");
            if (theProgram != null)
            {
               // UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, "aa");

                // Ensure that the callbacks are unregistered. It is important to do this
                // before the library is unloaded, otherwise an error will occur the next time
                // NX tries to invoke the callback.
                // theProgram.UnregisterCallbacks();
                //  theProgram.Close();
                theProgram.Dispose();
                theProgram = null;
            }
            return 0;
        }

    }

}