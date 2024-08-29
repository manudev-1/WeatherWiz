using System.Diagnostics;

namespace WeatherWiz.Components;

public partial class ProgressBar : ContentView
{
	public ProgressBar()
	{
		InitializeComponent();
	}
    // Bindable Property
    public static readonly BindableProperty ProgressProperty = BindableProperty.Create(
        nameof(Progress),
        typeof(string),
        typeof(Widget),
        default(string)
    );
    // Property
    public string Progress
    {
        get => (string)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }
}