using System;
using System.Collections.Generic;
using Domain.Petisco;
using System.Linq;
using Domain.Avaliacao;

namespace Data.Modelo
{
    public class EspecialidadeBD
    {
        public int IdEspecialidade { get; set; }

        public double Preco { get; set; }

        public string Fotografia { get; set; }

        public bool Ativo { get; set; }

        public int IdEstabelecimento { get; set; }

        public EstabelecimentoBD Estabelecimento { get; set; }

        public int IdPetisco { get; set; }

        public PetiscoBD Petisco { get; set; }

        public List<ReviewBD> Avaliacoes { get; set; }

        public List<CaracteristicasBD> Caracteristicas { get; set; }

        public EspecialidadeBD()
        {
            
        }

        public EspecialidadeBD(Especialidade especialidade, EstabelecimentoBD estabelecimento)
        {
            Preco = especialidade.Preco;
            Fotografia = especialidade.Fotografia;
            Ativo = especialidade.Ativo;
            Estabelecimento = estabelecimento;
            Petisco = new PetiscoBD(especialidade);
            Caracteristicas = especialidade.Caracteristicas.Select(c => new CaracteristicasBD(c, this)).ToList();
        }


        public Especialidade ToEspecialidade() 
        {
            Especialidade especialidade = new Especialidade()
            {
                Id = Petisco.IdPetisco,
                IdEspecialidade = IdEspecialidade,
                Nome = Petisco.Nome,
                Ativo = Ativo,
                Fotografia = Fotografia,
                Preco = Preco,
                Caracteristicas = Caracteristicas?.Select(c => c.Caracteristica).ToList(),
                Avaliacoes = Avaliacoes?.Select(a => a.ToReview()).ToList(),
                Estabelecimento = Estabelecimento.ToEstabelecimentoSimples()
            };

            if (especialidade.Caracteristicas == null) especialidade.Caracteristicas = new List<string>();
            if (especialidade.Avaliacoes == null) especialidade.Avaliacoes = new List<Review>();

            return especialidade;
        }

		public Especialidade ToEspecialidadeSimples()
		{
            Especialidade especialidade = new Especialidade()
            {
                Id = Petisco.IdPetisco,
                IdEspecialidade = IdEspecialidade,
                Nome = Petisco.Nome,
                Ativo = Ativo,
                Fotografia = Fotografia,
                Preco = Preco,
                Caracteristicas = Caracteristicas?.Select(c => c.Caracteristica).ToList(),
                Estabelecimento = Estabelecimento.ToEstabelecimentoSimples()
			};

            if (especialidade.Caracteristicas == null) especialidade.Caracteristicas = new List<string>();
            especialidade.Avaliacoes = new List<Review>();

			return especialidade;
		}
    }
}
