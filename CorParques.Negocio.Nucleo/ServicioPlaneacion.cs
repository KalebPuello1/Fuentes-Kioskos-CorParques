using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;
using CorParques.Transversales.Util;

namespace CorParques.Negocio.Nucleo
{

    public class ServicioPlaneacion : IServicioPlaneacion
    {

        private readonly IRepositorioPlaneacion _repositorio;

        #region Constructor

        public ServicioPlaneacion(IRepositorioPlaneacion repositorio)
        {

            _repositorio = repositorio;
        }

        #endregion

        #region Metodos

        public bool Actualizar(Planeacion modelo, out string strError)
        {
            strError = string.Empty;
            Planeacion objPlaneacion = new Planeacion();

            try
            {

                objPlaneacion = _repositorio.Obtener(modelo.IdPlaneacion);
                objPlaneacion.IdPlaneacion = modelo.IdPlaneacion;
                objPlaneacion.Fecha = DateTime.Parse(modelo.FechaTexto.Replace("-", "/").ToString());
                objPlaneacion.IdUsuarioModificacion = modelo.IdUsuarioModificacion;
                objPlaneacion.FechaModificacion = modelo.FechaModificacion;
                objPlaneacion.Valor = modelo.Valor;
                _repositorio.Actualizar(ref objPlaneacion);

            }
            catch (Exception ex)
            {
                strError = ex.Message;
                Utilidades.RegistrarError(ex, "ServicioPlaneacion_Crear");
            }

            return string.IsNullOrEmpty(strError);
        }

        public Planeacion Crear(Planeacion modelo, out string strError)
        {
            strError = string.Empty;

            try
            {
                modelo.Fecha = DateTime.Parse(modelo.FechaTexto.Replace("-", "/").ToString());
                modelo.IdPlaneacion = _repositorio.Insertar(ref modelo);
                return modelo;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                Utilidades.RegistrarError(ex, "ServicioPlaneacion_Crear");
                return null;
            }
        }


        public Planeacion Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        /// <summary>
        /// RDSH: Retorna una planeacion filtrada por id indicador y fecha.
        /// </summary>
        /// <param name="intIdIndicador"></param>
        /// <param name="strFecha"></param>
        /// <returns></returns>
        public IEnumerable<PlaneacionAuxiliar> ConsultarPlaneacion(int intIdIndicador, string strFecha)
        {            
            return _repositorio.ConsultarPlaneacion(intIdIndicador, strFecha);
        }

        /// <summary>
        /// RDSH: Obtiene la lista de indicadores.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerListaIndicadores()
        {
            return _repositorio.ObtenerListaIndicadores();
        }

        public IEnumerable<Planeacion> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// RDSH: Inserta la planeacion mensual.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(Planeacion modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }

        #endregion

        #region Metodos no implementados

        public bool Actualizar(Planeacion modelo)
        {
            throw new NotImplementedException();
        }

        public Planeacion Crear(Planeacion modelo)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
