using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using NXOpen;

namespace TDPPM
{
    public partial class ChartEditPanel : EditPanel
    {
        private List<S_Sheet> SheetList = new List<S_Sheet>();  //存储所有图纸页信息
        private List<S_Symbol> symbolGroup = new List<S_Symbol>();      //符号信息
        private int pub_a; //当前正在填写的图纸所在工序   0是工艺 
        private double KGB = 0.7;  //默认宽高比
        private double wordwidth = 1.17;  //每个一号字占用的大小
        bool bieluandong = true;
        //构造函数
        public ChartEditPanel()
        {
            InitializeComponent();
        }
        //初始化
        public void Init()
        {
            if (mainDlg == null || string.IsNullOrEmpty(ProPath) || string.IsNullOrEmpty(ProName))
            {
                //缺少初值，所有控件禁用  正常情况不应执行到这里
                tsrFill.Enabled = false;
                tsrPDF.Enabled = false;
                dgvChart.Enabled = false;
            }
            else
            {
                XmlFile = ToFullPath(NXFun.ProcessXML);
                IsEdit = false;
                tsrFill.Enabled = true;
                tsrPDF.Enabled = true;
                dgvChart.Enabled = true;
                //设置自定义符号
                SetSBF();
                //读取symbol
                symbolGroup = XML3DPPM.GetAllSymbols(NXFun.TDPPMPath + NXFun.SymbolXML);
                bieluandong = true;
                RefreshSheet();
            }
        }
        //设置自定义符号
        private void SetSBF()
        {
            string sbf_file = NXFun.TDPPMPath + NXFun.SBFFile;
            if (!NXFun.isFileExist(sbf_file))
            {
                SetStatusLabel("SBF符号文件丢失,基准符号可能无法正常显示!", 1);
            }
            else
            {
                Program.theUfSession.Drf.SetSpecifiedSbfFile(sbf_file);
            }
        }
        //刷新显示图纸
        public void RefreshSheet()
        {
            bieluandong = true;
            SheetList.Clear();
            //得到所有prt
            List<S_Model> ModelList = XML3DPPM.GetModelList(XmlFile);
            //得到所有DrawingSheet
            foreach (S_Model model in ModelList)
            {
                if (model.a == 0 && !string.IsNullOrEmpty(model.filename) && NXFun.isFileExist(ToFullPath(model.filename)))
                {
                    //后台打开模型
                    Part part = NXFun.OpenPrtQuiet(ToFullPath(model.filename));
                    //得到DrawingSheet信息
                    MultiMap<int, S_Sheet> SheetMap = new MultiMap<int, S_Sheet>();
                    foreach (NXOpen.Drawings.DrawingSheet ds in part.DrawingSheets)
                    {
                        S_Sheet s_sheet = NXFun.GetSheetTemplet(ds);
                        s_sheet.a = model.b;
                        s_sheet.prtName = model.filename;
                        SheetMap.Add(s_sheet.index_num, s_sheet);
                    }
                    //按index_num排序
                    int num = 1;
                    S_Sheet temp;
                    foreach (int k in SheetMap.Keys)
                    {
                        foreach (S_Sheet s in SheetMap[k])
                        {
                            temp = s;
                            temp.index_num = num;
                            NXFun.SetStringAttr(NXFun.GetSheetByGuid(ToFullPath(model.filename), s.guid), "NUMBER", num.ToString());
                            SheetList.Add(temp);
                            num++;
                        }
                    }
                }
            }
            dgvChart.Rows.Clear();
            //显示到界面
            for (int i = 0; i < SheetList.Count; i++)
            {
                string name = "";
                if (SheetList[i].a == 0)
                {
                    name = "工艺";
                }
                else
                {
                    name = "工序" + XML3DPPM.GetIndexAttr(SheetList[i].a, 0, "gongxu_gongxuhao", XmlFile);
                }
                dgvChart.Rows.Add(name);
                dgvChart[1, i].Value = SheetList[i].prtName;
                dgvChart[2, i].Value = SheetList[i].Remark;
                dgvChart[3, i].Value = NXFun.string2bool(SheetList[i].IsPrint);
                dgvChart[4, i].Value = NXFun.string2bool(SheetList[i].IsChecked);
            }
            //得到当前显示的 Sheet 并高亮对应行
            //  highlight_row = -1;
            string guid = NXFun.GetOnShowSheetGuid();
            string prt = NXFun.GetWorkPrtName(false);

            int index = SheetList.FindIndex(delegate(S_Sheet s) { return s.guid == guid && s.prtName == prt; });
            if (index != -1)
            {
                for (int i = 0; i < dgvChart.RowCount; i++)
                {
                    dgvChart.Rows[i].DefaultCellStyle.BackColor = i == index ? Color.Yellow : Color.White;
                }
            }
            bieluandong = false;
            dgvChart.ClearSelection();
        }
        //刷新显示按钮
        private void tsrbtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshSheet();
        }
        //填写显示按钮
        private void tsrbtnFillShow_Click(object sender, EventArgs e)
        {
            SetSBF();
            //得到当前显示的sheet
            NXOpen.Drawings.DrawingSheet ds = NXFun.GetOnShowSheet();
            if (ds == null)
            {
                SetStatusLabel("没有显示的图表！", 1);
                return;
            }
            //得到当前显示的prt是第几个工序（不是工序号）
            string prt = NXFun.GetOnShowPrtName();
            List<S_Model> ModelList = XML3DPPM.GetModelList(XmlFile);
            S_Model model = ModelList.Find(delegate(S_Model m) { return prt == ToFullPath(m.filename) && m.a == 0; });
            if (model.Equals(default(S_Model)))
            {
                SetStatusLabel("显示的图表不在本工艺下！", 1);
                return;
            }
            pub_a = model.b;
            //得到当前sheet的信息
            //NXFun.OpenPrt(ToFullPath(model.filename));
            string templet = NXFun.GetStringAttr(ds, "TEMPLET");
            string guid = NXFun.GetStringAttr(ds, "GUID");
            if (templet == "NX")
            {
                //NX自带模板，不用管
            }
            else if (!string.IsNullOrEmpty(templet))
            {
                //需要填写哦
                List<S_SheetItem> SheetItemList = XML3DPPM.GetSheetItemList(templet, NXFun.TDPPMPath + NXFun.SheetTempletXML);
                //格式化文本
                List<S_SheetNote> SheetNoteList = SheetItem2SheetNote(ref SheetItemList);
                foreach (S_SheetNote note in SheetNoteList)
                {
                    NXFun.WriteText(note, guid);
                }


                if (templet == "A3-3DPPM-gongxutubiao" || templet == "A4-3DPPM-gongxutubiao" || templet == "A4-3DPPM-gongxubiao")
                {
                    Fill_3DPPM_gongxu(templet);
                }
                else if (templet == "A4-3DPPM-gongyichengxu")
                {
                    Fill_3DPPM_gongyichengxu(templet);
                }
                else if (templet == "A4-3DPPM-gongyizhuangbei")
                {
                    Fill_3DPPM_gongyizhuangbei(templet);
                }
              //  NXFun.SavePrt(ToFullPath(prt));
            }
        }
        //填写所有按钮
        private void tsrbtnFillAll_Click(object sender, EventArgs e)
        {
            RefreshSheet();
            foreach (S_Sheet sheet in SheetList)
            {
                string prt = sheet.prtName;
                string guid = sheet.guid;
                NXFun.ShowSheetByGuid(ToFullPath(prt), guid);
                //得到当前显示图表的guid；
                NXFun.ClearNotesBySheetGuid(ToFullPath(prt), guid);
            }
            foreach (S_Sheet sheet in SheetList)
            {
                string prt = sheet.prtName;
                string guid = sheet.guid;
                NXFun.ShowSheetByGuid(ToFullPath(prt), guid);
                this.tsrbtnFillShow_Click(sender, e);
            }
            RefreshSheet();
            this.tsrbtnFillTitle_Click(sender, e);
        }
        //更新标头按钮
        private void tsrbtnFillTitle_Click(object sender, EventArgs e)
        {
            // TODO: 在此添加控件通知处理程序代码
            RefreshSheet();
            foreach (S_Sheet sheet in SheetList)
            {
                string prt = sheet.prtName;
                string guid = sheet.guid;
                NXFun.ShowSheetByGuid(ToFullPath(prt), guid);
                //得到当前显示图表的guid；
                NXFun.ClearOnShowTitleNotesBySheetGuid(guid);
                FillOnShowTitle();
            }
        }
        //清空显示按钮
        private void tsrbtnClearShow_Click(object sender, EventArgs e)
        {
            //得到当前选择的sheet的guid和prt
            string guid = NXFun.GetOnShowSheetGuid();
            if (string.IsNullOrEmpty(guid))
            {
                SetStatusLabel("得到当前显示图表出错！如果本图表是新建图表，请点刷新图表按钮！", 1);
                return;
            }
            string prtName = SheetList.Find(delegate(S_Sheet s) { return s.guid == guid; }).prtName;
            //清空prt里SheetGuid为guid的note
            NXFun.ClearNotesBySheetGuid(ToFullPath(prtName), guid);
        }
        //上移按钮
        private void tsrbtnMoveUp_Click(object sender, EventArgs e)
        {
            if (dgvChart.SelectedRows.Count == 0)
            {
                SetStatusLabel("请选定某一行", 1);
                return;
            }
            int OnSelItem = dgvChart.SelectedRows[0].Index;
            string prt1, prt2, guid1, guid2;
            //得到当前选中的guid
            if (OnSelItem >= 0 && OnSelItem < SheetList.Count)
            {
                prt1 = ToFullPath(SheetList[OnSelItem].prtName);
                guid1 = SheetList[OnSelItem].guid;
            }
            else
            {
                return;
            }
            //得到上个guid
            if (OnSelItem - 1 >= 0 && OnSelItem - 1 < SheetList.Count)
            {
                prt2 = ToFullPath(SheetList[OnSelItem - 1].prtName);
                guid2 = SheetList[OnSelItem - 1].guid;
            }
            else
            {
                return;
            }
            //如果不属于同一prt 结束
            if (prt2 != prt1)
            {
                return;
            }
            //置换两个guid的number
            NXOpen.Drawings.DrawingSheet st1 = NXFun.GetSheetByGuid(prt1, guid1);
            NXOpen.Drawings.DrawingSheet st2 = NXFun.GetSheetByGuid(prt2, guid2);
            string num1 = NXFun.GetStringAttr(st1, "Number");
            string num2 = NXFun.GetStringAttr(st2, "Number");
            NXFun.SetStringAttr(st1, "Number", num2);
            NXFun.SetStringAttr(st2, "Number", num1);
            //刷新
            int index = OnSelItem;
            RefreshSheet();
            if (index - 1 >= 0 && index - 1 < SheetList.Count)
            {
                dgvChart.Rows[index - 1].Selected = true;
            }
        }
        //下移按钮
        private void tsrbtnMoveDown_Click(object sender, EventArgs e)
        {
            if (dgvChart.SelectedRows.Count == 0)
            {
                SetStatusLabel("请选定某一行", 1);
                return;
            }
            int OnSelItem = dgvChart.SelectedRows[0].Index;
            string prt1, prt2, guid1, guid2;
            //得到当前选中的guid
            if (OnSelItem >= 0 && OnSelItem < SheetList.Count)
            {
                prt1 = ToFullPath(SheetList[OnSelItem].prtName);
                guid1 = SheetList[OnSelItem].guid;
            }
            else
            {
                return;
            }
            //得到下个guid
            if (OnSelItem + 1 >= 0 && OnSelItem + 1 < SheetList.Count)
            {
                prt2 = ToFullPath(SheetList[OnSelItem + 1].prtName);
                guid2 = SheetList[OnSelItem + 1].guid;
            }
            else
            {
                return;
            }
            //如果不属于同一prt 结束
            if (prt2 != prt1)
            {
                return;
            }
            //置换两个guid的number
            NXOpen.Drawings.DrawingSheet st1 = NXFun.GetSheetByGuid(prt1, guid1);
            NXOpen.Drawings.DrawingSheet st2 = NXFun.GetSheetByGuid(prt2, guid2);
            string num1 = NXFun.GetStringAttr(st1, "Number");
            string num2 = NXFun.GetStringAttr(st2, "Number");
            NXFun.SetStringAttr(st1, "Number", num2);
            NXFun.SetStringAttr(st2, "Number", num1);
            //刷新
            int index = OnSelItem;
            RefreshSheet();
            if (index + 1 >= 0 && index + 1 < SheetList.Count)
            {
                dgvChart.Rows[index + 1].Selected = true;
            }
        }
        //帮助按钮
        private void strbtnHelp_Click(object sender, EventArgs e)
        {
            string help =
                    "注意事项：\n" +
                    "1、新建或者删除图纸后，请点击刷新图表按钮；\n" +
                    "2、刷新图表后，黄色高亮显示的为当前显示的图表；\n" +
                    "3、双击“名称”或“所在模型”列可打开对应图纸；\n" +
                    "4、填写某页图纸时请先清空图纸。\n" +
                    "二维图表工具 V1.0";
            NXFun.MessageBox(help, "帮助", 1);
        }
        //本页PDF预览按钮
        private void tsrbtnPDFPreviewOne_Click(object sender, EventArgs e)
        {
            S_PDF pdf;
            pdf.path = ToFullPath("single.pdf");
            NXOpen.Drawings.DrawingSheet sheet = NXFun.GetOnShowSheet(); //得到当前显示的图表
            if (sheet == null)
            {
                NXFun.MessageBox("没有显示的图纸", "信息", 1);
                return;
            }
            pdf.sheet = sheet;
            pdf.append = false;
            NXFun.PrintPDF(pdf);
            NXFun.OpenFile(pdf.path);
        }
        //工艺PDF生成按钮
        private void tsrbtnPDFCreateAll_Click(object sender, EventArgs e)
        {
            S_PDF pdf;
            pdf.path = ToFullPath("whole.pdf");
            pdf.append = false;
            List<NXOpen.Drawings.DrawingSheet> sheets = new List<NXOpen.Drawings.DrawingSheet>();
            foreach (S_Sheet sheet in SheetList)
            {
                if (sheet.IsPrint == "是")
                {
                    string prt = ToFullPath(sheet.prtName);
                    string guid = sheet.guid;
                    NXOpen.Drawings.DrawingSheet ds = NXFun.ShowSheetByGuid(prt, guid);
                    if (ds == null)
                    {
                        NXFun.MessageBox("检测到不可用图纸信息，可能已被删除，请重新刷新图标并更新表头", "错误", 0);
                        return;
                    }
                    pdf.sheet = ds;
                    NXFun.PrintPDF(pdf);
                    pdf.append = true;
                }
            }
            NXFun.OpenFile(pdf.path);
        }
        //工艺PDF预览
        private void tsrbtnPDFPreviewAll_Click(object sender, EventArgs e)
        {
            string pdfPath = ToFullPath("whole.pdf");
            if (!NXFun.isFileExist(pdfPath))
            {
                UI.GetUI().NXMessageBox.Show("提示", NXMessageBox.DialogType.Warning, "请先生成工艺pdf");
                return;
            }
            else
            {
                NXFun.OpenFile(pdfPath);
            }
        }
        //工艺PDF导出按钮
        private void tsrbtnPDFOutputAll_Click(object sender, EventArgs e)
        {
            string pdfPath = ToFullPath("whole.pdf");
            if (!NXFun.isFileExist(pdfPath))
            {
                NXFun.MessageBox("请先生成工艺", "提示", 1);
                return;
            }
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.Title = "导出工艺PDF文件";
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.Filter = "PDF文件(*.pdf)|*.pdf";
            saveFileDialog1.OverwritePrompt = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                System.IO.File.Copy(pdfPath, filename, true);
            }
        }
        //双击dgvChart
        private void dgvChart_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex > 1)
            {
                return;
            }
            S_Sheet s_sheet = SheetList[e.RowIndex];
            NXFun.ShowSheetByGuid(ToFullPath(s_sheet.prtName), s_sheet.guid);
            //高亮背景显示当前打开的sheet
            for (int i = 0; i < dgvChart.RowCount; i++)
            {
                dgvChart.Rows[i].DefaultCellStyle.BackColor = i == e.RowIndex ? Color.Yellow : Color.White;
            }
        }
        //dgvChart单元格改变
        private void dgvChart_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (bieluandong)
            {
                return;
            }
            if (e.ColumnIndex == 2) //备注
            {
                S_Sheet s_sheet = SheetList[e.RowIndex];
                s_sheet.Remark = (string)(dgvChart[e.ColumnIndex, e.RowIndex].Value);
                SheetList[e.RowIndex] = s_sheet;
                //设置属性
                NXOpen.Drawings.DrawingSheet ds = NXFun.GetSheetByGuid(ToFullPath(SheetList[e.RowIndex].prtName), SheetList[e.RowIndex].guid);
                if (ds == null)
                {
                    UI.GetUI().NXMessageBox.Show("提示", NXMessageBox.DialogType.Error, "得到图纸出错，如果有删除图纸或删除模型文件，请点击刷新图纸按钮！");
                    return;
                }
                else
                {
                    NXFun.SetStringAttr(ds, "REMARK", s_sheet.Remark);
                }
            }
            else if (e.ColumnIndex == 3)    //打印
            {
                S_Sheet s_sheet = SheetList[e.RowIndex];
                s_sheet.IsPrint = NXFun.bool2string((bool)(dgvChart[e.ColumnIndex, e.RowIndex].Value));
                SheetList[e.RowIndex] = s_sheet;
                //设置属性
                NXOpen.Drawings.DrawingSheet ds = NXFun.GetSheetByGuid(ToFullPath(SheetList[e.RowIndex].prtName), SheetList[e.RowIndex].guid);
                if (ds == null)
                {
                    UI.GetUI().NXMessageBox.Show("提示", NXMessageBox.DialogType.Error, "得到图纸出错，如果有删除图纸或删除模型文件，请点击刷新图纸按钮！");
                    return;
                }
                else
                {
                    NXFun.SetStringAttr(ds, "ISPRINT", s_sheet.IsPrint);
                }
            }
            else if (e.ColumnIndex == 4)    //通过
            {
                S_Sheet s_sheet = SheetList[e.RowIndex];
                s_sheet.IsChecked = NXFun.bool2string((bool)(dgvChart[e.ColumnIndex, e.RowIndex].Value));
                SheetList[e.RowIndex] = s_sheet;
                //设置属性
                NXOpen.Drawings.DrawingSheet ds = NXFun.GetSheetByGuid(ToFullPath(SheetList[e.RowIndex].prtName), SheetList[e.RowIndex].guid);
                if (ds == null)
                {
                    UI.GetUI().NXMessageBox.Show("提示", NXMessageBox.DialogType.Error, "得到图纸出错，如果有删除图纸或删除模型文件，请点击刷新图纸按钮！");
                    return;
                }
                else
                {
                    NXFun.SetStringAttr(ds, "ISCHECKED", s_sheet.IsChecked);
                }
            }
        }
        //按长度分割字符串
        private string[] SplitStringByLength(string str, int length)
        {
            List<string> v_str = new List<string>();
            string a = ""; //存储单行
            string b = NXFun.Reverse(str);//存储多行
            //b头往a尾放，直到a有效并且长度大于length 再a尾往b头放 直到a有效并长度不大于length
            while (b.Length != 0)
            {
                char w = b[b.Length - 1];
                a = a + w;
                b = b.Remove(b.Length - 1);
                if (IsStringValid(a) && GetNXLength(a) > (double)length)
                {
                    //a往b回放
                    do
                    {
                        char w2 = a[a.Length - 1];
                        b = b + w2;
                        a = a.Remove(a.Length - 1);
                    } while (!IsStringValid(a) || GetNXLength(a) > (double)length);
                    v_str.Add(a);
                    a = "";
                    continue;
                }
            }
            //最后一个放进去呗
            v_str.Add(a);
            return NXFun.List2Array<string>(v_str);
        }
        //分号分割字符串
        private string[] SplitStringBySemicolon(string str, bool CanBeEmpty)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            string[] v_str = str.Split(";；".ToCharArray());
            if (v_str.Length == 0 && !CanBeEmpty)
            {
                string[] v = new string[1] { "" };
                return v;
            }
            else
            {
                return v_str;
            }
        }
        //得到工步信息
        private List<S_StepLine> GetStepLines(int a, int b, int num_neirong)
        {
            S_StepLine s_line;
            string num, neirong, renju, liangju, beizhu;
            num = b.ToString();
            neirong = XML3DPPM.GetIndexAttr(a, b, "gongbu_gongbuneirong", XmlFile);
            renju = XML3DPPM.GetIndexAttr(a, b, "gongbu_renju", XmlFile);
            liangju = XML3DPPM.GetIndexAttr(a, b, "gongbu_liangju", XmlFile);
            beizhu = XML3DPPM.GetIndexAttr(a, b, "gongbu_beizhu", XmlFile);
            //分割各项
            string[] v_num = new string[1] { num };
            string[] v_neirong = SplitStringByLength(neirong, num_neirong);
            string[] v_renju = SplitStringBySemicolon(renju, false);
            string[] v_liangju = SplitStringBySemicolon(liangju, false);
            string[] v_beizhu = SplitStringBySemicolon(beizhu, false);
            //得到最长项
            int lines = NXFun.GetMaxLength<string>(new string[][] { v_num, v_neirong, v_renju, v_liangju, v_beizhu });
            //写入表格
            List<S_StepLine> result = new List<S_StepLine>();
            S_StepLine[] steplines = new S_StepLine[lines];
            for (int i = 0; i < lines; i++)
            {
                if (v_num == null)
                   steplines[i].num = "";
                else
                   steplines[i].num = i < v_num.Length ? v_num[i] : "";

                if (v_neirong == null)
                    steplines[i].neirong = "";
                else
                    steplines[i].neirong = i < v_neirong.Length ? v_neirong[i] : "";

                if (v_renju == null)
                    steplines[i].renju = "";
                else
                    steplines[i].renju = i < v_renju.Length ? v_renju[i] : "";

                if (v_liangju == null)
                    steplines[i].liangju = "";
                else
                    steplines[i].liangju = i < v_liangju.Length ? v_liangju[i] : "";

                if (v_beizhu == null)
                    steplines[i].beizhu = "";
                else
                    steplines[i].beizhu = i < v_beizhu.Length ? v_beizhu[i] : "";
            }
            foreach (S_StepLine s in steplines)
            {
                result.Add(s);
            }
            //得到子工步内容
            S_ChildStep[] childsteps = XML3DPPM.GetChildStepList(a, b, XmlFile);
            foreach (S_ChildStep iter in childsteps)
            {
                s_line.num = "";
                s_line.neirong = iter.name;
                s_line.renju = iter.renju;
                s_line.liangju = iter.liangju;
                s_line.beizhu = iter.beizhu;
                if (!string.IsNullOrEmpty(s_line.num)|| 
                    !string.IsNullOrEmpty(s_line.neirong)|| 
                    !string.IsNullOrEmpty(s_line.renju)||
                    !string.IsNullOrEmpty(s_line.liangju)|| 
                    !string.IsNullOrEmpty(s_line.beizhu))
                {
                    result.Add(s_line);
                }
            }
            return result;
        }
        //填写工序
        private void Fill_3DPPM_gongxu(string templet)
        {
            string guid = NXFun.GetOnShowSheetGuid();
            List<S_StepLine> table = new List<S_StepLine>();
            S_StepLine stepline;
            //得到工步数
            int num_step = XML3DPPM.GetGongbuCount(pub_a, XmlFile);

            //得到表格宽度和行数
            List<S_SheetItem> item_list = XML3DPPM.GetSheetItemList(templet, NXFun.TDPPMPath + NXFun.SheetTempletXML);
            if (item_list.Count == 0)
            {
                NXFun.MessageBox("得到图纸模板信息出错！", "错误", 0);
                return;
            }
            double y0 = 0, y_first = 0, y_add = 0, x0 = 0, x1 = 0, x2 = 0, x3 = 0, x4 = 0, x5 = 0, height = 0, size = 0;
            int lines_first = 0, lines_add = 0;
            string font = "";
            /*
            0----1----2----3----4----5     y_first  gongxutubiao 首行高  y_add gongxubiao 首行高
            |项目|内容|刃具|量具|备注      height
            -------------------------->    y0 
            */
            S_SheetItem item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_xiangmu"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板项目信息出错！", "错误", 0);
                return;
            }
            x0 = item.x0;
            x1 = item.x1;
            y_first = item.y0;
            y0 = item.y1;
            lines_first = item.multiline;
            height = y_first - y0;
            font = item.font;
            size = item.size;

            item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_gongzuoneirong"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板工作内容信息出错！", "错误", 0);
                return;
            }
            x2 = item.x1;

            item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_renju"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板刃具信息出错！", "错误", 0);
                return;
            }
            x3 = item.x1;

            item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_liangju"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板量具信息出错！", "错误", 0);
                return;
            }
            x4 = item.x1;

            item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_beizhu"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板备注信息出错！", "错误", 0);
                return;
            }
            x5 = item.x1;
            //计算工作内容能填写的字符数
            int num_neirong = (int)((x2 - x1) / wordwidth / size / KGB);
            int num_renju = (int)((x3 - x2) / wordwidth / size / KGB);
            int num_liangju = (int)((x4 - x3) / wordwidth / size / KGB);
            int num_beizhu = (int)((x5 - x4) / wordwidth / size / KGB);
            //得到加工前准备
            string prepare = XML3DPPM.GetIndexAttr(pub_a, 0, "gongxu_jiagongzhunbei", XmlFile);
            if (!string.IsNullOrEmpty(prepare))
            {
                string[] v_prepare = SplitStringByLength(prepare, num_neirong);
                stepline.beizhu = "";
                stepline.num = "";
                stepline.liangju = "";
                stepline.renju = "";
                stepline.neirong = "      加工前准备";
                table.Add(stepline);
                foreach (string s in v_prepare)
                {
                    stepline.neirong = s;
                    table.Add(stepline);
                }
                stepline.neirong = "";
                table.Add(stepline);
            }
            //得到工步
            if (num_step > 0)
            {
                //s_line.neirong = "";
                //table.push_back(s_line);
                //s_line.neirong = "       工作内容";
                //table.push_back(s_line);
            }
            for (int i = 0; i < num_step; i++)
            {
                List<S_StepLine> step_lines = GetStepLines(pub_a, i + 1, num_neirong);
                table.AddRange(step_lines);
                //每个工步中间加空行
                stepline.beizhu = "";
                stepline.num = "";
                stepline.liangju = "";
                stepline.renju = "";
                stepline.neirong = "";
                if (i != num_step-1)
                {
                    table.Add(stepline);
                }                
            }
            //判断需要页数
            int pages = 1;
            string AddTempletName = "";
            if (table.Count > lines_first)
            {
                //需要换页

                if (templet == "A3-3DPPM-gongxutubiao")
                {
                    AddTempletName = "A3-3DPPM-gongxutubiao";
                }
                else if (templet == "A4-3DPPM-gongxutubiao")
                {
                    AddTempletName = "A4-3DPPM-gongxubiao";
                }
                else if (templet == "A4-3DPPM-gongxubiao")
                {
                    AddTempletName = "A4-3DPPM-gongxubiao";
                }
                else
                {
                    NXFun.MessageBox("得到附页出错！");
                    return;
                }
                item_list = XML3DPPM.GetSheetItemList(AddTempletName, NXFun.TDPPMPath + NXFun.SheetTempletXML);
                foreach (S_SheetItem i in item_list)
                {
                    if (i.text.Contains("_xiangmu"))
                    {
                        y_add = i.y0;
                        lines_add = i.multiline;
                    }
                }
                pages = (table.Count - lines_first - 1) / lines_add + 2;
            }
            //查询或创建图纸续页
            List<string> guid_list = new List<string>();
            guid_list.Add(guid);
            if (pages > 1)
            {
                //标识方法：图纸模板名称用A4-3DPPM-gongxubiao-add
                //fatherguid:母模板
                //addnum: 1 2 3 ……
                for (int i = 1; i < pages; i++)
                {
                    string guid_add = NXFun.FindAddSheetGuid(guid, i, AddTempletName + "-add");
                    if (string.IsNullOrEmpty(guid_add))
                    {
                        return;
                    }
                    guid_list.Add(guid_add);
                }
            }
            //开始填表啦！！！！累死了…… QQ交友:47965258
            S_SheetNote note;
            note.x = 0;
            note.y = 0;
            note.pos = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;
            note.CharSpaceFactor = 0.01;
            note.LineSpaceFactor = 1.6;
            note.TextFont = font;
            note.TextSize = size;
            note.AspectRatio = KGB;
            note.flag = "";
            string[] text = new string[1];

            for (int i = 0; i < pages; i++)
            {
                NXFun.ShowSheetByGuid(guid_list[i]);
                //每页要写的行数
                int lines = i == 0 ? Math.Min((int)table.Count, lines_first) : Math.Min((int)table.Count - lines_first - (i - 1) * lines_add, lines_add);
                int index = 0;//table的索引号
                for (int j = 0; j < lines; j++)
                {
                    note.y = (i == 0 ? y_first : y_add) - height / 2 - height * j;
                    //写项目
                    index = (i == 0) ? (j) : (j + lines_first + lines_add * (i - 1));
                    if (!string.IsNullOrEmpty(table[index].num))
                    {
                        note.AspectRatio = KGB;
                        string onshow_guid = NXFun.GetOnShowSheetGuid();
                        note.x = (x0 + x1) / 2;
                        note.pos = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;
                        text[0] = table[index].num;
                        note.text = text;
                        NXFun.WriteText(note, guid_list[i]);
                    }
                    //写工作内容
                    if (!string.IsNullOrEmpty(table[index].neirong))
                    {
                        note.AspectRatio = KGB;
                        note.x = x1 + 2;
                        note.pos = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidLeft;
                        double l = GetNXLength(table[index].neirong);
                        text[0] = (l > 0) ? Change2NX(table[index].neirong) : "";
                        note.text = text;
                        NXFun.WriteText(note, guid_list[i]);
                    }
                    //写刃具
                    if (!string.IsNullOrEmpty(table[index].renju))
                    {
                        note.x = (x2 + x3) / 2;
                        note.pos = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;
                        double l = GetNXLength(table[index].renju);
                        text[0] = (l > 0) ? Change2NX(table[index].renju) : "";
                        note.text = text;
                        note.AspectRatio = l > num_renju ? (KGB * num_renju / l) : KGB;
                        NXFun.WriteText(note, guid_list[i]);
                    }
                    //写量具
                    if (!string.IsNullOrEmpty(table[index].liangju))
                    {
                        note.x = (x4 + x3) / 2;
                        note.pos = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;
                        double l = GetNXLength(table[index].liangju);
                        text[0] = (l > 0) ? Change2NX(table[index].liangju) : "";
                        note.text = text;
                        note.AspectRatio = l > num_liangju ? (KGB * num_liangju / l) : KGB;
                        NXFun.WriteText(note, guid_list[i]);
                    }
                    //写备注
                    if (!string.IsNullOrEmpty(table[index].beizhu))
                    {
                        note.x = (x4 + x5) / 2;
                        note.pos = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;
                        double l = GetNXLength(table[index].beizhu);
                        text[0] = (l > 0) ? Change2NX(table[index].beizhu) : "";
                        note.text = text;
                        note.AspectRatio = l > num_beizhu ? (KGB * num_beizhu / l) : KGB;
                        NXFun.WriteText(note, guid_list[i]);
                    }
                }
            }
        }
        //格式化工艺程序表
        private List<S_GongYiChengXu> FormatGongYiChengXuTable(ref List<S_GongYiChengXu> table, int num_gongxumingcheng, int num_shebei, int num_beizhu)
        {
            List<S_GongYiChengXu> NewTable = new List<S_GongYiChengXu>();
            S_GongYiChengXu nu;
            nu.beizhu = "";
            nu.texingfuhao = "";
            nu.gongxumingcheng = "";
            nu.gongxuhao = "";
            nu.shebei = "";
            foreach (S_GongYiChengXu iter in table)
            {
                string[] v_gongxumingcheng = SplitStringByLength(iter.gongxumingcheng, num_gongxumingcheng);
                string[] v_shebei = SplitStringByLength(iter.shebei, num_shebei);
                string[] v_beizhu = SplitStringByLength(iter.beizhu, num_beizhu);
                int lines = NXFun.GetMaxLength<string>(new string[][] { v_gongxumingcheng, v_shebei, v_beizhu });
                S_GongYiChengXu[] gycx = new S_GongYiChengXu[lines];
                for (int i = 0; i < lines; i++)
                {
                    gycx[i].texingfuhao = i < 1 ? iter.texingfuhao : "";
                    gycx[i].gongxuhao = i < 1 ? iter.gongxuhao : "";
                    gycx[i].gongxumingcheng = i < (int)v_gongxumingcheng.Length ? v_gongxumingcheng[i] : "";
                    gycx[i].shebei = i < (int)v_shebei.Length ? v_shebei[i] : "";
                    gycx[i].beizhu = i < (int)v_beizhu.Length ? v_beizhu[i] : "";
                }
                foreach (S_GongYiChengXu iter2 in gycx)
                {
                    NewTable.Add(iter2);
                    NewTable.Add(nu);
                }
            }

            if (NewTable.Count != 0)
            {
                NewTable.RemoveAt(NewTable.Count - 1);
            }
            return NewTable;
        }
        
        //分割
        private List<S_Line_GZTJ> SeparateToLine(ref List<S_Gongxu> gongxu_list)
        {
            //清空line_list
            List<S_Line_GZTJ> line_list = new List<S_Line_GZTJ>();
            foreach (S_Gongxu s in gongxu_list)
            {
                SingleToLine(s, ref line_list);
            }
            return line_list;
        }
        //分割
        private void SingleToLine(S_Gongxu gongxu, ref List<S_Line_GZTJ> line_list)
        {
            S_Line_GZTJ lineNode;
            int jiamoju_num = gongxu.jiamoju_list.Count;
            int renju_num = gongxu.renju_list.Count;
            int liangju_num = gongxu.liangju_list.Count;

            int max_num = Math.Max(Math.Max(jiamoju_num, renju_num), liangju_num);
            int list_num = line_list.Count;
            List<string> jiaju = new List<string>();
            List<string> renju = new List<string>();
            List<string> liangju = new List<string>();
            bool flag1 = false, flag2 = false, flag3 = false;

            if (list_num != 0)
            {
                for (int i = 0; i < max_num; ++i)
                {
                    if (i < jiamoju_num)
                    {
                        flag1 = false;
                        for (int j = 0; j < list_num; ++j)
                        {
                            if (gongxu.jiamoju_list[i] == line_list[j].jiamoju)
                            {
                                line_list[j].chongyong_list1.Add(gongxu.gongxuhao);
                                flag1 = true;
                                break;
                            }
                        }
                        if (!flag1)
                        {
                            jiaju.Add(gongxu.jiamoju_list[i]);
                        }
                    }

                    if (i < renju_num)
                    {
                        flag2 = false;
                        for (int j = 0; j < list_num; ++j)
                        {
                            if (gongxu.renju_list[i] == line_list[j].renju)
                            {
                                line_list[j].chongyong_list2.Add(gongxu.gongxuhao);
                                flag2 = true;
                                break;
                            }
                        }
                        if (!flag2)
                        {
                            renju.Add(gongxu.renju_list[i]);
                        }
                    }

                    if (i < liangju_num)
                    {
                        flag3 = false;
                        for (int j = 0; j < list_num; ++j)
                        {
                            if (gongxu.liangju_list[i] == line_list[j].liangju)
                            {
                                line_list[j].chongyong_list3.Add(gongxu.gongxuhao);
                                flag3 = true;
                                break;
                            }
                        }
                        if (!flag3)
                        {
                            liangju.Add(gongxu.liangju_list[i]);
                        }
                    }
                }
            }
            else
            {
                jiaju = gongxu.jiamoju_list;
                renju = gongxu.renju_list;
                liangju = gongxu.liangju_list;
            }

            jiamoju_num = jiaju.Count;
            renju_num = renju.Count;
            liangju_num = liangju.Count;
            max_num = Math.Max(Math.Max(jiamoju_num, renju_num), liangju_num);
            if (max_num > 0)
            {
                lineNode.chongyong_list1 = new List<string>();
                lineNode.chongyong_list2 = new List<string>();
                lineNode.chongyong_list3 = new List<string>();
                for (int m = 0; m < max_num; ++m)
                {
                    lineNode.gongxuhao = ((m == 0) ? gongxu.gongxuhao : "");
                    lineNode.jiamoju = ((m < jiamoju_num) ? jiaju[m] : "");
                    lineNode.renju = ((m < renju_num) ? renju[m] : "");
                    lineNode.liangju = ((m < liangju_num) ? liangju[m] : "");
                    line_list.Add(lineNode);
                }
                S_Line_GZTJ EmptyNode;
                EmptyNode.chongyong_list1 = new List<string>();
                EmptyNode.chongyong_list2 = new List<string>();
                EmptyNode.chongyong_list3 = new List<string>();
                EmptyNode.gongxuhao = "";
                EmptyNode.jiamoju = "";
                EmptyNode.renju = "";
                EmptyNode.liangju = "";

                line_list.Add(EmptyNode);
                line_list.Add(EmptyNode);
            }
        }
        //连接字符串
        private string JoinVectorString(List<string> v_str)
        {
            List<string> temp = NXFun.RemoveTheSame<string>(v_str);
            string str = "";
            foreach (string s in temp)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    str = str + s + "/";
                }
            }
            if (str.Length != 0)
            {
                str = str.Remove(str.Length - 1);
            }
            return str;
        }
        
        // SheetItem到SheetNote的转换
        private List<S_SheetNote> SheetItem2SheetNote(ref List<S_SheetItem> SheetItemList)
        {
            List<S_SheetNote> SheetNoteList = new List<S_SheetNote>();
            S_SheetNote note;
            foreach (S_SheetItem item in SheetItemList)
            {
                note.AspectRatio = KGB;
                note.CharSpaceFactor = 0.01;
                note.LineSpaceFactor = 1.6;
                note.TextFont = item.font;
                note.TextSize = item.size;
                note.flag = item.text;
                string text = FormatText(item.text);
                double l = GetNXLength(text);
                text = (l > 0) ? Change2NX(text) : "";
                int num = (int)(Math.Abs(item.x0 - item.x1) / wordwidth / item.size / KGB);
                note.AspectRatio = l > num ? (KGB * num / l) : KGB;
                string[] strs = new string[1] { text };
                note.text = strs;
                note.x = (item.x0 + item.x1) / 2;
                note.y = (item.y0 + item.y1) / 2;
                note.pos = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;
                SheetNoteList.Add(note);
            }
            return SheetNoteList;
        }
        //获取NX字符串长
        private double GetNXLength(string inputString)
        {
            if (!IsStringValid(inputString))
            {
                NXFun.MessageBox("字符串" + inputString + "非法", "错误", 0);
                return 0;
            }

            return StringLength(inputString);
        }
        //字符串长
        private double StringLength(string inPutStr)
        {
            if (inPutStr.Length == 0)
            {
                return 0;
            }
            int groupSize = symbolGroup.Count;
            List<int> left = new List<int>();
            List<int> right = new List<int>();
            List<int> stack = new List<int>();
            int tempNum = 0;
            string singleStr = "";
            for (int j = 0; j < inPutStr.Length; ++j)
            {
                singleStr = inPutStr.Substring(j, 1);
                if (singleStr == "[")
                {
                    stack.Add(j);
                }
                else if (singleStr == "]")
                {
                    tempNum = stack[stack.Count - 1];
                    stack.RemoveAt(stack.Count - 1);
                    if (stack.Count == 0)
                    {
                        left.Add(tempNum);
                        right.Add(j);
                    }
                }
            }
            if (left.Count == 0)
            {
                return inPutStr.Length;
            }
            else
            {
                double unSymbolSize = (double)left[0];
                int lRsize = left.Count;
                unSymbolSize += GetSymbolLength(inPutStr.Substring(left[0], right[0] + 1 - left[0]));
                for (int i = 1; i < lRsize; ++i)
                {
                    unSymbolSize += left[i] - right[i - 1] - 1;
                    unSymbolSize += GetSymbolLength(inPutStr.Substring(left[i], right[i] + 1 - left[i]));
                }
                unSymbolSize += inPutStr.Length - 1 - right[lRsize - 1];
                return unSymbolSize;
            }
        }
        //获取符号字符串长
        private double GetSymbolLength(string inputString)
        {
            double length = -1;
            string strLength;
            int groupSize = symbolGroup.Count;
            for (int i = 0; i < groupSize; ++i)
            {
                strLength = symbolGroup[i].strLength;
                string strShowName = symbolGroup[i].showName;
                //若是第一类，第一类固定长可直接在XML扩充，代码不变
                if (!strLength.Contains("?"))
                {
                    if (inputString == symbolGroup[i].showName)
                    {
                        length = Convert.ToDouble(symbolGroup[i].strLength);
                        return length;
                    }
                }
                //若是第二类
                else
                {
                    if (inputString.Contains(strShowName))
                    {
                        string sub1 = inputString.Substring(1, inputString.Length - 1);
                        if (inputString.Substring(0, 2) == "[框" && sub1.Contains("["))
                        {
                            return StringLength(sub1.Substring(1, sub1.Length - 2)) + 1;
                        }
                        else
                        {
                            //遍历判断属于第二类的何种符号
                            if (strShowName == "[框")
                            {
                                return (inputString.Length - inputString.IndexOf("[框") - 2);
                            }
                            else if (strShowName == "[公差")
                            {
                                //获取分号字符大小比例
                                string strScale = strLength.Substring(strLength.IndexOf("*") + 1, strLength.Length - 1 - strLength.IndexOf("*"));
                                double scale = Convert.ToDouble(strScale);
                                int a = inputString.IndexOf("!") - inputString.IndexOf("[公差") - 3;
                                int b = inputString.Length - inputString.IndexOf("!") - 2;
                                return (a > b ? scale * a : scale * b);
                            }
                            else if (strShowName == "[分数")
                            {
                                int a = inputString.IndexOf("!") - inputString.IndexOf("[分数") - 3;
                                int b = inputString.Length - inputString.IndexOf("!") - 2;
                                return (a > b ? 0.7 * a : 0.7 * b);
                            }
                            else if (strShowName == "[上标" || strShowName == "[下标")
                            {
                                return (0.5 * (inputString.Length - inputString.IndexOf("[") - 4));
                            }
                            //TODO: 在此添加第二类扩充后的代码，添加if遍历，同Block的Update函数

                        }

                    }
                }
            }

            //若该字符串非规范，返回-1
            return -1;
        }
        //字符串是否有效
        private bool IsStringValid(string inputString)
        {
            if (inputString == null)
            {
                return false;
            }
            string str = "";
            List<int> left = new List<int>();
            List<char> stack = new List<char>();
            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString[i] == '[')
                {
                    if (stack.Count != 0)
                    {
                        string tempStr = inputString.Substring(left.Count - 1, 2);
                        if (tempStr != "[框")
                        {
                            return false;
                        }
                    }
                    stack.Add('[');
                    left.Add(i);
                }
                else if (inputString[i] == ']')
                {
                    if (stack.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        stack.RemoveAt(stack.Count - 1);
                        str = inputString.Substring(left[left.Count - 1], i + 1 - left[left.Count - 1]);
                        left.RemoveAt(left.Count - 1);
                        if (!IsInXML(str))
                        {
                            return false;
                        }
                    }
                }
            }
            if (stack.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //判断方框字符是否已在XML中规范
        private bool IsInXML(string inputString)
        {
            int groupSize = symbolGroup.Count;
            for (int i = 0; i < groupSize; ++i)
            {
                if (inputString.Contains(symbolGroup[i].showName))
                {
                    if (symbolGroup[i].showName.Contains("[公差"))
                    {
                        int a = inputString.IndexOf("[公差");
                        int b = inputString.IndexOf("!");
                        return b != -1 && b > a;
                    }
                    if (symbolGroup[i].showName.Contains("[分数"))
                    {
                        int a = inputString.IndexOf("[分数");
                        int b = inputString.IndexOf("!");
                        return b != -1 && b > a;
                    }
                    if (symbolGroup[i].showName.Contains("[下标") || symbolGroup[i].showName.Contains("[上标"))
                    {
                        return inputString.Length > 4;
                    }
                    return true;
                }
            }
            return false;
        }
        //转换到NX文本
        private string Change2NX(string inPutStr)
        {
            if (!IsStringValid(inPutStr))
            {
                return "";
            }
            int groupSize = symbolGroup.Count;
            List<int> left = new List<int>();
            List<int> right = new List<int>();
            List<int> stack = new List<int>();
            int tempNum = 0;
            for (int j = 0; j < inPutStr.Length; ++j)
            {
                if (inPutStr[j] == '[')
                {
                    stack.Add(j);
                }
                else if (inPutStr[j] == ']')
                {
                    tempNum = stack[stack.Count - 1];
                    stack.RemoveAt(stack.Count - 1);
                    if (stack.Count == 0)
                    {
                        left.Add(tempNum);
                        right.Add(j);
                    }
                }
            }
            if (left.Count == 0)
            {
                return inPutStr;
            }
            else
            {
                string realStr = inPutStr.Substring(0, left[0]);
                int lRsize = left.Count;
                for (int i = 0; i < lRsize; ++i)
                {
                    realStr += SymbolRealString(inPutStr.Substring(left[i], right[i] - left[i] + 1));
                    if (i != lRsize - 1)
                    {
                        realStr += inPutStr.Substring(right[i] + 1, left[i + 1] - right[i] - 1);
                    }
                }
                if (right[lRsize - 1] != inPutStr.Length - 1)
                {
                    realStr += inPutStr.Substring(right[lRsize - 1] + 1, inPutStr.Length - right[lRsize - 1] - 1);
                }
                return realStr;
            }
        }
        //得到字符串的真实字符串
        private string SymbolRealString(string inputString)
        {
            string strLength;
            int groupSize = symbolGroup.Count;
            for (int i = 0; i < groupSize; ++i)
            {
                strLength = symbolGroup[i].strLength;
                //若是第一类，第一类固定长可直接在XML扩充，代码不变
                if (!strLength.Contains("?"))
                {
                    if (inputString == symbolGroup[i].showName)
                    {
                        return symbolGroup[i].realName;
                    }
                }
                //若是第二类
                else
                {
                    string strShowName = symbolGroup[i].showName;
                    if (inputString.Contains(strShowName))
                    {
                        string sub1 = inputString.Substring(1, inputString.Length - 1);
                        if (inputString.Substring(0, 2) == "[框" && sub1.Contains("["))
                        {
                            string tempstr = symbolGroup[i].realName.Substring(0, symbolGroup[i].realName.IndexOf("i"));
                            string last = symbolGroup[i].realName.Substring(symbolGroup[i].realName.IndexOf("i") + 1, symbolGroup[i].realName.Length - symbolGroup[i].realName.IndexOf("i") - 1);
                            return (tempstr + Change2NX(sub1.Substring(2, sub1.Length - 3)) + last);
                        }
                        else
                        {
                            string secondtype = "";
                            string midStr = "";
                            //遍历判断属于第二类的何种符号
                            if (strShowName == "[框")
                            {
                                if (inputString.Length > 3)
                                {
                                    midStr = inputString.Substring(2, inputString.Length - 3);
                                }
                                secondtype = symbolGroup[i].realName.Substring(0, symbolGroup[i].realName.IndexOf("i"));
                                secondtype += midStr +
                                    symbolGroup[i].realName.Substring(symbolGroup[i].realName.IndexOf("i") + 1, symbolGroup[i].realName.Length - symbolGroup[i].realName.IndexOf("i") - 1);
                                return secondtype;
                            }
                            else if (strShowName == "[公差" || strShowName == "[分数")
                            {
                                if (inputString.Length > 5)
                                {
                                    midStr = inputString.Substring(3, inputString.Length - 4);
                                    secondtype = symbolGroup[i].realName.Substring(0, symbolGroup[i].realName.IndexOf("!"));
                                    secondtype += midStr + symbolGroup[i].realName.Substring(symbolGroup[i].realName.IndexOf("!") + 1, symbolGroup[i].realName.Length - symbolGroup[i].realName.IndexOf("!") - 1);
                                    return secondtype;
                                }
                                else
                                {
                                    return symbolGroup[i].realName;
                                }
                            }
                            else if (strShowName == "[上标" || strShowName == "[下标")
                            {
                                if (inputString.Length > 4)
                                {
                                    midStr = inputString.Substring(3, inputString.Length - 4);
                                }
                                secondtype = symbolGroup[i].realName.Substring(0, symbolGroup[i].realName.IndexOf("i"));
                                secondtype += midStr +
                                    symbolGroup[i].realName.Substring(symbolGroup[i].realName.IndexOf("i") + 1, symbolGroup[i].realName.Length - symbolGroup[i].realName.IndexOf("i") - 1);
                                return secondtype;
                            }
                            //TODO: 在此添加第二类扩充后的代码，添加if遍历

                        }

                    }
                }
            }
            return "";
        }
        //格式化字符串
        private string FormatText(string text)
        {
            try
            {
                string result = "";
                if (string.IsNullOrEmpty(text))
                {
                    return result;
                }
                string[] strs = text.Split('@');
                if (strs.Length == 1)
                {
                    //未配置
                }
                else if (strs.Length == 2)
                {
                    if (strs[0] == "def")
                    {
                        result = "";
                    }
                    else if (strs[0] == "fun")
                    {
                        //利用反射技术XML内容直接调用对应名称的方法
                        object re = NXFun.ReflectFun(this, strs[1]);
                        result = re.ToString();
                    }
                }
                else if (strs.Length == 3)
                {
                    if (strs[0] == "xml")
                    {
                        if (strs[1] == "gongyi")
                        {
                            result = XML3DPPM.GetIndexAttr(0, 0, strs[2], XmlFile);
                        }
                        else if (strs[1] == "gongxu")
                        {
                            //特性符号 特殊处理
                            if (strs[2] == "gongxu_texingfuhao")
                            {
                                bool isKey = NXFun.string2bool(XML3DPPM.GetIndexAttr(pub_a, 0,"gongxu_guanjiangongxu", XmlFile));
                                result = isKey? "[框关键过程]" : "";
                            } 
                            else
                            {
                                result = XML3DPPM.GetIndexAttr(pub_a, 0, strs[2], XmlFile);
                            }
                        }
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                NXFun.MessageBox(ex.Message);
                return "";
            }
        }

        //工艺程序表的总页数
        public string GetGYCXAll()
        {
            NXOpen.Drawings.DrawingSheet[] dss = Session.GetSession().Parts.Work.DrawingSheets.ToArray();
            int count = 0;
            foreach (NXOpen.Drawings.DrawingSheet ds in dss)
            {
                string templet = NXFun.GetStringAttr(ds, "TEMPLET");
                string isPrint = NXFun.GetStringAttr(ds, "ISPRINT");
                if (templet.Contains("gongyichengxu") && isPrint == "是")
                {
                    count++;
                }
            }
            return count.ToString();
        }
        //工艺程序表的当前页码
        public string GetGYCXCur()
        {
            string guid = NXFun.GetOnShowSheetGuid();
            NXOpen.Drawings.DrawingSheet[] dss = Session.GetSession().Parts.Work.DrawingSheets.ToArray();
            foreach (NXOpen.Drawings.DrawingSheet ds in dss)
            {
                string templet = NXFun.GetStringAttr(ds, "TEMPLET");
                string isPrint = NXFun.GetStringAttr(ds, "ISPRINT");
                string onguid = NXFun.GetStringAttr(ds, "GUID");
                if (templet.Contains("gongyichengxu") && isPrint == "是" && onguid == guid)
                {
                    if (templet.Contains("add"))
                    {
                        string num = NXFun.GetStringAttr(ds, "ADDNUM");
                        return (Convert.ToInt32(num) + 1).ToString();
                    }
                    else
                    {
                        return "1";
                    }
                }
            }
            return "1";
        }
        //总页数
        public string GetTotalPages()
        {
            return SheetList.FindAll(delegate(S_Sheet s) { return s.IsPrint == "是"; }).Count.ToString();
        }
        //得到当前页码
        public string GetCurrentPage()
        {
            string guid = NXFun.GetOnShowSheetGuid();
            int count = 0;
            foreach (S_Sheet sheet in SheetList)
            {
                if (sheet.a == pub_a && sheet.IsPrint == "是")
                {
                    count++;
                }
                if (sheet.guid == guid)
                {
                    break;
                }
            }
            return count.ToString();
        }
        //工序页数
        public string GetProcedurePages()
        {
            return SheetList.FindAll(delegate(S_Sheet s) { return s.a == pub_a && s.IsPrint == "是"; }).Count.ToString();
        }
        //工艺装备表的总页数
        public string GetGYZBAll()
        {
            NXOpen.Drawings.DrawingSheet[] dss = Session.GetSession().Parts.Work.DrawingSheets.ToArray();
            int count = 0;
            foreach (NXOpen.Drawings.DrawingSheet ds in dss)
            {
                string templet = NXFun.GetStringAttr(ds, "TEMPLET");
                string isPrint = NXFun.GetStringAttr(ds, "ISPRINT");
                if (templet.Contains("gongyizhuangbei") && isPrint == "是")
                {
                    count++;
                }
            }
            return count.ToString();
        }
        //工艺装备表的当前页码
        public string GetGYZBCur()
        {
            string guid = NXFun.GetOnShowSheetGuid();
            NXOpen.Drawings.DrawingSheet[] dss = Session.GetSession().Parts.Work.DrawingSheets.ToArray();
            foreach (NXOpen.Drawings.DrawingSheet ds in dss)
            {
                string templet = NXFun.GetStringAttr(ds, "TEMPLET");
                string isPrint = NXFun.GetStringAttr(ds, "ISPRINT");
                string onguid = NXFun.GetStringAttr(ds, "GUID");
                if (templet.Contains("gongyizhuangbei") && isPrint == "是" && onguid == guid)
                {
                    if (templet.Contains("add"))
                    {
                        string num = NXFun.GetStringAttr(ds, "ADDNUM");
                        return (Convert.ToInt32(num) + 1).ToString();
                    }
                    else
                    {
                        return "1";
                    }
                }
            }
            return "1";
        }

        //填写当前显示的表头
        private void FillOnShowTitle()
        {
            SetSBF();
            //得到当前显示的sheet
            NXOpen.Drawings.DrawingSheet ds = NXFun.GetOnShowSheet();
            if (ds == null)
            {
                SetStatusLabel("没有显示的图表！", 1);
                return;
            }
            //得到当前显示的prt是第几个工序（不是工序号）
            string prt = NXFun.GetOnShowPrtName();
            List<S_Model> ModelList = XML3DPPM.GetModelList(XmlFile);
            S_Model model = ModelList.Find(delegate(S_Model m) { return prt == ToFullPath(m.filename) && m.a == 0; });
            if (model.Equals(default(S_Model)))
            {
                SetStatusLabel("显示的图表不在本工艺下！", 1);
                return;
            }
            pub_a = model.b;
            //得到当前sheet的信息
            string templet = NXFun.GetStringAttr(ds, "TEMPLET");
            string guid = NXFun.GetStringAttr(ds, "GUID");
            if (templet == "NX")
            {
                //NX自带模板，不用管
            }
            else if (!string.IsNullOrEmpty(templet))
            {
                //需要填写哦
                List<S_SheetItem> SheetItemList = XML3DPPM.GetSheetItemList(templet, NXFun.TDPPMPath + NXFun.SheetTempletXML);
                //格式化文本
                List<S_SheetNote> SheetNoteList = SheetItem2SheetNote(ref SheetItemList);
                foreach (S_SheetNote note in SheetNoteList)
                {
                    if (note.flag == "xml@gongyi@gongyi_xinghao")
                    {
                        WriteXingHaoTable(templet);
                    } 
                    else
                    {
                        NXFun.WriteText(note, guid);
                    }                    
                }
            }
        }
        //填写工艺程序
        private void Fill_3DPPM_gongyichengxu(string templet)
        {
            S_GongYiChengXu s_gycx;
            List<S_GongYiChengXu> table = new List<S_GongYiChengXu>();
            string guid = NXFun.GetOnShowSheetGuid();

            //得到工序数
            int count = XML3DPPM.GetGongxuCount(XmlFile);
            //得到表格内容
            for (int i = 0; i < count; i++)
            {
                s_gycx.texingfuhao = XML3DPPM.GetIndexAttr(i + 1, 0, "gongxu_guanjiangongxu", XmlFile) == "是" ? " <&70><+>关键过程<+><&90> " : "";
                s_gycx.gongxuhao = XML3DPPM.GetIndexAttr(i + 1, 0, "gongxu_gongxuhao", XmlFile);
                s_gycx.gongxumingcheng = XML3DPPM.GetIndexAttr(i + 1, 0, "gongxu_gongxumingcheng", XmlFile);
                s_gycx.shebei = XML3DPPM.GetIndexAttr(i + 1, 0, "gongxu_shebei", XmlFile);
                s_gycx.beizhu = XML3DPPM.GetIndexAttr(i + 1, 0, "gongxu_beizhu", XmlFile);
                table.Add(s_gycx);
            }
            //得到表格宽度和行数
            List<S_SheetItem> item_list = XML3DPPM.GetSheetItemList(templet, NXFun.TDPPMPath + NXFun.SheetTempletXML);
            if (item_list.Count == 0)
            {
                NXFun.MessageBox("得到图纸模板信息出错！", "错误", 0);
                return;
            }
            double y0 = 0, y1 = 0, x0 = 0, x1 = 0, x2 = 0, x3 = 0, x4 = 0, x5 = 0, height = 0, size = 0;
            int lines = 0;
            string font = "";
            /*
            0--------1------2--------3----4----5     y1首行高
            |特性符号|工序号|工序名称|设备|备注|     height
            ------------------------------>          y0 
            */
            S_SheetItem item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_texingfuhao"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板项目信息出错！", "错误", 0);
                return;
            }
            x0 = item.x0;
            x1 = item.x1;
            y1 = item.y0;
            y0 = item.y1;
            lines = item.multiline;
            height = y1 - y0;
            font = item.font;
            size = item.size;
            item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_gongxuhao"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板工作内容信息出错！", "错误", 0);
                return;
            }
            x2 = item.x1;
            item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_gongxumingcheng"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板刃具信息出错！", "错误", 0);
                return;
            }
            x3 = item.x1;
            item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_shebei"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板量具信息出错！", "错误", 0);
                return;
            }
            x4 = item.x1;
            item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_beizhu"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板备注信息出错！", "错误", 0);
                return;
            }
            x5 = item.x1;
            //计算各内容能填写的字符数
            int num_texingfuhao = (int)((x1 - x0) / wordwidth / size / KGB);
            int num_gongxuhao = (int)((x2 - x1) / wordwidth / size / KGB);
            int num_gongxumingcheng = (int)((x3 - x2) / wordwidth / size / KGB);
            int num_shebei = (int)((x4 - x3) / wordwidth / size / KGB);
            int num_beizhu = (int)((x5 - x4) / wordwidth / size / KGB);

            //表格table格式化（一行变多行）
            table = FormatGongYiChengXuTable(ref table, num_gongxumingcheng, num_shebei, num_beizhu);

            //判断需要页数
            int pages = 1;
            if (table.Count > lines)
            {
                //需要换页
                pages = (table.Count - 1) / lines + 1;
            }
            //查询或创建图纸续页
            List<string> guid_list = new List<string>();
            guid_list.Add(guid);
            string AddTempletName = templet + "-add";
            if (pages > 1)
            {
                //标识方法：图纸模板名称用A4-3DPPM-gongxubiao-add
                //fatherguid:母模板
                //addnum: 1 2 3 ……
                for (int i = 1; i < pages; i++)
                {
                    string guid_add = NXFun.FindAddSheetGuid(guid, i, AddTempletName);
                    if (string.IsNullOrEmpty(guid_add))
                    {
                        return;
                    }
                    guid_list.Add(guid_add);
                }
            }
            //开始填表啦！！！！累死了…… QQ交友:47965258
            S_SheetNote note;
            note.x = 0;
            note.y = 0;
            note.pos = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;
            note.CharSpaceFactor = 0.01;
            note.LineSpaceFactor = 1.6;
            note.TextFont = font;
            note.TextSize = size;
            note.AspectRatio = KGB;
            note.flag = "";
            string[] text = new string[1];

            for (int i = 0; i < pages; i++)
            {
                string temp_guid = guid_list[i];
                NXFun.ShowSheetByGuid(temp_guid);
                //每页要写的行数
                int line = Math.Min(table.Count - i * lines, lines);
                int index = 0;//table的索引号
                for (int j = 0; j < line; j++)
                {
                    note.y = y1 - height / 2 - height * j;
                    //写特性符号
                    index = j + lines * i;
                    if (!string.IsNullOrEmpty(table[index].texingfuhao))
                    {
                        note.TextSize = size * 2 / 3;
                        string onshow_guid = NXFun.GetOnShowSheetGuid();
                        note.x = (x0 + x1) / 2;
                        note.pos = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;
                        double l = GetNXLength(table[index].texingfuhao);
                        text[0] = (l > 0) ? Change2NX(table[index].texingfuhao) : "";
                        note.text = text;
                        NXFun.WriteText(note, temp_guid);
                    }
                    //写工序号
                    if (!string.IsNullOrEmpty(table[index].gongxuhao))
                    {
                        note.TextSize = size;
                        note.x = (x2 + x1) / 2;
                        note.pos = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;
                        double l = GetNXLength(table[index].gongxuhao);
                        text[0] = (l > 0) ? Change2NX(table[index].gongxuhao) : "";
                        note.text = text;
                        NXFun.WriteText(note, temp_guid);
                    }
                    //工序名称
                    if (!string.IsNullOrEmpty(table[index].gongxumingcheng))
                    {
                        note.x = x2 + 1;
                        note.pos = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidLeft;
                        double l = GetNXLength(table[index].gongxumingcheng);
                        text[0] = (l > 0) ? Change2NX(table[index].gongxumingcheng) : "";
                        note.text = text;
                        NXFun.WriteText(note, temp_guid);
                    }
                    //写设备
                    if (!string.IsNullOrEmpty(table[index].shebei))
                    {
                        note.x = x3 + 1;
                        note.pos = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidLeft;
                        double l = GetNXLength(table[index].shebei);
                        text[0] = (l > 0) ? Change2NX(table[index].shebei) : "";
                        note.text = text;
                        NXFun.WriteText(note, temp_guid);
                    }
                    //写备注
                    if (!string.IsNullOrEmpty(table[index].beizhu))
                    {
                        note.x = x4 + 1;
                        note.pos = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidLeft;
                        double l = GetNXLength(table[index].beizhu);
                        text[0] = (l > 0) ? Change2NX(table[index].beizhu) : "";
                        note.text = text;
                        NXFun.WriteText(note, temp_guid);
                    }
                }
            }
        }
        //填写工装统计
        private void Fill_3DPPM_gongyizhuangbei(string templet)
        {
            string guid = NXFun.GetOnShowSheetGuid();

            //得到工序数
            int count = XML3DPPM.GetGongxuCount(XmlFile);
            //得到工艺装备集
            List<S_Gongxu> gongxu_list = new List<S_Gongxu>();
            for (int i = 0; i < count; i++)
            {
                S_Gongxu s_gongxu;
                s_gongxu.gongxuhao = XML3DPPM.GetIndexAttr(i + 1, 0, "gongxu_gongxuhao", XmlFile);
                s_gongxu.jiamoju_list = NXFun.Array2List<string>(SplitStringBySemicolon(XML3DPPM.GetIndexAttr(i + 1, 0, "gongxu_jiamoju", XmlFile), true));
                int num_step = XML3DPPM.GetGongbuCount(i + 1, XmlFile); //得到工步数
                s_gongxu.renju_list = new List<string>();
                s_gongxu.liangju_list = new List<string>();
                //得到工步所有刃具 量具
                List<string> renju_list = new List<string>();
                List<string> liangju_list = new List<string>();
                for (int j = 0; j < num_step; j++)
                {
                    renju_list = NXFun.Array2List<string>(SplitStringBySemicolon(XML3DPPM.GetIndexAttr(i + 1, j + 1, "gongbu_renju", XmlFile), true));
                    liangju_list = NXFun.Array2List<string>(SplitStringBySemicolon(XML3DPPM.GetIndexAttr(i + 1, j + 1, "gongbu_liangju", XmlFile), true));
                    //得到子工步的刃具 量具
                    S_ChildStep[] s_childstep = XML3DPPM.GetChildStepList(i+1, j+1, XmlFile);
                    foreach (S_ChildStep iter2 in s_childstep)
                    {
                        if (!string.IsNullOrEmpty(iter2.renju))
                        {
                            renju_list.Add(iter2.renju);
                        }
                        if (!string.IsNullOrEmpty(iter2.liangju))
                        {
                            liangju_list.Add(iter2.liangju);
                        }
                    }
                    s_gongxu.renju_list.AddRange(renju_list);
                    s_gongxu.liangju_list.AddRange(liangju_list);
                }
                s_gongxu.jiamoju_list = NXFun.RemoveTheSame<string>(s_gongxu.jiamoju_list);
                s_gongxu.liangju_list = NXFun.RemoveTheSame<string>(s_gongxu.liangju_list);
                s_gongxu.renju_list = NXFun.RemoveTheSame<string>(s_gongxu.renju_list);
                gongxu_list.Add(s_gongxu);
            }
            //转换成表格
            List<S_Line_GZTJ> s_ling_gztj = SeparateToLine(ref gongxu_list);
            List<S_GongYiZhuangBei> table = new List<S_GongYiZhuangBei>();
            S_GongYiZhuangBei s_gyzb;
            foreach (S_Line_GZTJ iter in s_ling_gztj)
            {
                s_gyzb.gongxuhao = iter.gongxuhao;
                s_gyzb.jiamoju = iter.jiamoju;
                s_gyzb.chongyong_jiamoju = JoinVectorString(iter.chongyong_list1);
                s_gyzb.renju = iter.renju;
                s_gyzb.chongyong_renju = JoinVectorString(iter.chongyong_list2);
                s_gyzb.liangju = iter.liangju;
                s_gyzb.chongyong_liangju = JoinVectorString(iter.chongyong_list3);
                table.Add(s_gyzb);
            }

            //得到表格宽度和行数
            List<S_SheetItem> item_list = XML3DPPM.GetSheetItemList(templet, NXFun.TDPPMPath + NXFun.SheetTempletXML);
            if (item_list.Count == 0)
            {
                NXFun.MessageBox("得到图纸模板信息出错！", "错误", 0);
                return;
            }
            double y0 = 0, y1 = 0, x0 = 0, x1 = 0, x2 = 0, x3 = 0, x4 = 0, x5 = 0, x6 = 0, x7 = 0, x8 = 0, x9 = 0, x10 = 0, height = 0, size = 0;
            int lines = 0;
            string font = "";

            //   0------1------2----3--------4----5----6--------7----8----9--------10     y1首行高
            //   |工序号|夹模具|批次|重用工序|刃具|批次|重用工序|量具|批次|重用工序|     height
            //   ------------------------------------------------------------------->     y0 

            S_SheetItem item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_gongxuhao"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板工序号信息出错！", "错误", 0);
                return;
            }
            x0 = item.x0;
            x1 = item.x1;
            y1 = item.y0;
            y0 = item.y1;
            lines = item.multiline;
            height = y1 - y0;
            font = item.font;
            size = item.size;

            item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_jiamoju"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板夹模具信息出错！", "错误", 0);
                return;
            }
            x2 = item.x1;

            item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_pici0"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板批次信息出错！", "错误", 0);
                return;
            }
            x3 = item.x1;

            item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_chongyonggongxu0"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板重用工序信息出错！", "错误", 0);
                return;
            }
            x4 = item.x1;

            item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_renju"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板刃具信息出错！", "错误", 0);
                return;
            }
            x5 = item.x1;

            item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_pici1"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板批次信息出错！", "错误", 0);
                return;
            }
            x6 = item.x1;

            item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_chongyonggongxu1"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板重用工序信息出错！", "错误", 0);
                return;
            }
            x7 = item.x1;

            item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_liangju"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板量具信息出错！", "错误", 0);
                return;
            }
            x8 = item.x1;

            item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_pici2"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板批次信息出错！", "错误", 0);
                return;
            }
            x9 = item.x1;

            item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains(templet + "_chongyonggongxu2"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板重用工序信息出错！", "错误", 0);
                return;
            }
            x10 = item.x1;

            //计算各内容能填写的字符数
            int num_jiamoju = (int)((x2 - x1) / wordwidth / size / KGB);
            int num_chongyong_jiamoju = (int)((x4 - x3) / wordwidth / size / KGB);
            int num_renju = (int)((x5 - x4) / wordwidth / size / KGB);
            int num_chongyong_renju = (int)((x7 - x6) / wordwidth / size / KGB);
            int num_liangju = (int)((x8 - x7) / wordwidth / size / KGB);
            int num_chongyong_liangju = (int)((x10 - x9) / wordwidth / size / KGB);
            //判断需要页数
            int pages = 1;
            if (table.Count > lines)
            {
                //需要换页
                pages = (table.Count - 1) / lines + 1;
            }

            //查询或创建图纸续页
            List<string> guid_list = new List<string>();
            guid_list.Add(guid);
            string AddTempletName = templet + "-add";
            if (pages > 1)
            {
                //标识方法：图纸模板名称用A4-3DPPM-gongxubiao-add
                //fatherguid:母模板
                //addnum: 1 2 3 ……
                for (int i = 1; i < pages; i++)
                {
                    string guid_add = NXFun.FindAddSheetGuid(guid, i, AddTempletName);
                    if (string.IsNullOrEmpty(guid_add))
                    {
                        return;
                    }
                    guid_list.Add(guid_add);
                }
            }
            //开始填表啦！
            S_SheetNote note;
            note.x = 0;
            note.y = 0;
            note.pos = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;
            note.CharSpaceFactor = 0.01;
            note.LineSpaceFactor = 1.6;
            note.TextFont = font;
            note.TextSize = size;
            note.AspectRatio = KGB;
            note.flag = "";
            string[] text = new string[1];

            for (int i = 0; i < pages; i++)
            {
                NXFun.ShowSheetByGuid(guid_list[i]);
                //每页要写的行数
                int line = Math.Min(table.Count - i * lines, lines);
                int index = 0;//table的索引号
                for (int j = 0; j < line; j++)
                {
                    note.y = y1 - height / 2 - height * j;
                    index = j + lines * i;
                    //写工序号
                    if (!string.IsNullOrEmpty(table[index].gongxuhao))
                    {
                        note.TextSize = size;
                        note.x = (x0 + x1) / 2;
                        text[0] = table[index].gongxuhao;
                        note.text = text;
                        NXFun.WriteText(note, guid_list[i]);
                    }
                    //夹模具
                    if (!string.IsNullOrEmpty(table[index].jiamoju))
                    {
                        note.x = (x2 + x1) / 2;
                        double l = GetNXLength(table[index].jiamoju);
                        text[0] = (l > 0) ? Change2NX(table[index].jiamoju) : "";
                        note.text = text;
                        note.AspectRatio = l > num_jiamoju ? (KGB * num_jiamoju / l) : KGB;
                        NXFun.WriteText(note, guid_list[i]);
                    }
                    //夹模具重用工序
                    if (!string.IsNullOrEmpty(table[index].chongyong_jiamoju))
                    {
                        note.x = (x3 + x4) / 2;
                        double l = GetNXLength(table[index].chongyong_jiamoju);
                        text[0] = (l > 0) ? Change2NX(table[index].chongyong_jiamoju) : "";
                        note.text = text;
                        note.AspectRatio = l > num_chongyong_jiamoju ? (KGB * num_chongyong_jiamoju / l) : KGB;
                        NXFun.WriteText(note, guid_list[i]);
                    }
                    //刃具
                    if (!string.IsNullOrEmpty(table[index].renju))
                    {
                        note.x = (x4 + x5) / 2;
                        double l = GetNXLength(table[index].renju);
                        text[0] = (l > 0) ? Change2NX(table[index].renju) : "";
                        note.text = text;
                        note.AspectRatio = l > num_renju ? (KGB * num_renju / l) : KGB;
                        NXFun.WriteText(note, guid_list[i]);
                    }
                    //刃具重用工序
                    if (!string.IsNullOrEmpty(table[index].chongyong_renju))
                    {
                        note.x = (x6 + x7) / 2;
                        double l = GetNXLength(table[index].chongyong_renju);
                        text[0] = (l > 0) ? Change2NX(table[index].chongyong_renju) : "";
                        note.text = text;
                        note.AspectRatio = l > num_chongyong_renju ? (KGB * num_chongyong_renju / l) : KGB;
                        NXFun.WriteText(note, guid_list[i]);
                    }
                    //量具
                    if (!string.IsNullOrEmpty(table[index].liangju))
                    {
                        note.x = (x7 + x8) / 2;
                        double l = GetNXLength(table[index].liangju);
                        text[0] = (l > 0) ? Change2NX(table[index].liangju) : "";
                        note.text = text;
                        note.AspectRatio = l > num_liangju ? (KGB * num_liangju / l) : KGB;
                        NXFun.WriteText(note, guid_list[i]);
                    }
                    //量具重用工序
                    if (!string.IsNullOrEmpty(table[index].chongyong_liangju))
                    {
                        note.x = (x9 + x10) / 2;
                        double l = GetNXLength(table[index].chongyong_liangju);
                        text[0] = (l > 0) ? Change2NX(table[index].chongyong_liangju) : "";
                        note.text = text;
                        note.AspectRatio = l > num_chongyong_liangju ? (KGB * num_chongyong_liangju / l) : KGB;
                        NXFun.WriteText(note, guid_list[i]);
                    }
                }
            }
        }
        //为了使GridView中CheckBox即时更改
        private void dgvChart_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvChart.IsCurrentCellDirty)
            {
                dgvChart.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        //填写型号表
        private void WriteXingHaoTable(string templet)
        {
            string guid = NXFun.GetOnShowSheetGuid();
            //得到表格宽度和行数
            List<S_SheetItem> item_list = XML3DPPM.GetSheetItemList(templet, NXFun.TDPPMPath + NXFun.SheetTempletXML);
            if (item_list.Count == 0)
            {
                NXFun.MessageBox("得到图纸模板信息出错！", "错误", 0);
                return;
            }
	        double x0 = 0,x1 = 0,y0 = 0,y1 = 0,height = 0,size = 0;
	        string font = "";
	        int lines = 0;
            S_SheetItem item = item_list.Find(delegate(S_SheetItem i) { return i.text.Contains("gongyi_xinghao"); });
            if (item.Equals(default(S_SheetItem)))
            {
                NXFun.MessageBox("得到图纸模板型号坐标出错！", "错误", 0);
                return;
            }
            x0 = item.x0;
            x1 = item.x1;
            y1 = item.y0;
            y0 = item.y1;
            height = y1 - y0;
            font = item.font;
            size = item.size;
            lines = item.multiline;

            S_SheetNote note;
            note.x = (x0+x1)/2;
            note.y = 0;
            note.pos = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;
            note.CharSpaceFactor = 0.01;
            note.LineSpaceFactor = 1.6;
            note.TextFont = font;
            note.TextSize = size;
            note.AspectRatio = KGB;
            note.flag = "xml@gongyi@gongyi_xinghao";
            int num_xinghao = (int)((x1 - x0) / wordwidth / size / KGB);
            string[] text = new string[1];

            string xinghao = XML3DPPM.GetIndexAttr(0,0,"gongyi_xinghao",XmlFile);
            string[] v_xinghao = SplitStringBySemicolon(xinghao, false);
            if (v_xinghao != null)
            {
                for (int i = 0; i < v_xinghao.Length; i++)
                {
                    note.y = y1 - height / 2 - height * i;
                    double l = GetNXLength(v_xinghao[i]);
                    text[0] = (l > 0) ? Change2NX(v_xinghao[i]) : "";
                    note.text = text;
                    note.AspectRatio = l > num_xinghao ? (KGB * num_xinghao / l) : KGB;
                    NXFun.WriteText(note, guid);
                }
            }
            
        }

    }
}
