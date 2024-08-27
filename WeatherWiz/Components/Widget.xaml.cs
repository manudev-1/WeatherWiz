namespace WeatherWiz.Components;

public partial class Widget : ContentView
{
	public Widget()
	{
		InitializeComponent();
        BindingContext = this;
    }
    // Bindable Property
    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
        nameof(ImageSource),
        typeof(string),
        typeof(Widget),
        default(string)
    );
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(Widget),
        default(string)
    );
    public static readonly BindableProperty XAMLChildrenProperty = BindableProperty.Create(
        nameof(XAMLChildren),
        typeof(View),
        typeof(Widget),
        default(View));

    // Property
    public string ImageSource
    {
        get => (string)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public View XAMLChildren
    {
        get => (View)GetValue(XAMLChildrenProperty);
        set => SetValue(XAMLChildrenProperty, value);
    }
}