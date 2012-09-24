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
        private string _ProPath = "";   //���ձ���·��;
        private string ProPath          //���ձ���·��;
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
        private string ProName = ""; //���ձ����ļ���;
        private bool IsProjectOpen = false; //��ǰ�Ƿ��д򿪵Ĺ���
        private string XmlFile = "";    //����xml�ļ�;
        private string TemplateXML = NXFun.TDPPMPath + NXFun.TemplateXML;
        //���캯��
        public MainDlg()
        {
            InitializeComponent();
            NXOpenUI.FormUtilities.ReparentForm(this);
            NXOpenUI.FormUtilities.SetApplicationIcon(this);          
            ProcessEdit.mainDlg = this;
            ModelEdit.mainDlg = this;
            SheetEdit.mainDlg = this;
            //��ʼ���ؼ�״̬
            pnlMain.Hide();
            pnlWelcome.Show();
        }
        //���������ڹر�ʱ
        private void MainDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsProjectOpen)
            {
                e.Cancel = false;
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("�Ƿ񱣴浱ǰ���գ�", "��ʾ��Ϣ", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CloseProject(true))
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        MessageBox.Show("���湤��ʧ�ܣ�");
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
        //�½�����
        private void CreateNewProject()
        {
            int flag = 0;
            //ѡ�񱣴�·��;
            saveFileDialog.AddExtension = true;
            saveFileDialog.Title = "��ѡ��Ҫ����Ĺ�������·��";
            saveFileDialog.DefaultExt = "3dppm";
            saveFileDialog.Filter = "��ά���ӹ����ļ�(*.3dppm)|*.3dppm";
            saveFileDialog.OverwritePrompt = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog.FileName;
                if (NXFun.IsChina(filename))
                {
                    MessageBox.Show("NX��֧������·����������ѡ�񱣴�·��");
                    flag = 1;
                }
                else
                {
                    //�õ�����·�����ļ���;
                    ProPath = Path.GetDirectoryName(filename);
                    ProName = Path.GetFileNameWithoutExtension(filename);
                    XmlFile = ToFullPath(NXFun.ProcessXML);
                    //ɾ���������ļ���;
                    bool result = NXFun.DeleteDirectory(ToFullPath(""));
                    if (!result)
                    {
                        MessageBox.Show("�ļ���" + ToFullPath("") + "�޷�ɾ�������ֶ��رտ����Ѵ򿪵��ļ���ɾ����Ŀ¼��");
                        return;
                    }
                    Directory.CreateDirectory(ToFullPath(""));
                    //���ƹ���ģ��;
                    File.Copy(NXFun.TDPPMPath + NXFun.ProcessXML, XmlFile, true);
                    OpenOrCreateProjectInit();
                    SetStatusLabel("�����½��ɹ���", 2);
                }
            }
            if (flag == 1)
            {
                //�������ģ�����ѡ���ļ�;
                CreateNewProject();

            }
        }
        //�򿪻��½�����ʱ��ʼ��
        private void OpenOrCreateProjectInit()
        {
            //����ֵ
            IsProjectOpen = true;
            
            //�ؼ�״̬
            pnlWelcome.Hide();
            pnlMain.Show();

            this.Text = ProPath + "\\" + ProName + ".3dppm";
            //���»�ǩ
            if (NXFun.isFileExist(ToFullPath(NXFun.SignOffXML)))
            {
                bool result = XML3DPPM.UpdateSignOff(XmlFile, ToFullPath(NXFun.SignOffXML));
                if (result)
                {
                    NXFun.MessageBox("���»�ǩ��Ϣ�ɹ�������\"�༭ͼֽ->��άͼ��\"ģ����\"���±�ͷ\"��ˢ�»�ǩ��Ϣ��");
                } 
                else
                {
                    NXFun.MessageBox("���»�ǩ��Ϣʧ�ܣ���ȷ�ϻ�ǩ�ļ��Ƿ���Ч��");
                }
            }
            //����
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
        //�õ�ȫ·��
        private string ToFullPath(string filename)
        {
            return ProPath + "\\" + ProName + "\\" + filename;
        }
        //�򿪹���
        private void OpenProject()
        {
            openFileDialog.Title = "��ѡ��Ҫ�򿪵Ĺ����ļ�:";
            openFileDialog.Filter = "3DPPM �ļ� (*.3dppm)|*.3dppm";
            openFileDialog.FileName = "";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                if (NXFun.IsChina(filename))
                {
                    MessageBox.Show("NX��֧������·�����뽫�����ļ��ŵ�Ӣ��·�����ٴ�.");
                    return;
                }
                else
                {
                    //�õ�����·�����ļ���;
                    ProPath = Path.GetDirectoryName(filename);
                    ProName = Path.GetFileNameWithoutExtension(filename);
                    XmlFile = ToFullPath(NXFun.ProcessXML);
                    //ɾ���ļ���;
                    bool result = NXFun.DeleteDirectory(ProPath +"\\"+ ProName);
                    if (!result)
                    {
                        MessageBox.Show("�ļ���" + ProPath +"\\"+ ProName + "�޷�ɾ�������ֶ��رտ����Ѵ򿪵��ļ���ɾ����Ŀ¼��");
                        return;
                    }
                    //�ж��Ƿ���ѹ���ļ�
                    int type = GetFileRealType(filename);
                    switch (type)
                    {
                        case -1:
                            MessageBox.Show("��ȡ�ļ�������ȷ���ļ��Ƿ���Ч��");
                            return;
                        case 0:
                            break;
                        case 1:
                            break;
                        default:
                            MessageBox.Show("��ȡ�ļ�������ȷ���ļ��Ƿ���Ч��");
                            return;
                    }
                    //��ѹ
                    bool isUnZip = true;
                    if (type == 0)
                    {
                        //��zip��ʽ��ѹ
                        isUnZip = Compress.UnZipFileDictory(filename, "");
                    }
                    else if (type == 1)
                    {
                        //��rar��ʽ��ѹ
                        isUnZip = Compress.UnRAR(NXFun.TDPPMPath + NXFun.RarEXE, filename,ToFullPath(""));     
                    }
                    if (!isUnZip)
                    {
                        MessageBox.Show(
                            "�����ļ���ʧ�ܣ���ȷ�����¼��\r\n" +
                            "1���ļ����Ƿ���Ч��\r\n" +
                            "2��������ʱ�ļ��л������ļ������ڴ�״̬��رա�");
                        return;
                    }
                    //����ļ���Ч��
                    if (!NXFun.isFileExist(XmlFile))
                    {
                        MessageBox.Show("���������ļ���ʧ����ȷ���ļ���Ч��");
                    }
                    //���汾��Ч�ԣ����ɰ����Զ�����
                    XML3DPPM.Update3dppm(XmlFile, TemplateXML);


                    OpenOrCreateProjectInit();
                    //������ģ��
                    List<S_Model> ModelList = XML3DPPM.GetModelList(XmlFile);
                    foreach (S_Model model in ModelList)
                    {
                        if (!String.IsNullOrEmpty(model.filename))
                        {
                            NXFun.OpenPrtQuiet(ToFullPath(model.filename));
                        }
                          
                    }
                    SetStatusLabel("���մ򿪳ɹ���", 2);
                }
            }
        }
        //�ָ�����
        private void RecoverProject()
        {
            folderBrowserDialog.Description = "��ѡ��Ҫ�򿪵Ĺ�����ʱ�ļ���:";
            folderBrowserDialog.ShowNewFolderButton = false;
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = folderBrowserDialog.SelectedPath;
                //�жϺϷ���
                string root = Path.GetPathRoot(path);
                string pathname = Path.GetDirectoryName(path);
                string filename = Path.GetFileName(path);
                if (path.Equals(root) && string.IsNullOrEmpty(pathname) && string.IsNullOrEmpty(filename))
                {
                    MessageBox.Show("�벻Ҫѡ���Ŀ¼��");
                    return;
                }
                else if (NXFun.IsChina(path))
                {
                    MessageBox.Show("NX��֧������·�������޸�·����");
                    return;
                } 
                else if(!NXFun.isFileExist(path + "\\" + NXFun.ProcessXML))
                {
                    MessageBox.Show("��ѡ·���ǹ�����ʱ���ݰ�");
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
        //���湤��
        private bool SaveProject(bool isQuit)
        {
            try
            {
                bool result = false;
                List<S_Model> ModelList = XML3DPPM.GetModelList(XmlFile);
                if (isQuit)
                {
                    //�ر����в�����ѹ��          
                    foreach (S_Model model in ModelList)
                    {
                        if (!String.IsNullOrEmpty(model.filename))
                        {
                            NXFun.ClosePrt(ToFullPath(model.filename), true);
                        }
                        
                    }
                    //ѹ��
                    string tempZip = ProPath + "\\" + ProName + ".tmp";
                    bool isSaved = Compress.RAR(NXFun.TDPPMPath + NXFun.RarEXE, tempZip, ProPath + "\\" + ProName);
                    if (isSaved)
                    {
                        File.Copy(tempZip, ProPath + "\\" + ProName + ".3dppm", true);
                        File.Delete(tempZip);
                        NXFun.DeleteDirectory(ProPath + "\\" + ProName);
                        SetStatusLabel("���湤�ճɹ���", 2);
                        result = true;
                    }
                    else
                    {
                        SetStatusLabel("���ձ���ʧ�ܣ�", 1);
                        MessageBox.Show("���ձ���ʧ�ܣ�");
                        result = false;
                    }
                }
                else
                {
                    //������
                    foreach (S_Model model in ModelList)
                    {
                        if (!String.IsNullOrEmpty(model.filename))
                        {
                            NXFun.SavePrt(ToFullPath(model.filename));
                        }
                        
                    }
                    //ѹ��
                    string tempZip = ProPath + "\\" + ProName + ".tmp";
                    bool isSaved = Compress.RAR(NXFun.TDPPMPath + NXFun.RarEXE, tempZip, ProPath + "\\" + ProName);
                    if (isSaved)
                    {
                        File.Copy(tempZip, ProPath + "\\" + ProName + ".3dppm", true);
                        File.Delete(tempZip);
                        SetStatusLabel("���湤�ճɹ���", 2);
                        result = true;
                    }
                    else
                    {
                        SetStatusLabel("���ձ���ʧ�ܣ�", 1);
                        MessageBox.Show("���ձ���ʧ�ܣ�");
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
        //�رչ���
        private bool CloseProject(bool isSave)
        {
            bool result = false;
            List<S_Model> ModelList = XML3DPPM.GetModelList(XmlFile);
            //�ر����в�����ѹ��          
            foreach (S_Model model in ModelList)
            {
                if (!String.IsNullOrEmpty(model.filename))
                {
                    NXFun.ClosePrt(ToFullPath(model.filename), isSave);
                }                
            }
            if(isSave)
            {
                //ѹ��
                string tempZip = ProPath + "\\" + ProName + ".tmp";
                bool isSaved = Compress.RAR(NXFun.TDPPMPath + NXFun.RarEXE, tempZip, ProPath + "\\" + ProName);
                if (isSaved)
                {
                    File.Copy(tempZip, ProPath + "\\" + ProName + ".3dppm", true);
                    File.Delete(tempZip);
                    NXFun.DeleteDirectory(ProPath + "\\" + ProName);
                    SetStatusLabel("���湤�ճɹ���", 2);
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
        //�˳�ϵͳ
        private void ExitProject()
        {
            if (!IsProjectOpen)
            {
                this.Close();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("�Ƿ񱣴浱ǰ���գ�", "��ʾ��Ϣ", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CloseProject(true))
                    {
                        IsProjectOpen = false;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("���湤��ʧ�ܣ�");
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
        //����״̬����ʾ����
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
        // ��ȫ·���õ��ļ�����,������ڹ����·���ȫ·��
        /// <summary>
        /// ��ȫ·���õ��ļ�����,������ڹ����·���ȫ·��
        /// </summary>
        /// <param name="fullpath">�ļ�ȫ·��</param>
        /// <returns>���ƻ�ȫ·��</returns>
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
        //�½��˵�
        private void tsmNew_Click(object sender, EventArgs e)
        {
            if (IsProjectOpen)
            {
                DialogResult dialogResult = MessageBox.Show("�½�������Ҫ�ȹرյ�ǰ���գ��Ƿ񱣴浱ǰ���գ�", "��ʾ��Ϣ", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CloseProject(true))
                    {
                        IsProjectOpen = false;
                        //��ʼ���ؼ�״̬
                        pnlMain.Hide();
                        pnlWelcome.Show();
                    }
                    else
                    {
                        MessageBox.Show("���湤��ʧ�ܣ�");
                        return;
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    CloseProject(false);
                    IsProjectOpen = false;
                    //��ʼ���ؼ�״̬
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
        //�򿪲˵�
        private void tsmOpen_Click(object sender, EventArgs e)
        {
            if (IsProjectOpen)
            {
                DialogResult dialogResult = MessageBox.Show("�½�������Ҫ�ȹرյ�ǰ���գ��Ƿ񱣴浱ǰ���գ�", "��ʾ��Ϣ", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CloseProject(true))
                    {
                        IsProjectOpen = false;
                        //��ʼ���ؼ�״̬
                        pnlMain.Hide();
                        pnlWelcome.Show();
                    }
                    else
                    {
                        MessageBox.Show("���湤��ʧ�ܣ�");
                        return;
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    CloseProject(false);
                    IsProjectOpen = false;
                    //��ʼ���ؼ�״̬
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
        //����˵�
        private void tsmSave_Click(object sender, EventArgs e)
        {
            SaveProject(false);
        }
        //�˳��˵�
        private void tsmExit_Click(object sender, EventArgs e)
        {
            ExitProject();
        }
        //�ָ������ļ��в˵�
        private void tsmRecover_Click(object sender, EventArgs e)
        {
            if (IsProjectOpen)
            {
                DialogResult dialogResult = MessageBox.Show("�½�������Ҫ�ȹرյ�ǰ���գ��Ƿ񱣴浱ǰ���գ�", "��ʾ��Ϣ", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (CloseProject(true))
                    {
                        IsProjectOpen = false;
                        //��ʼ���ؼ�״̬
                        pnlMain.Hide();
                        pnlWelcome.Show();
                    }
                    else
                    {
                        MessageBox.Show("���湤��ʧ�ܣ�");
                        return;
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    CloseProject(false);
                    IsProjectOpen = false;
                    //��ʼ���ؼ�״̬
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
        //������ģ�Ͳ˵�
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
//                 SetStatusLabel("���ں�̨������ģ�ͣ�",2);
//             } 
//             else
//             {
//                 SetStatusLabel("���ȴ�һ�����գ�", 1);
//                 return;
//             }
//         }
        //�л������˵�
        private void tsmChangeEnv_Click(object sender, EventArgs e)
        {

        }
        //�����˵�
        private void tsmHelp_Click(object sender, EventArgs e)
        {
            NXFun.ShowHelp();
        }
        //���ڲ˵�
        private void tsmAbout_Click(object sender, EventArgs e)
        {
            NXFun.ShowAbout();
        }
        //��ģ�����˵�
        private void tsmModeling_Click(object sender, EventArgs e)
        {
            bool result = NXFun.SetEnvironment(0);
            SetStatusLabel(result ? "���л�����ģ����" : "��ģ�ͼ���״̬�޷��л�����", result ? 2 : 1);
        }
        //��ͼ�����˵�
        private void tsmDrafting_Click(object sender, EventArgs e)
        {
             bool result = NXFun.SetEnvironment(1);
            SetStatusLabel(result ? "���л�����ͼ����" : "��ģ�ͼ���״̬�޷��л�����", result ? 2 : 1);
        }
        //TabControl�л�ģ���
        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sel = tabMain.SelectedIndex;
            if (sel == 0)
            {
                //�༭����
            }
            else if (sel == 1)
            {
                //�༭ģ��
                ModelEdit.Init();
            }
            else if (sel == 2)
            {
                //�༭ͼֽ
                SheetEdit.FreshSheetTree();
            }
        }
        //TabControl�л�ģ��ʱ
        private void tabMain_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (ModelEdit.IsEdit)
            {
                e.Cancel = true;
                SetStatusLabel("�������ģ�ͱ༭������", 1);
            }
            else if (ProcessEdit.IsEdit)
            {
                e.Cancel = true;
                SetStatusLabel("������ɹ��ձ༭������", 1);
            }
            else if (SheetEdit.LabelEdit.IsEdit)
            {
                e.Cancel = true;
                SetStatusLabel("������ɴ��ű༭������", 1);
            } 
        }
        //�л� ���ز˵��͹������Լ�������
        public void ShowOrHideMenu(bool isShow)
        {
            mnsMain.Visible = isShow;
            strMain.Visible = isShow;
            this.ControlBox = isShow;
        }
        //����ļ�����ʵ����  -1 ���� 0 zip  1 rar 2 ���� ���� 
        /// <summary>
        /// ����ļ�����ʵ����  -1 ���� 0 zip  1 rar 2 ���� ���� 
        /// </summary>
        /// <param name="fullpath">�ļ�·��</param>
        /// <returns>-1 ���� 0 zip  1 rar 2 ���� ���� </returns>
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
            /*�ļ���չ��˵��
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
                         *10056       bt���� 
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
        //�ӹ����ע
        private void tsmJiagongmian_Click(object sender, EventArgs e)
        {
            NXFun.ShowJiagongmian();
        }
        //�����ע
        private void tsmGongcha_Click(object sender, EventArgs e)
        {
            NXFun.ShowGongcha(); 
        }
        //��ת�޼�
        private void tsmHuizhuan_Click(object sender, EventArgs e)
        {
            NXFun.ShowHuizhuan();
        }
        //CAPP����
        private void tsmCAPP_Click(object sender, EventArgs e)
        {
            NXFun.ShowCAPP();
        }
    }
}