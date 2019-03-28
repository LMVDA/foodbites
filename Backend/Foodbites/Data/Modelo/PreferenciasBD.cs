using System;
using Domain.Utilizador;

namespace Data.Modelo
{
    public class PreferenciasBD
    {
        public int Id { get; set; }
        
		public DateTime Data { get; set; }

		public string Caracteristica { get; set; }

        public int IdPreferencia { get; set; }

		public PreferenciaBD Preferencia { get; set; }

        public PreferenciasBD()
        {
            
        }
    }
}
