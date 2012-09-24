using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using TDPPM;

namespace WaveRelationControl
{
    public partial class WaveRelationCtl : UserControl
    {
        /// <summary>
        /// 所有输入的独立绘图节点
        /// </summary>
        //private ArrayList _RootModellist;


        ArrayList WRClassList = new ArrayList();

       /// <summary>
       /// 所有需要绘制的模型列表
       /// </summary>
        Hashtable HTExistModel;

        /// <summary>
        /// 当前被选择的模型
        /// </summary>
        private ModelClass SelectModel;


        public event ModelSelectHandle ModelSelected;
        public event ModelDoubleClick ModelDClick;

        public WaveRelationCtl()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// 设置空间的Wave 根节点
        /// </summary>
        /// <param name="RootClass">设计模型根节点</param>
        /// <param name="AttachModellist">辅助节点列表</param>
        public void SetRootClass(ArrayList RootModellist)
        {
            this.HorizontalScroll.Value = 0;
            int StartX = 0;
            int StartY = 0;

            int MaxHeight = 0;


            HTExistModel = new Hashtable();

            for (int i = 0; i < RootModellist.Count; i++)
            {
                ModelClass M = RootModellist[i] as ModelClass;
                if (!HTExistModel.Contains(M.ModelID))
                {
                    WaveRelationClass WRClass = new WaveRelationClass(M);

                    WRClass.SetEveryModelLocation();

                    int MaxWaveLength = WRClass.GetLocationLength();
                    int MaxModelDepth = WRClass.GetLocationWidth();

                    int tWidth = 0;
                    int tHeight = CtlGlobalClass.UpBlank + CtlGlobalClass.DownBlank + (MaxWaveLength) * CtlGlobalClass.VShapeDis;

                    if (i == 0)
                    {
                        tWidth = CtlGlobalClass.LeftBlank + CtlGlobalClass.RightBlank + (MaxModelDepth) * CtlGlobalClass.HLayerDis;
                    }
                    else
                    {
                        tWidth = CtlGlobalClass.LeftBlank + (MaxModelDepth) * CtlGlobalClass.HLayerDis;
                    }

                    if (tHeight > MaxHeight)
                    {
                        MaxHeight = tHeight;
                    }

                    WRClass.InitModelCord(StartX, StartY, tWidth, tHeight);
                    StartX += tWidth;

                    Hashtable ExistNodeList =  WRClass.GetExistNodeList();
                    foreach (int key in ExistNodeList.Keys)
                    {
                        if (!HTExistModel.ContainsKey(key))
                        {
                            HTExistModel.Add(key, ExistNodeList[key]);
                        }
                    }

                    WRClassList.Add(WRClass);                
                }
            }

            this.DrawPanel.Top = 0;
            this.DrawPanel.Left = 0;

            this.DrawPanel.Width = StartX;
            this.DrawPanel.Height = MaxHeight;



            this.DrawPanel.Refresh();

        }


       
        


        private void WaveRelationCtl_Paint(object sender, PaintEventArgs e)
        {
            

        }

        private void DrawModel(Graphics g, ModelClass modelClass)
        {
            modelClass.DrawModel(g);
            
            for (int i = 0; i < modelClass.CopyChild.Count; i++)
            {
                DrawModel(g, (ModelClass)modelClass.CopyChild[i]);
            }

            for (int i = 0; i < modelClass.WaveChild.Count; i++)
            {
                DrawModel(g, (ModelClass)modelClass.WaveChild[i]);
            }


        }

        private void WaveRelationCtl_Load(object sender, EventArgs e)
        {
            // cordClass = new CordClass(new Point(0, 0), new Point(this.DrawPanel.Width, this.DrawPanel.Height), new PointF(0, 0), new PointF(1000, 1000));
        }


        public void ClearAll()
        {
            this.WRClassList.Clear();
            if (HTExistModel != null)
            {
                HTExistModel.Clear();
            }

            SelectModel = null;
            this.Refresh();
        }


        private void DrawPanel_Paint(object sender, PaintEventArgs e)
        {
            if (WRClassList.Count != 0)
            {
                foreach (WaveRelationClass WRClass in WRClassList)
                {
                    WRClass.DrawModel(e.Graphics);
                }
            }
        }

        private void DrawPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WRClassList.Count != 0)
            {
                if (HTExistModel != null)
                {
             
                    Point P = new Point(e.X, e.Y);
                    ModelClass M = GetSelectModel(P);
                    if (M != null)
                    {
                        if (ModelDClick != null)
                        {
                            ModelDClick(this, new ModelSelectChangeEventArgs(M));
                        }
                        // MessageBox.Show(M.SeqName); 
                    }

//                     WavePreview wp = new WavePreview();
//                     wp.Location = Control.MousePosition;
//                     wp.Show(this);
                    
                }
            }
        }

        private ModelClass GetSelectModel(Point P)
        {
            try
            {
                foreach (int ModelID in HTExistModel.Keys)
                {
                    ModelClass MC = (ModelClass)HTExistModel[ModelID];
                    if (MC.IsPointInModel(P))
                    {
                        return MC;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private void DrawPanel_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void DrawPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (WRClassList.Count != 0)
            {
                Point P = new Point(e.X, e.Y);
                ModelClass M = GetSelectModel(P);
                if (M != null)
                {

                    if (SelectModel != null)
                    {
                        SelectModel.IsSelected = false;
                    }
                    SelectModel = M;
                    M.IsSelected = true;

                    if (ModelSelected != null)
                    {
                        ModelSelected(this, new ModelSelectChangeEventArgs(SelectModel));
                    }
                }
                this.Refresh();
            }
        }
    }


    public delegate void ModelSelectHandle(object sender, ModelSelectChangeEventArgs e);

    public delegate void ModelDoubleClick(object sender,ModelSelectChangeEventArgs e);
 
    public class ModelSelectChangeEventArgs : EventArgs
    {
        private ModelClass  _M;
        public ModelSelectChangeEventArgs(ModelClass M)
        {
            _M = M;
        }
        public ModelClass SelectModel
        {
            get{return _M;}
        }
    }

}
