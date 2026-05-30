using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using PassKeep.ClassesGenerales;
using PassKeep.modeles;
using PassKeep.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PassKeep
{
    public partial class MainWindow : Window
    {
        public PKUser user;
        private List<ProfilConnexion> _tousLesProfils = new();

        public MainWindow(PKUser user)
        {
            InitializeComponent();
            this.user = user;
            ChargerTypes();
            ChargerProfils();
        }

        private void ChargerTypes()
        {
            try
            {
                using DataContext db = new DataContext();
                var types = db.TypeProfilConnexion.ToList();
                FilterTypeBox.ItemsSource = types;
                FilterTypeBox.DisplayMemberBinding = new Avalonia.Data.Binding("Nom");
            }
            catch (Exception ex)
            {
                ClassFonctionsGenerales.GestionErreur(ex, "Erreur lors du chargement des types de profil");
            }
        }

        public void ChargerProfils()
        {
            try
            {
                using DataContext db = new DataContext();

                if (user.IsAdmin)
                    _tousLesProfils = db.ProfilConnexion
                        .Include(p => p.PKUser)
                        .ToList();
                else
                    _tousLesProfils = db.ProfilConnexion
                        .Where(p => p.PKUserId == user.Id)
                        .ToList();

                AppliquerFiltres();
            }
            catch (Exception ex)
            {
                ClassFonctionsGenerales.GestionErreur(ex, "Erreur lors du chargement des profils");
            }
        }

        private void AppliquerFiltres()
        {
            var recherche = SearchBox?.Text?.Trim().ToLower() ?? string.Empty;
            var typeSelectionne = FilterTypeBox?.SelectedItem as TypeProfilConnexion;

            var resultats = _tousLesProfils.AsEnumerable();

            if (!string.IsNullOrEmpty(recherche))
                resultats = resultats.Where(p =>
                    p.ServiceName.ToLower().Contains(recherche));

            if (typeSelectionne != null)
                resultats = resultats.Where(p =>
                    p.TypeProfilConnexionId == typeSelectionne.Id);
            ProfilsList.ItemsSource = null;
            ProfilsList.ItemsSource = resultats.ToList();
        }

        private void OnSearchChanged(object? sender, TextChangedEventArgs e)
            => AppliquerFiltres();

        private void OnFilterChanged(object? sender, SelectionChangedEventArgs e)
            => AppliquerFiltres();

        public void ResetFiltre()
        {
            FilterTypeBox.SelectedItem = null;
            AppliquerFiltres();
        }

        private void OnResetFiltre(object? sender, RoutedEventArgs e)
        {
            FilterTypeBox.SelectedItem = null;
            SearchBox.Text = string.Empty;
            AppliquerFiltres();
        }

        private void OnHelpClicked(object? sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "AideEnLigne.html"),
                UseShellExecute = true
            });
        }

        private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
            => BeginMoveDrag(e);

        private void MinimizeWindow(object? sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        private void MaximizeWindow(object? sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        private void CloseWindow(object? sender, RoutedEventArgs e)
            => Close();

        private void OnAddProfilViewClicked(object? sender, RoutedEventArgs e)
        {
            var addProfilView = new AddProfilView(user);
            addProfilView.ShowDialog(this);
        }

        private async void OnSettingsClicked(object? sender, RoutedEventArgs e)
        {
            var settingsView = new SettingsView();
            await settingsView.ShowDialog(this);
            ChargerTypes();
            ChargerProfils();
        }
    }
}