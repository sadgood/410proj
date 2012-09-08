//==============================================================================
//  WARNING!!  This file is overwritten by the Block UI Styler while generating
//  the automation code. Any modifications to this file will be lost after
//  generating the code again.
//
//       Filename:  C:\Users\yibo\Desktop\ano.cs
//
//        This file was generated by the NX Block UI Styler
//        Created by: yibo
//              Version: NX 7.5
//              Date: 09-05-2012  (Format: mm-dd-yyyy)
//              Time: 14:26 (Format: hh-mm)
//
//==============================================================================

//==============================================================================
//  Purpose:  This TEMPLATE file contains C# source to guide you in the
//  construction of your Block application dialog. The generation of your
//  dialog file (.dlx extension) is the first step towards dialog construction
//  within NX.  You must now create a NX Open application that
//  utilizes this file (.dlx).
//
//  The information in this file provides you with the following:
//
//  1.  Help on how to load and display your Block UI Styler dialog in NX
//      using APIs provided in NXOpen.BlockStyler namespace
//  2.  The empty callback methods (stubs) associated with your dialog items
//      have also been placed in this file. These empty methods have been
//      created simply to start you along with your coding requirements.
//      The method name, argument list and possible return values have already
//      been provided for you.
//==============================================================================

//------------------------------------------------------------------------------
//These imports are needed for the following template code
//------------------------------------------------------------------------------
using System;
using NXOpen;
using NXOpen.BlockStyler;
using NXOpen.UF;
using System.Reflection;
using System.Runtime.InteropServices;

using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Text;
//------------------------------------------------------------------------------
//Represents Block Styler application class
//------------------------------------------------------------------------------
public class ano
{
    //class members
    private static Session theSession = null;
    private static UI theUI = null;
    public static ano theano;
    private string theDialogName;
    private NXOpen.BlockStyler.BlockDialog theDialog;
    private NXOpen.BlockStyler.UIBlock group0;// Block type: Group
    private NXOpen.BlockStyler.UIBlock enum0;// Block type: Enumeration
    private NXOpen.BlockStyler.UIBlock ifcro;// Block type: Toggle
    private NXOpen.BlockStyler.UIBlock cro;// Block type: Selection
    private NXOpen.BlockStyler.UIBlock realanno;// Block type: Multiline String
    private NXOpen.BlockStyler.UIBlock iflabel;// Block type: Toggle
    private NXOpen.BlockStyler.UIBlock selection01;// Block type: Selection
    private NXOpen.BlockStyler.UIBlock point0;// Block type: Specify Point
    private NXOpen.BlockStyler.UIBlock group;// Block type: Group
    private NXOpen.BlockStyler.UIBlock dic;// Block type: Multiline String
    private NXOpen.BlockStyler.UIBlock str;// Block type: Enumeration
    private NXOpen.BlockStyler.UIBlock button0;// Block type: Button
    private NXOpen.BlockStyler.UIBlock newviewname;// Block type: String
    private noteassistance thenotefun = new noteassistance();
    NXOpen.ModelingView[] aaa = null;//所有的视图
    string[] strvalue;
    public string viewname = null;
    //------------------------------------------------------------------------------
    //Bit Option for Property: SnapPointTypesEnabled
    //------------------------------------------------------------------------------
    public static readonly int              SnapPointTypesEnabled_UserDefined = (1 << 0);
    public static readonly int                 SnapPointTypesEnabled_Inferred = (1 << 1);
    public static readonly int           SnapPointTypesEnabled_ScreenPosition = (1 << 2);
    public static readonly int                 SnapPointTypesEnabled_EndPoint = (1 << 3);
    public static readonly int                 SnapPointTypesEnabled_MidPoint = (1 << 4);
    public static readonly int             SnapPointTypesEnabled_ControlPoint = (1 << 5);
    public static readonly int             SnapPointTypesEnabled_Intersection = (1 << 6);
    public static readonly int                SnapPointTypesEnabled_ArcCenter = (1 << 7);
    public static readonly int            SnapPointTypesEnabled_QuadrantPoint = (1 << 8);
    public static readonly int            SnapPointTypesEnabled_ExistingPoint = (1 << 9);
    public static readonly int             SnapPointTypesEnabled_PointonCurve = (1 <<10);
    public static readonly int           SnapPointTypesEnabled_PointonSurface = (1 <<11);
    public static readonly int         SnapPointTypesEnabled_PointConstructor = (1 <<12);
    public static readonly int     SnapPointTypesEnabled_TwocurveIntersection = (1 <<13);
    public static readonly int             SnapPointTypesEnabled_TangentPoint = (1 <<14);
    public static readonly int                    SnapPointTypesEnabled_Poles = (1 <<15);
    public static readonly int         SnapPointTypesEnabled_BoundedGridPoint = (1 <<16);
    //------------------------------------------------------------------------------
    //Bit Option for Property: SnapPointTypesOnByDefault
    //------------------------------------------------------------------------------
    public static readonly int          SnapPointTypesOnByDefault_UserDefined = (1 << 0);
    public static readonly int             SnapPointTypesOnByDefault_Inferred = (1 << 1);
    public static readonly int       SnapPointTypesOnByDefault_ScreenPosition = (1 << 2);
    public static readonly int             SnapPointTypesOnByDefault_EndPoint = (1 << 3);
    public static readonly int             SnapPointTypesOnByDefault_MidPoint = (1 << 4);
    public static readonly int         SnapPointTypesOnByDefault_ControlPoint = (1 << 5);
    public static readonly int         SnapPointTypesOnByDefault_Intersection = (1 << 6);
    public static readonly int            SnapPointTypesOnByDefault_ArcCenter = (1 << 7);
    public static readonly int        SnapPointTypesOnByDefault_QuadrantPoint = (1 << 8);
    public static readonly int        SnapPointTypesOnByDefault_ExistingPoint = (1 << 9);
    public static readonly int         SnapPointTypesOnByDefault_PointonCurve = (1 <<10);
    public static readonly int       SnapPointTypesOnByDefault_PointonSurface = (1 <<11);
    public static readonly int     SnapPointTypesOnByDefault_PointConstructor = (1 <<12);
    public static readonly int SnapPointTypesOnByDefault_TwocurveIntersection = (1 <<13);
    public static readonly int         SnapPointTypesOnByDefault_TangentPoint = (1 <<14);
    public static readonly int                SnapPointTypesOnByDefault_Poles = (1 <<15);
    public static readonly int     SnapPointTypesOnByDefault_BoundedGridPoint = (1 <<16);
    
