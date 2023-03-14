using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;


namespace CorParques.Presentacion.MVC.Controllers
{
    public class ReimpresionController : ControladorBase
    {

        public string FormatoFecha(string strFecha, string strHora)
        {
            string datFecha = null;
            string[] strSplit;
            string strFechaArmada = string.Empty;
            if (strFecha!=null)
            {
                strSplit = strFecha.Split('/');
                strFechaArmada = string.Join("", strSplit[2], strSplit[1], strSplit[0]);
                datFecha = strFechaArmada;
            }
            if (strHora != null)
            {
                datFecha = Convert.ToDateTime(strHora).ToString("HH:mm").Replace(":", "");
            }
            return datFecha;

        }
        public async Task<ActionResult> Index()
        {
            var modelo = new ReimpresionFiltros();
            IEnumerable<TipoGeneral> _list = new List<TipoGeneral>();
            var _listPuntos = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/GetAllSimple");
            if (_listPuntos != null)
                _list = _listPuntos;
            ViewBag.Puntos = _listPuntos;
            //var objReimpresion = await GetAsync<IEnumerable<Reimpresion>>($"Reimpresion/ObtenerReimpresion/{0}/{0}/{0}/{0}/{0}/{0}/{0}");
            //modelo.objReimpresion = objReimpresion;
            return View(modelo);
        }

        [HttpPost]
        public async Task<ActionResult> Index(ReimpresionFiltros modelo)
        {
            return View("Index", modelo);
        }

        public async Task<ActionResult> ObtenerDatosReimpresion(ReimpresionFiltros modelo)
        {
            IEnumerable<TipoGeneral> _list = new List<TipoGeneral>();
            var _listPuntos = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/GetAllSimple");
            if (_listPuntos != null)
                _list = _listPuntos;
            ViewBag.Puntos = _listPuntos;
            string Punto = modelo.CodPunto == null ? "0" : modelo.CodPunto.ToString();
            string FechaInicial = modelo.FechaInicial == null ? "0" : modelo.FechaInicial.ToString();
            string FechaFinal = modelo.FechaFinal == null ? "0" : modelo.FechaFinal.ToString();
            string HoraInicial = modelo.HoraInicial == null ? "0" : modelo.HoraInicial.ToString();
            string HoraFinal = modelo.HoraFinal == null ? "0" : modelo.HoraFinal.ToString();
            string CodBrazalete = modelo.CodBrazalete == null ? "0" : modelo.CodBrazalete.ToString();
            string CodInicialFactura = modelo.CodInicialFactura == null ? "0" : modelo.CodInicialFactura.ToString();
            string CodFinalFactura = modelo.CodFinalFactura == null ? "0" : modelo.CodFinalFactura.ToString();

            if (FechaInicial != null && FechaInicial != "0")
            {
                FechaInicial = FormatoFecha(FechaInicial.Substring(0, 10),null);
            }
            if (FechaFinal != null && FechaFinal != "0")
            {
                FechaFinal = FormatoFecha(FechaFinal.Substring(0, 10), null);
            }

            if (HoraInicial != null && HoraInicial != "0")
            {
                HoraInicial = FormatoFecha(null, HoraInicial);
            }
            if (HoraFinal != null && HoraFinal != "0")
            {
                HoraFinal = FormatoFecha(null, HoraFinal);
            }

            var objReimpresion = await GetAsync<IEnumerable<Reimpresion>>($"Reimpresion/ObtenerReimpresion/{Punto}/{FechaInicial}/{FechaFinal}/{HoraInicial}/{HoraFinal}/{CodBrazalete}/{CodInicialFactura}/{CodFinalFactura}");

            modelo.objReimpresion = objReimpresion;
            return await Index(modelo);
        }

        public async Task<ActionResult> Imprimir(Reimpresion modelo, int? id = null)
        {
            //Metodo para reimprimir
            //var resultado = await PostAsync<Reimpresion, string>("Reimpresion/Imprimir", modelo);
            //return Json(resultado, JsonRequestBehavior.AllowGet);
            var objReimpresion = await GetAsync<IEnumerable<Reimpresion>>($"Reimpresion/ObtenerReimpresion/{0}/{0}/{0}/{0}/{0}/{0}/{id}/{0}");
            if (objReimpresion != null)
            {
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Index", "Reimpresion");
            }
        }

        public async Task<ActionResult> ImprimirTodo(ReimpresionFiltros modelo)
        {
            //Metodo para reimprimir
            //var resultado = await PostAsync<Reimpresion, string>("Reimpresion/Imprimir", modelo);
            //return Json(resultado, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Index", "Reimpresion");

        }

        public async Task<ActionResult> Detalle(int id)
        {
            var modelo = new Reimpresion();
            var objReimpresion = await GetAsync<IEnumerable<Reimpresion>>($"Reimpresion/ObtenerReimpresion/{0}/{0}/{0}/{0}/{0}/{0}/{id}/{0}");
            if (objReimpresion != null)
            {
                foreach (var item in objReimpresion)
                {
                    modelo.IdFactura = item.IdFactura;
                    modelo.Punto = item.Punto;
                    modelo.Productos = item.Productos;
                    modelo.NombrePunto = item.NombrePunto;
                    modelo.Fecha = item.Fecha;
                }
            }
            return PartialView("_Detail", modelo);
        }        
    }
}