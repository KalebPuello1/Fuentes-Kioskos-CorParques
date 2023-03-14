using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    public class Reagendamiento
    {
        public IEnumerable<Factura> Factura { get; set; }
        public IEnumerable<DetalleFactura> DetalleFactura { get; set; }
        public IEnumerable<Boleteria> Boleteria { get; set; }
        public IEnumerable<MedioPagoFactura> MedioPagoFactura { get; set; }
        public IEnumerable<Producto> Productos { get; set; }

        public string fechaInicial { get; set; }
        public string fechaFinal { get; set; }
        public string fechaInicialAnterior { get; set; }
        public string fechaFinalAnterior { get; set; }
        public int idUsuarioLogueado { get; set; }
        public string Mensaje { get; set; }
        public string Consecutivo { get; set; }
        public string observacion { get; set; }

        [Editable(false)]
        public string NombreProducto { get; set; }
    }
}
