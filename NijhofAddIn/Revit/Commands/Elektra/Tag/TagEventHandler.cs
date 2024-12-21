using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NijhofAddIn.Revit.Commands.Elektra.Tag
{
    public class TagEventHandler : IExternalEventHandler
    {
        public Action<UIApplication> Operation { get; set; }

        public void Execute(UIApplication app)
        {
            Operation?.Invoke(app);
        }

        public string GetName()
        {
            return "TagEventHandler";
        }
    }
}
