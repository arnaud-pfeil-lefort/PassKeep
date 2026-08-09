using DotNetEnv;
using System;
using System.IO;

namespace PassKeep.ClassesGenerales
{
    public enum NiveauVerificationUrl
    {
        Desactive,
        ALaDemande,
        Automatique
    }

    public static class VerificationSecuriteService
    {
        private static string SettingsFilePath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "verification.dat");

        public static bool CleApiDisponible()
        {
            var envPath = Path.Combine(AppContext.BaseDirectory, ".env");
            if (File.Exists(envPath))
                Env.Load(envPath);

            var apiKey = Environment.GetEnvironmentVariable("GOOGLE_SAFE_BROWSING_API_KEY");
            return !string.IsNullOrWhiteSpace(apiKey);
        }

        public static NiveauVerificationUrl LoadNiveau()
        {
            if (!CleApiDisponible())
                return NiveauVerificationUrl.Desactive;

            if (!File.Exists(SettingsFilePath))
                return NiveauVerificationUrl.ALaDemande;

            var saved = File.ReadAllText(SettingsFilePath).Trim();
            if (Enum.TryParse<NiveauVerificationUrl>(saved, out var niveau))
                return niveau;

            return NiveauVerificationUrl.ALaDemande;
        }

        public static void SaveNiveau(NiveauVerificationUrl niveau)
            => File.WriteAllText(SettingsFilePath, niveau.ToString());
    }
}
