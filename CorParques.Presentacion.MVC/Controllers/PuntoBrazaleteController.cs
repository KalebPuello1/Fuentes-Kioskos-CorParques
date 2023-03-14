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
    public class PuntoBrazaleteController :  ControladorBase
    {
        // GET: PuntoBrazalete
        public async Task<ActionResult> Index()
        {
            IEnumerable<Puntos> puntos = await GetAsync<IEnumerable<Puntos>>($"Puntos/GetAllxTipoPunto/{(int)Enumerador.TiposPuntos.Taquilla}");
            IEnumerable<Puntos> ptoComida = await GetAsync<IEnumerable<Puntos>>($"Puntos/GetAllxTipoPunto/{(int)Enumerador.TiposPuntos.Comida}");
            List<Puntos> ptos = puntos.ToList();
            ptos.AddRange(ptoComida);
            return View(ptos);
        }

        public async Task<ActionResult> GetList()
        {
            IEnumerable<Puntos> puntos = await GetAsync<IEnumerable<Puntos>>($"Puntos/GetAllxTipoPunto/{(int)Enumerador.TiposPuntos.Taquilla}");
            IEnumerable<Puntos> ptoComida = await GetAsync<IEnumerable<Puntos>>($"Puntos/GetAllxTipoPunto/{(int)Enumerador.TiposPuntos.Comida}");
            List<Puntos> ptos = puntos.ToList();
            ptos.AddRange(ptoComida);
            return PartialView("_List",ptoComida);
        }

        public async Task<ActionResult> GetById(int id)
        {
            Puntos punto = await GetAsync<Puntos>($"Puntos/GetById/{id}");
            IEnumerable<PuntoBrazalete> brazalePtos = await GetAsync<IEnumerable<PuntoBrazalete>>($"Pos/ObtenerPasaportesXPunto/{id}");
            if (brazalePtos != null && brazalePtos.Count() > 0)
            {
                punto.BrazaletesAsociados = brazalePtos.ToList().Select(x => x.IdProducto).ToArray();
            }
            IEnumerable<Producto> brazaletes = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPasaportesActivos");
            List<Producto> ptos = brazaletes.ToList();            

            if (punto != null && punto.BrazaletesAsociados != null)
            {
                foreach (var pto in ptos)
                {
                    if (punto.BrazaletesAsociados.Where(x => x.Equals(pto.IdProducto)).Count() > 0)
                        pto.AplicaPunto = true;
                }
            }

            punto.Brazaletes = ptos;

            return PartialView("_Edit", punto);
        }

        public async Task<ActionResult> Update(Puntos modelo)
        {

            List<PuntoBrazalete> puntos = new List<PuntoBrazalete>();

            foreach (int idProducto in modelo.BrazaletesAsociados)
            {
                puntos.Add(new PuntoBrazalete
                {
                    IdProducto = idProducto,
                    IdPunto = modelo.Id,
                    UsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).NombreUsuario,
                    FechaCreacion = DateTime.Now
                });
            }

      
            var rta = await PutAsync<List<PuntoBrazalete>, string>("Pos/ActualizarPasaporteXPunto", puntos);
            if (rta != string.Empty)
            {
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error actualizando el grupo. Por favor intentelo de nuevo" });
        }
    }
}