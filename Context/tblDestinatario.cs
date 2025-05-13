using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblDestinatario", Schema = "Logistica")]
    public partial class tblDestinatario
    {
        public tblDestinatario()
        {
            tblEnvio = new HashSet<tblEnvio>();
        }

        [Key]
        public int idDestinatario { get; set; }
        public string denominacion { get; set; } = null!;
        public string? direccion { get; set; }
        public string? codigoPostal { get; set; }
        public string? ciudad { get; set; }
        public string? provincia { get; set; }
        public string? pais { get; set; }
        public string? nif { get; set; }

        [InverseProperty("idDestinatarioNavigation")]
        public virtual ICollection<tblEnvio> tblEnvio { get; set; }
    }
}
