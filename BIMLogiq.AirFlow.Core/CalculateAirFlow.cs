using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using BIMLogiq.AirFlow.Config;

namespace BIMLogiq.AirFlow.Core;


public class CalculateAirFlow
{
    private static readonly List<BuiltInCategory> FilteredCategories =
        new()
        {
            BuiltInCategory.OST_DuctCurves,
            BuiltInCategory.OST_DuctFitting,
            BuiltInCategory.OST_FlexDuctCurves,
        };
    

    public static double Calculate(UIApplication app)
    {
        try
        {
            var doc = app.ActiveUIDocument.Document;
            var uiDoc = app.ActiveUIDocument;
            var filter = new CategorySelectionFilter(FilteredCategories);
            var reference = uiDoc.Selection.PickObject(ObjectType.Element, filter, "Select a duct");
            var element = doc.GetElement(reference);
            var elements = GetConnectedElements(element);
            var airTerminals = elements.Where(x => x.Category.Id.IntegerValue == (int) BuiltInCategory.OST_DuctTerminal).ToList();
            var airTerminalsCount = airTerminals.Count;
            var airFlowSum = airTerminals.Select(x => x.get_Parameter(BuiltInParameter.RBS_DUCT_FLOW_PARAM)?.AsDouble() ?? 0).Sum();
            var airFlowSumCfm = airFlowSum * Constants.AirFlowFactor;
            var airFlowRound = Math.Round(airFlowSumCfm, 2);
            TaskDialog.Show(Constants.Calculate, $"airTerminalsCount={airTerminalsCount} airFlowSum={airFlowRound}");
            return 0.0;
        }
        catch (Exception ex)
        {
            TaskDialog.Show(Constants.Calculate, ex.Message);
            return 0.0;
        }
    }
    
    
    private static IEnumerable<Connector> GetConnectors(Element element)
    {
        return element switch
        {
            FamilyInstance {MEPModel: not null} fi => fi.MEPModel.ConnectorManager.Connectors.Cast<Connector>(),
            MEPCurve duct => duct.ConnectorManager.Connectors.Cast<Connector>(),
            _ => new List<Connector>()
        };
    }

    
    private static IEnumerable<Connector> GetConnectorsMulti(IEnumerable<Element> elements) =>
        elements.Select(GetConnectors).SelectMany(c => c);

    
    private static IEnumerable<Connector> GetRefs(IEnumerable<Connector> connectors) =>
        connectors.Select(c => c.AllRefs.Cast<Connector>())
            .SelectMany(c => c);

    
    private static IEnumerable<Element> GetOwners(IEnumerable<Connector> connectors) =>
        connectors.Select(connector => connector.Owner);
    
    
    private static IEnumerable<Element> Collector(List<Element> collectorElements, List<Element> owners)
    {
        while (true)
        {
            var elementIds = collectorElements.Select(c => c.Id);
            var conNew = GetConnectorsMulti(owners).ToList();
            var refsNew = GetRefs(conNew).ToList();
            var ownersNew = GetOwners(refsNew).Where(x => !elementIds.Contains(x.Id)).ToList();
            collectorElements.AddRange(ownersNew);
            if (!ownersNew.Any()) return collectorElements;
            owners = ownersNew;
        }
    }

    
    private static IEnumerable<Element> GetConnectedElements(Element element)
    {
        var collectorElements = new List<Element> { element };
        var connectors = GetConnectors(element).ToList();
        var refs = GetRefs(connectors).ToList();
        var owners = GetOwners(refs).ToList();
        collectorElements.AddRange(owners);
        return Collector(collectorElements, owners).Distinct(new ElementComparer()).ToList();
    }
}