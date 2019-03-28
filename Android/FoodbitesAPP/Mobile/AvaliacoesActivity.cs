
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
    [Activity(Label = "AvaliacoesActivity")]
    public class AvaliacoesActivity : Activity
    {
        String username;
        ListView AvalsLW;
        List<RatingUserModel> ListaRatings;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

			// esconder barra de titulo
			RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Avaliacoes);

            username = ((MainApplication)Application.Context).CurrentUser.Username;
            
            AvalsLW = FindViewById<ListView>(Resource.Id.listViewAvaliacoes);
            ListaRatings = ApiHTTP.GetAvaliacoes(username);
            if(ListaRatings == null) {
                Toast.MakeText(this, "Não há Avaliações", ToastLength.Short).Show();
                Finish();
            }
            else{
	            AvalsLW.Adapter = new AvaliacoesListViewAdapter(this, ListaRatings);
	            AvalsLW.ItemClick += AvalsLW_ItemClick;;
            }

        }

        void AvalsLW_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            // do nothing
        }
    }
}
