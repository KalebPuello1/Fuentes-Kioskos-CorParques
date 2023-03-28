using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Presentacion.MVC.Ventas.Core.Models;
using CorParques.Transversales.Util;


namespace CorParques.Presentacion.MVC.Ventas.Core.Controllers
{
    public class ReimpresionController : ControladorBase
    {

        ServicioImprimir objImprimir = new ServicioImprimir();
        #region Propiedades

        public int IdUsuario
        {
            get { return (Session["UsuarioAutenticado"] as Usuario).Id; }
        }

        #endregion


        public string FormatoFecha(string strFecha, string strHora)
        {
            string datFecha = null;
            string[] strSplit;
            string strFechaArmada = string.Empty;
            if (strFecha != null)
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
            IEnumerable<TipoGeneral> _list = new List<TipoGeneral>();
            var _listPuntos = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/GetAllSimple");
            if (_listPuntos != null)
                _list = _listPuntos;
            ViewBag.Puntos = _listPuntos;
            var modelo = new ReimpresionFiltros();
            //var objReimpresion = await GetAsync<IEnumerable<Reimpresion>>($"Reimpresion/ObtenerReimpresion/{0}/{0}/{0}/{0}/{0}/{0}/{0}");
            //modelo.objReimpresion = objReimpresion;
            modelo.FechaInicial = DateTime.Now.ToString("dd/MM/yyyy");
            modelo.FechaFinal = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.MostrarFechas = await MostrarFechas();

            return View(modelo);
        }

        [HttpPost]
        public async Task<ActionResult> Index(ReimpresionFiltros modelo)
        {
            //modelo.FechaInicial = DateTime.Now.ToString("dd/MM/yyyy");
            //modelo.FechaFinal = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.MostrarFechas = await MostrarFechas();

            return View("Index", modelo);
        }

        public async Task<ActionResult> ObtenerDatosReimpresion(ReimpresionFiltros modelo)
        {
            IEnumerable<TipoGeneral> _list = new List<TipoGeneral>();
            var _listPuntos = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/GetAllSimple");
            if (_listPuntos != null)
                _list = _listPuntos;
            ViewBag.Puntos = _listPuntos;            
            //string Punto = modelo.CodPunto == null ? "0" : modelo.CodPunto.ToString();
            //string FechaInicial = modelo.FechaInicial == null ? "0" : modelo.FechaInicial.ToString();
            //string FechaFinal = modelo.FechaFinal == null ? "0" : modelo.FechaFinal.ToString();
            //string HoraInicial = modelo.HoraInicial == null ? "0" : modelo.HoraInicial.ToString();
            //string HoraFinal = modelo.HoraFinal == null ? "0" : modelo.HoraFinal.ToString();
            //string CodBrazalete = modelo.CodBrazalete == null ? "0" : modelo.CodBrazalete.ToString();
            //string CodInicialFactura = modelo.CodInicialFactura == null ? "0" : modelo.CodInicialFactura.ToString();
            //string CodFinalFactura = modelo.CodFinalFactura == null ? "0" : modelo.CodFinalFactura.ToString();


            //if (FechaInicial != null && FechaInicial != "0")
            //{
            //    FechaInicial = FormatoFecha(FechaInicial.Substring(0, 10),null);
            //}
            //if (FechaFinal != null && FechaFinal != "0")
            //{
            //    FechaFinal = FormatoFecha(FechaFinal.Substring(0, 10), null);
            //}

            //if (HoraInicial != null && HoraInicial != "0")
            //{
            //    HoraInicial = FormatoFecha(null, HoraInicial);
            //}
            //if (HoraFinal != null && HoraFinal != "0")
            //{
            //    HoraFinal = FormatoFecha(null, HoraFinal);
            //}

            //var objReimpresion = await GetAsync<IEnumerable<Reimpresion>>($"Reimpresion/ObtenerReimpresion/{Punto}/{FechaInicial}/{FechaFinal}/{HoraInicial}/{HoraFinal}/{CodBrazalete}/{CodInicialFactura}/{CodFinalFactura}");
            
            modelo.objReimpresion = await RetornaReimpresion(modelo);
            return await Index(modelo);
        }

        public async Task<IEnumerable<Reimpresion>> RetornaReimpresion(ReimpresionFiltros modelo)
        {
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
                FechaInicial = FormatoFecha(FechaInicial.Substring(0, 10), null);
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

            return objReimpresion;
        }

        public async Task<ActionResult> Imprimir(Reimpresion modelo, int id)
        {
            modelo.IdFactura = id;
            FacturaImprimir resultadoFacturaImprimir = await GetAsync<FacturaImprimir>($"Pos/ObtenerFacturaImprimir/{modelo.IdFactura}");


            var respImprimir = objImprimir.ImprimirTicketPosFactura(resultadoFacturaImprimir);

            if (respImprimir == "")
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
            foreach (var item in modelo.objReimpresion)
            {
                FacturaImprimir resultadoFacturaImprimir = await GetAsync<FacturaImprimir>($"Pos/ObtenerFacturaImprimir/{item.IdFactura}");
                objImprimir.ImprimirTicketPosFactura(resultadoFacturaImprimir);
            }
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

        public async Task<ActionResult> Parqueadero()
        {
            return View();
        }

        public async Task<ActionResult> ParqueaderoReImprimir(string Placa)
        {
            ControlParqueadero parkReimprimir = await GetAsync<ControlParqueadero>($"Reimpresion/ParqueaderoReImprimir/{Placa}");
            //var respImprimir = objImprimir.ImprimirTicketPosFactura(resultadoFacturaImprimir);

            if (parkReimprimir == null)
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "No se ha encontrado el registro" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/ParqueaderoTextoHead");
                Parametro objParametro2 = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/ParqueaderoTextoHead2");
                //return RedirectToAction("Index", "Reimpresion");
                TicketImprimir objTicketPark = new TicketImprimir();
                objTicketPark.TituloRecibo = "Boleta de Parqueadero";
                string sCodBarrasPark = parkReimprimir.Id.ToString("000000000000");
                sCodBarrasPark = sCodBarrasPark + Utilidades.GenerarDigitoEAN13(sCodBarrasPark).ToString();
                objTicketPark.CodigoBarrasProp = sCodBarrasPark;
                objTicketPark.AdicionarContenidoHeader = string.Concat(objParametro.Valor, objParametro2.Valor);
                objTicketPark.ListaArticulos = new List<Articulo>();

                ServicioImprimir objImprimir = new ServicioImprimir();
                objImprimir.ImprimirParqueadero(objTicketPark, parkReimprimir.Placa);
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<bool> MostrarFechas()
        {
            var objPerfiles = (Session["UsuarioAutenticado"] as Usuario).ListaPerfiles;            
            string[] strSplit;
            Parametro objParametro = null;
            bool blnMostrarFechas = false;

            try
            {

                objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilFechasReimpresion");
                if (objParametro != null)
                {
                    strSplit = objParametro.Valor.Split(',');
                    if (strSplit.Length > 0)
                    {
                        for (int intContador = 0; intContador < strSplit.Length; intContador++)
                        {
                            if (objPerfiles.Where(x => x.Id == int.Parse(strSplit[intContador])).ToList().Count() > 0)
                            {
                                blnMostrarFechas = true;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                blnMostrarFechas = false;
                Utilidades.RegistrarError(ex, "ReimpresionController_MostrarFechas");
            }
            finally
            {
                objParametro = null;
                objPerfiles = null;
                strSplit = null;
            }

            return blnMostrarFechas;

        }

    }
}