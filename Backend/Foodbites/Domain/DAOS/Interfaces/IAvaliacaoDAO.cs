using System;
using System.Collections.Generic;
using Domain.Avaliacao;

namespace Domain.DAOS.Interfaces
{
    public interface IAvaliacaoDAO
    {
		void Add(string username, int idEspecialidade, Review avaliacao);
        List<Review> GetAll();
        Review Find(int id);
		void Remove(int id);
    }
}
