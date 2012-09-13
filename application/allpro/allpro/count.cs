using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NXOpen;
using NXOpen.BlockStyler;
using NXOpen.UF;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
public class count
{

    public bool countcircle(NXOpen.Annotations.Dimension[] zeng, NXOpen.Annotations.Dimension[] jian, out double a, out double b)//这个函数是进行尺寸链校核计算的主方法
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
            zengmax = getspec(zenghuan)[0] + getspec(zenghuan)[1] + zengmax;
            zengmin = getspec(zenghuan)[0] + getspec(zenghuan)[2] + zengmin;
        }
        foreach (NXOpen.Annotations.Dimension jianhuan in jian)
        {
            jianmax = getspec(jianhuan)[0] + getspec(jianhuan)[1] + jianmax;
            jianmin = getspec(jianhuan)[0] + getspec(jianhuan)[2] + jianmin;
        }

        //clsmax = getspec(fengbi)[0] + getspec(fengbi)[1];
        //clsmin = getspec(fengbi)[0] + getspec(fengbi)[2];
        aclsmax = zengmax - jianmin;
        aclsmin = zengmin - jianmax;
        a = aclsmax;
        b = aclsmin;
        //a = aclsmax - getspec(fengbi)[0];
        //a = aclsmax - getspec(fengbi)[0];
        //b = getspec(fengbi)[0] - aclsmin;
        //b = aclsmin - getspec(fengbi)[0];
        //b = aclsmin - getspec(fengbi)[0];
        //if (aclsmax >= clsmax && aclsmin <= clsmin)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;

        //}
        if (b <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool countcircle(NXOpen.Annotations.Dimension fengbi,NXOpen.Annotations.Dimension[] zeng, NXOpen.Annotations.Dimension[] jian, out double a, out double b)//这个函数是进行尺寸链校核计算的主方法
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
            zengmax = getspec(zenghuan)[0] + getspec(zenghuan)[1] + zengmax;
            zengmin = getspec(zenghuan)[0] + getspec(zenghuan)[2] + zengmin;
        }
        foreach (NXOpen.Annotations.Dimension jianhuan in jian)
        {
            jianmax = getspec(jianhuan)[0] + getspec(jianhuan)[1] + jianmax;
            jianmin = getspec(jianhuan)[0] + getspec(jianhuan)[2] + jianmin;
        }

        clsmax = getspec(fengbi)[0] + getspec(fengbi)[1];
        clsmin = getspec(fengbi)[0] + getspec(fengbi)[2];
        aclsmax = zengmax - jianmin;
        aclsmin = zengmin - jianmax;

        decimal aa = Convert.ToDecimal(aclsmax) - Convert.ToDecimal(getspec(fengbi)[0]);
        decimal bb = Convert.ToDecimal(aclsmin) - Convert.ToDecimal(getspec(fengbi)[0]);
        a = Convert.ToDouble(aa);
        b = Convert.ToDouble(bb);
        if (aclsmax >= clsmax && aclsmin <= clsmin)
        {
            return true;
        }
        else
        {
            return false;

        }
        //if (b <= 0)
        //{
        //    return false;
        //}
        //else
        //{
        //    return true;
        //}
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
    public void settolup(NXOpen.Annotations.Dimension dim, double up)//这个方法用来设定一个尺寸的上下公差
    {

        //dim.LowerMetricToleranceValue = up;
        dim.UpperMetricToleranceValue = up;
    }
    public void settoldown(NXOpen.Annotations.Dimension dim, double down)//这个方法用来设定一个尺寸的上下公差
    {
        //dim.UpperMetricToleranceValue = up;
        dim.LowerMetricToleranceValue = down;
    }
    public  void SetDimensionTolerance(NXOpen.Annotations.Dimension dimension, double upper, double lower)
    {
        Session theSession = Session.GetSession();
        Part workPart = theSession.Parts.Work;
        Part displayPart = theSession.Parts.Display;
        NXOpen.Session.UndoMarkId markId1;
        markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");
        NXOpen.Annotations.Value lowerToleranceMm1;
        lowerToleranceMm1.ItemValue = lower;
        Expression nullExpression = null;
        lowerToleranceMm1.ValueExpression = nullExpression;
        lowerToleranceMm1.ValuePrecision = 3;
        NXOpen.Annotations.Value upperToleranceMm1;
        upperToleranceMm1.ItemValue = upper;
        upperToleranceMm1.ValueExpression = nullExpression;
        upperToleranceMm1.ValuePrecision = 3;
        Object tol = ReflectFun(dimension, "GetTolerance");
        Type type = tol.GetType();
        if (type.Name == "LinearTolerance")
        {
            ReflectFun(tol, "SetLowerToleranceMm", lowerToleranceMm1);
            ReflectFun(tol, "SetUpperToleranceMm", upperToleranceMm1);
        }
        else
        {
            ReflectFun(tol, "SetLowerToleranceDegrees", lowerToleranceMm1);
            ReflectFun(tol, "SetUpperToleranceDegrees", upperToleranceMm1);
        }
        //设置公差类型
        if (upper + lower == 0)
        {
            ReflectSetProperty(tol, "ToleranceType", NXOpen.Annotations.ToleranceType.BilateralOneLine);
        }
        else if (lower == 0)
        {
            ReflectSetProperty(tol, "ToleranceType", NXOpen.Annotations.ToleranceType.UnilateralAbove);
        }
        else if (upper == 0)
        {
            ReflectSetProperty(tol, "ToleranceType", NXOpen.Annotations.ToleranceType.UnilateralBelow);
        }
        else
        {
            ReflectSetProperty(tol, "ToleranceType", NXOpen.Annotations.ToleranceType.BilateralTwoLines);
        }
        ReflectFun(dimension, "SetTolerance", tol);

        //设置公差的显示大小
        NXOpen.Annotations.LetteringPreferences letteringPreferences1;
        letteringPreferences1 = dimension.GetLetteringPreferences();
        NXOpen.Annotations.Lettering toleranceText1 = letteringPreferences1.GetDimensionText();
        if (upper + lower != 0)
            toleranceText1.Size = 0.57 * toleranceText1.Size;
        letteringPreferences1.SetToleranceText(toleranceText1);
        dimension.SetLetteringPreferences(letteringPreferences1);
        int nErrs2 = theSession.UpdateManager.DoUpdate(markId1);
    }
    public static object ReflectFun(object ob, string funName)
    {
        Type type = ob.GetType();
        MethodInfo mi = type.GetMethod(funName);
        return mi.Invoke(ob, null);
    }
    public static object ReflectFun(object ob, string funName, object parm)
    {
        Type type = ob.GetType();
        MethodInfo mi = type.GetMethod(funName);
        object[] parms = new object[1];
        parms[0] = parm;
        return mi.Invoke(ob, parms);
    }
    /// <summary>
    /// 这个多参的貌似有问题啊！
    /// </summary>
    /// <param name="ob"></param>
    /// <param name="funName"></param>
    /// <param name="parm1"></param>
    /// <param name="parm2"></param>
    /// <param name="parm3"></param>
    public static void ReflectFun(object ob, string funName, object parm1, object parm2, object parm3)
    {
        Type type = ob.GetType();
        MethodInfo mi = type.GetMethod(funName);
        object[] parms = new object[3];
        parms[0] = parm1;
        parms[1] = parm2;
        parms[2] = parm3;
        mi.Invoke(ob, parms);
        //mi.Invoke()
    }
    public static object ReflectGetProperty(object ob, string funName)
    {
        Type type = ob.GetType();
        PropertyInfo pi = type.GetProperty(funName);
        return pi.GetValue(ob, null);
    }
    public static void ReflectSetProperty(object ob, string funName, object parm)
    {
        Type type = ob.GetType();
        PropertyInfo pi = type.GetProperty(funName);
        pi.SetValue(ob, parm, null);
    }


 
}