using System;
using System.Collections.Generic;
using Domain.DAOS.Interfaces;
using Domain.Geolocalizacao;
using Domain.Petisco;

namespace Domain.Backoffice
{
    public class BackofficeFacade
    {
		private IEspecialidadeDAO especialidadeDAO;
		private IPetiscoDAO petiscoDAO;
		private IEstabelecimentoDAO estabelecimentoDAO;

        public BackofficeFacade(IEspecialidadeDAO especialidadeDAO, IPetiscoDAO petiscoDAO, IEstabelecimentoDAO estabelecimentoDAO)
        {
            this.especialidadeDAO = especialidadeDAO;
            this.petiscoDAO = petiscoDAO;
            this.estabelecimentoDAO = estabelecimentoDAO;
        }

        public void AdicionaEstabelecimento(string nome, string telefone, List<HorarioFuncionamento> horarios, Localizacao coordenadas, List<string> criticas) {

            Estabelecimento estabelecimento = new Estabelecimento()
            {
                Nome = nome,
                Telefone = telefone,
                Horarios = horarios,
                Localizacao = coordenadas,
                Criticas = criticas,
                Ativo = true
            };

            estabelecimentoDAO.Add(estabelecimento);
        } 

        public void AdicionaEspecialidade(int idPetisco, List<string> caracteristicas, double preco, int idEstabelecimento, string fotografia) {

            Especialidade especialidade = new Especialidade()
            {
                Id = idPetisco,
                Preco = preco,
                Fotografia = fotografia,
                Caracteristicas = caracteristicas,
                Ativo = true
            };

            especialidadeDAO.Add(idEstabelecimento, especialidade);
        }

        public void AdicionaPetisco(string nome) {
            var petisco = new Petisco.Petisco()
            {
                Nome = nome
            };

            petiscoDAO.Add(petisco);
        }

        public void AtualizaEstabelecimento(int id, String nome, String telefone, List<HorarioFuncionamento> horarios, Localizacao coordenadas, List<string> criticas) {
			Estabelecimento estabelecimento = new Estabelecimento()
			{
                Id = id,
				Nome = nome,
				Telefone = telefone,
				Horarios = horarios,
				Localizacao = coordenadas,
				Criticas = criticas
			};

            estabelecimentoDAO.Update(estabelecimento);
        }

        public void AtualizaEspecialidade(int id, int idPetisco, List<string> caracteristicas, double preco, int idEstabelecimento, string fotografia) {
            
            Especialidade especialidade = new Especialidade()
			{
                Id = idPetisco,
                IdEspecialidade = id,
				Preco = preco,
				Fotografia = fotografia,
				Caracteristicas = caracteristicas
			};

            especialidadeDAO.Update(especialidade);
        }

        public void AtualizaPetisco(int id, string nome)
        {
            Petisco.Petisco petisco = new Petisco.Petisco()
            {
                Id = id,
                Nome = nome
            };

            petiscoDAO.Update(petisco);
        }

        public void DesactivaEstabelecimento(int id) {
            estabelecimentoDAO.DesactivaEstabelecimento(id);

        }

        public void DesactivaEspecialidade(int id)
        {
            especialidadeDAO.DesactivaEspecialidade(id);
        }

        public List<Petisco.Petisco> GetAllPetiscos() {
            return petiscoDAO.GetAll();
        }
		
        public Petisco.Petisco GetPetisco(int id)
		{
            return petiscoDAO.Find(id);
		}

		public List<Estabelecimento> GetAllEstabelecimentos()
		{
			return estabelecimentoDAO.GetAll();
		}

		public Estabelecimento GetEstabelecimento(int id)
		{
            return estabelecimentoDAO.Find(id);
		}

		public List<Especialidade> GetAllEspecialidades()
		{
			return especialidadeDAO.GetAll();
		}

		public Especialidade GetEspecialidade(int id) {
            return especialidadeDAO.Find(id);
        }
    }
}
