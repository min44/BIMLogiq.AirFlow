module BIMLogiq.AirFlow.CoreFs.SelectionFilter

open Autodesk.Revit.DB
open Autodesk.Revit.UI.Selection

type SelectionFilterByCategories(categories: BuiltInCategory list) =
    interface ISelectionFilter with
        override this.AllowElement element =
            categories
            |> List.map ElementId
            |> List.contains element.Category.Id
        override this.AllowReference(_, _) = false