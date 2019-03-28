using System;
using System.Collections.Generic;
using Domain.Utilizador;
using System.Linq;

namespace Data.Modelo
{
    public class PreferenciaBD
    {
        public int IdPreferencia { get; set; }

        public string Username { get; set; }

        public UtilizadorBD Utilizador { get; set; }

        public List<PreferenciasBD> Preferencias { get; set; }

        public List<DespreferenciasBD> Despreferencias { get; set; }

        public int IdPetisco { get; set; }

        public PetiscoBD Petisco { get; set; }

        public PreferenciaBD()
        {
            
        }

        public Preferencia ToPreferencia() 
        {
            List<Caracteristica> caracteristicas = 
                Preferencias?
                .Select(p => new Caracteristica(p.Data, p.Caracteristica)) // map
                .ToList();

            if (caracteristicas == null) caracteristicas = new List<Caracteristica>();

            Preferencia preferencia = new Preferencia()
            {
                Id = IdPreferencia,
                Petisco = this.Petisco.ToPetisco(),
                Caracteristicas = caracteristicas
            };

            return preferencia;
        }

		public Preferencia ToDespreferencia()
		{
			List<Caracteristica> caracteristicas =
                Despreferencias?
				.Select(p => new Caracteristica(p.Data, p.Caracteristica)) // map
				.ToList();

            if (caracteristicas == null) caracteristicas = new List<Caracteristica>();

			Preferencia preferencia = new Preferencia()
			{
				Id = IdPreferencia,
                Petisco = this.Petisco.ToPetisco(),
				Caracteristicas = caracteristicas
			};

			return preferencia;
		}
	}
}
