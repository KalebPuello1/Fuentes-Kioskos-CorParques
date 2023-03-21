using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ProductosAppController : ControladorBase
    {
        //Ventana principal  
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<Producto>>("ProductosApp/Obtener");
            return View(lista);
        }

        public async Task<ActionResult> CambioEstado(int IdProducto,int activo)
        {

            try
            {
                Producto producto = new Producto();
                producto.IdProducto = IdProducto;
                producto.ActivoApp = activo;
                var respuesta = await PutAsync<Producto, string>($"ProductosApp/Inactivar", producto);

                if (string.IsNullOrWhiteSpace(respuesta))
                {
                    return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al cambiar el estado del producto. Por favor intentelo de nuevo" });
                }
            }
            catch (Exception)
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al inactivar el producto. Por favor intentelo de nuevo" });
            }
            
        }

       
        /// <summary>
        /// Obtener lista de productos en vista parcial
        /// </summary>
        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<Producto>>("ProductosApp/Obtener");
            return PartialView("_List", lista);
        }

        public async Task<ActionResult> GetById(int Id)
        {
            var producto = await GetAsync<Producto>($"ProductosApp/ObtenerId/{Id}");

            return PartialView("_Edit", producto);
        }

        public async Task<ActionResult> Crear(Producto producto)
        {
            try
            {
                Producto productoCrear = await GetAsync<Producto>($"ProductosApp/ObtenerId/{producto.IdProducto}");
                productoCrear.Descricpion = producto.Descricpion;
                productoCrear.Araza = producto.Araza;

                var respuesta = await PostAsync<Producto, string>("ProductosApp/Crear", productoCrear);

                return Json(respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "Crear_ProductosAppController");
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Hubo un inconveniente al crear el producto." }, JsonRequestBehavior.AllowGet);
            }
        }
            
        }
    }
     
