
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace FoodbitesAPP
{
    [Activity(Label = "PetiscosListActivity")]
    public class PetiscosListActivity : Activity
	{
		public List<PetiscoModel> ListaPetiscos { get; set; }
        string username;
        public ListView listViewP;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

			// esconder barra de titulo
			RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.PetiscosLista);

			username = ((MainApplication)Application.Context).CurrentUser.Username;
            ListaPetiscos = ((MainApplication)Application.Context).ListaPetiscos;

            listViewP = FindViewById<ListView>(Resource.Id.listViewPetiscos);
			listViewP.Adapter = new PetiscosListViewAdapter(this, ListaPetiscos);
			listViewP.ItemClick += ListViewP_ItemClick;
        }

        void ListViewP_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
			ApiHTTP.SelecionouPetisco(username, ListaPetiscos[e.Position].IdEspecialidade);
            ((MainApplication)Application.Context).CurrentPetisco = ListaPetiscos[e.Position];

            // mudar para a vista de lista de petiscos
            Intent i = new Intent(this, typeof(PetiscoActivity));
	        StartActivity(i);
        }
    }
}
