module BIMLogiq.AirFlow.CoreFs.CalculateAirFlow

open System
open System.Linq
open Autodesk.Revit.UI
open Autodesk.Revit.UI.Selection
open Autodesk.Revit.DB
open BIMLogiq.AirFlow.Config
open BIMLogiq.AirFlow.CoreFs
open SelectionFilter

let FilteredCategories =
   [ BuiltInCategory.OST_DuctCurves
     BuiltInCategory.OST_DuctFitting
     BuiltInCategory.OST_FlexDuctCurves ]

let GetConnectors (element: Element) =
    match element with
    | :? MEPCurve       as curve    -> curve.ConnectorManager.Connectors.Cast<Connector>()
    | :? FamilyInstance as instance -> instance.MEPModel.ConnectorManager.Connectors.Cast<Connector>()
    | _ -> []
    
let GetRefs = Seq.map(fun (c: Connector) -> c.AllRefs.Cast<Connector>()) >> Seq.concat

let GetOwners = Seq.map(fun (c: Connector) -> c.Owner)

let rec GetConnectedElements (acc: Element list) element =
    element
    |> GetConnectors
    |> GetRefs
    |> GetOwners
    |> Seq.filter(fun e -> acc |> Seq.exists(fun c -> c.Id = e.Id) |> not)
    |> Seq.fold GetConnectedElements (element :: acc)
    |> List.distinctBy(fun e -> e.Id)

let Calculate (uiApp: UIApplication) =
    try
    let uiDoc  = uiApp.ActiveUIDocument
    let doc    = uiDoc.Document
    let filter = SelectionFilterByCategories(FilteredCategories)
    let reference = uiDoc.Selection.PickObject(ObjectType.Element, filter, "Select a duct")
    let element  = doc.GetElement(reference)
    let elements = GetConnectedElements [] element
    let isDuctTerminal (x: Element) = x.Category.Id.IntegerValue = int BuiltInCategory.OST_DuctTerminal
    let getValues      (x: Element) = try x.get_Parameter(BuiltInParameter.RBS_DUCT_FLOW_PARAM).AsDouble() with _ -> 0.0
    let airTerminals      = List.filter isDuctTerminal elements
    let airTerminalsCount = airTerminals.Length
    let airFlowSum =
        airTerminals
        |> List.map getValues
        |> List.sum
        |> fun v -> v * Constants.AirFlowFactor
        |> fun v -> Math.Round(v, 2)
    TaskDialog.Show(Constants.Calculate, $"airTerminalsCount={airTerminalsCount} airFlowSum={airFlowSum}") |> ignore
    0.0
    with ex ->
        TaskDialog.Show("Error ssss", ex.Message) |> ignore
        0.0