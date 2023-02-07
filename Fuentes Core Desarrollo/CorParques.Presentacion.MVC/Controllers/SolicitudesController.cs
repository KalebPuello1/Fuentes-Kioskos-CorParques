using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class SolicitudesController : ControladorBase
    {
        // GET: Solicitudes impresion
        public async  Task<ActionResult> Index()
        {
            int idUsuario = IdUsuarioLogueado;
            var rta = await GetAsync<List<SolicitudBoleteria>>($"CentroImpresion/ObtenerListSolicitudBoleteria/{idUsuario}");
            return View(rta);
        }

        //GET: Obtener vista parcial crear solicitud
        [HttpGet]
        public async  Task<ActionResult> GetPartial()
        {
            var modelo = new SolicitudBoleteria();
            modelo.FechaUsoInicialDDMMYYY = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.listTipoProducto = await GetAsync<IEnumerable<TipoGeneral>>("Pos/ObtenerLineaProductos");
            return PartialView("_Create", modelo);
        }

        //GET: Obtener productos por tipo de producto
        [HttpGet]
        public async Task<JsonResult> ObtenerProductosporTipo(string CodSapTipoProducto)
        {
            var productos = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerProductoPorTipoProducto/{CodSapTipoProducto}");
            if (CodSapTipoProducto == "2055")
            {
                var CodSapTarjetaRecargable = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CodSapTarjetaRecargable");
                productos = productos.ToList().Where(x => x.CodigoSap.Equals(CodSapTarjetaRecargable.Valor));
            }
            return Json(productos, JsonRequestBehavior.AllowGet);
        }

        //GET: Insertar solicitud
        [HttpGet]
        public async Task<ActionResult> Insert(SolicitudBoleteria modelo)
        {

            modelo.IdEstado = (int)Enumerador.Estados.Abierto;
            if(!string.IsNullOrEmpty(modelo.strvalor))
                modelo.Valor = long.Parse(modelo.strvalor.Replace(".", ""));
            modelo.FechaUsoInicial = DateTime.ParseExact(modelo.FechaUsoInicialDDMMYYY, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            modelo.FechaUsoFinal = DateTime.ParseExact(modelo.FechaUsoInicialDDMMYYY + " "+ "23:59:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            modelo.FechaInicioEvento = modelo.FechaUsoInicial;
            modelo.FechaFinEvento = modelo.FechaUsoFinal;
            modelo.FechaCreacion = DateTime.Now;
            modelo.IdUsuarioCreacion = IdUsuarioLogueado;
            modelo.IdEstadoBoleta = (int)Enumerador.Estados.Inactivo;
            var rta = await PostAsync<SolicitudBoleteria, string>("CentroImpresion/AdicionarSolicitudImpresion", modelo);

            return Json(rta, JsonRequestBehavior.AllowGet);
        }

        //GET: Obtener lista de solicitudes
        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            int idUsuario = IdUsuarioLogueado;
            var rta = await GetAsync<List<SolicitudBoleteria>>($"CentroImpresion/ObtenerListSolicitudBoleteria/{idUsuario}");
            return PartialView("_List", rta);
        } 
    }
}