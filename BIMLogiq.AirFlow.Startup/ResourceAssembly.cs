using System.Reflection;
using System.Windows.Media.Imaging;

namespace BIMLogiq.AirFlow.Startup;

public static class ResourceAssembly
{
    public static BitmapImage GetImage(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream($"BIMLogiq.AirFlow.Startup.Images.{name}");
        if (stream == null) return null;
        var image = new BitmapImage();
        image.BeginInit();
        image.StreamSource = stream;
        image.EndInit();
        return image;
    }
}