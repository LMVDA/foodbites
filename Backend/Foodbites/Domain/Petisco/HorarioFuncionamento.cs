using System;
namespace Domain.Petisco
{
    public class HorarioFuncionamento
    {
        public TimeSpan HoraAbertura { get; set; }
        public TimeSpan HoraFecho { get; set; }
        public DayOfWeek Dia { get; set; }

        public HorarioFuncionamento()
        {
        }

        public HorarioFuncionamento(DayOfWeek dayOfWeek)
		{
			HoraAbertura = new TimeSpan(0, 0, 0);
			HoraFecho = new TimeSpan(0, 0, 0);
            Dia = dayOfWeek;
		}
    }
}
