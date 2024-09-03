using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Linq;
using System.IO;

namespace NijhofAddIn.Revit.Commands.GPS
{
    #region Load Alles
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Inladen : IExternalCommand /// Implementing the IExternalCommand interface
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            /// Call the LoadFamilies method from the Execute method
            LoadFamilies(doc);

            return Result.Succeeded;
        }

        public void LoadFamilies(Document doc)
        {
#if RELEASE2023
            string[] familyPaths = {
                @"F:\Stabiplan\Custom\Families\GPS\GPS Riool.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\GPS Lucht.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\GPS Koudwater.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\GPS Warmwater.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\GPS Elektra.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\GPS Meterkast.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\GPS Tag_Intercom.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\GPS Middellijn.rfa",
            };
#elif RELEASE2024
            string[] familyPaths = {
                @"F:\Stabiplan\Custom\Families\GPS\R24\GPS Riool.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\R24\GPS Lucht.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\R24\GPS Koudwater.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\R24\GPS Warmwater.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\R24\GPS Elektra.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\R24\GPS Meterkast.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\R24\GPS Tag_Intercom.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\R24\GPS Middellijn.rfa",
            };
#elif RELEASE2025
            string[] familyPaths = {
                @"F:\Stabiplan\Custom\Families\GPS\R24\GPS Riool.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\R24\GPS Lucht.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\R24\GPS Koudwater.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\R24\GPS Warmwater.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\R24\GPS Elektra.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\R24\GPS Meterkast.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\R24\GPS Tag_Intercom.rfa",
                @"F:\Stabiplan\Custom\Families\GPS\R24\GPS Middellijn.rfa",
            };
#endif

            foreach (string familyPath in familyPaths)
            {
                if (File.Exists(familyPath))
                {
                    string familyName = Path.GetFileNameWithoutExtension(familyPath);
                    var existingFamily = new FilteredElementCollector(doc)
                                            .OfClass(typeof(Family))
                                            .FirstOrDefault(e => e.Name.Equals(familyName));

                    if (existingFamily != null)
                    {
                        /// De familie bestaat al, dus sla het laden over
                        continue;
                    }

                    using (Transaction tx = new Transaction(doc))
                    {
                        try
                        {
                            tx.Start("Load Family");

                            Family family;
                            if (!doc.LoadFamily(familyPath, out family))
                            {
                                TaskDialog.Show("Notification", "Family is already loaded: " + familyPath);
                            }

                            tx.Commit();
                        }
                        catch (Exception ex)
                        {
                            TaskDialog.Show("Error", "Failed to load family: " + ex.Message);
                            if (tx.GetStatus() == TransactionStatus.Started)
                            {
                                tx.RollBack();
                            }
                        }
                    }
                }
                else
                {
                    TaskDialog.Show("Error", "File does not exist: " + familyPath);
                }
            }
        }
    }
