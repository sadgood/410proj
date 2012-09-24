using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using NXOpen;
using TDPPM;

class XML3DPPM
{
    //构造函数
    public XML3DPPM()
    {       
    }
    //得到公差
    /// <summary>
    /// 得到公差
    /// </summary>
    /// <param name="dim">尺寸</param>
    /// <param name="type">类型</param>
    /// <param name="upper">上差</param>
    /// <param name="lower">下差</param>
    /// <param name="xmlfile">公差表xml</param>
    /// <returns>成功否</returns>
    public static bool GetTolerance(double dim, string type, out double upper, out double lower,string xmlfile)
    {
        XmlDocument mydoc = new XmlDocument();
        mydoc.Load(xmlfile);
        XmlNodeList nodes = mydoc.SelectNodes("/Tolerance/Table/Tol");
        foreach (XmlElement xe in nodes)
        {
            if (xe.GetAttribute("type")==type)
            {
                string a = xe.GetAttribute("a");
                string b = xe.GetAttribute("b");
                char a_pre = a[0];
                char b_pre = b[0];
                double a_num = System.Convert.ToDouble(a.Substring(1));
                double b_num = System.Convert.ToDouble(b.Substring(1));
                if (dim > a_num && dim < b_num)
                {
                    upper = System.Convert.ToDouble(xe.GetAttribute("up"));
                    lower = System.Convert.ToDouble(xe.GetAttribute("low"));
                    return true;
                }
                else if (a_pre == 'E' && dim == a_num)
                {
                    upper = System.Convert.ToDouble(xe.GetAttribute("up"));
                    lower = System.Convert.ToDouble(xe.GetAttribute("low"));
                    return true;
                }
                else if (b_pre == 'E' && dim == b_num)
                {
                    upper = System.Convert.ToDouble(xe.GetAttribute("up"));
                    lower = System.Convert.ToDouble(xe.GetAttribute("low"));
                    return true;
                }                     
            }
        }
        upper = lower = 0;
        return false;     

    }
    //得到全部图纸模板信息
    /// <summary>
    /// 得到全部图纸模板信息
    /// </summary>
    /// <param name="xmlfile">图纸模板xml</param>
    /// <returns>图纸模板信息</returns>
    public static List<S_SheetTemplet> GetSheetTempletList(string xmlfile)
    {
        XmlDocument mydoc = new XmlDocument();
        mydoc.Load(xmlfile);
        List<S_SheetTemplet> SheetTempletList = new List<S_SheetTemplet>();
        S_SheetTemplet SheetTemplet;
        XmlNodeList nodes = mydoc.SelectNodes("/SYS_3DPPM/SheetTemplet/ST");
        foreach (XmlElement xe in nodes)
        {
            if (xe.GetAttribute("isShow") == "1")
            {
                SheetTemplet.name = xe.GetAttribute("name");
                SheetTemplet.chinese = xe.GetAttribute("chinese");
                SheetTemplet.filepath = xe.GetAttribute("filepath");
                SheetTemplet.type = xe.GetAttribute("type");
                SheetTempletList.Add(SheetTemplet);
            }
        }
        return SheetTempletList;         
    }
    //得到某个图纸模板信息
    /// <summary>
    /// 得到某个图纸模板信息
    /// </summary>
    /// <param name="name">模板名称</param>
    /// <param name="xmlfile">模板xml</param>
    /// <returns>模板信息</returns>
    public static S_SheetTemplet GetSheetTemplet(string name,string xmlfile)
    {
        XmlDocument mydoc = new XmlDocument();
        mydoc.Load(xmlfile);
        S_SheetTemplet SheetTemplet;
        string path = "/SYS_3DPPM/SheetTemplet/ST[@name='" + name + "']";
        XmlNodeList nodes = mydoc.SelectNodes(path);
        if (nodes.Count ==0)
        {
            throw new Exception("得到图纸模板信息出错！");
        }
        XmlElement xe = (XmlElement)nodes[0];
        SheetTemplet.name = xe.GetAttribute("name");
        SheetTemplet.chinese = xe.GetAttribute("chinese");
        SheetTemplet.filepath = xe.GetAttribute("filepath");
        SheetTemplet.type = xe.GetAttribute("type");
        return SheetTemplet;
    }
    //得到得到Information属性 （0,0）代表工艺  （M,0）代表第M个工序 (M,N)代表第M个工序的第N个工步
    /// <summary>
    /// 得到Information属性 （0,0）代表工艺  （M,0）代表第M个工序 (M,N)代表第M个工序的第N个工步
    /// </summary>
    /// <param name="a">坐标a</param>
    /// <param name="b">坐标b</param>
    /// <param name="title">属性名</param>
    /// <returns>属性值</returns>
    public static string GetIndexAttr(int a,int b,string title,string xmlfile)
    {
        try
        {
            XmlDocument mydoc = new XmlDocument();
            mydoc.Load(xmlfile);
            if (a == 0 && b == 0)
            {
                //工艺Information属性
                XmlElement xe = (XmlElement)mydoc.SelectSingleNode("/SYS_3DPPM/Gongyi/Information");
                return xe.GetAttribute(title);
            }
            else if (a != 0 && b == 0)
            {
                //工序Information属性
                string path = "/SYS_3DPPM/Gongyi/Gongxu[" + a.ToString() + "]/Information";
                XmlElement xe = (XmlElement)mydoc.SelectSingleNode(path);
                return xe.GetAttribute(title);
            }
            else
            {
                //工步Information属性
                string path = "/SYS_3DPPM/Gongyi/Gongxu[" + a.ToString() + "]/Gongbu[" + b.ToString() + "]/Information";
                XmlElement xe = (XmlElement)mydoc.SelectSingleNode(path);
                return xe.GetAttribute(title);
            }
        }
        catch (System.Exception ex)
        {
            UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
            return "";        	
        }       
    }
    //更新旧版工艺XML 
    /// <summary>
    /// 更新旧版工艺XML   0新版不操作   1  旧版更新成功  2 旧版更新失败
    /// </summary>
    /// <param name="xmlfile">旧版工艺XML</param>
    /// <param name="templateXml">新版工艺XML模板</param>
    /// <returns>0新版不操作   1  旧版更新成功  2 旧版更新失败</returns>
    public static int Update3dppm(string xmlfile,string templateXml)
    {
        XmlDocument olddoc = new XmlDocument();
        olddoc.Load(xmlfile);
        XmlNode root = olddoc.SelectSingleNode("/SYS_3DPPM");
        XmlElement xe_root = root as XmlElement;
        string version = xe_root.GetAttribute("version");
        if (version == "2.0")
        {
            //新版
            return 0;
        }        
        try
        {
            XmlNodeList nodes_old = olddoc.SelectNodes("/SYS_3DPPM/Process");
            if (nodes_old.Count == 0)
            {
                return 2;
            }
            //创建xml备份
            string newfile = xmlfile + ".bak";
            File.Copy(NXFun.TDPPMPath + NXFun.ProcessXML, newfile, true);

            XmlDocument newdoc = new XmlDocument();
            newdoc.Load(newfile);
            XmlNode new_gongyi_node = newdoc.SelectSingleNode("/SYS_3DPPM/Gongyi");
            XmlDocument templatedoc = new XmlDocument();
            templatedoc.Load(templateXml);
            XmlNode gongxu_node = templatedoc.SelectSingleNode("/Template/Gongxu[@Type='普通工序']");
            XmlNode gongbu_node = templatedoc.SelectSingleNode("/Template/Gongbu");
            XmlNode zigongbu_node = templatedoc.SelectSingleNode("/Template/Zigongbu");
            XmlNode ylt_node = templatedoc.SelectSingleNode("/Template/YLT");
            
            //添加工艺信息
            string model = ((XmlElement)(nodes_old[0])).GetAttribute("DesignModel");
            XmlNode node = newdoc.SelectSingleNode("/SYS_3DPPM/Gongyi/Model");
            ((XmlElement)node).SetAttribute("filename", model);
            XmlElement xe_old = nodes_old[0].SelectSingleNode("Information") as XmlElement;
            XmlElement xe_new = newdoc.SelectSingleNode("/SYS_3DPPM/Gongyi/Information") as XmlElement;
            foreach(XmlAttribute xa in xe_old.Attributes)
            {
                string name = xa.Name;
                if (xa.Name.Contains("edit_"))
                {
                    name = xa.Name.Substring(xa.Name.IndexOf("_") + 1, xa.Name.Length - xa.Name.IndexOf("_") - 1); 
                }
                xe_new.SetAttribute(name, xa.Value);
            }
            #region 添加工序、工步、子工步信息
            //添加工序信息
            
            nodes_old = olddoc.SelectNodes("/SYS_3DPPM/Process/Procedure");
            foreach (XmlElement xe_gongxu in nodes_old)
            {
                //添加一个工序
                XmlNode newnode = newdoc.ImportNode(gongxu_node, true);
                XmlNode new_gongxu = new_gongyi_node.AppendChild(newnode);
                model = xe_gongxu.GetAttribute("Model");
                ((XmlElement)new_gongxu.SelectSingleNode("Model")).SetAttribute("filename", model);
                XmlElement xe_gongxu_information = new_gongxu.SelectSingleNode("Information") as XmlElement;
                foreach (XmlAttribute xa in xe_gongxu.SelectSingleNode("Information").Attributes)
                {
                    string name = xa.Name;
                    if (xa.Name.Contains("edit_"))
                    {
                        name = xa.Name.Substring(xa.Name.IndexOf("_") + 1, xa.Name.Length - xa.Name.IndexOf("_") - 1);
                    }
                    xe_gongxu_information.SetAttribute(name, xa.Value);
                }
                //添加工步信息
                XmlNodeList nodes_gongbu_old = xe_gongxu.SelectNodes("StepGroup/Step");
                foreach (XmlElement xe_gonbu in nodes_gongbu_old)
                {
                    //添加一个工步
                    XmlNode newnode_gongbu = newdoc.ImportNode(gongbu_node, true);
                    XmlNode new_gongbu = new_gongxu.AppendChild(newnode_gongbu);
                    XmlElement xe_gongbu_information = new_gongbu.SelectSingleNode("Information") as XmlElement;
                    foreach (XmlAttribute xa in xe_gonbu.SelectSingleNode("Information").Attributes)
                    {
                        string name = xa.Name;
                        if (name == "edit_gongbu_gongbuhao")
                        {
                            name = "gongbu_gongbuhao";
                        }
                        else if (name == "edit_gongbu_gongbumingcheng")
                        {
                            name = "gongbu_gongbuneirong";
                        }
                        else if (name == "edit_gongbu_renju")
                        {
                            name = "gongbu_renju";
                        }
                        else if (name == "edit_gongbu_liangju")
                        {
                            name = "gongbu_liangju";
                        }
                        else if (name == "edit_gongbu_beizhu")
                        {
                            name = "gongbu_beizhu";
                        }
                        xe_gongbu_information.SetAttribute(name, xa.Value);
                    }
                    //添加子工步信息
                    XmlNodeList nodes_zigongbu_old = xe_gonbu.SelectNodes("ChildStep/CS");
                    foreach (XmlElement xe_zigonbu in nodes_zigongbu_old)
                    {
                        //添加一个子工步
                        XmlNode newnode_zigongbu = newdoc.ImportNode(gongbu_node, true);
                        XmlNode new_zigongbu = new_gongbu.AppendChild(newnode_zigongbu);
                        foreach (XmlAttribute xa in xe_zigonbu.Attributes)
                        {
                            string name = xa.Name;
                            if (name == "name")
                            {
                                name = "zigongbu_name";
                            }
                            else if (name == "renju")
                            {
                                name = "zigongbu_renju";
                            }
                            else if (name == "liangju")
                            {
                                name = "zigongbu_liangju";
                            }
                            else if (name == "beizhu")
                            {
                                name = "zigongbu_beizhu";
                            }
                            xe_gongbu_information.SetAttribute(name, xa.Value);
                        }
                    }                    
                }
            }
            #endregion 添加工序信息
            //添加余量图
            XmlNodeList nodes_ylt_old = olddoc.SelectNodes("/SYS_3DPPM/MarginMap/YLT");
            XmlNode ylt_root = newdoc.SelectSingleNode("/SYS_3DPPM/Gongyi/YLTs");
            foreach (XmlElement xe_ylt in nodes_ylt_old)
            {
                
                string filename = xe_ylt.GetAttribute("prt");
                if (!string.IsNullOrEmpty(filename))
                {
                    XmlNode newnode_ylt = newdoc.ImportNode(ylt_node, true);
                    XmlElement new_ylt = (XmlElement)ylt_root.AppendChild(newnode_ylt);
                    new_ylt.SetAttribute("filename", filename);
                    new_ylt.SetAttribute("description", xe_ylt.GetAttribute("description"));
                }
                
            }
            newdoc.Save(newfile);
            File.Delete(xmlfile);
            File.Move(newfile, xmlfile);
            return 1;
        }
        catch/* (System.Exception ex)*/
        {
            return 2;
        }
    }
    //删除工序工步节点  （M,0）代表第M个工序 (M,N)代表第M个工序的第N个工步
    /// <summary>
    /// 删除工序工步节点  （M,0）代表第M个工序 (M,N)代表第M个工序的第N个工步
    /// </summary>
    /// <param name="a">坐标a</param>
    /// <param name="b">坐标b</param>
    /// <param name="xmlfile">工艺XML</param>
    /// <returns>成功否</returns>
    public static bool DelNode(int a, int b,string xmlfile)
    {
        try
        {
            XmlDocument mydoc = new XmlDocument();
            mydoc.Load(xmlfile);
            if (a == 0 && b == 0)
            {
                return false;
            }
            else if (a != 0 && b == 0)
            {
                //删除工序
                string path = "/SYS_3DPPM/Gongyi/Gongxu[" + a.ToString() + "]";
                XmlNode node = mydoc.SelectSingleNode(path);
                //移动模型到辅助
                string filename = ((XmlElement)node.SelectSingleNode("Model")).GetAttribute("filename");   
                node.ParentNode.RemoveChild(node);
                UpdateGongxuGongbuhao(mydoc);
                mydoc.Save(xmlfile);
                if (!string.IsNullOrEmpty(filename))
                {
                    SetModelAttr(1, 0, filename, "已删工序模型", xmlfile, NXFun.TDPPMPath + NXFun.TemplateXML);
                }
                return true;
            }
            else
            {
                //删除工步
                string path = "/SYS_3DPPM/Gongyi/Gongxu[" + a.ToString() + "]/Gongbu[" + b.ToString() + "]";
                XmlNode node = mydoc.SelectSingleNode(path);
                node.ParentNode.RemoveChild(node);
                UpdateGongxuGongbuhao(mydoc);
                mydoc.Save(xmlfile);
                return true;
            }
        }
        catch (System.Exception ex)
        {
            NXFun.MessageBox(ex.Message);
            return false;
        }
    }
    //上下移动工序工步节点 （M,0）代表第M个工序 (M,N)代表第M个工序的第N个工步
    /// <summary>
    /// 上下移动工序工步节点  （M,0）代表第M个工序 (M,N)代表第M个工序的第N个工步
    /// </summary>
    /// <param name="a">工序坐标</param>
    /// <param name="b">工步坐标</param>
    /// <param name="isUp">true  上移  false  下移</param>
    /// <returns>成功否</returns>
    public static bool MoveNode(int a, int b,bool isUp,string xmlfile)
    {
        try
        {
            XmlDocument mydoc = new XmlDocument();
            mydoc.Load(xmlfile);
            if (a == 0 && b == 0)
            {
                return false;
            }
            else if (a != 0 && b == 0)
            {
                //移动工序
                string path = "/SYS_3DPPM/Gongyi/Gongxu";
                XmlNodeList nodes = mydoc.SelectNodes(path);
                if (nodes.Count < 2)
                {
                    //不到两个节点，移什么呀？
                    return false;
                }
                if (isUp)
                {
                    //上移
                    if (a == 1)
                    {
                        //工序已置顶
                        return false;
                    }
                    nodes[a - 1].ParentNode.InsertBefore(nodes[a - 1], nodes[a - 2]);                    
                } 
                else
                {
                    //下移
                    if (a >= nodes.Count)
                    {
                        //工序已置尾
                        return false;
                    }
                    nodes[a - 1].ParentNode.InsertAfter(nodes[a - 1], nodes[a]);     
                }
                UpdateGongxuGongbuhao(mydoc);
                mydoc.Save(xmlfile);
                return true;
            }
            else
            {
                //移动工步
                string path = "/SYS_3DPPM/Gongyi/Gongxu[" + a.ToString() + "]/Gongbu";
                XmlNodeList nodes = mydoc.SelectNodes(path);
                if (nodes.Count < 2)
                {
                    //不到两个节点，移什么呀？
                    return false;
                }
                if (isUp)
                {
                    //上移
                    if (b == 1)
                    {
                        //工步已置顶
                        return false;
                    }
                    nodes[b - 1].ParentNode.InsertBefore(nodes[b - 1], nodes[b - 2]);
                }
                else
                {
                    //下移
                    if (b >= nodes.Count)
                    {
                        //工步已置尾
                        return false;
                    }
                    nodes[b - 1].ParentNode.InsertAfter(nodes[b - 1], nodes[b]);
                }
                UpdateGongxuGongbuhao(mydoc);
                mydoc.Save(xmlfile);
                return true;
            }
        }
        catch (System.Exception ex)
        {
            NXFun.MessageBox(ex.Message);
            return false;
        }
    }
    //更新会签信息
    /// <summary>
    /// 更新会签信息
    /// </summary>
    /// <param name="xmlfile">工艺xml</param>
    /// <param name="signoffxml">会签xml</param>
    /// <returns>成功否</returns>
    public static bool UpdateSignOff(string xmlfile,string signoffxml)
    {
        try
        {
            XmlDocument signoffDoc = new XmlDocument();
            signoffDoc.Load(signoffxml);
            XmlDocument processDoc = new XmlDocument();
            processDoc.Load(xmlfile);
            XmlNode gyNode = processDoc.SelectSingleNode("//Gongyi/Information");
            XmlElement gyXE = (XmlElement)gyNode;

            XmlNode signoffGYNode = signoffDoc.SelectSingleNode("//SignOff");
            XmlAttributeCollection signoffAttrs = signoffGYNode.Attributes;

            XmlNodeList signoffGXNodes = signoffDoc.SelectNodes("//operation");
            //更新工艺会签信息
            foreach (XmlAttribute attr in signoffAttrs)
            {
                gyXE.SetAttribute(attr.Name, attr.Value);
            }
            //更新工序会签信息
            foreach (XmlNode signoffGXNode in signoffGXNodes)
            {
                XmlAttributeCollection signoffGXAttrs = signoffGXNode.Attributes;
                XmlNode gxNode = processDoc.SelectSingleNode("//Gongxu/Information[@gongxu_gongxuhao='" + signoffGXNode.Attributes["no"].Value + "']");
                XmlElement gxXE = (XmlElement)gxNode;
                foreach (XmlAttribute attr in signoffGXAttrs)
                {
                    gxXE.SetAttribute(attr.Name, attr.Value);
                }
            }
            processDoc.Save(xmlfile);
            File.Delete(signoffxml);
            return true;
        }
        catch/* (System.Exception ex)*/
        {
            return false;
        }
    }
    //设置Information属性  （0,0）代表工艺  （M,0）代表第M个工序 (M,N)代表第M个工序的第N个工步
    /// <summary>
    /// 设置Information属性 （0,0）代表工艺  （M,0）代表第M个工序 (M,N)代表第M个工序的第N个工步
    /// </summary>
    /// <param name="a">坐标a</param>
    /// <param name="b">坐标b</param>
    /// <param name="title">属性名</param>
    /// <param name="value">属性值</param>
    /// <param name="xmlfile">xml文件</param>
    public static void SetIndexAttr(int a, int b, string title, string value, string xmlfile)
    {
        try
        {
            XmlDocument mydoc = new XmlDocument();
            mydoc.Load(xmlfile);
            if (a == 0 && b == 0)
            {
                //工艺Information属性
                XmlElement xe = (XmlElement)mydoc.SelectSingleNode("/SYS_3DPPM/Gongyi/Information");
                xe.SetAttribute(title, value);
            }
            else if (a != 0 && b == 0)
            {
                //工序Information属性
                string path = "/SYS_3DPPM/Gongyi/Gongxu[" + a.ToString() + "]/Information";
                XmlElement xe = (XmlElement)mydoc.SelectSingleNode(path);
                xe.SetAttribute(title, value);
            }
            else
            {
                //工步Information属性
                string path = "/SYS_3DPPM/Gongyi/Gongxu[" + a.ToString() + "]/Gongbu[" + b.ToString() + "]/Information";
                XmlElement xe = (XmlElement)mydoc.SelectSingleNode(path);
                xe.SetAttribute(title, value);
            }
            mydoc.Save(xmlfile);
        }
        catch (System.Exception ex)
        {
            UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
        }
    }
    //得到图纸模板Item节点信息
    /// <summary>
    /// 得到图纸模板Item节点信息
    /// </summary>
    /// <param name="TEMPLET">模板名称</param>
    /// <param name="xmlfile">模板xml</param>
    /// <returns>图纸模板Item节点信息</returns>
    public static List<S_SheetItem> GetSheetItemList(string TEMPLET,string xmlfile)
    {
        XmlDocument mydoc = new XmlDocument();
        mydoc.Load(xmlfile);
        List<S_SheetItem> SheetItemList = new List<S_SheetItem>();
        try
        {           
            S_SheetItem SheetItem;
            string path = "/SYS_3DPPM/SheetTemplet/ST[@name='" + TEMPLET + "']";
            XmlNodeList nodes = mydoc.SelectNodes(path);
            if (nodes.Count > 0)
            {
                XmlNodeList xns = nodes[0].SelectNodes("Item");
                foreach (XmlElement xe in xns)
                {
                    SheetItem.name = xe.GetAttribute("name");
                    SheetItem.multiline = System.Convert.ToInt32(xe.GetAttribute("multiline"));
                    SheetItem.size = System.Convert.ToDouble(xe.GetAttribute("size"));
                    SheetItem.text = xe.GetAttribute("text");
                    SheetItem.x0 = System.Convert.ToDouble(xe.GetAttribute("x0"));
                    SheetItem.x1 = System.Convert.ToDouble(xe.GetAttribute("x1"));
                    SheetItem.y0 = System.Convert.ToDouble(xe.GetAttribute("y0"));
                    SheetItem.y1 = System.Convert.ToDouble(xe.GetAttribute("y1"));
                    SheetItem.font = xe.GetAttribute("font");
                    SheetItemList.Add(SheetItem);
                }
            }
            return SheetItemList;
        }
        catch (System.Exception ex)
        {
            NXFun.MessageBox(ex.Message);
            return SheetItemList;
        }
    }
    //得到全部模型信息
    /// <summary>
    /// 得到全部模型信息
    /// </summary>
    /// <param name="xmlfile">工艺xml</param>
    /// <returns>全部模型信息</returns>
    public static List<S_Model> GetModelList(string xmlfile)
    {
        XmlDocument mydoc = new XmlDocument();
        mydoc.Load(xmlfile);
        List<S_Model> modellist = new List<S_Model>();
        S_Model model;
        //设计模型
        XmlNodeList nodes = mydoc.SelectNodes("/SYS_3DPPM/Gongyi/Model");
        foreach (XmlElement xe in nodes)
        {
            model.a = 0;
            model.b = 0;
            model.filename = xe.GetAttribute("filename");
            model.description = xe.GetAttribute("description");
            modellist.Add(model);
        }
        //工序模型
        nodes = mydoc.SelectNodes("/SYS_3DPPM/Gongyi/Gongxu");
        for (int i = 0; i < nodes.Count; i++)
        {
            XmlElement xe = (XmlElement)nodes[i].SelectSingleNode("Model");
            model.a = 0;
            model.b = i + 1;
            model.filename = xe.GetAttribute("filename");
            model.description = xe.GetAttribute("description");
            modellist.Add(model);
        }
        //辅助模型
        nodes = mydoc.SelectNodes("/SYS_3DPPM/Gongyi/FuModels/FuModel");
        for (int i = 0; i < nodes.Count; i ++ )
        {
            XmlElement xe = (XmlElement)nodes[i];
            model.a = 1;
            model.b = i + 1;
            model.filename = xe.GetAttribute("filename");
            model.description = xe.GetAttribute("description");
            modellist.Add(model);
        }
        //余量图
        nodes = mydoc.SelectNodes("/SYS_3DPPM/Gongyi/YLTs/YLT");
        for (int i = 0; i < nodes.Count; i++)
        {
            XmlElement xe = (XmlElement)nodes[i];
            model.a = 2;
            model.b = i + 1;
            model.filename = xe.GetAttribute("filename");
            model.description = xe.GetAttribute("description");
            modellist.Add(model);
        }
        return modellist;
    }
    //得到转义符号信息
    /// <summary>
    /// 得到转义符号信息
    /// </summary>
    /// <param name="xmlfile">转义符号XML</param>
    /// <returns>转义符号信息</returns>
    public static List<S_Symbol> GetAllSymbols(string xmlfile)
    {
        XmlDocument mydoc = new XmlDocument();
        mydoc.Load(xmlfile);
        List<S_Symbol> SymbolGroup = new List<S_Symbol>();
        S_Symbol symbol;
        XmlNodeList nodes = mydoc.SelectNodes("/SymbolGroup/Symbol");
        foreach(XmlElement xe in nodes)
        {
            symbol.realName = xe.GetAttribute("RealStr");
            symbol.showName = xe.GetAttribute("ShowStr");
            symbol.strLength = xe.GetAttribute("StrLength");
            SymbolGroup.Add(symbol);
        }
        return SymbolGroup;
    }
    //得到工步数量
    /// <summary>
    /// 得到工步数量   工序坐标从1起
    /// </summary>
    /// <param name="a">工序坐标 从1起</param>
    /// <param name="xmlfile">工艺xml</param>
    /// <returns>工步数量</returns>
    static public int GetGongbuCount(int a, string xmlfile)
    {
        XmlDocument mydoc = new XmlDocument();
        mydoc.Load(xmlfile);
        string path = "/SYS_3DPPM/Gongyi/Gongxu[" + a.ToString() + "]/Gongbu";
        XmlNodeList nodes = mydoc.SelectNodes(path);
        return nodes.Count;
    }
    //得到工序数量
    /// <summary>
    /// 得到工序数量 
    /// </summary>
    /// <param name="xmlfile">工艺xml</param>
    /// <returns>工序数量</returns>
    public static int GetGongxuCount(string xmlfile)
    {
        XmlDocument mydoc = new XmlDocument();
        mydoc.Load(xmlfile);
        string path = "/SYS_3DPPM/Gongyi/Gongxu";
        XmlNodeList nodes = mydoc.SelectNodes(path);
        return nodes.Count;
    }
    //得到子工步信息
    /// <summary>
    /// 得到子工步信息  (M,N)第M个工序的第N个工步  都从1起
    /// </summary>
    /// <param name="a">坐标a</param>
    /// <param name="b">坐标b</param>
    /// <param name="xmlfile">工艺xml</param>
    /// <returns>子工步信息</returns>
    public static S_ChildStep[] GetChildStepList(int a, int b,string xmlfile)
    {
        XmlDocument mydoc = new XmlDocument();
        mydoc.Load(xmlfile);
        S_ChildStep childstep;
        List<S_ChildStep> childsteps = new List<S_ChildStep>();
        string path = "/SYS_3DPPM/Gongyi/Gongxu[" + a.ToString() + "]/Gongbu[" + b.ToString() + "]/Zigongbu";
        XmlNodeList nodes = mydoc.SelectNodes(path);
        foreach(XmlNode xn in nodes)
        {
            XmlElement xe = (XmlElement)xn.SelectSingleNode("Information");
            childstep.name = xe.GetAttribute("zigongbu_name");
            childstep.liangju = xe.GetAttribute("zigongbu_liangju");
            childstep.renju = xe.GetAttribute("zigongbu_renju");
            childstep.beizhu = xe.GetAttribute("zigongbu_beizhu");
            childsteps.Add(childstep);
        }
        return NXFun.List2Array<S_ChildStep>(childsteps);
    }
    //设置子工步信息
    /// <summary>
    /// 设置子工步信息  (M,N)第M个工序的第N个工步  都从1起
    /// </summary>
    /// <param name="a">坐标a</param>
    /// <param name="b">坐标b</param>
    /// <param name="childsteps">子工步信息</param>
    /// <param name="xmlfile">工艺xml</param>
    /// <param name="TemplateXML">模板xml</param>
    public static void SetChildStepList(int a, int b, List<S_ChildStep> childsteps, string xmlfile,string TemplateXML)
    {
        try
        {
            XmlDocument docTo = new XmlDocument();
            XmlDocument docForm = new XmlDocument();
            docForm.Load(TemplateXML);
            docTo.Load(xmlfile);
            string path = "/SYS_3DPPM/Gongyi/Gongxu[" + a.ToString() + "]/Gongbu[" + b.ToString() + "]";
            XmlNode node = docTo.SelectSingleNode(path);
            XmlNodeList nodes = node.SelectNodes("Zigongbu");
            path = "/Template/Zigongbu";
            XmlNode fromNode = docForm.SelectSingleNode(path);
            //删除所有原节点
            foreach (XmlNode xn in nodes)
            {
                node.RemoveChild(xn);
            }
            //添加新子工步
            foreach (S_ChildStep childstep in childsteps)
            {
                XmlNode newNode = (XmlElement)docTo.ImportNode(fromNode, true);
                XmlElement xe = (XmlElement)newNode.SelectSingleNode("Information");
                xe.SetAttribute("zigongbu_name", childstep.name);
                xe.SetAttribute("zigongbu_liangju", childstep.liangju);
                xe.SetAttribute("zigongbu_renju", childstep.renju);
                xe.SetAttribute("zigongbu_beizhu", childstep.beizhu);
                node.AppendChild(newNode);
            }
            docTo.Save(xmlfile);
        }
        catch (System.Exception ex)
        {
            NXFun.MessageBox(ex.Message);
        }
    }
    //添加工序  0为第一个，-1或更大的为最后一个
    //插入节点坐标的位置：
    //原    新（index）
    //		0
    //0-------
    //		1
    //1-------
    //		2
    //2-------
    //		3 or -1
    //--------
    /// <summary>
    /// 添加工序，即时保存
    /// </summary>
    /// <param name="index">0为第一个，-1或更大的为最后一个</param>
    /// <param name="type">普通工序  等等  待扩展</param>
    /// <param name="ProcessXML">工艺xml</param>
    /// <param name="TemplateXML">模板xml</param>
    public static void AddGongxu(int index,string type,string ProcessXML,string TemplateXML)
    {
        try
        {
            XmlDocument docForm = new XmlDocument();
            XmlDocument docTo = new XmlDocument();
            docForm.Load(TemplateXML);
            docTo.Load(ProcessXML);
            XmlNode rootNodel = docTo.SelectSingleNode("//Gongyi");
            string path = "/Template/Gongxu[@Type='" + type + "']";
            XmlNode fromNode = docForm.SelectSingleNode(path);
            XmlNode newNode = docTo.ImportNode(fromNode, true);
            string guid = Guid.NewGuid().ToString();
            ((XmlElement)newNode).SetAttribute("GUID", guid);

            //得到工序数量
            XmlNodeList nodes = rootNodel.SelectNodes("Gongxu");
            int count = nodes.Count;
            if (index < 0 || index > count - 1)
            {
                //加在最后
                rootNodel.AppendChild(newNode);
            }
            else
            {
                //加在index前
                rootNodel.InsertBefore(newNode, nodes[index]);
            }
            UpdateGongxuGongbuhao(docTo);
            docTo.Save(ProcessXML);
        }
        catch (System.Exception ex)
        {
            NXFun.MessageBox(ex.Message);
        }
    }
    //添加工步    0为第一个，-1或更大的为最后一个
    //插入节点坐标的位置：
    //原    新（index）
    //		0
    //0-------
    //		1
    //1-------
    //		2
    //2-------
    //		3 or -1
    //--------
    /// <summary>
    /// 添加工步，即时保存
    /// </summary>
    /// <param name="gongxu_index">第几个工序的孩子，从0开始</param>
    /// <param name="gongbu_index">0为第一个，-1或更大的为最后一个</param>
    /// <param name="ProcessXML">工艺xml</param>
    /// <param name="TemplateXML">模板xml</param>
    public static void AddGongbu(int gongxu_index, int gongbu_index, string ProcessXML, string TemplateXML)
    {
        try
        {
            XmlDocument docForm = new XmlDocument();
            XmlDocument docTo = new XmlDocument();
            docForm.Load(TemplateXML);
            docTo.Load(ProcessXML);
            string path = "/SYS_3DPPM/Gongyi/Gongxu[" + (gongxu_index+1).ToString() + "]";
            XmlNodeList gongxu_nodes = docTo.SelectNodes(path);
            if (gongxu_nodes.Count == 0)
            {
                throw new Exception("在未知工序位置插入工步节点！");
                //return;
            }
            path = "/Template/Gongbu";
            XmlNode fromNode = docForm.SelectSingleNode(path);
            XmlNode newNode = docTo.ImportNode(fromNode, true);
            string guid = Guid.NewGuid().ToString();
            ((XmlElement)newNode).SetAttribute("GUID", guid);

            //得到工步数量
            XmlNodeList nodes = gongxu_nodes[0].SelectNodes("Gongbu");
            int count = nodes.Count;
            if (gongbu_index < 0 || gongbu_index > count - 1)
            {
                //加在最后
                gongxu_nodes[0].AppendChild(newNode);
            }
            else
            {
                //加在index前
                gongxu_nodes[0].InsertBefore(newNode, nodes[gongbu_index]);
            }
            UpdateGongxuGongbuhao(docTo);
            docTo.Save(ProcessXML);
        }
        catch (System.Exception ex)
        {
            NXFun.MessageBox(ex.Message);
        }

    }
    //检测模型是否存在 （0，0）工艺模型 （0,M）工序模型  (1,M)辅助模型  (2,M)  余量图
    /// <summary>
    /// 检测模型是否存在   （0，0）工艺模型 （0,M）工序模型  (1,M)辅助模型  (2,M)  余量图
    /// </summary>
    /// <param name="a">坐标a</param>
    /// <param name="b">坐标b</param>
    /// <param name="xmlfile">工艺xml</param>
    /// <returns>是否存在</returns>
    public static bool IsModelExist(int a,int b,string xmlfile)
    {
        try
        {
            bool result = false;
            XmlDocument mydoc = new XmlDocument();
            mydoc.Load(xmlfile);
            string path = "";
            if (a == 0 && b == 0)
            {
                //设计模型是否存在
                path = "/SYS_3DPPM/Gongyi/Model";
            }
            else if (a == 0 && b > 0)
            {
                //工序模型是否存在
                path = "/SYS_3DPPM/Gongyi/Gongxu[" + b.ToString() + "]/Model"; ;
            }
            else if (a == 1 && b > 0)
            {
                //辅助模型是否存在
                path = "/SYS_3DPPM/Gongyi/FuModels/FuModel[" + b.ToString() + "]"; ;
            }
            else if (a == 2 && b > 0)
            {
                //余量图是否存在
                path = "/SYS_3DPPM/Gongyi/YLTs/YLT[" + b.ToString() + "]"; ;
            }
            else
            {
                return false;
            }
            XmlNodeList nodes = mydoc.SelectNodes(path);
            if (nodes.Count == 1)
            {
                XmlElement xe = (XmlElement)nodes[0];
                if (!String.IsNullOrEmpty(xe.GetAttribute("filename")))
                {
                    result = true;
                };
            }
            return result;
        }
        catch (System.Exception ex)
        {
            NXFun.MessageBox(ex.Message);
            return false;
        }
        
    }
    //得到（无则设置）工艺Guid
    /// <summary>
    /// 得到（无则设置）工艺Guid
    /// </summary>
    /// <param name="xmlfile">工艺xml</param>
    /// <returns>工艺guid</returns>
    public static string GetGongyiGuid(string xmlfile)
    {
        XmlDocument mydoc = new XmlDocument();
        mydoc.Load(xmlfile);
        XmlElement xe = (XmlElement)mydoc.SelectSingleNode("/SYS_3DPPM/Gongyi");
        string guid = xe.GetAttribute("GUID");
        if (String.IsNullOrEmpty(guid) || guid.Length < 4)
        {
            guid = Guid.NewGuid().ToString();
            xe.SetAttribute("GUID", guid);
            mydoc.Save(xmlfile);
        }
        return guid;
    }
    //得到模型流水号
    /// <summary>
    /// 得到模型流水号
    /// </summary>
    /// <param name="ModelPreName">模型前缀</param>
    /// <param name="xmlfile">工艺xml</param>
    /// <returns>模型流水号</returns>
    public static int GetModelNameNum(string ModelPreName, string xmlfile)
    {
        List<S_Model> modellist = GetModelList(xmlfile);
        int NameNum = -1;
        foreach(S_Model model in modellist)
        {
            if (model.filename.Contains(ModelPreName))
            {
                string[] ss = model.filename.Split("_.".ToCharArray());
                if (ss.Length > 1)
                {
                    string num = ss[1];
                    int i = -1;
                    try
                    {
                        i = Convert.ToInt32(num);
                    }
                    catch /*(System.Exception ex)*/
                    {
                        i = -1;
                    }
                    if (i > NameNum)
                    {
                        NameNum = i;
                    }
                }
            }
        }
        return NameNum == -1 ? 0 : NameNum + 1;
    }
    
