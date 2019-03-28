using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Petisco;

namespace Domain.Sugestao
{
    public class HistoricoPesquisa
    {
		public int IdEspecialidade { get; set; }
		public string NomeEstabelecimento { get; set; }
        public string NomePetisco { get; set; }
		public int NrEstrelas { get; set; }
		public double Preco { get; set; }
		public string Fotografia { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public string Telefone { get; set; }
		public List<string> Caracteristicas { get; set; }
        public List<string> Criticas { get; set; }
        public DateTime Data { get; set; }

		public HistoricoPesquisa()
		{
		}

        public HistoricoPesquisa(Especialidade especialidade, DateTime data)
		{
			IdEspecialidade = especialidade.IdEspecialidade;
			NrEstrelas = especialidade.Avaliacao;
			Preco = especialidade.Preco;
			Fotografia = especialidade.Fotografia;
			Latitude = especialidade.Estabelecimento.Localizacao.Latitude;
			Longitude = especialidade.Estabelecimento.Localizacao.Longitude;
			Telefone = especialidade.Estabelecimento.Telefone;
			NomePetisco = especialidade.Nome;
			Caracteristicas = especialidade.Caracteristicas?.Select(c => c).ToList();
            Criticas = especialidade.Estabelecimento.Criticas?.Select(c => c).ToList();
            Data = data;
		}
    }
}
