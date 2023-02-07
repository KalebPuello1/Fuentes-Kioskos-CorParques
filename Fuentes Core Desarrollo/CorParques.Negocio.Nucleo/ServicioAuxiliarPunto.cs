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

    public class ServicioAuxiliarPunto : IServicioAuxiliarPunto
    {

        private readonly IRepositorioAuxiliarPunto _repositorio;

        #region Constructor
        public ServicioAuxiliarPunto(IRepositorioAuxiliarPunto repositorio)
        {

            _repositorio = repositorio;
        }
        #endregion

        #region Metodos

        /// <summary>
        ///RDSH: Actualiza la fecha de modificacion de un auxiliar.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(AuxiliarPunto modelo, out string error)
        {
            return _repositorio.Actualizar(modelo, out error);
        }

        /// <summary>
        ///RDSH: Inserta una asociacion de auxiliares a un punto.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(AuxiliarPunto modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }

        /// <summary>
        /// RDSH: Lista de auxiliares asociados al punto.
        /// </summary>
        /// <param name="intIdPunto"></param>
        /// <returns></returns>        
        public IEnumerable<AuxiliarPunto> ObtenerListaAuxiliarPunto(int intIdPunto)
        {
            return _repositorio.ObtenerListaAuxiliarPunto(intIdPunto);
        }

        /// <summary>
        /// RDSH: Consulta la informacion de un empleado para agregarlo a los auxiliares de la atraccion.
        /// </summary>
        /// <param name="intIdPunto"></param>
        /// <param name="strDocumento"></param>
        /// <returns></returns>
        public EstructuraEmpleado ObtenerInformacionAuxiliar(int intIdPunto, string strDocumento)
        {
            return _repositorio.ObtenerInformacionAuxiliar(intIdPunto, strDocumento);
        }

        #endregion

        #region MetodosDapper

        public AuxiliarPunto Crear(AuxiliarPunto modelo)
        {
            throw new NotImplementedException();
        }

        public AuxiliarPunto Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AuxiliarPunto> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        public bool Actualizar(AuxiliarPunto modelo)
        {
            throw new NotImplementedException();
        }
            
        #endregion

    }
}
