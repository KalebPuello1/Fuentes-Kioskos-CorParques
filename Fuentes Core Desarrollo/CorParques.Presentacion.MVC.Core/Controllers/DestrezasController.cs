using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorParques.Negocio.Entidades;
using System.Configuration;
using System.Threading.Tasks;
using System.IO;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    
    public class DestrezasController : ControladorBase
    {

        public async Task<ActionResult> Index()
        {

            var objResultado = await GetAsync<ConsultaCodigodeBarras>($"CodigoDeBarras/ValidarPermisoOperativoPunto/{IdPunto}");
                       
            return View(objResultado);
        }


        [HttpGet]
        public async Task<ActionResult> ValidarAccesoDestreza(string CodigoDeBarras, long ValorAcumulado, long ValorAcumuladoConvenio, string CodigoDeBarrasDescargar)
        {
                       
            ConsultaCodigodeBarras objConsultaCodigodeBarras = null;

            try
            {

                objConsultaCodigodeBarras = await GetAsync<ConsultaCodigodeBarras>($"CodigoDeBarras/ValidarCodigoBarras/{CodigoDeBarras}/{IdPunto}/{IdUsuarioLogueado}/{ValorAcumulado}/{ValorAcumuladoConvenio}/{CodigoDeBarrasDescargar}");
                if (objConsultaCodigodeBarras.foto != null)
                {
                    objConsultaCodigodeBarras.fotoTexto = "data:image/png;base64," + Convert.ToBase64String(objConsultaCodigodeBarras.foto);
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("Error en ValidarAccesoAtraccion comuniquese con el administrador.");
            }

            return Json(objConsultaCodigodeBarras, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        public async Task<ActionResult> TransferirSaldo(string CodigoACargar, string CodigoDeBarrasDescargar, long ValorAcumulado)
        {

            ConsultaCodigodeBarras objConsultaCodigodeBarras = null;

            try
            {
                if (CodigoDeBarrasDescargar.Trim().Length > 0)
                {
                    objConsultaCodigodeBarras = await GetAsync<ConsultaCodigodeBarras>($"CodigoDeBarras/TransferirSaldo/{CodigoACargar}/{CodigoDeBarrasDescargar}/{ValorAcumulado}/{IdUsuarioLogueado}");
                }
                else
                {
                    objConsultaCodigodeBarras = new ConsultaCodigodeBarras();
                    objConsultaCodigodeBarras.SaldoActual = ValorAcumulado;
                }                

            }
            catch (Exception)
            {
                throw new ArgumentException("Error en TransferirSaldo comuniquese con el administrador.");
            }

            return Json(objConsultaCodigodeBarras, JsonRequestBehavior.AllowGet);


        }

    }
}