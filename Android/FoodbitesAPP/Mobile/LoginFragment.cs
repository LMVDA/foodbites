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

	public class LoginEventArgs : EventArgs
	{
		public String User { get; set; }
		public String Pass { get; set; }
	}

    public class LoginFragment : DialogFragment
    {
        public EventHandler<LoginEventArgs> onLoginCompleto;

		private EditText mUser;
		private EditText mPass;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.Login, container, false);

			mUser = view.FindViewById<EditText>(Resource.Id.editTextLoginUser);
			mPass = view.FindViewById<EditText>(Resource.Id.editTextLoginPass);
            var button = view.FindViewById<Button>(Resource.Id.buttonLoginOK);
            button.Click += Button_Click;
            return view;
        }

        void Button_Click(object sender, EventArgs e)
        {
            if(!mUser.Text.Trim().Equals("") && !mPass.Text.Trim().Equals("")) {
				onLoginCompleto.Invoke(this, new LoginEventArgs { User = mUser.Text, Pass = mPass.Text });
				this.Dismiss();
            }
			else
			{
                Toast.MakeText(Activity, "Por favor, insira o Username e a Password!", ToastLength.Short).Show();
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
