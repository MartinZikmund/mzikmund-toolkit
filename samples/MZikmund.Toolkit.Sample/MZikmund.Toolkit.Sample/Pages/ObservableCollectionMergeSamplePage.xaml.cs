using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Extensions;

namespace MZikmund.Toolkit.Sample;

public sealed partial class ObservableCollectionMergeSamplePage : Page
{
    private static readonly string[] InitialItems = ["Alpha", "Bravo", "Charlie", "Delta", "Echo"];

    private readonly ObservableCollection<string> _live = new(InitialItems);

    public ObservableCollectionMergeSamplePage()
    {
        this.InitializeComponent();
        LiveCollectionList.ItemsSource = _live;
        ResetEditor();
    }

    private void Merge_Click(object sender, RoutedEventArgs e)
    {
        var desired = (DesiredSequenceInput.Text ?? string.Empty)
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .Where(line => line.Length > 0)
            .ToList();

        _live.MergeWith(desired);
    }

    private void Shuffle_Click(object sender, RoutedEventArgs e)
    {
        var random = Random.Shared;
        var shuffled = _live.OrderBy(_ => random.Next()).ToList();
        DesiredSequenceInput.Text = string.Join(Environment.NewLine, shuffled);
    }

    private void Reset_Click(object sender, RoutedEventArgs e)
    {
        _live.MergeWith(InitialItems);
        ResetEditor();
    }

    private void ResetEditor()
    {
        DesiredSequenceInput.Text = string.Join(Environment.NewLine, _live);
    }
}
