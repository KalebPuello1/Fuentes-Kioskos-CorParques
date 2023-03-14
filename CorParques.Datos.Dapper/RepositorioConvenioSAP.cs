using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using CorParques.Transversales.Util;
using System.Data;
using System.Globalization;

namespace CorParques.Datos.Dapper
{
    public class RepositorioConvenioSAP : RepositorioBase<Convenio>, IRepositorioConvenioSAP
    {

        private  readonly IRepositorioFactura _factura;

        public RepositorioConvenioSAP(IRepositorioFactura factura)
        {
            _factura = factura;
        }
        public IEnumerable<ConvenioDetalle> ObtenerDetalleConvenio(int IdConvenio)
        {
            var detalle = _cnn.GetList<ConvenioDetalle>().Where(x => x.IdConvenio == IdConvenio);
            return detalle;
        }
        public IEnumerable<ConvenioDetalle> ObtenerDetalleConvenioByApp(string Consecutivo, string productos, int Otroconvenio)
        {
            var _listCopy = _cnn.GetList<ConvenioConsecutivos>().Where(x => x.Consecutivo == Consecutivo).FirstOrDefault();


            var productosReturn = new List<ConvenioDetalle>();
            if (_listCopy != null)
            {
                //Valida que el convenio tenga checkeado que sea código de barras -- 24/10/2017
                var convenio = _cnn.GetList<Convenio>().Where(x => x.IdConvenio == _listCopy.IdConvenio && x.EsActivo && x.FechaInicial <= DateTime.Now && x.FechaFinal >= DateTime.Now).FirstOrDefault();

                if (convenio != null)
                {
                    productosReturn.AddRange(_cnn.GetList<ConvenioDetalle>().Where(x => x.IdConvenio == convenio.IdConvenio && productos.Split(',').Contains(x.CodSapProducto)));
                }
                

            }
            productosReturn.AddRange(_cnn.GetList<ConvenioDetalle>().Where(x => x.IdConvenio == Otroconvenio && productos.Split(',').Contains(x.CodSapProducto)));
            return productosReturn;
        }
        public int? ObtenerConvenioXConsecutivo(string consecutivo)
        {
            var rta = _cnn.GetList<ConvenioConsecutivos>($"WHERE consecutivo = '{consecutivo}'").FirstOrDefault();
            if (rta != null)
                return rta.IdConvenio;
            return null;
        }
        private Producto ObtenerProducto(int idProducto)
        {
            Producto objProducto = new Producto();

            objProducto = _cnn.Query<Producto>("SP_ObtenerProducto",
                                                param: new { IdProducto = idProducto },
                                                commandType: CommandType.StoredProcedure).FirstOrDefault();

            //if (objProducto != null)
            //{
            //    objProducto.objListaRecetaProducto = ObtenerRecetaProducto(idProducto);
            //}

            return objProducto;

        }

