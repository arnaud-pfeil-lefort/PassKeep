using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using PassKeep.ClassesGenerales;
using PassKeep.modeles;
using System;
using System.Linq;
using Avalonia;



namespace PassKeep.Views;

public partial class AddProfilView : Window
{
    private readonly PKUser _currentUser;

    public AddProfilView(PKUser user)
    {
        InitializeComponent();
        _currentUser = user;
    }

    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
        => BeginMoveDrag(e);

    private void MinimizeWindow(object? sender, RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void CloseWindow(object? sender, RoutedEventArgs e)
        => Close();

    private void OnGeneratePassword(object? sender, RoutedEventArgs e)
    {
        var mots = ClassFonctionsGenerales.GenererMotDePasse();
        PasswordTextBox.Text = string.Join("-", mots);
        PasswordTextBox.PasswordChar = '\0';
    }

    private void OnAddProfil(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ServiceTextBox.Text) ||
            string.IsNullOrWhiteSpace(LoginTextBox.Text)   ||
            string.IsNullOrWhiteSpace(PasswordTextBox.Text))
        {
            ErrorText.IsVisible = true;
            return;
        }

        using DataContext db = new DataContext();
        var typeDefaut = db.TypeProfilConnexion.First();


        var profil = new ProfilConnexion
        {
            ServiceName  = ServiceTextBox.Text,
            ServiceUrl   = UrlTextBox.Text,
            ServiceLogin = LoginTextBox.Text,
            ServiceCryptPassword = ClassFonctionsGenerales.encrypterChaine(PasswordTextBox.Text),
            PKUserId   = _currentUser.Id,
            TypeProfilConnexionId = typeDefaut.Id
        };

        db.ProfilConnexion.Add(profil);
        db.SaveChanges();
        UpdateMainWindow();

        Close();
    }

    private void UpdateMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ChargerProfils();
            }
        }
    }
}
