using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Core.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class AsignacionBoletaController : ControladorBase
    {
        // GET: AsignacionBoleta
        public async  Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<JsonResult> ObtenerBoleta(string Codigo)
        {
            var rta = await GetAsync<Boleteria>($"Boleteria/ObtenerBoleta/{Codigo}");
            List<Arqueo> listaRestantes = await GetAsync<List<Arqueo>>($"Arqueo/ObtenerArqueo?idUsuario={IdUsuarioLogueado}&IdPunto={IdPunto}");
            string strMensaje = string.Empty;

            if (rta == null)
                strMensaje = "Boleta no existe";
            else
            {
                if(rta.IdEstado!=1)
                    strMensaje = "Boleta no se ecuentra activa para cambio";
                //Se cambia para validar contra fecha del evento 
                //if(rta.FechaUsoInicial.Date != DateTime.Now.Date)
                //    strMensaje = "Boleta no autorizada para cambio";
                if(DateTime.Now < rta.FechaInicioEvento || DateTime.Now > rta.FechaFinEvento)
                    strMensaje = "Boleta no se encuentra en las fechas para el cambio";
            }

            /*|| !strMensaje.Contains("Boleta no existe") || !strMensaje.Contains("Boleta no se ecuentra activa para cambio") ||
               !strMensaje.Contains("Boleta no se encuentra en las fechas para el cambio") || !strMensaje.Contains("Boleta no se ecuentra activa para cambio")
               || !strMensaje.Contains("La boleta no esta disponible para el uso") || !strMensaje.Contains("Boleta no se encuentra en las fechas para el cambio")*/
            Producto nombreBoleta = new Producto();

            if (strMensaje.Contains("")) //
            {
                nombreBoleta = await GetAsync<Producto>($"Boleteria/CambioboletaDato/{Codigo}");
                string nombreBoletaa = await GetAsync<string>($"Boleteria/Cambioboleta/{Codigo}");

                PosController pos = new PosController();
                ImpresionEnLinea impresion = new ImpresionEnLinea();


                    if (nombreBoleta != null)
                    {
                     if (!nombreBoleta.Nombre.Contains("No existe este producto en boleteria"))
                     {
                        if (nombreBoleta.CodSapTipoProducto == "2000" || nombreBoleta.CodSapTipoProducto == "2055")
                        {
                            if (listaRestantes[0].Brazalete.ToList().Exists(x => x.EnCaja < nombreBoleta.Cantidad && x.CodigoSap == nombreBoleta.CodigoSap && x.TipoBrazalete == nombreBoleta.Nombre) || !listaRestantes[0].Brazalete.ToList().Exists(x => x.CodigoSap == nombreBoleta.CodigoSap && x.TipoBrazalete == nombreBoleta.Nombre))
                            {
                                impresion.CambioBoleta = nombreBoleta;

                                IEnumerable<Producto> brazaletes = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPasaportesActivos");

                                Producto temp = await GetAsync<Producto>($"Pos/ValidarImpresionEnLinea/{nombreBoleta.CodigoSap}");

                                if (temp != null)
                                {
                                    if (temp.Nombre == nombreBoleta.Nombre && temp.CodigoSap == nombreBoleta.CodigoSap)
                                    {
                                        nombreBoleta.AplicaImpresionLinea = true;
                                    }
                                }


                                Producto p = new Producto();
                                //if (nombreBoleta.CodSapTipoProducto == "2000" || nombreBoleta.CodSapTipoProducto == "2055" || nombreBoleta.CodSapTipoProducto == "2005" && nombreBoleta.AplicaImpresionLinea)
                                if (nombreBoleta.AplicaImpresionLinea)
                                {
                                    if (nombreBoleta.CodSapTipoProducto == "2000" || nombreBoleta.CodSapTipoProducto == "2055" && nombreBoleta.AplicaImpresionLinea)
                                    {
                                        Producto productoI = brazaletes.Where(x => x.CodigoSap == nombreBoleta.CodigoSap).First();

                                        if (productoI != null)
                                        {
                                            productoI.IdUsuarioModificacion = IdUsuarioLogueado;
                                            var respuesta = await PostAsync<Producto, string>("Pos/RegistrarCodigoBoleteriaImpresionLinea", productoI);

                                            if (nombreBoleta.AplicaImpresionLinea)
                                            {
                                                ImpresionEnLinea impresionRegistroRollo = new ImpresionEnLinea();
                                                List<Producto> lProducto = new List<Producto>();
                                                lProducto.Add(nombreBoleta);
                                                impresionRegistroRollo.listaProductos = lProducto;
                                                await pos.registrarRolloInventario(impresionRegistroRollo, IdUsuarioLogueado);

                                            }

                                            /*if (nombreBoleta.AplicaImpresionLinea)
                                            {
                                                var iiii = 0;
                                                
                                                    var res = await pos.registrarRollo();
                                                    iiii++;
                                                
                                            }*/

                                            //p = await pos.asignarBoleta(_listprod, itemm, temp, brazaletes, respuesta);
                                            p = await pos.asignarCambioBoleta(impresion, nombreBoleta, temp, brazaletes, respuesta);

                                            //Me toca cambiar el Consecutio a estado 2

                                            string cambioEstado = await GetAsync<string>($"Boleteria/UpdateEstadoCambioboleta/{p.CodBarraInicio}");
                                            //Crear un update que modifique a estado 2
                                            //
                                            //string cons = respuesta.Elemento;
                                            impresion.CambioBoleta = p;

                                            await pos.imprimirBoletas(impresion, brazaletes, IdUsuarioLogueado);
                                            ViewBag.impresionEnLinea = "Se va imprimir en linea";

                                            
                                        }

                                    }
                                }
                            }

                        }
                    }
                    }
            }

            if (nombreBoleta.AplicaImpresionLinea)
            {
                return Json(new { rta = strMensaje , linea = "Se genero cambio boleta con Impresion en linea!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(strMensaje, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> ObtenerBoletas(string Codigo1, string Codigo2)
        {
            string strMensaje = string.Empty;

            if (!Codigo1.Trim().Equals(Codigo2.Trim()))
            {

                var cod1 = await GetAsync<Boleteria>($"Boleteria/ObtenerBoleta/{Codigo1}");
                var cod2 = await GetAsync<Boleteria>($"Boleteria/ObtenerBoleta/{Codigo2}");

                if (cod2 != null)
                {
                    if (cod2.IdProducto == cod1.IdProducto)
                    {
                        if (!(cod2.FechaUsoInicial <= DateTime.Now && cod2.FechaUsoFinal >= DateTime.Now))
                            strMensaje = "La boleta no esta disponible para el uso";
                        if (cod2.IdEstado != 2)
                            strMensaje = "La boleta no esta disponible para el uso";

                    }
                    else
                        strMensaje = "Las boletas no contienen el mismo producto";
                }
                else
                    strMensaje = "Boleta no autorizada para cambio";

            }
            else
                strMensaje = "Las boleta son iguales";

            return Json(strMensaje, JsonRequestBehavior.AllowGet);
        }
        

        public async Task<JsonResult> AsignarBoleta(string codigo1, string codigo2)
        {
            var modelo = new List<Boleteria>();
            modelo.Add(new Boleteria
            {
                Consecutivo = codigo1.Trim(), IdUsuarioCreacion = IdUsuarioLogueado,
                IdProducto= IdPunto
            });
            modelo.Add(new Boleteria
            {
                Consecutivo = codigo2.Trim(), IdUsuarioCreacion = IdUsuarioLogueado
            });

            var rta = await PostAsync<List<Boleteria>, string>("Boleteria/CambiarBoleta", modelo);


            await Cambioboleta(codigo1, codigo2);
            return Json(rta, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Cambioboleta(string codigo1, string codigo2)
        {
            var idProductoBoleteria = await GetAsync<Parametro>("Parameters/ObtenerParametroPorNombre/CodigoSapTipoProductoBoleteria");
            ServicioImprimir objImprimir = new ServicioImprimir();
            TicketImprimir objServicios = new TicketImprimir();
            Parametro aplicaImprersionBoleteria = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/AplicaImpresionBoleteria");

            // if (aplicaImprersionBoleteria.Valor != "0")
            {
                IEnumerable<Producto> ListaProductos = await GetAsync<IEnumerable<Producto>>("");
                List<Producto> productoImpresionBoleteria = new List<Producto>();

                //  productoImpresionBoleteria = ListaProductos.Where(x => (x.CodSapTipoProducto == idProductoBoleteria.Valor)).ToList();

                //  foreach (var item in productoImpresionBoleteria)
                {

                    //if (productoImpresionBoleteria != null)
                    {
                        TicketFactura ticketFactura = new TicketFactura();
                        objServicios.TituloRecibo = "CAMBIO DE BOLETA";
                        objServicios.TituloColumnas = "Nombre Brazalete | Num Ticket";
                        objServicios.ListaArticulos = new List<Articulo>();
                        string codigo11 = codigo1;
                        string codigo22 = codigo2;
                        List<Articulo> bol = new List<Articulo>();
                        Articulo a = new Articulo();
                        string nombreBoleta = await GetAsync<string>($"Boleteria/Cambioboleta/{codigo1}");

                        a.Nombre = nombreBoleta;
                        a.NumTicket = long.Parse(codigo11);
                        bol.Add(a);
                        a = new Articulo();
                        a.Nombre = nombreBoleta;
                        a.NumTicket = long.Parse(codigo22);
                        bol.Add(a);
                        // Boleteria.Add(a);
                        // objServicios.ListaArticulos = Boleteria;
                        Boleteria boletas = new Boleteria();
                        objServicios.ListaArticulos = bol;
                        objServicios.Firma = "";
                        Ticket t = new Ticket();
                        string IdUsuarioLogueadoo = NombreUsuarioLogueado;
                        objServicios.Usuario = IdUsuarioLogueadoo;
                        objServicios.NombrePunto = NombrePunto;
                        t.Usuario = IdUsuarioLogueadoo;
                        t.NombrePunto = NombrePunto;
                        //  objImprimir.ImprimirCupoDebito(objServicios);
                        objImprimir.ImprimirCambioboleta(objServicios);
                    }
                }
            }
            return Json(new { Cambioboleta = codigo1 });
        }
    }
}