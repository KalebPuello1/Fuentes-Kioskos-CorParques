using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_TransladoInventario")]
    public class TransladoInventario
    {

        #region Propiedades

        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("CodSapMaterial")]
        public string CodSapMaterial { get; set; }
        [Column("Cantidad")]
        public double Cantidad { get; set; }
        [Column("CodSapAlmacenOrigen")]
        public string CodSapAlmacenOrigen { get; set; }
        [Column("CodSapAlmacenDestino")]
        public string CodSapAlmacenDestino { get; set; }
        [Column("Fecha")]
        public DateTime Fecha { get; set; }
        [Column("idUsuario")]
        public int idUsuario { get; set; }
        [Column("Procesado")]
        public bool Procesado { get; set; }
        [Column("UnidadMedida")]
        public string UnidadMedida { get; set; }
        [Column("IdUsuarioRegistro")]
        public int IdUsuarioRegistro { get; set; }
        [Editable(false)]
        public IEnumerable<Materiales> Materiales { get; set; }
        public IEnumerable<Materiales> MaterialesAplicados { get; set; }

        [Editable(false)]
        public int IdPuntoOrigen { get; set; }
        [Editable(false)]
        public int IdPuntoDestino { get; set; }
        [Editable(false)]
        public string Pedido { get; set; }

        #endregion


    }
}

