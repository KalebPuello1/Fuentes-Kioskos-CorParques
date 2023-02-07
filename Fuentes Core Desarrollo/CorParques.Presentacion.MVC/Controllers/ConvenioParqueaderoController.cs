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
    public class ConvenioParqueaderoController : ControladorBase
    {
        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<ConvenioParqueadero>>("ConvenioParqueadero/ObtenerConvenioParqueadero");
            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<ConvenioParqueadero>>("ConvenioParqueadero/ObtenerConvenioParqueadero");
            return PartialView("_List", lista);
        }

        public async Task<ActionResult> GetPartial()
        {
            //var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.ConvenioParqueadero}");
            //modelo.ListaEstados = listaEstado;

            var modelo = new ConvenioParqueadero();
            var listaTipoVehiculo = await GetAsync<IEnumerable<TipoGeneral>>("TipoVehiculo/ObtenerLista");            
            var listaTipoConvenio = await GetAsync<IEnumerable<TipoGeneral>>("TipoConvenioParqueadero/ObtenerLista");
            var listaEmpleados = await GetAsync<IEnumerable<EstructuraEmpleado>>("ConvenioParqueadero/ObtenerListaEmpleados");
            var listaArea = await GetAsync<IEnumerable<TipoGeneral>>("Area/ObtenerListaAreas");
            modelo.ListaTipoVehiculo = listaTipoVehiculo;            
            modelo.ListaTipoConvenios = listaTipoConvenio;
            modelo.ListaEmpleados = listaEmpleados;
            modelo.ListaAreas = listaArea;            
            return PartialView("_Create", modelo);
        }

        public async Task<ActionResult> Insert(ConvenioParqueadero modelo)
        {
            modelo.FechaCreacion = DateTime.Now;
            modelo.IdUsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.IdEstado = (int)Enumerador.Estados.Activo;
            if (modelo.FechaVigencia != null)
            {
                if (modelo.FechaVigencia.Trim().Length > 0)                
                    modelo.FechaVigencia = Utilidades.FormatoFechaValido(modelo.FechaVigencia).ToString("yyyy/MM/dd");                
            }

            //RDSH: Si el convenio es de tipo 1 Empleado, toma los datos del combo de empleados
            if (modelo.IdTipoConvenioParqueadero == 1)
            {
                string[] strSplit;
                strSplit = modelo.DatosEmpleado.Split('_');
                if (strSplit.Length > 0)
                {
                    modelo.Documento = strSplit[0].ToString();
                    modelo.Nombre = strSplit[1].ToString();
                    modelo.Apellido = strSplit[2].ToString();
                    modelo.Area = strSplit[3].ToString();
                }
            }
            if (modelo.Area == null)
            {
                modelo.Area = string.Empty;
            }


            var resultado = await PostAsync<ConvenioParqueadero, string>("ConvenioParqueadero/Insertar", modelo);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Update(ConvenioParqueadero modelo)
        {
            modelo.FechaModificacion = DateTime.Now;
            modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            if (modelo.FechaVigencia != null)
            {
                if (modelo.FechaVigencia.Trim().Length > 0)
                    modelo.FechaVigencia = Utilidades.FormatoFechaValido(modelo.FechaVigencia).ToString("yyyy/MM/dd");
            }
                 
            if (modelo.Area == null)
            {
                modelo.Area = string.Empty;
            }

            var resultado = await PutAsync<ConvenioParqueadero, string>("ConvenioParqueadero/Actualizar", modelo);
            if (string.IsNullOrEmpty(resultado))
            {
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Obtener(int Id)
        {
            var item = await GetAsync<ConvenioParqueadero>($"ConvenioParqueadero/GetById/{Id}");
            var listaTipoVehiculo = await GetAsync<IEnumerable<TipoGeneral>>("TipoVehiculo/ObtenerLista");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.ConvenioParqueadero}");
            var listaTipoConvenio = await GetAsync<IEnumerable<TipoGeneral>>("TipoConvenioParqueadero/ObtenerLista");
            var listaEmpleados = await GetAsync<IEnumerable<EstructuraEmpleado>>("ConvenioParqueadero/ObtenerListaEmpleados");
            var listaArea = await GetAsync<IEnumerable<TipoGeneral>>("Area/ObtenerListaAreas");

            item.ListaTipoVehiculo = listaTipoVehiculo;
            item.ListaEstados = listaEstado;
            item.ListaTipoConvenios = listaTipoConvenio;
            item.ListaEmpleados = listaEmpleados;
            item.ListaAreas = listaArea;
            return PartialView("_Edit", item);
        }

        public async Task<ActionResult> UpdateEstado(int Id)
        {
            var modelo = await GetAsync<ConvenioParqueadero>($"ConvenioParqueadero/GetById/{Id}");
            modelo.FechaModificacion = DateTime.Now;
            modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id; ;
            modelo.IdEstado = (int)Enumerador.Estados.Inactivo;
            var resultado = await PutAsync<ConvenioParqueadero, string>("ConvenioParqueadero/Eliminar", modelo);
            if (string.IsNullOrEmpty(resultado))
            {
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Detalle(int Id)
        {
            var item = await GetAsync<ConvenioParqueadero>($"ConvenioParqueadero/GetById/{Id}");
            var listaTipoVehiculo = await GetAsync<IEnumerable<TipoGeneral>>("TipoVehiculo/ObtenerLista");
            var listaEstado = await GetAsync<IEnumerable<TipoGeneral>>($"Estado/ObtenerEstados/{(int)Enumerador.ModulosAplicacion.ConvenioParqueadero}");
            var listaTipoConvenio = await GetAsync<IEnumerable<TipoGeneral>>("TipoConvenioParqueadero/ObtenerLista");
            var listaEmpleados = await GetAsync<IEnumerable<EstructuraEmpleado>>("ConvenioParqueadero/ObtenerListaEmpleados");
            var listaArea = await GetAsync<IEnumerable<TipoGeneral>>("Area/ObtenerListaAreas");
            item.ListaTipoVehiculo = listaTipoVehiculo;
            item.ListaEstados = listaEstado;
            item.ListaTipoConvenios = listaTipoConvenio;
            item.ListaEmpleados = listaEmpleados;
            item.ListaAreas = listaArea;
            return PartialView("_Detail", item);
        }
    }
}