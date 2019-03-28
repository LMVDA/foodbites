using System;
namespace Domain.Utilizador
{
    public class Caracteristica
    {
        public DateTime Data { get; set; }
        public string Nome { get; set; }

        public Caracteristica(DateTime data, string nome)
        {
            Data = data;
            Nome = nome;
        }
    }
}
