using System;
using System.Collections.Generic;
using Domain.Petisco;

namespace Domain.DAOS.Interfaces
{
    public interface IEstabelecimentoDAO
    {
		void Add(Estabelecimento estabelecimento);
        List<Estabelecimento> GetAll();
        Estabelecimento Find(int id);
		void Remove(int id);
        void Update(Estabelecimento estabelecimento);
        void DesactivaEstabelecimento(int idEstabelecimento);
    }
}
