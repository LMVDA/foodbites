using System;
using Domain.DAOS.Interfaces;
using Domain.Utilizador;

namespace Domain.Avaliacao
{
    public class AvaliacaoFacade
    {
        private IAvaliacaoDAO avaliacaoDAO;

        public AvaliacaoFacade(IAvaliacaoDAO avaliacaoDAO)
        {
            this.avaliacaoDAO = avaliacaoDAO;
        }

        public void Avaliar(string utilizador, int idEspecialidade, int avaliacao)
        {
            Review review = new Review();
            review.NrEstrelas = avaliacao;
            review.Data = DateTime.Now;

            avaliacaoDAO.Add(utilizador, idEspecialidade, review);
        }
    }
}
