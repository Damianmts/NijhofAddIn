using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NijhofAddIn.Revit.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NijhofAddIn.Revit.Commands.Elektra
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class SwitchcodeList : IExternalCommand
    {
        public Result Execute(ExternalCommandData extCmdData, ref string msg, ElementSet element)
        {
            UIApplication uiapp = extCmdData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                var lettersCode = SwitchCodeGet(doc, "Switch code");
                var code2 = SwitchCodeGet(doc, "Switch Code");
                var code3 = SwitchCodeGet(doc, "Switch Code 1");
                var code4 = SwitchCodeGet(doc, "Switch Code 2");
                var code5 = SwitchCodeGet(doc, "Switchcode");

                lettersCode.AddRange(code2);
                lettersCode.AddRange(code3);
                lettersCode.AddRange(code4);
                lettersCode.AddRange(code5);

                if (lettersCode.Count == 0)
                {
                    TaskDialog.Show("Error", "Geen elementen met switch codes ingevuld in het model");
                    return Result.Succeeded;
                }

                // Group switch letters and prepare the final dictionary
                var switchLettersDict = lettersCode
                    .GroupBy(x => x)
                    .ToDictionary(g => g.Key, g => g.Count() > 1 ? g.Count() : 0);

                var form = new SwitchcodeListWPF(switchLettersDict);
                form.ShowDialog();
            }
            catch (Exception e)
            {
                msg = e.Message;
                return Result.Failed;
            }
            return Result.Succeeded;
        }

        public List<string> SwitchCodeGet(Document doc, string codeSwitch)
        {
            var categories = new List<BuiltInCategory>
            {
                BuiltInCategory.OST_ElectricalFixtures,
                BuiltInCategory.OST_LightingFixtures,
                BuiltInCategory.OST_LightingDevices,
                BuiltInCategory.OST_FireAlarmDevices
            };

            var elecElements = new List<FamilyInstance>();

            foreach (var category in categories)
            {
                var elements = new FilteredElementCollector(doc)
                    .OfClass(typeof(FamilyInstance))
                    .WhereElementIsNotElementType()
                    .OfCategory(category)
                    .Where(item =>
                    {
                        if (item.LookupParameter(codeSwitch) != null)
                        {
                            Parameter parameter = item.LookupParameter(codeSwitch);
                            if (parameter != null && parameter.HasValue)
                            {
                                return true;
                            }
                        }
                        return false;
                    })
                    .Cast<FamilyInstance>()
                    .ToList();

                elecElements.AddRange(elements);
            }

            List<string> switchCodeNames = new List<string>();
            foreach (var item in elecElements)
            {
                Parameter parameter = item.LookupParameter(codeSwitch);
                if (parameter != null && parameter.HasValue)
                {
                    switchCodeNames.Add(parameter.AsString());
                }
            }

            return switchCodeNames;
        }
    }
}
