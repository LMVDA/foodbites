using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backoffice.Models.Petiscos;

namespace Backoffice.Models.Avaliacao
{
    public class Critica
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Display(Name = "Crítica")]
        public String Texto { get; set; }
        public int EstabelecimentoID { get; set; }

        public virtual Estabelecimento Estabelecimento  { get; set; }
    }
}