#endregion

    #region Plaats Riool
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class InladenRiool : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            /// De naam van de family en type die je wilt plaatsen
            string familyName = "GPS Riool";
            string typeName = "GPS Riool";

            /// Zoek de family en het type
            FamilySymbol familySymbol = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .FirstOrDefault(f => f.Family.Name == familyName && f.Name == typeName);

            if (familySymbol == null)
            {
                TaskDialog.Show("Error", "Could not find the family or type in the project.");
                return Result.Failed;
            }

            try
            {
                /// Sta de gebruiker toe om een punt in de modelruimte te selecteren
                XYZ point = uiDoc.Selection.PickPoint("Please pick a point to place the family");

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("Place Family");

                    if (!familySymbol.IsActive)
                        familySymbol.Activate();

                    /// Plaats het family-exemplaar op het geselecteerde punt
                    FamilyInstance familyInstance = doc.Create.NewFamilyInstance(point, familySymbol, StructuralType.NonStructural);

                    tx.Commit();
                }
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                /// De gebruiker heeft de operatie geannuleerd. Keer terug zonder fout.
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Failed to place family: " + ex.Message);
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region Plaats Lucht
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class InladenLucht : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            /// De naam van de family en type die je wilt plaatsen
            string familyName = "GPS Lucht";
            string typeName = "GPS Lucht";

            /// Zoek de family en het type
            FamilySymbol familySymbol = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .FirstOrDefault(f => f.Family.Name == familyName && f.Name == typeName);

            if (familySymbol == null)
            {
                TaskDialog.Show("Error", "Could not find the family or type in the project.");
                return Result.Failed;
            }

            try
            {
                /// Sta de gebruiker toe om een punt in de modelruimte te selecteren
                XYZ point = uiDoc.Selection.PickPoint("Please pick a point to place the family");

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("Place Family");

                    if (!familySymbol.IsActive)
                        familySymbol.Activate();

                    /// Plaats het family-exemplaar op het geselecteerde punt
                    FamilyInstance familyInstance = doc.Create.NewFamilyInstance(point, familySymbol, StructuralType.NonStructural);

                    tx.Commit();
                }
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                /// De gebruiker heeft de operatie geannuleerd. Keer terug zonder fout.
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Failed to place family: " + ex.Message);
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region Plaats Koudwater
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class InladenKW : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            /// De naam van de family en type die je wilt plaatsen
            string familyName = "GPS Koudwater";
            string typeName = "GPS Koudwater";

            /// Zoek de family en het type
            FamilySymbol familySymbol = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .FirstOrDefault(f => f.Family.Name == familyName && f.Name == typeName);

            if (familySymbol == null)
            {
                TaskDialog.Show("Error", "Could not find the family or type in the project.");
                return Result.Failed;
            }

            try
            {
                /// Sta de gebruiker toe om een punt in de modelruimte te selecteren
                XYZ point = uiDoc.Selection.PickPoint("Please pick a point to place the family");

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("Place Family");

                    if (!familySymbol.IsActive)
                        familySymbol.Activate();

                    /// Plaats het family-exemplaar op het geselecteerde punt
                    FamilyInstance familyInstance = doc.Create.NewFamilyInstance(point, familySymbol, StructuralType.NonStructural);

                    tx.Commit();
                }
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                /// De gebruiker heeft de operatie geannuleerd. Keer terug zonder fout.
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Failed to place family: " + ex.Message);
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region Plaats Warmwater
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class InladenWW : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            /// De naam van de family en type die je wilt plaatsen
            string familyName = "GPS Warmwater";
            string typeName = "GPS Warmwater";

            /// Zoek de family en het type
            FamilySymbol familySymbol = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .FirstOrDefault(f => f.Family.Name == familyName && f.Name == typeName);

            if (familySymbol == null)
            {
                TaskDialog.Show("Error", "Could not find the family or type in the project.");
                return Result.Failed;
            }

            try
            {
                /// Sta de gebruiker toe om een punt in de modelruimte te selecteren
                XYZ point = uiDoc.Selection.PickPoint("Please pick a point to place the family");

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("Place Family");

                    if (!familySymbol.IsActive)
                        familySymbol.Activate();

                    /// Plaats het family-exemplaar op het geselecteerde punt
                    FamilyInstance familyInstance = doc.Create.NewFamilyInstance(point, familySymbol, StructuralType.NonStructural);

                    tx.Commit();
                }
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                /// De gebruiker heeft de operatie geannuleerd. Keer terug zonder fout.
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Failed to place family: " + ex.Message);
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region Plaats Elektra
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class InladenElektra : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            /// De naam van de family en type die je wilt plaatsen
            string familyName = "GPS Elektra";
            string typeName = "GPS Elektra";

            /// Zoek de family en het type
            FamilySymbol familySymbol = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .FirstOrDefault(f => f.Family.Name == familyName && f.Name == typeName);

            if (familySymbol == null)
            {
                TaskDialog.Show("Error", "Could not find the family or type in the project.");
                return Result.Failed;
            }

            try
            {
                /// Sta de gebruiker toe om een punt in de modelruimte te selecteren
                XYZ point = uiDoc.Selection.PickPoint("Please pick a point to place the family");

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("Place Family");

                    if (!familySymbol.IsActive)
                        familySymbol.Activate();

                    /// Plaats het family-exemplaar op het geselecteerde punt
                    FamilyInstance familyInstance = doc.Create.NewFamilyInstance(point, familySymbol, StructuralType.NonStructural);

                    tx.Commit();
                }
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                /// De gebruiker heeft de operatie geannuleerd. Keer terug zonder fout.
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Failed to place family: " + ex.Message);
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region Plaats Meterkast
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class InladenMeterkast : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            /// De naam van de family en type die je wilt plaatsen
            string familyName = "GPS Meterkast";
            string typeName = "GPS Meterkast";

            /// Zoek de family en het type
            FamilySymbol familySymbol = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .FirstOrDefault(f => f.Family.Name == familyName && f.Name == typeName);

            if (familySymbol == null)
            {
                TaskDialog.Show("Error", "Could not find the family or type in the project.");
                return Result.Failed;
            }

            try
            {
                /// Sta de gebruiker toe om een punt in de modelruimte te selecteren
                XYZ point = uiDoc.Selection.PickPoint("Please pick a point to place the family");

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("Place Family");

                    if (!familySymbol.IsActive)
                        familySymbol.Activate();

                    /// Plaats het family-exemplaar op het geselecteerde punt
                    FamilyInstance familyInstance = doc.Create.NewFamilyInstance(point, familySymbol, StructuralType.NonStructural);

                    tx.Commit();
                }
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                /// De gebruiker heeft de operatie geannuleerd. Keer terug zonder fout.
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Failed to place family: " + ex.Message);
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region Plaats Tag_Intercom
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class InladenTI : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            /// De naam van de family en type die je wilt plaatsen
            string familyName = "GPS Tag_Intercom";
            string typeName = "GPS Tag_Intercom";

            /// Zoek de family en het type
            FamilySymbol familySymbol = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .FirstOrDefault(f => f.Family.Name == familyName && f.Name == typeName);

            if (familySymbol == null)
            {
                TaskDialog.Show("Error", "Could not find the family or type in the project.");
                return Result.Failed;
            }

            try
            {
                /// Sta de gebruiker toe om een punt in de modelruimte te selecteren
                XYZ point = uiDoc.Selection.PickPoint("Please pick a point to place the family");

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("Place Family");

                    if (!familySymbol.IsActive)
                        familySymbol.Activate();

                    /// Plaats het family-exemplaar op het geselecteerde punt
                    FamilyInstance familyInstance = doc.Create.NewFamilyInstance(point, familySymbol, StructuralType.NonStructural);

                    tx.Commit();
                }
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                /// De gebruiker heeft de operatie geannuleerd. Keer terug zonder fout.
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Failed to place family: " + ex.Message);
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region Plaats Middellijn

    #endregion
}
