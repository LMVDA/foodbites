using System;
namespace Domain.Avaliacao
{
    public class AvaliacaoDetalhada
    {
        public string NomeEspecialidade { get; set; }
		public int NrEstrelas { get; set; }
		public DateTime Data { get; set; }
        public string Fotografia { get; set; }

        public AvaliacaoDetalhada(Review review)
        {
            this.NomeEspecialidade = review.Especialidade.Nome;
            this.NrEstrelas = review.NrEstrelas;
            this.Data = review.Data;
            this.Fotografia = review.Especialidade.Fotografia;
        }
    }
}
