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
    public class ServicioEstado : IServicioEstado
    {

        #region Declaraciones

        private readonly IRepositorioEstados _repositorio;

        #endregion

        #region Constructor

        public ServicioEstado(IRepositorioEstados objIRepositorioEstados)
        {
            _repositorio = objIRepositorioEstados;
        }

        #endregion

        public bool Actualizar(Estados modelo)
        {
            throw new NotImplementedException();
        }

        public Estados Crear(Estados modelo)
        {
            throw new NotImplementedException();
        }

        public Estados Obtener(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// RDSH: Retorna lista de estados para combo.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerEstados(int IdModulo)
        {
            return _repositorio.ObtenerEstados(IdModulo);
        }
     
        
        public IEnumerable<Estados> ObtenerTodos()
        {
            return _repositorio.ObtenerLista();
        }
    }
}
