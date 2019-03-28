using System.Threading.Tasks;
using Domain.Backoffice;
using Domain.Petisco;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebProject.Controllers
{
    public class PetiscosController : Controller
	{
        private BackofficeFacade backofficeFacade;

		public PetiscosController(BackofficeFacade backofficeFacade)
		{
			this.backofficeFacade = backofficeFacade;
		}

        public ActionResult Index()
        {
            var petiscos = backofficeFacade.GetAllPetiscos();

			return View(petiscos);
        }

		public ActionResult Create()
        {
            return View(new Petisco());
        } 

        [HttpPost]
        public ActionResult Create(Petisco petisco)
        {
			if (ModelState.IsValid)
			{
                backofficeFacade.AdicionaPetisco(petisco.Nome);

				return RedirectToAction("Index");
			}

            return View(petisco);

        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Petisco petisco = backofficeFacade.GetPetisco(id.Value);

            if (petisco == null)
            {
                return NotFound();
            }

            return View(petisco);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int? id, IFormCollection collection)
		{
			if (id == null)
			{
                return BadRequest();
			}

            Petisco petisco = backofficeFacade.GetPetisco(id.Value);

            if (await TryUpdateModelAsync(petisco))
			{
                backofficeFacade.AtualizaPetisco(petisco.Id, petisco.Nome);

				return RedirectToAction("Index");
			}

			return View(petisco);
        }
    }
}