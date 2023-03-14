using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;
using System.Configuration;
using System.Diagnostics;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class PedidoCocinaController : ControladorBase
    {
        //public int TiempoEsperaPedidoRestaurante = Convert.ToInt32(ConfigurationManager.AppSettings["TiempoEsperaPedidoRestaurante"].ToString());
        // GET: PedidoCocina - Lista modelo para mostrar las mesas que tienen un pedido 
        public async Task<ActionResult> Index()
        {
            ViewBag.ListPuntos = await GetAsync<IEnumerable<Mesa>>("Puntos/ObtenerMesas");

            //ViewBag.TiempoEspera = TiempoEsperaPedidoRestaurante;

            var ListMesasActivas = await GetAsync<IEnumerable<ProductosMesaCocinaGroup>>("Puntos/ListarProductosMesaCocinaGroup");
            if (ListMesasActivas == null)
            {
                ViewBag.ListMesasActivas = new List<ProductosMesaCocinaGroup>();
            }
            else
            {
                ViewBag.ListMesasActivas = ListMesasActivas.Where(l => (l.Id_Estado == 1) && (l.Id_TipoProdRestaurante == 1)).ToList();
            }
            ViewBag.TopProducto = null;
            var ListProductosMesaCocina = await GetAsync<IEnumerable<ProductosMesaCocina>>("Puntos/ListarProductosMesaCocina");
            if (ListProductosMesaCocina == null)
            {
                ViewBag.ListProductosMesaCocina = new List<ProductosMesaCocina>();
            }
            else
            {
                ViewBag.TopProducto = ListProductosMesaCocina.OrderByDescending(s => s.IdDetallePedido).FirstOrDefault().IdDetallePedido;
                ViewBag.ListProductosMesaCocina = ListProductosMesaCocina.Where(l => (l.EstadoDetallePedido == 1) && (l.Id_TipoProdRestaurante == 1)).ToList();
            }

            var ListColorTiempo = await GetAsync<IEnumerable<ColorTiempoRestaurante>>("Puntos/ListarColorTiempoRestaurante");
            if (ListColorTiempo == null)
            {
                ViewBag.ListColorTiempo = new List<ColorTiempoRestaurante>();
            }
            else
            {
                ViewBag.ListColorTiempo = ListColorTiempo;
            }

            //Proceso POS 
            Session["PosCont"] = true;


            //Validar tiempo 

            Stopwatch _time = new Stopwatch();
            _time.Start();
            //Obtiene TODOS LOS PRODUCTOS
            var _ListaTodosProductosSAP = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerTodosProductos");
            ViewBag.ListaTodosProductosSAP = _ListaTodosProductosSAP;

            ViewBag.ListAyB = _ListaTodosProductosSAP.Where(l => l.CtgProducto == 1).ToArray();

            //Obtener parámetros aplicación 



            _time.Stop();

            int segundos = _time.Elapsed.Seconds;

            return View(ViewBag.ListAyB);
        }

        //Actualiza las mesas cuando se da clic para cambiar el estado de un detalle del pedido 
        public async Task<ActionResult> ActualizarMesasCocina(int IdEstado, int IdTipoProd)
        {

            var resultado = new object();
            try
            {

                var ListMesasActivas = await GetAsync<IEnumerable<ProductosMesaCocinaGroup>>("Puntos/ListarProductosMesaCocinaGroup");
                if (ListMesasActivas == null)
                {
                    ViewBag.ListMesasActivas = new List<ProductosMesaCocinaGroup>();
                }
                else
                {
                    ViewBag.ListMesasActivas = ListMesasActivas.Where(l => (l.Id_Estado == IdEstado) && (l.Id_TipoProdRestaurante == IdTipoProd)).ToList();
                }
                ViewBag.TopProducto = null;
                var ListProductosMesaCocina = await GetAsync<IEnumerable<ProductosMesaCocina>>("Puntos/ListarProductosMesaCocina");
                if (ListProductosMesaCocina == null)
                {
                    ViewBag.ListProductosMesaCocina = new List<ProductosMesaCocina>();
                }
                else
                {
                    ViewBag.TopProducto = ListProductosMesaCocina.OrderByDescending(s => s.IdDetallePedido).FirstOrDefault().IdDetallePedido;
                    ViewBag.ListProductosMesaCocina = ListProductosMesaCocina.Where(l => (l.EstadoDetallePedido == IdEstado) && (l.Id_TipoProdRestaurante == IdTipoProd)).ToList();
                }
                var ListColorTiempo = await GetAsync<IEnumerable<ColorTiempoRestaurante>>("Puntos/ListarColorTiempoRestaurante");
                if (ListColorTiempo == null)
                {
                    ViewBag.ListColorTiempo = new List<ColorTiempoRestaurante>();
                }
                else
                {
                    ViewBag.ListColorTiempo = ListColorTiempo;
                }

                return PartialView("_ListaPedidos", ViewBag.ListMesasActivas);
            }
            catch (Exception ex)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            finally
            {

            }

        }

        //Se consulta si se agrega un pedido nuevo para mostrar alerta
        public async Task<ActionResult> ConsultarNuevoPedido(int IdEstado, int IdTipoProd)
        {

            var resultado = new object();
            try
            {


                ViewBag.TopProducto = 0;
                var ListProductosMesaCocina = await GetAsync<IEnumerable<ProductosMesaCocina>>("Puntos/ListarProductosMesaCocina");
                if (ListProductosMesaCocina == null)
                {
                    ViewBag.ListProductosMesaCocina = new List<ProductosMesaCocina>();
                }
                else
                {
                    ViewBag.TopProducto = ListProductosMesaCocina.OrderByDescending(s => s.IdDetallePedido).FirstOrDefault().IdDetallePedido;
                }

                return Json(ViewBag.TopProducto, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            finally
            {

            }

        }


        public async Task<ActionResult> ActualizarEstadoProducto(int IdDetallePedido, int IdEstadoProd, int IdEstado, int IdTipoProd)
        {

            var resultado = new object();
            try
            {
                var ListProductosMesaCocinaPre = await GetAsync<IEnumerable<ProductosMesaCocina>>("Puntos/ListarProductosMesaCocina");
                if (ListProductosMesaCocinaPre != null)
                {
                    var respucancela = ListProductosMesaCocinaPre.Where(l => (l.IdDetallePedido == IdDetallePedido) && (l.EstadoDetallePedido == 10)).ToList();
                    if (respucancela.Count() > 0)
                    {
                        return Json("Alerta", JsonRequestBehavior.AllowGet);
                    }
                }

                int estadonuevo = IdEstadoProd + 1;
                var BanderaUpdate = await GetAsync<string>($"Puntos/ActualizarDetalleProducto/{IdDetallePedido}/{estadonuevo}");

                var ListMesasActivas = await GetAsync<IEnumerable<ProductosMesaCocinaGroup>>("Puntos/ListarProductosMesaCocinaGroup");
                if (ListMesasActivas == null)
                {
                    ViewBag.ListMesasActivas = new List<ProductosMesaCocinaGroup>();
                }
                else
                {
                    ViewBag.ListMesasActivas = ListMesasActivas.Where(l => (l.Id_Estado == IdEstado) && (l.Id_TipoProdRestaurante == IdTipoProd)).ToList();
                }

                var ListProductosMesaCocina = await GetAsync<IEnumerable<ProductosMesaCocina>>("Puntos/ListarProductosMesaCocina");
                if (ListProductosMesaCocina == null)
                {
                    ViewBag.ListProductosMesaCocina = new List<ProductosMesaCocina>();
                }
                else
                {
                    ViewBag.ListProductosMesaCocina = ListProductosMesaCocina.Where(l => (l.EstadoDetallePedido == IdEstado) && (l.Id_TipoProdRestaurante == IdTipoProd)).ToList();
                }
                var ListColorTiempo = await GetAsync<IEnumerable<ColorTiempoRestaurante>>("Puntos/ListarColorTiempoRestaurante");
                if (ListColorTiempo == null)
                {
                    ViewBag.ListColorTiempo = new List<ColorTiempoRestaurante>();
                }
                else
                {
                    ViewBag.ListColorTiempo = ListColorTiempo;
                }

                return PartialView("_ListaPedidos", ViewBag.ListMesasActivas);
            }
            catch (Exception ex)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            finally
            {

            }

        }

        // GET: PedidoCocina/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PedidoCocina/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PedidoCocina/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PedidoCocina/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PedidoCocina/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PedidoCocina/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PedidoCocina/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
