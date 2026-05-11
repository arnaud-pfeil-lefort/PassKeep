using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PassKeep.modeles
{
    public class TypeProfilConnexion
    {
        [Key]
        public Guid Id { get; set; }

        public string Nom { get; set; }
    }
}