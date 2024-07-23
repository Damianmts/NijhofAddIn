using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.IO;

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
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // Initialize the handler and external event if not already done
            if (_handler == null)
            {
                _handler = new RequestHandler();
                _externalEvent = ExternalEvent.Create(_handler);
            }

            // File path to the family
            string familyFilePath = @"C:\Path\To\Your\Family\WCD_BJ_Future_1v_Wit.rfa";

            // Filter for the specific Electrical Fixture family symbol
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(BuiltInCategory.OST_ElectricalFixtures);
            collector.OfClass(typeof(FamilySymbol));

            // Adjust the following line to select the correct family and family symbol
            FamilySymbol symbol = null;
            foreach (FamilySymbol s in collector)
            {
                Family family = s.Family;
                if (family.Name == "WCD_BJ_Future_1v_Wit" && s.Name == "Standaard")
                {
                    symbol = s;
                    break;
                }
            }

            // Load the family if it is not found in the project
            if (symbol == null)
            {
                using (Transaction t = new Transaction(doc, "Load Family"))
                {
                    t.Start();

                    Family family;
                    if (doc.LoadFamily(familyFilePath, out family))
                    {
                        foreach (ElementId id in family.GetFamilySymbolIds())
                        {
                            FamilySymbol s = doc.GetElement(id) as FamilySymbol;
                            if (s.Name == "Standaard")
                            {
                                symbol = s;
                                break;
                            }
                        }
                    }
                    else
                    {
                        TaskDialog.Show("Error", "Failed to load the family from the specified location.");
                        t.RollBack();
                        return Result.Failed;
                    }

                    t.Commit();
                }
            }

            if (symbol == null)
            {
                TaskDialog.Show("Error", "The specified family 'WCD_BJ_Future_1v_Wit' with family symbol 'Standaard' was not found.");
                return Result.Failed;
            }

            // Activate the symbol if not already activated
            if (!symbol.IsActive)
            {
                using (Transaction t = new Transaction(doc, "Activate Family Symbol"))
                {
                    t.Start();
                    symbol.Activate();
                    t.Commit();
                }
            }

            // Set the symbol in the handler and raise the external event
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
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                // Transaction to modify the document
                using (Transaction trans = new Transaction(doc, "Place Electrical Fixture"))
                {
                    trans.Start();

                    // Get the family symbol (the electrical fixture family type) to place
                    FamilySymbol familySymbol = GetFamilySymbol(doc, "WCD_BJ_Future_2v_Wit");

                    if (familySymbol == null)
                    {
                        message = "Family Symbol not found.";
                        return Result.Failed;
                    }

                    // Ensure the family symbol is activated
                    if (!familySymbol.IsActive)
                    {
                        familySymbol.Activate();
                        doc.Regenerate();
                    }

                    // Prompt the user to pick a point
                    XYZ locationPoint = uidoc.Selection.PickPoint("Select a point to place the electrical fixture");

                    // Place the electrical fixture
                    FamilyInstance familyInstance = doc.Create.NewFamilyInstance(locationPoint, familySymbol, StructuralType.NonStructural);

                    trans.Commit();
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }

        private FamilySymbol GetFamilySymbol(Document doc, string familyName)
        {
            // Filter to find the family symbol by name
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilySymbol));

            foreach (FamilySymbol fs in collector)
            {
                if (fs.Family.Name.Equals(familyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return fs;
                }
            }

            return null;
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
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (_handler == null)
            {
                _handler = new RequestHandler();
                _externalEvent = ExternalEvent.Create(_handler);
            }

            /// Family name to check
            string familyName = "WCD_BJ_Future_1v_Wit";

            /// Path to the family file
#if RELEASE2023
            string familyPath = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R23\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";
#elif RELEASE2024
            string familyPath = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";
#elif RELEASE2025
            string familyPath = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R25\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";
#endif
            /// Load the family
            if (!LoadFamily(doc, familyPath))
            {
                message = "Failed to load family.";
                return Result.Failed;
            }

            /// Activate the symbol if not already activated
            FamilySymbol familySymbol = GetFamilySymbol(doc, familyName);
            if (familySymbol != null)
            {
                if (!familySymbol.IsActive)
                {
                    using (Transaction trans = new Transaction(doc, "Activate Family Symbol"))
                    {
                        trans.Start();
                        familySymbol.Activate();
                        trans.Commit();
                    }
                }

                // Set the symbol in the handler and raise the external event
                _handler.Symbol = familySymbol;
                _externalEvent.Raise();

                return Result.Succeeded;
            }
            else
            {
                message = "Family symbol not found.";
                return Result.Failed;
            }
        }

        private bool LoadFamily(Document doc, string familyPath)
        {
            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "The specified family file does not exist.");
                return false;
            }

            using (Transaction trans = new Transaction(doc, "Load Family"))
            {
                trans.Start();

                Family loadedFamily;
                if (doc.LoadFamily(familyPath, out loadedFamily))
                {
                    trans.Commit();
                    return true;
                }
                else
                {
                    trans.RollBack();
                    return false;
                }
            }
        }

        private FamilySymbol GetFamilySymbol(Document doc, string familyName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilySymbol));

            foreach (FamilySymbol symbol in collector)
            {
                if (symbol.Family.Name.Equals(familyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return symbol;
                }
            }
            return null;
        }
    }
    #endregion

    #region 1v Spatwaterdicht
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Spatwaterdicht1v : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            /// Family name to check
            string familyName = "WCD_Spatwaterdicht_1v_Wit";

            /// Path to the family file
            string familyPath2023 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";
            string familyPath2024 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_Spatwaterdicht_1v_Wit.rfa";
            string familyPath2025 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";

            /// Check if the family is already loaded
            bool isFamilyLoaded = IsFamilyLoaded(doc, familyName);

