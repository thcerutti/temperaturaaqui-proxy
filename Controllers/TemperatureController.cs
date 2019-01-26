using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DisplayTemperaturaProxy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private IConfiguration _config;
        public TemperatureController(IConfiguration config)
        {
            _config = config;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            using (var client = new HttpClient())
            {
                var url = (string)_config.GetValue(typeof(string), "AppSettings:ApiDeviceUrl");
                var response = await client.GetAsync(url);
                var jsonResponse = JObject.Parse(await response.Content.ReadAsStringAsync());
                return FormatTemperature((decimal)jsonResponse["t"]);
            }
        }

        private string FormatTemperature(decimal temperature) =>
            Math.Round(temperature, 0).ToString().PadLeft(2);
    }
}