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
    public class ServicioCargueBrazalete : IServicioCargueBrazalete
    {

        #region Declaraciones

        private readonly IRepositorioCargueBrazalete _repositorio;

        #endregion

        #region Constructor

        public ServicioCargueBrazalete(IRepositorioCargueBrazalete objIRepositorioCargueBrazalete)
        {
            _repositorio = objIRepositorioCargueBrazalete;
        }

        #endregion

        #region Metodos

        public bool Actualizar(CargueBrazalete modelo, out string error)
        {            
            return _repositorio.Actualizar(modelo, out error);
        }

        public bool Insertar(CargueBrazalete modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }

        public IEnumerable<CargueBrazalete> ObtenerTodos()
        {
            return _repositorio.ObtenerLista();
        }

        public IEnumerable<TipoGeneral> ObtenerTipoBrazalete()
        {
            return _repositorio.ObtenerTipoBrazalete();
        }
                
        public CargueBrazalete Crear(CargueBrazalete modelo)
        {
            throw new NotImplementedException();
        }

        public CargueBrazalete Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public bool Actualizar(CargueBrazalete modelo)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
