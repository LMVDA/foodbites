using System;
using System.Collections.Generic;
using Domain.Petisco;
using Domain.Geolocalizacao;
using System.Linq;


namespace Data.Modelo
{
    public class EstabelecimentoBD
    {
		public int IdEstabelecimento { get; set; }
		
        public string Nome { get; set; }

        public string Telefone { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool Ativo { get; set; }

        public List<HorarioFuncionamentoBD> HorarioFuncionamento { get; set; }

        public List<EspecialidadeBD> Especialidades { get; set; }

        public List<CriticasBD> Criticas { get; set; }

        public EstabelecimentoBD()
        {
            
        }

        public EstabelecimentoBD(Estabelecimento estabelecimento)
        {
            IdEstabelecimento = estabelecimento.Id;
            Nome = estabelecimento.Nome;
            Telefone = estabelecimento.Telefone;
            Ativo = estabelecimento.Ativo;
            Latitude = estabelecimento.Localizacao.Latitude;
            Longitude = estabelecimento.Localizacao.Longitude;
            HorarioFuncionamento = estabelecimento.Horarios?.Select(h => new HorarioFuncionamentoBD(h)).ToList();
            Criticas = estabelecimento.Criticas?.Select(c => new CriticasBD(c)).ToList();
            Especialidades = estabelecimento.Especialidades?.Select(e => new EspecialidadeBD(e, this)).ToList();
        }

        public Estabelecimento ToEstabelecimento() 
        {
            Estabelecimento estabelecimento = new Estabelecimento()
            {
                Id = IdEstabelecimento,
                Nome = Nome,
                Telefone = Telefone,
                Ativo = Ativo,
                Criticas = Criticas?.Select(c => c.Critica).ToList(),
                Especialidades = Especialidades?.Select(e => e.ToEspecialidade()).ToList(),
                Localizacao = new Localizacao(Latitude, Longitude),
                Horarios = HorarioFuncionamento?.Select(h => h.ToHorarioFuncionamento()).ToList()
            };

            if (estabelecimento.Criticas == null) estabelecimento.Criticas = new List<string>();
            if (estabelecimento.Especialidades == null) estabelecimento.Especialidades = new List<Especialidade>();
            if (estabelecimento.Horarios == null) estabelecimento.Horarios = new List<HorarioFuncionamento>();

            return estabelecimento;
        }

		public Estabelecimento ToEstabelecimentoSimples()
		{
			Estabelecimento estabelecimento = new Estabelecimento()
			{
				Id = IdEstabelecimento,
				Nome = Nome,
				Telefone = Telefone,
				Ativo = Ativo,
				Criticas = Criticas?.Select(c => c.Critica).ToList(),
                Especialidades = new List<Especialidade>(),
				Localizacao = new Localizacao(Latitude, Longitude),
				Horarios = HorarioFuncionamento?.Select(h => h.ToHorarioFuncionamento()).ToList()
			};

			if (estabelecimento.Criticas == null) estabelecimento.Criticas = new List<string>();
			if (estabelecimento.Horarios == null) estabelecimento.Horarios = new List<HorarioFuncionamento>();

			return estabelecimento;
		}
    }
}
