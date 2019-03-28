namespace Data.DAOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Modelo;
    using Data.DAOS.Context;
    using Domain.DAOS.Interfaces;
    using Domain.Utilizador;
    using Microsoft.EntityFrameworkCore;
    using Domain.Petisco;

    public class UtilizadorDAO : IUtilizadorDAO
    {
        private readonly FoodbitesContext contextoBD;

        public UtilizadorDAO(FoodbitesContext context)
        {
            contextoBD = context;
        }

        public List<Foodbiter> GetAll()
        {
            return contextoBD.Utilizadores.Select(u => u.ToFoodbiter()).ToList();
        }

        public void Add(Foodbiter foodBiter)
        {
            contextoBD.Utilizadores.Add(new UtilizadorBD(foodBiter));
            contextoBD.SaveChanges();
        }

        public void AddUserPreferences(string username, int idEspecialidade, List<string> preferencias, List<string> despreferencias)
        {
            var utilizador = contextoBD.Utilizadores
                                       .Include(u => u.Preferencias)
                                       .ThenInclude(p => p.Preferencias)
                                       .Include(u => u.Preferencias)
									   .ThenInclude(p => p.Despreferencias)
							           .Include(u => u.Preferencias)
							           .ThenInclude(p => p.Petisco)
                                       .Include(u => u.Avaliacoes)
                                       .ThenInclude(a => a.Especialidade)
                                       .Include(u => u.Avaliacoes)
                                       .ThenInclude(a => a.Especialidade.Estabelecimento)
                                       .Include(u => u.Avaliacoes)
                                       .ThenInclude(a => a.Especialidade.Petisco)
                                       .Where(u => u.Username.Equals(username)).FirstOrDefault();

            var petisco = contextoBD.Petiscos.Where(p => p.Especialidades.Any(e => e.IdEspecialidade == idEspecialidade)).FirstOrDefault(); 

            var data = DateTime.Now;

            if (utilizador != null)
            {
                var preferencia = utilizador.Preferencias.FirstOrDefault(p => p.Petisco.IdPetisco == petisco.IdPetisco);

                if (preferencia == null)
                {
                    preferencia = new PreferenciaBD();

                    preferencia.Petisco = petisco;
                    preferencia.Utilizador = utilizador;

                    contextoBD.Preferencias.Add(preferencia);
                    contextoBD.SaveChanges();

                    utilizador = contextoBD.Utilizadores.Where(u => u.Username.Equals(username)).FirstOrDefault();
                    preferencia = utilizador.Preferencias.FirstOrDefault(p => p.Petisco.IdPetisco == idEspecialidade);
                }

                foreach (var pref in preferencias)
                {
                    var preferenciaBD = new PreferenciasBD { Data = data, Preferencia = preferencia, Caracteristica = pref };
                    contextoBD.Add(preferenciaBD);
                }

                foreach (var despref in despreferencias)
                {
                    var despreferenciaBD = new DespreferenciasBD { Data = data, Preferencia = preferencia, Caracteristica = despref };
                    contextoBD.Add(despreferenciaBD);
                }

                contextoBD.SaveChanges();
            }
        }

        public Foodbiter Find(string username)
        {
            return contextoBD.Utilizadores
                             .Include(u => u.Preferencias)
                             .ThenInclude(p => p.Preferencias)
                             .Include(u => u.Preferencias)
                             .ThenInclude(p => p.Despreferencias)
                             .Include(u => u.Preferencias)
                             .ThenInclude(p => p.Petisco)
                             .Include(u => u.Avaliacoes)
                             .ThenInclude(a => a.Especialidade)
                             .Include(u => u.Avaliacoes)
                             .ThenInclude(a => a.Especialidade.Estabelecimento)
                             .Include(u => u.Avaliacoes)
                             .ThenInclude(a => a.Especialidade.Petisco)
                             .FirstOrDefault(f => f.Username.Equals(username))?.ToFoodbiter();
        }

        public void Remove(string username)
        {
            var entity = contextoBD.Utilizadores.First(f => f.Username.Equals(username));
            contextoBD.Utilizadores.Remove(entity);
            contextoBD.SaveChanges();
        }

        public void Update(string username, string nome, string email, DateTime? dataNascimento, string password)
        {
            var utilizador = contextoBD.Utilizadores.First(f => f.Username.Equals(username));

            if (username != null) utilizador.Username = username;
            if (nome != null) utilizador.Nome = nome;
            if (email != null) utilizador.Email = email;
            if (dataNascimento.HasValue) utilizador.DataNascimento = dataNascimento.Value;
            if (password != null) utilizador.Password = password;

            contextoBD.Utilizadores.Update(utilizador);
            contextoBD.SaveChanges();
		}

		public void AddSelecaoDegustacao(string username, int idEspecialidade)
		{
			var selecoesDegustacao = new SelecoesDegustacaoBD
			{
				Username = username,
				IdEspecialidade = idEspecialidade,
				Data = DateTime.Now
			};

			contextoBD.SelecoesDegustacao.Add(selecoesDegustacao);
			contextoBD.SaveChanges();
		}

		public List<(Especialidade, DateTime)> GetAllSelecoesDegustacao(string username)
		{
			var selecoesDegustacao = contextoBD.SelecoesDegustacao.Where(e => e.Username.Equals(username));

			var especialidadesIds = selecoesDegustacao.Select(s => s.IdEspecialidade);
			var especialidades = contextoBD.Especialidades
										   .Include(e => e.Avaliacoes)
										   .Include(e => e.Caracteristicas)
										   .Include(e => e.Estabelecimento)
										   .Include(e => e.Estabelecimento.Criticas)
										   .Include(e => e.Estabelecimento.HorarioFuncionamento)
										   .Include(e => e.Petisco)
										   .Where(e => especialidadesIds.Contains(e.IdEspecialidade))
										   .Select(e => e.ToEspecialidade())
										   .ToList();

			List<(Especialidade, DateTime)> lista = new List<(Especialidade, DateTime)>();

			foreach (var especialidade in especialidades)
			{
				var data = selecoesDegustacao.FirstOrDefault(s => s.IdEspecialidade == especialidade.IdEspecialidade).Data;
				lista.Add((especialidade, data));
			}

			return lista;
		}
    }
}
