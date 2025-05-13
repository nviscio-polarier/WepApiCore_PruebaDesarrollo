using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblHistoricoNominas", Schema = "RRHH")]
    public partial class tblHistoricoNominas
    {
        [Key]
        public int idPersona { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fechaAltaContrato { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime fechaNomina { get; set; }
        public bool isCerrado { get; set; }
        public int? idLavanderia { get; set; }
        public byte? idTipoTrabajo { get; set; }
        public short? idTipoContrato { get; set; }
        public byte? diasContratoMes { get; set; }
        public int? idCategoriaInterna { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? salarioBase { get; set; }
        public byte? numPagas { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? incentivo { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? plusAntiguedad { get; set; }
        public bool? isTrienio { get; set; }
        public byte? añosAntiguedad { get; set; }
        public byte? faltas { get; set; }
        public byte? numRetrasos { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? plusAsistencia { get; set; }
        public byte? festivosTrab { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? plusFestivoTrab { get; set; }
        public byte? baja { get; set; }
        public byte? ceseTemporal { get; set; }
        public byte? bajaLaboral { get; set; }
        [Column(TypeName = "decimal(9, 4)")]
        public decimal? ppext { get; set; }
        public short? horasExtra { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? impHorasExtra { get; set; }
        public short? horasNocturnas { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? plusNocturnidad { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? plusPeligrosidad { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? plusResponsabilidad { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? plusActividad { get; set; }
        public string? obsPlusActividad { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? plusProductividad { get; set; }
        public string? obsPlusProductividad { get; set; }
        public string? obsHorasExtra { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? plusViaje { get; set; }
        public string? obsPlusViaje { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? plusAbsorbible { get; set; }
        public string? obsPlusAbsorbible { get; set; }
        [Column(TypeName = "decimal(14, 2)")]
        public decimal? anticipos { get; set; }
        public string? obsAnticipos { get; set; }
        public bool? isEmbargo { get; set; }
        [Column(TypeName = "decimal(4, 3)")]
        public decimal? segAccidenteConvenio { get; set; }

        [ForeignKey("idCategoriaInterna")]
        [InverseProperty("tblHistoricoNominas")]
        public virtual tblCategoriaInterna? idCategoriaInternaNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblHistoricoNominas")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idPersona")]
        [InverseProperty("tblHistoricoNominas")]
        public virtual tblPersona idPersonaNavigation { get; set; } = null!;
        [ForeignKey("idTipoContrato")]
        [InverseProperty("tblHistoricoNominas")]
        public virtual tblTipoContrato? idTipoContratoNavigation { get; set; }
        [ForeignKey("idTipoTrabajo")]
        [InverseProperty("tblHistoricoNominas")]
        public virtual tblTipoTrabajo? idTipoTrabajoNavigation { get; set; }
    }
}
