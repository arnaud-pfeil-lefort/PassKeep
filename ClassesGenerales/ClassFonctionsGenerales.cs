using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;



namespace PassKeep.ClassesGenerales
{
    public static class ClassFonctionsGenerales
    {
        private static readonly byte[] _key = Encoding.UTF8.GetBytes("PassKeepSecret!!");

        public static string hacherChaine(string chaine)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(chaine));
            return Convert.ToHexString(bytes).ToLower();
        }

        public static string encrypterChaine(string chaine)
        {
            using Aes aes = Aes.Create();
            aes.Key = _key;
            aes.GenerateIV();

            using MemoryStream ms = new();
            ms.Write(aes.IV, 0, aes.IV.Length);

            using (CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (StreamWriter sw = new(cs))
                sw.Write(chaine);

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string decrypterChaine(string chaineEncryptee)
        {
            byte[] data = Convert.FromBase64String(chaineEncryptee);

            using Aes aes = Aes.Create();
            aes.Key = _key;

            byte[] iv = new byte[aes.BlockSize / 8];
            Array.Copy(data, 0, iv, 0, iv.Length);
            aes.IV = iv;

            using MemoryStream ms = new(data, iv.Length, data.Length - iv.Length);
            using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using StreamReader sr = new(cs);
            return sr.ReadToEnd();
        }

        public static void CreerUtilisateurSuperAdmin(MigrationBuilder oLocal_migrationBuilder)
        {
            {
                Guid gLocal_GuidUtilisateur = Guid.NewGuid(); //Stocke un GUID généré pour créer le user "utilisateur"
                Guid gLocal_GuidTypeProfilConnexion = Guid.NewGuid(); //Stocke un GUID généré pour créer le TypeProfilconnexion "PasswordYnov"
                Guid gLocal_GuidProfilConnexion = Guid.NewGuid(); //Stocke un GUID généré pour créer le Profilconnexion utilisant les deux GUID ci-dessus

                string sLocal_MotPasseSuperAdmin = hacherChaine(ClassConstantesGenerales.sUtilisateur_Password_Superadmin);

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

