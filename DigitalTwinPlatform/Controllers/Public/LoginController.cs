using DigitalTwinPlatform.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwinPlatform.Controllers.Public
{
    public class LoginController : Controller
    {
        private readonly HttpClient client = new HttpClient();
        private readonly string APIGateWayUri = Environment.GetEnvironmentVariable("APIGatewayUri");


        [HttpGet]
        [Route("public/login")]
        public IActionResult Index()
        {
            HttpContext.Session.Clear();

            LoginViewModel model = new LoginViewModel
            {
                User = HttpContext.Session.GetString(LoginViewModel.SessionKeyUser),
                Password= HttpContext.Session.GetString(LoginViewModel.SessionKeyPassword),
            };
            return View(model);
        }

        [HttpPost]
        [Route("public/login")]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            HttpContext.Session.SetString(LoginViewModel.SessionKeyUser, model.User);
            HttpContext.Session.SetString(LoginViewModel.SessionKeyPassword, model.Password);
            var parameters = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "username", model.User },
                    {"password", model.Password }
                };

            string queryString = string.Join("&", parameters
                .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value.ToString())}"));

            HttpResponseMessage response = await client.GetAsync(APIGateWayUri +"Authentication/getToken?" + queryString);
            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();
                Guid token = Newtonsoft.Json.JsonConvert.DeserializeObject<Guid>(jsonData);
                HttpContext.Session.SetString(LoginViewModel.SessionKeyToken, token.ToString());
                return RedirectToAction("Index", "DT");

            }
            else
            {
                TempData["ErrorMessage"] = "El usuario o contraseña son incorrectos";
                return View(model);
            }
        }
    }
}

//namespace ForgeSample.Controllers.Public
//{
//    public class LoginController : Controller
//    {
//        [HttpGet]
//        [Route("public/login")]
//        public IActionResult Index(Guid id)
//        {
//            LoginViewModel model = new LoginViewModel
//            {
//                LinkId = id,
//                Email = HttpContext.Session.GetString(LoginViewModel.SessionKeyEmail)
//            };

//            return View(model);
//        }

//        [HttpPost]
//        [Route("public/login")]
//        public IActionResult Index(LoginViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }

//            HttpContext.Session.SetString(LoginViewModel.SessionKeyEmail, model.Email);
//            return RedirectToAction("Index", "Viewer", new { Id = model.LinkId });
//        }
//    }
//}