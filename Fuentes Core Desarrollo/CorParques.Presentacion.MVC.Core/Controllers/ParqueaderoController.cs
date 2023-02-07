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
    public class ParqueaderoController : ControladorBase
    {
        // GET: Parqueadero
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: Parqueadero/Index
        public async Task<ActionResult> Index()
        {
            ControlParqueadero objModel = new ControlParqueadero();
            var listaTipoVehiculoPorParqueadero = await GetAsync<IEnumerable<TipoVehiculoPorParqueadero>>("ControlParqueadero/ObtenerDisponibilidad");
            objModel.TiposVehiculoDisponible = listaTipoVehiculoPorParqueadero;
            return View(objModel);
        }

        public async Task<ActionResult> GetPartial()
        {
            ControlParqueadero objModel = new ControlParqueadero();
            var listaTipoVehiculoPorParqueadero = await GetAsync<IEnumerable<TipoVehiculoPorParqueadero>>("ControlParqueadero/ObtenerDisponibilidad");
            objModel.TiposVehiculoDisponible = listaTipoVehiculoPorParqueadero;
            return PartialView("_ListTipoVehiculo", objModel);
        }

        // GET: Parqueadero/Ingresar
        [HttpPost]
        public async Task<JsonResult> Ingresar(ControlParqueadero model)
        {
            model.CodUsuarioIngreso = (Session["UsuarioAutenticado"] as Usuario).Id;
            model.Placa = model.Placa.ToUpper();

            var resultado = await PostAsync<ControlParqueadero, string>("ControlParqueadero/Insert", model);
            if (resultado != null && resultado.Correcto)
            {
                
                TicketImprimir objTicketPark = new TicketImprimir();
                objTicketPark.TituloRecibo = "Boleta de Parqueadero";
                //idParqueadero
                int idParqueadero = 0;
                int.TryParse((string)resultado.Elemento, out idParqueadero);
                string sCodBarrasPark = idParqueadero.ToString("000000000000");

                Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/ParqueaderoTextoHead");
                Parametro objParametro2 = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/ParqueaderoTextoHead2");

                sCodBarrasPark = sCodBarrasPark + Utilidades.GenerarDigitoEAN13(sCodBarrasPark).ToString();
                objTicketPark.IdInterno = idParqueadero;
                objTicketPark.CodigoBarrasProp = sCodBarrasPark;
                objTicketPark.AdicionarContenidoHeader = string.Concat(objParametro.Valor, objParametro2.Valor);
                objTicketPark.ListaArticulos = new List<Articulo>();

                ServicioImprimir objImprimir = new ServicioImprimir();
                objImprimir.ImprimirParqueadero(objTicketPark, model.Placa);


                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(resultado);
            }
        }

        // GET: Parqueadero/Salir
        public async Task<ActionResult> Salir()
        {
            ControlParqueadero objModel = new ControlParqueadero();
            return PartialView("Salir", objModel);
        }

        // GET: Parqueadero/Salir
        [HttpPost]
        public async Task<JsonResult> Salir(ControlParqueadero model)
        {
            model.CodUsuarioSalida = (Session["UsuarioAutenticado"] as Usuario).Id;

            var resultado = await PostAsync<ControlParqueadero, string>("ControlParqueadero/RegistrarSalida", model);
            if (resultado != null && resultado.Correcto)
            {
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(resultado);
            }

        }

    }
}