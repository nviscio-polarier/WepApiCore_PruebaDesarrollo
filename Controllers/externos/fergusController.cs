using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class.externos;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.auth
{
    public class fergusController : ODataController
    {
        private readonly bdERP db;

        public fergusController(bdERP context)
        {
            db = context;
        }


        [EnableQuery]
        [HttpPost("fergus/importarEstancias")]
        [Authorize]
        public async Task<ActionResult> ImportarEstancias([FromODataUri] DateTime fechaHasta, [FromODataUri] DateTime fechaDesde)
        {
            try
            {
                FergusDataService fergusDS = new(db, fechaDesde, fechaHasta);

                await fergusDS.ImportarEstancias();

                return Ok(true);
            }
            catch
            {
                return BadRequest(false);
            }
        }
    }
}