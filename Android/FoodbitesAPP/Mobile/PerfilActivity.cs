
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
    [Activity(Label = "PerfilActivity")]
    public class PerfilActivity : Activity
    {
        public UserModel user;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // esconder barra de titulo
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Perfil);

			user = ((MainApplication)Application.Context).CurrentUser;

            Button btAlterar = FindViewById<Button>(Resource.Id.buttonPerfilAlterar);
            btAlterar.Click += ButtonAlterar_Click;

			AtualizaTexto();
        }

        void ButtonAlterar_Click(object sender, System.EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            RegistarFragment reg = new RegistarFragment(user);
            reg.Show(transaction, "dialog registar");

            reg.onRegistoCompleto += Reg_OnAlterarCompletoAsync;
        }

        void Reg_OnAlterarCompletoAsync(object sender, FoodbitesAPP.RegistarEventArgs e)
        {
            user.Nome = e.Nome;
            user.Password = e.Pass;
            user.Email = e.Email;

			// post HTTP para registar
            bool ok = ApiHTTP.AtualizaUser(user);
			AtualizaTexto();
        }

        void AtualizaTexto() {
			TextView tvNome = FindViewById<TextView>(Resource.Id.textViewPerfilNome);
			TextView tvUser = FindViewById<TextView>(Resource.Id.textViewPerfilUsername);
			TextView tvPass = FindViewById<TextView>(Resource.Id.textViewPerfilPass);
			TextView tvEmail = FindViewById<TextView>(Resource.Id.textViewPerfilEmail);
			tvNome.Text = "Nome: " + user.Nome;
			tvUser.Text = "Username: " + user.Username;
			tvPass.Text = "Password: " + user.Password;
			tvEmail.Text = "Email: " + user.Email;
        }
    }

}
