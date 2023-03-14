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
    public class ServicioTarifasParqueadero : IServicioTarifasParqueadero
    {

        #region Declaraciones

        private readonly IRepositorioTarifasParqueadero _repositorio;

        #endregion

        #region Constructor

        public ServicioTarifasParqueadero(IRepositorioTarifasParqueadero objRepositorioTarifasParqueadero)
        {
            _repositorio = objRepositorioTarifasParqueadero;
        }

        #endregion

        #region Metodos

        public IEnumerable<TarifasParqueadero> ObtenerTodos()
        {
            return _repositorio.ObtenerLista();
        }

        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            return _repositorio.ObtenerListaSimple();
        }

        public TarifasParqueadero Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        public bool Insertar(TarifasParqueadero modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }

        public bool Actualizar(TarifasParqueadero modelo, out string error)
        {
            return _repositorio.Actualizar(modelo, out error);
        }

        public bool Eliminar(TarifasParqueadero modelo, out string error)
        {
            return _repositorio.Eliminar(modelo, out error);
        }

        public IEnumerable<TipoGeneral> ObtenerTarifaPorIdTipoVehiculo(int IdTipoVehiculo)
        {
            return _repositorio.ObtenerTarifaPorIdTipoVehiculo(IdTipoVehiculo);
        }
        
        #endregion

        #region Metodos No Implementados

        public bool Actualizar(TarifasParqueadero modelo)
        {
            throw new NotImplementedException();
        }

        public TarifasParqueadero Crear(TarifasParqueadero modelo)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
