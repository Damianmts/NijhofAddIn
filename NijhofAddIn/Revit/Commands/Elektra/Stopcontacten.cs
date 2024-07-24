using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.IO;
using System.Linq;

namespace NijhofAddIn.Revit.Commands.Elektra
{
    #region 1v Stopcontact
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Stopcontact1v : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "WCD_BJ_Future_1v_Wit"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\WCD\";
#elif RELEASE2024
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\WCD\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\WCD\";
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

    #region 2v Stopcontact
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Stopcontact2v : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "WCD_BJ_Future_2v_Wit"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\WCD\";
#elif RELEASE2024
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\WCD\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\WCD\";
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

    #region Opbouw Stopcontact
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Opbouw : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "WCD_Gira_2v_Wit"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\WCD\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\WCD\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\WCD\";
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

    #region 1v Spatwaterdicht
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Spatwaterdicht1v : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "WCD_Spatwaterdicht_1v_Wit"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\WCD\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\WCD\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\WCD\";
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

    #region 2v Spatwaterdicht
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Spatwaterdicht2v : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "WCD_Spatwaterdicht_2v_Wit"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\WCD\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\WCD\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\WCD\";
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

    #region Perilex Stopcontact
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Perilex : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "WCD_Perilex_ABL_Wit"; // Specifieke familienaam
            string symbolName = "Inbouw"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\WCD\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\WCD\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\WCD\";
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

    #region Krachtstroom Stopcontact
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Krachtstroom : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "WCD_WP_Krachtstroom"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\WCD\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\WCD\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\WCD\";
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

    #region Vloer Stopcontact
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Vloerstopcontact : IExternalCommand
    {
        private static RequestHandler _handler;
        private static ExternalEvent _externalEvent;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Stel de naam van de familie en het symbool in
            string familyName = "WCD_Vloer_1v"; // Specifieke familienaam
            string symbolName = "Standaard"; // Specifieke naam van het FamilySymbol

            string familyPathBase;
#if RELEASE2023
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R23\WCD\";
#elif RELEASE2024
            familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\WCD\";
#elif RELEASE2025
        familyPathBase = @"F:\Stabiplan\Custom\Families\1 - Elektra\BJ Future Wit R24\WCD\";
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

