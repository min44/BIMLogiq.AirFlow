using System;
using Autodesk.Revit.UI;
using BIMLogiq.AirFlow.Config;

namespace BIMLogiq.AirFlow.Startup;

public static class RibbonPanel
{
    public static void InitRibbonPanel(UIControlledApplication uiApp)
    {
        try
        {
            const string appName = Constants.AppName;
            const string calculate = Constants.Calculate;
            var location = typeof(MainCommand).Assembly.Location;
            var name = typeof(MainCommand).FullName;
            uiApp.CreateRibbonTab(appName);
            var panel = uiApp.CreateRibbonPanel(appName, appName);
            panel.Title = appName;
            var buttonData = new PushButtonData(calculate, calculate, location, name);
            var image = ResourceAssembly.GetImage(Constants.CalculatePng);
            buttonData.LargeImage = image;
            panel.AddItem(buttonData);
        }
        catch (Exception e)
        {
            TaskDialog.Show("AirFlow InitRibbonPanel Error", e.Message);
        }
    }
}