using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System;
using Android.Views;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FoodbitesAPP
{

    [Activity(Label = "Foodbites", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
		private Button buttonRegistar;
		private Button buttonLogin;
		private Button buttonSair;
		GeoLocation Location;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

			// esconder barra de titulo
			RequestWindowFeature(WindowFeatures.NoTitle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // geo location
            Location = new GeoLocation(this);
            Coordenadas.Locator.UpdateLocation();

            buttonRegistar = FindViewById<Button>(Resource.Id.buttonRegistar);
            buttonRegistar.Click += ButtonRegistar_Click;

			buttonLogin = FindViewById<Button>(Resource.Id.buttonLogin);
            buttonLogin.Click += ButtonLogin_Click;

			buttonSair = FindViewById<Button>(Resource.Id.buttonSair);
            buttonSair.Click += delegate { Finish(); };
        }

        void ButtonRegistar_Click(object sender, System.EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            RegistarFragment reg = new RegistarFragment();
            reg.Show(transaction, "dialog registar");

            reg.onRegistoCompleto += Reg_OnRegistoCompletoAsync;
        }

        void Reg_OnRegistoCompletoAsync(object sender, FoodbitesAPP.RegistarEventArgs e)
        {
            // post HTTP para registar
            UserModel user = ApiHTTP.Registar(e.Nome, e.User, e.Pass, e.Email);
            if(user != null) {
				//((MainApplication)Application.Context).CurrentUser = user;
				//Intent i = new Intent(this, typeof(PesquisaActivity));
				//StartActivity(i);
				Console.WriteLine("\tRegisto com sucesso!");
				Toast.MakeText(this, "Registo com sucesso!", ToastLength.Long).Show();
            }
            else {
				Console.WriteLine("\tErro no registo!");
                Toast.MakeText(this, "Erro a efetuar registo!", ToastLength.Long).Show();
            }
        }

        void ButtonLogin_Click(object sender, System.EventArgs e)
        {
			FragmentTransaction transaction = FragmentManager.BeginTransaction();
            LoginFragment log = new LoginFragment();
			log.Show(transaction, "dialog login");
            log.onLoginCompleto += Log_OnLoginCompleto;
        }

        void Log_OnLoginCompleto(object sender, FoodbitesAPP.LoginEventArgs e)
        {
			UserModel user = ApiHTTP.Login(e.User, e.Pass);
			if (user != null)
			{
				Toast.MakeText(this, "Login com sucesso!", ToastLength.Short).Show();
				((MainApplication)Application.Context).CurrentUser = user;
				Intent i = new Intent(this, typeof(PesquisaActivity));
				StartActivity(i);
            }
            else {
                Console.WriteLine("\tErro no login!");
                Toast.MakeText(this, "Erro a efetuar o login!", ToastLength.Long).Show();
            }


        }

    }
}

