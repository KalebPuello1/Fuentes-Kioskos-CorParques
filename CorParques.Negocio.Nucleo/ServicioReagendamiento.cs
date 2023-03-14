using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corparques.Datos.Contratos;
using CorParques.Negocio.Contratos;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioReagendamiento : IServicioReagendamiento
    {
        IRepositorioReagendamiento reagendamiento; 
        public ServicioReagendamiento(IRepositorioReagendamiento _reagendamiento)
        {
            reagendamiento = _reagendamiento;
        }

        public Boleteria ObtenerProducto(string consecutivo)
        {
            var dato = new Boleteria();
            dato = reagendamiento.ObtenerProducto(consecutivo);
            return dato;
        }

        public Reagendamiento ObtenerFacturaReagendamiento(string CodBarra)
        {
            return reagendamiento.ObtenerFacturaReagendamiento(CodBarra);
        }

        public string ModificarFecha(CambioFechaBoleta producto)
        {
            var dato = "";
            //cambioFechaCodigos
            dato = reagendamiento.ModificarFecha(producto);
            return dato;
        }
        public string prueba(string f) 
        {
            return "";
        }

        public string InsertarDetalleReagendamientoFecha(Reagendamiento producto)
        {
            var dato = reagendamiento.InsertarDetalleReagendamientoFecha(producto);
            return dato;
        }
        public Boleteria ObtenerDetalleReagendamientoFecha(string consecutivo)
        {
            var dato = reagendamiento.ObtenerDetalleReagendamientoFecha(consecutivo);
            return dato;
        }

    }
}
