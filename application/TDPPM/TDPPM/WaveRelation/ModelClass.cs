using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;

namespace WaveRelationControl
{
    public class ModelClass
    {
        /// <summary>
        /// 模型ID  作为模型的唯一识别符号
        /// </summary>
        private int _ModelID;

        public int ModelID
        {
            get { return _ModelID; }
            set { _ModelID = value; }
        }

 
        private string _ModelFileName;
        /// <summary>
        /// 模型文件名
        /// </summary>
        public string ModelFileName
        {
            get { return _ModelFileName; }
        }
        private string _SeqName;

        /// <summary>
        /// 工序名称
        /// </summary>
        public string SeqName
        {
            get { return _SeqName; }
        }

        private int _Layer;

        public int Layer
        {
            get { return _Layer; }
            set { _Layer = value; }
        }


        private bool _IsSelected = false;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set { _IsSelected = value; }
        }

        /// <summary>
        /// 绘图位置信息
        /// </summary>
        public Point Location = new Point(0,0);



        /// <summary>
        /// 此模型的父节点列表
        /// </summary>
        private ArrayList _ParentNodeList;

        public ArrayList ParentNodeList
        {
            get { return _ParentNodeList; }
            set { _ParentNodeList = value; }
        }

        /// <summary>
        /// 是否是Wave的节点
        /// </summary>
        private bool _IsWaveNode = true;

        public bool IsWaveNode
        {
            get { return _IsWaveNode; }
            set { _IsWaveNode = value; }
        }

        /// <summary>
        /// 用于记录Wave 链条的孩子
        /// </summary>
        private ArrayList _WaveChild;

        public ArrayList WaveChild
        {
            get { return _WaveChild; }
            set { _WaveChild = value; }
        }

        private ArrayList _CopyChild;

        public ArrayList CopyChild
        {
            get { return _CopyChild; }
            set { _CopyChild = value; }
        }



        /// <summary>
        /// 获取M1与此模型之间的关系
        /// </summary>
        /// <param name="M1">外部模型</param>
        /// <returns>0  没有关系
        ///          1  Wave 孩子关系
        ///          2  Copy 孩子关系
        ///          3  M1 是 当前模型的父亲</returns>
        public int GetRelationShip(ModelClass M1)
        {
            foreach (ModelClass M in _WaveChild)
            {
                if (M.ModelID == M1.ModelID)
                {
                    return 1;
                }
            }

            foreach (ModelClass M in _CopyChild)
            {
                if (M.ModelID == M1.ModelID)
                {
                    return 2;
                }
            }

            foreach (ModelClass M in _ParentNodeList)
            {
                if (M.ModelID == M1.ModelID)
                {
                    return 3;
                }
            }

            return 0;
        }



        /// <summary>
        /// 用于初始化模型类
        /// </summary>
        /// <param name="MName">模型文件名</param>
        /// <param name="SName">工序名称</param>
        public ModelClass(string MName, string SName)
        {
            _ModelID = GlobalClass.GlobalID++;

            _ModelFileName = MName;
            _SeqName = SName;
            _ParentNodeList = new ArrayList();


            _WaveChild = new ArrayList();
            _CopyChild = new ArrayList();
        }

        /// <summary>
        /// 设置父节点 
        /// </summary>
        /// <param name="PNode"></param>
        public void AddParentNode(ModelClass PNode)
        {
            _ParentNodeList.Add(PNode);
        }
       

        /// <summary>
        /// 添加 Wave 子节点 
        /// </summary>
        /// <param name="CNode"></param>
        public void AddWaveChild(ModelClass CNode)
        {
            _WaveChild.Add(CNode);
            CNode.AddParentNode(this);
        }

        /// <summary>
        /// 添加Copy  子节点
        /// </summary>
        /// <param name="CNode"></param>
        public void AddCopyChild(ModelClass CNode)
        {
            _CopyChild.Add(CNode);
            CNode.AddParentNode(this);
            CNode.IsWaveNode = false;
        }

        /// <summary>
        /// 计算连接线的交点
        /// </summary>
        /// <param name="LineP1"></param>
        /// <param name="Point2"></param>
        /// <param name="Radius"></param>
        /// <returns></returns>
        private ArrayList GetLineAndCircleJD(Point LineP1, Point Point2, double Radius)
        {
            double X1 = LineP1.X;
            double Y1 = LineP1.Y;

            double X2 = Point2.X;
            double Y2 = Point2.Y;

            double dx = X2 - X1;
            double dy = Y2 - Y1;


            double A = Math.Atan(dy / dx);
            double L = Math.Sqrt(dx * dx + dy * dy);

            double Temp1 = X1 + (L-Radius) * dx /L ;
            double Temp2 = Y1 + (L - Radius) *dy / L;



            double Temp3 = X1 + Radius * dx / L;
            double Temp4 = Y1 + Radius * dy / L;

            ArrayList AL = new ArrayList();


            AL.Add(new Point((int)Temp3, (int)Temp4));
            AL.Add(new Point((int)Temp1, (int)Temp2));

            return AL;

        }

