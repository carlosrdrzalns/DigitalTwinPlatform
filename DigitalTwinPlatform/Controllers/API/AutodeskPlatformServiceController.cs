using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalTwinPlatform.Controllers.API
{

    public class AutodeskPlatformServiceController : ControllerBase
    {
        private readonly HttpClient client = new HttpClient();
        private readonly string APIGateWayUri = Environment.GetEnvironmentVariable("APIGatewayUri");

        [HttpGet]
        [Route("APS/getToken")]
        public async Task<dynamic> getPublicAsync()
        {

            HttpResponseMessage response = await client.GetAsync(APIGateWayUri + "APS/getToken");
            if (!response.IsSuccessStatusCode)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        return Unauthorized();
                    case HttpStatusCode.BadRequest:
                        return BadRequest();
                    default:
                        return NotFound();
                }
            }

            string jsonData = await response.Content.ReadAsStringAsync();
            Autodesk.Forge.Model.DynamicJsonResponse data = Newtonsoft.Json.JsonConvert.DeserializeObject<Autodesk.Forge.Model.DynamicJsonResponse>(jsonData);
            return data.Dictionary;

        }

    }
}
