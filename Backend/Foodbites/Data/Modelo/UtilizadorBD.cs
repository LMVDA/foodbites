using System;
using System.Collections.Generic;
using Domain.Utilizador;
using Domain.Avaliacao;
using System.Linq;

namespace Data.Modelo
{
    public class UtilizadorBD
    {
        public string Username { get; set; }

        public string Nome { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public DateTime DataNascimento { get; set; }

		public List<ReviewBD> Avaliacoes { get; set; }

        public List<PreferenciaBD> Preferencias { get; set; }

        public UtilizadorBD()
        {
            
        }

        public UtilizadorBD(Foodbiter foodbiter)
        {
            Username = foodbiter.Username;
            Nome = foodbiter.Nome;
            Password = foodbiter.GetPasswordHashed();
            Email = foodbiter.Email;
            DataNascimento = foodbiter.DataNascimento;
            Avaliacoes = new List<ReviewBD>();
            Preferencias = new List<PreferenciaBD>();
        }

        public Foodbiter ToFoodbiter() 
        {
            Foodbiter foodbiter = new Foodbiter()
            {
                Username = Username,
                Nome = Nome,
				Password = Password,
                Email = Email,
                DataNascimento = DataNascimento,
                Avaliacoes = Avaliacoes?.Select(a => a.ToReview()).ToList(),
                Preferencias = Preferencias?.Select(p => p.ToPreferencia()).ToList(),
                Despreferencias = Preferencias?.Select(p => p.ToDespreferencia()).ToList()
            };

            if (foodbiter.Avaliacoes == null) foodbiter.Avaliacoes = new List<Review>();
			if (foodbiter.Preferencias == null) foodbiter.Preferencias = new List<Preferencia>();
			if (foodbiter.Despreferencias == null) foodbiter.Despreferencias = new List<Preferencia>();

            return foodbiter;
        }
	}
}
