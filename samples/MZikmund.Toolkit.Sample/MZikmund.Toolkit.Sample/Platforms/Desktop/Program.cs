namespace MZikmund.Toolkit.Sample;

internal class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        App.InitializeLogging();

        Microsoft.UI.Xaml.Application.Start(_ => new App());
    }
}
