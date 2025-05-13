using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    [Table("tblEnvio", Schema = "Logistica")]
    public partial class tblEnvio
    {
        public tblEnvio()
        {
            tblEnvio_Documento = new HashSet<tblEnvio_Documento>();
            tblPackingList = new HashSet<tblPackingList>();
            idCorreo = new HashSet<tblCorreosNLav>();
            idTipoDocumento_Envio = new HashSet<tblTipoDocumento_Envio>();
        }

        [Key]
        public int idEnvio { get; set; }
        public int? idProyecto { get; set; }
        public int? idDestinatario { get; set; }
        public int? idEmbarcador { get; set; }
        public int? idTipoContenedor { get; set; }
        public int? idIncotermProv { get; set; }
        public string? numCont { get; set; }
        public int? idPuertoCarga { get; set; }
        public int? idPuertoDestino { get; set; }
        public int? idIncotermCliente { get; set; }
        public string? blNumero { get; set; }
        [Column(TypeName = "date")]
        public DateTime? fechaEstimacionCarga { get; set; }
        [Column(TypeName = "date")]
        public DateTime? etd { get; set; }
        [Column(TypeName = "date")]
        public DateTime? etaDestino { get; set; }
        [Column(TypeName = "date")]
        public DateTime? despacho { get; set; }
        [Column(TypeName = "date")]
        public DateTime? entrega { get; set; }
        public bool isArchivado { get; set; }
        public string? comentarios { get; set; }

        [ForeignKey("idDestinatario")]
        [InverseProperty("tblEnvio")]
        public virtual tblDestinatario? idDestinatarioNavigation { get; set; }
        [ForeignKey("idEmbarcador")]
        [InverseProperty("tblEnvio")]
        public virtual tblEmbarcador? idEmbarcadorNavigation { get; set; }
        [ForeignKey("idIncotermCliente")]
        [InverseProperty("tblEnvioidIncotermClienteNavigation")]
        public virtual tblIncoterm? idIncotermClienteNavigation { get; set; }
        [ForeignKey("idIncotermProv")]
        [InverseProperty("tblEnvioidIncotermProvNavigation")]
        public virtual tblIncoterm? idIncotermProvNavigation { get; set; }
        [ForeignKey("idProyecto")]
        [InverseProperty("tblEnvio")]
        public virtual tblProyecto? idProyectoNavigation { get; set; }
        [ForeignKey("idPuertoCarga")]
        [InverseProperty("tblEnvioidPuertoCargaNavigation")]
        public virtual tblPuerto? idPuertoCargaNavigation { get; set; }
        [ForeignKey("idPuertoDestino")]
        [InverseProperty("tblEnvioidPuertoDestinoNavigation")]
        public virtual tblPuerto? idPuertoDestinoNavigation { get; set; }
        [ForeignKey("idTipoContenedor")]
        [InverseProperty("tblEnvio")]
        public virtual tblTipoContenedor? idTipoContenedorNavigation { get; set; }
        [InverseProperty("idEnvioNavigation")]
        public virtual ICollection<tblEnvio_Documento> tblEnvio_Documento { get; set; }
        [InverseProperty("idEnvioNavigation")]
        public virtual ICollection<tblPackingList> tblPackingList { get; set; }

        [ForeignKey("idEnvio")]
        [InverseProperty("idEnvio")]
        public virtual ICollection<tblCorreosNLav> idCorreo { get; set; }
        [ForeignKey("idEnvio")]
        [InverseProperty("idEnvio")]
        public virtual ICollection<tblTipoDocumento_Envio> idTipoDocumento_Envio { get; set; }
    }
}
