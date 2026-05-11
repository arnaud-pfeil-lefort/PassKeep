using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using PassKeep.ClassesGenerales;
using PassKeep.modeles;
using PassKeepDLL;
using System.Linq;
using Tmds.DBus.Protocol;





namespace PassKeep.Views;

public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
        => BeginMoveDrag(e);

    private void MinimizeWindow(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void CloseWindow(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        => Close();


    private void OnLoginClicked(object? sender, RoutedEventArgs e)
    {
        using DataContext db = new DataContext();

        PKUser? user = db.PKUser.FirstOrDefault(
            u => u.Email == EmailTextBox.Text &&
            u.Password == Cryptage.hacherChaine(PasswordTextBox.Text ?? string.Empty)
        );

        if (user == null)
        {
            // Afficher un message d'erreur
            ErrorTextBlock.Text = "Email ou mot de passe incorrect.";
            ErrorTextBlock.IsVisible = true;
            return;
        }

        var app = (IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!;
        var mainWindow = new MainWindow(user);
        app.MainWindow = mainWindow;
        mainWindow.Show();

        Close();
    }


    private void OnRegisterClicked(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var app = (IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!;
        var mainWindow = new RegisterView();
        app.MainWindow = mainWindow;
        mainWindow.Show();
        Close();
    }
}