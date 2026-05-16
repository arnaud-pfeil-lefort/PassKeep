using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using PassKeep.ClassesGenerales;
using PassKeep.modeles;
using PassKeep.Views;
using System;
using System.Diagnostics;
using System.Linq.Expressions;


namespace PassKeep.Views
{
    public partial class ProfilConnexionItem : UserControl
    {
        private bool _motDePasseVisible = false;
        private ProfilConnexion profil => DataContext as ProfilConnexion;

        public ProfilConnexionItem()
        {
            InitializeComponent();
            DataContextChanged += (_, _) =>
            {
                if (DataContext is ProfilConnexion profil)
                {
                    Debug.WriteLine("=== PROFIL OK ===" + profil.ServiceName);
                    PwdBox.Text = new string('·', profil.MotDePasseClair.Length);
                };
            };
                
        }


        private void OnToggleMotDePasse(object? sender, RoutedEventArgs e)
        {
            _motDePasseVisible = !_motDePasseVisible;
            if (DataContext is ProfilConnexion profil)
            {
                PwdBox.Text = _motDePasseVisible ? profil.MotDePasseClair : new string('·', profil.MotDePasseClair.Length);
            }
        }

        private void OnModifier(object? sender, RoutedEventArgs e)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                && desktop.MainWindow is MainWindow mainWindow)
            {
                var addProfilView = new AddProfilView(mainWindow.user, profil);
                addProfilView.ShowDialog(mainWindow);
            }
        }

        private async void OnCopier(object? sender, RoutedEventArgs e)
        {
            if (DataContext is not ProfilConnexion profil) return;

            var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
            if (clipboard != null)
                await clipboard.SetTextAsync(profil.MotDePasseClair);
        }
    }
}