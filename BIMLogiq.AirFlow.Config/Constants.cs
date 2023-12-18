namespace BIMLogiq.AirFlow.Config;

public static class Constants
{
    public const string AppName          = "AirFlow";
    public const string Company          = "BIMLogiq";
    private const string Startup         = "Startup";
    public const string Core             = "Core";
    public const string Config           = "Config";
    public const string CoreDll          = $"{Company}.{AppName}.{Core}.dll";
    public const string StartupDll       = $"{Company}.{AppName}.{Startup}.dll";
    public const string CalculateAirFlow = "CalculateAirFlow";
    public const string Calculate        = "Calculate";
    public const string CalculatePng     = $"{Calculate}.png";
    public const double AirFlowFactor    = 28.31684659199994;
    
}