using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NXOpen;

namespace TDPPM
{
    public partial class LabelEditPanel : EditPanel
    {
        NXObject[] ObjectsInView = new NXObject[0]; //timer里的东西  放这里可能会节约资源？
        bool bieluandong = true; //别乱动
        int DimStartNum = 1;    //尺寸起始编号
        int FcfStartNum = 1;    //形位起始编号
        object oldhighlight = null; //原来高亮的标注
        string OnEidtSheetGuid = ""; //正在编辑的SheetGuid

        MultiMap<int, DimInfor> LabelDimMap = new MultiMap<int, DimInfor>();    //自动排序的尺寸信息表
        MultiMap<int, DimInfor> LabelFcfMap = new MultiMap<int, DimInfor>();    //自动排序的形位信息表
        public List<DimInfor> LabelDimList = new List<DimInfor>();  //尺寸信息表
        public List<DimInfor> LabelFcfList = new List<DimInfor>();  //形位信息表

        //构造函数
        public LabelEditPanel()
        {
            InitializeComponent();
        }
        //初始化
        public void Init()
        {
            if (mainDlg == null || string.IsNullOrEmpty(ProPath) || string.IsNullOrEmpty(ProName))
            {
                //缺少初值，所有控件禁用  正常情况不应执行到这里
                this.Enabled = false;
            }
            else
            {
                XmlFile = ToFullPath(NXFun.ProcessXML);
                this.Enabled = true;
                IsEdit = false;
                tsrbtnSearch.Visible = true;
                tsrbtnOk.Visible = false;
                tsrbtnCancel.Visible = false;
            }
        }
        //上移
        private void tsrbtnUp_Click(object sender, EventArgs e)
        {
            if (dgvChicun.SelectedRows.Count != 0)
            {
                //移动尺寸
                int sel = dgvChicun.SelectedRows[0].Index;
                if (sel == 0)
                {
                    SetStatusLabel("选中的是第一项 无法继续移动", 1);
                    return;
                }
                else if (LabelDimList[sel].isLabel == "否")
                {
                    SetStatusLabel("选中的是不打标的项,移动没有意义", 1);
                    return;
                }
                else
                {
                    //替换两项
                    DimInfor temp1 = LabelDimList[sel - 1];
                    DimInfor temp2 = LabelDimList[sel];
                    DimInfor temp3 = temp1;
                    LabelDimList[sel - 1] = temp2;
                    LabelDimList[sel] = temp1;
                    RefreshDisplay();
                    //恢复选中
                    dgvChicun.Rows[sel - 1].Selected = true;
                    SetStatusLabel("向上移动成功", 2);
                    return;
                }
            }
            else if (dgvXingwei.SelectedRows.Count != 0)
            {
                //移动形位
                int sel = dgvXingwei.SelectedRows[0].Index;
                if (sel == 0)
                {
                    SetStatusLabel("选中的是第一项 无法继续移动", 1);
                    return;
                }
                else if (LabelFcfList[sel].isLabel == "否")
                {
                    SetStatusLabel("选中的是不打标的项,移动没有意义", 1);
                    return;
                }
                else
                {
                    //替换两项
                    DimInfor temp1 = LabelFcfList[sel - 1];
                    DimInfor temp2 = LabelFcfList[sel];
                    DimInfor temp3 = temp1;
                    LabelFcfList[sel - 1] = temp2;
                    LabelFcfList[sel] = temp1;
                    RefreshDisplay();
                    //恢复选中
                    dgvXingwei.Rows[sel - 1].Selected = true;
                    SetStatusLabel("向上移动成功", 2);
                    return;
                }
            }
            else
            {
                SetStatusLabel("请选中尺寸或形位中某一行", 1);
                return;
            }
        }
        //下移按钮
        private void tsrbtnDown_Click(object sender, EventArgs e)
        {
            if (dgvChicun.SelectedRows.Count != 0)
            {
                //移动尺寸
                int sel = dgvChicun.SelectedRows[0].Index;
                if (sel == LabelDimList.Count - 1)
                {
                    SetStatusLabel("选中的是最后一项 无法继续移动", 1);
                    return;
                }
                else if (LabelDimList[sel].isLabel == "是" && LabelDimList[sel + 1].isLabel == "否")
                {
                    SetStatusLabel("选中的是打标的最后一项 无法继续移动", 1);
                    return;
                }
                else if (LabelDimList[sel].isLabel == "否")
                {
                    SetStatusLabel("选中的是不打标的项,移动没有意义", 1);
                    return;
                }
                else
                {
                    //替换两项
                    DimInfor temp1 = LabelDimList[sel + 1];
                    DimInfor temp2 = LabelDimList[sel];
                    DimInfor temp3 = temp1;
                    LabelDimList[sel + 1] = temp2;
                    LabelDimList[sel] = temp1;
                    RefreshDisplay();
                    //恢复选中
                    dgvChicun.Rows[sel + 1].Selected = true;
                    SetStatusLabel("向下移动成功", 2);
                    return;
                }
            }
            else if (dgvXingwei.SelectedRows.Count != 0)
            {
                //移动形位
                int sel = dgvXingwei.SelectedRows[0].Index;
                if (sel == LabelFcfList.Count - 1)
                {
                    SetStatusLabel("选中的是最后一项 无法继续移动", 1);
                    return;
                }
                else if (LabelFcfList[sel].isLabel == "是" && LabelFcfList[sel + 1].isLabel == "否")
                {
                    SetStatusLabel("选中的是打标的最后一项 无法继续移动", 1);
                    return;
                }
                else if (LabelFcfList[sel].isLabel == "否")
                {
                    SetStatusLabel("选中的是不打标的项,移动没有意义", 1);
                    return;
                }
                else
                {
                    //替换两项
                    DimInfor temp1 = LabelFcfList[sel + 1];
                    DimInfor temp2 = LabelFcfList[sel];
                    DimInfor temp3 = temp1;
                    LabelFcfList[sel + 1] = temp2;
                    LabelFcfList[sel] = temp1;
                    RefreshDisplay();
                    //恢复选中
                    dgvXingwei.Rows[sel + 1].Selected = true;
                    SetStatusLabel("向下移动成功", 2);
                    return;
                }
            }
            else
            {
                SetStatusLabel("请选中尺寸或形位中某一行", 1);
                return;
            }
        }
        //查询按钮
        private void tsrbtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                NXFun.SetHighlight(oldhighlight, false);
                //确认打开图纸页
                NXOpen.Drawings.DrawingSheet ds = NXFun.GetOnShowSheet();
                if (ds == null)
                {
                    SetStatusLabel("请打开图纸页！", 1);
                    return;
                }
                IsEdit = true;
                S_Sheet sheet = NXFun.GetSheetTemplet(ds);
                OnEidtSheetGuid = sheet.guid;
                NXFun.Fit();
                NXFun.SelectAll();
                timer1.Interval = 1000;
                timer1.Tag = "全选";
                timer1.Start();
            }
            catch (System.Exception ex)
            {
                NXFun.MessageBox(ex.Message);
            }
        }
        //应用按钮
        private void tsrbtnOk_Click(object sender, EventArgs e)
        {
            string guid = NXFun.GetOnShowSheetGuid();
            if (string.IsNullOrEmpty(guid) || guid != OnEidtSheetGuid)
            {
                SetStatusLabel("当前信息与当前图纸不对应，请点击取消后重新查询尺寸标注，或者切换到之前的图纸！", 1);
                return;
            }
            Apply();
            IsEdit = false;
            //初始化
            LabelDimMap.Clear();
            LabelFcfMap.Clear();
            LabelDimList.Clear();
            LabelFcfList.Clear();
            RefreshDisplay();
            SetStatusLabel("标号编辑成功！", 2);
        }
        //取消按钮
        private void tsrbtnCancel_Click(object sender, EventArgs e)
        {
            IsEdit = false;
            NXFun.SetHighlight(oldhighlight, false);
            //初始化
            LabelDimMap.Clear();
            LabelFcfMap.Clear();
            LabelDimList.Clear();
            LabelFcfList.Clear();
            RefreshDisplay();
            SetStatusLabel("已取消操作！", 0);
        }
        //帮助按钮
        private void tsrbtnHelp_Click(object sender, EventArgs e)
        {
            string help =
                "注意事项：\n" +
                "1、支持继承的三维PMI标注；\n" +
                "2、编号必须在1～999之间；\n" +
                "3、如遇警告“试图使用不活动的对象”，请点击查询按钮重新查询；\n" +
                "4、投影视图如果不是1：1，可能标号尺寸会大小不一，NX手工操作亦然，此问题待解决。\n" +
                "------------------\n" +
                "打标号工具 V1.0";
            NXFun.MessageBox(help, "帮助", 1);
        }
        //刷新dgv
        private void RefreshDisplay()
        {
            try
            {
                tsrbtnSearch.Visible = !IsEdit;
                tsrbtnOk.Visible = IsEdit;
                tsrbtnCancel.Visible = IsEdit;

                bieluandong = true;
                dgvChicun.Rows.Clear();
                for (int i = 0; i < LabelDimList.Count; i++)
                {
                    string title = "";
                    if (LabelDimList[i].isLabel == "是")
                    {
                        title = (i + DimStartNum).ToString();
                        DimInfor temp = LabelDimList[i];
                        temp.id = i + DimStartNum;
                        LabelDimList[i] = temp;
                    }
                    dgvChicun.Rows.Add(title);
                    dgvChicun[1, i].Value = LabelDimList[i].text;
                    dgvChicun[2, i].Value = LabelDimList[i].value;
                    dgvChicun[3, i].Value = LabelDimList[i].up;
                    dgvChicun[4, i].Value = LabelDimList[i].low;
                    dgvChicun[5, i].Value = NXFun.string2bool(LabelDimList[i].isLabel);
                    DataGridViewCheckBoxCell a = (DataGridViewCheckBoxCell)dgvChicun[5, i];

                }
                dgvXingwei.Rows.Clear();
                for (int i = 0; i < LabelFcfList.Count; i++)
                {
                    string title = "";
                    if (LabelFcfList[i].isLabel == "是")
                    {
                        title = "J" + (i + FcfStartNum).ToString();
                        DimInfor temp = LabelFcfList[i];
                        temp.id = i + FcfStartNum;
                        LabelFcfList[i] = temp;
                    }
                    dgvXingwei.Rows.Add(title);
                    dgvXingwei[1, i].Value = LabelFcfList[i].text;
                    dgvXingwei[2, i].Value = LabelFcfList[i].value;
                    dgvXingwei[3, i].Value = NXFun.string2bool(LabelFcfList[i].isLabel);
                }
                bieluandong = false;
            }
            catch (System.Exception ex)
            {
                NXFun.MessageBox(ex.Message);
            }
        }
        //应用回调
        private void Apply()
        {
            try
            {
                NXFun.SetHighlight(oldhighlight, false);
                Session.UndoMarkId undomark = Program.theSession.SetUndoMark(Session.MarkVisibility.Visible, "打标号");
                foreach (DimInfor listitem in LabelDimList)
                {
                    if (listitem.isLabel == "是")
                    {
                        if (listitem.is_herited)  //PMIdimention
                        {
                            NXOpen.Annotations.BalloonNote balloon = NXFun.FindObjectByAttr<NXOpen.Annotations.BalloonNote>("GUID", listitem.guid);
                            if (balloon == null)
                            {
                                //没找到 添加
                                NXFun.SetDisplayState(1);
                                NXOpen.Annotations.BalloonNote balloonnote = NXFun.AddBalloonNote((NXOpen.Annotations.Annotation)listitem.dim, listitem.id.ToString(), listitem);
                                NXFun.SetDisplayState(2);
                                //找到二维继承的BalloonNote并设置大小。
                                NXOpen.Annotations.PmiAttribute[] bns = Session.GetSession().Parts.Work.PmiManager.PmiAttributes.ToArray();
                                foreach (NXOpen.Annotations.PmiAttribute pmi in bns)
                                {
                                    NXOpen.Annotations.Lettering Lettering = balloonnote.GetLetteringPreferences().GetDimensionText();
                                    if (NXFun.GetInheritParent(pmi).Equals(balloonnote) && !pmi.Equals(balloonnote))
                                    {
                                        NXFun.EditBalloonNoteSize((NXOpen.Annotations.BalloonNote)pmi, Lettering);
                                    }
                                }
                            }
                            else
                            {
                                //找到 改写
                                NXFun.EditBalloonNote(balloon, listitem.id.ToString(), listitem);
                            }
                            NXFun.SetStringAttr((NXObject)listitem.dimParent, "DISP", listitem.isLabel);
                            NXFun.SetIntegerAttr((NXObject)listitem.dimParent, "ID", listitem.id);
                        }
                        else               //普通dimention
                        {
                            //找到IdSymbol 并改写 否则添加
                            NXOpen.Annotations.IdSymbol idsymbol = NXFun.FindObjectByAttr<NXOpen.Annotations.IdSymbol>("GUID", listitem.guid);
                            if (idsymbol == null)
                            {
                                //没找到 添加
                                NXFun.AddIdSymbol((NXOpen.Annotations.Annotation)listitem.dim, listitem.id.ToString(), listitem);
                            }
                            else
                            {
                                //找到 改写
                                NXFun.EditIdSymbol(idsymbol, listitem.id.ToString(), listitem);
                            }
                            NXFun.SetStringAttr((NXObject)listitem.dimParent, "DISP", listitem.isLabel);
                            NXFun.SetIntegerAttr((NXObject)listitem.dimParent, "ID", listitem.id);
                        }
                    }
                    else if (listitem.isLabel == "否")
                    {
                        if (listitem.is_herited)  //PMIdimention
                        {
                            NXOpen.Annotations.BalloonNote balloon = NXFun.FindObjectByAttr<NXOpen.Annotations.BalloonNote>("GUID", listitem.guid);
                            if (balloon != null)
                            {
                                NXFun.DeleteBalloonNoteByAttr("GUID", listitem.guid);
                            }
                            NXFun.SetStringAttr((NXObject)listitem.dimParent, "DISP", listitem.isLabel);
                            NXFun.SetIntegerAttr((NXObject)listitem.dimParent, "ID", listitem.id);
                        }
                        else               //普通dimention
                        {
                            //找到IdSymbol 并改写 否则添加
                            NXOpen.Annotations.IdSymbol idsymbol = NXFun.FindObjectByAttr<NXOpen.Annotations.IdSymbol>("GUID", listitem.guid);
                            if (idsymbol != null)
                            {
                                NXFun.DeleteIdSymbolByAttr("GUID", listitem.guid);
                            }
                            NXFun.SetStringAttr((NXObject)listitem.dimParent, "DISP", listitem.isLabel);
                            NXFun.SetIntegerAttr((NXObject)listitem.dimParent, "ID", listitem.id);
                        }
                    }
                }
                foreach (DimInfor listitem in LabelFcfList)
                {
                    string num = "J<C0.5>" + listitem.id.ToString() + "<C>";
                    if (listitem.isLabel == "是")
                    {
                        if (listitem.is_herited)  //Fcf
                        {
                            NXOpen.Annotations.BalloonNote balloon = NXFun.FindObjectByAttr<NXOpen.Annotations.BalloonNote>("GUID", listitem.guid);
                            if (balloon == null)
                            {
                                //没找到 添加
                                NXFun.SetDisplayState(1);
                                NXOpen.Annotations.BalloonNote balloonnote = NXFun.AddBalloonNote((NXOpen.Annotations.Annotation)listitem.dim, num, listitem);
                                NXFun.SetDisplayState(2);
                                //找到二维继承的BalloonNote并设置大小。
                                NXOpen.Annotations.PmiAttribute[] bns = Session.GetSession().Parts.Work.PmiManager.PmiAttributes.ToArray();
                                foreach (NXOpen.Annotations.PmiAttribute pmi in bns)
                                {
                                    NXOpen.Annotations.Lettering Lettering = balloonnote.GetLetteringPreferences().GetDimensionText();
                                    if (NXFun.GetInheritParent(pmi).Equals(balloonnote) && !pmi.Equals(balloonnote))
                                    {
                                        NXFun.EditBalloonNoteSize((NXOpen.Annotations.BalloonNote)pmi, Lettering);
                                    }
                                }
                            }
                            else
                            {
                                //找到 改写
                                NXFun.EditBalloonNote(balloon, num, listitem);
                            }
                            NXFun.SetStringAttr((NXObject)listitem.dimParent, "DISP", listitem.isLabel);
                            NXFun.SetIntegerAttr((NXObject)listitem.dimParent, "ID", listitem.id);
                        }
                        else               //普通DraftingFcf
                        {
                            //找到IdSymbol 并改写 否则添加
                            NXOpen.Annotations.IdSymbol idsymbol = NXFun.FindObjectByAttr<NXOpen.Annotations.IdSymbol>("GUID", listitem.guid);
                            if (idsymbol == null)
                            {
                                //没找到 添加
                                NXFun.AddIdSymbol((NXOpen.Annotations.Annotation)listitem.dim, num, listitem);
                            }
                            else
                            {
                                //找到 改写
                                NXFun.EditIdSymbol(idsymbol, num, listitem);
                            }
                            NXFun.SetStringAttr((NXObject)listitem.dimParent, "DISP", listitem.isLabel);
                            NXFun.SetIntegerAttr((NXObject)listitem.dimParent, "ID", listitem.id);
                        }
                    }
                    else if (listitem.isLabel == "否")
                    {
                        if (listitem.is_herited)  //Fcf
                        {
                            NXOpen.Annotations.BalloonNote balloon = NXFun.FindObjectByAttr<NXOpen.Annotations.BalloonNote>("GUID", listitem.guid);
                            if (balloon != null)
                            {
                                NXFun.DeleteBalloonNoteByAttr("GUID", listitem.guid);
                            }
                            NXFun.SetStringAttr((NXObject)listitem.dimParent, "DISP", listitem.isLabel);
                            NXFun.SetIntegerAttr((NXObject)listitem.dimParent, "ID", listitem.id);
                        }
                        else               //DraftingFcf
                        {
                            //找到IdSymbol 并改写 否则添加
                            NXOpen.Annotations.IdSymbol idsymbol = NXFun.FindObjectByAttr<NXOpen.Annotations.IdSymbol>("GUID", listitem.guid);
                            if (idsymbol != null)
                            {
                                NXFun.DeleteIdSymbolByAttr("GUID", listitem.guid);
                            }
                            NXFun.SetStringAttr((NXObject)listitem.dimParent, "DISP", listitem.isLabel);
                            NXFun.SetIntegerAttr((NXObject)listitem.dimParent, "ID", listitem.id);
                        }
                    }
                }
                Program.theSession.UpdateManager.DoUpdate(undomark);
            }
            catch (System.Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            }

        }
        //尺寸选择发生改变
        private void dgvChicun_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvChicun.SelectedRows.Count == 0)
            {
                NXFun.SetHighlight(oldhighlight, false);
            }
            else
            {
                dgvXingwei.ClearSelection();
                NXFun.SetHighlight(oldhighlight, false);
                int index = dgvChicun.SelectedRows[0].Index;
                NXFun.SetHighlight(LabelDimList[index].dim, true);
                oldhighlight = LabelDimList[index].dim;
            }
        }
        //形位选择发生改变
        private void dgvXingwei_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvXingwei.SelectedRows.Count == 0)
            {
                NXFun.SetHighlight(oldhighlight, false);
            }
            else
            {
                dgvChicun.ClearSelection();
                NXFun.SetHighlight(oldhighlight, false);
                int index = dgvXingwei.SelectedRows[0].Index;
                NXFun.SetHighlight(LabelFcfList[index].dim, true);
                oldhighlight = LabelFcfList[index].dim;
            }
        }
        //计时器，多线程执行全选宏操作
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (((Timer)sender).Tag.ToString() == "全选")
                {
                    timer1.Stop();
                    ObjectsInView = NXFun.GetSelectObjects();
                    NXFun.RemoveAllSelect();
                    timer1.Interval = 1000;
                    timer1.Tag = "取消全选";
                    timer1.Start();
                }
                else if (((Timer)sender).Tag.ToString() == "取消全选")
                {
                    timer1.Stop();
                    //遍历尺寸
                    List<NXOpen.Annotations.Dimension> dims = NXFun.GetDimensionInCurrentView(ObjectsInView);
                    List<NXOpen.Annotations.BaseFcf> fcfs = NXFun.GetBaseFcfInCurrentView(ObjectsInView);
                    LabelDimMap.Clear();
                    foreach (NXOpen.Annotations.Dimension dim in dims)
                    {
                        DimInfor diminfor = NXFun.GetDimInfor(dim);
                        //存入Dictionary
                        LabelDimMap.Add(diminfor.id, diminfor);
                    }
                    LabelFcfMap.Clear();
                    foreach (NXOpen.Annotations.BaseFcf fcf in fcfs)
                    {
                        DimInfor diminfor = NXFun.GetFcfInfor(fcf);
                        LabelFcfMap.Add(diminfor.id, diminfor);
                    }
                    //转存到  LabelDimList
                    LabelDimList.Clear();
                    foreach (int k in LabelDimMap.Keys)
                    {
                        foreach (DimInfor diminfor in LabelDimMap[k])
                        {
                            LabelDimList.Add(diminfor);
                        }
                    }
                    //转存到  LabelFcfList
                    LabelFcfList.Clear();
                    foreach (int k in LabelFcfMap.Keys)
                    {
                        foreach (DimInfor diminfor in LabelFcfMap[k])
                        {
                            LabelFcfList.Add(diminfor);
                        }
                    }
                    //刷新显示
                    RefreshDisplay();
                    SetStatusLabel("查询结束！", 2);
                }
            }
            catch (System.Exception ex)
            {
                NXFun.MessageBox(ex.Message);
            }
        }
        //尺寸起始编号改变
        private void tsrtxtChicun_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int i = System.Convert.ToInt32(tsrtxtChicun.Text);
                DimStartNum = i;
                RefreshDisplay();
            }
            catch (System.Exception ex)
            {
                NXFun.MessageBox(ex.Message);
            }
        }
        //形位起始编号改变
        private void tsrtxtXingwei_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int i = System.Convert.ToInt32(tsrtxtXingwei.Text);
                FcfStartNum = i;
                RefreshDisplay();
            }
            catch (System.Exception ex)
            {
                NXFun.MessageBox(ex.Message);
            }
        }
        //尺寸打标checkbox
        private void dgvChicun_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (bieluandong)
            {
                return;
            }
            if (e.ColumnIndex == 5)  //标号
            {
                int sel = e.RowIndex;
                //修改这项并移动
                if ((bool)dgvChicun[e.ColumnIndex, e.RowIndex].Value)
                {
                    DimInfor temp = LabelDimList[sel];
                    if (temp.isLabel == "是")
                    {
                        //本来就是 是  一般不会出现这种情况
                    }
                    else
                    {
                        for (int i = sel; i >= 0; i--)
                        {
                            //移动到最末一个是的后面
                            if (LabelDimList[i].isLabel == "是")
                            {
                                DimInfor temp1 = LabelDimList[i + 1];
                                NXFun.Swap<DimInfor>(ref temp1, ref temp);
                                temp1.isLabel = "是";
                                LabelDimList[sel] = temp;
                                LabelDimList[i + 1] = temp1;
                                RefreshDisplay();
                                //恢复选中
                                dgvChicun.Rows[i + 1].Selected = true;
                                break;
                            }
                            //没有 是 移动到顶
                            else if (i == 0 && LabelDimList[i].isLabel == "否")
                            {
                                DimInfor temp1 = LabelDimList[0];
                                NXFun.Swap<DimInfor>(ref temp1, ref temp);
                                temp1.isLabel = "是";
                                LabelDimList[sel] = temp;
                                LabelDimList[0] = temp1;
                                RefreshDisplay();
                                //恢复选中
                                dgvChicun.Rows[0].Selected = true;
                                break;
                            }
                        }
                    }

                }
                else
                {
                    DimInfor temp = LabelDimList[sel];
                    if (temp.isLabel == "否")
                    {
                        //本来就是 否  一般不会出现这种情况
                    }
                    else
                    {
                        temp.isLabel = "否";
                        LabelDimList.RemoveAt(sel);
                        LabelDimList.Add(temp);
                        //移动到末
                        RefreshDisplay();
                        //恢复选中
                        dgvChicun.Rows[LabelDimList.Count - 1].Selected = true;
                    }
                }
            }
        }
        //形位打标checkbox
        private void dgvXingwei_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (bieluandong)
            {
                return;
            }
            if (e.ColumnIndex == 3)  //标号
            {
                int sel = e.RowIndex;
                //修改这项并移动
                if ((bool)dgvXingwei[e.ColumnIndex, e.RowIndex].Value)
                {
                    DimInfor temp = LabelFcfList[sel];
                    if (temp.isLabel == "是")
                    {
                        //本来就是 是  一般不会出现这种情况
                    }
                    else
                    {
                        for (int i = sel; i >= 0; i--)
                        {
                            //移动到最末一个是的后面
                            if (LabelFcfList[i].isLabel == "是")
                            {
                                DimInfor temp1 = LabelFcfList[i + 1];
                                NXFun.Swap<DimInfor>(ref temp1, ref temp);
                                temp1.isLabel = "是";
                                LabelFcfList[sel] = temp;
                                LabelFcfList[i + 1] = temp1;
                                RefreshDisplay();
                                //恢复选中
                                dgvXingwei.Rows[i + 1].Selected = true;
                                break;
                            }
                            //没有 是 移动到顶
                            else if (i == 0 && LabelFcfList[i].isLabel == "否")
                            {
                                DimInfor temp1 = LabelFcfList[0];
                                NXFun.Swap<DimInfor>(ref temp1, ref temp);
                                temp1.isLabel = "是";
                                LabelFcfList[sel] = temp;
                                LabelFcfList[0] = temp1;
                                RefreshDisplay();
                                //恢复选中
                                dgvXingwei.Rows[0].Selected = true;
                                break;
                            }
                        }
                    }

                }
                else
                {
                    DimInfor temp = LabelFcfList[sel];
                    if (temp.isLabel == "否")
                    {
                        //本来就是 否  一般不会出现这种情况
                    }
                    else
                    {
                        temp.isLabel = "否";
                        LabelFcfList.RemoveAt(sel);
                        LabelFcfList.Add(temp);
                        //移动到末
                        RefreshDisplay();
                        //恢复选中
                        dgvXingwei.Rows[LabelFcfList.Count - 1].Selected = true;
                    }
                }
            }
        }
        //为了使GridView中CheckBox即时更改
        private void dgvChicun_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvChicun.IsCurrentCellDirty)
            {
                dgvChicun.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }

        }
        //为了使GridView中CheckBox即时更改
        private void dgvXingwei_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvXingwei.IsCurrentCellDirty)
            {
                dgvXingwei.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
    }
}
