using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{

    public class RepositorioAuxiliarPunto : RepositorioBase<AuxiliarPunto>, IRepositorioAuxiliarPunto
    {

        #region Metodos

        /// <summary>
        /// RDSH: Actualiza la fecha de modificacion del auxiliar punto.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(AuxiliarPunto modelo, out string error)
        {
            error = string.Empty;

            try
            {
                error = _cnn.Query<string>("SP_ActualizarAuxiliarPunto", param: new
                {
                    IdPunto = modelo.IdPunto,
                    IdEstructuraEmpleado = modelo.IdEstructuraEmpleado,
                    IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                    FechaModificacion = modelo.FechaModificacion
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_RepositorioAuxiliarPunto: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: Inserta un auxiliar punto.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(AuxiliarPunto modelo, out string error)
        {
            error = string.Empty;

            try
            {
                error = _cnn.Query<string>("SP_InsertarAuxiliarPunto", param: new
                {
                    IdPunto = modelo.IdPunto,
                    IdEstructuraEmpleado = modelo.IdEstructuraEmpleado,
                    IdUbicacionPunto = modelo.IdUbicacionPunto,
                    Certificado = modelo.Certificado,
                    IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                    FechaCreacion = modelo.FechaCreacion
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_RepositorioAuxiliarPunto: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDHS: Retorna la lista de auxiliares sin fecha de modificacion asociados al punto enviado.
        /// </summary>
        /// <param name="intIdPunto"></param>
        /// <returns></returns>
        public IEnumerable<AuxiliarPunto> ObtenerListaAuxiliarPunto(int intIdPunto)
        {
            var objListaAuxiliarPunto = _cnn.Query<AuxiliarPunto>("SP_ObtenerAuxiliarPunto", param: new
            {
                IdPunto = intIdPunto
            }, commandType: System.Data.CommandType.StoredProcedure);

            return objListaAuxiliarPunto;

        }

        /// <summary>
        /// RDSH: Consulta la informacion de un empleado para agregarlo a los auxiliares de la atraccion.
        /// </summary>
        /// <param name="intIdPunto"></param>
        /// <param name="strDocumento"></param>
        /// <returns></returns>
        public EstructuraEmpleado ObtenerInformacionAuxiliar(int intIdPunto, string strDocumento)
        {
            var objEstructuraEmpleado = _cnn.Query<EstructuraEmpleado>("SP_ObtenerInformacionAuxiliar", param: new
            {
                IdPunto = intIdPunto,
                Documento = strDocumento
            }, commandType : System.Data.CommandType.StoredProcedure).FirstOrDefault();

            return objEstructuraEmpleado;
        }

        #endregion

    }
}
