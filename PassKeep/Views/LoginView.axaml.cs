using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using PassKeep.ClassesGenerales;
using PassKeep.modeles;
using PassKeepDLL;
using System;
using System.Linq;
using Tmds.DBus.Protocol;

namespace PassKeep.Views;

public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
        Opened += OnOpened;
    }

    private void OnOpened(object? sender, EventArgs e)
    {
        try
        {
            var userId = ClassFonctionsGenerales.ChargerSession();
            if (userId == null) return;

            using DataContext db = new DataContext();
            var user = db.PKUser.Find(userId.Value);
            if (user == null) { ClassFonctionsGenerales.SupprimerSession(); return; }

            OuvrirFenetrePrincipale(user);
        }
        catch { /* DB pas encore prête, formulaire affiché normalement */ }
    }

    private void OnHelpClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var helpView = new HelpView(HelpPage.Login);
        helpView.ShowDialog(this);
    }

    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
        => BeginMoveDrag(e);

    private void MinimizeWindow(object? sender, RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void CloseWindow(object? sender, RoutedEventArgs e)
        => Close();

    private void OnLoginClicked(object? sender, RoutedEventArgs e)
    {
        using DataContext db = new DataContext();

        string email = EmailTextBox.Text ?? string.Empty;
        string motDePasseSaisi = PasswordTextBox.Text ?? string.Empty;
        string motDePasseHache = Cryptage.hacherChaine(motDePasseSaisi);

        PKUser? user = db.PKUser.FirstOrDefault(
            u => u.Email == email && u.Password == motDePasseHache
        );

        if (user == null)
        {
            ErrorTextBlock.Text = "Email ou mot de passe incorrect.";
            ErrorTextBlock.IsVisible = true;
            return;
        }

        if (RememberMeCheckBox.IsChecked == true)
            ClassFonctionsGenerales.SauvegarderSession(user.Id);

        OuvrirFenetrePrincipale(user);
    }

    private void OuvrirFenetrePrincipale(PKUser user)
    {
        var app = (IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!;
        var mainWindow = new MainWindow(user);
        app.MainWindow = mainWindow;
        mainWindow.Show();
        Close();
    }

    private void OnRegisterClicked(object? sender, PointerPressedEventArgs e)
    {
        var app = (IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!;
        var registerView = new RegisterView();
        app.MainWindow = registerView;
        registerView.Show();
        Close();
    }
}
