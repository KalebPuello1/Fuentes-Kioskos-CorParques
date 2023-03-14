using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class DescargueMaterialMasivoController : ControladorBase
    {
        // GET: DescargueMaterialMasivo
        public async Task<ActionResult> Index()
        {
            var rta = await GetAsync<IEnumerable<ProductosPedidos>>("Pos/ObtenerProductosPedidosDia");
            var modelo = new ProductosPedidos();
            var lis = new List<int>();

            if (rta != null)
            {
                var puntos = await GetAsync<IEnumerable<Puntos>>("Puntos/GetAll");
                var _parametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdTiposPuntoDescargue");
                foreach (var item in _parametro.Valor.Split(','))
                    lis.Add(Convert.ToInt32(item.ToString().Trim()));
                

                modelo.ListaPuntos = puntos.Where(x => lis.Contains(x.IdTipoPunto)).ToList();
                modelo.ListaPedidos = rta.Select(x => new TipoGeneral { Nombre = x.CodigoVenta, CodSAP = x.CodigoVenta }).ToList();
                modelo.ListaPedidos = modelo.ListaPedidos.GroupBy(x => x.CodSAP).Select(x => x.First()).ToList();
                ViewBag.listaPedidos = rta;
                ViewBag.Tabla = rta.GroupBy(x => new { x.IdSolicitudBoleteria, x.Nombre, x.CodigoVenta })
                    .Select(m => new ProductosPedidos
                    {
                        Cantidad = m.Sum(x => x.Cantidad),
                        Nombre = m.Key.Nombre,
                        CodigoVenta = m.Key.CodigoVenta
                    });
            }
            
            return View(modelo);
        }


        //public async Task<JsonResult> DescargarProductos(List<ProductosPedidos> productosPedido, int IdPuntoDescargue)
        //{

        //    var productos = new List<Producto>();
        //    productos = productosPedido.Select(x => new Producto { IdProducto = x.IdProducto,
        //        CodigoSap = x.CodigoSap,
        //        IdDetalleProducto = x.IdBoleteria, Pedido = x.CodigoVenta }).ToList();

        //    var obj = new ImprimirBoletaControl();
        //    obj.ListaProductos = productos;
        //    obj.IdUsuario = IdUsuarioLogueado;
        //    obj.IdEstadoBoleta = (int)Enumerador.Estados.Entregado;
        //    obj.IdPunto = IdPuntoDescargue;
        //    var rta = await PostAsync<ImprimirBoletaControl, string>("Pos/DescargarProductosInstitucional", obj);

        //    if(rta != null && rta.Correcto)
        //    {

        //        foreach (var item in productos)
        //            item.IdDetalleProducto = 0;

        //        Inventario inventario = new Inventario();
        //        inventario.FechaInventario = Utilidades.FechaActualColombia;
        //        inventario.IdPunto = IdPuntoDescargue;
        //        inventario.IdUsuarioCeado = IdUsuarioLogueado;
        //        inventario.Productos = productos; // AYB 
        //        await PostAsync<Inventario, string>("Inventario/ActualizarInventario", inventario);

        //    }

        //    return Json(rta, JsonRequestBehavior.AllowGet);
        //}


        public async Task<JsonResult> DescargarProductos(string codigoPedido, int IdPuntoDescargue, string IdSolicitudes)
        {

            var productos = new List<Producto>();

            var obj = new ImprimirBoletaControl();
            obj.ListaProductos = productos;
            obj.IdUsuario = IdUsuarioLogueado;
            obj.IdEstadoBoleta = (int)Enumerador.Estados.Entregado;
            obj.IdPunto = IdPuntoDescargue;
            obj.CodigoPedido = codigoPedido;
            obj.IdSolicitudBoleteria = IdSolicitudes;

            var rta = await PostAsync<ImprimirBoletaControl, string>("Pos/DescargarProductosInstitucional", obj);

            //if (rta != null && rta.Correcto)
            //{

            //    foreach (var item in productos)
            //        item.IdDetalleProducto = 0;

            //    Inventario inventario = new Inventario();
            //    inventario.FechaInventario = Utilidades.FechaActualColombia;
            //    inventario.IdPunto = IdPuntoDescargue;
            //    inventario.IdUsuarioCeado = IdUsuarioLogueado;
            //    inventario.Productos = productos; // AYB 
            //    await PostAsync<Inventario, string>("Inventario/ActualizarInventario", inventario);

            //}

            return Json(rta, JsonRequestBehavior.AllowGet);
        }
    }
}