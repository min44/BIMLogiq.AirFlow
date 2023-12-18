using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace BIMLogiq.AirFlow.Core;

public class CategorySelectionFilter : ISelectionFilter
{
    private List<BuiltInCategory> Categories { get; set; }
    public CategorySelectionFilter(List<BuiltInCategory> categories)
    {
        Categories = categories;
    }
    public bool AllowElement(Element e) 
    {
        var catIds = Categories.Select(c => new ElementId(c)).ToList();
        return catIds.Contains(e.Category.Id);
    }
    
    public bool AllowReference(Reference reference, XYZ position)
    {
        return false;
    }
}