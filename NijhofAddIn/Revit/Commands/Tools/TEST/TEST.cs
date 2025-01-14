using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NijhofAddInWPF2024.Contracts.Views;
using System;
using System.Windows;

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
                // Start de WPF-applicatie en initialiseer DI
                var app = new NijhofAddInWPF2024.App();
                app.InitializeDI(); // Initialiseer handmatig de DI-container

                // Haal de ShellWindow op via DI
                var shellWindow = app.GetService<IShellWindow>() as Window;

                if (shellWindow == null)
                {
                    message = "ShellWindow kon niet worden geladen.";
                    return Result.Failed;
                }

                // Toon het venster
                shellWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                message = $"Fout bij het openen van de WPF-toepassing: {ex.Message}";
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }
}
