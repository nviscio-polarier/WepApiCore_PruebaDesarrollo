using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblPais", Schema = "General")]
    public partial class tblPais
    {
        public tblPais()
        {
            tblAdmCliente = new HashSet<tblAdmCliente>();
            tblAdmFormaPago = new HashSet<tblAdmFormaPago>();
            tblAdmProveedor = new HashSet<tblAdmProveedor>();
            tblAlmacenRecambios = new HashSet<tblAlmacenRecambios>();
            tblCategoriaConvenio = new HashSet<tblCategoriaConvenio>();
            tblCentroTrabajo = new HashSet<tblCentroTrabajo>();
            tblEmpresasPolarier = new HashSet<tblEmpresasPolarier>();
            tblIvaNPais = new HashSet<tblIvaNPais>();
            tblLavanderia = new HashSet<tblLavanderia>();
            tblLocalizacion = new HashSet<tblLocalizacion>();
            tblPersona = new HashSet<tblPersona>();
            tblPersona_PeticionCambioDatos = new HashSet<tblPersona_PeticionCambioDatos>();
            tblProveedor = new HashSet<tblProveedor>();
            tblPuerto = new HashSet<tblPuerto>();
            tblRecambioNProveedor = new HashSet<tblRecambioNProveedor>();
        }

        [Key]
        public int idPais { get; set; }
        public string denominacion { get; set; } = null!;
        [StringLength(3)]
        public string codigo { get; set; } = null!;
        public int idTraduccion { get; set; }
        public byte? idMoneda { get; set; }

        [ForeignKey("idMoneda")]
        [InverseProperty("tblPais")]
        public virtual tblMoneda? idMonedaNavigation { get; set; }
        [ForeignKey("idTraduccion")]
        [InverseProperty("tblPais")]
        public virtual tblTraduccion idTraduccionNavigation { get; set; } = null!;
        [InverseProperty("idPaisNavigation")]
        public virtual ICollection<tblAdmCliente> tblAdmCliente { get; set; }
        [InverseProperty("idPaisNavigation")]
        public virtual ICollection<tblAdmFormaPago> tblAdmFormaPago { get; set; }
        [InverseProperty("idPaisNavigation")]
        public virtual ICollection<tblAdmProveedor> tblAdmProveedor { get; set; }
        [InverseProperty("idPaisNavigation")]
        public virtual ICollection<tblAlmacenRecambios> tblAlmacenRecambios { get; set; }
        [InverseProperty("idPaisNavigation")]
        public virtual ICollection<tblCategoriaConvenio> tblCategoriaConvenio { get; set; }
        [InverseProperty("idPaisNavigation")]
        public virtual ICollection<tblCentroTrabajo> tblCentroTrabajo { get; set; }
        [InverseProperty("idPaisNavigation")]
        public virtual ICollection<tblEmpresasPolarier> tblEmpresasPolarier { get; set; }
        [InverseProperty("idPaisNavigation")]
        public virtual ICollection<tblIvaNPais> tblIvaNPais { get; set; }
        [InverseProperty("idPaisNavigation")]
        public virtual ICollection<tblLavanderia> tblLavanderia { get; set; }
        [InverseProperty("idPaisNavigation")]
        public virtual ICollection<tblLocalizacion> tblLocalizacion { get; set; }
        [InverseProperty("idPaisNavigation")]
        public virtual ICollection<tblPersona> tblPersona { get; set; }
        [InverseProperty("idPaisNavigation")]
        public virtual ICollection<tblPersona_PeticionCambioDatos> tblPersona_PeticionCambioDatos { get; set; }
        [InverseProperty("idPaisNavigation")]
        public virtual ICollection<tblProveedor> tblProveedor { get; set; }
        [InverseProperty("idPaisNavigation")]
        public virtual ICollection<tblPuerto> tblPuerto { get; set; }
        [InverseProperty("idPaisNavigation")]
        public virtual ICollection<tblRecambioNProveedor> tblRecambioNProveedor { get; set; }
    }
}
