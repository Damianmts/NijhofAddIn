using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NijhofAddIn.Revit.Commands.Tools.GPS
{
    #region Delete Alles
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class DelAlles : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                DelRiool cmd1 = new DelRiool();
                DelLucht cmd2 = new DelLucht();
                DelKoudWater cmd3 = new DelKoudWater();
                DelWarmWater cmd4 = new DelWarmWater();

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

    #region Delete Riool
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class DelRiool : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            string familyNameToDelete = "GPS Riool";
            List<ElementId> instanceIdsToDelete = new List<ElementId>();

            using (Transaction trans = new Transaction(doc, "Verwijder GPS Riool"))
            {
                trans.Start();

                /// Zoek naar FamilyInstance elementen die overeenkomen met de gespecificeerde familienaam
                var instances = new FilteredElementCollector(doc)
                                .OfClass(typeof(FamilyInstance))
                                .Cast<FamilyInstance>()
                                .Where(fi => fi.Symbol.Family.Name.Equals(familyNameToDelete));

                foreach (FamilyInstance fi in instances)
                {
                    instanceIdsToDelete.Add(fi.Id);
                }

                /// Verwijder de instanties in een aparte stap
                foreach (var id in instanceIdsToDelete)
                {
                    doc.Delete(id);
                }

                trans.Commit();
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region Delete Lucht
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class DelLucht : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            string familyNameToDelete = "GPS Lucht";
            List<ElementId> instanceIdsToDelete = new List<ElementId>();

            using (Transaction trans = new Transaction(doc, "Verwijder GPS Lucht"))
            {
                trans.Start();

                /// Zoek naar FamilyInstance elementen die overeenkomen met de gespecificeerde familienaam
                var instances = new FilteredElementCollector(doc)
                                .OfClass(typeof(FamilyInstance))
                                .Cast<FamilyInstance>()
                                .Where(fi => fi.Symbol.Family.Name.Equals(familyNameToDelete));

                foreach (FamilyInstance fi in instances)
                {
                    instanceIdsToDelete.Add(fi.Id);
                }

                /// Verwijder de instanties in een aparte stap
                foreach (var id in instanceIdsToDelete)
                {
                    doc.Delete(id);
                }

                trans.Commit();
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region Delete Koudwater
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class DelKoudWater : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            string familyNameToDelete = "GPS Koudwater";
            List<ElementId> instanceIdsToDelete = new List<ElementId>();

            using (Transaction trans = new Transaction(doc, "Verwijder GPS Koudwater"))
            {
                trans.Start();

                // Zoek naar FamilyInstance elementen die overeenkomen met de gespecificeerde familienaam
                var instances = new FilteredElementCollector(doc)
                                .OfClass(typeof(FamilyInstance))
                                .Cast<FamilyInstance>()
                                .Where(fi => fi.Symbol.Family.Name.Equals(familyNameToDelete));

                foreach (FamilyInstance fi in instances)
                {
                    instanceIdsToDelete.Add(fi.Id);
                }

                // Verwijder de instanties in een aparte stap
                foreach (var id in instanceIdsToDelete)
                {
                    doc.Delete(id);
                }

                trans.Commit();
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region Delete Warmwater
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class DelWarmWater : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            string familyNameToDelete = "GPS Warmwater";
            List<ElementId> instanceIdsToDelete = new List<ElementId>();

            using (Transaction trans = new Transaction(doc, "Verwijder GPS Warmwater"))
            {
                trans.Start();

                // Zoek naar FamilyInstance elementen die overeenkomen met de gespecificeerde familienaam
                var instances = new FilteredElementCollector(doc)
                                .OfClass(typeof(FamilyInstance))
                                .Cast<FamilyInstance>()
                                .Where(fi => fi.Symbol.Family.Name.Equals(familyNameToDelete));

                foreach (FamilyInstance fi in instances)
                {
                    instanceIdsToDelete.Add(fi.Id);
                }

                // Verwijder de instanties in een aparte stap
                foreach (var id in instanceIdsToDelete)
                {
                    doc.Delete(id);
                }

                trans.Commit();
            }

            return Result.Succeeded;
        }
    }
    #endregion
}
