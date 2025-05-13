using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApiCore.Class.externos;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.auth
{
    public class timonHotelController : ODataController
    {
        private readonly bdERP db;

        public timonHotelController(bdERP context)
        {
            db = context;
        }


        [EnableQuery]
        [HttpPost("timonHotel/importarEstancias")]
        [Authorize]
        public async Task<ActionResult> ImportarEstancias([FromODataUri] DateTime fechaHasta, [FromODataUri] DateTime fechaDesde)
        {
            try
            {
                TimonHotelDataService timonHotelDS = new(db, fechaDesde, fechaHasta);

                await timonHotelDS.ImportarEstancias();

                return Ok(true);
            }
            catch
            {
                return BadRequest(false);
            }
        }
    }
}