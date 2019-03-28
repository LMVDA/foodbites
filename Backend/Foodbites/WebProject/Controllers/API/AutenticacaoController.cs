using System;
using Domain.Utilizador;
using Microsoft.AspNetCore.Mvc;
using WebProject.Model;

namespace WebProject.Controllers.API
{
    [Route("api/[controller]")]
    public class AutenticacaoController : Controller
    {
		private UtilizadorFacade utilizadorFacade;

		public AutenticacaoController(UtilizadorFacade utilizadorFacade)
		{
			this.utilizadorFacade = utilizadorFacade;
		}

        [HttpPost("login")]
        public IActionResult Login([FromBody]DadosLogin dadosLogin)
		{
            if (utilizadorFacade.AutenticarUtilizador(dadosLogin.Username, dadosLogin.Password)) 
            {
                return Ok(utilizadorFacade.GetUtilizador(dadosLogin.Username));
            }
            else
            {
                return BadRequest();
            }
		}
    }
}
