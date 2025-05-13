using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;
using WebApiCore.Class;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.updater
{
    public class updaterController : Controller
    {

        private readonly string token;
        private readonly string ghe;
        private readonly HttpClient httpClient;
        private readonly bdERP db;

        public updaterController(IConfiguration configuration, bdERP context)
        {
            db = context;
            ghe = configuration.GetSection("GITHUB:GITHUB_ENTERPRISE").Value;
            token = configuration.GetSection("GITHUB:PERSONAL_ACCESS_TOKEN").Value;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", ghe);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("token", token);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/octet-stream"));
        }

        [BasicAuth]
        [HttpGet("updater")]
        [EnableQuery]
        public async Task<ActionResult> checkVersion([FromODataUri] int idLavanderia, [FromODataUri] string nombreRepositorio)
        {
            if (Utils.isProduccion())
            {
                //cuerpo de peticiones
                var client = new GitHubClient(new ProductHeaderValue(nombreRepositorio));
                //autenticación
                var tokenAuth = new Credentials(token);
                client.Credentials = tokenAuth;
                //obtencion del id de latest.json
                var release = await client.Repository.Release.GetLatest(ghe, nombreRepositorio);
                var latestAsset = release.Assets.FirstOrDefault(asset => asset.Name == "latest.json");
                //Obtencion del latest.json usando su id
                var latestJsonFile = JsonConvert.DeserializeObject<JObject>(await getAsset(latestAsset.Url));
                //Modificacion de url de descarga del setup
                string browserDownloadUrl = latestJsonFile["platforms"]["windows-x86_64"]["url"];
                string nombreArchivo = browserDownloadUrl[(browserDownloadUrl.LastIndexOf('/') + 1)..];
                var setupNsisAsset = release.Assets.FirstOrDefault(asset => asset.Name == nombreArchivo);
                latestJsonFile["platforms"]["windows-x86_64"]["url"] = $"https://{Request.Host}/download?nombreRepositorio={nombreRepositorio}&idAsset={setupNsisAsset.Id}";

                return Content(JsonConvert.SerializeObject(latestJsonFile));
            }
            return NoContent();
        }

        [BasicAuth]
        [HttpGet("download")]
        [EnableQuery]
        public async Task<ActionResult> downloadVersion([FromODataUri] string nombreRepositorio, [FromODataUri] int idAsset)
        {
            string url = "https://api.github.com/repos/" + ghe + "/" + nombreRepositorio + "/releases/assets/" + idAsset;
            var latestJsonFile = await httpClient.GetAsync(url);
            return new FileStreamResult(latestJsonFile.Content.ReadAsStream(), "application/octet-stream");
        }

        private async Task<dynamic> getAsset(string assetUrl)
        {
            var response = await httpClient.GetAsync(assetUrl);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
