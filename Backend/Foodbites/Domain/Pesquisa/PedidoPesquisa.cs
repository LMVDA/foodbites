using System;
using Domain.Geolocalizacao;

namespace Domain.Pesquisa
{
    public class PedidoPesquisa
    {
        public string Utilizador { get; set; }

        public string TextoPesquisa { get; set; }

        public Localizacao Localizacao { get; set; }

        public double? PrecoMax { get; set; }

        public double? PrecoMin { get; set; }

        public double? Distancia { get; set; }
    }
}
