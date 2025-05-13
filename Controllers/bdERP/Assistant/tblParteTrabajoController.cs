using Dapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers;

public class tblParteTrabajoController : ODataController
{
    private readonly bdERP db;
    private readonly tblIncidenciaController ic;

    private readonly Dictionary<string, int> idsCargo = new()
    {
        { "Desarrollador", 1 },
        { "Master", 2 }
    };

    public tblParteTrabajoController(bdERP context)
    {
        db = context;
        ic = new(db);
    }

    [EnableQuery(MaxExpansionDepth = 3)]
    [HttpGet]
    [Authorize]
    public ActionResult Get([FromODataUri] int? idIncidencia)
    {
        return Ok(db.tblParteTrabajo.Where(pt => idIncidencia == null || pt.tblIncidenciaNParte.Where(inp => inp.idIncidencia == idIncidencia).Count() > 0));
    }

    [EnableQuery]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Post([FromBody] tblParteTrabajo parteTrabajo)
    {
        var resultParameter = new SqlParameter
        {
            ParameterName = "@result",
            SqlDbType = SqlDbType.NVarChar,
            Direction = ParameterDirection.Output,
            Size = 8
        };

        db.Database.ExecuteSqlRaw(string.Format("SET @result = (SELECT Incidencias.funCodigoParteTrabajo({0}));", parteTrabajo.idLavanderia), resultParameter).ToString();
        parteTrabajo.codigo = (string)resultParameter.Value;

        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
        parteTrabajo.idUsuarioCrea = idUsuario;

        tblParteTrabajo parteTrabajoResult = new();

        if (parteTrabajo.tblRecambioNParteTrabajo.Count > 0)
        {
            Response_IUD_tblParteTrabajo response = await spIUD_tblParteTrabajo(parteTrabajo, "post");

            if (!response.resultado)
                return BadRequest();

            parteTrabajoResult = db.tblParteTrabajo.Find(response.idParteTrabajo);
        }
        else
        {
            db.tblParteTrabajo.Add(parteTrabajo);

            await Update_estadoIncidencia(parteTrabajo.tblIncidenciaNParte);

            parteTrabajoResult = parteTrabajo;
        }

        if (parteTrabajo.tblIncidenciaNParte.Count == 0)
        {
            return Created(parteTrabajoResult);
        }

        var incidencia = db.tblIncidencia.FirstOrDefault(i => i.idIncidencia == parteTrabajo.tblIncidenciaNParte.First().idIncidencia);

        if (incidencia == null || !incidencia.estado)
        {
            return Created(parteTrabajoResult);
        }

        var tipoSub = db.tblTipoSubIncidencia.FirstOrDefault(tsi => tsi.idSubTipoIncidencia == incidencia.idSubTipoIncidencia);

        var listCorreos = db.tblCorreosNLav
            .Where(cnl =>
                tipoSub != null
                && cnl.idLavanderia == incidencia.idLavanderia
                && cnl.idTipoIncidencia.Any(ti => ti.idTipoIncidencia == tipoSub.idTipoIncidencia)
            )
            .Select(x => x.denominacion)
            .ToList();

        if (listCorreos.Count > 0)
        {
            string correos = String.Join(",", listCorreos);
            await ic.Notificar(incidencia.idIncidencia, correos, false, idUsuario);
        }

        return Created(parteTrabajoResult);
    }

    [EnableQuery]
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult> Patch([FromODataUri] int key, [FromBody] JsonPatchDocument<tblParteTrabajo> parteTrabajo)
    {
        int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());

        var entity = db.tblParteTrabajo
            .Include(x => x.tblIncidenciaNParte)
            .Include(x => x.tblPersonasNParte)
            .Include(x => x.tblRecambioNParteTrabajo)
            .Include(x => x.tblServicioExternoNParteTrabajo)
            .Include(x => x.tblRecambioNParteTrabajoIBS)
            .FirstOrDefault(x => x.idParteTrabajo.Equals(key));
        if (entity == null)
            return NotFound();