#if RELEASE2023
            if (!isFamilyLoaded)
            {
                /// Load the family if not already loaded
                if (!LoadFamily(doc, familyPath2023))
                {
                    message = "Failed to load family.";
                    return Result.Failed;
                }
            }
#elif RELEASE2024
            if (!isFamilyLoaded)
            {
                /// Load the family if not already loaded
                if (!LoadFamily(doc, familyPath2024))
                {
                    message = "Failed to load family.";
                    return Result.Failed;
                }
            }
#elif RELEASE2025
            if (!isFamilyLoaded)
            {
                /// Load the family if not already loaded
                if (!LoadFamily(doc, familyPath2025))
                {
                    message = "Failed to load family.";
                    return Result.Failed;
                }
            }
#endif

            /// Place the family with a mouse click
            FamilySymbol familySymbol = GetFamilySymbol(doc, familyName);
            if (familySymbol != null)
            {
                PlaceFamilyWithClick(uiApp, familySymbol);
                return Result.Succeeded;
            }
            else
            {
                message = "Family symbol not found.";
                return Result.Failed;
            }
        }

        private bool IsFamilyLoaded(Document doc, string familyName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Family));

            foreach (Family family in collector)
            {
                if (family.Name.Equals(familyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private bool LoadFamily(Document doc, string familyPath)
        {
            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "The specified family file does not exist.");
                return false;
            }

            using (Transaction trans = new Transaction(doc, "Load Family"))
            {
                trans.Start();

                Family loadedFamily;
                if (doc.LoadFamily(familyPath, out loadedFamily))
                {
                    trans.Commit();
                    return true;
                }
                else
                {
                    trans.RollBack();
                    return false;
                }
            }
        }

        private FamilySymbol GetFamilySymbol(Document doc, string familyName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilySymbol));

            foreach (FamilySymbol symbol in collector)
            {
                if (symbol.Family.Name.Equals(familyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return symbol;
                }
            }
            return null;
        }

        private void PlaceFamilyWithClick(UIApplication uiApp, FamilySymbol familySymbol)
        {
            if (!familySymbol.IsActive)
            {
                using (Transaction trans = new Transaction(uiApp.ActiveUIDocument.Document, "Activate Family Symbol"))
                {
                    trans.Start();
                    familySymbol.Activate();
                    trans.Commit();
                }
            }

            uiApp.ActiveUIDocument.PromptForFamilyInstancePlacement(familySymbol);
        }
    }
    #endregion

    #region 2v Spatwaterdicht
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Spatwaterdicht2v : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            /// Family name to check
            string familyName = "WCD_Spatwaterdicht_2v_Wit";

            /// Path to the family file
            string familyPath2023 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";
            string familyPath2024 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_Spatwaterdicht_2v_Wit.rfa";
            string familyPath2025 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";

            /// Check if the family is already loaded
            bool isFamilyLoaded = IsFamilyLoaded(doc, familyName);

