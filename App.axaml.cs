using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using PassKeep.modeles;
using SQLitePCL;
using System;


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
                desktop.MainWindow = new MainWindow();
            }

            Batteries.Init();

            base.OnFrameworkInitializationCompleted();

            using (DataContext oLocal_DataContext = new DataContext())
                try
                {
                    oLocal_DataContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    {
                        Console.WriteLine(ex);
                        throw;
                    }
                }
            /*
                        DataContext oLocal_DataContext = new DataContext();

                        oLocal_DataContext.Database.EnsureCreated();*/
        }
    }
}