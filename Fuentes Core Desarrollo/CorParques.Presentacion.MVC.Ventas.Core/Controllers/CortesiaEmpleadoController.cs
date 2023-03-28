using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Ventas.Core.Controllers
{
    public class CortesiaEmpleadoController : ControladorBase
    {
        // GET: CortesiaEmpleado
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> ObtenerEmpleado(string documento)
        {
            var rta = await GetAsync<CortesiasEmpleado>($"Pos/ObtenerCortesiaEmpleado/{documento}");
            return Json(rta, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> ObtenerProducto(string CodBarra)
        {
            Producto _producto = new Producto();
            var _prod = await GetAsync<Producto>($"Pos/ObtenerBrazaleteConsecutivo/{CodBarra}/{(Session["UsuarioAutenticado"] as Usuario).Id}/0");
            if (_prod != null)
                _producto = _prod;

            Parametro _productos = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/ProductosCortesiasEmpleado");

            //Validar si el producto aplica
            bool _blnAplica = false;
            if (_productos != null && _productos.Valor != null && _producto.IdProducto > 0)
            {
                foreach (var item in _productos.Valor.Split(','))
                {
                    if (item.Trim() == _producto.CodigoSap)
                        _blnAplica = true;
                }
            }

            if (!_blnAplica && _producto.IdProducto > 0)
                _producto = new Producto() {MensajeValidacion = "Producto no valido!" };

            return Json(_producto, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GuardarCortesiaEmpleado(CortesiasEmpleado empleado, List<Producto> productos)
        {
            var obj = new GuardarCortesiaEmpleado()
            {
                idUsuario = IdUsuarioLogueado, Documento = empleado.NumDocumento, ListaProductos = productos
            };
            var rta = await PostAsync<GuardarCortesiaEmpleado, string>("Pos/GuardarCortesiaEmpleado", obj);
            if (rta.Correcto && string.IsNullOrEmpty(rta.Elemento.ToString()))
            {
                Inventario inventario = new Inventario();
                inventario.FechaInventario = Utilidades.FechaActualColombia;
                inventario.IdPunto = IdPunto;
                inventario.IdUsuarioCeado = IdUsuarioLogueado;
                inventario.Productos = productos; 
                await PostAsync<Inventario, string>("Inventario/ActualizarInventario", inventario);
            }

            if (!string.IsNullOrEmpty(rta.Elemento.ToString()))
            {
                rta.Correcto = false;
                rta.Mensaje = rta.Elemento.ToString();
            }

            return Json(rta, JsonRequestBehavior.AllowGet);
        }
    }
}