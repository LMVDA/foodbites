using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domain.Backoffice;
using Domain.DAOS.Interfaces;
using Domain.Petisco;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebProject.Controllers
{
    public class EstabelecimentosController : Controller
    {

        private BackofficeFacade backofficeFacade;

        public EstabelecimentosController(BackofficeFacade backofficeFacade)
        {
            this.backofficeFacade = backofficeFacade;
        }

		public ActionResult Index()
		{
            List<Estabelecimento> estabelecimentos = backofficeFacade.GetAllEstabelecimentos();

            return View(estabelecimentos);
		}

		public ActionResult Create()
		{
            var estabelecimento = new Estabelecimento
            {
                Horarios = new List<HorarioFuncionamento>{
					new HorarioFuncionamento{ Dia = DayOfWeek.Monday },
                    new HorarioFuncionamento{ Dia = DayOfWeek.Tuesday },
                    new HorarioFuncionamento{ Dia = DayOfWeek.Wednesday },
                    new HorarioFuncionamento{ Dia = DayOfWeek.Thursday },
                    new HorarioFuncionamento{ Dia = DayOfWeek.Friday },
					new HorarioFuncionamento{ Dia = DayOfWeek.Saturday },
                    new HorarioFuncionamento{ Dia = DayOfWeek.Sunday }
                },

				Criticas = new List<string>
				{
					"",
					"",
					"",
					"",
					""
				}
            };

			return View(estabelecimento);
		}

		[HttpPost]
		public ActionResult Create(Estabelecimento estabelecimento, IFormCollection collection)
		{
			if (ModelState.IsValid)
			{
				List<HorarioFuncionamento> horarios = ExtraiHorarios(estabelecimento.Horarios, collection);

				List<string> l = estabelecimento.Criticas.Where(c => !string.IsNullOrWhiteSpace(c)).ToList();
				estabelecimento.Criticas = l;

				backofficeFacade.AdicionaEstabelecimento(estabelecimento.Nome, estabelecimento.Telefone, horarios, estabelecimento.Localizacao, estabelecimento.Criticas);

				return RedirectToAction("Index");
			}

			return View(estabelecimento);

		}

        public ActionResult Edit(int? id)
		{
			if (id == null)
			{
                return BadRequest();
			}

            Estabelecimento estabelecimento = backofficeFacade.GetEstabelecimento(id.Value);

            ViewBag.HorariosEscolhidos = GetHorariosEscolhidos(estabelecimento);

            for (int i = estabelecimento.Criticas.Count; i < 5; i++)
			{
                estabelecimento.Criticas.Add("");
			}

            if (estabelecimento == null)
			{
                return NotFound();
			}

			return View(estabelecimento);
		}

        [HttpPost]
		public async Task<ActionResult> Edit(int? id, IFormCollection collection)
		{
			if (id == null)
			{
                return BadRequest();
			}

            Estabelecimento estabelecimento = backofficeFacade.GetEstabelecimento(id.Value);

			if (await TryUpdateModelAsync(estabelecimento))
			{
                List<HorarioFuncionamento> horarios = ExtraiHorarios(estabelecimento.Horarios, collection);

                List<string> l = estabelecimento.Criticas.Where(c => !string.IsNullOrWhiteSpace(c)).ToList();
                estabelecimento.Criticas = l;

                backofficeFacade.AtualizaEstabelecimento(estabelecimento.Id, estabelecimento.Nome, estabelecimento.Telefone, horarios, estabelecimento.Localizacao, estabelecimento.Criticas);

				return RedirectToAction("Index");
			}

			return View(estabelecimento);
		}

		public ActionResult Desativar(int id)
		{
            backofficeFacade.DesactivaEstabelecimento(id);

			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult Desativar(int id, FormCollection collection)
		{
			backofficeFacade.DesactivaEstabelecimento(id);

			return RedirectToAction("Index");
		}

        private List<HorarioFuncionamento> ExtraiHorarios(List<HorarioFuncionamento> todosHorarios, IFormCollection collection)
        {
            List<HorarioFuncionamento> horarios = new List<HorarioFuncionamento>();

            for (int dia = 0; dia < 7; dia++) 
            {
                Microsoft.Extensions.Primitives.StringValues aberto;

                if (collection.TryGetValue($"Horarios[{dia}].Aberto", out aberto)) 
                {
                    if (aberto[0].Equals("true")) 
                    {
                        horarios.Add(todosHorarios.First(h => h.Dia == (DayOfWeek)dia));
                    }
                }
            }

            return horarios;
        }

        private Dictionary<DayOfWeek, (HorarioFuncionamento, bool)> GetHorariosEscolhidos(Estabelecimento estabelecimento)
        {
            var horariosEscolhidos = new Dictionary<DayOfWeek, (HorarioFuncionamento, bool)>();

            if (estabelecimento.Horarios == null) return horariosEscolhidos;
            
            for (int i = 0; i <= (int)DayOfWeek.Saturday; i++)
            {
				var dayOfWeek = (DayOfWeek)i;

                if (estabelecimento.Horarios == null)
                {
                    horariosEscolhidos.Add(dayOfWeek, (new HorarioFuncionamento(dayOfWeek), false));
                }
                else 
                {
                    var escolhido = estabelecimento.Horarios.Any(h => h.Dia == dayOfWeek);
                    var horario = escolhido ? estabelecimento.Horarios.First(h => h.Dia == dayOfWeek) : new HorarioFuncionamento(dayOfWeek);

					horariosEscolhidos.Add(dayOfWeek, (horario, escolhido));
                }
            }

            return horariosEscolhidos;
        }
    }
}
