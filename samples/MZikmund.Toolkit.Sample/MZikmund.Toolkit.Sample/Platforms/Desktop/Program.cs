using Uno.UI.Hosting;

namespace MZikmund.Toolkit.Sample;

internal class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        App.InitializeLogging();

        var host = UnoPlatformHostBuilder.Create()
            .App(() => new App())
            .UseWin32()
            .UseX11()
            .UseMacOS()
            .Build();

        host.Run();
    }
}
