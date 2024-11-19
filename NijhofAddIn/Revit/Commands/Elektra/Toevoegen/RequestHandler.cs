using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NijhofAddIn.Revit.Commands.Elektra.Toevoegen
{
    public class RequestHandler : IExternalEventHandler
    {
        public FamilySymbol Symbol { get; set; }

        public void Execute(UIApplication app)
        {
            UIDocument uidoc = app.ActiveUIDocument;
            if (Symbol != null)
            {
                uidoc.PostRequestForElementTypePlacement(Symbol);
            }
        }

        public string GetName()
        {
            return "RequestHandler";
        }
    }
}
