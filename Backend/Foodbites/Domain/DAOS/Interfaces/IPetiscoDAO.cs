using System;
using System.Collections.Generic;
using Domain.Petisco;

namespace Domain.DAOS.Interfaces
{
    public interface IPetiscoDAO
    {
        void Add(Petisco.Petisco petisco);
        List<Petisco.Petisco> GetAll();
        Petisco.Petisco Find(int id);
		void Remove(int id);
        void Update(Petisco.Petisco petisco);
    }
}
