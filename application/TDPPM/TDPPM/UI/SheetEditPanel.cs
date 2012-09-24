using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using NXOpen;

namespace TDPPM
{
    public partial class SheetEditPanel : EditPanel
    {
        private bool IsFill = false;                //是否为独立模式
        private Size mainDlgOldSize = new Size();   //记录主窗口大小
        private Size CharSize = new Size(450, 400); //独立模式的窗口大小
        //构造函数
        public SheetEditPanel()
        {
            InitializeComponent();
        }
        //初始化设置
        public void Init()
        {
            if (mainDlg == null || string.IsNullOrEmpty(ProPath) || string.IsNullOrEmpty(ProName))
            {
                //缺少初值，所有控件禁用  正常情况不应执行到这里
                tsrSheetTree.Enabled = false;
                tabSheetTool.Enabled = false;
            }
            else
            {
                XmlFile = ToFullPath(NXFun.ProcessXML);
                IsEdit = false;
                
                tsrSheetTree.Enabled = true;
                tabSheetTool.Enabled = true;
                //初始化子组件
                CharEdit.ProPath = ProPath;
                CharEdit.ProName = ProName;
                CharEdit.mainDlg = mainDlg;
                CharEdit.Init();

                LabelEdit.ProPath = ProPath;
                LabelEdit.ProName = ProName;
                LabelEdit.mainDlg = mainDlg;
                LabelEdit.Init();

                FreshSheetTree();

            }
        }
        //刷新图纸树
        public void FreshSheetTree()
        {
            tvwSheet.Nodes.Clear();
            tvwSheet.ImageIndex = 3;
            tvwSheet.SelectedImageIndex = 3;
            //第一个为工艺节点
            TreeNode gyTreeNode = new TreeNode();
            string filename = XML3DPPM.GetModelName(0, 0, XmlFile);
            gyTreeNode.Text = 
                XML3DPPM.GetIndexAttr(0, 0, "gongyi_jianming", XmlFile)+
                " ("+
                filename+
                ")";
            gyTreeNode.Tag = "Gongyi";            
            bool IsNull = string.IsNullOrEmpty(filename);
            gyTreeNode.ImageIndex = IsNull?1:0;
            gyTreeNode.SelectedImageIndex = IsNull ? 1 : 0;
            tvwSheet.Nodes.Add(gyTreeNode);
            if (!string.IsNullOrEmpty(filename))
            {
                List<S_Sheet> SheetTempletList = NXFun.GetSheetTempletList(ToFullPath(filename));
                foreach (S_Sheet sheet in SheetTempletList)
                {
                    TreeNode sheetTreeNode = new TreeNode();
                    sheetTreeNode.Text = sheet.SheetName + "_" + sheet.Remark;
                    sheetTreeNode.Tag = "Sheet";
                    sheetTreeNode.Name = sheet.guid; //利用guid标识图纸
                    sheetTreeNode.ImageIndex = 2;
                    sheetTreeNode.SelectedImageIndex = 2;
                    gyTreeNode.Nodes.Add(sheetTreeNode);
                }
            }
            //之后为工序节点
            int gxNum = XML3DPPM.GetGongxuCount(XmlFile);
            for (int i = 0; i < gxNum; i ++ )
            {
                TreeNode gxTreeNode = new TreeNode();
                filename = XML3DPPM.GetModelName(0, i + 1, XmlFile);
                gxTreeNode.Text =
                    XML3DPPM.GetIndexAttr(i + 1, 0, "gongxu_gongxuhao", XmlFile)+
                    "_"+
                    XML3DPPM.GetIndexAttr(i + 1, 0, "gongxu_gongxumingcheng", XmlFile)+
                    " ("+
                    filename+
                    ")";
                gxTreeNode.Tag = "Gongxu";                
                IsNull = string.IsNullOrEmpty(filename);
                gxTreeNode.ImageIndex = IsNull ? 1 : 0;
                gxTreeNode.SelectedImageIndex = IsNull ? 1 : 0;
                tvwSheet.Nodes.Add(gxTreeNode); 
                if (!string.IsNullOrEmpty(filename))
                {
                    List<S_Sheet> SheetTempletList = NXFun.GetSheetTempletList(ToFullPath(filename));
                    foreach (S_Sheet sheet in SheetTempletList)
                    {
                        TreeNode sheetTreeNode = new TreeNode();
                        sheetTreeNode.Text = sheet.SheetName + "_" + sheet.Remark;
                        sheetTreeNode.Tag = "Sheet";
                        sheetTreeNode.Name = sheet.guid; //利用guid标识图纸
                        sheetTreeNode.ImageIndex = 2;
                        sheetTreeNode.SelectedImageIndex = 2;
                        gxTreeNode.Nodes.Add(sheetTreeNode);
                    }
                }
            }
            tvwSheet.ExpandAll();
            CharEdit.RefreshSheet();
        }
        //刷新图纸树按钮
        private void tsrbtnRefresh_Click(object sender, EventArgs e)
        {
            FreshSheetTree();
            
        }
        //新建图纸页按钮
        private void tsrbtnAdd_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwSheet.SelectedNode;
            if (sel == null || (sel.Tag.ToString() != "Gongyi" && sel.Tag.ToString() != "Gongxu"))
            {
                SetStatusLabel("请选中树中工艺或工序节点", 1);
                return;
            }
            int index = sel.Index;
            string filename = XML3DPPM.GetModelName(0,index,XmlFile);
            if (string.IsNullOrEmpty(filename))
            {
                SetStatusLabel("选中节点下无模型，请在编辑模型模块中创建模型！", 1);
                return;
            }
            Part prt = NXFun.OpenPrt(ToFullPath(filename));
            if (prt == null)
            {
                SetStatusLabel("打开模型错误！", 1);
                return;
            }
            mainDlg.Enabled = false;    //如果这里不禁用主窗口，再点击一次添加或退出就会出错,而如果直接隐藏主窗口有时会把NX隐藏，奇怪的很。
           // mainDlg.WindowState = FormWindowState.Minimized;
            CreateSheet.MainFun();
            mainDlg.Enabled = true;
          //  mainDlg.WindowState = FormWindowState.Normal;
            SetStatusLabel("添加图纸页返回", 2);
            FreshSheetTree();

        }
        //删除图纸页按钮
        private void tsrbtnDel_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwSheet.SelectedNode;
            if (sel == null || sel.Tag.ToString() != "Sheet")
            {
                SetStatusLabel("请选中树中图纸节点", 1);
                return;
            }
            DialogResult dialogResult = MessageBox.Show("是否删除选中图纸？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.Yes)
            {
                return;
            }
            string prtfullpath = ToFullPath(XML3DPPM.GetModelName(0, sel.Parent.Index, XmlFile));
            string guid = sel.Name;
            NXFun.OpenPrt(prtfullpath);
            NXOpen.Drawings.DrawingSheet ds = NXFun.GetSheetByGuid(prtfullpath, guid);
            NXFun.DeleteObject(ds);
            SetStatusLabel("已删除图纸页", 2);
            FreshSheetTree();
        }
        //打开图纸页按钮
        private void tsrbtnOpen_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwSheet.SelectedNode;
            if (sel == null || sel.Tag.ToString() != "Sheet")
            {
                SetStatusLabel("请选中树中图纸节点", 1);
                return;
            }
            string prtfullpath = ToFullPath(XML3DPPM.GetModelName(0, sel.Parent.Index, XmlFile));
            string guid = sel.Name;
            NXFun.OpenPrt(prtfullpath);
            NXFun.ShowSheetByGuid(prtfullpath, guid);
            SetStatusLabel("已显示图纸页", 2);
            //FreshSheetTree();
        }
        //节点选中变化
        private void tvwSheet_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode sel = tvwSheet.SelectedNode;
            if (sel == null)
            {
                tsrbtnAdd.Enabled = false;
                tsrbtnDel.Enabled = false;
                tsrbtnOpen.Enabled = false;
            }
            else if (sel.Tag.ToString() == "Gongyi" || sel.Tag.ToString() == "Gongxu")
            {
                string filename = XML3DPPM.GetModelName(0, sel.Index, XmlFile);
                if (string.IsNullOrEmpty(filename))
                {
                    tsrbtnAdd.Enabled = false;
                    tsrbtnDel.Enabled = false;
                    tsrbtnOpen.Enabled = false;
                } 
                else
                {
                    tsrbtnAdd.Enabled = true;
                    tsrbtnDel.Enabled = false;
                    tsrbtnOpen.Enabled = false;
                }
            }
            else if (sel.Tag.ToString() == "Sheet")
            {
                tsrbtnAdd.Enabled = false;
                tsrbtnDel.Enabled = true;
                tsrbtnOpen.Enabled = true;
            }
        }
        //双击打开模型
        private void tvwSheet_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                System.Drawing.Point clickPoint = new System.Drawing.Point(e.X, e.Y);
                TreeNode sel = tvwSheet.GetNodeAt(clickPoint);
                //判断点击的是否是一个节点
                if (sel != null && sel.Tag.ToString() == "Sheet")
                {
                    string prtfullpath = ToFullPath(XML3DPPM.GetModelName(0, sel.Parent.Index, XmlFile));
                    string guid = sel.Name;
                    NXFun.OpenPrt(prtfullpath);
                    NXFun.ShowSheetByGuid(prtfullpath, guid);
                    SetStatusLabel("已显示图纸页", 2);
                }                
            }
        }
        //右击菜单
        private void tvwSheet_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                System.Drawing.Point clickPoint = new System.Drawing.Point(e.X, e.Y);
                TreeNode sel = tvwSheet.GetNodeAt(clickPoint);
                if (sel != null)
                {
                    tvwSheet.SelectedNode = sel;
                    if (sel.Tag.ToString() == "Gongyi" || sel.Tag.ToString() == "Gongxu")
                    {
                        string filename = XML3DPPM.GetModelName(0, sel.Index, XmlFile);
                        if (!string.IsNullOrEmpty(filename))
                        {
                            sel.ContextMenuStrip = cmsModel;
                        }
                    }
                    else if (sel.Tag.ToString() == "Sheet")
                    {
                        sel.ContextMenuStrip = cmsSheet;
                    }
                }
            }
        }
        //新建图纸菜单
        private void cmsCreateSheet_Click(object sender, EventArgs e)
        {
            tsrbtnAdd_Click(sender, e);
        }
        //打开图纸菜单
        private void cmsOpenSheet_Click(object sender, EventArgs e)
        {
            tsrbtnOpen_Click(sender, e);
        }
        //删除图纸菜单
        private void cmsDelSheet_Click(object sender, EventArgs e)
        {
            tsrbtnDel_Click(sender, e);
        }
        //双击tab标签  切换独立模式
        private void tabSheetTool_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!IsFill)
            {
                tsrSheetTree.Enabled = false;
                tvwSheet.Enabled = false;
                mainDlg.Controls.Add(tabSheetTool);
                mainDlg.ShowOrHideMenu(false);
                tabSheetTool.BringToFront();
                mainDlgOldSize = mainDlg.Size;
                mainDlg.Size = CharSize;
                IsFill = true;
            }
            else
            {
                tsrSheetTree.Enabled = true;
                tvwSheet.Enabled = true;
                mainDlg.Size = mainDlgOldSize;
                splMain.Panel2.Controls.Add(tabSheetTool);
                mainDlg.ShowOrHideMenu(true);
                IsFill = false;
            }
            
        }
        //切换tab卡
        private void tabSheetTool_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (LabelEdit.IsEdit)
            {
                e.Cancel = true;
                SetStatusLabel("请先完成打标号编辑操作！", 1);
            }
        }



    }
}
