using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace NijhofAddIn.Revit.Commands.Prefab.Views.Extensions
{
    #region Element Selection Filter
    public class ESF : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem is Viewport;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;

        }
    }
    #endregion

    #region ESF Fittings
    public class ESFFittings : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            Parameter parameterType = element.LookupParameter("Family and Type");
            if (parameterType != null && parameterType.HasValue)
            {
                if (parameterType.AsValueString().Contains("SA_Bend_Composite 2x45_MEPcontent_DYKA PVC Binnenriolering_2S-1S") ||
                    parameterType.AsValueString().Contains("SA_Bend_Circular_MEPcontent_DYKA PVC Binnenriolering_2S") ||
                    parameterType.AsValueString().Contains("SA_Bend_Circular_MEPcontent_DykaSono PVC Binnenriolering_2SR") ||
                    parameterType.AsValueString().Contains("SA_Bend_Composite 2x45_MEPcontent_DykaSono PVC Binnenriolering_2SR-1SR") ||
                    parameterType.AsValueString().Contains("SA_Bend_Circular_MEPcontent_DykaSono PVC Binnenriolering_1SR"))
                {
                    return true;
                }
            }
            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;

        }
    }
    #endregion

    #region ESF Pipes
    public class ESFPipe : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            Parameter parameterType = elem.LookupParameter("Article Type");
            Parameter parameterDiameter = elem.LookupParameter("D1 Description");
            if (parameterType != null && parameterType.HasValue)
            {
                if ((parameterType.AsString() == "HWA 6m" || parameterType.AsString() == "PVC 5,55m" || parameterType.AsString() == "HWA 5,55m")
                    && (parameterDiameter.AsString() == "80" || parameterDiameter.AsString() == "100"))
                {
                    return true;
                }
            }
            return false;
            //return elem is Pipe;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;

        }
    }
    #endregion
}
