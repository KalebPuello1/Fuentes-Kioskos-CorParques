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



        [HttpGet]
        public async Task<string> GenerarArchivo(Materiales modelo)
        {
            int Id_Supervisor = modelo.Id_Supervisor;

            var usuario = await GetAsync<Usuario>($"Usuario/GetById?id={Id_Supervisor}&Punto={0}");
            var NombreSupervisor = usuario.Nombre + " " + usuario.Apellido;

            var Idpunto = IdPunto;
            var NombreDePunto = NombrePunto;
            var IdUserLogueado = IdUsuarioLogueado;
            var NombreUsuario = NombreUsuarioLogueado;

            var FechaInventario = DateTime.Now;
            var CodSapAlmacen = await GetAsync<string>($"Inventario/ObtenerCodSapAlmacenInventarioFisico/{Idpunto}");

            string[] observaciones = new string[0];
            string[] nombres = new string[0];
            string[] diferencia = new string[0];
            string[] arregloTeorico = new string[0];
            string[] arregloCamtidadDisponible = new string[0];
            string[] arregloTipoMovimiento = new string[0];

            string strRetorno = string.Empty;

            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();


            ReporteInventarioFisico ObjReporte = new ReporteInventarioFisico();

            var NombreMateriales = ObjReporte.NombresFisicoR = modelo.NombresFisico;
            var CantidadFisica = ObjReporte.ArregloInventarioFisicoR = modelo.ArregloInventarioFisico;
            var CantidadTeorica = ObjReporte.ArregloTeoricoFisicoR = modelo.ArregloTeoricoFisico;
            var Observaciones = ObjReporte.ObservacionesFisicoR = modelo.ObservacionesFisico;
            var Diferencias = ObjReporte.DiferenciasR = modelo.Diferencias;
            var TipoMovimientos = ObjReporte.TipoMovimientosR = modelo.TipoMovimientos;
            var IdSupervisorr = ObjReporte.IdSupervisor = modelo.Id_Supervisor;

            observaciones = Observaciones.Split(',');
            nombres = NombreMateriales.Split(',');
            diferencia = Diferencias.Split(';');
            arregloTeorico = CantidadTeorica.Split(',');
            arregloCamtidadDisponible = CantidadFisica.Split(',');
            arregloTipoMovimiento = TipoMovimientos.Split(',');

            try
            {
                IEnumerable<Materiales> _Materiales = await GetAsync<IEnumerable<Materiales>>($"Inventario/ObtenerMaterialesxPunto/{IdPunto}");

                int a = 0;

                foreach (var item in _Materiales)
                {

                    item.CantidadFisica = arregloCamtidadDisponible[a];
                    item.CantidadTeorica = arregloTeorico[a];
                    item.Observacion = observaciones[a];
                    item.Diferencia = diferencia[a];
                    item.TipoMovimiento = arregloTipoMovimiento[a];
                    item.FechaInventario = FechaInventario;
                    item.NombrePunto = NombreDePunto;
                    item.NombreUsuario = NombreUsuario;
                    item.CodigoSapAlmacen = CodSapAlmacen;
                    item.id_Punto = Idpunto;
                    item.IdUsuario = IdUserLogueado;
                    item.Id_Supervisor = Id_Supervisor;
                    item.NombreSupervisor = NombreSupervisor;

                    a++;
                }

                if (_Materiales != null)
                {
                    if (_Materiales.Count() > 0)
                        strRetorno = objReportes.GenerarReporteInventarioFisico(_Materiales);
                }
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("Se produjo una excepción de tipo 'System.OutOfMemoryException'."))
                {
                    strRetorno = string.Concat("Error generando reporte: ", "Muchos datos para exportar - MAX 4 - 12 dias para consultar ");
                }
                else
                {
                    strRetorno = string.Concat("Error generando reporte: ", ex.Message);
                }
            }

            if (strRetorno.Contains("Error en GenerarReporte: Se produjo una excepción de tipo 'System.OutOfMemoryException'."))
            {
                strRetorno = string.Concat("Error generando reporte: ", "Muchos datos para exportar - MAX 4 - 12 dias para consultar ");
            }

            return strRetorno;
        }

        [HttpGet]
        public async Task<string> RegistrarInventarioFisico(Materiales modelo)
        {
            //variables:

            int Id_Supervisor = modelo.Id_Supervisor;
            var usuario = await GetAsync<Usuario>($"Usuario/GetById?id={Id_Supervisor}&Punto={0}");
            var NombreSupervisor = usuario.Nombre + " " + usuario.Apellido;


            var Idpunto = IdPunto;
            var NombreDePunto = NombrePunto;
            var IdUserLogueado = IdUsuarioLogueado;
            var NombreUsuario = NombreUsuarioLogueado;
            var FechaInventario = DateTime.Now;
            var CodSapAlmacen = await GetAsync<string>($"Inventario/ObtenerCodSapAlmacenInventarioFisico/{Idpunto}");

            string[] observaciones = new string[0];
            string[] nombres = new string[0];
            string[] diferencia = new string[0];
            string[] arregloTeorico = new string[0];
            string[] arregloCamtidadDisponible = new string[0];
            string[] arregloTipoMovimiento = new string[0];

            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();


            ReporteInventarioFisico ObjReporte = new ReporteInventarioFisico();

            var NombreMateriales = ObjReporte.NombresFisicoR = modelo.NombresFisico;
            var CantidadFisica = ObjReporte.ArregloInventarioFisicoR = modelo.ArregloInventarioFisico;
            var CantidadTeorica = ObjReporte.ArregloTeoricoFisicoR = modelo.ArregloTeoricoFisico;
            var Observaciones = ObjReporte.ObservacionesFisicoR = modelo.ObservacionesFisico;
            var Diferencias = ObjReporte.DiferenciasR = modelo.Diferencias;
            var TipoMovimientos = ObjReporte.TipoMovimientosR = modelo.TipoMovimientos;
            var IdSupervisor = ObjReporte.IdSupervisor = modelo.Id_Supervisor;

            observaciones = Observaciones.Split(',');
            nombres = NombreMateriales.Split(',');
            diferencia = Diferencias.Split(';');

            arregloTeorico = CantidadTeorica.Split(',');
            arregloCamtidadDisponible = CantidadFisica.Split(',');
            arregloTipoMovimiento = TipoMovimientos.Split(',');

            try
            {
                IEnumerable<Materiales> _Materiales = await GetAsync<IEnumerable<Materiales>>($"Inventario/ObtenerMaterialesxPunto/{IdPunto}");

                int a = 0;

                foreach (var item in _Materiales)
                {

                    item.CantidadFisica = arregloCamtidadDisponible[a];
                    item.CantidadTeorica = arregloTeorico[a];
                    item.Observacion = observaciones[a];
                    item.Diferencia = diferencia[a];
                    item.TipoMovimiento = arregloTipoMovimiento[a];
                    item.FechaInventario = FechaInventario;
                    item.NombrePunto = NombreDePunto;
                    item.NombreUsuario = NombreUsuario;
                    item.CodigoSapAlmacen = CodSapAlmacen;
                    item.id_Punto = Idpunto;
                    item.IdUsuario = IdUserLogueado;
                    item.Id_Supervisor = IdSupervisor;
                    item.NombreSupervisor = NombreSupervisor;


                    a++;
                }

                var dato = await PostAsync<IEnumerable<Materiales>, string>("Inventario/InsertarDetalleInventarioFisico", _Materiales);

                if (dato == null && dato.Correcto == false)
                {
                    strRetorno = string.Concat("Error generando inserción: ", "Inventario físico nó se pudo insertar correctamente");
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error generando inserción de los datos: ", ex.Message);
            }
            return strRetorno;

        }

        [HttpGet]
        public virtual FileContentResult Download(string Data)
        {
            FileContentResult objFileContentResult = null;
            try
            {
                objFileContentResult = File(Transversales.Util.Utilidades.ObtenerBytesArchivo(Data), System.Net.Mime.MediaTypeNames.Application.Octet, Data);
            }
            catch (Exception)
            {
                return null;
            }
            return objFileContentResult;
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

        
        public async Task<ActionResult> CreaYenviaMail(Materiales modelo)

        {
            //variables:

            int Id_Supervisor = modelo.Id_Supervisor;

            var usuario = await GetAsync<Usuario>($"Usuario/GetById?id={Id_Supervisor}&Punto={0}");
            var NombreSupervisor = usuario.Nombre + " " + usuario.Apellido;

            var Idpunto = IdPunto;
            var NombreDePunto = NombrePunto;
            var IdUserLogueado = IdUsuarioLogueado;
            var NombreUsuario = NombreUsuarioLogueado;
            var FechaInventario = DateTime.Now;
            var CodSapAlmacen = await GetAsync<string>($"Inventario/ObtenerCodSapAlmacenInventarioFisico/{Idpunto}");


            string[] observaciones = new string[0];
            string[] nombres = new string[0];
            string[] diferencias = new string[0];
            string[] arregloTeorico = new string[0];
            string[] arregloCamtidadDisponible = new string[0];
            string[] arregloTipoMovimiento = new string[0];

            string strRetorno = string.Empty;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();


            ReporteInventarioFisico ObjReporte = new ReporteInventarioFisico();

            var NombreMateriales = ObjReporte.NombresFisicoR = modelo.NombresFisico;
            var CantidadFisica = ObjReporte.ArregloInventarioFisicoR = modelo.ArregloInventarioFisico;
            var CantidadTeorica = ObjReporte.ArregloTeoricoFisicoR = modelo.ArregloTeoricoFisico;
            var Observaciones = ObjReporte.ObservacionesFisicoR = modelo.ObservacionesFisico;
            var Diferencias = ObjReporte.DiferenciasR = modelo.Diferencia;
            var TipoMovimientos = ObjReporte.TipoMovimientosR = modelo.TipoMovimientos;

            observaciones = Observaciones.Split(',');
            nombres = NombreMateriales.Split(',');
            diferencias = Diferencias.Split(';');
            arregloTeorico = CantidadTeorica.Split(',');
            arregloCamtidadDisponible = CantidadFisica.Split(',');
            arregloTipoMovimiento = TipoMovimientos.Split(',');

            try
            {
                
                IEnumerable<Materiales> _Materiales = await GetAsync<IEnumerable<Materiales>>($"Inventario/ObtenerMaterialesxPunto/{IdPunto}");

                int a = 0;

                foreach (var item in _Materiales)
                {

                    item.CantidadFisica = arregloCamtidadDisponible[a];
                    item.CantidadTeorica = arregloTeorico[a];
                    item.Observacion = observaciones[a];
                    item.Diferencia = diferencias[a];
                    item.TipoMovimiento = arregloTipoMovimiento[a];
                    item.FechaInventario = FechaInventario;
                    item.NombrePunto = NombreDePunto;
                    item.NombreUsuario = NombreUsuario;
                    item.CodigoSapAlmacen = CodSapAlmacen;
                    item.id_Punto = Idpunto;
                    item.IdUsuario = IdUserLogueado;
                    item.Id_Supervisor = Id_Supervisor;
                    item.NombreSupervisor = NombreSupervisor;

                    a++;
                }

                if (_Materiales != null)
                {
                    if (_Materiales.Count() > 0)
                        strRetorno = objReportes.GenerarReporteInventarioFisico(_Materiales);
                }

            }
            catch (Exception e)
            {
                return Json(new { dato = "Su operación falló - no se logró modificar las fechas correctamente", error = "error" });
            }

            //------------------------------------------------------------------------------------------------------------------------------------------
            //-------------------------------------------------------[ TAQUILLAS ]----------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------------------------------------------------
            if (CodSapAlmacen == "C253")
            {

                Parametro _parametro1 = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CorreoInventarioFisicoTaquillas");

                DateTime MyDateTime = FechaInventario;
                int year = MyDateTime.Year;
                int month = MyDateTime.Month;
                int day = MyDateTime.Day;

                var FechaEditada = string.Format("{0}/{1}/{2}", day, month, year);

                string to = _parametro1.Valor;
                string subject = "INVENTARIO FÍSICO - PUNTO " + NombreDePunto + " - FECHA - " + FechaEditada;
                string mensaje = "Adjunto el reporte Excel del inventario fisico generado en el punto : " + NombreDePunto;
                string attachmentt = strRetorno;

                DetalleCorreo objDetalleCorreo = new DetalleCorreo();

                objDetalleCorreo.To = to;
                objDetalleCorreo.Subject = subject;
                objDetalleCorreo.Mensaje = mensaje;
                objDetalleCorreo.Attachmentt = attachmentt;

                var dato = await PostAsync<DetalleCorreo, string>("Inventario/EmailInventarioFisico", objDetalleCorreo);

                if (!dato.Elemento.ToString().Contains("fallo"))
                {
                    return Json(new { dato = "Su operación fue satisfactoria", success = "success", ObjDetalleCorreo = objDetalleCorreo, to = objDetalleCorreo.To, subject = objDetalleCorreo.Subject, mensaje = objDetalleCorreo.Mensaje, attachmentt = objDetalleCorreo.Attachmentt });
                }
                else
                {
                    return Json(new { dato = "Su operación falló - no se logró modificar las fechas correctamente", error = "error" });
                }
             }

            //------------------------------------------------------------------------------------------------------------------------------------------
            //-------------------------------------------------------[ DESTREZAS ]----------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------------------------------------------------

            else if (CodSapAlmacen == "C238" || CodSapAlmacen == "C239" || CodSapAlmacen == "C240" || CodSapAlmacen == "C241" || CodSapAlmacen == "C242" || CodSapAlmacen == "C243"

                || CodSapAlmacen == "C244" || CodSapAlmacen == "C245" || CodSapAlmacen == "C246" || CodSapAlmacen == "C247" || CodSapAlmacen == "C248" || CodSapAlmacen == "C249"

                || CodSapAlmacen == "C250" || CodSapAlmacen == "C251")

                {

                Parametro _parametro2 = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CorreoInventarioFisicoDestrezas");

                DateTime MyDateTime = FechaInventario;
                int year = MyDateTime.Year;
                int month = MyDateTime.Month;
                int day = MyDateTime.Day;

                var FechaEditada = string.Format("{0}/{1}/{2}", day, month, year);

                string to = _parametro2.Valor;
                string subject = "INVENTARIO FÍSICO - PUNTO " + NombreDePunto + " - FECHA - " + FechaEditada;
                string mensaje = "Adjunto el reporte Excel del inventario fisico generado en el punto : " + NombreDePunto;
                string attachmentt = strRetorno;

                DetalleCorreo objDetalleCorreo = new DetalleCorreo();

                objDetalleCorreo.To = to;
                objDetalleCorreo.Subject = subject;
                objDetalleCorreo.Mensaje = mensaje;
                objDetalleCorreo.Attachmentt = attachmentt;

                var dato = await PostAsync<DetalleCorreo, string>("Inventario/EmailInventarioFisico", objDetalleCorreo);

                if (!dato.Elemento.ToString().Contains("fallo"))
                {
                    return Json(new { dato = "Su operación fue satisfactoria", success = "success", ObjDetalleCorreo = objDetalleCorreo, to = objDetalleCorreo.To, subject = objDetalleCorreo.Subject, mensaje = objDetalleCorreo.Mensaje, attachmentt = objDetalleCorreo.Attachmentt });
                }
                else
                {
                    return Json(new { dato = "Su operación falló - no se logró modificar las fechas correctamente", error = "error" });
                }
            }
            //------------------------------------------------------------------------------------------------------------------------------------------
            //-------------------------------------------------------[ SOUVENIRES ]----------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------------------------------------------------

            else if (CodSapAlmacen == "C234" || CodSapAlmacen == "C261" || CodSapAlmacen == "C269" || CodSapAlmacen == "C270")
                {

                Parametro _parametro3 = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CorreoInventarioFisicoSouvenires");

                DateTime MyDateTime = FechaInventario;
                int year = MyDateTime.Year;
                int month = MyDateTime.Month;
                int day = MyDateTime.Day;

                var FechaEditada = string.Format("{0}/{1}/{2}", day, month, year);

                string to = _parametro3.Valor;
                string subject = "INVENTARIO FÍSICO - PUNTO " + NombreDePunto + " - FECHA - " + FechaEditada;
                string mensaje = "Adjunto el reporte Excel del inventario fisico generado en el punto : " + NombreDePunto;
                string attachmentt = strRetorno;

                DetalleCorreo objDetalleCorreo = new DetalleCorreo();

                objDetalleCorreo.To = to;
                objDetalleCorreo.Subject = subject;
                objDetalleCorreo.Mensaje = mensaje;
                objDetalleCorreo.Attachmentt = attachmentt;

                var dato = await PostAsync<DetalleCorreo, string>("Inventario/EmailInventarioFisico", objDetalleCorreo);

                if (!dato.Elemento.ToString().Contains("fallo"))
                {
                    return Json(new { dato = "Su operación fue satisfactoria", success = "success", ObjDetalleCorreo = objDetalleCorreo, to = objDetalleCorreo.To, subject = objDetalleCorreo.Subject, mensaje = objDetalleCorreo.Mensaje, attachmentt = objDetalleCorreo.Attachmentt });
                }
                else
                {
                    return Json(new { dato = "Su operación falló - no se logró modificar las fechas correctamente", error = "error" });
                }
            }

            //------------------------------------------------------------------------------------------------------------------------------------------
            //-------------------------------------------------------[ A Y B ]----------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------------------------------------------------

            else if (CodSapAlmacen == "C207" || CodSapAlmacen == "C209" || CodSapAlmacen == "C212" || CodSapAlmacen == "C213" || CodSapAlmacen == "C214" || CodSapAlmacen == "C215"

                    || CodSapAlmacen == "C216" || CodSapAlmacen == "C217" || CodSapAlmacen == "C219" || CodSapAlmacen == "C220" || CodSapAlmacen == "C223" || CodSapAlmacen == "C224"

                    || CodSapAlmacen == "C253" || CodSapAlmacen == "C231" || CodSapAlmacen == "C232" || CodSapAlmacen == "C233" || CodSapAlmacen == "C235" || CodSapAlmacen == "C200"

                    || CodSapAlmacen == "C211" || CodSapAlmacen == "C218" || CodSapAlmacen == "C230" || CodSapAlmacen == "C268")
            {

                Parametro _parametro3 = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CorreoInventarioFisicoAyB");

                DateTime MyDateTime = FechaInventario;
                int year = MyDateTime.Year;
                int month = MyDateTime.Month;
                int day = MyDateTime.Day;

                var FechaEditada = string.Format("{0}/{1}/{2}", day, month, year);

                string to = _parametro3.Valor;
                string subject = "INVENTARIO FÍSICO - PUNTO " + NombreDePunto + " - FECHA - " + FechaEditada;
                string mensaje = "Adjunto el reporte Excel del inventario fisico generado en el punto : " + NombreDePunto;
                string attachmentt = strRetorno;

                DetalleCorreo objDetalleCorreo = new DetalleCorreo();

                objDetalleCorreo.To = to;
                objDetalleCorreo.Subject = subject;
                objDetalleCorreo.Mensaje = mensaje;
                objDetalleCorreo.Attachmentt = attachmentt;

                var dato = await PostAsync<DetalleCorreo, string>("Inventario/EmailInventarioFisico", objDetalleCorreo);

                if (!dato.Elemento.ToString().Contains("fallo"))
                {
                    return Json(new { dato = "Su operación fue satisfactoria", success = "success", ObjDetalleCorreo = objDetalleCorreo, to = objDetalleCorreo.To, subject = objDetalleCorreo.Subject, mensaje = objDetalleCorreo.Mensaje, attachmentt = objDetalleCorreo.Attachmentt });
                }
                else
                {
                    return Json(new { dato = "Su operación falló - no se logró modificar las fechas correctamente", error = "error" });
                }
            }

            //------------------------------------------------------------------------------------------------------------------------------------------
            //-------------------------------------------------------[ ARAZA ]----------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------------------------------------------------

            else if (CodSapAlmacen == "C203")
            {

                Parametro _parametro3 = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CorreoInventarioFisicoAraza");

                DateTime MyDateTime = FechaInventario;
                int year = MyDateTime.Year;
                int month = MyDateTime.Month;
                int day = MyDateTime.Day;

                var FechaEditada = string.Format("{0}/{1}/{2}", day, month, year);

                string to = _parametro3.Valor;
                string subject = "INVENTARIO FÍSICO - PUNTO " + NombreDePunto + " - FECHA - " + FechaEditada;
                string mensaje = "Adjunto el reporte Excel del inventario fisico generado en el punto : " + NombreDePunto;
                string attachmentt = strRetorno;

                DetalleCorreo objDetalleCorreo = new DetalleCorreo();

                objDetalleCorreo.To = to;
                objDetalleCorreo.Subject = subject;
                objDetalleCorreo.Mensaje = mensaje;
                objDetalleCorreo.Attachmentt = attachmentt;

                var dato = await PostAsync<DetalleCorreo, string>("Inventario/EmailInventarioFisico", objDetalleCorreo);

                if (!dato.Elemento.ToString().Contains("fallo"))
                {
                    return Json(new { dato = "Su operación fue satisfactoria", success = "success", ObjDetalleCorreo = objDetalleCorreo, to = objDetalleCorreo.To, subject = objDetalleCorreo.Subject, mensaje = objDetalleCorreo.Mensaje, attachmentt = objDetalleCorreo.Attachmentt });
                }
                else
                {
                    return Json(new { dato = "Su operación falló - no se logró modificar las fechas correctamente", error = "error" });
                }
            }
            //------------------------------------------------------------------------------------------------------------------------------------------
            //-------------------------------------------------------[ MUNDO NATURAL ]----------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------------------------------------------------

            else if (CodSapAlmacen == "C103" || CodSapAlmacen == "C105")
            {

                Parametro _parametro3 = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/CorreoInventarioFisicoMundoNatural");

                DateTime MyDateTime = FechaInventario;
                int year = MyDateTime.Year;
                int month = MyDateTime.Month;
                int day = MyDateTime.Day;

                var FechaEditada = string.Format("{0}/{1}/{2}", day, month, year);

                string to = _parametro3.Valor;
                string subject = "INVENTARIO FÍSICO - PUNTO " + NombreDePunto + " - FECHA - " + FechaEditada;
                string mensaje = "Adjunto el reporte Excel del inventario fisico generado en el punto : " + NombreDePunto;
                string attachmentt = strRetorno;

                DetalleCorreo objDetalleCorreo = new DetalleCorreo();
                

                objDetalleCorreo.To = to;
                objDetalleCorreo.Subject = subject;
                objDetalleCorreo.Mensaje = mensaje;
                objDetalleCorreo.Attachmentt = attachmentt;

                var dato = await PostAsync<DetalleCorreo, string>("Inventario/EmailInventarioFisico", objDetalleCorreo);

                if (!dato.Elemento.ToString().Contains("fallo"))
                {
                    return Json(new { dato = "Su operación fue satisfactoria", success = "success", ObjDetalleCorreo = objDetalleCorreo, to = objDetalleCorreo.To, subject = objDetalleCorreo.Subject, mensaje = objDetalleCorreo.Mensaje, attachmentt = objDetalleCorreo.Attachmentt });
                }
                else
                {
                    return Json(new { dato = "Su operación falló - no se logró modificar las fechas correctamente", error = "error" });
                }
            }


            return Json(new { dato = "Debe ser el siguiente punto", error = "error" });


            }
        }
    }




