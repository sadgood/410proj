using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WaveRelationControl
{
    public class CtlGlobalClass
    {
        /// <summary>
        /// 每一形状的实际直径值
        /// </summary>
        public  static int WaveShapeDia = 40;

        public static Pen ShapePen = new Pen(Color.Red, 2);

        /// <summary>
        /// Wave 链条之间的实际距离
        /// </summary>
        public static int HLayerDis = 140;

        public static int VShapeDis = 80;


        /// <summary>
        ///  上余量
        /// </summary>
        public static int UpBlank = 80;

        /// <summary>
        /// 下余量
        /// </summary>
        public static int DownBlank = 20;


        /// <summary>
        /// 左余量
        /// </summary>
        public static int LeftBlank = 30;


        /// <summary>
        /// 右余量
        /// </summary>
        public static int RightBlank = 30;

    } 
}
