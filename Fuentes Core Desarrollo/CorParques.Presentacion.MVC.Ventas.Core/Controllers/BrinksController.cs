using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Ventas.Core.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Ventas.Core.Controllers
{
    public class BrinksController : ControladorBase
    {
        // GET: Brinks
        public async Task<ActionResult> Index()
        {

            //Validar por taquillero la apertura
            var _aperturas = await GetAsync<IEnumerable<Apertura>>(
                $"Apertura/ObtenerAperturasTaquillero/{(Session["UsuarioAutenticado"] as Usuario).Id}/{(int)Enumerador.Estados.Entregado}");

            ViewBag.Apertura = true;

            if (_aperturas == null || _aperturas.Count() == 0)
                ViewBag.Apertura = false; 


            var _cantidad = await GetAsync<RecoleccionAuxliar>($"Recoleccion/ObtenerTopesCierreTaquilla/{(Session["UsuarioAutenticado"] as Usuario).Id}/{IdPunto}");

            ViewBag.cantidad = (_cantidad.MaximoCierre - _cantidad.MaximoBase);
            return View();
        }

        public async Task<ActionResult> Insertar(PagoFacturaMediosPago modelo)
        {

            modelo.IdMedioPago = (int)Enumerador.MediosPago.DocumentoBrinks;
            
            PagoFactura model = new PagoFactura
            {
                IdUsuario = (Session["UsuarioAutenticado"] as Usuario).Id,
                listaProducto = new List<Producto>(),
                listMediosPago = new List<PagoFacturaMediosPago> { modelo},
                IdPunto = IdPunto
            };

            var resultado = await PostAsync<PagoFactura, string>("Pos/InsertarCompra", model);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
    }
}