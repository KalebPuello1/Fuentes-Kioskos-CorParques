using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class DashBoardController : ControladorBase
    {

        #region Metodos

        public async Task<ActionResult> Index()
        {
            var modelo = await ObtenerInformacion(DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("dd-MM-yyyy"));
            return View(modelo);
        }

        [HttpGet]
        public async Task<PartialViewResult> CargarDashBoard(string FechaInicial, string FechaFinal)
        {
            var data = await ObtenerInformacion(FechaInicial, FechaFinal);
            return PartialView("_Reporte", data);
        }

        private async Task<IEnumerable<DashBoard>> ObtenerInformacion(string FechaInicial, string FechaFinal)
        {
            IEnumerable<DashBoard> objDashBoard = null;
            IEnumerable<BitacoraDia> objBitacoraDia = null;

            try
            {
                FechaInicial = FechaInicial.Replace("/", "-");
                FechaFinal = FechaFinal.Replace("/", "-");
                objDashBoard = await GetAsync<IEnumerable<DashBoard>>($"DashBoard/ObtenerInformacionDashBoard/{FechaInicial}/{FechaFinal}");

                if (FechaInicial.Equals(FechaFinal))
                {
                    objBitacoraDia = await ObtenerBitacoraDia(FechaInicial);
                    ViewBag.Bitacora = objBitacoraDia;
                    if (objBitacoraDia != null)
                    {
                        if (objBitacoraDia.Count().Equals(0))
                            ViewBag.TieneObservaciones = "0";
                    }
                    else
                    {
                        ViewBag.TieneObservaciones = "0";
                    }
                    
                }
                else
                {
                    ViewBag.Bitacora = null;
                    ViewBag.TieneObservaciones = "0";
                }

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "DashBoardController_ObtenerInformacion");
            }
            return objDashBoard.Count() > 0 ? objDashBoard : null;
        }

        /// <summary>
        /// RDSH: Consulta la bitacora del día.
        /// </summary>
        /// <param name="strFecha"></param>
        /// <returns></returns>
        private async Task<IEnumerable<BitacoraDia>> ObtenerBitacoraDia(string strFecha)
        {

            string[] strSplit;

            try
            {
                strSplit = strFecha.Split('-');
                strFecha = string.Join("-", strSplit[2], strSplit[1], strSplit[0]);
                var resultado = await GetAsync<IEnumerable<BitacoraDia>>($"BitacoraDia/Get/{strFecha}");

                if (resultado != null)
                {
                    if (resultado.Count() > 0)
                    {
                        if (resultado.Where(x => x.Observacion.Length > 0).FirstOrDefault() != null)
                        {
                            ViewBag.TieneObservaciones = "1";
                        }
                        else
                        {
                            ViewBag.TieneObservaciones = "0";
                        }
                    }
                }
                else
                {
                    ViewBag.TieneObservaciones = "0";
                }                            

                return resultado;
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "DashBoardController_ObtenerBitacoraDia");
                return null;
            }

        }

        #endregion

    }
}