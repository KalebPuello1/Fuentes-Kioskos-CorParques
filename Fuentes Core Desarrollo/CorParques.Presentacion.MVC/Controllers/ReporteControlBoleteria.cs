using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ReporteControlBoleteriaController : ControladorBase
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string strPerfiles = string.Empty;
            Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilesAperturaEntregaInventarioPuntos");
            strPerfiles = objParametro.Valor;

            ViewBag.Usuarios = (await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosPorPerfil?idsPerfil={strPerfiles}")).OrderBy(x=>x.Nombre);
            return View();
        }
        [HttpGet]
        public async Task<string> GenerarArchivo(string fecha,string usuario)
        {
            string strRetorno = string.Empty;
            usuario = usuario == null ? "null" : usuario;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try{
                IEnumerable<ReporteBoleteria> inv =
                await GetAsync<IEnumerable<ReporteBoleteria>>($"ReporteInventarioGeneral/Boleteria/{fecha}/{usuario}");

                if (inv != null){
                    if (inv.Count() > 0)
                        strRetorno = objReportes.GenerarReporteBoleteria(inv);
                }
            }
            catch (Exception ex){
                strRetorno = string.Concat("Error generando reporte: ", ex.Message);
            }
            return strRetorno;
        }

        [HttpGet]
        public virtual FileContentResult Download(string Data)
        {
            FileContentResult objFileContentResult = null;
            try{
                objFileContentResult = File(Transversales.Util.Utilidades.ObtenerBytesArchivo(Data), System.Net.Mime.MediaTypeNames.Application.Octet, Data);
            }
            catch (Exception){
                return null;
            }
            return objFileContentResult;
        }
    }
}