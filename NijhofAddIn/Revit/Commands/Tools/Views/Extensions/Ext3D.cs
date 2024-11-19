using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Linq;

namespace NijhofAddIn.Revit.Commands.Tools.Views.Extensions
{
    #region Bounding Box Extender
    public class BBE
    {
        public BoundingBoxXYZ BoundingBoxExpander(BoundingBoxXYZ boundingBox, Element element)
        {
            double xMin = boundingBox.Min.X;
            double yMin = boundingBox.Min.Y;
            double zMin = boundingBox.Min.Z;

            double xMax = boundingBox.Max.X;
            double yMax = boundingBox.Max.Y;
            double zMax = boundingBox.Max.Z;

            BoundingBoxXYZ elementBoundingBox = element.get_BoundingBox(null);

            double exMin = elementBoundingBox.Min.X;
            double eyMin = elementBoundingBox.Min.Y;
            double ezMin = elementBoundingBox.Min.Z;

            double exMax = elementBoundingBox.Max.X;
            double eyMax = elementBoundingBox.Max.Y;
            double ezMax = elementBoundingBox.Max.Z;

            if (exMin < xMin)
            {
                boundingBox.Min = new XYZ(exMin, boundingBox.Min.Y, boundingBox.Min.Z);
            }

            if (eyMin < yMin)
            {
                boundingBox.Min = new XYZ(boundingBox.Min.X, eyMin, boundingBox.Min.Z);
            }

            if (ezMin < zMin)
            {
                boundingBox.Min = new XYZ(boundingBox.Min.X, boundingBox.Min.Y, ezMin);
            }

            if (exMax > xMax)
            {
                boundingBox.Max = new XYZ(exMax, boundingBox.Max.Y, boundingBox.Max.Z);
            }

            if (eyMax > yMax)
            {
                boundingBox.Max = new XYZ(boundingBox.Max.X, eyMax, boundingBox.Max.Z);
            }

            if (ezMax > zMax)
            {
                boundingBox.Max = new XYZ(boundingBox.Max.X, boundingBox.Max.Y, ezMax);
            }

            return boundingBox;
        }
    }
    #endregion

    #region Close Active View
    internal class CAV
    {
        public void CloseActiveView(UIDocument uidoc)
        {
            UIView uiView = uidoc.GetOpenUIViews().First(); //.FirstOrDefault(uiv => uiv.ViewId == activeView.Id);
            uiView.Close();
        }
    }
    #endregion

    #region Vector From Horizontal Vertical Angles
    public class VFA
    {
        public XYZ VectorFromHorizVertAngles(
              double angleHorizD,
              double angleVertD)
        {
            /// Convert degrees to radians
            double degToRadian = Math.PI * 2 / 360;
            double angleHorizR = angleHorizD * degToRadian;
            double angleVertR = angleVertD * degToRadian;

            /// Return unit vector in 3D
            double a = Math.Cos(angleVertR);
            double b = Math.Cos(angleHorizR);
            double c = Math.Sin(angleHorizR);
            double d = Math.Sin(angleVertR);

            return new XYZ(a * b, a * c, d);
        }
    }
    #endregion
}
