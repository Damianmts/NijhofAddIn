using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NijhofAddIn.Revit.Commands.Elektra
{
    #region 1v Stopcontact
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Stopcontact1v : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Family name to check
            string familyName = "WCD_BJ_Future_1v_Wit";

            // Path to the family file
            string familyPath2023 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";
            string familyPath2024 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";
            string familyPath2025 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";

            // Check if the family is already loaded
            bool isFamilyLoaded = IsFamilyLoaded(doc, familyName);

#if RELEASE2023
            if (!isFamilyLoaded)
            {
                // Load the family if not already loaded
                if (!LoadFamily(doc, familyPath2023))
                {
                    message = "Failed to load family.";
                    return Result.Failed;
                }
            }
#elif RELEASE2024
        if (!isFamilyLoaded)
        {
            // Load the family if not already loaded
            if (!LoadFamily(doc, familyPath2024))
            {
                message = "Failed to load family.";
                return Result.Failed;
            }
        }
#elif RELEASE2025
        if (!isFamilyLoaded)
        {
            // Load the family if not already loaded
            if (!LoadFamily(doc, familyPath2025))
            {
                message = "Failed to load family.";
                return Result.Failed;
            }
        }
#endif

            // Place the family at a specified point
            FamilySymbol familySymbol = GetFamilySymbol(doc, familyName);
            if (familySymbol != null)
            {
                // Start the transaction before prompting for point selection
                using (Transaction trans = new Transaction(doc, "Place Family Instance"))
                {
                    trans.Start();
                    try
                    {
                        XYZ insertionPoint = uidoc.Selection.PickPoint("Select a point to place the family instance.");
                        if (!familySymbol.IsActive)
                        {
                            familySymbol.Activate();
                        }
                        doc.Create.NewFamilyInstance(insertionPoint, familySymbol, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                        trans.Commit();
                    }
                    catch (OperationCanceledException)
                    {
                        trans.RollBack();
                        message = "Operation cancelled.";
                        return Result.Cancelled;
                    }
                    catch (Exception ex)
                    {
                        trans.RollBack();
                        message = ex.Message;
                        return Result.Failed;
                    }
                }
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
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            /// Family name to check
            string familyName = "WCD_BJ_Future_2v_Wit";

            /// Path to the family file
            string familyPath2023 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";
            string familyPath2024 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_BJ_Future_2v_Wit.rfa";
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

    #region Opbouw Stopcontact
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Opbouw : IExternalCommand
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
            string familyName = "WCD_Gira_2v_Wit";

            /// Path to the family file
            string familyPath2023 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_BJ_Future_1v_Wit.rfa";
            string familyPath2024 = @"G:\Mijn Drive\2. Werk\2. Nijhof\3. Revit\2. Families\60. Elektra\1. Busch Jaeger\1. Future Linear\2. Wit R24\2. Stopcontacten\1. Definitief\WCD_Gira_2v_Wit.rfa";
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
