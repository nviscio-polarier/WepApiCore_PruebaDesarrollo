using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class.externos;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.auth
{
    public class macHotelsController : ODataController
    {
        private readonly bdERP db;

        public macHotelsController(bdERP context)
        {
            db = context;
        }


        [EnableQuery]
        [HttpPost("macHotels/importarEstancias")]
        [Authorize]
        public async Task<ActionResult> ImportarEstancias([FromODataUri] DateTime fechaHasta, [FromODataUri] DateTime fechaDesde)
        {
            try
            {
                MacHotelsDataService macHotelsDs = new(db, fechaDesde, fechaHasta);

                await macHotelsDs.ImportarEstancias();

                return Ok(true);
            }
            catch
            {
                return BadRequest(false);
            }
        }
    }
}