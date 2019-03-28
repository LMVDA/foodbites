using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Utilizador;
using Domain.Sugestao;
using Domain.Petisco;
using Domain.Geolocalizacao;
using Domain.DAOS.Interfaces;

namespace Domain.Pesquisa
{
    public class PesquisaFacade
    {
        private IEstabelecimentoDAO estabelecimentoDAO;
        private IEspecialidadeDAO especialidadeDAO;
		private IUtilizadorDAO utilizadorDAO;
        private MotorSugestoes motorSugestoes;
        private ParserPesquisa parserPesquisa;

        public PesquisaFacade(IEstabelecimentoDAO estabelecimentoDAO, IEspecialidadeDAO especialidadeDAO, IUtilizadorDAO utilizadorDAO)
        {
            this.estabelecimentoDAO = estabelecimentoDAO;
            this.especialidadeDAO = especialidadeDAO;
            this.utilizadorDAO = utilizadorDAO;
            parserPesquisa = new ParserPesquisa();
            motorSugestoes = new MotorSugestoes();
        }

        public List<Sugestao.Sugestao> Pesquisa(Foodbiter utilizador, string textoPesquisa, Localizacao localizacaoAtual, double? precoMin, double? precoMax, double distancia)
        {
			List<string> nomesPetiscos = especialidadeDAO.GetAllNomesPetiscos();
			Pesquisa pesquisa = parserPesquisa.ParsePesquisa(textoPesquisa, nomesPetiscos);
            List<Especialidade> especialidades = especialidadeDAO.GetAll(pesquisa.Petisco);

            var sugestoes = motorSugestoes.CalculaSugestoes(utilizador, pesquisa, localizacaoAtual, precoMin, precoMax, distancia, especialidades);

            if (sugestoes.Count > 0)
            {
                utilizadorDAO.AddUserPreferences(utilizador.Username, sugestoes[0].IdEspecialidade, pesquisa.Preferencias, pesquisa.Despreferencias);
            }

            return sugestoes;
        }

		public List<Sugestao.Sugestao> GetTendencias(Localizacao localizacaoAtual)
		{
            var tendencias = especialidadeDAO.GetAllEntreDatas(DateTime.Now.AddMonths(-1), DateTime.Now);

            return tendencias
                .Where(t => t.Estabelecimento.Localizacao.DistanciaA(localizacaoAtual) < 50000)
                .Select(t => new Sugestao.Sugestao(t))
                .ToList();
		}
    }
}