#if RELEASE2023
            if (!isFamilyLoaded)
            {
                /// Load the family if not already loaded
                if (!LoadFamily(doc, familyPath2023))
                {
                    message = "Failed to load family.";
                    return Result.Failed;
                }
            }
#elif RELEASE2024
            if (!isFamilyLoaded)
            {
                /// Load the family if not already loaded
                if (!LoadFamily(doc, familyPath2024))
                {
                    message = "Failed to load family.";
                    return Result.Failed;
                }
            }
#elif RELEASE2025
            if (!isFamilyLoaded)
            {
                /// Load the family if not already loaded
                if (!LoadFamily(doc, familyPath2025))
                {
                    message = "Failed to load family.";
                    return Result.Failed;
                }
            }
#endif

            /// Place the family with a mouse click
            FamilySymbol familySymbol = GetFamilySymbol(doc, familyName);
            if (familySymbol != null)
            {
                PlaceFamilyWithClick(uiApp, familySymbol);
                return Result.Succeeded;
            }
            else
            {
                message = "Family symbol not found.";
                return Result.Failed;
            }
        }

        private bool IsFamilyLoaded(Document doc, string familyName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Family));

            foreach (Family family in collector)
            {
                if (family.Name.Equals(familyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private bool LoadFamily(Document doc, string familyPath)
        {
            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "The specified family file does not exist.");
                return false;
            }

            using (Transaction trans = new Transaction(doc, "Load Family"))
            {
                trans.Start();

                Family loadedFamily;
                if (doc.LoadFamily(familyPath, out loadedFamily))
                {
                    trans.Commit();
                    return true;
                }
                else
                {
                    trans.RollBack();
                    return false;
                }
            }
        }

        private FamilySymbol GetFamilySymbol(Document doc, string familyName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilySymbol));

            foreach (FamilySymbol symbol in collector)
            {
                if (symbol.Family.Name.Equals(familyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return symbol;
                }
            }
            return null;
        }

        private void PlaceFamilyWithClick(UIApplication uiApp, FamilySymbol familySymbol)
        {
            if (!familySymbol.IsActive)
            {
                using (Transaction trans = new Transaction(uiApp.ActiveUIDocument.Document, "Activate Family Symbol"))
                {
                    trans.Start();
                    familySymbol.Activate();
                    trans.Commit();
                }
            }

            uiApp.ActiveUIDocument.PromptForFamilyInstancePlacement(familySymbol);
        }
    }
    #endregion

    #region Perilex Stopcontact
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Perilex : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            /// Family name to check
            string familyName = "WCD_Perilex_ABL_Wit";

            /// Path to the family file
            string familyPath2023 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";
            string familyPath2024 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_Perilex_ABL_Wit.rfa";
            string familyPath2025 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";

            /// Check if the family is already loaded
            bool isFamilyLoaded = IsFamilyLoaded(doc, familyName);

#if RELEASE2023
            if (!isFamilyLoaded)
            {
                /// Load the family if not already loaded
                if (!LoadFamily(doc, familyPath2023))
                {
                    message = "Failed to load family.";
                    return Result.Failed;
                }
            }
