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
    public class AnulacionController : ControladorBase
    {
        // GET: Anulacion
        public async Task<ActionResult> Index()
        {
            IEnumerable<AnulacionFactura> lista = await GetAsync<IEnumerable<AnulacionFactura>>($"Pos/ObtenerFacturasAnular/{this.IdPunto}");

            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            IEnumerable<AnulacionFactura> lista = await GetAsync<IEnumerable<AnulacionFactura>>($"Pos/ObtenerFacturasAnular/{this.IdPunto}");
            return PartialView("_List", lista);
        }

        public async Task<ActionResult> Anular(string[] IdsAnular, int IdSupervisor)
        {
            //var PermiteAnular = await GetAsync<string>($"Pos/ValidaPermiteAnular/{this.IdPunto}");

            var objReturn = new RespuestaViewModel();

            try
            {
                var PermiteAnular = await GetAsync<string>($"Pos/ValidaPermiteAnularUsuario/{IdUsuarioLogueado}");

                if (string.IsNullOrEmpty(PermiteAnular) || PermiteAnular == "0")
                    objReturn = new RespuestaViewModel { Correcto = false, Mensaje = "No es posible anular, taquillero no está habilitado." };
                
                else
                {
                    List<AnulacionFactura> listaAnular = new List<AnulacionFactura>();
                    foreach (var item in IdsAnular)
                    {
                        var arregloDatos = item.Split('|');
                        int _IdFactura = 0;
                        if (int.TryParse(arregloDatos[0], out _IdFactura))
                        {
                            AnulacionFactura objFact = new AnulacionFactura();
                            objFact.IdFactura = _IdFactura;
                            objFact.Observacion = arregloDatos[1];
                            objFact.IdEstado = 3;
                            objFact.IdUsuarioModificacion = IdSupervisor;
                            listaAnular.Add(objFact);
                        }
                    }

                    if (await PostAsync<IEnumerable<AnulacionFactura>, string>("Pos/AnularFacturas", listaAnular) != null)
                        objReturn = new RespuestaViewModel { Correcto = true };
                    else
                        objReturn = new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error. Por favor intentelo de nuevo" };

                }
            }catch(Exception ex)
            {
                objReturn = new RespuestaViewModel { Correcto = false, Mensaje = ex.Message };
            }

            return Json(objReturn, JsonRequestBehavior.AllowGet);
        }
    }
}