using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using CorParques.Transversales.Util;

namespace CorParques.Datos.Dapper
{
    public class RepositorioCentroImpresion : IRepositorioCentroImpresion
    {
        #region Propiedades

        private readonly SqlConnection _cnn;

        #endregion Propiedades

        #region Constructor

        public RepositorioCentroImpresion()
        {
            this._cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        #endregion Constructor

        public int? InsertarSolicitudImpresion(SolicitudBoleteria modelo)
        {
            //return this._cnn.Insert<int>(modelo);
            var rta = _cnn.Query<int>(sql: "SP_InsertarSolicitudBoleteria",
                                       commandType: CommandType.StoredProcedure,
                                       param: new
                                       {
                                           IdProducto = modelo.IdProducto,
                                           Cantidad = modelo.Cantidad,
                                           FechaUsoInicial = modelo.FechaUsoInicial,
                                           FechaUsoFinal = modelo.FechaUsoFinal,
                                           IdEstadoMaterial = modelo.IdEstadoMaterial,
                                           IdEstado = modelo.IdEstado,
                                           Observaciones = modelo.Observaciones,
                                           IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                                           Valor = modelo.Valor,
                                           IdEstadoBoleta = modelo.IdEstadoBoleta
                                       });

            return rta != null ? rta.First() : 0;
        }

        public IEnumerable<SolicitudBoleteria> ObtenerListSolicitudBoleteria(int idUsuario)
        {
            var rta = _cnn.Query<SolicitudBoleteria>(
                sql: "SP_ObtenerSolicitudesImpresionUsuario",
                param: new { IdUsuarioCreacion = idUsuario },
                commandType: CommandType.StoredProcedure
                );

            return rta;

        }

        public IEnumerable<SolicitudBoleteria> ObtenerTodasSolicitudes()
        {
            return _cnn.Query<SolicitudBoleteria>(
               sql: "SP_ObtenerSolicitudesImpresion",
               commandType: CommandType.StoredProcedure
               );
        }

        /// <summary>
        /// Genera la boleteria de un pedido de centro de impresión.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        public IEnumerable<SolicitudBoleteria> GestionarCentroImpresion(SolicitudBoleteria modelo)
        {

            try
            {
                var Lista = _cnn.Query<SolicitudBoleteria>(
                                                            sql: "SP_CentroImpresion",
                                                            param: new
                                                            {
                                                                IdSolicitudBoleteria = modelo.IdSolicitudBoleteria,
                                                                IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                                                                IdEstado = (int)Enumerador.Estados.Procesado,
                                                                IdEstadoBoleta = modelo.IdEstado,
                                                                EsBoletaControl = modelo.EsBoletaControl,
                                                                Procesar = modelo.Procesar
                                                            },
                                                            commandType: CommandType.StoredProcedure,
                                                            commandTimeout: 900000
                                                            );

                return Lista;
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioCentroImpresion_GestionarCentroImpresion");
                return null;
            }


        }

        public bool EliminarSolicitudImpresion(SolicitudBoleteria modelo)
        {
            var rta = _cnn.Query<bool>(
                sql: "SP_EliminarSolicitudImpresion",
                param: new
                {
                    EsBoletaControl = modelo.EsBoletaControl,
                    IdSolicitud = modelo.IdSolicitudBoleteria,
                    IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                    IdEstado = (int)Enumerador.Estados.Inactivo
                },
                commandType: CommandType.StoredProcedure
                );

            return rta.First();
        }


        #region Métodos
        #endregion Métodos
    }
}
