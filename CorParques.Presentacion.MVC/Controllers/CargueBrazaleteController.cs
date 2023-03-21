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
    public class CargueBrazaleteController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<CargueBoleteria>>("CargueBoleteria/ObtenerListaCargueBoleteria");
            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<CargueBoleteria>>("CargueBoleteria/ObtenerListaCargueBoleteria");
            return PartialView("_List", lista);
        }

        public async Task<ActionResult> GetPartial()
        {
            var modelo = new CargueBoleteria();
            var lista = await GetAsync<IEnumerable<TipoGeneral>>("CargueBoleteria/ObtenerTipoBoleteria");
            modelo.TipoBoleteria = lista;            
            return PartialView("_Create", modelo);
        }

        public async Task<ActionResult> Insert(CargueBoleteria modelo)
        {

            modelo.FechaInicioVigencia = Transversales.Util.Utilidades.FormatoFechaValido(modelo.FechaInicioVigenciaDDMMAAAA);
            modelo.FechaFinVigencia = Transversales.Util.Utilidades.FormatoFechaValido(modelo.FechaFinVigenciaDDMMAAAA, "23:59");
            modelo.FechaCreacion = DateTime.Now;
            modelo.IdUsuarioCreacion = IdUsuarioLogueado;
            modelo.IdEstado = (int)Enumerador.Estados.Activo; //Activo para la tabla [TB_CargueBoleteria].

            var resultado = await PostAsync<CargueBoleteria, string>("CargueBoleteria/Insertar", modelo);                    
            return Json(resultado, JsonRequestBehavior.AllowGet);
            
        }

        public async Task<ActionResult> Update(int id)
        {
            
            var modelo = new CargueBoleteria();
            modelo.IdCargueBoleteria = id;
            modelo.FechaModificacion = DateTime.Now;
            modelo.IdUsuarioModificacion = IdUsuarioLogueado;
            modelo.IdEstado = (int)Enumerador.Estados.Anulado; // Estado anulado para tabla TB_CargueBoleteria.

            var resultado = await PostAsync<CargueBoleteria, string>("CargueBoleteria/Actualizar", modelo);
            return Json(resultado, JsonRequestBehavior.AllowGet);

        }

    }
}