        public IEnumerable<ConvenioDetalle> ObtenerDetalleConvenioByConsec(string Consecutivo)
        {
            IEnumerable<ConvenioDetalle> lista = new List<ConvenioDetalle>().ToArray();
            var facturas = _factura.ObtenerFacturaXConvenio(Consecutivo);
            var _listCopy = _cnn.GetList<ConvenioConsecutivos>().Where(x => x.Consecutivo == Consecutivo);
            List<ConvenioConsecutivos> consec = new List<ConvenioConsecutivos>();
            foreach (var conItem in _listCopy)
            {
                if (conItem.FechaInicial != null && conItem.FechaFinal != null)
                {
                    if (conItem.FechaInicial <= Utilidades.FechaActualColombia && conItem.FechaFinal >= Utilidades.FechaActualColombia)
                        
                        consec.Add(conItem);
                }
                else
                    consec.Add(conItem);

            }

            if (consec != null && consec.Count() > 0)
            {
                //Valida que el convenio tenga checkeado que sea código de barras -- 24/10/2017
                var convenio = _cnn.GetList<Convenio>().Where(x => x.IdConvenio == consec.First().IdConvenio && x.EsCodigoBarras && x.EsActivo && x.FechaInicial <= DateTime.Now && x.FechaFinal >= DateTime.Now);

                if (convenio != null && convenio.Count() > 0)
                {
                    var conv = convenio.FirstOrDefault(x => x.ReutilizaCodigoBarras.Equals(false));
                    if (conv != null)
                    {
                        if (facturas.FirstOrDefault(x => x.IdConvenio.Equals(conv.IdConvenio)) != null)
                        {
                            return lista;
                        }
                    }
                    lista = _cnn.GetList<ConvenioDetalle>().Where(x => x.IdConvenio == consec.First().IdConvenio);

                    //EDSP : Lista productos CodSapProductos

                    //var listaProductos = _cnn.GetList<Parametro>().Where(x => x.Nombre == "ProductosConvenio").FirstOrDefault();
                    var listaProductos = _cnn.GetList<ConvenioProducto>($"WHERE IdConvenio = {consec.First().IdConvenio} AND IdEstado = 1" );

                    //if (!string.IsNullOrEmpty(consec.FirstOrDefault().FechasEspeciales) && !string.IsNullOrEmpty(listaProductos.Valor))
                    if (!string.IsNullOrEmpty(consec.FirstOrDefault().FechasEspeciales) && listaProductos.Count() > 0)
                    { 
                        if (ValidarFechaEspecial(consec.FirstOrDefault().FechasEspeciales))
                        {
                            List<ConvenioDetalle> listaTemp = lista.ToList();
                            //var arreloProductosObsequio = listaProductos.Valor.Split(',');
                            foreach (var item in listaProductos)
                            {
                                var queryProducto = _cnn.GetList<Producto>($"WHERE CodigoSap = '{item.CodSapProducto}' AND IdEstado = 1");
                                if (queryProducto != null && queryProducto.Count() > 0)
                                {
                                    //cantidad por convenio
                                    var _facturas = _cnn.GetList<Factura>($"WHERE IdConvenio = {consec.First().IdConvenio} AND ConsecutivoConvenio = '{Consecutivo}' AND YEAR(Fecha) = YEAR(GETDATE()) AND MONTH(Fecha) = MONTH(GETDATE()) AND IdEstado = 1");
                                    var list = new List<DetalleFactura>().AsEnumerable();
                                    if (_facturas != null && _facturas.Count() > 0)
                                        list = _cnn.GetList<DetalleFactura>($"WHERE  Id_Factura IN @ids AND Id_Producto = {queryProducto.FirstOrDefault().IdProducto} AND Precio = 0",new { @ids = _facturas.Select(x => x.Id_Factura).Distinct() });

                                    if (list.Count() >= item.Cantidad)
                                        continue;


                                    Producto objProducto = ObtenerProducto(queryProducto.FirstOrDefault().IdProducto);
                                    ConvenioDetalle obsequio = new ConvenioDetalle();
                                    obsequio.IdConvenioDetalle = -2;
                                    obsequio.CodSapConvenio = consec.First().CodSapConvenio;
                                    obsequio.IdConvenio = consec.First().IdConvenio;
                                    obsequio.CodSapProducto = objProducto.CodigoSap;
                                    obsequio.CodSapTipoProducto = objProducto.CodSapTipoProducto;
                                    obsequio.Valor = 0;
                                    obsequio.Cantidad = item.Cantidad == list.Count() ? item.Cantidad: item.Cantidad - list.Count();
                                    obsequio.CantidadxDia = 1;
                                    listaTemp.Add(obsequio);
                                }
                            }
                            lista = listaTemp.ToArray();
                        }
                    }
                }
            }

            return lista;
        }

        public string InsertarConvenioConsecutivos (List<ConvenioConsecutivos> lista)
        {
            var rta=  _cnn.Query<string>(sql: "SP_InsertarConvenioConsecutivos",
                                commandType: System.Data.CommandType.StoredProcedure,
                                param: new
                                {
                                    lista = Utilidades.convertTable(lista.Select(x => new TablaGeneral
                                    {
                                        col1 = x.IdConvenio.ToString(),
                                        col2 = x.Consecutivo,
                                        col3 = x.FechasEspeciales,
                                        col4 = ObtenerFechaFormato(x.FechaInicial), 
                                        col5 = ObtenerFechaFormato(x.FechaFinal)
                                    }))
                                });

            return rta.Single();
        }

        private string ObtenerFechaFormato(DateTime? fecha)
        {
            return fecha == null ? "" : Convert.ToDateTime(fecha).ToString("dd/MM/yyyy");
        }

