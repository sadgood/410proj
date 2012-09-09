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
        public form1(NXOpen.Annotations.Dimension fengbihuan,NXOpen.Annotations.Dimension[] zeng,NXOpen.Annotations.Dimension[] jian)
        {
            InitializeComponent();
            fengbi = fengbihuan;
            zenghuan = zeng;
            jianhuan = jian;
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
                pt2 = new System.Drawing.Point(Convert.ToInt32((jianhuaninfo[0] + q)) + m, 50);
                Graphics drawfb = panel1.CreateGraphics();
                drawfb.DrawLine(new Pen(Color.Red), pt1, pt2);
                drawduanxian(pt1, pt2);
                m = Convert.ToInt32((jianhuaninfo[0] + q)) + m;
                pt1 = pt2;

            }

        }

        private void form1_Load(object sender, EventArgs e)
        {
          
           
        }

        private void button1_Click(object sender, EventArgs e)
        {

          
            drawfengbi(fengbi);
            drawzenghuan(zenghuan);
            drawjianhuan(jianhuan);
        }
    }

