﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backoffice.Models.Avaliacao;

namespace Backoffice.Models.Petiscos
{
    public class Especialidade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int PetiscoID { get; set; }

        [Required]
		public int EstabelecimentoID { get; set; }

        [Required]
		[DataType(DataType.Currency)]
		[Display(Name = "Preço")]
		[DisplayFormat(NullDisplayText = "Sem Preço")]
		public double Preco { get; set; }


		[DisplayFormat(NullDisplayText = "Sem Foto")]
		public String Fotografia { get; set; }

        [Required]
        //[DataType(DataType.MultilineText)]
        public IList<Caracteristica> Caracteristicas { get; set; }
        public bool Ativo { get; set; } = true;

        public  Estabelecimento Estabelecimento { get; set; }
        public  Petisco Petisco { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }


        public void Desactiva(){
            Ativo = false;
        }

    }
}
       