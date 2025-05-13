using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblLecturaCarro", Schema = "MyRealData")]
    public partial class tblLecturaCarro
    {
        [Key]
        public int idLecturaCarro { get; set; }
        public int? idCarro { get; set; }
        public DateTimeOffset? fecha { get; set; }
        public byte? idEstadoMovimientoElemLog { get; set; }
        public int? idCantidadNMovimientoElemLog { get; set; }
        public byte? idTipoLectura { get; set; }
        public byte? idLecturaCarro_Estado { get; set; }
        public int? idUsuario { get; set; }
        public int? idLavanderia { get; set; }
        public int? idEntidad { get; set; }

        [ForeignKey("idCantidadNMovimientoElemLog")]
        [InverseProperty("tblLecturaCarro")]
        public virtual tblCantidadNMovimientoElemLog? idCantidadNMovimientoElemLogNavigation { get; set; }
        [ForeignKey("idCarro")]
        [InverseProperty("tblLecturaCarro")]
        public virtual tblCarro? idCarroNavigation { get; set; }
        [ForeignKey("idEntidad")]
        [InverseProperty("tblLecturaCarro")]
        public virtual tblEntidad? idEntidadNavigation { get; set; }
        [ForeignKey("idEstadoMovimientoElemLog")]
        [InverseProperty("tblLecturaCarro")]
        public virtual tblEstadoMovimientoElemLog? idEstadoMovimientoElemLogNavigation { get; set; }
        [ForeignKey("idLavanderia")]
        [InverseProperty("tblLecturaCarro")]
        public virtual tblLavanderia? idLavanderiaNavigation { get; set; }
        [ForeignKey("idLecturaCarro_Estado")]
        [InverseProperty("tblLecturaCarro")]
        public virtual tblLecturaCarro_Estado? idLecturaCarro_EstadoNavigation { get; set; }
        [ForeignKey("idTipoLectura")]
        [InverseProperty("tblLecturaCarro")]
        public virtual tblTipoLectura? idTipoLecturaNavigation { get; set; }
        [ForeignKey("idUsuario")]
        [InverseProperty("tblLecturaCarro")]
        public virtual tblUsuario? idUsuarioNavigation { get; set; }
    }
}
