using System.ComponentModel.DataAnnotations;
using WebApiCore.Context;

namespace WebApiCore.Class.Proyectos.MyPolarier.RRHH
{
    public class PersonaLlamamiento
    {
        [Key]
        public int idPersona { get; set; }
        public int? idLavanderia { get; set; }
        public string nombre { get; set; }
        public string? apellidos { get; set; }
        public string? telefono { get; set; }
        public int? idCategoriaInterna { get; set; }
        public byte? idTipoTrabajo { get; set; }
        public tblDatosSalariales? tblDatosSalariales { get; set; }
        public tblPersonaNTipoContrato? tblPersonaNTipoContrato { get; set; }
    }
}
