using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTraduccion", Schema = "General")]
    public partial class tblTraduccion
    {
        public tblTraduccion()
        {
            tblApartado = new HashSet<tblApartado>();
            tblAplicacion = new HashSet<tblAplicacion>();
            tblAreaLavanderia = new HashSet<tblAreaLavanderia>();
            tblCalendarioEntidad_Estados = new HashSet<tblCalendarioEntidad_Estados>();
            tblCalendario_Estado = new HashSet<tblCalendario_Estado>();
            tblCategoriaAbono = new HashSet<tblCategoriaAbono>();
            tblCategoriaRecurso = new HashSet<tblCategoriaRecurso>();
            tblConceptoNomina = new HashSet<tblConceptoNomina>();
            tblDenoPrenda = new HashSet<tblDenoPrenda>();
            tblDiscapacidad = new HashSet<tblDiscapacidad>();
            tblEstadoCivil = new HashSet<tblEstadoCivil>();
            tblEstadoMovimientoElemLog = new HashSet<tblEstadoMovimientoElemLog>();
            tblEstadoNomina = new HashSet<tblEstadoNomina>();
            tblEventoPersona_Estado = new HashSet<tblEventoPersona_Estado>();
            tblFamiliaidTraduccionNavigation = new HashSet<tblFamilia>();
            tblFamiliaidTraduccion_abrNavigation = new HashSet<tblFamilia>();
            tblFormatoDiasLibres = new HashSet<tblFormatoDiasLibres>();
            tblFormulario = new HashSet<tblFormulario>();
            tblFrecuenciaMantenimiento = new HashSet<tblFrecuenciaMantenimiento>();
            tblGenero = new HashSet<tblGenero>();
            tblGrupoEnergetico = new HashSet<tblGrupoEnergetico>();
            tblMotivoIncumplimientoJornada = new HashSet<tblMotivoIncumplimientoJornada>();
            tblNivelEstudios = new HashSet<tblNivelEstudios>();
            tblPais = new HashSet<tblPais>();
            tblPermiso = new HashSet<tblPermiso>();
            tblPoliticaDisciplinaria = new HashSet<tblPoliticaDisciplinaria>();
            tblTipoDocumento = new HashSet<tblTipoDocumento>();
            tblTipoElemLog = new HashSet<tblTipoElemLog>();
            tblTipoIncidenciaidTraduccionNavigation = new HashSet<tblTipoIncidencia>();
            tblTipoIncidenciaidTraduccion_abrNavigation = new HashSet<tblTipoIncidencia>();
            tblTipoMovimiento = new HashSet<tblTipoMovimiento>();
            tblTipoNomina = new HashSet<tblTipoNomina>();
            tblTipoNomina_MX = new HashSet<tblTipoNomina_MX>();
            tblTipoNomina_RD = new HashSet<tblTipoNomina_RD>();
            tblTipoNotificacionidTraduccionDenominacionNavigation = new HashSet<tblTipoNotificacion>();
            tblTipoNotificacionidTraduccionDescripcionNavigation = new HashSet<tblTipoNotificacion>();
            tblTipoPrenda = new HashSet<tblTipoPrenda>();
            tblTipoRetiro = new HashSet<tblTipoRetiro>();
            tblTipoSalidaRecambio = new HashSet<tblTipoSalidaRecambio>();
            tblTipoSubIncidencia = new HashSet<tblTipoSubIncidencia>();
        }

        public string clave { get; set; } = null!;
        public string apartado { get; set; } = null!;
        public string? es { get; set; }
        public string? en { get; set; }
        public string? pt { get; set; }
        [Key]
        public int idTraduccion { get; set; }

        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblApartado> tblApartado { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblAplicacion> tblAplicacion { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblAreaLavanderia> tblAreaLavanderia { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblCalendarioEntidad_Estados> tblCalendarioEntidad_Estados { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblCalendario_Estado> tblCalendario_Estado { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblCategoriaAbono> tblCategoriaAbono { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblCategoriaRecurso> tblCategoriaRecurso { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblConceptoNomina> tblConceptoNomina { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblDenoPrenda> tblDenoPrenda { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblDiscapacidad> tblDiscapacidad { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblEstadoCivil> tblEstadoCivil { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblEstadoMovimientoElemLog> tblEstadoMovimientoElemLog { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblEstadoNomina> tblEstadoNomina { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblEventoPersona_Estado> tblEventoPersona_Estado { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblFamilia> tblFamiliaidTraduccionNavigation { get; set; }
        [InverseProperty("idTraduccion_abrNavigation")]
        public virtual ICollection<tblFamilia> tblFamiliaidTraduccion_abrNavigation { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblFormatoDiasLibres> tblFormatoDiasLibres { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblFormulario> tblFormulario { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblFrecuenciaMantenimiento> tblFrecuenciaMantenimiento { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblGenero> tblGenero { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblGrupoEnergetico> tblGrupoEnergetico { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblMotivoIncumplimientoJornada> tblMotivoIncumplimientoJornada { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblNivelEstudios> tblNivelEstudios { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblPais> tblPais { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblPermiso> tblPermiso { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblPoliticaDisciplinaria> tblPoliticaDisciplinaria { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblTipoDocumento> tblTipoDocumento { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblTipoElemLog> tblTipoElemLog { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblTipoIncidencia> tblTipoIncidenciaidTraduccionNavigation { get; set; }
        [InverseProperty("idTraduccion_abrNavigation")]
        public virtual ICollection<tblTipoIncidencia> tblTipoIncidenciaidTraduccion_abrNavigation { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblTipoMovimiento> tblTipoMovimiento { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblTipoNomina> tblTipoNomina { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblTipoNomina_MX> tblTipoNomina_MX { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblTipoNomina_RD> tblTipoNomina_RD { get; set; }
        [InverseProperty("idTraduccionDenominacionNavigation")]
        public virtual ICollection<tblTipoNotificacion> tblTipoNotificacionidTraduccionDenominacionNavigation { get; set; }
        [InverseProperty("idTraduccionDescripcionNavigation")]
        public virtual ICollection<tblTipoNotificacion> tblTipoNotificacionidTraduccionDescripcionNavigation { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblTipoPrenda> tblTipoPrenda { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblTipoRetiro> tblTipoRetiro { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblTipoSalidaRecambio> tblTipoSalidaRecambio { get; set; }
        [InverseProperty("idTraduccionNavigation")]
        public virtual ICollection<tblTipoSubIncidencia> tblTipoSubIncidencia { get; set; }
    }
}
