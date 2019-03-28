﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace FoodbitesAPP
{

	public class RegistarEventArgs : EventArgs
	{
		public String Nome { get; set; }
		public String User { get; set; }
		public String Pass { get; set; }
		public String Email { get; set; }
	}

	public class RegistarFragment : DialogFragment
	{
		public EventHandler<RegistarEventArgs> onRegistoCompleto;

        public EditText mNome;
		public EditText mUser;
		public EditText mPass;
		public EditText mEmail;
        public Button botao;
        public UserModel user;
        bool alterar = false;

        public RegistarFragment()
        {
            alterar = false;
        }

        public RegistarFragment(UserModel u) {
            user = u;
            alterar = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Registar, container, false);

			mNome = view.FindViewById<EditText>(Resource.Id.editTextRegNome);
            mUser = view.FindViewById<EditText>(Resource.Id.editTextRegUser);
			mPass = view.FindViewById<EditText>(Resource.Id.editTextRegPass);
            mEmail = view.FindViewById<EditText>(Resource.Id.editTextRegEmail);
            botao = view.FindViewById<Button>(Resource.Id.buttonRegistarOK);
            botao.Click += Button_Click;

            if(alterar) {
	            mNome.Text = user.Nome;
                mUser.Visibility = ViewStates.Gone;
	            mPass.Text = user.Password;
	            mEmail.Text = user.Email;
	            botao.Text = "Alterar";
            }

			return view;
        }

        void Button_Click(object sender, EventArgs e)
		{
            string nome = mNome.Text.Trim();
            string username = alterar ? user.Username : mUser.Text.Trim();
            string pass = mPass.Text.Trim();
            string email = mEmail.Text.Trim();

            if (!email.Equals("") && !username.Equals("") && !pass.Equals("") && !email.Equals(""))
			{
				onRegistoCompleto.Invoke(this, new RegistarEventArgs
				{
					Nome = nome,
                    User = username,
					Pass = pass,
					Email = email
				});
				this.Dismiss();
			}
			else
			{
				Toast.MakeText(Activity, "Por favor, preencha todos os campos!", ToastLength.Short).Show();
			}
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            // tira a barra de cima
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
        }
    }
}
