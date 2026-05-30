using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using System;
using System.IO;
using System.Linq;

namespace PassKeep.ClassesGenerales
{
    public static class ThemeService
    {
        private static string ThemeFilePath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "theme.dat");

        public static string CurrentTheme { get; private set; } = "Dark";

        public static string LoadTheme()
        {
            if (!File.Exists(ThemeFilePath)) return "Dark";
            var saved = File.ReadAllText(ThemeFilePath).Trim();
            return saved is "Dark" or "Light" ? saved : "Dark";
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

            var existing = merged
                .OfType<ResourceInclude>()
                .FirstOrDefault(r => r.Source?.ToString().Contains("Theme.axaml") == true);
            if (existing != null)
                merged.Remove(existing);

            var uri = new Uri($"avares://PassKeep/Styles/{theme}Theme.axaml");
            merged.Add(new ResourceInclude(uri) { Source = uri });
        }
    }
}
