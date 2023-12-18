using Autodesk.Revit.UI;

namespace BIMLogiq.AirFlow.Startup;

public class Main : IExternalApplication
{
    public Result OnStartup(UIControlledApplication uiApp)
    {
        RibbonPanel.InitRibbonPanel(uiApp);

        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        return Result.Succeeded;
    }
}