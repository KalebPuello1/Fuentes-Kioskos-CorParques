using ClosedXML.Excel;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static CorParques.Transversales.Util.Enumerador;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class Reporte_SI_Consumo_SF_FechaAbiertaController : ControladorBase
    {
        // GET: Reporte_SI_Consumo_SF_FechaAbierta
        public ActionResult Index()
        {
            return View();
        }

        // GET: Reporte_SI_Consumo_SF_FechaAbierta/Details/5
        [HttpGet]
        public async Task<string> Details(string fechaI, string fechaF, string Npedido, string redencion)
        {
            var dato = await GetAsync<IEnumerable<Reporte_SI_Consumo_SF_FechaAbierta>>($"Reporte_SI_Consumo_SF_FechaAbierta/getDato/{fechaI}/{fechaF}/{Npedido}/{redencion}");
            var saldoInicial = 0;
            var saldoRedimido = 0;
            var saldoFinal = 0;
            var boletasInicial = 0;
            var boletasRedimidas = 0;
            var boletasFinales = 0;

            List<Reporte_SI_Consumo_SF_FechaAbierta> datoConsumido = new List<Reporte_SI_Consumo_SF_FechaAbierta>();
            
            /*foreach (var f in dato)
            {
                saldoInicial += int.Parse(f.Tot);
                if (f.Redencion == 1)
                {
                    saldoRedimido += int.Parse(f.Tot);
                    datoConsumido.Add(f);
                }

                if (f.Nombre.Contains("PASAPORTE"))
                { 
                
                }
            }*/
            saldoFinal = (saldoInicial - saldoRedimido);
            Reportes reporteee = new Reportes();
            //reporteee.GenerarReporteExcelMultiHojas();
            List<Parametro> parametros = new List<Parametro>();
            parametros.Add(new Parametro { Nombre = "DESCRIPCION", Valor = "Descripcion" });
            parametros.Add(new Parametro { Nombre = "CodSapPedido", Valor = "CodSapPedido" });
            parametros.Add(new Parametro { Nombre = "NombreVendedor", Valor = "Nombre Vendedor" });
            //parametros.Add(new Parametro { Nombre = "Valor", Valor = "Valor" });
            parametros.Add(new Parametro { Nombre = "Nombres", Valor = "Nombres" });
            //parametros.Add(new Parametro { Nombre = "Factura", Valor = "Factura" });
            parametros.Add(new Parametro { Nombre = "CodSapCliente", Valor = "Cod Sap Cliente" });
            parametros.Add(new Parametro { Nombre = "FechaInicial", Valor = "Fecha Inicial" });
            parametros.Add(new Parametro { Nombre = "FechaFinal", Valor = "Fecha Final" });
            parametros.Add(new Parametro { Nombre = "CodSapTipoProducto", Valor = "Cod Sap Tipo Producto" });
            parametros.Add(new Parametro { Nombre = "Nombre", Valor = "Nombre" });
            //parametros.Add(new Parametro { Nombre = "cantBoleteria", Valor = "cant Boleteria" });
            //parametros.Add(new Parametro { Nombre = "Valor", Valor = "Valor" });
            //parametros.Add(new Parametro { Nombre = "Valor", Valor = "Valor" });
            //parametros.Add(new Parametro { Nombre = "Factura", Valor = "Factura" });
            parametros.Add(new Parametro { Nombre = "cantSaldoinicial", Valor = "cant Saldo inicial" });
            parametros.Add(new Parametro { Nombre = "ValorSaldoInicial", Valor = "Valor Saldo Inicial" });
            parametros.Add(new Parametro { Nombre = "CantAdicion", Valor = "Cant Adicion" });
            parametros.Add(new Parametro { Nombre = "Adicion", Valor = "Valor Saldo Adicion" });
            parametros.Add(new Parametro { Nombre = "cantSaldoFinal", Valor = "cant SaldoFinal" });
            parametros.Add(new Parametro { Nombre = "ValorSaldoFinal", Valor = "Valor SaldoFinal" });
            //parametros.Add(new Parametro { Nombre = "Factura", Valor = "Factura" });
            parametros.Add(new Parametro { Nombre = "CantUsos", Valor = "Cant Usos" });
            parametros.Add(new Parametro { Nombre = "SaldoUsos", Valor = "Saldo Usos" });
            parametros.Add(new Parametro { Nombre = "Observacion", Valor = "Observacion" });
            /*    parametros.Add(new Parametro { Nombre = "CantSaldoSinUsar", Valor = "Cant Saldo Sin Usar" });
                parametros.Add(new Parametro { Nombre = "SaldoSinUsar", Valor = "Saldo Sin Usar" });*/

            string reporte = "";
            if (dato.Count() == 0 || dato == null)
            {
                reporte = "No hay datos para exportar en estas fecha";
            }
            else
            {
                //reporte = GenerarReporteExcel(saldoInicial, saldoRedimido, saldoFinal, dato, parametros, "Reporte_SaldoInicial_Comsumo_SaldoFinal");
                reporte = reporteee.GenerarReporteExcel(dato, parametros, "Reporte_SaldoInicial_Comsumo_SaldoFinal");
            }
            return reporte;
        }

        // GET: Reporte_SI_Consumo_SF_FechaAbierta/Create
        public virtual FileContentResult download(string dato)
        {
            FileContentResult resultConstent = null;
            try
            {
                resultConstent = File(Transversales.Util.Utilidades.ObtenerBytesArchivo(dato), System.Net.Mime.MediaTypeNames.Application.Octet, dato);
            }
            catch (Exception e)
            {

                throw;
            }
            return resultConstent;
        }


        private string[] ObtenerEncabezado(object elemento)
        {
            List<string> encabezado = new List<string>();
            foreach (var item in elemento.GetType().GetProperties())
            {
                encabezado.Add(item.Name);
            }
            return encabezado.ToArray();
        }

        private IEnumerable<string[]> ObtenerDetalle(IEnumerable<object> lista)
        {
            List<string[]> detalle = new List<string[]>();

            foreach (var item in lista)
            {
                List<string> registro = new List<string>();
                foreach (var reg in item.GetType().GetProperties())
                {

                    var value = reg.GetValue(item, null) == null ? string.Empty : reg.GetValue(item, null).ToString();
                    registro.Add(value);
                }
                detalle.Add(registro.ToArray());
            }
            return detalle;
        }

        public string GenerarReporteExcel(int saldoInicial,int saldoUtilizado,int saldoFinal, IEnumerable<object> lista, IEnumerable<Parametro> mapeo, string NombreReporte = "", string[] decimales = null, string[] sumatoria = null)
        {
            if (lista.Count() == 0)
            {
                throw new Exception("No hay datos en la consulta");
            }

            var Encabezado = ObtenerEncabezado(lista.FirstOrDefault());
            var Detalle = ObtenerDetalle(lista);

            string strNombreArchivo = string.Empty;
            string strRutaArchivo = string.Empty;
            string strRutaReportes = string.Empty;
            List<int> idsMapeo = new List<int>();

            try
            {
                strRutaReportes = Utilidades.RutaReportes();
                if (strRutaReportes.Trim().IndexOf("Error") >= 0)
                {
                    throw new ArgumentException(strRutaReportes);
                }

                strNombreArchivo = string.Concat(NombreReporte, "_", Utilidades.FechaString(), ".xlsx");//tellez 2017/05/24 - ahora exporrta en archivo excel

                strRutaArchivo = string.Concat(strRutaReportes, strNombreArchivo);

                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Reporte");



                int contadorFila = 1;
                int contador = 2;
                ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Value = "Saldo Inicial";
                contadorFila = 2;
                contador = 2;
                ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Value = saldoInicial;
                contadorFila = 3;
                contador = 2;
                ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Value = "Saldo Consumido";
                contadorFila = 4;
                contador = 2;
                ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Value = saldoUtilizado;
                contadorFila = 5;
                contador = 2;
                ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Value = "Saldo Final";
                contadorFila = 6;
                contador = 2;
                ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Value = saldoFinal;

                contadorFila = 1;
                contador = 4;
                ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Value = "Productos Consumidos";

                contadorFila = 1;
                contador = 7;

                if (NombreReporte.Equals("ReporteVentasDiarias"))
                {
                    ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}1").Value = "Comprobante Informe Diario";
                    contador = 3;
                }


                for (int i = 1; i <= Encabezado.Count(); i++)
                {
                    if (mapeo.Any(M => M.Nombre.Equals(Encabezado[i - 1])))
                    {
                        Parametro valorParametro = mapeo.Where(M => M.Nombre.Equals(Encabezado[i - 1])).FirstOrDefault();
                        if (NombreReporte.Equals("ReporteVentasDiarias"))
                        {
                            ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}2").Value = valorParametro.Valor;
                        }
                        else
                        {
                            ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}6").Value = valorParametro.Valor;
                        }

                        idsMapeo.Add(i - 1);
                        contadorFila++;
                    }
                }


                contadorFila = 1;
                foreach (var item in Detalle)
                {
                    for (int i = 1; i <= item.Count(); i++)
                    {
                        if (idsMapeo.Any(M => M == i - 1))
                        {
                            double valor;

                            if (double.TryParse(item[i - 1], out valor))
                            {
                                ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Value = valor;
                                ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Style.NumberFormat.NumberFormatId = 1;// 0_General - 2_0.00 - 3_#,##0
                            }
                            else
                                ws.Cell(((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString() + contador).Value = item[i - 1];
                            contadorFila++;
                        }
                    }
                    contadorFila = 1;
                    contador++;
                }

                if (decimales != null)
                    for (int i = 0; i < decimales.Length; i++)
                    {
                        if (decimales[i].Split(':').Length > 1)
                        {
                            ws.Column(decimales[i].Split(':')[0]).Style.NumberFormat.Format = string.Concat("0.", new String('0', int.Parse(decimales[i].Split(':')[1].ToString())));
                        }
                        else
                        {
                            ws.Column(decimales[i]).Style.NumberFormat.NumberFormatId = 2;
                        }
                    }

                if (sumatoria != null)
                {
                    double suma = 0;
                    int k = 0;
                    for (int i = 0; i < sumatoria.Length; i++)
                    {
                        for (k = 0; k < lista.Count(); k++)
                            suma += double.Parse(ws.Cell(sumatoria[i] + (k + 2)).Value.ToString());
                        ws.Cell(k + 2, sumatoria[i]).Value = suma;
                        ws.Cell(k + 2, sumatoria[i]).Style.NumberFormat.NumberFormatId = 1;
                        suma = 0;
                    }
                }


                ws.Columns().AdjustToContents();

                wb.SaveAs(strRutaArchivo);
                wb.Dispose();
            }
            catch (Exception ex)
            {
                strNombreArchivo = string.Concat("Error en GenerarReporte: ", ex.Message, " ", strNombreArchivo);
            }

            return strNombreArchivo;
        }


    }
}
