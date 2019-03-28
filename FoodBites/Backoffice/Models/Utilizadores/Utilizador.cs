using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backoffice.Models.Avaliacao;

namespace Backoffice.Models.Utilizadores
{
    public class Utilizador
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
		public String Email { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Nascimento { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
