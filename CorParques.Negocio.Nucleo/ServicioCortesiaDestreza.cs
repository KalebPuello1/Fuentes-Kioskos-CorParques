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

    public class ServicioCortesiaDestreza : IServicioCortesiaDestreza
    {

        private readonly IRepositorioCortesiaDestreza _repositorio;

        #region Constructor

        public ServicioCortesiaDestreza(IRepositorioCortesiaDestreza repositorio)
        {

            _repositorio = repositorio;
        }

        #endregion

        #region Metodos

        public bool Actualizar(CortesiaDestreza modelo, out string error)
        {
            return _repositorio.Actualizar(modelo, out error);
        }

        public bool Insertar(CortesiaDestreza modelo, out string error, out string CodigoBarras)
        {
            return _repositorio.Insertar(modelo, out error, out CodigoBarras);
        }

        public IEnumerable<CortesiaDestreza> ObtenerPorDestrezaAtraccion(int IdDestreza, int IdAtraccion)
        {
            return _repositorio.ObtenerPorDestrezaAtraccion(IdDestreza, IdAtraccion);
        }

        public CortesiaDestreza ObtenerPorId(int Id)
        {
            return _repositorio.ObtenerPorId(Id);
        }

        /// <summary>
        /// RDSH: Retorna un objeto CortesiaDestreza por su codigo de barras.
        /// </summary>
        /// <param name="CodigoBarras"></param>
        /// <returns></returns>
        public CortesiaDestreza ObtenerPorCodigoBarras(string CodigoBarras)
        {
            return _repositorio.ObtenerPorCodigoBarras(CodigoBarras);
        }
        

        #endregion
    }
}
