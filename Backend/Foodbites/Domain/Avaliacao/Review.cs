using System;
using Domain.Petisco;

namespace Domain.Avaliacao
{
    public class Review
    {
        public int Id { get; set; }
        public int NrEstrelas { get; set; }
        public DateTime Data { get; set; }

        public Especialidade Especialidade { get; set; }

        public Review()
        {
        }

        public bool AvaliacaoFraca(int idEspecialidade)
        {
            if (Especialidade.IdEspecialidade == idEspecialidade)
            {
                return NrEstrelas < 3;
            }

            return false;
        }
    }
}
