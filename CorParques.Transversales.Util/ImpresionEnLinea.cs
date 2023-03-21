using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Transversales.Util
{
    public class ImpresionEnLinea
    {

        #region properties
        public List<Producto> listaProductos { get; set; }
        public List<Producto> listaProductosExistentes { get; set; }
        public List<AdicionPedidos> listaAdicionPedidos { get; set; }
        public Producto CambioBoleta { get; set; }
        public List<ImpresionEnLineaConsecutivos> listConsecutivos { get; set; }
        public Producto producto { get; set; }
        public int cantidad { get; set; }

        #endregion

        public AdicionPedidos ValidacionAdicionPedido(AdicionPedidos AdicionPedidos, List<Arqueo> listaRestantes, Producto temp)
        {
            List<AdicionPedidos> lProductos = new List<AdicionPedidos>();


            //objListaAdicionPedidos = await GetAsync<List<AdicionPedidos>>($"AdicionPedidos/DetallePedido/{CodigoSapPedido}");

            //if (objListaAdicionPedidos != null || objListaAdicionPedidos.Count() >= 0)
            if (AdicionPedidos != null)
            {
                var f = new DescargueBoletaControl();
                List<AdicionPedidos> lp = new List<AdicionPedidos>();
                //if (AdicionPedidos != null)
                //{
                //foreach (var item in objListaAdicionPedidos)
                //{
                /*if (AdicionPedidos.CodSapTipoProducto == "2000" || AdicionPedidos.CodSapTipoProducto == "2055" || AdicionPedidos.CodSapTipoProducto == "2005")
                {*/
                    var fff = false;
                    if (listaRestantes[0].Brazalete.Count() > 0)
                    {
                        //if (listaRestantes[0].Brazalete.ToList().Exists(x => x.EnCaja < AdicionPedidos.Cantidad && x.CodigoSap == AdicionPedidos.CodigoSapProducto))
                     

                            if (temp != null)
                            {
                                AdicionPedidos p = new AdicionPedidos();
                                AdicionPedidos.AplicaImpresionLinea = true;
                                AdicionPedidos.existe = false;
                                AdicionPedidos.MostrarTexto = false;
                                p = AdicionPedidos;
                                lp.Add(p);
                                //ViewBag.Mensaje = "";
                                fff = true;
                            }
                            else
                            {
                                if (AdicionPedidos.Cantidad <= listaRestantes[0].Brazalete.Where(x => x.CodigoSap == AdicionPedidos.CodigoSapProducto).First().EnCaja)
                                {
                                    AdicionPedidos p = new AdicionPedidos();
                                    AdicionPedidos.existe = true;
                                    AdicionPedidos.MostrarTexto = true;
                                    AdicionPedidos.AplicaImpresionLinea = false;
                                    //p = AdicionPedidos;
                                }
                                else
                                {
                                    //ViewBag.Mensaje = "No cuenta con inventario para realizar esta entrega. ni puede imprimir en linea. ";
                                }
                            }
                       
                       
                    }
                    else
                    {
                        //if (AdicionPedidos.CodSapTipoProducto == "2000" || AdicionPedidos.CodSapTipoProducto == "2055" || AdicionPedidos.CodSapTipoProducto == "2005")
                        if (AdicionPedidos.CodSapTipoProducto == "2000" || AdicionPedidos.CodSapTipoProducto == "2055")
                        {

                            //Producto temp = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{item.CodigoSapProducto}");
                            if (temp != null)
                            {
                                AdicionPedidos p = new AdicionPedidos();
                                AdicionPedidos.AplicaImpresionLinea = true;
                                AdicionPedidos.existe = false;
                                AdicionPedidos.MostrarTexto = false;
                                p = AdicionPedidos;
                                lp.Add(p);
                                //ViewBag.Mensaje = "";
                                fff = true;
                            }
                            else
                            {
                                if (listaRestantes[0].Brazalete.Count() > 0)
                                {
                                    AdicionPedidos.AplicaImpresionLinea = false;
                                    AdicionPedidos.existe = true;
                                    AdicionPedidos.MostrarTexto = true;
                                    lp.Add(AdicionPedidos);
                                }
                                else
                                {
                                    lp = new List<AdicionPedidos>();
                                }
                            }
                        }
                    else
                    {
                        AdicionPedidos = new AdicionPedidos();
                    }

                        /* if (fff)
                         {
                             //lp = new List<AdicionPedidos>();
                             //ViewBag.Mensaje = "No cuenta con inventario para realizar esta entrega. solo puede imprimir en linea. ";
                         }
                         else
                         {
                     //lp = new List<AdicionPedidos>();
                     AdicionPedidos = AdicionPedidos;
                             //ViewBag.Mensaje = "No cuenta con inventario para realizar esta entrega. ni puede imprimir en linea. ";
                         }*/

                    }


                    //}

                    //}


                    /*if (lp.Count() == objListaAdicionPedidos.Count())
                    {
                        objListaAdicionPedidos = lp;
                        //ViewBag.Mensaje = "";
                    }
                    else
                    {
                        //lp = new List<AdicionPedidos>();
                        objListaAdicionPedidos = new List<AdicionPedidos>();
                        //ViewBag.Mensaje = "No cuenta con inventario, ni impresion en linea para este pedido";
                    }*/


                    //}
                    // lp;

                //}
                
            }


            
         /*   else
            {
                //foreach (var item in objListaAdicionPedidos)
                //{
                    if (AdicionPedidos.CodSapTipoProducto == "2000" || AdicionPedidos.CodSapTipoProducto == "2055")
                    {
                    AdicionPedidos.existe = true;
                    AdicionPedidos.AplicaImpresionLinea = false;
                    }
                //}
            }*/

            return AdicionPedidos;
        }


        //public DescargueBoletaControl validcionDescargueBoleta(string mensaje,List<Arqueo> listaRestantes, Producto DescargueBoleta, Producto ) 
        public Producto validacionDescargueBoleta(string mensaje, List<Arqueo> listaRestantes, Producto DescargueBoleta, Producto temp) 
        {
            Producto descargar = new Producto();


           // List<Arqueo> listaRestantes = await GetAsync<List<Arqueo>>($"Arqueo/ObtenerArqueo?idUsuario={IdUsuarioLogueado}&IdPunto={IdPunto}");

            var fff = false;
            if (listaRestantes[0].Brazalete.Count() > 0)
            {
                if (listaRestantes[0].Brazalete.ToList().Exists(x => x.EnCaja < DescargueBoleta.Cantidad && x.CodigoSap == DescargueBoleta.CodigoSap && x.TipoBrazalete == DescargueBoleta.Nombre) || !listaRestantes[0].Brazalete.ToList().Exists(x => x.CodigoSap == DescargueBoleta.CodigoSap && x.TipoBrazalete == DescargueBoleta.Nombre))
                {
                    //Producto temp = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{item.CodigoSap}");
                    if (temp != null)
                    {
                        Producto p = new Producto();
                        DescargueBoleta.AplicaImpresionLinea = true;
                        DescargueBoleta.existe = false;
                        p = DescargueBoleta;
                        //lp.Add(p);
                        //ViewBag.Mensaje = "";
                        fff = true;
                        descargar = DescargueBoleta;
                    }
                    else
                    {
                        if (DescargueBoleta.Cantidad <= listaRestantes[0].Brazalete.Where(x => x.CodigoSap == DescargueBoleta.CodigoSap).First().EnCaja)
                        {
                            Producto p = new Producto();
                            DescargueBoleta.existe = true;
                            DescargueBoleta.AplicaImpresionLinea = false;
                            p = DescargueBoleta;
                            descargar = DescargueBoleta;
                        }
                        else
                        {
                            //ViewBag.Mensaje = "No cuenta con inventario para realizar esta entrega. ni puede imprimir en linea. ";
                        }
                    }
                }
                else
                {
                    DescargueBoleta.AplicaImpresionLinea = false;
                    DescargueBoleta.existe = true;
                    descargar = DescargueBoleta;
                    //lp.Add(DescargueBoleta);
                }
            }
            else
            {
                //if (DescargueBoleta.CodSapTipoProducto == "2000" || DescargueBoleta.CodSapTipoProducto == "2055" || DescargueBoleta.CodSapTipoProducto == "2005")
                if (DescargueBoleta.CodSapTipoProducto == "2000" || DescargueBoleta.CodSapTipoProducto == "2055")
                {

                    //Producto temp = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{item.CodigoSap}");
                    if (temp != null)
                    {
                        Producto p = new Producto();
                        DescargueBoleta.AplicaImpresionLinea = true;
                        DescargueBoleta.existe = false;
                        p = DescargueBoleta;
                        //lp.Add(p);
                        //ViewBag.Mensaje = "";
                        descargar = DescargueBoleta;
                        fff = true;
                    }
                    else
                    {
                        DescargueBoleta.Nombre = "Producto no existe, y no puede imprimirse en linea";
                        descargar = DescargueBoleta;
                    }
                }
                else
                {

                }
                //descargar = DescargueBoleta;
                /*if (fff)
                {
                    lp = new List<Producto>();
                    ViewBag.Mensaje = "No cuenta con inventario para realizar esta entrega. solo puede imprimir en linea. ";
                }
                else
                {
                    //lp = new List<Producto>();
                    ViewBag.Mensaje = "No cuenta con inventario para realizar esta entrega. ni puede imprimir en linea. ";
                }*/
            }


            return descargar;
        }

        /*public void validaImpresionEnLinea(Producto itemm, Producto temp, List<Producto> _listprod, IEnumerable<Producto> brazaletes)
        {

            ///DescargueBoletaController desc = new DescargueBoletaController();

            if (temp != null && itemm.existe == false)
            {
                *//*if (itemm.CodigoSap == temp.CodigoSap)
                {
                    itemm.AplicaImpresionLinea = true;
                }*//*

                if (_listprod.Exists(x => x.CodigoSap == temp.CodigoSap))
                {
                    //calcular codigo impresion en linea
                    foreach (var item in _listprod.Where(x => x.AplicaImpresionLinea).ToList())
                    {
                        Producto productoI = brazaletes.Where(x => x.CodigoSap == item.CodigoSap).First();

                        if (productoI != null)
                        {

                            //var respuesta = await PostAsync<Producto, string>("Pos/RegistrarCodigoBoleteriaImpresionLinea", productoI);

                           // var respuesta =  desc.RegistrarCodigoBoleteriaImpresionLinea(productoI);


                            if (!respuesta.Correcto)
                                throw new ArgumentException(respuesta.Mensaje);


                            if (!respuesta.Elemento.ToString().Contains("Error"))
                            {

                                var idBoleteria = int.Parse(respuesta.Elemento.ToString().Split('|')[0]);
                                var consecutivo = respuesta.Elemento.ToString().Split('|')[1];
                                item.CodBarraInicio = consecutivo;
                                item.IdDetalleProducto = idBoleteria;
                            }
                        }
                    }
                }
            }
        }*/

    }
    
}
