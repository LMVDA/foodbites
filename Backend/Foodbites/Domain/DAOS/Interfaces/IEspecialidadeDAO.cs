using System;
using System.Collections.Generic;
using Domain.Petisco;

namespace Domain.DAOS.Interfaces
{
    public interface IEspecialidadeDAO
    {
        List<Especialidade> GetAll();
        List<Especialidade> GetAll(string nomePetisco);
        List<Especialidade> GetAllEntreDatas(DateTime dataInicio, DateTime dataFim);
        List<string> GetAllNomesPetiscos();
        Especialidade Find(int id);
        void Remove(int id);
        void Update(Especialidade especialidade);
        void Add(int idEstabelecimento, Especialidade especialidade);
        void DesactivaEspecialidade(int idEspecialidade);
    }
}
