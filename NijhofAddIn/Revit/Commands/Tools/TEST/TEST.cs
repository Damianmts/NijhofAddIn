using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.IO;
using System.Diagnostics;

namespace NijhofAddIn.Revit.Commands.Tools.TEST
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class TEST : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // Dynamisch pad naar de uitvoermap van de plugin
                string pluginDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string exePathDynamic = Path.Combine(pluginDirectory, "WPFNetFrame48.exe");

                // Hardcoded pad als fallback
                string exePathHardcoded = @"C:\Users\damia\Documents\Revit Plugins\NijhofAddIn\NijhofAddIn\bin\Debug\WPFNetFrame48.exe";

                // Controleer of het dynamische pad geldig is
                string exePath = File.Exists(exePathDynamic) ? exePathDynamic : exePathHardcoded;

                // Controleer of het uiteindelijke pad geldig is
                if (!File.Exists(exePath))
                {
                    TaskDialog.Show("Error", $"Kan het bestand niet vinden. Geprobeerd:\n{exePathDynamic}\n{exePathHardcoded}");
                    return Result.Failed;
                }

                // Specifiek argument voor de pagina die je wilt openen
                string pageArgument = "settings"; // Bijvoorbeeld 'settings', 'treeview', etc.

                // Start de WPF applicatie met het argument
                Process.Start(exePath, pageArgument);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                // Foutafhandeling
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }
        }
    }
}
