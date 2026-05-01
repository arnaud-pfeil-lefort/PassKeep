using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using PassKeep.modeles;
using PassKeep.Views;
using System;
using PassKeep.ClassesGenerales;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;


namespace PassKeep.Views
{
    public partial class ProfilConnexionItem : UserControl
    {
        private bool _motDePasseVisible = false;

        public ProfilConnexionItem()
        {
            InitializeComponent();
            DataContextChanged += (_, _) =>
            {
                Debug.WriteLine("=== PROFIL PAS OK ===");
                if (DataContext is ProfilConnexion profil)
                {
                    Debug.WriteLine("=== PROFIL OK ===" + profil.ServiceName);
                    PwdBox.Text = new string('●', profil.MotDePasseClair.Length);
                    Debug.WriteLine(PwdBox.GetValue);

                }
                ;
            };
                
        }


        private void OnToggleMotDePasse(object? sender, RoutedEventArgs e)
        {
            _motDePasseVisible = !_motDePasseVisible;
            if (DataContext is ProfilConnexion profil)
            {
                PwdBox.Text = _motDePasseVisible ? profil.MotDePasseClair : new string('●', profil.MotDePasseClair.Length);
            }
        }

        private void OnModifier(object? sender, RoutedEventArgs e)
        {
            if (DataContext is ProfilConnexion profil)
            {
                try {
                    var i = 0;
                    var m = i / 0; // pour tester la gestion d'erreur
                } catch (Exception ex) {

                    ClassFonctionsGenerales.GestionErreur(ex);
                }
            }
        }

        private void OnSupprimer(object? sender, RoutedEventArgs e)
        {
            if (DataContext is ProfilConnexion profil)
            {
                using DataContext db = new DataContext();
                db.ProfilConnexion.Remove(db.ProfilConnexion.Find(profil.Id)!);
                db.SaveChanges();

                // rafraîchir la liste dans MainWindow
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                    && desktop.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ChargerProfils();
                }
            }
        }
    }
}