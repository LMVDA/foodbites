using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backoffice.Models.Petiscos
{
    public class Caracteristica
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
		[Display(Name = "Característica")]
		public String Texto { get; set; }
		[Required]
		public int EspecialidadeID { get; set; }

        public Especialidade Especialidade  { get; set; }
    }
}
