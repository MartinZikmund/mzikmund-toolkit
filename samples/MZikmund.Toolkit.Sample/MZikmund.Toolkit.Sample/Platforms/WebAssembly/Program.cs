namespace MZikmund.Toolkit.Sample;

public class Program
{
    public static void Main(string[] args)
    {
        App.InitializeLogging();

        Microsoft.UI.Xaml.Application.Start(_ => new App());
    }
}