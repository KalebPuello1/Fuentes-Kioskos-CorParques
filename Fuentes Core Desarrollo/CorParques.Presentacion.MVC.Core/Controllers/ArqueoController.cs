//Cambioquitar: Este controlador usa el enumerador de perfiles.
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
using System.Data;

namespace CorParques.Presentacion.MVC.Core.Controllers
{
    public class ArqueoController : ControladorBase
    {
        #region Declaraciones

        private readonly IServicioImprimir _service;

        public ArqueoController(IServicioImprimir service)
        {
            _service = service;
        }

        #endregion

        public async Task<ActionResult> Index()
        {
            // IEnumerable<Arqueo> item = await GetAsync<IEnumerable<Arqueo>>($"Apertura/ObtenerArqueo/idUsuario{0}&IdPunto={IdPunto}"); ;
            //string strPerfiles = ((int)Enumerador.Perfiles.Taquilla).ToString();
            string strPerfiles = string.Empty;
            Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilesArqueo");
            strPerfiles = objParametro.Valor;

            var _listUsuarios = await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosSinAperturaPorPefil?idsPerfil={strPerfiles}&Tipo=1");
            ViewBag.Taquillero = _listUsuarios.Select(x => new TipoGeneral { Id = x.Id, Nombre = string.Concat(x.Nombre, " ", x.Apellido) });                        
            return View();
        }

        public async Task<ActionResult> ObtenerArqueo(int IdUsuario)
        {
            IEnumerable<Arqueo> arqueo = await GetAsync<IEnumerable<Arqueo>>($"Arqueo/ObtenerArqueo?idUsuario={IdUsuario}&IdPunto={IdPunto}");
            ViewBag.NotaCredito = await GetAsync<TipoGeneral>($"Pos/ObtenerListaNotaCredito?Usuario={IdUsuario}");
            ViewBag.Anulaciones = await GetAsync<TipoGeneral>($"Pos/ObtenerListaAnulaciones?Usuario={IdUsuario}");
            foreach (Arqueo item in arqueo)
            {
                ViewBag.Brazaletes = item.Brazalete;
            }
            
            return PartialView("_Arqueo", arqueo) ;
        }

