using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorParques.Negocio.Entidades;
using System.Configuration;
using System.Threading.Tasks;
using System.IO;
using CorParques.Transversales.Util;
using CorParques.Presentacion.MVC.Core.Models;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    
    public class EntregaPremiosController : ControladorBase
    {

        public async Task<ActionResult> Index()
        {

            //Obtiene el codigo sap para el tipo producto premios destrezas.
            //var objLineaProducto = await GetAsync<LineaProducto>($"Pos/ObtenerCodSapPorTipoProducto/{(int)Enumerador.LineaProducto.PremiosDestrezas}");
            //Obtiene los productos del codigo sap premios destrezas.
            //var objProductos = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerProductoPorTipoProducto/{objLineaProducto.CodigoSap}");
            var objProductos = await GetAsync<IEnumerable<Producto>>($"Pos/ObtenerPremiosDestrezas");
            ViewBag.PremiosDestrezas = objProductos;

            return View(objProductos);
        }


        [HttpGet]
        public async Task<ActionResult> EntregarPremio(int IdProducto, string CodigoSap)
        {
            Producto objProducto = new Producto();
            Inventario objInventario = new Inventario();
            List<Producto> objListaProducto = new List<Producto>();

            try
            {

                objProducto.IdProducto = IdProducto;
                objProducto.CodigoSap = CodigoSap;
                objListaProducto.Add(objProducto);

                objInventario.IdPunto = IdPunto;
                objInventario.FechaInventario = DateTime.Now;
                objInventario.IdUsuarioCeado = IdUsuarioLogueado;
                objInventario.Productos = objListaProducto;

                var respuesta = await PostAsync<Inventario, string>("Inventario/ActualizarInventario", objInventario);

                if (string.IsNullOrEmpty(respuesta.Mensaje))
                {
                    return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new RespuestaViewModel { Correcto = false, Mensaje = respuesta.Mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("Error en ValidarAccesoAtraccion comuniquese con el administrador.");
            }            
        } 
    }
}