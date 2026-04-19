using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using PassKeep.modeles;
using SQLitePCL;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using PassKeep.Views;


namespace PassKeep
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                //desktop.MainWindow = new MainWindow();
                desktop.MainWindow = new LoginView();
            }

            Batteries.Init();

            base.OnFrameworkInitializationCompleted();

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                Debug.WriteLine($"=== UNOBSERVED : {e.Exception.Message}");
                Debug.WriteLine(e.Exception.StackTrace);
                e.SetObserved();
            };

            Task.Run(async () =>
            {
                try
                {
                    Debug.WriteLine("=== BEGIN ===");

                    using DataContext oLocal_DataContext = new DataContext();
                    await oLocal_DataContext.Database.MigrateAsync();
                    Debug.WriteLine("=== MIGRATE OK ===");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"=== MIGRATE ERREUR : {ex.Message}");
                    Debug.WriteLine(ex.StackTrace);
                }
            });


        }
    }
}