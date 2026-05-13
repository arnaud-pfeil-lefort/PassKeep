using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using DotNetEnv;
using Microsoft.EntityFrameworkCore.Metadata;
using PassKeep.ClassesGenerales;
using PassKeep.modeles;
using System.IO;
using PassKeepDLL;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PassKeep.Views;



public partial class AddProfilView : Window
{

    private readonly PKUser _currentUser;
    private readonly ProfilConnexion? _profilToUpdate;
    private static readonly HttpClient _http = new HttpClient();


    public AddProfilView(PKUser user, ProfilConnexion? profilToUpdate = null)
    {
        InitializeComponent();
        _currentUser = user;
        _profilToUpdate = profilToUpdate;

        var envPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".env");
        Env.Load(Path.GetFullPath(envPath));
        LoadTypeProfils();

        if (_profilToUpdate != null)
            PopulateForm();
    }

    private void OnDeleteProfil(object? sender, RoutedEventArgs e)
    {
        if (_profilToUpdate == null) return;

        using DataContext db = new DataContext();
        var profil = db.ProfilConnexion.Find(_profilToUpdate.Id);
        if (profil != null)
        {
            db.ProfilConnexion.Remove(profil);
            db.SaveChanges();
        }

        UpdateMainWindow();
        Close();
    }

    private void PopulateForm()
    {
        FormTitle.Text = "Modifier le profil";
        FormSubtitle.Text = "Modifie les informations du compte";
        SubmitButton.Content = "Enregistrer les modifications";

        DeleteButton.IsVisible = true;

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


    private async void OnCheckUrl(object? sender, RoutedEventArgs e)
    {
        var url = UrlTextBox.Text?.Trim();
        Debug.WriteLine("OnCheckUrl appelé");


        if (string.IsNullOrEmpty(url))
        {
            UrlSafetyText.Text = "⚠ Entrez d'abord une URL.";
            UrlSafetyText.Foreground = Avalonia.Media.Brushes.Orange;
            UrlSafetyText.IsVisible = true;
            return;
        }

        UrlSafetyText.Text = "Vérification en cours...";
        UrlSafetyText.Foreground = Avalonia.Media.Brushes.Gray;
        UrlSafetyText.IsVisible = true;

        try
        {
            var isSafe = await CheckUrlSafety(url);

            if (isSafe)
            {
                UrlSafetyText.Text = "✔ URL sûre";
                UrlSafetyText.Foreground = Avalonia.Media.Brushes.LightGreen;
            }
            else
            {
                UrlSafetyText.Text = "✖ URL dangereuse détectée !";
                UrlSafetyText.Foreground = Avalonia.Media.Brushes.Red;
            }
        }
        catch (Exception ex)
{
    UrlSafetyText.Text = "⚠ Erreur : " + ex.Message;
    UrlSafetyText.Foreground = Avalonia.Media.Brushes.Orange;

    Debug.WriteLine(ex.ToString());
    Console.WriteLine(ex.ToString());
}

        UrlSafetyText.IsVisible = true;
    }

    private async Task<bool> CheckUrlSafety(string url)
    {
        var body = new
        {
            client = new
            {
                clientId = "password-manager-project",
                clientVersion = "1.0"
            },
            threatInfo = new
            {
                threatTypes = new[] { "MALWARE", "SOCIAL_ENGINEERING", "UNWANTED_SOFTWARE" },
                platformTypes = new[] { "ANY_PLATFORM" },
                threatEntryTypes = new[] { "URL" },
                threatEntries = new[] { new { url } }
            }
        };
        var apiKey = Environment.GetEnvironmentVariable("GOOGLE_SAFE_BROWSING_API_KEY");
        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Clé API Google Safe Browsing manquante.");
        }
        Debug.WriteLine("salut" + apiKey);
        var response = await _http.PostAsync(
            $"https://safebrowsing.googleapis.com/v4/threatMatches:find?key={apiKey}",
            content);

        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();

        // {} = aucune menace = safe
        using var doc = JsonDocument.Parse(responseJson);
        return !doc.RootElement.TryGetProperty("matches", out _);
    }
}