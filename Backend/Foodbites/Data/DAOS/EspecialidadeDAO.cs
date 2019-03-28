namespace Data.DAOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Modelo;
    using Data.DAOS.Context;
    using Domain.DAOS.Interfaces;
    using Domain.Petisco;
    using Microsoft.EntityFrameworkCore;

    public class EspecialidadeDAO : IEspecialidadeDAO
    {
        private readonly FoodbitesContext contextoBD;

        public EspecialidadeDAO(FoodbitesContext context)
        {
            contextoBD = context;
        }

        public List<Especialidade> GetAll()
        {
            return contextoBD.Especialidades
                             .Include(e => e.Avaliacoes)
                             .Include(e => e.Caracteristicas)
                             .Include(e => e.Estabelecimento)
                             .Include(e => e.Estabelecimento.Criticas)
                             .Include(e => e.Estabelecimento.HorarioFuncionamento)
                             .Include(e => e.Petisco)
                             .Select(e => e.ToEspecialidade()).ToList();
        }

        public Especialidade Find(int id)
        {
			return contextoBD.Especialidades
							 .Include(e => e.Avaliacoes)
							 .Include(e => e.Caracteristicas)
							 .Include(e => e.Estabelecimento)
							 .Include(e => e.Estabelecimento.Criticas)
							 .Include(e => e.Estabelecimento.HorarioFuncionamento)
							 .Include(e => e.Petisco)
                             .FirstOrDefault(e => e.IdEspecialidade == id)?.ToEspecialidade();
        }

        public void Remove(int id)
        {
            var entity = contextoBD.Especialidades.First(e => e.IdEspecialidade == id);
            contextoBD.Especialidades.Remove(entity);
            contextoBD.SaveChanges();
        }

        public void Update(Especialidade especialidade)
        {
            EspecialidadeBD especialidadeBD = contextoBD.Especialidades.First(e => e.IdEspecialidade == especialidade.IdEspecialidade);

            especialidadeBD.Preco = especialidade.Preco;
            especialidadeBD.Fotografia = especialidade.Fotografia;
            especialidadeBD.Ativo = especialidade.Ativo;
            especialidadeBD.Caracteristicas = especialidade.Caracteristicas.Select(c => new CaracteristicasBD(c, especialidadeBD)).ToList();

            contextoBD.Especialidades.Update(especialidadeBD);
            contextoBD.SaveChanges();
        }

        public List<Especialidade> GetAll(string nomePetisco)
        {
            var petisco = contextoBD.Petiscos
                                    .Include(p => p.Especialidades)
                                    .Include(p => p.Preferencias)
                                    .Where(p => p.Nome.ToLower().Equals(nomePetisco.ToLower())).FirstOrDefault();

            if (petisco == null || petisco.Especialidades == null) return new List<Especialidade>();

            var idsEspecialidades = petisco.Especialidades.Select(e => e.IdEspecialidade);

            var especialidades = contextoBD.Especialidades
                                           .Include(e => e.Avaliacoes)
                                           .Include(e => e.Caracteristicas)
                                           .Include(e => e.Estabelecimento)
                                           .Include(e => e.Estabelecimento.Criticas)
                                           .Include(e => e.Estabelecimento.HorarioFuncionamento)
                                           .Include(e => e.Petisco)
                                           .Where(e => idsEspecialidades.Contains(e.IdEspecialidade))
                                           .Where(e => e.Ativo)
                                           .Where(e => e.Estabelecimento.Ativo)
                                           .Select(e => e.ToEspecialidade())
                                           .ToList();

            return especialidades;
        }

        public List<Especialidade> GetAllEntreDatas(DateTime dataInicio, DateTime dataFim)
        {
			return contextoBD.Especialidades
							 .Include(e => e.Avaliacoes)
							 .Include(e => e.Caracteristicas)
							 .Include(e => e.Estabelecimento)
							 .Include(e => e.Estabelecimento.Criticas)
							 .Include(e => e.Estabelecimento.HorarioFuncionamento)
							 .Include(e => e.Petisco)
                             .Where(e => e.Avaliacoes.Any(a => a.Data >= dataInicio && a.Data <= dataFim))
                             .Select(e => e.ToEspecialidade())
                             .ToList();
        }

        public List<string> GetAllNomesPetiscos()
        {
            return contextoBD.Petiscos.Select(p => p.Nome).ToList();
        }

        public void Add(int idEstabelecimento, Especialidade especialidade)
        {
            var estabelecimentoBD = contextoBD.Estabelecimentos.FirstOrDefault(e => e.IdEstabelecimento == idEstabelecimento);

            if (estabelecimentoBD != null) 
            {
                var petiscoBD = contextoBD.Petiscos.FirstOrDefault(p => p.IdPetisco == especialidade.Id);

                if (petiscoBD != null)
                {
                    EspecialidadeBD especialidadeBD = new EspecialidadeBD(especialidade, estabelecimentoBD);
                    especialidadeBD.Petisco = petiscoBD;

                    contextoBD.Especialidades.Add(especialidadeBD);
                    contextoBD.SaveChanges();
                }
            }
        }

        public void DesactivaEspecialidade(int idEspecialidade)
        {
            EspecialidadeBD especialidadeBD = contextoBD.Especialidades.First(e => e.IdEspecialidade == idEspecialidade);

            especialidadeBD.Ativo = false;

            contextoBD.Especialidades.Update(especialidadeBD);
            contextoBD.SaveChanges();
        }
    }
}
