using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NijhofAddIn.Revit.Commands.GPS
{
    #region Add Alles
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class AddAlles : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                AddRiool cmd1 = new AddRiool();
                AddLucht cmd2 = new AddLucht();
                AddKoudWater cmd3 = new AddKoudWater();
                AddWarmWater cmd4 = new AddWarmWater();

                cmd1.Execute(commandData, ref message, elements);
                cmd2.Execute(commandData, ref message, elements);
                cmd3.Execute(commandData, ref message, elements);
                cmd4.Execute(commandData, ref message, elements);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }
    #endregion

    #region Add Riool
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class AddRiool : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            /// Vind het FamilySymbol voor de "GPS Riool" family
            FamilySymbol gpsRioolSymbol = null;
            FilteredElementCollector symbolCollector = new FilteredElementCollector(doc);
            foreach (FamilySymbol fs in symbolCollector.OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_GenericModel))
            {
                if (fs.FamilyName == "GPS Riool")
                {
                    gpsRioolSymbol = fs;
                    break;
                }
            }

            if (gpsRioolSymbol == null)
            {
                TaskDialog.Show("Error", "Family 'GPS Riool' niet gevonden. Laad de family en probeer opnieuw");
                return Result.Failed;
            }

            /// Activeer het FamilySymbol binnen een transactie
            using (Transaction trans = new Transaction(doc, "Activeer GPS Riool Symbol"))
            {
                trans.Start();
                if (!gpsRioolSymbol.IsActive)
                {
                    gpsRioolSymbol.Activate();
                    doc.Regenerate();
                }
                trans.Commit();
            }

            /// Verzamel alle "SA_End Piece Cover_Circular_MEPcontent_DYKA PE Binnenriolering_Lid" elementen van de categorie "Pipe Fittings"
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_PipeFitting);

            IList<Element> PipeFittings = collector
                .Where(x => x.Name == "SA_End Piece Cover_Circular_MEPcontent_DYKA PE Binnenriolering_Lid")
                .ToList();

            using (Transaction trans = new Transaction(doc, "Plaats element op Speciedeksel"))
            {
                trans.Start();

                foreach (Element Speciedeksel in PipeFittings)
                {
                    LocationPoint locationPoint = Speciedeksel.Location as LocationPoint;
                    if (locationPoint != null)
                    {
                        XYZ point = locationPoint.Point;
                        /// Plaats de "GPS Lucht_Riool" op de locatie van het speciedeksel
                        doc.Create.NewFamilyInstance(point, gpsRioolSymbol, StructuralType.NonStructural);
                    }
                }

                trans.Commit();
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region Add Lucht
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class AddLucht : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            /// Vind het FamilySymbol voor de "GPS Lucht" family
            FamilySymbol gpsLuchtSymbol = null;
            FilteredElementCollector symbolCollector = new FilteredElementCollector(doc);
            foreach (FamilySymbol fs in symbolCollector.OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_GenericModel))
            {
                if (fs.FamilyName == "GPS Lucht")
                {
                    gpsLuchtSymbol = fs;
                    break;
                }
            }

            if (gpsLuchtSymbol == null)
            {
                TaskDialog.Show("Error", "Family 'GPS Lucht' niet gevonden. Laad de family en probeer opnieuw");
                return Result.Failed;
            }

            /// Activeer het FamilySymbol binnen een transactie
            using (Transaction trans = new Transaction(doc, "Activeer GPS Lucht Symbol"))
            {
                trans.Start();
                if (!gpsLuchtSymbol.IsActive)
                {
                    gpsLuchtSymbol.Activate();
                    doc.Regenerate();
                }
                trans.Commit();
            }

            /// Filter om alleen "Air Terminal" elementen te verzamelen.
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilyInstance));
            collector.OfCategory(BuiltInCategory.OST_DuctTerminal);

            /// Start een nieuwe transactie om wijzigingen in het document te maken
            using (Transaction trans = new Transaction(doc))
            {
                trans.Start("Plaats GPS Lucht op Air Terminals");

                foreach (FamilyInstance terminal in collector)
                {
                    LocationPoint locationPoint = terminal.Location as LocationPoint;
                    if (locationPoint != null)
                    {
                        XYZ point = locationPoint.Point;
                        /// Plaats de "GPS Lucht" op de locatie van het ventiel
                        doc.Create.NewFamilyInstance(point, gpsLuchtSymbol, StructuralType.NonStructural);
                    }
                }

                trans.Commit();
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region Add Koudwater
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class AddKoudWater : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            /// Zoek het Generic Model type met de naam "GPS Koudwater"
            FamilySymbol gpsKoudwaterType = (FamilySymbol)new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_GenericModel)
                .FirstOrDefault(e => e.Name.Equals("GPS Koudwater"));

            if (gpsKoudwaterType == null)
            {
                TaskDialog.Show("Error", "Family 'GPS Koudwater' niet gevonden in het project. Laad de family en probeer opnieuw");
                return Result.Failed;
            }

            /// Verzamel alle leidingen met systeemclassificatie "Domestic Cold Water"
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> pipes = collector.OfClass(typeof(Pipe)).OfCategory(BuiltInCategory.OST_PipeCurves)
                .Where(e => e.LookupParameter("System Classification").AsString() == "Domestic Cold Water").ToList();

            /// Start de transactie om wijzigingen in het Revit-document aan te brengen
            using (Transaction trans = new Transaction(doc))
            {
                trans.Start("Plaats GPS Koudwater op het hoogste open uiteinde van verticale Domestic Cold Water leidingen");

                if (!gpsKoudwaterType.IsActive)
                    gpsKoudwaterType.Activate();

                foreach (Pipe pipe in pipes.Cast<Pipe>())
                {
                    Line line = (pipe.Location as LocationCurve).Curve as Line;
                    XYZ direction = line.Direction;

                    /// Bepaal of de leiding verticaal is
                    bool isVertical = Math.Abs(direction.Z) > Math.Abs(direction.X) && Math.Abs(direction.Z) > Math.Abs(direction.Y);

                    if (isVertical)
                    {
                        Connector highestConnector = null;
                        double highestElevation = double.MinValue;

                        foreach (Connector connector in pipe.ConnectorManager.Connectors)
                        {
                            if (connector.ConnectorType == ConnectorType.End && !connector.IsConnected)
                            {
                                if (highestConnector == null || connector.Origin.Z > highestElevation)
                                {
                                    highestConnector = connector;
                                    highestElevation = connector.Origin.Z;
                                }
                            }
                        }

                        if (highestConnector != null)
                        {
                            /// Plaats het 'GPS Koudwater' model op het hoogste open uiteinde van de verticale leiding
                            XYZ location = highestConnector.Origin;
                            FamilyInstance instance = doc.Create.NewFamilyInstance(location, gpsKoudwaterType, StructuralType.NonStructural);
                        }
                    }
                }

                trans.Commit();
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region Add Warmwater
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class AddWarmWater : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            /// Zoek het Generic Model type met de naam "GPS Warmwater"
            FamilySymbol gpsWarmwaterType = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_GenericModel)
                .FirstOrDefault(e => e.Name.Equals("GPS Warmwater")) as FamilySymbol;

            if (gpsWarmwaterType == null)
            {
                TaskDialog.Show("Error", "Family 'GPS Warmwater' niet gevonden in het project. Laad de family en probeer opnieuw");
                return Result.Failed;
            }

            /// Verzamel alle leidingen met systeemclassificatie "Domestic Hot Water"
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> pipes = collector.OfClass(typeof(Pipe)).OfCategory(BuiltInCategory.OST_PipeCurves)
                .Where(e => e.LookupParameter("System Classification").AsString() == "Domestic Hot Water").ToList();

            /// Start de transactie om wijzigingen in het Revit-document aan te brengen
            using (Transaction trans = new Transaction(doc))
            {
                trans.Start("Plaats GPS Warmwater op het hoogste open uiteinde van verticale Domestic Hot Water leidingen");

                if (!gpsWarmwaterType.IsActive)
                    gpsWarmwaterType.Activate();

                foreach (Pipe pipe in pipes)
                {
                    Line line = (pipe.Location as LocationCurve).Curve as Line;
                    XYZ direction = line.Direction;

                    /// Bepaal of de leiding verticaal is
                    bool isVertical = Math.Abs(direction.Z) > Math.Abs(direction.X) && Math.Abs(direction.Z) > Math.Abs(direction.Y);

                    if (isVertical)
                    {
                        Connector highestConnector = null;
                        double highestElevation = double.MinValue;

                        foreach (Connector connector in pipe.ConnectorManager.Connectors)
                        {
                            if (connector.ConnectorType == ConnectorType.End && !connector.IsConnected)
                            {
                                if (highestConnector == null || connector.Origin.Z > highestElevation)
                                {
                                    highestConnector = connector;
                                    highestElevation = connector.Origin.Z;
                                }
                            }
                        }

                        if (highestConnector != null)
                        {
                            /// Plaats het 'GPS Warmwater' model op het hoogste open uiteinde van de verticale leiding
                            XYZ location = highestConnector.Origin;
                            FamilyInstance instance = doc.Create.NewFamilyInstance(location, gpsWarmwaterType, StructuralType.NonStructural);
                        }
                    }
                }

                trans.Commit();
            }

            return Result.Succeeded;
        }
    }
    #endregion
}
