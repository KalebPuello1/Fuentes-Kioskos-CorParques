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

    public class ServicioTipoVehiculoPorParqueadero : IServicioTipoVehiculoPorParqueadero
    {

        private readonly IRepositorioTipoVehiculoPorParqueadero _repositorio;

        #region Constructor

        public ServicioTipoVehiculoPorParqueadero(IRepositorioTipoVehiculoPorParqueadero repositorio)
        {
            _repositorio = repositorio;
        }

        #endregion

        #region Metodos

        public bool Actualizar(TipoVehiculoPorParqueadero modelo, out string error)
        {
            return _repositorio.Actualizar(modelo, out error);
        }

        public bool Eliminar(TipoVehiculoPorParqueadero modelo, out string error)
        {
            return _repositorio.Eliminar(modelo, out error);
        }

        public bool Insertar(TipoVehiculoPorParqueadero modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }

        public IEnumerable<TipoVehiculoPorParqueadero> ObtenerLista()
        {
            return _repositorio.ObtenerLista();
        }

        public TipoVehiculoPorParqueadero ObtenerPorId(int Id)
        {
            return _repositorio.ObtenerPorId(Id);
        }

        public IEnumerable<TipoVehiculoPorParqueadero> ObtenerPorIdTipoVehiculo(int IdTipoVehiculo)
        {
            return _repositorio.ObtenerPorIdTipoVehiculo(IdTipoVehiculo);
        }
        #endregion
    }
}
