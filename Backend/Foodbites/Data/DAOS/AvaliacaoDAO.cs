namespace Data.DAOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Modelo;
    using Data.DAOS.Context;
    using Domain.Avaliacao;
    using Domain.DAOS.Interfaces;

    public class AvaliacaoDAO : IAvaliacaoDAO
	{
		private readonly FoodbitesContext contextoBD;

		public AvaliacaoDAO(FoodbitesContext context)
		{
			contextoBD = context;
		}

		public List<Review> GetAll()
		{
            return contextoBD.Avaliacoes.Select(a => a.ToReview()).ToList();
		}

		public Review Find(int id)
		{
            return contextoBD.Avaliacoes.FirstOrDefault(a => a.IdReview == id)?.ToReview();
		}

		public void Remove(int id)
		{
			var entity = contextoBD.Avaliacoes.First(e => e.IdReview == id);
			contextoBD.Avaliacoes.Remove(entity);
			contextoBD.SaveChanges();
		}

        public void Add(string username, int idEspecialidade, Review avaliacao)
        {
            var utilizador = contextoBD.Utilizadores
                                       .First(u => u.Username.Equals(username));

            var especialidade = contextoBD.Especialidades
                                          .First(e => e.IdEspecialidade == idEspecialidade);

            var avaliacaoBD = new ReviewBD
            {
                Estrelas = avaliacao.NrEstrelas,
                Utilizador = utilizador,
                Especialidade = especialidade,
                Data = avaliacao.Data
            };

            contextoBD.Avaliacoes.Add(avaliacaoBD);
            contextoBD.SaveChanges();
        }
    }
}
