using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using PassKeep.modeles;
using System;
using System.ComponentModel;
using System.Linq;

namespace PassKeep.Views;

public partial class TypeProfilView : Window
{
    public TypeProfilView()
    {
        InitializeComponent();
        ChargerTypes();
    }

    private void OnHelpClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var helpView = new HelpView(HelpPage.TypeProfil);
        helpView.ShowDialog(this);
    }

    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
        => BeginMoveDrag(e);

    private void MinimizeWindow(object? sender, RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void CloseWindow(object? sender, RoutedEventArgs e)
        => Close();

    private void ChargerTypes()
    {
        try
        {
            using DataContext db = new DataContext();
            TypesList.ItemsSource = db.TypeProfilConnexion.ToList();
        }
        catch (Exception ex)
        {
            PassKeep.ClassesGenerales.ClassFonctionsGenerales.GestionErreur(ex, "Erreur lors du chargement des types");
            ShowFeedback("Impossible de charger les types.", isError: true);
        }
    }

    private void OnAjouter(object? sender, RoutedEventArgs e)
    {
        var nom = NomTextBox.Text?.Trim();
        if (string.IsNullOrEmpty(nom))
        {
            ShowFeedback("Le nom ne peut pas être vide.", isError: true);
            return;
        }

        try
        {
            using DataContext db = new DataContext();

            if (db.TypeProfilConnexion.Any(t => t.Nom == nom))
            {
                ShowFeedback("Ce type existe déjà.", isError: true);
                return;
            }

            db.TypeProfilConnexion.Add(new TypeProfilConnexion
            {
                Id = Guid.NewGuid(),
                Nom = nom
            });
            db.SaveChanges();

            NomTextBox.Text = string.Empty;
            ShowFeedback("Type ajouté avec succès.", isError: false);
            ChargerTypes();
        }
        catch (Exception ex)
        {
            PassKeep.ClassesGenerales.ClassFonctionsGenerales.GestionErreur(ex, "Erreur lors de l'ajout du type");
            ShowFeedback("Impossible d'ajouter le type. Veuillez réessayer.", isError: true);
        }
    }

    private void OnSupprimer(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is TypeProfilConnexion type)
        {
            try
            {
                using DataContext db = new DataContext();
                var entity = db.TypeProfilConnexion.Find(type.Id);
                if (entity != null)
                {
                    db.TypeProfilConnexion.Remove(entity);
                    db.SaveChanges();
                }
                ShowFeedback("Type supprimé.", isError: false);
                ChargerTypes();
            }
            catch (Exception ex)
            {
                PassKeep.ClassesGenerales.ClassFonctionsGenerales.GestionErreur(ex, "Erreur lors de la suppression du type");
                ShowFeedback("Impossible de supprimer le type. Veuillez réessayer.", isError: true);
            }
        }
    }

    private void ShowFeedback(string message, bool isError)
    {
        FeedbackText.Text = message;
        FeedbackText.Foreground = isError
            ? Avalonia.Media.Brushes.Red
            : Avalonia.Media.Brushes.LightGreen;
        FeedbackText.IsVisible = true;
    }
}