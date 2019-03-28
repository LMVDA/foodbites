
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class AvaliacoesListViewAdapter : BaseAdapter<RatingUserModel>
    {
        public List<RatingUserModel> ListaRatings { get; set; }
        private Context mContext;
        public override int Count {
            get { return ListaRatings.Count; }
        }
        public override RatingUserModel this[int position] {
            get { return ListaRatings[position]; }
        }

		public AvaliacoesListViewAdapter(Context context, List<RatingUserModel> lista)
		{
			mContext = context;
			ListaRatings = lista;
		}

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View linha = convertView;

            if(linha == null) {
                linha = LayoutInflater.From(mContext).Inflate(Resource.Layout.AvaliacaoLinha, null, false);
            }

            var nome = linha.FindViewById<TextView>(Resource.Id.textViewAvalNome);
            var data = linha.FindViewById<TextView>(Resource.Id.textViewAvalDate);
			var rating = linha.FindViewById<RatingBar>(Resource.Id.ratingBarAval);
			var imagem = linha.FindViewById<ImageView>(Resource.Id.imageViewAvalImg);

            nome.Text = ListaRatings[position].NomeEspecialidade;
            data.Text = "Data de acesso: " + ListaRatings[position].Data.Date.ToShortDateString();
            rating.Rating = ListaRatings[position].NrEstrelas;
            GetImagemPestiscoAsync(imagem, ListaRatings[position].Fotografia);

            return linha;
        }

        async System.Threading.Tasks.Task GetImagemPestiscoAsync(ImageView imagem, String url)
        {
            var imageBitmap = await ApiHTTP.GetImageBitmapFromUrlAsync(url);
            imagem.SetImageBitmap(imageBitmap);
        }
    }
}
