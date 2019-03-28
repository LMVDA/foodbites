using System;
using System.Collections.Generic;
using System.Text;
using Domain.Avaliacao;

namespace Domain.Utilizador
{
    public class Foodbiter
    {
        public string Username { get; set; }
        public string Nome { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }

        public List<Preferencia> Preferencias { get; set; }
        public List<Preferencia> Despreferencias { get; set; }

        public List<Review> Avaliacoes { get; set; }

        public Foodbiter()
        {
        }

        public bool PasswordIgual(string userPassword)
        {
            var passwordHash = HashText(userPassword);

            return Password.Equals(passwordHash);
        }

        public string GetPasswordHashed()
        {
            return HashText(Password);
        }

        private static string HashText(string text) 
        {
			var data = new UTF8Encoding().GetBytes(text);
            var hashBytes = System.Security.Cryptography.MD5.Create().ComputeHash(data);
			String hash = Convert.ToBase64String(hashBytes);

            return hash;
        }
    }
}