#elif RELEASE2024
            if (!isFamilyLoaded)
            {
                /// Load the family if not already loaded
                if (!LoadFamily(doc, familyPath2024))
                {
                    message = "Failed to load family.";
                    return Result.Failed;
                }
            }
#elif RELEASE2025
            if (!isFamilyLoaded)
            {
                /// Load the family if not already loaded
                if (!LoadFamily(doc, familyPath2025))
                {
                    message = "Failed to load family.";
                    return Result.Failed;
                }
            }
#endif

            /// Place the family with a mouse click
            FamilySymbol familySymbol = GetFamilySymbol(doc, familyName);
            if (familySymbol != null)
            {
                PlaceFamilyWithClick(uiApp, familySymbol);
                return Result.Succeeded;
            }
            else
            {
                message = "Family symbol not found.";
                return Result.Failed;
            }
        }

        private bool IsFamilyLoaded(Document doc, string familyName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Family));

            foreach (Family family in collector)
            {
                if (family.Name.Equals(familyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private bool LoadFamily(Document doc, string familyPath)
        {
            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "The specified family file does not exist.");
                return false;
            }

            using (Transaction trans = new Transaction(doc, "Load Family"))
            {
                trans.Start();

                Family loadedFamily;
                if (doc.LoadFamily(familyPath, out loadedFamily))
                {
                    trans.Commit();
                    return true;
                }
                else
                {
                    trans.RollBack();
                    return false;
                }
            }
        }

        private FamilySymbol GetFamilySymbol(Document doc, string familyName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilySymbol));

            foreach (FamilySymbol symbol in collector)
            {
                if (symbol.Family.Name.Equals(familyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return symbol;
                }
            }
            return null;
        }

        private void PlaceFamilyWithClick(UIApplication uiApp, FamilySymbol familySymbol)
        {
            if (!familySymbol.IsActive)
            {
                using (Transaction trans = new Transaction(uiApp.ActiveUIDocument.Document, "Activate Family Symbol"))
                {
                    trans.Start();
                    familySymbol.Activate();
                    trans.Commit();
                }
            }

            uiApp.ActiveUIDocument.PromptForFamilyInstancePlacement(familySymbol);
        }
    }
    #endregion

    #region Krachtstroom Stopcontact
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Krachtstroom : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            /// Family name to check
            string familyName = "WCD_WP_Krachtstroom";

            /// Path to the family file
            string familyPath2023 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";
            string familyPath2024 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_WP_Krachtstroom.rfa";
            string familyPath2025 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";

            /// Check if the family is already loaded
            bool isFamilyLoaded = IsFamilyLoaded(doc, familyName);

#if RELEASE2023
            if (!isFamilyLoaded)
            {
                /// Load the family if not already loaded
                if (!LoadFamily(doc, familyPath2023))
                {
                    message = "Failed to load family.";
                    return Result.Failed;
                }
            }
#elif RELEASE2024
            if (!isFamilyLoaded)
            {
                /// Load the family if not already loaded
                if (!LoadFamily(doc, familyPath2024))
                {
                    message = "Failed to load family.";
                    return Result.Failed;
                }
            }
#elif RELEASE2025
            if (!isFamilyLoaded)
            {
                /// Load the family if not already loaded
                if (!LoadFamily(doc, familyPath2025))
                {
                    message = "Failed to load family.";
                    return Result.Failed;
                }
            }
