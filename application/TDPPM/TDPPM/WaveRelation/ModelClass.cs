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
        /// ģ��ID  ��Ϊģ�͵�Ψһʶ�����
        /// </summary>
        private int _ModelID;

        public int ModelID
        {
            get { return _ModelID; }
            set { _ModelID = value; }
        }

 
        private string _ModelFileName;
        /// <summary>
        /// ģ���ļ���
        /// </summary>
        public string ModelFileName
        {
            get { return _ModelFileName; }
        }
        private string _SeqName;

        /// <summary>
        /// ��������
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
        /// ��ͼλ����Ϣ
        /// </summary>
        public Point Location = new Point(0,0);



        /// <summary>
        /// ��ģ�͵ĸ��ڵ��б�
        /// </summary>
        private ArrayList _ParentNodeList;

        public ArrayList ParentNodeList
        {
            get { return _ParentNodeList; }
            set { _ParentNodeList = value; }
        }

        /// <summary>
        /// �Ƿ���Wave�Ľڵ�
        /// </summary>
        private bool _IsWaveNode = true;

        public bool IsWaveNode
        {
            get { return _IsWaveNode; }
            set { _IsWaveNode = value; }
        }

        /// <summary>
        /// ���ڼ�¼Wave �����ĺ���
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
        /// ��ȡM1���ģ��֮��Ĺ�ϵ
        /// </summary>
        /// <param name="M1">�ⲿģ��</param>
        /// <returns>0  û�й�ϵ
        ///          1  Wave ���ӹ�ϵ
        ///          2  Copy ���ӹ�ϵ
        ///          3  M1 �� ��ǰģ�͵ĸ���</returns>
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
        /// ���ڳ�ʼ��ģ����
        /// </summary>
        /// <param name="MName">ģ���ļ���</param>
        /// <param name="SName">��������</param>
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
        /// ���ø��ڵ� 
        /// </summary>
        /// <param name="PNode"></param>
        public void AddParentNode(ModelClass PNode)
        {
            _ParentNodeList.Add(PNode);
        }
       

        /// <summary>
        /// ��� Wave �ӽڵ� 
        /// </summary>
        /// <param name="CNode"></param>
        public void AddWaveChild(ModelClass CNode)
        {
            _WaveChild.Add(CNode);
            CNode.AddParentNode(this);
        }

        /// <summary>
        /// ���Copy  �ӽڵ�
        /// </summary>
        /// <param name="CNode"></param>
        public void AddCopyChild(ModelClass CNode)
        {
            _CopyChild.Add(CNode);
            CNode.AddParentNode(this);
            CNode.IsWaveNode = false;
        }

        /// <summary>
        /// ���������ߵĽ���
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

        // �����ͷ������
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
        /// �ж�һ���Ƿ���ģ���ڲ�
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


        // ����ģ��
        public void DrawModel(Graphics g)
        {
            Font drawFont = new Font("Arial", 8);
            SolidBrush drawBrush = new SolidBrush(Color.Red);

            SolidBrush WaveBrush = new SolidBrush(Color.YellowGreen);
            SolidBrush CopyBrush = new SolidBrush(Color.BurlyWood);

            Pen CopyLinePen = new Pen(Color.Red,2);
            Pen WaveLinePen = new Pen(Color.Black, 2);
          
            // ���ƽڵ�˵��
            Point SeqLocation = new Point(Location.X - CtlGlobalClass.WaveShapeDia/2-20,Location.Y - CtlGlobalClass.WaveShapeDia/2-20);
            g.DrawString(SeqName, drawFont, drawBrush, SeqLocation);

            // ���ƽڵ�
            if (this.IsWaveNode)
            {
                g.FillEllipse(WaveBrush, new Rectangle(Location.X - CtlGlobalClass.WaveShapeDia / 2, Location.Y - CtlGlobalClass.WaveShapeDia / 2, CtlGlobalClass.WaveShapeDia, CtlGlobalClass.WaveShapeDia));
            }
            else
            {
                g.FillEllipse(CopyBrush, new Rectangle(Location.X - CtlGlobalClass.WaveShapeDia / 2, Location.Y - CtlGlobalClass.WaveShapeDia / 2, CtlGlobalClass.WaveShapeDia, CtlGlobalClass.WaveShapeDia));
            }



            // ������Copy�ڵ�ļ�ͷ
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

            // ������Wave�ڵ�ļ�ͷ
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
