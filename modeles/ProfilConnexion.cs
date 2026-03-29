using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PassKeep.modeles
{
    public class ProfilConnexion
    {
        [Key]
        public Guid Id { get; set; }

        public string ServiceName { get; set; }

        public string ServiceUrl { get; set; }
        public string ServiceLogin { get; set; }
        public string ServiceCryptPassword { get; set; }

        public Guid PKUserId { get; set; }
        public virtual PKUser PKUser { get; set; }




    }
}