    public static bool SetModelAttr(int a,int b,string name,string description,string xmlfile,string TemplateXML)
    {
        try
        {
            XmlDocument mydoc = new XmlDocument();
            mydoc.Load(xmlfile);
            string path = "";
            if (a == 0 && b == 0)
            {
                //修改设计模型
                path = "/SYS_3DPPM/Gongyi/Model";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                node.SetAttribute("filename", name);
                node.SetAttribute("description", description);
                mydoc.Save(xmlfile);
                return true;
            }
            else if (a == 0 && b > 0)
            {
                //修改工序模型
                path = "/SYS_3DPPM/Gongyi/Gongxu[" + b.ToString() + "]/Model";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                node.SetAttribute("filename", name);
                node.SetAttribute("description", description);
                mydoc.Save(xmlfile);
                return true;
            }
            else if (a == 1 && b == 0)
            {
                //添加辅助模型
                path = "/SYS_3DPPM/Gongyi/FuModels";
                XmlNode node = mydoc.SelectSingleNode(path);
                XmlDocument docForm = new XmlDocument();
                docForm.Load(TemplateXML);
                XmlNode fromNode = docForm.SelectSingleNode("/Template/FuModel");
                XmlNode newNode = mydoc.ImportNode(fromNode, true);
                newNode = node.AppendChild(newNode);
                XmlElement xe = newNode as XmlElement;
                xe.SetAttribute("filename", name);
                xe.SetAttribute("description", description);
                mydoc.Save(xmlfile);
                return true;
            }
            else if (a == 1 && b > 0)
            {
                //修改辅助模型
                path = "/SYS_3DPPM/Gongyi/FuModels/FuModel[" + b.ToString() + "]";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                node.SetAttribute("filename", name);
                node.SetAttribute("description", description);
                mydoc.Save(xmlfile);
                return true;
            }
            else if (a == 2 && b == 0)
            {
                //添加余量图
                path = "/SYS_3DPPM/Gongyi/YLTs";
                XmlNode node = mydoc.SelectSingleNode(path);
                XmlDocument docForm = new XmlDocument();
                docForm.Load(TemplateXML);
                XmlNode fromNode = docForm.SelectSingleNode("/Template/YLT");
                XmlNode newNode = mydoc.ImportNode(fromNode, true);
                newNode = node.AppendChild(newNode);
                XmlElement xe = newNode as XmlElement;
                xe.SetAttribute("filename", name);
                xe.SetAttribute("description", description);
                mydoc.Save(xmlfile);
                return true;
            }
            else if (a == 2 && b > 0)
            {
                //修改余量图
                path = "/SYS_3DPPM/Gongyi/YLTs/YLT[" + b.ToString() + "]";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                node.SetAttribute("filename", name);
                node.SetAttribute("description", description);
                mydoc.Save(xmlfile);
                return true;
            }
            return false;
        }
        catch (System.Exception ex)
        {
            NXFun.MessageBox(ex.Message);
            return false;
        }
    }
    public static bool DelModelNode(int a, int b, string xmlfile)
    {
        try
        {
            XmlDocument mydoc = new XmlDocument();
            mydoc.Load(xmlfile);
            string path = "";
            if (a == 0 && b == 0)
            {
                //删除设计模型
                path = "/SYS_3DPPM/Gongyi/Model";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                node.SetAttribute("filename", "");
                node.SetAttribute("description", "");
                mydoc.Save(xmlfile);
                return true;
            }
            else if (a == 0 && b > 0)
            {
                //删除工序模型
                path = "/SYS_3DPPM/Gongyi/Gongxu[" + b.ToString() + "]/Model";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                node.SetAttribute("filename", "");
                node.SetAttribute("description", "");
                mydoc.Save(xmlfile);
                return true;
            }
            else if (a == 1 && b > 0)
            {
                //删除辅助模型
                path = "/SYS_3DPPM/Gongyi/FuModels/FuModel[" + b.ToString() + "]";
                XmlNode node = mydoc.SelectSingleNode(path);
                node.ParentNode.RemoveChild(node);
                mydoc.Save(xmlfile);
                return true;
            }
            else if (a == 2 && b > 0)
            {
                //删除余量图
                path = "/SYS_3DPPM/Gongyi/YLTs/YLT[" + b.ToString() + "]";
                XmlNode node = mydoc.SelectSingleNode(path);
                node.ParentNode.RemoveChild(node);
                mydoc.Save(xmlfile);
                return true;
            }
            return false;
        }
        catch (System.Exception ex)
        {
            NXFun.MessageBox(ex.Message);
            return false;
        }

    }
    public static bool DelModelNode(string filename, string xmlfile)
    {
        try
        {
            XmlDocument mydoc = new XmlDocument();
            mydoc.Load(xmlfile);
            List<S_Model> modellist = GetModelList(xmlfile);
            S_Model model = modellist.Find(delegate(S_Model m) { return m.filename == filename; });
            if (model.Equals(default(S_Model)))
            {
                return false;
            }
            int a = model.a;
            int b = model.b;
            string path = "";
            if (a == 0 && b == 0)
            {
                //删除设计模型
                path = "/SYS_3DPPM/Gongyi/Model";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                node.SetAttribute("filename", "");
                node.SetAttribute("description", "");
                mydoc.Save(xmlfile);
                return true;
            }
            else if (a == 0 && b > 0)
            {
                //删除工序模型
                path = "/SYS_3DPPM/Gongyi/Gongxu[" + b.ToString() + "]/Model";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                node.SetAttribute("filename", "");
                node.SetAttribute("description", "");
                mydoc.Save(xmlfile);
                return true;
            }
            else if (a == 1 && b > 0)
            {
                //删除辅助模型
                path = "/SYS_3DPPM/Gongyi/FuModels/FuModel[" + b.ToString() + "]";
                XmlNode node = mydoc.SelectSingleNode(path);
                node.ParentNode.RemoveChild(node);
                mydoc.Save(xmlfile);
                return true;
            }
            else if (a == 2 && b > 0)
            {
                //删除余量图
                path = "/SYS_3DPPM/Gongyi/YLTs/YLT[" + b.ToString() + "]";
                XmlNode node = mydoc.SelectSingleNode(path);
                node.ParentNode.RemoveChild(node);
                mydoc.Save(xmlfile);
                return true;
            }
            return false;
        }
        catch (System.Exception ex)
        {
            NXFun.MessageBox(ex.Message);
            return false;
        }

    }

