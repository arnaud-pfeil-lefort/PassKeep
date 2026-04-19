using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using PassKeep.modeles;

namespace PassKeep
{
    public partial class MainWindow : Window
    {
        public MainWindow(PKUser user)
        {
            InitializeComponent();
            WelcomeText.Text = $"Bienvenue {user.Nom}";

        }

        private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            BeginMoveDrag(e);
        }

        private void MinimizeWindow(object? sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow(object? sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void CloseWindow(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}