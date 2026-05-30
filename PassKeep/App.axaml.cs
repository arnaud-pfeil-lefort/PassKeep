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
using System.IO;
using PassKeep.ClassesGenerales;



namespace PassKeep
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static void Log(string message)
        {
            var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "passkeep.log");
            File.AppendAllText(logPath, $"[{DateTime.Now:HH:mm:ss}] {message}\n");
        }

        public override void OnFrameworkInitializationCompleted()
        {
            Batteries.Init();

            Task.Run(async () =>
            {
                try
                {
                    Log("=== BEGIN ===");
                    using DataContext oLocal_DataContext = new DataContext();

                    var connString = oLocal_DataContext.Database.GetConnectionString();
                    Log($"=== DB PATH : {connString}");

                    var pending = await oLocal_DataContext.Database.GetPendingMigrationsAsync();
                    Log($"=== MIGRATIONS EN ATTENTE : {string.Join(", ", pending)}");

                    await oLocal_DataContext.Database.MigrateAsync();
                    Log("=== MIGRATE OK ===");
                }
                catch (Exception ex)
                {
                    Log($"=== MIGRATE ERREUR : {ex.Message}");
                    Log(ex.StackTrace ?? "");
                }
            });

            ThemeService.ApplyTheme(ThemeService.LoadTheme());

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new LoginView();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}