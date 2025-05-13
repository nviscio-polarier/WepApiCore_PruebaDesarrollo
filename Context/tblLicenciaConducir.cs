using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLicenciaConducir", Schema = "RRHH")]
    public partial class tblLicenciaConducir
    {
        public tblLicenciaConducir()
        {
            idPersona = new HashSet<tblPersona>();
            idPeticionCambioDatos = new HashSet<tblPersona_PeticionCambioDatos>();
        }

        [Key]
        public byte idLicenciaConducir { get; set; }
        public string denominacion { get; set; } = null!;
        public string? descripcion { get; set; }

        [ForeignKey("idLicenciaConducir")]
        [InverseProperty("idLicenciaConducir")]
        public virtual ICollection<tblPersona> idPersona { get; set; }
        [ForeignKey("idLicenciaConducir")]
        [InverseProperty("idLicenciaConducir")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> idPeticionCambioDatos { get; set; }
    }
}
