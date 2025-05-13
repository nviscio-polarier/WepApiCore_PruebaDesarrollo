using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPosicionNAreaLavanderiaNLavanderia", Schema = "General")]
    public partial class tblPosicionNAreaLavanderiaNLavanderia
    {
        public tblPosicionNAreaLavanderiaNLavanderia()
        {
            tblCuadrantePersonal = new HashSet<tblCuadrantePersonal>();
        }

        [Key]
        public short idPosicionNAreaLavanderiaNLavanderia { get; set; }
        public byte idAreaLavanderia { get; set; }
        public int idLavanderia { get; set; }
        public byte numPos { get; set; }
        public int? idMaquina { get; set; }
        [Required]
        public bool? activo { get; set; }
        public string denominacion { get; set; } = null!;

        [ForeignKey("idAreaLavanderia")]
        [InverseProperty("tblPosicionNAreaLavanderiaNLavanderia")]
        public virtual tblAreaLavanderia idAreaLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblPosicionNAreaLavanderiaNLavanderia")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idMaquina")]
        [InverseProperty("tblPosicionNAreaLavanderiaNLavanderia")]
        public virtual tblMaquina? idMaquinaNavigation { get; set; }
        [InverseProperty("idPosicionNAreaLavanderiaNLavanderiaNavigation")]
        public virtual ICollection<tblCuadrantePersonal> tblCuadrantePersonal { get; set; }
    }
}
