//Cambioquitar: Este controlador usa el enumerador de perfiles.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ReporteRecoleccionController : ControladorBase
    {
        

        public async Task<ActionResult> Index()
        {
            string strPerfiles = string.Empty;
            string strTaquillero = string.Empty;
            Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilSupervisor");
            Parametro objParametroRecolector = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilRecolector");
            Parametro objParametroTaquillero = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilTaquillero");

            strPerfiles = String.Concat(objParametro.Valor, ",", objParametroRecolector.Valor);
            strTaquillero = String.Concat(objParametroTaquillero.Valor);


            var _listUsuarios = await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosPorPerfil?idsPerfil={strPerfiles}");
            var _listUsuariosTaquillero = await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosPorPerfil?idsPerfil={strTaquillero}");
            ViewBag.Usuarios = _listUsuarios.Select(x => new TipoGeneral { Id = x.Id, Nombre = string.Concat(x.Nombre, " ", x.Apellido) });
           
            var _listMediosPagos = await GetAsync<IEnumerable<TipoGeneral>>("MediosPago/GetAllSimple");


            ViewBag.Taquilleros = _listUsuariosTaquillero.Select(x => new TipoGeneral { Id = x.Id, Nombre = string.Concat(x.Nombre, " ", x.Apellido) });
            ViewBag.supervisor = _listUsuarios.Select(x => new TipoGeneral { Id = x.Id, Nombre = string.Concat(x.Nombre, " ", x.Apellido) });
            ViewBag.MetodosPago = _listMediosPagos;

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> VistaPrevia(string Fecha, string IdTaquillero, string IdSupervisor )
        {
            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try
            {
                IEnumerable<ReporteRecoleccion> orden =
                await GetAsync<IEnumerable<ReporteRecoleccion>>($"ReporteRecoleccion/set/{Fecha}/{IdTaquillero}/{IdSupervisor}");
                Reportes Reportes = new Reportes();
                List<Parametro> PA = new List<Parametro>();
                
                PA.Add(new Parametro { Nombre = "Taquillero" , Valor = "Taquillero" });
                Reportes.GenerarReporteExcel(orden,PA,"ReporteRecoleccion");
                if (orden != null)
                {
                    if (orden.Count() > 0)
                        return PartialView("_Index", orden);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
      [HttpGet]
        //public async Task<string> GenerarArchivo(ReporteRecoleccion  modelo)
        public async Task<string> GenerarArchivo(string Fecha, string IdTaquillero, string IdSupervisor)
        {
      string strRetorno = string.Empty;
        Reportes objReportes = new Reportes();
        try
          {
              
                var Recoleccion = await GetAsync<IEnumerable<ReporteRecoleccion>>($"ReporteRecoleccion/ObtenerReporte/{Fecha}/{(IdTaquillero == "" ? "null" : IdTaquillero.ToString())}/{ (IdSupervisor == null ? "null" : IdSupervisor.ToString())}/");
                if (Recoleccion != null)
                    {
                         if (Recoleccion.Count() > 0)
                            strRetorno = objReportes.GenerarReporteRecoleccion(Recoleccion);
                     }
            }
          catch (Exception ex)
         {
         strRetorno = string.Concat("Error generando reporte: ", ex.Message);
         }

          if (strRetorno.Contains("No hay datos"))
         strRetorno = "";
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
    }
}