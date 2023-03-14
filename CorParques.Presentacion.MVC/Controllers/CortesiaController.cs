using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class CortesiaController : ControladorBase
    {
        // public string RutaSoporteCorreos = ConfigurationManager.AppSettings["RutaSoporteCorreos"].ToString();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            int IdTipoCortesia = 1;
            var lista = await GetAsync<IEnumerable<UsuarioVisitante>>($"Cortesia/ListarObtenerUsuarioVisitanteCortesias/{IdTipoCortesia}");
            return View(lista);
        }
        [HttpGet]
        public async Task<ActionResult> IndexEjecutivos()
        {
            int IdTipoCortesia = 5;
            var lista = await GetAsync<IEnumerable<UsuarioVisitante>>($"Cortesia/ListarObtenerUsuarioVisitanteCortesias/{IdTipoCortesia}");
            return View(lista);
        }
        [HttpGet]
        public async Task<ActionResult> Autorizacion()
        {
            var lista = await GetAsync<IEnumerable<ListaCortesia>>("Cortesia/ListarCortesias");

            var lista2 = lista.Any(l => l.Activo == false &&  l.Aprobacion == true)
              ? lista.Where(l => l.Activo == false && l.Aprobacion == true).ToArray()
              : new List<ListaCortesia>().ToArray();

            return View(lista2);
        }
        [HttpGet]
        public async Task<ActionResult> AprobacionCortesia(int IdCortesia)
        {
            var resulAprobacion = await GetAsync<string>($"Cortesia/AprobacionCortesia/{IdCortesia}");

            var lista = await GetAsync<IEnumerable<ListaCortesia>>("Cortesia/ListarCortesias");

            var lista2 = lista.Any(l => l.Activo == false && l.Aprobacion == true)
              ? lista.Where(l => l.Activo == false && l.Aprobacion == true).ToArray()
              : new List<ListaCortesia>().ToArray();

            return PartialView("_ListAutorizacion", lista2);
        }
        
        /// <summary>
        /// Retorna vista partial para crear usuario 
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> CrearUsuarioModal(int IdTipoCortesia)
        {
            var usuario = new UsuarioVisitanteViewModel();
            usuario.IdTipoCortesia = IdTipoCortesia;
            var _ListaTodosProductosSAP = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerTodosProductos");
            ViewBag.ListaTodosProductosSAP = _ListaTodosProductosSAP;

            if (IdTipoCortesia == 1)
            {
                if (_ListaTodosProductosSAP == null)
                {
                    ViewBag.ListaTodosProductosSAP = new List<Mesa>();
                }
                else
                {
                    ViewBag.ListaTodosProductosSAP = _ListaTodosProductosSAP.Where(l => (l.CodSapTipoProducto == "2000") ).ToList();
                }
            }
            else if (IdTipoCortesia == 5)
            {
                if (_ListaTodosProductosSAP == null)
                {
                    ViewBag.ListaTodosProductosSAP = new List<Mesa>();
                }
                else
                {
                    ViewBag.ListaTodosProductosSAP = _ListaTodosProductosSAP.Where(l => (l.CodSapTipoProducto == "2025") && (l.CodSapTipoImpuesto == "G0") ||  (l.CodSapTipoProducto == "2000")).ToList();
                }
            }

            ViewBag.lista = await GetAsync<IEnumerable<TipoGeneral>>("TipoDocumento/ObtenerTipoDocumento");
            ViewBag.ComplejidadCortesia = await GetAsync<IEnumerable<ComplejidadCortesia>>("Cortesia/ObtenerComplejidadCortesia");
            ViewBag.PlazoCortesia = await GetAsync<IEnumerable<PlazoCortesia>>("Cortesia/ObtenerPlazoCortesia");
            ViewBag.TipoRedencionCortesia = await GetAsync<IEnumerable<TipoRedencionCortesia>>("Cortesia/ObtenerTipoRedencionCortesia");
            // return PartialView("_Create", usuario);
            return PartialView("_Create", usuario);
        }

        /// <summary>
        /// Inserta Usuario 
        /// </summary>

        [HttpPost]
        public async Task<ActionResult> InsertarUsuarioVisitante(UsuarioVisitanteViewModel usuario, List<Producto> listaProductosAgregar)
        {

            try
            {
                
                string rta = string.Empty;
                usuario.IdUsuarioCreacion = IdUsuarioLogueado;
                usuario.Activo = true;
                usuario.Aprobacion = false;
                usuario.FechaCreacion = System.DateTime.Now;
                usuario.listaProductosAgregar = listaProductosAgregar;
                int banderaAutorizacion = 0;

                var ParametrosPedido = await GetAsync<IEnumerable<Parametro>>("Parameters/ObtenerParametrosGlobales");
                if (usuario.IdTipoCortesia == 1)
                {
                    var objParametro = ParametrosPedido.Where(x => x.Nombre.Equals("Parametro1CortesiasPQR")).FirstOrDefault();
                    var objParametro2 = ParametrosPedido.Where(x => x.Nombre.Equals("Parametro2CortesiasPQR")).FirstOrDefault();
                    Int64 objParametrovalue = 0;
                    Int64 objParametrovalue2 = 0;
                    if (objParametro != null && objParametro2 != null)
                    {
                        if (objParametro.Valor != null && objParametro2.Valor != null)
                        {
                            if (Regex.IsMatch(objParametro.Valor, @"^[0-9]+$") && Regex.IsMatch(objParametro2.Valor, @"^[0-9]+$"))
                            {
                                objParametrovalue = Convert.ToInt64(objParametro.Valor);
                                objParametrovalue2 = Convert.ToInt64(objParametro2.Valor);
                                if (usuario.Cantidad > objParametrovalue2)
                                {
                                    return Json(new RespuestaViewModel { Correcto = false, Mensaje = "El número de cortesias excede el monto maximo, el valor maximo es de " + objParametrovalue2 }, JsonRequestBehavior.AllowGet);
                                }
                                else if (usuario.Cantidad > objParametrovalue)
                                {
                                    banderaAutorizacion = 1;
                                    usuario.Activo = false;
                                    usuario.Aprobacion = true;

                                }
                            }
                        }

                    }
                }
                else if (usuario.IdTipoCortesia == 5)
                {
                    var objParametro = ParametrosPedido.Where(x => x.Nombre.Equals("Parametro1CortesiasEjecutivos")).FirstOrDefault();
                    var objParametro2 = ParametrosPedido.Where(x => x.Nombre.Equals("Parametro2CortesiasEjecutivos")).FirstOrDefault();
                    Int64 objParametrovalue = 0;
                    Int64 objParametrovalue2 = 0;
                    if (objParametro != null && objParametro2 != null)
                    {
                        if (objParametro.Valor != null && objParametro2.Valor != null)
                        {
                            if (Regex.IsMatch(objParametro.Valor, @"^[0-9]+$") && Regex.IsMatch(objParametro2.Valor, @"^[0-9]+$"))
                            {
                                objParametrovalue = Convert.ToInt64(objParametro.Valor);
                                objParametrovalue2 = Convert.ToInt64(objParametro2.Valor);
                                if (usuario.Cantidad > objParametrovalue2)
                                {
                                    return Json(new RespuestaViewModel { Correcto = false, Mensaje = "El número de cortesias excede el monto maximo, el valor maximo es de " + objParametrovalue2 }, JsonRequestBehavior.AllowGet);
                                }
                                else if (usuario.Cantidad > objParametrovalue)
                                {
                                    banderaAutorizacion = 1;
                                    usuario.Activo = false;
                                    usuario.Aprobacion = true;
                                }
                            }
                        }

                    }
                }
              

                if (usuario.Archivo != null)
                {
                    var path = Path.Combine(Server.MapPath("~/Archivos/tempSoportes/"), usuario.Archivo);
                    usuario.RutaSoporte = path;
                }
                else
                {
                    usuario.RutaSoporte = null;
                }

                var respuesta = await PostAsync<UsuarioVisitanteViewModel, string>("Cortesia/InsertarUsuarioVisitante", usuario);

                if (usuario.Archivo != null)
                {
                    RemoverArchivoTemporal(usuario.Archivo);
                }

                if (string.IsNullOrEmpty(respuesta.Mensaje) && respuesta.Correcto == true)
                {
                    if (banderaAutorizacion == 1)
                    {
                        return Json(new RespuestaViewModel { Correcto = true, Mensaje = "Tener en cuenta que las cortesias requieren de autorizacion para poder ser redimidas de acuerdo a la cantidad solicitada" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
                    }
                   
                }
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el usuario visitante. Por favor intentelo de nuevo" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando el usuario visitante. Por favor intentelo de nuevo" }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Obtener lista de usuarios en vista parcial
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> ObtenerListaUsuarioVisitante(int Idtipocortesia)
        {
            var lista = await GetAsync<IEnumerable<UsuarioVisitante>>($"Cortesia/ListarObtenerUsuarioVisitanteCortesias/{Idtipocortesia}");

            if (Idtipocortesia == 1)
            {
                return RedirectToAction("Index", "Cortesia");

            }
            else
            {
                return RedirectToAction("IndexEjecutivos", "Cortesia");   
            }
           
        }


        //Eliminar Usuario 
        public async Task<ActionResult> Delete(int id)
        {
            var usuario = await GetAsync<UsuarioVisitante>($"Cortesia/ObtenerUsuarioVisitanteId?id={id}");
            if (await PostAsync<UsuarioVisitante, string>("Cortesia/UsuarioVisitante", usuario) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al eliminar el registro del usuario visitante. Por favor intentelo de nuevo" });
        }

        /// <summary>
        /// Retorna objeto Usuario para actualizarlo
        /// </summary>
        public async Task<ActionResult> GetById(int id)
        {
            var usuario = await GetAsync<Usuario>($"Cortesia/ObtenerUsuarioVisitanteId?id={id}");
            return PartialView("_Edit", usuario);
        }

        [HttpPost]
        public void GuardarArchivoTemporal()
        {
            System.Web.HttpPostedFileBase Files;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    Files = file;
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath(@"~/Archivos/tempSoportes/"), fileName);
                    file.SaveAs(path);
                }
            }

        }
        public void RemoverArchivoTemporal(string name)
        {
            var path = Path.Combine(Server.MapPath(@"~/Archivos/tempSoportes/"), name);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }
    }
}