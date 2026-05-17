
using System;
using System.ComponentModel.DataAnnotations;

namespace PassKeep.modeles
{
    public class PKUser
    {
        [Key]
        public Guid Id { get; set; }

        public string Login { get; set; }
        public string Nom { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; } = "User";

        public bool IsAdmin => Role == "Admin";
    }
}
