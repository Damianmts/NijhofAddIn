using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Diagnostics;

namespace NijhofAddIn.Revit.Commands.Tools.Overig
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class Klik : IExternalCommand
    {
        public Result Execute(ExternalCommandData extCmdData, ref string msg, ElementSet element)
        {
            UIApplication uiapp = extCmdData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://www.youtube.com/watch?v=rB8LX7IeDkY",
                    UseShellExecute = true
                });
            }

            catch (Exception e)
            {
                msg = e.Message;
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }
}
