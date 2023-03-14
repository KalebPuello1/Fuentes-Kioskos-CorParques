using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ResolucionFacturaController : ControladorBase
    {
        // GET: ResolucionFactura
        public async Task<ActionResult> Index()
        {
            var rta = await ObtenerDatosIniciales();
            return View(rta);
        }
        public async Task<ActionResult> GetPartial()
        {
            var lista = await GetAsync<IEnumerable<Puntos>>("Puntos/GetAll");
            ViewBag.Meses = (await GetAsync<Parametro>("Parameters/ObtenerParametroPorNombre/MesesResolucion")).Valor;
            return PartialView("_Create", lista);
        }
        public async Task<ActionResult> GetList()
        {
            var modelo = await ObtenerDatosIniciales();
            return PartialView("_List", modelo);
        }
        private async Task<IEnumerable<ResolucionFactura>> ObtenerDatosIniciales()
        {
            ViewBag.Aprovador = false;
            var misperfiles = (Session["UsuarioAutenticado"] as Usuario).ListaPerfiles.Select(x => x.Id).ToList();
            var perfilesaprovadores = (await GetAsync<Parametro>("Parameters/ObtenerParametroPorNombre/PerfilesAprovadores")).Valor.Split(',');
            foreach (var item in perfilesaprovadores)
            {
                if (misperfiles.Contains(int.Parse(item)))
                {
                    ViewBag.Aprovador = true;
                }
            }
            return await GetAsync<IEnumerable<ResolucionFactura>>($"ResolucionFactura/ObtenerTodos/{(ViewBag.Aprovador ? "1":"0")}");

        }
        public async Task<ActionResult> Insert(ResolucionFactura modelo)
        {
            var lista = await ObtenerDatosIniciales();
            if (lista != null)
            {
                if (lista.ToList().Exists(x => x.IdPunto.Equals(modelo.IdPunto) && x.IdEstado != 2 &&
                    ((DateTime.Parse(x.FechaInicio) <= DateTime.Parse(modelo.FechaInicio) && 
                    DateTime.Parse(x.FechaFinal) >= DateTime.Parse(modelo.FechaInicio)) || 
                    (DateTime.Parse(x.FechaInicio) <= DateTime.Parse(modelo.FechaFinal) && 
                    DateTime.Parse(x.FechaFinal) >= DateTime.Parse(modelo.FechaFinal)))))
                {
                    return Json(new RespuestaViewModel { Correcto = false, Mensaje = "El rango de fechas seleccionado presenta conflicto con otra resolucion del mismo punto." }, JsonRequestBehavior.AllowGet);
                }

                if (lista.ToList().Exists(x => x.Resolucion.Equals(modelo.Resolucion) && x.Prefijo.Equals(modelo.Prefijo)))
                {
                    return Json(new RespuestaViewModel { Correcto = false, Mensaje = string.Concat("Existe un punto con la misma resolución y prefijo.") }, JsonRequestBehavior.AllowGet);
                }
            }

            IEnumerable<ResolucionFactura> resolucionFacturas = await GetAsync<IEnumerable<ResolucionFactura>>($"ResolucionFactura/ObtenerPrefijoConsecutivo/{modelo.Prefijo}/{modelo.ConsecutivoInicial}");

            if(resolucionFacturas != null && resolucionFacturas.Count() >= 0)
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = string.Concat("Existe ya un punto configurado con el mismo prefijo y consecutivo.") }, JsonRequestBehavior.AllowGet);
            }

            modelo.Usuario = IdUsuarioLogueado;
            modelo.FechaInicio = modelo.FechaInicio;
            modelo.FechaFinal = modelo.FechaFinal;

            var rta = await PostAsync<ResolucionFactura, string>("ResolucionFactura/Insertar", modelo);
            if (string.IsNullOrEmpty(rta.Mensaje))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = rta.Mensaje }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> Delete(int id)
        {
            if (await DeleteAsync($"ResolucionFactura/Eliminar/{id}"))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error eliminando el registro. Por favor intentelo de nuevo" }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> Update(int id)
        {
            if (await DeleteAsync($"ResolucionFactura/Aprobar/{id}"))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error aprobando el registro. Por favor intentelo de nuevo" }, JsonRequestBehavior.AllowGet);
        }
    }
}