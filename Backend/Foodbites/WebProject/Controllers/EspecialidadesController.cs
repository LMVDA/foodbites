using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Backoffice;
using Domain.DAOS.Interfaces;
using Domain.Petisco;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebProject.Controllers
{
    public class EspecialidadesController : Controller
	{
        private IHostingEnvironment environment;
        private BackofficeFacade backofficeFacade;

        public EspecialidadesController(IHostingEnvironment environment, BackofficeFacade backofficeFacade)
        {
            this.environment = environment;
            this.backofficeFacade = backofficeFacade;
        }

		public ActionResult Index()
		{
            List<Especialidade> es = backofficeFacade.GetAllEspecialidades();
			
			return View(es);
		}

		public ActionResult Create()
		{
            PopulateEstabelecimentosDropDownList();
            PopulatePetiscosDropDownList();
            var e = new Especialidade
            {
                Caracteristicas = new List<string>
				{
					"",
					"",
					"",
					"",
					""
                }
            };
			return View(e);
		}

		[HttpPost]
        public async Task<ActionResult> Create(Especialidade e, IFormFile file, IFormCollection collection)
		{
			int estabelecimentoId = int.Parse(collection["EstabelecimentoId"]);
			int petiscoId = int.Parse(collection["PetiscoId"]);

            if (ModelState.IsValid && estabelecimentoId > 0 && petiscoId > 0)
			{
                List<string> l = e.Caracteristicas.Where(c => !string.IsNullOrWhiteSpace(c)).ToList();
                e.Caracteristicas = l;

                var uploads = Path.Combine(environment.WebRootPath, "uploads");
				using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
				{
					await file.CopyToAsync(fileStream);
				}

                e.Fotografia = Path.Combine("/uploads/", file.FileName);

                backofficeFacade.AdicionaEspecialidade(petiscoId, e.Caracteristicas, e.Preco, estabelecimentoId, e.Fotografia);

				return RedirectToAction("Index");
			}

            PopulateEstabelecimentosDropDownList(estabelecimentoId);
            PopulatePetiscosDropDownList(petiscoId);
			return View(e);
		}

		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
                return BadRequest();
			}

            Especialidade especialidade = backofficeFacade.GetEspecialidade(id.Value);
            
			if (especialidade == null)
			{
                return NotFound();
			}

            for (int i = especialidade.Caracteristicas.Count; i < 5; i++) {
                especialidade.Caracteristicas.Add("");
            }

            PopulateEstabelecimentosDropDownList(especialidade.Estabelecimento.Id);
            PopulatePetiscosDropDownList(especialidade.Id);

			return View(especialidade);
		}

		[HttpPost]
		public async Task<ActionResult> Edit(int? id, IFormCollection collection)
		{
			int estabelecimentoId = int.Parse(collection["EstabelecimentoId"]);
			int petiscoId = int.Parse(collection["PetiscoId"]);

            if (id == null || estabelecimentoId <= 0 || petiscoId <= 0)
			{
                return BadRequest();
			}

            Especialidade e = backofficeFacade.GetEspecialidade(id.Value);

            if (await TryUpdateModelAsync(e))
			{
				List<string> l = e.Caracteristicas.Where(c => !string.IsNullOrWhiteSpace(c)).ToList();
				e.Caracteristicas = l;

                if (collection.Files.Count > 0 && !string.IsNullOrWhiteSpace(collection.Files[0].FileName))
                {
					var uploads = Path.Combine(environment.WebRootPath, "uploads");
					using (var fileStream = new FileStream(Path.Combine(uploads, collection.Files[0].FileName), FileMode.Create))
					{
						await collection.Files[0].CopyToAsync(fileStream);
					}

					e.Fotografia = Path.Combine("/uploads/", collection.Files[0].FileName);
                }

                backofficeFacade.AtualizaEspecialidade(e.IdEspecialidade, petiscoId, e.Caracteristicas, e.Preco, estabelecimentoId, e.Fotografia);

				return RedirectToAction("Index");
			}
			
            PopulateEstabelecimentosDropDownList(e.Estabelecimento.Id);
            PopulatePetiscosDropDownList(e.Id);
			
            return View(e);
		}

		public ActionResult Desativar(int id)
		{
            backofficeFacade.DesactivaEspecialidade(id);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult Desativar(int id, FormCollection collection)
		{
			backofficeFacade.DesactivaEspecialidade(id);
			return RedirectToAction("Index");
		}

		private void PopulateEstabelecimentosDropDownList(object selectedEstabelecimento = null)
		{
            var estabelecimentos = backofficeFacade.GetAllEstabelecimentos();
			
			ViewBag.EstabelecimentoId = new SelectList(estabelecimentos, "Id", "Nome", selectedEstabelecimento);
		}

		private void PopulatePetiscosDropDownList(object selectedPetisco = null)
		{
            var petiscos = backofficeFacade.GetAllPetiscos();

			ViewBag.PetiscoId = new SelectList(petiscos, "Id", "Nome", selectedPetisco);
		}
	}
}