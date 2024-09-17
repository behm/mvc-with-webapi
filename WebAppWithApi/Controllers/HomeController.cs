using System;
using System.Web.Mvc;
using WebAppWithApi.Data;

namespace WebAppWithApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAppInfoRepo _repo;

        public HomeController(IAppInfoRepo repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            var app = _repo.GetAppInfo(Guid.NewGuid());

            ViewBag.Title = $"Home Page";
            ViewBag.CurrentUser = $"{app.FirstName} {app.LastName} - {app.AppInfoId}";

            return View();
        }
    }
}
