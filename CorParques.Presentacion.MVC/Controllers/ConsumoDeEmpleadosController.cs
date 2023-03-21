using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Transversales.Util;
using static CorParques.Transversales.Util.Enumerador;
using ClosedXML.Excel;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ConsumoDeEmpleadosController : ControladorBase
    {
        #region properties 
        IEnumerable<ConsumoDeEmpleados>[] _listConsumo;
        public static string ruta;
        public static string rutaFinal;
        #endregion

        // GET: ConsumoDeEmpleados
        public async Task<ActionResult> Index()
        {
            var _listaEmpresas = await GetAsync<IEnumerable<EstructuraEmpleado>>("ConsumoDeEmpleados/BuscarEmpresas");
            string empresas = string.Empty;
            ViewBag.empresas = _listaEmpresas.Select(x => 
             new EstructuraEmpleado {Area = x.Area}
            );
            return View();
        }

        [HttpGet]
        public async Task<string> Buscar(string FInicial, string FFinal, string NDocumento, string Area)
        {
                ConsumoDeEmpleados c = new ConsumoDeEmpleados();
                c.fechaInicial = FInicial;
                c.fechaFinal = FFinal;
                _listConsumo = await GetAsync<IEnumerable<ConsumoDeEmpleados>[]>($"ConsumoDeEmpleados/Buscar/" + FInicial + "/" + FFinal + "/" + NDocumento + "/" + Area); 
                var test = rutaFinal;
                return imprimirExcel(_listConsumo, c);   
        }

        public string imprimirExcel(IEnumerable<ConsumoDeEmpleados>[] listaPintar, ConsumoDeEmpleados c)
        {
            List<Parametro>[] parametros = new List<Parametro>[2];
            List<Parametro> DatoMostrar = new List<Parametro>();
            List<Parametro> DatoMost = new List<Parametro>();
            DatoMost.Add(new Parametro { Nombre = "Fecha", Valor = "Fecha" });
            DatoMost.Add(new Parametro { Nombre = "Documento", Valor = "Documento" });
            DatoMost.Add(new Parametro { Nombre = "Nombres", Valor = "Nombres" });
            DatoMost.Add(new Parametro { Nombre = "Apellidos", Valor = "Apellidos" });
            DatoMost.Add(new Parametro { Nombre = "codigosap", Valor = "codigosap" });
            DatoMost.Add(new Parametro { Nombre = "Area", Valor = "Area" });
            DatoMost.Add(new Parametro { Nombre = "Valor", Valor = "Valor" });
            parametros[0] = DatoMost;

            DatoMostrar.Add(new Parametro { Nombre = "Documento", Valor = "Documento" });
            DatoMostrar.Add(new Parametro { Nombre = "Fecha", Valor = "Fecha" });
            DatoMostrar.Add(new Parametro { Nombre = "Empleado", Valor= "Empleado" });
            DatoMostrar.Add(new Parametro { Nombre = "Producto" , Valor = "Producto" });
            DatoMostrar.Add(new Parametro { Nombre = "Cantidad", Valor = "Cantidad" });
            DatoMostrar.Add(new Parametro { Nombre = "Factura", Valor = "Factura" });
            DatoMostrar.Add(new Parametro { Nombre = "Punto", Valor = "Punto" }); 
            DatoMostrar.Add(new Parametro { Nombre = "Cod_SAP", Valor = "Cod_SAP" });
            DatoMostrar.Add(new Parametro { Nombre = "Valor", Valor = "Valor" });
            parametros[1] = DatoMostrar;


            string[] nom = new string[2];
            nom[0] = "Consolidado";
            nom[1] = "Detalle";

            Reportes reportes = new Reportes();
            return GenerarReporteExcelMultiHojas(c,listaPintar, parametros, nom, "Consumo de empleados");
        }

      

        [HttpGet]
        public virtual FileContentResult Download(string Data)
        {
            FileContentResult objFileContentResult = null;
            try
            {
                objFileContentResult = File(Transversales.Util.Utilidades.ObtenerBytesArchivo(Data), System.Net.Mime.MediaTypeNames.Application.Octet, Data );
            }
            catch(Exception e)
            {
                return null;
            }
            return objFileContentResult;
        }

        #region ImprimirMultiplesHojas
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
        public string GenerarReporteExcelMultiHojas(ConsumoDeEmpleados c,IEnumerable<object>[] listas, IEnumerable<Parametro>[] mapeos, string[] nombreHojas, string NombreReporte = "", string[] decimales = null)
        {

            if (listas.Count() != mapeos.Count())
                throw new Exception("No hay datos en la cosnulta");

            var wb = new XLWorkbook();

            string strNombreArchivo = string.Empty;
            string strRutaArchivo = string.Empty;
            string strRutaReportes = string.Empty;

            strRutaReportes = Utilidades.RutaReportes();
            if (strRutaReportes.Trim().IndexOf("Error") >= 0)
                throw new ArgumentException(strRutaReportes);

            strNombreArchivo = string.Concat(NombreReporte, "_", Utilidades.FechaString(), ".xlsx");
         
            strRutaArchivo = string.Concat(strRutaReportes, strNombreArchivo);
            ruta = "/excel/" + strNombreArchivo;
            rutaFinal = strRutaArchivo;
            try
            {
                //inicio de bucle de las hojas
                for (int j = 0; j < listas.Count(); j++)
                {
                    if (listas[j].Count() == 0)
                        throw new Exception("No hay datos en la consulta");

                    var Encabezado = ObtenerEncabezado(listas[j].FirstOrDefault());
                    var Detalle = ObtenerDetalle(listas[j]);

                    List<int> idsMapeo = new List<int>();

                    var ws = wb.Worksheets.Add(nombreHojas[j]);
                    //int contadorFila = 1;
                    
                    int contadorFila = 1;
                    ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}1").Value = "Fecha Inicial";
                    contadorFila++;
                    ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}1").Value = c.fechaInicial;
                    contadorFila++;
                    ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}1").Value = "Fecha Final";
                    contadorFila++;
                    ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}1").Value = c.fechaFinal;
                    contadorFila = 1;
                    ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}3").Value = "Fecha descarga";
                    contadorFila++;
                    ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}3").Value = DateTime.Now.Date.ToString();
                    contadorFila++;
                    ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}3").Value = "Hora descarga";
                    contadorFila++;
                    ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}3").Value = DateTime.Now.ToString("hh:mm:ss");
                    contadorFila = 1;

                    for (int i = 1; i <= Encabezado.Count(); i++)
                        if (mapeos[j].Any(M => M.Nombre.Equals(Encabezado[i - 1])))
                        {
                            Parametro valorParametro = mapeos[j].Where(M => M.Nombre.Equals(Encabezado[i - 1])).FirstOrDefault();
                            ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}5").Value = valorParametro.Valor;
                            idsMapeo.Add(i - 1);
                            contadorFila++;
                        }
                    //Inicia en la columna 6
                    int contador = 6;
                    contadorFila = 1;
                    foreach (var item in Detalle)
                    {
                        for (int i = 1; i <= item.Count(); i++)
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
                        contadorFila = 1;
                        contador++;
                    }
                    if (decimales != null)
                        for (int i = 0; i < decimales.Length; i++)
                            if (decimales[i].Split(',')[0] == j.ToString())
                                ws.Column(decimales[i].Split(',')[1]).Style.NumberFormat.NumberFormatId = 2;
                    ws.Columns().AdjustToContents();


                }
                wb.SaveAs(strRutaArchivo);
                wb.Dispose();
            }
            catch (Exception ex)
            {
                strNombreArchivo = string.Concat("Error en GenerarReporteExcel: ", ex.Message, " ", strNombreArchivo);
            }
            return strNombreArchivo;
        }
        #endregion
    }
}
