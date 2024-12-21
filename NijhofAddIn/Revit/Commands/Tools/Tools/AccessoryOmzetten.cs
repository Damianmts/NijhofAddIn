using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NijhofAddIn.Revit.Commands.Tools.Tools
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class AccessoryOmzetten : IExternalCommand
    {
        public Result Execute(ExternalCommandData extCmdData, ref string msg, ElementSet elmtSet)
        {
            UIApplication uiApp = extCmdData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            try
            {
                // Select an element
                var selectedReference = uiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, "Selecteer een element om te converteren naar een fitting.");
                if (selectedReference == null)
                {
                    return Result.Failed;
                }

                Element element = doc.GetElement(selectedReference.ElementId);

                // Check if the element is a FamilyInstance
                if (element is FamilyInstance familyInstance)
                {
                    FamilySymbol symbol = doc.GetElement(familyInstance.GetTypeId()) as FamilySymbol;
                    Family family = symbol.Family;

                    // Cache familie bewerkingsdocument
                    Document familyDoc = doc.EditFamily(family);
                    Family f = familyDoc.OwnerFamily;

                    // Minimaliseer transacties
                    using (Transaction t = new Transaction(familyDoc, "Change Category"))
                    {
                        t.Start();

                        // Update category
                        f.FamilyCategoryId = new ElementId(BuiltInCategory.OST_PipeFitting);

                        t.Commit();
                    }

                    // Gebruik een snelle FamilyLoad-optie
                    familyDoc.LoadFamily(doc, new CustomFamilyLoadOption());
                    familyDoc.Close(false);

                    return Result.Succeeded;
                }
                else
                {
                    msg = "Het geselecteerde element is geen geldig accessoire.";
                    return Result.Failed;
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                return Result.Failed;
            }
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
    }
}
