using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NXOpen;
using NXOpen.BlockStyler;
using NXOpen.UF;
using System.Reflection;
using System.Runtime.InteropServices;
using TDPPM;
using System.Windows.Forms;

namespace TDPPM
{
    public enum E_WaveType
    {
        LINKED_BODY,
        LINKED_FACE,
        LINKED_CURVE,
        LINKED_POINT,
        LINKED_DATUM,
        LINKED_SKETCH,
    }
    public struct S_WaveInfor
    {
        public string fatherpath;
        public string childpath;
        public string childname;
        public string fathername;
        public NXObject child;
        public NXObject father;
        public E_WaveType wave_type;
        public bool isbreak;
    }

    //DrawingSheet创建时的初始信息，存储其属性里。
    public struct S_SheetAttr
    {
        public string TEMPLET;
        public string GUID;
        public string ISPRINT;
        public string REMARK;
        public string ISCHECKED;
    }
    
    //模型信息 （0，0）工艺模型 （0,M）工序模型  (1,M)辅助模型  (2,M)  余量图
    /// <summary>
    /// 模型信息 （0，0）工艺模型 （0,M）工序模型  (1,M)辅助模型  (2,M)  余量图
    /// </summary>
    public struct S_Model
    {
        public string filename;
        public string description;
        public int a;
        public int b;
    }

    public struct S_Yingdu
    {
        public string gongxuhao;
        public string yingdu;
    }
    public struct S_PDF
    {
        public NXOpen.Drawings.DrawingSheet sheet;
        public string path;
        public bool append;
    }
    public struct S_Sheet
    {
        public string Templet;
        public string prtName;
        public string Remark;
        public string IsPrint;
        public string IsChecked;
        public string guid;
        public int a;
        public int index_num;
        public string SheetName;
    };
    /// <summary>
    /// 存储text内容
    /// </summary>
    public struct S_SheetItem
    {
        public string name;
        public double x0;
        public double y0;
        public double x1;
        public double y1;

        public string text;
        public string font; //chesenes_fs
        public double size;
        public int multiline;
    };
    //子工步
    public struct S_ChildStep
    {
        public string name;
        public string renju;
        public string liangju;
        public string beizhu;
    };
    public struct S_SheetNote
    {
        public double x;
        public double y;
        public string[] text;
        public double CharSpaceFactor;
        public double LineSpaceFactor;
        public string TextFont;
        public double TextSize;
        public double AspectRatio;
        public string flag;
        public NXOpen.Annotations.OriginBuilder.AlignmentPosition pos;
    };
    public struct S_GongYiChengXu
    {
        public string texingfuhao;
        public string gongxuhao;
        public string gongxumingcheng;
        public string shebei;
        public string beizhu;
    }
    public struct S_Gongxu //工装统计表用的
    {
        public string gongxuhao;
        public List<string> jiamoju_list;
        public List<string> renju_list;
        public List<string> liangju_list;
    };

    public struct S_Line_GZTJ //工装统计
    {
        public string gongxuhao;
        public string jiamoju;
        public List<string> chongyong_list1;
        public string renju;
        public List<string> chongyong_list2;
        public string liangju;
        public List<string> chongyong_list3;
    };

    public struct S_GongYiZhuangBei
    {
        public string gongxuhao;
        public string jiamoju;
        public string chongyong_jiamoju;
        public string renju;
        public string chongyong_renju;
        public string liangju;
        public string chongyong_liangju;
    };


    public struct S_Symbol
    {
        public string showName;
        public string realName;
        public string strLength;
    };
    public struct S_StepLine
    {
        public string num;
        public string neirong;
        public string renju;
        public string liangju;
        public string beizhu;
    };
    public struct DimInfor
    {
        public object dim;
        public object dimParent;
        public string guid;
        public string text;
        public string value;
        public string up;
        public string low;
        public string type;
        public string isLabel;
        public int id;
        public bool is_herited;
    }

    class NXFun
    {
        //定义常量
        public const string ProcessXML = "Process_Card.xml";
        public const string SignOffXML = "signoff.xml";
        public const string TemplateXML = "Template.xml";
        public const string SheetTempletXML = "SheetTemplet.xml";
        public const string SBFFile = "3dppm_uds.sbf";
        public const string SymbolXML = "UserDefinedSymbol.xml";
        public const string CAPPAssistantEXE = "CAPP助手.exe";
        public const string RarEXE = "rar.exe";
        public const string HelpDoc = "用户使用手册.pdf";
        public const string EmptyModel = "EmptyModel.prt";
        public const string Macro_SelectAll = "SelectAll";
        public const string Macro_Drafting = "Drafting";
        public const string Macro_Modeling = "Modeling";

