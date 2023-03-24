using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ClosedXML.Excel;
using CorParques.Negocio.Entidades;
using static CorParques.Transversales.Util.Enumerador;


namespace CorParques.Transversales.Util
{
    public class Reportes
    {

        #region Metodos

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

        /// <summary>
        /// Genera los reportes de una hoja
        /// </summary>
        /// <param name="lista">lista del elemento donde se encuentran los datos,los campos que tienen numeros, seran en fomato numerico</param>
        /// <param name="mapeo">nombre de los encabezados, de como se mapean de base de datos al reporte</param>
        /// <param name="NombreReporte">nombre de como se llamara el reporte</param>
        /// <param name="decimales">las columnas que deben tener decimales, EJ: new string[]{"columna","A","D"};</param>
        /// <returns></returns>
        public string GenerarReporteExcel(IEnumerable<object> lista, IEnumerable<Parametro> mapeo, string NombreReporte = "", string[] decimales = null, string[] sumatoria = null)
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
                            ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}1").Value = valorParametro.Valor;
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

        public string GenerarReporteRecoleccion(IEnumerable<ReporteRecoleccion>[] recoleccion)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Genera los reportes de una dos o mas hojas, los campos que tienen numeros, seran en fomato numerico
        /// </summary>
        /// <param name="listas">arrego de las listas de los reportes a mostrar en cada hoja</param>
        /// <param name="mapeos">arreglo de los encabezados en las hojas del reporte</param>
        /// <param name="NombreReporte">nombre de como se llamara el reporte</param>
        /// <param name="decimales">las columnas que deben tener decimales, las hojas empiezan desde 0 EJ: new string[]{"hoja,columna","0,A","1,D"};</param>
        /// <returns></returns>
        public string GenerarReporteExcelMultiHojas(IEnumerable<object>[] listas, IEnumerable<Parametro>[] mapeos, string[] nombreHojas, string NombreReporte = "", string[] decimales = null)
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

            strNombreArchivo = string.Concat(NombreReporte, "_", Utilidades.FechaString(), ".xlsx");//tellez 2017/05/24 - ahora exporrta en archivo excel
            strRutaArchivo = string.Concat(strRutaReportes, strNombreArchivo);

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

                    int contadorFila = 1;
                    for (int i = 1; i <= Encabezado.Count(); i++)
                        if (mapeos[j].Any(M => M.Nombre.Equals(Encabezado[i - 1])))
                        {
                            Parametro valorParametro = mapeos[j].Where(M => M.Nombre.Equals(Encabezado[i - 1])).FirstOrDefault();
                            ws.Cell($"{((Alfabeto[])(Enum.GetValues(typeof(Alfabeto))))[contadorFila].ToString()}1").Value = valorParametro.Valor;
                            idsMapeo.Add(i - 1);
                            contadorFila++;
                        }

                    int contador = 2;
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


