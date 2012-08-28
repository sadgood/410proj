using System;
using NXOpen;
using NXOpen.BlockStyler;
using NXOpenUI;
using NXOpen.UF;
using System.Collections;
using System.Collections.Generic;
public class count
{

    public bool countcircle(NXOpen.Annotations.Dimension[] zeng, NXOpen.Annotations.Dimension[] jian, NXOpen.Annotations.Dimension fengbi, out double a, out double b)//这个函数是进行尺寸链校核计算的主方法
    {
        double zengmax = 0;//所有增环的最大尺寸之和
        double zengmin = 0;//所有增环的最小尺寸之和
        double jianmax = 0;//所有减环的最大尺寸之和
        double jianmin = 0;//所有减环的最小尺寸之和
        double clsmax = 0;//封闭环最大尺寸
        double clsmin = 0;//封闭环最小尺寸
        a = 0;//最大上公差
        b = 0;//最小下公差
        double aclsmax = 0;//所有 增环最大尺寸减去所有减环最小极限尺寸
        double aclsmin = 0;//所有增环最小尺寸减去所有减环最大尺寸
        foreach (NXOpen.Annotations.Dimension zenghuan in zeng)
        {
            zengmax = getspec(zenghuan)[0] + getspec(zenghuan)[1];
            zengmin = getspec(zenghuan)[0] + getspec(zenghuan)[2];
        }
        foreach (NXOpen.Annotations.Dimension jianhuan in jian)
        {
            jianmax = getspec(jianhuan)[0] + getspec(jianhuan)[1];
            jianmin = getspec(jianhuan)[0] + getspec(jianhuan)[2];
        }

        clsmax = getspec(fengbi)[0] + getspec(fengbi)[1];
        clsmin = getspec(fengbi)[0] + getspec(fengbi)[2];
        aclsmax = zengmax - jianmin;
        aclsmin = zengmin - jianmax;
        a = aclsmax - getspec(fengbi)[0];
        b = aclsmin - getspec(fengbi)[0];
        if (aclsmax >= clsmax && aclsmin <= clsmin)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public void settol(NXOpen.Annotations.Dimension dim, double up, double down)//这个方法用来设定一个尺寸的上下公差
    {
        dim.UpperMetricToleranceValue = up;
        dim.LowerMetricToleranceValue = down;
    }
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