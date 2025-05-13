using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Newtonsoft.Json;
using System.Text;
using WebApiCore.Class.auth;
using WebApiCore.Class.sms;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.utils
{
    [ApiController]
    [Route("sms")]
    public class smsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly bdERP db;
        public smsController(IConfiguration configuration, IHttpClientFactory clientFactory, bdERP context)
        {
            this._configuration = configuration;
            this._clientFactory = clientFactory;
            db = context;
        }

        [HttpPost("testSMS")]
        [BasicAuth]
        //[Authorize]
        public async Task<IActionResult> testSMS([FromODataUri] string phone, [FromODataUri] string text)
        {
            string url = "https://api.gateway360.com/api/3.0/sms/send";
            string from = "Polarier";
            string to = phone;

            VerificarTelefonoRequest objRequest = new VerificarTelefonoRequest(_configuration);
            VerificarTelefonoRequest.Message message = new VerificarTelefonoRequest.Message()
            {
                to = to,
                from = from,
                text = text
            };

            objRequest.messages.Add(message);
            var content = JsonConvert.SerializeObject(objRequest);

            HttpClient httpClient = _clientFactory.CreateClient();
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, stringContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            if (responseContent.Contains("error"))
                return BadRequest();

            if (response.IsSuccessStatusCode)
            {
                return Ok("Ok");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("validatePhone")]
        public async Task<IActionResult> validatePhone([FromBody] PhoneValidationRequest phoneValidationRequest)
        {
            tblUsuario objUsuario = db.tblUsuario
             .Where(x => (x.email == phoneValidationRequest.Email || x.usuario == phoneValidationRequest.Email || x.idPersonaNavigation.email == phoneValidationRequest.Email) &&
            "+" + x.idPersonaNavigation.prefijoTelefonico + x.idPersonaNavigation.telefono == phoneValidationRequest.NumTelefono &&
                         !x.isEliminado)
             .FirstOrDefault();

            if (objUsuario == null)
                return BadRequest();

            db.tblRecoveryPassword.RemoveRange(db.tblRecoveryPassword.Where(x => x.idUsuario == objUsuario.idUsuario));

            tblRecoveryPassword objRecovery = new tblRecoveryPassword()
            {
                idUsuario = objUsuario.idUsuario,
                fecha = DateTime.Now,
                guid = Guid.NewGuid()
            };

            db.tblRecoveryPassword.Add(objRecovery);
            await db.SaveChangesAsync();

            string url = "https://api.gateway360.com/api/3.0/sms/send";
            string from = "Polarier";
            string to = phoneValidationRequest.NumTelefono;
            string codigo = new Random().Next(1000, 9999).ToString();
            string text = String.Format("Bienvenido/a. Tu código es {0}. Introduce el código en la aplicación para autorizar este teléfono.", codigo);

            VerificarTelefonoRequest objRequest = new VerificarTelefonoRequest(_configuration);
            VerificarTelefonoRequest.Message message = new VerificarTelefonoRequest.Message()
            {
                to = to,
                from = from,
                text = text
            };

            objRequest.messages.Add(message);
            var content = JsonConvert.SerializeObject(objRequest);

            HttpClient httpClient = _clientFactory.CreateClient();
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, stringContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            if (responseContent.Contains("error"))
                return BadRequest();

            if (response.IsSuccessStatusCode)
            {
                PhoneValidationResponse objResponse = new PhoneValidationResponse()
                {
                    phoneToken = codigo,
                    passwordToken = objRecovery.guid.ToString()
                };
                return Ok(objResponse);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
