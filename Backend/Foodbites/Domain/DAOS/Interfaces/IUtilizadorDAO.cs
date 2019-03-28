using System;
using System.Collections.Generic;
using Domain.Petisco;
using Domain.Utilizador;

namespace Domain.DAOS.Interfaces
{
    public interface IUtilizadorDAO
    {
		void Add(Foodbiter foodBiter);
        void AddUserPreferences(string username, int idEspecialidade, List<string> preferencias, List<string> despreferencias);
        List<Foodbiter> GetAll();
        Foodbiter Find(string username);
		void Remove(string username);
        void Update(string username, string nome, string email, DateTime? dataNascimento, string password);
		void AddSelecaoDegustacao(string username, int idEspecialidade);
		List<(Especialidade, DateTime)> GetAllSelecoesDegustacao(string username);
    }
}
