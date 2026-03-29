using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PassKeep.modeles
{
    internal class DbDictionnaire
    {
        [Key]
        public Guid Id { get; set; }

        public string Mot { get; set; }

    }
}


