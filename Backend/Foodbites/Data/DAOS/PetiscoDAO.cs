namespace Data.DAOS
{
    using System.Collections.Generic;
    using System.Linq;
    using Data.Modelo;
    using Data.DAOS.Context;
	using Domain.DAOS.Interfaces;
    using Domain.Petisco;

    public class PetiscoDAO : IPetiscoDAO
	{
		private readonly FoodbitesContext contextoBD;

		public PetiscoDAO(FoodbitesContext context)
		{
			contextoBD = context;
		}

		public List<Petisco> GetAll()
		{
            return contextoBD.Petiscos.Select(p => p.ToPetisco()).ToList();
		}

        public void Add(Petisco petisco)
		{
            contextoBD.Petiscos.Add(new PetiscoBD(petisco));
			contextoBD.SaveChanges();
		}

		public Petisco Find(int id)
		{
            return contextoBD.Petiscos.FirstOrDefault(p => p.IdPetisco == id)?.ToPetisco();
		}

		public void Remove(int id)
		{
            var entity = contextoBD.Petiscos.First(p => p.IdPetisco == id);
            contextoBD.Petiscos.Remove(entity);
			contextoBD.SaveChanges();
		}

        public void Update(Petisco petisco)
		{
            PetiscoBD petiscoDB = contextoBD.Petiscos.First(p => p.IdPetisco == petisco.Id);

            petiscoDB.Nome = petisco.Nome;

            contextoBD.Petiscos.Update(petiscoDB);
			contextoBD.SaveChanges();
		}
	}
}