    public static bool SetModelDescription(int a, int b, string description, string xmlfile)
    {
        try
        {
            XmlDocument mydoc = new XmlDocument();
            mydoc.Load(xmlfile);
            string path = "";
            if (a == 0 && b == 0)
            {
                //修改设计模型
                path = "/SYS_3DPPM/Gongyi/Model";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                node.SetAttribute("description", description);
                mydoc.Save(xmlfile);
                return true;
            }
            else if (a == 0 && b > 0)
            {
                //修改工序模型
                path = "/SYS_3DPPM/Gongyi/Gongxu[" + b.ToString() + "]/Model";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                node.SetAttribute("description", description);
                mydoc.Save(xmlfile);
                return true;
            }
            else if (a == 1 && b > 0)
            {
                //修改辅助模型
                path = "/SYS_3DPPM/Gongyi/FuModels/FuModel[" + b.ToString() + "]";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                node.SetAttribute("description", description);
                mydoc.Save(xmlfile);
                return true;
            }
            else if (a == 2 && b > 0)
            {
                //修改余量图
                path = "/SYS_3DPPM/Gongyi/YLTs/YLT[" + b.ToString() + "]";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                node.SetAttribute("description", description);
                mydoc.Save(xmlfile);
                return true;
            }
            return false;
        }
        catch (System.Exception ex)
        {
            NXFun.MessageBox(ex.Message);
            return false;
        }

    }
    public static string GetModelDescription(int a, int b, string xmlfile)
    {
        try
        {
            XmlDocument mydoc = new XmlDocument();
            mydoc.Load(xmlfile);
            string path = "";
            if (a == 0 && b == 0)
            {
                //设计模型
                path = "/SYS_3DPPM/Gongyi/Model";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                return node.GetAttribute("description");
            }
            else if (a == 0 && b > 0)
            {
                //工序模型
                path = "/SYS_3DPPM/Gongyi/Gongxu[" + b.ToString() + "]/Model";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                return node.GetAttribute("description");
            }
            else if (a == 1 && b == 0)
            {
                //添加辅助模型
                path = "/SYS_3DPPM/Gongyi/FuModels/FuModel";
                XmlNodeList nodes = mydoc.SelectNodes(path);
                List<string> names = new List<string>();
                foreach (XmlElement xe in nodes)
                {
                    names.Add(xe.GetAttribute("description"));
                }
                int i = 1;
                string des = "";
                do 
                {
                    des = "辅助模型"+(i++).ToString();
                } while (NXFun.isFindInList(NXFun.List2Array<string>(names),des));
                return des;
            }
            else if (a == 1 && b > 0)
            {
                //编辑辅助模型
                path = "/SYS_3DPPM/Gongyi/FuModels/FuModel[" + b.ToString() + "]";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                return node.GetAttribute("description");
            }
            else if (a == 2 && b == 0)
            {
                //添加余量图
                path = "/SYS_3DPPM/Gongyi/YLTs/YLT";
                XmlNodeList nodes = mydoc.SelectNodes(path);
                List<string> names = new List<string>();
                foreach (XmlElement xe in nodes)
                {
                    names.Add(xe.GetAttribute("description"));
                }
                int i = 1;
                string des = "";
                do 
                {
                    des = "余量图" + (i++).ToString();
                } while (NXFun.isFindInList(NXFun.List2Array<string>(names),des));
                return des;
            }
            else if (a == 2 && b > 0)
            {
                //编辑余量图
                path = "/SYS_3DPPM/Gongyi/YLTs/YLT[" + b.ToString() + "]";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                return node.GetAttribute("description");
            }
            return "";
        }
        catch (System.Exception ex)
        {
            NXFun.MessageBox(ex.Message);
            return "";
        }

    }
    /// <summary>
    /// 得到模型名称 0,0工艺   0,N 工序   1,N 辅助   2,N 余量图   N从1起
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="xmlfile"></param>
    /// <returns></returns>
    public static string GetModelName(int a, int b, string xmlfile)
    {
        try
        {
            XmlDocument mydoc = new XmlDocument();
            mydoc.Load(xmlfile);
            string path = "";
            if (a == 0 && b == 0)
            {
                //设计模型
                path = "/SYS_3DPPM/Gongyi/Model";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                return node.GetAttribute("filename");
            }
            else if (a == 0 && b > 0)
            {
                //工序模型
                path = "/SYS_3DPPM/Gongyi/Gongxu[" + b.ToString() + "]/Model";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                return node.GetAttribute("filename");
            }
            else if (a == 1 && b > 0)
            {
                //辅助模型
                path = "/SYS_3DPPM/Gongyi/FuModels/FuModel[" + b.ToString() + "]";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                return node.GetAttribute("filename");
            }
            else if (a == 2 && b > 0)
            {
                //余量图
                path = "/SYS_3DPPM/Gongyi/YLTs/YLT[" + b.ToString() + "]";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                return node.GetAttribute("filename");
            }
            return "";
        }
        catch (System.Exception ex)
        {
            NXFun.MessageBox(ex.Message);
            return "";
        }

    }


