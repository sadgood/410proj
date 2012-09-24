using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WaveRelationControl
{
    public class GlobalClass
    {
        public static int GlobalID = 1;

        /// <summary>
        /// 计算两点之间的距离
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <returns></returns>
        public static double CalePointDis(Point P1, Point P2)
        {
            double X1 = P1.X;
            double Y1 = P1.Y;

            double X2 = P2.X;
            double Y2 = P2.Y;

            double dx = X2 - X1;
            double dy = Y2 - Y1;

            double L = Math.Sqrt(dx * dx + dy * dy);

            return L;
        }
    }
}
