using Microsoft.EntityFrameworkCore.Migrations;
using PassKeep.modeles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
using PassKeepDLL;



namespace PassKeep.ClassesGenerales
{
    public static class ClassFonctionsGenerales
    {
        private static readonly byte[] _key = Encoding.UTF8.GetBytes("PassKeepSecret!!");


        public static void CreerFichierJournalErreur(string message)
        {
            try
            {
                string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "error.log");
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}";

                File.AppendAllText(logFilePath, logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Impossible d'écrire dans le fichier log : {ex.Message}");
            }
        }

        public static void GestionErreur(Exception? ex, string message = "", bool fermerApplication = false)
        {
            if (ex == null)
            {
                Console.WriteLine("Erreur inconnue.");
                CreerFichierJournalErreur("Erreur inconnue.");
                return;
            }

            string fullMessage = string.IsNullOrWhiteSpace(message) ? ex.Message : $"{message} | {ex.Message}";

            Console.WriteLine($"Erreur : {fullMessage}");
            CreerFichierJournalErreur(fullMessage);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("=== STACK TRACE ===");
                Console.WriteLine(ex.StackTrace);
            }

            if (fermerApplication)
            {
                Environment.Exit(1);
            }
        }



        public static Boolean AjouterMots(string content)
        {
            var mots = content
                .Split('\n')
                .Select(m => m.Trim())
                .Where(m => !string.IsNullOrEmpty(m))
                .Select(m => new DbDictionnaire { Mot = m })
                .ToList();
            try
            {
                using DataContext db = new DataContext();
                db.Dictionnaire.AddRange(mots);
                db.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout des mots : {ex.Message}");
                return false;

            }
        }

        public static Boolean SupprimerMots()
        {
            try
            {
                using DataContext db = new DataContext();
                db.Dictionnaire.RemoveRange(db.Dictionnaire);
                db.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression des mots : {ex.Message}");
                return false;
            }
        }

        public static List<string> GenererMotDePasse()
        {
            using DataContext db = new DataContext();

            var mots = db.Dictionnaire
                .Select(d => d.Mot)
                .ToList()
                .OrderBy(m => Guid.NewGuid())
                .Take(new Random().Next(3, 6))
                .ToList();
            return mots;
        }

        private static string SessionFilePath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "session.dat");

        public static void SauvegarderSession(Guid userId)
            => File.WriteAllText(SessionFilePath, userId.ToString());

        public static Guid? ChargerSession()
        {
            if (!File.Exists(SessionFilePath)) return null;
            var content = File.ReadAllText(SessionFilePath).Trim();
            return Guid.TryParse(content, out var id) ? id : null;
        }

        public static void SupprimerSession()
        {
            if (File.Exists(SessionFilePath))
                File.Delete(SessionFilePath);
        }

        public static void CreerUtilisateurSuperAdmin(MigrationBuilder migrationBuilder)
        {
            Guid guidAdmin        = Guid.NewGuid();
            Guid guidUser         = Guid.NewGuid();
            Guid guidTypeWeb      = Guid.NewGuid();
            Guid guidTypeDesktop  = Guid.NewGuid();
            Guid guidProfilGoogle = Guid.NewGuid();

            string mdpAdmin        = Cryptage.hacherChaine(ClassConstantesGenerales.sUtilisateur_Password_Superadmin);
            string mdpUser         = Cryptage.hacherChaine(ClassConstantesGenerales.sUtilisateur_Password_User);
            string mdpGoogleCrypte = Cryptage.encrypterChaine(ClassConstantesGenerales.sUtilisateur_Password_Superadmin);

            // Types de profil
            migrationBuilder.InsertData(
                table: "TypeProfilConnexion",
                columns: new[] { "Id", "Nom" },
                columnTypes: new[] { "TEXT", "TEXT" },
                values: new object[] { guidTypeWeb, ClassConstantesGenerales.sTypeProfilConnexion_Web }
            );
            migrationBuilder.InsertData(
                table: "TypeProfilConnexion",
                columns: new[] { "Id", "Nom" },
                columnTypes: new[] { "TEXT", "TEXT" },
                values: new object[] { guidTypeDesktop, ClassConstantesGenerales.sTypeProfilConnexion_Desktop }
            );

            // Utilisateur Admin
            migrationBuilder.InsertData(
                table: "PKUser",
                columns: new[] { "Id", "Login", "Nom", "Email", "Password", "Role" },
                columnTypes: new[] { "TEXT", "TEXT", "TEXT", "TEXT", "TEXT", "TEXT" },
                values: new object[] {
                    guidAdmin,
                    ClassConstantesGenerales.sUtilisateur_Login_Superadmin,
                    ClassConstantesGenerales.sUtilisateur_Nom_Superadmin,
                    ClassConstantesGenerales.sUtilisateur_Email_Superadmin,
                    mdpAdmin,
                    "Admin"
                }
            );

            // Utilisateur basique
            migrationBuilder.InsertData(
                table: "PKUser",
                columns: new[] { "Id", "Login", "Nom", "Email", "Password", "Role" },
                columnTypes: new[] { "TEXT", "TEXT", "TEXT", "TEXT", "TEXT", "TEXT" },
                values: new object[] {
                    guidUser,
                    ClassConstantesGenerales.sUtilisateur_Login_User,
                    ClassConstantesGenerales.sUtilisateur_Nom_User,
                    ClassConstantesGenerales.sUtilisateur_Email_User,
                    mdpUser,
                    "User"
                }
            );

            // ProfilConnexion Google pour l'admin
            migrationBuilder.InsertData(
                table: "ProfilConnexion",
                columns: new[] { "Id", "TypeProfilConnexionId", "PKUserId", "ServiceName", "ServiceUrl", "ServiceLogin", "ServiceCryptPassword" },
                columnTypes: new[] { "TEXT", "TEXT", "TEXT", "TEXT", "TEXT", "TEXT", "TEXT" },
                values: new object[] {
                    guidProfilGoogle,
                    guidTypeWeb,
                    guidAdmin,
                    "Google",
                    "https://google.com",
                    ClassConstantesGenerales.sUtilisateur_Email_Superadmin,
                    mdpGoogleCrypte
                }
            );
        }

    }

}

