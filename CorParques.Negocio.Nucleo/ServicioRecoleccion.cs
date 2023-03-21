using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;
using CorParques.Transversales.Util;

namespace CorParques.Negocio.Nucleo
{

    public class ServicioRecoleccion : IServicioRecoleccion
    {

        private readonly IRepositorioRecoleccion _repositorio;
        private readonly IRepositorioDetalleRecoleccionMonetaria _repositorioDetalleMonetario;
        private readonly IRepositorioDetalleRecoleccionDocumento _repositorioDetalleDocumento;
        private readonly IRepositorioDetalleRecoleccionBrazalete _repositorioDetalleBrazalete;
        private readonly IRepositorioDetalleRecoleccionNovedad _repositorioDetalleNovedad;

        #region Constructor

        public ServicioRecoleccion(IRepositorioRecoleccion repositorio, IRepositorioDetalleRecoleccionMonetaria repositorioDetalleMonetario, IRepositorioDetalleRecoleccionDocumento repositorioDetalleDocumento, IRepositorioDetalleRecoleccionBrazalete repositorioDetalleBrazalete, IRepositorioDetalleRecoleccionNovedad repositorioDetalleNovedad)
        {
            _repositorio = repositorio;
            _repositorioDetalleMonetario = repositorioDetalleMonetario;
            _repositorioDetalleDocumento = repositorioDetalleDocumento;
            _repositorioDetalleBrazalete = repositorioDetalleBrazalete;
            _repositorioDetalleNovedad = repositorioDetalleNovedad;
        }

        #endregion

        #region Metodos

        public bool Actualizar(Recoleccion modelo, out string error)
        {
            bool blnResultado = true;
            error = string.Empty;

            try
            {
                //RDSH: Actualiza la cabecera TB_Recoleccion
                ActualizarEncabezado(modelo, out error);
                if (error.Trim().Length == 0)
                {                    
                    ActualizarDetalleMonetario(modelo);
                    ActualizarDetalleDocumentos(modelo);
                    ActualizarDetalleNovedad(modelo);
                    //if (modelo.Cierre)
                    //{
                    //    if (modelo.RecoleccionBrazalete != null)
                    //        ActualizarDetalleBoleteria(modelo);
                    //}
                }

            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_ServicioRecoleccion: ", ex.Message);
                blnResultado = false;
            }

            return blnResultado;
        }

        public bool Eliminar(Recoleccion modelo, out string error)
        {
            return _repositorio.Eliminar(modelo, out error);
        }

        public bool Insertar(Recoleccion modelo, out string error)
        {

            bool blnResultado = true;
            int intIdRecoleccion;

            try
            {

                if (modelo.IdRecoleccion <= 0)
                {
                    //Inserta cabecera.
                    _repositorio.Insertar(modelo, out error, out intIdRecoleccion);
                }
                else
                {
                    //Actualiza el usuario modificacion y la fecha de modificacion de la cabecera.
                    intIdRecoleccion = modelo.IdRecoleccion;
                    ActualizarEncabezado(modelo, out error);
                }

                //Valida si cabecera fue insertada, si lo fue, inserta el detalle monetario.
                if (error.Trim().Length <= 0 && intIdRecoleccion > 0)
                {
                    modelo.IdRecoleccion = intIdRecoleccion;
                    InsertarDetalleMonetario(modelo);
                    InsertarDetalleDocumentos(modelo);
                    InsertarDetalleNovedad(modelo);
                    if (modelo.RecoleccionBrazalete != null)
                        InsertarDetalleBoleteria(modelo);
                                     
                }
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_ServicioRecoleccion: ", ex.Message);
                blnResultado = false;
            }

            return blnResultado;

        }

        public int InsertaGeneral(Recoleccion modelo, out string error)
        {

            bool blnResultado = true;
            int intIdRecoleccion = 0;

            try
            {

                if (modelo.IdRecoleccion <= 0)
                {
                    //Inserta cabecera.
                    _repositorio.Insertar(modelo, out error, out intIdRecoleccion);
                }
                else
                {
                    //Actualiza el usuario modificacion y la fecha de modificacion de la cabecera.
                    intIdRecoleccion = modelo.IdRecoleccion;
                    ActualizarEncabezado(modelo, out error);
                }

                //Valida si cabecera fue insertada, si lo fue, inserta el detalle monetario.
                if (error.Trim().Length <= 0 && intIdRecoleccion > 0)
                {
                    modelo.IdRecoleccion = intIdRecoleccion;
                    InsertarDetalleMonetarioGenaral(modelo);
                    InsertarDetalleDocumentosGeneral(modelo);
                    InsertarDetalleNovedadGeneral(modelo);
                    if (modelo.RecoleccionBrazalete != null)
                        InsertarDetalleBoleteriaGeneral(modelo);

                }
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_ServicioRecoleccion: ", ex.Message);
                blnResultado = false;
            }

            return intIdRecoleccion;

        }

