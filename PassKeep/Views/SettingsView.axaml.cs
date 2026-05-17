using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.IO;
using System.Linq;
using PassKeep.ClassesGenerales;
using PassKeep.modeles;

namespace PassKeep.Views;

public partial class SettingsView : Window
{
    public SettingsView()
    {
        InitializeComponent();
    }

    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
        => BeginMoveDrag(e);

    private void MinimizeWindow(object? sender, RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void CloseWindow(object? sender, RoutedEventArgs e)
        => Close();

    private async void OnImportDictionnaire(object? sender, RoutedEventArgs e)
    {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Importer un dictionnaire",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("Fichier texte") { Patterns = new[] { "*.txt" } }
            }
        });

        if (files.Count == 0) return;

        var file = files[0];
        await using var stream = await file.OpenReadAsync();
        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync();

        if (ClassFonctionsGenerales.AjouterMots(content)) {
            FeedbackText.Text = "Mots importés avec succès.";
            FeedbackText.Foreground = Avalonia.Media.Brushes.LightGreen;
            FeedbackText.IsVisible = true;
        } else { 
            FeedbackText.Text = "Erreur lors de l'importation du dictionnaire.";
            FeedbackText.Foreground = Avalonia.Media.Brushes.Red;
            FeedbackText.IsVisible = true;
        }
    }

    private void OnDeleteDictionnaire(object? sender, RoutedEventArgs e)
    {
        if (ClassFonctionsGenerales.SupprimerMots()) {
            FeedbackText.Text = "Dictionnaire vidé avec succès.";
            FeedbackText.Foreground = Avalonia.Media.Brushes.LightGreen;
            FeedbackText.IsVisible = true;
        } else {
            FeedbackText.Text = "Erreur lors de la suppression du dictionnaire.";
            FeedbackText.Foreground = Avalonia.Media.Brushes.Red;
            FeedbackText.IsVisible = true;
        }
    }

    private void OnGererTypes(object? sender, RoutedEventArgs e)
    {
        var typeProfilView = new TypeProfilView();
        typeProfilView.ShowDialog(this);
    }

    private void OnLogout(object? sender, RoutedEventArgs e)
    {
        ClassFonctionsGenerales.SupprimerSession();
        var app = (IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!;
        var loginView = new LoginView();
        app.MainWindow = loginView;
        loginView.Show();
        (Owner as Window)?.Close();
    }
}
