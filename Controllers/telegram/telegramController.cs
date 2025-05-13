using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Newtonsoft.Json;
using System.Text;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.utils
{
    [ApiController]
    [Route("telegram")]
    public class telegramController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly bdERP db;
        public telegramController(IConfiguration configuration, IHttpClientFactory clientFactory, bdERP context)
        {
            this._configuration = configuration;
            this._clientFactory = clientFactory;
            db = context;
        }

        [HttpPost("sendMessage_telegram_smartHub")]
        [BasicAuth]
        public async Task<IActionResult> sendMessage_telegram_smartHub([FromODataUri] string text)
        {
            string botToken = "7841629147:AAHaFlVchDYjI9H_0_yW-pasDkcCXu4Gjkg"; // token del bot
            string url = "https://api.telegram.org/bot" + botToken + "/sendMessage";
            string chatId = "-4569713866"; // Grupo de telegram
            string message = text;

            HttpClient httpClient = _clientFactory.CreateClient();

            var payload = new
            {
                chat_id = chatId,
                text = message
            };

            var content = JsonConvert.SerializeObject(payload);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, stringContent);

            var responseContent = await response.Content.ReadAsStringAsync();

            return Ok(responseContent);
        }

    }
}
