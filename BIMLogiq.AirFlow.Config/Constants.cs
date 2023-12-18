namespace BIMLogiq.AirFlow.Config;

public static class Constants
{
    public const string  AppName          = "AirFlow";
    private const string Company          = "BIMLogiq";
    private const string Startup          = "Startup";
    private const string Core             = "Core";
    public const string  Config           = "Config";
    private const string StartupNs        = $"{Company}.{AppName}.{Startup}";
    private const string CoreNs           = $"{Company}.{AppName}.{Core}";
    public const string  CoreFsNs         = $"{Company}.{AppName}.{Core}Fs";
    public const string  CoreDll          = $"{CoreNs}.dll";
    public const string  CoreFsDll        = $"{CoreFsNs}.dll";
    public const string  StartupDll       = $"{StartupNs}.dll";
    public const string  CalculateAirFlow = "CalculateAirFlow";
    public const string  Calculate        = "Calculate";
    public const string  CalculatePng     = $"{Calculate}.png";
    public const double  AirFlowFactor    = 28.31684659199994;
    
}