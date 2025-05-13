using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblCalendario_Estado", Schema = "RRHH")]
    public partial class tblCalendario_Estado
    {
        public tblCalendario_Estado()
        {
            tblCalendarioCentroTrabajo = new HashSet<tblCalendarioCentroTrabajo>();
            tblCalendarioLavanderia = new HashSet<tblCalendarioLavanderia>();
            tblCalendarioPersonal = new HashSet<tblCalendarioPersonal>();
            tblCuadrantePersonal = new HashSet<tblCuadrantePersonal>();
        }

        [Key]
        public byte idCalendario_Estado { get; set; }
        public string denominacion { get; set; } = null!;
        public string? traduccion { get; set; }
        [StringLength(7)]
        public string? colorHexa { get; set; }
        public bool isLavanderia { get; set; }
        public int? idTraduccion { get; set; }

        [ForeignKey("idTraduccion")]
        [InverseProperty("tblCalendario_Estado")]
        public virtual tblTraduccion? idTraduccionNavigation { get; set; }
        [InverseProperty("idCalendario_EstadoNavigation")]
        public virtual ICollection<tblCalendarioCentroTrabajo> tblCalendarioCentroTrabajo { get; set; }
        [InverseProperty("idCalendario_EstadoNavigation")]
        public virtual ICollection<tblCalendarioLavanderia> tblCalendarioLavanderia { get; set; }
        [InverseProperty("idCalendario_EstadoNavigation")]
        public virtual ICollection<tblCalendarioPersonal> tblCalendarioPersonal { get; set; }
        [InverseProperty("idCalendario_EstadoNavigation")]
        public virtual ICollection<tblCuadrantePersonal> tblCuadrantePersonal { get; set; }
    }
}
