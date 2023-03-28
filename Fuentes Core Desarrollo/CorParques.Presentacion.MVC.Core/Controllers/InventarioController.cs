using CorParques.Negocio.Entidades;
using CorParques.Presentacion.MVC.Core.Models;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CorParques.Transversales.Contratos;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class InventarioController : ControladorBase
    {

        public async Task<ActionResult> Index()
        {
            var item = new TransladoInventario();
            ViewBag.ListPuntos = await GetAsync<IEnumerable<Puntos>>("Puntos/ObtenerPuntosConAlmacen");
            ViewBag.Taquillero = await GetAsync<IEnumerable<Usuario>>("Usuario/ObtenerSinAdmin");
            return View(item);
        }

        public async Task<ActionResult> ObtenerMaterialesxPunto(int IdPunto)
        {
            var item = new TransladoInventario();
            IEnumerable<Materiales> _Materiales = await GetAsync<IEnumerable<Materiales>>($"Inventario/ObtenerMaterialesxPunto/{IdPunto}");
            item.Materiales = _Materiales;
            item.MaterialesAplicados = null;
            return PartialView("_Materiales", item);
        }

        [HttpGet]
        public async Task<ActionResult> PruebaInventario()
        {
            var item = new Inventario();
            List<Producto> listproductos = new List<Producto>();

            var producto2 = new Producto();

            producto2.IdProducto = 110;
            producto2.CodigoSap = "SAP_SerieDulce";

            listproductos.Add(producto2);

            item.IdPunto = IdPunto;
            item.FechaInventario = System.DateTime.Now;
            item.IdUsuarioCeado = (Session["UsuarioAutenticado"] as Usuario).Id;
            item.Productos = listproductos;

            var respuesta = await PostAsync<Inventario, string>("Inventario/ActualizarInventario", item);
            if (string.IsNullOrEmpty(respuesta.Mensaje))
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);

            return Json(new RespuestaViewModel { Correcto = false, Mensaje = respuesta.Mensaje });

        }

        public async Task<ActionResult> ObtenerLogin()
        {
            return PartialView("_Login");
        }

        public async Task<ActionResult> AdicionarMaterial(TransladoInventario modelo, int IdPunto, string ListMateriales)
        {

            var MaterialAdd = new Materiales();

            IEnumerable<Materiales> _Materiales = await GetAsync<IEnumerable<Materiales>>($"Inventario/ObtenerMaterialesxPunto/{IdPunto}");
            modelo.Materiales = _Materiales;
            List<Materiales> _MaterialesAplicados = new List<Materiales>();
            foreach (string CodSap in ListMateriales.Split(';'))
            {
                if (CodSap != "")
                {
                    var split = CodSap.Split('|');
                    double CantidadSplit = double.Parse(split[1]);
                    MaterialAdd = modelo.Materiales.Where(x => x.CodigoSap == split[0]).FirstOrDefault();
                    MaterialAdd.Cantidad = CantidadSplit;
                    _MaterialesAplicados.Add(MaterialAdd);

                }
            }
            if (_MaterialesAplicados.Count() > 0)
                modelo.MaterialesAplicados = _MaterialesAplicados;

            return PartialView("_Materiales", modelo);

        }

        public async Task<ActionResult> Guardar(IEnumerable<TransladoInventario> modelo)
        {
            foreach (TransladoInventario item in modelo)
            {
                if (item.idUsuario == 0)
                {
                    item.idUsuario = (Session["UsuarioAutenticado"] as Usuario).Id;
                }
                item.IdUsuarioRegistro = (Session["UsuarioAutenticado"] as Usuario).Id;
                item.Procesado = false;
            }

            var respuesta = await PostAsync<IEnumerable<TransladoInventario>, string>("Inventario/ActualizarTransladoInventario", modelo);
            if (respuesta.Mensaje == null)
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al realizar el translado. Por favor intentelo de nuevo" }, JsonRequestBehavior.AllowGet);

            return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);

        }

        public async Task<JsonResult> GuardarInventario(AjustesInventario[] modelo)
        {
            IEnumerable<MotivosInventario> motivosInv = await GetAsync<IEnumerable<MotivosInventario>>("Inventario/BuscarMotivosInventario");
            string motivosB = "";
            string motivosC = "";
            foreach (AjustesInventario item in modelo)
            {
                item.FechaAjuste = System.DateTime.Now;
                item.FechaRegistro = System.DateTime.Now;
                item.IdPunto = IdPunto;
                item.IdUsuarioRegistro = (Session["UsuarioAutenticado"] as Usuario).Id;
                item.CodSapMotivo = item.CodSapTipoAjuste == "C" ? "12" : "13";
                if (!(string.IsNullOrEmpty(item.Observaciones)))
                {
                    item.Observaciones = System.Uri.UnescapeDataString(item.Observaciones);

                }
            }
            var respuesta = await PostAsync<IEnumerable<AjustesInventario>, string>("Inventario/ActualizarAjusteInventario", modelo.Where(x => x.Cantidad > 0));
            if (respuesta.Mensaje == null)
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al realizar el ajuste. Por favor intentelo de nuevo" }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> Ajustes()
        {
            AjustesInventario modelAjusteInv = new AjustesInventario();
            modelAjusteInv.ListaPuntos = await GetAsync<List<Puntos>>("Puntos/ObtenerPuntosConAlmacen");
            modelAjusteInv.ListaTipoAjuste = await GetAsync<List<TipoAjusteInventario>>("Inventario/ObtenerTiposAjuste");

            return View(modelAjusteInv);
        }

        public async Task<ActionResult> ObtenerMaterialesxPuntoAjax(int IdPunto)
        {
            var listaMateriales = await GetAsync<List<Materiales>>($"Inventario/ObtenerMaterialesxPunto/{IdPunto}");

            if (listaMateriales == null)
                listaMateriales = new List<Materiales>();

            return Json(new RespuestaViewModel { Correcto = true, Elemento = listaMateriales }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ObtenerMotivosxTipoAjuste(string CodSapAjuste)
        {
            var listaMotivos = await GetAsync<List<MotivosInventario>>($"Inventario/ObtenerMotivosAjuste/{CodSapAjuste}");

            if (listaMotivos == null)
                listaMotivos = new List<MotivosInventario>();

            return Json(new RespuestaViewModel { Correcto = true, Elemento = listaMotivos }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AjustesGuardar(List<AjustesInventario> modelo)
        {
            foreach (AjustesInventario item in modelo)
            {
                item.IdUsuarioAjuste = (Session["UsuarioAutenticado"] as Usuario).Id;
                item.IdUsuarioRegistro = (Session["UsuarioAutenticado"] as Usuario).Id;
                item.Procesado = false;
            }

            if (await PostAsync<IEnumerable<AjustesInventario>, string>("Inventario/InsertarAjusteInventario", modelo) != null)
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error al realizar el translado. Por favor intentelo de nuevo" });
        }

        //GALD Ajustes de entrada y salida

        public async Task<ActionResult> InventarioFisico()
        {

            IEnumerable<Materiales> _Materiales = await GetAsync<IEnumerable<Materiales>>($"Inventario/ObtenerMaterialesxPunto/{IdPunto}");

            ViewBag.Motivos = await GetAsync<List<MotivosInventario>>("Inventario/ObtenerTodosMotivos");

            if (ViewBag.Motivos == null)
                ViewBag.Motivos = new List<MotivosInventario>();


            return View(_Materiales);
        }


        //Enviar y crear email
            [HttpGet]
            public async Task<string> CreaYenviaMail(string Observaciones, string Nombres, string Diferencia, string ArregloTeorico, string ArregloCamtidadDisponible, string ArregloTipoMovimiento)
            {
                string[] observaciones = new string[0];
                string[] nombres = new string[0];
                string[] diferencia = new string[0];
                string[] arregloTeorico = new string[0];
                string[] arregloCamtidadDisponible = new string[0];
                string[] arregloTipoMovimiento = new string[0];
                try
                {
                    observaciones = Observaciones.Split(',');
                    nombres = Nombres.Split(',');
                    diferencia = Diferencia.Split(';');
                    arregloTeorico = ArregloTeorico.Split(',');
                    arregloCamtidadDisponible = ArregloCamtidadDisponible.Split(',');
                    arregloTipoMovimiento = ArregloTipoMovimiento.Split(',');
                } catch (Exception e)
                {
                    return "Existe un error " + e;
                }

                Reportes reportes = new Reportes();
                Parametro _parametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CORREOALVERTENCIA");//PROPINA

                string preto =  _parametro.Valor;
                string subject = "Inventario Físico";
                string mensaje = "Adjunto Inventario Físico";
                List<Inventario> inv = new List<Inventario>();
                string nombreExcel = "lol";
                string f = "";
                if (observaciones.Length != 0 && nombres.Length != 0 && diferencia.Length != 0)
                {
                    for (var l = 0; (l < observaciones.Length && l < nombres.Length); l++)
                    {
                        if (observaciones[l] != "" && nombres[l] != "" && nombres[l] != "")
                        {
                            Inventario i = new Inventario();
                            i.Perfil = NombreUsuarioLogueado;
                            i.Motivo = observaciones[l] == "No hay datos para mostrar"? "Inventario en 0,00" : "Cierre punto";
                            i.Observacion = observaciones[l];
                            i.NProducto = nombres[l];
                            i.Diferencia = diferencia[l];
                            i.arregloTeorico = arregloTeorico[l];
                            i.arregloCantidadDisponible = arregloCamtidadDisponible[l];
                            i.arregloTipoMovimiento = arregloTipoMovimiento[l];
                            nombreExcel = "Inventario";
                            inv.Add(i);
                        }
                    }
                }

                string nombreUsuario = NombreUsuarioLogueado;
                List<Parametro> par = new List<Parametro>();
                par.Add(new Parametro() { Nombre = "NProducto", Valor = "Nombre Producto" });
                par.Add(new Parametro() { Nombre = "Diferencia", Valor = "Diferencia" });
                par.Add(new Parametro() { Nombre = "Motivo", Valor = "Motivo" });
                par.Add(new Parametro() { Nombre = "Observacion", Valor = "Observacion" });
                par.Add(new Parametro() { Nombre = "Perfil", Valor = "Usuario Ajuste"});
                par.Add(new Parametro() { Nombre = "arregloTeorico", Valor = "Inventario Teorico" });
                par.Add(new Parametro() { Nombre = "arregloCantidadDisponible", Valor = "Inventario Real" });
                par.Add(new Parametro() { Nombre = "arregloTipoMovimiento", Valor = "Tipo Movimiento" });
                string Nom = reportes.GenerarReporteExcel(inv, par, nombreExcel);
                string[] n = Nom.Split('.');
                string attachmentt = n[0];

                string data = "";
            
                data = await GetAsync<string>($"Inventario/Maill/{preto}/{subject}/{mensaje}/{attachmentt}");
                string[] ff = data.Split(',');
                if (ff.Length > 0)
                {
                    foreach (var t in ff) {
                        if (t != "") {
                            if (System.IO.File.Exists(t))
                            {
                                System.IO.File.Delete(t);
                            }
                        }
                    }
                }
                else
                {
                    if (System.IO.File.Exists(data))
                    {
                        System.IO.File.Delete(data);
                    }
                }

                return data;
            }

    }
}