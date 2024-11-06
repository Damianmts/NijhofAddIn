using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;

namespace NijhofAddIn.Revit.Commands.Prefab.Maken
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class PrefabAdd : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                // Stap 1: Selecteer een element om parameters op te halen
                Reference referenceElement = uidoc.Selection.PickObject(ObjectType.Element, "Selecteer een element om prefab parameters op te halen");
                Element sourceElement = doc.GetElement(referenceElement);

                // Haal de Prefab Set en Prefab Color ID parameters op van het geselecteerde element
                string prefabSet = GetParameterValue(sourceElement, "Prefab Set");
                string prefabColorID = GetParameterValue(sourceElement, "Prefab Color ID");

                if (string.IsNullOrEmpty(prefabSet) || string.IsNullOrEmpty(prefabColorID))
                {
                    message = "Het geselecteerde element bevat geen geldige prefab parameters.";
                    return Result.Failed;
                }

                // Stap 2: Selecteer nieuwe elementen om de parameters aan toe te voegen
                IList<Reference> selectedElements = uidoc.Selection.PickObjects(ObjectType.Element, "Selecteer de elementen om prefab parameters toe te wijzen");

                using (Transaction trans = new Transaction(doc, "Prefab set toewijzen"))
                {
                    trans.Start();

                    foreach (Reference reference in selectedElements)
                    {
                        Element element = doc.GetElement(reference);

                        // Toewijzen van de Prefab Color ID en Prefab Set parameters
                        Parameter prefabSetParam = element.LookupParameter("Prefab Set");
                        Parameter prefabColorIDParam = element.LookupParameter("Prefab Color ID");

                        if (prefabSetParam != null && prefabColorIDParam != null)
                        {
                            prefabSetParam.Set(prefabSet);
                            prefabColorIDParam.Set(prefabColorID);
                        }
                    }

                    trans.Commit();
                }

                TaskDialog.Show("Prefab Toegevoegd", $"Prefab set {prefabSet} en kleur ID {prefabColorID} toegewezen aan de geselecteerde elementen.");

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }

        // Functie om de parameterwaarde op te halen als string
        private string GetParameterValue(Element element, string paramName)
        {
            Parameter param = element.LookupParameter(paramName);
            if (param != null && param.HasValue)
            {
                return param.AsString();
            }
            return null;
        }
    }
}
