using System;
using System.Collections.Generic;
using Fastenshtein;
using System.Linq;

namespace Domain.Pesquisa
{
    public class ParserPesquisa
    {
        private Levenshtein algoritmoComparacao;

        public ParserPesquisa()
        {
        }

        public Pesquisa ParsePesquisa(string frase, List<string> nomesPetiscos)
        {
            var pesquisa = ParsePesquisaPorTratar(frase);

            pesquisa.Petisco = GetPalavraSimilar(pesquisa.Petisco, nomesPetiscos);

            return pesquisa;
        }

        private Pesquisa ParsePesquisaPorTratar(string frase)
		{
			string[] separadorPrincipalPreferencias = { "com", "c/" };
			string[] separadorPrincipalDespreferencias = { "sem", "s/" };

			List<string> preferencias = new List<string>();
			List<string> despreferencias = new List<string>();
			string petisco = string.Empty;

            if (string.IsNullOrWhiteSpace(frase)) return new Pesquisa(petisco, preferencias, despreferencias);

			List<string> preferenciasATratar = new List<string>();
			List<string> despreferenciasATratar = new List<string>();


			string[] possiveisPreferencias = frase.Split(separadorPrincipalPreferencias, System.StringSplitOptions.RemoveEmptyEntries);
			string[] petiscoEDespreferencias = possiveisPreferencias[0].Split(separadorPrincipalDespreferencias, System.StringSplitOptions.RemoveEmptyEntries);

            petisco = petiscoEDespreferencias[0].Trim();

			for (int i = 1; i < petiscoEDespreferencias.Length; i++)
			{
				despreferenciasATratar.Add(petiscoEDespreferencias[i]);
			}

			for (int i = 1; i < possiveisPreferencias.Length; i++)
			{
				string[] possiveisDespreferencias = possiveisPreferencias[i].Split(separadorPrincipalDespreferencias, System.StringSplitOptions.RemoveEmptyEntries);

				despreferenciasATratar.AddRange(possiveisDespreferencias.Skip(1));

				preferenciasATratar.Add(possiveisDespreferencias[0]);
			}

			preferencias = TratarPreferencias(preferenciasATratar);
			despreferencias = TratarDespreferencias(despreferenciasATratar);

			return new Pesquisa(petisco, preferencias, despreferencias);
		}

		private List<string> TratarPreferencias(List<string> preferenciasATratar)
		{
			string[] separadorSecundarioPreferencias = { " e ", ",", "com", "c/" };
			List<string> preferencias = new List<string>();

			foreach (string preferenciaNaoTratada in preferenciasATratar)
			{
				string[] preferenciasTratadas = preferenciaNaoTratada.Split(separadorSecundarioPreferencias, System.StringSplitOptions.RemoveEmptyEntries);

				// remover espaços do inicio e fim da string " canela " => "canela"
				var preferenciasTratadasSemEspacos = preferenciasTratadas.Select(s => s.Trim());

				preferencias.AddRange(preferenciasTratadasSemEspacos);
			}

			return preferencias;
		}

		private List<string> TratarDespreferencias(List<string> despreferenciasATratar)
		{
			string[] separadorSecundarioDespreferencias = { " e ", ",", "nem", "s/" };
			List<string> despreferencias = new List<string>();

			foreach (string despreferenciaNaoTratada in despreferenciasATratar)
			{
				string[] despreferenciasTratadas = despreferenciaNaoTratada.Split(separadorSecundarioDespreferencias, System.StringSplitOptions.RemoveEmptyEntries);

				// remover espaços do inicio e fim da string " canela " => "canela"
				var despreferenciasTratadasSemEspacos = despreferenciasTratadas.Select(s => s.Trim());

				despreferencias.AddRange(despreferenciasTratadasSemEspacos);
			}

			return despreferencias;
		}

        private string GetPalavraSimilar(string palavra, List<string> palavras)
		{
            algoritmoComparacao = new Levenshtein(palavra.ToLower());

			// [ "sopa", "cardo", "creme" ], "caldo" => [ ("sopa", 5), ("cardo", 1), ("creme", 4) ]
            var palavrasComSimilaridades = palavras.Select(w => (palavra: w, distancia: algoritmoComparacao.Distance(w.ToLower())));
			// [ ("sopa", 5), ("cardo", 1), ("creme", 4) ] => [ ("cardo", 1), ("creme", 4), ("sopa", 5) ]
			var palavrasComSimilaridadesOrdenadas = palavrasComSimilaridades.OrderBy(tuple => tuple.distancia);

            var PalavraComSimilaridade = palavrasComSimilaridadesOrdenadas.FirstOrDefault();

            return PalavraComSimilaridade.palavra;
		}
    }
}
