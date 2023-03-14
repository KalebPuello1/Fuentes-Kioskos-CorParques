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
    /// <summary>
    /// KADM Configuración Cargos
    /// </summary>
    public class CargosController : ControladorBase
    {

        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<Cargos>>("Cargos/ObtenerLista");           
      
            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<Cargos>>("Cargos/ObtenerLista");
            return PartialView("_List", lista);
        }

        public async Task<ActionResult> GetPartial()
        {
            var modelo = new Perfil();
            modelo.ListaMenus = await GetAsync<IEnumerable<Menu>>("Menu/ObtenerListaActivos");
            //modelo.ListaEstados = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.Perfil}");
            return PartialView("_Create", modelo);
        }

        public async Task<ActionResult> Insert(Cargos modelo, string hdListaPerfiles)
        {

            modelo.FechaCreacion = DateTime.Now;
            modelo.UsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.IdEstado = Enumerador.Estados.Activo.GetHashCode();
            List<Perfil> perfil = new List<Perfil>();

            string[] ids = hdListaPerfiles.Split(',');
            foreach (string id in ids)
                perfil.Add(new Perfil { IdPerfil = Convert.ToInt32(string.IsNullOrEmpty(id) ? "0" : id) });

            modelo.Perfiles = perfil;

            var resultado = await PostAsync<Cargos, string>("Cargos/Insertar", modelo);
            return Json(resultado, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> Update(Cargos modelo, string hdListaPerfiles)
        {

            List<Perfil> perfil = new List<Perfil>();
            string[] ids = hdListaPerfiles.Split(',');
            foreach (string id in ids)
                perfil.Add(new Perfil { IdPerfil = Convert.ToInt32(string.IsNullOrEmpty(id) ? "0" : id) });

            modelo.Perfiles = perfil;

            string segregacion = await PutAsync<IEnumerable<Perfil>, string>("Perfil/ValidarSegurgacion", modelo.Perfiles);

            if (!string.IsNullOrWhiteSpace(segregacion))
            {
                return Json(new RespuestaViewModel { Correcto = true, Mensaje = segregacion, Elemento = modelo.IdCargo }, JsonRequestBehavior.AllowGet);
            }

            var resultado = await PutAsync<Cargos, string>("Cargos/Actualizar", modelo);
            if (string.IsNullOrEmpty(resultado))
                return Json(new RespuestaViewModel { Correcto = true, Mensaje = string.Empty  }, JsonRequestBehavior.AllowGet);
            else
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado, Elemento = modelo.IdCargo }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> UpdateEmail(int idCargo, string hdListaPerfiles)
        {
            Cargos modelo = new Cargos();
            List<Perfil> perfil = new List<Perfil>();
            string[] ids = hdListaPerfiles.Split(',');
            foreach (string id in ids)
                perfil.Add(new Perfil { IdPerfil = Convert.ToInt32(string.IsNullOrEmpty(id) ? "0" : id) });

            modelo.IdCargo = idCargo;
            modelo.Perfiles = perfil;
            modelo.IdCargo = IdUsuarioLogueado;

            var resultado = await PutAsync<Cargos, string>("Cargos/ActualizarEmail", modelo);
            if (string.IsNullOrEmpty(resultado))

                return Json(new RespuestaViewModel { Correcto = true, Mensaje = string.Empty }, JsonRequestBehavior.AllowGet);
            else
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
        }




        public async Task<ActionResult> Obtener(int IdCargo)
        {
            var modelo = await GetAsync<Cargos>($"Cargos/ObtenerxId/{IdCargo}");
            IEnumerable<CargosPerfil> cargosPerfil = await GetAsync<IEnumerable<CargosPerfil>>($"Cargos/ObtenerListaCargoPerfil/{IdCargo}");
            IEnumerable<Perfil>  perfilesObtener =  await GetAsync<IEnumerable<Perfil>>("Perfil/ObtenerLista");

            List<Perfil> perfiles = new List<Perfil>();
            if (cargosPerfil != null)
            {
                foreach (CargosPerfil item in cargosPerfil)
                {
                    Perfil perfil = perfilesObtener.Where(x => x.IdPerfil == item.IdPerfil).FirstOrDefault();
                    if (perfil != null)
                    {
                        perfiles.Add(perfil);
                    }                    
                }
            }
            modelo.ListaPerfiles = perfilesObtener;
            modelo.Perfiles = perfiles;
            ViewBag.lista = await GetAsync<IEnumerable<TipoGeneral>>("Perfil/ObtenerListaSimple");
            return PartialView("_Edit", modelo);
        }

        //public string ValidarPerfil(int idPerfil, string hdListaPerfiles)
        //{
        //    IEnumerable<Perfil> perfiles  = await GetAsync<IEnumerable<Perfil>>($"Perfil/ConsultarSegregacion/{idPerfil}");
        //    string perfilesConflito = string.Empty;


        //    foreach (Perfil perfil in perfiles)
        //    {

        //        foreach (Perfil perfil in perfiles)
        //        {
        //            if (perfiles.Count(x => x.IdPerfil == Convert.ToInt32(string.IsNullOrEmpty(id) ? "0" : id)) > 0)
        //            {
        //                perfilesConflito = string.Concat(perfilesConflito, ",", perfiles.Where(x => x.IdPerfil == Convert.ToInt32(string.IsNullOrEmpty(id) ? "0" : id)).FirstOrDefault().Nombre);
        //            }
        //        }


        //        if (perfiles.Count(x => x.IdPerfil == Convert.ToInt32(string.IsNullOrEmpty(id) ? "0" : id)) > 0)
        //        {
        //            perfilesConflito = string.Concat(perfilesConflito, ",", perfiles.Where(x => x.IdPerfil == Convert.ToInt32(string.IsNullOrEmpty(id) ? "0" : id)).FirstOrDefault().Nombre);
        //        }
        //    }



        //    string[] ids = hdListaPerfiles.Split(',');
        //    foreach (string id in ids)
        //    {
        //        if (perfiles.Count(x => x.IdPerfil == Convert.ToInt32(string.IsNullOrEmpty(id) ? "0" : id)) > 0)
        //        {
        //            perfilesConflito = string.Concat(perfilesConflito, ",", perfiles.Where(x => x.IdPerfil == Convert.ToInt32(string.IsNullOrEmpty(id) ? "0" : id)).FirstOrDefault().Nombre);
        //        }
        //    }

        //    if (string.IsNullOrWhiteSpace(perfilesConflito))
        //    {
        //        return Json(new RespuestaViewModel { Correcto = true, Mensaje = string.Empty, Elemento = idPerfil }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new RespuestaViewModel { Correcto = false, Mensaje = perfilesConflito }, JsonRequestBehavior.AllowGet);
        //    }
            
            
        //}


    }
}