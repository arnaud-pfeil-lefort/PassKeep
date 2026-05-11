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

namespace PassKeep.Views;

public partial class AddProfilView : Window
{
    private readonly PKUser _currentUser;
    private readonly ProfilConnexion? _profilToUpdate;

    public AddProfilView(PKUser user, ProfilConnexion? profilToUpdate = null)
    {
        InitializeComponent();
        _currentUser = user;
        _profilToUpdate = profilToUpdate;

        LoadTypeProfils();

        if (_profilToUpdate != null)
            PopulateForm();
    }

    private void PopulateForm()
    {
        FormTitle.Text = "Modifier le profil";
        FormSubtitle.Text = "Modifie les informations du compte";
        SubmitButton.Content = "Enregistrer les modifications";

        ServiceTextBox.Text = _profilToUpdate!.ServiceName;
        UrlTextBox.Text = _profilToUpdate.ServiceUrl;
        LoginTextBox.Text = _profilToUpdate.ServiceLogin;

        PasswordTextBox.Text = Cryptage.decrypterChaine(_profilToUpdate.ServiceCryptPassword);

        TypeProfilComboBox.SelectedItem = (TypeProfilComboBox.ItemsSource as System.Collections.Generic.List<TypeProfilConnexion>)
            ?.FirstOrDefault(t => t.Id == _profilToUpdate.TypeProfilConnexionId);
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

    private void LoadTypeProfils()
    {
        using DataContext db = new DataContext();
        var types = db.TypeProfilConnexion.ToList();
        TypeProfilComboBox.ItemsSource = types;
        TypeProfilComboBox.DisplayMemberBinding = new Avalonia.Data.Binding("Nom");
    }

    private void OnAddProfil(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ServiceTextBox.Text) ||
            string.IsNullOrWhiteSpace(LoginTextBox.Text) ||
            string.IsNullOrWhiteSpace(PasswordTextBox.Text))
        {
            ErrorText.IsVisible = true;
            return;
        }

        var typeProfil = TypeProfilComboBox.SelectedItem as TypeProfilConnexion;
        if (typeProfil == null)
        {
            ErrorText.Text = "Veuillez sélectionner un type de profil.";
            ErrorText.IsVisible = true;
            return;
        }

        using DataContext db = new DataContext();

        if (_profilToUpdate != null)
        {
            // Mode UPDATE
            var profil = db.ProfilConnexion.Find(_profilToUpdate.Id);
            if (profil == null) { Close(); return; }

            profil.ServiceName = ServiceTextBox.Text;
            profil.ServiceUrl = UrlTextBox.Text;
            profil.ServiceLogin = LoginTextBox.Text;
            profil.ServiceCryptPassword = Cryptage.encrypterChaine(PasswordTextBox.Text);
            profil.TypeProfilConnexionId = typeProfil.Id;
        }
        else
        {
            // Mode CREATE
            var profil = new ProfilConnexion
            {
                ServiceName = ServiceTextBox.Text,
                ServiceUrl = UrlTextBox.Text,
                ServiceLogin = LoginTextBox.Text,
                ServiceCryptPassword = Cryptage.encrypterChaine(PasswordTextBox.Text),
                PKUserId = _currentUser.Id,
                TypeProfilConnexionId = typeProfil.Id
            };
            db.ProfilConnexion.Add(profil);
        }

        db.SaveChanges();
        UpdateMainWindow();
        Close();
    }

    private void UpdateMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            if (desktop.MainWindow is MainWindow mainWindow)
                mainWindow.ChargerProfils();
    }
}