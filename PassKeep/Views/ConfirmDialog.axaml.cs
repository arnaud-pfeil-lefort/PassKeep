using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace PassKeep.Views;

public partial class ConfirmDialog : Window
{
    public ConfirmDialog(string title, string message)
    {
        InitializeComponent();
        TitleText.Text = title;
        MessageText.Text = message;
    }

    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
        => BeginMoveDrag(e);

    private void OnConfirm(object? sender, RoutedEventArgs e)
        => Close(true);

    private void OnCancel(object? sender, RoutedEventArgs e)
        => Close(false);
}
