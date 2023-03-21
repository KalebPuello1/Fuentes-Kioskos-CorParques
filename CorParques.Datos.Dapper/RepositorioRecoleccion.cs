using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace CorParques.Datos.Dapper
{

    public class RepositorioRecoleccion : IRepositorioRecoleccion
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;
        private IRepositorioParametros parametros;
        #endregion

        #region Constructor

        public RepositorioRecoleccion(IRepositorioParametros _parametros)
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
            this.parametros = _parametros;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Actualiza una recoleccion.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(Recoleccion modelo, out string error)
        {

            try
            {

                error = _cnn.Query<string>("SP_ActualizarRecoleccion", param: new
                {
                    IdRecoleccion = modelo.IdRecoleccion,
                    IdPunto = modelo.IdPunto,
                    IdEstado = modelo.IdEstado,
                    IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                    FechaModificacion = modelo.FechaModificacion,
                    IdUsuarioSupervisor = modelo.IdUsuarioSupervisor,
                    IdUsuarioNido = modelo.IdUsuarioNido
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_RepositorioRecoeccion: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);

        }

        /// <summary>
        /// RDSH: Borrado logico de una recoleccion.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Eliminar(Recoleccion modelo, out string error)
        {
            error = _cnn.Query<string>("SP_EliminarRecoleccion", param: new
            {
                IdConvenioParqueadero = modelo.IdRecoleccion,
                IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                FechaModificacion = modelo.FechaModificacion                
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: Inserta una recoleccion.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(Recoleccion modelo, out string error, out int IdRecoleccion)
        {

            IdRecoleccion = 0;

            try
            {
                error = _cnn.Query<string>("SP_InsertarRecoleccion", param: new
                {
                    IdPunto = modelo.IdPunto,
                    IdEstado = modelo.IdEstado,
                    IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                    FechaCreacion = modelo.FechaCreacion,
                    Cierre = modelo.Cierre
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                if (error.Trim().Length > 0)
                {
                    int.TryParse(error, out IdRecoleccion);
                    if (IdRecoleccion > 0)
                    {
                        error = string.Empty;
                    }
                }
                else
                {
                    throw new ArgumentException("No fue posible guardar la recolección");
                }

            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_RepositorioRecoleccion: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);

        }

        /// <summary>
        /// RDSH: Retorna una recoleccion por id para su edicion.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Recoleccion ObtenerPorId(int Id)
        {
            var objRecoleccion = _cnn.GetList<Recoleccion>().Where(x => x.IdRecoleccion == Id).FirstOrDefault();
            return objRecoleccion;
        }

        /// <summary>
        /// RDSH: Retorna una recoleccion por id del usuario y por el id del punto.
        /// </summary>
        /// <param name="intIdUsuario"></param>
        /// <returns></returns>
        public Recoleccion ObtenerRecoleccionActiva(int intIdUsuario, int intIdPunto, bool blnCierre, int IdEstado)
        {
            var objRecoleccion = _cnn.Query<Recoleccion>("SP_ObtenerRecoleccionActiva", param: new
            {
                IdPunto = intIdPunto,
                IdUsuario = intIdUsuario,
                Cierre = blnCierre,
                IdEstado = IdEstado
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return objRecoleccion;
        }

        /// <summary>
        /// RDSH: Retorna los documentos pendientes de recolección.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MediosPagoFactura> ObtenerDocumentosRecoleccion(int IdUsuario)
        {
            var objMediosPagoFactura = _cnn.Query<MediosPagoFactura>("SP_ObtenerDocumentosRecoleccion", param: new {
                IdUsuario = IdUsuario
            }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return objMediosPagoFactura;
        }

        /// <summary>
        /// RDSH: Retorna si se muestra la recoleccion base, corte y los topes para cada una de estas.
        /// </summary>
        /// <param name="intIdUsuario"></param>
        /// <param name="intIdPunto"></param>
        /// <returns></returns>
        public RecoleccionAuxliar ObtenerTopesRecoleccion(int intIdUsuario, int intIdPunto)
        {
            var objRecoleccionAuxliar = _cnn.Query<RecoleccionAuxliar>("SP_ReglasAlistamientoTaquillero", param: new
            {
                IdPunto = intIdPunto,
                IdUsuario = intIdUsuario
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return objRecoleccionAuxliar;
        }

        public RecoleccionAuxliar ObtenerTopesCierreTaquilla(int intIdUsuario, int intIdPunto)
        {
            var objRecoleccionAuxliar = _cnn.Query<RecoleccionAuxliar>("SP_ReglasCierreTaquilla", param: new
            {
                IdPunto = intIdPunto,
                IdUsuario = intIdUsuario
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return objRecoleccionAuxliar;
        }

        public string ObtenerNumBoletaImpresionEnLinea()
        {
            var numImpresionLinea = _cnn.Query<string>("SP_ObtenerNumBoletasImpresionEnLinea", commandType: System.Data.CommandType.StoredProcedure).First();
            return numImpresionLinea;
        }

        /// <summary>
        /// RDSH: Retorna la cantidad de brazaletes restantes.
        /// </summary>
        /// <param name="intIdUsuario"></param>
        /// <param name="intIdPunto"></param>
        /// <returns></returns>
        public IEnumerable<CierreBrazalete> ObtenerBrazaletesRestantes(int intIdUsuario, int intIdPunto)
        {
            var cierre = new List<CierreBrazalete>();

            var objCorteBrazalete = _cnn.Query<CierreBrazalete>("SP_ObtenerBrazaletesRestantes", param: new
            {
                IdUsuario = intIdUsuario,
                IdPunto = intIdPunto                
            }, commandType: System.Data.CommandType.StoredProcedure).ToList();

            var objCorteBrazaleteImpresionEnLinea = _cnn.Query<CierreBrazalete>("SP_ConsultarBrazaletesImpresionEnLinea",
                param: new {
                    @IdUsuario = intIdUsuario
                }, commandType: System.Data.CommandType.StoredProcedure).ToList();



            var datoRestar = "ROLLO";

            var datoRollo = parametros.ObtenerParametroPorNombre("RestarRollo");
            datoRestar = datoRollo.Valor;

            ///Aca se va hacer la validacion del rollo a descuento



            /*if (objCorteBrazaleteImpresionEnLinea != null)
            {
                foreach (var item in objCorteBrazaleteImpresionEnLinea)
                { 
                    objCorteBrazalete.Add(new CierreBrazalete()
                    {
                        TipoBrazalete = item.TipoBrazalete,
                        CodigoSap = item.CodigoSap,
                        TotalVendidos = item.TotalVendidos,
                        Asignados = 0,
                        IdTipoBrazalete = item.IdTipoBrazalete,
                        TotalDiferencia = 0
                    });
                }
             }*/

            if (objCorteBrazalete.Count() != 0 || objCorteBrazalete == null)
            {
                var i = 0;
                foreach (var item in objCorteBrazalete)
                {
                    cierre.Add(item);
                    //if (objCorteBrazaleteImpresionEnLinea.Exists(X => X.IdTipoBrazalete == item.IdTipoBrazalete && !X.TipoBrazalete.ToUpper().Contains("ROLLO")))
                    if (objCorteBrazaleteImpresionEnLinea.Exists(X => X.IdTipoBrazalete == item.IdTipoBrazalete && !X.TipoBrazalete.ToUpper().Contains(datoRestar)))
                    {
                        if (objCorteBrazaleteImpresionEnLinea != null || objCorteBrazaleteImpresionEnLinea.Count() > 0)
                        {
                            if (!cierre.Exists(x => objCorteBrazaleteImpresionEnLinea.Exists(xy => xy.TipoBrazalete == x.TipoBrazalete)))
                            {
                                cierre.Add(objCorteBrazaleteImpresionEnLinea.Where(X => X.IdTipoBrazalete == item.IdTipoBrazalete).First());
                            }
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            foreach (var it in objCorteBrazaleteImpresionEnLinea)
                            {
                                if (!cierre.Exists(x => x.TipoBrazalete == it.TipoBrazalete))
                                {
                                    cierre.Add(it);
                                }
                            }
                            i++;
                        }
                     }
                    
                }
            }
            else
            {
                foreach (var item in objCorteBrazaleteImpresionEnLinea)
                {
                    //if (!item.TipoBrazalete.ToUpper().Contains("ROLLO"))
                    if (!item.TipoBrazalete.ToUpper().Contains(datoRestar))
                    {
                        cierre.Add(item);
                    }
                    
                }
                /*if (objCorteBrazaleteImpresionEnLinea.Exists(X => X.IdTipoBrazalete == item.IdTipoBrazalete && !X.TipoBrazalete.ToUpper().Contains("ROLLO")))
                {
                    cierre.Add(objCorteBrazaleteImpresionEnLinea.Where(X => X.IdTipoBrazalete == item.IdTipoBrazalete).First());
                }*/
            }

            int cantidadVendidos = 0;
            foreach (var item in cierre)
            {
                //if (item.TipoBrazalete.ToUpper().Contains("ROLLO"))
                if (item.TipoBrazalete.ToUpper().Contains(datoRestar))
                {
                    foreach (var it in cierre)
                    {
                        if (it.TipoBrazalete.ToUpper().Contains("IMPRESION EN LINEA"))
                        {
                            cantidadVendidos += it.TotalVendidos;
                        }
                    }
                    item.EnCaja = item.Asignados - cantidadVendidos;
                    item.TotalVendidos = cantidadVendidos;
                }
                
            }
           
            //return objCorteBrazalete;
            return cierre;
        }

        public IEnumerable<NotificacionAlerta> ObtenerNotificaciones()
        {
            IEnumerable<NotificacionAlerta> notificaciones = _cnn.Query<NotificacionAlerta>("SP_GetNotificationsPoints", commandType: System.Data.CommandType.StoredProcedure);

            //Daniel Salgado
            return notificaciones;
            //return null;
        }

        /// <summary>
        /// RDSH: Consulta los puntos que tienen recoleccion segun el estado y si la recoleccion es de cierre o no.
        /// </summary>
        /// <param name="intIdEstado"></param>
        /// <param name="blnCierre"></param>
        /// <returns></returns>
        public IEnumerable<TipoGeneralValor> ObtenerPuntosRecoleccion(int intIdEstado, bool blnCierre)
        {
            var objListaPuntos = _cnn.Query<TipoGeneralValor>("SP_ObtenerPuntosRecoleccion", param: new
            {
                IdEstado = intIdEstado,
                Cierre = blnCierre
            }, commandType: System.Data.CommandType.StoredProcedure).Select(x => new TipoGeneralValor { Valor = x.Valor, Nombre = x.Nombre });
            return objListaPuntos;
        }

        /// <summary>
        /// RDSH: Consulta los taquilleros que tienen alistamiento de cierre o de recoleccion, esto para el proceso de entrega a supervisor.
        /// </summary>
        /// <param name="intIdEstado"></param>
        /// <param name="blnCierre"></param>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerTaquillerosConRecoleccion(int intIdEstado, bool blnCierre)
        {
            var objListaPuntos = _cnn.Query<TipoGeneral>("SP_ObtenerTaquillerosConRecoleccion", param: new
            {
                IdEstado = intIdEstado,
                Cierre = blnCierre
            }, commandType: System.Data.CommandType.StoredProcedure).Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });
            return objListaPuntos;
        }

        /// <summary>
        /// RDSH: Retorna las novedades pendientes de recolección.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<NovedadArqueo> ObtenerNovedadesRecoleccion(int IdUsuario)
        {            
            var objMediosPagoFactura = _cnn.Query<NovedadArqueo>("SP_ObtenerNovedadesRecoleccion", param: new
            {
                IdUsuario = IdUsuario
            }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return objMediosPagoFactura;
        }


        /// <summary>
        /// RDSH: Borrado del detalle de las recolecciones al editar una recoleccion y desmarcar los voucher, documentos, novedad.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool EliminarDetalleRecoleccion(int intIdRecoleccion, int intIdTipoRecoleccion)
        {

            string strError = string.Empty;
            
            try
            {
                strError = _cnn.Query<string>("SP_EliminarDetalleRecoleccion", param: new
                {
                    IdRecoleccion = intIdRecoleccion,
                    IdTipoRecoleccion = intIdTipoRecoleccion
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception)
            {
                strError = string.Concat("Error en EliminarDetalleRecoleccion_RepositorioRecoleccion");
            }

            return string.IsNullOrEmpty(strError);

        }

        /// <summary>
        /// RDSH: Retorna las recolecciones activas.
        /// </summary>
        /// <param name="intIdUsuario"></param>
        /// <returns></returns>
        public IEnumerable<Recoleccion> ObtenerRecoleccionesActivas(int intIdUsuario, int intIdPunto, bool blnCierre, int IdEstado)
        {

            IEnumerable<Recoleccion> objRecoleccion = null;

            try
            {
                objRecoleccion = _cnn.Query<Recoleccion>("SP_ObtenerRecoleccionActiva", param: new
                {
                    IdPunto = intIdPunto,
                    IdUsuario = intIdUsuario,
                    Cierre = blnCierre,
                    IdEstado = IdEstado
                }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en RepositorioRecoleccion_ObtenerRecoleccionesActivas: ", ex.Message));              
            }

            return objRecoleccion;
        }

        public int ControlBoleteria(int idBoleta)
        {
            int dato = 0;
            try
            {
                var d = _cnn.Query<string>("SP_ObtenerNumBoletaEnLinea", commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    @idBoleta = idBoleta
                }).First();
                dato = d == null ? 0 : int.Parse(d);
            }
            catch (Exception)
            {
                dato = 0;
            }
            return dato;
        }

        public IEnumerable<Producto> NumBoletasControlBoleteria()
        {
            IEnumerable<Producto> dato = _cnn.Query<Producto>("SP_ObtenerNumBoletasControlBoleteria", 
                commandType: System.Data.CommandType.StoredProcedure).ToList();
            return dato;
        }

        public string ModificarControlBoleteria(int idBoleta, int NumBoletasRestantes)
        {
            string dato = "";
            try
            {
                NumBoletasRestantes = (NumBoletasRestantes < 0 || NumBoletasRestantes == null) ? 0 : NumBoletasRestantes; 
                 dato = _cnn.Query<string>("SP_ModificarNumBoletaEnLinea", commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    @idBoleta = idBoleta,
                    @NumBoletas = NumBoletasRestantes
                }).First();
            }
            catch (Exception)
            {
                dato = "Esta boleta genero error";
            }
            return dato;
        }


        public Producto ValidarImpresionEnLinea(int idBoleteria)
        {
            Producto dato = new Producto();
            try
            {
                dato = _cnn.Query<Producto>("SP_ObtenerPasaportesImpresionEnLInea", commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    @idProducto = idBoleteria
                }).First();
            }
            catch (Exception e)
            {
                dato = new Producto();
                dato.Nombre = "No existe el producto o es exeption";
                dato.Cantidad = 1;
            }
            return dato;
        }


        public IEnumerable<Producto> VerPasaportesCodigoPedido(string codigoPedido)
        {
            var dato = new List<Producto>();
                try
            {
                dato = _cnn.Query<Producto>("SP_ObtenerPasaportesCodigoPedido", commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    @codigoPedido = codigoPedido
                }).ToList();
                dato = dato == null ? new List<Producto>() : dato;
            }
            catch (Exception)
            {
                Producto p = new Producto();
                p.Nombre = "No existe";
                p.Cantidad = 0;
                dato = new List<Producto>();
                dato.Add(p);
            }
            return dato;
        }
        #endregion




    }
}
