using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Geolocalizacao;

namespace Domain.Petisco
{
    public class Estabelecimento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<string> Criticas { get; set; }
        public string Telefone { get; set; }
        public bool Ativo { get; set; }

        public List<Especialidade> Especialidades { get; set; }
        public List<HorarioFuncionamento> Horarios { get; set; }

        public Localizacao Localizacao { get; set; }

        public Estabelecimento()
        {
            Especialidades = new List<Especialidade>();
            Horarios = new List<HorarioFuncionamento>();
        }

        public bool Aberto(DateTime data) 
        {
            if (!Horarios.Any(h => h.Dia == data.DayOfWeek)) return false;

            var horariosAux = Horarios.ToLookup(h => h.Dia, h => new { HoraAbertura = h.HoraAbertura, HoraFecho = h.HoraFecho });
            var horas = horariosAux[data.DayOfWeek].ToList();

            bool aberto = false;

            for (int i = 0; i < horas.Count() && !aberto; i++)
            {
                var intervaloHora = horas[i];
                aberto = data.TimeOfDay >= intervaloHora.HoraAbertura && data.TimeOfDay <= intervaloHora.HoraFecho;
            }

            return aberto;
        }
    }
}
