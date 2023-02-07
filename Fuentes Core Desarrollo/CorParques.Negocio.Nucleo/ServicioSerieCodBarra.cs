using CorParques.Datos.Contratos;
using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioSerieCodBarra : IServicioSerieCodBarra
    {
        private readonly IRepositorioSerieCodBarra _repositorio;
        
        public ServicioSerieCodBarra(IRepositorioSerieCodBarra repositorio)
        {
            _repositorio = repositorio;
        }

        public SerieCodigoBarras ObtenerSerie(string consecutivo)
        {
            var _rta = _repositorio.ObtenerLista($"WHERE Consecutivo = '{consecutivo}'");
            return (_rta != null && _rta.Count() > 0) ? _rta.Single() : null;
        }

        public SerieCodigoBarras ObtenerSerie(int id)
        {
            return _repositorio.Obtener(id);
        }
    }
}