        public static NXOpen.Features.Revolve Revolve(Section section, Axis axis)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Features.Feature nullFeatures_Feature = null;
            if (!workPart.Preferences.Modeling.GetHistoryMode())
            {
                throw new Exception("Create or edit of a Feature was recorded in History Mode but playback is in History-Free Mode.");
            }
            NXOpen.Features.RevolveBuilder revolveBuilder1;
            revolveBuilder1 = workPart.Features.CreateRevolveBuilder(nullFeatures_Feature);
            revolveBuilder1.Limits.StartExtend.Value.RightHandSide = "0";
            revolveBuilder1.Limits.EndExtend.Value.RightHandSide = "360";
            revolveBuilder1.Limits.StartExtend.Value.RightHandSide = "0";
            revolveBuilder1.Limits.EndExtend.Value.RightHandSide = "360";
            revolveBuilder1.BooleanOperation.Type = NXOpen.GeometricUtilities.BooleanOperation.BooleanType.Create;
            Body[] targetBodies1 = new Body[1];
            Body nullBody = null;
            targetBodies1[0] = nullBody;
            revolveBuilder1.BooleanOperation.SetTargetBodies(targetBodies1);
            revolveBuilder1.Offset.StartOffset.RightHandSide = "0";
            revolveBuilder1.Offset.EndOffset.RightHandSide = "5";
            revolveBuilder1.Tolerance = 0.001;
            revolveBuilder1.Section = section;
            double[] starthelperpoint1 = new double[3];
            starthelperpoint1[0] = 0.0;
            starthelperpoint1[1] = 0.0;
            starthelperpoint1[2] = 0.0;
            revolveBuilder1.SetStartLimitHelperPoint(starthelperpoint1);
            double[] endhelperpoint1 = new double[3];
            endhelperpoint1[0] = 0.0;
            endhelperpoint1[1] = 0.0;
            endhelperpoint1[2] = 0.0;
            revolveBuilder1.SetEndLimitHelperPoint(endhelperpoint1);
            revolveBuilder1.Section = section;
            revolveBuilder1.Axis = axis;
            revolveBuilder1.FeatureOptions.BodyType = NXOpen.GeometricUtilities.FeatureOptions.BodyStyle.Sheet;
            revolveBuilder1.Section = section;
            revolveBuilder1.ParentFeatureInternal = false;
            NXOpen.Features.Feature feature1;
            feature1 = revolveBuilder1.CommitFeature();
            Expression expression1 = revolveBuilder1.Limits.StartExtend.Value;
            Expression expression2 = revolveBuilder1.Limits.EndExtend.Value;
            revolveBuilder1.Destroy();
            return (NXOpen.Features.Revolve)feature1;
        }
        public static int DeleteObject(NXObject ob)
        {
            Session theSession = Session.GetSession();
            bool notifyOnDelete1;
            notifyOnDelete1 = theSession.Preferences.Modeling.NotifyOnDelete;
            theSession.UpdateManager.ClearErrorList();
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Delete");
            NXObject[] objects1 = new NXObject[1];
            objects1[0] = ob;
            int nErrs1;
            nErrs1 = theSession.UpdateManager.AddToDeleteList(objects1);
            bool notifyOnDelete2;
            notifyOnDelete2 = theSession.Preferences.Modeling.NotifyOnDelete;
            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(markId1);
            theSession.DeleteUndoMark(markId1, "Delete");
            return 0;
        }
        public static int TrimBody(Body body, Body revolve, bool reverse)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Features.TrimBody2 nullFeatures_TrimBody2 = null;
            if (!workPart.Preferences.Modeling.GetHistoryMode())
            {
                throw new Exception("Create or edit of a Feature was recorded in History Mode but playback is in History-Free Mode.");
            }
            NXOpen.Features.TrimBody2Builder trimBody2Builder1;
            trimBody2Builder1 = workPart.Features.CreateTrimBody2Builder(nullFeatures_TrimBody2);
            trimBody2Builder1.Tolerance = 0.001;
            trimBody2Builder1.BooleanTool.ExtrudeRevolveTool.ToolSection.DistanceTolerance = 0.001;
            trimBody2Builder1.BooleanTool.ExtrudeRevolveTool.ToolSection.ChainingTolerance = 0.00095;
            trimBody2Builder1.BooleanTool.ReverseDirection = reverse;
            ScCollector scCollector1;
            scCollector1 = workPart.ScCollectors.CreateCollector();
            Body[] bodies1 = new Body[1];
            bodies1[0] = body;
            BodyDumbRule bodyDumbRule1;
            bodyDumbRule1 = workPart.ScRuleFactory.CreateRuleBodyDumb(bodies1);
            SelectionIntentRule[] rules1 = new SelectionIntentRule[1];
            rules1[0] = bodyDumbRule1;
            scCollector1.ReplaceRules(rules1, false);
            trimBody2Builder1.TargetBodyCollector = scCollector1;
            FaceBodyRule faceBodyRule1;
            faceBodyRule1 = workPart.ScRuleFactory.CreateRuleFaceBody(revolve);
            SelectionIntentRule[] rules2 = new SelectionIntentRule[1];
            rules2[0] = faceBodyRule1;
            trimBody2Builder1.BooleanTool.FacePlaneTool.ToolFaces.FaceCollector.ReplaceRules(rules2, false);
            //trimBody2Builder1.ShowResults();
            NXObject nXObject1;
            nXObject1 = trimBody2Builder1.Commit();
            trimBody2Builder1.Destroy();
            return 0;
        }
        public static void SetDimensionTolerance(NXOpen.Annotations.Dimension dimension, double upper, double lower)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");
            NXOpen.Annotations.Value lowerToleranceMm1;
            lowerToleranceMm1.ItemValue = lower;
            Expression nullExpression = null;
            lowerToleranceMm1.ValueExpression = nullExpression;
            lowerToleranceMm1.ValuePrecision = 3;
            NXOpen.Annotations.Value upperToleranceMm1;
            upperToleranceMm1.ItemValue = upper;
            upperToleranceMm1.ValueExpression = nullExpression;
            upperToleranceMm1.ValuePrecision = 3;
            Object tol = ReflectFun(dimension, "GetTolerance");
            Type type = tol.GetType();
            if (type.Name == "LinearTolerance")
            {
                ReflectFun(tol, "SetLowerToleranceMm", lowerToleranceMm1);
                ReflectFun(tol, "SetUpperToleranceMm", upperToleranceMm1);
            }
            else
            {
                ReflectFun(tol, "SetLowerToleranceDegrees", lowerToleranceMm1);
                ReflectFun(tol, "SetUpperToleranceDegrees", upperToleranceMm1);
            }
            //设置公差类型
            if (upper + lower == 0)
            {
                ReflectSetProperty(tol, "ToleranceType", NXOpen.Annotations.ToleranceType.BilateralOneLine);
            }
            else if (lower == 0)
            {
                ReflectSetProperty(tol, "ToleranceType", NXOpen.Annotations.ToleranceType.UnilateralAbove);
            }
            else if (upper == 0)
            {
                ReflectSetProperty(tol, "ToleranceType", NXOpen.Annotations.ToleranceType.UnilateralBelow);
            }
            else
            {
                ReflectSetProperty(tol, "ToleranceType", NXOpen.Annotations.ToleranceType.UnilateralAbove);
            }
            ReflectFun(dimension, "SetTolerance", tol);

            //设置公差的显示大小
            NXOpen.Annotations.LetteringPreferences letteringPreferences1;
            letteringPreferences1 = dimension.GetLetteringPreferences();
            NXOpen.Annotations.Lettering toleranceText1 = letteringPreferences1.GetDimensionText();
            if (upper + lower != 0)
                toleranceText1.Size = 0.57 * toleranceText1.Size;
            letteringPreferences1.SetToleranceText(toleranceText1);
            dimension.SetLetteringPreferences(letteringPreferences1);
            int nErrs2 = theSession.UpdateManager.DoUpdate(markId1);
        }
        /// <summary>
        /// 反射技术，唯我独尊！！！
        /// </summary>
        /// <param name="ob"></param>
        /// <param name="funName"></param>
        /// <returns></returns>
        public static object ReflectFun(object ob, string funName)
        {
            Type type = ob.GetType();
            MethodInfo mi = type.GetMethod(funName);
            return mi.Invoke(ob, null);
        }
        public static object ReflectFun(object ob, string funName, object parm)
        {
            Type type = ob.GetType();
            MethodInfo mi = type.GetMethod(funName);
            object[] parms = new object[1];
            parms[0] = parm;
            return mi.Invoke(ob, parms);
        }
        /// <summary>
        /// 这个多参的貌似有问题啊！
        /// </summary>
        /// <param name="ob"></param>
        /// <param name="funName"></param>
        /// <param name="parm1"></param>
        /// <param name="parm2"></param>
        /// <param name="parm3"></param>
        public static void ReflectFun(object ob, string funName, object parm1, object parm2, object parm3)
        {
            Type type = ob.GetType();
            MethodInfo mi = type.GetMethod(funName);
            object[] parms = new object[3];
            parms[0] = parm1;
            parms[1] = parm2;
            parms[2] = parm3;
            mi.Invoke(ob, parms);
            //mi.Invoke()
        }
        public static object ReflectGetProperty(object ob, string funName)
        {
            Type type = ob.GetType();
            PropertyInfo pi = type.GetProperty(funName);
            return pi.GetValue(ob, null);
        }
        public static void ReflectSetProperty(object ob, string funName, object parm)
        {
            Type type = ob.GetType();
            PropertyInfo pi = type.GetProperty(funName);
            pi.SetValue(ob, parm, null);
        }
        public static double GetDimensionValue(NXOpen.Annotations.Dimension dimension)
        {
            try
            {
                string[] mainTextLines;
                string[] dualTextLines;
                dimension.GetDimensionText(out mainTextLines, out dualTextLines);
                if (mainTextLines.Length > 0)
                {
                    //这里可能包含特殊字符，从中得到连续数字
                    string num = GetNumberFromString(mainTextLines[0]);
                    return System.Convert.ToDouble(num);
                }
                else
                {
                    return 0;
                }
            }
            catch/* (System.Exception ex)*/
            {
                throw new Exception("读取尺寸值失败，请手动指定尺寸值");
            }

        }
        public static string GetNumberFromString(string str)
        {
            string num = "";
            foreach (char a in str)
            {
                if (isNum(a))
                {
                    if (a == ',')
                    {
                        num += '.';
                    }
                    else
                    {
                        num += a;
                    }
                }
            }
            return num;

        }
        public static bool isNum(char a)
        {
            string num = "0123456789.,-";
            string s = "" + a;
            return num.Contains(s);
        }
        public static void SetViewCurveWidth(NXOpen.Drawings.DraftingView draftingview, DisplayableObject curve, NXOpen.ViewDependentDisplayManager.Width type)
        {
            try
            {
                DisplayableObject[] objects1 = new DisplayableObject[1];
                objects1[0] = curve;
                draftingview.DependentDisplay.ApplyWireframeEdit(NXOpen.ViewDependentDisplayManager.Font.Object, type, objects1);

            }
            catch/*(System.Exception ex)*/
            {
                //   UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);    	
            }
        }
        public static void SetViewEdgesPreference(NXOpen.Drawings.DraftingView draftingview, NXOpen.Preferences.GeneralExtractedEdgesOption option)
        {
            draftingview.Style.General.ExtractedEdges = option;
            draftingview.Commit();
        }
        public static NXOpen.Preferences.GeneralExtractedEdgesOption GetViewEdgesPreference(NXOpen.Drawings.DraftingView draftingview)
        {
            return draftingview.Style.General.ExtractedEdges;
        }
        public static int GetSelectObjects(out NXObject[] obs)
        {
            //得到选中的数量        
            int num = UI.GetUI().SelectionManager.GetNumSelectedObjects();
            //得到选中的东东
            NXObject[] obs1 = new NXObject[num];
            for (int i = 0; i < num; i++)
            {
                obs1[i] = UI.GetUI().SelectionManager.GetSelectedObject(i);
            }
            obs = obs1;
            return num;
        }
        public static NXObject[] GetSelectObjects()
        {
            //得到选中的数量        
            int num = Program.theUI.SelectionManager.GetNumSelectedObjects();
            //得到选中的东东
            NXObject[] obs1 = new NXObject[num];
            for (int i = 0; i < num; i++)
            {
                obs1[i] = Program.theUI.SelectionManager.GetSelectedObject(i);
            }
            return obs1;
        }
        public static bool isArrayEqual(string[] a, string[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i] != b[i])
                    {
                        return false;
                    }
                }
            }
            return true;

        }
        /// <summary>
        /// 利用的win32的动态调用dll功能，来调用NX的dll执行宏命令，
        /// </summary>
        /// <param name="path">宏文件的绝对路径,貌似不能含.marco奇怪了</param>
        public static void PlayMacro(string path)
        {
            try
            {
                DLD myfun = new DLD();
                //得到libugui.dll的路径
                string libugui = NXPath + "UGII\\libugui.dll";
                myfun.LoadDll(libugui);
                if (isNX64)//这里64位和32位的是不一样滴！
                {
                    myfun.LoadFun("?MACRO_playback_from_usertool@@YAXPEBD@Z");
                }
                else
                {
                    myfun.LoadFun("?MACRO_playback_from_usertool@@YAXPBD@Z");
                }
                object[] Parameters = new object[] { (string)path, (IntPtr)(IntPtr.Zero), (int)1 }; // 实参为 0
                Type[] ParameterTypes = new Type[] { typeof(string), typeof(IntPtr), typeof(int) }; // 实参类型为 int
                DLD.ModePass[] themode = new DLD.ModePass[] { DLD.ModePass.ByValue, DLD.ModePass.ByValue, DLD.ModePass.ByValue }; // 传送方式为值传
                Type Type_Return = typeof(int); // 返回类型为 int
                myfun.Invoke(Parameters, ParameterTypes, themode, Type_Return);
                myfun.UnLoadDll();
            }
            catch (System.Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            }
        }
        /// <summary>
        /// 得到NX的路径 如:E:\Program Files\UGS\NX 7.5\
        /// </summary>
        /// <returns></returns>
        public static string NXPath
        {
            get
            {
                string ugraf = Process.GetCurrentProcess().MainModule.FileName;
                return ugraf.Substring(0, ugraf.Length - 14);
            }
        }
        public static string ApplicationPath
        {
            get
            {
                return System.AppDomain.CurrentDomain.BaseDirectory;
            }
        }
        //得到3dppm文件夹的Path,利用的读取dll BaseDirectory 后面是带 \ 的
        public static string TDPPMPath
        {
            get
            {
                string dll = ApplicationPath;
                return dll.Substring(0, dll.Length - 12) + "3DPPM\\";
            }
        }
        public static bool isNX64
        {
            get
            {
                if (System.IntPtr.Size == 8)
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// 调用宏文件，执行取消选定，宏在执行时会另启线程，所以应放在代码结束段，或者至少用Timer延时
        /// </summary>
        public static void RemoveAllSelect()
        {
            //得到宏文件路径
            string marco = TDPPMPath + "RemoveAllSelect";
            PlayMacro(marco);

        }
        public static void SelectAll()
        {
            //得到宏文件路径
            string marco = TDPPMPath + Macro_SelectAll;
            PlayMacro(marco);

        }
        public static void SetObjectWidth(DisplayableObject ob, NXOpen.DisplayableObject.ObjectWidth type)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            DisplayModification displayModification1;
            displayModification1 = theSession.DisplayManager.NewDisplayModification();
            displayModification1.ApplyToAllFaces = false;
            displayModification1.ApplyToOwningParts = false;
            displayModification1.NewWidth = NXOpen.DisplayableObject.ObjectWidth.Thick;
            DisplayableObject[] objects1 = new DisplayableObject[1];
            objects1[0] = ob;
            displayModification1.Apply(objects1);
            displayModification1.Dispose();
        }
        /// <summary>
        /// 检测obj是否在某个视图里
        /// </summary>
        /// <param name="father">视图</param>
        /// <param name="child">obj</param>
        /// <returns>在返回true，不在返回false</returns>
        public static bool isFindInView(NXOpen.View father, NXObject child)
        {
            DisplayableObject[] obs = father.AskVisibleObjects();
            foreach (DisplayableObject ob in obs)
            {
                if (ob.Equals(child))
                {
                    return true;
                }
            }
            return false;

        }
        public static bool isFindInList(string[] strs, string str)
        {
            foreach (string s in strs)
            {
                if (s == str)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool isFindInNXObjects(NXObject[] father, NXObject child)
        {
            foreach (NXObject ob in father)
            {
                if (ob.Equals(child))
                {
                    return true;
                }
            }
            return false;
        }
        public static List<NXOpen.Annotations.Dimension> GetDimensionInCurrentView()
        {
            Part workpart = Session.GetSession().Parts.Work;
            NXOpen.Annotations.Dimension[] dims = workpart.Dimensions.ToArray();
            NXOpen.View view = workpart.Views.WorkView;
            List<NXOpen.Annotations.Dimension> dimsInView = new List<NXOpen.Annotations.Dimension>();
            foreach (NXOpen.Annotations.Dimension dim in dims)
            {
                if (isFindInView(view, dim))
                {
                    dimsInView.Add(dim);
                }
            }
            return dimsInView;
        }
        public static List<NXOpen.Annotations.Dimension> GetDimensionInCurrentView(NXObject[] ObjectInView)
        {
            List<NXOpen.Annotations.Dimension> dimsInView = new List<NXOpen.Annotations.Dimension>();
            foreach (NXOpen.Annotations.Dimension dim in Session.GetSession().Parts.Work.Dimensions)
            {
                if (isFindInNXObjects(ObjectInView, dim))
                {
                    dimsInView.Add(dim);
                }
            }
            return dimsInView;
        }
        public static List<NXOpen.Annotations.BaseFcf> GetBaseFcfInCurrentView(NXObject[] ObjectInView)
        {
            List<NXOpen.Annotations.BaseFcf> FcfsInView = new List<NXOpen.Annotations.BaseFcf>();
            NXOpen.Annotations.Gdt[] gdts = Session.GetSession().Parts.Work.Gdts.ToArray();

            foreach (NXOpen.Annotations.Gdt gdt in gdts)
            {
                if (isFindInNXObjects(ObjectInView, gdt))
                {
                    try
                    {
                        NXOpen.Annotations.BaseFcf fcf = (NXOpen.Annotations.BaseFcf)gdt;
                        FcfsInView.Add(fcf);
                    }
                    catch
                    {

                    }
                }
            }
            return FcfsInView;
        }
        public static DimInfor GetDimInfor(NXOpen.Annotations.Dimension dim)
        {
            NXOpen.Annotations.Dimension dimParent = (NXOpen.Annotations.Dimension)GetInheritParent(dim);
            DimInfor diminfor = new DimInfor();
            diminfor.value = GetDimensionValue(dim).ToString();
            diminfor.dimParent = dimParent;
            diminfor.dim = dim;
            diminfor.type = "尺寸";
            diminfor.guid = GetStringAttr(dimParent, "GUID");
            if (string.IsNullOrEmpty(diminfor.guid))
            {
                diminfor.guid = Guid.NewGuid().ToString();
                SetStringAttr(dimParent, "GUID", diminfor.guid);
            }
            diminfor.isLabel = GetStringAttr(dimParent, "DISP");
            if (diminfor.isLabel == "否")
            {
                diminfor.id = 1001;
            }
            else
            {
                diminfor.isLabel = "是";
                SetStringAttr(dimParent, "DISP", "是");
                int id = GetIntegerAttr(dimParent, "ID");
                if (id != -1)
                {
                    diminfor.id = id;
                }
                else
                {
                    diminfor.id = 1000;
                }
            }
            diminfor.text = GetChineseName(dim);
            string type = dim.ToString();
            if (type.Contains("Pmi"))
            {
                diminfor.is_herited = true;
            }
            else
            {
                diminfor.is_herited = false;
            }
            double up, low;
            GetTolerance(dim, out up, out low);
            diminfor.up = up.ToString();
            diminfor.low = low.ToString();
            return diminfor;
        }
        public static DimInfor GetFcfInfor(NXOpen.Annotations.BaseFcf fcf)
        {
            NXOpen.Annotations.BaseFcf dimParent = (NXOpen.Annotations.BaseFcf)GetInheritParent(fcf);
            DimInfor diminfor = new DimInfor();
            diminfor.value = "value";
            diminfor.dimParent = dimParent;
            diminfor.dim = fcf;
            diminfor.type = "形位";
            diminfor.guid = GetStringAttr(dimParent, "GUID");
            if (string.IsNullOrEmpty(diminfor.guid))
            {
                diminfor.guid = Guid.NewGuid().ToString();
                SetStringAttr(dimParent, "GUID", diminfor.guid);
            }
            diminfor.isLabel = GetStringAttr(dimParent, "DISP");
            if (diminfor.isLabel == "否")
            {
                diminfor.id = 1001;
            }
            else
            {
                diminfor.isLabel = "是";
                SetStringAttr(dimParent, "DISP", "是");
                int id = GetIntegerAttr(dimParent, "ID");
                if (id != -1)
                {
                    diminfor.id = id;
                }
                else
                {
                    diminfor.id = 1000;
                }
            }
            //这里是有问题的
            diminfor.text = GetChineseName(fcf);
            string type = fcf.ToString();
            if (type.Contains("Drafting"))
            {
                diminfor.is_herited = false;
            }
            else
            {
                diminfor.is_herited = true;
            }
            return diminfor;
        }
        public static string GetStringAttr(NXObject obj, string title)
        {
            try
            {
                string attr = obj.GetStringAttribute(title);
                return attr;
            }
            catch/* (System.Exception ex)*/
            {
                return "";
            }
        }
        public static int GetIntegerAttr(NXObject obj, string title)
        {
            try
            {
                int attr = obj.GetIntegerAttribute(title);
                return attr;
            }
            catch/* (System.Exception ex)*/
            {
                return -1;
            }
        }
        public static void SetStringAttr(NXObject obj, string title, string attr)
        {
            try
            {
                if (obj == null)
                {
                    return;
                }
                obj.SetAttribute(title, attr);
            }
            catch
            {

            }
        }
        public static void SetIntegerAttr(NXObject obj, string title, int attr)
        {
            try
            {
                if (obj == null)
                {
                    return;
                }
                obj.SetAttribute(title, attr);
            }
            catch
            {

            }
        }
        public static string GetChineseName(object obj)
        {
            string english = obj.GetType().Name;
            //查询字典 Dictionary.xml
            string chinese = english;
            return chinese;
        }
        public static void GetTolerance(NXOpen.Annotations.Dimension dim, out double up, out double low)
        {
            //目前未考虑精度
            try
            {
                up = 0;
                low = 0;
                Object tol = ReflectFun(dim, "GetTolerance");
                Type type = tol.GetType();
                NXOpen.Annotations.Value v_up = new NXOpen.Annotations.Value();
                NXOpen.Annotations.Value v_low = new NXOpen.Annotations.Value();
                if (type.Name == "LinearTolerance")
                {
                    v_low = (NXOpen.Annotations.Value)ReflectFun(tol, "GetLowerToleranceMm");
                    v_up = (NXOpen.Annotations.Value)ReflectFun(tol, "GetUpperToleranceMm");
                }
                else
                {
                    v_low = (NXOpen.Annotations.Value)ReflectFun(tol, "GetLowerToleranceDegrees");
                    v_up = (NXOpen.Annotations.Value)ReflectFun(tol, "GetUpperToleranceDegrees");
                }
                up = v_up.ItemValue;
                low = v_low.ItemValue;
            }
            catch (System.Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Warning, ex.Message);
                up = 0;
                low = 0;
            }
        }
        public static void SetHighlight(object obj, bool isHightlight)
        {
            try
            {
                if (obj == null)
                {
                    return;
                }
                NXOpen.UF.UFSession theUFSession = NXOpen.UF.UFSession.GetUFSession();
                NXOpen.DisplayableObject ob = (NXOpen.DisplayableObject)obj;
                if (isHightlight)
                {
                    theUFSession.Disp.SetHighlight(ob.Tag, 1);
                    // ob.Highlight();
                }
                else
                {
                    theUFSession.Disp.SetHighlight(ob.Tag, 0);
                    //  ob.Unhighlight();
                }
            }
            catch/* (System.Exception ex)*/
            {

            }
        }
        public static void DeleteIdSymbolByAttr(string attr_name, string attr_value)
        {
            try
            {
                NXOpen.Annotations.IdSymbolCollection idsc = Session.GetSession().Parts.Work.Annotations.IdSymbols;
                foreach (NXOpen.Annotations.IdSymbol ids in idsc)
                {
                    if (GetStringAttr(ids, attr_name) == attr_value)
                    {
                        DeleteObject(ids);
                    }
                }
            }
            catch (System.Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            }
        }
        public static void DeleteBalloonNoteByAttr(string attr_name, string attr_value)
        {
            try
            {
                NXOpen.Annotations.PmiAttributeCollection pac = Session.GetSession().Parts.Work.PmiManager.PmiAttributes;
                foreach (NXOpen.Annotations.PmiAttribute pa in pac)
                {
                    if (GetStringAttr(pa, attr_name) == attr_value)
                    {
                        DeleteObject(pa);
                    }
                }
            }
            catch (System.Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            }
        }
        public static void SelectAnnotation(NXOpen.Annotations.Annotation anno, bool add)
        {
            if (!add)
            {
                ClickPoint(anno.AnnotationOrigin);
            }
        }
        public static void ClickPoint(Point3d point)
        {
            //重写宏文件
            string marco = TDPPMPath + "SingleSelect";
            string file = marco + ".macro";
            string newLineValue = "CPOS " + point.X.ToString() + "," + point.Y.ToString() + "," + point.Z.ToString();
            string newfile =
                @"NX 7.5.0.32" + "\r\n" +
                @"Macro File: C:\sw.macro" + "\r\n" +
                @"Macro Version 7.50" + "\r\n" +
                @"Macro List Language and Codeset: simpl_chinese 13" + "\r\n" +
                @"Created by ZWL on Sun Aug 29 15:29:16 2010" + "\r\n" +
                @"Part Name Display Style: $FILENAME" + "\r\n" +
                @"Selection Parameters 1 2 0.305704 1" + "\r\n" +
                @"Display Parameters 1.000000 12.706939 7.327559 -1.000000 -0.576658 1.000000 0.576658" + "\r\n" +
                @"*****************" + "\r\n" +
                @"RESET" + "\r\n" +
                @"CURSOR_EVENT 1001 3,1,100 ! single_pt, mb1/0+0, U_Sel_sngl (P+:0+0)" + "\r\n" +
                newLineValue;

            FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(newfile);
            sw.Close();
            // 
            //                 sw.Flush();  

            //  EditFile(12, newLineValue, file);
            PlayMacro(marco);
        }
        public static void EditFile(int curLine, string newLineValue, string patch)
        {
            FileStream fs = new FileStream(patch, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string line = sr.ReadLine();
            StringBuilder sb = new StringBuilder();
            for (int i = 1; line != null; i++)
            {
                sb.Append(line + "\r\n");
                if (i != curLine - 1)
                    line = sr.ReadLine();
                else
                {
                    sr.ReadLine();
                    line = newLineValue;
                }
            }
            sr.Close();
            fs.Close();
            FileStream fs1 = new FileStream(patch, FileMode.Open, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs1);
            sw.Write(sb.ToString());
            sw.Close();
            fs.Close();
        }
        public static void AddIdSymbol(NXOpen.Annotations.Annotation anno, string id, DimInfor listitem)
        {
            try
            {
                if (listitem.id.ToString().Length == 0)
                    return;
                Session theSession = Session.GetSession();
                Part workPart = theSession.Parts.Work;
                Part displayPart = theSession.Parts.Display;
                NXOpen.Annotations.IdSymbol nullAnnotations_IdSymbol = null;
                NXOpen.Annotations.IdSymbolBuilder idSymbolBuilder1;
                idSymbolBuilder1 = workPart.Annotations.IdSymbols.CreateIdSymbolBuilder(nullAnnotations_IdSymbol);
                idSymbolBuilder1.Origin.Plane.PlaneMethod = NXOpen.Annotations.PlaneBuilder.PlaneMethodType.XyPlane;
                idSymbolBuilder1.Origin.SetInferRelativeToGeometry(true);
                idSymbolBuilder1.Origin.Anchor = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;
                idSymbolBuilder1.UpperText = id;
                idSymbolBuilder1.Origin.Plane.PlaneMethod = NXOpen.Annotations.PlaneBuilder.PlaneMethodType.XyPlane;
                idSymbolBuilder1.Origin.SetInferRelativeToGeometry(true);
                NXOpen.Annotations.LeaderData leaderData1;
                leaderData1 = workPart.Annotations.CreateLeaderData();
                leaderData1.Arrowhead = NXOpen.Annotations.LeaderData.ArrowheadType.FilledArrow;
                idSymbolBuilder1.Leader.Leaders.Append(leaderData1);
                leaderData1.StubSide = NXOpen.Annotations.LeaderSide.Inferred;
                idSymbolBuilder1.Origin.SetInferRelativeToGeometry(true);
                idSymbolBuilder1.Origin.SetInferRelativeToGeometry(true);
                idSymbolBuilder1.Style.LetteringStyle.HorizontalTextJustification = NXOpen.Annotations.TextJustification.Center;
                idSymbolBuilder1.Style.LetteringStyle.StackVerticalAlignment = NXOpen.Annotations.StackVerticalAlignment.Center;
                idSymbolBuilder1.Style.LetteringStyle.StackHorizontalAlignment = NXOpen.Annotations.StackHorizontalAlignment.Middle;
                //计算宽高比
                double DimensionSize = anno.GetLetteringPreferences().GetDimensionText().Size;
                double AspectRatio = 1;
                switch (listitem.id.ToString().Length)
                {
                    case 0:
                        AspectRatio = 1;
                        break;
                    case 1:
                        AspectRatio = 0.7;
                        break;
                    case 2:
                        AspectRatio = 0.6;
                        break;
                    case 3:
                        AspectRatio = 0.4;
                        break;
                    default:
                        AspectRatio = 1.2 / listitem.id.ToString().Length;
                        break;
                }
                idSymbolBuilder1.Style.LetteringStyle.GeneralTextAspectRatio = AspectRatio;
                idSymbolBuilder1.Style.LetteringStyle.GeneralTextFont = workPart.Fonts.AddFont("cadds4");
                idSymbolBuilder1.Style.LetteringStyle.GeneralTextSize = DimensionSize;
                idSymbolBuilder1.Size = DimensionSize * 6 / 3.5;
                NXOpen.Annotations.Annotation.AssociativeOriginData assocOrigin1;
                assocOrigin1.OriginType = NXOpen.Annotations.AssociativeOriginType.AttachedToStack;
                NXOpen.View nullView = null;
                assocOrigin1.View = nullView;
                assocOrigin1.ViewOfGeometry = nullView;
                Point nullPoint = null;
                assocOrigin1.PointOnGeometry = nullPoint;
                assocOrigin1.VertAnnotation = null;
                assocOrigin1.VertAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
                assocOrigin1.HorizAnnotation = null;
                assocOrigin1.HorizAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
                assocOrigin1.AlignedAnnotation = null;
                assocOrigin1.DimensionLine = 0;
                assocOrigin1.AssociatedView = nullView;
                assocOrigin1.AssociatedPoint = nullPoint;
                assocOrigin1.OffsetAnnotation = anno;
                assocOrigin1.OffsetAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
                assocOrigin1.XOffsetFactor = 0.0;
                assocOrigin1.YOffsetFactor = 0.0;
                assocOrigin1.StackAlignmentPosition = NXOpen.Annotations.StackAlignmentPosition.Right;
                idSymbolBuilder1.Origin.SetAssociativeOrigin(assocOrigin1);
                Point3d point1 = new Point3d(99.8820875166223, 167.7571875, 0.0);
                idSymbolBuilder1.Origin.Origin.SetValue(null, nullView, point1);
                idSymbolBuilder1.Origin.SetInferRelativeToGeometry(true);
                NXObject nXObject1;
                nXObject1 = idSymbolBuilder1.Commit();
                SetStringAttr(nXObject1, "GUID", listitem.guid);
                SetStringAttr((NXObject)listitem.dimParent, "DISP", listitem.isLabel);
                SetIntegerAttr((NXObject)listitem.dimParent, "ID", listitem.id);
                ((NXOpen.Annotations.IdSymbol)nXObject1).InsertIntoStack(anno, NXOpen.Annotations.StackAlignmentPosition.Right);
                idSymbolBuilder1.Destroy();
            }
            catch (System.Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            }

        }
        public static T FindObjectByAttr<T>(string attr_name, string attr_value)
        {
            try
            {
                Type type = typeof(T);
                string name = type.Name;
                if (name == "IdSymbol")
                {
                    NXOpen.Annotations.IdSymbol[] ids = Session.GetSession().Parts.Work.Annotations.IdSymbols.ToArray();
                    foreach (NXOpen.Annotations.IdSymbol id in ids)
                    {
                        if (GetStringAttr(id, attr_name) == attr_value)
                        {
                            return (T)(object)id;
                        }
                    }
                }
                else if (name == "BalloonNote")
                {
                    NXOpen.Annotations.PmiAttribute[] bns = Session.GetSession().Parts.Work.PmiManager.PmiAttributes.ToArray();
                    foreach (NXOpen.Annotations.PmiAttribute bn in bns)
                    {
                        if (GetStringAttr(bn, attr_name) == attr_value)
                        {
                            return (T)(object)bn;
                        }
                    }
                }
                return default(T);
            }
            catch (System.Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
                return default(T);
            }
        }
        public static void EditIdSymbol(NXOpen.Annotations.IdSymbol idsymbol, string num, DimInfor listitem)
        {
            //计算宽高比
            double AspectRatio = 1;
            switch (listitem.id.ToString().Length)
            {
                case 0:
                    AspectRatio = 1;
                    break;
                case 1:
                    AspectRatio = 0.7;
                    break;
                case 2:
                    AspectRatio = 0.6;
                    break;
                case 3:
                    AspectRatio = 0.4;
                    break;
                default:
                    AspectRatio = 1.2 / listitem.id.ToString().Length;
                    break;
            }
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Annotations.IdSymbolBuilder idSymbolBuilder1;
            idSymbolBuilder1 = workPart.Annotations.IdSymbols.CreateIdSymbolBuilder(idsymbol);
            idSymbolBuilder1.Origin.Plane.PlaneMethod = NXOpen.Annotations.PlaneBuilder.PlaneMethodType.UserDefined;
            idSymbolBuilder1.Origin.SetInferRelativeToGeometry(true);
            idSymbolBuilder1.Origin.Plane.PlaneMethod = NXOpen.Annotations.PlaneBuilder.PlaneMethodType.UserDefined;
            idSymbolBuilder1.Origin.SetInferRelativeToGeometry(true);
            NXOpen.Annotations.LeaderData leaderData1;
            leaderData1 = workPart.Annotations.CreateLeaderData();
            leaderData1.Arrowhead = NXOpen.Annotations.LeaderData.ArrowheadType.FilledArrow;
            idSymbolBuilder1.Leader.Leaders.Append(leaderData1);
            leaderData1.StubSide = NXOpen.Annotations.LeaderSide.Inferred;
            idSymbolBuilder1.Origin.SetInferRelativeToGeometry(true);
            idSymbolBuilder1.Origin.SetInferRelativeToGeometry(true);
            idSymbolBuilder1.UpperText = num;
            idSymbolBuilder1.Style.LetteringStyle.GeneralTextAspectRatio = AspectRatio;
            NXObject nXObject1;
            nXObject1 = idSymbolBuilder1.Commit();
            idSymbolBuilder1.Destroy();
        }
        public static NXOpen.Annotations.BalloonNote AddBalloonNote(NXOpen.Annotations.Annotation anno, string num, DimInfor listitem)
        {
            try
            {
                //计算宽高比
                double DimensionSize = ((NXOpen.Annotations.Annotation)(listitem.dim)).GetLetteringPreferences().GetDimensionText().Size;
                double AspectRatio = 1;
                switch (listitem.id.ToString().Length)
                {
                    case 0:
                        AspectRatio = 1;
                        break;
                    case 1:
                        AspectRatio = 0.7;
                        break;
                    case 2:
                        AspectRatio = 0.6;
                        break;
                    case 3:
                        AspectRatio = 0.4;
                        break;
                    default:
                        AspectRatio = 1.2 / listitem.id.ToString().Length;
                        break;
                }
                Session theSession = Session.GetSession();
                Part workPart = theSession.Parts.Work;
                Part displayPart = theSession.Parts.Display;
                NXOpen.Annotations.BalloonNote nullAnnotations_BalloonNote = null;
                NXOpen.Annotations.BalloonNoteBuilder balloonNoteBuilder1;
                balloonNoteBuilder1 = workPart.PmiManager.PmiAttributes.CreateBalloonNoteBuilder(nullAnnotations_BalloonNote);
                balloonNoteBuilder1.Origin.Plane.PlaneMethod = NXOpen.Annotations.PlaneBuilder.PlaneMethodType.XyPlane;
                balloonNoteBuilder1.Origin.SetInferRelativeToGeometry(true);
                balloonNoteBuilder1.Origin.Anchor = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;
                balloonNoteBuilder1.Title = "Balloon Note";
                balloonNoteBuilder1.Category = "User Defined";
                balloonNoteBuilder1.Identifier = "User Defined";
                balloonNoteBuilder1.Revision = "-";
                balloonNoteBuilder1.BalloonText = num;
                String[] text1 = new String[1];
                text1[0] = "";
                balloonNoteBuilder1.SetText(text1);
                balloonNoteBuilder1.Origin.Plane.PlaneMethod = NXOpen.Annotations.PlaneBuilder.PlaneMethodType.ModelView;
                balloonNoteBuilder1.Origin.SetInferRelativeToGeometry(true);
                NXOpen.Annotations.LeaderData leaderData1;
                leaderData1 = workPart.Annotations.CreateLeaderData();
                leaderData1.Arrowhead = NXOpen.Annotations.LeaderData.ArrowheadType.FilledArrow;
                balloonNoteBuilder1.Leader.Leaders.Append(leaderData1);
                leaderData1.StubSide = NXOpen.Annotations.LeaderSide.Inferred;
                String[] text2 = new String[1];
                text2[0] = "";
                balloonNoteBuilder1.SetText(text2);
                balloonNoteBuilder1.Origin.SetInferRelativeToGeometry(true);
                balloonNoteBuilder1.Origin.SetInferRelativeToGeometry(true);
                balloonNoteBuilder1.Style.LetteringStyle.HorizontalTextJustification = NXOpen.Annotations.TextJustification.Center;
                balloonNoteBuilder1.Style.LetteringStyle.StackVerticalAlignment = NXOpen.Annotations.StackVerticalAlignment.Center;
                balloonNoteBuilder1.Style.LetteringStyle.StackHorizontalAlignment = NXOpen.Annotations.StackHorizontalAlignment.Middle;
                balloonNoteBuilder1.Style.LetteringStyle.GeneralTextFont = workPart.Fonts.AddFont("cadds4");
                balloonNoteBuilder1.Style.LetteringStyle.GeneralTextAspectRatio = AspectRatio;
                balloonNoteBuilder1.Style.LetteringStyle.GeneralTextSize = DimensionSize;
                balloonNoteBuilder1.Scale = 0.6;
                NXOpen.Annotations.Annotation.AssociativeOriginData assocOrigin1;
                //balloonNoteBuilder1.Style.s
                assocOrigin1.OriginType = NXOpen.Annotations.AssociativeOriginType.AttachedToStack;
                NXOpen.View nullView = null;
                assocOrigin1.View = nullView;
                assocOrigin1.ViewOfGeometry = nullView;
                Point nullPoint = null;
                assocOrigin1.PointOnGeometry = nullPoint;
                assocOrigin1.VertAnnotation = null;
                assocOrigin1.VertAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
                assocOrigin1.HorizAnnotation = null;
                assocOrigin1.HorizAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
                assocOrigin1.AlignedAnnotation = null;
                assocOrigin1.DimensionLine = 0;
                assocOrigin1.AssociatedView = nullView;
                assocOrigin1.AssociatedPoint = nullPoint;
                assocOrigin1.OffsetAnnotation = (NXOpen.Annotations.Annotation)listitem.dimParent;
                assocOrigin1.OffsetAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
                assocOrigin1.XOffsetFactor = 0.0;
                assocOrigin1.YOffsetFactor = 0.0;
                assocOrigin1.StackAlignmentPosition = NXOpen.Annotations.StackAlignmentPosition.Right;
                balloonNoteBuilder1.Origin.SetAssociativeOrigin(assocOrigin1);
                Point3d point1 = new Point3d(52.9158709179874, -19.3122227111216, 0.0);
                balloonNoteBuilder1.Origin.Origin.SetValue(null, nullView, point1);
                balloonNoteBuilder1.Origin.SetInferRelativeToGeometry(true);
                NXObject nXObject1;
                nXObject1 = balloonNoteBuilder1.Commit();
                SetStringAttr(nXObject1, "GUID", listitem.guid);
                SetStringAttr((NXObject)listitem.dimParent, "DISP", listitem.isLabel);
                SetIntegerAttr((NXObject)listitem.dimParent, "ID", listitem.id);
                balloonNoteBuilder1.Destroy();
                return (NXOpen.Annotations.BalloonNote)nXObject1;
            }
            catch (System.Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
                return null;
            }
        }
        public static void EditBalloonNoteSize(NXOpen.Annotations.BalloonNote balloonnote, NXOpen.Annotations.Lettering lettering)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Annotations.BalloonNoteBuilder balloonNoteBuilder1;
            balloonNoteBuilder1 = workPart.PmiManager.PmiAttributes.CreateBalloonNoteBuilder(balloonnote);
            balloonNoteBuilder1.Style.LetteringStyle.GeneralTextSize = lettering.Size;
            balloonNoteBuilder1.Style.LetteringStyle.GeneralTextAspectRatio = lettering.AspectRatio;
            balloonNoteBuilder1.Style.LetteringStyle.GeneralTextFont = workPart.Fonts.AddFont("cadds4");
            NXObject nXObject1;
            nXObject1 = balloonNoteBuilder1.Commit();
            balloonNoteBuilder1.Destroy();
        }
        public static void EditBalloonNote(NXOpen.Annotations.BalloonNote balloonnote, string num, DimInfor listitem)
        {
            //计算宽高比
            double DimensionSize = ((NXOpen.Annotations.Annotation)(listitem.dim)).GetLetteringPreferences().GetDimensionText().Size;
            double AspectRatio = 1;
            switch (listitem.id.ToString().Length)
            {
                case 0:
                    AspectRatio = 1;
                    break;
                case 1:
                    AspectRatio = 0.7;
                    break;
                case 2:
                    AspectRatio = 0.6;
                    break;
                case 3:
                    AspectRatio = 0.4;
                    break;
                default:
                    AspectRatio = 1.2 / listitem.id.ToString().Length;
                    break;
            }
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Annotations.BalloonNoteBuilder balloonNoteBuilder1;
            balloonNoteBuilder1 = workPart.PmiManager.PmiAttributes.CreateBalloonNoteBuilder(balloonnote);
            balloonNoteBuilder1.Origin.Plane.PlaneMethod = NXOpen.Annotations.PlaneBuilder.PlaneMethodType.UserDefined;
            balloonNoteBuilder1.Origin.SetInferRelativeToGeometry(true);
            String[] text1 = new String[1];
            text1[0] = "";
            balloonNoteBuilder1.SetText(text1);
            balloonNoteBuilder1.Origin.Plane.PlaneMethod = NXOpen.Annotations.PlaneBuilder.PlaneMethodType.UserDefined;
            balloonNoteBuilder1.Origin.SetInferRelativeToGeometry(true);
            NXOpen.Annotations.LeaderData leaderData1;
            leaderData1 = workPart.Annotations.CreateLeaderData();
            leaderData1.Arrowhead = NXOpen.Annotations.LeaderData.ArrowheadType.FilledArrow;
            balloonNoteBuilder1.Leader.Leaders.Append(leaderData1);
            leaderData1.StubSide = NXOpen.Annotations.LeaderSide.Inferred;
            balloonNoteBuilder1.Origin.SetInferRelativeToGeometry(true);
            balloonNoteBuilder1.Origin.SetInferRelativeToGeometry(true);
            balloonNoteBuilder1.BalloonText = num;
            balloonNoteBuilder1.Style.LetteringStyle.GeneralTextAspectRatio = AspectRatio;
            balloonNoteBuilder1.Style.LetteringStyle.GeneralTextSize = DimensionSize;
            NXObject nXObject1;
            nXObject1 = balloonNoteBuilder1.Commit();
            balloonNoteBuilder1.Destroy();
        }
        /// <summary>
        /// 设置显示类型
        /// </summary>
        /// <param name="state">1 = Modeling View 2 = Drawing View</param>
        public static void SetDisplayState(int state)
        {

            NXOpen.UF.UFSession theUFSession = NXOpen.UF.UFSession.GetUFSession();
            theUFSession.Draw.SetDisplayState(state);
        }
        public static void Swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }
        public static NXOpen.Annotations.Annotation GetInheritParent(NXOpen.Annotations.Annotation pmi)
        {
            NXOpen.Annotations.PmiManager pm = Session.GetSession().Parts.Work.PmiManager;
            if (pm.IsInheritedPmi(pmi))
            {
                return pm.GetInheritParent(pmi);
            }
            else
            {
                return pmi;
            }
        }
        /// <summary>
        /// 创建图纸页
        /// </summary>
        /// <param name="sheetname">图纸页名称</param>
        /// <param name="template">图纸模板全路径</param>
        /// <param name="sheetattr">图纸属性</param>
        /// <returns>图纸页</returns>
        public static NXOpen.Drawings.DrawingSheet CreateSheet(string sheetname, string template, S_SheetAttr sheetattr)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            NXOpen.Drawings.DrawingSheet nullDrawings_DrawingSheet = null;
            NXOpen.Drawings.DrawingSheetBuilder drawingSheetBuilder1;
            drawingSheetBuilder1 = workPart.DrawingSheets.DrawingSheetBuilder(nullDrawings_DrawingSheet);
            drawingSheetBuilder1.AutoStartViewCreation = true;
            drawingSheetBuilder1.StandardMetricScale = NXOpen.Drawings.DrawingSheetBuilder.SheetStandardMetricScale.S11;
            drawingSheetBuilder1.StandardEnglishScale = NXOpen.Drawings.DrawingSheetBuilder.SheetStandardEnglishScale.S11;
            drawingSheetBuilder1.EnglishSheetTemplateLocation = "";
            drawingSheetBuilder1.StandardMetricScale = NXOpen.Drawings.DrawingSheetBuilder.SheetStandardMetricScale.S11;
            drawingSheetBuilder1.StandardEnglishScale = NXOpen.Drawings.DrawingSheetBuilder.SheetStandardEnglishScale.S11;
            drawingSheetBuilder1.ScaleNumerator = 1.0;
            drawingSheetBuilder1.ScaleDenominator = 1.0;
            drawingSheetBuilder1.Units = NXOpen.Drawings.DrawingSheetBuilder.SheetUnits.Metric;
            drawingSheetBuilder1.ProjectionAngle = NXOpen.Drawings.DrawingSheetBuilder.SheetProjectionAngle.First;
            drawingSheetBuilder1.MetricSheetTemplateLocation = template;
            drawingSheetBuilder1.Name = sheetname;
            NXObject nXObject1;
            nXObject1 = drawingSheetBuilder1.Commit();
            //设置属性
            SetStringAttr(nXObject1, "GUID", sheetattr.GUID);
            SetStringAttr(nXObject1, "ISCHECKED", sheetattr.ISCHECKED);
            SetStringAttr(nXObject1, "ISPRINT", sheetattr.ISPRINT);
            SetStringAttr(nXObject1, "REMARK", sheetattr.REMARK);
            SetStringAttr(nXObject1, "TEMPLET", sheetattr.TEMPLET);
            drawingSheetBuilder1.Destroy();
            return (NXOpen.Drawings.DrawingSheet)nXObject1;
        }
        /// <summary>
        /// 通过当前显示的模型得到工艺xml路径
        /// </summary>
        /// <returns>xml路径，未得到返回空</returns>
        public static string GetProcessXMLByDisplayPrt()
        {
            try
            {
                string str = "";
                //得到当前显示的模型路径
                str = Session.GetSession().Parts.Display.FullPath;
                //得到同级目录下的Process_Card.xml
                str = str.Substring(0, str.LastIndexOf("\\")) + "\\" + ProcessXML;
                //判断文件是否存在
                if (isFileExist(str))
                {
                    return str;
                }
                else
                {
                    return "";
                }
            }
            catch/* (System.Exception ex)*/
            {
                return "";
            }

        }
        /// <summary>
        /// 后台打开指定路径的prt
        /// </summary>
        /// <param name="fullpath">prt路径</param>
        /// <returns>prt</returns>
        public static Part OpenPrtQuiet(string fullpath)
        {
            try
            {
                if (Program.theUfSession.Part.IsLoaded(fullpath) == 0)
                {
                    //没有打开
                    PartLoadStatus status;
                    Part part = Session.GetSession().Parts.Open(fullpath, out status);
                    return part;
                }
                else
                {
                    //打开状态
                    Tag tag = Program.theUfSession.Part.AskPartTag(fullpath);
                    if (tag == 0)
                    {
                        throw new Exception("无法载入" + fullpath);
                        //return null;
                    }
                    else
                    {
                        return Tag2NXObject<Part>(tag);
                    }
                }
            }
            catch (System.Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Tag到NXObject的转换
        /// </summary>
        /// <typeparam name="T">NXObject的类型</typeparam>
        /// <param name="tag">tag号</param>
        /// <returns>NXObject</returns>
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

        /// <summary>
        /// 得到模型图纸信息列表
        /// </summary>
        /// <param name="fullpath">模型全路径</param>
        /// <returns></returns>
        public static List<S_Sheet> GetSheetTempletList(string fullpath)
        {
            List<S_Sheet> SheetTempletList = new List<S_Sheet>();
            try
            {                
                if (!isFileExist(fullpath))
                {
                    return SheetTempletList;
                }
                Part prt = OpenPrtQuiet(fullpath);
                if (prt == null)
                {
                    return SheetTempletList;
                }
                NXOpen.Drawings.DrawingSheet[] sheets = prt.DrawingSheets.ToArray();
                foreach (NXOpen.Drawings.DrawingSheet ds in sheets)
                {
                    SheetTempletList.Add(GetSheetTemplet(ds));
                }
                return SheetTempletList;
            }
            catch (System.Exception ex)
            {
                MessageBox(ex.Message);
                return SheetTempletList;
            }   
        }
        public static S_Sheet GetSheetTemplet(NXOpen.Drawings.DrawingSheet ds)
        {
            S_Sheet s_sheet;
            //s_sheet.ds = ds;
            s_sheet.guid = GetORSetStringAttr(ds, "GUID", Guid.NewGuid().ToString());
            s_sheet.IsChecked = GetORSetStringAttr(ds, "ISCHECKED", "否");
            s_sheet.IsPrint = GetORSetStringAttr(ds, "ISPRINT", "否");
            s_sheet.Remark = GetORSetStringAttr(ds, "REMARK", "");
            s_sheet.Templet = GetORSetStringAttr(ds, "TEMPLET", "NX");
            s_sheet.SheetName = ds.Name;
            
            string num = GetStringAttr(ds, "NUMBER");
            if (string.IsNullOrEmpty(num))
            {
                s_sheet.index_num = 100;
            }
            else
            {
                s_sheet.index_num = System.Convert.ToInt32(num);
            }
            s_sheet.a = 0;
            s_sheet.prtName = "";
            return s_sheet;
        }
        /// <summary>
        /// 得到或者设置obj的属性
        /// </summary>
        /// <param name="obj">NXObject</param>
        /// <param name="title">属性名</param>
        /// <param name="default_value">默认属性值</param>
        /// <returns>属性值</returns>
        public static string GetORSetStringAttr(NXObject obj, string title, string default_value)
        {
            string value = GetStringAttr(obj, title);
            if (string.IsNullOrEmpty(value))
            {
                value = default_value;
                SetStringAttr(obj, title, value);
            }
            return value;
        }
        public static bool string2bool(string str)
        {
            if (str == "是")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string bool2string(bool b)
        {
            if (b == true)
            {
                return "是";
            }
            else
            {
                return "否";
            }
        }
        public static NXOpen.Drawings.DrawingSheet ShowSheetByGuid(string prtpath, string guid)
        {
            Part part = OpenPrt(prtpath);
            foreach (NXOpen.Drawings.DrawingSheet ds in part.DrawingSheets)
            {
                if (GetStringAttr(ds, "GUID") == guid)
                {
                    ds.Open();
                    return ds;
                }
            }
            return null;
        }
        public static void ShowSheetByGuid(string guid)
        {
            NXOpen.Drawings.DrawingSheet[] dss = Session.GetSession().Parts.Work.DrawingSheets.ToArray();
            foreach (NXOpen.Drawings.DrawingSheet ds in dss)
            {
                if (GetStringAttr(ds, "GUID") == guid)
                {
                    ds.Open();
                }
            }
        }
        public static NXOpen.Drawings.DrawingSheet GetSheetByGuid(string prtpath, string guid)
        {
            Part part = OpenPrtQuiet(prtpath);
            foreach (NXOpen.Drawings.DrawingSheet ds in part.DrawingSheets)
            {
                if (GetStringAttr(ds, "GUID") == guid)
                {
                    return ds;
                }
            }
            return null;
        }
        /// <summary>
        /// 打开并显示模型
        /// </summary>
        /// <param name="fullpath">模型全路径</param>
        /// <returns>Part</returns>
        public static Part OpenPrt(string fullpath)
        {
            if (!isFileExist(fullpath))
            {
                return null;
            }
            PartLoadStatus status;
            if (Program.theUfSession.Part.IsLoaded(fullpath) == 0)
            {
                //没有打开
                Part part = (Part)Session.GetSession().Parts.OpenDisplay(fullpath, out status);
                return part;
            }
            else
            {
                //打开状态
                Tag tag = Program.theUfSession.Part.AskPartTag(fullpath);
                if (tag == 0)
                {
                    throw new Exception("无法载入" + fullpath);
                    //return null;
                }
                else
                {
                    Part part = Tag2NXObject<Part>(tag);
                    Session.GetSession().Parts.SetDisplay(part, true, true, out status);
                    return part;
                }
            }
        }
        public static bool isFileExist(string path)
        {
            return System.IO.File.Exists(path);
        }
        /// <summary>
        /// 另起线程打开文件
        /// </summary>
        /// <param name="filepath">文件全路径</param>
        /// <returns>文件存在返回真</returns>
        public static bool OpenFile(string filepath)
        {
            if (isFileExist(filepath))
            {
                Process.Start(filepath);
                return true;
            }
            else
            {
                MessageBox("找不到文件:" + filepath, "错误", 0);
                return false;
            }
        }  
        /// <summary>
        /// 得到某个模型作为孩子的详细wave信息
        /// </summary>
        /// <param name="prtPath">模型全路径</param>
        /// <returns>信息列表</returns>
        public static List<S_WaveInfor> GetSinglePrtWaveInfor(string prtPath)
        {
            List<S_WaveInfor> waveinforlist = new List<S_WaveInfor>();
            if (!isFileExist(prtPath))
            {
                return waveinforlist;
            }
            S_WaveInfor waveinfor;
            Part part = OpenPrtQuiet(prtPath);

            NXOpen.Features.Feature[] fs = part.Features.ToArray();
            foreach (NXOpen.Features.Feature f in fs)
            {
                if (f.FeatureType == "LINKED_BODY")
                {
                    bool isbroken;
                    Program.theUfSession.Wave.IsLinkBroken(f.Tag, out isbroken);
                    waveinfor.isbreak = isbroken;
                    waveinfor.childpath = f.OwningPart.FullPath;
                    waveinfor.childname = f.JournalIdentifier;

                    waveinfor.wave_type = E_WaveType.LINKED_BODY;
                    waveinfor.child = f;
                    waveinfor.father = null;
                    if (isbroken)
                    {
                        string prt_name;
                        string source_handle;
                        Program.theUfSession.Wave.AskBrokenLinkSourcePart(f.Tag, out prt_name, out source_handle);
                        waveinfor.fatherpath = prt_name;
                        waveinfor.fathername = source_handle;
                    }
                    else
                    {
                        NXObject ob = GetWaveFather(f);
                        waveinfor.father = ob;
                        if (ob == null)
                        {
                            waveinfor.fatherpath = "外部";
                            waveinfor.fathername = "未知";
                        }
                        else
                        {
                            waveinfor.fatherpath = ob.OwningPart.FullPath;
                            waveinfor.fathername = ob.JournalIdentifier;
                        }
                    }
                    waveinforlist.Add(waveinfor);
                }
                else if (f.FeatureType == "LINKED_FACE")
                {
                    bool isbroken;
                    Program.theUfSession.Wave.IsLinkBroken(f.Tag, out isbroken);
                    waveinfor.isbreak = isbroken;
                    waveinfor.childpath = f.OwningPart.FullPath;
                    waveinfor.childname = f.JournalIdentifier;

                    waveinfor.wave_type = E_WaveType.LINKED_FACE;
                    waveinfor.child = f;
                    waveinfor.father = null;
                    if (isbroken)
                    {
                        string prt_name;
                        string source_handle;
                        Program.theUfSession.Wave.AskBrokenLinkSourcePart(f.Tag, out prt_name, out source_handle);
                        waveinfor.fatherpath = prt_name;
                        waveinfor.fathername = source_handle;
                    }
                    else
                    {
                        NXObject ob = GetWaveFather(f);
                        waveinfor.father = ob;
                        if (ob == null)
                        {
                            waveinfor.fatherpath = "外部";
                            waveinfor.fathername = "未知";
                        }
                        else
                        {
                            waveinfor.fatherpath = ob.OwningPart.FullPath;
                            waveinfor.fathername = ob.JournalIdentifier;
                        }
                    }
                    waveinforlist.Add(waveinfor);
                }
                else if (f.FeatureType == "LINKED_CURVE")
                {
                    bool isbroken;
                    Program.theUfSession.Wave.IsLinkBroken(f.Tag, out isbroken);
                    waveinfor.isbreak = isbroken;
                    waveinfor.childpath = f.OwningPart.FullPath;
                    waveinfor.childname = f.JournalIdentifier;

                    waveinfor.wave_type = E_WaveType.LINKED_CURVE;
                    waveinfor.child = f;
                    waveinfor.father = null;
                    if (isbroken)
                    {
                        string prt_name;
                        string source_handle;
                        Program.theUfSession.Wave.AskBrokenLinkSourcePart(f.Tag, out prt_name, out source_handle);
                        waveinfor.fatherpath = prt_name;
                        waveinfor.fathername = source_handle;
                    }
                    else
                    {
                        NXObject ob = GetWaveFather(f);
                        waveinfor.father = ob;
                        if (ob == null)
                        {
                            waveinfor.fatherpath = "外部";
                            waveinfor.fathername = "未知";
                        }
                        else
                        {
                            waveinfor.fatherpath = ob.OwningPart.FullPath;
                            waveinfor.fathername = ob.JournalIdentifier;
                        }
                    }
                    waveinforlist.Add(waveinfor);
                }
                else if (f.FeatureType == "LINKED_POINT")
                {
                    bool isbroken;
                    Program.theUfSession.Wave.IsLinkBroken(f.Tag, out isbroken);
                    waveinfor.isbreak = isbroken;
                    waveinfor.childpath = f.OwningPart.FullPath;
                    waveinfor.childname = f.JournalIdentifier;

                    waveinfor.wave_type = E_WaveType.LINKED_POINT;
                    waveinfor.child = f;
                    waveinfor.father = null;
                    if (isbroken)
                    {
                        string prt_name;
                        string source_handle;
                        Program.theUfSession.Wave.AskBrokenLinkSourcePart(f.Tag, out prt_name, out source_handle);
                        waveinfor.fatherpath = prt_name;
                        waveinfor.fathername = source_handle;
                    }
                    else
                    {
                        NXObject ob = GetWaveFather(f);
                        waveinfor.father = ob;
                        if (ob == null)
                        {
                            waveinfor.fatherpath = "外部";
                            waveinfor.fathername = "未知";
                        }
                        else
                        {
                            waveinfor.fatherpath = ob.OwningPart.FullPath;
                            waveinfor.fathername = ob.JournalIdentifier;
                        }
                    }
                    waveinforlist.Add(waveinfor);
                }
                else if (f.FeatureType == "EXTRACT_DATUM_PLANE" && f.GetType().Name == "WaveDatum")
                {
                    bool isbroken;
                    Program.theUfSession.Wave.IsLinkBroken(f.Tag, out isbroken);
                    waveinfor.isbreak = isbroken;
                    waveinfor.childpath = f.OwningPart.FullPath;
                    waveinfor.childname = f.JournalIdentifier;

                    waveinfor.wave_type = E_WaveType.LINKED_DATUM;
                    waveinfor.child = f;
                    waveinfor.father = null;
                    if (isbroken)
                    {
                        string prt_name;
                        string source_handle;
                        Program.theUfSession.Wave.AskBrokenLinkSourcePart(f.Tag, out prt_name, out source_handle);
                        waveinfor.fatherpath = prt_name;
                        waveinfor.fathername = source_handle;
                    }
                    else
                    {
                        NXObject ob = GetWaveFather(f);
                        waveinfor.father = ob;
                        if (ob == null)
                        {
                            waveinfor.fatherpath = "外部";
                            waveinfor.fathername = "未知";
                        }
                        else
                        {
                            waveinfor.fatherpath = ob.OwningPart.FullPath;
                            waveinfor.fathername = ob.JournalIdentifier;
                        }
                    }
                    waveinforlist.Add(waveinfor);
                }
                else if (f.FeatureType == "SKETCH" && f.GetType().Name == "WaveSketch")
                {
                    bool isbroken;
                    Program.theUfSession.Wave.IsLinkBroken(f.Tag, out isbroken);
                    waveinfor.isbreak = isbroken;
                    waveinfor.childpath = f.OwningPart.FullPath;
                    waveinfor.childname = f.JournalIdentifier;

                    waveinfor.wave_type = E_WaveType.LINKED_SKETCH;
                    waveinfor.child = f;
                    waveinfor.father = null;
                    if (isbroken)
                    {
                        string prt_name;
                        string source_handle;
                        Program.theUfSession.Wave.AskBrokenLinkSourcePart(f.Tag, out prt_name, out source_handle);
                        waveinfor.fatherpath = prt_name;
                        waveinfor.fathername = source_handle;
                    }
                    else
                    {
                        NXObject ob = GetWaveFather(f);
                        waveinfor.father = ob;
                        if (ob == null)
                        {
                            waveinfor.fatherpath = "外部";
                            waveinfor.fathername = "未知";
                        }
                        else
                        {
                            waveinfor.fatherpath = ob.OwningPart.FullPath;
                            waveinfor.fathername = ob.JournalIdentifier;
                        }
                    }
                    waveinforlist.Add(waveinfor);
                }
            }

            return waveinforlist;
        }
        /// <summary>
        /// 打断wave关系
        /// </summary>
        /// <param name="linkfeature">wave特征</param>
        /// <returns>如果原本断开，返回false，执行成功返回true</returns>
        public static bool AcceptLinkBroken(Tag linkfeature)
        {
            try
            {
                bool isbroken;
                Program.theUfSession.Wave.IsLinkBroken(linkfeature, out isbroken);
                if (isbroken)
                {
                    return false;
                }
                Session theSession = Session.GetSession();
                Part workPart = theSession.Parts.Work;
                Part displayPart = theSession.Parts.Display;
                NXOpen.Features.Feature currentFeature = workPart.CurrentFeature;
                NXOpen.Features.ExtractFace extractFace1 = Tag2NXObject<NXOpen.Features.ExtractFace>(linkfeature);
                extractFace1.MakeCurrentFeature();
                NXOpen.Features.ExtractFaceBuilder extractFaceBuilder1;
                extractFaceBuilder1 = workPart.Features.CreateExtractFaceBuilder(extractFace1);
                extractFaceBuilder1.Associative = false;
                NXObject nXObject1;
                nXObject1 = extractFaceBuilder1.Commit();
                extractFaceBuilder1.Destroy();
                NXOpen.Features.ExtractFace extractFace2 = (NXOpen.Features.ExtractFace)nXObject1;
                extractFace2.MakeCurrentFeature();
                currentFeature.MakeCurrentFeature();
                return true;
            }
            catch (System.Exception ex)
            {
                MessageBox(ex.Message);
                return false;
            }
        }
//         public static UFSession Program.theUfSession
//         {
//             get
//             {
//                 return NXOpen.UF.UFSession.GetUFSession();
//             }
//         }
//         public static Session theSession
//         {
//             get
//             {
//                 return Session.GetSession();
//             }
//         }
        public static void Fit()
        {            
            Program.theSession.Parts.Work.Views.WorkView.Fit();
        }
        public static NXObject GetWaveFather(NXOpen.Features.Feature fe)
        {
            try
            {
                Tag tag;
                Program.theUfSession.Wave.AskLinkSource(fe.Tag, true, out tag);
                NXObject ob = Tag2NXObject<NXObject>(tag);
                return ob;
            }
            catch /*(System.Exception ex)*/
            {
                return null;
            }
        }
        public static string GetOnShowSheetGuid()
        {
            try
            {
                NXOpen.Drawings.DrawingSheet ds = Session.GetSession().Parts.Work.DrawingSheets.CurrentDrawingSheet;
                if (ds == null)
                {
                    return "";
                }
                else
                {
                    return GetStringAttr(ds, "GUID");
                }
            }
            catch/* (System.Exception ex)*/
            {
                return "";
            }
        }
        public static NXOpen.Drawings.DrawingSheet GetOnShowSheet()
        {
            try
            {
                NXOpen.Drawings.DrawingSheet ds = Session.GetSession().Parts.Work.DrawingSheets.CurrentDrawingSheet;
                if (ds == null)
                {
                    return null;
                }
                else
                {
                    return ds;
                }
            }
            catch/* (System.Exception ex)*/
            {
                return null;
            }
        }
        public static void WriteText(S_SheetNote s_note, string sheetguid)
        {
            if (s_note.text.Length == 0)
            {
                return;
            }
            if (s_note.text.Length == 1 && string.IsNullOrEmpty(s_note.text[0]))
            {
                return;
            }
            Part workPart = Session.GetSession().Parts.Work;
            NXOpen.Annotations.SimpleDraftingAid nullAnnotations_SimpleDraftingAid = null;
            NXOpen.Annotations.DraftingNoteBuilder draftingNoteBuilder1;
            draftingNoteBuilder1 = workPart.Annotations.CreateDraftingNoteBuilder(nullAnnotations_SimpleDraftingAid);
            draftingNoteBuilder1.Origin.Plane.PlaneMethod = NXOpen.Annotations.PlaneBuilder.PlaneMethodType.XyPlane;
            draftingNoteBuilder1.Origin.SetInferRelativeToGeometry(false);
            draftingNoteBuilder1.Origin.SetInferRelativeToGeometry(true);
            draftingNoteBuilder1.Origin.Anchor = s_note.pos;
            draftingNoteBuilder1.Text.TextBlock.SetText(s_note.text);
            draftingNoteBuilder1.TextAlignment = NXOpen.Annotations.DraftingNoteBuilder.TextAlign.Belowbottom;
            draftingNoteBuilder1.Origin.Plane.PlaneMethod = NXOpen.Annotations.PlaneBuilder.PlaneMethodType.XyPlane;
            draftingNoteBuilder1.Origin.SetInferRelativeToGeometry(true);
            NXOpen.Annotations.LeaderData leaderData1;
            leaderData1 = workPart.Annotations.CreateLeaderData();
            leaderData1.Arrowhead = NXOpen.Annotations.LeaderData.ArrowheadType.FilledArrow;
            draftingNoteBuilder1.Leader.Leaders.Append(leaderData1);
            leaderData1.StubSide = NXOpen.Annotations.LeaderSide.Inferred;
            draftingNoteBuilder1.Origin.SetInferRelativeToGeometry(true);
            draftingNoteBuilder1.Origin.SetInferRelativeToGeometry(true);
            draftingNoteBuilder1.Style.LetteringStyle.GeneralTextSize = s_note.TextSize;
            draftingNoteBuilder1.Style.LetteringStyle.GeneralTextFont = workPart.Fonts.AddFont(s_note.TextFont);
            draftingNoteBuilder1.Style.LetteringStyle.GeneralTextLineSpaceFactor = s_note.LineSpaceFactor;
            draftingNoteBuilder1.Style.LetteringStyle.GeneralTextCharSpaceFactor = s_note.CharSpaceFactor;
            draftingNoteBuilder1.Style.LetteringStyle.GeneralTextAspectRatio = s_note.AspectRatio;
            NXOpen.Annotations.Annotation.AssociativeOriginData assocOrigin1;
            assocOrigin1.OriginType = NXOpen.Annotations.AssociativeOriginType.Drag;
            NXOpen.View nullView = null;
            assocOrigin1.View = nullView;
            assocOrigin1.ViewOfGeometry = nullView;
            Point nullPoint = null;
            assocOrigin1.PointOnGeometry = nullPoint;
            assocOrigin1.VertAnnotation = null;
            assocOrigin1.VertAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
            assocOrigin1.HorizAnnotation = null;
            assocOrigin1.HorizAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
            assocOrigin1.AlignedAnnotation = null;
            assocOrigin1.DimensionLine = 0;
            assocOrigin1.AssociatedView = nullView;
            assocOrigin1.AssociatedPoint = nullPoint;
            assocOrigin1.OffsetAnnotation = null;
            assocOrigin1.OffsetAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
            assocOrigin1.XOffsetFactor = 0.0;
            assocOrigin1.YOffsetFactor = 0.0;
            assocOrigin1.StackAlignmentPosition = NXOpen.Annotations.StackAlignmentPosition.Above;
            draftingNoteBuilder1.Origin.SetAssociativeOrigin(assocOrigin1);
            Point3d point1 = new Point3d(s_note.x, s_note.y, 0.0);
            draftingNoteBuilder1.Origin.Origin.SetValue(null, nullView, point1);
            draftingNoteBuilder1.Origin.SetInferRelativeToGeometry(true);
            NXObject nXObject1;
            nXObject1 = draftingNoteBuilder1.Commit();
            //标记属性  程序创建 1
            SetStringAttr(nXObject1, "FLAG", s_note.flag);
            SetStringAttr(nXObject1, "SHEETGUID", sheetguid);
            draftingNoteBuilder1.Destroy();
        }
        public static string GetOnShowPrtName()
        {
            try
            {
                string name = Session.GetSession().Parts.Work.FullPath;
                return name;
            }
            catch/* (System.Exception ex)*/
            {
                return "";
            }            
        }

        public static List<NXOpen.Drawings.DrawingSheet> GetDrawingSheets()
        {
            List<NXOpen.Drawings.DrawingSheet> result = new List<NXOpen.Drawings.DrawingSheet>();
            NXOpen.Drawings.DrawingSheet[] dss = Session.GetSession().Parts.Work.DrawingSheets.ToArray();
            foreach (NXOpen.Drawings.DrawingSheet ds in dss)
            {
                result.Add(ds);
            }
            return result;
        }
        public static void ClearNotesBySheetGuid(string prtpath, string guid)
        {
            Session theSession = Session.GetSession();
            Part workPart = Session.GetSession().Parts.Work;
            theSession.UpdateManager.ClearErrorList();
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Delete");
            //NXOpen.Annotations.Note[] notes = (NXOpen.Annotations.Note[])workPart.Notes.ToArray();//ori
            List<NXOpen.Annotations.Note> notes = new List<NXOpen.Annotations.Note>(0);
            foreach (object c in workPart.Notes)
            {
                if (c is NXOpen.Annotations.Note)
                {
                    notes.Add((NXOpen.Annotations.Note)c);
                }
            }

            List<NXOpen.Annotations.Annotation> noto2del = new List<NXOpen.Annotations.Annotation>();
            foreach (NXOpen.Annotations.Annotation note in notes)
           {
               if (GetStringAttr(note, "SHEETGUID") == guid)
                   noto2del.Add(note);
           }
           int nErrs1;
           nErrs1 = theSession.UpdateManager.AddToDeleteList(List2Array(noto2del));
           bool notifyOnDelete2;
           notifyOnDelete2 = theSession.Preferences.Modeling.NotifyOnDelete;
           int nErrs2;
           nErrs2 = theSession.UpdateManager.DoUpdate(markId1);
        }
        public static T[] List2Array<T>(List<T> list)
        {
            int l = list.Count;
            T[] array = new T[l];
            for (int i = 0; i < l; i++)
            {
                array[i] = list[i];
            }
            return array;
        }
        public static List<T> Array2List<T>(T[] array)
        {
            List<T> list = new List<T>();
            if (array == null)
            {
                return list;
            }
            foreach (T i in array)
            {
                list.Add(i);
            }
            return list;
        }
        /// <summary>
        /// NXMessageBox
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="title">title</param>
        /// <param name="type">0=Error，1=Information，2=Question，3=Warning</param>
        public static void MessageBox(string message, string title, int type)
        {
            NXMessageBox.DialogType dt = NXMessageBox.DialogType.Error;
            switch (type)
            {
                case 0: dt = NXMessageBox.DialogType.Error;
                    break;
                case 1: dt = NXMessageBox.DialogType.Information;
                    break;
                case 2: dt = NXMessageBox.DialogType.Question;
                    break;
                case 3: dt = NXMessageBox.DialogType.Warning;
                    break;
            }
            UI.GetUI().NXMessageBox.Show(title, dt, message);
        }
        /// <summary>
        /// 默认以消息格式，没有title
        /// </summary>
        /// <param name="message"></param>
        public static void MessageBox(string message)
        {
            UI.GetUI().NXMessageBox.Show("", NXMessageBox.DialogType.Information, message);
        }
        public static string Reverse(string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public static int GetMaxLength<T>(T[][] list)
        {
            int count = 0;
            foreach (T[] l in list)
            {
                if (l!=null && l.Length > count)
                {
                    count = l.Length;
                }
            }
            return count;
        }
        public static string FindAddSheetGuid(string guid, int addnum, string sheetname)
        {
            string addguid = "";
            int flag = 0;
            NXOpen.Drawings.DrawingSheet[] dss = Session.GetSession().Parts.Work.DrawingSheets.ToArray();
            foreach (NXOpen.Drawings.DrawingSheet ds in dss)
            {
                if (GetStringAttr(ds, "FATHERGUID") == guid && GetStringAttr(ds, "ADDNUM") == addnum.ToString())
                {
                    addguid = GetStringAttr(ds, "GUID");
                    flag = 1;
                    break;
                }
            }
            if (flag == 1)
            {
                //本来就有续页
                return addguid;
            }
            else
            {
                //创建续页
                string path = TDPPMPath + "Templet\\" + sheetname + ".prt";
                if (!isFileExist(path))
                {
                    MessageBox("找不到文件" + path, "错误", 0);
                    return "";
                }
                S_SheetTemplet SheetTemplet = XML3DPPM.GetSheetTemplet(sheetname, TDPPMPath + SheetTempletXML);
                S_SheetAttr sheetattr;
                sheetattr.GUID = Guid.NewGuid().ToString();
                sheetattr.ISPRINT = "是";
                sheetattr.ISCHECKED = "否";
                sheetattr.REMARK = SheetTemplet.chinese;
                sheetattr.TEMPLET = SheetTemplet.name;
                NXOpen.Drawings.DrawingSheet ds = CreateSheet("a", path, sheetattr);
                SetStringAttr(ds, "FATHERGUID", guid);
                SetStringAttr(ds, "ADDNUM", addnum.ToString());
                return sheetattr.GUID;
            }
        }
        public static void ClearOnShowTitleNotesBySheetGuid(string guid)
        {
            Session theSession = Session.GetSession();
            Part workPart = Session.GetSession().Parts.Work;
            theSession.UpdateManager.ClearErrorList();
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Delete");
            //NXOpen.Annotations.Note[] notes = null;
            //   notes = workPart.Notes.ToArray();

            List<NXOpen.Annotations.Note> notes = new List<NXOpen.Annotations.Note>(0);
            foreach (object c in workPart.Notes)
            {
                if (c is NXOpen.Annotations.Note)
                {
                    notes.Add((NXOpen.Annotations.Note)c);
                }
            }

            List<NXOpen.Annotations.Note> noto2del = new List<NXOpen.Annotations.Note>();
            foreach (NXOpen.Annotations.Note note in notes)
            {
                if (GetStringAttr(note, "SHEETGUID") == guid && !string.IsNullOrEmpty(GetStringAttr(note, "FLAG")))
                    noto2del.Add(note);
            }
            int nErrs1;
            nErrs1 = theSession.UpdateManager.AddToDeleteList(List2Array(noto2del));
            bool notifyOnDelete2;
            notifyOnDelete2 = theSession.Preferences.Modeling.NotifyOnDelete;
            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(markId1);
        }
        public static List<T> RemoveTheSame<T>(List<T> list)
        {
            List<T> new_list = new List<T>();
            foreach (T i in list)
            {
                if (!new_list.Contains(i))
                {
                    new_list.Add(i);
                }
            }
            return new_list;
        }
        /// <summary>
        /// 关闭模型
        /// </summary>
        /// <param name="fullpath">模型全路径</param>
        /// <param name="isSave">关闭时是否保存 true 保存 flase 不保存</param>
        public static void ClosePrt(string fullpath, bool isSave)
        {
            Tag part_tag = Tag.Null;
            if (Program.theUfSession.Part.IsLoaded(fullpath) > 0)
            {
                part_tag = Program.theUfSession.Part.AskPartTag(fullpath);
                if (isSave)
                {
                    Program.theUfSession.Part.SetDisplayPart(part_tag);
                    Program.theUfSession.Part.Save();
                }
                Program.theUfSession.Part.Close(part_tag, 1, 1);
            }
        }
        /// <summary>
        /// 保存模型
        /// </summary>
        /// <param name="fullpath">模型全路径</param>
        public static void SavePrt(string fullpath)
        {
            Tag part_tag = Tag.Null;
            if (Program.theUfSession.Part.IsLoaded(fullpath) > 0)
            {
                part_tag = Program.theUfSession.Part.AskPartTag(fullpath);
                Program.theUfSession.Part.SetDisplayPart(part_tag);
                Program.theUfSession.Part.Save();
            }
        }
        /// <summary>
        /// 创建wave
        /// </summary>
        /// <param name="obj">要wave的父亲几何tag</param>
        /// <param name="prt">要wave的孩子所在prt的tag</param>
        /// <param name="update">时间戳，默认推荐false</param>
        /// <returns>wave特征的tag</returns>
        public static Tag CreateWave(Tag obj, Tag prt, bool update)
        {
            Tag linked_feature = Tag.Null;
            Tag xform = Tag.Null;
            int type = 0, subtype = 0;
            Program.theUfSession.Obj.AskTypeAndSubtype(obj, out type, out subtype);
            if (type == NXOpen.UF.UFConstants.UF_sketch_type)
            {
                Program.theUfSession.Wave.CreateLinkedSketch(obj, xform, prt, out linked_feature);
            }
            else if ((type == NXOpen.UF.UFConstants.UF_solid_type) && (subtype == NXOpen.UF.UFConstants.UF_solid_body_subtype))
            {
                Program.theUfSession.Wave.CreateLinkedBody(obj, xform, prt, update, out linked_feature);
            }
            else if (type == NXOpen.UF.UFConstants.UF_sketch_type)
            {
                Program.theUfSession.Wave.CreateLinkedSketch(obj, xform, prt, out linked_feature);
            }
            else if (type == NXOpen.UF.UFConstants.UF_line_type || //可能不够  需要添加
                type == NXOpen.UF.UFConstants.UF_circle_type ||
                type == NXOpen.UF.UFConstants.UF_spline_type ||
                type == NXOpen.UF.UFConstants.UF_conic_type)
            {
                Program.theUfSession.Wave.CreateLinkedCurve(obj, xform, prt, update, out linked_feature);
            }
            else if (type == NXOpen.UF.UFConstants.UF_datum_plane_type)
            {
                Program.theUfSession.Wave.CreateLinkedDatum(obj, xform, prt, out linked_feature);
            }
            else if ((type == NXOpen.UF.UFConstants.UF_solid_type) && (subtype == NXOpen.UF.UFConstants.UF_solid_face_subtype))
            {
                Program.theUfSession.Wave.CreateLinkedFace(obj, xform, prt, update, out linked_feature);
            }
            else if ((type == NXOpen.UF.UFConstants.UF_solid_type) && (subtype == NXOpen.UF.UFConstants.UF_solid_edge_subtype))
            {
                Program.theUfSession.Wave.CreateLinkedCurve(obj, xform, prt, update, out linked_feature);
            }
            else if (type == NXOpen.UF.UFConstants.UF_point_type)
            {
                Program.theUfSession.Wave.CreateLinkedPtPoint(obj, xform, prt, out linked_feature);
            }
            return linked_feature;
        }
        public static void PrintPDF(S_PDF pdf)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            PrintPDFBuilder printPDFBuilder1;
            printPDFBuilder1 = workPart.PlotManager.CreatePrintPdfbuilder();
            printPDFBuilder1.Scale = 1.0;
            printPDFBuilder1.Size = NXOpen.PrintPDFBuilder.SizeOption.ScaleFactor;
            printPDFBuilder1.OutputText = NXOpen.PrintPDFBuilder.OutputTextOption.Polylines;
            printPDFBuilder1.Colors = NXOpen.PrintPDFBuilder.Color.BlackOnWhite;
            printPDFBuilder1.Widths = NXOpen.PrintPDFBuilder.Width.CustomThreeWidths;
            printPDFBuilder1.RasterImages = true;
            printPDFBuilder1.Watermark = "";
            NXObject[] sheets = new NXObject[1];
            sheets[0] = pdf.sheet;
            printPDFBuilder1.SourceBuilder.SetSheets(sheets);
            printPDFBuilder1.Filename = pdf.path;
            printPDFBuilder1.Append = pdf.append;
            NXObject nXObject1 = printPDFBuilder1.Commit();
            printPDFBuilder1.Destroy();
        }
        /// <summary>
        /// 得到当前显示的prt名
        /// </summary>
        /// <param name="isfullpath">是否全路径</param>
        /// <returns>prt名或全路径</returns>
        public static string GetWorkPrtName(bool isfullpath)
        {
            Part prt = Session.GetSession().Parts.Work;
            string fullpath = "";
            if (prt != null)
            {
                fullpath = prt.FullPath;
            }
            if (!isfullpath)
            {
                fullpath = fullpath.Substring(fullpath.LastIndexOf("\\") + 1, fullpath.Length - fullpath.LastIndexOf("\\") - 1);
            }
            return fullpath;
        }
        public static void BlankObject(DisplayableObject obj)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            DisplayableObject[] objects1 = new DisplayableObject[1];
            objects1[0] = obj;
            theSession.DisplayManager.BlankObjects(objects1);
        }
        public static bool IsDrafting()
        {
            int module_id;
            Program.theUfSession.UF.AskApplicationModule(out module_id);
            if (module_id == UFConstants.UF_APP_DRAFTING)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 判断字符串是否含有中文字符
        /// </summary>
        /// <param name="CString">待检测字符串</param>
        /// <returns>True 含中文，False 不含中文</returns>
        public static bool IsChina(string CString)
        {
            bool BoolValue = false;
            for (int i = 0; i < CString.Length; i++)
            {
                if (Convert.ToInt32(Convert.ToChar(CString.Substring(i, 1))) > Convert.ToInt32(Convert.ToChar(128)))
                {
                    BoolValue = true;
                }
            }
            return BoolValue;
        }
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dir">目录路径</param>
        /// <returns>成功返回true，否则返回false</returns>
        public static bool DeleteDirectory(string dir)
        {
            try
            {
                if (!Directory.Exists(dir))
                {
                    return true;
                }

                Directory.Delete(dir, true);
                return true;
            }
            catch/* (System.Exception ex)*/
            {
                return false;
            }
        }
        public static void CreateNewPart(string fullpath)
        {
            string sourcePrt = TDPPMPath + EmptyModel;
            if (isFileExist(fullpath))
            {
                ClosePrt(fullpath, false);
            }
            File.Copy(sourcePrt, fullpath, true);
        }
        /// <summary>
        /// 删除模型
        /// </summary>
        /// <param name="fullpath">模型全路径</param>
        /// <param name="force">是否强制删除带wave链接的</param>
        /// <returns>删除成功否</returns>
        public static bool DeletePrt(string fullpath, bool force)
        {
            if (force)
            {
                ClosePrt(fullpath, false);
                File.Delete(fullpath);
                return true;
            }
            else
            {
                //待更新
                return false;
            }
        }
        public static bool CopyPrt(string fromPrt, string toPrt)
        {
            try
            {
                SavePrt(fromPrt);
                File.Copy(fromPrt, toPrt, true);
                return true;
            }
            catch (System.Exception ex)
            {
                MessageBox(ex.Message);
                return false;
            }


        }
        /// <summary>
        /// 设置NX环境  必须有模型打开  0  建模   1  制图  ……
        /// </summary>
        /// <param name="env">0  建模   1  制图</param>
        /// <returns>无模型返回false</returns>
        public static bool SetEnvironment(int env)
        {
            if (string.IsNullOrEmpty(GetOnShowPrtName()))
            {
                return false;
            }
            string marco = "";
            switch (env)
            {
                case 0://进入建模环境
                    marco = TDPPMPath + Macro_Modeling;
                    PlayMacro(marco);
                    break;
                case 1:
                    marco = TDPPMPath + Macro_Drafting;
                    PlayMacro(marco);
                    break;
                default:
                    break;
            }
            return true;                        
        }
        private static int init_proc_wave(IntPtr select, IntPtr user_data)
        {
            NXOpen.UF.UFUi.Mask[] mask_triples = new NXOpen.UF.UFUi.Mask[10];

            //设置过滤器类型
            //面
            mask_triples[0].object_type = NXOpen.UF.UFConstants.UF_solid_type;
            mask_triples[0].object_subtype = NXOpen.UF.UFConstants.UF_all_subtype;
            mask_triples[0].solid_type = NXOpen.UF.UFConstants.UF_UI_SEL_FEATURE_ANY_FACE;
            //边
            mask_triples[1].object_type = NXOpen.UF.UFConstants.UF_solid_type;
            mask_triples[1].object_subtype = NXOpen.UF.UFConstants.UF_all_subtype;
            mask_triples[1].solid_type = NXOpen.UF.UFConstants.UF_UI_SEL_FEATURE_ANY_EDGE;
            //实体
            mask_triples[2].object_type = NXOpen.UF.UFConstants.UF_solid_type;
            mask_triples[2].object_subtype = 0;
            mask_triples[2].solid_type = 0;
            //线
            mask_triples[3].object_type = NXOpen.UF.UFConstants.UF_line_type;
            mask_triples[3].object_subtype = 0;
            mask_triples[3].solid_type = 0;
            //圆弧
            mask_triples[4].object_type = NXOpen.UF.UFConstants.UF_circle_type;
            mask_triples[4].object_subtype = 0;
            mask_triples[4].solid_type = 0;
            //二次曲线
            mask_triples[5].object_type = NXOpen.UF.UFConstants.UF_conic_type;
            mask_triples[5].object_subtype = NXOpen.UF.UFConstants.UF_all_subtype;
            mask_triples[5].solid_type = 0;
            //样条
            mask_triples[6].object_type = NXOpen.UF.UFConstants.UF_spline_type;
            mask_triples[6].object_subtype = 0;
            mask_triples[6].solid_type = 0;
            //草图
            mask_triples[7].object_type = NXOpen.UF.UFConstants.UF_sketch_type;
            mask_triples[7].object_subtype = 0;
            mask_triples[7].solid_type = 0;
            //基准平面
            mask_triples[8].object_type = NXOpen.UF.UFConstants.UF_datum_plane_type;
            mask_triples[8].object_subtype = 0;
            mask_triples[8].solid_type = 0;
            //点
            mask_triples[9].object_type = NXOpen.UF.UFConstants.UF_point_type;
            mask_triples[9].object_subtype = 0;
            mask_triples[9].solid_type = 0;
            Program.theUfSession.Ui.SetSelMask(
                select,
                NXOpen.UF.UFUi.SelMaskAction.SelMaskClearAndEnableSpecific,
                mask_triples.Length,
                mask_triples
                );
            return (NXOpen.UF.UFConstants.UF_UI_SEL_SUCCESS);
        }
        public static Tag[] SelectWaveObjects()
        {
            Program.theUfSession.Ui.LockUgAccess(NXOpen.UF.UFConstants.UF_UI_FROM_CUSTOM);
            int response = 0;
            int count = 0;
            Tag[] objects = new Tag[0];
            Program.theUfSession.Ui.SelectWithClassDialog(
                "",
                "选择几何对象",
                NXOpen.UF.UFConstants.UF_UI_SEL_SCOPE_NO_CHANGE,
                init_proc_wave,
                IntPtr.Zero,
                out response,
                out count,
                out objects
                );
            Program.theUfSession.Disp.Refresh();
            Program.theUfSession.Ui.UnlockUgAccess(NXOpen.UF.UFConstants.UF_UI_FROM_CUSTOM);
            //还原高亮 否则会永久高亮选不到
            foreach (Tag tag in objects)
            {
                Program.theUfSession.Disp.SetHighlight(tag, 0);
            }
            if (response == NXOpen.UF.UFConstants.UF_UI_OK && count > 0)
            {
                return objects;
            }
            else
            {
                return null;
            }
        }
        public static void ShowHelp()
        {
            NXFun.OpenFile(NXFun.TDPPMPath + NXFun.HelpDoc);
        }
        public static void ShowAbout()
        {
            string message =
                        "三维机加工艺设计系统\n" +
                        "V0.9.6 测试版";
            NXFun.MessageBox(message, "关于", 1);
        }
        public static void ShowJiagongmian()
        {
            if (IsDrafting())
            {
                MachinedSurface ms = new MachinedSurface();
                ms.Show();
            }
            else
            {
                MessageBox("请在制图环境下使用本工具", "提示", 0);
            }
        }
        public static void ShowGongcha()
        {
            if (string.IsNullOrEmpty(GetOnShowPrtName()))
            {
                MessageBox("请先打开一个模型再使用本工具！");
            }
            else
            {
                AddTolerance.MainFun();
            }   
        }
        public static void ShowHuizhuan()
        {
            if (string.IsNullOrEmpty(GetOnShowPrtName()))
            {
                MessageBox("请先打开一个模型再使用本工具！");
            }
            else
            {
                RevolveTrim.MainFun();
            } 
        }
        public static void ShowCAPP()
        {
            OpenFile(TDPPMPath + CAPPAssistantEXE);
        }
    }
}