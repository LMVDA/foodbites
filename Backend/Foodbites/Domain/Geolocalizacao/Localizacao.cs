using System;
using GeoCoordinatePortable;

namespace Domain.Geolocalizacao
{
    public class Localizacao
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Localizacao()
        {
            
        }

        public Localizacao(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double DistanciaA(Localizacao localizacao)
        {
			var coordenadasA = new GeoCoordinate(Latitude, Longitude);
			var coordenadasB = new GeoCoordinate(localizacao.Latitude, localizacao.Longitude);

            return coordenadasA.GetDistanceTo(coordenadasB);
        }
    }
}