#endif

            /// Place the family with a mouse click
            FamilySymbol familySymbol = GetFamilySymbol(doc, familyName);
            if (familySymbol != null)
            {
                PlaceFamilyWithClick(uiApp, familySymbol);
                return Result.Succeeded;
            }
            else
            {
                message = "Family symbol not found.";
                return Result.Failed;
            }
        }

        private bool IsFamilyLoaded(Document doc, string familyName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Family));

            foreach (Family family in collector)
            {
                if (family.Name.Equals(familyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private bool LoadFamily(Document doc, string familyPath)
        {
            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "The specified family file does not exist.");
                return false;
            }

            using (Transaction trans = new Transaction(doc, "Load Family"))
            {
                trans.Start();

                Family loadedFamily;
                if (doc.LoadFamily(familyPath, out loadedFamily))
                {
                    trans.Commit();
                    return true;
                }
                else
                {
                    trans.RollBack();
                    return false;
                }
            }
        }

        private FamilySymbol GetFamilySymbol(Document doc, string familyName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilySymbol));

            foreach (FamilySymbol symbol in collector)
            {
                if (symbol.Family.Name.Equals(familyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return symbol;
                }
            }
            return null;
        }

        private void PlaceFamilyWithClick(UIApplication uiApp, FamilySymbol familySymbol)
        {
            if (!familySymbol.IsActive)
            {
                using (Transaction trans = new Transaction(uiApp.ActiveUIDocument.Document, "Activate Family Symbol"))
                {
                    trans.Start();
                    familySymbol.Activate();
                    trans.Commit();
                }
            }

            uiApp.ActiveUIDocument.PromptForFamilyInstancePlacement(familySymbol);
        }
    }
    #endregion

    #region Vloer Stopcontact
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Vloerstopcontact : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            /// Family name to check
            string familyName = "WCD_Vloer_1v";

            /// Path to the family file
            string familyPath2023 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";
            string familyPath2024 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_Vloer_1v.rfa";
            string familyPath2025 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";

            /// Check if the family is already loaded
            bool isFamilyLoaded = IsFamilyLoaded(doc, familyName);

#if RELEASE2023
            if (!isFamilyLoaded)
            {
                /// Load the family if not already loaded
                if (!LoadFamily(doc, familyPath2023))
                {
                    message = "Failed to load family.";
                    return Result.Failed;
                }
            }
#elif RELEASE2024
            if (!isFamilyLoaded)
            {
                /// Load the family if not already loaded
                if (!LoadFamily(doc, familyPath2024))
                {
                    message = "Failed to load family.";
                    return Result.Failed;
                }
            }
#elif RELEASE2025
            if (!isFamilyLoaded)
            {
                /// Load the family if not already loaded
                if (!LoadFamily(doc, familyPath2025))
                {
                    message = "Failed to load family.";
                    return Result.Failed;
                }
            }
#endif

            /// Place the family with a mouse click
            FamilySymbol familySymbol = GetFamilySymbol(doc, familyName);
            if (familySymbol != null)
            {
                PlaceFamilyWithClick(uiApp, familySymbol);
                return Result.Succeeded;
            }
            else
            {
                message = "Family symbol not found.";
                return Result.Failed;
            }
        }

        private bool IsFamilyLoaded(Document doc, string familyName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Family));

            foreach (Family family in collector)
            {
                if (family.Name.Equals(familyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private bool LoadFamily(Document doc, string familyPath)
        {
            if (!File.Exists(familyPath))
            {
                TaskDialog.Show("Error", "The specified family file does not exist.");
                return false;
            }

            using (Transaction trans = new Transaction(doc, "Load Family"))
            {
                trans.Start();

                Family loadedFamily;
                if (doc.LoadFamily(familyPath, out loadedFamily))
                {
                    trans.Commit();
                    return true;
                }
                else
                {
                    trans.RollBack();
                    return false;
                }
            }
        }

        private FamilySymbol GetFamilySymbol(Document doc, string familyName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilySymbol));

            foreach (FamilySymbol symbol in collector)
            {
                if (symbol.Family.Name.Equals(familyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return symbol;
                }
            }
            return null;
        }

        private void PlaceFamilyWithClick(UIApplication uiApp, FamilySymbol familySymbol)
        {
            if (!familySymbol.IsActive)
            {
                using (Transaction trans = new Transaction(uiApp.ActiveUIDocument.Document, "Activate Family Symbol"))
                {
                    trans.Start();
                    familySymbol.Activate();
                    trans.Commit();
                }
            }

            uiApp.ActiveUIDocument.PromptForFamilyInstancePlacement(familySymbol);
        }
    }
    #endregion
}

