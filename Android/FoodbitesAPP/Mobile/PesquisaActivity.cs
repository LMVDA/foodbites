
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Newtonsoft.Json;

namespace FoodbitesAPP
{
    [Activity(Label = "Pesquisa FoodBites")]
    public class PesquisaActivity : Activity
	{
		bool isRecording;
		readonly int VOICE = 10;
        Button btPerfil;
		Button btTendencias;
		Button btVoz;
		Button btRatings;
		Button btHist;
        ImageButton btFiltro;
		LinearLayout filtro1;
		LinearLayout filtro2;
		TextView tvKM;
        EditText editMin;
		EditText editMax;
        SeekBar distSB;
		SearchView caixaPesquisa;
        UserModel user;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // esconder barra de titulo
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Pesquisa);

            user = ((MainApplication)Application.Context).CurrentUser;

            // set the isRecording flag to false (not recording)
            isRecording = false;

            btPerfil = FindViewById<Button>(Resource.Id.buttonPesPerf);
            btPerfil.Click += BtPerfil_Click;

            btTendencias = FindViewById<Button>(Resource.Id.buttonPesTend);
            btTendencias.Click += BtTendencias_ClickAsync;

            btVoz = FindViewById<Button>(Resource.Id.buttonPesMicro);
            btVoz.Click += BtVoz_Click;

            btRatings = FindViewById<Button>(Resource.Id.buttonPesAval);
            btRatings.Click += BtRatings_Click;

            btHist = FindViewById<Button>(Resource.Id.buttonPesHist);
            btHist.Click += BtHist_Click;

			filtro1 = FindViewById<LinearLayout>(Resource.Id.linearLayoutFiltro1);
			filtro2 = FindViewById<LinearLayout>(Resource.Id.linearLayoutFiltro2);
			filtro1.Visibility = ViewStates.Gone;
			filtro2.Visibility = ViewStates.Gone;

			tvKM = FindViewById<TextView>(Resource.Id.textViewPesKMs);
            editMin = FindViewById<EditText>(Resource.Id.editTextPrecoMin);
            editMax = FindViewById<EditText>(Resource.Id.editTextPrecoMax);
			editMin.FocusChange += delegate
			{
				if (!editMin.Text.Equals("") && !editMax.Text.Equals(""))
				{
					int min = Int32.Parse(editMin.Text);
                    int max = Int32.Parse(editMax.Text);
					if (max < min){
                        editMin.Text = editMax.Text;
                    }
                }
            };
			editMax.FocusChange += delegate {
				if (!editMin.Text.Equals("") && !editMax.Text.Equals("")) {
                    int min = Int32.Parse(editMin.Text);
                    int max = Int32.Parse(editMax.Text);
					if (max < min) {
						editMax.Text = editMin.Text;
					}
				}
			};

            distSB = FindViewById<SeekBar>(Resource.Id.seekBar1);
            distSB.ProgressChanged += delegate {
                tvKM.Text = distSB.Progress + " KMs";
            };

            btFiltro = FindViewById<ImageButton>(Resource.Id.imageButtonFilter);
            btFiltro.Click += BtFiltro_Click;
                    
