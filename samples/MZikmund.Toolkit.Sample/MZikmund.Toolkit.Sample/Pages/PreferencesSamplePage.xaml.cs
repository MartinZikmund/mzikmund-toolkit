using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.Sample;

public sealed partial class PreferencesSamplePage : Page
{
    private readonly IPreferences _preferences;

    public PreferencesSamplePage()
    {
        this.InitializeComponent();
        _preferences = new Preferences();
    }

    private void SaveSimpleValue_Click(object sender, RoutedEventArgs e)
    {
        var value = SimpleValueInput.Text;
        _preferences.Set("SampleString", value);
        SimpleValueResult.Text = $"Saved: {value}";
    }

    private void LoadSimpleValue_Click(object sender, RoutedEventArgs e)
    {
        if (_preferences.TryGet<string>("SampleString", out var value))
        {
            SimpleValueInput.Text = value;
            SimpleValueResult.Text = $"Loaded: {value}";
        }
        else
        {
            SimpleValueResult.Text = "No value found";
        }
    }

    private void ClearSimpleValue_Click(object sender, RoutedEventArgs e)
    {
        _preferences.Set<string>("SampleString", null);
        SimpleValueInput.Text = string.Empty;
        SimpleValueResult.Text = "Value cleared";
    }

    private void SaveComplexValue_Click(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(PersonAgeInput.Text, out var age))
        {
            var person = new Person
            {
                Name = PersonNameInput.Text,
                Age = age
            };
            _preferences.SetComplex("SamplePerson", person);
            ComplexValueResult.Text = $"Saved: {person.Name}, Age {person.Age}";
        }
        else
        {
            ComplexValueResult.Text = "Invalid age";
        }
    }

    private void LoadComplexValue_Click(object sender, RoutedEventArgs e)
    {
        if (_preferences.TryGetComplex<Person>("SamplePerson", out var person))
        {
            PersonNameInput.Text = person.Name;
            PersonAgeInput.Text = person.Age.ToString();
            ComplexValueResult.Text = $"Loaded: {person.Name}, Age {person.Age}";
        }
        else
        {
            ComplexValueResult.Text = "No person found";
        }
    }

    private void ClearComplexValue_Click(object sender, RoutedEventArgs e)
    {
        _preferences.SetComplex<Person>("SamplePerson", null);
        PersonNameInput.Text = string.Empty;
        PersonAgeInput.Text = string.Empty;
        ComplexValueResult.Text = "Person cleared";
    }

    private class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}
