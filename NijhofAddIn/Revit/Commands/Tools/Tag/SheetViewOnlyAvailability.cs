using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NijhofAddIn.Revit.Commands.Tools.Tag
{
    public class SheetViewOnlyAvailability : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication app, CategorySet selectedCategories)
        {
            // Controleer of de huidige view een sheet view is
            Document doc = app.ActiveUIDocument?.Document;
            if (doc != null)
            {
                View activeView = doc.ActiveView;
                return activeView.ViewType == ViewType.DrawingSheet;
            }
            return false;
        }
    }
}