        public string GenerarReportePrimerosAuxilios(IEnumerable<Procedimiento> lista)
        {
            List<Procedimiento> listaProcedimientos = lista.ToList();

            foreach (Procedimiento item in listaProcedimientos)
            {
                item.FechaCortaIncidente = item.FechaIncidente.ToString("dd/MM/yyyy");
                item.HoraCortaIncidente = item.FechaIncidente.ToString("HH:mm");
                item.Causa = item.Causa.Replace("\r\n", " ").Trim();
                item.Sintomas = item.Sintomas.Replace("\r\n", " ").Trim();
                item.Alergias = item.Alergias.Replace("\r\n", " ").Trim();
                item.Tratamiento = item.Tratamiento.Replace("\r\n", " ").Trim();
                item.Traslado = item.Traslado.Replace("\r\n", " ").Trim();
                item.Recomendaciones = item.Recomendaciones.Replace("\r\n", " ").Trim();
            }

            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "IdProcedimiento", Valor = "Consecutivo" });
            elementosMostrar.Add(new Parametro { Nombre = "Categoria", Valor = "Categoria" });
            elementosMostrar.Add(new Parametro { Nombre = "TipoPaciente", Valor = "TipoPaciente" });
            elementosMostrar.Add(new Parametro { Nombre = "Triage", Valor = "Triage" });
            elementosMostrar.Add(new Parametro { Nombre = "ZonaArea", Valor = "Zona / Área" });
            elementosMostrar.Add(new Parametro { Nombre = "UbicacionMedica", Valor = "Ubicación" });
            elementosMostrar.Add(new Parametro { Nombre = "FechaCortaIncidente", Valor = "FechaIncidente" });
            elementosMostrar.Add(new Parametro { Nombre = "HoraCortaIncidente", Valor = "HoraIncidente" });
            elementosMostrar.Add(new Parametro { Nombre = "NombrePaciente", Valor = "NombrePaciente" });
            elementosMostrar.Add(new Parametro { Nombre = "ApellidoPaciente", Valor = "ApellidoPaciente" });
            elementosMostrar.Add(new Parametro { Nombre = "TipoDocumento", Valor = "TipoDocumento" });
            elementosMostrar.Add(new Parametro { Nombre = "DocumentoPaciente", Valor = "Documento" });
            elementosMostrar.Add(new Parametro { Nombre = "EdadPaciente", Valor = "EdadPaciente" });
            elementosMostrar.Add(new Parametro { Nombre = "Meses", Valor = "MesesPaciente" });
            elementosMostrar.Add(new Parametro { Nombre = "DireccionPaciente", Valor = "Dirección" });
            elementosMostrar.Add(new Parametro { Nombre = "TelefonoPaciente", Valor = "Celular/Fijo" });
            elementosMostrar.Add(new Parametro { Nombre = "CorreoPaciente", Valor = "Email" });
            elementosMostrar.Add(new Parametro { Nombre = "TipoDocumentoAcompanante", Valor = "TipoDocumentoAcompañante" });
            elementosMostrar.Add(new Parametro { Nombre = "DocumentoAcudiente", Valor = "NúmeroDocumentoAcompañante" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreAcudiente", Valor = "NombreAcompañante" });
            elementosMostrar.Add(new Parametro { Nombre = "TelefonoAcudiente", Valor = "Celular/FijoAcompañante" });
            elementosMostrar.Add(new Parametro { Nombre = "Causa", Valor = "MotivoConsulta" });
            elementosMostrar.Add(new Parametro { Nombre = "Sintomas", Valor = "Sintomas" });
            elementosMostrar.Add(new Parametro { Nombre = "Alergias", Valor = "Alergias" });
            elementosMostrar.Add(new Parametro { Nombre = "Tratamiento", Valor = "ProcedimientoRealizado" });
            elementosMostrar.Add(new Parametro { Nombre = "Traslado", Valor = "Traslado" });
            elementosMostrar.Add(new Parametro { Nombre = "Recomendaciones", Valor = "Observaciones" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreMedico", Valor = "Funcionario" });
            elementosMostrar.Add(new Parametro { Nombre = "Eps", Valor = "EPS" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreEps", Valor = "Nombre EPS" });

            return GenerarReporteExcel(listaProcedimientos.AsEnumerable(), elementosMostrar, "PrimerosAuxilios");
        }

        public string GenerarReporteVentasDirectas(IEnumerable<ReporteVentas>[] listas)
        {
            List<Parametro>[] listaMapeo = new List<Parametro>[2];
            List<Parametro> elementosMostrar = new List<Parametro>();

            //----------------------- HOJA PRODUCTOS | CAMPOS CON SU VALOR -----------------------
            elementosMostrar.Add(new Parametro { Nombre = "Fecha", Valor = "Fecha" });
            elementosMostrar.Add(new Parametro { Nombre = "Consecutivo", Valor = "Número Factura" });
            elementosMostrar.Add(new Parametro { Nombre = "Taquilla", Valor = "Taquilla" });
            elementosMostrar.Add(new Parametro { Nombre = "Producto", Valor = "Producto" });
            elementosMostrar.Add(new Parametro { Nombre = "Cantidad", Valor = "Cantidad" });
            elementosMostrar.Add(new Parametro { Nombre = "ValorSinImpuesto", Valor = "Vr Venta antes de impuestos" });
            elementosMostrar.Add(new Parametro { Nombre = "Impuesto", Valor = "Impuesto" });
            elementosMostrar.Add(new Parametro { Nombre = "TotalRecibido", Valor = "Total Ventas" });
            elementosMostrar.Add(new Parametro { Nombre = "Taquillero", Valor = "Taquillero" });
            elementosMostrar.Add(new Parametro { Nombre = "Anulaciones", Valor = "Anulaciones" });
            elementosMostrar.Add(new Parametro { Nombre = "NotaCredito", Valor = "Valor nota crédito" });
            elementosMostrar.Add(new Parametro { Nombre = "NumNotaCredito", Valor = "Número nota crédito" });
            elementosMostrar.Add(new Parametro { Nombre = "Convenio", Valor = "Convenio" });
            listaMapeo[0] = elementosMostrar;

            elementosMostrar = new List<Parametro>();

            //----------------------- HOJA MEDIOS PAGO | CAMPOS CON SU VALOR -----------------------
            elementosMostrar.Add(new Parametro { Nombre = "Fecha", Valor = "Fecha" });
            elementosMostrar.Add(new Parametro { Nombre = "Consecutivo", Valor = "Número Factura" });
            elementosMostrar.Add(new Parametro { Nombre = "Taquilla", Valor = "Taquilla" });
            elementosMostrar.Add(new Parametro { Nombre = "Taquillero", Valor = "Taquillero" });
            elementosMostrar.Add(new Parametro { Nombre = "MedioPago", Valor = "Forma de pago" });
            elementosMostrar.Add(new Parametro { Nombre = "Franquicia", Valor = "Franquicia" });
            elementosMostrar.Add(new Parametro { Nombre = "NumAprobacion", Valor = "Número de aprobación" });
            elementosMostrar.Add(new Parametro { Nombre = "TotalRecibido", Valor = "Valor pagado por medio de pago" });
            //elementosMostrar.Add(new Parametro { Nombre = "Anulaciones", Valor = "Anulaciones" });
            //elementosMostrar.Add(new Parametro { Nombre = "NumNotaCredito", Valor = "Número nota crédito" });
            //elementosMostrar.Add(new Parametro { Nombre = "Propina", Valor = "Propina" });
            elementosMostrar.Add(new Parametro { Nombre = "TipoCliente", Valor = "Tipo de cliente" });
            elementosMostrar.Add(new Parametro { Nombre = "idCliente", Valor = "Id Cliente" });
            elementosMostrar.Add(new Parametro { Nombre = "Nombre", Valor = "Cliente" });

            listaMapeo[1] = elementosMostrar;

            //----------------------- Nombre de hojas de EXCEL -----------------------
            string[] nombreHojas = new string[2];
            nombreHojas[0] = "Productos";
            nombreHojas[1] = "MediosPago";
            //string nombreLibro = "ReporteVentasDirectas";
            string[] camposDecimales = new string[] { "0,F", "0,G" };

            //----------------------- GENERA EL ARCHIVO EXCEL -----------------------
            return GenerarReporteExcelMultiHojas(listas, listaMapeo, nombreHojas, "ReporteVentasDirectas", camposDecimales);
            //return GenerarReporteExcelVariasHojas(listas, listaMapeo, nombreHojas, nombreLibro, false);
        }

        public string GenerarReporteRecoleccion(IEnumerable<ReporteRecoleccion> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "TipoRecoleccion", Valor = "Tipo Recoleccion" });
            elementosMostrar.Add(new Parametro { Nombre = "IdRecoleccion", Valor = "Numero Recoleccion" });
            elementosMostrar.Add(new Parametro { Nombre = "Punto", Valor = "Punto" });
            elementosMostrar.Add(new Parametro { Nombre = "Taquillero", Valor = "Taquillero" });
            elementosMostrar.Add(new Parametro { Nombre = "Total", Valor = "Valor" });
            elementosMostrar.Add(new Parametro { Nombre = "Supervisor", Valor = "Recolector/Supervisor" });
            elementosMostrar.Add(new Parametro { Nombre = "Cierre", Valor = "Cierre" });
            elementosMostrar.Add(new Parametro { Nombre = "NumReferencia", Valor = "Numero Referencia" });

            return GenerarReporteExcel(lista, elementosMostrar, "ReporteRecoleccion");
        }
        public string GenerarReporteFallasAtracciones(IEnumerable<ReporteFallaAtraccion> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "atraccion", Valor = "Atracción" });
            elementosMostrar.Add(new Parametro { Nombre = "HoraFalla", Valor = "Hora de Falla" });
            elementosMostrar.Add(new Parametro { Nombre = "HoraSolucion", Valor = "Hora de solución" });
            elementosMostrar.Add(new Parametro { Nombre = "TiempoRespuesta", Valor = "Tiempo de Respuesta" });
            elementosMostrar.Add(new Parametro { Nombre = "DescripcionFallaAuxiliar", Valor = "Descripción de la Falla por Auxiliar" });
            elementosMostrar.Add(new Parametro { Nombre = "ObservacionMantenimiento", Valor = "Observación de Mantenimiento" });
            elementosMostrar.Add(new Parametro { Nombre = "orden", Valor = "Orden de falla" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreTecnico", Valor = "Nombre de Técnico" });
            elementosMostrar.Add(new Parametro { Nombre = "fechaFalla", Valor = "Fecha" });
            elementosMostrar.Add(new Parametro { Nombre = "Area", Valor = "Area" });

            return GenerarReporteExcel(lista, elementosMostrar, "FallasAtraccion");
        }

        public string GenerarReporteReservaEspacio(IEnumerable<ReporteReservaEspacio> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();

            elementosMostrar.Add(new Parametro { Nombre = "Fecha", Valor = "Fecha" });
            elementosMostrar.Add(new Parametro { Nombre = "TipoReserva", Valor = "Tipo Reserva" });
            elementosMostrar.Add(new Parametro { Nombre = "HoraInicio", Valor = "Hora inicial" });
            elementosMostrar.Add(new Parametro { Nombre = "HoraFin", Valor = "Hora final" });
            elementosMostrar.Add(new Parametro { Nombre = "NumeroReserva", Valor = "Número de la reserva" });
            elementosMostrar.Add(new Parametro { Nombre = "NumeroPedido", Valor = "Número de pedido" });
            elementosMostrar.Add(new Parametro { Nombre = "Asesor", Valor = "Asesor" });
            elementosMostrar.Add(new Parametro { Nombre = "Cliente", Valor = "Cliente" });
            elementosMostrar.Add(new Parametro { Nombre = "Espacio", Valor = "Espacio" });
            elementosMostrar.Add(new Parametro { Nombre = "TipoEspacio", Valor = "Tipo Espacio" });
            elementosMostrar.Add(new Parametro { Nombre = "productos", Valor = "Productos" });
            elementosMostrar.Add(new Parametro { Nombre = "Observaciones", Valor = "Observaciones" });
            elementosMostrar.Add(new Parametro { Nombre = "Cantidad", Valor = "Cantidad" });
            //elementosMostrar.Add(new Parametro { Nombre = "IdEspacio", Valor = "ggggggggggg" });
            //elementosMostrar.Add(new Parametro { Nombre = "IdTipoEspacio", Valor = "ggggggggggg" });

            return GenerarReporteExcel(lista, elementosMostrar, "ReservaEspacio");
        }

        public string GenerarReporteDestreza(IEnumerable<ReporteDestrezas> listas)
        {

            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "Punto", Valor = "Punto" });
            elementosMostrar.Add(new Parametro { Nombre = "Asesor", Valor = "Asesor" });
            elementosMostrar.Add(new Parametro { Nombre = "Cliente", Valor = "Cliente" });
            elementosMostrar.Add(new Parametro { Nombre = "Convenio", Valor = "Convenio" });
            elementosMostrar.Add(new Parametro { Nombre = "ValorTotalIngreso", Valor = "Valor total ingresos" });
            elementosMostrar.Add(new Parametro { Nombre = "ValorVentaUnoAUno", Valor = "Valor total venta uno a uno" });
            elementosMostrar.Add(new Parametro { Nombre = "ValorVentaInstitucional", Valor = "Valor total venta institucional" });
            elementosMostrar.Add(new Parametro { Nombre = "ValorUso", Valor = "Valor uso" });
            elementosMostrar.Add(new Parametro { Nombre = "TipoBoletaDescargada", Valor = "Tipo Boleta" });
            elementosMostrar.Add(new Parametro { Nombre = "CantidadUsos", Valor = "Cantidad usos" });
            elementosMostrar.Add(new Parametro { Nombre = "CantidadPremios", Valor = "Cantidad premios" });
            elementosMostrar.Add(new Parametro { Nombre = "TotalCantPremios", Valor = "Total costo premios" });
            elementosMostrar.Add(new Parametro { Nombre = "Porcentaje", Valor = "Porcentaje por premio" });

            return GenerarReporteExcel(listas, elementosMostrar, "Destrezas", new string[] { "M" });
        }

        public string GenerarReporteControlCaja(IEnumerable<ReporteControlCaja> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "Fecha", Valor = "Fecha" });
            elementosMostrar.Add(new Parametro { Nombre = "Documento", Valor = "Cedula" });
            elementosMostrar.Add(new Parametro { Nombre = "Nombres", Valor = "Nombre" });
            elementosMostrar.Add(new Parametro { Nombre = "Perfiles", Valor = "Perfil" });
            elementosMostrar.Add(new Parametro { Nombre = "Punto", Valor = "Punto" });
            elementosMostrar.Add(new Parametro { Nombre = "Base", Valor = "Base" });
            elementosMostrar.Add(new Parametro { Nombre = "AlistamientoBase", Valor = "Alistamiento base" });
            elementosMostrar.Add(new Parametro { Nombre = "SueprvisorBase", Valor = "Supervisor base" });
            elementosMostrar.Add(new Parametro { Nombre = "TaquilleroBase", Valor = "Taquillero base" });
            //elementosMostrar.Add(new Parametro { Nombre = "producto", Valor = "Tipo boleta" });
            //elementosMostrar.Add(new Parametro { Nombre = "BoleteriaApertura", Valor = "Cantidad boleteria apertura" });
            //elementosMostrar.Add(new Parametro { Nombre = "boleteriaReabastecimiento", Valor = "Reabastecimiento" });
            //elementosMostrar.Add(new Parametro { Nombre = "boleteriaEntregada", Valor = "Total boletería asignada" });
            //elementosMostrar.Add(new Parametro { Nombre = "cantidadVendida", Valor = "Boleteria vendida" });
            //elementosMostrar.Add(new Parametro { Nombre = "CantidaadSupervisor", Valor = "Boleteria entregada" });
            elementosMostrar.Add(new Parametro { Nombre = "notacredito", Valor = "Nota credito" });
            elementosMostrar.Add(new Parametro { Nombre = "anulaciones", Valor = "Anulaciones" });
            elementosMostrar.Add(new Parametro { Nombre = "Faltante", Valor = "Faltantes" });
            elementosMostrar.Add(new Parametro { Nombre = "sobrante", Valor = "Sobrante" });
            elementosMostrar.Add(new Parametro { Nombre = "recoleccion", Valor = "Total recoleccion" });
            elementosMostrar.Add(new Parametro { Nombre = "totalventas", Valor = "Total ventas" });
            elementosMostrar.Add(new Parametro { Nombre = "efectivo", Valor = "Ventas en efectivo" });
            elementosMostrar.Add(new Parametro { Nombre = "tarjetas", Valor = "Ventas en tarjetas" });
            elementosMostrar.Add(new Parametro { Nombre = "BonoRegalo", Valor = "Ventas en bono regalo" });
            elementosMostrar.Add(new Parametro { Nombre = "BonoSodexo", Valor = "Ventas en bono sodexo" });
            elementosMostrar.Add(new Parametro { Nombre = "DescuentoEmpleado", Valor = "Ventas empleado" });
            elementosMostrar.Add(new Parametro { Nombre = "RecoleccionBase", Valor = "Recoleccion base" });
            elementosMostrar.Add(new Parametro { Nombre = "RecoleccionEfectivo", Valor = "Recoleccion efectivo" });
            elementosMostrar.Add(new Parametro { Nombre = "recolecciontarjeta", Valor = "Recoleccion tarjeta" });
            elementosMostrar.Add(new Parametro { Nombre = "recolecciondocumentos", Valor = "Recoleccion documentos" });
            elementosMostrar.Add(new Parametro { Nombre = "recoleccionnovedadessobrantes", Valor = "Recoleccion novedades sobrantes" });
            elementosMostrar.Add(new Parametro { Nombre = "recoleccionefectivoentregado", Valor = "Entrega final en efectivo" });
            elementosMostrar.Add(new Parametro { Nombre = "recolecciontarjetaentregado", Valor = "Entrega final en tarjeta" });
            elementosMostrar.Add(new Parametro { Nombre = "recolecciondocumentosentregado", Valor = "Entrega final en documentos" });
            elementosMostrar.Add(new Parametro { Nombre = "recoleccionnovedadessobrantesentregado", Valor = "Entrega final novedades sobrantes" });
            //elementosMostrar.Add(new Parametro { Nombre = "CantidadTaquillero", Valor = "Cantidad boletería a recibir" });
            //elementosMostrar.Add(new Parametro { Nombre = "CantidadNido", Valor = "Cantidad boletería recibida" });


            return GenerarReporteExcel(lista, elementosMostrar, "ControlCaja");
        }

        public string GenerarReportePasajerosAtracciones(IEnumerable<ReportePasajeros> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "Fecha", Valor = "Fecha" });
            elementosMostrar.Add(new Parametro { Nombre = "Atraccion", Valor = "Punto" });
            elementosMostrar.Add(new Parametro { Nombre = "TipoProducto", Valor = "Tipo producto" });
            elementosMostrar.Add(new Parametro { Nombre = "Producto", Valor = "Producto" });
            elementosMostrar.Add(new Parametro { Nombre = "CantidadUsos", Valor = "Cantidad Usos" });
            elementosMostrar.Add(new Parametro { Nombre = "ValorVenta", Valor = "Valor de Venta" });
            elementosMostrar.Add(new Parametro { Nombre = "Valor", Valor = "Total Ingreso" });
            elementosMostrar.Add(new Parametro { Nombre = "Convenio", Valor = "Convenio" });
            elementosMostrar.Add(new Parametro { Nombre = "Torniquete", Valor = "Total Torniquete" });
            elementosMostrar.Add(new Parametro { Nombre = "Porcentaje", Valor = "Porcentaje de uso" });
            return GenerarReporteExcel(lista, elementosMostrar, "PasajerosAtracciones", decimales: new string[] { "J" });
        }

        public string GenerarReporteRedencionFechaAbierta(IEnumerable<ReporteRedFechaAbierta> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "NumeroFactura", Valor = "Número factura institucional" });
            elementosMostrar.Add(new Parametro { Nombre = "FechRed", Valor = "Fecha de redención" });
            elementosMostrar.Add(new Parametro { Nombre = "NumSolicPed", Valor = "Número de solicitud de pedido" });
            elementosMostrar.Add(new Parametro { Nombre = "Producto", Valor = "Producto" });
            elementosMostrar.Add(new Parametro { Nombre = "ValorBruto", Valor = "Vr Redención antes de impuestos" });
            elementosMostrar.Add(new Parametro { Nombre = "Impuesto", Valor = "Impuesto" });
            elementosMostrar.Add(new Parametro { Nombre = "Total", Valor = "Total transacción" });
            elementosMostrar.Add(new Parametro { Nombre = "CantTicketVend", Valor = "Cantidad de boletas vendidas" });
            elementosMostrar.Add(new Parametro { Nombre = "CantTicketRed", Valor = "Cantidad de boletas redimidas" });
            elementosMostrar.Add(new Parametro { Nombre = "CantTicketNoRed", Valor = "Cantidad de boletas no redimidas" });
            elementosMostrar.Add(new Parametro { Nombre = "Asesor", Valor = "Asesor" });
            elementosMostrar.Add(new Parametro { Nombre = "Canal", Valor = "Canal" });

            return GenerarReporteExcel(lista, elementosMostrar, "FechaAbierta", decimales: new string[] { "G", "H" });
        }
        public string GenerarReporteArovechamientoFA(IEnumerable<ReporteAprovechamientoFA> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "numerofactura", Valor = "Número Factura" });
            elementosMostrar.Add(new Parametro { Nombre = "NIT", Valor = "NIT" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreCliente", Valor = "Nombre Cliente" });
            elementosMostrar.Add(new Parametro { Nombre = "FehcaNegociacion", Valor = "Fecha Negociación" });
            elementosMostrar.Add(new Parametro { Nombre = "FechaVencimiento", Valor = "Fecha Vencimiento" });
            elementosMostrar.Add(new Parametro { Nombre = "NumeroSolicitud", Valor = "Numero Solicitud" });
            elementosMostrar.Add(new Parametro { Nombre = "cantidadVendidas", Valor = "Boletas Vendidas" });
            elementosMostrar.Add(new Parametro { Nombre = "cantidadRedimida", Valor = "Boletas Redimidas" });
            elementosMostrar.Add(new Parametro { Nombre = "CantidadNoRedimidas", Valor = "Boletas No Redimidas" });
            elementosMostrar.Add(new Parametro { Nombre = "ValorAprovechamiento", Valor = "Valor Aprovechamiento" });
            elementosMostrar.Add(new Parametro { Nombre = "producto", Valor = "producto" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreAsesor", Valor = "Nombre Asesor" });

            return GenerarReporteExcel(lista, elementosMostrar, "AprovechamientoFA", decimales: new string[] { });
        }

        public string GenerarReporteUsuarioApp(IEnumerable<ReporteUsuarioApp>[] listaUsuarioApp)
        {
            List<Parametro>[] parametros = new List<Parametro>[3];
            List<Parametro> DatoMostrar = new List<Parametro>();

            DatoMostrar.Add(new Parametro { Nombre = "MailCliente", Valor = "Correo electronico" });
            DatoMostrar.Add(new Parametro { Nombre = "CodigoFactura", Valor = "Codigo Factura" });
            DatoMostrar.Add(new Parametro { Nombre = "IdDetalleFactura", Valor = "Detalle Cliente" });
            DatoMostrar.Add(new Parametro { Nombre = "Nombre", Valor = "Nombre" });
            DatoMostrar.Add(new Parametro { Nombre = "Redimido", Valor = "Redimido" });
            DatoMostrar.Add(new Parametro { Nombre = "CodeQR", Valor = "Codigo QR" });
            DatoMostrar.Add(new Parametro { Nombre = "Precio", Valor = "Precio" });
            DatoMostrar.Add(new Parametro { Nombre = "Fecha", Valor = "Fecha" });
            DatoMostrar.Add(new Parametro { Nombre = "FechaVencimiento", Valor = "FechaVencimiento" });
            DatoMostrar.Add(new Parametro { Nombre = "FechaUso", Valor = "Fecha uso" });
            parametros[0] = DatoMostrar;

            DatoMostrar = new List<Parametro>();
            DatoMostrar.Add(new Parametro { Nombre = "MailCliente", Valor = "Correo electronico" });
            DatoMostrar.Add(new Parametro { Nombre = "CodigoFactura", Valor = "Codigo factura" });
            DatoMostrar.Add(new Parametro { Nombre = "Nombre", Valor = "Nombre " });
            DatoMostrar.Add(new Parametro { Nombre = "IdRegistroUsoBoleteria", Valor = "Registro uso boleteria" });
            DatoMostrar.Add(new Parametro { Nombre = "Consecutivo", Valor = "Consecutivo" });
            DatoMostrar.Add(new Parametro { Nombre = "FechaHoraUso", Valor = "Fecha hora uso" });
            parametros[1] = DatoMostrar;

            DatoMostrar = new List<Parametro>();
            DatoMostrar.Add(new Parametro { Nombre = "Id", Valor = "Id" });
            DatoMostrar.Add(new Parametro { Nombre = "DateTransaction", Valor = "Fecha Transaccion" });
            DatoMostrar.Add(new Parametro { Nombre = "PriceInitial", Valor = "Precio Inicial" });
            DatoMostrar.Add(new Parametro { Nombre = "User", Valor = "Usuario" });
            DatoMostrar.Add(new Parametro { Nombre = "Data", Valor = "Datos" });
            DatoMostrar.Add(new Parametro { Nombre = "DateResponse", Valor = "Fecha Respuesta" });
            DatoMostrar.Add(new Parametro { Nombre = "IdResponse", Valor = "Id Respuesta" });
            DatoMostrar.Add(new Parametro { Nombre = "Bank", Valor = "Banco" });
            DatoMostrar.Add(new Parametro { Nombre = "Price", Valor = "Precio" });
            parametros[2] = DatoMostrar;

            string[] ReporteUsuarioApp = new string[3];
            ReporteUsuarioApp[0] = "Consolidado";
            ReporteUsuarioApp[1] = "Detalle";
            ReporteUsuarioApp[2] = "Transacciones";



            return GenerarReporteExcelMultiHojas(listaUsuarioApp, parametros, ReporteUsuarioApp, decimales: new string[] { });
        }

        public string GenerarReporteDonaciones(IEnumerable<ReporteDonaciones> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "Fecha", Valor = "Fecha" });
            elementosMostrar.Add(new Parametro { Nombre = "NombrePunto", Valor = "Punto" });
            elementosMostrar.Add(new Parametro { Nombre = "Consecutivo", Valor = "Consecutivo" });
            elementosMostrar.Add(new Parametro { Nombre = "Donante", Valor = "Cliente" });
            elementosMostrar.Add(new Parametro { Nombre = "Producto", Valor = "Producto" });
            elementosMostrar.Add(new Parametro { Nombre = "NotaCredito", Valor = "Total donacion" });
            elementosMostrar.Add(new Parametro { Nombre = "Anulaciones", Valor = "Anulacion" });
            elementosMostrar.Add(new Parametro { Nombre = "MedioPago", Valor = "Medio de pago" });

            return GenerarReporteExcel(lista, elementosMostrar, "Donaciones", decimales: new string[] { });
        }
        public string GenerarReporteTarjetaRecargable(IEnumerable<ReporteTarjetaRecargable> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "FechaCompra", Valor = "Fecha Compra" });
            elementosMostrar.Add(new Parametro { Nombre = "FechaTransaccion", Valor = "Fecha Transaccion" });
            elementosMostrar.Add(new Parametro { Nombre = "Cliente", Valor = "Documento Cliente" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreCliente", Valor = "Cliente" });
            elementosMostrar.Add(new Parametro { Nombre = "CodigoTarjeta", Valor = "Codigo Tarjeta" });
            elementosMostrar.Add(new Parametro { Nombre = "Estado", Valor = "Estado" });
            elementosMostrar.Add(new Parametro { Nombre = "NuevaRecarga", Valor = "Nueva Recarga" });
            elementosMostrar.Add(new Parametro { Nombre = "ValorUsado", Valor = "Valor Usado" });
            elementosMostrar.Add(new Parametro { Nombre = "FechaVencimiento", Valor = "Fecha Vencimiento" });
            elementosMostrar.Add(new Parametro { Nombre = "Factura", Valor = "Factura" });
            elementosMostrar.Add(new Parametro { Nombre = "Correo", Valor = "Correo" });
            elementosMostrar.Add(new Parametro { Nombre = "Direccion", Valor = "Direccion" });
            elementosMostrar.Add(new Parametro { Nombre = "Telefono", Valor = "Telefono" });

            return GenerarReporteExcel(lista, elementosMostrar, "Tarjetas recargables", decimales: new string[] { });
        }
        public string GenerarReporteCostoProducto(IEnumerable<ReporteCostoProducto> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "codigo", Valor = "Código" });
            elementosMostrar.Add(new Parametro { Nombre = "nombre", Valor = "Nombre" });
            elementosMostrar.Add(new Parametro { Nombre = "valorVentaUnitario", Valor = "Valor de Venta Unitario" });
            elementosMostrar.Add(new Parametro { Nombre = "cantidad", Valor = "Cantidad" });
            elementosMostrar.Add(new Parametro { Nombre = "costoUnitario", Valor = "Costo Unitario" });
            elementosMostrar.Add(new Parametro { Nombre = "valorTotal", Valor = "Venta Total" });
            elementosMostrar.Add(new Parametro { Nombre = "costoTotal", Valor = "Costo Total" });
            elementosMostrar.Add(new Parametro { Nombre = "porcentajeCosto", Valor = "Porcentaje Costo" });

            return GenerarReporteExcel(lista, elementosMostrar, "CostoProducto", decimales: new string[] { "C", "H", "E", "F", "G" });
        }

        public string GenerarReporteVentasPorHora(IEnumerable<ReporteVentasPorHora> listas)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "Punto", Valor = "Punto" });
            //elementosMostrar.Add(new Parametro { Nombre = "Fecha", Valor = "Fecha" });
            elementosMostrar.Add(new Parametro { Nombre = "Hora", Valor = "Rango de Hora" });
            elementosMostrar.Add(new Parametro { Nombre = "CodigoProducto", Valor = "Código de Producto" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreProducto", Valor = "Nombre del Producto" });
            elementosMostrar.Add(new Parametro { Nombre = "Cantidad", Valor = "Cantidad" });
            elementosMostrar.Add(new Parametro { Nombre = "ValorSinImpuesto", Valor = "Valor sin Impuesto" });
            elementosMostrar.Add(new Parametro { Nombre = "ValorConImpuesto", Valor = "Valor con Impuesto" });

            //elementosMostrar.Add(new Parametro { Nombre = "CostoUnitario", Valor = "Costo unitario" });
            //elementosMostrar.Add(new Parametro { Nombre = "CostoTotal", Valor = "Costo total" });
            //elementosMostrar.Add(new Parametro { Nombre = "PCosto", Valor = "Porcentaje de costo" });
            elementosMostrar.Add(new Parametro { Nombre = "PParticipacion", Valor = "Porcentaje de participación" });
            elementosMostrar.Add(new Parametro { Nombre = "TipoProducto", Valor = "Tipo de producto" });

            //return GenerarReporteExcel(listas, elementosMostrar, "VentasPorHora", decimales: new string[] { "H", "J", "K" });
            return GenerarReporteExcel(listas, elementosMostrar, "VentasPorHora", decimales: new string[] { "G", "I:4" });
        }

        public string GenerarReporteVentasPorProducto(IEnumerable<ReporteVentasPorProducto> listas)
        {
            //List<Parametro>[] listaMapeo = new List<Parametro>[1];
            List<Parametro> elementosMostrar = new List<Parametro>();

            elementosMostrar.Add(new Parametro { Nombre = "CodigoProducto", Valor = "Código de Producto" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreProducto", Valor = "Nombre del Producto" });
            elementosMostrar.Add(new Parametro { Nombre = "Punto", Valor = "Punto" });
            elementosMostrar.Add(new Parametro { Nombre = "Cantidad", Valor = "Cantidad" });
            elementosMostrar.Add(new Parametro { Nombre = "ValorSinImpuesto", Valor = "Valor sin Impuesto" });
            elementosMostrar.Add(new Parametro { Nombre = "ValorConImpuesto", Valor = "Valor con Impuesto" });

            elementosMostrar.Add(new Parametro { Nombre = "Nombre", Valor = "Tipo producto" });
            elementosMostrar.Add(new Parametro { Nombre = "Porcentaje", Valor = "Porcentaje de impuesto" });
            //elementosMostrar.Add(new Parametro { Nombre = "CostoUnitario", Valor = "Costo unitario" });
            //elementosMostrar.Add(new Parametro { Nombre = "CostoTotal", Valor = "Costo total" });
            //elementosMostrar.Add(new Parametro { Nombre = "porcentajeCosto", Valor = "Porcentaje de costo" });
            elementosMostrar.Add(new Parametro { Nombre = "porcentajeParticipacion", Valor = "Porcentaje de participacion" });
            //listaMapeo[0] = elementosMostrar;

            string[] nombreHojas = new string[1];
            nombreHojas[0] = "VentasPorProducto";
            string nombreLibro = "ReporteVentasPorProducto";

            return GenerarReporteExcel(listas, elementosMostrar, nombreLibro, sumatoria: new string[] { "D", "E", "F" }, decimales: new string[] { "I" });
            //return GenerarReporteExcelVariasHojas(listas, listaMapeo, nombreHojas, nombreLibro, true);
        }

        public string GenerarReporteRecaudosVentas(IEnumerable<ReporteRecaudosVentas> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "Concecutivo", Valor = "Consecutivo" });
            elementosMostrar.Add(new Parametro { Nombre = "Fecha", Valor = "Fecha" });
            elementosMostrar.Add(new Parametro { Nombre = "Cliente", Valor = "Cliente" });
            elementosMostrar.Add(new Parametro { Nombre = "Valor", Valor = "Valor" });
            //elementosMostrar.Add(new Parametro { Nombre = "FormaPago", Valor = "Forma de Pago" });
            //elementosMostrar.Add(new Parametro { Nombre = "NumeroReferencia", Valor = "Numero de Referencia" });
            //elementosMostrar.Add(new Parametro { Nombre = "Entidad", Valor = "Entidad" });
            //elementosMostrar.Add(new Parametro { Nombre = "Franquicia", Valor = "Franquicia" });
            //elementosMostrar.Add(new Parametro { Nombre = "Observaciones", Valor = "Observaciones" });

            return GenerarReporteExcel(lista, elementosMostrar, "RecaudoVentas");
        }


        public string GenerarReporteInventarioGeneral(IEnumerable<ReporteInventarioGeneral> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "Fecha", Valor = "Fecha" });
            elementosMostrar.Add(new Parametro { Nombre = "Almacen", Valor = "Almacen" });
            elementosMostrar.Add(new Parametro { Nombre = "CodSapMaterial", Valor = "Codigo Material" });
            elementosMostrar.Add(new Parametro { Nombre = "Nombre", Valor = "Material" });
            elementosMostrar.Add(new Parametro { Nombre = "Unidad", Valor = "Unidad de Medida" });
            elementosMostrar.Add(new Parametro { Nombre = "CostoPromedio", Valor = "Costo Promedio" });

            elementosMostrar.Add(new Parametro { Nombre = "Inicial", Valor = "Inventario inicial" });
            elementosMostrar.Add(new Parametro { Nombre = "CostoInicial", Valor = "Costo inventario inicial" });
            elementosMostrar.Add(new Parametro { Nombre = "Entradas", Valor = "Entradas de inventario" });
            elementosMostrar.Add(new Parametro { Nombre = "CostoEntradas", Valor = "Costo entradas de inventario" });
            elementosMostrar.Add(new Parametro { Nombre = "Salidas", Valor = "Salidas de inventario" });
            elementosMostrar.Add(new Parametro { Nombre = "CostoSalidas", Valor = "Costo de salidas de inventario" });
            elementosMostrar.Add(new Parametro { Nombre = "Final", Valor = "Inventario final" });
            elementosMostrar.Add(new Parametro { Nombre = "CostoFinal", Valor = "Costo inventario final" });

            return GenerarReporteExcel(lista, elementosMostrar, "InventarioGeneral");
        }
        public string GenerarReporteEntregaPedidos(IEnumerable<ReporteEntregaPedido> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "FechaEntrega", Valor = "Fecha Entrega" });
            elementosMostrar.Add(new Parametro { Nombre = "FechaImpresion", Valor = "Fecha Impresion" });
            elementosMostrar.Add(new Parametro { Nombre = "FechaUso", Valor = "Fecha Uso" });
            elementosMostrar.Add(new Parametro { Nombre = "Pedido", Valor = "Pedido" });
            elementosMostrar.Add(new Parametro { Nombre = "Cliente", Valor = "Cliente" });
            elementosMostrar.Add(new Parametro { Nombre = "Asesor", Valor = "Asesor" });
            elementosMostrar.Add(new Parametro { Nombre = "TipoVenta", Valor = "Tipo Venta" });

            elementosMostrar.Add(new Parametro { Nombre = "Producto", Valor = "Nombre Boleta" });
            elementosMostrar.Add(new Parametro { Nombre = "Cantidad", Valor = "Cantidad" });
            elementosMostrar.Add(new Parametro { Nombre = "Estado", Valor = "Estado" });
            elementosMostrar.Add(new Parametro { Nombre = "EntregadoPor", Valor = "Entregado Por" });
            elementosMostrar.Add(new Parametro { Nombre = "RecibidoPor", Valor = "Recibido Por" });
            elementosMostrar.Add(new Parametro { Nombre = "Devolucion", Valor = "Num Devolucion" });
            elementosMostrar.Add(new Parametro { Nombre = "DevolucionRecibe", Valor = "Devolucion Recibe" });
            elementosMostrar.Add(new Parametro { Nombre = "DevolucionEntrega", Valor = "Devolucion Entrega" });
            elementosMostrar.Add(new Parametro { Nombre = "CantidadRetorno", Valor = "Cantidad" });

            return GenerarReporteExcel(lista, elementosMostrar, "InventarioGeneral");
        }

        public string GenerarEntregaPedidos(IEnumerable<SolicitudRetorno> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "FechaUso", Valor = "Fecha Entrega" });
            elementosMostrar.Add(new Parametro { Nombre = "CodSapPedido", Valor = "Pedido" });
            elementosMostrar.Add(new Parametro { Nombre = "Asesor", Valor = "Asesor" });
            elementosMostrar.Add(new Parametro { Nombre = "Cliente", Valor = "Cliente" });
            elementosMostrar.Add(new Parametro { Nombre = "Tipo", Valor = "Tipo" });
            elementosMostrar.Add(new Parametro { Nombre = "Producto", Valor = "Producto" });
            elementosMostrar.Add(new Parametro { Nombre = "Cantidad", Valor = "Cantidad" });

            elementosMostrar.Add(new Parametro { Nombre = "FechaIni", Valor = "Fecha Inicial" });
            elementosMostrar.Add(new Parametro { Nombre = "FechaFin", Valor = "FechaFin" });
            elementosMostrar.Add(new Parametro { Nombre = "Estado", Valor = "Estado" });
            elementosMostrar.Add(new Parametro { Nombre = "Entrega", Valor = "Entregado Por" });
            elementosMostrar.Add(new Parametro { Nombre = "Recibe", Valor = "Recibido Por" });
            elementosMostrar.Add(new Parametro { Nombre = "FechaEntre", Valor = "Fecha entrega" });

            return GenerarReporteExcel(lista, elementosMostrar, "SalidaPedidos");
        }
        public string GenerarReporteBoleteria(IEnumerable<ReporteBoleteria> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "Fecha", Valor = "Fecha" });
            elementosMostrar.Add(new Parametro { Nombre = "Taquillero", Valor = "Taquillero" });
            elementosMostrar.Add(new Parametro { Nombre = "TipoBrazalete", Valor = "Nombre Boleta" });
            elementosMostrar.Add(new Parametro { Nombre = "Asignados", Valor = "Cantidad Asignada" });
            elementosMostrar.Add(new Parametro { Nombre = "TotalVendidos", Valor = "Venta" });
            elementosMostrar.Add(new Parametro { Nombre = "Redenciones", Valor = "Redenciones" });
            elementosMostrar.Add(new Parametro { Nombre = "Adiciones", Valor = "Adiciones" });
            elementosMostrar.Add(new Parametro { Nombre = "Entregados", Valor = "Devolucion" });
            elementosMostrar.Add(new Parametro { Nombre = "Sobrante", Valor = "Sobrantes" });
            elementosMostrar.Add(new Parametro { Nombre = "Faltante", Valor = "Faltantes" });
            elementosMostrar.Add(new Parametro { Nombre = "EnCaja", Valor = "En Caja" });
            elementosMostrar.Add(new Parametro { Nombre = "impresionEnLinea", Valor = "Impresion En Linea" });

            return GenerarReporteExcel(lista, elementosMostrar, "ControlBoleteria");
        }

        public string GenerarReporteMovimientoInventario(IEnumerable<ReporteMovimientoInventario> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "FechaMovimiento", Valor = "Fecha movimiento" });
            elementosMostrar.Add(new Parametro { Nombre = "TipoMovimiento", Valor = "Tipo movimiento" });
            elementosMostrar.Add(new Parametro { Nombre = "Motivo", Valor = "Motivo Ajuste" });
            elementosMostrar.Add(new Parametro { Nombre = "CodigoSapMaterial", Valor = "Código material" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreMaterial", Valor = "Nombre material" });
            elementosMostrar.Add(new Parametro { Nombre = "UnidadMedida", Valor = "Unidad medida" });
            elementosMostrar.Add(new Parametro { Nombre = "Cantidad", Valor = "Cantidad" });
            elementosMostrar.Add(new Parametro { Nombre = "Costo", Valor = "Costo unidad" });
            elementosMostrar.Add(new Parametro { Nombre = "CostoTotal", Valor = "Costo total" });
            elementosMostrar.Add(new Parametro { Nombre = "PuntoOrigen", Valor = "Punto origen" });
            elementosMostrar.Add(new Parametro { Nombre = "PuntoSalida", Valor = "Punto destino" });
            elementosMostrar.Add(new Parametro { Nombre = "CodSapAlmacenOrigen", Valor = "CodSapAlmacenOrigen" });
            elementosMostrar.Add(new Parametro { Nombre = "CodSapAlmacenDestino", Valor = "CodSapAlmacenDestino" });
            elementosMostrar.Add(new Parametro { Nombre = "Reponsable", Valor = "Responsable" });
            elementosMostrar.Add(new Parametro { Nombre = "Observaciones", Valor = "Observaciones" });

            return GenerarReporteExcel(lista, elementosMostrar, "MovimientoInventario", decimales: new string[] { "G", "I" });
        }

        public string GenerarReporteRestaurante(IEnumerable<ReportePedidoRestaurante> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "FechaPedido", Valor = "Fecha pedido" });
            elementosMostrar.Add(new Parametro { Nombre = "HoraRegistro", Valor = "hora registro pedido" });
            elementosMostrar.Add(new Parametro { Nombre = "HoraFinal", Valor = "hora cierre" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreZona", Valor = "Nombre Zona" });
            elementosMostrar.Add(new Parametro { Nombre = "Punto", Valor = "Punto" });
            elementosMostrar.Add(new Parametro { Nombre = "Almacen", Valor = "Almacen" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreMesa", Valor = "Mesa" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreVen", Valor = "Nombre Vendedor" });
            elementosMostrar.Add(new Parametro { Nombre = "ApellidoVen", Valor = "Apellido vendedor" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreEstadoPedido", Valor = "Estado pedido" });
            elementosMostrar.Add(new Parametro { Nombre = "VentaTotal", Valor = " Valor venta" });
            elementosMostrar.Add(new Parametro { Nombre = "Id_Factura", Valor = "Id Factura" });
            elementosMostrar.Add(new Parametro { Nombre = "Productos", Valor = "Productos" });


            return GenerarReporteExcel(lista, elementosMostrar, "ReporteRestaurante");
        }
        public string GenerarReporteBonoRegalo(IEnumerable<ReporteBonoRegalo> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "FechaCompra", Valor = "Fecha de compra" });
            elementosMostrar.Add(new Parametro { Nombre = "FechaInicial", Valor = "Fecha Inicial" });
            elementosMostrar.Add(new Parametro { Nombre = "FechaFinal", Valor = "Fecha Final" });
            elementosMostrar.Add(new Parametro { Nombre = "EsBoletaControl", Valor = "Es Boleta control" });
            elementosMostrar.Add(new Parametro { Nombre = "Fiestas", Valor = "Fiesta" });
            elementosMostrar.Add(new Parametro { Nombre = "Adicional", Valor = "Adicional" });
            elementosMostrar.Add(new Parametro { Nombre = "CodSapPedido", Valor = "CodSapPedido" });
            elementosMostrar.Add(new Parametro { Nombre = "CodSapCliente", Valor = "CodSapCliente" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreCliente", Valor = "Nombre Cliente" });
            elementosMostrar.Add(new Parametro { Nombre = "CodSapVendedor", Valor = "CodSapVendedor" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreVendedor", Valor = "Nombre Vendedor" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreProducto", Valor = "Nombre Producto" });
            elementosMostrar.Add(new Parametro { Nombre = "Cantidad", Valor = "Cantidad" });
            elementosMostrar.Add(new Parametro { Nombre = "Valor", Valor = "Valor" });
            elementosMostrar.Add(new Parametro { Nombre = "consumo", Valor = "consumo" });
            elementosMostrar.Add(new Parametro { Nombre = "saldo", Valor = "saldo" });
           


            return GenerarReporteExcel(lista, elementosMostrar, "ReporteBonoRegalo");
        }

        public string GenerarReporteCuadreDiarioFlujoCajasTaq(IEnumerable<ReporteCuadreDiarioFlujoCajasTaq> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "Fecha", Valor = "Fecha" });
            elementosMostrar.Add(new Parametro { Nombre = "Efectivo", Valor = "Ingresos en efectivo" });
            elementosMostrar.Add(new Parametro { Nombre = "Tarjetas", Valor = "Ingresos en tarjeta" });
            elementosMostrar.Add(new Parametro { Nombre = "BonoRegalo", Valor = "Ingresos por bonos" });
            elementosMostrar.Add(new Parametro { Nombre = "BonoSodexo", Valor = "Ingresos bonos Sodexo" });
            elementosMostrar.Add(new Parametro { Nombre = "Deposito", Valor = "Ingresos deposito" });
            elementosMostrar.Add(new Parametro { Nombre = "Payulatam", Valor = "Ingresos Payulatam" });
            elementosMostrar.Add(new Parametro { Nombre = "DescuentoEmpleadoActivos", Valor = "Consumos Empleados Activos" });
            elementosMostrar.Add(new Parametro { Nombre = "DescuentoEmpleadoServiola", Valor = "Consumos Empleados Serviola" });
            elementosMostrar.Add(new Parametro { Nombre = "DescuentoEmpleadoCorparques", Valor = "Consumo Empleados Corparques" });

            //elementosMostrar.Add(new Parametro { Nombre = "Recaudos", Valor = "Recaudos" });
            elementosMostrar.Add(new Parametro { Nombre = "ConsumosCortesias", Valor = "Consumo de Cortesias" });
            elementosMostrar.Add(new Parametro { Nombre = "Sobrantes", Valor = "Sobrantes" });
            elementosMostrar.Add(new Parametro { Nombre = "Faltantes", Valor = "Faltantes" });
            elementosMostrar.Add(new Parametro { Nombre = "TotalEfectivo", Valor = "Total efectivo" });

            return GenerarReporteExcel(lista, elementosMostrar, "CuadreDiarioFlujoCajasTaq");
        }

        /// <summary>
        /// RDSH: Genera el archivo en excel para el reporte de notificaciones.
        /// </summary>
        /// <param name="listas"></param>
        /// <returns></returns>
        public string GenerarReporteNotificaciones(IEnumerable<ReporteNotificaciones> listas)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "Remitente", Valor = "Remitente" });
            elementosMostrar.Add(new Parametro { Nombre = "Asunto", Valor = "Asunto" });
            elementosMostrar.Add(new Parametro { Nombre = "Contenido", Valor = "Contenido" });
            elementosMostrar.Add(new Parametro { Nombre = "Receptor", Valor = "Receptor" });
            elementosMostrar.Add(new Parametro { Nombre = "FechaRecepcion", Valor = "Fecha Recepción" });

            return GenerarReporteExcel(listas, elementosMostrar, "Notificaciones");
        }

        public string GenerarReporteInventarioEquipos(IEnumerable<ReporteInventario> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "Dependencia", Valor = "Dependencia" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreEquipo", Valor = "Nombre de equipo" });
            elementosMostrar.Add(new Parametro { Nombre = "Serial", Valor = "Serial" });
            elementosMostrar.Add(new Parametro { Nombre = "CodigoActivo", Valor = "Codigo Activo" });
            elementosMostrar.Add(new Parametro { Nombre = "IdPuntoSap", Valor = "Id Punto Core" });
            elementosMostrar.Add(new Parametro { Nombre = "Ubicacion", Valor = "Ubicación por cada establecimiento de comercio" });
            elementosMostrar.Add(new Parametro { Nombre = "Sede", Valor = "Sede" });
            elementosMostrar.Add(new Parametro { Nombre = "Oficina", Valor = "Oficina" });

            return GenerarReporteExcel(lista, elementosMostrar, "InventarioMaquinas");
        }

        public string GenerarReporteVentasDiarias(IEnumerable<ReporteVentasDiario> lista)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "RazonSocial", Valor = "Razon Social" });
            elementosMostrar.Add(new Parametro { Nombre = "Nit", Valor = "Nit" });
            elementosMostrar.Add(new Parametro { Nombre = "Serial", Valor = "Serial Computador" });
            elementosMostrar.Add(new Parametro { Nombre = "FechaCreacion", Valor = "Fecha Emisión Factura" });
            elementosMostrar.Add(new Parametro { Nombre = "FacturaInicial", Valor = "Factura Inicial" });
            elementosMostrar.Add(new Parametro { Nombre = "FacturaFinal", Valor = "Factura Final" });
            elementosMostrar.Add(new Parametro { Nombre = "CodigoFactura", Valor = "Factura" });
            elementosMostrar.Add(new Parametro { Nombre = "Id_Producto", Valor = "Id Producto" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreProducto", Valor = "Producto" });
            elementosMostrar.Add(new Parametro { Nombre = "Precio", Valor = "Valor" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreImpuesto", Valor = "Tipo Impuesto" });
            elementosMostrar.Add(new Parametro { Nombre = "Porcentaje", Valor = "Valor Impuesto" });
            elementosMostrar.Add(new Parametro { Nombre = "PrecioNeto", Valor = "Valor Neto" });
            elementosMostrar.Add(new Parametro { Nombre = "Departamento", Valor = "Departamento" });
            elementosMostrar.Add(new Parametro { Nombre = "idPunto", Valor = "Id Punto" });
            elementosMostrar.Add(new Parametro { Nombre = "NombrePunto", Valor = "Nombre Punto" });


            return GenerarReporteExcel(lista, elementosMostrar, "ReporteVentasDiarias");
        }

        public string GenerarReporteVentasPorConvenio(IEnumerable<ReporteVentasPorConvenio> listas)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "Fecha", Valor = "Fecha" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreConvenio", Valor = "NombreConvenio" });
            elementosMostrar.Add(new Parametro { Nombre = "TipoProducto", Valor = "TipoProducto" });
            elementosMostrar.Add(new Parametro { Nombre = "CodigoProducto", Valor = "CodigoProducto" });
            elementosMostrar.Add(new Parametro { Nombre = "NombreProducto", Valor = "NombreProducto" });
            elementosMostrar.Add(new Parametro { Nombre = "PrecioSinDescuento", Valor = "PrecioSinDescuento" });
            elementosMostrar.Add(new Parametro { Nombre = "PrecioconDescuento", Valor = "PrecioconDescuento" });
            elementosMostrar.Add(new Parametro { Nombre = "PorcentajeDescuento", Valor = "PorcentajeDescuento" });
            elementosMostrar.Add(new Parametro { Nombre = "ValorSinImpuesto", Valor = "ValorSinImpuesto" });
            elementosMostrar.Add(new Parametro { Nombre = "Cantidad", Valor = "Cantidad" });

            return GenerarReporteExcel(listas, elementosMostrar, "ReporteVentasPorConvenio");
        }

        public string GenerarReporteFanVendidas(IEnumerable<ReporteFANVendidas> listas)
        {
            List<Parametro> elementosMostrar = new List<Parametro>();
            elementosMostrar.Add(new Parametro { Nombre = "IdBoleteria", Valor = "IdBoleteria" });
            elementosMostrar.Add(new Parametro { Nombre = "IdProducto", Valor = "IdProducto" });
            elementosMostrar.Add(new Parametro { Nombre = "Consecutivo", Valor = "Consecutivo" });
            elementosMostrar.Add(new Parametro { Nombre = "IdSolicitudBoleteria", Valor = "IdSolicitudBoleteria" });
            elementosMostrar.Add(new Parametro { Nombre = "IdEstado", Valor = "IdEstado" });
            elementosMostrar.Add(new Parametro { Nombre = "Valor", Valor = "Valor" });
            elementosMostrar.Add(new Parametro { Nombre = "CodigoVenta", Valor = "CodigoVenta" });
            elementosMostrar.Add(new Parametro { Nombre = "FechaImpresion", Valor = "FechaImpresion" });
            elementosMostrar.Add(new Parametro { Nombre = "Documento", Valor = "Documento" });
            elementosMostrar.Add(new Parametro { Nombre = "Nombre", Valor = "Nombre" });
            elementosMostrar.Add(new Parametro { Nombre = "Correo", Valor = "Correo" });
            elementosMostrar.Add(new Parametro { Nombre = "Telefono", Valor = "Telefono" });
            elementosMostrar.Add(new Parametro { Nombre = "FechaNacimiento", Valor = "FechaNacimiento" });
            elementosMostrar.Add(new Parametro { Nombre = "Genero", Valor = "Genero" });
            elementosMostrar.Add(new Parametro { Nombre = "Direccion", Valor = "Direccion" });

            return GenerarReporteExcel(listas, elementosMostrar, "ReporteTarjetasFANVendidas");
        }

        #endregion
    }
}
