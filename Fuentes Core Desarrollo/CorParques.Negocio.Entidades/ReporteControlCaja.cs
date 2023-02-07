using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteControlCaja
    {
        [Editable(false)]
        public DateTime fechaInicial { get; set; }

        [Editable(false)]
        public DateTime fechaFinal { get; set; }

        [Editable(false)]
        public int idPerfil { get; set; }

        [Editable(false)]
        public int idTaquillero { get; set; }
        public DateTime Fecha { get; set; }
        public int IdApertura { get; set; }
        public int IdUsuario { get; set; }
        public string Documento { get; set; }
        public string Nombres { get; set; }
        public string Perfiles { get; set; }
        public string Punto { get; set; }
        public string Base { get; set; }
        public string AlistamientoBase { get; set; }
        public string SueprvisorBase { get; set; }
        public string TaquilleroBase { get; set; }
        public string IdProducto { get; set; }
        public string producto { get; set; }
        public string cantidadVendida { get; set; }
        public string CantidaadSupervisor { get; set; }
        public string BoleteriaApertura { get; set; }
        public string boleteriaReabastecimiento { get; set; }
        public string anulaciones { get; set; }
        public string notacredito { get; set; }
        public string Faltante { get; set; }
        public string sobrante { get; set; }
        public string recoleccion { get; set; }
        public string totalventas { get; set; }
        public string efectivo { get; set; }
        public string tarjetas { get; set; }
        public string BonoRegalo { get; set; }
        public string BonoSodexo { get; set; }
        public string DescuentoEmpleado { get; set; }
        public string RecoleccionBase { get; set; }
        public string RecoleccionEfectivo { get; set; }
        public string recolecciontarjeta { get; set; }
        public string recolecciondocumentos { get; set; }
        public string recoleccionnovedadessobrantes { get; set; }
        public string recoleccionbaseentregado { get; set; }
        public string recoleccionefectivoentregado { get; set; }
        public string recolecciontarjetaentregado { get; set; }
        public string recolecciondocumentosentregado { get; set; }
        public string recoleccionnovedadessobrantesentregado { get; set; }
        public string CantidadTaquillero { get; set; }
        public string CantidadNido { get; set; }

    }
}
