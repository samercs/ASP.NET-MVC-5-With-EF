
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CrossOver.Core.Service;
using CrossOver.Models.TestUi;
using CrossOver.Service;


namespace CrossOver.Controllers
{
    public class TestUiController : ApplicationController
    {
        private readonly LanguageService _languageService;
        public TestUiController(IAppService appService) : base(appService)
        {
            _languageService = new LanguageService(DataContextFactory);
        }

        // GET: TestUi
        public ActionResult Index()
        {
            var model = new IndexViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(IndexViewModel model)
        {
            return Content(model.Name.ToString());
        }

        public async  Task<JsonResult> GetAutoComplete(string term = "")
        {
            
            var langauges = await _languageService.GetByQuery(term);
            return Json(langauges.Select(i => new
            {
                id = i.LanguageId,
                value = i.Name
            }// autocomplete jquery plugin accepts result in this format(id & value)
            ), JsonRequestBehavior.AllowGet);
        }


    }
}