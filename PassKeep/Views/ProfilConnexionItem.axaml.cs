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

        private ProfilConnexion profil
        {
            get { return DataContext as ProfilConnexion; }
        }

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
                string texteAffiche;
                if (_motDePasseVisible)
                    texteAffiche = profil.MotDePasseClair;
                else
                    texteAffiche = new string('·', profil.MotDePasseClair.Length);

                PwdBox.Text = texteAffiche;
            }
        }

        private void OnModifier(object? sender, RoutedEventArgs e)
        {
            var lifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            if (lifetime == null) return;

            var mainWindow = lifetime.MainWindow as MainWindow;
            if (mainWindow == null) return;

            var addProfilView = new AddProfilView(mainWindow.user, profil);
            addProfilView.ShowDialog(mainWindow);
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