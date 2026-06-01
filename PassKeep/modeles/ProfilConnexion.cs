using PassKeep.ClassesGenerales;
using PassKeepDLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PassKeep.modeles
{
    public class ProfilConnexion
    {
        [Key]
        public Guid Id { get; set; }

        public string ServiceName { get; set; }

        public string? ServiceUrl { get; set; }
        public string? ServiceLogin { get; set; }
        public string ServiceCryptPassword { get; set; }

        public Guid PKUserId { get; set; }
        public virtual PKUser PKUser { get; set; }
        public Guid? TypeProfilConnexionId { get; set; }

        public virtual TypeProfilConnexion? TypeProfilConnexion { get; set; }


        [NotMapped]
        public string Initiale => string.IsNullOrEmpty(ServiceName) ? "?" : ServiceName[0].ToString().ToUpper();
        [NotMapped]
        public string MotDePasseClair => Cryptage.decrypterChaine(ServiceCryptPassword);
        [NotMapped]
        public string OwnerNom => PKUser?.Nom ?? "";
    }
}

