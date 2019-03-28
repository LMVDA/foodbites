using System;
using Domain.Avaliacao;

namespace Data.Modelo
{
    public class ReviewBD
    {
        public int IdReview { get; set; }

        public int Estrelas { get; set; }

        public DateTime Data { get; set; }

        public string Username { get; set; }

        public UtilizadorBD Utilizador { get; set; }

        public int IdEspecialidade { get; set; }

        public EspecialidadeBD Especialidade { get; set; }

        public ReviewBD()
        {
            
        }

        public ReviewBD(Review review, UtilizadorBD utilizador) 
        {
            IdReview = review.Id;
            Estrelas = review.NrEstrelas;
            Data = review.Data;
            Utilizador = utilizador;
        }

        public Review ToReview()
        {
            Review review = new Review()
            {
				Id = IdReview,
                Data = Data,
                NrEstrelas = Estrelas,
                Especialidade = Especialidade.ToEspecialidadeSimples()
            };

            return review;
        }
    }
}
