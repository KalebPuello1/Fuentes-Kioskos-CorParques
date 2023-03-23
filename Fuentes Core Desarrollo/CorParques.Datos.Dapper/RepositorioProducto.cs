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

namespace CorParques.Datos.Dapper
{

    public class RepositorioProducto : RepositorioBase<Producto>, IRepositorioProducto
    {

        public IEnumerable<Factura> ObtenerFacturaXConvenio(string consecutivo)
        {
            return null;
        }
        /// <summary>
        /// Insertar factura
        /// </summary>
        public string InsertarFactura(PagoFactura model)
        {

            var rta = _cnn.Query<string>("SP_InsertarFactura",
                       commandType: CommandType.StoredProcedure,
                       param: new
                       {
                           IdUsuario = model.IdUsuario,
                           IdPunto = model.IdPunto,
                           Contingencia = model.EsContingencia,
                           //DANR: 22-01-2019 -- Adicion campo donante
                           Donante = model.Donante,
                           Cambio = model.Cambio,
                           //fin DANR: 22-01-2019 -- Adicion campo donante

                           DetalleFactura = Utilidades.convertTable(model.listaProducto
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdProducto.ToString(),
                                                            col2 = x.Cantidad.ToString(),
                                                            col3 = x.Precio.ToString(),
                                                            col4 = x.IdDetalleProducto.ToString(),
                                                            col5 = x.Entregado.ToString(),
                                                            //DANR: Adicion para tarjetas recargables
                                                            col8 = x.DataExtension?.ToString()

                                                        })),
                           MedioPagoFactura = Utilidades.convertTable(model.listMediosPago
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdMedioPago.ToString(),
                                                            col2 = x.Valor.ToString(),
                                                            col3 = x.NumReferencia,
                                                            col4 = x.IdFranquicia.ToString()
                                                        })),
                           BanderaBonoRegalo = model.BanderaBonoRegalo
                       });

            return rta.Single().ToString();
        }

        public int ObtenerVentasProductoCliente(string codigo, int producto)
        {
            return _cnn.Query<int>("SP_GetVentasProductoClienteDia"
                   , commandType: CommandType.StoredProcedure
                   , param: new { Consecutivo = codigo, Producto = producto }).First();
        }
        public string ValidarAlerta(int usuario, int producto)
        {
            return _cnn.Query<string>("SP_GenerarAlerta"
                   , commandType: CommandType.StoredProcedure
                   , param: new { IdUsuario = usuario, IdProducto = producto }).First();
        }
        public Producto ObtenerProducto(int idProducto)
        {
            Producto objProducto = new Producto();

            objProducto = _cnn.Query<Producto>("SP_ObtenerProducto",
                                                param: new { IdProducto = idProducto },
                                                commandType: CommandType.StoredProcedure).FirstOrDefault();

            if (objProducto != null)
            {
                objProducto.objListaRecetaProducto = ObtenerRecetaProducto(idProducto);
            }

            return objProducto;

        }
        public Producto ObtenerProductoPtoEntrega(int idProducto)
        {
            Producto objProducto = new Producto();

            objProducto = _cnn.Query<Producto>("SP_ObtenerProductoPtoEntrega",
                                                param: new { IdProducto = idProducto },
                                                commandType: CommandType.StoredProcedure).FirstOrDefault();

            if (objProducto != null)
            {
                objProducto.objListaRecetaProducto = ObtenerRecetaProducto(idProducto);
            }

            return objProducto;

        }
        public Producto ObtenerProductoPtoFactura(int idProducto)
        {
            Producto objProducto = new Producto();

            objProducto = _cnn.Query<Producto>("SP_ObtenerProductoPtoFactura",
                                                param: new { IdProducto = idProducto },
                                                commandType: CommandType.StoredProcedure).FirstOrDefault();

            if (objProducto != null)
            {
                objProducto.objListaRecetaProducto = ObtenerRecetaProducto(idProducto);
            }

            return objProducto;

        }

        public IEnumerable<TipoGeneral> ObtenerProductosDonacion()
        {
            IEnumerable<TipoGeneral> objProductos = new List<TipoGeneral>();

            objProductos = _cnn.Query<TipoGeneral>("SP_ObtenerProductosCampanasDonacion",
                                                commandType: CommandType.StoredProcedure);

            return objProductos;

        }
        /// <summary>
        /// Consultar un Brazalete por su consecutivo (código de barra)
        /// </summary>
        /// <param name="Consecutivo"></param>
        /// <returns></returns>
        public Producto ObtenerBrazaleteConsecutivo(string Consecutivo, int taquillero, int recarga)
        {
            try
            {

                var rta = _cnn.QueryMultiple("SP_ConsultarBrazaletePorConsecutivo", new { Consecutivo = Consecutivo, IdUsuario = taquillero, Accion = recarga }, commandType: System.Data.CommandType.StoredProcedure);

                Producto producto = null;

                var listRta = rta.Read<Producto>();

                if (listRta.Count() > 0)
                    producto = listRta.First();

                if (producto == null)
                {
                    producto = new Producto();
                    var rtaMensaje = rta.Read<string>();
                    producto.MensajeValidacion = rtaMensaje.Count() > 0 ? rtaMensaje.Single() : "";
                }
                else
                {
                    producto.ConseutivoDetalleProducto = Consecutivo;
                }

                return producto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //public bool ActualizarBrazaleteEstado(List<Producto> lista)
        //{
        //    try
        //    {
        //        DataTable objTableAsParam = new DataTable();
        //        objTableAsParam.Columns.Add("col1");
        //        objTableAsParam.Columns.Add("col2");

        //        objTableAsParam.Columns.Add("col3");
        //        objTableAsParam.Columns.Add("col4");
        //        objTableAsParam.Columns.Add("col5");
        //        objTableAsParam.Columns.Add("col6");
        //        objTableAsParam.Columns.Add("col7");
        //        objTableAsParam.Columns.Add("col8");
        //        objTableAsParam.Columns.Add("col9");

        //        foreach (Producto item in lista)
        //        {
        //            DataRow objRow = objTableAsParam.NewRow();
        //            objRow[0] = item.IdProducto.ToString();
        //            objRow[1] = item.IdEstado.ToString();

        //            objTableAsParam.Rows.Add(objRow);
        //        }

        //        var rta = _cnn.QueryMultiple("SP_ActualizaBrazaleteEstado", new { ListaUpdate = objTableAsParam }, commandType: System.Data.CommandType.StoredProcedure);
        //        //var producto = rta.Read<Producto>().Single();
        //        //if (producto != null)
        //        //    producto.MensajeValidacion = rta.Read<string>();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //Obtiene todos los productos
        public IEnumerable<Producto> ObtenerProductos()
        {
            IEnumerable<Producto> _lis = new List<Producto>();

            try
            {
                _lis = _cnn.Query<Producto>("SP_ObtenerProductos", commandType: CommandType.StoredProcedure);

            }
            catch
            {
                throw;
            }

            return _lis;
        }

        public IEnumerable<Producto> ObtenerProductosPtoEntrega()
        {
            IEnumerable<Producto> _lis = new List<Producto>();

            try
            {
                _lis = _cnn.Query<Producto>("SP_ObtenerProductosPtoEntrega", commandType: CommandType.StoredProcedure);

            }
            catch
            {
                throw;
            }

            return _lis;
        }
        public IEnumerable<Producto> ObtenerProductosPtoFactura()
        {
            IEnumerable<Producto> _lis = new List<Producto>();

            try
            {
                _lis = _cnn.Query<Producto>("SP_ObtenerProductosPtoFactura", commandType: CommandType.StoredProcedure);

            }
            catch
            {
                throw;
            }

            return _lis;
        }
        
        public IEnumerable<TipoGeneral> ObtenerLineaProducto()
        {
            return _cnn.GetList<LineaProducto>().Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre, CodSAP = x.CodigoSap });
        }

        public bool ActualizarProducto(Producto modelo)
        {
            var rta = _cnn.Query<bool>("SP_ActualizarProducto", commandType: CommandType.StoredProcedure, param: new
            {
                IdProducto = modelo.IdProducto,
                IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                Codigo = modelo.Codigo,
                Imagen = modelo.Imagen
                //Nombre = modelo.Nombre,
                //IdEstado = modelo.IdEstado,
                //IdLineaProducto = modelo.IdLineaProducto
            });

            return rta.Single();
        }
        public bool ActualizarProductoPuntosEntrega(Producto modelo)
        {
            var rta = _cnn.Query<bool>("SP_ActualizarProductoPuntosEntrega", commandType: CommandType.StoredProcedure, param: new
            {
                IdProducto = modelo.IdProducto,
                IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                IdPunto = modelo.hdListPuntos
                //Nombre = modelo.Nombre,
                //IdEstado = modelo.IdEstado,
                //IdLineaProducto = modelo.IdLineaProducto
            });

            return rta.Single();
        }
        public bool ActualizarProductoPuntosFactura(Producto modelo)
        {
            var rta = _cnn.Query<bool>("SP_ActualizarProductoPuntosFactura", commandType: CommandType.StoredProcedure, param: new
            {
                IdProducto = modelo.IdProducto,
                IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                IdPunto = modelo.hdListPuntos
                //Nombre = modelo.Nombre,
                //IdEstado = modelo.IdEstado,
                //IdLineaProducto = modelo.IdLineaProducto
            });

            return rta.Single();
        }

        /// <summary>
        /// Obtener Factura
        /// </summary>
        public Factura ObtenerFactura(string codigoFactura)
        {
            Factura _factura = null;

            try
            {
                var rta = _cnn.QueryMultiple("SP_ObtenerFactura", new { CodigoFactura = codigoFactura }, commandType: System.Data.CommandType.StoredProcedure);
                var factura = rta.Read<Factura>();
                if (factura != null && factura.Count() > 0)
                {
                    _factura = new Factura();
                    _factura = factura.Single();
                    _factura.DetalleFactura = rta.Read<DetalleFactura>();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _factura;
        }
        /// <summary>
        /// Obtener Factura
        /// </summary>
        public Factura ObtenerFactura(int idFactura)
        {
            Factura _factura = null;

            try
            {
                var rta = _cnn.QueryMultiple("SP_ObtenerFactura", new { IdFactura = idFactura }, commandType: System.Data.CommandType.StoredProcedure);
                var factura = rta.Read<Factura>();
                if (factura != null && factura.Count() > 0)
                {
                    _factura = new Factura();
                    _factura = factura.Single();
                    _factura.DetalleFactura = rta.Read<DetalleFactura>();
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return _factura;
        }

        public int GuardarNotaCredito(NotaCredito modelo)
        {
            int IdNotaCredito = 0;
            string IdsDetalleFactura = string.Join(
                        ",", modelo.DetalleFactura.Where(x => x.NotaCredito).Select(x => x.IdDetalleFactura));

            try
            {
                var rta = _cnn.Query<int>("SP_CrearNotaCreadito",
                                    param: new
                                    {
                                        IdsDetalleFactura = IdsDetalleFactura,
                                        Observacion = modelo.Observacion,
                                        IdUsuario = modelo.IdUsuarioCreacion,
                                        IdSupervisor = modelo.IdSupervisor,
                                        PrecioNotaCredito = Utilidades.convertTable(modelo.DetalleFactura.Select(x => new TablaGeneral { col1 = x.IdDetalleFactura.ToString(), col2 = x.PrecioNotaCredito.ToString() })),
                                        IdPunto = modelo.IdPunto
                                    },
                                        commandType: CommandType.StoredProcedure
                                    );

                IdNotaCredito = rta.Single();
            }
            catch
            {
                throw;
            }

            return IdNotaCredito;
        }

        public TipoGeneral ObtenerNotaCredito(int IdUsuario)
        {
            //var _rta = _cnn.GetList<DetalleFactura>($"WHERE IdUsuarioCreacion = {IdUsuario} AND CONVERT(VARCHAR,FechaCreacion,103) = '{DateTime.Now.ToString("dd/MM/yyyy")}' ");
            //var _notaCredito = new List<NotaCredito>();
            //foreach (var item in _rta.GroupBy(x=> x.IdNotaCredito))
            //{
            //    if(item != null)
            //    {
            //        var _nota = _cnn.Get<NotaCredito>(item.Key);
            //        if (_nota != null)
            //        {
            //            _nota.DetalleFactura = _cnn.GetList<DetalleFactura>($"WHERE IdNotaCredito = {_nota.Id}").ToList();
            //            //foreach (var item2 in _nota.DetalleFactura)
            //            //{
            //            //   var _nombre = _cnn.GetList<Producto>($"WHERE IdProducto = {item2.Id_Producto}");
            //            //    item2.Nombre = _nombre != null && _nombre.Count() > 0 ? _nombre.First().Nombre : "";
            //            //}

            //            _notaCredito.Add(_nota);
            //        }
            //    }
            //}


            var _rta = _cnn.Query<TipoGeneral>("SP_ObtenerNotaCredito",
                                                commandType: CommandType.StoredProcedure,
                                                param: new { IdUsuario = IdUsuario });

            return _rta.Single();
        }


        public TipoGeneral ObtenerAnulaciones(int IdUsuario)
        {
            var _rta = _cnn.Query<TipoGeneral>("SP_ObtenerAnulaciones",
                                                  commandType: CommandType.StoredProcedure,
                                                  param: new { IdUsuario = IdUsuario });

            return _rta.Single();
        }

        public IEnumerable<AnulacionFactura> ObtenerFacturasAnular(int idPunto)
        {
            throw new NotImplementedException();
        }

        public bool AnularFacturas(IEnumerable<AnulacionFactura> modelo)
        {
            throw new NotImplementedException();
        }

        public DateTime? ObtenerFechaPago(int IdDetalleProducto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// RDSH: Retorna la receta de un producto.
        /// </summary>
        /// <param name="intIdProducto"></param>
        /// <returns></returns>
        public IEnumerable<RecetaProducto> ObtenerRecetaProducto(int intIdProducto)
        {

            var objRecetaProducto = _cnn.Query<RecetaProducto>("SP_ObtenerRecetaProducto",
                param: new { IdProducto = intIdProducto },
                commandType: CommandType.StoredProcedure).ToList();

            return objRecetaProducto;
        }

        public IEnumerable<Producto> ObtenerProductoPorTipoProducto(string CodSapTipoProducto, bool activo = false)
        {

            try
            {
                var rta = _cnn.Query<Producto>("SP_ObtenerProductoPorTipoProducto",
                               param: new { CodSapTipoProducto = CodSapTipoProducto, EstadoActivo = activo },
                               commandType: CommandType.StoredProcedure).ToList();
                return rta;
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioProducto_ObtenerProductoPorTipoProducto");
                return null;
            }


        }

        public LineaProducto ObtenerCodSapPorTipoProducto(int IdTipoProducto)
        {
            var _rta = _cnn.GetList<LineaProducto>($"WHERE IdTipoProducto = {IdTipoProducto}");
            return _rta.Single();
        }

        public string ValidaBoletaControlProducto(string CodigoBarrasBoletaControl, string CodSapProducto)
        {
            var rta = _cnn.Query<string>("SP_ValidaBoletaControlParqueadero",
                       commandType: CommandType.StoredProcedure,
                       param: new
                       {
                           CodBarrasBoletaControl = CodigoBarrasBoletaControl,
                           CodSapProducto = CodSapProducto
                       });

            return rta.Count() == 0 ? "0" : rta.Single().ToString();
        }

        public DescargueBoletaControl ObtenerProductosBoletaControl(string CodBarra, int usuario)
        {
            var _rta = new DescargueBoletaControl();
            var rta = _cnn.QueryMultiple("SP_ObtenerProductosBoleta", new { codigoBarras = CodBarra, IdUsuario = usuario }, commandType: System.Data.CommandType.StoredProcedure);
            _rta.Productos = rta.Read<Producto>();
            _rta.Mensaje = rta.Read<string>().Single();
            _rta.TipoProd = rta.Read<string>().Single();
            _rta.Cantidad = rta.Read<int>().Single();

            return _rta;
        }

        public DescargueBoletaControl ObtenerProductoInstitucional(string CodBarra)
        {
            var _rta = new DescargueBoletaControl();
            var rta = _cnn.QueryMultiple("SP_ObtenerProductoInstitucional", new { codigoBarras = CodBarra }, commandType: System.Data.CommandType.StoredProcedure);
            _rta.Productos = rta.Read<Producto>();
            _rta.Mensaje = rta.Read<string>().Single();
            return _rta;
        }

        public DescargueBoletaControl ObtenerProductosInstitucional(string CodBarra1, string CodBarra2)
        {
            var _rta = new DescargueBoletaControl();
            var rta = _cnn.QueryMultiple("SP_ObtenerProductosInstitucional", new { CodigoBarraInicio = CodBarra1, CodigoBarraFin = CodBarra2 }, commandType: System.Data.CommandType.StoredProcedure);
            _rta.Productos = rta.Read<Producto>();
            _rta.Mensaje = rta.Read<string>().Single();
            return _rta;
        }

        public string DescargarProductosInstitucional(ImprimirBoletaControl modelo)
        {
            var _rta = string.Empty;
            var rta = _cnn.Query<string>("SP_DescargueBoletasInstitucional",
                new
                {
                    IdUsuario = modelo.IdUsuario,
                    IdEstadoBoleta = modelo.IdEstadoBoleta,
                    IdPunto = modelo.IdPunto,
                    SolicitudBoleteria = modelo.IdSolicitudBoleteria,
                    CodigoPedido = modelo.CodigoPedido
                }
                , commandType: System.Data.CommandType.StoredProcedure);
            //_rta = rta.Read<string>().Single();
            return _rta;
        }
        /// <summary>
        /// Obtener productos de pedido por día
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductosPedidos> ObtenerProductosPedidosDia()
        {
            var rta = _cnn.Query<ProductosPedidos>(sql: "SP_ObtenerProductosPedidos", commandType: CommandType.StoredProcedure);
            return rta != null && rta.Count() > 0 ? rta : null;
        }
        public IEnumerable<ProductosPedidos> ObtenerPedidosTraslado()
        {
            var rta = _cnn.Query<ProductosPedidos>(sql: "SP_ObtenerPedidosDisponiblesTraslado", commandType: CommandType.StoredProcedure);
            return rta != null && rta.Count() > 0 ? rta : null;
        }
        public IEnumerable<SolicitudRetorno> ObtenerPedidosEntregaAsesor(int idPunto)
        {
            var rta = _cnn.Query<SolicitudRetorno>(sql: "SP_ObtenerPedidosParaEntregar", param: new { idPunto = idPunto }, commandType: CommandType.StoredProcedure);
            return rta != null && rta.Count() > 0 ? rta : null;
        }
        public DescargueBoletaControl ObtenerListaDescargue(string CodBarra)
        {
            var _rta = new DescargueBoletaControl();
            try
            {
                var rta = _cnn.QueryMultiple("SP_ObtenerProductosBoleteria", new { codigoBarras = CodBarra }, commandType: System.Data.CommandType.StoredProcedure);
                _rta.Productos = rta.Read<Producto>();
                _rta.Mensaje = rta.Read<string>().Single();
                return _rta;
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarErrorContingencia(ex, "RepositorioProducto_ObtenerListaDescargue");
                return null;
            }
        }



        public RedencionBoletaControl RedencionBoletaControl(ImprimirBoletaControl modelo)
        {
            var modeloRetorno = new RedencionBoletaControl();

            var rta = _cnn.QueryMultiple("SP_DescargaProductoBControl",
                                          commandType: CommandType.StoredProcedure,
                                          commandTimeout: 1800000,
                                          param: new
                                          {
                                              CodBarraInicio = modelo.CodBarraInicio,
                                              CodBarraFin = modelo.CodBarraFinal,
                                              TablaProducto = Utilidades.convertTable(modelo.ListaProductos.Select(x => new TablaGeneral
                                              {
                                                  col1 = x.Cantidad.ToString(),
                                                  col2 = x.CodBarraInicio,
                                                  col3 = x.CodBarraFin,
                                                  col4 = x.CodSapTipoProducto,
                                                  col5 = x.CodigoSap,
                                                  col6 = x.Nombre
                                              })),
                                              IdUsuario = modelo.IdUsuario
                                          });

            if (rta != null)
            {
                modeloRetorno.Mensaje = rta.Read<string>().First();
                modeloRetorno.modeloImprimir = rta.Read<ImprimirDescargueProducto>().ToList();
            }

            return modeloRetorno;
        }

        public string DescargueBoletaFactura(List<Producto> listaProductos)
        {
            var mensaje = string.Empty;
            var producto = listaProductos.First();
            var _rta = _cnn.Query<string>(
                    sql: "SP_DescargueBoleteria",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        Institucional = producto.Institucional,
                        IdEstado = producto.IdEstado,
                        IdUsuarioModificacion = producto.IdUsuarioModificacion,
                        Productos = Utilidades.convertTable(listaProductos.Select(x => new TablaGeneral
                        {
                            col1 = x.IdDetalleProducto.ToString(),
                            col2 = x.CodigoSap,
                            col3 = x.CodSapTipoProducto,
                            col4 = x.Entregado.ToString(),
                            col5 = x.IdPuntoDescarga.ToString()
                        }))
                    }
                );

            if (_rta != null)
                mensaje = _rta.FirstOrDefault();

            return mensaje;
        }


        public IEnumerable<Producto> ObtenerTodosProductos()
        {
            var rta = _cnn.Query<Producto>("SP_ObtenerTodosProductos",
                null,
                commandType: CommandType.StoredProcedure).ToList();
            return rta;
        }

        public IEnumerable<Producto> ObtenerProductosXPuntoSurtido()
        {
            var rta = _cnn.Query<Producto>("SP_ObtenerProductosXPuntoSurtido",
                null,
                commandType: CommandType.StoredProcedure).ToList();
            return rta;
        }
        public IEnumerable<Producto> ObtenerTodosProductosRestaurante()
        {
            var rta = _cnn.Query<Producto>("SP_ObtenerTodosProductosRestaurante",
                null,
                commandType: CommandType.StoredProcedure).ToList();
            return rta;
        }

        public Factura ObtenerUltimaFactura(int IdPunto)
        {
            var rta = _cnn.Query<Factura>("SP_ObtenerFacturaPunto",
                                  commandType: CommandType.StoredProcedure,
                                  param: new { @IdPunto = IdPunto });

            var factura = new Factura();

            if (rta != null && rta.Count() > 0)
                factura = rta.First();

            return factura;
        }


        public FacturaImprimir ObtenerFacturaImprimir(int IdFactura)
        {
            FacturaImprimir objFact = new FacturaImprimir();
            CommandDefinition cmd = new CommandDefinition("SP_ConsultaFacturaImprimir", new { Id_Factura = IdFactura }, null, null, CommandType.StoredProcedure);

            var reader = _cnn.ExecuteReader(cmd);
            //Cabecera de la factura
            while (reader.Read())
            {
                objFact.CodigoFactura = reader.IsDBNull(0) ? "" : reader.GetString(0);
                objFact.Fecha = reader.IsDBNull(1) ? DateTime.MinValue : reader.GetDateTime(1);
                objFact.Usuario = reader.IsDBNull(2) ? "" : reader.GetString(2);
                objFact.Punto = reader.IsDBNull(3) ? "" : reader.GetString(3);
                objFact.ConsecutivoInicialPunto = reader.IsDBNull(4) ? "" : reader.GetString(4);
                objFact.ConsecutivoFinalPunto = reader.IsDBNull(5) ? "" : reader.GetString(5);
                objFact.ResolucionPunto = reader.IsDBNull(6) ? "" : reader.GetString(6);
                objFact.FechaResolucion = reader.IsDBNull(7) ? DateTime.MinValue : reader.GetDateTime(7);
                objFact.FechaFinalResolucion = reader.IsDBNull(8) ? DateTime.MinValue : reader.GetDateTime(8);
                objFact.IdentificacionCliente = reader.IsDBNull(9) ? "" : reader.GetString(9);
                objFact.NombreCliente = reader.IsDBNull(10) ? "" : reader.GetString(10);
                objFact.Id_Factura = reader.IsDBNull(11) ? "" : reader.GetString(11);
            }

            //Detalle deprouctos
            reader.NextResult();
            while (reader.Read())
            {
                string[] detalleProductos = new string[3];
                detalleProductos[0] = reader.IsDBNull(0) ? "0" : reader.GetInt32(0).ToString(); //Cantidad
                detalleProductos[1] = reader.IsDBNull(1) ? "" : reader.GetString(1); //Producto
                detalleProductos[2] = reader.IsDBNull(2) ? "0" : reader.GetValue(2).ToString(); //VALOR
                objFact.ListaProductos.Add(detalleProductos);
            }

            //Medios de pago
            reader.NextResult();
            while (reader.Read())
            {
                string[] detallePago = new string[2];
                detallePago[0] = reader.IsDBNull(0) ? "" : reader.GetString(0); //Nombre
                detallePago[1] = reader.IsDBNull(1) ? "0" : reader.GetValue(1).ToString(); //VALOR
                objFact.MetodosPago.Add(detallePago);
            }

            //Impuestos
            reader.NextResult();
            while (reader.Read())
            {
                string[] detalleImpuestos = new string[3];
                detalleImpuestos[0] = reader.IsDBNull(0) ? "" : reader.GetString(0); //Tarifa
                detalleImpuestos[1] = reader.IsDBNull(1) ? "0" : reader.GetValue(1).ToString(); //Base
                detalleImpuestos[2] = reader.IsDBNull(2) ? "0" : reader.GetValue(2).ToString(); //Impuesto
                objFact.Impuestos.Add(detalleImpuestos);
            }

            //Propina
            reader.NextResult();
            while (reader.Read())
            {
                string[] Propina = new string[2];
                Propina[0] = reader.IsDBNull(0) ? "" : reader.GetString(0); //Nombre
                Propina[1] = reader.IsDBNull(1) ? "0" : reader.GetValue(1).ToString(); //VALOR
                objFact.Propina.Add(Propina);
            }

            //Boleteria Adicional
            reader.NextResult();
            while (reader.Read())
            {
                string[] detalleBoleteria = new string[6];
                detalleBoleteria[0] = reader.IsDBNull(0) ? "" : reader.GetString(0); //Tipo
                detalleBoleteria[1] = reader.IsDBNull(1) ? "" : reader.GetString(1); //Producto
                detalleBoleteria[2] = reader.IsDBNull(2) ? "0" : reader.GetValue(2).ToString(); //Precio
                detalleBoleteria[3] = reader.IsDBNull(3) ? "" : reader.GetString(3); //Consecutivo
                detalleBoleteria[4] = reader.IsDBNull(4) ? "" : reader.GetString(4); //Imprime cupo debito
                detalleBoleteria[5] = reader.IsDBNull(5) ? "0" : reader.GetValue(5).ToString();//CantidadUsos
                objFact.BoleteriaAdicional.Add(detalleBoleteria);
            }
            reader.NextResult();
            while (reader.Read())
            {
                objFact.ListaProdSap.Add(reader.IsDBNull(0) ? "" : reader.GetString(0));
            }
            return objFact;
        }

        public FacturaImprimir ObtenerFacturaNotaCredImprimir(int IdFactura, int IdNotaCredito)
        {
            FacturaImprimir objFact = new FacturaImprimir();
            CommandDefinition cmd = new CommandDefinition("SP_ConsultaFacturaImprimirNC", new { Id_Factura = IdFactura, IdNotaCredito = IdNotaCredito }, null, null, CommandType.StoredProcedure);

            var reader = _cnn.ExecuteReader(cmd);
            //Cabecera de la factura
            while (reader.Read())
            {
                objFact.CodigoFactura = reader.IsDBNull(0) ? "" : reader.GetString(0);
                objFact.Fecha = reader.IsDBNull(1) ? DateTime.MinValue : reader.GetDateTime(1);
                objFact.Usuario = reader.IsDBNull(2) ? "" : reader.GetString(2);
                objFact.Punto = reader.IsDBNull(3) ? "" : reader.GetString(3);
                objFact.Supervisor = reader.IsDBNull(4) ? "" : reader.GetString(4);
            }

            //Detalle deprouctos
            reader.NextResult();
            while (reader.Read())
            {
                string[] detalleProductos = new string[3];
                detalleProductos[0] = reader.IsDBNull(0) ? "0" : reader.GetInt32(0).ToString(); //Cantidad
                detalleProductos[1] = reader.IsDBNull(1) ? "" : reader.GetString(1); //Producto
                detalleProductos[2] = reader.IsDBNull(2) ? "0" : reader.GetValue(2).ToString(); //VALOR
                objFact.ListaProductos.Add(detalleProductos);
            }

            //Medios de pago
            reader.NextResult();
            while (reader.Read())
            {
                string[] detallePago = new string[2];
                detallePago[0] = reader.IsDBNull(0) ? "" : reader.GetString(0); //Nombre
                detallePago[1] = reader.IsDBNull(1) ? "0" : reader.GetValue(1).ToString(); //VALOR
                objFact.MetodosPago.Add(detallePago);
            }

            //Impuestos
            reader.NextResult();
            while (reader.Read())
            {
                string[] detalleImpuestos = new string[3];
                detalleImpuestos[0] = reader.IsDBNull(0) ? "" : reader.GetString(0); //Tarifa
                detalleImpuestos[1] = reader.IsDBNull(1) ? "0" : reader.GetValue(1).ToString(); //Base
                detalleImpuestos[2] = reader.IsDBNull(2) ? "0" : reader.GetValue(2).ToString(); //Impuesto
                objFact.Impuestos.Add(detalleImpuestos);
            }

            return objFact;
        }

        public ProductoBoleta BuscarBoleta(string CodBarraInicio, string CodBarraFinal, string Codproducto, int usuario)
        {

            // var productoBoleta = new ProductoBoleta();
            ProductoBoleta productoBoleta = new ProductoBoleta();
            var _rta = _cnn.Query<ProductoBoleta>(sql: "SP_ValidarProductoBoletaControl", param: new
            {
                CodBarraInicio = CodBarraInicio,
                CodBarraFin = CodBarraFinal,
                CodSapProducto = Codproducto,
                IdUsuario = usuario
            }, commandType: CommandType.StoredProcedure
            );

            if (_rta != null && _rta.Count() > 0)
                productoBoleta = _rta.First();

            return productoBoleta;
        }


        public void ActualizarFacturaConvenio(string IdFactura, int IdConvenio, string ConsecutivoConvenio)
        {
            throw new NotImplementedException();
        }

        public string ValidaFacturaConvenio(List<Producto> listaProducto, string CodSapConvenio, string ConsecutivoConvenio)
        {
            throw new NotImplementedException();
        }

        public Producto ObtenerProductoPorCodSapProd_TipoProd(string CodSapTipoProducto, string CodSapProducto)
        {
            Producto retProducto = null;
            try
            {
                var prod = _cnn.GetList<Producto>($"WHERE CodigoSap = '{CodSapProducto}' AND CodSapTipoProducto = '{CodSapTipoProducto}'");
                if (prod != null && prod.Count() > 0)
                {
                    retProducto = prod.FirstOrDefault();
                }
            }
            catch (Exception)
            {
            }
            return retProducto;
        }

        public int GuardarLogVenta(LogVentaPunto modelo)
        {
            return _cnn.Insert<int>(modelo);
        }

        /// <summary>
        /// RDSH: Retorna los productos que se van a entregar desde destrezas.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Producto> ObtenerPremiosDestrezas()
        {

            IEnumerable<Producto> _lis = new List<Producto>();

            try
            {
                _lis = _cnn.Query<Producto>("SP_ObtenerProductosPremiosDestrezas", commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioProducto_ObtenerPremiosDestrezas");
            }

            return _lis;
        }

        /// <summary>
        /// GALD
        /// Obtiene las facturas que estan en contingencia para enviar servidor principal
        /// </summary>
        public IEnumerable<Factura> ObtenerFacturaContingencia()
        {
            IEnumerable<Factura> factura;
            try
            {
                var rta = _cnn.QueryMultiple("SP_ObtenerFacturaContingencia", commandType: System.Data.CommandType.StoredProcedure);
                factura = rta.Read<Factura>();
                var detalleFactura = rta.Read<DetalleFactura>();
                var MediosFactura = rta.Read<MediosPagoFactura>();
                if (factura != null)
                {
                    foreach (Factura item in factura)
                    {
                        item.DetalleFactura = detalleFactura.Where(x => x.Id_Factura == item.Id_Factura);
                        item.MediosPago = MediosFactura.Where(x => x.Id_Factura == item.Id_Factura);
                    }
                }


            }
            catch (Exception ex)
            {
                Utilidades.RegistrarErrorContingencia(ex, "ObtenerFacturaContingencia");
                return null;
            }

            return factura;
        }

        /// <summary>
        /// GALD
        /// Procesa las facturas que estan en contingencia para enviar servidor principal
        /// </summary>
        public List<Factura> ProcesaFacturaContingencia(IEnumerable<Factura> _factura)
        {
            List<Factura> facturasaborrar = new List<Factura>();
            try
            {
                foreach (Factura item in _factura)
                {

                    string Respuesta = _cnn.Query("SP_InsertarFacturaContingencia",
                       commandType: CommandType.StoredProcedure,
                       param: new
                       {
                           Factura = new TablaGeneral
                           {
                               col1 = item
                               .IdUsuarioCreacion.ToString(),
                               col2 = item.IdPunto.ToString(),
                               col3 = item.Fecha.ToString(),
                               col4 = item.FechaCreacion.ToString(),
                               col5 = item.IdApertura.ToString(),
                               col6 = item.CodigoFactura.ToString(),
                           },
                           DetalleFactura = Utilidades.convertTable(item.DetalleFactura.Select(x => new TablaGeneral
                           {
                               col1 = x.Id_Producto.ToString(),
                               col2 = x.Cantidad.ToString(),
                               col3 = x.Precio.ToString(),
                               col4 = x.IdDetalleProducto.ToString(),
                               col5 = x.FechaCreacion.ToString(),

                           })),

                           MediosPagoFactura = Utilidades.convertTable(item.MediosPago.Select(x => new TablaGeneral
                           {
                               col1 = x.IdMedioPago.ToString(),
                               col2 = x.Valor.ToString(),
                               col3 = x.NumReferencia.ToString(),
                               col4 = x.IdFranqicia.ToString(),
                               col5 = x.Cambio.ToString(),
                               col6 = x.FechaCreacion.ToString()
                           }))
                       }).ToString();

                    if (Respuesta != "")
                    {
                        Utilidades.RegistrarErrorContingencia(new Exception(Respuesta), "ProcesarFacturaContingencia");
                    }
                    else
                    {
                        facturasaborrar.Add(item);
                    }

                }

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarErrorContingencia(ex, "ProcesarFacturaContingencia");
                return null;
            }

            return facturasaborrar;
        }


        /// <summary>
        /// GALD
        /// borra las facturas que estan en contingencia para enviar servidor principal
        /// </summary>
        public string BorrarFacturaContingencia(List<Factura> _factura)
        {
            try
            {
                foreach (Factura item in _factura)
                {
                    _cnn.Query("DELETE FROM TB_MediosPagoFactura WHERE Id_Factura =" + item.Id_Factura + "");
                    _cnn.Query("DELETE FROM TB_DetalleFactura WHERE Id_Factura =" + item.Id_Factura + "");
                    _cnn.Query("DELETE FROM TB_Factura WHERE Id_Factura = " + item.Id_Factura + "");
                }
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarErrorContingencia(ex, "BorrarFacturaContingencia");
                return "Error al borrar facturas contingencia";
            }

            return "";
        }



        public string UpdatePedidoRestFactura(PagoFactura modelo)
        {
            try
            {
                var resp2 = _cnn.Query<int>(sql: "SP_UpdatePedidoRestFactura", param: new
                {
                    IdPedido = modelo.IdPedido,
                    IdFactura = modelo.IdFactura
                }, commandType: CommandType.StoredProcedure
             );
                return modelo.IdPedido.ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string ActualizarTipoAcompaRestaurante(TipoAcompanamiento modelo)
        {
            try
            {

                if (modelo.IdTipoAcomp > 0)
                {
                    var respAcom = _cnn.Query<int>(sql: "SP_ActualizarTipoAcompaRestaurante", param: new
                    {
                        IdTipoAcomp = modelo.IdTipoAcomp,
                        NombreTipo = modelo.Nombre,
                        Estado = modelo.Estado
                    }, commandType: CommandType.StoredProcedure
                   );
                }
                else
                {
                    var respAcom = _cnn.Query<int>(sql: "SP_InsertarTipoAcompaRestaurante", param: new
                    {
                        NombreTipo = modelo.Nombre,
                        Estado = 1
                    }, commandType: CommandType.StoredProcedure
                );
                }


                return "succes";

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarErrorContingencia(ex, "BorrarFacturaContingencia");
                return "Error al insertar pedido restaurante";
            }

        }

        public string AgregarProducAraza(PagoFactura modelo)
        {
            try
            {
                foreach (var item in modelo.listaProducto)
                {
                    if (item.IdProducto > 0)
                    {
                        var resp33 = _cnn.Query<int>(sql: "SP_AgregarProductoAdminRestaurante", param: new
                        {
                            IdProducto = item.IdProducto,
                            Id_TipoProdRestaurante = item.Id_TipoProdRestaurante,
                            Id_TipoAcomp = item.Id_TipoAcomp
                        }, commandType: CommandType.StoredProcedure
                  );
                    }
                }
                return "1";

            }
            catch (Exception ex)
            {

                return "Error al insertar pedido restaurante";
            }



        }
        public string InsertarPedidoR(PagoFactura modelo)
        {
            try
            {
                int bandera = 0;
                var fechahoy = DateTime.Today;
                var idpedido = _cnn.Query<int>(sql: "SP_ObtenerPedidoRestaurante", param: new
                {
                    IdMesa = modelo.IdMesa
                }, commandType: CommandType.StoredProcedure
                );


                if (idpedido.Count() > 0)
                {

                    var respUpdatePedi = _cnn.Query<int>(sql: "SP_ActualizarInfoPedidoRestaurante", param: new
                    {
                        IdPedido = idpedido.ElementAt(0),
                        NombreCliente = modelo.NombreCliente
                    }, commandType: CommandType.StoredProcedure
                    );


                    IEnumerable<int> idpedidoDetalle = new List<int>();
                    idpedidoDetalle = _cnn.Query<int>(sql: "SP_ObtenerDetallePedidoRestaurante", param: new
                    {
                        IdMesa = modelo.IdMesa
                    }, commandType: CommandType.StoredProcedure
               );
                    foreach (var item in idpedidoDetalle)
                    {
                        var consulta = modelo.listaProducto.Where(x => x.IdDetallePedido == item).ToList();
                        if (consulta.Count() < 1)
                        {

                            var respAcom = _cnn.Query<int>(sql: "SP_ActualizarDetallePedidoRestaurante", param: new
                            {
                                IdDetallePedido = item,
                                Entregado = false,
                                IdEstado = 10
                            }, commandType: CommandType.StoredProcedure
                 );
                        }
                    }


                }
                else
                {
                    bandera = 1;
                    var resp2 = _cnn.Query<int>(sql: "SP_InsertarPedidoRestaurante", param: new
                    {
                        IdMesa = modelo.IdMesa,
                        IdVendedor = modelo.IdUsuario,
                        Fecha = fechahoy,
                        NombreCliente = modelo.NombreCliente
                    }, commandType: CommandType.StoredProcedure
                );
                    idpedido = resp2;
                    //_cnn.Query("INSERT INTO TB_DetallePedidoRestaurante (Id_Producto, Id_Pedido , Cantidad) VALUES(" + item.IdProducto + ", 1,1); ");

                }


                foreach (var item in modelo.listaProducto)
                {
                    if (item.IdDetallePedido > 0)
                    {
                        var resp33 = _cnn.Query<int>(sql: "SP_ActualizarDetallePedidoRestaurante", param: new
                        {
                            IdDetallePedido = item.IdDetallePedido,
                            Entregado = item.Entregado
                        }, commandType: CommandType.StoredProcedure
                  );
                    }
                    else
                    {
                        var datoobserva = "";
                        int tipoentre = 0;
                        if (modelo.listaAcomp != null)
                        {
                            var observaciones = modelo.listaAcomp.Where(x => x.Consecutivo == item.Consecutivo).ToList();

                            if (observaciones.Count() > 0)
                            {
                                datoobserva = observaciones.FirstOrDefault().Observaciones;
                                tipoentre = observaciones.FirstOrDefault().TipoEntrega;
                            }
                        }
                        var resp3 = _cnn.Query<int>(sql: "SP_InsertarDetallePedidoRestaurante", param: new
                        {
                            IdProducto = item.IdProducto,
                            IdPedido = idpedido.ElementAt(0),
                            Cantidad = 1,
                            IdVendedor = modelo.IdUsuario,
                            Fecha = fechahoy,
                            Entregado = item.Entregado,
                            Descripcion = datoobserva,
                            TipoEntrega = tipoentre
                        }, commandType: CommandType.StoredProcedure
                    );
                        if (modelo.listaAcomp != null)
                        {

                            foreach (var item2 in modelo.listaAcomp.Where(x => x.Consecutivo == item.Consecutivo))
                            {
                                if (item2.IdAcomp != null && item2.IdAcomp != 0)
                                {
                                    var resp4 = _cnn.Query<int>(sql: "SP_InsertarAcompaPedidoRestaurante", param: new
                                    {
                                        IdDetallePedido = resp3.ElementAt(0),
                                        IdAcomp = item2.IdAcomp,
                                        Observaciones = item2.Observaciones
                                    }, commandType: CommandType.StoredProcedure
                               );
                                }

                            }
                        }
                    }
                    //_cnn.Query("INSERT INTO TB_DetallePedidoRestaurante (Id_Producto, Id_Pedido , Cantidad) VALUES(" + item.IdProducto + ", 1,1); ");

                }
                if (bandera == 1)
                {
                    var respMe = _cnn.Query<string>(sql: "SP_ModificarEstadoMesa", param: new
                    {
                        IdMesa = modelo.IdMesa,
                        Estado = 1
                    }, commandType: CommandType.StoredProcedure);

                    //var respMe22 = _cnn.Query<string>(sql: "SP_ActualizarPedidoRestaurante", param: new
                    //{
                    //    IdPedido = idpedido.ElementAt(0)
                    //}, commandType: CommandType.StoredProcedure);

                }


                return idpedido.ElementAt(0).ToString();

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarErrorContingencia(ex, "BorrarFacturaContingencia");
                return "Error al insertar pedido restaurante";
            }

            return "";
        }
        public string GuardarAcompaProduAdmin(ProductoxAcompanamiento modelo)
        {
            try
            {

                var idpedido = _cnn.Query<int>(sql: "SP_EliminarAcompanaXproductoRestaurante", param: new
                {
                    IdProducto = modelo.IdProducto
                }, commandType: CommandType.StoredProcedure
                );
                if (modelo.listaAcomp != null)
                {

                    foreach (var item2 in modelo.listaAcomp)
                    {
                        if (item2.IdAcomp != null && item2.IdAcomp != 0)
                        {
                            var resp4 = _cnn.Query<int>(sql: "SP_InsertarAcompaProductoAdminRestaurante", param: new
                            {
                                IdProducto = modelo.IdProducto,
                                IdAcomp = item2.IdAcomp,
                            }, commandType: CommandType.StoredProcedure
                       );
                        }

                    }
                }

                return "succes";

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarErrorContingencia(ex, "BorrarFacturaContingencia");
                return "Error al insertar pedido restaurante";
            }

            return "";
        }
        public string ActualizarProductoAdminRestaurante(Producto modelo)
        {
            try
            {
                if (modelo.Id_TipoAcomp == 0) modelo.Id_TipoAcomp = null;
                if (modelo.Id_TipoProdRestaurante == 0) modelo.Id_TipoProdRestaurante = null;

                var idpedido = _cnn.Query<int>(sql: "SP_ActualizarProductoAdminRestaurante", param: new
                {
                    IdProducto = modelo.IdProducto,
                    Id_TipoProdRestaurante = modelo.Id_TipoProdRestaurante,
                    Id_TipoAcomp = modelo.Id_TipoAcomp
                }, commandType: CommandType.StoredProcedure
                );


                return "succes";

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarErrorContingencia(ex, "BorrarFacturaContingencia");
                return "Error al insertar pedido restaurante";
            }

            return "";
        }

        public DiccionarioContigencia ObtenerDiccionarioContigencia()
        {
            var rta = new DiccionarioContigencia();
            try
            {
                var _rta = _cnn.QueryMultiple(sql: "SP_ObtenerDiccionarioContengencia", commandType: CommandType.StoredProcedure);
                rta.ListMenuPerfil = _rta.Read<MenuPerfil>();
                rta.ListPerfilUsuario = _rta.Read<PerfilUsuario>();
                rta.ListPtoUsuario = _rta.Read<PuntoUsuario>();
                rta.ListCampanaDocacion = _rta.Read<CampanaDonacion>();
                rta.ListProductoCampanaDocanion = _rta.Read<ProductoCampanaDocanion>();

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarErrorContingencia(ex, "BorrarFacturaContingencia");
                rta = null;
            }

            return rta;
        }

        public Factura ObtenerUltimaFactura(string CodSapPunto)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PuntoBrazalete> ObtenerPasaporteXpunto(int idPunto)
        {
            return _cnn.GetList<PuntoBrazalete>().Where(x => x.IdPunto == idPunto && x.IdEstado == 1);
        }

        public IEnumerable<PuntoBrazalete> ObtenerPasaporteXPuntoPOS(int idpunto, int idUsuario)
        {
            var rta = _cnn.QueryMultiple("SP_ObtenerBrazaletesImpresionPOS", commandType: CommandType.StoredProcedure, param: new { IdPunto = idpunto, IdUsuario = idUsuario });
            return rta.Read<PuntoBrazalete>();
        }


        public bool ActualizarPasaporteXPunto(List<PuntoBrazalete> puntoBrazalete)
        {
            PuntoBrazalete pto = puntoBrazalete.First();
            List<PuntoBrazalete> productoAnterior = ObtenerPasaporteXpunto(pto.IdPunto).ToList();

            List<PuntoBrazalete> productosInhabilitados = new List<PuntoBrazalete>();
            List<PuntoBrazalete> productosNuevos = new List<PuntoBrazalete>();


            foreach (var item in productoAnterior)
            {
                bool existe = false;
                puntoBrazalete.ForEach(x =>
                {
                    if (x.IdProducto == item.IdProducto)
                    {
                        existe = true;
                    }
                });

                if (!existe)
                {
                    item.IdEstado = 2;
                    item.FechaModificacion = DateTime.Now;
                    item.UsuarioModifica = pto.UsuarioCreacion;
                    productosInhabilitados.Add(item);
                }
            }

            foreach (var item in puntoBrazalete)
            {
                bool existe = false;

                productoAnterior.ForEach(x =>
                {
                    if (x.IdProducto == item.IdProducto)
                    {
                        existe = true;
                    }
                });

                if (!existe)
                {
                    item.IdEstado = 1;
                    productosNuevos.Add(item);
                }
            }

            //actualizar

            foreach (var item in productosInhabilitados)
            {
                _cnn.Update(item);
            }

            //insertar 

            foreach (var item in productosNuevos)
            {
                _cnn.Insert<int>(item);

            }


            return true;
        }

        public ArchivoBrazalete ObtenerArchivoBrazalete(int idProducto)
        {
            return _cnn.GetList<ArchivoBrazalete>().Where(x => x.IdProducto == idProducto && x.IdEstado == 1).FirstOrDefault();
        }

        public bool ActualizarArchivoBrazalete(ArchivoBrazalete archivoBrazalete)
        {
            ArchivoBrazalete modelo = _cnn.GetList<ArchivoBrazalete>().Where(x => x.IdProducto == archivoBrazalete.IdProducto && x.IdEstado == 1).FirstOrDefault();
            if (modelo != null)
            {
                modelo.UsuarioModificacion = archivoBrazalete.UsuarioCreacion;
                modelo.FechaModificacion = DateTime.Now;
                modelo.IdEstado = 2;
                _cnn.Update(modelo);


            }

            archivoBrazalete.IdEstado = 1;
            archivoBrazalete.FechaCreacion = DateTime.Now;
            _cnn.Insert<int>(archivoBrazalete);


            return true;

        }

        public string GenerarNumeroFactura(int IdPunto)
        {
            throw new NotImplementedException();
        }

        public Producto ObtenerProductoPorCodSap(string CodSapProducto)
        {
            return _cnn.GetList<Producto>().Where(x => x.CodigoSap == CodSapProducto).FirstOrDefault();
        }

        public IEnumerable<MedioPagoFactura> ValidarTipoFactura(int idFactura)
        {
            IEnumerable<MedioPagoFactura> facturasRedeban;
            var rta = _cnn.QueryMultiple("SP_ValidarTipoFactura", commandType: CommandType.StoredProcedure, param: new { IdFactura = idFactura });
            facturasRedeban = rta.Read<MedioPagoFactura>();
            return facturasRedeban;
        }


        public IEnumerable<AnulacionFacturaRedeban> ObtenerFacturasRedebanAnular(int idPunto)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FacturaValidaUsoRespuesta> ValidarUsoFactura(string codigoFactura)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BrazaleteReimpresion> ObtenerBrazaleteReimpresion(int idPunto)
        {
            return _cnn.Query<BrazaleteReimpresion>("ObtenerBrazaletesReimpresion", param: new { IdPunto = idPunto }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public RespuestaTransaccionRedaban ObtenerIdFranquiciaRedeban(string CodFranquicia)
        {
            throw new NotImplementedException();
        }
        public List<Producto> ObtenerListaa(string[] Cod, List<string> tipo)
        {
            var lista = new List<Producto>();
            var codigos = "";
            var tipoCod = "";
            for (int i = 0; i < Cod.Length; i++)
            {
                if (i < Cod.Length - 1)
                {
                    codigos += Cod[i] + ",";
                }
                else
                {
                    codigos += Cod[i];
                }
            }
            for (int i = 0; i < tipo.Count(); i++)
            {
                if (i < tipo.Count() - 1)
                {
                    tipoCod += tipo[i] + ",";
                }
                else
                {
                    tipoCod += tipo[i];
                }
            }
            var list = _cnn.Query<Producto>($"SELECT * FROM TB_Producto WHERE CodigoSap in ('{codigos}') AND CodSapTipoProducto in ({tipoCod})");


            return list.ToList();
        }

        public IEnumerable<PlantillaBrazalete> ObtenerPlantillaBrazalete()
        {
            return _cnn.GetList<PlantillaBrazalete>().Where(x => x.Estado == true);
        }

        public bool InsertarPlanillaBrazalete(PlantillaBrazalete plantilla)
        {
            return _cnn.Insert<int>(plantilla) > 0;
        }

        public IEnumerable<DetalleFactura> ObtenerDetallesConsecutivoConvenioDia(string consecutivoConvenio)
        {
            throw new NotImplementedException();
        }
    }
}
