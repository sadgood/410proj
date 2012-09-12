using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NXOpen;
using NXOpen.UF;

    public partial class form1 : Form
    {
        NXOpen.Annotations.Dimension fengbi = null;
        NXOpen.Annotations.Dimension[] zenghuan = null;
        NXOpen.Annotations.Dimension[] jianhuan = null;
        string inup = null;
        string indown = null;
        double inupdou = 0;
         double indowndou = 0;
        public form1(NXOpen.Annotations.Dimension fengbihuan,NXOpen.Annotations.Dimension[] zeng,NXOpen.Annotations.Dimension[] jian)
        {
            InitializeComponent();
            fengbi = fengbihuan;
            zenghuan = zeng;
            jianhuan = jian;
        }
        public form1(NXOpen.Annotations.Dimension[] zeng, NXOpen.Annotations.Dimension[] jian,NXOpen.Annotations.Dimension fengbihuan,string up,string down)
        {
            InitializeComponent();
            zenghuan = zeng;
            jianhuan = jian;
            inup = up;
            indown = down;
            fengbi = fengbihuan;
        }
        public form1(NXOpen.Annotations.Dimension[] zeng, NXOpen.Annotations.Dimension[] jian, NXOpen.Annotations.Dimension fengbihuan, double up, double down)
        {
            InitializeComponent();
            zenghuan = zeng;
            jianhuan = jian;
            inupdou = up;
            indowndou = down;
            fengbi = fengbihuan;
        }
        public form1()
        {
            InitializeComponent();
        }
        public Pen thepen = new Pen(Color.Black);
        public int q = 0;
        pubfun thepubfun = new pubfun();

        private void 尺寸链二维展示窗口_Load(object sender, EventArgs e)
        {

        }
        public void drawfengbi(NXOpen.Annotations.Dimension fengbi)
        {
            double[] fengbiinfo = null;
            fengbiinfo = thepubfun.getspec(fengbi);
            q = Convert.ToInt32((fengbiinfo[0] + 100));
            System.Drawing.Point pt1 = new System.Drawing.Point(100, 50);
            System.Drawing.Point pt2 = new System.Drawing.Point(q, 50);
            Graphics drawfb = panel1.CreateGraphics();
            drawfb.DrawLine(thepen, pt1, pt2);
            drawduanxian(pt1, pt2);
            drawdim(fengbi, pt1, pt2);
        }
        public void drawfengbi(string uup,string ddown,NXOpen.Annotations.Dimension fengbi)
        {
            double[] fengbiinfo = null;
            fengbiinfo = thepubfun.getspec(fengbi);
            q = Convert.ToInt32((fengbiinfo[0] + 100));
            System.Drawing.Point pt1 = new System.Drawing.Point(100, 50);
            System.Drawing.Point pt2 = new System.Drawing.Point(q, 50);
            Graphics drawfb = panel1.CreateGraphics();
            drawfb.DrawLine(thepen, pt1, pt2);
            drawduanxian(pt1, pt2);
            //drawdim(inup,indown,pt1,pt2);
            drawdim(inupdou, indowndou, fengbi, pt1, pt2);
        }
        public void drawfengbi(double inupdou, double indowndou, NXOpen.Annotations.Dimension fengbi)
        {
            double[] fengbiinfo = null;
            fengbiinfo = thepubfun.getspec(fengbi);
            q = Convert.ToInt32((fengbiinfo[0] + 100));
            System.Drawing.Point pt1 = new System.Drawing.Point(100, 50);
            System.Drawing.Point pt2 = new System.Drawing.Point(q, 50);
            Graphics drawfb = panel1.CreateGraphics();
            drawfb.DrawLine(thepen, pt1, pt2);
            drawduanxian(pt1, pt2);
            //drawdim(inup,indown,pt1,pt2);
            drawdim(inupdou, indowndou, fengbi, pt1, pt2);
        }
        public void drawdim(string inup,string indown, System.Drawing.Point pt1, System.Drawing.Point pt2)//这个drawdim是用来在多工序间进行校核的时候
        {
            double[] fengbiinfo = null;
            fengbiinfo = thepubfun.getspec(fengbi);
            Graphics drawstr = panel1.CreateGraphics();
            System.Drawing.Point strpoint = new System.Drawing.Point();
            strpoint.X = Convert.ToInt32((pt1.X + pt2.X) / 2.5);
            strpoint.Y = (pt1.Y - 20);
            drawstr.DrawString(inup + "/" + indown, new Font("Verdana", 10), new SolidBrush(Color.Black), strpoint);
          
        }
        public void drawdim(NXOpen.Annotations.Dimension dim, System.Drawing.Point pt1, System.Drawing.Point pt2)//这个是正常的画线函数
        {
            Graphics drawstr = panel1.CreateGraphics();
            double[] info = thepubfun.getspec(dim);

            System.Drawing.Point strpoint = new System.Drawing.Point();
            strpoint.X = Convert.ToInt32((pt1.X + pt2.X) / 2.2);
            strpoint.Y = (pt1.Y - 20);
            drawstr.DrawString(info[0].ToString(), new Font("Verdana", 10), new SolidBrush(Color.Black), strpoint);
            System.Drawing.Point upstrpoint = new System.Drawing.Point();
            upstrpoint.X = strpoint.X + 25;
            upstrpoint.Y = strpoint.Y;
            drawstr.DrawString(info[1].ToString(), new Font("Verdana", 5), new SolidBrush(Color.Black), upstrpoint);
            System.Drawing.Point downstrpoint = new System.Drawing.Point();
            downstrpoint.X = strpoint.X + 22;
            downstrpoint.Y = strpoint.Y + 10;
            drawstr.DrawString(info[2].ToString(), new Font("Verdana", 5), new SolidBrush(Color.Black), downstrpoint);

        }
        public void drawdim(double inupdou, double indowndou,NXOpen.Annotations.Dimension fengbi, System.Drawing.Point pt1, System.Drawing.Point pt2)//这个drawdim是用来在多工序间进行校核的时候
        {
            Graphics drawstr = panel1.CreateGraphics();
            double[] fengbiinfo = null;
            fengbiinfo = thepubfun.getspec(fengbi);
            decimal fengbiinfo1 = 0;
            decimal fengbiinfo2 = 0;
       
             fengbiinfo1 =Convert.ToDecimal(inupdou) - Convert.ToDecimal( fengbiinfo[0]);
            fengbiinfo2 =Convert.ToDecimal( indowndou) - Convert.ToDecimal(fengbiinfo[0]);
           

        //fengbiinfo[1] = fengbiinfo1;
        //fengbiinfo[2] = fengbiinfo2;
        System.Drawing.Point strpoint = new System.Drawing.Point();
        strpoint.X = Convert.ToInt32((pt1.X + pt2.X) / 2.2);
        strpoint.Y = (pt1.Y - 20);
        drawstr.DrawString(fengbiinfo[0].ToString(), new Font("Verdana", 10), new SolidBrush(Color.Black), strpoint);
        System.Drawing.Point upstrpoint = new System.Drawing.Point();
        upstrpoint.X = strpoint.X + 25;
        upstrpoint.Y = strpoint.Y;
        drawstr.DrawString(fengbiinfo1.ToString() , new Font("Verdana", 5), new SolidBrush(Color.Black), upstrpoint);
        System.Drawing.Point downstrpoint = new System.Drawing.Point();
        downstrpoint.X = strpoint.X + 22;
        downstrpoint.Y = strpoint.Y + 10;
        drawstr.DrawString(fengbiinfo2.ToString(), new Font("Verdana", 5), new SolidBrush(Color.Black), downstrpoint);

        }
        public void drawduanxian(System.Drawing.Point pt1, System.Drawing.Point pt2)
        {
            System.Drawing.Point pt1up = new System.Drawing.Point(pt1.X, (pt1.Y - 15));
            System.Drawing.Point pt1down = new System.Drawing.Point(pt1.X, (pt1.Y + 15));
            System.Drawing.Point pt2up = new System.Drawing.Point(pt2.X, (pt2.Y - 15));
            System.Drawing.Point pt2down = new System.Drawing.Point(pt2.X, (pt2.Y + 15));
            Graphics draw = panel1.CreateGraphics();
            draw.DrawLine(thepen, pt1up, pt1down);
            draw.DrawLine(thepen, pt2up, pt2down);
        }
        public void drawzenghuan(NXOpen.Annotations.Dimension[] zenghuan)
        {
            double[] zenghuaninfo = null;
            int m = 100;
            System.Drawing.Point pt1 = new System.Drawing.Point(100, 100);//这是起始点，从这里开始画，对应于封闭环
            System.Drawing.Point pt2 = new System.Drawing.Point();
            foreach (NXOpen.Annotations.Dimension dim in zenghuan)
            {
                zenghuaninfo = thepubfun.getspec(dim);
                pt2 = new System.Drawing.Point(Convert.ToInt32((zenghuaninfo[0] + m)), 100);
                Graphics drawfb = panel1.CreateGraphics();
                drawfb.DrawLine(new Pen(Color.Green), pt1, pt2);
                drawduanxian(pt1, pt2);
                m = Convert.ToInt32((zenghuaninfo[0] + m));
                drawdim(dim, pt1, pt2);
                pt1 = pt2;
            }

        }
     

        public void drawjianhuan(NXOpen.Annotations.Dimension[] jianhuan)
        {
            double[] jianhuaninfo = null;
            int m = 0;
            System.Drawing.Point pt1 = new System.Drawing.Point(q, 50);//这是起始点，从这里开始画，对应于封闭环
            System.Drawing.Point pt2 = new System.Drawing.Point();
            foreach (NXOpen.Annotations.Dimension dim in jianhuan)
            {
                jianhuaninfo = thepubfun.getspec(dim);
                //pt2 = new System.Drawing.Point(Convert.ToInt32((jianhuaninfo[0] + q)) + m, 50);
                pt2.Y = pt1.Y;
                pt2.X = pt1.X + Convert.ToInt32(jianhuaninfo[0]);
                Graphics drawfb = panel1.CreateGraphics();
                drawfb.DrawLine(new Pen(Color.Red), pt1, pt2);
                drawduanxian(pt1, pt2);
                m = Convert.ToInt32((jianhuaninfo[0] + q)) + m;
                drawdim(dim, pt1, pt2);
                pt1 = pt2;

            }

        }

        public void dd()
        {
            SolidBrush Brush_tilte = new SolidBrush(Color.Red);
            SolidBrush Brush_front = new SolidBrush(Color.Black);
            System.Drawing.Graphics g = panel1.CreateGraphics();
            Font drawFont = new Font("Arial", 8);
            System.Drawing.Point titleLocation = new System.Drawing.Point(6, 6);
            g.DrawString("尺寸链校核", drawFont, Brush_tilte, titleLocation);
            Pen pg = new Pen(Color.Green, 2);
            g.DrawLine(pg, 520, 10, 538, 10);
            Pen pr = new Pen(Color.Red, 2);
            g.DrawLine(pr, 520, 22, 538, 22);
            System.Drawing.Point LineLocation1 = new System.Drawing.Point(550, 5);
            g.DrawString("增环", drawFont, Brush_front, LineLocation1);
            System.Drawing.Point LineLocation2 = new System.Drawing.Point(550, 17);
            g.DrawString("减环", drawFont, Brush_front, LineLocation2);
 
            pg.Dispose();
            pr.Dispose();
            
            Brush_tilte.Dispose();
            Brush_front.Dispose();
        }
        private void form1_Load(object sender, EventArgs e)
        {
         
        }

        private void button1_Click(object sender, EventArgs e)
        { 
              drawfengbi(inupdou, indowndou, fengbi);
              drawzenghuan(zenghuan);
              drawjianhuan(jianhuan);
              dd();
              this.Show();
        }
    }

