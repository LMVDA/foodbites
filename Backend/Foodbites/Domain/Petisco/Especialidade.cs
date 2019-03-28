using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Avaliacao;

namespace Domain.Petisco
{
    public class Especialidade : Petisco
    {
        public int IdEspecialidade { get; set; }
        public double Preco { get; set; }
        public string Fotografia { get; set; }
        public bool Ativo { get; set; }
        public List<string> Caracteristicas { get; set; }

        public List<Review> Avaliacoes { get; set; }

        public Estabelecimento Estabelecimento { get; set; }

        public int Avaliacao 
        {
            get 
            {
                if (Avaliacoes == null || Avaliacoes.Count == 0) return 0;

                return (int)Avaliacoes.Average(a => a.NrEstrelas);
            }
        }

        public Especialidade()
        {
            Caracteristicas = new List<string>();
            Avaliacoes = new List<Review>();
        }

    }
}
