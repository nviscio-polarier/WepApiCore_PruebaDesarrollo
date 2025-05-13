using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblUnidadesPeso", Schema = "General")]
    public partial class tblUnidadesPeso
    {
        public tblUnidadesPeso()
        {
            tblLavanderia = new HashSet<tblLavanderia>();
        }

        [Key]
        public byte idUnidadesPeso { get; set; }
        [StringLength(50)]
        public string? denominacion { get; set; }
        [StringLength(5)]
        public string? codigo { get; set; }

        [InverseProperty("idUnidadesPesoNavigation")]
        public virtual ICollection<tblLavanderia> tblLavanderia { get; set; }
    }
}
