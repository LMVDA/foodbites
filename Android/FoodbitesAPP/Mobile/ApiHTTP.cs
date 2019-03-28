using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Graphics;
using Newtonsoft.Json;
using RestSharp;

namespace FoodbitesAPP
{

	class DadosLogin
	{
		public String Username { get; set; }
		public String Password { get; set; }
	}


    public static class ApiHTTP
    {
        public static bool Debug { get; set; } = false;

        const string HOST = "http://192.168.43.106:5000";
        const string URL_LOGIN = "/api/autenticacao/login";
        const string URL_REGISTO = "/api/utilizador";
        const string URL_ACTUALIZA_USER = "/api/utilizador/{username}";
        const string URL_AVALIACOES = "/api/utilizador/{username}/avaliacoes";
        const string URL_HISTORICO = "/api/utilizador/{username}/historico";
        const string URL_AVALIAR = "/api/utilizador/{username}/avaliacao/{id}";
        const string URL_TENDENCIAS = "/api/pesquisa/tendencias";
        const string URL_SUGESTOES = "/api/pesquisa/sugestoes";

        public static UserModel Registar(String nome, String username, String password, String email)
        {
            if (Debug)
            {
                Console.WriteLine("API DEBUG: registei");
                return new UserModel
                {
                    Nome = nome,
                    Username = username,
                    Password = password,
                    Email = email
                };
            }

            var client = new RestClient(HOST);
            var request = new RestRequest(URL_REGISTO, Method.POST);
            request.RequestFormat = DataFormat.Json;
            string json = JsonConvert.SerializeObject(
                new UserModel { Nome = nome, Username = username, Password = password, Email = email });
            request.AddParameter("text/json", json, ParameterType.RequestBody);

            var response = client.Execute(request);
            Console.WriteLine("API registo: " + response.StatusCode);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new UserModel
                {
                    Nome = nome,
                    Username = username,
                    Password = password,
                    Email = email
                };

            }
            return null;
        }


        public static UserModel Login(String username, String password)
        {
            if (Debug)
            {
                Console.WriteLine("API DEBUG: Login");
                return new UserModel
                {
                    Nome = "Ana",
                    Username = username,
                    Password = password,
                    Email = "ana@li4.com"
                };
            }

            var client = new RestClient(HOST);
            var request = new RestRequest(URL_LOGIN, Method.POST);
            request.RequestFormat = DataFormat.Json;
            //request.AddBody(new { A = "foo", B = "bar" }); // uses JsonSerializerstring json = JsonConvert.SerializeObject(dados);
            string json = JsonConvert.SerializeObject(new DadosLogin { Username = username, Password = password });
            request.AddParameter("text/json", json, ParameterType.RequestBody);

            var response = client.Execute(request);
            Console.WriteLine("API login: " + response.StatusCode);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<UserModel>(response.Content);
            }

