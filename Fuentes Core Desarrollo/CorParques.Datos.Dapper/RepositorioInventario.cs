using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace CorParques.Datos.Dapper
{

    public class RepositorioInventario : RepositorioBase<TransladoInventario>, IRepositorioInventario
    {
        public string ActualizarInventario(Inventario Inventario)
        {
            try
            {
                var rta = _cnn.Query<string>("SP_ActualizarInventario",
                           commandType: CommandType.StoredProcedure,
                           param: new
                           {
                               IdPunto = Inventario.IdPunto,
                               FechaInventario = Inventario.FechaInventario,
                               IdUsuarioCeado = Inventario.IdUsuarioCeado,
                               FechaCreado = Inventario.FechaInventario,
                               IdProducto = Utilidades.convertTable(Inventario.Productos
                                                            .Select(x => new TablaGeneral
                                                            {
                                                                col1 = x.IdProducto.ToString(),
                                                                col2 = x.CodigoSap.ToString(),
                                                                col3 = string.IsNullOrEmpty(x.Pedido) ? null : x.Pedido.ToString(),
                                                                col4 = x.IdDetalleProducto.ToString(),
                                                                col5 = x.Posicion == 0 ? null : x.Posicion.ToString(),

                                                            }))
                           });

                return "";
            }
            catch (Exception ex)
            {
                return string.Concat("Error al actualizar el inventario: ", ex.Message);
            }
        }
        public string RegistrarSalidaPedido(int idusuario, string pedido, int idUsuarioRecibe)
        {
            try
            {
                var rta = _cnn.Query<string>("SP_SalidaPedido",
                           commandType: CommandType.StoredProcedure,
                           param: new
                           {
                               IdUsuarioCreado = idusuario,
                               IdUsuarioRecibe = idUsuarioRecibe,
                               CodSapPedido = pedido
                           });

                return "";
            }
            catch (Exception ex)
            {
                return string.Concat("Error al actualizar el inventario: ", ex.Message);
            }
        }

        public IEnumerable<MotivosInventario> BuscarMotivosInventario()
        {
            var _list = _cnn.Query<MotivosInventario>("select * from TB_MotivosInventario ");
            return _list;
        }
        public string ActualizarAjustesInventario(IEnumerable<AjustesInventario> AjusteInventario)
        {
            try
            {
                var rta = _cnn.Query<string>("SP_ActualizarAjusteInventario",
                           commandType: CommandType.StoredProcedure,
                           param: new
                           {
                               Fecha = System.DateTime.Now,
                               AjusteInventario = Utilidades.convertTable(AjusteInventario
                                                            .Select(x => new TablaGeneral
                                                            {
                                                                col1 = x.CodSapMaterial.ToString(),
                                                                col2 = x.Cantidad.ToString(),
                                                                col3 = x.IdPunto.ToString(),
                                                                col4 = x.CodSapTipoAjuste.ToString(),
                                                                col5 = x.CodSapMotivo.ToString(),
                                                                col6 = x.IdUsuarioRegistro.ToString(),
                                                                col7 = x.IdUsuarioAjuste.ToString(),
                                                                col8 = x.Observaciones.ToString(),
                                                                col9 = x.UnidadMedida.ToString(),
                                                            }))
                           });

                return "";
            }
            catch (Exception ex)
            {
                return string.Concat("Error al realizar el ajuste: ", ex.Message);
            }
        }


        public string ActualizarTransladoInventario(IEnumerable<TransladoInventario> TransladoInventario)
        {
            try
            {
                DateTime FechaNow = System.DateTime.Now;

                var rta = _cnn.Query<string>("SP_ActualizarTransladoInventario",
                           commandType: CommandType.StoredProcedure,
                           param: new
                           {
                               Fecha = FechaNow,
                               TransladoInventario = Utilidades.convertTable(TransladoInventario
                                                            .Select(x => new TablaGeneral
                                                            {
                                                                col1 = x.CodSapMaterial.ToString(),
                                                                col2 = x.Cantidad.ToString(),
                                                                col3 = x.IdPuntoOrigen.ToString(),
                                                                col4 = x.IdPuntoDestino.ToString(),
                                                                col5 = x.idUsuario.ToString(),
                                                                col6 = x.Procesado.ToString(),
                                                                col7 = x.UnidadMedida.ToString(),
                                                                col8 = x.IdUsuarioRegistro.ToString(),
                                                                col9 = string.IsNullOrEmpty(x.Pedido) ? null : x.Pedido.ToString()
                                                            }))
                           });

                return "";
            }
            catch (Exception ex)
            {
                return string.Concat("Error al actualizar el inventario: ", ex.Message);
            }
        }

        public int InsertarAjusteInventario(AjustesInventario Ajuste)
        {
            int resp = _cnn.Insert<int>(Ajuste);
            return resp;
        }

        public IEnumerable<TipoAjusteInventario> ObtenerTiposAjuste()
        {
            var lista = _cnn.GetList<TipoAjusteInventario>();
            return lista;
        }

        public IEnumerable<MotivosInventario> ObtenerMotivosAjuste(string CodSapAjuste)
        {
            var lista = _cnn.GetList<MotivosInventario>($"WHERE CodSapAjuste = '{CodSapAjuste}'");
            return lista;
        }

        public IEnumerable<MotivosInventario> ObtenerTodosMotivos()
        {
            var lista = _cnn.GetList<MotivosInventario>();
            return lista;
        }

        public IEnumerable<ResumenCierre> ObtenerResumenCierre(DateTime? Fecha)
        {
            Fecha = (Fecha == null) ? System.DateTime.Now : Fecha;

            return _cnn.Query<ResumenCierre>("SP_ObtenerResumenCierre", new { Fecha = Fecha }, commandType: System.Data.CommandType.StoredProcedure).ToList();

        }

        public string ObtenerCodSapAlmacen(int idPunto)
        {
            return _cnn.Query<string>("SP_ObtenerCodSapAlmacen", new { @idPunto = idPunto }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
        }
        public IEnumerable<SolicitudRetorno> ObtenerSolicitudesDevolucion()
        {
            return _cnn.Query<SolicitudRetorno>("SP_ConsultarSolicitudesRetorno", commandType: System.Data.CommandType.StoredProcedure).ToList();
        }
        public string ActualizarPedidoTrasladado(int idPunto, string pedido)
        {
            return _cnn.Query<string>("SP_ActualizarTransladoPedido", new { @CodSapPedido = pedido, @IdPunto = idPunto }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
        }
        public IEnumerable<SolicitudRetorno> ConsultarPedidoRetorno(string pedido)
        {
            return _cnn.Query<SolicitudRetorno>("SP_ConsultarPedidoRetorno", new { @Pedido = pedido }, commandType: System.Data.CommandType.StoredProcedure);
        }
        public IEnumerable<TipoGeneral> ConsultarMotivosRetorno()
        {
            return _cnn.Query<TipoGeneral>("SP_ConsultarMotivosRetorno", commandType: System.Data.CommandType.StoredProcedure);
        }
        public string CrearSolicitudRetorno(SolicitudRetorno modelo)
        {
            return _cnn.Query<string>("SP_CrearSolicitudPedidoRetorno", new
            {
                @Pedido = modelo.CodSapPedido,
                @Usuario = modelo.UsuarioCrea
                                                                        ,
                @Motivo = int.Parse(modelo.Motivo),
                @Observacion = modelo.Observacion,
                @Accion = modelo.Id
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
        }



        public string InsertarDetalleInventarioFisico(IEnumerable<Materiales> _Materiales)
        {
            Materiales ObjInsercion = new Materiales();
            string producto;

            var CodigoSap = "";
            DateTime Fecha_Inventario;
            var InventarioTeorico = "";
            var InventarioFisico = "";
            var Diferencia = "";
            var Tipo_Movimiento = "";
            var CodSapAlmacen = "";
            int IdPunto = 0;
            var Observaciones = "";
            int IdUsuario = 0;


            try
            {

                foreach (var item in _Materiales)
                {
                    CodigoSap = item.CodigoSap;
                    Fecha_Inventario = item.FechaInventario;
                    InventarioTeorico = item.CantidadTeorica;
                    InventarioFisico = item.CantidadFisica;
                    Diferencia = item.Diferencia;
                    Tipo_Movimiento = item.TipoMovimiento;
                    CodSapAlmacen = item.CodigoSapAlmacen;
                    IdPunto = item.id_Punto;
                    Observaciones = item.Observacion;
                    IdUsuario = item.Id_Supervisor;


                    _cnn.Query("SP_InsertarDetalleInventarioFisico", commandType: System.Data.CommandType.StoredProcedure, param: new
                    {

                        @CodigoSap = CodigoSap,
                        @Fecha_Inventario = Fecha_Inventario,
                        @InventarioTeorico = InventarioTeorico,
                        @InventarioFisico = InventarioFisico,
                        @Diferencia = Diferencia,
                        @TipoMovimiento = Tipo_Movimiento,
                        @CodSapAlmacen = CodSapAlmacen,
                        @IdPunto = IdPunto,
                        @Observaciones = Observaciones,
                        @IdUsuario = IdUsuario

                    });

                }

                producto = "Exitoso";
            }

            catch (Exception e)
            {
                producto = "fallo";

            }
            return producto;
        }
    }
}
