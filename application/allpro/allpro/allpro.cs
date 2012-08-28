//==============================================================================
//  WARNING!!  This file is overwritten by the Block UI Styler while generating
//  the automation code. Any modifications to this file will be lost after
//  generating the code again.
//
//       Filename:  C:\Users\Jerry\Desktop\allpro.cs
//
//        This file was generated by the NX Block UI Styler
//        Created by: Jerry
//              Version: NX 7.5
//              Date: 08-23-2012  (Format: mm-dd-yyyy)
//              Time: 16:58 (Format: hh-mm)
//
//==============================================================================

//==============================================================================
//==============================================================================

//------------------------------------------------------------------------------
//These imports are needed for the following template code
//------------------------------------------------------------------------------
using System;
using NXOpen;
using NXOpen.BlockStyler;
using System.IO;
using System.Collections;
using System.Collections.Generic;
//------------------------------------------------------------------------------
//Represents Block Styler application class
//------------------------------------------------------------------------------
public class allpro
{
    //class members
    private static Session theSession = null;
    private static UI theUI = null;
    public static allpro theallpro;
    private string theDialogName;
    private NXOpen.BlockStyler.BlockDialog theDialog;
    private NXOpen.BlockStyler.UIBlock selection0;// Block type: Selection
    private NXOpen.BlockStyler.UIBlock nativeFolderBrowser0;// Block type: NativeFolderBrowser
    private NXOpen.BlockStyler.UIBlock toggle0;// Block type: Toggle
    private NXOpen.BlockStyler.UIBlock selectPart0;// Block type: Select Part
    private NXOpen.BlockStyler.Tree tree_control0;// Block type: Tree Control
    private NXOpen.BlockStyler.UIBlock button0;// Block type: Button
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
    public ArrayList dimary = new ArrayList();//这个动态数组存放所有需要进行校核的尺寸(也包括封闭环尺寸)，很重要。
    public NXOpen.Annotations.Dimension[] dimarydim;//与dimary对应的数组
    public NXOpen.TaggedObject[] theoripmi;//要校核的PMI
    public ArrayList addcir = new ArrayList();//存储增环
    public ArrayList deccir = new ArrayList();//存储减环
    public ArrayList thedown = new ArrayList();//记录整个工序内除过要校核尺寸外的尺寸
    List<int[]> lst_Combination = new List<int[]>();//list用add方法时也要用new分配内存
    double finalx = 0;//对应一个尺寸属性列表中的x分值，下同。
    double finaly = 0;
    double finalz = 0;
    NXOpen.Annotations.Dimension[] thefinalori = null;//不预先设定校核范围时，最后存储尺寸的变量
    DatumAxis xformone = null;//要校核的尺寸生成的轴
    NXOpen.Annotations.Dimension[] left = null;//存放除过要校核的之外的所有尺寸
    public ArrayList theday = new ArrayList();//theday是一个里面存放排列组合后得到的尺寸的数组
    public List<NXOpen.Annotations.Dimension[]> finaloneinpro = new List<NXOpen.Annotations.Dimension[]>();//校核的时候存放和要校核的尺寸成环的其他所有尺寸的list
    //------------------------------------------------------------------------------
    //Constructor for NX Styler class
    //------------------------------------------------------------------------------
    public allpro()
    {
        try
        {
            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theDialogName = "allpro.dlx";
            theDialog = theUI.CreateDialog(theDialogName);
            theDialog.AddApplyHandler(new NXOpen.BlockStyler.BlockDialog.Apply(apply_cb));
            theDialog.AddOkHandler(new NXOpen.BlockStyler.BlockDialog.Ok(ok_cb));
            theDialog.AddUpdateHandler(new NXOpen.BlockStyler.BlockDialog.Update(update_cb));
            theDialog.AddCancelHandler(new NXOpen.BlockStyler.BlockDialog.Cancel(cancel_cb));
            theDialog.AddInitializeHandler(new NXOpen.BlockStyler.BlockDialog.Initialize(initialize_cb));
            theDialog.AddFocusNotifyHandler(new NXOpen.BlockStyler.BlockDialog.FocusNotify(focusNotify_cb));
            theDialog.AddKeyboardFocusNotifyHandler(new NXOpen.BlockStyler.BlockDialog.KeyboardFocusNotify(keyboardFocusNotify_cb));
            theDialog.AddDialogShownHandler(new NXOpen.BlockStyler.BlockDialog.DialogShown(dialogShown_cb));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
//#if USER_EXIT_OR_MENU
    public static void Main()
    {
        try
        {
            theallpro = new allpro();
            theallpro.Show();
        }
        catch (Exception ex)
        {
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
        finally
        {
            theallpro.Dispose();
        }
    }
//#endif//USER_EXIT_OR_MENU
#if USER_EXIT
     public static int GetUnloadOption(string arg)
    {
        //return System.Convert.ToInt32(Session.LibraryUnloadOption.Explicitly);
         return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);
        // return System.Convert.ToInt32(Session.LibraryUnloadOption.AtTermination);
    }
    
    public static int UnloadLibrary(string arg)
    {
        try
        {
        }
        catch (Exception ex)
        {
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
        return 0;
    }
#endif//USER_EXIT
    
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
    
#if CALLBACK
    //------------------------------------------------------------------------------
    //Method name: Show_allpro
    //------------------------------------------------------------------------------
    public static void Show_allpro()
    {
        try
        {
            theallpro = new allpro();
            theallpro.Show();
        }
        catch (Exception ex)
        {
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
        finally
        {
            theallpro.Dispose();
        }
    }
#endif//CALLBACK
    
    
    //------------------------------------------------------------------------------
    //Callback Name: initialize_cb
    //------------------------------------------------------------------------------
    public void initialize_cb()
    {
        try
        {
            selection0 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("selection0");
            nativeFolderBrowser0 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("nativeFolderBrowser0");
            toggle0 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("toggle0");
            selectPart0 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("selectPart0");
            tree_control0 = (NXOpen.BlockStyler.Tree)theDialog.TopBlock.FindBlock("tree_control0");
            button0 = (NXOpen.BlockStyler.UIBlock)theDialog.TopBlock.FindBlock("button0");
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
        }
        catch (Exception ex)
        {
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
    }
    
    //------------------------------------------------------------------------------
    //Callback Name: dialogShown_cb
    //------------------------------------------------------------------------------
    public void dialogShown_cb()
    {
        try
        {
            selectPart0.GetProperties().SetLogical("Enable", false);
            tree_control0.InsertColumn(1, "尺寸链", 130);//一定有注意不同的回调函数的问题
            tree_control0.InsertColumn(2, "名义尺", 100);
            tree_control0.InsertColumn(3, "上公差", 100);
            tree_control0.InsertColumn(4, "下公差", 100);
            tree_control0.InsertColumn(5, "增/减环", 100);
            tree_control0.InsertColumn(6,"所在部件",100);
            //tree_control0.InsertColumn(7,"具体细节",100);
        }
        catch (Exception ex)
        {
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
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
        }
        catch (Exception ex)
        {
            errorCode = 1;
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
        return errorCode;
    }
    
    //------------------------------------------------------------------------------
    //Callback Name: update_cb
    //------------------------------------------------------------------------------
    public ArrayList getpartlist(string path)////////得到一个文件夹里面所有prt的全路径
    {
        //string[] prtinfo = {"",""};//第一个
        ArrayList prtinfoary = new ArrayList();
        FileInfo[] prtfile;
        DirectoryInfo profolder = new DirectoryInfo(path);
        prtfile = profolder.GetFiles("*.prt");
        foreach (FileInfo eachprt in prtfile)
        {
            ////eachprt.FullName
            prtinfoary.Add(eachprt.FullName);
            //eachprt.Name
            //prtinfo[0] = eachprt.FullName;//第一个是全路径，第二个是文件名
            //prtinfo[1] = eachprt.Name;
            //prtinfoary.Add(prtinfo);
           //
        }
        return prtinfoary;
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
    public int update_cb( NXOpen.BlockStyler.UIBlock block)
    {
        try
        {
            if(block == selection0)
            {
            }
            else if(block == nativeFolderBrowser0)
            {
            }
            else if(block == toggle0)
            {
                if (toggle0.GetProperties().GetLogical("Value"))
                {
                    selectPart0.GetProperties().SetLogical("Enable", true);
                    nativeFolderBrowser0.GetProperties().SetLogical("Enable", false);
                }
                else
                {
                    selectPart0.GetProperties().SetLogical("Enable", false);
                    nativeFolderBrowser0.GetProperties().SetLogical("Enable", true);
                }
            }
            else if(block == selectPart0)
            {
            }
            else if(block == button0)
            {


                theoripmi = selection0.GetProperties().GetTaggedObjectVector("SelectedObjects");  //需要校核的尺寸
                NXOpen.Annotations.Dimension theoridim = Tag2NXObject<NXOpen.Annotations.Dimension>(theoripmi[0].Tag);
                xformone = creataxis(theoridim);//需要校核的尺寸所生成的轴
                hideit((NXObject)xformone);//隐藏需要校核的尺寸所生成的轴
                finalx = theoridim.GetRealAttribute("X");
                finaly = theoridim.GetRealAttribute("Y");
                finalz = theoridim.GetRealAttribute("Z");
                Part thework = theSession.Parts.Display;//当前工作部件的路径
                if (!selectPart0.GetProperties().GetLogical("Enable"))
                {   ArrayList prtnameary = new ArrayList();//存放所有在工艺文件夹下的prt的全路径
                string path = nativeFolderBrowser0.GetProperties().GetString("Path");
                prtnameary = getpartlist(path);//得到所有在工艺文件夹下的prt的全路径

                string workpath = thework.FullPath;

                foreach (NXOpen.Annotations.Dimension workdim in thework.Dimensions)
                {
                    if (workdim != theoridim)
                    {
                        dimary.Add(workdim);
                    }
                }
                foreach (string loadpath in prtnameary)
                {
                    if (loadpath != workpath)
                    {
                        PartLoadStatus loadcondition;
                        Part thetempart;//这个存放每个暂时打开的prt文件，然后收集其中的PMI尺寸
                        thetempart = theSession.Parts.Open(loadpath, out loadcondition);
                        //NXOpen.PartCollection.//一定要解决这个问题

                        foreach (NXOpen.Annotations.Dimension eachdim in thetempart.Dimensions.ToArray())
                        {
                            dimary.Add(eachdim);
                            FileInfo prtname = new FileInfo(loadpath);
                            string realprtname = prtname.Name;
                            eachdim.SetAttribute("所属部件", realprtname);
                        }

                    }
                }
                List<int[]> nene;
                left = (NXOpen.Annotations.Dimension[])dimary.ToArray(typeof(NXOpen.Annotations.Dimension));//把动态数组转化成数组
                int[] arr = new int[left.Length];//下面这个for循环定义了一个索引数组，里面存放的是left这个数组的索引。一一对应
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = i;
                }
                for (int t = 2; t <= left.Length; t++)//该循环从2开始，因为一个尺寸连最起码有三个，它也有可能达到left数组的长度。
                {

                    nene = Algorithms.PermutationAndCombination<int>.GetCombination(arr, t);//nene是每一次循环得到的结果，如果直接用nene参与下一步计算，以前的循环结果就作废了
                    foreach (int[] ne in nene)
                    {
                        lst_Combination.Add(ne);
                    }

                }

                foreach (int[] a in lst_Combination)//遍历list里面存的索引数组
                {
                    theday.Clear();
                    for (int j = 0; j < a.Length; j++)//下面这个for循环从索引得到对应的dimension数组。
                    {

                        theday.Add(left[a[j]]);//这一步有问题。。。。。妈的。------这里一个小错误就让我调试了一早上
                        // thefinalori[j] = left[j];//得到索引所表示的数组

                    }
                    thefinalori = (NXOpen.Annotations.Dimension[])theday.ToArray(typeof(NXOpen.Annotations.Dimension));
                    if (checknow(thefinalori))//如果成环的话。。。。almost there
                    {
                        finaloneinpro.Add(thefinalori);//这个list存放的是所有和需校核尺寸成环的尺寸链，记住其里面的每个元素是一个尺寸数组

                    };

                }
                count ct = new count();
                ArrayList zengzu = new ArrayList();
                ArrayList jianzu = new ArrayList();
                NXOpen.Annotations.Dimension[] zengshuzu = null;
                NXOpen.Annotations.Dimension[] jianshuzu = null;
                NXOpen.BlockStyler.Node finalnode = null;
                foreach (NXOpen.Annotations.Dimension[] ori in finaloneinpro)//这部分终于搞定了。哈哈，很高兴啊。
                {
                    jianzu.Clear();
                    zengzu.Clear();
                    finalnode = tree_control0.CreateNode("成环尺寸链");
                    tree_control0.InsertNode(finalnode, null, null, Tree.NodeInsertOption.Last);
                    foreach (NXOpen.Annotations.Dimension sda in ori)
                    {
                        NXOpen.BlockStyler.Node finalchild = tree_control0.CreateNode("NodeData");
                        DataContainer nodeData = finalchild.GetNodeData();
                        int p = 0;
                        double[] final = { 0, 0, 0 };
                        nodeData.AddTaggedObject("Data", sda);
                        nodeData.Dispose();
                        tree_control0.InsertNode(finalchild, finalnode, null, Tree.NodeInsertOption.Last);
                        p = cirdect(sda);
                        if (p == -1)
                        {

                            finalchild.SetColumnDisplayText(5, "增环");
                            //jianzu.Add(sda);
                            zengzu.Add(sda);
                        }
                        if (p == 1)
                        {
                            finalchild.SetColumnDisplayText(5, "减环");
                            //zengzu.Add(sda);
                            jianzu.Add(sda);
                        }
                        if (p == 0)
                        {
                            finalchild.SetColumnDisplayText(5, "对封闭环无贡献");
                        }
                        final = ct.getspec(sda);
                        finalchild.SetColumnDisplayText(2, final[0].ToString());
                        finalchild.SetColumnDisplayText(3, final[1].ToString());
                        finalchild.SetColumnDisplayText(4, final[2].ToString());
                        Part sdapart = (Part)sda.OwningPart;
                        FileInfo sdafile = new FileInfo(sdapart.FullPath);
                        string prtname = sdafile.Name;
                        finalchild.SetColumnDisplayText(6, prtname);
                    }
                    zengshuzu = (NXOpen.Annotations.Dimension[])zengzu.ToArray(typeof(NXOpen.Annotations.Dimension));
                    jianshuzu = (NXOpen.Annotations.Dimension[])jianzu.ToArray(typeof(NXOpen.Annotations.Dimension));
                    if (ct.countcircle(zengshuzu, jianshuzu, theoridim))
                    {
                        //tree_control0.InsertColumn(1, "尺寸链", 100);//一定有注意不同的回调函数的问题

                        finalnode.SetColumnDisplayText(1, "符合尺寸链规则");
                        finalnode.ForegroundColor = 60;//红色表示未通过尺寸链校核
                    }
                    else
                    {
                        finalnode.SetColumnDisplayText(1, "不符合尺寸链规则");
                        finalnode.ForegroundColor = 198;
                    }

                }
                }
           else
                {
                    
                NXOpen.TaggedObject[] partcol = selectPart0.GetProperties().GetTaggedObjectVector("SelectedObjects");//存放所有选择的部件
                //NXOpen.Part[] realpart = null;
                ArrayList realpart = new ArrayList();
                     //NXOpen.Annotations.Dimension theoridim = Tag2NXObject<NXOpen.Annotations.Dimension>(theoripmi[0].Tag);
                    for(int i = 0;i< partcol.Length;i++)
                    {
                         //realpart[i] = Tag2NXObject<NXOpen.Part>(partcol[i].Tag);//将他们转换成part数组
                        realpart.Add(Tag2NXObject<NXOpen.Part>(partcol[i].Tag));
                    }
                    foreach (NXOpen.Part eachpart in realpart)
                    {
                        foreach (NXOpen.Annotations.Dimension eachdim in eachpart.Dimensions.ToArray())
                        {
                            if (eachdim != theoridim)
                            {
                                dimary.Add(eachdim);
                            }
                        }
                    
                    }
                    List<int[]> nene;
                    left = (NXOpen.Annotations.Dimension[])dimary.ToArray(typeof(NXOpen.Annotations.Dimension));//把动态数组转化成数组
                    int[] arr = new int[left.Length];//下面这个for循环定义了一个索引数组，里面存放的是left这个数组的索引。一一对应
                    for (int i = 0; i < arr.Length; i++)
                    {
                        arr[i] = i;
                    }
                    for (int t = 2; t <= left.Length; t++)//该循环从2开始，因为一个尺寸连最起码有三个，它也有可能达到left数组的长度。
                    {

                        nene = Algorithms.PermutationAndCombination<int>.GetCombination(arr, t);//nene是每一次循环得到的结果，如果直接用nene参与下一步计算，以前的循环结果就作废了
                        foreach (int[] ne in nene)
                        {
                            lst_Combination.Add(ne);
                        }

                    }

                    foreach (int[] a in lst_Combination)//遍历list里面存的索引数组
                    {
                        theday.Clear();
                        for (int j = 0; j < a.Length; j++)//下面这个for循环从索引得到对应的dimension数组。
                        {

                            theday.Add(left[a[j]]);//这一步有问题。。。。。妈的。------这里一个小错误就让我调试了一早上
                            // thefinalori[j] = left[j];//得到索引所表示的数组

                        }
                        thefinalori = (NXOpen.Annotations.Dimension[])theday.ToArray(typeof(NXOpen.Annotations.Dimension));
                        if (checknow(thefinalori))//如果成环的话。。。。almost there
                        {
                            finaloneinpro.Add(thefinalori);//这个list存放的是所有和需校核尺寸成环的尺寸链，记住其里面的每个元素是一个尺寸数组

                        };

                    }
                    count ct = new count();
                    ArrayList zengzu = new ArrayList();
                    ArrayList jianzu = new ArrayList();
                    NXOpen.Annotations.Dimension[] zengshuzu = null;
                    NXOpen.Annotations.Dimension[] jianshuzu = null;
                    NXOpen.BlockStyler.Node finalnode = null;
                    foreach (NXOpen.Annotations.Dimension[] ori in finaloneinpro)//这部分终于搞定了。哈哈，很高兴啊。
                    {
                        jianzu.Clear();
                        zengzu.Clear();
                        finalnode = tree_control0.CreateNode("成环尺寸链");
                        tree_control0.InsertNode(finalnode, null, null, Tree.NodeInsertOption.Last);
                        foreach (NXOpen.Annotations.Dimension sda in ori)
                        {
                            NXOpen.BlockStyler.Node finalchild = tree_control0.CreateNode("NodeData");
                            DataContainer nodeData = finalchild.GetNodeData();
                            int p = 0;
                            double[] final = { 0, 0, 0 };
                            nodeData.AddTaggedObject("Data", sda);
                            nodeData.Dispose();
                            tree_control0.InsertNode(finalchild, finalnode, null, Tree.NodeInsertOption.Last);
                            p = cirdect(sda);
                            if (p == -1)
                            {

                                finalchild.SetColumnDisplayText(5, "增环");
                                //jianzu.Add(sda);
                                zengzu.Add(sda);
                            }
                            if (p == 1)
                            {
                                finalchild.SetColumnDisplayText(5, "减环");
                                //zengzu.Add(sda);
                                jianzu.Add(sda);
                            }
                            if (p == 0)
                            {
                                finalchild.SetColumnDisplayText(5, "对封闭环无贡献");
                            }
                            final = ct.getspec(sda);
                            finalchild.SetColumnDisplayText(2, final[0].ToString());
                            finalchild.SetColumnDisplayText(3, final[1].ToString());
                            finalchild.SetColumnDisplayText(4, final[2].ToString());
                            Part sdapart = (Part)sda.OwningPart;
                            FileInfo sdafile = new FileInfo(sdapart.FullPath);
                            string prtname = sdafile.Name;
                            finalchild.SetColumnDisplayText(6, prtname);
                        }
                        zengshuzu = (NXOpen.Annotations.Dimension[])zengzu.ToArray(typeof(NXOpen.Annotations.Dimension));
                        jianshuzu = (NXOpen.Annotations.Dimension[])jianzu.ToArray(typeof(NXOpen.Annotations.Dimension));
                        if (ct.countcircle(zengshuzu, jianshuzu, theoridim))
                        {
                            //tree_control0.InsertColumn(1, "尺寸链", 100);//一定有注意不同的回调函数的问题

                            finalnode.SetColumnDisplayText(1, "符合尺寸链规则");
                            finalnode.ForegroundColor = 98;//红色表示未通过尺寸链校核
                        }
                        else
                        {
                            finalnode.SetColumnDisplayText(1, "不符合尺寸链规则");
                            finalnode.ForegroundColor = 98;
                        }

                    }


                }

            }
        }
        catch (Exception ex)
        {
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
        }
        catch (Exception ex)
        {
            errorCode = 1;
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
        return errorCode;
    }
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
    public double anglemethod(DatumAxis a, DatumAxis b)
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
        theoripmi = selection0.GetProperties().GetTaggedObjectVector("SelectedObjects");  //需要校核的尺寸
        NXOpen.Annotations.Dimension theoridim = Tag2NXObject<NXOpen.Annotations.Dimension>(theoripmi[0].Tag);
        finalx = theoridim.GetRealAttribute("X");
        finaly = theoridim.GetRealAttribute("Y");
        finalz = theoridim.GetRealAttribute("Z");
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
    public void afandfor(NXOpen.Annotations.Dimension[] a, out NXOpen.Annotations.Dimension[] b, out NXOpen.Annotations.Dimension[] c)//这个函数对一个数组中的增减环进行筛选，注意这个数组中的尺寸应该已经和需要校核的尺寸成环
    {
        ///////////////////其中a是输入进来的需要进行增减环赛选的尺寸数组，b是减环，c是增环
        ArrayList vv = new ArrayList();//存放减环
        ArrayList bb = new ArrayList();//存放增环
        int qq = 0;
        foreach (NXOpen.Annotations.Dimension f in a)
        {
            qq = cirdect(f);
            if (qq == -1)
            {
                vv.Add(f);//加入减环
            }
            if (qq == 1)
            {
                bb.Add(f);//加入增环
            }
            if (qq == 0)
            {


            }

        }
        b = (NXOpen.Annotations.Dimension[])vv.ToArray(typeof(NXOpen.Annotations.Dimension));
        c = (NXOpen.Annotations.Dimension[])bb.ToArray(typeof(NXOpen.Annotations.Dimension));

    }
    //------------------------------------------------------------------------------
    //Callback Name: cancel_cb
    //------------------------------------------------------------------------------
    public int cancel_cb()
    {
        try
        {
        }
        catch (Exception ex)
        {
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
        return 0;
    }
    
    //------------------------------------------------------------------------------
    //Callback Name: focusNotify_cb
    //------------------------------------------------------------------------------
    public void focusNotify_cb(NXOpen.BlockStyler.UIBlock block, bool focus)
    {
        try
        {
        }
        catch (Exception ex)
        {
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
    }
    
    //------------------------------------------------------------------------------
    //Callback Name: keyboardFocusNotify_cb
    //------------------------------------------------------------------------------
    public void keyboardFocusNotify_cb(NXOpen.BlockStyler.UIBlock block, bool focus)
    {
        try
        {
        }
        catch (Exception ex)
        {
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
