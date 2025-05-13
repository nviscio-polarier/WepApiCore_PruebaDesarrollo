using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblBacsCarro", Schema = "Logistica")]
    public partial class tblBacsCarro
    {
        [Key]
        public int idLavanderia { get; set; }
        [Key]
        public byte idFamilia { get; set; }
        public int cantidad { get; set; }

        [ForeignKey("idFamilia")]
        [InverseProperty("tblBacsCarro")]
        public virtual tblFamilia idFamiliaNavigation { get; set; } = null!;
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblBacsCarro")]
        public virtual tblLavanderia idLavanderiaNavigation { get; set; } = null!;
    }
}
