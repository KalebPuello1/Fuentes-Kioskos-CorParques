using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using ClosedXML.Excel;
using CorParques.Negocio.Entidades;
using System.Globalization;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ConvenioConsecutivoController : ControladorBase
    {
        // GET: ConvenioConsecutivo
        public async Task<ActionResult> Index()
        {
            var listaConvenios = await GetAsync<IEnumerable<Convenio>>("Convenio/ObtenerListaConvenios");
            if (listaConvenios != null && listaConvenios.Count() > 0)
                listaConvenios = listaConvenios.Where(x => x.EsActivo);
            return View(listaConvenios);
        }


        public async  Task<JsonResult> SubirArchivo(string archivo, int convenio)
        {

            string rta = string.Empty;
            List<ConvenioConsecutivos> _lista = new List<ConvenioConsecutivos>();
            bool CamposValidos = true;

            try
            {
                var path = Path.Combine(Server.MapPath("~/Archivos/temp/"), archivo);
                DataTable dt = new DataTable();

                using (XLWorkbook workBook = new XLWorkbook(path))
                {
                    IXLWorksheet workSheet = workBook.Worksheet(1);

                    bool firstRow = true;
                    foreach (IXLRow row in workSheet.Rows())
                    {
                        if (firstRow)
                        {
                            foreach (IXLCell cell in row.Cells())
                            {
                                dt.Columns.Add(cell.Value.ToString());
                            }
                            firstRow = false;
                        }
                        else
                        {
                            dt.Rows.Add();
                            int i = 0;
                            foreach (IXLCell cell in row.Cells("1:4"))
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                i++;
                            }
                        }
                    }
                }



                if (dt != null && dt.Columns.Count > 0 && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        //var codSapConvenio = string.Empty;
                        var Consecutivo = string.Empty;
                        //var codSapProductos = string.Empty;
                        var FechasEspeciales = string.Empty;
                        DateTime? FechaInicial = null;
                        DateTime? FechaFinal = null;
                        DateTime fecha = DateTime.Now;

                        FechasEspeciales = item["FechaCumpleaños"] == null ? string.Empty : item["FechaCumpleaños"].ToString();
                        if (!string.IsNullOrEmpty(FechasEspeciales))
                        {
                            var _listaFechas = FechasEspeciales.Split(',');
                            foreach (var _fecha in _listaFechas)
                            {
                                if (!DateTime.TryParseExact(_fecha, "dd-MM-yyyy", CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out fecha))
                                    throw new ArgumentException("Fecha con formato incorrecto campo FechaCumpleaños, día-mes-año");
                            }
                        }

                        if(item["FechaInicial"]!= null && (!string.IsNullOrEmpty(item["FechaInicial"].ToString())))
                        {
                            if (!DateTime.TryParseExact(item["FechaInicial"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out fecha))
                                throw new ArgumentException("Fecha con formato incorrecto campo Fechainicial, día-mes-año");
                            else
                                FechaInicial = fecha;

                        }
                        if(item["FechaFinal"]!= null && (!string.IsNullOrEmpty(item["FechaFinal"].ToString())))
                        {
                            if (!DateTime.TryParseExact(item["FechaFinal"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture,
                                DateTimeStyles.None, out fecha))
                                throw new ArgumentException("Fecha con formato incorrecto campo fechaFinal, día-mes-año");
                            else
                                FechaFinal = fecha;
                        }


                        //codSapConvenio = item["CodSapConvenio"].ToString();
                        Consecutivo = item["Consecutivos"].ToString();
                        //codSapProductos = item["CodSapProductos"].ToString();
                        

                        if (!(string.IsNullOrEmpty(Consecutivo) && string.IsNullOrEmpty(FechasEspeciales) 
                            && FechaInicial == null && FechaFinal == null))
                        {

                            _lista.Add(new ConvenioConsecutivos
                            {
                                Consecutivo = Consecutivo,
                                FechasEspeciales = FechasEspeciales,
                                FechaInicial = FechaInicial,
                                FechaFinal = FechaFinal,
                                IdConvenio = convenio
                            });
                        }
                    }

                    if (_lista.Count == 0)
                        rta = "El documento no tiene registros para cargar";
                    else
                    {
                        //Validar  los campos obligatorios (CodSapConvenio,Consecutivo)

                        foreach (var item in _lista)
                        {
                            //if (string.IsNullOrEmpty(item.CodSapConvenio))
                            //    CamposValidos = false;

                            if (string.IsNullOrEmpty(item.Consecutivo))
                                CamposValidos = false;
                        }

                        if (!CamposValidos)
                            rta = "El campo Consecutivo es obligatorio ";
                        else
                        {
                            var resp = await PostAsync<List<ConvenioConsecutivos>, string>("ConvenioSAP/InsertarConvenioConsecutivos", _lista);
                            rta = resp.Elemento != null ? resp.Elemento.ToString() : "";
                        }
                    }

                }
                else
                    rta = "El documento no tiene registros para cargar o esta corrupto el archivo";

            }
            catch (Exception ex)
            {
                rta = "Se presento el siguiente error " + ex.Message;
            }


            RemoveFile(archivo);
            return Json(rta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void SaveFile()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Archivos/temp/"), fileName);
                    file.SaveAs(path);
                }
            }

        }
        public void RemoveFile(string name)
        {
            var path = Path.Combine(Server.MapPath("~/Archivos/temp/"), name);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }
        
    }
}