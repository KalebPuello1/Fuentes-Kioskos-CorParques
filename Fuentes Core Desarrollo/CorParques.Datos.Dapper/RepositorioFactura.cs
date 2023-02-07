using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace CorParques.Datos.Dapper
{
    public class RepositorioFactura : RepositorioBase<PagoFactura>, IRepositorioFactura
    {
        public string InsertarFactura(PagoFactura model)
        {


            var rta = _cnn.Query<int>("SP_InsertarFactura",
                       commandType: CommandType.StoredProcedure,
                       param: new
                       {
                           IdUsuario = model.IdUsuario,
                           //DANR: 22-01-2019 -- Adicion campo donante
                           Donante = model.Donante,
                           Cambio = model.Cambio,
                           //fin DANR: 22-01-2019 -- Adicion campo donante

                           DetalleFactura = Utilidades.convertTable(model.listaProducto
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdProducto.ToString(),
                                                            col2 = "0",/* Pendiente*/
                                                            col3 = x.Precio.ToString()
                                                        })),
                           MedioPagoFactura = Utilidades.convertTable(model.listMediosPago
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdMedioPago.ToString(),
                                                            col2 = x.Valor.ToString(),
                                                            col3 = x.NumReferencia
                                                        }))
                       });

            return rta.First().ToString();
        }


        public int ObtenerVentasProductoCliente(string codigo, int producto)
        {
            return _cnn.Query<int>("SP_GetVentasProductoClienteDia"
                   , commandType: CommandType.StoredProcedure
                   , param: new { Consecutivo = codigo, Producto = producto }).First();
        }

        /// <summary>
        /// Obtener Factura
        /// </summary>
        public Factura ObtenerFactura(string codigoFactura)
        {
            Factura _factura = new Factura();

            try
            {
                var rta = _cnn.QueryMultiple("SP_ObtenerFactura", new { CodigoFactura = codigoFactura }, commandType: System.Data.CommandType.StoredProcedure);
                var factura = rta.Read<Factura>().Single();
                if (factura != null)
                    factura.DetalleFactura = rta.Read<DetalleFactura>();

                _factura = factura;
            }
            catch
            {
                throw;
            }

            return _factura;
        }

        public IEnumerable<AnulacionFactura> ObtenerFacturasAnular(int idPunto)
        
        {
            IEnumerable<AnulacionFactura> _lis = new List<AnulacionFactura>();

            try
            {
                _lis = _cnn.Query<AnulacionFactura>("SP_GetFacturaParaAnular"
                    , commandType: CommandType.StoredProcedure
                    , param: new { IdPunto = idPunto });

            }
            catch
            {
                throw;
            }

            return _lis;
        }

        public IEnumerable<AnulacionFacturaRedeban> ObtenerFacturasRedebanAnular(int idPunto)
        {
            IEnumerable<AnulacionFacturaRedeban> _lis = new List<AnulacionFacturaRedeban>();

            try
            {
                _lis = _cnn.Query<AnulacionFacturaRedeban>("SP_GetFacturaRedebanParaAnular"
                    , commandType: CommandType.StoredProcedure
                    , param: new { IdPunto = idPunto });

            }
            catch
            {
                throw;
            }

            return _lis;
        }

        public DateTime? ObtenerFechaPago(int IdDetalleProducto)
        {
            DateTime? fechaPago = null;
            var lista = _cnn.GetList<DetalleFactura>($"WHERE id_producto = 15 AND IdDetalleProducto = {IdDetalleProducto}");

            if (lista != null)
            {
                if (lista.Count() >= 1)
                {
                    foreach (var item in lista)
                    {
                        fechaPago = item.FechaCreacion;
                    }
                }
            }

            return fechaPago;
        }

        public bool AnularFacturas(IEnumerable<AnulacionFactura> modelo)
        {

            var rta = _cnn.Query("SP_AnularFacturas",
                       commandType: CommandType.StoredProcedure,
                       param: new
                       {
                           ListaUpdate = Utilidades.convertTable(modelo
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdFactura.ToString(),
                                                            col2 = x.IdEstado.ToString(),
                                                            col3 = x.IdUsuarioModificacion.ToString(),
                                                            col4 = x.Observacion
                                                        }))
                       });

            return (rta != null);
        }

        public IEnumerable<MedioPagoFactura> ValidarTipoFactura(int idFactura)
        {

            IEnumerable<MedioPagoFactura> facturasRedeban;
            var rta = _cnn.QueryMultiple("SP_ValidarTipoFactura",commandType: CommandType.StoredProcedure, param: new { IdFactura = idFactura });
            facturasRedeban = rta.Read<MedioPagoFactura>();
            return facturasRedeban;
        }

        /// <summary>
        /// ActualizarFacturaConvenio
        /// </summary>
        public void ActualizarFacturaConvenio(string IdFactura, int IdConvenio, string ConsecutivoConvenio)
        {
            Factura _factura = new Factura();

            try
            {
                //Envia a actualizar TB_Factura .CodSapConvenio y .ConsecutivoConvenio
                var rta = _cnn.QueryMultiple("SP_ObtenerFactura", new { IdFactura = IdFactura }, commandType: System.Data.CommandType.StoredProcedure);
                _factura = rta.Read<Factura>().Single();
                if (_factura != null)
                    _factura.DetalleFactura = rta.Read<DetalleFactura>();

                _factura.IdConvenio = IdConvenio;
                _factura.ConsecutivoConvenio = ConsecutivoConvenio;
                _cnn.Update(_factura);

                //y actualiza TB_ConvenioDetalle.CantidadDisponible 
                var detalle = _cnn.GetList<ConvenioDetalle>().Where(x => x.IdConvenio == IdConvenio);

                foreach (var item in detalle)
                {
                    if (item.Cantidad != null && item.Cantidad > 0)
                    {
                        //var prod = _cnn.GetList<Producto>($"WHERE CodigoSap = '{item.CodSapProducto}' AND CodSapTipoProducto = '{item.CodSapTipoProducto}'");
                        var prod = _cnn.Query<Producto>($"Select * From TB_Producto WHERE CodigoSap = '{item.CodSapProducto}' AND CodSapTipoProducto = '{item.CodSapTipoProducto}' ");
                        if (prod != null && prod.Count() > 0)
                        {
                            var ItemPorFactura = _factura.DetalleFactura.Where(d => d.Id_Producto == prod.First().IdProducto);
                            if (ItemPorFactura != null && ItemPorFactura.Count() > 0)
                            {
                                item.CantidadDisponible = (item.CantidadDisponible == null ? item.Cantidad : item.CantidadDisponible) - ItemPorFactura.Count();
                                _cnn.Update(item);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<Factura> ObtenerFacturaXConvenio(string consecutivo)
        {
            return _cnn.GetList<Factura>($"WHERE ConsecutivoConvenio='{consecutivo}' AND IdEstado=1");
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

                    string Respuesta = _cnn.Query<string>("SP_InsertarFacturaContingencia",
                       commandType: CommandType.StoredProcedure,
                       param: new
                       {
                           Factura = Utilidades.convertTable(new List<TablaGeneral> {new TablaGeneral
                               {

                                   col1 = item.IdUsuarioCreacion.ToString(),
                                   col2 = item.IdPunto.ToString(),
                                   col3 = item.Fecha.ToString("yyyy-MM-dd hh:mm:ss"),
                                   col4 = item.FechaCreacion.ToString("yyyy-MM-dd hh:mm:ss"),
                                   col5 = item.IdApertura.ToString(),
                                   col6 = item.CodigoFactura.ToString(),
                               } }.AsEnumerable()),
                           DetalleFactura = Utilidades.convertTable(item.DetalleFactura.Select(x => new TablaGeneral
                           {
                               col1 = x.Id_Producto.ToString(),
                               col2 = x.Cantidad.ToString(),
                               col3 = x.Precio.ToString(),
                               col4 = x.IdDetalleProducto.ToString(),
                               col5 = x.FechaCreacion.ToString("yyyy-MM-dd hh:mm:ss"),

                           })),

                           MedioPagoFactura = Utilidades.convertTable(item.MediosPago.Select(x => new TablaGeneral
                           {
                               col1 = x.IdMedioPago.ToString(),
                               col2 = x.Valor.ToString(),
                               col3 = x.NumReferencia != null ? x.NumReferencia.ToString() : null,
                               col4 = x.IdFranqicia.ToString(),
                               col5 = x.Cambio.ToString(),
                               col6 = x.FechaCreacion.ToString("yyyy-MM-dd hh:mm:ss"),
                           }))
                       }).First();


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
                    //_cnn.Delete(item.MediosPago);
                    //_cnn.Delete(item.DetalleFactura);
                    //_cnn.Delete(item);
                }
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarErrorContingencia(ex, "BorrarFacturaContingencia");
                return "Error al borrar facturas contingencia";
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
                rta.ListFranquicia = _rta.Read<Franquicia>();

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarErrorContingencia(ex, "BorrarFacturaContingencia");
                rta = null;
            }

            return rta;
        }

        public RespuestaTransaccionRedaban ObtenerIdFranquiciaRedeban(string CodFranquicia)
        {
            RespuestaTransaccionRedaban idfranquicia = null ;
            try
            {

                idfranquicia = _cnn.Query<RespuestaTransaccionRedaban>("SP_ObtenerFranquiciaRedebanHom",
                                                    param: new { @CodFranquicia = CodFranquicia },
                                                    commandType: CommandType.StoredProcedure).FirstOrDefault();
              
                return idfranquicia;

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarErrorContingencia(ex, "BorrarFacturaContingencia");
                return idfranquicia;
            }

     
        }

        public Factura ObtenerUltimaFactura(string CodSapPunto)
        {
            Factura rta = new Factura();
            int IdPunto = 0;
            try
            {
                //Obtener el id del punto
                var pto = _cnn.GetList<Puntos>($"WHERE CodigoSap = '{CodSapPunto}'");
                if (pto != null && pto.Count() > 0)
                    IdPunto = pto.First().Id;

                if (IdPunto > 0)
                {
                    var _rta = _cnn.Query<Factura>($"select TOP 1 CodigoFactura, IdPunto from TB_Factura WHERE IdPunto = {IdPunto} order by Id_Factura DESC");
                    if (_rta != null && _rta.Count() > 0)
                        rta = _rta.First();
                }
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarErrorContingencia(ex, "ObtenerUltimaFactura");
                rta = new Factura();
            }

            return rta;
        }

        public string ValidarAlerta(int usuario, int producto)
        {
            return _cnn.Query<string>("SP_GenerarAlerta"
                  , commandType: CommandType.StoredProcedure
                  , param: new { IdUsuario = usuario, IdProducto = producto }).First();
        }

        public Factura ObtenerFactura(int idFactura)
        {
            throw new NotImplementedException();
        }

        public string GenerarNumeroFactura(int IdPunto)
        {
            var result = _cnn.Query($"select dbo.FN_GenerarCodigoFactura({IdPunto}) as codigoFactura");
            var firstRow = result.FirstOrDefault();
            var Heading = ((IDictionary<string, object>)firstRow).Keys.ToArray();
            var details = ((IDictionary<string, object>)firstRow);
            var values = details[Heading[0]];

            return values.ToString();
        }

        public IEnumerable<FacturaValidaUsoRespuesta> ValidarUsoFactura(string codigoFactura)
        {
            IEnumerable<FacturaValidaUsoRespuesta> facturas;
            try
            {
                facturas = _cnn.Query<FacturaValidaUsoRespuesta>("SP_ConsultarFacturasUso", new { CodigoFactura = codigoFactura }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarErrorContingencia(ex, "ValidarUsoFactura");
                return null;
            }

            return facturas;
        }

    }

}