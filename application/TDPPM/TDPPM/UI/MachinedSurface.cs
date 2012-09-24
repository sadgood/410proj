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
                undomark = theSession.SetUndoMark(Session.MarkVisibility.Visible, "加工面标注");
                //记录并改变View的类型
                Part workpart = theSession.Parts.Work;
                foreach (NXOpen.Drawings.DraftingView draftingview in workpart.DraftingViews)
                {
                    NXOpen.Preferences.GeneralExtractedEdgesOption type = NXFun.GetViewEdgesPreference(draftingview);
                    map.Add(draftingview, type);
                    NXFun.SetViewEdgesPreference(draftingview, NXOpen.Preferences.GeneralExtractedEdgesOption.Associative);
                }
                label1.Text = "- 请选择加工面 (0)";
                //开始监控选定的东东
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
                //得到选中
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
                            //在视图中都尝试改变线型
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
            //得到选中
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
                        //在所有视图中都尝试改变线型
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
                //监控状态指示的小东东，嘻嘻
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
                //得到选中的东东
                NXObject[] nxobs;
                int num = NXFun.GetSelectObjects(out nxobs);
                label1.Text = star + " 请选择加工面 (" + System.Convert.ToString(num) + ")";
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
                //检测有无改变
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
                theSession.UndoToMark(undomark, "加工面标注");
                theSession.DeleteUndoMark(undomark, "加工面标注");
                //还原视图类型
                //7.5有bug，还原时会出现标注附着混乱，8.0不存在。
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
            //还原视图类型
            //7.5有bug，还原时会出现标注附着混乱，8.0不存在。
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
                "注意事项：\n" +
                "1、目前加工面标注只支持投影视图内的曲线，自绘草图曲线请用编辑显示功能设置线宽；\n" +
                "2、程序启动时自动将所有“视图样式-抽取的边”设置为关联，这样才能选中。\n" +
                "3、在7.5里，请勿将“抽取的边”设为“关联”后再设为“无”，这将导致视图尺寸标注位置混乱。NX8.0已修正此BUG。\n" +
                "—————————————\n" +
                "加工面标注工具 V1.0";
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