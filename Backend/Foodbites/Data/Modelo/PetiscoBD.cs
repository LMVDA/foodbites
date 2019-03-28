using System;
using System.Collections.Generic;
using Domain.Petisco;

namespace Data.Modelo
{
    public class PetiscoBD
    {
        public int IdPetisco { get; set; }

		public string Nome { get; set; }

        public List<EspecialidadeBD> Especialidades { get; set; }

        public List<PreferenciaBD> Preferencias { get; set; }

        public PetiscoBD()
        {
        }

		public PetiscoBD(Petisco petisco)
		{
            Nome = petisco.Nome;
            Especialidades = new List<EspecialidadeBD>();
            Preferencias = new List<PreferenciaBD>();
		}

        public PetiscoBD(Especialidade especialidade)
        {
            IdPetisco = especialidade.Id;
            Nome = especialidade.Nome;
        }

        public Petisco ToPetisco()
        {
            return new Petisco
            {
                Nome = Nome,
                Id = IdPetisco
            };
        }

	}
}
