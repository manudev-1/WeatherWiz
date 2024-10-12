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
        typeof(double),
        typeof(Widget),
        default(double)
    );
    // Property
    public double Progress
    {
        get => (double)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }
}