using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using System;
using System.IO;
using System.Linq;

namespace PassKeep.ClassesGenerales
{
    public static class ThemeService
    {
        private static string ThemeFilePath
        {
            get
            {
                string dossierBase = AppDomain.CurrentDomain.BaseDirectory;
                return Path.Combine(dossierBase, "theme.dat");
            }
        }

        public static string CurrentTheme { get; private set; } = "Dark";

        public static string LoadTheme()
        {
            if (!File.Exists(ThemeFilePath)) return "Dark";

            var saved = File.ReadAllText(ThemeFilePath).Trim();

            bool themeValide = saved == "Dark" || saved == "Light";
            if (themeValide)
                return saved;

            return "Dark";
        }

        public static void SaveTheme(string theme)
        {
            CurrentTheme = theme;
            File.WriteAllText(ThemeFilePath, theme);
        }

        public static void ApplyTheme(string theme)
        {
            CurrentTheme = theme;
            var app = Application.Current!;
            var merged = app.Resources.MergedDictionaries;

            var ressourcesDeType = merged.OfType<ResourceInclude>();
            ResourceInclude? existing = null;
            foreach (var ressource in ressourcesDeType)
            {
                string? sourceString = ressource.Source?.ToString();
                bool estUnFichierTheme = sourceString != null && sourceString.Contains("Theme.axaml");
                if (estUnFichierTheme)
                {
                    existing = ressource;
                    break;
                }
            }

            if (existing != null)
                merged.Remove(existing);

            var uri = new Uri($"avares://PassKeep/Styles/{theme}Theme.axaml");
            merged.Add(new ResourceInclude(uri) { Source = uri });
        }
    }
}
