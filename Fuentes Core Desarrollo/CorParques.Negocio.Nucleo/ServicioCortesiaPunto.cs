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

    public class ServicioCortesiaPunto : IServicioCortesiaPunto
    {

        #region Declaraciones

        private readonly IRepositorioCortesiaPunto _repositorio;
        
        #endregion

        #region Constructor

        public ServicioCortesiaPunto(IRepositorioCortesiaPunto repositorio)
        {
            _repositorio = repositorio;
        }

        #endregion

        #region Metodos


        public bool Actualizar(CortesiaPunto modelo, out string error)
        {
            return _repositorio.Actualizar(modelo, out error);
        }

        public bool Eliminar(CortesiaPunto modelo, out string error)
        {
            return _repositorio.Eliminar(modelo, out error);
        }

        public bool Insertar(CortesiaPunto modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }

        public IEnumerable<CortesiaPunto> ObtenerPorDestrezaAtraccion(int IdDestreza, int IdAtraccion)
        {
            return _repositorio.ObtenerPorDestrezaAtraccion(IdDestreza, IdAtraccion);
        }

        public CortesiaPunto ObtenerPorId(int Id)
        {
            return _repositorio.ObtenerPorId(Id);
        }

        public IEnumerable<TipoGeneral> ObtenerProductos(string CodTipoProducto)
        {
            return _repositorio.ObtenerProductos(CodTipoProducto);
        }

        #endregion
    }
}
