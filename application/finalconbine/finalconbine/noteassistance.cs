using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using NXOpen;
using NXOpenUI;
using NXOpen.UF;
using NXOpen.Utilities;
using NXOpen.Features;
using NXOpen.Annotations;

 class noteassistance
 {

        public static Session theSession = Session.GetSession();
        public Part workPart = theSession.Parts.Work;
        public Part displayPart = theSession.Parts.Display;

        public void function_note(string[] zhushiwenzi, DisplayableObject guanlian, NXObject zhiyinobj, Point3d placeptobj)
        {
            NXOpen.Annotations.SimpleDraftingAid nullAnnotations_SimpleDraftingAid = null;
            NXOpen.Annotations.PmiNoteBuilder pmiNoteBuilder1;
            pmiNoteBuilder1 = workPart.Annotations.CreatePmiNoteBuilder(nullAnnotations_SimpleDraftingAid);

            pmiNoteBuilder1.Origin.SetInferRelativeToGeometry(true);

            pmiNoteBuilder1.Origin.Anchor = NXOpen.Annotations.OriginBuilder.AlignmentPosition.MidCenter;

            pmiNoteBuilder1.TextAlignment = NXOpen.Annotations.DraftingNoteBuilder.TextAlign.Middle;

            pmiNoteBuilder1.Origin.Plane.PlaneMethod = NXOpen.Annotations.PlaneBuilder.PlaneMethodType.ModelView;

            pmiNoteBuilder1.Origin.SetInferRelativeToGeometry(true);

            NXOpen.Annotations.LeaderData leaderData1;
            leaderData1 = workPart.Annotations.CreateLeaderData();

            leaderData1.Arrowhead = NXOpen.Annotations.LeaderData.ArrowheadType.FilledArrow;

            pmiNoteBuilder1.Leader.Leaders.Append(leaderData1);

            leaderData1.StubSide = NXOpen.Annotations.LeaderSide.Inferred;

            leaderData1.StubSize = 5.0;

            leaderData1.Perpendicular = false;

            //double symbolscale1;
            //symbolscale1 = pmiNoteBuilder1.Text.TextBlock.SymbolScale;

            //double symbolaspectratio1;
            //symbolaspectratio1 = pmiNoteBuilder1.Text.TextBlock.SymbolAspectRatio;

            Xform xform1;
            xform1 = workPart.Annotations.GetDefaultAnnotationPlane(NXOpen.Annotations.PmiDefaultPlane.ModelView);

            pmiNoteBuilder1.Origin.SetInferRelativeToGeometry(true);

            //pmiNoteBuilder1.Origin.SetInferRelativeToGeometry(true);
            
            pmiNoteBuilder1.Text.TextBlock.SetText(zhushiwenzi);

            //NXOpen.Annotations.Annotation.AssociativeOriginData assocOrigin1;
            //assocOrigin1.OriginType = NXOpen.Annotations.AssociativeOriginType.Drag;
            //NXOpen.View nullView = null;
            //assocOrigin1.View = nullView;
            //assocOrigin1.ViewOfGeometry = nullView;
            //NXOpen.Point nullPoint = null;
            //assocOrigin1.PointOnGeometry = nullPoint;
            //assocOrigin1.VertAnnotation = null;
            //assocOrigin1.VertAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
            //assocOrigin1.HorizAnnotation = null;
            //assocOrigin1.HorizAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
            //assocOrigin1.AlignedAnnotation = null;
            //assocOrigin1.DimensionLine = 0;
            //assocOrigin1.AssociatedView = nullView;
            //assocOrigin1.AssociatedPoint = nullPoint;
            //assocOrigin1.OffsetAnnotation = null;
            //assocOrigin1.OffsetAlignmentPosition = NXOpen.Annotations.AlignmentPosition.TopLeft;
            //assocOrigin1.XOffsetFactor = 0.0;
            //assocOrigin1.YOffsetFactor = 0.0;
            //assocOrigin1.StackAlignmentPosition = NXOpen.Annotations.StackAlignmentPosition.Above;
            //pmiNoteBuilder1.Origin.SetAssociativeOrigin(assocOrigin1);

            bool added1;
           added1 = pmiNoteBuilder1.AssociatedObjects.Objects.Add(guanlian);
            

                pmiNoteBuilder1.Origin.Origin.SetValue(null, null, placeptobj);

                pmiNoteBuilder1.Origin.SetInferRelativeToGeometry(true);
                if (zhiyinobj != null)
                {
                    Point zhiyin = (Point)zhiyinobj;
                    Point3d zhiyindian = zhiyin.Coordinates;
                    //NXOpen.Features.Extrude extrude1 = (NXOpen.Features.Extrude)workPart.Features.FindObject("EXTRUDE(3)");
                    //Edge edge1 = (Edge)extrude1.FindObject("EDGE * 120 * 140 {(11,2,-5)(11,0.5,-5)(11,-1,-5) EXTRUDE(2)}");
                    //edge1.GetVertices();
                    leaderData1.Leader.SetValue(zhiyin, workPart.ModelingViews.WorkView, zhiyindian);
                }
                NXObject nXObject1;
                nXObject1 = pmiNoteBuilder1.Commit();

                pmiNoteBuilder1.Destroy();

            }


      public void Xmlforcapp(string value,string Innertext)
          {
       string folderpath = "3dppmplugin\\";
          string  cappxml = "CAPP.xml";
              XmlDocument doc = new XmlDocument();
              doc.Load(finalconbine.ApplicationPath + folderpath + cappxml);
             
              int m = doc.SelectSingleNode("/CAPPAssistant").ChildNodes.Count;


              // TreeNode[] n=new TreeNode[m];
              for (int i = 1; i < m + 1; i++)
              {
                  try
                  {
                      XmlNode node = doc.SelectSingleNode("/CAPPAssistant/Type[" + i + "]");
                      int num = doc.SelectSingleNode("/CAPPAssistant/Type[" + i + "]").ChildNodes.Count;
                      if (node.Attributes["name"].Value.ToString() == value)
                      {
                          XmlElement xe = doc.CreateElement("Item");
                          xe.SetAttribute("remark","");
                          xe.SetAttribute("text",Innertext);
                          node.AppendChild(xe);
                          doc.Save(finalconbine.ApplicationPath + folderpath + cappxml);                   
                      }


                  }
                  catch (Exception error)
                  {
                      MessageBox.Show(error.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  }
              }
      }
      public void Xmlforcappdelet(string value, string Innertext)
      {
          string folderpath = "3dppmplugin\\";
          string cappxml = "CAPP.xml";
          XmlDocument doc = new XmlDocument();
          doc.Load(finalconbine.ApplicationPath + folderpath + cappxml);

          int m = doc.SelectSingleNode("/CAPPAssistant").ChildNodes.Count;


          // TreeNode[] n=new TreeNode[m];

          for (int i = 1; i < m + 1; i++)
          {
              try
              {
                  XmlNode node = doc.SelectSingleNode("/CAPPAssistant/Type[" + i + "]");
                  int num = doc.SelectSingleNode("/CAPPAssistant/Type[" + i + "]").ChildNodes.Count;
                  if (node.Attributes["name"].Value.ToString() == value)
                  {

                      XmlNode nodedelet = doc.SelectSingleNode("/CAPPAssistant/Type[" + i + "]/Item[@text='" + Innertext + "']");
                      if (nodedelet != null)
                      {
                          node.RemoveChild(nodedelet);
                          doc.Save(finalconbine.ApplicationPath + folderpath + cappxml);
                          break;
                      }
                      else
                      {
                          MessageBox.Show("存在无效删除，请查证后继续?");
                          break;
                      }
                  }


              }
              catch (Exception error)
              {
                  MessageBox.Show(error.Message, "警ˉ告?", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
          }
      }



        
 }

