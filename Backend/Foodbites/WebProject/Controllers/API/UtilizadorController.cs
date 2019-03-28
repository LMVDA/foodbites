using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Avaliacao;
using Domain.Sugestao;
using Domain.Utilizador;
using Microsoft.AspNetCore.Mvc;
using WebProject.Model;

namespace WebProject.Controllers.API
{
    [Route("api/[controller]")]
    public class UtilizadorController : Controller
    {
		private UtilizadorFacade utilizadorFacade;
        private AvaliacaoFacade avaliacaoFacade;

        public UtilizadorController(UtilizadorFacade utilizadorFacade, AvaliacaoFacade avaliacaoFacade)
		{
            this.utilizadorFacade = utilizadorFacade;
            this.avaliacaoFacade = avaliacaoFacade;
		}

		[HttpPost]
		public IActionResult Registar([FromBody]Foodbiter foodbiter)
		{
			var utilizador = utilizadorFacade.GetUtilizador(foodbiter.Username);

			if (utilizador != null) 
			{
				return BadRequest();
			}

            utilizadorFacade.Registar(foodbiter.Username, foodbiter);

            return Ok();
		}

		[HttpGet("{username}")]
		public Foodbiter GetFoodbiter([Required] string username)
		{
            var utilizador = utilizadorFacade.GetUtilizador(username);
            return utilizador;
		}

		[HttpPut("{username}")]
		public void AtualizarPerfil([FromBody]Foodbiter foodbiter)
		{
            utilizadorFacade.AtualizarPerfil(foodbiter.Username, foodbiter.Nome, foodbiter.Email, foodbiter.DataNascimento, foodbiter.Password);
		}

        [HttpGet("{username}/avaliacoes")]
        public List<AvaliacaoDetalhada> GetHistoricoAvaliacoes([Required]string username)
        {
            return utilizadorFacade.GetAvaliacoes(username);
        }

        [HttpPost("{username}/avaliacao/{id}")]
		public void Avaliar(
			string username,
            int id,
			[FromBody] int avaliacao)
		{
            avaliacaoFacade.Avaliar(username, id, avaliacao);
		}

		[HttpGet("{username}/historico")]
		public List<HistoricoPesquisa> GetHistorico([Required]string username)
		{
            return utilizadorFacade.GetHistorico(username);
		}

		[HttpPost("{username}/historico")]
		public void RegistaHistorico([Required]string username, [FromBody]int id)
		{
            utilizadorFacade.RegistaHistorico(username, id);
		}
    }
}