        #region Guardar tblIncidenciaNParte
        var operation_incNParte = parteTrabajo.Operations.FirstOrDefault(x => x.path.Equals("/tblIncidenciaNParte"));
        if (operation_incNParte != null)
        {
            entity.tblIncidenciaNParte.Clear();
        }
        #endregion

        #region Guardar tblPersonasNParte
        var operation_perNParte = parteTrabajo.Operations.FirstOrDefault(x => x.path.Equals("/tblPersonasNParte"));
        if (operation_perNParte != null)
        {
            entity.tblPersonasNParte.Clear();
        }
        #endregion

        #region Guardar tblRecambioNParteTrabajo
        var operation_tblRecambioNParteTrabajo = parteTrabajo.Operations.FirstOrDefault(x => x.path.Equals("/tblRecambioNParteTrabajo"));
        if (operation_tblRecambioNParteTrabajo != null)
        {
            entity.tblRecambioNParteTrabajo.Clear();
        }
        #endregion

        #region Guardar tblServicioExternoNParteTrabajo
        var operation_tblServicioExternoNParteTrabajo = parteTrabajo.Operations.FirstOrDefault(x => x.path.Equals("/tblServicioExternoNParteTrabajo"));
        if (operation_tblServicioExternoNParteTrabajo != null)
        {
            db.tblServicioExternoNParteTrabajo.RemoveRange(db.tblServicioExternoNParteTrabajo.Where(senpt => senpt.idParteTrabajo == key));
        }
        #endregion

        #region Guardar tblRecambioNParteTrabajoIBS
        var operation_tblRecambioNParteTrabajoIBS = parteTrabajo.Operations.FirstOrDefault(x => x.path.Equals("/tblRecambioNParteTrabajoIBS"));
        if (operation_tblRecambioNParteTrabajoIBS != null)
        {
            db.tblRecambioNParteTrabajoIBS.RemoveRange(db.tblRecambioNParteTrabajoIBS.Where(senpt => senpt.idParteTrabajo == key));
        }
        #endregion

        parteTrabajo.ApplyTo(entity);

        await db.SaveChangesAsync();

        if (entity.tblRecambioNParteTrabajo.Count > 0)
        {
            Response_IUD_tblParteTrabajo response = await spIUD_tblParteTrabajo(entity, "patch");

            if (!response.resultado)
                return BadRequest();
        }

        if (entity.tblIncidenciaNParte.Count == 0)
        {
            return Updated(entity);
        }

        await Update_estadoIncidencia(entity.tblIncidenciaNParte);

        var incidencia = db.tblIncidencia.FirstOrDefault(i => i.idIncidencia == entity.tblIncidenciaNParte.First().idIncidencia);

        if (incidencia == null || !incidencia.estado)
        {
            return Updated(entity);
        }

        var tipoSub = db.tblTipoSubIncidencia.FirstOrDefault(tsi => tsi.idSubTipoIncidencia == incidencia.idSubTipoIncidencia);

        var listCorreos = db.tblCorreosNLav
            .Where(cnl =>
                tipoSub != null
                && cnl.idLavanderia == incidencia.idLavanderia
                && cnl.idTipoIncidencia.Any(ti => ti.idTipoIncidencia == tipoSub.idTipoIncidencia)
            )
            .Select(x => x.denominacion)
            .ToList();

        if (listCorreos.Count > 0)
        {
            string correos = String.Join(",", listCorreos);
            await ic.Notificar(incidencia.idIncidencia, correos, false, idUsuario);
        }

