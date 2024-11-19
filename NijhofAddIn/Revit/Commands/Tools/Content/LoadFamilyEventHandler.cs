using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.IO;

namespace NijhofAddIn.Revit.Commands.Tools.Content
{
    public class LoadFamilyEventHandler : IExternalEventHandler
    {
        private List<string> _familyPaths;

        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;

            using (Transaction tx = new Transaction(doc, "Load Families"))
            {
                tx.Start();

                foreach (string familyPath in _familyPaths)
                {
                    if (File.Exists(familyPath))
                    {
                        Family family;
                        if (!doc.LoadFamily(familyPath, out family))
                        {
                            TaskDialog.Show("Fout", $"Kon de familie niet laden: {familyPath}");
                        }
                    }
                    else
                    {
                        TaskDialog.Show("Fout", $"Familiebestand niet gevonden: {familyPath}");
                    }
                }

                tx.Commit();
            }
        }

        public string GetName()
        {
            return "Load Family Event Handler";
        }

        public void SetFamiliesToLoad(List<string> familyPaths)
        {
            _familyPaths = familyPaths;
        }
    }
}
