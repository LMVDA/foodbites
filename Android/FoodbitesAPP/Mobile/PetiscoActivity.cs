
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace FoodbitesAPP
{
    
    [Activity(Label = "PetiscoActivity")]
    public class PetiscoActivity : Activity
    {
        ImageView imgView;
        Button btMapa;
        Button btShare;
        RatingBar ratBar;

        PetiscoModel Petisco;
        String userID;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // esconder barra de titulo
            RequestWindowFeature(WindowFeatures.NoTitle);
			SetContentView(Resource.Layout.Petisco);

            userID = ((MainApplication)Application.Context).CurrentUser.Username;
            Petisco = ((MainApplication)Application.Context).CurrentPetisco;

			TextView txtNome = FindViewById<TextView>(Resource.Id.textViewPetNome);
			TextView txtEsta = FindViewById<TextView>(Resource.Id.textViewPetEstab);
			TextView txtTlm = FindViewById<TextView>(Resource.Id.textViewPetTlm);
			TextView txtPreco = FindViewById<TextView>(Resource.Id.textViewPetPreco);
			TextView txtCars = FindViewById<TextView>(Resource.Id.textViewPetCars);
            TextView txtCriticas = FindViewById<TextView>(Resource.Id.textViewCriticas);
			
            txtNome.Text = Petisco.NomePetisco;
            txtEsta.Text = Petisco.NomeEstabelecimento;
            txtTlm.Text = "Contacto: " + Petisco.Telefone;
            txtPreco.Text = "Preço: " + Petisco.Preco.ToString() + " €";
            txtCars.Text = "Características: " + String.Join(",", Petisco.Caracteristicas.ToArray());
            String criticas;
			if (Petisco.Criticas != null && Petisco.Criticas.Count != 0)
			{
				criticas = String.Join("\n", Petisco.Criticas.ToArray());
			}
			else
			{
				criticas = " Sem Críticas";
            }
            txtCriticas.Text = "Críticas: \n" + criticas;

            ratBar = FindViewById<RatingBar>(Resource.Id.ratingBarPet);
            ratBar.Rating = Petisco.NrEstrelas;
            ratBar.RatingBarChange += RatBar_RatingBarChange;

			btMapa = FindViewById<Button>(Resource.Id.buttonPetDirecao);
            btMapa.Click += BtMapa_Click;

            btShare = FindViewById<Button>(Resource.Id.buttonPetPartilhar);
            btShare.Click += BtShare_Click;

            imgView = FindViewById<ImageView>(Resource.Id.imageViewPet);
			imgView.Click += BtMapa_Click;

            GetImagemPestiscoAsync(imgView, Petisco);
		}

        // vai chamar uma aplicação de mapas com as coordenadas do estabelecimento
        void BtMapa_Click(object sender, EventArgs e)
        {
			// fonte: https://developer.xamarin.com/guides/android/platform_features/maps_and_location/maps/part_1_-_maps_application/
			String coords = 
                "geo:0,0?q=" + Petisco.Latitude.ToString().Replace(",", ".") + "," + Petisco.Longitude.ToString().Replace(",", ".") +
                "(" + Petisco.NomeEstabelecimento + ")";
            Console.WriteLine("Mapa: "+coords);
            var geoUri = Android.Net.Uri.Parse(coords);
            var mapIntent = new Intent(Intent.ActionView, geoUri);
            StartActivity(mapIntent);
        }

        void RatBar_RatingBarChange(object sender, RatingBar.RatingBarChangeEventArgs e)
        {
            Petisco.NrEstrelas = (int)e.Rating;
            // fazer PUT HTTP para atualizar o rating
            var res =  ApiHTTP.AvaliarPetisco(userID, Petisco.IdEspecialidade, Petisco.NrEstrelas);
        }

        void BtShare_Click(object sender, EventArgs e)
        {
			Intent intentsend = new Intent();
			intentsend.SetAction(Intent.ActionSend);
            intentsend.PutExtra(Intent.ExtraText, "Vejam '"+Petisco.NomePetisco+"' na aplicação Foodbites!");
			intentsend.SetType("text/plain");
			StartActivity(intentsend);
        }


        async System.Threading.Tasks.Task GetImagemPestiscoAsync(ImageView imgV, PetiscoModel p)
		{
			if (!p.JaTemImagem)
			{
				p.ImageBitmap = await ApiHTTP.GetImageBitmapFromUrlAsync(p.Fotografia);
				p.JaTemImagem = true;
			}
			imgV.SetImageBitmap(p.ImageBitmap);
        }
    }
}
