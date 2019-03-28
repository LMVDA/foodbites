using System;
using Domain.Petisco;

namespace Data.Modelo
{
    public class HorarioFuncionamentoBD
    {
        public int IdHorarioFuncionamento { get; set; }

        public DayOfWeek Dia { get; set; }

        public TimeSpan HoraAbertura { get; set; }

        public TimeSpan HoraFecho { get; set; }

        public int IdEstabelecimento { get; set; }

        public EstabelecimentoBD Estabelecimento { get; set; }

        public HorarioFuncionamentoBD()
        {
            
        }

		public HorarioFuncionamentoBD(HorarioFuncionamento horarioFuncionamento)
		{
			Dia = horarioFuncionamento.Dia;
			HoraAbertura = horarioFuncionamento.HoraAbertura;
			HoraFecho = horarioFuncionamento.HoraFecho;
		}

        public HorarioFuncionamento ToHorarioFuncionamento()
        {
            HorarioFuncionamento horarioFuncionamento = new HorarioFuncionamento()
            {
                Dia = Dia,
                HoraAbertura = HoraAbertura,
                HoraFecho = HoraFecho
            };

            return horarioFuncionamento;
        }
    }
}
