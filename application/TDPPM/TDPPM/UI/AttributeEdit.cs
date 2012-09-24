using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NXOpen;

namespace TDPPM
{
    public partial class AttributeEdit : Form
    {
        public struct s_Attr
        {
            public string title;
            public string s_value;
            public int i_value;
            public string type;
        }
        public NXObject obj;
        public List<s_Attr> AttrList = new List<s_Attr>();
        Session.UndoMarkId undomark = 0;
        private Session theSession;


        public AttributeEdit()
        {
            InitializeComponent();
            theSession = Session.GetSession();
            NXOpenUI.FormUtilities.ReparentForm(this);
            NXOpenUI.FormUtilities.SetApplicationIcon(this);
            undomark = theSession.SetUndoMark(Session.MarkVisibility.Visible, "属性编辑");
        }

        private void button_read_Click(object sender, EventArgs e)
        {
            NXObject[] nxobs = NXFun.GetSelectObjects();
            if (nxobs.Length == 0)
            {
                MessageBox.Show("没有在NX选中东东哦！");
            }
            else   //查询
            {
                AttrList.Clear();
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    //得到sheet 做二维图表需要设置隐藏属性
                    NXOpen.Drawings.DrawingSheet[] ds = theSession.Parts.Work.DrawingSheets.ToArray();
                    if (ds.Length!=0)
                    {
                        obj = ds[0];
                    } 
                    else
                    {
                        obj = nxobs[0];
                    }
                    
                }
                else
                {
                    obj = nxobs[0];
                }                
                s_Attr s_attr;
                NXObject.AttributeInformation[] ais = obj.GetAttributeTitlesByType(NXObject.AttributeType.String);
                for (int i = 0; i < ais.Length; i ++ )
                {
                    s_attr.title = ais[i].Title;
                    s_attr.s_value = obj.GetStringAttribute(s_attr.title);
                    s_attr.type = "string";
                    s_attr.i_value = 0;
                    AttrList.Add(s_attr);
                }
                ais = obj.GetAttributeTitlesByType(NXObject.AttributeType.Integer);
                for (int i = 0; i < ais.Length; i++)
                {
                    s_attr.title = ais[i].Title;
                    s_attr.i_value = obj.GetIntegerAttribute(s_attr.title);
                    s_attr.type = "int";
                    s_attr.s_value = "";
                    AttrList.Add(s_attr);
                }
                RefreshDisplay();
            }
            
        }
        private void RefreshDisplay()
        {
            try
            {
                listView1.Items.Clear();
                for (int i = 0; i < AttrList.Count; i++)
                {
                    listView1.Items.Add(AttrList[i].title);
                    listView1.Items[i].SubItems.Add(AttrList[i].type);
                    if (AttrList[i].type == "string")
                    {
                        listView1.Items[i].SubItems.Add(AttrList[i].s_value);
                    }
                    else if (AttrList[i].type == "int")
                    {
                        listView1.Items[i].SubItems.Add(AttrList[i].i_value.ToString());
                    }
                }
                listView1.Columns[0].AutoResize(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.Columns[2].AutoResize(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            catch (System.Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            try
            {
                theSession.UndoToMark(undomark, "属性编辑");
                theSession.DeleteUndoMark(undomark, "属性编辑");           
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

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                textBox_name.Text = AttrList[e.ItemIndex].title;
                if (AttrList[e.ItemIndex].type == "string")
                {
                    textBox_value.Text = AttrList[e.ItemIndex].s_value;
                }
                else if (AttrList[e.ItemIndex].type == "int")
                {
                    textBox_value.Text = AttrList[e.ItemIndex].i_value.ToString();
                }
                
            }
               
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button_save_Click(object sender, EventArgs e)
        {

        }      
    }
}