using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblTipoDocumentoIdentidad", Schema = "RRHH")]
    public partial class tblTipoDocumentoIdentidad
    {
        public tblTipoDocumentoIdentidad()
        {
            tblPersona_PeticionCambioDatosidTipoDocumentoIdentidadNavigation = new HashSet<tblPersona_PeticionCambioDatos>();
            tblPersona_PeticionCambioDatosidTipoDocumentoIdentidad_tutorNavigation = new HashSet<tblPersona_PeticionCambioDatos>();
            tblPersonaidTipoDocumentoIdentidadNavigation = new HashSet<tblPersona>();
            tblPersonaidTipoDocumentoIdentidad_tutorNavigation = new HashSet<tblPersona>();
        }

        [Key]
        public byte idTipoDocumentoIdentidad { get; set; }
        public string denominacion { get; set; } = null!;

        [InverseProperty("idTipoDocumentoIdentidadNavigation")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatosidTipoDocumentoIdentidadNavigation { get; set; }
        [InverseProperty("idTipoDocumentoIdentidad_tutorNavigation")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatosidTipoDocumentoIdentidad_tutorNavigation { get; set; }
        [InverseProperty("idTipoDocumentoIdentidadNavigation")]
        public virtual ICollection<tblPersona> tblPersonaidTipoDocumentoIdentidadNavigation { get; set; }
        [InverseProperty("idTipoDocumentoIdentidad_tutorNavigation")]
        public virtual ICollection<tblPersona> tblPersonaidTipoDocumentoIdentidad_tutorNavigation { get; set; }
    }
}
