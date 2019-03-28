﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backoffice.Models.Petiscos;
using Backoffice.Models.Utilizadores;


namespace Backoffice.Models.Avaliacao
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Range(0, 5)]
        public int Estrelas { get; set; }
        public DateTime Data { get; set; }
		public int EspecialidadeID { get; set; }
		public int UtilizadorID { get; set; }

        public virtual Especialidade Especialidade { get; set; }
        public virtual Utilizador Utilizador { get; set; }
    }
}
