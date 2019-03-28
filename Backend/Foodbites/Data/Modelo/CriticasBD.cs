using System;
namespace Data.Modelo
{
    public class CriticasBD
    {
        public int IdCritica { get; set; }

        public string Critica { get; set; }

        public int IdEstabelecimento { get; set; }

        public EstabelecimentoBD Estabelecimento { get; set; }

        public CriticasBD()
        {
            
        }

		public CriticasBD(string critica)
		{
			Critica = critica;
		}
    }
}
