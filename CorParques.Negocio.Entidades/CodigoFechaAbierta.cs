using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{

    // [Table("TB_PedidosBoletaControl")]
    public class CodigoFechaAbierta
    {
        #region Propiedades

        [Key]
        [Column("IdPedidoBoletaControl")]
        public int IdPedidoBoletaControl { get; set; }

        [Column("CodSapPedido")]
        public string CodSapPedido { get; set; }

        [Column("CodBarrasBoletaControl")]
        public string CodBarrasBoletaControl { get; set; }

        [Column("FechaInicial")]
        public DateTime? FechaInicial { get; set; }

        [Column("FechaFinal")]
        public DateTime? FechaFinal { get; set; }

        [Column("Redencion")]
        public byte Redencion { get; set; }

        [Column("IdSolicitudBoletaControl")]
        public int IdSolicitudBoletaControl { get; set; }

        [Column("FechaRedencion")]
        public DateTime? FechaRedencion { get; set; }

        //Nueva propiedads
        [Column("IdEstado")]
        public int IdEstado { get; set; }

        public List<CodigoFechaAbierta> ListaCodigoFechaAbierta { get; set; }
        public List<List<CodigoFechaAbierta>> ListaListFechaAbierta { get; set; }
        // public IEnumerable<CodigoFechaAbierta> Lista { get; set; }
        // public IEnumerable<CodigoFechaAbierta> Listaa { get; set; }

        //public byte[] codigoBarras { get; set; } 
        public string codigoBarras { get; set; }

        public string datoDesacarga { get; set; }
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public int CantidadProducto { get; set; }
        public int cantidadDisponible { get; set; }
        public int IdSAPCliente { get; set; }
        public int CantEnviar { get; set; }
        public string Correo { get; set; }
        public string CodSapProducto { get; set; }
        public string Consecutivo { get; set; }
        public bool boleteria { get; set; }
        public string CodSapTipoProducto { get; set; }
        public string Valor { get; set; }
        public int posicion { get; set; }
        public string NombreCliente { get; set; }
        public string Mostrar { get; set; }
        public string NombreEnviado { get; set; }
        public string RutaQR { get; set; }
        public string RutaPDF { get; set; }
        public bool enviarProductos { get; set; }
        public Usuario usuario { get; set; }
        public string rtaLogo { get; set; }
        public string CodCliente { get; set; }
        public string NombreClientePDF { get; set; }
        #endregion
    }
}