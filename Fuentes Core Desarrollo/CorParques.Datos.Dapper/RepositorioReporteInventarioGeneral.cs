using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace CorParques.Datos.Dapper
{
    public class RepositorioReporteInventarioGeneral : RepositorioBase<ReporteInventarioGeneral>, IRepositorioReporteInventarioGeneral
    {

        public IEnumerable<ReporteInventarioGeneral> ObtenerTodos(ReporteInventarioGeneral modelo)
        {
            try
            {
            var objResultado = _cnn.Query<ReporteInventarioGeneral>("SP_ReporteInventarioGeneral",
                                             param: new
                                             {
                                                 @fIni = modelo.fechaInicial,
                                                 @fFin = modelo.fechaFinal,
                                                 @CodSapAlmacen = modelo.Almacen == "0" ? "" : modelo.Almacen,
                                                 @idMaterial = modelo.idMaterial,
                                                 @CentroBeneficio = modelo.CB
                                            },
                                            commandType: System.Data.CommandType.StoredProcedure, commandTimeout: 80000);
                 return objResultado;
            }
            catch (Exception ex)
            {
                Transversales.Util.Utilidades.RegistrarError(ex, "RepositorioReporteInventarioGeneral_ObtenerTodos");
                return null;
            }

     
         }
        public IEnumerable<ReporteBoleteria> Obtenerboleteria(string fecha,string usuario)
        {

            try
            {
                List<ReporteBoleteria> Respuesta = new List<ReporteBoleteria>();    
                var objResultado = _cnn.Query<ReporteBoleteria>(
                                             "SP_ReporteBoleteriaUsuario",
                                             param: new
                                             {
                                                 @Fecha = fecha,
                                                 @IdUsuario = usuario
                                             },
                                             commandType: System.Data.CommandType.StoredProcedure);

               var objCorteBrazaleteImpresionEnLinea = _cnn.Query<ReporteBoleteria>("SP_ConsultarMovimientoBrazaletesImpresionEnLine",
                                param: new
                                {
                                    @Fecha = fecha,
                                    @IdUsuario = usuario
                                }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                if (objResultado != null && objResultado.Count() != 0)
                {
                    var ii = 0;
                    foreach (var item in objResultado)
                    {
                        if (!item.TipoBrazalete.ToUpper().Contains("USO") && !item.TipoBrazalete.ToUpper().Contains("DESTREZA"))
                        {
                            Respuesta.Add(item);
                        }
                        if (objCorteBrazaleteImpresionEnLinea.Exists(X => X.TipoBrazalete.Contains(item.TipoBrazalete) && !X.TipoBrazalete.ToUpper().Contains("ROLLO")))
                        {
                            if (!Respuesta.Exists(x => x.TipoBrazalete == item.TipoBrazalete && x.Taquillero == item.Taquillero) && !item.TipoBrazalete.ToUpper().Contains("USO") && !item.TipoBrazalete.ToUpper().Contains("DESTREZA"))
                            {
                                Respuesta.Add(objCorteBrazaleteImpresionEnLinea.Where(X => X.TipoBrazalete.Contains(item.TipoBrazalete)).First());
                            }
                        }
                        else
                        {
                            if (ii == 0)
                            {
                                foreach (var it in objCorteBrazaleteImpresionEnLinea)
                                {
                                    if (!Respuesta.Exists(x => x.TipoBrazalete == it.TipoBrazalete && x.Taquillero == it.Taquillero) && !item.TipoBrazalete.ToUpper().Contains("USO") && !item.TipoBrazalete.ToUpper().Contains("DESTREZA"))
                                    {
                                        Respuesta.Add(it);
                                    }
                                }
                                ii++;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in objCorteBrazaleteImpresionEnLinea)
                    {
                        if (!item.TipoBrazalete.ToUpper().Contains("ROLLO") && !item.TipoBrazalete.ToUpper().Contains("USO") && !item.TipoBrazalete.ToUpper().Contains("DESTREZA"))
                        {
                            Respuesta.Add(item);
                        }
                    }
                }
                int cantidadVendidos = 0;
                var i = 0;
               /* foreach (var item in Respuesta)
                {
                    if (item.TipoBrazalete.ToUpper().Contains("ROLLO"))
                    {
                        foreach (var it in Respuesta)
                        {
                            cantidadVendidos += it.TotalVendidos;
                        }
                        item.EnCaja = item.Asignados - cantidadVendidos;
                        item.TotalVendidos = cantidadVendidos;
                    }
                    i++;
                }*/

                //return objResultado;
                return Respuesta;

            }
            catch (Exception ex)
            {
                Transversales.Util.Utilidades.RegistrarError(ex, "RepositorioReporteInventarioGeneral_ObtenerTodos");
                return null;
            }


        }

        public IEnumerable<ReporteEntregaPedido> ObtenerEntregasInstitucionales(string fechaEntrega,string fechaUso, string pedido, string asesor,string cliente,string producto)
        {

            try
            {
                var objResultado = _cnn.Query<ReporteEntregaPedido>(
                                             "SP_ReporteEntregaInstitucional",
                                             param: new
                                             {
                                                 @FechaEntrega = fechaEntrega,
                                                 @FechaUso = fechaUso,
                                                 @Pedido = pedido,
                                                 @Asesor = asesor,
                                                 @Cliente= cliente,
                                                 @Producto = producto
                                             },
                                             commandType: System.Data.CommandType.StoredProcedure);
                return objResultado;

            }
            catch (Exception ex)
            {
                Transversales.Util.Utilidades.RegistrarError(ex, "RepositorioReporteInventarioGeneral_ObtenerTodos");
                return null;
            }


        }
    }
}
