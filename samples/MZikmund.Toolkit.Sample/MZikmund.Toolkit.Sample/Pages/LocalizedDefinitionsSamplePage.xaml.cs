using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Localization;

namespace MZikmund.Toolkit.Sample;

public sealed partial class LocalizedDefinitionsSamplePage : Page
{
    private static readonly DictionaryLocalizer English = new(new()
    {
        ["Achievement_FirstSteps_Name"] = "First Steps",
        ["Achievement_FirstSteps_Description"] = "Complete the onboarding tutorial.",
        ["Achievement_HabitFormed_Name"] = "Habit Formed",
        ["Achievement_HabitFormed_Description"] = "Maintain a streak for 7 days in a row.",
        ["Achievement_Marathon_Name"] = "Marathon",
        ["Achievement_Marathon_Description"] = "Hit a 30-day streak. You're unstoppable.",
    });

    private static readonly DictionaryLocalizer Czech = new(new()
    {
        ["Achievement_FirstSteps_Name"] = "První kroky",
        ["Achievement_FirstSteps_Description"] = "Dokončete úvodní průvodce.",
        ["Achievement_HabitFormed_Name"] = "Návyk vytvořen",
        ["Achievement_HabitFormed_Description"] = "Udržujte sérii 7 dní v řadě.",
        ["Achievement_Marathon_Name"] = "Maraton",
        ["Achievement_Marathon_Description"] = "Dosáhněte 30denní série. Jste nezastavitelní.",
    });

    private static readonly Achievement[] Definitions =
    [
        new("Achievement_FirstSteps_Name", "Achievement_FirstSteps_Description"),
        new("Achievement_HabitFormed_Name", "Achievement_HabitFormed_Description"),
        new("Achievement_Marathon_Name", "Achievement_Marathon_Description"),
    ];

    public LocalizedDefinitionsSamplePage()
    {
        Localizer.Current = English;
        this.InitializeComponent();
        Refresh();
    }

    private void UseEnglish_Click(object sender, RoutedEventArgs e)
    {
        Localizer.Current = English;
        Refresh();
    }

    private void UseCzech_Click(object sender, RoutedEventArgs e)
    {
        Localizer.Current = Czech;
        Refresh();
    }

    private void Refresh()
    {
        AchievementsList.ItemsSource = Definitions
            .Select(d => new LocalizedAchievement(d.GetName(), d.GetDescription()))
            .ToArray();
    }

    private sealed record Achievement(string NameKey, string DescriptionKey) : ILocalizedDefinition;

    private sealed class DictionaryLocalizer(Dictionary<string, string> entries) : ILocalizer
    {
        public string GetString(string key) =>
            entries.TryGetValue(key, out var value) ? value : $"???{key}???";
    }
}

public sealed record LocalizedAchievement(string Name, string Description);
