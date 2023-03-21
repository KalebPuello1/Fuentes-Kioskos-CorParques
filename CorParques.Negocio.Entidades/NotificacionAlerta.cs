using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class NotificacionAlerta
    {
        public int IdPunto { get; set; }
        public string Nombre { get; set; }
        public string NombreTaquillero { get; set; }
        public int CordenadaX { get; set; }
        public int CordenadaY { get; set; }
        public double MontoRecaudo { get; set; }
        public double TotalVentas { get; set; }
        public double TotalRecoleccion { get; set; }
        public double TotalCaja { get; set; }
        public int SinRecoleccionBase { get; set; }
        public double TotalBase { get; set; }
        public int Alerta { get; set; }
    }
}
