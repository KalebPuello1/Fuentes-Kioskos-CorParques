using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CorParques.Negocio.Entidades;

namespace CorParques.Presentacion.MVC.Ventas.Core.Models
{
    public class ConsultaKiosco
    {
        public Boleteria Boleta { get; set; }
        public DetalleBoleta DetalleBoleta { get; set; }
        public Factura Factura { get; set; }
        public ConsultaMovimientoBoletaControl BolControl { get; set; }
    }
}