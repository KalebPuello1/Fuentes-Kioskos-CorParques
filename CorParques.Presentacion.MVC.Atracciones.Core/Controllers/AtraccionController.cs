using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorParques.Negocio.Entidades;
using System.Configuration;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Timers;
using System.IO;

namespace CorParques.Presentacion.MVC.Atracciones.Core.Controllers
{
    
    public class AtraccionController : ControladorBase
    {
        SerialPort puertoSerial;

        System.Timers.Timer tm;
        public AtraccionController()
        {
            puertoSerial = new SerialPort();
        }

        public async Task<ActionResult> Index()
        {

            var objResultado = await GetAsync<ConsultaCodigodeBarras>($"CodigoDeBarras/ValidarPermisoOperativoPunto/{IdPunto}");

            //ViewBag.ValidacionOperativa = objResultado.MensajeAcceso;

            return View(objResultado);
        }


        [HttpGet]
        public async Task<ActionResult> ValidarAccesoAtraccion(string CodigoDeBarras, long ValorAcumulado, long ValorAcumuladoConvenio, string CodigoDeBarrasDescargar)
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
            if (objConsultaCodigodeBarras != null)
                if (objConsultaCodigodeBarras.Valido)
                    EnviarPulso();
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

        private void EnviarPulso()
        {
            if (ConfigurationManager.AppSettings["puertoSerial"] != null)
            {
                try
                {
                    puertoSerial.PortName = ConfigurationManager.AppSettings["puertoSerial"].ToString();
                    puertoSerial.Open();

                    puertoSerial.RtsEnable = true;
                    tm = new Timer(800);
                    tm.Elapsed += eventTimer;
                    tm.AutoReset = true;
                    tm.Enabled = true;
                }
                catch (Exception)
                {
                }
            }

        }

        private void eventTimer(Object sender, ElapsedEventArgs e)
        {
            puertoSerial.RtsEnable = false;
            puertoSerial.Close();
            tm.Stop();
            tm.Dispose();
        }

    }
}