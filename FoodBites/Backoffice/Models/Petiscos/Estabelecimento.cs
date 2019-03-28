﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backoffice.Models.Avaliacao;
using Backoffice.Models.GeoLocalizacao;

namespace Backoffice.Models.Petiscos
{
    public class Estabelecimento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
		public String Nome { get; set; }
		[DisplayFormat(NullDisplayText = "Sem Telefone")]
        public String Telefone { get; set; }
        public bool Ativo { get; set; } = true;
		public Localizacao Localizacao { get; set; }
		public List<Critica> Criticas { get; set; }

		public ICollection<HorarioFuncionamento> Horarios { get; set; }

        public virtual ICollection<Especialidade> Especialidades { get; set; }


        public void Desactiva() {
            Ativo = false;
        }

    }
}
