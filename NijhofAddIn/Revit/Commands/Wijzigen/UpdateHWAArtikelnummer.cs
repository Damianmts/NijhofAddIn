using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NijhofAddIn.Revit.Commands.Wijzigen
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class UpdateHWAArtikelnummer : IExternalCommand
    {
        public Result Execute(ExternalCommandData extCmdData, ref string msg, ElementSet element)
        {
            UIApplication uiapp = extCmdData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;


            var hwaPipes = new FilteredElementCollector(doc)
                .OfClass(typeof(Pipe))
                .Where(a =>
                {
                    Parameter parameterType = a.LookupParameter("Article Type");
                    Parameter parameterDiameter = a.LookupParameter("D1 Description");
                    Parameter parameterArtikelNr = a.LookupParameter("Manufacturer Art. No.");
                    if (parameterType != null && parameterType.HasValue)
                    {
                        if (parameterType.AsString() == "HWA 6m"
                        && !(parameterArtikelNr.AsString() == "20033890"))
                        {
                            return parameterType.AsString() == "HWA 6m";
                        }
                        else if (parameterType.AsString() == "PVC 5,55m"
                            && parameterDiameter.AsString() == "80"
                            && !(parameterArtikelNr.AsString() == "20033890"))
                        {
                            return parameterType.AsString() == "PVC 5,55m";
                        }
                        else if (parameterType.AsString() == "HWA 5,55m"
                        && parameterDiameter.AsString() == "80"
                        && !(parameterArtikelNr.AsString() == "20033890"))
                        {
                            return parameterType.AsString() == "HWA 5,55m";
                        }


                    }
                    return false;
                })
                .Cast<Element>()
                .ToList();


            if (hwaPipes.Count == 0)
            {
                TaskDialog.Show("NT - HWA artikelnummer updater", "Alle ø80 HWA artikelen zijn geupdate naar het juiste artikelnummer. (20033890)");
                return Result.Cancelled;
            }

            try
            {

                using (var transaction = new Transaction(doc, "NT - Artikel nr HWA update"))
                {
                    transaction.Start();

                    foreach (Element pipe in hwaPipes)
                    {
                        Parameter para = pipe.LookupParameter("Manufacturer Art. No.");
                        para.Set("20033890");
                    }

                    TaskDialog.Show("NT - HWA artikelnummer updater", "Bij " + hwaPipes.Count + " pipe(s) is/zijn het artikelnummer geupdate.");

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
            return Result.Succeeded;
        }
    }
}
