using System;
using System.Collections.Generic;
using System.Text;


    class pubfun
    {
        public double[] getspec(NXOpen.Annotations.Dimension dim)//返回一个尺寸的名义值和上下公差，第一个值是名义值，第二个是上公差，第三个是下公差
        {
            string[] a;
            string[] b;
            double maindim = 0;
            double low = 0;
            double up = 0;
            double[] final = { 0, 0, 0 };
            dim.GetDimensionText(out a, out b);
            maindim = double.Parse(a[0]);
            up = dim.UpperMetricToleranceValue;
            low = dim.LowerMetricToleranceValue;
            final[0] = maindim;
            final[1] = up;
            final[2] = low;
            return final;
        }
    }

