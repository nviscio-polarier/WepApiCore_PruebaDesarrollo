using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblAreaLavanderiaNLavanderia", Schema = "General")]
    public partial class tblAreaLavanderiaNLavanderia
    {
        [Key]
        public byte idAreaLavanderia { get; set; }
        [Key]
        public int idLavanderia { get; set; }
        [Required]
        public bool? visibleSmartArea { get; set; }

        [ForeignKey("idAreaLavanderia")]
        [InverseProperty("tblAreaLavanderiaNLavanderia")]
        public virtual tblAreaLavanderia idAreaLavanderiaNavigation { get; set; } = null!;
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblAreaLavanderiaNLavanderia")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
    }
}
