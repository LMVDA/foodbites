using System;
using System.Collections.Generic;
using Android.Graphics;

namespace FoodbitesAPP
{
    public class PetiscoModel
	{
		public int IdEspecialidade { get; set; }
		public String NomePetisco { get; set; }
		public double Preco { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public String NomeEstabelecimento { get; set; }
		public String Telefone { get; set; }
		public int NrEstrelas { get; set; }
		public List<String> Caracteristicas { get; set; }
		public List<String> Criticas { get; set; }
		public DateTime? Data { get; set; }
		public String Fotografia { get; set; }
        public Bitmap ImageBitmap { get; set; }
        public bool JaTemImagem { get; set; } = false;
    }
}
