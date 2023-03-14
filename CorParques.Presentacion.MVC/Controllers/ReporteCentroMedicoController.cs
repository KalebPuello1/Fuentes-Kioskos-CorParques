using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CorParques.Transversales;
using System.Text;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ReporteCentroMedicoController : ControladorBase
    {

        #region Metodos

        /// <summary>
        /// RDSH: Formatea la fecha a un formato valido para enviar a guardar.
        /// </summary>
        /// <param name="strFecha"></param>
        /// <returns></returns>
        public DateTime FormatoFechaInicidente(string strFecha)
        {
            DateTime datFechaIncidente;
            string[] strSplitEspacio;
            string[] strSplit;
            string strFechaArmada = string.Empty;

            try
            {

                strSplitEspacio = strFecha.Split(' ');
                strSplit = strSplitEspacio[0].Split('/');
                strFechaArmada = string.Join("/", strSplit[2], strSplit[1], strSplit[0]);
                strFechaArmada = string.Concat(strFechaArmada, " ", strSplitEspacio[1], ":00 ", (strSplitEspacio[2] == "AM" ? "a.m." : "p.m."));
                datFechaIncidente = DateTime.Parse(strFechaArmada);

            }
            catch (Exception)
            {
                datFechaIncidente = DateTime.Now;
            }

            return datFechaIncidente;

        }

        #endregion


        public async Task<ActionResult> Index()
        {
            var modelo = new Procedimiento();
            var listaTipoDocumento = await GetAsync<IEnumerable<TipoGeneral>>("TipoDocumento/ObtenerTipoDocumento");
            var listaCategoriaAtencion = await GetAsync<IEnumerable<TipoGeneral>>("Procedimiento/ObtenerCategoriaAtencion");
            var listaTipoPaciente = await GetAsync<IEnumerable<TipoGeneral>>("Procedimiento/ObtenerTipoPaciente");
            var listaZonaArea = await GetAsync<IEnumerable<TipoGeneral>>($"CentroMedico/ObtenerListaZonaAreaUbicacion/{0}");
            var listaUbicacion = await GetAsync<IEnumerable<TipoGeneral>>("CentroMedico/ObtenerListaCentroMedico");

            modelo.ListaTipoDocumento = listaTipoDocumento;
            modelo.ListaCategoriaAtencion = listaCategoriaAtencion;
            modelo.ListaTipoPaciente = listaTipoPaciente;
            modelo.FechaIncidenteDDMMAAAA = DateTime.Now.ToString("dd/MM/yyyy");
            modelo.FechaIncidenteFinalDDMMAAAA = DateTime.Now.ToString("dd/MM/yyyy");
            modelo.ListaZonaArea = listaZonaArea;
            modelo.ListaUbicacion = listaUbicacion;
            return View(modelo);
        }

        [HttpGet]
        public async Task<string> GenerarArchivo(Procedimiento modelo)
        {                                
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();

            try
            {

                if (modelo.FechaIncidenteDDMMAAAA == null)
                {
                    modelo.FechaIncidenteDDMMAAAA = "0"; 
                }
                if (modelo.FechaIncidenteFinalDDMMAAAA == null)
                {
                    modelo.FechaIncidenteFinalDDMMAAAA = "0";
                }
                modelo.FechaIncidenteDDMMAAAA = modelo.FechaIncidenteDDMMAAAA.Replace("/", "_");
                modelo.FechaIncidenteFinalDDMMAAAA = modelo.FechaIncidenteFinalDDMMAAAA.Replace("/", "_");
                var lista = await GetAsync<IEnumerable<Procedimiento>>($"Procedimiento/ReporteAtenciones/{modelo.IdTipoDocumentoPaciente}/{modelo.IdCategoriaAtencion}/{modelo.IdTipoPaciente}/{modelo.FechaIncidenteDDMMAAAA}/{modelo.FechaIncidenteFinalDDMMAAAA}/{modelo.IdProcedimiento}/{modelo.IdZonaArea}/{modelo.IdCentroMedico}");

                if (lista != null)
                {
                    strRetorno = objReportes.GenerarReportePrimerosAuxilios(lista);
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error generando reporte: ", ex.Message);
            }

            return strRetorno;

        }
             
        [HttpGet]
        public virtual FileContentResult Download(string Data)
        {

            FileContentResult objFileContentResult = null;

            try
            {                
                objFileContentResult = File(Transversales.Util.Utilidades.ObtenerBytesArchivo(Data), System.Net.Mime.MediaTypeNames.Application.Octet, Data);
            }
            catch (Exception)
            {
                return null;
            }

            return objFileContentResult;
        }

        [HttpGet]
        public async Task<ActionResult> BuscarUbicaciones(int Id)
        {

            IEnumerable<TipoGeneral> objListaTipoGeneral = null;

            try
            {
                if (Id == 0)
                {
                    objListaTipoGeneral = await GetAsync<IEnumerable<TipoGeneral>>("CentroMedico/ObtenerListaCentroMedico");
                }
                else
                {
                    objListaTipoGeneral = await GetAsync<IEnumerable<TipoGeneral>>($"CentroMedico/ObtenerListaZonaAreaUbicacion/{Id}");
                }
                

            }
            catch (Exception)
            {
                throw;
            }

            return Json(objListaTipoGeneral,JsonRequestBehavior.AllowGet);

        }
    }
}