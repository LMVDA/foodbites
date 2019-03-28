using System;
namespace Data.Modelo
{
    public class CaracteristicasBD
    {
        public int IdCaracteristica { get; set; }

        public string Caracteristica { get; set; }

        public int IdEspecialidade { get; set; }

        public EspecialidadeBD Especialidade { get; set; }

        public CaracteristicasBD() {
            
        }

        public CaracteristicasBD(string caracteristica, EspecialidadeBD especialidade)
        {
            Caracteristica = caracteristica;
            Especialidade = especialidade;
        }
    }
}
