using System;
using System.IO;
using System.Reflection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BIMLogiq.AirFlow.Config;

namespace BIMLogiq.AirFlow.Startup;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class MainCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData cd, ref string message, ElementSet elements)
    {
        try
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var directory = Path.GetDirectoryName(location);
            if (directory == null) return Result.Failed;
            var path = Path.Combine(directory, Constants.CoreDll);
            var rawAssembly = File.ReadAllBytes(path);
            var assembly = Assembly.Load(rawAssembly);
            var type = assembly.GetType(
                $"{Constants.Company}.{Constants.AppName}.{Constants.Core}.{Constants.CalculateAirFlow}");
            var methodInfo = type?.GetMethod(Constants.Calculate);
            if (methodInfo == null) return Result.Failed;
            var del = Delegate.CreateDelegate(typeof(Func<UIApplication, double>), methodInfo);
            del.Method.Invoke(null, new object[] {cd.Application});
            return Result.Succeeded;
        }
        catch (Exception e)
        {
            TaskDialog.Show("AirFlow MainCommand Error", e.Message);
            return Result.Failed;
        }
    }
}