using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Petisco;

namespace Domain.Utilizador
{
    public class Preferencia
    {
        public int Id { get; set; }
        public List<Caracteristica> Caracteristicas { get; set; }
        public Petisco.Petisco Petisco { get; set; }

        public Preferencia()
        {
        }
    }
}
