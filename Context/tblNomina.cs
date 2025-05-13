using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblNomina", Schema = "RRHH")]
    [Index("idPersona", "fechaDesde", "fechaHasta", Name = "IX_tblNomina_idPersona_fechaDesde_fechaHasta")]
    public partial class tblNomina
    {
        public tblNomina()
        {
            tblConceptoNominaNNomina = new HashSet<tblConceptoNominaNNomina>();
            tblConceptoNominaNNomina_Gestoria = new HashSet<tblConceptoNominaNNomina_Gestoria>();
            tblDetalleNNomina = new HashSet<tblDetalleNNomina>();
            tblDocumentoNNomina = new HashSet<tblDocumentoNNomina>();
            tblEstadoNominaNNomina = new HashSet<tblEstadoNominaNNomina>();
            tblHistoricoAsientoNomina = new HashSet<tblHistoricoAsientoNomina>();
        }

        [Key]
        public int idNomina { get; set; }
        public byte idEstadoNomina { get; set; }
        public int idPersona { get; set; }
        [Column(TypeName = "date")]
        public DateTime fechaDesde { get; set; }
        [Column(TypeName = "date")]
        public DateTime fechaHasta { get; set; }
        [Precision(3)]
        public DateTimeOffset? fechaEmision { get; set; }
        [Column(TypeName = "date")]
        public DateTime fechaAntiguedad { get; set; }
        public string numSegSocial_empresa { get; set; } = null!;
        public string numSegSocial_persona { get; set; } = null!;
        public string NIF_empresa { get; set; } = null!;
        public string empresa { get; set; } = null!;
        public string nombreCompleto { get; set; } = null!;
        public string domicilio { get; set; } = null!;
        public string numDocumentoIdentidad { get; set; } = null!;
        public string IBAN { get; set; } = null!;
        public string denoCategoria { get; set; } = null!;
        public short numPagas { get; set; }
        [Column(TypeName = "decimal(4, 3)")]
        public decimal percSegSocial { get; set; }
        public bool isTrienio { get; set; }
        public string? tarifa { get; set; }
        public string? seccion { get; set; }
        public string? NRO { get; set; }
        public string? puestoTrabajo { get; set; }
        public string? CNAE { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? baseAC { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? baseCC { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? ppext { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? totalBaseIRPF { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? totalRemuneration { get; set; }
        [Column(TypeName = "decimal(5, 4)")]
        public decimal? percBaseIncapacidadTemporal { get; set; }
        [Column(TypeName = "decimal(5, 4)")]
        public decimal? percAT_EP { get; set; }
        [Column(TypeName = "decimal(5, 4)")]
        public decimal? percDesempleo { get; set; }
        [Column(TypeName = "decimal(5, 4)")]
        public decimal? percFormacionProfesional { get; set; }
        [Column(TypeName = "decimal(5, 4)")]
        public decimal? percFondoGarantiaSalarial { get; set; }
        [Column(TypeName = "decimal(5, 4)")]
        public decimal? percSegSocialHorasExtra { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? baseSegSocialHorasExtra { get; set; }
        public string? tipoPaga { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? salarioBruto { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? liquidoPercibir { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? anticipo { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? embargo { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? absentismo { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? indemnizacion { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? segSocialEmpresa { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? totalTC1 { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? costeEmpresa { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? importeBaseIncapacidadTemporal { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? importeAT_EP { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? importeDesempleo { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? importeFormacionProfesional { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? importeFondoGarantiaSalarial { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? importeSegSocialHorasExtra { get; set; }
        public string? poblacion { get; set; }
        public short idTipoNomina { get; set; }
        public int? idAdmCentroCoste { get; set; }
        public int? idAdmElementoPEP { get; set; }
        public short? idTipoContrato { get; set; }
        public byte? idTipoTrabajo { get; set; }
        public short? idEmpresaPolarier { get; set; }
        public int? idAdmCuentaContable_Salario { get; set; }
        public int? idAdmCuentaContable_SSEmpresa { get; set; }
        public bool isRetenida { get; set; }
        public byte? idEstadoHistoricoAsientoNomina { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? descuentoPreaviso { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaBaja { get; set; }
        public byte? idMotivoBaja { get; set; }
        public string? detalles { get; set; }
        public int? idUsuario_modifica { get; set; }
        public DateTimeOffset? fecha_modifica { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? tributacionEspeciesEmpresa { get; set; }

        [ForeignKey("idAdmCentroCoste")]
        [InverseProperty("tblNomina")]
        public virtual tblAdmCentroCoste? idAdmCentroCosteNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_SSEmpresa")]
        [InverseProperty("tblNominaidAdmCuentaContable_SSEmpresaNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_SSEmpresaNavigation { get; set; }
        [ForeignKey("idAdmCuentaContable_Salario")]
        [InverseProperty("tblNominaidAdmCuentaContable_SalarioNavigation")]
        public virtual tblAdmCuentaContable? idAdmCuentaContable_SalarioNavigation { get; set; }
        [ForeignKey("idAdmElementoPEP")]
        [InverseProperty("tblNomina")]
        public virtual tblAdmElementoPEP? idAdmElementoPEPNavigation { get; set; }
        [ForeignKey("idEmpresaPolarier")]
        [InverseProperty("tblNomina")]
        public virtual tblEmpresasPolarier? idEmpresaPolarierNavigation { get; set; }
        [ForeignKey("idEstadoHistoricoAsientoNomina")]
        [InverseProperty("tblNomina")]
        public virtual tblEstadoHistoricoAsientoNomina? idEstadoHistoricoAsientoNominaNavigation { get; set; }
        [ForeignKey("idEstadoNomina")]
        [InverseProperty("tblNomina")]
        public virtual tblEstadoNomina idEstadoNominaNavigation { get; set; } = null!;
        [ForeignKey("idMotivoBaja")]
        [InverseProperty("tblNomina")]
        public virtual tblMotivoBaja? idMotivoBajaNavigation { get; set; }
        [ForeignKey("idPersona")]
        [InverseProperty("tblNomina")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
        [ForeignKey("idTipoContrato")]
        [InverseProperty("tblNomina")]
        public virtual tblTipoContrato? idTipoContratoNavigation { get; set; }
        [ForeignKey("idTipoNomina")]
        [InverseProperty("tblNomina")]
        public virtual tblTipoNomina idTipoNominaNavigation { get; set; } = null!;
        [ForeignKey("idTipoTrabajo")]
        [InverseProperty("tblNomina")]
        public virtual tblTipoTrabajo? idTipoTrabajoNavigation { get; set; }
        [ForeignKey("idUsuario_modifica")]
        [InverseProperty("tblNomina")]
        public virtual tblUsuario? idUsuario_modificaNavigation { get; set; }
        [InverseProperty("idNominaNavigation")]
        public virtual ICollection<tblConceptoNominaNNomina> tblConceptoNominaNNomina { get; set; }
        [InverseProperty("idNominaNavigation")]
        public virtual ICollection<tblConceptoNominaNNomina_Gestoria> tblConceptoNominaNNomina_Gestoria { get; set; }
        [InverseProperty("idNominaNavigation")]
        public virtual ICollection<tblDetalleNNomina> tblDetalleNNomina { get; set; }
        [InverseProperty("idNominaNavigation")]
        public virtual ICollection<tblDocumentoNNomina> tblDocumentoNNomina { get; set; }
        [InverseProperty("idNominaNavigation")]
        public virtual ICollection<tblEstadoNominaNNomina> tblEstadoNominaNNomina { get; set; }
        [InverseProperty("idNominaNavigation")]
        public virtual ICollection<tblHistoricoAsientoNomina> tblHistoricoAsientoNomina { get; set; }
    }
}