    //------------------------------------------------------------------------------
    //Constructor for NX Styler class
    //------------------------------------------------------------------------------
    public ano()
    {
        try
        {
            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theDialogName = "ano.dlx";
            theDialog = theUI.CreateDialog(theDialogName);
            theDialog.AddApplyHandler(new NXOpen.BlockStyler.BlockDialog.Apply(apply_cb));
            theDialog.AddOkHandler(new NXOpen.BlockStyler.BlockDialog.Ok(ok_cb));
            theDialog.AddUpdateHandler(new NXOpen.BlockStyler.BlockDialog.Update(update_cb));
            theDialog.AddCancelHandler(new NXOpen.BlockStyler.BlockDialog.Cancel(cancel_cb));
            theDialog.AddFilterHandler(new NXOpen.BlockStyler.BlockDialog.Filter(filter_cb));
            theDialog.AddInitializeHandler(new NXOpen.BlockStyler.BlockDialog.Initialize(initialize_cb));
            theDialog.AddFocusNotifyHandler(new NXOpen.BlockStyler.BlockDialog.FocusNotify(focusNotify_cb));
            theDialog.AddKeyboardFocusNotifyHandler(new NXOpen.BlockStyler.BlockDialog.KeyboardFocusNotify(keyboardFocusNotify_cb));
            theDialog.AddDialogShownHandler(new NXOpen.BlockStyler.BlockDialog.DialogShown(dialogShown_cb));
        }
        catch (Exception ex)
        {
            //---- Enter your exception handling code here -----
            throw ex;
        }
    }
    //------------------------------- DIALOG LAUNCHING ---------------------------------
    //
    //    Before invoking this application one needs to open any part/empty part in NX
    //    because of the behavior of the blocks.
    //
    //    Make sure the dlx file is in one of the following locations:
    //        1.) From where NX session is launched
    //        2.) $UGII_USER_DIR/application
    //        3.) For released applications, using UGII_CUSTOM_DIRECTORY_FILE is highly
    //            recommended. This variable is set to a full directory path to a file 
    //            containing a list of root directories for all custom applications.
    //            e.g., UGII_CUSTOM_DIRECTORY_FILE=$UGII_ROOT_DIR\menus\custom_dirs.dat
    //
    //    You can create the dialog using one of the following way:
    //
    //    1. Journal Replay
    //
    //        1) Replay this file through Tool->Journal->Play Menu.
    //
    //    2. USER EXIT
    //
    //        1) Create the Shared Library -- Refer "Block UI Styler programmer's guide"
    //        2) Invoke the Shared Library through File->Execute->NX Open menu.
    //
    //------------------------------------------------------------------------------
    public static void Main()
    {
        try
        {
            theano = new ano();
            // The following method shows the dialog immediately
            theano.Show();
        }
        catch (Exception ex)
        {
            //---- Enter your exception handling code here -----
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
        finally
        {
            theano.Dispose();
        }
    }
    //------------------------------------------------------------------------------
    // This method specifies how a shared image is unloaded from memory
    // within NX. This method gives you the capability to unload an
    // internal NX Open application or user  exit from NX. Specify any
    // one of the three constants as a return value to determine the type
    // of unload to perform:
    //
    //
    //    Immediately : unload the library as soon as the automation program has completed
    //    Explicitly  : unload the library from the "Unload Shared Image" dialog
    //    AtTermination : unload the library when the NX session terminates
    //
    //
    // NOTE:  A program which associates NX Open applications with the menubar
    // MUST NOT use this option since it will UNLOAD your NX Open application image
    // from the menubar.
    //------------------------------------------------------------------------------
     public static int GetUnloadOption(string arg)
    {
        //return System.Convert.ToInt32(Session.LibraryUnloadOption.Explicitly);
         return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);
        // return System.Convert.ToInt32(Session.LibraryUnloadOption.AtTermination);
    }
    
