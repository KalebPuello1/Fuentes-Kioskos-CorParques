using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_Procedimiento")]
    public class Procedimiento
    {

        #region Propiedades

        [Key]        
        [Column("IdProcedimiento")]
        public int IdProcedimiento { get; set; }
        [Editable(false)]
        public string Categoria { get; set; }
        [Editable(false)]
        public string TipoPaciente { get; set; }
        [Column("Triage")]
        public int Triage { get; set; }
        [Editable(false)]
        public string ZonaArea { get; set; }
        [Editable(false)]
        public string UbicacionMedica { get; set; }
        [Editable(false)]
        public string FechaCortaIncidente { get; set; }
        [Editable(false)]
        public string HoraCortaIncidente { get; set; }
        [Editable(false)]
        public string TipoDocumento { get; set; }
        [Column("DocumentoPaciente")]
        public string DocumentoPaciente { get; set; }
        [Column("NombrePaciente")]
        public string NombrePaciente { get; set; }
        [Column("ApellidoPaciente")]
        public string ApellidoPaciente { get; set; }
        [Column("EdadPaciente")]
        public int EdadPaciente { get; set; }
        [Column("Meses")]
        public int Meses { get; set; }
        [Column("DireccionPaciente")]
        public string DireccionPaciente { get; set; }
        [Column("TelefonoPaciente")]
        public string TelefonoPaciente { get; set; }
        [Column("CorreoPaciente")]
        public string CorreoPaciente { get; set; }        
        [Editable(false)]
        public string TipoDocumentoAcompanante { get; set; }
        [Column("DocumentoAcudiente")]
        public string DocumentoAcudiente { get; set; }
        [Column("NombreAcudiente")]
        public string NombreAcudiente { get; set; }
        [Column("TelefonoAcudiente")]
        public string TelefonoAcudiente { get; set; }
        [Column("Causa")]
        public string Causa { get; set; }
        [Column("Sintomas")]
        public string Sintomas { get; set; }
        [Column("Alergias")]
        public string Alergias { get; set; }
        [Column("Tratamiento")]
        public string Tratamiento { get; set; }
        [Column("Traslado")]
        public string Traslado { get; set; }
        [Column("Eps")]
        public string Eps { get; set; }
        [Column("NombreEps")]
        public string NombreEps { get; set; }
        [Column("Recomendaciones")]
        public string Recomendaciones { get; set; }
        [Column("NombreMedico")]
        public string NombreMedico { get; set; }
        

        [Column("IdCentroMedico")]
        public int IdCentroMedico { get; set; }        
        [Column("IdEstado")]
        public int IdEstado { get; set; }
        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
        [Column("IdUsuarioModificacion")]
        public int IdUsuarioModificacion { get; set; }
        [Column("FechaModificacion")]
        public DateTime FechaModificacion { get; set; }
        [Column("IdCategoriaAtencion")]
        public int IdCategoriaAtencion { get; set; }
        [Column("IdTipoPaciente")]
        public int IdTipoPaciente { get; set; }
        [Column("IdTipoDocumentoPaciente")]
        public int IdTipoDocumentoPaciente { get; set; }       
        [Column("IdTipoDocumentoAcudiente")]
        public int IdTipoDocumentoAcudiente { get; set; }
        [Column("FechaIncidente")]
        public DateTime FechaIncidente { get; set; }
        
        [Editable(false)]
        public int IdZonaArea { get; set; }
        [Editable(false)]
        public string FechaIncidenteDDMMAAAA { get; set; }
        [Editable(false)]
        public string FechaIncidenteFinalDDMMAAAA { get; set; }
        [Editable(false)]
        public string HoraIncidente { get; set; }
        [Editable(false)]
        public string Notas { get; set; }

        public IEnumerable<TipoGeneral> ListaUbicacion { get; set; }
        public IEnumerable<TipoGeneral> ListaTipoDocumento { get; set; }
        public IEnumerable<TipoGeneral> ListaCategoriaAtencion { get; set; }
        public IEnumerable<TipoGeneral> ListaTipoPaciente { get; set; }
        public IEnumerable<TipoGeneral> ListaTriage { get; set; }
        public IEnumerable<TipoGeneral> ListaZonaArea { get; set; }
        public IEnumerable<EstructuraEmpleado> ListaEmpleados { get; set; }

        [Editable(false)]
        public string DatosEmpleado { get; set; }

        #endregion


    }
}
