using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Ventas.Core.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Ventas.Core.Controllers
{
    public class BrazaleteReimpresionController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            var brazaletesReimpresion = await GetAsync<List<BrazaleteReimpresion>>($"Pos/ObtenerBrazaleteReimpresion/{IdPunto}");
            return View(brazaletesReimpresion);
        }

        public async Task<ActionResult> GetList()
        {
            var brazaletesReimpresion = await GetAsync<List<BrazaleteReimpresion>>($"Pos/ObtenerBrazaleteReimpresion/{IdPunto}");
            return PartialView("_List", brazaletesReimpresion);
        }

        public async Task<ActionResult> Reimprimir(string[] IdsReimprimir, int IdSupervisor)
        {

            var objReturn = new RespuestaViewModel();

            try
            {
                IEnumerable<Producto> brazaletes = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPasaportesActivos");

                foreach (var item in IdsReimprimir)
                {
                    var consecutivo = item.Split('_');
                    var secuenciaBar = consecutivo[0].ToString();
                    var idProducto = int.Parse(consecutivo[1].ToString());
                    Producto producto = brazaletes.Where(x => x.IdProducto == idProducto).First();
                    if (producto != null && producto.ArchivoProducto != null)
                    {
                        string archivo = Path.Combine(Server.MapPath("~/Temp"), producto.ArchivoProducto.Archivo);
                        WebClient webClient = new WebClient();
                        webClient.DownloadFile(ConfigurationManager.AppSettings["RutaArchivos"].ToString() + producto.ArchivoProducto.Archivo, archivo);
                        //webClient.DownloadFile("http://localhost:62696/Archivos/Brazaletes/asd3.txt", archivo);

                        StringBuilder contenidoEtiqueta = new StringBuilder();
                        contenidoEtiqueta.Append(System.IO.File.ReadAllText(archivo, Encoding.GetEncoding(1252)));
                        contenidoEtiqueta.Replace(ConfigurationManager.AppSettings["TagCodigoBarras"].ToString(), secuenciaBar);

                        string respuesta = PrintDirect.PrintText(contenidoEtiqueta.ToString(), 0);

                        if (!string.IsNullOrWhiteSpace(respuesta))
                        {
                            Utilidades.RegistrarError(new Exception("Error al reimprimir"), respuesta);
                        }
                    }

                }

                objReturn = new RespuestaViewModel { Correcto = true };
            }
            catch (Exception ex)
            {
                objReturn = new RespuestaViewModel { Correcto = false, Mensaje = ex.Message };
            }

            return Json(objReturn, JsonRequestBehavior.AllowGet);
        }
    }
}