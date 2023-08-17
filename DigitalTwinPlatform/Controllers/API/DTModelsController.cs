using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DigitalTwinPlatform.DBModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalTwinPlatform.Controllers.API
{

    public class DTModelsController : ControllerBase
    {

        private readonly HttpClient client = new HttpClient();
        private readonly string APIGateWayUri = Environment.GetEnvironmentVariable("APIGatewayUri");
        private readonly DTModelsContext ctx;

        public DTModelsController(DTModelsContext ctx)
        {
            this.ctx = ctx;
        }

        public List<DBModels.Models> getDBModels(string username)
        {
            return this.ctx.Models.Where(m => m.username == username).ToList();
        }

        [HttpGet]
        [Route("getUseCases")]
        public List<UseCase> GetUseCases(string modelId)
        {
            Guid Id = Guid.Parse(modelId);
            return this.ctx.UseCase.Where(uc => uc.modelId == Id).ToList();
        }

        [HttpGet]
        [Route("getUseCase")]
        public async Task<UseCase> GetUseCase(string id)
        {
            Guid Id = Guid.Parse(id);
            return await this.ctx.UseCase.FindAsync(Id);
        }

        [HttpGet]
        [Route("getPumpData")]
        public async Task<List<dynamic>> getPumpData(double nElements)
        {
            var parameters = new System.Collections.Generic.Dictionary<string, double>
                {
                    { "nElements", nElements }
                };

            string queryString = string.Join("&", parameters
                .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value.ToString())}"));

            HttpResponseMessage response = await client.GetAsync(APIGateWayUri + "WaterPump/getData?" + queryString);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            string jsonData = await response.Content.ReadAsStringAsync();
            List<dynamic> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<dynamic>>(jsonData);
            return data;
        }

        [HttpGet]
        [Route("getPredictionData")]
        public async Task<List<dynamic>> getPredictionData(double nElements)
        {
           var parameters = new System.Collections.Generic.Dictionary<string, double>
                {
                    { "nElements", nElements }
                };

            string queryString = string.Join("&", parameters
                .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value.ToString())}"));

            HttpResponseMessage response = await client.GetAsync(APIGateWayUri + "WaterPumpPredictions/getData?" + queryString);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            string jsonData = await response.Content.ReadAsStringAsync();
            List<dynamic> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<dynamic>>(jsonData);
            return data;
        }

        [HttpGet]
        [Route("getReactorData")]
        public async Task<List<dynamic>> getReactorData(double nElements)
        {
            var parameters = new System.Collections.Generic.Dictionary<string, double>
                {
                    { "nElements", nElements }
                };

            string queryString = string.Join("&", parameters
                .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value.ToString())}"));

            HttpResponseMessage response = await client.GetAsync(APIGateWayUri + "BiologicalReactor/getData?" + queryString);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            string jsonData = await response.Content.ReadAsStringAsync();
            List<dynamic> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<dynamic>>(jsonData);

            return data;
        }

    }
}
