using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using PassKeep.ClassesGenerales;
using PassKeep.modeles;
using PassKeep.Views;
using System;
using System.Diagnostics;
using System.Linq;
using System.Diagnostics;
using System.IO;


namespace PassKeep
{
    public partial class MainWindow : Window
    {
        public PKUser user;

        public MainWindow(PKUser user)
        {
            InitializeComponent();
            this.user = user;
            WelcomeText.Text = $"Bienvenue {user.Nom}";
            ChargerProfils();
        }

        public void ChargerProfils()
        {
            using DataContext db = new DataContext();
            var profils = db.ProfilConnexion
                .Where(p => p.PKUserId == user.Id)
                .ToList();

            ProfilsList.ItemsSource = profils;
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
        {
            BeginMoveDrag(e);
        }

        private void MinimizeWindow(object? sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow(object? sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void CloseWindow(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnAddProfilViewClicked(object? sender, RoutedEventArgs e)
        {
            using DataContext db = new DataContext();


            var addProfilView = new AddProfilView(user);
            addProfilView.ShowDialog(this);

        }
        private void OnSettingsClicked(object? sender, RoutedEventArgs e)
        {
            using DataContext db = new DataContext();
            var settingsView = new SettingsView();
            settingsView.ShowDialog(this);

        }


        
    }
}