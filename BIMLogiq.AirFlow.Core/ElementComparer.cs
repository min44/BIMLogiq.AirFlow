using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace BIMLogiq.AirFlow.Core;

internal class ElementComparer : IEqualityComparer<Element>
{
    public bool Equals(Element x, Element y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
        return x.Id == y.Id && x.Name == y.Name;
    }

    public int GetHashCode(Element element)
    {
        var hashElementName = element.Name == null ? 0 : element.Name.GetHashCode();
        var hashElementCode = element.Id.GetHashCode();
        return hashElementName ^ hashElementCode;
    }
}