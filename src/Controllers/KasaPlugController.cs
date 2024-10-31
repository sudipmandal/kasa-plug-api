using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Kasa;
using System.Net;

namespace KasaSmartPlugAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KasaPlugController : ControllerBase
    {
        private string plugIp;
        private readonly ILogger<KasaPlugController> _logger;

        public KasaPlugController(ILogger<KasaPlugController> logger)
        {
            _logger = logger;
            plugIp = Environment.GetEnvironmentVariable("PLUG_IP");

            //validate if plugIp is valid ip address
            if (!IPAddress.TryParse(plugIp, out _))
            {
                throw new ArgumentException("Please Enter Valid IP address in the PLUG_IP env variable");
            }
        }

        [HttpGet]
        [Route("turnon")]
        public async void TurnOn()
        {
            try
            {
                using IKasaOutlet kasa = new KasaOutlet(plugIp);
                await kasa.System.SetOutletOn(true);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error turning on plug");
                throw;
            }
        }

        [HttpGet]
        [Route("turnoff")]
        public async void TurnOff()
        {
            try
            {
                using IKasaOutlet kasa = new KasaOutlet(plugIp);
                await kasa.System.SetOutletOn(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error turning off plug");
                throw;
            }
        }
    }
}
