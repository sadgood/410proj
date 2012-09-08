using System;
using System.Collections.Generic;
using System.Text;
using NXOpen;
using NXOpenUI;
using NXOpen.BlockStyler;
using NXOpen.UF;

public class pubfun
{
    private static Session theSession = Session.GetSession();
    private Part workPart = theSession.Parts.Work;
    private Part displayPart = theSession.Parts.Display;
   // private static UI theUI = null;


    public void function(string ToleranceValue, string PrimaryDatumReference, string SecondaryDatumReference, string TertiaryDatumReference, object ZoneShape, object MaterialModifier, object PrimaryMaterialCondition, object SecondaryMaterialCondition, object TertiaryMaterialCondition,
        object frameStyle, double Annotationletter, double duanxian, Point3d point1, Point3d point2, NXObject guanlian)
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

            leaderData1.StubSize =duanxian;

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

            if (guanlian!=null)
            {
                Type a = null; 
                a = guanlian.GetType();
                if (a.Name == "Body")
                { 
                Body guanl = (Body)guanlian; 
                bool added1;
                added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }//关于关联的代码
                else if (a.Name == "Face")
                { 
                Face guanl = (Face)guanlian; 
                bool added1;
                added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
                else if (a.Name == "Curve")
                {
                    Curve guanl = (Curve)guanlian;
                    bool added1;
                    added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
                else if (a.Name == "Line")
                {
                    Line guanl = (Line)guanlian;
                    bool added1;
                    added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
                else if (a.Name == "Point")
                {
                    Point guanl = (Point)guanlian;
                    bool added1;
                    added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
                else if (a.Name == "PmiParallelDimension")
                {
                    NXOpen.Annotations.PmiParallelDimension guanl = (NXOpen.Annotations.PmiParallelDimension)guanlian;
                    bool added1;
                    added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
                else if (a.Name == "PmiArcLengthDimension")
                {
                    NXOpen.Annotations.PmiArcLengthDimension guanl = (NXOpen.Annotations.PmiArcLengthDimension)guanlian;
                    bool added1;
                    added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
                else if (a.Name == "PmiBaselineDimension")
                {
                    NXOpen.Annotations.PmiBaselineDimension guanl = (NXOpen.Annotations.PmiBaselineDimension)guanlian;
                    bool added1;
                    added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
                else if (a.Name == "PmiChainDimension")
                {
                    NXOpen.Annotations.PmiChainDimension guanl = (NXOpen.Annotations.PmiChainDimension)guanlian;
                    bool added1;
                    added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
                else if (a.Name == "PmiChamferDimension")
                {
                    NXOpen.Annotations.PmiChamferDimension guanl = (NXOpen.Annotations.PmiChamferDimension)guanlian;
                    bool added1;
                    added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
                else if (a.Name == "PmiConcentricCircleDimension")
                {
                    NXOpen.Annotations.PmiConcentricCircleDimension guanl = (NXOpen.Annotations.PmiConcentricCircleDimension)guanlian;
                    bool added1;
                    added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
                else if (a.Name == "PmiCylindricalDimension")
                {
                    NXOpen.Annotations.PmiCylindricalDimension guanl = (NXOpen.Annotations.PmiCylindricalDimension)guanlian;
                    bool added1;
                    added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
                else if (a.Name == "PmiDiameterDimension")
                {
                    NXOpen.Annotations.PmiDiameterDimension guanl = (NXOpen.Annotations.PmiDiameterDimension)guanlian;
                    bool added1;
                    added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
                else if (a.Name == "PmiHoleDimension")
                {
                    NXOpen.Annotations.PmiHoleDimension guanl = (NXOpen.Annotations.PmiHoleDimension)guanlian;
                    bool added1;
                    added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
                else if (a.Name == "PmiFoldedRadiusDimension")
                {
                    NXOpen.Annotations.PmiFoldedRadiusDimension guanl = (NXOpen.Annotations.PmiFoldedRadiusDimension)guanlian;
                    bool added1;
                    added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
                else if (a.Name == "PmiHorizontalDimension")
                {
                    NXOpen.Annotations.PmiHorizontalDimension guanl = (NXOpen.Annotations.PmiHorizontalDimension)guanlian;
                    bool added1;
                    added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
                else if (a.Name == "PmiHorizontalOrdinateDimension")
                {
                    NXOpen.Annotations.PmiHorizontalOrdinateDimension guanl = (NXOpen.Annotations.PmiHorizontalOrdinateDimension)guanlian;
                    bool added1;
                    added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
                else if (a.Name == "PmiMajorAngularDimension")
                {
                    NXOpen.Annotations.PmiMajorAngularDimension guanl = (NXOpen.Annotations.PmiMajorAngularDimension)guanlian;
                    bool added1;
                    added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
                else if (a.Name == "PmiMinorAngularDimension")
                {
                    NXOpen.Annotations.PmiMinorAngularDimension guanl = (NXOpen.Annotations.PmiMinorAngularDimension)guanlian;
                    bool added1;
                    added1 = pmiFeatureControlFrameBuilder1.AssociatedObjects.Objects.Add(guanl);
                }
               


            }
            //NXOpen.Point point4;
            //point4 = workPart.Points.CreatePoint(point1);
            pmiFeatureControlFrameBuilder1.Origin.Origin.SetValue(null, nullView, point1);
           
           
            NXOpen.Point point3;
            point3 = workPart.Points.CreatePoint(point2);


           
            leaderData1.Leader.SetValue(point3, workPart.ModelingViews.WorkView, point2);//折线


            // leaderData1.TerminatorType = NXOpen.Annotations.LeaderData.LeaderType.Flag;//标志
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