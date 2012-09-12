using System;
using System.Collections.Generic;
using System.Text;
using NXOpen;
using NXOpenUI;
using NXOpen.BlockStyler;
using NXOpen.UF;

    class pubfun
    {
        private static Session theSession = Session.GetSession();
        private static UFSession theUfSession = UFSession.GetUFSession();
        private Part workPart = theSession.Parts.Work;
        private Part displayPart = theSession.Parts.Display;
        // private static UI theUI = null;


        public void function(string ToleranceValue, string PrimaryDatumReference, string SecondaryDatumReference, string TertiaryDatumReference, object ZoneShape, object MaterialModifier, object PrimaryMaterialCondition, object SecondaryMaterialCondition, object TertiaryMaterialCondition,
            object frameStyle, double Annotationletter, double duanxian, Point3d point1, DisplayableObject point2, DisplayableObject guanlian, object LeaderType)
        {

            NXOpen.Annotations.Fcf nullAnnotations_Fcf = null;
            NXOpen.Annotations.PmiFeatureControlFrameBuilder pmiFeatureControlFrameBuilder1;
            pmiFeatureControlFrameBuilder1 = workPart.Annotations.CreatePmiFeatureControlFrameBuilder(nullAnnotations_Fcf);

            pmiFeatureControlFrameBuilder1.Origin.SetInferRelativeToGeometry(true);

            pmiFeatureControlFrameBuilder1.Origin.Anchor = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;

            pmiFeatureControlFrameBuilder1.Characteristic = NXOpen.Annotations.FeatureControlFrameBuilder.FcfCharacteristic.ProfileOfASurface;


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

            featureControlFrameDataBuilder1.MaterialModifier = NXOpen.Annotations.FeatureControlFrameDataBuilder.ToleranceMaterialModifier.RegardlessOfFeatureSize;
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
       
    }

