using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NXOpen;
using NXOpen.BlockStyler;
using NXOpen.UF;

namespace TDPPM
{
    public partial class MachinedSurface : Form
    {
        bool isShowFullType = false;
        string[] ob_list = new string[0];
        // IntPtr ptr;
        private int count = 0;
        private Session theSession;
        Session.UndoMarkId undomark = 0;
        private Dictionary<NXOpen.Drawings.DraftingView, NXOpen.Preferences.GeneralExtractedEdgesOption> map
            = new Dictionary<NXOpen.Drawings.DraftingView, NXOpen.Preferences.GeneralExtractedEdgesOption>();


        public MachinedSurface()
        {
            try
            {
                InitializeComponent();
                theSession = Session.GetSession();
                NXOpenUI.FormUtilities.ReparentForm(this);
                NXOpenUI.FormUtilities.SetApplicationIcon(this);
                undomark = theSession.SetUndoMark(Session.MarkVisibility.Visible, "�ӹ����ע");
                //��¼���ı�View������
                Part workpart = theSession.Parts.Work;
                foreach (NXOpen.Drawings.DraftingView draftingview in workpart.DraftingViews)
                {
                    NXOpen.Preferences.GeneralExtractedEdgesOption type = NXFun.GetViewEdgesPreference(draftingview);
                    map.Add(draftingview, type);
                    NXFun.SetViewEdgesPreference(draftingview, NXOpen.Preferences.GeneralExtractedEdgesOption.Associative);
                }
                label1.Text = "- ��ѡ��ӹ��� (0)";
                //��ʼ���ѡ���Ķ���
                this.timer1.Start();
            }
            catch (System.Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            }
        }
        private void button_add_Click(object sender, EventArgs e)
        {
            try
            {
                //�õ�ѡ��
                NXObject[] obs;
                NXFun.GetSelectObjects(out obs);
                Part workpart = theSession.Parts.Work;
                workpart.Views.WorkView.Fit();
                foreach (NXOpen.Drawings.DraftingView draftingview in workpart.DraftingViews)
                {
                    foreach (NXObject ob in obs)
                    {
                        try
                        {
                            //����ͼ�ж����Ըı�����
                            if (NXFun.isFindInView(draftingview, ob))
                            {
                                NXFun.SetViewCurveWidth(draftingview, (NXOpen.DisplayableObject)ob, NXOpen.ViewDependentDisplayManager.Width.Thick);
                            }
                        }
                        catch/*(System.Exception ex)*/
                        {

                        }
                    }
                }
                NXFun.RemoveAllSelect();

            }
            catch (Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            }
        }
        private void button_remove_Click(object sender, EventArgs e)
        {
            //�õ�ѡ��
            NXObject[] obs;
            NXFun.GetSelectObjects(out obs);
            Part workpart = theSession.Parts.Work;
            workpart.Views.WorkView.Fit();
            foreach (NXOpen.Drawings.DraftingView draftingview in workpart.DraftingViews)
            {
                foreach (NXObject ob in obs)
                {
                    try
                    {
                        //��������ͼ�ж����Ըı�����
                        if (NXFun.isFindInView(draftingview, ob))
                        {
                            NXFun.SetViewCurveWidth(draftingview, (NXOpen.DisplayableObject)ob, NXOpen.ViewDependentDisplayManager.Width.Normal);
                        }
                    }
                    catch/* (System.Exception ex)*/
                    {

                    }
                }
            }
            NXFun.RemoveAllSelect();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //���״ָ̬ʾ��С����������
                count = (++count) % 3;
                string star = "\\";
                switch (count)
                {
                    case 0: star = "-";
                        break;
                    case 1: star = "\\";
                        break;
                    case 2: star = "/";
                        break;
                }
                //�õ�ѡ�еĶ���
                NXObject[] nxobs;
                int num = NXFun.GetSelectObjects(out nxobs);
                label1.Text = star + " ��ѡ��ӹ��� (" + System.Convert.ToString(num) + ")";
                string[] obs = new string[num];
                for (int i = 0; i < num; i++)
                {
                    if (isShowFullType)
                    {
                        obs[i] = nxobs[i].GetType().FullName;
                    }
                    else
                    {
                        obs[i] = nxobs[i].ToString();
                    }

                }
                //������޸ı�
                if (!NXFun.isArrayEqual(ob_list, obs))
                {
                    listBox1.Items.Clear();
                    foreach (string str in obs)
                    {
                        listBox1.Items.Add(str);
                    }
                }
                ob_list = obs;
            }
            catch (System.Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            }

        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            NXFun.RemoveAllSelect();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            try
            {
                theSession.UndoToMark(undomark, "�ӹ����ע");
                theSession.DeleteUndoMark(undomark, "�ӹ����ע");
                //��ԭ��ͼ����
                //7.5��bug����ԭʱ����ֱ�ע���Ż��ң�8.0�����ڡ�
                //                 foreach (KeyValuePair<NXOpen.Drawings.DraftingView, NXOpen.Preferences.GeneralExtractedEdgesOption> a in map)
                //                 {
                //                     NXFun.SetViewEdgesPreference(a.Key, a.Value);
                //                 }
                NXFun.RemoveAllSelect();
            }
            catch (System.Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            }
            finally
            {
                this.Close();
            }

        }
        private void button_ok_Click(object sender, EventArgs e)
        {
            //��ԭ��ͼ����
            //7.5��bug����ԭʱ����ֱ�ע���Ż��ң�8.0�����ڡ�
            //foreach (KeyValuePair<NXOpen.Drawings.DraftingView, NXOpen.Preferences.GeneralExtractedEdgesOption> a in map)
            //{
            //    NXFun.SetViewEdgesPreference(a.Key, a.Value);
            //}
            NXFun.RemoveAllSelect();
            this.Close();
        }

        private void button_help_Click(object sender, EventArgs e)
        {

            string help =
                "ע�����\n" +
                "1��Ŀǰ�ӹ����עֻ֧��ͶӰ��ͼ�ڵ����ߣ��Ի��ͼ�������ñ༭��ʾ���������߿�\n" +
                "2����������ʱ�Զ������С���ͼ��ʽ-��ȡ�ıߡ�����Ϊ��������������ѡ�С�\n" +
                "3����7.5����𽫡���ȡ�ıߡ���Ϊ��������������Ϊ���ޡ����⽫������ͼ�ߴ��עλ�û��ҡ�NX8.0��������BUG��\n" +
                "��������������������������\n" +
                "�ӹ����ע���� V1.0";
            UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Information, help);
        }

        private void form_doubleclick(object sender, EventArgs e)
        {
            isShowFullType = !isShowFullType;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void MachinedSurface_FormClosed(object sender, FormClosedEventArgs e)
        {
            //             skinEngine1.Active = false;
            //             skinEngine1.Dispose();
        }

    }
}