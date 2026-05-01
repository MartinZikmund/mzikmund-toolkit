using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Models;

namespace MZikmund.Toolkit.Sample;

public sealed partial class PaginatedResponseSamplePage : Page
{
    private static readonly string[] AllItems =
        Enumerable.Range(1, 137).Select(i => $"Item #{i:000}").ToArray();

    private int _currentPage = 1;
    private int _pageSize = 10;

    public PaginatedResponseSamplePage()
    {
        this.InitializeComponent();
        PageSizePicker.SelectedIndex = 1;
        Refresh();
    }

    private void Prev_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        _currentPage--;
        Refresh();
    }

    private void Next_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        _currentPage++;
        Refresh();
    }

    private void PageSize_Changed(object sender, SelectionChangedEventArgs e)
    {
        if (PageSizePicker.SelectedItem is int newSize && newSize > 0)
        {
            _pageSize = newSize;
            _currentPage = 1;
            Refresh();
        }
    }

    private void Refresh()
    {
        var response = QueryPage(_currentPage, _pageSize);

        ItemsList.ItemsSource = response.Items;
        StatusText.Text =
            $"Page {response.Page} of {response.TotalPages} • {response.Items.Length} of {response.TotalCount} items";
        PrevButton.IsEnabled = response.HasPreviousPage;
        NextButton.IsEnabled = response.HasNextPage;
    }

    private static PaginatedResponse<string> QueryPage(int page, int pageSize)
    {
        if (pageSize <= 0)
        {
            return PaginatedResponse<string>.Empty();
        }

        var skip = (page - 1) * pageSize;
        var slice = AllItems.Skip(skip).Take(pageSize).ToArray();
        return new PaginatedResponse<string>(slice, page, pageSize, AllItems.Length);
    }
}
