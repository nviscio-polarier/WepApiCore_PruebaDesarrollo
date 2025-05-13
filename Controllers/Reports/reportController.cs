using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using WebApiCore.Context;

namespace WebAPI_bdERP.Controllers
{
    public class reportController : ControllerBase
    {
        private readonly bdERP db;
        public reportController(bdERP context)
        {
            db = context;
        }

        [HttpPost]
        [Route("odata/report/getReportBase64")]
        public async Task<ActionResult> getReportBase64([FromBody] List<ReportService_StartBuild_Request.Parameter> bodyParam, [FromODataUri] string denominacionReport)
        {
            tblReports getReport = db.tblReports.Where(x => x.denominacion == denominacionReport).First(); // Saca el idReport a partir de la denominación
            int idReport = getReport.idReport;
            string nombreReport = getReport.nombreVisible;

            string url = "https://localhost:446/WebDocumentViewer/Invoke";
            string message = "";

            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                HttpClient client = new HttpClient(clientHandler);

                var reportId = reportService_OpenReport(client, denominacionReport, url).Result;
                if (reportId == null) { return BadRequest("No se puede abrir el report"); }

                string documentId = reportService_StartBuild(client, denominacionReport, reportId, bodyParam, url).Result;
                if (documentId == null) { return BadRequest("No se puede construir el report"); }

                if (await reportService_DocumentOperation(client, documentId, url) != null)
                {
                    message = reportService_DocumentOperation(client, documentId, url).Result;
                    reportService_DocumentOperation(client, documentId, url).Wait();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok(message);
        }

        private async Task<string> reportService_OpenReport(HttpClient client, string reportName, string url)
        {
            var values = new Dictionary<string, string>
            {
                { "actionKey","openReport"},
                {"arg",reportName},
            };

            var content = new FormUrlEncodedContent(values);
            var response = client.PostAsync(url, content).Result;
            if (response.ReasonPhrase.Equals("OK"))
            {
                ReportService_OpenReport_Result responseString = await response.Content.ReadAsAsync<ReportService_OpenReport_Result>();
                return responseString.result.reportId;
            }
            return null;
        }

        private async Task<string> reportService_StartBuild(HttpClient client, string reportName, string reportId, List<ReportService_StartBuild_Request.Parameter> parameters, string url)
        {
            ReportService_StartBuild_Request objRequest = new ReportService_StartBuild_Request()
            {
                reportUrl = reportName,
                reportId = reportId,
                timeZoneOffset = 120,
                parameters = parameters
            };

            var values = new Dictionary<string, string>
            {
                { "actionKey","startBuild"},
                {"arg", Newtonsoft.Json.JsonConvert.SerializeObject(objRequest)},
            };

            var content = new FormUrlEncodedContent(values);
            var response = client.PostAsync(url, content).Result;
            if (response.ReasonPhrase.Equals("OK"))
            {
                ReportService_StartBuild_Result responseString = await response.Content.ReadAsAsync<ReportService_StartBuild_Result>();
                return responseString.result.documentId;
            }
            return null;
        }

        private async Task<string> reportService_DocumentOperation(HttpClient client, string documentId, string url)
        {
            ReportService_DocumentOperation_Request objRequest = new ReportService_DocumentOperation_Request()
            {
                documentId = documentId
            };

            var values = new Dictionary<string, string>
                {
                    { "actionKey","documentOperation"},
                    {"arg", Newtonsoft.Json.JsonConvert.SerializeObject(objRequest)},
                };

            var content = new FormUrlEncodedContent(values);
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content,
            };

            var response = client.SendAsync(request, HttpCompletionOption.ResponseContentRead).Result;

            if (response.ReasonPhrase.Equals("OK"))
            {
                ReportService_DocumentOperation_Result responseString = await response.Content.ReadAsAsync<ReportService_DocumentOperation_Result>();
                return responseString.result.message; // pdf en base64
            }
            return null;
        }

        public class ReportService_OpenReport_Result
        {
            public object error { get; set; }
            public Result result { get; set; }
            public bool success { get; set; }

            public class Result
            {
                public object documentId { get; set; }
                public string exportOptions { get; set; }
                public Pagesettings pageSettings { get; set; }
                public Parametersinfo parametersInfo { get; set; }
                public string reportId { get; set; }
                public string reportUrl { get; set; }
            }
            public class Pagesettings
            {
                public string color { get; set; }
                public int height { get; set; }
                public int width { get; set; }
            }
            public class Parametersinfo
            {
                public object[] knownEnums { get; set; }
                public Parameter[] parameters { get; set; }
                public bool shouldRequestParameters { get; set; }
            }
            public class Parameter
            {
                public bool AllowNull { get; set; }
                public string Description { get; set; }
                public bool IsFilteredLookUpSettings { get; set; }
                public object LookUpValues { get; set; }
                public bool MultiValue { get; set; }
                public string Name { get; set; }
                public string Path { get; set; }
                public bool SelectAllValues { get; set; }
                public string Tag { get; set; }
                public string TypeName { get; set; }
                public object Value { get; set; }
                public bool Visible { get; set; }
                public string ValueInfo { get; set; }
            }
        }

        public class ReportService_StartBuild_Request
        {
            public string reportId { get; set; }
            public string reportUrl { get; set; }
            public object[] drillDownKeys { get; set; }
            public object[] sortingState { get; set; }
            public int timeZoneOffset { get; set; }
            public List<Parameter> parameters { get; set; }

            public class Parameter
            {
                public string Value { get; set; }
                public string Key { get; set; }
                //   public string TypeName { get; set; }
            }
        }

        public class ReportService_StartBuild_Result
        {
            public object error { get; set; }
            public Result result { get; set; }
            public bool success { get; set; }

            public class Result
            {
                public string documentId { get; set; }
            }
        }

        public class ReportService_DocumentOperation_Request
        {
            public string documentId { get; set; }
        }

        public class ReportService_DocumentOperation_Result
        {
            public object error { get; set; }
            public Result result { get; set; }
            public bool success { get; set; }

            public class Result
            {
                public object customData { get; set; }
                public object documentId { get; set; }
                public string message { get; set; }
                public bool succeeded { get; set; }
            }
        }

    }
}