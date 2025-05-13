using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class.externos;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.auth
{
    public class allSunController : ODataController
    {
        private readonly bdERP db;

        public allSunController(bdERP context)
        {
            db = context;
        }


        [EnableQuery]
        [HttpPost("allSun/importarEstancias")]
        [Authorize]
        public async Task<ActionResult> ImportarEstancias([FromODataUri] DateTime fechaHasta, [FromODataUri] DateTime fechaDesde)
        {
            try
            {
                AllSunDataService allSunDS = new(db, fechaDesde, fechaHasta);

                await allSunDS.ImportarEstancias();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}