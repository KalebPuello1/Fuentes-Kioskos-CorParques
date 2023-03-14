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
    public class PlaneacionController : ControladorBase
    {

        #region Metodos

        public async Task<ActionResult> Index()
        {
            Planeacion objPlaneacion = new Planeacion();
            objPlaneacion.ListaIndicadores = await GetAsync<IEnumerable<TipoGeneral>>("Planeacion/ObtenerListaIndicadores");
            objPlaneacion.FechaTexto = DateTime.Now.ToString("MM/yyyy");
            return View(objPlaneacion);
        }

        /// <summary>
        /// RDSH: Inserta una planeacion.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        public async Task<ActionResult> Insertar(Planeacion modelo)
        {
            RespuestaViewModel objRespuestaViewModel;

            try
            {

                modelo.FechaCreacion = DateTime.Now;
                modelo.IdUsuarioCreacion = IdUsuarioLogueado;
                modelo.Planeaciones.ToList().Where(x => x.ValorTexto == ",").ToList().ForEach(x => x.ValorTexto = null);
                modelo.Planeaciones.ToList().Where(x => x.ValorTexto != null).ToList().ForEach(x => x.Valor = decimal.Parse(x.ValorTexto.Replace(".",",")));
                objRespuestaViewModel = await PostAsync<Planeacion, string>("Planeacion/Insertar", modelo);

            }
            catch (Exception ex)
            {
                objRespuestaViewModel = new RespuestaViewModel { Correcto = false, Mensaje = "Ocurrio un problema al guardar la información, contacte al administador." };
                Utilidades.RegistrarError(ex, "PlaneacionController_Insertar");
            }

            return Json(objRespuestaViewModel, JsonRequestBehavior.AllowGet);

        }
        
        /// <summary>
        /// RDSH: Consulta la planeacion por indicador y por fecha.
        /// </summary>
        /// <param name="IdIndicador"></param>
        /// <param name="Fecha"></param>
        /// <returns></returns>
        public async Task<ActionResult> ConsultarPlaneacion(int IdIndicador, string Fecha)
        {
            Fecha = Utilidades.FormatoFechaValido("01/" + Fecha).ToString("yyyy-MM");
            var objPlaneacionAuxiliar = await GetAsync<IEnumerable<PlaneacionAuxiliar>>($"Planeacion/ConsultarPlaneacion/{IdIndicador}/{Fecha}");
            var objPlaneacion = new Planeacion();
            objPlaneacion.Planeaciones = objPlaneacionAuxiliar;
            objPlaneacion.Planeaciones.ToList().Where(x => x.Valor > 0).ToList().ForEach(x => x.ValorTexto = string.Format("{0:0.00}", x.Valor));
            return PartialView("_Planeacion", objPlaneacion);

            //else
            //    objPlaneacion.ValorTexto = string.Format("{0:0.00}", objPlaneacion.Valor);
            //return Json(objPlaneacion, JsonRequestBehavior.AllowGet);
        }

        #endregion
        
        #region Metodos no usados


        /// <summary>
        /// RDSH: Actualiza una planeacion.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        public async Task<ActionResult> Actualizar(Planeacion modelo)
        {

            RespuestaViewModel objRespuestaViewModel;
            string strRespuesta = string.Empty;
            decimal decValor = 0;

            try
            {
                decimal.TryParse(modelo.ValorTexto, out decValor);

                if (decValor <= 0)
                {
                    throw new ArgumentException(string.Concat("No fue posible convertir a decimal el valor: ", modelo.ValorTexto));
                }
                modelo.Valor = decValor;
                modelo.FechaModificacion = DateTime.Now;
                modelo.IdUsuarioModificacion = IdUsuarioLogueado;
                if (modelo.FechaTexto != null)
                {
                    if (modelo.FechaTexto.Trim().Length > 0)
                        modelo.FechaTexto = Utilidades.FormatoFechaValido(modelo.FechaTexto).ToString("yyyy-MM-dd");
                }

                strRespuesta = await PutAsync<Planeacion, string>("Planeacion/Actualizar", modelo);
                if (string.IsNullOrEmpty(strRespuesta))
                {
                    return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new ArgumentException(strRespuesta);
                }
            }
            catch (Exception ex)
            {
                objRespuestaViewModel = new RespuestaViewModel { Correcto = false, Mensaje = "Ocurrio un problema al guardar la información, contacte al administador." };
                Utilidades.RegistrarError(ex, "PlaneacionController_Actualizar");
                return Json(objRespuestaViewModel, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
        
    }
}