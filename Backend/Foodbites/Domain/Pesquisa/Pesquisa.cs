using System;
using System.Collections.Generic;

namespace Domain.Pesquisa
{
    public class Pesquisa
    {
        public string Petisco { get; set; }
        public List<string> Preferencias { get; set; }
        public List<string> Despreferencias { get; set; }

        public Pesquisa()
        {
        }

		public Pesquisa(string petisco, List<string> preferencias, List<string> despreferencias)
		{
            Petisco = petisco;
            Preferencias = preferencias;
            Despreferencias = despreferencias;
		}
    }
}
