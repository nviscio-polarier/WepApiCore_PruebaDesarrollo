using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.Proyectos.MyPolarier.GestionInterna
{
    public class UsuarioController : ODataController
    {

        private readonly bdERP db;

        public UsuarioController(bdERP context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet("odata/GestionInterna/Usuario/GetUsuarioActual")]
        [Authorize]
        public async Task<ActionResult> GetUsuarioActual()
        {
            int idUsuario = int.Parse(HttpContext.Items["idUsuario"].ToString());
            var usuario = (
            from usu in db.tblUsuario
            where usu.idUsuario == idUsuario
            select new UsuarioDto
            {
                idUsuario = usu.idUsuario,
                nombre = usu.nombre,
                idIdioma = usu.idIdioma,
                idCargo = usu.idCargo,
                refactura = usu.refactura,
                idAplicacionInicial = usu.idAplicacionInicial,
                importaEntidades = usu.importaEntidades,
                enableFullScreen = usu.enableFullScreen,
                idLavanderiaInicio = usu.idLavanderiaInicio,
                //idEmpresaPolarier = usu.idEmpresaPolarier,
                idPersona = usu.idPersona,
                enableDatosRRHH = usu.enableDatosRRHH,
                isDepartamentoControl = usu.isDepartamentoControl,
                idLocalizacionNavigation = new LocalizacionDto
                {
                    idLocalizacion = usu.idLocalizacionNavigation.idLocalizacion,
                    denominacion = usu.idLocalizacionNavigation.denominacion,
                    idZonaHoraria = usu.idLocalizacionNavigation.idZonaHoraria,
                    idPais = usu.idLocalizacionNavigation.idPais
                },
                idFormularioInicioNavigation = usu.idFormularioInicioNavigation == null ? null : new FormularioInicioDto
                {
                    formulario = usu.idFormularioInicioNavigation.formulario,
                    informe = usu.idFormularioInicioNavigation.informe,
                    idFormulario = usu.idFormularioInicioNavigation.idFormulario,
                    idApartadoNavigation = new ApartadoDto
                    {
                        apartado = usu.idFormularioInicioNavigation.idApartadoNavigation.apartado,
                    },
                    tblAplicacion = new AplicacionDto
                    {
                        idAplicacion = usu.idFormularioInicioNavigation.idAplicacionNavigation.idAplicacion,
                        denominacion = usu.idFormularioInicioNavigation.idAplicacionNavigation.denominacion,
                        descripcion = usu.idFormularioInicioNavigation.idAplicacionNavigation.descripcion,
                        idTraduccion = usu.idFormularioInicioNavigation.idAplicacionNavigation.idTraduccion,
                        idFormularioInicio = usu.idFormularioInicioNavigation.idAplicacionNavigation.idFormularioInicio,
                        orden = usu.idFormularioInicioNavigation.idAplicacionNavigation.orden,
                        icon = usu.idFormularioInicioNavigation.idAplicacionNavigation.icon,
                        color = usu.idFormularioInicioNavigation.idAplicacionNavigation.color,
                    }
                },
                tblFormularioNUsuario = usu.tblFormularioNUsuario.Select(formulario => new FormularioNUsuarioDto
                {
                    idUsuario = formulario.idUsuario,
                    idFormulario = formulario.idFormulario,
                    isEscritura = formulario.isEscritura,
                    isBorrado = formulario.isBorrado,
                    idFormularioNavigation = new FormularioNavigationDto
                    {
                        idFormulario = formulario.idFormularioNavigation.idFormulario,
                        denominacion = formulario.idFormularioNavigation.denominacion,
                        formulario = formulario.idFormularioNavigation.formulario,
                        idApartado = formulario.idFormularioNavigation.idApartado,
                        informe = formulario.idFormularioNavigation.informe,
                        icon = formulario.idFormularioNavigation.icon,
                        orden = formulario.idFormularioNavigation.orden,
                        idTraduccion = formulario.idFormularioNavigation.idTraduccion,
                        visibleEntidad = formulario.idFormularioNavigation.visibleEntidad,
                        idAplicacion = formulario.idFormularioNavigation.idAplicacion,
                        idTraduccionNavigation = new TraduccionDto
                        {
                            //idTraduccion = formulario.idFormularioNavigation.idTraduccionNavigation.idTraduccion,
                            clave = formulario.idFormularioNavigation.idTraduccionNavigation.clave,
                            apartado = formulario.idFormularioNavigation.idTraduccionNavigation.apartado,
                            es = formulario.idFormularioNavigation.idTraduccionNavigation.es,
                            en = formulario.idFormularioNavigation.idTraduccionNavigation.en,
                            pt = formulario.idFormularioNavigation.idTraduccionNavigation.pt,
                        }
                    }
                }).ToList(),
                tblPersona = new PersonaDto
                {
                    idCategoriaInterna = usu.idPersonaNavigation.idCategoriaInterna,
                },
                tblTipoTrabajoNUsuario = usu.tblTipoTrabajoNUsuario.ToList(),
                tblEmpresaPolarierNUsuario = usu.idEmpresaPolarier.Select(x => new { x.idEmpresaPolarier, x.idPais }).ToList(),
                idPermisoNFormulario = usu.idPermiso.Select(x => x.codigo)

            }).FirstOrDefault();

            if(usuario.idCargo == 1)
            {
                usuario.tblEmpresaPolarierNUsuario = db.tblEmpresasPolarier.Select(x => new { x.idEmpresaPolarier, x.idPais }).ToList();
            }

            return Ok(usuario);
        }

        public class UsuarioDto
        {
            public int idUsuario { get; set; }
            public string nombre { get; set; }
            public int idIdioma { get; set; }
            public int idCargo { get; set; }
            public bool refactura { get; set; }
            public int? idAplicacionInicial { get; set; }
            public bool importaEntidades { get; set; }
            public bool enableFullScreen { get; set; }
            public int? idLavanderiaInicio { get; set; }
            public int? idEmpresaPolarier { get; set; }
            public int? idPersona { get; set; }
            public bool enableDatosRRHH { get; set; }
            public bool isDepartamentoControl { get; set; }
            public LocalizacionDto idLocalizacionNavigation { get; set; }
            public FormularioInicioDto idFormularioInicioNavigation { get; set; }
            public IEnumerable<FormularioNUsuarioDto> tblFormularioNUsuario { get; set; }
            public PersonaDto tblPersona { get; set; }
            public List<tblTipoTrabajoNUsuario> tblTipoTrabajoNUsuario { get; set; }
            public dynamic tblEmpresaPolarierNUsuario { get; set; }
            public IEnumerable<string> idPermisoNFormulario { get; set; }
        }

        public class LocalizacionDto
        {
            public int idLocalizacion { get; set; }
            public string denominacion { get; set; }
            public int idZonaHoraria { get; set; }
            public int? idPais { get; set; }
        }

        public class FormularioInicioDto
        {
            public string formulario { get; set; }
            public bool informe { get; set; }
            public int idFormulario { get; set; }
            public ApartadoDto idApartadoNavigation { get; set; }
            public AplicacionDto tblAplicacion { get; set; }
        }

        public class ApartadoDto
        {
            public string apartado { get; set; }
        }

        public class AplicacionDto
        {
            public int idAplicacion { get; set; }
            public string denominacion { get; set; }
            public string descripcion { get; set; }
            public int idTraduccion { get; set; }
            public int? idFormularioInicio { get; set; }
            public int? orden { get; set; }
            public string icon { get; set; }
            public string color { get; set; }
        }

        public class FormularioNUsuarioDto
        {
            public int idUsuario { get; set; }
            public int idFormulario { get; set; }
            public bool? isEscritura { get; set; }
            public bool? isBorrado { get; set; }
            public FormularioNavigationDto idFormularioNavigation { get; set; }
        }

        public class FormularioNavigationDto
        {
            public int idFormulario { get; set; }
            public string denominacion { get; set; }
            public string formulario { get; set; }
            public int idApartado { get; set; }
            public bool informe { get; set; }
            public string icon { get; set; }
            public int? orden { get; set; }
            public int? idTraduccion { get; set; }
            public bool visibleEntidad { get; set; }
            public int? idAplicacion { get; set; }
            public TraduccionDto idTraduccionNavigation { get; set; }
        }

        public class TraduccionDto
        {
            public int? idTraduccion { get; set; }
            public string clave { get; set; }
            public string apartado { get; set; }
            public string es { get; set; }
            public string en { get; set; }
            public string pt { get; set; }
        }

        public class PersonaDto
        {
            public int? idCategoriaInterna { get; set; }
        }

        public class TipoTrabajoNUsuarioDto
        {
            // Agregar las propiedades necesarias
        }

        public class EmpresaPolarierDto
        {
            public int idEmpresaPolarier { get; set; }
            public int idPais { get; set; }
        }
    }
}
