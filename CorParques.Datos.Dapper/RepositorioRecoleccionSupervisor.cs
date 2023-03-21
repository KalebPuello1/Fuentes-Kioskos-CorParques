using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using CorParques.Transversales.Util;

namespace CorParques.Datos.Dapper
{
    public class RepositorioRecoleccionSupervisor : IRepositorioRecoleccionSupervisor
    {
        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioRecoleccionSupervisor()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion

        #region Metodos


        public bool ActualizarRecoleccionMonetaria(DetalleRecoleccionMonetaria modelo, out string error)
        {
            try
            {
                error = _cnn.Query<string>("SP_ActualizarRecoleccionMonetaria", param: new
                {
                    IdDetalleRecoleccionMonetaria = modelo.IdDetalleRecoleccionMonetaria,
                    IdRecoleccion = modelo.IdRecoleccion,
                    IdUsuarioSupervisor = modelo.IdUsuarioSupervisor,
                    CantidadSupervisor = modelo.CantidadSupervisor,
                    IdUsuarioNido = modelo.IdUsuarioNido,
                    CantidadNido = modelo.CantidadNido,
                    FechaModificacion = modelo.FechaModificacion,
                    IdEstado = modelo.IdEstado
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                return string.IsNullOrEmpty(error);
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_DetalleRecoleccionMonetaria", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        public bool ActualizarRecoleccionDocumentos(DetalleRecoleccionDocumento modelo, out string error)
        {
            try
            {
                error = _cnn.Query<string>("SP_ActualizarRecoleccionDocumentos", param: new
                {
                    IdDetalleRecoleccionDocumento = modelo.IdDetalleRecoleccionDocumento,
                    RevisionSupervisor = modelo.RevisionSupervisor,
                    IdUsuarioSupervisor = modelo.IdUsuarioSupervisor,
                    RevisionNido = modelo.RevisionNido,
                    IdUsuarioNido = modelo.IdUsuarioNido,
                    FechaModificacion = modelo.FechaModificacion,
                    IdRecoleccion = modelo.IdRecoleccion,
                    IdEstado = modelo.IdEstado
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                return string.IsNullOrEmpty(error);
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_DetalleRecoleccionDocumento", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        public bool ActualizarRecoleccion(Recoleccion modelo, out string error)
        {
            try
            {
                error = _cnn.Query<string>("SP_ActualizaRecoleccion", param: new
                {
                    IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                    FechaModificacion = modelo.FechaModificacion,
                    IdRecoleccion = modelo.IdRecoleccion,
                    IdUsuarioSupervisor = modelo.IdUsuarioSupervisor,
                    IdUsuarioNido = modelo.IdUsuarioNido,
                    IdEstado = modelo.IdEstado
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                return string.IsNullOrEmpty(error);
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_Recoleccion", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        public Recoleccion ObtenerRecoleccionActiva(int intIdUsuario, int intIdPunto, int IdEstado)
        {
            var objRecoleccion = _cnn.Query<Recoleccion>("SP_ObtenerRecoleccion", param: new
            {
                IdPunto = intIdPunto,
                IdUsuario = intIdUsuario,
                IdEstado = IdEstado
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return objRecoleccion;
        }

        public bool InsertaObservacion(ObservacionRecoleccion modelo, out string error, out int IdRecoleccion)
        {
            IdRecoleccion = 0;

            try
            {
                error = _cnn.Query<string>("SP_InsertarObservacionRecoleccion", param: new
                {
                    IdRecoleccion = modelo.IdRecoleccion,
                    Observacion = modelo.Observacion,
                    IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                    FechaCreacion = modelo.FechaCreacion
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                if (error.Trim().Length > 0)
                {
                    int.TryParse(error, out IdRecoleccion);
                    if (IdRecoleccion > 0)
                    {
                        error = string.Empty;
                    }
                }
                else
                {
                    throw new ArgumentException("No fue posible guardar la recolección");
                }

            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_ObservacionRecoleccion: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        public IEnumerable<MediosPagoFactura> ObtenerDocumentos(int intIdUsuario)
        {
            var objMediosPagoFactura = _cnn.Query<MediosPagoFactura>("SP_ObtenerRecoleccionDocumentosSupervisor", param: new
            {
                IdUsuario = intIdUsuario
            }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return objMediosPagoFactura;
        }

        public bool ActualizarRecoleccionNovedades(DetalleRecoleccionNovedad modelo, out string error)
        {
            try
            {
                error = _cnn.Query<string>("SP_ActualizarRecoleccionNovedades", param: new
                {
                    IdDetalleRecoleccionNovedad = modelo.IdDetalleRecoleccionNovedad,
                    RevisionSupervisor = modelo.RevisionSupervisor,
                    IdUsuarioSupervisor = modelo.IdUsuarioSupervisor,
                    RevisionNido = modelo.RevisionNido,
                    IdUsuarioNido = modelo.IdUsuarioNido,
                    FechaModificacion = modelo.FechaModificacion,
                    IdRecoleccion = modelo.IdRecoleccion,
                    IdEstado = modelo.IdEstado,
                    IdNovedadArqueo = modelo.IdNovedadArqueo
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                return string.IsNullOrEmpty(error);
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_DetalleRecoleccionNovedad", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        public IEnumerable<NovedadArqueo> ObtenerNovedadPorIdUsuario(int IdUsuario)
        {
            var objNovedad = _cnn.Query<NovedadArqueo>("SP_ObtenerNovedadesRecoleccionSupervisor", param: new
            {
                IdUsuario = IdUsuario
            }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return objNovedad;
        }

        public IEnumerable<DetalleRecoleccion> ObtenerDetalleRecoleccion(int IdApertura, int TipoConsulta)
        {
            var objDetalle = _cnn.Query<DetalleRecoleccion>("SP_ObtenerDetalleRecoleccion", param: new
            {
                IdApertura = IdApertura,
                TipoConsulta = TipoConsulta
            }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return objDetalle;
        }

        public string RegresarEstado(int idEstado, int idApertura)
        {
            var retorno = _cnn.Query<string>("SP_RegrersarEstado", param: new
            {
                IdApertura = idApertura,
                IdEstado = idEstado
            }, commandType: System.Data.CommandType.StoredProcedure).ToList().FirstOrDefault();
            return retorno;
        }

        #endregion
    }
}
