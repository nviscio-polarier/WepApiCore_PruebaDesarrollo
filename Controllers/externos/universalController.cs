using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class.externos;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.auth
{
    public class universalController : ODataController
    {
        private readonly bdERP db;

        public universalController(bdERP context)
        {
            db = context;
        }


        [EnableQuery]
        [HttpPost("universal/importarEstancias")]
        [Authorize]
        public async Task<ActionResult> ImportarEstancias([FromODataUri] DateTime fechaHasta, [FromODataUri] DateTime fechaDesde)
        {
            try
            {
                UniversalDataService universalDs = new(db, fechaDesde, fechaHasta);

                await universalDs.ImportarEstancias();

                return Ok(true);
            }
            catch
            {
                return BadRequest(false);
            }
        }
    }
}