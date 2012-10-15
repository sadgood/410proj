using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Xml;

namespace TDPPM
{
    public partial class ProcessEditPanel : EditPanel
    {
        private int OnEditGongxu = 0;       //正在编辑的工序
        private int OnEditGongbu = 0;       //正在编辑的工步

        bool IsEditYingdu = false;

        public ProcessEditPanel()
        {
            InitializeComponent();
        }

      
        //初始化组件状态，主窗口必须调用。
        /// <summary>
        /// 初始化组件状态，主窗口必须调用。
        /// </summary>
        public void Init()
        {
            if (mainDlg == null || string.IsNullOrEmpty(ProPath) || string.IsNullOrEmpty(ProName))
            {
                //缺少初值，所有控件禁用  正常情况不应执行到这里
                tsrGongyiTree.Enabled = false;
                tsrGongxuEdit.Enabled = false;
            }
            else
            {
                XmlFile = ToFullPath(NXFun.ProcessXML);
                //控件状态初始化
                IsEdit = false;
                OnEditGongbu = 0;
                OnEditGongxu = 0;
                tsrGongyiTree.Enabled = true;
                tsrGongxuEdit.Enabled = true;
                FreshGongYiTree();
                FreshEditTable();
            }
        }
        //设置编辑板的只读状态。
        /// <summary>
        /// 设置编辑板的只读状态
        /// </summary>
        /// <param name="isreadonly">是否只读</param>
        private void SetEditTableReadOnly(bool isreadonly)
        {
            if (isreadonly)
            {
                foreach (Control control in tlpGongyiNormal.Controls)
                {
                    if (control is TextBox)
                    {
                        ((TextBox)control).ReadOnly = true;
                        ((TextBox)control).BackColor = System.Drawing.SystemColors.Window;
                    }
                }
                foreach (Control control in tlpGongxuNormal.Controls)
                {
                    if (control is TextBox)
                    {
                        ((TextBox)control).ReadOnly = true;
                        ((TextBox)control).BackColor = System.Drawing.SystemColors.Window;
                    }
                }
                foreach (Control control in tlpGongbu.Controls)
                {
                    if (control is TextBox)
                    {
                        ((TextBox)control).ReadOnly = true;
                        ((TextBox)control).BackColor = System.Drawing.SystemColors.Window;
                    }
                }
                dgvZigongbu.ReadOnly = true;
                gongxu_guanjiangongxu.Enabled = false;
            }
            else
            {
                foreach (Control control in tlpGongyiNormal.Controls)
                {
                    if (control is TextBox)
                    {
                        ((TextBox)control).ReadOnly = false;
                    }
                }
                foreach (Control control in tlpGongxuNormal.Controls)
                {
                    if (control is TextBox)
                    {
                        ((TextBox)control).ReadOnly = false;

                    }
                }
                foreach (Control control in tlpGongbu.Controls)
                {
                    if (control is TextBox)
                    {
                        ((TextBox)control).ReadOnly = false;
                    }
                }
                dgvZigongbu.ReadOnly = false;
                gongbu_gongbuhao.ReadOnly = true;
                gongxu_gongxuhao.ReadOnly = true;
                gongxu_guanjiangongxu.Enabled = true;
                dgvZigongbu.Columns[0].ReadOnly = true;
            }
        }
        //读取控件的值
        /// <summary>
        /// 读取控件的值   （0，0）表示工艺   （N,0）表示第N个工序 （N,M）表示第N个工序的第M个工步
        /// </summary>
        /// <param name="a">第几个工序</param>
        /// <param name="b">第几个工步</param>
        private void InitializeInputBox(int a, int b)
        {
            if (a == 0 && b == 0)
            {
                //读取并刷新工艺面板
                foreach (Control control in tlpGongyiNormal.Controls)
                {
                    if (control is TextBox)
                    {
                        ((TextBox)control).Text = XML3DPPM.GetIndexAttr(a, b, control.Name, XmlFile);
                    }
                }
            }
            else if (a > 0 && b == 0)
            {
                //读取并刷新工序面板
                foreach (Control control in tlpGongxuNormal.Controls)
                {
                    if (control is TextBox)
                    {
                        ((TextBox)control).Text = XML3DPPM.GetIndexAttr(a, b, control.Name, XmlFile);
                    }
                    else if (control is CheckBox)
                    {
                        bool check = NXFun.string2bool(XML3DPPM.GetIndexAttr(a, b, control.Name, XmlFile));
                        ((CheckBox)control).CheckState = check?CheckState.Checked:CheckState.Unchecked;
                    }
                }
            }
            else if (a > 0 && b > 0)
            {
                //读取并刷新工步面板
                foreach (Control control in tlpGongbu.Controls)
                {
                    if (control is TextBox)
                    {
                        ((TextBox)control).Text = XML3DPPM.GetIndexAttr(a, b, control.Name, XmlFile);
                    }
                }
                //读取并刷新子工步面板
                UpdateZigongbu(a, b, true);
            }
        }
        //更新或保存子工步信息
        /// <summary>
        /// 更新或保存子工步信息
        /// </summary>
        /// <param name="a">工序</param>
        /// <param name="b">工步</param>
        /// <param name="isRead">true为读xml到控件，flase为写控件到xml</param>
        private void UpdateZigongbu(int a, int b, bool isRead)
        {
            if (isRead)
            {
                S_ChildStep[] childsteps = XML3DPPM.GetChildStepList(a, b, XmlFile);
                dgvZigongbu.Rows.Clear();
                for (int i = 0; i < childsteps.Length; i++)
                {
                    dgvZigongbu.Rows.Add();
                    dgvZigongbu[0, i].Value = i;
                    dgvZigongbu[1, i].Value = childsteps[i].name;
                    dgvZigongbu[2, i].Value = childsteps[i].renju;
                    dgvZigongbu[3, i].Value = childsteps[i].liangju;
                    dgvZigongbu[4, i].Value = childsteps[i].beizhu;
                }
            }
            else
            {
                List<S_ChildStep> childsteps = new List<S_ChildStep>();
                S_ChildStep childstep;
                for (int i = 0; i < dgvZigongbu.Rows.Count; i++)
                {
                    childstep.name = (string)dgvZigongbu[1, i].Value;
                    childstep.renju = (string)dgvZigongbu[2, i].Value;
                    childstep.liangju = (string)dgvZigongbu[3, i].Value;
                    childstep.beizhu = (string)dgvZigongbu[4, i].Value;
                    if (!(string.IsNullOrEmpty(childstep.name) && 
                        string.IsNullOrEmpty(childstep.renju) && 
                        string.IsNullOrEmpty(childstep.liangju) &&
                        string.IsNullOrEmpty(childstep.beizhu)))
                    {
                        childsteps.Add(childstep);
                    }                    
                }
                XML3DPPM.SetChildStepList(a, b, childsteps, XmlFile, TemplateXML);
            }

        }
        //刷新编辑板
        /// <summary>
        /// 刷新编辑板
        /// </summary>
        private void FreshEditTable()
        {
            tsrbtnOk.Visible = IsEdit;
            tsrbtnCancel.Visible = IsEdit;
            tsrbtnEdit.Visible = !IsEdit;
            tvwGongYi.Enabled = !IsEdit;
            tsrGongyiTree.Enabled = !IsEdit;
            SetEditTableReadOnly(!IsEdit);
            if (OnEditGongxu == 0 && OnEditGongbu == 0)
            {
                //显示工艺面板
                tlpGongyiNormal.Show();
                tlpGongxuNormal.Hide();
                splGongbu.Hide();
                //读取工艺节点信息
                InitializeInputBox(0, 0);
            }
            else if (OnEditGongxu > 0 && OnEditGongbu == 0)
            {
                //显示工序面板
                tlpGongyiNormal.Hide();
                tlpGongxuNormal.Show();
                splGongbu.Hide();
                InitializeInputBox(OnEditGongxu, OnEditGongbu);
            }
            else if (OnEditGongxu > 0 && OnEditGongbu > 0)
            {
                //显示工步面板
                tlpGongyiNormal.Hide();
                tlpGongxuNormal.Hide();
                splGongbu.Show();
                InitializeInputBox(OnEditGongxu, OnEditGongbu);
            }
        }
        // 刷新工艺树
        /// <summary>
        /// 刷新工艺树
        /// </summary>
        private void FreshGongYiTree()
        {
            tvwGongYi.Nodes.Clear();
            XmlDocument textDoc = new XmlDocument();
            textDoc.Load(XmlFile);
            XmlNode gongyiNode = textDoc.SelectSingleNode("//Gongyi");
            TreeNode gongyiTreeNode = new TreeNode();
            gongyiTreeNode.Name = gongyiNode.Attributes["GUID"].Value.ToString();
            foreach (XmlNode gongyiSubNode in gongyiNode.ChildNodes)
            {
                if (gongyiSubNode.Name == "Information")
                {
                    gongyiTreeNode.ImageIndex = 0;
                    gongyiTreeNode.SelectedImageIndex = 0;
                    gongyiTreeNode.Text = gongyiSubNode.Attributes["gongyi_jianming"].Value.ToString();
                    gongyiTreeNode.Tag = "Gongyi";
                    tvwGongYi.Nodes.Add(gongyiTreeNode);
                }
                else if (gongyiSubNode.Name == "Gongxu")
                {
                    TreeNode gongxuTreeNode = new TreeNode();
                    gongxuTreeNode.Name = gongyiSubNode.Attributes["GUID"].Value.ToString();
                    foreach (XmlNode gongxuSubNode in gongyiSubNode.ChildNodes)
                    {
                        if (gongxuSubNode.Name == "Information")
                        {
                            gongxuTreeNode.ImageIndex = 1;
                            gongxuTreeNode.SelectedImageIndex = 1;
                            gongxuTreeNode.Text = 
                                gongxuSubNode.Attributes["gongxu_gongxuhao"].Value.ToString() + 
                                "-" + 
                                gongxuSubNode.Attributes["gongxu_gongxumingcheng"].Value.ToString();
                            gongxuTreeNode.Tag = "Gongxu";
                            gongyiTreeNode.Nodes.Add(gongxuTreeNode);
                        }
                        else if (gongxuSubNode.Name == "Gongbu")
                        {
                            TreeNode gongbuTreeNode = new TreeNode();
                            gongbuTreeNode.Name = gongxuSubNode.Attributes["GUID"].Value.ToString();
                            foreach (XmlNode gongbuSubNode in gongxuSubNode)
                            {
                                if (gongbuSubNode.Name == "Information")
                                {
                                    gongbuTreeNode.ImageIndex = 2;
                                    gongbuTreeNode.SelectedImageIndex = 2;
                                    gongbuTreeNode.Text = 
                                        gongbuSubNode.Attributes["gongbu_gongbuhao"].Value.ToString() + 
                                        "-" + 
                                        gongbuSubNode.Attributes["gongbu_gongbuneirong"].Value.ToString();
                                    gongbuTreeNode.Tag = "Gongbu";
                                    gongxuTreeNode.Nodes.Add(gongbuTreeNode);
                                }
                            }
                        }
                    }
                }
            }
            tvwGongYi.ExpandAll();
        }
        //删除工序或者工步
        //  0 0 工艺    不可用
        //  N 0 第N个工序
        //  N M 第N个工序的第M个工步
        /// <summary>
        /// 删除工序或者工步
        /// </summary>
        /// <param name="gongxu_index">工序坐标</param>
        /// <param name="gongbu_index">工步坐标</param>
        private bool DelNodeFun(int gongxu_index,int gongbu_index)
        {
            return XML3DPPM.DelNode(gongxu_index, gongbu_index, XmlFile);
        }
        //添加工序节点的函数
        //插入节点坐标的位置：
        //原    新（index）
        //		0
        //0-------
        //		1
        //1-------
        //		2
        //2-------
        //		3 or -1
        //--------
        /// <summary>
        /// 添加工序节点
        /// </summary>
        /// <param name="index">插入节点坐标的位置</param>
        private void AddGongxuFun(int index)
        {
            XML3DPPM.AddGongxu(index, "普通工序", XmlFile, TemplateXML);
        }
        //添加工步节点的函数
        //插入节点坐标的位置：
        //原    新（index）
        //		0
        //0-------
        //		1
        //1-------
        //		2
        //2-------
        //		3 or -1
        //--------
        /// <summary>
        /// 添加工步节点
        /// </summary>
        /// <param name="gongxu_index">工序位置 从0算起 </param>
        /// <param name="gongbu_index">新工步位置  0  为首 …… -1为末</param>
        private void AddGongbuFun(int gongxu_index, int gongbu_index)
        {
            XML3DPPM.AddGongbu(gongxu_index, gongbu_index, XmlFile, TemplateXML);
        }