    //------------------------------------------------------------------------------
    // Following method cleanup any housekeeping chores that may be needed.
    // This method is automatically called by NX.
    //------------------------------------------------------------------------------
    public static int UnloadLibrary(string arg)
    {
        try
        {
            //---- Enter your code here -----
        }
        catch (Exception ex)
        {
            //---- Enter your exception handling code here -----
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
        return 0;
    }
    
    //------------------------------------------------------------------------------
    //This method shows the dialog on the screen
    //------------------------------------------------------------------------------
    public NXOpen.UIStyler.DialogResponse Show()
    {
        try
        {
            theDialog.Show();
        }
        catch (Exception ex)
        {
            //---- Enter your exception handling code here -----
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
        return 0;
    }
    
    //------------------------------------------------------------------------------
    //Method Name: Dispose
    //------------------------------------------------------------------------------
    public void Dispose()
    {
        if(theDialog != null)
        {
            theDialog.Dispose();
            theDialog = null;
        }
    }
    
    //------------------------------------------------------------------------------
    //---------------------Block UI Styler Callback Functions--------------------------
    //------------------------------------------------------------------------------
    
    //------------------------------------------------------------------------------
    //Callback Name: initialize_cb
    //------------------------------------------------------------------------------
    public void initialize_cb()
    {
        try
        {
            group0 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("group0");
            enum0 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("enum0");
            ifcro = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("ifcro");
            cro = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("cro");
            realanno = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("realanno");
            iflabel = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("iflabel");
            selection01 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("selection01");
            point0 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("point0");
            group = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("group");
            dic = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("dic");
            str = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("str");
            button0 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("button0");
            newviewname = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("newviewname");
        }
        catch (Exception ex)
        {
            //---- Enter your exception handling code here -----
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
    }
    
    //------------------------------------------------------------------------------
    //Callback Name: dialogShown_cb
    //This callback is executed just before the dialog launch. Thus any value set 
    //here will take precedence and dialog will be launched showing that value. 
    //------------------------------------------------------------------------------
    private void SetSBF()
    {
        string sbf_file = "E:\\gitest\\096\\096\\3dppm\\3dppm_uds.sbf";
        if (!isFileExist(sbf_file))
        {
            theUI.NXMessageBox.Show("未能加载", NXMessageBox.DialogType.Error, "未能加载Sbf文件");
        }
        else
        {
            UFSession theuf = UFSession.GetUFSession();
            theuf.Drf.SetSpecifiedSbfFile(sbf_file);
            //Program.theUfSession.Drf.SetSpecifiedSbfFile(sbf_file);
        }
    }
    public void dialogShown_cb()
    {
        try
        {
            realanno.GetProperties().SetLogical("Show",true);
            
            newviewname.GetProperties().SetLogical("Show", false);
            ifcro.GetProperties().SetLogical("Value", false);
            cro.GetProperties().SetLogical("Enable",false);
            if (ifcro.GetProperties().GetLogical("Value"))
            {
                cro.GetProperties().SetLogical("Enable", true);
            }
            else if (!ifcro.GetProperties().GetLogical("Value"))
            {
                cro.GetProperties().SetLogical("Enable", false);
            }
            iflabel.GetProperties().SetLogical("Value", false);
            selection01.GetProperties().SetLogical("Enable", false);
            OpenFile("E:\\gitest\\410proj\\3dppm\\CAPP助手.exe");
           // SetSBF();
           //Part workPart = theSession.Parts.Work;
           //     ModelingViewCollection a = workPart.ModelingViews;
           //     aaa = a.ToArray();
           //  strvalue = new string[aaa.Length+1];
           // int i ;
           //     for ( i = 0; i < aaa.Length;i++ )
           //     {
           //         //enum0.GetProperties().SetEnum("Value", i);
           //        strvalue[i] = aaa[i].Name;
                  
           //     }
           //     strvalue[i] = "添加新视图";
           //     enum0.GetProperties().SetEnumMembers("Value", strvalue);
            refreshenum();
            //---- Enter your callback code here -----
        }
        catch (Exception ex)
        {
            //---- Enter your exception handling code here -----
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
    }
    public static bool isFileExist(string path)
    {
        return System.IO.File.Exists(path);
    }
    public static bool OpenFile(string filepath)
    {
        if (isFileExist(filepath))
        {
            Process.Start(filepath);
            return true;
        }
        else
        {
            theUI.NXMessageBox.Show("未能打开", NXMessageBox.DialogType.Error, "未能打开CAPP助手");
            return false;
        }
    }  
    //------------------------------------------------------------------------------
    //Callback Name: apply_cb
    //------------------------------------------------------------------------------
    public int apply_cb()
    {
        int errorCode = 0;
        try
        {
          
            //---- Enter your callback code here -----
        }
        catch (Exception ex)
        {
            //---- Enter your exception handling code here -----
            errorCode = 1;
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
        return errorCode;
    }
    public void refreshenum()
    {
        Part workPart = theSession.Parts.Work;
        ModelingViewCollection a = workPart.ModelingViews;
        aaa = a.ToArray();
        strvalue = new string[aaa.Length + 1];
        int i;
        for (i = 0; i < aaa.Length; i++)
        {
            //enum0.GetProperties().SetEnum("Value", i);
            strvalue[i] = aaa[i].Name;

        }
        strvalue[i] = "添加新视图";
        enum0.GetProperties().SetEnumMembers("Value", strvalue);
    
    }
    //------------------------------------------------------------------------------
    //Callback Name: update_cb
    //------------------------------------------------------------------------------
    public int update_cb( NXOpen.BlockStyler.UIBlock block)
    {
        try
        {
            if(block == enum0)
            {
                 Part workPart = theSession.Parts.Work;
                 Layout layout1 = (Layout)workPart.Layouts.FindObject("L1");//此处写死了。。。。。TAG ERROR
                int sel = enum0.GetProperties().GetEnum("Value");
                string addnewstr = enum0.GetProperties().GetEnumAsString("Value");
                    if (addnewstr != "添加新视图")
                    {
                layout1.ReplaceView(workPart.ModelingViews.WorkView, aaa[sel], true);
                    }
                    else
                    {
                        newviewname.GetProperties().SetLogical("Show", true);
                       
                        //string newviewname = null;
                        ////add.Show_add();
                        //add theadd = new add();
                        //theadd.Show();
                       //newviewname = theaddnew.string0.GetProperties().GetString("Value");
                      
                    }
              
            //---------Enter your code here-----------
            }
            else if(block == ifcro)
            {
                if(ifcro.GetProperties().GetLogical("Value"))
                {
                cro.GetProperties().SetLogical("Enable",true);
                
                }
                else if (!ifcro.GetProperties().GetLogical("Value"))
                {
                cro.GetProperties().SetLogical("Enable",false);
                }
                
           
            //---------Enter your code here-----------
            }
            else if(block == cro)
            {
            //---------Enter your code here-----------
            }
            else if(block == realanno)
            {
            //---------Enter your code here-----------
            }
            else if(block == iflabel)
            {
                if (iflabel.GetProperties().GetLogical("Value"))
                {

                    selection01.GetProperties().SetLogical("Enable", true);

                }
                else if (!iflabel.GetProperties().GetLogical("Value"))
                {
                    selection01.GetProperties().SetLogical("Enable", false);
                }
            //---------Enter your code here-----------
            }
            else if(block == selection01)
            {

            //---------Enter your code here-----------
            }
            else if(block == point0)
            {
                NXOpen.TaggedObject[] guanlian = null;
                NXOpen.DisplayableObject guanlianobj = null;
                if (ifcro.GetProperties().GetLogical("Value") == true)
                {
                    guanlian = cro.GetProperties().GetTaggedObjectVector("SelectedObjects");
                    guanlianobj = Tag2NXObject<DisplayableObject>(guanlian[0].Tag);//这个是关联对象
                }

                string[] zhushiwenzi = null;
                zhushiwenzi = realanno.GetProperties().GetStrings("Value");//这个是注释文字
               // int ps = zhushiwenzi[0].Length;
                //if (zhushiwenzi == null)
                //{

                //    theUI.NXMessageBox.Show("警告", NXMessageBox.DialogType.Error, "注释文字不能为空");
                //}
                NXOpen.TaggedObject[] zhiyin = null;
                NXOpen.NXObject zhiyinobj = null;

                if (iflabel.GetProperties().GetLogical("Value") == true)
                {              
                    zhiyin = selection01.GetProperties().GetTaggedObjectVector("SelectedObjects");
            
                    zhiyinobj = Tag2NXObject<NXObject>(zhiyin[0].Tag);//指引线
                }
             NXOpen.TaggedObject[] placept = null;
             placept = point0.GetProperties().GetTaggedObjectVector("SelectedObjects");
             Point placeptobj = Tag2NXObject<Point>(placept[0].Tag);//注释最后的放置点
             Point3d placeptobj3d = placeptobj.Coordinates;
            //---------Enter our code here-----------
             SetSBF();
             thenotefun.function_note(zhushiwenzi,guanlianobj,zhiyinobj,placeptobj3d);



            }
            else if(block == dic)
            {
            //---------Enter your code here-----------
            }
            else if (block == newviewname)
            {
                Part workPart = theSession.Parts.Work;
                viewname = newviewname.GetProperties().GetString("Value");
                workPart.Views.SaveAs(workPart.ModelingViews.WorkView, viewname, true, false);
                refreshenum();
                
                //enum0.GetProperties().SetEnumAsString("Value", viewname);//TAG undone
                //enum0.GetProperties().get
                //---------Enter your code here-----------
            }
            else if(block == str)
            {
            //---------Enter your code here-----------
            }
            else if(block == button0)//这个就是添加按钮，在这里调用
            {
                string[] diction = null;//这个有”添加至CAPP助手“中得到的字符串
              diction = dic.GetProperties().GetStrings("Value");
              if (diction == null)
              {
                  theUI.NXMessageBox.Show("警告", NXMessageBox.DialogType.Error, "添加文字不能为空");
              
              }
            string strvalue = str.GetProperties().GetEnumAsString("Value");

            thenotefun.Xmlforcapp(strvalue,diction[0]);

            StopProcess("CAPP助手");
            OpenFile("E:\\gitest\\410proj\\3dppm\\CAPP助手.exe");
            //switch (strvalue)
            //{
            //    case "加工前准备":
                   
            //        break;
            //    case "工作内容":
                    

            //        break;
            //    case "技术条件":

            //        break;
            //    case "附注":
                    
            //        break;
            //    case "特种工艺术语":
                   
            //        break;
            //    case "检验工序":
                 
            //        break;
            //    case "常用符号":
                  
            //        break;
               

            //}
            //---------Enter your code here-----------
            }
        }
        catch (Exception ex)
        {
            //---- Enter your exception handling code here -----
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
        return 0;
    }
    public static void StopProcess(string processName)
    {
        try
        {
            Process[] ps = System.Diagnostics.Process.GetProcessesByName(processName);
            foreach (Process p in ps)
            {
                p.Kill();
            }
           
        }
        catch (Exception ex)

        {

            throw ex;
        }
    
    }
    public static T Tag2NXObject<T>(Tag tag)
    {
        try
        {
            object to = NXOpen.Utilities.NXObjectManager.Get(tag);
            return (T)to;
        }
        catch (System.Exception ex)
        {
            UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            return default(T);
        }
    }
    //------------------------------------------------------------------------------
    //Callback Name: ok_cb
    //------------------------------------------------------------------------------
    public int ok_cb()
    {
        int errorCode = 0;
        try
        {
            StopProcess("CAPP助手");
            errorCode = apply_cb();
            //---- Enter your callback code here -----
        }
        catch (Exception ex)
        {
            //---- Enter your exception handling code here -----
            errorCode = 1;
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
        return errorCode;
    }
    
    //------------------------------------------------------------------------------
    //Callback Name: cancel_cb
    //------------------------------------------------------------------------------
    public int cancel_cb()
    {
        try
        {
       
        //    int q = theUI.NXMessageBox.Show("关闭CAPP助手添加新视图", NXMessageBox.DialogType.Question, "是否关闭CAPP助手");
        //    switch(q)
        //    {
        //        case 2://no
                   
        //            break;
        //        case 1://yes
        //            //StopProcess("F:\\ano\\application\\CAPP助手.exe");
                    StopProcess("CAPP助手");
                    //break;

            //}
            
            //---- Enter your callback code here -----
        }
        catch (Exception ex)
        {
            //---- Enter your exception handling code here -----
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
        return 0;
    }
    
    //------------------------------------------------------------------------------
    //Callback Name: filter_cb
    //------------------------------------------------------------------------------
    public int filter_cb(NXOpen.BlockStyler.UIBlock block, NXOpen.TaggedObject selectedObject)
    {
        return(NXOpen.UF.UFConstants.UF_UI_SEL_ACCEPT);
    }
    
    //------------------------------------------------------------------------------
    //Callback Name: focusNotify_cb
    //This callback is executed when any block (except the ones which receive keyboard entry such as Integer block) receives focus.
    //------------------------------------------------------------------------------
    public void focusNotify_cb(NXOpen.BlockStyler.UIBlock block, bool focus)
    {
        try
        {
            //---- Enter your callback code here -----
        }
        catch (Exception ex)
        {
            //---- Enter your exception handling code here -----
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
    }
    
    //------------------------------------------------------------------------------
    //Callback Name: keyboardFocusNotify_cb
    //This callback is executed when block which can receive keyboard entry, receives the focus.
    //------------------------------------------------------------------------------
    public void keyboardFocusNotify_cb(NXOpen.BlockStyler.UIBlock block, bool focus)
    {
        try
        {
            //---- Enter your callback code here -----
        }
        catch (Exception ex)
        {
            //---- Enter your exception handling code here -----
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
    }
    
}
