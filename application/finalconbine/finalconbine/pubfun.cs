using System;
using System.Collections.Generic;
using System.Text;
using NXOpen;
using NXOpenUI;
using NXOpen.BlockStyler;
using NXOpen.UF;
using System.Reflection;
using System.Runtime.InteropServices;

    class pubfun
    {
        private static Session theSession = Session.GetSession();
        private static UFSession theUfSession = UFSession.GetUFSession();
        private Part workPart = theSession.Parts.Work;
        private Part displayPart = theSession.Parts.Display;
        // private static UI theUI = null;



        public static string GetDimensionValue(NXOpen.Annotations.Dimension dimension)
        {
            try
            {
                string[] mainTextLines;
                string[] dualTextLines;
                dimension.GetDimensionText(out mainTextLines, out dualTextLines);
                if (mainTextLines.Length > 0)
                {
                    //这里可能包含特殊字符，从中得到连续数字
                    string num = GetNumberFromString(mainTextLines[0]);
                    return num;
                }
                else
                {
                    return null;
                }
            }
            catch/* (System.Exception ex)*/
            {
                throw new Exception("读取尺寸值失败，请手动指定尺寸值");
            }

        }
        public static string GetNumberFromString(string str)
        {
            string num = "";
            if (str.Contains("<$s>"))
            {

               str = str.Replace("<$s>", "'");
                num = str;
            }
            else
            {
                num = str;
                //foreach (char a in str)
                //{
                //    if (isNum(a))
                //    {
                //        if (a == ',')
                //        {
                //            num += '.';
                //        }
                //        else
                //        {
                //            num += a;
                //        }
                //    }
                //}

              
            }
            return num;
        }
        public static bool isNum(char a)
        {
            string num = "0123456789.,-";
            string s = "" + a;
            return num.Contains(s);
        }

        public static void GetTolerance(NXOpen.Annotations.Dimension dim, out double up, out double low)
        {
            //目前未考虑精度
            try
            {
                up = 0;
                low = 0;
                Object tol = ReflectFun(dim, "GetTolerance");

             

                // ReflectSetProperty(tol, "ToleranceType", NXOpen.Annotations.ToleranceType.BilateralTwoLines);


                //ReflectFun(dim, "SetTolerance", tol);
                Type type = tol.GetType();
              
                if (type.Name == "LinearTolerance")
                {
                    NXOpen.Annotations.Value v_up = new NXOpen.Annotations.Value();
                    NXOpen.Annotations.Value v_low = new NXOpen.Annotations.Value();
                    NXOpen.Annotations.LinearTolerance lint = (NXOpen.Annotations.LinearTolerance)tol;
                    NXOpen.Annotations.ToleranceType oldtype = lint.ToleranceType;

                    if (lint.ToleranceType == NXOpen.Annotations.ToleranceType.BilateralOneLine)
                    {
                        //v_low = (NXOpen.Annotations.Value)ReflectFun(tol, "GetLowerToleranceMm");
                        v_up = (NXOpen.Annotations.Value)ReflectFun(tol, "GetUpperToleranceMm");
                        v_low = v_up;
                        up = v_up.ItemValue;
                        low = v_low.ItemValue * (-1);


                    }
                    else if (lint.ToleranceType == NXOpen.Annotations.ToleranceType.UnilateralBelow)
                    {
                        up = 0;
                        v_low = (NXOpen.Annotations.Value)ReflectFun(tol, "GetLowerToleranceMm");
                        low = v_low.ItemValue;
                    }
                    else if (lint.ToleranceType == NXOpen.Annotations.ToleranceType.UnilateralAbove)
                    {
                        low = 0;
                        v_up = (NXOpen.Annotations.Value)ReflectFun(tol, "GetUpperToleranceMm");
                        up = v_up.ItemValue;
                    }
                    else if (lint.ToleranceType == NXOpen.Annotations.ToleranceType.BilateralTwoLines)
                    {
                        v_low = (NXOpen.Annotations.Value)ReflectFun(tol, "GetLowerToleranceMm");
                        v_up = (NXOpen.Annotations.Value)ReflectFun(tol, "GetUpperToleranceMm");
                        up = v_up.ItemValue;
                        low = v_low.ItemValue;
                    }
                    ReflectSetProperty(tol, "ToleranceType", oldtype);
                    ReflectFun(dim, "SetTolerance", tol);
                }
                else
                {
                    NXOpen.Annotations.AngularTolerance lint = (NXOpen.Annotations.AngularTolerance)tol;
                    NXOpen.Annotations.ToleranceType oldtype = lint.ToleranceType;
                    NXOpen.Annotations.Value v_up = new NXOpen.Annotations.Value();
                    NXOpen.Annotations.Value v_low = new NXOpen.Annotations.Value();
                    if (lint.ToleranceType == NXOpen.Annotations.ToleranceType.BilateralOneLine)
                    {
                        //v_low = (NXOpen.Annotations.Value)ReflectFun(tol, "GetLowerToleranceMm");
                        v_up = (NXOpen.Annotations.Value)ReflectFun(tol, "GetUpperToleranceDegrees");
                        v_low = v_up;
                        up = v_up.ItemValue;
                        low = v_low.ItemValue * (-1);


                    }
                    else if (lint.ToleranceType == NXOpen.Annotations.ToleranceType.UnilateralBelow)
                    {
                        up = 0;
                        v_low = (NXOpen.Annotations.Value)ReflectFun(tol, "GetLowerToleranceDegrees");
                        low = v_low.ItemValue;
                    }
                    else if (lint.ToleranceType == NXOpen.Annotations.ToleranceType.UnilateralAbove)
                    {
                        low = 0;
                        v_up = (NXOpen.Annotations.Value)ReflectFun(tol, "GetUpperToleranceDegrees");
                        up = v_up.ItemValue;
                    }
                    else if (lint.ToleranceType == NXOpen.Annotations.ToleranceType.BilateralTwoLines)
                    {
                        v_low = (NXOpen.Annotations.Value)ReflectFun(tol, "GetLowerToleranceDegrees");
                        v_up = (NXOpen.Annotations.Value)ReflectFun(tol, "GetUpperToleranceDegrees");
                        up = v_up.ItemValue;
                        low = v_low.ItemValue;
                    }
                    ReflectSetProperty(tol, "ToleranceType", oldtype);
                    ReflectFun(dim, "SetTolerance", tol);
                   
                }



               
            }
            catch (System.Exception ex)
            {
                //UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Warning, ex.Message);
                up = 0;
                low = 0;
            }
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

        public string[] getspec(NXOpen.Annotations.Dimension dim)//返回一个尺寸的名义值和上下公差，第一个值是名义值，第二个是上公差，第三个是下公差
        {
            string[] a;
            string[] b;
            string maindim = null;
            double low = 0;
            double up = 0;
            string[] final = {"","",""};

            maindim = GetDimensionValue(dim);
            GetTolerance(dim, out up, out low);
            final[0] = maindim;
            final[1] = up.ToString();
            final[2] = low.ToString();
            return final;
        }
        public void function(string ToleranceValue, string PrimaryDatumReference, string SecondaryDatumReference, string TertiaryDatumReference, object ZoneShape,object Characteristic, object MaterialModifier, object PrimaryMaterialCondition, object SecondaryMaterialCondition, object TertiaryMaterialCondition,
            object frameStyle, double Annotationletter, double duanxian, Point3d point1, DisplayableObject point2, DisplayableObject guanlian, object LeaderType)
        {

            NXOpen.Annotations.Fcf nullAnnotations_Fcf = null;
            NXOpen.Annotations.PmiFeatureControlFrameBuilder pmiFeatureControlFrameBuilder1;
            pmiFeatureControlFrameBuilder1 = workPart.Annotations.CreatePmiFeatureControlFrameBuilder(nullAnnotations_Fcf);

            pmiFeatureControlFrameBuilder1.Origin.SetInferRelativeToGeometry(true);

            pmiFeatureControlFrameBuilder1.Origin.Anchor = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;

            pmiFeatureControlFrameBuilder1.Characteristic = (NXOpen.Annotations.FeatureControlFrameBuilder.FcfCharacteristic)Characteristic;


            pmiFeatureControlFrameBuilder1.Origin.Plane.PlaneMethod = NXOpen.Annotations.PlaneBuilder.PlaneMethodType.ModelView;

            pmiFeatureControlFrameBuilder1.Origin.SetInferRelativeToGeometry(true);

            NXOpen.Annotations.LeaderData leaderData1;
            leaderData1 = workPart.Annotations.CreateLeaderData();

            leaderData1.Arrowhead = NXOpen.Annotations.LeaderData.ArrowheadType.FilledArrow;

            pmiFeatureControlFrameBuilder1.Leader.Leaders.Append(leaderData1);

            leaderData1.Perpendicular = false;

            leaderData1.StubSize = duanxian;

            leaderData1.StubSide = NXOpen.Annotations.LeaderSide.Inferred;

            TaggedObject taggedObject1;
            taggedObject1 = pmiFeatureControlFrameBuilder1.FeatureControlFrameDataList.FindItem(0);

            NXOpen.Annotations.FeatureControlFrameDataBuilder featureControlFrameDataBuilder1 = (NXOpen.Annotations.FeatureControlFrameDataBuilder)taggedObject1;

            featureControlFrameDataBuilder1.ZoneShape = (NXOpen.Annotations.FeatureControlFrameDataBuilder.ToleranceZoneShape)ZoneShape;

            featureControlFrameDataBuilder1.ToleranceValue = ToleranceValue;

            featureControlFrameDataBuilder1.MaterialModifier = (NXOpen.Annotations.FeatureControlFrameDataBuilder.ToleranceMaterialModifier)MaterialModifier;
            featureControlFrameDataBuilder1.PrimaryDatumReference.Letter = PrimaryDatumReference;//基准
            featureControlFrameDataBuilder1.SecondaryDatumReference.Letter = SecondaryDatumReference;
            featureControlFrameDataBuilder1.TertiaryDatumReference.Letter = TertiaryDatumReference;
            if (PrimaryMaterialCondition != null)
            {

                featureControlFrameDataBuilder1.PrimaryDatumReference.MaterialCondition = (NXOpen.Annotations.DatumReferenceBuilder.DatumReferenceMaterialCondition)PrimaryMaterialCondition;//材料按顺序为L M 不填 S

            }

            if (SecondaryMaterialCondition != null)
            {
                featureControlFrameDataBuilder1.SecondaryDatumReference.MaterialCondition = (NXOpen.Annotations.DatumReferenceBuilder.DatumReferenceMaterialCondition)SecondaryMaterialCondition;
            }

            if (TertiaryMaterialCondition != null)
            {
                featureControlFrameDataBuilder1.TertiaryDatumReference.MaterialCondition = (NXOpen.Annotations.DatumReferenceBuilder.DatumReferenceMaterialCondition)TertiaryMaterialCondition;
            }

            pmiFeatureControlFrameBuilder1.Origin.SetInferRelativeToGeometry(true);

            NXOpen.Annotations.Annotation.AssociativeOriginData assocOrigin1;
            assocOrigin1.OriginType = NXOpen.Annotations.AssociativeOriginType.Drag;
            NXOpen.View nullView = null;
            assocOrigin1.View = nullView;
            assocOrigin1.ViewOfGeometry = nullView;
            NXOpen.Point nullPoint = null;
            assocOrigin1.PointOnGeometry = nullPoint;

            assocOrigin1.VertAnnotation = null;
            assocOrigin1.VertAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
            assocOrigin1.HorizAnnotation = null;
            assocOrigin1.HorizAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
            assocOrigin1.AlignedAnnotation = null;
            assocOrigin1.DimensionLine = 0;
            assocOrigin1.AssociatedView = nullView;
            assocOrigin1.AssociatedPoint = nullPoint;
            assocOrigin1.OffsetAnnotation = null;
            assocOrigin1.OffsetAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
            assocOrigin1.XOffsetFactor = 0.0;
            assocOrigin1.YOffsetFactor = 0.0;
            assocOrigin1.StackAlignmentPosition = NXOpen.Annotations.StackAlignmentPosition.Above;
            pmiFeatureControlFrameBuilder1.Origin.SetAssociativeOrigin(assocOrigin1);

            pmiFeatureControlFrameBuilder1.FrameStyle = (NXOpen.Annotations.FeatureControlFrameBuilder.FcfFrameStyle)frameStyle;//单框复合框

            if (guanlian != null)
            {
                bool added1;
                added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanlian);

            }
            //NXOpen.Point point4;
            //point4 = workPart.Points.CreatePoint(point1);
            pmiFeatureControlFrameBuilder1.Origin.Origin.SetValue(null, nullView, point1);

            point2.Highlight();
            try
            {
            //    if (LeaderType.Equals(NXOpen.Annotations.LeaderData.LeaderType.Plain))
            //    {
                    leaderData1.TerminatorType = (NXOpen.Annotations.LeaderData.LeaderType)LeaderType;
                    NXOpen.Point point3 = (Point)point2;
                    // point3 = workPart.Points.CreatePoint(point2);

                    Point3d p = point3.Coordinates;
                    leaderData1.Leader.SetValue(point2, workPart.ModelingViews.WorkView, p);//折线
               // }
            }
            catch (Exception ex)
            { UI.GetUI().NXMessageBox.Show("error", NXMessageBox.DialogType.Error, ex.ToString()); }
            //else
            //{
            //    leaderData1.TerminatorType =(NXOpen.Annotations.LeaderData.LeaderType)LeaderType;//标志
            //    NXOpen.Edge point3 = (Edge)point2;
            //    double[] p1=new double[3];
            //    double[] p2=new double[3];
            //    int v;
            //    theUfSession.Modl.AskEdgeVerts(point3.Tag,p1,p2,out v);
            //    double[] p3 = new double[3];
            //    p3[0] = (p1[0] + p2[0]) / 2;
            //    p3[1] = (p1[1] + p2[1]) / 2;
            //    p3[2] = (p1[2] + p2[2]) / 2;
            //    Point3d pp = new Point3d(p3[0], p3[0], p3[0]);
            //    // //Point3d p = point3.Coordinates;
            //   leaderData1.Leader.SetValue(point2, workPart.ModelingViews.WorkView, pp);//折线
            //}
            // NXOpen.Features.Extrude extrude1 = (NXOpen.Features.Extrude)workPart.Features.FindObject("EXTRUDE(3)");
            // Edge edge1 = (Edge)extrude1.FindObject("EDGE * 180 * 190 {(3,10,5)(3,10,0)(3,10,-5) EXTRUDE(2)}");
            // Point3d point2 = new Point3d(3.0, 10.0, 3.02209729734126);
            // leaderData1.Leader.SetValue(edge1, workPart.ModelingViews.WorkView, point1);

            // leaderData1.Arrowhead = NXOpen.Annotations.LeaderData.ArrowheadType.None;

            //// NXOpen.View nullView = null;
            // Point3d point3= new Point3d(3.0, 10.0, 5.0);
            // leaderData1.Leader.SetValue(edge1, workPart.ModelingViews.WorkView, point2);


            NXObject nXObject1;
            nXObject1 = pmiFeatureControlFrameBuilder1.Commit();


            //样式设置
            NXOpen.Annotations.Fcf fcf1 = (NXOpen.Annotations.Fcf)nXObject1;

            NXOpen.Annotations.SymbolPreferences symbolPreferences1;
            symbolPreferences1 = fcf1.GetSymbolPreferences();

            NXOpen.Annotations.LineAndArrowPreferences lineAndArrowPreferences1;
            lineAndArrowPreferences1 = fcf1.GetLineAndArrowPreferences();

            NXOpen.Annotations.LetteringPreferences letteringPreferences1;
            letteringPreferences1 = fcf1.GetLetteringPreferences();
            lineAndArrowPreferences1.LeaderLocation = NXOpen.Annotations.VerticalTextJustification.Middle;

            fcf1.SetLineAndArrowPreferences(lineAndArrowPreferences1);
            NXOpen.Annotations.Lettering generalText1;
            generalText1.Size = Annotationletter;//文本设置值
            generalText1.CharacterSpaceFactor = 0.5;
            generalText1.AspectRatio = 0.57;
            generalText1.LineSpaceFactor = 1.0;
            generalText1.Cfw.Color = 211;
            generalText1.Cfw.Font = 2;
            generalText1.Cfw.Width = NXOpen.Annotations.LineWidth.Thin;
            letteringPreferences1.SetGeneralText(generalText1);
            fcf1.LeaderOrientation = NXOpen.Annotations.LeaderOrientation.FromRight;
            //fcf1.LeaderOrientation = NXOpen.Annotations.LeaderOrientation.FromLeft;
            fcf1.SetLetteringPreferences(letteringPreferences1);

            pmiFeatureControlFrameBuilder1.Destroy();

        }

        public void SurfaceFinishFunction(string a, string d, object StandardType, object FinishType, Point3d point1, Face obj, NXOpen.Point point,Point testpoint)
        {
            //生成粗糙度符号
            NXOpen.Annotations.SurfaceFinish nullAnnotations_SurfaceFinish = null;
            NXOpen.Annotations.SurfaceFinishBuilder surfaceFinishBuilder1;
           
            surfaceFinishBuilder1 = workPart.PmiManager.PmiAttributes.CreateSurfaceFinishBuilder(nullAnnotations_SurfaceFinish);

            surfaceFinishBuilder1.Origin.Anchor = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;

            surfaceFinishBuilder1.Origin.Plane.PlaneMethod = NXOpen.Annotations.PlaneBuilder.PlaneMethodType.ModelView;

            surfaceFinishBuilder1.Origin.Plane.PlaneMethod = NXOpen.Annotations.PlaneBuilder.PlaneMethodType.ModelView;

            surfaceFinishBuilder1.Origin.SetInferRelativeToGeometry(true);
            surfaceFinishBuilder1.Origin.SetInferRelativeToGeometry(true);

            NXOpen.Annotations.LeaderData leaderData1;
            leaderData1 = workPart.Annotations.CreateLeaderData();

            leaderData1.Arrowhead = NXOpen.Annotations.LeaderData.ArrowheadType.FilledArrow;

            surfaceFinishBuilder1.Leader.Leaders.Append(leaderData1);

            leaderData1.StubSide = NXOpen.Annotations.LeaderSide.Inferred;

            surfaceFinishBuilder1.Title = "Surface Finish";

            surfaceFinishBuilder1.A1 = a;// "Ra 0.025";//粗糙度

            surfaceFinishBuilder1.F1 = "";

            surfaceFinishBuilder1.Angle = 0.0;

            surfaceFinishBuilder1.D = d;// "=";//放置符号

            surfaceFinishBuilder1.Standard = (NXOpen.Annotations.SurfaceFinishBuilder.StandardType)StandardType;//.Iso;//标准

            surfaceFinishBuilder1.Finish = (NXOpen.Annotations.SurfaceFinishBuilder.FinishType)FinishType;//.MaterialRemovalRequired;//材料移除

            surfaceFinishBuilder1.Origin.SetInferRelativeToGeometry(true);

            surfaceFinishBuilder1.Origin.SetInferRelativeToGeometry(true);


            NXOpen.Annotations.Annotation.AssociativeOriginData assocOrigin1;
            assocOrigin1.OriginType = NXOpen.Annotations.AssociativeOriginType.Drag;
            NXOpen.View nullView = null;
            assocOrigin1.View = nullView;
            assocOrigin1.ViewOfGeometry = nullView;
            NXOpen.Point nullPoint = testpoint;
            assocOrigin1.PointOnGeometry = nullPoint;
            assocOrigin1.VertAnnotation = null;
            assocOrigin1.VertAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
            assocOrigin1.HorizAnnotation = null;
            assocOrigin1.HorizAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
            assocOrigin1.AlignedAnnotation = null;
            assocOrigin1.DimensionLine = 0;
            assocOrigin1.AssociatedView = nullView;
            assocOrigin1.AssociatedPoint = nullPoint;
            assocOrigin1.OffsetAnnotation = null;
            assocOrigin1.OffsetAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
            assocOrigin1.XOffsetFactor = 0.0;
            assocOrigin1.YOffsetFactor = 0.0;
            assocOrigin1.StackAlignmentPosition = NXOpen.Annotations.StackAlignmentPosition.Above;
            surfaceFinishBuilder1.Origin.SetAssociativeOrigin(assocOrigin1);

            //Point p=null;
            //try
            //{
            //    p.SetCoordinates(point1);
            //}
            //catch (Exception ex)
            //{
            //    UI.GetUI().NXMessageBox.Show("error",NXMessageBox.DialogType.Error,ex.ToString());
            //}
            surfaceFinishBuilder1.Origin.Origin.SetValue(null, nullView, point1);
            if (point!=null)
            {
                Point3d point2 = point.Coordinates;//折线点            
                leaderData1.Leader.SetValue(point, workPart.ModelingViews.WorkView, point2);//折线
            }
            surfaceFinishBuilder1.Origin.SetInferRelativeToGeometry(true);
            bool added1;
            added1 = surfaceFinishBuilder1.AssociatedObjects.Objects.Add(obj);//关联对象
            NXObject nXObject1;
            nXObject1 = surfaceFinishBuilder1.Commit();
            surfaceFinishBuilder1.Destroy();
        }
       
    }

