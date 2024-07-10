using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using NijhofAddIn.Revit.Commands.Prefab.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NijhofAddIn.Revit.Commands.Wijzigen
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class StandleidingLengte : IExternalCommand
    {
        public Result Execute(ExternalCommandData extCmdData, ref string msg, ElementSet element)
        {
            UIApplication uiapp = extCmdData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            HoekVerkeerd = true;

            try
            {
                //-------------------------Pick opjects aanroepen met juiste filter-------------------------------
                var selectedObjects = uidoc.Selection.PickObjects(ObjectType.Element, new ESFFittings(), "Selecteer de bochten van de standleiding.");
                if (selectedObjects.Count == 0)
                {
                    TaskDialog.Show("Error", "Niks geselecteerd.");
                    return Result.Cancelled;
                }

                List<Element> elementFittingListToModify = new List<Element>();
                foreach (var item in selectedObjects)
                {
                    var fittingElement = doc.GetElement(item);
                    elementFittingListToModify.Add(fittingElement);
                }

                //---------------------als er meer hulpstukken geselecteerd worden test----------------------
                if (elementFittingListToModify.Count > 2)
                {
                    TaskDialog.Show("Error", "Er zijn teveel hulpstukken geselecteerd");
                    return Result.Cancelled;
                }
                else if (elementFittingListToModify.Count < 2)
                {
                    TaskDialog.Show("Error", "Er zijn te weinig hulpstukken geseleceerd.");
                    return Result.Cancelled;
                }

                //---------------------voor als er sono met dempingsmof gebruikt wordt----------------------
                Parameter parameterType = elementFittingListToModify[0].LookupParameter("Family and Type");
                Boolean sonoDemping = false;
                if (parameterType.AsValueString().Contains("SA_Bend_Circular_MEPcontent_DykaSono PVC Binnenriolering_1SR"))
                {
                    sonoDemping |= true;
                }
                else if (parameterType.AsValueString().Contains("SA_Bend_Circular_MEPcontent_DykaSono PVC Binnenriolering_2SR") ||
                            parameterType.AsValueString().Contains("SA_Bend_Composite 2x45_MEPcontent_DykaSono PVC Binnenriolering_2SR-1SR"))
                {
                    TaskDialog.Show("Stl Lengte", "Let op, er is Sono toegepast maar geen dempingsmof.");
                }

                //-------------------------------------Uitvoering--------------------------------------------
                var FittingAboveXYZ = new XYZ();
                Element fittingToMove = null;
                //Element fittingabove = null;
                Element fitting1 = elementFittingListToModify[0];
                Element fitting2 = elementFittingListToModify[1];
                var fittingLocationPoint1 = fitting1.Location as LocationPoint;
                var fittingLocationPoint2 = fitting2.Location as LocationPoint;

                if (fittingLocationPoint1.Point.Z > fittingLocationPoint2.Point.Z)
                {
                    //fittingabove = fitting1;
                    fittingToMove = fitting2;
                    FittingAboveXYZ = fittingLocationPoint1.Point;
                }
                else
                {
                    //fittingabove = fitting2;
                    fittingToMove = fitting1;
                    FittingAboveXYZ = fittingLocationPoint2.Point;
                }

                double angleDeg = AngleDeg(doc, fittingToMove, FittingAboveXYZ);
                angleDeg = (Math.Round(angleDeg, 0));
                if (angleDeg == Convert.ToDouble(0) || angleDeg == Convert.ToDouble(45) || angleDeg == Convert.ToDouble(90) || angleDeg == Convert.ToDouble(135) || angleDeg == Convert.ToDouble(180) ||
                    angleDeg == Convert.ToDouble(225) || angleDeg == Convert.ToDouble(270) || angleDeg == Convert.ToDouble(315) || angleDeg == Convert.ToDouble(360)) { }
                else
                {
                    HoekVerkeerd = false;
                }

                using (var transaction = new Transaction(doc, "NT - Stl lengte aanpassen"))
                {
                    transaction.Start();

                    if (HoekVerkeerd)
                    {
                        if (!sonoDemping)
                        {
                            OnderFittingMove(doc, elementFittingListToModify, fittingToMove, FittingAboveXYZ, angleDeg);
                        }
                        else if (sonoDemping)
                        {
                            int indexCounter = 0;
                            while (Math.Round(GetRelativeDistance(elementFittingListToModify), 3) != 0.714 && indexCounter <= 25)
                            {
                                OnderFittingMove(doc, elementFittingListToModify, fittingToMove, FittingAboveXYZ, angleDeg);
                                BovenFittingMove(doc, elementFittingListToModify);
                                indexCounter++;
                            }
                        }
                    }
                    else
                    {
                        TaskDialog.Show("Error", "De hoek van de installatie is helaas niet geschikt voor deze functie.");
                        transaction.Commit();
                        return Result.Cancelled;
                    }
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                return Result.Failed;
            }
            return Result.Succeeded;
        }
        public XYZ Translation { get; set; }
        public Boolean HoekVerkeerd { get; set; }
        public void OnderFittingMove(Document doc, List<Element> elementFittingListToModify, Element fittingToMove, XYZ FittingAboveXYZ, double angleDeg)
        {
            if (elementFittingListToModify is null)
            {
                throw new ArgumentNullException(nameof(elementFittingListToModify));
            }

            var fittingToMoveLocationPoint = fittingToMove.Location as LocationPoint;
            var dpX = FittingAboveXYZ.X;
            var dpY = FittingAboveXYZ.Y;
            var dfX = fittingToMoveLocationPoint.Point.X;
            var dfY = fittingToMoveLocationPoint.Point.Y;
            var dTotal = 0.714557991;
            var dSchuin = 0.5052493438;

            switch ((Math.Round(angleDeg, 0)))
            {
                case 0:
                    Translation = new XYZ(((dpX - dfX) - dTotal), 0, 0);
                    break;

                case 45:
                    Translation = new XYZ(((dpX - dfX) - dSchuin), ((dpY - dfY) - dSchuin), 0);
                    break;

                case 90:
                    Translation = new XYZ(0, ((dpY - dfY) - dTotal), 0);
                    break;

                case 135:
                    Translation = new XYZ(((dpX - dfX) + dSchuin), ((dpY - dfY) - dSchuin), 0);
                    break;

                case 180:
                    Translation = new XYZ(((dpX - dfX) + dTotal), 0, 0);
                    break;

                case 225:
                    Translation = new XYZ(((dpX - dfX) + dSchuin), ((dpY - dfY) + dSchuin), 0);
                    break;

                case 270:
                    Translation = new XYZ(0, ((dpY - dfY) + dTotal), 0);
                    break;

                case 315:
                    Translation = new XYZ(((dpX - dfX) - dSchuin), ((dpY - dfY) + dSchuin), 0);
                    break;

                case 360:
                    Translation = new XYZ(((dpX - dfX) - dTotal), 0, 0);
                    break;

                default:
                    //TaskDialog.Show("Error", "De hoek van de installatie is helaas niet geschikt voor deze functie.");
                    HoekVerkeerd = false;
                    break;
            }

            ElementTransformUtils.MoveElement(doc, fittingToMove.Id, Translation);


        }
        public double AngleDeg(Document doc, Element fittingToMove, XYZ FittingAboveXYZ)
        {
            if (doc is null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            var fittingToMoveLocation = fittingToMove.Location as LocationPoint;

            double dx = (Math.Round(FittingAboveXYZ.X, 3)) - (Math.Round(fittingToMoveLocation.Point.X, 3));
            double dy = (Math.Round(FittingAboveXYZ.Y, 3)) - (Math.Round(fittingToMoveLocation.Point.Y, 3));

            //Bereken de hoek in radialen
            double angleRad = Math.Atan2(dy, dx);

            //zorg ervoor dat de hoek positief is (in graden)
            double angleDeg = angleRad * (180.0 / Math.PI);
            if (angleDeg < 0)
            {
                angleDeg += 360.0;
            }
            return angleDeg;
        }
        public void BovenFittingMove(Document doc, List<Element> elementFittingListToModify)
        {
            Element fittingabove;
            Element fitting1 = elementFittingListToModify[0];
            Element fitting2 = elementFittingListToModify[1];
            var fittingLocationPoint1 = fitting1.Location as LocationPoint;
            var fittingLocationPoint2 = fitting2.Location as LocationPoint;

            if (fittingLocationPoint1.Point.Z > fittingLocationPoint2.Point.Z)
            {
                fittingabove = fitting1;
            }
            else
            {
                fittingabove = fitting2;
            }

            ElementTransformUtils.MoveElement(doc, fittingabove.Id, -0.5 * Translation);
        }
        public double GetRelativeDistance(List<Element> elementFittingListToModify)
        {
            Element fitting1 = elementFittingListToModify[0];
            Element fitting2 = elementFittingListToModify[1];
            var fittingLocationPoint1 = fitting1.Location as LocationPoint;
            var fittingLocationPoint2 = fitting2.Location as LocationPoint;

            // Bereken het verschil in X- en Y-coördinaten
            double deltaX = fittingLocationPoint2.Point.X - fittingLocationPoint1.Point.X;
            double deltaY = fittingLocationPoint2.Point.Y - fittingLocationPoint1.Point.Y;

            // Maak een nieuwe XYZ om het verschil op te slaan
            double relativeDistance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);


            return relativeDistance;
        }
    }
}