        public bool ValidarFechaEspecial(string fechas)
        {
            bool blnResult = false;
            DateTime _fecha = new DateTime();
            foreach (var item in fechas.Split(','))
            {
                if (DateTime.TryParseExact(item, "dd-MM-yyyy", CultureInfo.InvariantCulture,
                               DateTimeStyles.None, out _fecha))
                {
                    //if (_fecha == Utilidades.FechaActualColombia.Date)
                    //    blnResult = true;
                    if (_fecha.Month == Utilidades.FechaActualColombia.Date.Month)
                        blnResult = true;
                }
            }

            return blnResult;
        }

        public int ObtieneVentasConvenioProductoHoy(string CodSapConvenio, string CodigoSap, string CodSapTipoProducto)
        {
            int count = 0;

            try
            {
                count = _cnn.Query<int>("SP_ObtieneVentaConvenioProd", param: new
                {
                    CodSapConvenio = @CodSapConvenio,
                    CodigoSap = @CodigoSap,
                    CodSapTipoProducto = @CodSapTipoProducto
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception)
            {
            }

            return count;
        }

        //#region Declaraciones

        //private readonly SqlConnection _cnn;

        //#endregion

        //#region Constructor

        //public RepositorioConvenioDescuento()
        //{
        //    _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        //}

        //#endregion

        ///// <summary>
        ///// RDSH: Actualiza un convenio.
        ///// </summary>
        ///// <param name="modelo"></param>
        ///// <param name="error"></param>
        ///// <returns></returns>
        //public bool Actualizar(ConvenioDescuento modelo, out string error)
        //{
        //    error = _cnn.Query<string>("SP_ActualizarConvenioDescuento", param: new
        //    {
        //        IdConvenioDescuento = modelo.Id,
        //        Nombre = modelo.Nombre,
        //        Apellido = modelo.Apellido,
        //        IdTipoConvenioDescuento = modelo.IdTipoConvenioDescuento,
        //        IdTipoVehiculo = modelo.IdTipoVehiculo,
        //        Placa = modelo.Placa,
        //        IdEstado = modelo.IdEstado,
        //        IdUsuarioModificacion = modelo.IdUsuarioModificacion,
        //        FechaModificacion = modelo.FechaModificacion
        //    }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
        //    return string.IsNullOrEmpty(error);
        //}

        ///// <summary>
        ///// RDSH: Borrado logico de convenio.
        ///// </summary>
        ///// <param name="modelo"></param>
        ///// <param name="error"></param>
        ///// <returns></returns>
        //public bool Eliminar(ConvenioDescuento modelo, out string error)
        //{
        //    error = _cnn.Query<string>("SP_EliminarConvenioDescuento", param: new
        //    {
        //        IdConvenioDescuento = modelo.Id,
        //        IdUsuarioModificacion = modelo.IdUsuarioModificacion,
        //        FechaModificacion = modelo.FechaModificacion
        //    }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
        //    return string.IsNullOrEmpty(error);
        //}

        ///// <summary>
        ///// RDSH: Inserta un convenio.
        ///// </summary>
        ///// <param name="modelo"></param>
        ///// <param name="error"></param>
        ///// <returns></returns>
        //public bool Insertar(ConvenioDescuento modelo, out string error)
        //{
        //    error = _cnn.Query<string>("SP_InsertarConvenioDescuento", param: new
        //    {
        //        Nombre = modelo.Nombre,
        //        Apellido = modelo.Apellido,
        //        IdTipoConvenioDescuento = modelo.IdTipoConvenioDescuento,
        //        IdTipoVehiculo = modelo.IdTipoVehiculo,
        //        Placa = modelo.Placa,
        //        IdEstado = modelo.IdEstado,
        //        IdUsuarioCreacion = modelo.IdUsuarioCreacion,
        //        FechaCreacion = modelo.FechaCreacion
        //    }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

        //    return string.IsNullOrEmpty(error);
        //}

        ///// <summary>
        ///// RDSH: Retorna un convenio por id para su edicion.
        ///// </summary>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        //public ConvenioDescuento ObtenerPorId(int Id)
        //{
        //    var ConvenioDescuento = _cnn.GetList<ConvenioDescuento>().Where(x => x.Id == Id).FirstOrDefault();
        //    return ConvenioDescuento;
        //}

        //public IEnumerable<ConvenioDescuento> ObtenerLista()
        //{
        //    var lista = _cnn.Query<ConvenioDescuento>("SP_ObtenerConvenioDescuento", null, commandType: System.Data.CommandType.StoredProcedure).ToList();
        //    return lista;
        //}
    }
}
