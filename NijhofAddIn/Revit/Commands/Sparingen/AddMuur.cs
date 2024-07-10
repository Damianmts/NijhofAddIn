using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NijhofAddIn.Revit.Commands.Sparingen
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class AddMuur : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            try
            {
                FilteredElementCollector pipeCollector = new FilteredElementCollector(doc)
                    .OfClass(typeof(Pipe));

                FilteredElementCollector linkCollector = new FilteredElementCollector(doc)
                    .OfClass(typeof(RevitLinkInstance));

                foreach (RevitLinkInstance linkInstance in linkCollector)
                {
                    Document linkedDoc = linkInstance.GetLinkDocument();
                    if (linkedDoc == null) continue;

                    FilteredElementCollector wallCollector = new FilteredElementCollector(linkedDoc)
                        .OfClass(typeof(Wall));

                    Transform transform = linkInstance.GetTransform();

                    foreach (Pipe pipe in pipeCollector)
                    {
                        if (!IsSanitaryM524(pipe)) continue;

                        GeometryElement pipeGeometryElement = pipe.get_Geometry(new Options());
                        List<Solid> intersectionSolids = new List<Solid>();

                        foreach (GeometryObject geometryObject in pipeGeometryElement)
                        {
                            Solid geometrySolid = geometryObject as Solid;
                            if (geometrySolid == null) continue;

                            foreach (Wall wall in wallCollector)
                            {
                                GeometryElement wallGeometryElement = wall.get_Geometry(new Options());
                                foreach (GeometryObject wallGeometryObject in wallGeometryElement)
                                {
                                    Solid wallSolid = wallGeometryObject as Solid;
                                    if (wallSolid == null) continue;

                                    Solid transformedWallSolid = SolidUtils.CreateTransformed(wallSolid, transform);
                                    Solid intersectionSolid = BooleanOperationsUtils.ExecuteBooleanOperation(geometrySolid, transformedWallSolid, BooleanOperationsType.Intersect);

                                    if (intersectionSolid != null && intersectionSolid.Volume > 0)
                                    {
                                        intersectionSolids.Add(intersectionSolid);
                                    }
                                }
                            }
                        }

                        Solid combinedSolid = CombineSolids(doc, intersectionSolids);
                        if (combinedSolid != null && combinedSolid.Volume > 0)
                        {
                            XYZ intersectionPoint = FindIntersectionPoint(combinedSolid);

                            if (!DoesElementExistAtPoint(doc, intersectionPoint, "Sparingsopgave Fundering", "Sparing ø200"))
                            {
                                Transaction trans = new Transaction(doc, "Place Element");
                                trans.Start();

                                FamilySymbol familySymbol = new FilteredElementCollector(doc)
                                    .OfClass(typeof(FamilySymbol))
                                    .OfCategory(BuiltInCategory.OST_GenericModel)
                                    .Cast<FamilySymbol>()
                                    .FirstOrDefault(s => s.Family.Name == "Sparingsopgave Fundering" && s.Name == "Sparing ø200");

                                if (familySymbol != null)
                                {
                                    if (!familySymbol.IsActive)
                                        familySymbol.Activate();

                                    doc.Create.NewFamilyInstance(intersectionPoint, familySymbol, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                                }
                                else
                                {
                                    TaskDialog.Show("Error", "Kan de gespecificeerde familie en type niet vinden.");
                                }

                                trans.Commit();
                            }
                        }
                    }
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }

        private bool IsSanitaryM524(Pipe pipe)
        {
            string systemClassification = pipe.get_Parameter(BuiltInParameter.RBS_PIPING_SYSTEM_TYPE_PARAM).AsValueString();
            Parameter systemAbbreviationParam = pipe.LookupParameter("System Abbreviation");
            string systemAbbreviation = systemAbbreviationParam != null ? systemAbbreviationParam.AsString() : "";

            return systemClassification.Equals("Sanitary", StringComparison.OrdinalIgnoreCase) &&
                   systemAbbreviation != null && systemAbbreviation.Equals("M524", StringComparison.OrdinalIgnoreCase);
        }

        private Solid CombineSolids(Document doc, List<Solid> solids)
        {
            if (solids == null || solids.Count == 0) return null;

            Solid combinedSolid = solids[0];
            for (int i = 1; i < solids.Count; i++)
            {
                // Combineer de solids. Let op: BooleanOperationsUtils.ExecuteBooleanOperation kan null teruggeven als de operatie faalt.
                Solid tempSolid = BooleanOperationsUtils.ExecuteBooleanOperation(combinedSolid, solids[i], BooleanOperationsType.Union);
                if (tempSolid != null)
                {
                    combinedSolid = tempSolid;
                }
            }

            return combinedSolid;
        }

        private XYZ FindIntersectionPoint(Solid solid)
        {
            if (solid == null || solid.Faces.Size == 0)
            {
                return null;
            }

            XYZ centroid = new XYZ(0, 0, 0);
            double totalArea = 0;

            foreach (Face face in solid.Faces)
            {
                XYZ faceCentroid = GetFaceCentroid(face);
                double faceArea = face.Area;

                if (faceCentroid != null)
                {
                    centroid += faceCentroid * faceArea;
                    totalArea += faceArea;
                }
            }

            return totalArea > 0 ? centroid / totalArea : null;
        }

        private XYZ GetFaceCentroid(Face face)
        {
            XYZ centroid = new XYZ(0, 0, 0);
            double totalArea = 0.0;

            Mesh mesh = face.Triangulate();
            for (int i = 0; i < mesh.NumTriangles; i++)
            {
                MeshTriangle triangle = mesh.get_Triangle(i);
                XYZ v1 = triangle.get_Vertex(0);
                XYZ v2 = triangle.get_Vertex(1);
                XYZ v3 = triangle.get_Vertex(2);

                XYZ triangleCentroid = (v1 + v2 + v3) / 3.0;
                double triangleArea = 0.5 * ((v2 - v1).CrossProduct(v3 - v1)).GetLength();
                centroid += triangleCentroid * triangleArea;
                totalArea += triangleArea;
            }

            return totalArea > 0 ? centroid / totalArea : null;
        }

        private bool DoesElementExistAtPoint(Document doc, XYZ point, string familyName, string typeName, double tolerance = 0.1)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilyInstance))
                .OfCategory(BuiltInCategory.OST_GenericModel)
                .WhereElementIsNotElementType();

            foreach (Element elem in collector)
            {
                FamilyInstance fi = elem as FamilyInstance;
                if (fi != null && fi.Symbol.Family.Name == familyName && fi.Name == typeName)
                {
                    if (fi.Location is LocationPoint locPoint)
                    {
                        if (locPoint.Point.DistanceTo(point) < tolerance)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
