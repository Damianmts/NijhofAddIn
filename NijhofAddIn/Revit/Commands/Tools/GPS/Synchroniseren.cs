using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

namespace NijhofAddIn.Revit.Commands.Tools.GPS
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class Synchroniseren : IExternalCommand
    {
        private static readonly Guid schemaGuid = new Guid("{CACB8CF0-88E6-42C1-83C2-194BC8EE0DB6}");
        private const double Tolerance = 0.001; // Tolerantie voor XYZ-vergelijking

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // Call the synchronization method
            SyncGpsPointsOptimized(doc);

            return Result.Succeeded;
        }

        // Maak het extensible storage schema voor GPS mapping
        private static Schema CreateGpsConnectorSchema()
        {
            SchemaBuilder schemaBuilder = new SchemaBuilder(schemaGuid);
            schemaBuilder.SetSchemaName("GpsConnectorMapping");

            // Voeg velden toe voor de connector-ID en locatie-sleutel
            schemaBuilder.AddSimpleField("ConnectorId", typeof(ElementId));
            schemaBuilder.AddSimpleField("LocationKey", typeof(string)); // Locatie als unieke string

            return schemaBuilder.Finish();
        }

        // Functie om de GPS locatie op te slaan als een unieke string sleutel, alleen voor Cap fittings
        public void StoreConnectorMapping(Element gpsPoint, ElementId connectorId, XYZ origin)
        {
            Schema schema = Schema.Lookup(schemaGuid) ?? CreateGpsConnectorSchema();

            // Creëer een unieke sleutel voor locatie door XYZ om te zetten naar een string
            string locationKey = $"{origin.X}_{origin.Y}_{origin.Z}";

            Entity entity = new Entity(schema);
            entity.Set("ConnectorId", connectorId);
            entity.Set("LocationKey", locationKey);

            gpsPoint.SetEntity(entity); // Sla de gegevens op in extensible storage
        }

        // Functie om de GPS-punten te synchroniseren
        public void SyncGpsPointsOptimized(Document doc)
        {
            Schema schema = Schema.Lookup(schemaGuid);
            if (schema == null)
            {
                TaskDialog.Show("Error", "Schema niet gevonden. Voeg eerst de koppelingen toe.");
                return;
            }

            // Verzamel alle GPS-punten in het model
            FilteredElementCollector gpsCollector = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilyInstance))
                .OfCategory(BuiltInCategory.OST_GenericModel);

            Dictionary<ElementId, XYZ> connectorLocationCache = new Dictionary<ElementId, XYZ>();
            List<Element> gpsPointsToSync = new List<Element>();

            using (Transaction trans = new Transaction(doc, "Synchronize GPS Points"))
            {
                trans.Start();

                foreach (Element gpsPoint in gpsCollector)
                {
                    Entity entity = gpsPoint.GetEntity(schema);
                    if (entity.IsValid())
                    {
                        ElementId connectorId = entity.Get<ElementId>("ConnectorId");
                        string storedLocationKey = entity.Get<string>("LocationKey");

                        if (!connectorLocationCache.ContainsKey(connectorId))
                        {
                            // Haal de locatie van de Cap-fitting connector op
                            Element connectorElement = doc.GetElement(connectorId);
                            XYZ connectorLocation = GetConnectorOriginForCap(connectorElement);

                            if (connectorLocation != null)
                            {
                                connectorLocationCache[connectorId] = connectorLocation;

                                // Aanroep van StoreConnectorMapping met XYZ als derde parameter binnen een actieve transactie
                                StoreConnectorMapping(gpsPoint, connectorId, connectorLocation);
                            }
                        }

                        // Vergelijk de opgeslagen positie met de huidige positie
                        if (connectorLocationCache.ContainsKey(connectorId))
                        {
                            XYZ storedLocation = ParseXYZ(storedLocationKey);
                            XYZ currentLocation = connectorLocationCache[connectorId];

                            // Check of de opgeslagen locatie en huidige locatie verschillend zijn binnen de tolerantie
                            if (!ArePointsEqual(storedLocation, currentLocation, Tolerance))
                            {
                                gpsPointsToSync.Add(gpsPoint);
                            }
                        }
                    }
                }

                // Verplaats GPS-punten die moeten worden gesynchroniseerd
                foreach (Element gpsPoint in gpsPointsToSync)
                {
                    Entity entity = gpsPoint.GetEntity(schema);
                    ElementId connectorId = entity.Get<ElementId>("ConnectorId");

                    if (connectorLocationCache.ContainsKey(connectorId))
                    {
                        LocationPoint gpsLocation = gpsPoint.Location as LocationPoint;
                        if (gpsLocation != null)
                        {
                            gpsLocation.Point = connectorLocationCache[connectorId];
                        }
                    }
                }

                trans.Commit();
            }

            TaskDialog.Show("Resultaat", $"{gpsPointsToSync.Count} GPS-punten succesvol gesynchroniseerd.");
        }

        // Hulpmethode om een XYZ te parseren vanuit een string
        private XYZ ParseXYZ(string locationKey)
        {
            string[] values = locationKey.Split('_');
            return new XYZ(double.Parse(values[0]), double.Parse(values[1]), double.Parse(values[2]));
        }

        // Hulpmethode om twee XYZ-punten te vergelijken met een tolerantie
        private bool ArePointsEqual(XYZ point1, XYZ point2, double tolerance)
        {
            return point1.DistanceTo(point2) < tolerance;
        }

        // Functie om de locatie van een Cap-fitting connector op te halen
        private XYZ GetConnectorOriginForCap(Element element)
        {
            if (element is FamilyInstance familyInstance && familyInstance.MEPModel is MechanicalFitting fitting && fitting.PartType == PartType.Cap)
            {
                ConnectorManager connectorManager = familyInstance.MEPModel.ConnectorManager;
                if (connectorManager != null)
                {
                    foreach (Connector connector in connectorManager.Connectors)
                    {
                        // Geef de locatie van de eerste connector van de Cap terug
                        return connector.Origin;
                    }
                }
            }
            return null;
        }
    }
}
