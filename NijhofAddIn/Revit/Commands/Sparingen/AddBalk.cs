using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NijhofAddIn.Revit.Commands.Sparingen
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class AddBalk : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            // Definieer een filter voor 'Sanitary' systemen
            ParameterValueProvider provider = new ParameterValueProvider(new ElementId(BuiltInParameter.RBS_PIPING_SYSTEM_TYPE_PARAM));
            FilterStringRuleEvaluator evaluator = new FilterStringEquals();
            // Maak de FilterRule aan met de juiste parameters
            FilterRule rule = new FilterStringRule(provider, evaluator, "W5240_Riolering");
            ElementParameterFilter filter = new ElementParameterFilter(rule);

            // Verzamel alle 'Sanitary' pijpen in het hoofdmodel
            FilteredElementCollector sanitaryPipesCollector = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_PipeCurves)
                .OfClass(typeof(Pipe))
                .WherePasses(filter);

            List<ElementId> clashElements = new List<ElementId>();
            StringBuilder clashResults = new StringBuilder();

            // Itereer door elk gelinkt model
            foreach (RevitLinkInstance linkInstance in new FilteredElementCollector(doc).OfClass(typeof(RevitLinkInstance)).Cast<RevitLinkInstance>())
            {
                Document linkedDoc = linkInstance.GetLinkDocument();
                if (linkedDoc == null || !linkedDoc.Title.ToLower().Contains("con")) continue;

                // Verzamel alle structurele framing-elementen in het huidige gelinkte model
                FilteredElementCollector structuralFramingCollector = new FilteredElementCollector(linkedDoc)
                    .OfCategory(BuiltInCategory.OST_StructuralFraming)
                    .OfClass(typeof(FamilyInstance));

                // Voer clashdetectie uit
                foreach (Pipe pipe in sanitaryPipesCollector.Cast<Pipe>())
                {
                    BoundingBoxXYZ pipeBB = pipe.get_BoundingBox(null);
                    foreach (FamilyInstance framing in structuralFramingCollector.Cast<FamilyInstance>())
                    {
                        BoundingBoxXYZ framingBB = framing.get_BoundingBox(null);
                        if (pipeBB != null && framingBB != null && BoundingBoxesIntersect(pipeBB, framingBB))
                        {
                            clashResults.AppendLine($"Clash detected in linked model '{linkedDoc.Title}': Pipe {pipe.Id} with Framing {framing.Id}");
                            clashElements.Add(pipe.Id);
                            clashElements.Add(framing.Id);
                        }
                    }
                }
            }

            // Definieer de familienaam en het type dat je wilt vinden
            string familyName = "Sparingsopgave Fundering";
            string familyType = "Sparing 200";

            // Vind het FamilySymbol voor de "Sparingsopgave Fundering 200" family
            FamilySymbol sparingSymbol = null;
            FilteredElementCollector symbolCollector = new FilteredElementCollector(doc);
            foreach (FamilySymbol fs in symbolCollector.OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_GenericModel).Cast<FamilySymbol>())
            {
                if (fs.FamilyName.Equals(familyName) && fs.Name.Equals(familyType))
                {
                    sparingSymbol = fs;
                    break;
                }
            }

            if (sparingSymbol == null)
            {
                TaskDialog.Show("Error", "Family 'Sparingsopgave Fundering 200' niet gevonden. Laad de family en probeer opnieuw");
                return Result.Failed;
            }

            // Activeer het FamilySymbol binnen een transactie
            using (Transaction trans = new Transaction(doc, "Activeer Sparingsopgave Fundering Symbol"))
            {
                trans.Start();
                if (!sparingSymbol.IsActive)
                {
                    sparingSymbol.Activate();
                    doc.Regenerate();
                }
                trans.Commit();
            }

            // Start een nieuwe transactie om elementen toe te voegen aan het Revit-model
            using (Transaction trans = new Transaction(doc, "Plaats Sparingsopgave Fundering Familie"))
            {
                trans.Start();

                // Verzamel bestaande "Sparingsopgave Fundering" instanties
                List<XYZ> bestaandeSparingLocaties = new FilteredElementCollector(doc)
                    .OfClass(typeof(FamilyInstance))
                    .OfCategory(BuiltInCategory.OST_GenericModel)
                    .Where(x => ((FamilyInstance)x).Symbol.FamilyName == familyName)
                    .Select(x => ((FamilyInstance)x).Location as LocationPoint)
                    .Where(x => x != null)
                    .Select(x => x.Point)
                    .ToList();

                double tolerantie = 0.001; // Definieer een kleine tolerantie voor punten die als gelijk worden beschouwd

                // Itereer door elk gelinkt model dat "con" in de titel bevat
                foreach (RevitLinkInstance linkInstance in new FilteredElementCollector(doc).OfClass(typeof(RevitLinkInstance)).Cast<RevitLinkInstance>())
                {
                    Document linkedDoc = linkInstance.GetLinkDocument();
                    if (linkedDoc == null || !linkedDoc.Title.ToLower().Contains("con")) continue;

                    // Verzamel alle structurele framing-elementen in het huidige gelinkte model
                    FilteredElementCollector structuralFramingCollector = new FilteredElementCollector(linkedDoc)
                        .OfCategory(BuiltInCategory.OST_StructuralFraming)
                        .OfClass(typeof(FamilyInstance));

                    foreach (Pipe pipe in sanitaryPipesCollector.Cast<Pipe>())
                    {
                        BoundingBoxXYZ pipeBB = pipe.get_BoundingBox(null);
                        foreach (FamilyInstance framing in structuralFramingCollector.Cast<FamilyInstance>())
                        {
                            BoundingBoxXYZ framingBB = framing.get_BoundingBox(null);
                            if (pipeBB != null && framingBB != null && BoundingBoxesIntersect(pipeBB, framingBB))
                            {
                                XYZ clashPoint = GetClashPoint(pipeBB, framingBB);

                                if (!bestaandeSparingLocaties.Any(p => IsBijnaGelijk(p, clashPoint, tolerantie)))
                                {
                                    PlaatsSparingInDezelfdeRichtingAlsPijpleiding(doc, sparingSymbol, clashPoint, pipe);
                                    bestaandeSparingLocaties.Add(clashPoint); // Voeg dit punt toe aan de lijst om dubbele plaatsing te voorkomen
                                }
                            }
                        }
                    }
                }

                trans.Commit();
            }

            return Result.Succeeded;
        }

        private XYZ GetClashPoint(BoundingBoxXYZ box1, BoundingBoxXYZ box2)
        {
            return (box1.Min + box1.Max) / 2;
        }

        private bool IsBijnaGelijk(XYZ punt1, XYZ punt2, double tolerantie)
        {
            return (punt1 - punt2).GetLength() < tolerantie;
        }

        private bool BoundingBoxesIntersect(BoundingBoxXYZ box1, BoundingBoxXYZ box2)
        {
            bool separate = box1.Max.X < box2.Min.X || box1.Min.X > box2.Max.X ||
                            box1.Max.Y < box2.Min.Y || box1.Min.Y > box2.Max.Y ||
                            box1.Max.Z < box2.Min.Z || box1.Min.Z > box2.Max.Z;
            return !separate;
        }

        //private void PlaatsSparingInDezelfdeRichtingAlsPijpleiding(Document doc, FamilySymbol sparingSymbol, XYZ clashPoint, Pipe pipe)
        //{
        //    Line line = (pipe.Location as LocationCurve).Curve as Line;
        //    XYZ pipeDirection = (line.GetEndPoint(1) - line.GetEndPoint(0)).Normalize();

        //    XYZ defaultDirection = new XYZ(1, 0, 0);
        //    double angle = Math.Acos(defaultDirection.DotProduct(pipeDirection));
        //    XYZ axis = defaultDirection.CrossProduct(pipeDirection).Normalize();

        //    if (axis.GetLength() < 0.0001)
        //    {
        //        if (defaultDirection.DotProduct(pipeDirection) < 0)
        //        {
        //            axis = XYZ.BasisY;
        //            angle = Math.PI;
        //        }
        //        else
        //        {
        //            axis = XYZ.BasisZ;
        //            angle = 0;
        //        }
        //    }

        //    FamilyInstance instance = doc.Create.NewFamilyInstance(clashPoint, sparingSymbol, StructuralType.NonStructural);
        //    LocationPoint locationPoint = instance.Location as LocationPoint;

        //    if (locationPoint != null && angle > 0.0001)
        //    {
        //        ElementTransformUtils.RotateElement(doc, instance.Id, Line.CreateBound(clashPoint, clashPoint + axis), angle);
        //    }
        //}

        private void PlaatsSparingInDezelfdeRichtingAlsPijpleiding(Document doc, FamilySymbol sparingSymbol, XYZ clashPoint, Pipe pipe)
        {
            if (pipe.Location is LocationCurve locationCurve && locationCurve.Curve is Line line)
            {
                XYZ pipeDirection = (line.GetEndPoint(1) - line.GetEndPoint(0)).Normalize();
                XYZ defaultDirection = new XYZ(1, 0, 0);
                double angle = Math.Acos(defaultDirection.DotProduct(pipeDirection));
                XYZ axis = defaultDirection.CrossProduct(pipeDirection).Normalize();

                if (axis.GetLength() < 0.0001)
                {
                    if (defaultDirection.DotProduct(pipeDirection) < 0)
                    {
                        axis = XYZ.BasisY;
                        angle = Math.PI;
                    }
                    else
                    {
                        axis = XYZ.BasisZ;
                        angle = 0;
                    }
                }

                FamilyInstance instance = doc.Create.NewFamilyInstance(clashPoint, sparingSymbol, StructuralType.NonStructural);

                if (instance.Location is LocationPoint && angle > 0.0001)
                {
                    ElementTransformUtils.RotateElement(doc, instance.Id, Line.CreateBound(clashPoint, clashPoint + axis), angle);
                }
            }
        }
    }
}
