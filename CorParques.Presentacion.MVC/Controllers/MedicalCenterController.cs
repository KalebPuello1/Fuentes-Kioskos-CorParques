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
    public class MedicalCenterController : ControladorBase
    {

        #region Metodos

        public IEnumerable<TipoGeneral> ListaTriage()
        {
            List<TipoGeneral> objTriage = new List<TipoGeneral>();

            for (int intI = 1; intI <= 5; intI++)
            {
                TipoGeneral objTipoGeneral = new TipoGeneral{ Id = intI, Nombre = intI.ToString() };
                objTriage.Add(objTipoGeneral);
            }

            return objTriage;
        }

        /// <summary>
        /// RDSH: Formatea la fecha a un formato valido para enviar a guardar.
        /// </summary>
        /// <param name="strFecha"></param>
        /// <returns></returns>
        public DateTime FormatoFechaIncidente(string strFecha, string strHora)
        {
            DateTime datFechaIncidente;          
            string[] strSplit;
            string strFechaArmada = string.Empty;
            
            try
            {
                                              
                strSplit = strFecha.Split('/');
                strFechaArmada = string.Join("/", strSplit[2], strSplit[1], strSplit[0]);
                strFechaArmada = string.Concat(strFechaArmada, " ", strHora);

                datFechaIncidente = DateTime.Parse(strFechaArmada);

            }
            catch (Exception)
            {
                datFechaIncidente = DateTime.Now;
            }

            return datFechaIncidente;

        }

        #endregion

        public async Task<ActionResult> Index()
        {
            var lista = await GetAsync<IEnumerable<Procedimiento>>("Procedimiento/ObtenerListaProcedimientos");
            return View(lista);
        }

        public async Task<ActionResult> GetList()
        {
            var lista = await GetAsync<IEnumerable<Procedimiento>>("Procedimiento/ObtenerListaProcedimientos");
            return PartialView("_List", lista);
        }

        public async Task<ActionResult> GetById(int id)
        {
            var item = await GetAsync<Procedimiento>($"Procedimiento/GetById/{id}");
            //item.objPaciente = await GetAsync<Paciente>($"Paciente/GetById/{item.IdPaciente}");
            var listaUbicacion = await GetAsync<IEnumerable<TipoGeneral>>("CentroMedico/ObtenerListaCentroMedico");
            var listaTipoDocumento = await GetAsync<IEnumerable<TipoGeneral>>("TipoDocumento/ObtenerTipoDocumento");
            var listaCategoriaAtencion = await GetAsync<IEnumerable<TipoGeneral>>("Procedimiento/ObtenerCategoriaAtencion");
            var listaTipoPaciente = await GetAsync<IEnumerable<TipoGeneral>>("Procedimiento/ObtenerTipoPaciente");
            item.ListaUbicacion = listaUbicacion;
            item.ListaTipoDocumento = listaTipoDocumento;
            item.ListaCategoriaAtencion = listaCategoriaAtencion;
            item.ListaTipoPaciente = listaTipoPaciente;
            item.ListaTriage = ListaTriage();
            item.FechaIncidenteDDMMAAAA = item.FechaIncidente.ToString("dd/MM/yyyy");
            item.HoraIncidente = item.FechaIncidente.ToString("HH:mm");
            return PartialView("_Edit", item);
        }
        
        public async Task<ActionResult> GetPartial()
        {
            var modelo = new Procedimiento();
            var listaUbicacion = await GetAsync<IEnumerable<TipoGeneral>>("CentroMedico/ObtenerListaCentroMedico");
            var listaTipoDocumento = await GetAsync<IEnumerable<TipoGeneral>>("TipoDocumento/ObtenerTipoDocumento");
            var listaCategoriaAtencion = await GetAsync<IEnumerable<TipoGeneral>>("Procedimiento/ObtenerCategoriaAtencion");
            var listaTipoPaciente = await GetAsync<IEnumerable<TipoGeneral>>("Procedimiento/ObtenerTipoPaciente");
            var listaEmpleados = await GetAsync<IEnumerable<EstructuraEmpleado>>("ConvenioParqueadero/ObtenerListaEmpleados");
            modelo.ListaUbicacion = listaUbicacion;
            modelo.ListaTipoDocumento = listaTipoDocumento;
            modelo.ListaCategoriaAtencion = listaCategoriaAtencion;
            modelo.ListaTipoPaciente = listaTipoPaciente;
            modelo.ListaTriage = ListaTriage();
            modelo.FechaIncidenteDDMMAAAA = DateTime.Now.ToString("dd/MM/yyyy");
            modelo.HoraIncidente = DateTime.Now.ToString("HH:mm");
            modelo.ListaEmpleados = listaEmpleados;
            return PartialView("_Create", modelo);
        }

        public async Task<ActionResult> Insert(Procedimiento modelo)
        {
            modelo.FechaCreacion = DateTime.Now;
            modelo.IdUsuarioCreacion = (Session["UsuarioAutenticado"] as Usuario).Id;
            modelo.IdEstado = (int)Enumerador.Estados.Activo;
            modelo.FechaIncidente = FormatoFechaIncidente(modelo.FechaIncidenteDDMMAAAA, modelo.HoraIncidente);            
            var resultado = await PostAsync<Procedimiento, string>("Procedimiento/Insertar", modelo);
            return Json(resultado, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> Update(Procedimiento modelo)
        {
            modelo.FechaModificacion = DateTime.Now;
            modelo.IdUsuarioModificacion = (Session["UsuarioAutenticado"] as Usuario).Id; 
            modelo.IdEstado = (int)Enumerador.Estados.Activo;
            modelo.FechaIncidente = FormatoFechaIncidente(modelo.FechaIncidenteDDMMAAAA, modelo.HoraIncidente);
            var resultado = await PutAsync<Procedimiento, string>("Procedimiento/Actualizar", modelo);
            if (string.IsNullOrEmpty(resultado))
            {
                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new RespuestaViewModel { Correcto = false, Mensaje = resultado }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Detalle(int id)
        {
            var item = await GetAsync<Procedimiento>($"Procedimiento/GetById/{id}");
            //item.objPaciente = await GetAsync<Paciente>($"Paciente/GetById/{item.IdPaciente}");
            var listaUbicacion = await GetAsync<IEnumerable<TipoGeneral>>("CentroMedico/ObtenerListaCentroMedico");
            var listaTipoDocumento = await GetAsync<IEnumerable<TipoGeneral>>("TipoDocumento/ObtenerTipoDocumento");
            var listaCategoriaAtencion = await GetAsync<IEnumerable<TipoGeneral>>("Procedimiento/ObtenerCategoriaAtencion");
            var listaTipoPaciente = await GetAsync<IEnumerable<TipoGeneral>>("Procedimiento/ObtenerTipoPaciente");
            var objParametro = await GetAsync<Parametro>("Parameters/ObtenerParametroPorNombre/NotaPrimerosAuxilios");

            item.ListaUbicacion = listaUbicacion;
            item.ListaTipoDocumento = listaTipoDocumento;
            item.ListaCategoriaAtencion = listaCategoriaAtencion;
            item.ListaTipoPaciente = listaTipoPaciente;
            item.ListaTriage = ListaTriage();
            item.FechaIncidenteDDMMAAAA = item.FechaIncidente.ToString("dd/MM/yyyy");
            item.HoraIncidente = item.FechaIncidente.ToString("HH:mm");
            if (objParametro != null)
            {
                item.Notas = objParametro.Valor;
            }
            
            return PartialView("_Detail", item);
        }
    }
}