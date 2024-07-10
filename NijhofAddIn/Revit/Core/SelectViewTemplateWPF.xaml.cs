using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NijhofAddIn.Revit.Core
{
    public partial class SelectViewTemplateWPF : Window
    {
        Document Doc;

        public Autodesk.Revit.DB.View ViewTemplateSelect { get; private set; }

        public SelectViewTemplateWPF(Document doc)
        {
            InitializeComponent();
            Doc = doc;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var collector3D = new FilteredElementCollector(Doc)
                   .OfCategory(BuiltInCategory.OST_Views)
                   .WhereElementIsNotElementType()
                   .Cast<Autodesk.Revit.DB.View>()
                   .Where(v => v.ViewType == ViewType.ThreeD)
                   .Where(x => x.IsTemplate == true);

            foreach (Element view in collector3D.OrderBy(v => v.Name))
            {
                _3DViewTempaltes.Items.Add(view.Name);
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var collector3D = new FilteredElementCollector(Doc)
                   .OfCategory(BuiltInCategory.OST_Views)
                   .WhereElementIsNotElementType()
                   .Cast<Autodesk.Revit.DB.View>()
                   .Where(v => v.ViewType == ViewType.ThreeD)
                   .Where(x => x.IsTemplate == true);

            string selectedPipeTag = this._3DViewTempaltes.SelectedItem.ToString();

            foreach (Element view in collector3D.OrderBy(v => v.Name))
            {
                if (view.Name == selectedPipeTag)
                {
                    ViewTemplateSelect = view as Autodesk.Revit.DB.View;
                    //break;
                }
            }
            Close();
        }
        private void Cancel(object sender, RoutedEventArgs e)
        {
            throw new InvalidOperationException("Functie is gestopt.");
        }
    }
}
