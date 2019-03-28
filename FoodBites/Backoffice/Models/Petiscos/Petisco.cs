﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backoffice.Models.Petiscos
{
    public class Petisco
    {

	    [Key]
	    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String Nome { get; set; }

        public virtual ICollection<Especialidade> Especialidades { get; set; }

    }
}