        return Updated(entity);
    }

    [EnableQuery]
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> Delete([FromODataUri] int key)
    {
        var entity = await db.tblParteTrabajo.FindAsync(key);
        if (entity == null)
            return NotFound(false);

        var parteTrabajo = await db.tblParteTrabajo.FindAsync(key);

        db.tblParteTrabajo.Remove(parteTrabajo);

        await Update_estadoIncidencia(parteTrabajo.tblIncidenciaNParte);

        return Ok(true);
    }

    private async Task Update_estadoIncidencia(ICollection<tblIncidenciaNParte> tblIncidenciaNParte)
    {
        int? idIncidencia = null;
        if (tblIncidenciaNParte.Count > 0)
            idIncidencia = tblIncidenciaNParte.First().idIncidencia;

        //ACTUALIZAMOS ESTADO INCIDENCIA
        if (idIncidencia != null)
        {
            tblIncidencia objInci = db.tblIncidencia.Find(idIncidencia);
            if (objInci != null)
            {
                if (objInci.tblIncidenciaNParte.Count > 0)
                {
                    tblIncidenciaNParte objInciNParte = objInci.tblIncidenciaNParte.OrderByDescending(x => x.idParteNavigation.fecha).First();
                    if (objInciNParte != null)
                    {
                        if (objInciNParte.estadoActual == 0) //CERRAR
                        {
                            objInci.estado = true;
                            objInci.fechaCierre = objInciNParte.fecha;
                        }
                        else
                        {
                            objInci.estado = false;
                            objInci.fechaCierre = null;
                        }

                        objInci.estadoMaquina = objInciNParte.estadoActual;
                    }
                }
                else //SIN PARTE
                {
                    objInci.estado = false;
                    objInci.fechaCierre = null;
                    objInci.estadoMaquina = objInci.estadoMaquinaInicial;
                }
            }
        }

        await db.SaveChangesAsync();
    }

    public async Task<List<Result_spGet_CantPrecio_ParteTrabajo>> spGet_CantPrecio_ParteTrabajo(DateTimeOffset fecha, string? idsAlmacen, int? idParteTrabajo)
    {
        if (idsAlmacen == null)
        {
            int idUsuario = int.Parse(this.HttpContext.Items["idUsuario"].ToString());
            tblUsuario user = db.tblUsuario
                .Where(x => x.idUsuario == idUsuario && !x.isEliminado)
                .Select(x => new tblUsuario { idPersona = x.idPersona, idCargo = x.idCargo, idLavanderia = x.idLavanderia }).FirstOrDefault();
            bool isMasterDev = idsCargo.ContainsValue(user.idCargo);

            var alms = db.tblAlmacenRecambiosNPersona.Where(x =>
                x.idPersona == user.idPersona
                && x.idAlmacenNavigation.idAlmacenPadre != null
                && x.idAlmacenNavigation.activo == true
                && x.idAlmacenNavigation.eliminado == false
            )
            .Select(y => new { y.idAlmacen, y.idAlmacenNavigation.idAlmacenPadre }).ToList();

            var alms_hijos = alms.Select(x => x.idAlmacen);

            idsAlmacen = string.Join('|', alms_hijos);
        }

        #region Tabla almacenes
        string[] numberStrings_almacenes;
        List<int> idsAlmacenList = new();

        numberStrings_almacenes = idsAlmacen.Split('|');
        idsAlmacenList = Array.ConvertAll(numberStrings_almacenes, int.Parse).ToList();

        DataTable dtIdSimple = new();
        dtIdSimple.SetTypeName("General.ttIdSimple");
        dtIdSimple.Columns.Add("id", typeof(int));

        foreach (int idAlmacen in idsAlmacenList)
        {
            var row = dtIdSimple.NewRow();
            row["id"] = idAlmacen;
            dtIdSimple.Rows.Add(row);
        }
        #endregion

        var connection = db.Database.GetDbConnection();
        var result = (await connection.QueryAsync<Result_spGet_CantPrecio_ParteTrabajo>("EXEC [Assistant].[EF_recambios_spGet_CantPrecio_ParteTrabajo] @idsAlmacen, @fecha, @idParteTrabajo",
            new
            {
                idsAlmacen = dtIdSimple,
                fecha,
                idParteTrabajo
            })).ToList();

        return result;
    }

    public async Task<Response_IUD_tblParteTrabajo> spIUD_tblParteTrabajo(tblParteTrabajo parteTrabajo, string tipoAccion)
    {
        #region Tabla ParteTrabajo
        DataTable dtParteTrabajo = new DataTable();
        dtParteTrabajo.SetTypeName("Assistant.ttParteTrabajo");
        dtParteTrabajo.Columns.Add("idParteTrabajo", typeof(int));
        dtParteTrabajo.Columns.Add("fecha", typeof(DateTimeOffset));
        dtParteTrabajo.Columns.Add("resolucion", typeof(string));
        dtParteTrabajo.Columns.Add("idxParteTrabajo", typeof(string));
        dtParteTrabajo.Columns.Add("idMaquina", typeof(int));
        dtParteTrabajo.Columns.Add("idLavanderia", typeof(int));
        dtParteTrabajo.Columns.Add("codigo", typeof(string));
        dtParteTrabajo.Columns.Add("isApp", typeof(bool));
        dtParteTrabajo.Columns.Add("idUsuarioCrea", typeof(int));

        var row = dtParteTrabajo.NewRow();
        row["idParteTrabajo"] = tipoAccion == "post" ? -1 : parteTrabajo.idParteTrabajo;
        row["fecha"] = parteTrabajo.fecha;
        row["resolucion"] = parteTrabajo.resolucion;
        if (parteTrabajo.idxParteTrabajo != null) row["idxParteTrabajo"] = parteTrabajo.idxParteTrabajo;
        row["idMaquina"] = parteTrabajo.idMaquina;
        row["idLavanderia"] = parteTrabajo.idLavanderia;
        if (parteTrabajo.codigo != null) row["codigo"] = parteTrabajo.codigo;
        if (parteTrabajo.isApp != null) row["isApp"] = parteTrabajo.isApp;
        if (parteTrabajo.idUsuarioCrea != null) row["idUsuarioCrea"] = parteTrabajo.idUsuarioCrea;
        dtParteTrabajo.Rows.Add(row);
        #endregion

        #region Tabla RecambioNParteTrabajo
        DataTable dtRecambioNParteTrabajo = new DataTable();
        dtRecambioNParteTrabajo.SetTypeName("Assistant.ttRecambioNParteTrabajo");
        dtRecambioNParteTrabajo.Columns.Add("idRecambio", typeof(int));
        dtRecambioNParteTrabajo.Columns.Add("idParteTrabajo", typeof(int));
        dtRecambioNParteTrabajo.Columns.Add("cantidad", typeof(int));
        dtRecambioNParteTrabajo.Columns.Add("idAlmacen", typeof(int));
        dtRecambioNParteTrabajo.Columns.Add("precio", typeof(decimal));

        foreach (var recambio in parteTrabajo.tblRecambioNParteTrabajo)
        {
            var rowR = dtRecambioNParteTrabajo.NewRow();
            rowR["idRecambio"] = recambio.idRecambio;
            rowR["idParteTrabajo"] = tipoAccion == "post" ? -1 : recambio.idParteTrabajo;
            rowR["cantidad"] = recambio.cantidad;
            rowR["idAlmacen"] = recambio.idAlmacen;
            if (recambio.precio != null) rowR["precio"] = recambio.precio;
            dtRecambioNParteTrabajo.Rows.Add(rowR);
        }
        #endregion

        #region Tabla IncidenciaNParteTrabajo
        DataTable dtIncidenciaNParteTrabajo = new DataTable();
        dtIncidenciaNParteTrabajo.SetTypeName("Assistant.ttIncidenciaNParteTrabajo");
        dtIncidenciaNParteTrabajo.Columns.Add("idIncidencia", typeof(int));
        dtIncidenciaNParteTrabajo.Columns.Add("idParte", typeof(int));
        dtIncidenciaNParteTrabajo.Columns.Add("estadoInicial", typeof(byte));
        dtIncidenciaNParteTrabajo.Columns.Add("estadoActual", typeof(byte));
        dtIncidenciaNParteTrabajo.Columns.Add("fecha", typeof(DateTime));
        dtIncidenciaNParteTrabajo.Columns.Add("peso", typeof(int));
        dtIncidenciaNParteTrabajo.Columns.Add("pesoInicial", typeof(byte));

        foreach (var incidencia in parteTrabajo.tblIncidenciaNParte)
        {
            var rowI = dtIncidenciaNParteTrabajo.NewRow();
            rowI["idIncidencia"] = incidencia.idIncidencia;
            rowI["idParte"] = tipoAccion == "post" ? -1 : incidencia.idParte;
            rowI["estadoInicial"] = incidencia.estadoInicial;
            rowI["estadoActual"] = incidencia.estadoActual;
            rowI["fecha"] = incidencia.fecha;
            if (incidencia.peso != null) rowI["peso"] = incidencia.peso;
            if (incidencia.pesoInicial != null) rowI["pesoInicial"] = incidencia.pesoInicial;
            dtIncidenciaNParteTrabajo.Rows.Add(rowI);
        }
        #endregion

        #region Tabla ServicioExternoNParteTrabajo
        DataTable dtServicioExternoNParteTrabajo = new DataTable();
        dtServicioExternoNParteTrabajo.SetTypeName("Assistant.ttServicioExternoNParteTrabajo");
        dtServicioExternoNParteTrabajo.Columns.Add("idServicioExterno", typeof(int));
        dtServicioExternoNParteTrabajo.Columns.Add("idParteTrabajo", typeof(int));
        dtServicioExternoNParteTrabajo.Columns.Add("denominacion", typeof(string));
        dtServicioExternoNParteTrabajo.Columns.Add("cantidad", typeof(int));
        dtServicioExternoNParteTrabajo.Columns.Add("precio", typeof(decimal));

        foreach (var servicio in parteTrabajo.tblServicioExternoNParteTrabajo)
        {
            var rowS = dtServicioExternoNParteTrabajo.NewRow();
            rowS["idServicioExterno"] = tipoAccion == "post" ? -1 : servicio.idServicioExterno;
            rowS["idParteTrabajo"] = tipoAccion == "post" ? -1 : servicio.idParteTrabajo;
            rowS["denominacion"] = servicio.denominacion;
            rowS["cantidad"] = servicio.cantidad;
            rowS["precio"] = servicio.precio;
            dtServicioExternoNParteTrabajo.Rows.Add(rowS);
        }
        #endregion

        #region Tabla RecambioNParteTrabajoIBS
        DataTable dtRecambioNParteTrabajoIBS = new DataTable();
        dtRecambioNParteTrabajoIBS.SetTypeName("Assistant.ttRecambioNParteTrabajoIBS");
        dtRecambioNParteTrabajoIBS.Columns.Add("idRecambioNParteTrabajo", typeof(int));
        dtRecambioNParteTrabajoIBS.Columns.Add("idParteTrabajo", typeof(int));
        dtRecambioNParteTrabajoIBS.Columns.Add("referenciaRecambio", typeof(string));
        dtRecambioNParteTrabajoIBS.Columns.Add("denominacion", typeof(string));
        dtRecambioNParteTrabajoIBS.Columns.Add("cantidad", typeof(short));
        dtRecambioNParteTrabajoIBS.Columns.Add("precio", typeof(decimal));

        foreach (var recambio in parteTrabajo.tblRecambioNParteTrabajoIBS)
        {
            var rowR = dtRecambioNParteTrabajoIBS.NewRow();
            rowR["idRecambioNParteTrabajo"] = tipoAccion == "post" ? -1 : recambio.idRecambioNParteTrabajo;
            rowR["idParteTrabajo"] = tipoAccion == "post" ? -1 : recambio.idParteTrabajo;
            rowR["referenciaRecambio"] = recambio.referenciaRecambio;
            rowR["denominacion"] = recambio.denominacion;
            rowR["cantidad"] = recambio.cantidad;
            rowR["precio"] = recambio.precio;
            dtRecambioNParteTrabajoIBS.Rows.Add(rowR);
        }
        #endregion

        #region Tabla PersonaNParteTrabajo
        DataTable dtPersonaNParteTrabajo = new DataTable();
        dtPersonaNParteTrabajo.SetTypeName("Assistant.ttPersonaNParteTrabajo");
        dtPersonaNParteTrabajo.Columns.Add("idPersona", typeof(int));
        dtPersonaNParteTrabajo.Columns.Add("idParte", typeof(int));
        dtPersonaNParteTrabajo.Columns.Add("horas", typeof(TimeSpan));

        foreach (var persona in parteTrabajo.tblPersonasNParte)
        {
            var rowP = dtPersonaNParteTrabajo.NewRow();
            rowP["idPersona"] = persona.idPersona;
            rowP["idParte"] = tipoAccion == "post" ? -1 : persona.idParte;
            rowP["horas"] = persona.horas;
            dtPersonaNParteTrabajo.Rows.Add(rowP);
        }
        #endregion

        var connection = db.Database.GetDbConnection();

        Response_IUD_tblParteTrabajo response = new();

        using (var result = await connection.QueryMultipleAsync("EXEC [Assistant].[EF_recambios_spIUD_tblParteTrabajo] @ttParteTrabajo, @ttRecambioNParteTrabajo, @ttIncidenciaNParteTrabajo, @ttServicioExternoNParteTrabajo, @ttRecambioNParteTrabajoIBS, @ttPersonaNParteTrabajo, @tipoAccion", new { ttParteTrabajo = dtParteTrabajo, ttRecambioNParteTrabajo = dtRecambioNParteTrabajo, ttIncidenciaNParteTrabajo = dtIncidenciaNParteTrabajo, ttServicioExternoNParteTrabajo = dtServicioExternoNParteTrabajo, ttRecambioNParteTrabajoIBS = dtRecambioNParteTrabajoIBS, ttPersonaNParteTrabajo = dtPersonaNParteTrabajo, tipoAccion }))
        {
            response.resultado = result.Read<Resultado>().ToList().FirstOrDefault()?.resultado ?? false;
            response.idParteTrabajo = result.Read<IdParteTrabajo>().ToList().FirstOrDefault()?.idParteTrabajo ?? -1;
            response.historicoRecambio = result.Read<HistoricoRecambio>().ToList();
        }

        return response;
    }

    public class Result_spGet_CantPrecio_ParteTrabajo
    {
        public int idAlmacen { get; set; }
        public int idRecambio { get; set; }
        public int max { get; set; }
        public decimal? precio { get; set; }
    }

    public class Response_IUD_tblParteTrabajo
    {
        public bool resultado { get; set; }
        public int idParteTrabajo { get; set; }
        public List<HistoricoRecambio>? historicoRecambio { get; set; }
    }

    private class Resultado
    {
        public bool resultado { get; set; }
    }

    private class IdParteTrabajo
    {
        public int idParteTrabajo { get; set; }
    }

    public class HistoricoRecambio
    {
        public string? ID { get; set; }
        public DateTimeOffset? fecha { get; set; }
        public string? origen { get; set; }
        public string? destino { get; set; }
        public string? denoLavanderia { get; set; }
        public int? cantidad { get; set; }
        public decimal? precio { get; set; }
        public decimal? importe { get; set; }
        public int? idAlmacen { get; set; }
        public string? denoAlmacen { get; set; }
        public int? idRecambio { get; set; }
        public string? denoRecambio { get; set; }
        public string? tipoMovimiento { get; set; }
        public int? idTipoMovimientoRecambioOriginal { get; set; }
        public int? idTipoMovimientoRecambio { get; set; }
        public string? codigoMoneda { get; set; }
    }
}
