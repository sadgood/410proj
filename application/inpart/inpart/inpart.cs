//==============================================================================
//  WARNING!!  This file is overwritten by the Block UI Styler while generating
//  the automation code. Any modifications to this file will be lost after
//  generating the code again.
//
//       Filename:  C:\Users\Jerry\Desktop\inpart.cs
//
//        This file was generated by the NX Block UI Styler
//        Created by: Jerry
//              Version: NX 7.5
//              Date: 07-04-2012  (Format: mm-dd-yyyy)
//              Time: 10:43 (Format: hh-mm)
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
using NXOpenUI;
using NXOpen.UF;
using System.Collections;
using System.Collections.Generic;

//------------------------------------------------------------------------------
//Represents Block Styler application class
//------------------------------------------------------------------------------
public class inpart
{
    //class members
    private static Session theSession = null;
    private static UI theUI = null;
    public static inpart theinpart;
    private string theDialogName;
    private NXOpen.BlockStyler.BlockDialog theDialog;
    private NXOpen.BlockStyler.UIBlock group01;// Block type: Group
    private NXOpen.BlockStyler.UIBlock selection0;// Block type: Selection
    private NXOpen.BlockStyler.UIBlock toggle0;// Block type: Toggle
    private NXOpen.BlockStyler.UIBlock selection01;// Block type: Selection
    private NXOpen.BlockStyler.UIBlock integer0;// Block type: Integer
    private NXOpen.BlockStyler.Tree tree_control0;// Block type: Tree Control
    private NXOpen.BlockStyler.UIBlock label0;// Block type: Label
    private NXOpen.BlockStyler.UIBlock group03;// Block type: Group
    private NXOpen.BlockStyler.UIBlock group02;// Block type: Group
    private NXOpen.BlockStyler.UIBlock group0;// Block type: Group
    public NXOpen.TaggedObject[] theoripmi;//要校核的PMI
    public NXOpen.TaggedObject[] theallpmi;//在这些PMI中进行校核
    public ArrayList dimarylist = new ArrayList();//记录所有除要校核尺寸外的尺寸
    public ArrayList addcir = new ArrayList();//存储增环
    public ArrayList deccir = new ArrayList();//存储减环
    public ArrayList thedown = new ArrayList();//记录整个工序内除过要校核尺寸外的尺寸
    List<int[]> lst_Combination;
    double finalx = 0;//对应一个尺寸属性列表中的x分值，下同。
    double finaly = 0;
    double finalz = 0;
    NXOpen.Annotations.Dimension[] thefinalori = null;//不预先设定校核范围时，最后存储尺寸的变量
    DatumAxis xformone = null;//要校核的尺寸生成的轴
    NXOpen.Annotations.Dimension[] dimary = null;
    NXOpen.Annotations.Dimension[] left = null;
    public ArrayList theday = new ArrayList();
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
    public static readonly int     SnapPointTypesOnByDefault_BoundedGridPoint = (1 <<16);
    