    public static bool GetModelAttr(int a, int b, out string name, out string description, string xmlfile)
    {
        try
        {
            name = "";
            description = "";
            XmlDocument mydoc = new XmlDocument();
            mydoc.Load(xmlfile);
            string path = "";
            if (a == 0 && b == 0)
            {
                //设计模型
                path = "/SYS_3DPPM/Gongyi/Model";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                name = node.GetAttribute("filename");
                description = node.GetAttribute("description");
                return true;
            }
            else if (a == 0 && b > 0)
            {
                //工序模型
                path = "/SYS_3DPPM/Gongyi/Gongxu[" + b.ToString() + "]/Model";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                name = node.GetAttribute("filename");
                description = node.GetAttribute("description");
                return true;
            }
            else if (a == 1 && b == 0)
            {
                return true;
            }
            else if (a == 1 && b > 0)
            {
                //修改辅助模型
                path = "/SYS_3DPPM/Gongyi/FuModels/FuModel[" + b.ToString() + "]";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                name = node.GetAttribute("filename");
                description = node.GetAttribute("description");
                return true;
            }
            else if (a == 2 && b == 0)
            {
                return true;
            }
            else if (a == 2 && b > 0)
            {
                //修改余量图
                path = "/SYS_3DPPM/Gongyi/YLTs/YLT[" + b.ToString() + "]";
                XmlElement node = (XmlElement)mydoc.SelectSingleNode(path);
                name = node.GetAttribute("filename");
                description = node.GetAttribute("description");
                return true;
            }
            return false;
        }
        catch (System.Exception ex)
        {
            NXFun.MessageBox(ex.Message);
            name = "";
            description = "";
            return false;
        }
    }

