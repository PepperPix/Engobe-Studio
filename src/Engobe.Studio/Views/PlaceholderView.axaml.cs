using Avalonia;
using Avalonia.Controls;

namespace Engobe.Studio.Views;

public partial class PlaceholderView : UserControl
{
    public static readonly StyledProperty<string?> HeaderProperty =
        AvaloniaProperty.Register<PlaceholderView, string?>(nameof(Header));

    public string? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public PlaceholderView()
    {
        InitializeComponent();
    }
}
