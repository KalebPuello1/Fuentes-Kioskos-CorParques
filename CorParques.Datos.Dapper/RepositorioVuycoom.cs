using Corparques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace CorParques.Datos.Dapper
{
    public class RepositorioVuycoom : IRepositorioVuycoom
    {
        private readonly SqlConnection __cnn;
        private object serviVuycoom;

        public RepositorioVuycoom()
        {
            __cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #region metodos
        public IEnumerable<Factura> facturas()
        {
            IEnumerable<Factura> _list = new List<Factura>();
            try
            {
              //_list = __cnn.Query<Factura>("SP_ObtenerCodigoFacturaTest", commandType: System.Data.CommandType.StoredProcedure);
              _list = __cnn.Query<Factura>("select DISTINCT CodigoFactura from TB_Factura");
            } catch (Exception e)
            {
                throw e;
            }
            return _list;
        }


        /*public string InsertarFactura(string vyucoom)
        {
            string[] datos = vyucoom.Split('|');
            var dat = "";
            try
            {
                dat = __cnn.Query<string>("SP_InsertaFacturaVerTest", commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    Id_Punto = datos[0],
                    CodFactura = datos[1],
                    Id_Usuario = datos[2],
                    FechaCreacion = DateTime.Now
                }).First();
            }
            catch (Exception e)
            {
                dat = "Fallo insertar y traer dato " + e.Message;
            }

            return dat;
            
        }*/

        public Vyucoom getClienteNuevo(string datoCliente) 
        {
            var dato = new Vyucoom();
            try
            {
                 dato = __cnn.Query<Vyucoom>("SP_ObtenerClientePago", commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    @CodPedido = datoCliente
                }).First();
            }
            catch (Exception e)
            {
                dato = null;
            }
            return dato;
        }

        public string InsertarFactura(Vyucoom vyucoom)
        {
            var dat = "";
            try
            {
                dat = __cnn.Query<string>("SP_InsertaFacturaVerTest",
                commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    //Id_Factura = vyucoom.Id_Factura,
                    Id_Punto = vyucoom.Id_Punto,
                    CodFactura = vyucoom.CodFactura,
                    Id_Usuario = vyucoom.Id_Usuario,
                    FechaCreacion = DateTime.Now,
                    //FechaCreacion = DateTime.ParseExact("2022-02-21","yyyy-M-dd",null),
                    /// -------------------
                    //Id_Punto = vyucoom.Id_Punto,
                    Id_Estado = vyucoom.Id_Estado,
                    //Cliente = vyucoom.Cliente,
                    Cantidad = vyucoom.Cantidad,
                    IdDetalleProducto = vyucoom.IdDetalleProducto,
                    Id_Producto = vyucoom.Id_Producto,
                    //CodFactura = vyucoom.CodFactura,
                    precio = vyucoom.Precio,
                    /// ----------------------
                }).First();
            }
            catch (Exception e)
            {
                dat = "Fallo insertar y traer dato " + e.Message;
            }

            return dat;
            
        }

        public int getNumeroFactuar()
        {
            var NumFacura = __cnn.Query<int>("SP_ObtenerNumFacturaTest", commandType: System.Data.CommandType.StoredProcedure).First();
            return NumFacura;
        }
        public string InsertarDato(Vyucoom vyucoom)
        {
            var dato = "";
            try
            {
                string tiempo = DateTime.Now.ToString();
                __cnn.Query<Vyucoom>("SP_InsertarFacturacionTest",
                    commandType: System.Data.CommandType.StoredProcedure,
                    param: new
                    {
                        Id_Factura = vyucoom.Id_Factura,
                        //   SI  Id_Punto = vyucoom.Id_Punto,
                        Id_Usuario = vyucoom.Id_Usuario,
                        //   NO  Id_Apertura = vyucoom.Id_Apertura,
                        //   SI  Id_Estado = vyucoom.Id_Estado,
                        //   NO  FechaCreacion = vyucoom.FechaCreacion,
                        //   NO  FechaCreacion = DateTime.ParseExact("2023-01-01", "yyyy-M-d", null),
                        FechaCreacion = DateTime.Now,
                        IdMedioPago = vyucoom.IdMedioPago,
                        MontoPago = vyucoom.MontoPago,
                        //   SI  Cliente = vyucoom.Cliente,
                        //   SI  Cantidad = vyucoom.Cantidad,
                        //   SI  IdDetalleProducto = vyucoom.IdDetalleProducto,
                        Cambio = vyucoom.Cambio,
                        NumReferencia = vyucoom.NumReferencia
                        //   SI  Id_Producto = vyucoom.Id_Producto,
                        //   SI  CodFactura = vyucoom.CodFactura,
                        //   SI  precio = vyucoom.Precio
                    });
                dato = "Inserto correctamente";
            }
            catch (Exception e)
            {
                string tiempo = DateTime.Now.ToString();
                __cnn.Query<Vyucoom>("SP_InsertarFacturacionTest",
                    commandType: System.Data.CommandType.StoredProcedure,
                    param: new
                    {
                        Id_Factura = vyucoom.Id_Factura,
                        //   SI  Id_Punto = vyucoom.Id_Punto,
                        Id_Usuario = vyucoom.Id_Usuario,
                        //   NO  Id_Apertura = vyucoom.Id_Apertura,
                        //   SI  Id_Estado = vyucoom.Id_Estado,
                        //   NO  FechaCreacion = vyucoom.FechaCreacion,
                        //   NO  FechaCreacion = DateTime.ParseExact("2023-01-01", "yyyy-M-d", null),
                        FechaCreacion = DateTime.Now,
                        IdMedioPago = vyucoom.IdMedioPago,
                        MontoPago = vyucoom.MontoPago,
                        //   SI  Cliente = vyucoom.Cliente,
                        //   SI  Cantidad = vyucoom.Cantidad,
                        //   SI  IdDetalleProducto = vyucoom.IdDetalleProducto,
                        Cambio = vyucoom.Cambio,
                        NumReferencia = vyucoom.NumReferencia
                        //   SI  Id_Producto = vyucoom.Id_Producto,
                        //   SI  CodFactura = vyucoom.CodFactura,
                        //   SI  precio = vyucoom.Precio

                        /* Id_Factura = vyucoom.Id_Factura,
                         Id_Punto = vyucoom.Id_Punto,
                         Id_Usuario = vyucoom.Id_Usuario,
                         //Id_Apertura = vyucoom.Id_Apertura,
                         Id_Estado = vyucoom.Id_Estado,
                         //FechaCreacion = vyucoom.FechaCreacion,
                         //FechaCreacion = DateTime.ParseExact("2023-01-01", "yyyy-M-d", null),
                         FechaCreacion = DateTime.Now,
                         IdMedioPago = vyucoom.IdMedioPago,
                         MontoPago = vyucoom.MontoPago,
                         Cliente = vyucoom.Cliente,
                         Cantidad = vyucoom.Cantidad,
                         IdDetalleProducto = vyucoom.IdDetalleProducto,
                         Cambio = vyucoom.Cambio,
                         NumReferencia = vyucoom.NumReferencia,
                         Id_Producto = vyucoom.Id_Producto,
                         CodFactura = vyucoom.CodFactura,
                         precio = vyucoom.Precio */
                    });
                dato = "Fallo insertando dato " + e.Message;
            }

            return dato;
        }
       

        public Parametro BuscarPrecioFijo()
        {
            Parametro _list = __cnn.Query<Parametro>("SELECT * FROM TB_PARAMETROS WHERE NOMBRE = 'PrecioFijo'").FirstOrDefault();
            return _list;
        }

        public IEnumerable<Apertura> apertura()
        {
            IEnumerable<Apertura> apertura = new List<Apertura>();
            return apertura = __cnn.Query<Apertura>("select IdApertura from TB_Apertura");
        }

        public int pedido(string dato)
        {
            var pedido = 0;
            try
            {
                pedido = (int) __cnn.Query<int?>("SP_ObtenerPedidoTestDelet", commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    TipDato = 0,
                    CodSapPedido = dato
                }).FirstOrDefault();
            }
            catch (Exception e)
            {
                return pedido;
            }
            return pedido;
        }
        
        public string NCliPedido(string dato)
        {
            var pedido = "";
            try
            {
                pedido =  __cnn.Query<string>("SP_ObtenerNCliPedidoTest", commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    NPedido = dato
                }).FirstOrDefault();
            }
            catch (Exception e)
            {
                return pedido;
            }
            return pedido;
        }
        public string InsertarRecCaja(Vyucoom dato)
        {
            var pedido = "Inserto el dato correctamente";
            try
            {
                __cnn.Query("SP_InsertarReciboCaja", commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    @Nombres = dato.Cliente,
                    @TipoDoc = dato.TipDocumento,
                    @Documento = dato.Documento,
                    @IdFactura = dato.IdFactura,
                    @MontoPagado = dato.MontoPagado,
                    @MontoAPagar = dato.MontoAPagar,
                    @CodPedidio = dato.CodPedido
                }).FirstOrDefault();
            }
            catch (Exception e)
            {
                pedido = "No inserto el dato";
            }
            return pedido;
        }


        public string ModificarPagoReciboCaja(Vyucoom cliente)
        {
            string modifico = "";
            string codVer = cliente.CodPedido.ToString();
            Vyucoom datoSumar = getClienteNuevo(codVer);
            try
            {
                modifico = __cnn.Query<string>("SP_ActualizarClienteReciboCaja", commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    @CodPedido = cliente.CodPedido,
                    @MontoPago = cliente.MontoPagado + datoSumar.MontoPagado,
                    @MontoDebe = cliente.MontoAPagar
                }).First();
            }
            catch (Exception e)
            {
                modifico = "Fallo la modificacion";
                throw;
            }
            return modifico;
        }


        public IEnumerable<Vyucoom>[] Reimprimir(string Npedido)
        {
            IEnumerable<Vyucoom>[] vyu = new IEnumerable<Vyucoom>[3];
            try
            {
                var datoRe = __cnn.QueryMultiple("SP_ObtenerReimpresionReciboCajaVyucoom", commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    @NumPedido = Npedido
                }
                );
                vyu[0] = datoRe.Read<Vyucoom>().ToList();
                vyu[1] = datoRe.Read<Vyucoom>().ToList();
                vyu[2] = datoRe.Read<Vyucoom>().ToList();
            }
            catch (Exception)
            {
                Vyucoom Completar = new Vyucoom();
                Completar.Cantidad = 1;
                Completar.CodPedido = 222222;
                Completar.CodFactura = "22222";
                Completar.Fecha = DateTime.Now;
                Completar.IdMedioPago = "0";
                Completar.IdPunto = 0;
                Completar.IdUsuarioCreacion = 0;
                Completar.Id_Factura = "0";
                Completar.Valor = "0";
                Completar.FechaCreacion = DateTime.Now;
                List<Vyucoom> vyuu = new List<Vyucoom>();
                vyuu.Add(Completar);
                vyu[0] = null;
                vyu[1] = vyuu;
                Completar = new Vyucoom();
                vyuu = new List<Vyucoom>();
                Completar.Precio = 10;
                vyuu.Add(Completar);
                vyu[2] = vyuu;
            }
            return vyu;
        }

        public Vyucoom UsuarioReciboCajaVyucoom(string dato)
        {
            Vyucoom usuarioCaja = new Vyucoom();
                try
               {
                 usuarioCaja = __cnn.Query<Vyucoom>("SP_ObtenerUsuarioReciboCajaVyucoom", commandType: System.Data.CommandType.StoredProcedure,
                   param: new
                 {
                   IdFactura = dato
                 }).First();
               }
               catch (Exception)
               {
                Vyucoom vyu = new Vyucoom();
                vyu.Nombres = "No esta registrado";
                vyu.IdFactura = "22222";
                vyu.Documento = "00000";
                vyu.TipDocumento = "CC";
                usuarioCaja = vyu;
               }
            return usuarioCaja;
        }

        public string UsuarioPorId(string id)
        {
            string usuario = "";
            try
            {
              usuario =  __cnn.Query<string>("SP_ObtenerUsuarioPorId", commandType: System.Data.CommandType.StoredProcedure,
                    param: new
                    {
                        @IdUsuario = id
                    }).First();
            }
            catch (Exception e)
            {
                usuario = "No encontro usuario";
            }
            return usuario;
        }
        
        public string PuntoPorId(string id)
        {
            string punto = "";
            try
            {
                punto =  __cnn.Query<string>("SP_ObtenerNombrePunto", commandType: System.Data.CommandType.StoredProcedure,
                    param: new
                    {
                        @IdPunto = id
                    }).First();
            }
            catch (Exception e)
            {
                punto = "No encontro Punto venta";
            }
            return punto;
        }
        #endregion
    }
}