        public Recoleccion ObtenerPorId(int Id)
        {

            Recoleccion objRecoleccion = null;
            IEnumerable<DetalleRecoleccionMonetaria> objDetalleRecoleccionMonetaria;
            IEnumerable<DetalleRecoleccionDocumento> objDetalleRecoleccionDocumento;

            try
            {
                objRecoleccion = _repositorio.ObtenerPorId(Id);
                objDetalleRecoleccionMonetaria = _repositorioDetalleMonetario.ObtenerPorIDRecoleccion(Id);
                objDetalleRecoleccionDocumento = _repositorioDetalleDocumento.ObtenerPorIDRecoleccion(Id);

                if (objDetalleRecoleccionMonetaria != null)
                {
                    objRecoleccion.RecoleccionBase = objDetalleRecoleccionMonetaria.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Base);
                    objRecoleccion.RecoleccionCorte = objDetalleRecoleccionMonetaria.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Corte);
                }
                if (objDetalleRecoleccionDocumento != null)
                {
                    objRecoleccion.RecoleccionVoucher = objDetalleRecoleccionDocumento.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Voucher);
                    objRecoleccion.RecoleccionDocumentos = objDetalleRecoleccionDocumento.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Documentos);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objRecoleccion;
        }

        /// <summary>
        /// RDSH: Busca la recoleccion activa y devuelve el header con sus respectivos detalles.
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <param name="IdPunto"></param>
        /// <param name="Cierre"></param>
        /// <param name="IdEstado"></param>
        /// <returns></returns>
        public Recoleccion ObtenerRecoleccionActiva(int IdUsuario, int IdPunto, bool Cierre, int IdEstado)
        {
            Recoleccion objRecoleccion = null;
            IEnumerable<DetalleRecoleccionMonetaria> objDetalleRecoleccionMonetaria;
            IEnumerable<DetalleRecoleccionDocumento> objDetalleRecoleccionDocumento;
            IEnumerable<DetalleRecoleccionBrazalete> objDetalleRecoleccionBrazalete;
            IEnumerable<DetalleRecoleccionNovedad> objDetalleRecoleccionNovedad;

            try
            {
                objRecoleccion = _repositorio.ObtenerRecoleccionActiva(IdUsuario, IdPunto, Cierre, IdEstado);

                if (objRecoleccion != null)
                {
                    objDetalleRecoleccionMonetaria = _repositorioDetalleMonetario.ObtenerPorIDRecoleccion(objRecoleccion.IdRecoleccion);
                    objDetalleRecoleccionDocumento = _repositorioDetalleDocumento.ObtenerPorIDRecoleccion(objRecoleccion.IdRecoleccion);
                    objDetalleRecoleccionNovedad = _repositorioDetalleNovedad.ObtenerPorIDRecoleccion(objRecoleccion.IdRecoleccion);

                    if (objDetalleRecoleccionMonetaria != null)
                    {
                        if (Cierre)
                        {
                            //Carga el detalle del efectivo para el cierre.
                            objRecoleccion.RecoleccionBase = objDetalleRecoleccionMonetaria.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.CierreTaquilla);
                            if (objDetalleRecoleccionMonetaria.Where(x => x.Tipo == "Billete" && x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.CierreTaquilla).Count() > 0)
                            {
                                objRecoleccion.SobreBilletesBase = objDetalleRecoleccionMonetaria.Where(x => x.Tipo == "Billete" && x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.CierreTaquilla).FirstOrDefault().NumeroSobre;
                            }
                            if (objDetalleRecoleccionMonetaria.Where(x => x.Tipo == "Moneda" && x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.CierreTaquilla).Count() > 0)
                            {
                                objRecoleccion.SobreMonedasBase = objDetalleRecoleccionMonetaria.Where(x => x.Tipo == "Moneda" && x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.CierreTaquilla).FirstOrDefault().NumeroSobre;
                            }                            
                        }
                        else
                        {
                            //Carga el detalle de la base para recoleccion.
                            objRecoleccion.RecoleccionBase = objDetalleRecoleccionMonetaria.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Base);
                            if (objDetalleRecoleccionMonetaria.Where(x => x.Tipo == "Billete" && x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Base).Count() > 0)
                            {
                                objRecoleccion.SobreBilletesBase = objDetalleRecoleccionMonetaria.Where(x => x.Tipo == "Billete" && x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Base).FirstOrDefault().NumeroSobre;
                            }
                            if (objDetalleRecoleccionMonetaria.Where(x => x.Tipo == "Moneda" && x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Base).Count() > 0)
                            {
                                objRecoleccion.SobreMonedasBase = objDetalleRecoleccionMonetaria.Where(x => x.Tipo == "Moneda" && x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Base).FirstOrDefault().NumeroSobre;
                            }                            
                        }
                        //Carga el detalle de corte para recoleccion.                        
                        objRecoleccion.RecoleccionCorte = objDetalleRecoleccionMonetaria.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Corte);
                        if (objDetalleRecoleccionMonetaria.Where(x => x.Tipo == "Billete" && x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Corte).Count() > 0)
                        {
                            objRecoleccion.SobreBilletesBase = objDetalleRecoleccionMonetaria.Where(x => x.Tipo == "Billete" && x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Corte).FirstOrDefault().NumeroSobre;
                        }
                        if (objDetalleRecoleccionMonetaria.Where(x => x.Tipo == "Moneda" && x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Corte).Count() > 0)
                        {
                            objRecoleccion.SobreMonedasBase = objDetalleRecoleccionMonetaria.Where(x => x.Tipo == "Moneda" && x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Corte).FirstOrDefault().NumeroSobre;
                        }                        
                    }
                    //Carga el detalle de documentos tipo voucher y documentos
                    if (objDetalleRecoleccionDocumento != null)
                    {
                        objRecoleccion.RecoleccionVoucher = objDetalleRecoleccionDocumento.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Voucher);
                        objRecoleccion.RecoleccionDocumentos = objDetalleRecoleccionDocumento.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Documentos);
                        if (objDetalleRecoleccionDocumento.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Voucher).Count() > 0)
                        {
                            objRecoleccion.SobreVoucher = objDetalleRecoleccionDocumento.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Voucher).FirstOrDefault().NumeroSobre;
                        }
                        if (objDetalleRecoleccionDocumento.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Documentos).Count() > 0)
                        {
                            objRecoleccion.SobreDocumentos = objDetalleRecoleccionDocumento.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Documentos).FirstOrDefault().NumeroSobre;
                        }
                        
                    }
                    if (Cierre)
                    {
                        //Aqui se esta Insertando los datos de ImpresionEnLinea
                        var lista = _repositorio.ObtenerBrazaletesRestantes(IdUsuario, IdPunto);
                        List<DetalleRecoleccionBrazalete> dr = new List<DetalleRecoleccionBrazalete>();
                    

                        //Carga el detalle de los brazaletes al objeto de recoleccion
                        //objDetalleRecoleccionBrazalete = lista.Select(x => new DetalleRecoleccionBrazalete { TipoBrazalete = x.TipoBrazalete, Asignados=x.Asignados,TotalVendidos = x.TotalVendidos, CantidadTaquillero=x.EnCaja});  //_repositorioDetalleBrazalete.ObtenerPorIDRecoleccion(objRecoleccion.IdRecoleccion);

                        //Este dato fue el que se modifico para integrar impresion en linea
                        foreach (DetalleRecoleccionBrazalete i in lista.Select(x => new DetalleRecoleccionBrazalete { TipoBrazalete = x.TipoBrazalete, Asignados = x.Asignados, TotalVendidos = x.TotalVendidos, CantidadTaquillero = x.EnCaja }))
                        {
                            dr.Add(i);
                        }
                        objDetalleRecoleccionBrazalete = dr.Select(x => new DetalleRecoleccionBrazalete
                        {
                            TipoBrazalete = x.TipoBrazalete,
                            TotalVendidos = x.TotalVendidos,
                            Asignados = x.Asignados,
                            IdTipoBrazalete = x.IdTipoBrazalete, 
                            CantidadTaquillero = x.CantidadTaquillero,
                        });
                        //Hasta aca se modifico

                        if (objDetalleRecoleccionBrazalete != null)
                        {   
                            objRecoleccion.RecoleccionBrazalete = objDetalleRecoleccionBrazalete;                            
                        }
                            
                    }
                    //Carga las novedades al objeto de recoleccion.
                    if (objDetalleRecoleccionNovedad != null)
                    {
                        objRecoleccion.RecoleccionNovedad = objDetalleRecoleccionNovedad;
                        if (objDetalleRecoleccionNovedad.Count() > 0)
                        {
                            objRecoleccion.SobreNovedad = objDetalleRecoleccionNovedad.FirstOrDefault().NumeroSobre;
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objRecoleccion;
        }

        public IEnumerable<MediosPagoFactura> ObtenerDocumentosRecoleccion(int IdUsuario)
        {
            return _repositorio.ObtenerDocumentosRecoleccion(IdUsuario);
        }

        /// <summary>
        /// RDSH: Inserta el detalle de la recoleccion monetaria.
        /// </summary>
        /// <param name="modelo"></param>
        private void InsertarDetalleMonetario(Recoleccion modelo)
        {

            IEnumerable<DetalleRecoleccionMonetaria> DetalleMonetario = null;
            string strError = string.Empty;

            try
            {
                if (modelo.RecoleccionBase != null && modelo.RecoleccionCorte != null)
                {
                    DetalleMonetario = modelo.RecoleccionBase.Concat(modelo.RecoleccionCorte);
                }
                else if (modelo.RecoleccionBase != null)
                {
                    DetalleMonetario = modelo.RecoleccionBase;
                }
                else if (modelo.RecoleccionCorte != null)
                {
                    DetalleMonetario = modelo.RecoleccionCorte;
                }

                if (DetalleMonetario != null)
                {
                    //Inserta detalle monetario.    
                    if (DetalleMonetario.Count() > 0)
                    {
                        _repositorioDetalleMonetario.Insertar(modelo.IdRecoleccion, modelo.IdEstado, modelo.IdUsuarioCreacion, DetalleMonetario, out strError);

                        if (strError.Trim().Length > 0)
                        {
                            throw new ArgumentException(string.Concat("Error en InsertarDetalleMonetario_ServicioRecoleccion: ", strError));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DetalleMonetario = null;
            }
        }

        private void InsertarDetalleMonetarioGenaral(Recoleccion modelo)
        {

            string strError = string.Empty;

            try
            {            
                    
                _repositorioDetalleMonetario.InsertarRecoleccionGeneral(modelo.IdRecoleccion, modelo.IdEstado, modelo.IdUsuarioCreacion, float.Parse(modelo.ValorRecoleccionBase),
                                                                        float.Parse(modelo.ValorRecoleccionCorte), modelo.IdUsuarioSupervisor, out strError);

                if (strError.Trim().Length > 0)
                {
                    throw new ArgumentException(string.Concat("Error en InsertarDetalleMonetario_ServicioRecoleccion: ", strError));
                }
                    

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// RDSH: Inserta el detalle de la recoleccion de documentos.
        /// </summary>
        /// <param name="modelo"></param>
        private void InsertarDetalleDocumentos(Recoleccion modelo)
        {

            IEnumerable<DetalleRecoleccionDocumento> DetalleDocumentos = null;
            string strError = string.Empty;

            try
            {
                if (modelo.RecoleccionVoucher != null && modelo.RecoleccionDocumentos != null)
                {
                    DetalleDocumentos = modelo.RecoleccionVoucher.Concat(modelo.RecoleccionDocumentos);
                }
                else if (modelo.RecoleccionVoucher != null)
                {
                    DetalleDocumentos = modelo.RecoleccionVoucher;
                }
                else if (modelo.RecoleccionDocumentos != null)
                {
                    DetalleDocumentos = modelo.RecoleccionDocumentos;
                }

                if (DetalleDocumentos != null)
                {
                    //Inserta detalle documentos.  
                    if (DetalleDocumentos.Count() > 0)
                    {
                        _repositorioDetalleDocumento.Insertar(modelo.IdRecoleccion, modelo.IdEstado, modelo.IdUsuarioCreacion, DetalleDocumentos, out strError);

                        if (strError.Trim().Length > 0)
                        {
                            throw new ArgumentException(string.Concat("Error en InsertarDetalleDocumentos_ServicioRecoleccion: ", strError));
                        }
                    }
                    else
                    {
                        //Si no marcaron nada verifica que no queden asociados a la recoleccion que se esta editando.
                        EliminarDetalleRecoleccion(modelo.IdRecoleccion, (int)Transversales.Util.Enumerador.TipoRecoleccion.Voucher);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DetalleDocumentos = null;
            }
        }

        /// <summary>
        /// RDSH: Inserta el detalle de la recoleccion de documentos.
        /// </summary>
        /// <param name="modelo"></param>
        private void InsertarDetalleDocumentosGeneral(Recoleccion modelo)
        {

            IEnumerable<DetalleRecoleccionDocumento> DetalleDocumentos = null;
            string strError = string.Empty;

            try
            {
                if (modelo.RecoleccionVoucher != null && modelo.RecoleccionDocumentos != null)
                {
                    DetalleDocumentos = modelo.RecoleccionVoucher.Concat(modelo.RecoleccionDocumentos);
                }
                else if (modelo.RecoleccionVoucher != null)
                {
                    DetalleDocumentos = modelo.RecoleccionVoucher;
                }
                else if (modelo.RecoleccionDocumentos != null)
                {
                    DetalleDocumentos = modelo.RecoleccionDocumentos;
                }

                if (DetalleDocumentos != null)
                {
                    //Inserta detalle documentos.  
                    if (DetalleDocumentos.Count() > 0)
                    {
                        _repositorioDetalleDocumento.InsertarGeneral(modelo.IdRecoleccion, modelo.IdEstado, modelo.IdUsuarioCreacion, DetalleDocumentos, modelo.IdUsuarioSupervisor, out strError);

                        if (strError.Trim().Length > 0)
                        {
                            throw new ArgumentException(string.Concat("Error en InsertarDetalleDocumentos_ServicioRecoleccion: ", strError));
                        }
                    }
                    else
                    {
                        //Si no marcaron nada verifica que no queden asociados a la recoleccion que se esta editando.
                        EliminarDetalleRecoleccion(modelo.IdRecoleccion, (int)Transversales.Util.Enumerador.TipoRecoleccion.Voucher);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DetalleDocumentos = null;
            }
        }


        /// <summary>
        /// RDSH: Inserta el detalle del cierre de boleteria.
        /// </summary>
        /// <param name="modelo"></param>
        private void InsertarDetalleBoleteria(Recoleccion modelo)
        {

            string strError = string.Empty;

            try
            {
                //Inserta detalle documentos.  
                if (modelo.RecoleccionBrazalete.Count() > 0)
                {
                    _repositorioDetalleBrazalete.Insertar(modelo.IdRecoleccion, modelo.IdEstado, modelo.IdUsuarioCreacion, modelo.RecoleccionBrazalete, out strError);

                    if (strError.Trim().Length > 0)
                    {
                        throw new ArgumentException(string.Concat("Error en InsertarDetalleBoleteria_ServicioRecoleccion: ", strError));
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }

        private void InsertarDetalleBoleteriaGeneral(Recoleccion modelo)
        {

            string strError = string.Empty;

            try
            {
                //Inserta detalle documentos.  
                if (modelo.RecoleccionBrazalete.Count() > 0)
                {
                    _repositorioDetalleBrazalete.InsertarGeneral(modelo.IdRecoleccion, modelo.IdEstado, modelo.IdUsuarioCreacion, modelo.RecoleccionBrazalete, modelo.IdUsuarioSupervisor, out strError);

                    if (strError.Trim().Length > 0)
                    {
                        throw new ArgumentException(string.Concat("Error en InsertarDetalleBoleteria_ServicioRecoleccion: ", strError));
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// RDSH: Inserta el detalle de la recoleccion de novedades.
        /// </summary>
        /// <param name="modelo"></param>
        private void InsertarDetalleNovedad(Recoleccion modelo)
        {

            IEnumerable<DetalleRecoleccionNovedad> DetalleNovedad = null;
            string strError = string.Empty;

            try
            {

                DetalleNovedad = modelo.RecoleccionNovedad;

                if (DetalleNovedad != null)
                {
                    //Inserta detalle novedades.  
                    if (DetalleNovedad.Count() > 0)
                    {
                        _repositorioDetalleNovedad.Insertar(modelo.IdRecoleccion, modelo.IdEstado, modelo.IdUsuarioCreacion, DetalleNovedad, out strError);

                        if (strError.Trim().Length > 0)
                        {
                            throw new ArgumentException(string.Concat("Error en InsertarDetalleNovedad_ServicioRecoleccion: ", strError));
                        }
                    }
                    else
                    {
                        //Si no marcaron nada verifica que no queden asociados a la recoleccion que se esta editando.
                        EliminarDetalleRecoleccion(modelo.IdRecoleccion, (int)Transversales.Util.Enumerador.TipoRecoleccion.Novedad);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DetalleNovedad = null;
            }
        }

        /// <summary>
        /// GALD: Inserta el detalle de la recoleccion de novedades en un solo paso .
        /// </summary>
        /// <param name="modelo"></param>
        private void InsertarDetalleNovedadGeneral(Recoleccion modelo)
        {

            IEnumerable<DetalleRecoleccionNovedad> DetalleNovedad = null;
            string strError = string.Empty;

            try
            {

                DetalleNovedad = modelo.RecoleccionNovedad;

                if (DetalleNovedad != null)
                {
                    //Inserta detalle novedades.  
                    if (DetalleNovedad.Count() > 0)
                    {
                        _repositorioDetalleNovedad.InsertarGeneral(modelo.IdRecoleccion, modelo.IdEstado, modelo.IdUsuarioCreacion, DetalleNovedad, modelo.IdUsuarioSupervisor, out strError);

                        if (strError.Trim().Length > 0)
                        {
                            throw new ArgumentException(string.Concat("Error en InsertarDetalleNovedad_ServicioRecoleccion: ", strError));
                        }
                    }
                    else
                    {
                        //Si no marcaron nada verifica que no queden asociados a la recoleccion que se esta editando.
                        EliminarDetalleRecoleccion(modelo.IdRecoleccion, (int)Transversales.Util.Enumerador.TipoRecoleccion.Novedad);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DetalleNovedad = null;
            }
        }


        /// <summary>
        //RDSH: Retorna si se muestra la recoleccion base, corte y los topes para cada una de estas.
        /// </summary>
        /// <param name="intIdUsuario"></param>
        /// <param name="intIdPunto"></param>
        /// <returns></returns>
        public RecoleccionAuxliar ObtenerTopesRecoleccion(int IdUsuario, int IdPunto)
        {
            return _repositorio.ObtenerTopesRecoleccion(IdUsuario, IdPunto);
        }

        /// <summary>
        /// RDSH: Retorna la cantidad maxima de dinero que se puede recolectar para cierre.
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <param name="IdPunto"></param>
        /// <returns></returns>
        public RecoleccionAuxliar ObtenerTopesCierreTaquilla(int IdUsuario, int IdPunto)
        {
            return _repositorio.ObtenerTopesCierreTaquilla(IdUsuario, IdPunto);
        }

        ///RDSH: Retorna la cantidad de brazaletes restantes.
        public IEnumerable<CierreBrazalete> ObtenerBrazaletesRestantes(int intIdUsuario, int intIdPunto)
        {
            return _repositorio.ObtenerBrazaletesRestantes(intIdUsuario, intIdPunto);
        }

        private bool ActualizarEncabezado(Recoleccion modelo, out string error)
        {
            return _repositorio.Actualizar(modelo, out error);
        }

        public IEnumerable<NotificacionAlerta> ObtenerNotificaciones()
        {
            return _repositorio.ObtenerNotificaciones();
        }

        /// <summary>
        /// RDSH: Actualiza el detalle monetario de una recoleccion.
        /// </summary>
        /// <param name="modelo"></param>
        private void ActualizarDetalleMonetario(Recoleccion modelo)
        {
            IEnumerable<DetalleRecoleccionMonetaria> DetalleMonetario = null;
            string strError = string.Empty;

            try
            {
                if (modelo.RecoleccionBase != null && modelo.RecoleccionCorte != null)
                {
                    DetalleMonetario = modelo.RecoleccionBase.Concat(modelo.RecoleccionCorte);
                }
                else if (modelo.RecoleccionBase != null)
                {
                    DetalleMonetario = modelo.RecoleccionBase;
                }
                else if (modelo.RecoleccionCorte != null)
                {
                    DetalleMonetario = modelo.RecoleccionCorte;
                }

                if (DetalleMonetario != null)
                {
                    //Actuliza detalle monetario.    
                    if (DetalleMonetario.Count() > 0)
                    {
                        _repositorioDetalleMonetario.Actualizar(modelo.IdRecoleccion, modelo.IdEstado, modelo.IdUsuarioModificacion, DetalleMonetario, out strError);

                        if (strError.Trim().Length > 0)
                        {
                            throw new ArgumentException(string.Concat("Error en ActualizarDetalleMonetario_ServicioRecoleccion: ", strError));
                        }
                    }
                }            

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DetalleMonetario = null;
            }
        }

        /// <summary>
        /// RDSH: Actualiza el detalle de una recoleccion de documentos.
        /// </summary>
        /// <param name="modelo"></param>
        private void ActualizarDetalleDocumentos(Recoleccion modelo)
        {
            IEnumerable<DetalleRecoleccionDocumento> DetalleDocumentos = null;
            string strError = string.Empty;

            try
            {
                if (modelo.RecoleccionVoucher != null && modelo.RecoleccionDocumentos != null)
                {
                    DetalleDocumentos = modelo.RecoleccionVoucher.Concat(modelo.RecoleccionDocumentos);
                }
                else if (modelo.RecoleccionVoucher != null)
                {
                    DetalleDocumentos = modelo.RecoleccionVoucher;
                }
                else if (modelo.RecoleccionDocumentos != null)
                {
                    DetalleDocumentos = modelo.RecoleccionDocumentos;
                }

                if (DetalleDocumentos != null)
                {
                    //Actualiza detalle documentos.  
                    if (DetalleDocumentos.Count() > 0)
                    {
                        _repositorioDetalleDocumento.Actualizar(modelo.IdRecoleccion, modelo.IdEstado, modelo.IdUsuarioModificacion, DetalleDocumentos, out strError);

                        if (strError.Trim().Length > 0)
                        {
                            throw new ArgumentException(string.Concat("Error en ActualizarDetalleDocumentos_ServicioRecoleccion: ", strError));
                        }
                    }               
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DetalleDocumentos = null;
            }
        }

        /// <summary>
        /// RDSH: Actualiza el detalle de una recoleccion de brazaletes (boleteria).
        /// </summary>
        /// <param name="modelo"></param>
        private void ActualizarDetalleBoleteria(Recoleccion modelo)
        {
            string strError = string.Empty;

            try
            {
                //Actualiza detalle boleteria.  
                if (modelo.RecoleccionBrazalete.Count() > 0)
                {
                    _repositorioDetalleBrazalete.Actualizar(modelo.IdRecoleccion, modelo.IdEstado, modelo.IdUsuarioModificacion, modelo.RecoleccionBrazalete, out strError);

                    if (strError.Trim().Length > 0)
                    {
                        throw new ArgumentException(string.Concat("Error en ActualizarDetalleBoleteria_ServicioRecoleccion: ", strError));
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// RDSH: Actualiza el detalle de una recoleccion de novedad.
        /// </summary>
        /// <param name="modelo"></param>
        private void ActualizarDetalleNovedad(Recoleccion modelo)
        {
            IEnumerable<DetalleRecoleccionNovedad> DetalleNovedad = null;
            string strError = string.Empty;

            try
            {

                DetalleNovedad = modelo.RecoleccionNovedad;
                if (DetalleNovedad != null)
                {
                    //Actualiza detalle novedades.  
                    if (DetalleNovedad.Count() > 0)
                    {
                        _repositorioDetalleNovedad.Actualizar(modelo.IdRecoleccion, modelo.IdEstado, modelo.IdUsuarioModificacion, DetalleNovedad, out strError);

                        if (strError.Trim().Length > 0)
                        {
                            throw new ArgumentException(string.Concat("Error en ActualizarDetalleNovedad_ServicioRecoleccion: ", strError));
                        }
                    }
                }            
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DetalleNovedad = null;
            }
        }

        /// <summary>
        /// RDSH: Consulta los puntos que tienen recoleccion segun el estado y si la recoleccion es de cierre o no.
        /// </summary>
        /// <param name="intIdEstado"></param>
        /// <param name="blnCierre"></param>
        /// <returns></returns>
        public IEnumerable<TipoGeneralValor> ObtenerPuntosRecoleccion(int intIdEstado, bool blnCierre)
        {
            return _repositorio.ObtenerPuntosRecoleccion(intIdEstado, blnCierre);
        }

        /// <summary>
        /// RDSH: Consulta los taquilleros que tienen alistamiento de cierre o de recoleccion, esto para el proceso de entrega a supervisor.
        /// </summary>
        /// <param name="intIdEstado"></param>
        /// <param name="blnCierre"></param>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerTaquillerosConRecoleccion(int intIdEstado, bool blnCierre)
        {
            return _repositorio.ObtenerTaquillerosConRecoleccion(intIdEstado, blnCierre);
        }

        //RDSH: Retorna las novedades pendientes de recolección.
        public IEnumerable<NovedadArqueo> ObtenerNovedadesRecoleccion(int IdUsuario)
        {
            return _repositorio.ObtenerNovedadesRecoleccion(IdUsuario);
        }

        /// <summary>
        /// RDSH: Borrado del detalle de las recolecciones al editar una recoleccion y desmarcar los voucher, documentos, novedad.
        /// </summary>
        /// <param name="intIdRecoleccion"></param>
        /// <param name="intIdTipoRecoleccion"></param>
        /// <returns></returns>
        private bool EliminarDetalleRecoleccion(int intIdRecoleccion, int intIdTipoRecoleccion)
        {
            return _repositorio.EliminarDetalleRecoleccion(intIdRecoleccion, intIdTipoRecoleccion);
        }

        
        /// <summary>
        /// RDSH: Retorna todas las recolecciones activas.
        /// </summary>
        /// <param name="intIdUsuario"></param>
        /// <param name="intIdPunto"></param>
        /// <param name="blnCierre"></param>
        /// <param name="IdEstado"></param>
        /// <returns></returns>
        public IEnumerable<Recoleccion> ObtenerRecoleccionesActivas(int intIdUsuario, int intIdPunto, bool blnCierre, int IdEstado)
        {
            IEnumerable<Recoleccion> objRecoleccion = null;
            List<Recoleccion> objRecoleccionRetorno = new List<Recoleccion>();
            IEnumerable<DetalleRecoleccionMonetaria> objDetalleRecoleccionMonetaria;
            IEnumerable<DetalleRecoleccionDocumento> objDetalleRecoleccionDocumento;
            IEnumerable<DetalleRecoleccionBrazalete> objDetalleRecoleccionBrazalete;
            IEnumerable<DetalleRecoleccionNovedad> objDetalleRecoleccionNovedad;

            try
            {
                objRecoleccion = _repositorio.ObtenerRecoleccionesActivas(intIdUsuario, intIdPunto, blnCierre, IdEstado);

                if (objRecoleccion != null)
                {
                    foreach (Recoleccion objRecoleccionFor in objRecoleccion)
                    {
                        objDetalleRecoleccionMonetaria = _repositorioDetalleMonetario.ObtenerPorIDRecoleccion(objRecoleccionFor.IdRecoleccion);
                        objDetalleRecoleccionDocumento = _repositorioDetalleDocumento.ObtenerPorIDRecoleccion(objRecoleccionFor.IdRecoleccion);
                        objDetalleRecoleccionNovedad = _repositorioDetalleNovedad.ObtenerPorIDRecoleccion(objRecoleccionFor.IdRecoleccion);

                        if (objDetalleRecoleccionMonetaria != null)
                        {
                            if (blnCierre)
                            {
                                objRecoleccionFor.RecoleccionBase = objDetalleRecoleccionMonetaria.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.CierreTaquilla);
                            }
                            else
                            {
                                objRecoleccionFor.RecoleccionBase = objDetalleRecoleccionMonetaria.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Base);
                            }
                            objRecoleccionFor.RecoleccionCorte = objDetalleRecoleccionMonetaria.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Corte);
                        }
                        if (objDetalleRecoleccionDocumento != null)
                        {
                            objRecoleccionFor.RecoleccionVoucher = objDetalleRecoleccionDocumento.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Voucher);
                            objRecoleccionFor.RecoleccionDocumentos = objDetalleRecoleccionDocumento.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Documentos);
                        }
                        if (blnCierre)
                        {
                            objDetalleRecoleccionBrazalete = _repositorioDetalleBrazalete.ObtenerPorIDRecoleccion(objRecoleccionFor.IdRecoleccion);
                            if (objDetalleRecoleccionBrazalete != null)
                                objRecoleccionFor.RecoleccionBrazalete = objDetalleRecoleccionBrazalete;
                        }
                        if (objDetalleRecoleccionNovedad != null)
                            objRecoleccionFor.RecoleccionNovedad = objDetalleRecoleccionNovedad;
                        
                        objRecoleccionRetorno.Add(objRecoleccionFor);
                        objDetalleRecoleccionMonetaria = null;
                        objDetalleRecoleccionDocumento = null;
                        objDetalleRecoleccionBrazalete = null;
                        objDetalleRecoleccionNovedad = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objRecoleccionRetorno;
        }
        
        #endregion
    }
}
