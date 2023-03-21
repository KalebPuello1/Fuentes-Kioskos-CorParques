using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioReporteCostoProducto : IServicioReporteCostoProducto
    {
        private readonly IRepositorioReporteCostoProducto _respositorio;

        public ServicioReporteCostoProducto(IRepositorioReporteCostoProducto repositorio)
        {
            _respositorio = repositorio;
        }

        public IEnumerable<TipoGeneral> obtenerProductos()
        {
            return _respositorio.obtenerProductos();
        }

        public IEnumerable<ReporteCostoProducto> Obtener(ReporteCostoProducto modelo)
        {
            return _respositorio.Obtener(modelo);
        }
    }
}
