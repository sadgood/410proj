using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using NXOpen;
using NXOpen.UF;
using WaveRelationControl;
using System.Collections;

namespace TDPPM
{
    public partial class ModelEditPanel : EditPanel
    {
        private string ModelPreName = ""; //模型前缀名;
        private int NameNum = 0;        //模型名称流水号; 

        //（0，0）表示设计模型   （1，1） 第一个辅助模型  ……
        private int OnEditModel_a = 0; //0 工序模型  1  辅助模型  2 余量图
        private int OnEditModel_b = 0; //0 根节点    1  第一个节点  ……
        private bool isAutoFreshWave = true;   //是否自动更新Wave关系图;
        private bool isWaveShowAll = true;  //显示级别  true全部   false 单个; 
        private int WaveShowNameType = 1;   //WaveView显示名称类型  0  模型名称  1  模型描述
        private List<E_WaveType> WaveSetting_All = new List<E_WaveType>();//Wave全部显示的设置
        private List<E_WaveType> WaveSetting_Single = new List<E_WaveType>();//单个模型的WAVE显示设置
        List<Tag> WaveSelectObjects = new List<Tag>();  //wave编辑时所选的几何
        public ModelEditPanel()
        {
            InitializeComponent();
        }
        //控件初始化设置
        public void Init()
        {
            if (mainDlg == null || string.IsNullOrEmpty(ProPath) || string.IsNullOrEmpty(ProName))
            {
                //缺少初值，所有控件禁用  正常情况不应执行到这里
                tsrModelTree.Enabled = false;
                tsrWaveView.Enabled = false;
            }
            else
            {
                XmlFile = ToFullPath(NXFun.ProcessXML);
                //创建并得到工艺节点GUID
                string guid = XML3DPPM.GetGongyiGuid(XmlFile);
                ModelPreName = "M" + guid.Substring(0, 4).ToUpper() + "_";
                //得到流水号
                NameNum = XML3DPPM.GetModelNameNum(ModelPreName, XmlFile);
                //控件状态初始化
                IsEdit = false;
                tsrModelTree.Enabled = true;
                tsrWaveView.Enabled = true;
                //wave过滤器设置;
                WaveSetting_All.Add(E_WaveType.LINKED_BODY);
                WaveSetting_Single.Add(E_WaveType.LINKED_BODY);
                WaveSetting_Single.Add(E_WaveType.LINKED_POINT);
                WaveSetting_Single.Add(E_WaveType.LINKED_FACE);
                WaveSetting_Single.Add(E_WaveType.LINKED_DATUM);
                WaveSetting_Single.Add(E_WaveType.LINKED_CURVE);
                WaveSetting_Single.Add(E_WaveType.LINKED_SKETCH);

                FreshWaveViewToolStripStatus();

                FreshModelTree();
                pnlModelEdit.Hide();
                tsrWaveView.Visible = true;
                pnlWaveView.Show();
                FreshWaveMap(isAutoFreshWave, true, "");
            }
        }
        //刷新显示wave关系图
        public void FreshWaveMap(bool isfresh, bool isShowAll, string filename)
        {
            if (!isfresh)
            {
                return;
            }
            waveRelationCtl.HorizontalScroll.Value = 0;
            waveRelationCtl.VerticalScroll.Value = 0;
            if (isShowAll)
            {
                waveRelationCtl.ClearAll();
                //加载Wave关系图
                List<S_Model> ModelList = XML3DPPM.GetModelList(XmlFile);
                List<S_WaveInfor> WaveInforList = new List<S_WaveInfor>();
                Hashtable ht = new Hashtable();
                ModelClass M0;
                ModelClass M1;
                ModelClass M2;
                ModelClass Mout = new ModelClass("未知", "外部");
                ArrayList AL = new ArrayList();
                foreach (S_Model model in ModelList)
                {
                    WaveInforList.AddRange(NXFun.GetSinglePrtWaveInfor(ToFullPath(model.filename)));
                    if (!string.IsNullOrEmpty(model.filename))
                    {
                        M0 = new ModelClass(ToFullPath(model.filename), GetWaveName(model.filename));
                        ht.Add(ToFullPath(model.filename), M0);
                        AL.Add(M0);
                    }
                }
                ht.Add("未知", Mout);
                foreach (S_WaveInfor waveinfor in WaveInforList)
                {
                    if (WaveSetting_All.Contains(waveinfor.wave_type))
                    {
                        if (ht.Contains(waveinfor.fatherpath))
                        {
                            M1 = (ModelClass)ht[waveinfor.fatherpath];
                        }
                        else
                        {
                            M1 = Mout;
                            AL.Add(Mout);
                        }
                        M2 = (ModelClass)ht[waveinfor.childpath];
                        M1.AddWaveChild(M2);
                    }
                }
                waveRelationCtl.SetRootClass(AL);
            }
            else
            {
                if (string.IsNullOrEmpty(filename))
                {
                    return;
                }
                string fullpath = ToFullPath(filename);
                waveRelationCtl.ClearAll();
                //加载Wave关系图
                List<S_Model> ModelList = XML3DPPM.GetModelList(XmlFile);
                List<S_WaveInfor> WaveInforList = new List<S_WaveInfor>();
                Hashtable ht = new Hashtable();
                ModelClass M0;
                ModelClass M1;
                ModelClass M2;
                ModelClass Mout = new ModelClass("未知", "外部");
                ArrayList AL = new ArrayList();
                foreach (S_Model model in ModelList)
                {
                    WaveInforList.AddRange(NXFun.GetSinglePrtWaveInfor(ToFullPath(model.filename)));
                    if (!string.IsNullOrEmpty(model.filename))
                    {
                        M0 = new ModelClass(ToFullPath(model.filename), GetWaveName(model.filename));
                        ht.Add(ToFullPath(model.filename), M0);
                        AL.Add(M0);
                    }
                }
                ht.Add("未知", Mout);
                foreach (S_WaveInfor waveinfor in WaveInforList)
                {
                    if (WaveSetting_Single.Contains(waveinfor.wave_type))
                    {
                        if (ht.Contains(waveinfor.fatherpath))
                        {
                            M1 = (ModelClass)ht[waveinfor.fatherpath];
                        }
                        else
                        {
                            M1 = Mout;
                            AL.Add(M1);
                        }
                        M2 = (ModelClass)ht[waveinfor.childpath];
                        if (waveinfor.fatherpath == fullpath || waveinfor.childpath == fullpath)
                        {
                            M1.AddWaveChild(M2);
                        }
                    }
                }
                ArrayList remove = new ArrayList();
                foreach (ModelClass m in AL)
                {
                    if (m.ModelFileName != fullpath) //不是中间节点
                    {
                        if (!m.ParentNodeList.Contains(ht[fullpath])) //不是filename的子节点
                        {
                            if (!m.WaveChild.Contains(ht[fullpath]))  //不是filename的父节点
                            {
                                remove.Add(m);
                            }
                        }
                    }
                }
                foreach (object obj in remove)
                {
                    AL.Remove(obj);
                }
                waveRelationCtl.SetRootClass(AL);
            }

        }
        private string GetWaveName(string filename)
        {
            if (WaveShowNameType == 0)
            {
                return filename;
            }
            else
            {
                List<S_Model> modellist = XML3DPPM.GetModelList(XmlFile);
                S_Model model = modellist.Find(delegate(S_Model m) { return m.filename == filename; });
                if (model.Equals(default(S_Model)))
                {
                    return "外部";
                }
                else if (model.a == 0 && model.b == 0)
                {
                    //设计模型                
                    return "设计模型";
                }
                else if (model.a == 0 && model.b > 0)
                {
                    //工序N_工序内容
                    return XML3DPPM.GetIndexAttr(model.b, 0, "gongxu_gongxuhao", XmlFile) +
                        "_" +
                        XML3DPPM.GetIndexAttr(model.b, 0, "gongxu_gongxumingcheng", XmlFile);
                }
                else
                {
                    //辅助模型或余量图
                    return XML3DPPM.GetModelDescription(model.a, model.b, XmlFile);
                }
            }
        }
        //刷新显示模型树
        private void FreshModelTree()
        {
            tvwModel.Nodes.Clear();
            tvwModel.ImageIndex = 2;
            tvwModel.SelectedImageIndex = 2;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XmlFile);
            XmlNode gongyiNode = xmlDoc.SelectSingleNode("//Gongyi");
            TreeNode gongyiTreeNode = new TreeNode();
            gongyiTreeNode.Name = gongyiNode.Attributes["GUID"].Value.ToString();
            foreach (XmlNode gongyiSubNode in gongyiNode.ChildNodes)
            {
                if (gongyiSubNode.Name == "Information")
                {
                    gongyiTreeNode.Text = gongyiSubNode.Attributes["gongyi_jianming"].Value.ToString();
                    gongyiTreeNode.Tag = "Gongyi";
                    tvwModel.Nodes.Add(gongyiTreeNode);
                }
                else if (gongyiSubNode.Name == "Model")
                {
                    if (!string.IsNullOrEmpty(gongyiSubNode.Attributes["filename"].Value.ToString()))
                    {
                        gongyiTreeNode.Text = gongyiTreeNode.Text + "(" + gongyiSubNode.Attributes["filename"].Value + ")";
                        gongyiTreeNode.ImageIndex = 0;
                        gongyiTreeNode.SelectedImageIndex = 0;
                    }
                    else
                    {
                        gongyiTreeNode.ImageIndex = 1;
                        gongyiTreeNode.SelectedImageIndex = 1;
                    }
                }
                else if (gongyiSubNode.Name == "Gongxu")
                {
                    TreeNode gongxuTreeNode = new TreeNode();
                    gongxuTreeNode.Name = gongyiSubNode.Attributes["GUID"].Value.ToString();
                    foreach (XmlNode gongxuSubNode in gongyiSubNode.ChildNodes)
                    {
                        if (gongxuSubNode.Name == "Information")
                        {
                            gongxuTreeNode.Text = gongxuSubNode.Attributes["gongxu_gongxuhao"].Value.ToString() + "-" + gongxuSubNode.Attributes["gongxu_gongxumingcheng"].Value.ToString();
                            gongxuTreeNode.Tag = "Gongxu";
                            gongyiTreeNode.Nodes.Add(gongxuTreeNode);
                        }
                        else if (gongxuSubNode.Name == "Model")
                        {
                            if (!string.IsNullOrEmpty(gongxuSubNode.Attributes["filename"].Value.ToString()))
                            {
                                gongxuTreeNode.Text = gongxuTreeNode.Text + "(" + gongxuSubNode.Attributes["filename"].Value + ")";
                                gongxuTreeNode.ImageIndex = 0;
                                gongxuTreeNode.SelectedImageIndex = 0;
                            }
                            else
                            {
                                gongxuTreeNode.ImageIndex = 1;
                                gongxuTreeNode.SelectedImageIndex = 1;
                            }
                        }
                    }
                }
                else if (gongyiSubNode.Name == "FuModels")
                {
                    TreeNode fuModelsTreeNode = new TreeNode();
                    fuModelsTreeNode.Text = "辅助模型";
                    fuModelsTreeNode.Tag = "fuModels";
                    tvwModel.Nodes.Add(fuModelsTreeNode);
                    if (gongyiSubNode.ChildNodes.Count > 0)
                    {
                        foreach (XmlNode fuModelSubNode in gongyiSubNode)
                        {
                            TreeNode fuModelTreeNode = new TreeNode();
                            if (fuModelSubNode.Name == "FuModel")
                            {
                                fuModelTreeNode.Text = fuModelSubNode.Attributes["description"].Value.ToString() + "(" + fuModelSubNode.Attributes["filename"].Value.ToString() + ")";
                                fuModelTreeNode.Tag = "fuModel";
                                fuModelTreeNode.Name = fuModelSubNode.Attributes["filename"].Value.ToString();
                                fuModelTreeNode.ImageIndex = 0;
                                fuModelTreeNode.SelectedImageIndex = 0;
                                fuModelsTreeNode.Nodes.Add(fuModelTreeNode);
                            }
                        }
                    }
                }
                else if (gongyiSubNode.Name == "YLTs")
                {
                    TreeNode yltsTreeNode = new TreeNode();
                    yltsTreeNode.Text = "余量图";
                    yltsTreeNode.Tag = "ylts";
                    tvwModel.Nodes.Add(yltsTreeNode);
                    if (gongyiSubNode.ChildNodes.Count > 0)
                    {
                        foreach (XmlNode yltSubNode in gongyiSubNode)
                        {
                            TreeNode yltTreeNode = new TreeNode();
                            if (yltSubNode.Name == "YLT")
                            {
                                yltTreeNode.Text = yltSubNode.Attributes["description"].Value.ToString() + "(" + yltSubNode.Attributes["filename"].Value.ToString() + ")";
                                yltTreeNode.Tag = "ylt";
                                yltTreeNode.Name = yltSubNode.Attributes["filename"].Value.ToString();
                                yltTreeNode.ImageIndex = 0;
                                yltTreeNode.SelectedImageIndex = 0;
                                yltsTreeNode.Nodes.Add(yltTreeNode);
                            }
                        }
                    }
                }
            }
            tvwModel.ExpandAll();
        }
        //刷新显示模型编辑界面
        private void FreshModelEditTable()
        {
            if (IsEdit)
            {
                tsrModelTree.Enabled = false;
                tvwModel.Enabled = false;
                tsrWaveView.Visible = false;
                pnlWaveView.Hide();
                pnlModelEdit.Show();
                txtModelDescription.Text = XML3DPPM.GetModelDescription(OnEditModel_a, OnEditModel_b, XmlFile);
                GetModelEditInfor();
            }
            else
            {
                tsrModelTree.Enabled = true;
                tvwModel.Enabled = true;
                tsrWaveView.Visible = true;
                pnlWaveView.Show();
                pnlModelEdit.Hide();
                FreshWaveMap(isAutoFreshWave, true, "");
            }

        }
        //添加或编辑模型菜单回调（工艺、工序、辅助、余量图）
        private void cmsAddModel_Click(object sender, EventArgs e)
        {

            if (sender.Equals(cmsAddGongyiModel) || sender.Equals(cmsEditGongyiModel))
            {
                //新建或编辑设计模型
                OnEditModel_a = 0;
                OnEditModel_b = 0;
                SetStatusLabel("正在编辑设计模型……", 0);
            }
            else if (sender.Equals(cmsAddGongxuModel) || sender.Equals(cmsEditGongxuModel))
            {
                //新建或编辑工序模型
                OnEditModel_a = 0;
                OnEditModel_b = tvwModel.SelectedNode.Index + 1;
                SetStatusLabel("正在编辑工序模型……", 0);
            }
            else if (sender.Equals(cmsAddFuModel))
            {
                //新建辅助模型
                OnEditModel_a = 1;
                OnEditModel_b = 0;
                SetStatusLabel("正在新建辅助模型……", 0);
            }
            else if (sender.Equals(cmsEditFuModel))
            {
                //编辑辅助模型
                OnEditModel_a = 1;
                OnEditModel_b = tvwModel.SelectedNode.Index + 1;
                SetStatusLabel("正在编辑辅助模型……", 0);
            }
            else if (sender.Equals(cmsAddYLT))
            {
                //新建余量图模型
                OnEditModel_a = 2;
                OnEditModel_b = 0;
                SetStatusLabel("正在新建余量图模型……", 0);
            }
            else if (sender.Equals(cmsEditYLT))
            {
                //编辑余量图模型
                OnEditModel_a = 2;
                OnEditModel_b = tvwModel.SelectedNode.Index + 1;
                SetStatusLabel("正在编辑余量图模型……", 0);
            }
            IsEdit = true;
            FreshModelEditTable();
        }
        //得到模型的详细描述
        /// <summary>
        /// 得到模型的详细描述
        /// </summary>
        /// <param name="filename">模型名称</param>
        /// <returns>描述</returns>
        private string GetModelFullDescription(string filename)
        {
            List<S_Model> modellist = XML3DPPM.GetModelList(XmlFile);
            S_Model model = modellist.Find(delegate(S_Model m) { return m.filename == filename; });
            if (model.Equals(default(S_Model)))
            {
                return "外部";
            }
            return GetModelFullDescription(model.a, model.b);
        }
        //得到模型的详细描述（重载1）
        /// <summary>
        /// 得到模型的详细描述
        /// </summary>
        /// <param name="a">模型一级坐标</param>
        /// <param name="b">模型二级坐标</param>
        /// <returns>描述</returns>
        private string GetModelFullDescription(int a, int b)
        {
            string fulldescription = "";
            string description = XML3DPPM.GetModelDescription(a, b, XmlFile);
            if (a == 0 && b == 0)
            {
                //设计模型
                //设计模型（描述）                
                if (string.IsNullOrEmpty(description))
                {
                    fulldescription = "设计模型";
                }
                else
                {
                    fulldescription = "设计模型(" + description + ")";
                }
            }
            else if (a == 0 && b > 0)
            {
                //工序N_工序内容
                //工序N（描述）工序内容                
                if (string.IsNullOrEmpty(description))
                {

                    fulldescription = "工序" + XML3DPPM.GetIndexAttr(b, 0, "gongxu_gongxuhao", XmlFile) + "_" + XML3DPPM.GetIndexAttr(b, 0, "gongxu_gongxumingcheng", XmlFile);
                }
                else
                {
                    fulldescription = "工序" + XML3DPPM.GetIndexAttr(b, 0, "gongxu_gongxuhao", XmlFile) + "(" + description + ")" + XML3DPPM.GetIndexAttr(b, 0, "gongxu_gongxumingcheng", XmlFile);
                }
            }
            else if (a == 1 && b > 0)
            {
                //辅助模型
                //辅助模型（描述）
                if (string.IsNullOrEmpty(description))
                {
                    fulldescription = "辅助模型";
                }
                else
                {
                    fulldescription = "辅助模型(" + description + ")";
                }
            }
            else if (a == 2 && b > 0)
            {
                //余量图
                //余量图（描述）
                if (string.IsNullOrEmpty(description))
                {
                    fulldescription = "余量图";
                }
                else
                {
                    fulldescription = "余量图(" + description + ")";
                }
            }
            return fulldescription;
        }
        //得到模型编辑界面所有信息
        private void GetModelEditInfor()
        {
            //复制和移动和WAVE的模型列表
            lvwCopy.Items.Clear();
            lvwMove.Items.Clear();
            lvwWaveModel.Items.Clear();
            List<S_Model> modellist = XML3DPPM.GetModelList(XmlFile);
            foreach (S_Model model in modellist)
            {
                if (!string.IsNullOrEmpty(model.filename))
                {
                    ListViewItem newitem = new ListViewItem(model.filename);
                    newitem.SubItems.Add(GetModelFullDescription(model.a, model.b));
                    lvwCopy.Items.Add(newitem);
                    lvwMove.Items.Add((ListViewItem)newitem.Clone());
                    lvwWaveModel.Items.Add((ListViewItem)newitem.Clone());
                }
            }
            //导入
            txtModelEditImport.Text = "";
            //Wave
            WaveSelectObjects.Clear();
            label_waveSelect.Text = "已选定对象数量（" + WaveSelectObjects.Count.ToString() + "）";
            lvwWaveInfor.Items.Clear();
            string filename = XML3DPPM.GetModelName(OnEditModel_a, OnEditModel_b, XmlFile);
            if (!string.IsNullOrEmpty(filename))
            {
                List<S_WaveInfor> WaveInforList = NXFun.GetSinglePrtWaveInfor(ToFullPath(filename));
                foreach (S_WaveInfor waveInfor in WaveInforList)
                {
                    ListViewItem li = new ListViewItem(waveInfor.childname);
                    li.Tag = waveInfor.child;
                    li.SubItems.Add(waveInfor.isbreak ? "断开" : "正常");
                    li.SubItems.Add(ToFileName(waveInfor.fatherpath));
                    li.SubItems.Add(GetModelFullDescription(ToFileName(waveInfor.fatherpath)));
                    li.SubItems.Add(waveInfor.fathername);
                    lvwWaveInfor.Items.Add(li);
                }
            }
        }
        //得到新部件名称，流水号自动+1，自动跳过现有重名文件
        private string newPrtName
        {
            get
            {
                string prtName = "";
                do
                {
                    prtName = ModelPreName + (NameNum++).ToString() + ".prt";
                } while (NXFun.isFileExist(ToFullPath(prtName)));
                return prtName;
            }
        }
        //复制模型Ok按钮回调
        private void btnModelEditCopyOk_Click(object sender, EventArgs e)
        {
            if (lvwCopy.SelectedItems.Count == 0)
            {
                SetStatusLabel("请选择要复制的模型", 1);
                return;
            }
            string copyfilename = lvwCopy.SelectedItems[0].SubItems[0].Text;
            string description = txtModelDescription.Text;
            string oldfilename = XML3DPPM.GetModelName(OnEditModel_a, OnEditModel_b, XmlFile);
            if (oldfilename == copyfilename)
            {
                SetStatusLabel("不能复制本节点的模型到本节点！", 1);
                return;
            }
            if (!string.IsNullOrEmpty(oldfilename))
            {
                DialogResult dialogResult = MessageBox.Show("当前节点下存在模型，是否覆盖？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }
                //删除模型文件
                NXFun.DeletePrt(ToFullPath(oldfilename), true);
            }
            string name = newPrtName;
            bool result = XML3DPPM.SetModelAttr(OnEditModel_a, OnEditModel_b, name, description, XmlFile, TemplateXML);
            if (result)
            {
                NXFun.CopyPrt(ToFullPath(copyfilename), ToFullPath(name));
                IsEdit = false;
                FreshModelTree();
                FreshModelEditTable();
                SetStatusLabel("复制模型成功！", 2);
            }
            else
            {
                NameNum--;
                SetStatusLabel("复制模型失败！", 1);
                return;
            }
        }
        //移动模型Ok按钮回调
        private void btnModelEditMoveOk_Click(object sender, EventArgs e)
        {
            if (lvwMove.SelectedItems.Count == 0)
            {
                SetStatusLabel("请选择要移动的模型", 1);
                return;
            }
            string movefilename = lvwMove.SelectedItems[0].SubItems[0].Text;
            string description = txtModelDescription.Text;
            string oldfilename = XML3DPPM.GetModelName(OnEditModel_a, OnEditModel_b, XmlFile);
            if (oldfilename == movefilename)
            {
                SetStatusLabel("不能移动本节点的模型到本节点！", 1);
                return;
            }
            if (!string.IsNullOrEmpty(oldfilename))
            {
                DialogResult dialogResult = MessageBox.Show("当前节点下存在模型，是否覆盖？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }
                //删除模型文件
                NXFun.DeletePrt(ToFullPath(oldfilename), true);
            }
            XML3DPPM.DelModelNode(movefilename, XmlFile);
            bool result = XML3DPPM.SetModelAttr(OnEditModel_a, OnEditModel_b, movefilename, description, XmlFile, TemplateXML);
            if (result)
            {
                IsEdit = false;
                FreshModelTree();
                FreshModelEditTable();
                SetStatusLabel("移动模型成功！", 2);
            }
            else
            {
                SetStatusLabel("移动模型失败！", 1);
                return;
            }
        }
        //导入模型打开按钮回调
        private void btnModelEditImportOpen_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "请选择要导入的模型文件:";
            openFileDialog.Filter = "模型文件 (*.prt)|*.prt";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                txtModelEditImport.Text = filename;
            }
        }
        //导入模型Ok按钮回调
        private void btnModelEditImportOk_Click(object sender, EventArgs e)
        {
            string copyfilepath = txtModelEditImport.Text;
            if (!NXFun.isFileExist(copyfilepath))
            {
                SetStatusLabel("请输入正确的文件路径！", 1);
                return;
            }
            string description = txtModelDescription.Text;
            string oldfilename = XML3DPPM.GetModelName(OnEditModel_a, OnEditModel_b, XmlFile);
            if (!string.IsNullOrEmpty(oldfilename))
            {
                DialogResult dialogResult = MessageBox.Show("当前节点下存在模型，是否覆盖？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }
                //删除模型文件
                NXFun.DeletePrt(ToFullPath(oldfilename), true);
            }
            string name = newPrtName;
            bool result = XML3DPPM.SetModelAttr(OnEditModel_a, OnEditModel_b, name, description, XmlFile, TemplateXML);
            if (result)
            {
                NXFun.CopyPrt(copyfilepath, ToFullPath(name));
                IsEdit = false;
                FreshModelTree();
                FreshModelEditTable();
                SetStatusLabel("导入模型成功！", 2);
            }
            else
            {
                NameNum--;
                SetStatusLabel("导入模型失败！", 1);
                return;
            }
        }
        //wave模型确定按钮回调
        private void btnModelEditWaveOk_Click(object sender, EventArgs e)
        {
            int result = ModelWaveApply();
            if (result >= 0)
            {
                IsEdit = false;
                FreshModelTree();
                FreshModelEditTable();
            }
        }
        //WAVE应用的回调
        /// <summary>
        /// WAVE应用的回调  成功返回 1   没有操作返回 0   失败返回 -1  XML读取错误 -2;
        /// </summary>
        /// <returns></returns>
        private int ModelWaveApply()
        {
            try
            {
                if (WaveSelectObjects.Count == 0)
                {
                    SetStatusLabel("没有选中的关联几何,模型未发生更改!", 0);
                    return 0;
                }
                string description = txtModelDescription.Text;
                string filename = XML3DPPM.GetModelName(OnEditModel_a, OnEditModel_b, XmlFile);
                if (string.IsNullOrEmpty(filename))
                {
                    //创建一个新模型
                    string name = newPrtName;
                    bool result = XML3DPPM.SetModelAttr(OnEditModel_a, OnEditModel_b, name, description, XmlFile, TemplateXML);
                    if (result)
                    {
                        NXFun.CreateNewPart(ToFullPath(name));
                    }
                    else
                    {
                        NameNum--;
                        SetStatusLabel("创建模型失败!", 1);
                        return -1;
                    }
                    filename = name;
                }
                Part childPart = NXFun.OpenPrtQuiet(ToFullPath(filename));
                foreach (Tag obj in WaveSelectObjects)
                {
                    NXFun.CreateWave(obj, childPart.Tag, false);
                }
                NXFun.SavePrt(ToFullPath(filename));
                List<S_Model> modellist = XML3DPPM.GetModelList(XmlFile);
                S_Model model = modellist.Find(delegate(S_Model m) { return m.filename == filename; });
                if (model.Equals(default(S_Model)))
                {
                    SetStatusLabel("XML读取错误,创建可能未完全成功,请取消重试!", 1);
                    return -2;
                }
                OnEditModel_a = model.a;
                OnEditModel_b = model.b;
                FreshModelTree();
                FreshModelEditTable();
                SetStatusLabel("创建WAVE模型成功!", 2);
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
        }
        //ListView（复制、移动、Wave）双击打开模型
        private void lvw_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ListViewItem item = ((ListView)sender).GetItemAt(e.X, e.Y);
                if (item != null)
                {
                    string filename = item.SubItems[0].Text;
                    NXFun.OpenPrt(ToFullPath(filename));
                }
            }
        }
        //Wave信息ListView右键菜单
        private void lvwWaveInfor_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && lvwWaveInfor.SelectedItems.Count > 0)
            {
                if (lvwWaveInfor.SelectedItems[0].SubItems[1].Text == "正常")
                {
                    cmsWaveEdit.Show((Control)sender, e.X, e.Y);
                }
            }
        }
        //打断wave菜单
        private void cmsBreakWave_Click(object sender, EventArgs e)
        {
            if (lvwWaveInfor.SelectedItems.Count == 0)
            {
                return;
            }
            DialogResult dialogResult = MessageBox.Show("断开链接不可撤销，只能手动再次指定链接，是否继续", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.Yes)
            {
                return;
            }
            foreach (ListViewItem item in lvwWaveInfor.SelectedItems)
            {
                NXFun.OpenPrt(((NXObject)item.Tag).OwningPart.FullPath);
                NXFun.AcceptLinkBroken(((NXObject)item.Tag).Tag);
            }
            FreshModelEditTable();
        }
        //tsrWaveView工具栏按钮回调集成（几何类型、显示级别、显示名称、刷新类型）
        private void tsrbtnWaveView_Click(object sender, EventArgs e)
        {
            if (sender.Equals(tsrbtnFreshModeAuto))
            {
                isAutoFreshWave = true;
            }
            else if (sender.Equals(tsrbtnFreshModeManual))
            {
                isAutoFreshWave = false;
            }
            else if (sender.Equals(tsrbtnShowLevelAll))
            {
                isWaveShowAll = true;
            }
            else if (sender.Equals(tsrbtnShowLevelSingle))
            {
                isWaveShowAll = false;
            }
            else if (sender.Equals(tsrbtnModelName))
            {
                WaveShowNameType = 0;
            }
            else if (sender.Equals(tsrbtnModelDescription))
            {
                WaveShowNameType = 1;
            }
            else if (sender.Equals(tsrbtnGeoTypeAllPoint))
            {
                if (WaveSetting_All.Contains(E_WaveType.LINKED_POINT))
                {
                    WaveSetting_All.Remove(E_WaveType.LINKED_POINT);
                }
                else
                {
                    WaveSetting_All.Add(E_WaveType.LINKED_POINT);
                }
            }
            else if (sender.Equals(tsrbtnGeoTypeAllLine))
            {
                if (WaveSetting_All.Contains(E_WaveType.LINKED_CURVE))
                {
                    WaveSetting_All.Remove(E_WaveType.LINKED_CURVE);
                }
                else
                {
                    WaveSetting_All.Add(E_WaveType.LINKED_CURVE);
                }
            }
            else if (sender.Equals(tsrbtnGeoTypeAllFace))
            {
                if (WaveSetting_All.Contains(E_WaveType.LINKED_FACE))
                {
                    WaveSetting_All.Remove(E_WaveType.LINKED_FACE);
                }
                else
                {
                    WaveSetting_All.Add(E_WaveType.LINKED_FACE);
                }
            }
            else if (sender.Equals(tsrbtnGeoTypeAllBody))
            {
                if (WaveSetting_All.Contains(E_WaveType.LINKED_BODY))
                {
                    WaveSetting_All.Remove(E_WaveType.LINKED_BODY);
                }
                else
                {
                    WaveSetting_All.Add(E_WaveType.LINKED_BODY);
                }
            }
            else if (sender.Equals(tsrbtnGeoTypeAllSketch))
            {
                if (WaveSetting_All.Contains(E_WaveType.LINKED_SKETCH))
                {
                    WaveSetting_All.Remove(E_WaveType.LINKED_SKETCH);
                }
                else
                {
                    WaveSetting_All.Add(E_WaveType.LINKED_SKETCH);
                }
            }
            else if (sender.Equals(tsrbtnGeoTypeAllDatum))
            {
                if (WaveSetting_All.Contains(E_WaveType.LINKED_DATUM))
                {
                    WaveSetting_All.Remove(E_WaveType.LINKED_DATUM);
                }
                else
                {
                    WaveSetting_All.Add(E_WaveType.LINKED_DATUM);
                }
            }
            else if (sender.Equals(tsrbtnGeoSinglePoint))
            {
                if (WaveSetting_Single.Contains(E_WaveType.LINKED_POINT))
                {
                    WaveSetting_Single.Remove(E_WaveType.LINKED_POINT);
                }
                else
                {
                    WaveSetting_Single.Add(E_WaveType.LINKED_POINT);
                }
            }
            else if (sender.Equals(tsrbtnGeoSingleLine))
            {
                if (WaveSetting_Single.Contains(E_WaveType.LINKED_CURVE))
                {
                    WaveSetting_Single.Remove(E_WaveType.LINKED_CURVE);
                }
                else
                {
                    WaveSetting_Single.Add(E_WaveType.LINKED_CURVE);
                }
            }
            else if (sender.Equals(tsrbtnGeoSingleFace))
            {
                if (WaveSetting_Single.Contains(E_WaveType.LINKED_FACE))
                {
                    WaveSetting_Single.Remove(E_WaveType.LINKED_FACE);
                }
                else
                {
                    WaveSetting_Single.Add(E_WaveType.LINKED_FACE);
                }
            }
            else if (sender.Equals(tsrbtnGeoSingleBody))
            {
                if (WaveSetting_Single.Contains(E_WaveType.LINKED_BODY))
                {
                    WaveSetting_Single.Remove(E_WaveType.LINKED_BODY);
                }
                else
                {
                    WaveSetting_Single.Add(E_WaveType.LINKED_BODY);
                }
            }
            else if (sender.Equals(tsrbtnGeoSingleSketch))
            {
                if (WaveSetting_Single.Contains(E_WaveType.LINKED_SKETCH))
                {
                    WaveSetting_Single.Remove(E_WaveType.LINKED_SKETCH);
                }
                else
                {
                    WaveSetting_Single.Add(E_WaveType.LINKED_SKETCH);
                }
            }
            else if (sender.Equals(tsrbtnGeoSingleDatum))
            {
                if (WaveSetting_Single.Contains(E_WaveType.LINKED_DATUM))
                {
                    WaveSetting_Single.Remove(E_WaveType.LINKED_DATUM);
                }
                else
                {
                    WaveSetting_Single.Add(E_WaveType.LINKED_DATUM);
                }
            }
            FreshWaveMap(true, true, "");
            FreshWaveViewToolStripStatus();
        }
        //刷新WaveView工具条的状态;
        private void FreshWaveViewToolStripStatus()
        {
            //刷新选中状态
            tsrbtnFreshModeAuto.CheckState = isAutoFreshWave ? CheckState.Checked : CheckState.Unchecked;
            tsrbtnFreshModeManual.CheckState = isAutoFreshWave ? CheckState.Unchecked : CheckState.Checked;

            tsrbtnShowLevelAll.CheckState = isWaveShowAll ? CheckState.Checked : CheckState.Unchecked;
            tsrbtnShowLevelSingle.CheckState = isWaveShowAll ? CheckState.Unchecked : CheckState.Checked;

            tsrbtnModelName.CheckState = WaveShowNameType == 0 ? CheckState.Checked : CheckState.Unchecked;
            tsrbtnModelDescription.CheckState = WaveShowNameType == 1 ? CheckState.Checked : CheckState.Unchecked;

            tsrbtnGeoTypeAllPoint.CheckState = WaveSetting_All.Contains(E_WaveType.LINKED_POINT) ? CheckState.Checked : CheckState.Unchecked;
            tsrbtnGeoTypeAllLine.CheckState = WaveSetting_All.Contains(E_WaveType.LINKED_CURVE) ? CheckState.Checked : CheckState.Unchecked;
            tsrbtnGeoTypeAllFace.CheckState = WaveSetting_All.Contains(E_WaveType.LINKED_FACE) ? CheckState.Checked : CheckState.Unchecked;
            tsrbtnGeoTypeAllBody.CheckState = WaveSetting_All.Contains(E_WaveType.LINKED_BODY) ? CheckState.Checked : CheckState.Unchecked;
            tsrbtnGeoTypeAllSketch.CheckState = WaveSetting_All.Contains(E_WaveType.LINKED_SKETCH) ? CheckState.Checked : CheckState.Unchecked;
            tsrbtnGeoTypeAllDatum.CheckState = WaveSetting_All.Contains(E_WaveType.LINKED_DATUM) ? CheckState.Checked : CheckState.Unchecked;

            tsrbtnGeoSinglePoint.CheckState = WaveSetting_Single.Contains(E_WaveType.LINKED_POINT) ? CheckState.Checked : CheckState.Unchecked;
            tsrbtnGeoSingleLine.CheckState = WaveSetting_Single.Contains(E_WaveType.LINKED_CURVE) ? CheckState.Checked : CheckState.Unchecked;
            tsrbtnGeoSingleFace.CheckState = WaveSetting_Single.Contains(E_WaveType.LINKED_FACE) ? CheckState.Checked : CheckState.Unchecked;
            tsrbtnGeoSingleBody.CheckState = WaveSetting_Single.Contains(E_WaveType.LINKED_BODY) ? CheckState.Checked : CheckState.Unchecked;
            tsrbtnGeoSingleSketch.CheckState = WaveSetting_Single.Contains(E_WaveType.LINKED_SKETCH) ? CheckState.Checked : CheckState.Unchecked;
            tsrbtnGeoSingleDatum.CheckState = WaveSetting_Single.Contains(E_WaveType.LINKED_DATUM) ? CheckState.Checked : CheckState.Unchecked;
        }
        //wave模型应用按钮回调
        private void btnModelEditWaveApply_Click(object sender, EventArgs e)
        {
            ModelWaveApply();
        }
        //wave模型清空按钮回调
        private void btnModelEditWaveClear_Click(object sender, EventArgs e)
        {
            WaveSelectObjects.Clear();
        }
        //wave模型选择按钮回调
        private void btnModelEditWaveSelect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NXFun.GetOnShowPrtName()))
            {
                SetStatusLabel("请先双击列表打开父模型!", 1);
                return;
            }
            mainDlg.WindowState = FormWindowState.Minimized;
            WaveSelectObjects.AddRange(NXFun.Array2List<Tag>(NXFun.SelectWaveObjects()));
            WaveSelectObjects = NXFun.RemoveTheSame<Tag>(WaveSelectObjects);
            label_waveSelect.Text = "已选定对象数量（" + WaveSelectObjects.Count.ToString() + "）";
            mainDlg.WindowState = FormWindowState.Normal;
        }
        //模型树右键菜单
        private void tvwModel_MouseClick(object sender, MouseEventArgs e)
        {
            //判断是否是右键
            if (e.Button == MouseButtons.Right)
            {
                System.Drawing.Point clickPoint = new System.Drawing.Point(e.X, e.Y);
                TreeNode sel = tvwModel.GetNodeAt(clickPoint);
                //判断点击的是否是一个节点
                if (sel != null)
                {
                    tvwModel.SelectedNode = sel;
                    //点击的是工艺节点
                    if (sel.Tag.ToString() == "Gongyi")
                    {
                        sel.ContextMenuStrip = cmsGongyi;
                        if (XML3DPPM.IsModelExist(0, 0, XmlFile) == true)
                        {
                            cmsAddGongyiModel.Visible = false;
                            cmsEditGongyiModel.Visible = true;
                            cmsDelGongyiModel.Visible = true;
                        }
                        else
                        {
                            cmsAddGongyiModel.Visible = true;
                            cmsEditGongyiModel.Visible = false;
                            cmsDelGongyiModel.Visible = false;
                        }
                    }
                    //点击的是工序节点
                    else if (sel.Tag.ToString() == "Gongxu")
                    {
                        sel.ContextMenuStrip = cmsGongxu;
                        if (XML3DPPM.IsModelExist(0, sel.Index + 1, XmlFile) == true)
                        {
                            cmsAddGongxuModel.Visible = false;
                            cmsEditGongxuModel.Visible = true;
                            cmsDelGongxuModel.Visible = true;
                        }
                        else
                        {
                            cmsAddGongxuModel.Visible = true;
                            cmsEditGongxuModel.Visible = false;
                            cmsDelGongxuModel.Visible = false;
                        }

                    }
                    //点击辅助模型父节点
                    else if (sel.Tag.ToString() == "fuModels")
                    {
                        sel.ContextMenuStrip = cmsFuModels;
                    }
                    //点击辅助模型节点
                    else if (sel.Tag.ToString() == "fuModel")
                    {
                        sel.ContextMenuStrip = cmsFuModel;
                    }
                    //点击余量图父节点
                    else if (sel.Tag.ToString() == "ylts")
                    {
                        sel.ContextMenuStrip = cmsYLTs;
                    }
                    //点击余量图节点
                    else if (sel.Tag.ToString() == "ylt")
                    {
                        sel.ContextMenuStrip = cmsYLT;
                    }
                }
            }
        }
        //模型树选中节点改变
        private void tvwModel_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode sel = e.Node;
            //判断点击的是否是一个节点
            int a = 0; int b = 0;
            {
                tvwModel.SelectedNode = sel;
                if (sel == null)
                {
                    tsrbtnModelAdd.Enabled = false;
                    tsrbtnModelDel.Enabled = false;
                    tsrbtnModelEdit.Enabled = false;
                    tsrbtnModelOpen.Enabled = false;
                }
                else if (sel.Tag.ToString() == "Gongyi")
                {
                    a = 0;
                    b = 0;
                }
                else if (sel.Tag.ToString() == "Gongxu")
                {
                    a = 0;
                    b = sel.Index + 1;
                }
                else if (sel.Tag.ToString() == "fuModels")
                {
                    a = 1;
                    b = 0;
                }
                else if (sel.Tag.ToString() == "fuModel")
                {
                    a = 1;
                    b = sel.Index + 1;
                }
                else if (sel.Tag.ToString() == "ylts")
                {
                    a = 2;
                    b = 0;
                }
                else if (sel.Tag.ToString() == "ylt")
                {
                    a = 2;
                    b = sel.Index + 1;
                }
                string filename = XML3DPPM.GetModelName(a, b, XmlFile);
                if (string.IsNullOrEmpty(filename))
                {
                    tsrbtnModelAdd.Enabled = true;
                    tsrbtnModelDel.Enabled = false;
                    tsrbtnModelEdit.Enabled = false;
                    tsrbtnModelOpen.Enabled = false;
                }
                else
                {
                    tsrbtnModelAdd.Enabled = false;
                    tsrbtnModelDel.Enabled = true;
                    tsrbtnModelEdit.Enabled = true;
                    tsrbtnModelOpen.Enabled = true;
                    if (!isWaveShowAll)
                    {
                        //显示单个模型的wave关系
                        FreshWaveMap(isAutoFreshWave, false, filename);
                    }
                }
            }
        }
        //新建模型确定按钮回调
        private void btnModelNewOk_Click(object sender, EventArgs e)
        {
            string description = txtModelDescription.Text;
            string filename = XML3DPPM.GetModelName(OnEditModel_a, OnEditModel_b, XmlFile);
            if (!string.IsNullOrEmpty(filename))
            {
                DialogResult dialogResult = MessageBox.Show("当前节点下存在模型，是否覆盖？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }
                //删除模型文件
                NXFun.DeletePrt(ToFullPath(filename), true);
            }
            string name = newPrtName;
            bool result = XML3DPPM.SetModelAttr(OnEditModel_a, OnEditModel_b, name, description, XmlFile, TemplateXML);
            if (result)
            {
                NXFun.CreateNewPart(ToFullPath(name));
                IsEdit = false;
                FreshModelTree();
                FreshModelEditTable();
                SetStatusLabel("新建模型成功！", 2);
            }
            else
            {
                NameNum--;
                return;
            }
        }
        //描述模型确定按钮回调
        private void btnModelEditDesOk_Click(object sender, EventArgs e)
        {
            //修改模型描述
            if (XML3DPPM.IsModelExist(OnEditModel_a, OnEditModel_b, XmlFile))
            {
                XML3DPPM.SetModelDescription(OnEditModel_a, OnEditModel_b, txtModelDescription.Text, XmlFile);
                IsEdit = false;
                FreshModelTree();
                FreshModelEditTable();
                SetStatusLabel("模型描述更新成功！", 2);
            }
            else
            {
                SetStatusLabel("模型描述更新失败！", 1);
            }
        }
        //编辑模型所有取消按钮集成
        private void btnModelEditCancel_Click(object sender, EventArgs e)
        {
            IsEdit = false;
            FreshModelEditTable();
            SetStatusLabel("已取消操作！", 2);
        }
        //删除模型菜单集成（工艺、工序、辅助、余量图）
        private void cmsDelModel_Click(object sender, EventArgs e)
        {
            if (sender.Equals(cmsDelGongyiModel))
            {
                //删除设计模型
                OnEditModel_a = 0;
                OnEditModel_b = 0;
            }
            else if (sender.Equals(cmsDelGongxuModel))
            {
                //删除工序模型
                OnEditModel_a = 0;
                OnEditModel_b = tvwModel.SelectedNode.Index + 1;
            }
            else if (sender.Equals(cmsDelFuModel))
            {
                //删除工序模型
                OnEditModel_a = 1;
                OnEditModel_b = tvwModel.SelectedNode.Index + 1;
            }
            else if (sender.Equals(cmsDelYLT))
            {
                //删除工序模型
                OnEditModel_a = 2;
                OnEditModel_b = tvwModel.SelectedNode.Index + 1;
            }
            string name, des;
            XML3DPPM.GetModelAttr(OnEditModel_a, OnEditModel_b, out name, out des, XmlFile);
            if (string.IsNullOrEmpty(name))
            {
                SetStatusLabel("该节点下没有模型！", 1);
                return;
            }
            DialogResult dialogResult = MessageBox.Show("是否删除模型" + name, "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.Yes)
            {
                return;
            }
            XML3DPPM.DelModelNode(OnEditModel_a, OnEditModel_b, XmlFile);
            NXFun.DeletePrt(ToFullPath(name), true);
            FreshModelTree();
            FreshWaveMap(isAutoFreshWave, true, "");
            SetStatusLabel("模型已成功删除！", 2);
        }
        //双击模型树节点   打开模型
        private void tvwModel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //判断是否是左键
            if (e.Button == MouseButtons.Left)
            {
                System.Drawing.Point clickPoint = new System.Drawing.Point(e.X, e.Y);
                TreeNode sel = tvwModel.GetNodeAt(clickPoint);
                //判断点击的是否是一个节点
                if (sel != null)
                {
                    int a = -1;
                    int b = 0;
                    if (sel.Tag.ToString() == "Gongyi")
                    {
                        a = 0;
                        b = 0;
                    }
                    else if (sel.Tag.ToString() == "Gongxu")
                    {
                        a = 0;
                        b = sel.Index + 1;
                    }
                    else if (sel.Tag.ToString() == "fuModel")
                    {
                        a = 1;
                        b = sel.Index + 1;
                    }
                    else if (sel.Tag.ToString() == "ylt")
                    {
                        a = 2;
                        b = sel.Index + 1;
                    }
                    if (a != -1)
                    {
                        string filename = XML3DPPM.GetModelName(a, b, XmlFile);
                        if (!string.IsNullOrEmpty(filename))
                        {
                            NXFun.OpenPrt(ToFullPath(filename));
                        }
                    }
                }
            }
        }
        //添加模型按钮
        private void tsrbtnModelAdd_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwModel.SelectedNode;
            if (sel == null)
            {
                SetStatusLabel("请选中树节点", 1);
                return;
            }
            if (sel.Tag.ToString() == "Gongyi")
            {
                //添加设计模型
                OnEditModel_a = 0;
                OnEditModel_b = 0;
                SetStatusLabel("正在编辑设计模型……", 0);
            }
            else if (sel.Tag.ToString() == "Gongxu")
            {
                //添加工序模型
                OnEditModel_a = 0;
                OnEditModel_b = sel.Index + 1;
                SetStatusLabel("正在编辑工序模型……", 0);
            }
            else if (sel.Tag.ToString() == "fuModels")
            {
                //添加辅助模型
                OnEditModel_a = 1;
                OnEditModel_b = 0;
                SetStatusLabel("正在添加辅助模型……", 0);
            }
            else if (sel.Tag.ToString() == "fuModel")
            {
                //编辑辅助模型
                OnEditModel_a = 1;
                OnEditModel_b = sel.Index + 1;
                SetStatusLabel("正在编辑辅助模型……", 0);

            }
            else if (sel.Tag.ToString() == "ylts")
            {
                //添加余量图
                OnEditModel_a = 2;
                OnEditModel_b = 0;
                SetStatusLabel("正在添加余量图…", 0);
            }
            else if (sel.Tag.ToString() == "ylt")
            {
                //编辑余量图
                OnEditModel_a = 2;
                OnEditModel_b = sel.Index + 1;
                SetStatusLabel("正在编辑余量图……", 0);
            }
            IsEdit = true;
            FreshModelEditTable();
        }
        //删除模型按钮
        private void tsrbtnModelDel_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwModel.SelectedNode;
            if (sel == null)
            {
                SetStatusLabel("请选中树节点", 1);
                return;
            }
            if (sel.Tag.ToString() == "Gongyi")
            {
                //删除设计模型
                OnEditModel_a = 0;
                OnEditModel_b = 0;
            }
            else if (sel.Tag.ToString() == "Gongxu")
            {
                //删除工序模型
                OnEditModel_a = 0;
                OnEditModel_b = sel.Index + 1;
            }
            else if (sel.Tag.ToString() == "fuModels")
            {
                //删除辅助模型
                OnEditModel_a = 1;
                OnEditModel_b = 0;
                SetStatusLabel("请选中辅助模型子节点", 1);
                return;
            }
            else if (sel.Tag.ToString() == "fuModel")
            {
                //删除辅助模型
                OnEditModel_a = 1;
                OnEditModel_b = sel.Index + 1;
            }
            else if (sel.Tag.ToString() == "ylts")
            {
                //删除余量图
                OnEditModel_a = 2;
                OnEditModel_b = 0;
                SetStatusLabel("请选中余量图子节点", 1);
                return;
            }
            else if (sel.Tag.ToString() == "ylt")
            {
                //删除余量图
                OnEditModel_a = 2;
                OnEditModel_b = sel.Index + 1;
            }
            string name, des;
            XML3DPPM.GetModelAttr(OnEditModel_a, OnEditModel_b, out name, out des, XmlFile);
            if (string.IsNullOrEmpty(name))
            {
                SetStatusLabel("该节点下没有模型！", 1);
                return;
            }
            DialogResult dialogResult = MessageBox.Show("是否删除模型" + name, "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.Yes)
            {
                return;
            }
            XML3DPPM.DelModelNode(OnEditModel_a, OnEditModel_b, XmlFile);
            NXFun.DeletePrt(ToFullPath(name), true);
            FreshModelTree();
            FreshWaveMap(isAutoFreshWave, true, "");
            SetStatusLabel("模型已成功删除！", 2);
        }
        //编辑模型按钮
        private void tsrbtnModelEdit_Click(object sender, EventArgs e)
        {
            tsrbtnModelAdd_Click(sender, e);
        }
        //打开模型按钮
        private void tsrbtnModelOpen_Click(object sender, EventArgs e)
        {
            TreeNode sel = tvwModel.SelectedNode;
            if (sel == null)
            {
                SetStatusLabel("请选中树节点", 1);
                return;
            }
            if (sel.Tag.ToString() == "Gongyi")
            {
                //打开设计模型
                OnEditModel_a = 0;
                OnEditModel_b = 0;
            }
            else if (sel.Tag.ToString() == "Gongxu")
            {
                //打开工序模型
                OnEditModel_a = 0;
                OnEditModel_b = sel.Index + 1;
            }
            else if (sel.Tag.ToString() == "fuModels")
            {
                //打开辅助模型
                OnEditModel_a = 1;
                OnEditModel_b = 0;
                SetStatusLabel("请选中辅助模型子节点", 1);
                return;
            }
            else if (sel.Tag.ToString() == "fuModel")
            {
                //打开辅助模型
                OnEditModel_a = 1;
                OnEditModel_b = sel.Index + 1;
            }
            else if (sel.Tag.ToString() == "ylts")
            {
                //打开余量图
                OnEditModel_a = 2;
                OnEditModel_b = 0;
                SetStatusLabel("请选中余量图子节点", 1);
                return;
            }
            else if (sel.Tag.ToString() == "ylt")
            {
                //打开余量图
                OnEditModel_a = 2;
                OnEditModel_b = sel.Index + 1;
            }
            string name, des;
            XML3DPPM.GetModelAttr(OnEditModel_a, OnEditModel_b, out name, out des, XmlFile);
            if (string.IsNullOrEmpty(name))
            {
                SetStatusLabel("该节点下没有模型！", 1);
                return;
            }
            string filename = XML3DPPM.GetModelName(OnEditModel_a, OnEditModel_b, XmlFile);
            if (!string.IsNullOrEmpty(filename))
            {
                NXFun.OpenPrt(ToFullPath(filename));
            }
            SetStatusLabel("模型已成功显示！", 2);
        }
        //刷新Wave关系图
        private void tsrbtnWaveFresh_Click(object sender, EventArgs e)
        {
            FreshWaveMap(true, true, "");
        }


    }
}
