using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Linq;
using PassKeep.modeles;
using PassKeep.ClassesGenerales;


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

        var profil = new ProfilConnexion
        {
            ServiceName  = ServiceTextBox.Text,
            ServiceUrl   = UrlTextBox.Text,
            ServiceLogin = LoginTextBox.Text,
            ServiceCryptPassword = PasswordTextBox.Text,
            PKUserId   = _currentUser.Id
        };

        db.ProfilConnexion.Add(profil);
        db.SaveChanges();

        Close();
    }
}
