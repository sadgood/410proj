using System;
using NXOpen;
using NXOpen.BlockStyler;
using NXOpenUI;
using NXOpen.UF;
using System.Collections;
using System.Collections.Generic;
public class count
{
    public double zengcountlow = 0;//存储增环的下公差的变量
    public double zengcountup = 0;//存储增环的上公差的变量
    public double jiancountlow = 0;//存储减环的下公差的变量
    public double jiancountup = 0;//存储减环上公差的变量
    public void countcircle(NXOpen.Annotations.Dimension[] zeng,NXOpen.Annotations.Dimension[] jian,NXOpen.Annotations.Dimension fengbi)//这个函数是进行尺寸链校核计算的主方法
    {
        foreach (NXOpen.Annotations.Dimension zenghuan in zeng)
        {
            zengcountlow = zenghuan.LowerToleranceValue + zengcountlow;
            zengcountup = zenghuan.UpperToleranceValue + zengcountup;
         
        }
        foreach(NXOpen.Annotations.Dimension jianhuan in jian)
        {
            jiancountlow = jiancountlow + jianhuan.LowerToleranceValue;
            jiancountup = jiancountup + jianhuan.LowerToleranceValue;
        
        }
    }

}