    //------------------------------------------------------------------------------
    //Constructor for NX Styler class
    //------------------------------------------------------------------------------
    public inpart()
    {
        try
        {
            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theDialogName = "inpart.dlx";
            theDialog = theUI.CreateDialog(theDialogName);
            theDialog.AddApplyHandler(new NXOpen.BlockStyler.BlockDialog.Apply(apply_cb));
            theDialog.AddOkHandler(new NXOpen.BlockStyler.BlockDialog.Ok(ok_cb));
            theDialog.AddUpdateHandler(new NXOpen.BlockStyler.BlockDialog.Update(update_cb));
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
            theinpart = new inpart();
            // The following method shows the dialog immediately
            theinpart.Show();
        }
        catch (Exception ex)
        {
            //---- Enter your exception handling code here -----
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
        finally
        {
            theinpart.Dispose();
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
            group01 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("group01");
            selection0 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("selection0");
            toggle0 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("toggle0");
            selection01 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("selection01");
            integer0 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("integer0");
            tree_control0 = (NXOpen.BlockStyler.Tree)theDialog.TopBlock.FindBlock("tree_control0");
            label0 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("label0");
            group03 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("group03");
            group02 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("group02");
            group0 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("group0");
            //------------------------------------------------------------------------------
            //Registration of Treelist specific callbacks
            //------------------------------------------------------------------------------
            //tree_control0.SetOnExpandHandler(new NXOpen.BlockStyler.Tree.OnExpandCallback(OnExpandCallback));
            
            //tree_control0.SetOnInsertColumnHandler(new NXOpen.BlockStyler.Tree.OnInsertColumnCallback(OnInsertColumnCallback));
            
            //tree_control0.SetOnInsertNodeHandler(new NXOpen.BlockStyler.Tree.OnInsertNodeCallback(OnInsertNodecallback));
            
            //tree_control0.SetOnDeleteNodeHandler(new NXOpen.BlockStyler.Tree.OnDeleteNodeCallback(OnDeleteNodecallback));
            
            //tree_control0.SetOnPreSelectHandler(new NXOpen.BlockStyler.Tree.OnPreSelectCallback(OnPreSelectcallback));
            
            //tree_control0.SetOnSelectHandler(new NXOpen.BlockStyler.Tree.OnSelectCallback(OnSelectcallback));
            
            //tree_control0.SetOnStateChangeHandler(new NXOpen.BlockStyler.Tree.OnStateChangeCallback(OnStateChangecallback));
            
            //tree_control0.SetToolTipTextHandler(new NXOpen.BlockStyler.Tree.ToolTipTextCallback(ToolTipTextcallback));
            
            //tree_control0.SetColumnSortHandler(new NXOpen.BlockStyler.Tree.ColumnSortCallback(ColumnSortcallback));
            
            //tree_control0.SetStateIconNameHandler(new NXOpen.BlockStyler.Tree.StateIconNameCallback(StateIconNameCallback));
            
            //tree_control0.SetOnBeginLabelEditHandler(new NXOpen.BlockStyler.Tree.OnBeginLabelEditCallback(OnBeginLabelEditCallback));
            
            //tree_control0.SetOnEndLabelEditHandler(new NXOpen.BlockStyler.Tree.OnEndLabelEditCallback(OnEndLabelEditCallback));
            
            //tree_control0.SetOnEditOptionSelectedHandler(new NXOpen.BlockStyler.Tree.OnEditOptionSelectedCallback(OnEditOptionSelectedCallback));
            
           // tree_control0.SetAskEditControlHandler(new NXOpen.BlockStyler.Tree.AskEditControlCallback(AskEditControlCallback));
            
            //tree_control0.SetOnMenuHandler(new NXOpen.BlockStyler.Tree.OnMenuCallback(OnMenuCallback));;
            
            //tree_control0.SetOnMenuSelectionHandler(new NXOpen.BlockStyler.Tree.OnMenuSelectionCallback(OnMenuSelectionCallback));;
            
            //tree_control0.SetIsDropAllowedHandler(new NXOpen.BlockStyler.Tree.IsDropAllowedCallback(IsDropAllowedCallback));;
            
            //tree_control0.SetIsDragAllowedHandler(new NXOpen.BlockStyler.Tree.IsDragAllowedCallback(IsDragAllowedCallback));;
            
            //tree_control0.SetOnDropHandler(new NXOpen.BlockStyler.Tree.OnDropCallback(OnDropCallback));;
            
            //tree_control0.SetOnDropMenuHandler(new NXOpen.BlockStyler.Tree.OnDropMenuCallback(OnDropMenuCallback));
            
            //------------------------------------------------------------------------------
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
    public void dialogShown_cb()
    {
        try
        {
           toggle0.GetProperties().SetLogical("Value", false);
           selection01.GetProperties().SetLogical("Enable",false);
           
      
        }
        catch (Exception ex)
        {
            //---- Enter your exception handling code here -----
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
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
    //public static string left(string sSource, int iLength)
    //{
      //  return sSource.Substring(0, iLength > sSource.Length ? sSource.Length : iLength);
    //}
    //------------------------------------------------------------------------------
    //Callback Name: apply_cb
    //------------------------------------------------------------------------------
    public NXObject shank(Point point1, Point point2)
    {
        Part workPart = theSession.Parts.Work;
        NXObject shanker;
        NXOpen.Features.Feature nullFeatures_Feature = null;
        NXOpen.Features.DatumAxisBuilder abuilder;
        abuilder = workPart.Features.CreateDatumAxisBuilder(nullFeatures_Feature);
        abuilder.ArcLength.Expression.RightHandSide = "0";
        abuilder.Type = NXOpen.Features.DatumAxisBuilder.Types.TwoPoints;
        abuilder.IsAssociative = true;
        Xform nullXform = null;
        Point pt1 = workPart.Points.CreatePoint(point1, nullXform, NXOpen.SmartObject.UpdateOption.WithinModeling);
        Point pt2 = workPart.Points.CreatePoint(point2, nullXform, NXOpen.SmartObject.UpdateOption.WithinModeling);
        abuilder.Point1 = pt1;
        abuilder.Point2 = pt2;
        shanker = abuilder.Commit();
        abuilder.Destroy();

        return shanker;

    }
    public DatumAxis creataxis(NXOpen.Annotations.Dimension dimen)//这个函数直接从一个dimension得到一个它的轴
    {
        Part workPart = theSession.Parts.Work;
        double startx = 0;
        double starty = 0;
        double startz = 0;
        double endx = 0;
        double endy = 0;
        double endz = 0;
        startx = dimen.GetRealAttribute("START-X");
        starty = dimen.GetRealAttribute("START-Y");
        startz = dimen.GetRealAttribute("START-Z");
        endx = dimen.GetRealAttribute("END-X");
        endy = dimen.GetRealAttribute("END-Y");
        endz = dimen.GetRealAttribute("END-Z");
        Point3d stpt;
        stpt.X = startx;
        stpt.Y = starty;
        stpt.Z = startz;
        Point3d endpt;
        endpt.X = endx;
        endpt.Y = endy;
        endpt.Z = endz;
        Point realstart = workPart.Points.CreatePoint(stpt);//得到这个PMI矢量的起点
        Point realend = workPart.Points.CreatePoint(endpt);//得到这个PMI矢量的终点
        NXOpen.Features.DatumAxisFeature fiansis = (NXOpen.Features.DatumAxisFeature)shank(realstart, realend);
       // fiansis.DatumAxis;
        DatumAxis fian = fiansis.DatumAxis;
        return fian;
    }
    public double anglemethod(DatumAxis a,DatumAxis b)
{

    Part workPart = theSession.Parts.Work;
    NXObject nullNXObject = null;
    MeasureAngleBuilder bbuilder;
    bbuilder = workPart.MeasureManager.CreateMeasureAngleBuilder(nullNXObject);
    bbuilder.Object1.Value = a;
    bbuilder.Object2.Value = b;
    Unit nullUnit = null;
    MeasureAngle measureAngle1;
    measureAngle1 = workPart.MeasureManager.NewAngle(nullUnit, a, NXOpen.MeasureManager.EndpointType.None, b, NXOpen.MeasureManager.EndpointType.None, true, false);
    double deg = measureAngle1.Value;
    return deg;
}
    public static double ConvertDegreesToRadians(double degrees)//角度到弧度的转换方法
    {
        double radians = (Math.PI / 180) * degrees;
        // return (radians);
        return radians;//return 的这两种写法都是可以的
    }
    public void hideit(NXObject objtohide)//////这是一个隐藏NXObject的方法
    {
        DisplayableObject a = (DisplayableObject)objtohide;
        DisplayableObject[] objects1 = new DisplayableObject[1];
        objects1[0] = a;
        theSession.DisplayManager.BlankObjects(objects1);

    }
    static List<int[]> GetPermutation(int h, int t)
    {
        int[] result = new int[t];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = i;
        }
        List<int[]> resultlist = new List<int[]>();
        resultlist.Add(result);
        while (result != null)
        {
            result = GetNextResult(result, h);
            if (result != null)
            {
                resultlist.Add(result);
            }
        }
        return resultlist;
    }
    static int[] GetNextResult(int[] result, int h)
        {
            int[] nextresult = new int[result.Length];
            result.CopyTo(nextresult, 0);
            bool bAdd = true;
            for (int i = result.Length - 1; i >= 0; i--)
            {

                if (bAdd)
                {
                    
                    nextresult[i] = result[i] + 1;

                    bAdd = false;
                }
                else
                {
                    nextresult[i] = result[i];
                }


                List<int> checkrepeat = new List<int>(nextresult);
                checkrepeat.RemoveAt(i);
                while (checkrepeat.Contains(nextresult[i]))
                {
                    nextresult[i]++;
                }

                if (nextresult[i] >= h)
                {
                    if (i == 0) return null;
                    bAdd = true;
                    nextresult[i] = 0;
                    checkrepeat.RemoveAt(i);
                    while (checkrepeat.Contains(nextresult[i]))
                    {
                        nextresult[i]++;
                    }
                }
            }


            return nextresult;
        }
    public bool checknow(NXOpen.Annotations.Dimension[] finalallpmi)//这个函数判断这个dimension数组是否和要校核的尺寸成环
    {
        for (int i = 0; i < finalallpmi.Length; i++)
        {

            NXOpen.Annotations.Dimension ok = Tag2NXObject<NXOpen.Annotations.Dimension>(finalallpmi[i].Tag);
            finalx = finalx + ok.GetRealAttribute("X");
            finaly = finaly + ok.GetRealAttribute("Y");
            finalz = finalz + ok.GetRealAttribute("Z");
           // dimarylist.Add(ok);

        }
        bool wetx = (Math.Abs(finalx)) <= 0.000000000001;
        bool wety = (Math.Abs(finaly)) <= 0.000000000001;
        bool wetz = (Math.Abs(finalz)) <= 0.000000000001;
        bool puanduan = (wetx && wety && wetz);
        return puanduan;
    }
    public void setappendzeng(NXOpen.Annotations.Dimension zengdim)//true为增环，false为减环,zengdim为要加文本的
    {

        Part workPart = theSession.Parts.Work;
        Part displayPart = theSession.Parts.Display;
        NXOpen.Annotations.DimensionPreferences dimensionPreferences1;
        dimensionPreferences1 = zengdim.GetDimensionPreferences();
        NXOpen.Annotations.AppendedText appendedText1;
        appendedText1 = workPart.Annotations.NewAppendedText();
        string[] lines4 = new string[1];
       
            lines4[0] = "增";
            appendedText1.SetAfterText(lines4);
            zengdim.SetAppendedText(appendedText1);
            appendedText1.Dispose();


    }
    public void setappendjian(NXOpen.Annotations.Dimension zengdim)//true为增环，false为减环,zengdim为要加文本的
    {
      
        Part workPart = theSession.Parts.Work;
        Part displayPart = theSession.Parts.Display;
        NXOpen.Annotations.DimensionPreferences dimensionPreferences1;
        dimensionPreferences1 = zengdim.GetDimensionPreferences();
           NXOpen.Annotations.AppendedText appendedText1;
           appendedText1 = workPart.Annotations.NewAppendedText();
           string[] lines4 = new string[1];

               lines4[0] = "减";
               appendedText1.SetAfterText(lines4);
               zengdim.SetAppendedText(appendedText1);

               appendedText1.Dispose();
           
        

    }
    public int cirdect(NXOpen.Annotations.Dimension cirdick)//这个方法判断一个尺寸对于要校核的是增环还是减环，或者垂直无贡献
    { 
    // -1为减环，0为无贡献，1为增环
        int aa = 0;
        DatumAxis correct = creataxis(cirdick);
        double anglllll = anglemethod(correct, xformone);
        hideit((NXObject)correct);
        if (anglllll > 90)
        {
            
            aa = -1;
        }
        if (anglllll < 90)
        {
         
            aa = 1;

        }
        if (anglllll == 90)
        {

            aa = 0;
        }
        return aa;


    }
    public int apply_cb()
    {
                                    
    
       
        int errorCode = 0;
        try
        {
           //需要校核的尺寸的相关操作
            theoripmi = selection0.GetProperties().GetTaggedObjectVector("SelectedObjects");  //需要校核的尺寸
            NXOpen.Annotations.Dimension theoridim = Tag2NXObject<NXOpen.Annotations.Dimension>(theoripmi[0].Tag);
            xformone = creataxis(theoridim);//需要校核的尺寸所生成的轴
            hideit((NXObject)xformone);//隐藏需要校核的尺寸所生成的轴
            finalx = theoridim.GetRealAttribute("X");
            finaly = theoridim.GetRealAttribute("Y");
            finalz = theoridim.GetRealAttribute("Z");
           if(!selection01.GetProperties().GetLogical("Enable"))
           {
           //下面这段得到一个数组，这个数组里面有除过要校核的尺寸之外所有的尺寸。
            dimary = theSession.Parts.Work.Dimensions.ToArray();
            foreach (NXOpen.Annotations.Dimension a in dimary)
            {
                if (a != theoridim)//如果遍历的尺寸不等于需要校核的尺寸，则把他加入到thedown中，起名字太难了。
                {
                 
                    thedown.Add(a);
                }
            }
           
            left = (NXOpen.Annotations.Dimension[])thedown.ToArray(typeof(NXOpen.Annotations.Dimension));//把动态数组转化成数组
                int[] arr = new int[left.Length];//下面这个for循环定义了一个索引数组，里面存放的是left这个数组的索引。一一对应
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = i;
                }
            for (int t = 2; t <= left.Length; t++)//该循环从2开始，因为一个尺寸连最起码有三个，它也有可能达到left数组的长度。
            {

              lst_Combination = Algorithms.PermutationAndCombination<int>.GetCombination(arr, t);

            }
          
            foreach (int[] a in lst_Combination)//遍历list里面存的索引数组
            {

                for (int j = 0; j < a.Length; j++)//下面这个for循环从索引得到对应的dimension数组。
                {
                    theday.Add(left[j]);
                   // thefinalori[j] = left[j];//得到索引所表示的数组
                    
                }
                thefinalori = (NXOpen.Annotations.Dimension[])theday.ToArray(typeof(NXOpen.Annotations.Dimension));
                if (checknow(thefinalori))//如果成环的话。。。。almost there
                { 
                foreach(NXOpen.Annotations.Dimension g in thefinalori)
                {
                    int qq = 0;
                 qq = cirdect(g);
                 if (qq == -1)
                 {
                     deccir.Add(g);
                 }
                 if (qq == 1)
                 {
                     addcir.Add(g);
                 }
                 if (qq == 0)
                 { 
                 
                 
                 }
                }
                
                };
            
            }
           }
            else
           {
             theallpmi = selection01.GetProperties().GetTaggedObjectVector("SelectedObjects");//这是一种特殊情况，假设用户已经指定了这些尺寸。
          //   NXOpen.Annotations.Dimension[] nonamedim = null;
             ArrayList noname = new ArrayList();
             for (int u = 0; u < theallpmi.Length; u++)
             {
                 noname.Add(Tag2NXObject<NXOpen.Annotations.Dimension>(theallpmi[u].Tag));
              // nonamedim[u] = Tag2NXObject<NXOpen.Annotations.Dimension>(theallpmi[0].Tag);
             }
             NXOpen.Annotations.Dimension[] nonamedim = (NXOpen.Annotations.Dimension[])noname.ToArray(typeof(NXOpen.Annotations.Dimension));
                 if (checknow(nonamedim))//或 ||与 &&非 ！
                 {
                     
                     foreach (NXOpen.Annotations.Dimension k in nonamedim)
                     {
                        int p = 0;
                        p = cirdect(k);

                        if (p == -1)
                        {
                            deccir.Add(k);
                            NXOpen.Annotations.Dimension[] ho = (NXOpen.Annotations.Dimension[])deccir.ToArray(typeof(NXOpen.Annotations.Dimension));
                            foreach (NXOpen.Annotations.Dimension qqq in ho)
                            {
                                //setappendzeng(qqq);
                                setappendjian(qqq);
                            }
                        }
                        if (p == 1)
                        {
                            addcir.Add(k);
                            NXOpen.Annotations.Dimension[] jerry = (NXOpen.Annotations.Dimension[])addcir.ToArray(typeof(NXOpen.Annotations.Dimension));
                            foreach (NXOpen.Annotations.Dimension qq in jerry)
                            {
                                setappendzeng(qq);

                            }
                          //  setappendzeng(k);
                        }
                        if (p == 0)
                        {


                        }
                     }
                   
                   
                 }

            //  tree_control0.InsertColumn(1, "jerry", -1);
           
            //NXOpen.BlockStyler.Node node1 = tree_control0.CreateNode("NodeDisplayText");
            ////  // node1.DisplayText = "成环尺寸";
            ////   node1.ForegroundColor = 198;
         

            ////   tree_control0.InsertNode(node1, null, null, Tree.NodeInsertOption.First);
            ////   // BlockStyler.Node node = tree_control0.CreateNode(“NodeDisplayText”);
            ////   NXOpen.BlockStyler.Node node = tree_control0.CreateNode("NodeData");
            ////    DataContainer nodeData = node.GetNodeData();
            ////    nodeData.AddTaggedObject("Data",(TaggedObject)theoridim );
            ////    nodeData.Dispose();
            ////    tree_control0.InsertNode(node,node1, null, Tree.NodeInsertOption.AlwaysLast);
            ////   // tree_control0.set
     

            }
           
      
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
    //Callback Name: update_cb
    //------------------------------------------------------------------------------
    public int update_cb( NXOpen.BlockStyler.UIBlock block)
    {
        try
        {
            if(block == selection0)
            {
            //---------Enter your code here-----------
            }
            else if(block == toggle0)
            {
           //      toggle0.GetProperties().SetLogical("Value", false);
           //selection01.GetProperties().SetLogical("Enable",false);
                if(toggle0.GetProperties().GetLogical("Value"))
                {
                    selection01.GetProperties().SetLogical("Enable",true);
                }
            //---------Enter your code here-----------
            }
            else if(block == selection01)
            {
            //---------Enter your code here-----------
            }
            else if(block == integer0)
            {
            //---------Enter your code here-----------
            }
            else if(block == label0)
            {
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
    
    //------------------------------------------------------------------------------
    //Callback Name: ok_cb
    //------------------------------------------------------------------------------
    public int ok_cb()
    {
        int errorCode = 0;
        try
        {
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
    //------------------------------------------------------------------------------
    //Treelist specific callbacks
    //------------------------------------------------------------------------------
    //public void OnExpandCallback(NXOpen.BlockStyler.Tree tree, NXOpen.BlockStyler.Node node)
    //{
    //}
    
    //public void OnInsertColumnCallback(NXOpen.BlockStyler.Tree tree, NXOpen.BlockStyler.Node node, int columnID)
    //{
    //}
    
    //public void OnInsertNodecallback(NXOpen.BlockStyler.Tree tree, NXOpen.BlockStyler.Node node)
    //{
    //}
    
    //public void OnDeleteNodecallback(NXOpen.BlockStyler.Tree tree, NXOpen.BlockStyler.Node node)
    //{
    //}
    
    //public void OnPreSelectcallback(NXOpen.BlockStyler.Tree tree, NXOpen.BlockStyler.Node node, int columnID, bool Selected)
    //{
    //}
    
    //public void OnSelectcallback(NXOpen.BlockStyler.Tree tree, NXOpen.BlockStyler.Node node, int columnID, bool Selected)
    //{
    //}
    
    //public void OnStateChangecallback(NXOpen.BlockStyler.Tree tree, NXOpen.BlockStyler.Node node, int State)
    //{
    //}
    
    //public string ToolTipTextcallback(NXOpen.BlockStyler.Tree tree, NXOpen.BlockStyler.Node node, int columnID)
    //{
    //}
    
    //public int ColumnSortcallback(NXOpen.BlockStyler.Tree tree, int columnID, NXOpen.BlockStyler.Node node1, NXOpen.BlockStyler.Node node2)
    //{
    //}
    
    //public string StateIconNameCallback(Tree tree, Node node, int state)
    //{
    //}
    
    //public Tree.BeginLabelEditState OnBeginLabelEditCallback(Tree tree, Node node, int columnID)
    //{
    //}
    
    //public Tree.EndLabelEditState OnEndLabelEditCallback(Tree tree, Node node, int columnID, string editedText)
    //{
    //}
    
    //public Tree.EditControlOption OnEditOptionSelectedCallback(Tree tree, Node node, int columnID, int selectedOptionID, string selectedOptionText, Tree.ControlType type)
    //{
    //}
    
    //public Tree.ControlType AskEditControlCallback(Tree tree, Node node, int columnID)
    //{
    //}
    
    //public void OnMenuCallback(Tree tree, Node node, int columnID)
    //{
    //}
    
    //public void OnMenuSelectionCallback(Tree tree, Node node, int menuItemID)
    //{
    //}
    
    //public Node.DropType IsDropAllowedCallback(Tree tree, Node node, int columnID, Node targetNode, int targetColumnID)
    //{
    //}
    
    //public Node.DragType IsDragAllowedCallback(Tree tree, Node node, int columnID)
    //{
    //}
    
    //public bool OnDropCallback(Tree tree, Node[] node, int columnID, Node targetNode, int targetColumnID, Node.DropType dropType, int dropMenuItemId)
    //{
    //}
    
    //public void OnDropMenuCallback(BlockStyler.Tree tree, BlockStyler.Node node, int columnID, BlockStyler.Node targetNode, int targetColumnID)
    //{
    //}
    
    
}