        //添加工序按钮
        private void tsrbtnAddGongxu_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwGongYi.SelectedNode;
            if (sel == null)
            {
                SetStatusLabel("请先选中树节点",1);
                return;
            }
            if (sel.Tag.ToString() == "Gongyi")
            {
                AddGongxuFun(-1);
                SetStatusLabel("已创建新工序到末尾", 2);
                FreshGongYiTree();
                //编辑工序节点
                OnEditGongxu = XML3DPPM.GetGongxuCount(XmlFile);
                OnEditGongbu = 0;
                IsEdit = true;
                FreshEditTable();
                return;
            }
            else if (sel.Tag.ToString() == "Gongxu")
            {
                int index = sel.Index;
                AddGongxuFun(index);
                SetStatusLabel("已插入新工序在选中节点前", 2);
                FreshGongYiTree();
                //编辑工序节点
                OnEditGongxu = index + 1;
                OnEditGongbu = 0;
                IsEdit = true;
                FreshEditTable();
                return;
            }
            else if (sel.Tag.ToString() == "Gongbu")
            {
                SetStatusLabel("请选中工艺或工序节点", 2);
                return;
            }
            return;
        }
        //删除工序按钮
        private void tsrbtnDelGongxu_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwGongYi.SelectedNode;
            if (sel == null || sel.Tag.ToString() != "Gongxu")
            {
                SetStatusLabel("请先选中工序节点", 1);
                return;
            }
            DialogResult dialogResult = MessageBox.Show("确定删除本工序？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.Yes)
            {
                SetStatusLabel("已取消操作", 0);
                return;
            }
            if (DelNodeFun(sel.Index + 1, 0))
            {
                SetStatusLabel("工序已删除", 2);
            }
            else
            {
                SetStatusLabel("工序删除失败", 1);
            }
            OnEditGongbu = 0;
            OnEditGongxu = 0;
            IsEdit = false;
            FreshGongYiTree();
            FreshEditTable();
            return;
        }
        //删除工步按钮
        private void tsrbtnDelGongbu_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwGongYi.SelectedNode;
            if (sel == null || sel.Tag.ToString() != "Gongbu")
            {
                SetStatusLabel("请先选中工步节点", 1);
                return;
            }
            DialogResult dialogResult = MessageBox.Show("确定删除本工步？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.Yes)
            {
                SetStatusLabel("已取消操作", 0);
                return;
            }
            if (DelNodeFun(sel.Parent.Index + 1, sel.Index + 1))
            {
                SetStatusLabel("工步已删除", 2);
            }
            else
            {
                SetStatusLabel("工步删除失败", 1);
            }
            OnEditGongbu = 0;
            OnEditGongxu = 0;
            IsEdit = false;
            FreshGongYiTree();
            FreshEditTable();
            return;
        }
        //添加工步按钮
        private void tsrbtnAddGongbu_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwGongYi.SelectedNode;
            if (sel == null || sel.Tag.ToString() == "Gongyi")
            {
                SetStatusLabel("请选中工序或工步节点", 1);
                return;
            }
            else if (sel.Tag.ToString() == "Gongxu")
            {
                int index = sel.Index;
                AddGongbuFun(index, -1);
                SetStatusLabel("已插入新工步在选中工序末尾", 2);
                FreshGongYiTree();
                //编辑工序节点
                OnEditGongxu = index + 1;
                OnEditGongbu = XML3DPPM.GetGongbuCount(index+1,XmlFile);
                IsEdit = true;
                FreshEditTable();
                return;                
            }
            else if (sel.Tag.ToString() == "Gongbu")
            {
                int a = sel.Parent.Index;
                int b = sel.Index;
                AddGongbuFun(a,b);
                SetStatusLabel("已插入新工步在选中节点前", 2);
                FreshGongYiTree();
                //编辑工序节点
                OnEditGongxu = a + 1;
                OnEditGongbu = b + 1;
                IsEdit = true;
                FreshEditTable();
                return;   
            }
            FreshGongYiTree();
            return;
        }
        //向上移动按钮
        private void tsrbtnMoveUp_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwGongYi.SelectedNode;
            if (sel == null || sel.Tag.ToString() == "Gongyi")
            {
                SetStatusLabel("请选中工序或工步节点", 1);
                return;
            }
            else if (sel.Tag.ToString() == "Gongxu")
            {
                if (sel.Index>0)
                {
                    int a = sel.Index;
                    XML3DPPM.MoveNode(a + 1, 0, true,XmlFile);
                    SetStatusLabel("已成功向上移动工序", 2);
                    FreshGongYiTree();
                    tvwGongYi.SelectedNode = GetIndexedTreeNode(a, 0);
                }
                else
                {
                    SetStatusLabel("该工序已置顶", 1);
                }
                
            }
            else if (sel.Tag.ToString() == "Gongbu")
            {
                if (sel.Index > 0)
                {
                    int a = sel.Parent.Index;
                    int b = sel.Index;
                    XML3DPPM.MoveNode(a + 1, b + 1, true, XmlFile);
                    SetStatusLabel("已成功向上移动工步", 2);
                    FreshGongYiTree();
                    tvwGongYi.SelectedNode = GetIndexedTreeNode(a + 1, b);
                }
                else
                {
                    SetStatusLabel("该工步已置顶", 1);
                }
            }
            return;
        }
        //向下移动按钮
        private void tsrbtnMoveDown_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwGongYi.SelectedNode;
            if (sel == null)
            {
                SetStatusLabel("请选中工序或工步节点", 1);
                return;
            }
            if (sel.Tag.ToString() == "Gongyi")
            {
                SetStatusLabel("请选中工序或工步节点", 1);
                return;
            }
            else if (sel.Tag.ToString() == "Gongxu")
            {
                if (sel.NextNode != null)
                {
                    int a = sel.Index;
                    XML3DPPM.MoveNode(a + 1, 0, false, XmlFile);
                    SetStatusLabel("已成功向下移动工序", 2);
                    FreshGongYiTree();
                    tvwGongYi.SelectedNode = GetIndexedTreeNode(a + 2, 0);
                }
                else
                {
                    SetStatusLabel("该工序已置尾", 1);
                }

            }
            else if (sel.Tag.ToString() == "Gongbu")
            {
                if (sel.NextNode != null)
                {
                    int a = sel.Parent.Index;
                    int b = sel.Index;
                    XML3DPPM.MoveNode(a + 1, b + 1, false, XmlFile);
                    SetStatusLabel("已成功向下移动工步", 2);
                    FreshGongYiTree();
                    tvwGongYi.SelectedNode = GetIndexedTreeNode(a + 1, b + 2);
                }
                else
                {
                    SetStatusLabel("该工步已置尾", 1);
                }
            }
            return;
        }
        //确定按钮
        private void tsrbtnOk_Click(object sender, EventArgs e)
        {
            if (IsEditYingdu)
            {
                //编辑硬度
                dgvYingdu.EndEdit();
                List<S_Yingdu> YingduList = new List<S_Yingdu>();
                S_Yingdu s_yingdu;
                for (int i = 0; i < dgvYingdu.Rows.Count; i++)
                {
                    s_yingdu.gongxuhao = (string)dgvYingdu[1, i].Value;
                    s_yingdu.yingdu = (string)dgvYingdu[2, i].Value;
                    YingduList.Add(s_yingdu);
                }
                bool result = XML3DPPM.SetYingduList(YingduList, XmlFile);
                if (!result)
                {
                    SetStatusLabel("硬度设置失败！请检查输入工序号正确与否。",1);
                    return;
                }
                tsrbtnSetHardness.Visible = true;
                IsEditYingdu = false;                
                dgvYingdu.SendToBack();
                FreshEditTable();
                SetStatusLabel("硬度设置成功", 2);
                return;
            }
            else
            {
                //编辑工艺
                dgvZigongbu.EndEdit();
                IsEdit = false;
                //保存
                if (OnEditGongxu == 0 && OnEditGongbu == 0)
                {
                    //保存工艺界面
                    foreach (Control control in tlpGongyiNormal.Controls)
                    {
                        if (control is TextBox)
                        {
                            XML3DPPM.SetIndexAttr(OnEditGongxu, OnEditGongbu, control.Name, ((TextBox)control).Text, XmlFile);
                        }
                    }
                }
                else if (OnEditGongxu > 0 && OnEditGongbu == 0)
                {
                    //保存工序界面
                    foreach (Control control in tlpGongxuNormal.Controls)
                    {
                        if (control is TextBox)
                        {
                            XML3DPPM.SetIndexAttr(OnEditGongxu, OnEditGongbu, control.Name, ((TextBox)control).Text, XmlFile);
                        }
                        else if (control is CheckBox)
                        {
                            bool check = ((CheckBox)control).CheckState == CheckState.Checked;
                            XML3DPPM.SetIndexAttr(OnEditGongxu, OnEditGongbu, control.Name, NXFun.bool2string(check), XmlFile);
                        }
                    }
                }
                else if (OnEditGongxu > 0 && OnEditGongbu > 0)
                {
                    //保存工步界面
                    foreach (Control control in tlpGongbu.Controls)
                    {
                        if (control is TextBox)
                        {
                            XML3DPPM.SetIndexAttr(OnEditGongxu, OnEditGongbu, control.Name, ((TextBox)control).Text, XmlFile);
                        }
                    }
                    UpdateZigongbu(OnEditGongxu, OnEditGongbu, false);
                }
                FreshEditTable();
                FreshGongYiTree();
                SetStatusLabel("已保存编辑", 2);
            }
        }
        //取消按钮
        private void tsrbtnCancel_Click(object sender, EventArgs e)
        {
            if (IsEditYingdu)
            {
                //编辑硬度
                tsrbtnSetHardness.Visible = true;
                IsEditYingdu = false;
                dgvYingdu.SendToBack();
                FreshEditTable();
                SetStatusLabel("已取消编辑硬度", 0);
            }
            else
            {
                IsEdit = false;
                FreshEditTable();
                SetStatusLabel("已取消编辑", 0);
            }
        }
        //编辑按钮
        private void tsrbtnEdit_Click(object sender, EventArgs e)
        {
            IsEdit = true;
            FreshEditTable();
        }
        //设置硬度按钮
        private void tsrbtnSetHardness_Click(object sender, EventArgs e)
        {
            IsEditYingdu = true;

            tsrbtnOk.Visible = true;
            tsrbtnCancel.Visible = true;
            tsrbtnEdit.Visible = false;
            tsrbtnSetHardness.Visible = false;
            tvwGongYi.Enabled = false;
            tsrGongyiTree.Enabled = false;

            List<S_Yingdu> YingduList = XML3DPPM.GetYingduList(XmlFile);
            int i = 0;
            dgvYingdu.Rows.Clear();
            string tempYD = "硬度临时值";
            foreach (S_Yingdu s_yingdu in YingduList)
            {
                if (s_yingdu.yingdu != tempYD)
                {
                    dgvYingdu.Rows.Add();
                    dgvYingdu[0, i].Value = (i+1).ToString();
                    dgvYingdu[1, i].Value = s_yingdu.gongxuhao;
                    dgvYingdu[2, i].Value = tempYD = s_yingdu.yingdu;
                    i++;
                }
            }
            dgvYingdu.BringToFront();
            SetStatusLabel("提示：仅设置发生硬度更改的工序即可！", 2);
        }
        //鼠标右击树控件
        private void tvwGongYi_MouseClick(object sender, MouseEventArgs e)
        {
            //判断是否是右键
            if (e.Button == MouseButtons.Right)
            {
                System.Drawing.Point clickPoint = new System.Drawing.Point(e.X, e.Y);
                TreeNode sel = tvwGongYi.GetNodeAt(clickPoint);
                //判断点击的是否是一个节点
                if (sel != null)
                {
                    tvwGongYi.SelectedNode = sel;
                    if (sel.Tag.ToString() == "Gongyi")
                    {
                        sel.ContextMenuStrip = cmsGongyi;
                        tsrbtnAddGongxu.Enabled = true;
                        tsrbtnDelGongxu.Enabled = false;
                        tsrbtnAddGongbu.Enabled = false;
                        tsrbtnDelGongbu.Enabled = false;
                        tsrbtnMoveUp.Enabled = false;
                        tsrbtnMoveDown.Enabled = false;
                    }
                    else if (sel.Tag.ToString() == "Gongxu")
                    {
                        sel.ContextMenuStrip = cmsGongxu;
                        tsrbtnAddGongxu.Enabled = true;
                        tsrbtnDelGongxu.Enabled = true;
                        tsrbtnAddGongbu.Enabled = true;
                        tsrbtnDelGongbu.Enabled = false;
                        tsrbtnMoveUp.Enabled = sel.Index != 0;
                        tsrbtnMoveDown.Enabled = sel.NextNode != null;
                    }
                    else if (sel.Tag.ToString() == "Gongbu")
                    {
                        sel.ContextMenuStrip = cmsGongbu;
                        tsrbtnAddGongxu.Enabled = false;
                        tsrbtnDelGongxu.Enabled = false;
                        tsrbtnAddGongbu.Enabled = true;
                        tsrbtnDelGongbu.Enabled = true;
                        tsrbtnMoveUp.Enabled = sel.Index != 0;
                        tsrbtnMoveDown.Enabled = sel.NextNode != null;
                    }
                }
            }
        }
        //编辑工艺菜单
        private void cmsGongyiEditGongyi_Click(object sender, EventArgs e)
        {
            //编辑工艺
            IsEdit = true;
            OnEditGongxu = 0;
            OnEditGongbu = 0;
            SetStatusLabel("正在编辑工艺", 0);
            FreshEditTable();
        }
        //添加工序菜单
        private void cmsGongyiAddGongxu_Click(object sender, EventArgs e)
        {
            AddGongxuFun(-1);
            SetStatusLabel("已添加工序到末尾", 2);
            FreshGongYiTree();
            //编辑工序节点
            OnEditGongxu = XML3DPPM.GetGongxuCount(XmlFile);
            OnEditGongbu = 0;
            IsEdit = true;
            FreshEditTable();            
        }
        //在上方添加工序菜单
        private void cmsGongxuInsertUp_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwGongYi.SelectedNode;
            if (sel == null || sel.Tag.ToString() != "Gongxu")
            {
                SetStatusLabel("请选中工序节点", 1);
                return;
            }
            int a = sel.Index;
            AddGongxuFun(a);
            SetStatusLabel("已添加工序到选中节点上方", 2);
            FreshGongYiTree();
            //编辑工序节点
            OnEditGongxu = a + 1;
            OnEditGongbu = 0;
            IsEdit = true;
            FreshEditTable();
            return;
        }
        //在下方添加工序菜单
        private void cmsGongxuInsertDown_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwGongYi.SelectedNode;
            if (sel == null)
            {
                SetStatusLabel("请选中工序节点", 1);
                return;
            }
            if (sel.Tag.ToString() != "Gongxu")
            {
                SetStatusLabel("请选中工序节点", 1);
                return;
            }
            int a = sel.Index;
            AddGongxuFun(a + 1);
            SetStatusLabel("已添加工序到选中节点下方", 2);
            FreshGongYiTree();
            //编辑工序节点
            OnEditGongxu = a + 2;
            OnEditGongbu = 0;
            IsEdit = true;
            FreshEditTable();
            return;
        }
        //编辑工序菜单
        private void cmsGongxuEditGongxu_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwGongYi.SelectedNode;
            if (sel == null || sel.Tag.ToString() != "Gongxu")
            {
                SetStatusLabel("请选中工序节点", 1);
                return;
            }
            IsEdit = true;
            OnEditGongxu = sel.Index + 1;
            OnEditGongbu = 0;
            SetStatusLabel("正在编辑工序", 0);
            FreshEditTable();
        }
        //删除工序菜单
        private void cmsGongxuDelGongxu_Click(object sender, EventArgs e)
        {
            tsrbtnDelGongxu_Click(sender, e);
        }
        //添加工步菜单
        private void cmsGongxuAddGongbu_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwGongYi.SelectedNode;
            if (sel == null || sel.Tag.ToString() != "Gongxu")
            {
                SetStatusLabel("请选中工序节点", 1);
                return;
            }
            int index = sel.Index;
            AddGongbuFun(index, -1);
            SetStatusLabel("已插入新工步在选中工序末尾", 2);
            FreshGongYiTree();
            //编辑工序节点
            OnEditGongxu = index + 1;
            OnEditGongbu = XML3DPPM.GetGongbuCount(index + 1, XmlFile);
            IsEdit = true;
            FreshEditTable();
            return;

        }
        //上方添加工步菜单
        private void cmsGongbuInsertUp_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwGongYi.SelectedNode;
            if (sel == null || sel.Tag.ToString() != "Gongbu")
            {
                SetStatusLabel("请选中工步节点", 1);
                return;
            }
            int a = sel.Parent.Index;
            int b = sel.Index;
            AddGongbuFun(a, b);
            SetStatusLabel("已插入新工步在选中节点前", 2);
            FreshGongYiTree();
            //编辑工序节点
            OnEditGongxu = a + 1;
            OnEditGongbu = b + 1;
            IsEdit = true;
            FreshEditTable();
            return;
        }
        //下方添加工步菜单
        private void cmsGongbuInsertDown_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwGongYi.SelectedNode;
            if (sel == null || sel.Tag.ToString() != "Gongbu")
            {
                SetStatusLabel("请选中工步节点", 1);
                return;
            }
            int a = sel.Parent.Index;
            int b = sel.Index;
            AddGongbuFun(a, b+1);
            SetStatusLabel("已插入新工步在选中节点后", 2);
            FreshGongYiTree();
            //编辑工序节点
            OnEditGongxu = a + 1;
            OnEditGongbu = b + 2;
            IsEdit = true;
            FreshEditTable();
            return;
        }
        //编辑工步菜单
        private void cmsGongbuEditGongbu_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwGongYi.SelectedNode;
            if (sel == null || sel.Tag.ToString() != "Gongbu")
            {
                SetStatusLabel("请选中工步节点", 1);
                return;
            }
            IsEdit = true;
            OnEditGongxu = sel.Parent.Index + 1;
            OnEditGongbu = sel.Index + 1;
            SetStatusLabel("正在编辑工步", 0);
            FreshEditTable();
        }
        //删除工步菜单
        private void cmsGongbuDelGongbu_Click(object sender, EventArgs e)
        {
            tsrbtnDelGongbu_Click(sender, e);
        }
        //得到树中节点
        /// <summary>
        /// 得到书中节点    0 0  为根
        /// </summary>
        /// <param name="a">二级节点坐标</param>
        /// <param name="b">三级节点坐标</param>
        /// <returns>节点</returns>
        private TreeNode GetIndexedTreeNode(int a,int b)
        {
            try
            {
                TreeNode node = null;
                TreeNode root = null;
                //找到根节点
                foreach (TreeNode n in tvwGongYi.Nodes)
                {
                    if (n.Parent == null)
                    {
                        root = n;
                    }
                }
                if (a == 0 && b == 0)
                {
                    //返回工艺节点
                    return root;
                }
                else if (a != 0 && b == 0)
                {
                    //返回工序节点
                    node = root.FirstNode;
                    if (node == null)
                    {
                        return null;
                    }
                    for (int i = 1; i < a; i++)
                    {
                        node = node.NextNode;
                        if (node == null)
                        {
                            break;
                        }
                    }
                    return node;
                }
                else if (a != 0 && b != 0)
                {
                    //返回工步节点
                    node = root.FirstNode;
                    if (node == null)
                    {
                        return null;
                    }
                    for (int i = 1; i < a; i++)
                    {
                        node = node.NextNode;
                        if (node == null)
                        {
                            break;
                        }
                    }
                    if (node == null)
                    {
                        return null;
                    }
                    //找到工步
                    node = node.FirstNode;
                    if (node == null)
                    {
                        return null;
                    }
                    for (int i = 1; i < b; i++)
                    {
                        node = node.NextNode;
                        if (node == null)
                        {
                            break;
                        }
                    }
                    return node;
                }
                return node;
            }
            catch/* (System.Exception ex)*/
            {
                return null;
            }            
        }
        //选中节点发生改变
        private void tvwGongYi_AfterSelect(object sender, TreeViewEventArgs e)
        {
            IsEdit = false;
            TreeNode sel = e.Node;
            //判断点击的是否是一个节点
            if (sel != null)
            {
                tvwGongYi.SelectedNode = sel;
                if (sel.Tag.ToString() == "Gongyi")
                {
                    IsEdit = false;
                    OnEditGongxu = 0;
                    OnEditGongbu = 0;
                    FreshEditTable();
                    tsrbtnAddGongxu.Enabled = true;
                    tsrbtnDelGongxu.Enabled = false;
                    tsrbtnAddGongbu.Enabled = false;
                    tsrbtnDelGongbu.Enabled = false;
                    tsrbtnMoveUp.Enabled = false;
                    tsrbtnMoveDown.Enabled = false;
                }
                else if (sel.Tag.ToString() == "Gongxu")
                {
                    IsEdit = false;
                    OnEditGongxu = sel.Index + 1;
                    OnEditGongbu = 0;
                    FreshEditTable();
                    tsrbtnAddGongxu.Enabled = true;
                    tsrbtnDelGongxu.Enabled = true;
                    tsrbtnAddGongbu.Enabled = true;
                    tsrbtnDelGongbu.Enabled = false;
                    tsrbtnMoveUp.Enabled = sel.Index != 0;
                    tsrbtnMoveDown.Enabled = sel.NextNode != null;
                }
                else if (sel.Tag.ToString() == "Gongbu")
                {
                    IsEdit = false;
                    OnEditGongxu = sel.Parent.Index + 1;
                    OnEditGongbu = sel.Index + 1;
                    FreshEditTable();
                    tsrbtnAddGongxu.Enabled = false;
                    tsrbtnDelGongxu.Enabled = false;
                    tsrbtnAddGongbu.Enabled = true;
                    tsrbtnDelGongbu.Enabled = true;
                    tsrbtnMoveUp.Enabled = sel.Index != 0;
                    tsrbtnMoveDown.Enabled = sel.NextNode != null;
                }
            }
        }
    }
}
