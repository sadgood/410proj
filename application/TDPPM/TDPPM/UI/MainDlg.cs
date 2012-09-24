using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using WaveRelationControl;
using NXOpen;


namespace TDPPM
{
    
    public partial class MainDlg : Form
    {
        private string _ProPath = "";   //工艺保存路径;
        private string ProPath          //工艺保存路径;
        {
            get
            {
                return _ProPath;
            }
            set
            {
                if (value.EndsWith("\\"))
                {
                    _ProPath = value.Substring(0, value.Length - 1);
                }
                else
                {
                    _ProPath = value;
                }                
            }
        }
        private string ProName = ""; //工艺保存文件名;
        private bool IsProjectOpen = false; //当前是否有打开的工艺
        private string XmlFile = "";    //工艺xml文件;
        private string TemplateXML = NXFun.TDPPMPath + NXFun.TemplateXML;
        //构造函数
        public MainDlg()
        {
            InitializeComponent();
            NXOpenUI.FormUtilities.ReparentForm(this);
            NXOpenUI.FormUtilities.SetApplicationIcon(this);          
            ProcessEdit.mainDlg = this;
            ModelEdit.mainDlg = this;
            SheetEdit.mainDlg = this;
            //初始化控件状态
            pnlMain.Hide();
            pnlWelcome.Show();
        }
        //主窗口正在关闭时
        private void MainDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsProjectOpen)
            {
                e.Cancel = false;
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("是否保存当前工艺？", "提示信息", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CloseProject(true))
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        MessageBox.Show("保存工艺失败！");
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    CloseProject(false);
                    e.Cancel = false;
                }
                else
                    e.Cancel = true;
            }
        }
        //新建工艺
        private void CreateNewProject()
        {
            int flag = 0;
            //选择保存路径;
            saveFileDialog.AddExtension = true;
            saveFileDialog.Title = "请选择要保存的工艺所在路径";
            saveFileDialog.DefaultExt = "3dppm";
            saveFileDialog.Filter = "三维机加工艺文件(*.3dppm)|*.3dppm";
            saveFileDialog.OverwritePrompt = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog.FileName;
                if (NXFun.IsChina(filename))
                {
                    MessageBox.Show("NX不支持中文路径，请重新选择保存路径");
                    flag = 1;
                }
                else
                {
                    //得到工艺路径和文件名;
                    ProPath = Path.GetDirectoryName(filename);
                    ProName = Path.GetFileNameWithoutExtension(filename);
                    XmlFile = ToFullPath(NXFun.ProcessXML);
                    //删除并创建文件夹;
                    bool result = NXFun.DeleteDirectory(ToFullPath(""));
                    if (!result)
                    {
                        MessageBox.Show("文件夹" + ToFullPath("") + "无法删除，请手动关闭可能已打开的文件并删除该目录。");
                        return;
                    }
                    Directory.CreateDirectory(ToFullPath(""));
                    //复制工艺模板;
                    File.Copy(NXFun.TDPPMPath + NXFun.ProcessXML, XmlFile, true);
                    OpenOrCreateProjectInit();
                    SetStatusLabel("工艺新建成功！", 2);
                }
            }
            if (flag == 1)
            {
                //包含中文，重新选择文件;
                CreateNewProject();

            }
        }
        //打开或新建工艺时初始化
        private void OpenOrCreateProjectInit()
        {
            //变量值
            IsProjectOpen = true;
            
            //控件状态
            pnlWelcome.Hide();
            pnlMain.Show();

            this.Text = ProPath + "\\" + ProName + ".3dppm";
            //更新会签
            if (NXFun.isFileExist(ToFullPath(NXFun.SignOffXML)))
            {
                bool result = XML3DPPM.UpdateSignOff(XmlFile, ToFullPath(NXFun.SignOffXML));
                if (result)
                {
                    NXFun.MessageBox("更新会签信息成功，请在\"编辑图纸->二维图表\"模块点击\"更新表头\"来刷新会签信息。");
                } 
                else
                {
                    NXFun.MessageBox("更新会签信息失败，请确认会签文件是否有效！");
                }
            }
            //操作
            ProcessEdit.ProPath = ProPath;
            ProcessEdit.ProName = ProName;
            ProcessEdit.Init();

            ModelEdit.ProPath = ProPath;
            ModelEdit.ProName = ProName;
            ModelEdit.Init();

            SheetEdit.ProPath = ProPath;
            SheetEdit.ProName = ProName;
            SheetEdit.Init();
            
        }
        //得到全路径
        private string ToFullPath(string filename)
        {
            return ProPath + "\\" + ProName + "\\" + filename;
        }
        //打开工艺
        private void OpenProject()
        {
            openFileDialog.Title = "请选择要打开的工艺文件:";
            openFileDialog.Filter = "3DPPM 文件 (*.3dppm)|*.3dppm";
            openFileDialog.FileName = "";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                if (NXFun.IsChina(filename))
                {
                    MessageBox.Show("NX不支持中文路径，请将工艺文件放到英文路径下再打开.");
                    return;
                }
                else
                {
                    //得到工艺路径和文件名;
                    ProPath = Path.GetDirectoryName(filename);
                    ProName = Path.GetFileNameWithoutExtension(filename);
                    XmlFile = ToFullPath(NXFun.ProcessXML);
                    //删除文件夹;
                    bool result = NXFun.DeleteDirectory(ProPath +"\\"+ ProName);
                    if (!result)
                    {
                        MessageBox.Show("文件夹" + ProPath +"\\"+ ProName + "无法删除，请手动关闭可能已打开的文件并删除该目录。");
                        return;
                    }
                    //判断是否是压缩文件
                    int type = GetFileRealType(filename);
                    switch (type)
                    {
                        case -1:
                            MessageBox.Show("读取文件出错，请确认文件是否有效！");
                            return;
                        case 0:
                            break;
                        case 1:
                            break;
                        default:
                            MessageBox.Show("读取文件出错，请确认文件是否有效！");
                            return;
                    }
                    //解压
                    bool isUnZip = true;
                    if (type == 0)
                    {
                        //按zip格式解压
                        isUnZip = Compress.UnZipFileDictory(filename, "");
                    }
                    else if (type == 1)
                    {
                        //按rar格式解压
                        isUnZip = Compress.UnRAR(NXFun.TDPPMPath + NXFun.RarEXE, filename,ToFullPath(""));     
                    }
                    if (!isUnZip)
                    {
                        MessageBox.Show(
                            "工艺文件打开失败！请确认以下几项：\r\n" +
                            "1：文件有是否有效；\r\n" +
                            "2：工艺临时文件夹或其中文件若处于打开状态请关闭。");
                        return;
                    }
                    //检测文件有效性
                    if (!NXFun.isFileExist(XmlFile))
                    {
                        MessageBox.Show("工艺数据文件丢失！请确认文件有效！");
                    }
                    //检测版本有效性，若旧版则自动升级
                    XML3DPPM.Update3dppm(XmlFile, TemplateXML);


                    OpenOrCreateProjectInit();
                    //打开所有模型
                    List<S_Model> ModelList = XML3DPPM.GetModelList(XmlFile);
                    foreach (S_Model model in ModelList)
                    {
                        if (!String.IsNullOrEmpty(model.filename))
                        {
                            NXFun.OpenPrtQuiet(ToFullPath(model.filename));
                        }
                          
                    }
                    SetStatusLabel("工艺打开成功！", 2);
                }
            }
        }
        //恢复工艺
        private void RecoverProject()
        {
            folderBrowserDialog.Description = "请选择要打开的工艺临时文件夹:";
            folderBrowserDialog.ShowNewFolderButton = false;
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = folderBrowserDialog.SelectedPath;
                //判断合法性
                string root = Path.GetPathRoot(path);
                string pathname = Path.GetDirectoryName(path);
                string filename = Path.GetFileName(path);
                if (path.Equals(root) && string.IsNullOrEmpty(pathname) && string.IsNullOrEmpty(filename))
                {
                    MessageBox.Show("请不要选择根目录！");
                    return;
                }
                else if (NXFun.IsChina(path))
                {
                    MessageBox.Show("NX不支持中文路径，请修改路径！");
                    return;
                } 
                else if(!NXFun.isFileExist(path + "\\" + NXFun.ProcessXML))
                {
                    MessageBox.Show("所选路径非工艺临时数据包");
                    return;
                }
                else
                {
                    ProPath = pathname;
                    ProName = filename;
                    XmlFile = ToFullPath(NXFun.ProcessXML);
                    XML3DPPM.Update3dppm(XmlFile, TemplateXML);
                    OpenOrCreateProjectInit();
                }
            }            
        }
        //保存工艺
        private bool SaveProject(bool isQuit)
        {
            try
            {
                bool result = false;
                List<S_Model> ModelList = XML3DPPM.GetModelList(XmlFile);
                if (isQuit)
                {
                    //关闭所有并保存压缩          
                    foreach (S_Model model in ModelList)
                    {
                        if (!String.IsNullOrEmpty(model.filename))
                        {
                            NXFun.ClosePrt(ToFullPath(model.filename), true);
                        }
                        
                    }
                    //压缩
                    string tempZip = ProPath + "\\" + ProName + ".tmp";
                    bool isSaved = Compress.RAR(NXFun.TDPPMPath + NXFun.RarEXE, tempZip, ProPath + "\\" + ProName);
                    if (isSaved)
                    {
                        File.Copy(tempZip, ProPath + "\\" + ProName + ".3dppm", true);
                        File.Delete(tempZip);
                        NXFun.DeleteDirectory(ProPath + "\\" + ProName);
                        SetStatusLabel("保存工艺成功！", 2);
                        result = true;
                    }
                    else
                    {
                        SetStatusLabel("工艺保存失败！", 1);
                        MessageBox.Show("工艺保存失败！");
                        result = false;
                    }
                }
                else
                {
                    //仅保存
                    foreach (S_Model model in ModelList)
                    {
                        if (!String.IsNullOrEmpty(model.filename))
                        {
                            NXFun.SavePrt(ToFullPath(model.filename));
                        }
                        
                    }
                    //压缩
                    string tempZip = ProPath + "\\" + ProName + ".tmp";
                    bool isSaved = Compress.RAR(NXFun.TDPPMPath + NXFun.RarEXE, tempZip, ProPath + "\\" + ProName);
                    if (isSaved)
                    {
                        File.Copy(tempZip, ProPath + "\\" + ProName + ".3dppm", true);
                        File.Delete(tempZip);
                        SetStatusLabel("保存工艺成功！", 2);
                        result = true;
                    }
                    else
                    {
                        SetStatusLabel("工艺保存失败！", 1);
                        MessageBox.Show("工艺保存失败！");
                        result = false;
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        //关闭工艺
        private bool CloseProject(bool isSave)
        {
            bool result = false;
            List<S_Model> ModelList = XML3DPPM.GetModelList(XmlFile);
            //关闭所有并保存压缩          
            foreach (S_Model model in ModelList)
            {
                if (!String.IsNullOrEmpty(model.filename))
                {
                    NXFun.ClosePrt(ToFullPath(model.filename), isSave);
                }                
            }
            if(isSave)
            {
                //压缩
                string tempZip = ProPath + "\\" + ProName + ".tmp";
                bool isSaved = Compress.RAR(NXFun.TDPPMPath + NXFun.RarEXE, tempZip, ProPath + "\\" + ProName);
                if (isSaved)
                {
                    File.Copy(tempZip, ProPath + "\\" + ProName + ".3dppm", true);
                    File.Delete(tempZip);
                    NXFun.DeleteDirectory(ProPath + "\\" + ProName);
                    SetStatusLabel("保存工艺成功！", 2);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = true;
            }
            return result;
        }
        //退出系统
        private void ExitProject()
        {
            if (!IsProjectOpen)
            {
                this.Close();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("是否保存当前工艺？", "提示信息", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CloseProject(true))
                    {
                        IsProjectOpen = false;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("保存工艺失败！");
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    CloseProject(false);
                    IsProjectOpen = false;
                    this.Close();
                }
            }
        }
        //设置状态栏显示文字
        public void SetStatusLabel(string text, int color)
        {
            tslMessage.Text = text;
            switch (color)
            {
                case 0:
                    tslMessage.ForeColor = Color.Black;
                    break;
                case 1:
                    tslMessage.ForeColor = Color.Red;
                    break;
                case 2:
                    tslMessage.ForeColor = Color.Blue;
                    break;
                default:
                    tslMessage.ForeColor = Color.Black;
                    break;
            }
        }
        // 从全路径得到文件名称,如果不在工艺下返回全路径
        /// <summary>
        /// 从全路径得到文件名称,如果不在工艺下返回全路径
        /// </summary>
        /// <param name="fullpath">文件全路径</param>
        /// <returns>名称或全路径</returns>
        private string ToFileName(string fullpath)
        {
            if (fullpath.Contains(ProPath))
            {
                return Path.GetFileName(fullpath);
            }
            else
            {
                return fullpath;
            }
        }
        //新建菜单
        private void tsmNew_Click(object sender, EventArgs e)
        {
            if (IsProjectOpen)
            {
                DialogResult dialogResult = MessageBox.Show("新建操作需要先关闭当前工艺，是否保存当前工艺？", "提示信息", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CloseProject(true))
                    {
                        IsProjectOpen = false;
                        //初始化控件状态
                        pnlMain.Hide();
                        pnlWelcome.Show();
                    }
                    else
                    {
                        MessageBox.Show("保存工艺失败！");
                        return;
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    CloseProject(false);
                    IsProjectOpen = false;
                    //初始化控件状态
                    pnlMain.Hide();
                    pnlWelcome.Show();
                } 
                else
                {
                    return;
                }
            }
            CreateNewProject();
        }
        //打开菜单
        private void tsmOpen_Click(object sender, EventArgs e)
        {
            if (IsProjectOpen)
            {
                DialogResult dialogResult = MessageBox.Show("新建操作需要先关闭当前工艺，是否保存当前工艺？", "提示信息", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CloseProject(true))
                    {
                        IsProjectOpen = false;
                        //初始化控件状态
                        pnlMain.Hide();
                        pnlWelcome.Show();
                    }
                    else
                    {
                        MessageBox.Show("保存工艺失败！");
                        return;
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    CloseProject(false);
                    IsProjectOpen = false;
                    //初始化控件状态
                    pnlMain.Hide();
                    pnlWelcome.Show();
                }
                else
                {
                    return;
                }
            }
            OpenProject();
        }
        //保存菜单
        private void tsmSave_Click(object sender, EventArgs e)
        {
            SaveProject(false);
        }
        //退出菜单
        private void tsmExit_Click(object sender, EventArgs e)
        {
            ExitProject();
        }
        //恢复工程文件夹菜单
        private void tsmRecover_Click(object sender, EventArgs e)
        {
            if (IsProjectOpen)
            {
                DialogResult dialogResult = MessageBox.Show("新建操作需要先关闭当前工艺，是否保存当前工艺？", "提示信息", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CloseProject(true))
                    {
                        IsProjectOpen = false;
                        //初始化控件状态
                        pnlMain.Hide();
                        pnlWelcome.Show();
                    }
                    else
                    {
                        MessageBox.Show("保存工艺失败！");
                        return;
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    CloseProject(false);
                    IsProjectOpen = false;
                    //初始化控件状态
                    pnlMain.Hide();
                    pnlWelcome.Show();
                }
                else
                {
                    return;
                }
            }
            RecoverProject();
        }
        //打开所有模型菜单
//         private void tsmOpenAllPrt_Click(object sender, EventArgs e)
//         {
//             if (IsProjectOpen)
//             {
//                 List<S_Model> ModelList = XML3DPPM.GetModelList(XmlFile);
//                 foreach (S_Model model in ModelList)
//                 {
//                     if (!String.IsNullOrEmpty(model.filename))
//                     {
//                         NXFun.OpenPrtQuiet(ToFullPath(model.filename));
//                     }                        
//                 }
//                 SetStatusLabel("已在后台打开所有模型！",2);
//             } 
//             else
//             {
//                 SetStatusLabel("请先打开一本工艺！", 1);
//                 return;
//             }
//         }
        //切换环境菜单
        private void tsmChangeEnv_Click(object sender, EventArgs e)
        {

        }
        //帮助菜单
        private void tsmHelp_Click(object sender, EventArgs e)
        {
            NXFun.ShowHelp();
        }
        //关于菜单
        private void tsmAbout_Click(object sender, EventArgs e)
        {
            NXFun.ShowAbout();
        }
        //建模环境菜单
        private void tsmModeling_Click(object sender, EventArgs e)
        {
            bool result = NXFun.SetEnvironment(0);
            SetStatusLabel(result ? "已切换到建模环境" : "无模型加载状态无法切换环境", result ? 2 : 1);
        }
        //制图环境菜单
        private void tsmDrafting_Click(object sender, EventArgs e)
        {
             bool result = NXFun.SetEnvironment(1);
            SetStatusLabel(result ? "已切换到制图环境" : "无模型加载状态无法切换环境", result ? 2 : 1);
        }
        //TabControl切换模块后
        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sel = tabMain.SelectedIndex;
            if (sel == 0)
            {
                //编辑工艺
            }
            else if (sel == 1)
            {
                //编辑模型
                ModelEdit.Init();
            }
            else if (sel == 2)
            {
                //编辑图纸
                SheetEdit.FreshSheetTree();
            }
        }
        //TabControl切换模块时
        private void tabMain_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (ModelEdit.IsEdit)
            {
                e.Cancel = true;
                SetStatusLabel("请先完成模型编辑操作！", 1);
            }
            else if (ProcessEdit.IsEdit)
            {
                e.Cancel = true;
                SetStatusLabel("请先完成工艺编辑操作！", 1);
            }
            else if (SheetEdit.LabelEdit.IsEdit)
            {
                e.Cancel = true;
                SetStatusLabel("请先完成打标号编辑操作！", 1);
            } 
        }
        //切换 隐藏菜单和工具栏以及标题栏
        public void ShowOrHideMenu(bool isShow)
        {
            mnsMain.Visible = isShow;
            strMain.Visible = isShow;
            this.ControlBox = isShow;
        }
        //获得文件的真实类型  -1 出错 0 zip  1 rar 2 其他 …… 
        /// <summary>
        /// 获得文件的真实类型  -1 出错 0 zip  1 rar 2 其他 …… 
        /// </summary>
        /// <param name="fullpath">文件路径</param>
        /// <returns>-1 出错 0 zip  1 rar 2 其他 …… </returns>
        private int GetFileRealType(string fullpath)
        {
            System.IO.FileStream fs = new System.IO.FileStream(fullpath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader r = new System.IO.BinaryReader(fs);
            string fileclass = "";
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                fileclass = buffer.ToString();
                buffer = r.ReadByte();
                fileclass += buffer.ToString();
            }
            catch
            {
                return -1;
            }
            r.Close();
            fs.Close();
            /*文件扩展名说明
                         *7173        gif 
                         *255216      jpg
                         *13780       png
                         *6677        bmp
                         *239187      txt,aspx,asp,sql
                         *208207      xls.doc.ppt
                         *6063        xml
                         *6033        htm,html
                         *4742        js
                         *8075        xlsx,zip,pptx,mmap,zip
                         *8297        rar   
                         *01          accdb,mdb
                         *7790        exe,dll           
                         *5666        psd 
                         *255254      rdp 
                         *10056       bt种子 
                         *64101       bat 
            */
            if (fileclass == "8075")
            {
                return 0;
            }
            else if (fileclass == "8297")
            {
                return 1;
            }
            else
            {
                return 2;
            }            
        }
        //加工面标注
        private void tsmJiagongmian_Click(object sender, EventArgs e)
        {
            NXFun.ShowJiagongmian();
        }
        //公差标注
        private void tsmGongcha_Click(object sender, EventArgs e)
        {
            NXFun.ShowGongcha(); 
        }
        //回转修剪
        private void tsmHuizhuan_Click(object sender, EventArgs e)
        {
            NXFun.ShowHuizhuan();
        }
        //CAPP助手
        private void tsmCAPP_Click(object sender, EventArgs e)
        {
            NXFun.ShowCAPP();
        }
    }
}