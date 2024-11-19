using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NijhofAddIn.Revit.Commands.Tools.Tools
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class OntstoppingsstukOmzetten : IExternalCommand
    {
        public Result Execute(ExternalCommandData extCmdData, ref string msg, ElementSet elmtSet)
        {
            UIApplication uiApp = extCmdData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            try
            {
                while (OntstoppingsStukken(doc).Count > 0)
                {
                    var ontstoppingsstuk = OntstoppingsStukken(doc);
                    //TaskDialog.Show("mes", "aantal gevonden stukken: " + ontstoppingsstuk.Count.ToString());
                    FamilySymbol symbol = doc.GetElement(ontstoppingsstuk[0].GetTypeId()) as FamilySymbol;
                    Family family = symbol.Family;

                    // Open de bewerkingsmodus voor de nieuwe familie
                    Document familyDoc = doc.EditFamily(family);
                    Family f = familyDoc.OwnerFamily;
                    Category c = f.FamilyCategory;
                    var partTypeParam = f.get_Parameter(BuiltInParameter.FAMILY_CONTENT_PART_TYPE);

                    using (var t = new Transaction(doc, "NT - Onstp modify"))
                    {
                        t.Start();

                        f.FamilyCategoryId = new ElementId(BuiltInCategory.OST_PipeFitting);

                        t.Commit();
                    }

                    f = familyDoc.LoadFamily(doc, new CustomFamilyLoadOption());
                    familyDoc.Close(false);
                }

            }
            catch (Exception e)
            {
                TaskDialog.Show("Error", "Er zijn geen onstoppingsstukken met category Pipe Accessories aanwezig in het model.");
                msg = e.Message;
                return Result.Cancelled;
            }
            return Result.Succeeded;
        }

        public class CustomFamilyLoadOption : IFamilyLoadOptions
        {
            public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
            {
                overwriteParameterValues = true;
                return true;
            }

            public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
            {
                source = FamilySource.Family;
                overwriteParameterValues = true;
                return true;
            }
        }

        public List<Element> OntstoppingsStukken(Document doc)
        {
            //zoek een ontstoppingsstukken die pipe accesory zijn
            List<Element> ontstoppingsstuk = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_PipeAccessory)
                .WhereElementIsNotElementType()
                .Where(e => e.Name.Contains("Ontst") && e.Name.Contains("stuk") && (e.Name.Contains("Manchet") || e.Name.Contains("schroefdeksel")))
                .Cast<Element>()
                .ToList();

            return ontstoppingsstuk;
        }
    }
}
