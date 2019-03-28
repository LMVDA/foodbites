using System;
using System.Collections.Generic;
using Domain.Geolocalizacao;
using Domain.Pesquisa;
using Domain.Petisco;
using System.Linq;
using Domain.Utilizador;
using Domain.Avaliacao;

namespace Domain.Sugestao
{
    public class MotorSugestoes
    {
        public List<Sugestao> CalculaSugestoes(Foodbiter utilizador, Pesquisa.Pesquisa pesquisa, Localizacao localizacaoAtual, double? precoMin, double? precoMax, double distancia, List<Especialidade> especialidades)
        {
            var dataAgora = DateTime.Now;

            var especialidadesTratadas =
                especialidades
                    // filtro das horas de abertura
                    .Where(e => e.Estabelecimento.Aberto(dataAgora))
                    // filtro da distancia
                    .Where(e => e.Estabelecimento.Localizacao.DistanciaA(localizacaoAtual) < distancia)
                    // filtro do preço
                    .Where(e => e.Preco >= precoMin.GetValueOrDefault(0) && e.Preco <= precoMax.GetValueOrDefault(e.Preco))
                    // filtro das despreferencias
                    .Where(e => pesquisa.Despreferencias.Select(c => c.ToLower()).Intersect(e.Caracteristicas.Select(c => c.ToLower())).Count() == 0)
                    // filtro das avaliacoes < 3
                    .Where(e => utilizador.Avaliacoes.All(a => a.AvaliacaoFraca(e.IdEspecialidade) == false));

            especialidadesTratadas = AplicaCorrespondencia(especialidadesTratadas, utilizador.Preferencias, pesquisa.Preferencias);

            var sugestoes =
                especialidadesTratadas
                    // Limitar a 3
                    .Take(3)
                    // Ordenar por distancia 
                    .OrderBy(e => e.Estabelecimento.Localizacao.DistanciaA(localizacaoAtual))
                    // Ordenar por preco
                    .ThenByDescending(e => e.Preco)
                    // converter em sugestoes
                    .Select(e => new Sugestao(e))
                    .ToList();

            return sugestoes;
        }

        private IEnumerable<Especialidade> AplicaCorrespondencia(IEnumerable<Especialidade> especialidadesTratadas, List<Preferencia> preferencias, List<string> preferenciasPesquisa)
        {
            if (especialidadesTratadas == null || especialidadesTratadas.Count() == 0) return new List<Especialidade>();
            if (preferencias == null || preferenciasPesquisa == null) return especialidadesTratadas;

            var petisco = preferencias.FirstOrDefault(p => p.Petisco.Id == especialidadesTratadas.FirstOrDefault()?.Id);
            var caracteristicaNVezes = GetCaracteristicasNumeroVezes(petisco);

            return especialidadesTratadas
                    // ordernar primeiro as especialidades que têm preferencias de pesquisa. Sub conjunto
                    .OrderByDescending(e => preferenciasPesquisa.Select(c => c.ToLower()).Intersect(e.Caracteristicas.Select(c => c.ToLower())).Count())
				    // ordernar depois por gostos do utilizador
                    .ThenByDescending(e => GetValorPreferencia(caracteristicaNVezes, e));
        }

        private int GetValorPreferencia(Dictionary<string, int> caracteristicaPorVezes, Especialidade especialidade)
        {
            int total = 0;

            foreach (var caracteristica in especialidade.Caracteristicas)
            {
                int nrVezes = 0;

                caracteristicaPorVezes.TryGetValue(caracteristica, out nrVezes);

                total += nrVezes;
            }

            return total;
        }

        private Dictionary<string, int> GetCaracteristicasNumeroVezes(Preferencia preferencia)
        {
            if (preferencia == null) return new Dictionary<string, int>();

            var caracteristicas = new List<Caracteristica>();
            caracteristicas.AddRange(preferencia.Caracteristicas);

            var auxiliarNumeroVezes = new Dictionary<string, int>();

            foreach (var caracteristica in caracteristicas)
            {
                int nrVezes = 0;

                auxiliarNumeroVezes.TryGetValue(caracteristica.Nome, out nrVezes);

                if (DataRecente(caracteristica.Data))
                {
                    auxiliarNumeroVezes.Add(caracteristica.Nome, nrVezes + 4);
                }
                else if (DataMedia(caracteristica.Data))
                {
                    auxiliarNumeroVezes.Add(caracteristica.Nome, nrVezes + 1);
                }
            }

            return auxiliarNumeroVezes;
        }

        private bool DataRecente(DateTime data)
        {
            return data <= DateTime.UtcNow.AddMonths(-1);
        }

        private bool DataMedia(DateTime data)
        {
            return data <= DateTime.UtcNow.AddMonths(-2);
        }
    }
}
