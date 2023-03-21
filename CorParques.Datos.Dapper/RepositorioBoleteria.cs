using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Transversales.Util;

namespace CorParques.Datos.Dapper
{

    public class RepositorioBoleteria : RepositorioBase<Boleteria>, IRepositorioBoleteria
    {
        /// <summary>
        /// RDSH: Inserta una boleta.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public string InsertarBoleteriaApp(int tipoBoleta, int producto, double precio)
        {

            try
            {
                return _cnn.Query<string>("SP_InsertarBrazaleteApp", param: new
                {
                    TipoBoleteria = tipoBoleta,
                    IdProducto = producto,
                    Valor = precio
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return string.Concat("Error en Insertar_RepositorioBoleteria: ", ex.Message);
            }

        }
        /// <summary>
        /// RDSH: Inserta una boleta.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(Boleteria modelo, out string error)
        {
            error = string.Empty;

            try
            {
                error = _cnn.Query<string>("SP_InsertarBoleteria", param: new
                {
                    IdProducto = modelo.IdProducto,
                    Consecutivo = modelo.Consecutivo,
                    IdCargueBoleteria = modelo.IdSolicitudBoleteria,
                    IdEstado = modelo.IdEstado,
                    Valor = modelo.Valor,
                    CodigoSapConvenio = modelo.CodigoSapConvenio,


                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_RepositorioBoleteria: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: Actualiza la tabla de boleteria.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(Boleteria modelo, out string error)
        {

            error = string.Empty;

            try
            {
                error = _cnn.Query<string>("SP_ActualizarBoleteria", param: new
                {
                    IdBoleteria = modelo.IdBoleteria,
                    IdEstado = modelo.IdEstado,
                    Valor = modelo.Valor,
                    IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                    FechaModificacion = modelo.FechaModificacion
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_RepositorioBoleteria: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: Retorna un objeto boleteria por su consecutivo (codigo de barras).
        /// </summary>
        /// <param name="strConsecutivo"></param>
        /// <returns></returns>
        public Boleteria ObtenerPorConsecutivo(string strConsecutivo)
        {

            Boleteria objBoleteria = null;
            objBoleteria = _cnn.Query<Boleteria>("SP_ObtenerBoleteriaPorCosecutivo", param: new
            {
                Consecutivo = strConsecutivo
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            return objBoleteria;
        }
        public ConsultaMovimientoBoletaControl ConsultaMovimientoBoletaControl(string CodBarrasBoletaControl)
        {

            ConsultaMovimientoBoletaControl objBoleteria = null;
            objBoleteria = _cnn.Query<ConsultaMovimientoBoletaControl>("SP_ConsultaMovimientoBoletaControl", param: new
            {
                CodBarrasBoletaControl = CodBarrasBoletaControl
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            return objBoleteria;
        }
        


        public int NumeroUsosBoleteria(string strConsecutivo)
        {
            var objBoleteria = _cnn.Query<int>("ObtenerUsoAtraccionesHoy", param: new
            {
                Consecutivos = strConsecutivo
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            return objBoleteria;
        }
        public string ObtenerSaldoTarjetaRecargable(string strConsecutivo)
        {


            var rta = _cnn.QueryMultiple("SP_ObtenerSaldoTarjetaRecargable", param: new
            {
                Consecutivo = strConsecutivo
            }, commandType: System.Data.CommandType.StoredProcedure);
            var mensaje = rta.Read<string>().Single();
            var saldo = rta.Read<long>().Single();
            var foto = rta.Read<byte[]>().Single();
            var nombre = rta.Read<string>().Single();

            string retorno = saldo.ToString();
            retorno += $"|{(foto != null ? ("data:image/png;base64," + Convert.ToBase64String(foto)) : "")}|" + nombre;

            return (!string.IsNullOrEmpty(mensaje) ? mensaje + "|" : "") + retorno;
        }

        public string CambiarBoleta(List<Boleteria> modelo)
        {
            var rta = _cnn.Query<string>(sql: "SP_CambioBoleta",
                                         param: new
                                         {
                                             codigoBoletaOrigen = modelo[0].Consecutivo,
                                             codigoBoletaDestino = modelo[1].Consecutivo,
                                             IdPunto = modelo[0].IdProducto,
                                             IdUsuario = modelo[0].IdUsuarioCreacion
                                         }, commandType: System.Data.CommandType.StoredProcedure);
            return rta.First();
        }

        public string Cambioboleta(string codigo1)
        {
            var rta = _cnn.Query<string>(sql: "SP_obtenerCambioNombreBoleta",
                                         param: new
                                         {
                                             @NumConsecutivo = codigo1,

                                         }, commandType: System.Data.CommandType.StoredProcedure);
            return rta.First();
        }

        public Producto CambioboletaDato(string codigo1)
        {
            var rta = new Producto();
            try
            {
                rta = _cnn.Query<Producto>(sql: "SP_obtenerCambioNombreBoletaDato",
                                        param: new
                                        {
                                            @NumConsecutivo = codigo1,

                                        }, commandType: System.Data.CommandType.StoredProcedure).First();

            }
            catch (Exception)
            {
                Producto p = new Producto();
                p.AplicaImpresionLinea = false;
                p.Nombre = "No existe este producto en boleteria";
                rta = p;
                return rta;
            }
            return rta;
        }

        /// <summary>
        /// RDSH: Retorna un objeto boleteria para el ajuste de medio de pago bono regalo.
        /// </summary>
        /// <param name="strConsecutivo"></param>
        /// <returns></returns>
        public Boleteria ConsultarBonoRegalo(string strConsecutivo)
        {
            Boleteria objBoleteria = null;

            try
            {

                objBoleteria = _cnn.Query<Boleteria>("SP_ConsultarBonoRegalo", param: new
                {
                    Consecutivo = strConsecutivo
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioBoleteria_ConsultarBonoRegalo");
            }

            return objBoleteria;
        }


        //----------------------------------------------------------------------------------------------------------------------------------------------------

        public IEnumerable<ReporteBoleteria> ConsultarInventarioDia()
        {
            List<ReporteBoleteria> boleteria = new List<ReporteBoleteria>(); ;
            try
            {

                boleteria = _cnn.Query<ReporteBoleteria>("SP_ObtenerTotalesInventarioBoleteria", commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioBoleteria_ConsultarInventarioDia");
            }

            return boleteria;
        }
        public IEnumerable<RegistroFallasInvOperacion> ConsultarInventarioProdDia(int producto)
        {
            List<RegistroFallasInvOperacion> boleteria = new List<RegistroFallasInvOperacion>(); ;
            try
            {

                boleteria = _cnn.Query<RegistroFallasInvOperacion>("SP_ObtenerInventarioProd", param: new { IdProd = producto }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioBoleteria_ConsultarInventarioDia");
            }

            return boleteria;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// GALD: Inserta una boleta externa.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public string InsertarBoleteriaExterna(int tipoBoleta, int producto, double precio, DateTime fechaUso, int Origen, int usuarioCreacion)
        {

            try
            {
                return _cnn.Query<string>("SP_InsertarBoleteriaExterna", param: new
                {
                    TipoBoleteria = tipoBoleta,
                    IdProducto = producto,
                    Valor = precio,
                    FechaUso = fechaUso,
                    Origen = Origen,
                    IdUsuarioCreacion = usuarioCreacion
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return string.Concat("Error en Insertar_RepositorioBoleteria: ", ex.Message);
            }

        }

        public string RegistroRolloImpresionLinea(Producto producto)
        {
            try
            {
                _cnn.Query("SP_RegistroRolloImpresionLinea", param: new
                {
                    @Nombre = producto.Nombre,
                    @Codigo = producto.Codigo,
                    @CodigoSap = producto.CodigoSap,
                    @IdProducto = producto.IdProducto,
                    @Cantidad = producto.Cantidad,
                    @IdUsuarioModificacion = producto.IdUsuarioModificacion,
                    @Precio = producto.Precio,
                    @PrecioTotal = producto.PrecioTotal,
                    @NombreImpuesto = producto.NombreImpuesto,
                    @IdEstado = producto.IdEstado,
                    @IdTipoProducto = producto.IdTipoProducto,
                    @Entregado = producto.Entregado,
                    @IdDetalleProducto = producto.IdDetalleProducto,
                    @UsuarioCreacion = producto.UsuarioCreacion,
                    @IdPuntoDescarga = producto.IdPuntoDescarga,
                    @Origen = 4
                }, commandType: System.Data.CommandType.StoredProcedure);
                return "registro el rollo";
            }
            catch (Exception e)
            {
                return "Error registrando el rollo";
            }
        }

        public string UpdateEstadoCambioboleta(string codigo1)
        {
            try
            {
                _cnn.Query("UPDATE TB_Boleteria SET IdEstado = 2 WHERE Consecutivo = " + codigo1);
                return "Modificacion Satisfactoria";
            }
            catch (Exception e)
            {

                return "Fallo el cambio de estado en la boleta genera para cambio boleta";
            }
        }
    }
}
