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
    public class ServicioTipoTarifaParqueadero : IServicioTipoTarifaParqueadero
    {

        #region Declaraciones

        private readonly IRepositorioTipoTarifaParqueadero _repositorio;

        #endregion

        #region Constructor

        public ServicioTipoTarifaParqueadero(IRepositorioTipoTarifaParqueadero objIRepositorioTipoTarifaParqueadero)
        {
            _repositorio = objIRepositorioTipoTarifaParqueadero;
        }

        #endregion

        #region Metodos

        public IEnumerable<TipoTarifaParqueadero> ObtenerTodos()
        {
            return _repositorio.ObtenerLista();
        }

        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            return _repositorio.ObtenerListaSimple();
        }

        public bool Insertar(TipoTarifaParqueadero modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }

        public bool Actualizar(TipoTarifaParqueadero modelo, out string error)
        {
            return _repositorio.Actualizar(modelo, out error);
        }
        
        public bool Eliminar(TipoTarifaParqueadero modelo, out string error)
        {
            return _repositorio.Eliminar(modelo, out error);
        }

        public TipoTarifaParqueadero Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }



        public bool Actualizar(TipoTarifaParqueadero modelo)
        {
            throw new NotImplementedException();
        }        
        public TipoTarifaParqueadero Crear(TipoTarifaParqueadero modelo)
        {
            throw new NotImplementedException();
        }
                
        #endregion

    }
}
