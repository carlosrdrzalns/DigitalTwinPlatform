using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DigitalTwinPlatform.DBModels;
using DigitalTwinPlatform.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalTwinPlatform.Controllers.Public
{
    public class DTController : Controller
    {
        private readonly HttpClient client = new HttpClient();
        private readonly string APIGateWayUri = Environment.GetEnvironmentVariable("APIGatewayUri");
        private readonly DTModelsContext ctx;

        public DTController(DTModelsContext ctx)
        {
            this.ctx = ctx;
        }


        [HttpGet]
        [Route("public/DT")]
        public async Task<IActionResult> Index()
        {
            DTModels model = new DTModels
            {
                token = Guid.Parse(HttpContext.Session.GetString(DTModels.SessionKeyToken)),
                User = HttpContext.Session.GetString(DTModels.SessionKeyUser),
                models = null
            };
            var parameters = new System.Collections.Generic.Dictionary<string, Guid>
                {
                    { "token", model.token }
                };

            string queryString = string.Join("&", parameters
                .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value.ToString())}"));

            HttpResponseMessage response = await client.GetAsync(APIGateWayUri + "Authentication/checkToken?" + queryString);
            if (!response.IsSuccessStatusCode)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        return RedirectToAction("Index", "Login");
                    case HttpStatusCode.BadRequest:
                        return BadRequest();
                    default:
                        return NotFound();
                }
            }

            List<DBModels.Models> models = this.ctx.Models.Where(m => m.username == model.User).ToList();
            model.models = models;
            return View(model);
        }
    }
}
