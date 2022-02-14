using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RevitAPITraining_2022_3._3
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            {
                
                var selectedElementReference = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, "Выберите элементы");
                var SelectedElement = doc.GetElement(selectedElementReference); 
                

                foreach (var selectedElement in selectedElementReference)
                {
                    Element element = doc.GetElement(selectedElement);
                    if (element is MEPCurve)
                    {
                        using (Transaction ts = new Transaction(doc, "Устанавливаем длину труб с запасом"))
                        {
                            ts.Start();
                            var FamilyInstance = element as FamilyInstance;
                            Parameter baseLength = FamilyInstance.LookupParameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                            double addlength = UnitUtils.ConvertFromInternalUnits(baseLength.AsDouble() * 1.1, UnitTypeId.Meters);
                            ParameterElement addlength1 = FamilyInstance.SetEntity(addlength.ToString());
                            ts.Commit();
                        };
                    }

                    return Result.Succeeded;
                }
            }
        }
    }
}
