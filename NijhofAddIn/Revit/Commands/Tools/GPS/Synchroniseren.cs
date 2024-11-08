using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

namespace NijhofAddIn.Revit.Commands.Tools.GPS
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class Synchroniseren : IExternalCommand
    {
        private static readonly Guid schemaGuid = new Guid("AC649BD8-2E9B-4FA6-B57C-7AB379F1ADFE");

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

        // Functie om de GPS locatie op te slaan als een unieke string sleutel
        public void StoreConnectorMapping(Element gpsPoint, ElementId connectorId, XYZ initialLocation)
        {
            Schema schema = Schema.Lookup(schemaGuid) ?? CreateGpsConnectorSchema();

            // Creëer een unieke sleutel voor locatie door XYZ om te zetten naar een string
            string locationKey = $"{initialLocation.X}_{initialLocation.Y}_{initialLocation.Z}";

            Entity entity = new Entity(schema);
            entity.Set("ConnectorId", connectorId);
            entity.Set("LocationKey", locationKey);

            // Sla de gegevens op in extensible storage zonder een extra transactie
            gpsPoint.SetEntity(entity);
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

            FilteredElementCollector gpsCollector = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilyInstance))
                .OfCategory(BuiltInCategory.OST_GenericModel);

            Dictionary<ElementId, XYZ> connectorLocationCache = new Dictionary<ElementId, XYZ>();
            List<Element> gpsPointsToSync = new List<Element>();

            foreach (Element gpsPoint in gpsCollector)
            {
                Entity entity = gpsPoint.GetEntity(schema);
                if (entity.IsValid())
                {
                    ElementId connectorId = entity.Get<ElementId>("ConnectorId");
                    string storedLocationKey = entity.Get<string>("LocationKey");

                    // Bereken de huidige locatie en genereer de bijbehorende sleutel
                    XYZ currentLocation = (gpsPoint.Location as LocationPoint)?.Point;
                    if (currentLocation == null) continue;

                    string currentLocationKey = $"{currentLocation.X}_{currentLocation.Y}_{currentLocation.Z}";

                    // Laad de locatie van de connector in de cache indien nog niet aanwezig
                    if (!connectorLocationCache.ContainsKey(connectorId))
                    {
                        Element connector = doc.GetElement(connectorId);
                        if (connector != null)
                        {
                            connectorLocationCache[connectorId] = (connector.Location as LocationPoint)?.Point;
                        }
                    }

                    // Controleer of de GPS-punt verplaatst moet worden
                    if (connectorLocationCache.ContainsKey(connectorId) &&
                        storedLocationKey != currentLocationKey)
                    {
                        gpsPointsToSync.Add(gpsPoint);
                    }
                }
            }

            using (Transaction trans = new Transaction(doc, "Synchronize GPS Points"))
            {
                trans.Start();
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
        }
    }
}