            return null;
        }


        public static bool AtualizaUser(UserModel user)
        {
            if (Debug)
            {
                Console.WriteLine("API DEBUG: Atualizei user!");
                return true;
            }
            var client = new RestClient(HOST);
            var request = new RestRequest(URL_ACTUALIZA_USER, Method.PUT);
            request.AddUrlSegment("username", user.Username);
            request.RequestFormat = DataFormat.Json;
            string json = JsonConvert.SerializeObject(user);
            request.AddParameter("text/json", json, ParameterType.RequestBody);


            var response = client.Execute(request);
            Console.WriteLine("API atualiza user: " + response.ResponseStatus);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }


        public static List<RatingUserModel> GetAvaliacoes(String username)
        {
            if (Debug)
            {
                Console.WriteLine("API DEBUG: avaliacoes!");
                return new List<RatingUserModel> {
                    new RatingUserModel {
                        NomeEspecialidade="Espetada de Chouriço",
                        NrEstrelas=1,
                        Data = new DateTime(2017,10,11),
                        Fotografia = "http://images-cdn.impresa.pt/expresso/2016-07-15-petiscos.jpeg/original"
                    },
                    new RatingUserModel {
                        NomeEspecialidade="Chouriça Assada",
                        NrEstrelas=4,
                        Data = new DateTime(2010,03,23),
                        Fotografia = "http://images-cdn.impresa.pt/bcbm/2016-04-26-img_0473.jpg/3x2/mw-1240"
                    }
                };
            }

            var client = new RestClient(HOST);
            var request = new RestRequest(URL_AVALIACOES, Method.GET);
            request.AddUrlSegment("username", username);

            var response = client.Execute(request);
			Console.WriteLine("API avaliacoes: " + response.Content);
			if (response.StatusCode == HttpStatusCode.OK)
			{
                var lista = JsonConvert.DeserializeObject<List<RatingUserModel>>(response.Content);
                CompletaURLFotografia2(lista);
                return lista;
			}

            return null;
        }


        public static List<PetiscoModel> GetHistorico(String username)
        {

            if (Debug)
            {
                Console.WriteLine("API DEBUG: historico!");
                return ExemploListaPeticos();
            }

            var client = new RestClient(HOST);
            var request = new RestRequest(URL_HISTORICO, Method.GET);
            request.AddUrlSegment("username", username);

            var response = client.Execute(request);
            Console.WriteLine("API historico: " + response.Content);

            if (response.StatusCode == HttpStatusCode.OK)
			{
				var lista = JsonConvert.DeserializeObject<List<PetiscoModel>>(response.Content);
				CompletaURLFotografia(lista);
				return lista;
            }
            return null;
        }


        public static bool SelecionouPetisco(String username, int idPetisco)
        {
            if (Debug)
            {
                Console.WriteLine("API DEBUG: selecionou ->" + idPetisco);
                return true;
            }

            var client = new RestClient(HOST);
            var request = new RestRequest(URL_HISTORICO, Method.POST);
            request.AddUrlSegment("username", username);
            request.RequestFormat = DataFormat.Json;
            string json = JsonConvert.SerializeObject(idPetisco);
            request.AddParameter("text/json", json, ParameterType.RequestBody);

            var response = client.Execute(request);
            Console.WriteLine("API selecionou petisco: " + response.ResponseStatus);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }


        public static bool AvaliarPetisco(String username, int idPetisco, int rating)
        {
            if (Debug)
            {
                Console.WriteLine("API DEBUG: User: " + username + " deu rating de " + rating + " ao pestico " + idPetisco);
                return true;
            }

            var client = new RestClient(HOST);
            var request = new RestRequest(URL_AVALIAR, Method.POST);
            request.AddUrlSegment("username", username);
            request.AddUrlSegment("id", idPetisco.ToString());
            request.RequestFormat = DataFormat.Json;
            string json = JsonConvert.SerializeObject(rating);
            request.AddParameter("text/json", json, ParameterType.RequestBody);

            var response = client.Execute(request);
            Console.WriteLine("API avaliou petisco: " + response.ResponseStatus);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }




        public static List<PetiscoModel> GetTendencias(String username)
        {
            Coordenadas.Locator.UpdateLocation();
            if (Debug)
            {
                Console.WriteLine("API DEBUG: tendencias!");
                Console.WriteLine("\tLAT: " + Coordenadas.Latitude);
                Console.WriteLine("\tLON: " + Coordenadas.Longitude);

                return ExemploListaPeticos();
            }

            double lat = Coordenadas.Latitude;
            double lon = Coordenadas.Longitude;

            var client = new RestClient(HOST);
            var request = new RestRequest(URL_TENDENCIAS, Method.GET);
            request.AddQueryParameter("latitude", lat.ToString().Replace(",","."));
            request.AddQueryParameter("longitude", lon.ToString().Replace(",", "."));

            var response = client.Execute(request);
            Console.WriteLine("API tendencias: " + response.Content);

            if (response.StatusCode == HttpStatusCode.OK)
			{
				var lista = JsonConvert.DeserializeObject<List<PetiscoModel>>(response.Content);
				CompletaURLFotografia(lista);
				return lista;
            }
            return null;
        }

        public static List<PetiscoModel> GetSugestoes(String username, String pesquisa, int? precoMin, int? precoMax, double? distancia)
        {
            Coordenadas.Locator.UpdateLocation();
            if (Debug)
            {
                Console.WriteLine("API DEBUG: sugestoes {0} {1} {2} {3} {4}",
                                  username, pesquisa, precoMin, precoMax, distancia);
                return ExemploListaPeticos();
            }

            double lat = Coordenadas.Latitude;
            double lon = Coordenadas.Longitude;

            var client = new RestClient(HOST);
            var request = new RestRequest(URL_SUGESTOES, Method.GET);
            request.AddQueryParameter("latitude", lat.ToString().Replace(",", "."));
            request.AddQueryParameter("longitude", lon.ToString().Replace(",", "."));
            request.AddQueryParameter("textoPesquisa", pesquisa);
            request.AddQueryParameter("utilizador", username);
            if (precoMax.HasValue) { request.AddQueryParameter("precoMax", precoMax.Value.ToString()); }
            if (precoMin.HasValue) { request.AddQueryParameter("precoMin", precoMin.Value.ToString()); }
            if (distancia.HasValue) {
                double dist = distancia.Value * 1000;
                request.AddQueryParameter("distancia", dist.ToString()); }

            var response = client.Execute(request);
            Console.WriteLine("API sugestoes: " + response.Content);

            if (response.StatusCode == HttpStatusCode.OK)
			{
				var lista = JsonConvert.DeserializeObject<List<PetiscoModel>>(response.Content);
				CompletaURLFotografia(lista);
				return lista;
            }
            return null;

        }

        public static async Task<Bitmap> GetImageBitmapFromUrlAsync(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = await webClient.DownloadDataTaskAsync(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = await BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        static void CompletaURLFotografia(List<PetiscoModel> lista) {
            foreach(PetiscoModel p in lista) {
                p.Fotografia = HOST + p.Fotografia;
            }
        } 

        static void CompletaURLFotografia2(List<RatingUserModel> lista) {
            foreach(RatingUserModel p in lista) {
                p.Fotografia = HOST + p.Fotografia;
            }
        } 


        // para debug sem o servidor
        private static List<PetiscoModel> ExemploListaPeticos() {


			PetiscoModel p1 = new PetiscoModel
			{
				Data = new DateTime(2007, 11, 11),
				NomePetisco = "Espetada de Chouriço",
				NomeEstabelecimento = "Casa da Avó",
				Preco = 5,
				Telefone = "911238790",
				NrEstrelas = 3,
				Latitude = 41.5503,
				Longitude = -8.4201,
				Fotografia = "http://images-cdn.impresa.pt/expresso/2016-07-15-petiscos.jpeg/original",
				Caracteristicas = new List<String> { "Batata doce", "Maça", "Tomate" }
			};

			PetiscoModel p2 = new PetiscoModel
			{
				Data = new DateTime(2015, 11, 11),
				NomePetisco = "Chouriça Assada",
				NomeEstabelecimento = "Tasquinha",
				Preco = 7,
				Telefone = "911238790",
				NrEstrelas = 2,
				Latitude = 41.5503,
				Longitude = -8.4201,
				Fotografia = "http://images-cdn.impresa.pt/bcbm/2016-04-26-img_0473.jpg/3x2/mw-1240",
				Caracteristicas = new List<String> { "Molho Cocktail", "Sal" }
			};

			PetiscoModel p3 = new PetiscoModel
			{
				Data = new DateTime(2010, 11, 11),
				NomePetisco = "Bruschetta",
				NomeEstabelecimento = "Canudo",
				Preco = 9,
				Telefone = "918762341",
				NrEstrelas = 4,
				Latitude = 41.5503,
				Longitude = -8.4201,
				Fotografia = "http://itanhanga.com.br/site/wp-content/uploads/2015/07/tapas-1600x667.jpg",
				Caracteristicas = new List<String> { "Tomate", "Cebola", "Azeitona" }
			};

			PetiscoModel p4 = new PetiscoModel
			{
				Data = new DateTime(2001, 11, 11),
				NomePetisco = "Bruschetta",
				NomeEstabelecimento = "Casa dos Santos",
				Preco = 25,
				Telefone = "918762341",
				NrEstrelas = 5,
				Latitude = 41.5503,
				Longitude = -8.4201,
				Fotografia = "http://www.telegraph.co.uk/content/dam/Food%20and%20drink/Spark/mahou-tapas-and-beer/tapas-Pintxos-xlarge.jpg",
				Caracteristicas = new List<String> { "Presunto", "Queijo", "Rúcula" }
			};


			PetiscoModel p5 = new PetiscoModel
			{
				Data = new DateTime(2000, 11, 11),
				NomePetisco = "Salada",
				NomeEstabelecimento = "Taberna Belga",
				Preco = 14,
				Telefone = "918162341",
				NrEstrelas = 5,
				Latitude = 41.5503,
				Longitude = -8.4201,
				Fotografia = "http://cdn-img.health.com/sites/default/files/styles/medium_16_9/public/updated_main_images/healthy-food-fill-raw.jpg",
				Caracteristicas = new List<String> { "alface", "limão" }
			};

            PetiscoModel p6 = new PetiscoModel
            {
                Data = new DateTime(2010, 11, 11),
                NomePetisco = "Bruschetta",
                NomeEstabelecimento = "Taberna Inglesa",
                Preco = 9,
                Telefone = "938762111",
                NrEstrelas = 2,
                Latitude = 41.5503,
                Longitude = -8.4201,
                Fotografia = "http://images.sweetauthoring.com/recipe/2673_593.jpg",
                Caracteristicas = new List<String> { "Tomate", "Couve" }
            };


			return new List<PetiscoModel> { p6, p3, p4, p1, p2, p5 };
			//return new List<PetiscoModel> { p6, p3, p4};
        }

	}
}
