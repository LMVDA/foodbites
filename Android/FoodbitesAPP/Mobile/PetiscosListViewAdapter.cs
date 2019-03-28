
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace FoodbitesAPP
{
    public class PetiscosListViewAdapter : BaseAdapter<PetiscoModel>
    {
        public List<PetiscoModel> ListaPetiscos { get; set; }
        private Context mContext;
        public override int Count {
            get { return ListaPetiscos.Count; }
        }
        public override PetiscoModel this[int position] {
            get { return ListaPetiscos[position]; }
        }


		public PetiscosListViewAdapter(Context context, List<PetiscoModel> lista)
		{
			mContext = context;
			ListaPetiscos = lista;
		}

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View linha = convertView;

            if(linha == null) {
                linha = LayoutInflater.From(mContext).Inflate(Resource.Layout.PetiscoLinha, null, false);
            }


			var nome = linha.FindViewById<TextView>(Resource.Id.textViewPLNome);
			var estab = linha.FindViewById<TextView>(Resource.Id.textViewPLEsta);
			var preco = linha.FindViewById<TextView>(Resource.Id.textViewPLPreco);
			var imagem = linha.FindViewById<ImageView>(Resource.Id.imageViewPL);

			nome.Text = ListaPetiscos[position].NomePetisco;
            estab.Text = ListaPetiscos[position].NomeEstabelecimento;
            preco.Text = "Preço: " + ListaPetiscos[position].Preco + " €";
            GetImagemPestiscoAsync(imagem, ListaPetiscos[position]);

            return linha;
        }

        async System.Threading.Tasks.Task GetImagemPestiscoAsync(ImageView imagem, PetiscoModel p)
        {
            if (!p.JaTemImagem) {
                p.ImageBitmap = await ApiHTTP.GetImageBitmapFromUrlAsync(p.Fotografia);
				p.JaTemImagem = true;
			}
			imagem.SetImageBitmap(p.ImageBitmap);
        }
    }
}
