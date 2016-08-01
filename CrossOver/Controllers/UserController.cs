using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CrossOver.Core.Service;
using CrossOver.Entity;
using CrossOver.Models;
using CrossOver.Models.User;
using CrossOver.Service;
using CrossOver.Service.Identity;
using Microsoft.AspNet.Identity;
using IndexViewModel = CrossOver.Models.User.IndexViewModel;

namespace CrossOver.Controllers
{
    [Authorize]
    public class UserController : ApplicationController
    {
        private readonly IAuthService _authService;
        private readonly UserService _userService;
        private readonly StockCodeService _stockCodeService;
        private readonly UserManager _userManager;
        private readonly ApiService _apiService;
        public UserController(IAppService appService) : base(appService)
        {
            _authService = appService.AuthService;
            _userService = new UserService(DataContextFactory);
            _stockCodeService = new StockCodeService(DataContextFactory);
            _userManager = new UserManager(DataContextFactory);
            _apiService = new ApiService();
        }

        public async Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {

                var user = await _authService.CurrentUser();
                user.StockCodes = (ICollection<StockCode>)await _userService.GetUserStock(user.Id);
                var webApiUrl = Url.RouteUrl(
                    "DefaultApi",
                    new { httproute = "", controller = "Stock" },
                    Request.Url.Scheme
                );

                var codesParameter = string.Empty;
                foreach (var stockCode in user.StockCodes)
                {
                    if (string.IsNullOrEmpty(codesParameter))
                    {
                        codesParameter += $"?codes={stockCode.StockCodeId}";
                    }
                    else
                    {
                        codesParameter += $"&codes={stockCode.StockCodeId}";
                    }
                }
                webApiUrl += "/" + codesParameter;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

                var result = client
                    .GetAsync(webApiUrl)
                    .Result
                    .Content.ReadAsAsync<IList<StockResponse>>().Result;

                var displayStock = new List<StockViewModel>();
                foreach (var stockCode in user.StockCodes)
                {
                    displayStock.Add(new StockViewModel()
                    {
                        StockCode = stockCode,
                        Prices = result.First(i => i.StockCodeId == stockCode.StockCodeId).Prices
                    });
                }

                var model = new IndexViewModel()
                {
                    User = user,
                    UserStockList = displayStock

                };
                this.HttpContext.Response.AddHeader("refresh", "10; url=" + Url.Action("Index"));
                return View(model);
            }
            

        }

        public async Task<ActionResult> AddStock()
        {
            var user = await _authService.CurrentUser();
            user.StockCodes = (ICollection<StockCode>)await _userService.GetUserStock(user.Id);
            var model = new AddStockViewModel()
            {
                UserStockCodes = user.StockCodes.ToList(),
                AllStockCodes = (await _stockCodeService.GetAll())
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddStock(AddStockViewModel model, IList<string> stockCodes)
        {
            var user = await _authService.CurrentUser();
            var allStockCodes = await _stockCodeService.GetAll();
            if (stockCodes != null)
            {
                var selectedStockCode = stockCodes.Select(int.Parse).ToList();
                await _userService.AddStock(user.Id, selectedStockCode);
                SetStatusMessage("Stock codes has been saved successfully.");
                return RedirectToAction("Index");
            }
            SetStatusMessage("Select at least one stock code.");
            return RedirectToAction("AddStock", user);

        }

        public ActionResult SetPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _authService.CurrentUser();
                var result = await _userManager.ChangePasswordAsync(user.Id, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    SetStatusMessage("password has been update successfully.");
                    return RedirectToAction("Index", "User");
                }
                SetStatusMessage("Error: can't change your password");
                return View(model);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}