        // 计算箭头的坐标
        public ArrayList GetArrowPoint(Point LineP1, Point Point2, double r)
        {

            double X1 = LineP1.X;
            double Y1 = LineP1.Y;

            double X2 = Point2.X;
            double Y2 = Point2.Y;

            double dx = X2 - X1;
            double dy = Y2 - Y1;

            double A = Math.Atan(dy / dx);
            double RJ =  15 * Math.PI/180;

            double A1 = A + RJ;

            double Temp1,Temp2;


            ArrayList AL = new ArrayList();

            if (dx >=0)
            {
                Temp1 = X2 - r * Math.Cos(A1);
                Temp2 = Y2 - r * Math.Sin(A1);
                AL.Add(new Point((int)Temp1, (int)Temp2));

                A1 = A - RJ;
                Temp1 = X2 - r * Math.Cos(A1);
                Temp2 = Y2 - r * Math.Sin(A1);

                AL.Add(new Point((int)Temp1, (int)Temp2));
            }
            else
            {
                Temp1 = X2 + r * Math.Cos(A1);
                Temp2 = Y2 + r * Math.Sin(A1);
                AL.Add(new Point((int)Temp1, (int)Temp2));

                A1 = A - RJ;
                Temp1 = X2 + r * Math.Cos(A1);
                Temp2 = Y2 + r * Math.Sin(A1);

                AL.Add(new Point((int)Temp1, (int)Temp2));
            }
            return AL;
        }


        /// <summary>
        /// 判断一点是否在模型内部
        /// </summary>
        /// <param name="P1"></param>
        /// <returns></returns>
        public bool IsPointInModel(Point P1)
        {
            double Dis = GlobalClass.CalePointDis(P1, Location);
            if (Dis <= CtlGlobalClass.WaveShapeDia)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        // 绘制模型
        public void DrawModel(Graphics g)
        {
            Font drawFont = new Font("Arial", 8);
            SolidBrush drawBrush = new SolidBrush(Color.Red);

            SolidBrush WaveBrush = new SolidBrush(Color.YellowGreen);
            SolidBrush CopyBrush = new SolidBrush(Color.BurlyWood);

            Pen CopyLinePen = new Pen(Color.Red,2);
            Pen WaveLinePen = new Pen(Color.Black, 2);
          
            // 绘制节点说明
            Point SeqLocation = new Point(Location.X - CtlGlobalClass.WaveShapeDia/2-20,Location.Y - CtlGlobalClass.WaveShapeDia/2-20);
            g.DrawString(SeqName, drawFont, drawBrush, SeqLocation);

            // 绘制节点
            if (this.IsWaveNode)
            {
                g.FillEllipse(WaveBrush, new Rectangle(Location.X - CtlGlobalClass.WaveShapeDia / 2, Location.Y - CtlGlobalClass.WaveShapeDia / 2, CtlGlobalClass.WaveShapeDia, CtlGlobalClass.WaveShapeDia));
            }
            else
            {
                g.FillEllipse(CopyBrush, new Rectangle(Location.X - CtlGlobalClass.WaveShapeDia / 2, Location.Y - CtlGlobalClass.WaveShapeDia / 2, CtlGlobalClass.WaveShapeDia, CtlGlobalClass.WaveShapeDia));
            }



            // 到各个Copy节点的箭头
            for (int i = 0; i < CopyChild.Count; i++)
            {
                ArrayList AL = GetLineAndCircleJD(Location, ((ModelClass)CopyChild[i]).Location, CtlGlobalClass.WaveShapeDia / 2);
                Point JDPoint1 = (Point)AL[0];
                Point JDPoint2 = (Point)AL[1];

                g.DrawLine(WaveLinePen, JDPoint1, JDPoint2);

                AL = GetArrowPoint(Location, JDPoint2, 6);

                Point JDPoint3 = (Point)AL[0];
                Point JDPoint4 = (Point)AL[1];

                g.DrawLine(WaveLinePen, JDPoint2, JDPoint3);
                g.DrawLine(WaveLinePen, JDPoint2, JDPoint4);
            }

            // 到各个Wave节点的箭头
            for (int i = 0; i < WaveChild.Count; i++)
            {
                ArrayList AL  = GetLineAndCircleJD(Location, ((ModelClass)WaveChild[i]).Location, CtlGlobalClass.WaveShapeDia / 2);
                Point JDPoint1 = (Point)AL[0];
                Point JDPoint2 = (Point)AL[1];

                g.DrawLine(WaveLinePen, JDPoint1, JDPoint2);

                AL = GetArrowPoint(Location, JDPoint2, 12);

                Point JDPoint3 = (Point)AL[0];
                // g.FillEllipse(new SolidBrush(Color.Black), new Rectangle(JDPoint.X - 3, JDPoint.Y - 3, 6, 6));

                Point JDPoint4 = (Point)AL[1];

                g.DrawLine(WaveLinePen, JDPoint2, JDPoint3);
                g.DrawLine(WaveLinePen, JDPoint2, JDPoint4);

            }

            Pen SelectPen = new Pen(Color.Red,2);

            if (this.IsSelected)
            {
                g.DrawEllipse(SelectPen, new Rectangle(Location.X - CtlGlobalClass.WaveShapeDia / 2, Location.Y - CtlGlobalClass.WaveShapeDia / 2, CtlGlobalClass.WaveShapeDia, CtlGlobalClass.WaveShapeDia));
            }
           
        }
    }
}
