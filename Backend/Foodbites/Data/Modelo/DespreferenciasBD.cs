using System;
namespace Data.Modelo
{
    public class DespreferenciasBD
    {
        public int IdDespreferencia { get; set; }

        public DateTime Data { get; set; }

        public string Caracteristica { get; set; }

        public int IdPreferencia { get; set; }

        public PreferenciaBD Preferencia { get; set; }

        public DespreferenciasBD()
        {
            
        }
    }
}
