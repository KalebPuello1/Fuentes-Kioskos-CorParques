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
    public class ServicioTipoVehiculo : IServicioTipoVehiculo
    {

        #region Declaraciones

        private readonly IRepositorioTiposVehiculos _repositorio;

        #endregion

        #region Constructor

        public ServicioTipoVehiculo(IRepositorioTiposVehiculos objIRepositorioTiposVehiculos)
        {
            _repositorio = objIRepositorioTiposVehiculos;
        }

        public bool Actualizar(TipoVehiculo modelo)
        {
            throw new NotImplementedException();
        }

        public TipoVehiculo Crear(TipoVehiculo modelo)
        {
            throw new NotImplementedException();
        }

        public TipoVehiculo Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TipoGeneral> ObtenerTipoVehiculo()
        {
            return _repositorio.ObtenerLista().Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });
        }

        public IEnumerable<TipoVehiculo> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
