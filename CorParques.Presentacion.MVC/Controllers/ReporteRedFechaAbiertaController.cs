using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ReporteRedFechaAbiertaController : ControladorBase
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            //IEnumerable<Puntos> ListaPuntos = await GetAsync<IEnumerable<Puntos>>("Puntos/GetAll");
            //IEnumerable<TipoGeneral> ListaAreas = await GetAsync<IEnumerable<TipoGeneral>>("Area/ObtenerListaAreas");

            IEnumerable<TipoGeneral> Productos = await GetAsync<IEnumerable<TipoGeneral>>("obtenerTiposProducto/set");
            IEnumerable<TipoGeneral> Vendedores = await GetAsync<IEnumerable<TipoGeneral>>("obtenerTodosVendedores/set");


            ViewBag.Productos = Productos;
            ViewBag.Vendedores = Vendedores;

            return View();
        }

        [HttpGet]
        public async Task<string> GenerarArchivo(string fechaInicial, string fechaFinal, string SapTipoProducto, string SapAsesor)
        {
            
            string strRetorno = string.Empty;
           Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            try{
                IEnumerable<ReporteRedFechaAbierta> orden = await GetAsync<IEnumerable<ReporteRedFechaAbierta>>($"ReporteRedFechaAbierta/set/{fechaInicial}/{fechaFinal}/{SapTipoProducto}/{SapAsesor}");
                //await GetAsync<IEnumerable<ReporteRedFechaAbierta>[]>($"ReporteRedFechaAbierta/set/{objeto.fechaInicial}/{objeto.fechaFinal}/{objeto.SapTipoProducto}/{objeto.SapAsesor}");
               
                if (orden != null){
                    if (orden.Count() > 0)
                        //strRetorno = objReportes.GenerarReporteRedencionFechaAbierta(orden);
                        strRetorno = GenerarReporteRedencionFechaAbierta(orden);
                }
            }
            catch (Exception ex){
                strRetorno = string.Concat("Error generando reporte: ", ex.Message , " - Muchos datos para exportar");
            }
            return strRetorno;
        }

        [HttpGet]
        public string test(ReporteRedFechaAbierta objeto) {
            return "relol";
        }
        public string GenerarReporteRedencionFechaAbierta(IEnumerable<ReporteRedFechaAbierta> lista)
        {
            List<Parametro>[] elementosMostrar = new List<Parametro>[2];
            List<Parametro> elemento = new List<Parametro>();
            //elemento.Add(new Parametro { Nombre = "NumeroFactura", Valor = "Número factura institucional" });
            elemento.Add(new Parametro { Nombre = "FechRed", Valor = "Fecha de redención" });
            elemento.Add(new Parametro { Nombre = "NumSolicPed", Valor = "Número de solicitud de pedido" });
            elemento.Add(new Parametro { Nombre = "Producto", Valor = "Producto" });
            elemento.Add(new Parametro { Nombre = "ValorBruto", Valor = "Vr Redención antes de impuestos" });
            elemento.Add(new Parametro { Nombre = "Impuesto", Valor = "Impuesto" });
            elemento.Add(new Parametro { Nombre = "Total", Valor = "Total transacción" });
            elemento.Add(new Parametro { Nombre = "CantTicketVend", Valor = "Cantidad de boletas vendidas" });
            elemento.Add(new Parametro { Nombre = "CantTicketRed", Valor = "Cantidad de boletas redimidas" });
            elemento.Add(new Parametro { Nombre = "CantTicketNoRed", Valor = "Cantidad de boletas no redimidas" });
            elemento.Add(new Parametro { Nombre = "Asesor", Valor = "Asesor" });
            elemento.Add(new Parametro { Nombre = "Canal", Valor = "Canal" });
            //elementosMostrar[0] = elemento;
            Transversales.Util.Reportes objReportes = new Transversales.Util.Reportes();
            /*elemento = new List<Parametro>();
            elemento.Add(new Parametro { Nombre = "codsappedido", Valor = "Cod Sap Pedido" });
            elemento.Add(new Parametro { Nombre = "CodSapTipoImpuesto", Valor = "Cod Sap Tipo Impuesto" });
            elemento.Add(new Parametro { Nombre = "TotalVencido", Valor = "Total" }); 
            elemento.Add(new Parametro { Nombre = "codsapproducto", Valor = "Cod Sap Producto" });
            elemento.Add(new Parametro { Nombre = "nombre", Valor = "Nombre" });
            elemento.Add(new Parametro { Nombre = "Fecha", Valor = "Fecha" });
            elemento.Add(new Parametro { Nombre = "0", Valor = "0" });
            elementosMostrar[1] = elemento;
            
            string[] nombres = new string[2];
            nombres[0] = "Reporte";*/
            //nombres[1] = "BoletaControl";
            //return objReportes.GenerarReporteExcelMultiHojas(lista, elementosMostrar, nombres,"FechaAbierta"/*, decimales: new string[] { "G", "H" }*/);
            
            return objReportes.GenerarReporteExcel(lista, elemento, "FechaAbierta"/*, decimales: new string[] { "G", "H" }*/);
        }

        [HttpGet]
        public virtual FileContentResult Download(string Data)
        {
            FileContentResult objFileContentResult = null;
            try{
                objFileContentResult = File(Transversales.Util.Utilidades.ObtenerBytesArchivo(Data), System.Net.Mime.MediaTypeNames.Application.Octet, Data);
            }
            catch (Exception){
                return null;
            }
            return objFileContentResult;
        }
    }
}