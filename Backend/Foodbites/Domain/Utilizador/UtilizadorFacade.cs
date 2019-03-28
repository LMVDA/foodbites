using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Avaliacao;
using Domain.DAOS.Interfaces;
using Domain.Sugestao;

namespace Domain.Utilizador
{
    public class UtilizadorFacade
    {
        private IUtilizadorDAO utilizadorDAO;

        public UtilizadorFacade(IUtilizadorDAO utilizadorDAO)
        {
            this.utilizadorDAO = utilizadorDAO;
        }

        public bool AutenticarUtilizador(string username, string password)
        {
            var utilizador = utilizadorDAO.Find(username);

            if (utilizador == null) return false;

            return utilizador.PasswordIgual(password);
        }

        public void AtualizarPerfil(string username, string nome, string email, DateTime? dataNascimento, string password)
		{
            utilizadorDAO.Update(username, nome, email, dataNascimento, password);
		}

        public void Registar(string username, Foodbiter foodbiter)
		{
            utilizadorDAO.Add(foodbiter);
		}

        public Foodbiter GetUtilizador(string username)
        {
            return utilizadorDAO.Find(username);
        }

        public List<AvaliacaoDetalhada> GetAvaliacoes(string username)
        {
            var utilizador = utilizadorDAO.Find(username);

            if (utilizador == null) return new List<AvaliacaoDetalhada>();

            return utilizador.Avaliacoes.Select(a => new AvaliacaoDetalhada(a)).ToList();
		}

        public List<HistoricoPesquisa> GetHistorico(string username)
		{
            return utilizadorDAO.GetAllSelecoesDegustacao(username).Select(s => new HistoricoPesquisa(s.Item1, s.Item2)).ToList();
		}

        public void RegistaHistorico(string username, int idEspecialidade)
        {
            utilizadorDAO.AddSelecaoDegustacao(username, idEspecialidade);
        }
    }
}
