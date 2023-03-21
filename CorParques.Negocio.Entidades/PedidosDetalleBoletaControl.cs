using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_PedidosDetalleBoletaControl")]
    public class PedidosDetalleBoletaControl
    {
        [Key]
        [Column("IdPedidoDetalleBoletaControl")]
        public int IdPedidoDetalleBoletaControl { get; set; }

        [Column("CodBarrasBoletaControl")]
        public string CodBarrasBoletaControl { get; set; }

        [Column("CodSapTipoProducto")]
        public string CodSapTipoProducto { get; set; }

        [Column("CodSapProducto")]
        public string CodSapProducto { get; set; }

        [Column("Valor")]
        public decimal Valor { get; set; }

        [Column("Cantidad")]
        public int Cantidad { get; set; }

    }
}