            caixaPesquisa = FindViewById<SearchView>(Resource.Id.searchViewPes);
			caixaPesquisa.SetQueryHint("ex: caldo verde com chouriça");
            caixaPesquisa.QueryTextSubmit += CaixaPesquisa_QueryTextSubmit;
            caixaPesquisa.Iconified = false;

        }


        void BtFiltro_Click(object sender, EventArgs e)
        {
			if (filtro1.Visibility.Equals(ViewStates.Gone))
			{
                filtro1.Visibility = ViewStates.Visible;
                filtro2.Visibility = ViewStates.Visible;
            }
			else
			{
				filtro1.Visibility = ViewStates.Gone;
				filtro2.Visibility = ViewStates.Gone;
			}
        }

        /**
	     * Showing google speech input dialog
	     * */
        void BtVoz_Click(object sender, EventArgs e)
		{
			// setup da Voz-Para-Texto
			string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
			if (rec != "android.hardware.microphone")
			{
				var alert = new AlertDialog.Builder(btVoz.Context);
				alert.SetTitle("O microfone parece não estar a funcionar");
				alert.SetPositiveButton("OK", (sender2, e2) =>
				{
					return;
				});
				alert.Show();
			}
            else
			{
				// change the text on the button
                //btVoz.Text = "Terminar Voz";
				isRecording = !isRecording;
                if (isRecording)
                {
                    // create the intent and start the activity
                    var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

                    // put a message on the modal dialog
                    voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, Application.Context.GetString(Resource.String.messageSpeakNow));

                    // if there is more then 1.5s of silence, consider the speech over
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

                    // you can specify other languages recognised here, for example
                    // voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.German);
                    // if you wish it to recognise the default Locale language and German
                    // if you do use another locale, regional dialects may not be recognised very well

                    voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                    StartActivityForResult(voiceIntent, VOICE);
                }
			}

		}


		protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
		{
			if (requestCode == VOICE)
			{
				if (resultVal == Result.Ok)
				{
					var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
						//string textInput = btTendencias.Text + matches[0];
						string textInput = matches[0];

                        // limit the output to 500 characters
                        if (textInput.Length > 500)
                        {
                            textInput = textInput.Substring(0, 500);
                        }
                        caixaPesquisa.SetQuery(textInput, true);
                        caixaPesquisa.RequestFocus();
                        Console.WriteLine("Voz: "+textInput);
                    }
                    else
                        caixaPesquisa.SetQuery("Não perbeci ...", false);
					// change the text back on the button
                    //btVoz.Text = "Voz";
				}
			}
            isRecording = false;

			base.OnActivityResult(requestCode, resultVal, data);
		}

        void BtTendencias_ClickAsync(object sender, EventArgs e)
		{
			// passar a lista
			List<PetiscoModel> lista = ApiHTTP.GetTendencias(user.Username);
			if (lista.Count == 0)
			{
				Toast.MakeText(this, "Não há tendências neste momento.", ToastLength.Short).Show();
				return;
			}
            ((MainApplication)Application.Context).SetPetiscos(lista);

            // mudar para a vista de lista de petiscos
            Intent i = new Intent(this, typeof(PetiscosListActivity));
            StartActivity(i);
        }

		void BtHist_Click(object sender, EventArgs e)
		{
			// passar a lista
			List<PetiscoModel> lista = ApiHTTP.GetHistorico(user.Username);
			if (lista.Count == 0)
			{
				Toast.MakeText(this, "Sem Histórico.", ToastLength.Short).Show();
                return;
            }
			// ordenar por data descendente
			lista.Sort((x, y) => -1 * DateTime.Compare(x.Data.Value, y.Data.Value));
			((MainApplication)Application.Context).SetPetiscos(lista);

            // mudar para a vista de lista de petiscos
            Intent i = new Intent(this, typeof(PetiscosListActivity));
			StartActivity(i);
		}

        void CaixaPesquisa_QueryTextSubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            caixaPesquisa.ClearFocus();
            if(!e.Query.Trim().Equals("")) {
                Console.WriteLine("Pesquisa: "+e.Query);

                // passar a lista
                List<PetiscoModel> lista;
                if (filtro1.Visibility.Equals(ViewStates.Gone)) {
                    lista = ApiHTTP.GetSugestoes(user.Username, e.Query, null, null, null);
                }
				else
				{
					int min, max;
                    bool minB = Int32.TryParse(editMin.Text, out min);
					bool maxB = Int32.TryParse(editMax.Text, out max);
                    int? minRes = null;
					if (minB) { minRes = min; }
					int? maxRes = null;
					if (maxB) { maxRes = max; }
                    lista = ApiHTTP.GetSugestoes(user.Username, e.Query, minRes, maxRes, distSB.Progress);
				}

	            if (lista.Count == 0)
	            {
	                Toast.MakeText(this, "Não há sugestões para esta pesquisa.", ToastLength.Short).Show();
	                return;
	            }

			    ((MainApplication)Application.Context).SetPetiscos(lista);
				// mudar para a vista de lista de petiscos
				Intent i = new Intent(this, typeof(PetiscosListActivity));
				StartActivity(i);
            }
			else
			{
				Console.WriteLine("Nenhuma Pesquisa");
                Toast.MakeText(this, "Escreva algo para pesquisar", ToastLength.Short).Show();
            }
        }

        void BtPerfil_Click(object sender, EventArgs e)
		{
			// mudar para a vista de lista de petiscos
            Intent i = new Intent(this, typeof(PerfilActivity));
			StartActivity(i);
        }


        void BtRatings_Click(object sender, EventArgs e)
        {
            // mudar para a vista de avaliacoes
            Intent i = new Intent(this, typeof(AvaliacoesActivity));
            StartActivity(i);
        }
    }
}
