using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_ConvenioParqueadero")]
    public class ConvenioParqueadero
    {

        #region Propiedades

        [Key]
        [Column("IdConvenioParqueadero")]
        public int Id { get; set; }
        [Column("Documento")]
        public string Documento { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Column("Apellido")]
        public string Apellido { get; set; }
        [Column("Area")]
        public string Area { get; set; }
        [Column("IdTipoConvenioParqueadero")]
        public int IdTipoConvenioParqueadero { get; set; }   
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

        [Editable(false)]
        public string FechaVigencia { get; set; }

        [Editable(false)]
        public string Estado { get; set; }
        [Editable(false)]
        public string TipoConvenio { get; set; }

        [Editable(false)]
        public int IdTipoVehiculo { get; set; }

        [Editable(false)]
        public int TipoVehiculo { get; set; }

        [Editable(false)]
        public string Placa { get; set; }

        [Editable(false)]
        public string ListaIdTipoVehiculo { get; set; }

        [Editable(false)]
        public string ListaPlacas { get; set; }

        /// <summary>
        /// Para la grilla principal.
        /// </summary>
        [Editable(false)]
        public string Placas { get; set; }

        public IEnumerable<TipoGeneral> ListaTipoVehiculo { get; set; }
        public IEnumerable<TipoGeneral> ListaEstados { get; set; }
        public IEnumerable<TipoGeneral> ListaTipoConvenios { get; set; }
        public IEnumerable<TipoGeneral> ListaAreas { get; set; }
        public IEnumerable<EstructuraEmpleado> ListaEmpleados { get; set; }

        public IEnumerable<AutorizacionVehiculo> objAutorizacionVehiculo { get; set; }

        [Editable(false)]
        public string DatosEmpleado { get; set; }

        #endregion


    }
}
