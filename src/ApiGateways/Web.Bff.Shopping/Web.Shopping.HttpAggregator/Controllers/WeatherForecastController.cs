using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Shopping.HttpAggregator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var client = new HttpClient();

            string? url = _configuration.GetValue<string>("Identity:Url");
            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = url,
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false,
                    ValidateEndpoints = false,
                },
            });
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return StatusCode(500);
            }

            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return StatusCode(500, tokenResponse.Error);
            }

            // call api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var ordersUrl = _configuration.GetValue<string>("Urls:Orders");
            var response = await apiClient.GetAsync($"{ordersUrl}/identity");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
        }
    }
}