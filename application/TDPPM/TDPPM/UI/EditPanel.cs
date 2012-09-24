using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using TDPPM;

namespace TDPPM
{
    /// <summary>
    /// Tab控件中，各个组件的基类
    /// </summary>
    public class EditPanel:UserControl
    {
        public string ProPath = null;       //工程路径
        public string ProName = null;       //工程名
        public string XmlFile = null;       //工艺xml文件
        public string TemplateXML = NXFun.TDPPMPath + NXFun.TemplateXML; //工艺xml模板
        public MainDlg mainDlg = null;      //主窗口
        public bool IsEdit = false;         //是否为编辑状态

        //转换全路径
        public string ToFullPath(string filename)
        {
            return ProPath + "\\" + ProName + "\\" + filename;
        }
        //从全路径得到文件名称,如果不在工艺下返回全路径
        /// <summary>
        /// 从全路径得到文件名称,如果不在工艺下返回全路径
        /// </summary>
        /// <param name="fullpath">文件全路径</param>
        /// <returns>名称或全路径</returns>
        public string ToFileName(string fullpath)
        {
            if (fullpath.Contains(ProPath))
            {
                return Path.GetFileName(fullpath);
            }
            else
            {
                return fullpath;
            }
        }
        //设置StatusLabel
        /// <summary>
        /// 设置StatusLabel
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="color">颜色 0  黑色  1  红色  2  绿色 ……</param>
        public void SetStatusLabel(string text, int color)
        {
            mainDlg.SetStatusLabel(text, color);
        }
    }
}
