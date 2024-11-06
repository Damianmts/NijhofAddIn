using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NijhofAddIn.Revit.Commands.Elektra.Toevoegen
{
    #region Enkelpolig
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SchakelaarEnkelpolig : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "Schak_Enkelp_BJ_Future_1v_Wit"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\Schakelaars\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#endif

            string familyPath = Path.Combine(familyPathBase, familyName + ".rfa");

            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "File does not exist: " + familyPath);
                return Result.Failed;
            }

            FamilySymbol symbol = new FilteredElementCollector(doc)
                                    .OfClass(typeof(FamilySymbol))
                                    .Cast<FamilySymbol>()
                                    .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol != null)
            {
                /// Familie is al geladen en symbool gevonden, externe gebeurtenis activeren
                if (_handler == null)
                {
                    _handler = new RequestHandler();
                    _externalEvent = ExternalEvent.Create(_handler);
                }

                _handler.Symbol = symbol;
                _externalEvent.Raise();
                return Result.Succeeded;
            }

            Family family;
            using (Transaction tx = new Transaction(doc, "Load Family"))
            {
                try
                {
                    tx.Start();
                    if (!doc.LoadFamily(familyPath, out family))
                    {
                        TaskDialog.Show("Error", "Failed to load the family.");
                        tx.RollBack();
                        return Result.Failed;
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", "An unexpected error occurred while loading the family: " + ex.Message);
                    if (tx.GetStatus() == TransactionStatus.Started)
                    {
                        tx.RollBack();
                    }
                    return Result.Failed;
                }
            }

            /// Zoek het specifieke FamilySymbol opnieuw na het laden van de familie
            symbol = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol == null)
            {
                TaskDialog.Show("Error", $"FamilySymbol '{symbolName}' not found for the loaded family '{familyName}'.");
                return Result.Failed;
            }

            if (_handler == null)
            {
                _handler = new RequestHandler();
                _externalEvent = ExternalEvent.Create(_handler);
            }

            _handler.Symbol = symbol;
            _externalEvent.Raise();

            return Result.Succeeded;
        }
    }
    #endregion

    #region Dubbelpolig
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SchakelaarDubbelpolig : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "Schak_Dubbelp_BJ_Future_1v_Wit"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\Schakelaars\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#endif

            string familyPath = Path.Combine(familyPathBase, familyName + ".rfa");

            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "File does not exist: " + familyPath);
                return Result.Failed;
            }

            FamilySymbol symbol = new FilteredElementCollector(doc)
                                    .OfClass(typeof(FamilySymbol))
                                    .Cast<FamilySymbol>()
                                    .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol != null)
            {
                /// Familie is al geladen en symbool gevonden, externe gebeurtenis activeren
                if (_handler == null)
                {
                    _handler = new RequestHandler();
                    _externalEvent = ExternalEvent.Create(_handler);
                }

                _handler.Symbol = symbol;
                _externalEvent.Raise();
                return Result.Succeeded;
            }

            Family family;
            using (Transaction tx = new Transaction(doc, "Load Family"))
            {
                try
                {
                    tx.Start();
                    if (!doc.LoadFamily(familyPath, out family))
                    {
                        TaskDialog.Show("Error", "Failed to load the family.");
                        tx.RollBack();
                        return Result.Failed;
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", "An unexpected error occurred while loading the family: " + ex.Message);
                    if (tx.GetStatus() == TransactionStatus.Started)
                    {
                        tx.RollBack();
                    }
                    return Result.Failed;
                }
            }

            /// Zoek het specifieke FamilySymbol opnieuw na het laden van de familie
            symbol = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol == null)
            {
                TaskDialog.Show("Error", $"FamilySymbol '{symbolName}' not found for the loaded family '{familyName}'.");
                return Result.Failed;
            }

            if (_handler == null)
            {
                _handler = new RequestHandler();
                _externalEvent = ExternalEvent.Create(_handler);
            }

            _handler.Symbol = symbol;
            _externalEvent.Raise();

            return Result.Succeeded;
        }
    }
    #endregion

    #region Vierpolig
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SchakelaarVierpolig : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "Schak_4polig"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\Schakelaars\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#endif

            string familyPath = Path.Combine(familyPathBase, familyName + ".rfa");

            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "File does not exist: " + familyPath);
                return Result.Failed;
            }

            FamilySymbol symbol = new FilteredElementCollector(doc)
                                    .OfClass(typeof(FamilySymbol))
                                    .Cast<FamilySymbol>()
                                    .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol != null)
            {
                /// Familie is al geladen en symbool gevonden, externe gebeurtenis activeren
                if (_handler == null)
                {
                    _handler = new RequestHandler();
                    _externalEvent = ExternalEvent.Create(_handler);
                }

                _handler.Symbol = symbol;
                _externalEvent.Raise();
                return Result.Succeeded;
            }

            Family family;
            using (Transaction tx = new Transaction(doc, "Load Family"))
            {
                try
                {
                    tx.Start();
                    if (!doc.LoadFamily(familyPath, out family))
                    {
                        TaskDialog.Show("Error", "Failed to load the family.");
                        tx.RollBack();
                        return Result.Failed;
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", "An unexpected error occurred while loading the family: " + ex.Message);
                    if (tx.GetStatus() == TransactionStatus.Started)
                    {
                        tx.RollBack();
                    }
                    return Result.Failed;
                }
            }

            /// Zoek het specifieke FamilySymbol opnieuw na het laden van de familie
            symbol = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol == null)
            {
                TaskDialog.Show("Error", $"FamilySymbol '{symbolName}' not found for the loaded family '{familyName}'.");
                return Result.Failed;
            }

            if (_handler == null)
            {
                _handler = new RequestHandler();
                _externalEvent = ExternalEvent.Create(_handler);
            }

            _handler.Symbol = symbol;
            _externalEvent.Raise();

            return Result.Succeeded;
        }
    }
    #endregion

    #region Wissel
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SchakelaarWissel : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "Schak_Wissel_BJ_Future_1v_Wit"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\Schakelaars\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#endif

            string familyPath = Path.Combine(familyPathBase, familyName + ".rfa");

            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "File does not exist: " + familyPath);
                return Result.Failed;
            }

            FamilySymbol symbol = new FilteredElementCollector(doc)
                                    .OfClass(typeof(FamilySymbol))
                                    .Cast<FamilySymbol>()
                                    .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol != null)
            {
                /// Familie is al geladen en symbool gevonden, externe gebeurtenis activeren
                if (_handler == null)
                {
                    _handler = new RequestHandler();
                    _externalEvent = ExternalEvent.Create(_handler);
                }

                _handler.Symbol = symbol;
                _externalEvent.Raise();
                return Result.Succeeded;
            }

            Family family;
            using (Transaction tx = new Transaction(doc, "Load Family"))
            {
                try
                {
                    tx.Start();
                    if (!doc.LoadFamily(familyPath, out family))
                    {
                        TaskDialog.Show("Error", "Failed to load the family.");
                        tx.RollBack();
                        return Result.Failed;
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", "An unexpected error occurred while loading the family: " + ex.Message);
                    if (tx.GetStatus() == TransactionStatus.Started)
                    {
                        tx.RollBack();
                    }
                    return Result.Failed;
                }
            }

            /// Zoek het specifieke FamilySymbol opnieuw na het laden van de familie
            symbol = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol == null)
            {
                TaskDialog.Show("Error", $"FamilySymbol '{symbolName}' not found for the loaded family '{familyName}'.");
                return Result.Failed;
            }

            if (_handler == null)
            {
                _handler = new RequestHandler();
                _externalEvent = ExternalEvent.Create(_handler);
            }

            _handler.Symbol = symbol;
            _externalEvent.Raise();

            return Result.Succeeded;
        }
    }
    #endregion

    #region Dubbel Wissel
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SchakelaarDubbelWissel : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "Schak_Wissel Dubbelp_BJ_Future_1v_Wit"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\Schakelaars\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#endif

            string familyPath = Path.Combine(familyPathBase, familyName + ".rfa");

            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "File does not exist: " + familyPath);
                return Result.Failed;
            }

            FamilySymbol symbol = new FilteredElementCollector(doc)
                                    .OfClass(typeof(FamilySymbol))
                                    .Cast<FamilySymbol>()
                                    .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol != null)
            {
                /// Familie is al geladen en symbool gevonden, externe gebeurtenis activeren
                if (_handler == null)
                {
                    _handler = new RequestHandler();
                    _externalEvent = ExternalEvent.Create(_handler);
                }

                _handler.Symbol = symbol;
                _externalEvent.Raise();
                return Result.Succeeded;
            }

            Family family;
            using (Transaction tx = new Transaction(doc, "Load Family"))
            {
                try
                {
                    tx.Start();
                    if (!doc.LoadFamily(familyPath, out family))
                    {
                        TaskDialog.Show("Error", "Failed to load the family.");
                        tx.RollBack();
                        return Result.Failed;
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", "An unexpected error occurred while loading the family: " + ex.Message);
                    if (tx.GetStatus() == TransactionStatus.Started)
                    {
                        tx.RollBack();
                    }
                    return Result.Failed;
                }
            }

            /// Zoek het specifieke FamilySymbol opnieuw na het laden van de familie
            symbol = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol == null)
            {
                TaskDialog.Show("Error", $"FamilySymbol '{symbolName}' not found for the loaded family '{familyName}'.");
                return Result.Failed;
            }

            if (_handler == null)
            {
                _handler = new RequestHandler();
                _externalEvent = ExternalEvent.Create(_handler);
            }

            _handler.Symbol = symbol;
            _externalEvent.Raise();

            return Result.Succeeded;
        }
    }
    #endregion

    #region 2x Wissel
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Schakelaar2xWissel : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "Schak_Wissel 2x_BJ_Future_1v_Wit"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\Schakelaars\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#endif

            string familyPath = Path.Combine(familyPathBase, familyName + ".rfa");

            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "File does not exist: " + familyPath);
                return Result.Failed;
            }

            FamilySymbol symbol = new FilteredElementCollector(doc)
                                    .OfClass(typeof(FamilySymbol))
                                    .Cast<FamilySymbol>()
                                    .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol != null)
            {
                /// Familie is al geladen en symbool gevonden, externe gebeurtenis activeren
                if (_handler == null)
                {
                    _handler = new RequestHandler();
                    _externalEvent = ExternalEvent.Create(_handler);
                }

                _handler.Symbol = symbol;
                _externalEvent.Raise();
                return Result.Succeeded;
            }

            Family family;
            using (Transaction tx = new Transaction(doc, "Load Family"))
            {
                try
                {
                    tx.Start();
                    if (!doc.LoadFamily(familyPath, out family))
                    {
                        TaskDialog.Show("Error", "Failed to load the family.");
                        tx.RollBack();
                        return Result.Failed;
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", "An unexpected error occurred while loading the family: " + ex.Message);
                    if (tx.GetStatus() == TransactionStatus.Started)
                    {
                        tx.RollBack();
                    }
                    return Result.Failed;
                }
            }

            /// Zoek het specifieke FamilySymbol opnieuw na het laden van de familie
            symbol = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol == null)
            {
                TaskDialog.Show("Error", $"FamilySymbol '{symbolName}' not found for the loaded family '{familyName}'.");
                return Result.Failed;
            }

            if (_handler == null)
            {
                _handler = new RequestHandler();
                _externalEvent = ExternalEvent.Create(_handler);
            }

            _handler.Symbol = symbol;
            _externalEvent.Raise();

            return Result.Succeeded;
        }
    }
    #endregion

    #region Serie
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SchakelaarSerie : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "Schak_Serie_BJ_Future_1v_Wit"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\Schakelaars\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#endif

            string familyPath = Path.Combine(familyPathBase, familyName + ".rfa");

            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "File does not exist: " + familyPath);
                return Result.Failed;
            }

            FamilySymbol symbol = new FilteredElementCollector(doc)
                                    .OfClass(typeof(FamilySymbol))
                                    .Cast<FamilySymbol>()
                                    .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol != null)
            {
                /// Familie is al geladen en symbool gevonden, externe gebeurtenis activeren
                if (_handler == null)
                {
                    _handler = new RequestHandler();
                    _externalEvent = ExternalEvent.Create(_handler);
                }

                _handler.Symbol = symbol;
                _externalEvent.Raise();
                return Result.Succeeded;
            }

            Family family;
            using (Transaction tx = new Transaction(doc, "Load Family"))
            {
                try
                {
                    tx.Start();
                    if (!doc.LoadFamily(familyPath, out family))
                    {
                        TaskDialog.Show("Error", "Failed to load the family.");
                        tx.RollBack();
                        return Result.Failed;
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", "An unexpected error occurred while loading the family: " + ex.Message);
                    if (tx.GetStatus() == TransactionStatus.Started)
                    {
                        tx.RollBack();
                    }
                    return Result.Failed;
                }
            }

            /// Zoek het specifieke FamilySymbol opnieuw na het laden van de familie
            symbol = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol == null)
            {
                TaskDialog.Show("Error", $"FamilySymbol '{symbolName}' not found for the loaded family '{familyName}'.");
                return Result.Failed;
            }

            if (_handler == null)
            {
                _handler = new RequestHandler();
                _externalEvent = ExternalEvent.Create(_handler);
            }

            _handler.Symbol = symbol;
            _externalEvent.Raise();

            return Result.Succeeded;
        }
    }
    #endregion

    #region Kruis
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SchakelaarKruis : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "Schak_Kruis_BJ_Future_1v_Wit"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\Schakelaars\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#endif

            string familyPath = Path.Combine(familyPathBase, familyName + ".rfa");

            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "File does not exist: " + familyPath);
                return Result.Failed;
            }

            FamilySymbol symbol = new FilteredElementCollector(doc)
                                    .OfClass(typeof(FamilySymbol))
                                    .Cast<FamilySymbol>()
                                    .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol != null)
            {
                /// Familie is al geladen en symbool gevonden, externe gebeurtenis activeren
                if (_handler == null)
                {
                    _handler = new RequestHandler();
                    _externalEvent = ExternalEvent.Create(_handler);
                }

                _handler.Symbol = symbol;
                _externalEvent.Raise();
                return Result.Succeeded;
            }

            Family family;
            using (Transaction tx = new Transaction(doc, "Load Family"))
            {
                try
                {
                    tx.Start();
                    if (!doc.LoadFamily(familyPath, out family))
                    {
                        TaskDialog.Show("Error", "Failed to load the family.");
                        tx.RollBack();
                        return Result.Failed;
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", "An unexpected error occurred while loading the family: " + ex.Message);
                    if (tx.GetStatus() == TransactionStatus.Started)
                    {
                        tx.RollBack();
                    }
                    return Result.Failed;
                }
            }

            /// Zoek het specifieke FamilySymbol opnieuw na het laden van de familie
            symbol = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol == null)
            {
                TaskDialog.Show("Error", $"FamilySymbol '{symbolName}' not found for the loaded family '{familyName}'.");
                return Result.Failed;
            }

            if (_handler == null)
            {
                _handler = new RequestHandler();
                _externalEvent = ExternalEvent.Create(_handler);
            }

            _handler.Symbol = symbol;
            _externalEvent.Raise();

            return Result.Succeeded;
        }
    }
    #endregion

    #region Leddimmer
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SchakelaarDimmer : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "Schak_Leddimmer_BJ_Future_1v_Wit"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\Schakelaars\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#endif

            string familyPath = Path.Combine(familyPathBase, familyName + ".rfa");

            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "File does not exist: " + familyPath);
                return Result.Failed;
            }

            FamilySymbol symbol = new FilteredElementCollector(doc)
                                    .OfClass(typeof(FamilySymbol))
                                    .Cast<FamilySymbol>()
                                    .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol != null)
            {
                /// Familie is al geladen en symbool gevonden, externe gebeurtenis activeren
                if (_handler == null)
                {
                    _handler = new RequestHandler();
                    _externalEvent = ExternalEvent.Create(_handler);
                }

                _handler.Symbol = symbol;
                _externalEvent.Raise();
                return Result.Succeeded;
            }

            Family family;
            using (Transaction tx = new Transaction(doc, "Load Family"))
            {
                try
                {
                    tx.Start();
                    if (!doc.LoadFamily(familyPath, out family))
                    {
                        TaskDialog.Show("Error", "Failed to load the family.");
                        tx.RollBack();
                        return Result.Failed;
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", "An unexpected error occurred while loading the family: " + ex.Message);
                    if (tx.GetStatus() == TransactionStatus.Started)
                    {
                        tx.RollBack();
                    }
                    return Result.Failed;
                }
            }

            /// Zoek het specifieke FamilySymbol opnieuw na het laden van de familie
            symbol = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol == null)
            {
                TaskDialog.Show("Error", $"FamilySymbol '{symbolName}' not found for the loaded family '{familyName}'.");
                return Result.Failed;
            }

            if (_handler == null)
            {
                _handler = new RequestHandler();
                _externalEvent = ExternalEvent.Create(_handler);
            }

            _handler.Symbol = symbol;
            _externalEvent.Raise();

            return Result.Succeeded;
        }
    }
    #endregion

    #region Leddimmer Wissel
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SchakelaarDimmerWissel : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "Schak_Leddimmer Wissel_BJ_Future_1v_Wit"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\Schakelaars\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#endif

            string familyPath = Path.Combine(familyPathBase, familyName + ".rfa");

            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "File does not exist: " + familyPath);
                return Result.Failed;
            }

            FamilySymbol symbol = new FilteredElementCollector(doc)
                                    .OfClass(typeof(FamilySymbol))
                                    .Cast<FamilySymbol>()
                                    .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol != null)
            {
                /// Familie is al geladen en symbool gevonden, externe gebeurtenis activeren
                if (_handler == null)
                {
                    _handler = new RequestHandler();
                    _externalEvent = ExternalEvent.Create(_handler);
                }

                _handler.Symbol = symbol;
                _externalEvent.Raise();
                return Result.Succeeded;
            }

            Family family;
            using (Transaction tx = new Transaction(doc, "Load Family"))
            {
                try
                {
                    tx.Start();
                    if (!doc.LoadFamily(familyPath, out family))
                    {
                        TaskDialog.Show("Error", "Failed to load the family.");
                        tx.RollBack();
                        return Result.Failed;
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", "An unexpected error occurred while loading the family: " + ex.Message);
                    if (tx.GetStatus() == TransactionStatus.Started)
                    {
                        tx.RollBack();
                    }
                    return Result.Failed;
                }
            }

            /// Zoek het specifieke FamilySymbol opnieuw na het laden van de familie
            symbol = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol == null)
            {
                TaskDialog.Show("Error", $"FamilySymbol '{symbolName}' not found for the loaded family '{familyName}'.");
                return Result.Failed;
            }

            if (_handler == null)
            {
                _handler = new RequestHandler();
                _externalEvent = ExternalEvent.Create(_handler);
            }

            _handler.Symbol = symbol;
            _externalEvent.Raise();

            return Result.Succeeded;
        }
    }
    #endregion

    #region Jaloezie
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SchakelaarJaloezie : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "Schak_Jaloezie_BJ_Future_1v_Wit"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\Schakelaars\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#endif

            string familyPath = Path.Combine(familyPathBase, familyName + ".rfa");

            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "File does not exist: " + familyPath);
                return Result.Failed;
            }

            FamilySymbol symbol = new FilteredElementCollector(doc)
                                    .OfClass(typeof(FamilySymbol))
                                    .Cast<FamilySymbol>()
                                    .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol != null)
            {
                /// Familie is al geladen en symbool gevonden, externe gebeurtenis activeren
                if (_handler == null)
                {
                    _handler = new RequestHandler();
                    _externalEvent = ExternalEvent.Create(_handler);
                }

                _handler.Symbol = symbol;
                _externalEvent.Raise();
                return Result.Succeeded;
            }

            Family family;
            using (Transaction tx = new Transaction(doc, "Load Family"))
            {
                try
                {
                    tx.Start();
                    if (!doc.LoadFamily(familyPath, out family))
                    {
                        TaskDialog.Show("Error", "Failed to load the family.");
                        tx.RollBack();
                        return Result.Failed;
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", "An unexpected error occurred while loading the family: " + ex.Message);
                    if (tx.GetStatus() == TransactionStatus.Started)
                    {
                        tx.RollBack();
                    }
                    return Result.Failed;
                }
            }

            /// Zoek het specifieke FamilySymbol opnieuw na het laden van de familie
            symbol = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol == null)
            {
                TaskDialog.Show("Error", $"FamilySymbol '{symbolName}' not found for the loaded family '{familyName}'.");
                return Result.Failed;
            }

            if (_handler == null)
            {
                _handler = new RequestHandler();
                _externalEvent = ExternalEvent.Create(_handler);
            }

            _handler.Symbol = symbol;
            _externalEvent.Raise();

            return Result.Succeeded;
        }
    }
    #endregion

    #region Wand Bewegingsmelder
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SchakelaarBewegingWand : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "Bewegingmelder Wand"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\Schakelaars\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#endif

            string familyPath = Path.Combine(familyPathBase, familyName + ".rfa");

            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "File does not exist: " + familyPath);
                return Result.Failed;
            }

            FamilySymbol symbol = new FilteredElementCollector(doc)
                                    .OfClass(typeof(FamilySymbol))
                                    .Cast<FamilySymbol>()
                                    .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol != null)
            {
                /// Familie is al geladen en symbool gevonden, externe gebeurtenis activeren
                if (_handler == null)
                {
                    _handler = new RequestHandler();
                    _externalEvent = ExternalEvent.Create(_handler);
                }

                _handler.Symbol = symbol;
                _externalEvent.Raise();
                return Result.Succeeded;
            }

            Family family;
            using (Transaction tx = new Transaction(doc, "Load Family"))
            {
                try
                {
                    tx.Start();
                    if (!doc.LoadFamily(familyPath, out family))
                    {
                        TaskDialog.Show("Error", "Failed to load the family.");
                        tx.RollBack();
                        return Result.Failed;
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", "An unexpected error occurred while loading the family: " + ex.Message);
                    if (tx.GetStatus() == TransactionStatus.Started)
                    {
                        tx.RollBack();
                    }
                    return Result.Failed;
                }
            }

            /// Zoek het specifieke FamilySymbol opnieuw na het laden van de familie
            symbol = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol == null)
            {
                TaskDialog.Show("Error", $"FamilySymbol '{symbolName}' not found for the loaded family '{familyName}'.");
                return Result.Failed;
            }

            if (_handler == null)
            {
                _handler = new RequestHandler();
                _externalEvent = ExternalEvent.Create(_handler);
            }

            _handler.Symbol = symbol;
            _externalEvent.Raise();

            return Result.Succeeded;
        }
    }
    #endregion

    #region Plafond Bewegingsmelder
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SchakelaarBewegingPlafond : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "Bewegingmelder Plafond"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\Schakelaars\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#endif

            string familyPath = Path.Combine(familyPathBase, familyName + ".rfa");

            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "File does not exist: " + familyPath);
                return Result.Failed;
            }

            FamilySymbol symbol = new FilteredElementCollector(doc)
                                    .OfClass(typeof(FamilySymbol))
                                    .Cast<FamilySymbol>()
                                    .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol != null)
            {
                /// Familie is al geladen en symbool gevonden, externe gebeurtenis activeren
                if (_handler == null)
                {
                    _handler = new RequestHandler();
                    _externalEvent = ExternalEvent.Create(_handler);
                }

                _handler.Symbol = symbol;
                _externalEvent.Raise();
                return Result.Succeeded;
            }

            Family family;
            using (Transaction tx = new Transaction(doc, "Load Family"))
            {
                try
                {
                    tx.Start();
                    if (!doc.LoadFamily(familyPath, out family))
                    {
                        TaskDialog.Show("Error", "Failed to load the family.");
                        tx.RollBack();
                        return Result.Failed;
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", "An unexpected error occurred while loading the family: " + ex.Message);
                    if (tx.GetStatus() == TransactionStatus.Started)
                    {
                        tx.RollBack();
                    }
                    return Result.Failed;
                }
            }

            /// Zoek het specifieke FamilySymbol opnieuw na het laden van de familie
            symbol = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol == null)
            {
                TaskDialog.Show("Error", $"FamilySymbol '{symbolName}' not found for the loaded family '{familyName}'.");
                return Result.Failed;
            }

            if (_handler == null)
            {
                _handler = new RequestHandler();
                _externalEvent = ExternalEvent.Create(_handler);
            }

            _handler.Symbol = symbol;
            _externalEvent.Raise();

            return Result.Succeeded;
        }
    }
    #endregion

    #region Schemer
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SchakelaarSchemer : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "Schemerschakelaar"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\Schakelaars\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\Schakelaars\";
#endif

            string familyPath = Path.Combine(familyPathBase, familyName + ".rfa");

            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "File does not exist: " + familyPath);
                return Result.Failed;
            }

            FamilySymbol symbol = new FilteredElementCollector(doc)
                                    .OfClass(typeof(FamilySymbol))
                                    .Cast<FamilySymbol>()
                                    .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol != null)
            {
                /// Familie is al geladen en symbool gevonden, externe gebeurtenis activeren
                if (_handler == null)
                {
                    _handler = new RequestHandler();
                    _externalEvent = ExternalEvent.Create(_handler);
                }

                _handler.Symbol = symbol;
                _externalEvent.Raise();
                return Result.Succeeded;
            }

            Family family;
            using (Transaction tx = new Transaction(doc, "Load Family"))
            {
                try
                {
                    tx.Start();
                    if (!doc.LoadFamily(familyPath, out family))
                    {
                        TaskDialog.Show("Error", "Failed to load the family.");
                        tx.RollBack();
                        return Result.Failed;
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", "An unexpected error occurred while loading the family: " + ex.Message);
                    if (tx.GetStatus() == TransactionStatus.Started)
                    {
                        tx.RollBack();
                    }
                    return Result.Failed;
                }
            }

            /// Zoek het specifieke FamilySymbol opnieuw na het laden van de familie
            symbol = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(e => e.FamilyName.Equals(familyName) && e.Name.Equals(symbolName));

            if (symbol == null)
            {
                TaskDialog.Show("Error", $"FamilySymbol '{symbolName}' not found for the loaded family '{familyName}'.");
                return Result.Failed;
            }

            if (_handler == null)
            {
                _handler = new RequestHandler();
                _externalEvent = ExternalEvent.Create(_handler);
            }

            _handler.Symbol = symbol;
            _externalEvent.Raise();

            return Result.Succeeded;
        }
    }
    #endregion
}