    /// <summary>
    /// 更新工序工步号
    /// </summary>
    /// <param name="doc"></param>
    private static void UpdateGongxuGongbuhao(XmlDocument doc)
    {
        string path = "/SYS_3DPPM/Gongyi/Gongxu";
        XmlNodeList gxNodes = doc.SelectNodes(path);
        for (int i = 0; i < gxNodes.Count; i ++ )
        {
            XmlElement xe = gxNodes[i].SelectSingleNode("Information") as XmlElement;
            xe.SetAttribute("gongxu_gongxuhao", (i * 5).ToString());
            XmlNodeList gbNodes = gxNodes[i].SelectNodes("Gongbu");
            for (int j = 0; j < gbNodes.Count; j ++ )
            {
                xe = gbNodes[j].SelectSingleNode("Information") as XmlElement;
                xe.SetAttribute("gongbu_gongbuhao", (j + 1).ToString());
            }
        }
    }

    public static List<S_Yingdu> GetYingduList(string xmlfile)
    {
        List<S_Yingdu> YingduList = new List<S_Yingdu>();
        try
        {
            S_Yingdu s_yingdu;
            s_yingdu.gongxuhao = "";
            s_yingdu.yingdu = "";
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlfile);
            XmlNodeList gxNodeList = doc.SelectNodes("//Gongxu/Information");
            foreach (XmlElement xe in gxNodeList)
            {
                s_yingdu.gongxuhao = xe.GetAttribute("gongxu_gongxuhao");
                s_yingdu.yingdu = xe.GetAttribute("gongxu_yingdu");
                YingduList.Add(s_yingdu);
            }
            return YingduList;
        }
        catch (System.Exception ex)
        {
            NXFun.MessageBox(ex.Message);
            return YingduList;        	
        }        
    }

    public static bool SetYingduList(List<S_Yingdu> YingduList,string xmlfile)
    {
        try
        {
            //按工序号排序  double是为了支持小数工序号
            SortedDictionary<double,string> sort = new SortedDictionary<double,string>();
            foreach(S_Yingdu s_yingdu in YingduList)
            {
                if (!string.IsNullOrEmpty(s_yingdu.gongxuhao))
                {
                    sort.Add(Convert.ToDouble(s_yingdu.gongxuhao),s_yingdu.yingdu);
                }
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlfile);
            XmlNodeList gxNodeList = doc.SelectNodes("//Gongxu/Information");
            
            foreach(double key in sort.Keys)
            {
                double gxHaoStart = key;
                foreach (XmlElement xe in gxNodeList)
                {
                    if (Convert.ToDouble(xe.GetAttribute("gongxu_gongxuhao"))>= gxHaoStart)
                    {
                        xe.SetAttribute("gongxu_yingdu", sort[key]);
                    }
                }
            }
            doc.Save(xmlfile);
            return true;
        }
        catch/* (System.Exception ex)*/
        {
            return false;
        }
    }
}