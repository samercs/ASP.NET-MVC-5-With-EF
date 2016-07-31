using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrossOver.Core.Base;
using CrossOver.Core.Service;
using CrossOver.Data;

namespace CrossOver.Controllers
{
    public class ApplicationController : Controller
    {
        protected readonly IDataContextFactory DataContextFactory;
        protected readonly IAuthService AuthService;
        protected readonly ICookieService CookieService;

        public ApplicationController(IAppService appService)
        {
            DataContextFactory = appService.DataContextFactory;
            AuthService = appService.AuthService;
            CookieService = appService.CookieService;
        }
        // GET: Application
        public ActionResult Index()
        {
            return View();
        }

        public void SetStatusMessage(string message, StatusMessageType statusMessageType = StatusMessageType.Success)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            TempData["StatusMessage"] = new StatusMessage(message, statusMessageType);
        }

        public StatusMessage GetStatusMessage()
        {
            return TempData["StatusMessage"] as StatusMessage;
        }
    }
}