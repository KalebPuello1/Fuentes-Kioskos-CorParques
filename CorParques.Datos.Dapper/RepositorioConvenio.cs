using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Transversales.Util;

namespace CorParques.Datos.Dapper
{

	public class RepositorioConvenio : RepositorioBase<Convenio>,  IRepositorioConvenio
	{

        #region Metodos
        
        /// <summary>
        /// RDSH: Retorna la información de los pedidos.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Convenio> ObtenerListaConvenios()
        {

            List<Convenio> objListaConvenios = null; 

            try
            {
                objListaConvenios = _cnn.Query<Convenio>("SP_ObtenerListaConvenios", null, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ObtenerListaConvenios_RepositorioConvenio");
            }

            return objListaConvenios;
        }

        /// <summary>
        /// RDSH: Actualiza un convenio.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(Convenio modelo, out string error)
        {

            error = string.Empty;

            try
            {
                error = _cnn.Query<string>("SP_ActualizarConvenio", param: new
                {
                    IdConvenio = modelo.IdConvenio,
                    //CodSapConvenio = modelo.CodSapConvenio,
                    CodSapPedido = modelo.CodSapPedido,
                    Nombre = modelo.Nombre,
                    FechaInicial = modelo.FechaInicial,
                    FechaFinal = modelo.FechaFinal,
                    EsCodigoBarras = modelo.EsCodigoBarras,
                    ReutilizaCodigoBarras =  modelo.ReutilizaCodigoBarras,
                    EsActivo = modelo.EsActivo,
                    IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                    FechaModificacion = modelo.FechaModificacion,
                    DetalleConvenio = Utilidades.convertTable(modelo.ListaDetalle
                                                       .Select(x => new TablaGeneral
                                                       {
                                                           col1 = x.CodSapTipoProducto,
                                                           col2 = x.CodSapProducto,
                                                           col3 = x.Valor.ToString(),
                                                           col4 = x.Cantidad.ToString(),
                                                           col5 = x.CantidadxDia.ToString(),
                                                           col6 = x.CantidadDisponible.ToString(),
                                                           col7 = x.IdConvenioDetalle.ToString(),
                                                           col8 = x.Porcentaje.ToString()
                                                       })),
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_RepositorioConvenioParqueadero: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: Inserta un convenio. Retorna el id del convenio en la propiedad error
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(Convenio modelo, out string error)
        {
            error = string.Empty;

            try
            {
                error = _cnn.Query<string>("SP_InsertarConvenio", param: new
                {
                    //CodSapConvenio = modelo.CodSapConvenio.Trim(),
                    CodSapPedido = modelo.CodSapPedido.Trim(),
                    Nombre = modelo.Nombre.Trim(),
                    FechaInicial = modelo.FechaInicial,
                    FechaFinal = modelo.FechaFinal,
                    EsCodigoBarras = modelo.EsCodigoBarras,
                    ReutilizaCodigoBarras = modelo.ReutilizaCodigoBarras,
                    EsActivo = modelo.EsActivo,
                    IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                    FechaCreacion = modelo.FechaCreacion,
                    DetalleConvenio = Utilidades.convertTable(modelo.ListaDetalle
                                                       .Select(x => new TablaGeneral
                                                       {
                                                           col1 = x.CodSapTipoProducto,
                                                           col2 = x.CodSapProducto,
                                                           col3 = x.Valor.ToString(),
                                                           col4 = x.Cantidad.ToString(),
                                                           col5 = x.CantidadxDia.ToString(),
                                                           col6 = x.Porcentaje.ToString()                                                         
                                                       })),
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_RepositorioConvenio: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }


        /// <summary>
        /// EDSP: Actualizar precios convenios
        /// </summary>
        public string ActualizarPreciosConvenios(ActualizarPrecios modelo)
        {
            string strRta = string.Empty;
            try
            {
                var rta = _cnn.Query<string>(sql: "SP_ActualizarPrecios", param: new
                {
                    Todos = modelo.Todos,
                    CodSapProducto = modelo.CodSapProducto ?? "",
                    CodSapTipoProducto = modelo.CodSapTipoProducto ?? ""
                }, commandType: System.Data.CommandType.StoredProcedure);

                strRta = rta.First();
            }
            catch(Exception ex)
            {
                Utilidades.RegistrarError(ex, 
                    string.Concat(this.GetType().Name, "_", System.Reflection.MethodBase.GetCurrentMethod().Name));
            }

            return strRta;
        }

        /// <summary>
        /// EDSP: Insertar Exclusión convenio
        /// </summary>
        public string InsertarExclusionConvenio(ExclusionConvenio modelo)
        {
            string strRta = string.Empty;
            try
            {
                var rta = _cnn.Query<string>(sql: "SP_InsertarExclusionConvenio", param: new
                {
                    IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                    IdEstado = modelo.IdEstado,
                    FechaInicio= modelo.FechaInicio,
                    FechaFin = modelo.FechaFin,
                    IdConvenio = modelo.IdConvenio
                }, commandType: System.Data.CommandType.StoredProcedure);

                strRta = rta.First();
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex,
                    string.Concat(this.GetType().Name, "_", System.Reflection.MethodBase.GetCurrentMethod().Name));
                strRta = ex.Message;
            }

            return strRta;
        }

        /// <summary>
        /// EDSP: Actualizar exclusión convenio
        /// </summary>
        public int DeshabilitarExclusion(ExclusionConvenio modelo)
        {
           return  _cnn.Update(modelo);
        }

        // EDSP: Obtener todas las exclusiones
        public IEnumerable<ExclusionConvenio> ObtenerExclusionesConvenio()
        {
            return _cnn.GetList<ExclusionConvenio>();
        }

        //EDSP: Obtener exclusion por id de convenio
        public IEnumerable<ExclusionConvenio> ObtenerExclusionesPorIdConvenio(int IdConvenio)
        {
            return _cnn.GetList<ExclusionConvenio>($"WHERE IdConvenio ={IdConvenio}");
        }

        //EDSP: Obtenerlos productos por convenio
        public IEnumerable<ConvenioProducto> ObtenerProductoConvenio(int IdConvenio)
        {
            return _cnn.GetList<ConvenioProducto>($"WHERE IdConvenio = {IdConvenio}");
        }

        //EDSP: Actualizar o insertar convenio producto
        public string ActualizarProductosConvenio(IEnumerable<ConvenioProducto> modelo)
        {
            string strMensaje = string.Empty;
            var _rows = _cnn.GetList<ConvenioProducto>($"WHERE IdConvenio = '{modelo.First().IdConvenio}'");
            
            //productos de convenios eliminados
            foreach (var item in _rows.Where(x => x.IdEstado == 1))
            {
                if(!modelo.Any(x => x.CodSapProducto == item.CodSapProducto))
                {
                    item.IdEstado = 0;
                    _cnn.Update(item);
                }
            }

            try
            {
                

                foreach (var item in modelo)
                {
                    bool exist = false; ;

                    if (_rows != null && _rows.Count() > 0)
                    {
                        foreach (var item2 in _rows.Where(x => x.IdEstado == 1))
                        {
                            if (item2.CodSapProducto == item.CodSapProducto)
                            {
                                item2.Cantidad = item.Cantidad;
                                _cnn.Update(item2);
                                exist = true;
                            }
                        }
                    }
                    if (!exist && !string.IsNullOrEmpty(item.CodSapProducto))
                        _cnn.Insert<int>(item);
                }
            }catch(Exception ex)
            {
                strMensaje = ex.Message;
            }

            return strMensaje;
        }
        

        /// <summary>
        /// Obtener exclusión
        /// </summary>
        public ExclusionConvenio ObtenerExclusion(int id)
        {
            return _cnn.Get<ExclusionConvenio>(id);
        }

        /// <summary>
        /// RDSH: Retorna el detalle de un convenio por su codigo sap.
        /// EDSP: Se envia el ID del convenio para obtener el detalle del convenio 29/12/2017
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ConvenioDetalle> ObtenerDetalleConvenio(int IdConvenio)
        {

            List<ConvenioDetalle> objListaConvenioDetalle = null;

            try
            {
                objListaConvenioDetalle = _cnn.Query<ConvenioDetalle>("SP_ObtenerDetalleConvenio", param: new  {
                    IdConvenio = IdConvenio
                }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ObtenerDetalleConvenio_RepositorioConvenio");
            }

            return objListaConvenioDetalle;
        }


        #endregion

    }
}
