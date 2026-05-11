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

        public static void CreerUtilisateurSuperAdmin(MigrationBuilder oLocal_migrationBuilder)
        {
            {
                Guid gLocal_GuidUtilisateur = Guid.NewGuid(); //Stocke un GUID généré pour créer le user "utilisateur"
                Guid gLocal_GuidTypeProfilConnexion = Guid.NewGuid(); //Stocke un GUID généré pour créer le TypeProfilconnexion "PasswordYnov"
                Guid gLocal_GuidProfilConnexion = Guid.NewGuid(); //Stocke un GUID généré pour créer le Profilconnexion utilisant les deux GUID ci-dessus

                string sLocal_MotPasseSuperAdmin = Cryptage.hacherChaine(ClassConstantesGenerales.sUtilisateur_Password_Superadmin);

                // On ajoute automatiquement l'utilisateur "SuperAdmin"
                oLocal_migrationBuilder.InsertData(
                    table: "PKUser",
                    columns: new[] { "Id", "Login", "Nom", "Email", "Password" },
                    columnTypes: new[] { "TEXT", "TEXT", "TEXT", "TEXT", "TEXT" },
                    values: new object[] {
                        gLocal_GuidUtilisateur,
                        ClassConstantesGenerales.sUtilisateur_Login_Superadmin,
                        ClassConstantesGenerales.sUtilisateur_Nom_Superadmin,
                        "admin@email.com",
                        sLocal_MotPasseSuperAdmin
                    }
                );

                oLocal_migrationBuilder.InsertData(
                    table: "TypeProfilConnexion",
                    columns: new[] { "Id", "Nom" },
                    columnTypes: new[] { "TEXT", "TEXT" },
                    values: new object[] {
                        gLocal_GuidTypeProfilConnexion,
                        ClassConstantesGenerales.sTypeProfilConnexion_Nom_TypeUtilisateur
                    }
                );
                // Créer le profilconnexion "superadmin" avec deux clés étrangères.

                oLocal_migrationBuilder.InsertData(
                    table: "ProfilConnexion",
                    columns: new[] { "Id", "TypeProfilConnexionId", "PKUserId",
                          "ServiceName", "ServiceUrl", "ServiceLogin", "ServiceCryptPassword" },
                    columnTypes: new[] { "TEXT", "TEXT", "TEXT", "TEXT", "TEXT", "TEXT", "TEXT" },
                    values: new object[] { gLocal_GuidProfilConnexion,
                    gLocal_GuidTypeProfilConnexion,
                    gLocal_GuidUtilisateur,
                    ClassConstantesGenerales.sUtilisateur_Nom_Superadmin,
                    "",
                    ClassConstantesGenerales.sUtilisateur_Nom_Superadmin,
                    sLocal_MotPasseSuperAdmin });
            }
        }

    }

}

