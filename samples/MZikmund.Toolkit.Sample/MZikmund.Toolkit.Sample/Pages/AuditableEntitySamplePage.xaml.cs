using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Data;

namespace MZikmund.Toolkit.Sample;

public sealed partial class AuditableEntitySamplePage : Page
{
    private Reminder? _reminder;

    public AuditableEntitySamplePage()
    {
        this.InitializeComponent();
        UpdateState();
    }

    private void Create_Click(object sender, RoutedEventArgs e)
    {
        _reminder = new Reminder { Name = NameInput.Text };
        _reminder.StampForCreate();
        UpdateState();
    }

    private void Update_Click(object sender, RoutedEventArgs e)
    {
        if (_reminder is null)
        {
            StateText.Text = "Create the entity first.";
            return;
        }
        _reminder.Name = NameInput.Text;
        _reminder.StampForUpdate();
        UpdateState();
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (_reminder is null)
        {
            StateText.Text = "Create the entity first.";
            return;
        }
        _reminder.MarkDeleted();
        UpdateState();
    }

    private void Restore_Click(object sender, RoutedEventArgs e)
    {
        if (_reminder is null)
        {
            StateText.Text = "Create the entity first.";
            return;
        }
        _reminder.Restore();
        UpdateState();
    }

    private void UpdateState()
    {
        if (_reminder is null)
        {
            StateText.Text = "(no entity yet)";
            return;
        }

        StateText.Text =
            $"Name: {_reminder.Name}\n" +
            $"CreatedAt: {_reminder.CreatedAt:O}\n" +
            $"LastModifiedAt: {_reminder.LastModifiedAt:O}\n" +
            $"IsDeleted: {_reminder.IsDeleted}\n" +
            $"DeletedAt: {(_reminder.DeletedAt is { } d ? d.ToString("O") : "(null)")}";
    }

    private sealed class Reminder : IAuditableEntity, ISoftDeletable
    {
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
