namespace Data.DAOS
{
    using System.Collections.Generic;
    using System.Linq;
    using Data.Modelo;
    using Data.DAOS.Context;
	using Domain.DAOS.Interfaces;
    using Domain.Petisco;
    using Microsoft.EntityFrameworkCore;

    public class EstabelecimentoDAO : IEstabelecimentoDAO
	{
		private readonly FoodbitesContext contextoBD;

		public EstabelecimentoDAO(FoodbitesContext context)
		{
			contextoBD = context;
		}

		public List<Estabelecimento> GetAll()
		{
			return contextoBD.Estabelecimentos
							 .Include(e => e.Criticas)
                             .Include(e => e.Especialidades)
				             .ThenInclude(e => e.Petisco)
							 .Include(e => e.HorarioFuncionamento)
                             .Select(e => e.ToEstabelecimento()).ToList();
		}

        public void Add(Estabelecimento estabelecimento)
		{
            contextoBD.Estabelecimentos.Add(new EstabelecimentoBD(estabelecimento));
			contextoBD.SaveChanges();
		}

		public Estabelecimento Find(int id)
		{
            return contextoBD.Estabelecimentos
                             .Include(e => e.Criticas)
                             .Include(e => e.Especialidades)
                             .ThenInclude(e => e.Petisco)
                             .Include(e => e.HorarioFuncionamento)
                             .FirstOrDefault(e => e.IdEstabelecimento == id)?.ToEstabelecimento();
		}

		public void Remove(int id)
		{
			var entity = contextoBD.Estabelecimentos.First(e => e.IdEstabelecimento == id);
			contextoBD.Estabelecimentos.Remove(entity);
			contextoBD.SaveChanges();
		}

        public void Update(Estabelecimento estabelecimento)
		{
            EstabelecimentoBD estabelecimentoBD = contextoBD.Estabelecimentos.First(e => e.IdEstabelecimento == estabelecimento.Id);

            estabelecimentoBD.Nome = estabelecimento.Nome;
            estabelecimentoBD.Latitude = estabelecimento.Localizacao.Latitude;
            estabelecimentoBD.Longitude = estabelecimento.Localizacao.Longitude;
            estabelecimentoBD.Telefone = estabelecimento.Telefone;
            estabelecimentoBD.HorarioFuncionamento = estabelecimento.Horarios.Select(h => new HorarioFuncionamentoBD(h)).ToList();
            estabelecimentoBD.Criticas = estabelecimento.Criticas.Select(c => new CriticasBD(c)).ToList();

            contextoBD.Estabelecimentos.Update(estabelecimentoBD);
			contextoBD.SaveChanges();
		}

        public void DesactivaEstabelecimento(int idEstabelecimento)
        {
            EstabelecimentoBD estabelecimentoBD = contextoBD.Estabelecimentos.First(e => e.IdEstabelecimento == idEstabelecimento);

            estabelecimentoBD.Ativo = false;

			contextoBD.Estabelecimentos.Update(estabelecimentoBD);
			contextoBD.SaveChanges();
        }
	}
}
