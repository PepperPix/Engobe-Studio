using Avalonia.Controls;
using Engobe.Studio.ViewModels;

namespace Engobe.Studio.Views;

public partial class ShellWindow : Window
{
    private bool _forceClose;

    public ShellWindow()
    {
        InitializeComponent();
        Closing += OnClosing;
    }

    private async void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        if (_forceClose)
            return;
        if (DataContext is not ShellViewModel vm)
            return;

        e.Cancel = true;

        if (!await vm.ResolveUnsavedChangesAsync(allowCancel: true).ConfigureAwait(true))
            return;

        _forceClose = true;
        Close();
    }
}
