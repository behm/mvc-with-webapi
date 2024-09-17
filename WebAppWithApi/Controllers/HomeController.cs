using Microsoft.Extensions.Logging;
using System;
using System.Web.Mvc;
using WebAppWithApi.Data;

namespace WebAppWithApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAppInfoRepo _repo;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IAppInfoRepo repo, ILogger<HomeController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public ActionResult Index()
        {
            var app = _repo.GetAppInfo(Guid.NewGuid());

            ViewBag.Title = $"Home Page";
            ViewBag.CurrentUser = $"{app.FirstName} {app.LastName} - {app.AppInfoId}";

            _logger.LogInformation("App Info - {firstName}, {lastName}, {appInfoId}", app.FirstName, app.LastName, app.AppInfoId);

            return View();
        }
    }
}
