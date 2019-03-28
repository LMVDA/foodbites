﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backoffice.Models.Petiscos
{
    
    public class HorarioFuncionamento
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

        public bool Ativo { get; set; } = false;

        //[DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:t}", ApplyFormatInEditMode = true)]
        public DateTime? HoraAbertura { get; set; } = DateTime.Today;

        //[DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:t}", ApplyFormatInEditMode = true)]
		public DateTime? HoraFecho { get; set; } = DateTime.Today;

        public DayOfWeek Dia { get; set; }

        public int EstabelecimentoID { get; set; }
        public virtual Estabelecimento Estabelecimento { get; set; }
    }
}
