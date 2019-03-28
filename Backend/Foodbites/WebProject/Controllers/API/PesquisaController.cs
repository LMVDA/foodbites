using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Geolocalizacao;
using Domain.Pesquisa;
using Domain.Sugestao;
using Domain.Utilizador;
using Microsoft.AspNetCore.Mvc;

namespace WebProject.Controllers.API
{

    [Route("api/[controller]")]
    public class PesquisaController : Controller
    {
		private PesquisaFacade pesquisaFacade;
        private UtilizadorFacade utilizadorFacade;

        public PesquisaController(PesquisaFacade pesquisaFacade, UtilizadorFacade utilizadorFacade)
        {
            this.pesquisaFacade = pesquisaFacade;
            this.utilizadorFacade = utilizadorFacade;
        }

		[HttpGet("sugestoes")]
        public IActionResult GetSugestoes(
            [Required] string utilizador,
            [Required] string textoPesquisa,
			[Required] double latitude,
            [Required] double longitude,
			double? precoMax,
			double? precoMin,
			double distancia = 20000)
		{
            var foodbiter = utilizadorFacade.GetUtilizador(utilizador);

            if (foodbiter == null) return BadRequest();

            var localizacao = new Localizacao(latitude, longitude);

            return Ok(pesquisaFacade.Pesquisa(foodbiter, textoPesquisa, localizacao, precoMin, precoMax, distancia));
		}

		[HttpGet("tendencias")]
		public List<Sugestao> GetTendencias(
			[Required] double latitude,
			[Required] double longitude)
		{
            var localizacao = new Localizacao(latitude, longitude);

            return pesquisaFacade.GetTendencias(localizacao);
		}
    }
}
