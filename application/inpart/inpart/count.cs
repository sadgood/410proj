using System;
using NXOpen;
using NXOpen.BlockStyler;
using NXOpenUI;
using NXOpen.UF;
using System.Collections;
using System.Collections.Generic;
public class count
{
    //public double zengcountlow = 0;//存储增环的下公差的变量
    //public double zengcountup = 0;//存储增环的上公差的变量
    //public double jiancountlow = 0;//存储减环的下公差的变量
    //public double jiancountup = 0;//存储减环上公差的变量
    
    public void countcircle(NXOpen.Annotations.Dimension[] zeng,NXOpen.Annotations.Dimension[] jian,NXOpen.Annotations.Dimension fengbi)//这个函数是进行尺寸链校核计算的主方法
    {
       double zengmax = 0;//所有增环的最大尺寸之和
       double zengmin = 0;//所有增环的最小尺寸之和
       double jianmax = 0;//所有减环的最大尺寸之和
       double jianmin = 0;//所有减环的最小尺寸之和
       double clsmax = 0;//封闭环最大尺寸
       double clsmin =0;//封闭环最小尺寸
       double aclsmax = 0;//所有 增环最大尺寸减去所有减环最小极限尺寸
       double aclsmin = 0;//所有增环最小尺寸减去所有减环最大尺寸
        foreach (NXOpen.Annotations.Dimension zenghuan in zeng)
        {
            zengmax = getspec(zenghuan)[0] + getspec(zenghuan)[1];
            zengmin = getspec(zenghuan)[0] + getspec(zenghuan)[2];
        }
        foreach(NXOpen.Annotations.Dimension jianhuan in jian)
        {
            jianmax = getspec(jianhuan)[0] + getspec(jianhuan)[1];
            jianmin = getspec(jianhuan)[0] + getspec(jianhuan)[2];
        }
     
        clsmax = getspec(fengbi)[0] + getspec(fengbi)[1];
        clsmin = getspec(fengbi)[0] + getspec(fengbi)[2];
        aclsmax = zengmax - jianmin;
        aclsmin = zengmin - jianmin;
     if(aclsmax <= clsmax && aclsmin >= clsmin)
     {
         
     }

    }
    public double[] getspec(NXOpen.Annotations.Dimension dim)//返回一个尺寸的名义值和上下公差，第一个值是名义值，第二个是上公差，第三个是下公差
    {
         string[] a;
         string[] b;
         double maindim = 0;
         double low = 0;
         double up = 0;
         double[] final = {0,0,0};
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