        public async Task<ActionResult> Insert(string TipoNovedad, int IdTaquillero, string Observaciones)
        {
            
            List<NovedadArqueo> _NovedadArqueo = new List<NovedadArqueo>();
            var TipoSplit = TipoNovedad.Split('|');
            var novedad = new NovedadArqueo();

            TicketImprimir objTicketImprimir = new TicketImprimir();
            List<TicketImprimir> objListaTickets = new List<TicketImprimir>();
            List<Articulo> ListaArticulos = new List<Articulo>();
            var user = new Usuario();
            DataTable objDataTable;
            DataRow objDataRow;

            user = await GetAsync<Usuario>($"Usuario/GetById?id={IdTaquillero}&Punto={0}");
            objDataTable = new DataTable();
            objDataTable.Columns.Add("Tipo");
            objDataTable.Columns.Add("Valor");
            objDataTable.Columns.Add("Dif.");

            foreach (string tipo in TipoSplit)
            {
                novedad = new NovedadArqueo();
                var valorSplit = tipo.Split(',');
                novedad.IdEstado = (int)Enumerador.Estados.Activo;
                novedad.IdPunto = IdPunto;
                novedad.IdTaquillero = IdTaquillero;
                novedad.TipoNovedad = (int.Parse(valorSplit[1]) == 0) ? 2 : (int.Parse(valorSplit[1]) > 0 ? 1 : 0);  
                novedad.UsuarioCreado = ((Usuario)Session["UsuarioAutenticado"]).Id;
                novedad.FechaCreado = System.DateTime.Now;
                novedad.Valor = int.Parse(valorSplit[1]) > 0 ? int.Parse(valorSplit[1]) : (int.Parse(valorSplit[1]) < 0 ? (int.Parse(valorSplit[1]) * -1) : 0);
                novedad.IdTipoNovedadArqueo = (int.Parse(valorSplit[0]) + 1);
                novedad.Observaciones = Observaciones;

                string TipoNOvedadArqueo = "";
                if (int.Parse(valorSplit[0]) == 0)
                    TipoNOvedadArqueo = "Efectivo";

                if (int.Parse(valorSplit[0]) == 1)
                    TipoNOvedadArqueo = "Voucher";

                if (int.Parse(valorSplit[0]) == 2)
                    TipoNOvedadArqueo = "Documentos";

                if (int.Parse(valorSplit[0]) == 3)
                {
                    novedad.Observaciones = valorSplit[3];
                    TipoNOvedadArqueo = "Boleteria";
                }

                _NovedadArqueo.Add(novedad);
                objDataRow = objDataTable.NewRow();
                objDataRow["Tipo"] = TipoNOvedadArqueo;
                objDataRow["Valor"] = int.Parse(valorSplit[2]).ToString("C0");
                objDataRow["Dif."] = int.Parse(valorSplit[1]).ToString("C0");
                if(TipoNOvedadArqueo != "Boleteria")
                objDataTable.Rows.Add(objDataRow);
                objDataRow = null;                                          

            }
            if (await PostAsync<List<NovedadArqueo>, string>("Arqueo/Insert", _NovedadArqueo) != null)
            {

                objTicketImprimir.TituloRecibo = string.Concat("Arqueo - ", NombrePunto, "\r\n", "DINERO EN CAJA", "\r\n", string.Empty);
                objTicketImprimir.TablaDetalle = objDataTable;
                objTicketImprimir.EsDataTable = true;
                objListaTickets.Add(objTicketImprimir);
                objTicketImprimir = null;                
                var BrazaletesAsignados = await GetAsync<IEnumerable<CierreBrazalete>>($"Apertura/ObtenerBoleteriaAsignada/{IdTaquillero}/{false}");

                if (BrazaletesAsignados.Count() > 0)
                {
                    objTicketImprimir = new TicketImprimir();

                    foreach (CierreBrazalete objBrazalete in BrazaletesAsignados)
                    {
                        Articulo objArticulo = new Articulo();                        
                        objArticulo.Nombre = objBrazalete.TipoBrazalete;                        
                        objArticulo.Cantidad = objBrazalete.EnCaja;
                        objArticulo.Boleteria = true;
                        objArticulo.Otro = (objBrazalete.TotalVendidos).ToString();
                        ListaArticulos.Add(objArticulo);
                        objArticulo = null;                        
                    }
                    objTicketImprimir.TituloColumnas = "TIPO     |EN CAJA|SALIDAS";
                    objTicketImprimir.ListaArticulos = ListaArticulos;
                    objTicketImprimir.TituloRecibo = "BOLETERIA EN CAJA";
                    objListaTickets.Add(objTicketImprimir);
                }

                objTicketImprimir = new TicketImprimir();                
                objTicketImprimir.Firma = string.Concat("Supervisor: ", NombreUsuarioLogueado, "|", "Taquillero: ", user.Nombre, " ", user.Apellido);
                objTicketImprimir.PieDePagina = string.Concat("Observaciones \r\n", (string.IsNullOrEmpty(Observaciones) ? string.Empty : Observaciones.Trim()));
                objTicketImprimir.ListaTickets = objListaTickets;
                                
                _service.ImprimirTicketArqueo(objTicketImprimir);

                return Json(new RespuestaViewModel { Correcto = true }, JsonRequestBehavior.AllowGet);

            } else {             
            return Json(new RespuestaViewModel { Correcto = false, Mensaje = "Se presento un error creando la novedad de arqueo. Por favor intentelo de nuevo" }, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<ActionResult> ObtenerLogin()
        {
            return PartialView("_Login");
        }
    }
}