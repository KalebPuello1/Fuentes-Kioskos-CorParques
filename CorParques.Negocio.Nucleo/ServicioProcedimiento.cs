using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioProcedimiento : IServicioProcedimiento
    {
        IRepositorioProcedimiento _repositorio;
        //IRepositorioPaciente _repositorioPaciente;
        IRepositorioCategoriaAtencion _repositorioCategoriaAtencion;
        IRepositorioTipoPaciente _repositorioTipoPaciente;

        //public ServicioProcedimiento(IRepositorioProcedimiento repositorio, IRepositorioPaciente repositorioPaciente)

        #region Metodos

        /// <summary>
        /// RDSH: Formatea la fecha a un formato valido para enviar a guardar.
        /// </summary>
        /// <param name="strFecha"></param>
        /// <returns></returns>
        public DateTime FechaReporte(string strFecha, bool blnFechaInicial = true)
        {
            DateTime datFechaIncidente;            
            string[] strSplit;
            string strFechaArmada = string.Empty;

            try
            {
                strFecha = strFecha.Replace("_", "/");
                strSplit = strFecha.Split('/');
                if (blnFechaInicial)
                {
                    strFechaArmada = string.Join("/", strSplit[2], strSplit[1], strSplit[0]);
                    strFechaArmada = string.Concat(strFechaArmada, " 00:00");
                }
                else
                {
                    strFechaArmada = string.Join("/", strSplit[2], strSplit[1], strSplit[0]);
                    strFechaArmada = string.Concat(strFechaArmada, " 23:59");
                }

                datFechaIncidente = DateTime.Parse(strFechaArmada);
            }
            catch (Exception)
            {
                datFechaIncidente = DateTime.Now;
            }

            return datFechaIncidente;

        }


        /// <summary>
        /// RDSH: Formatea la fecha a un formato valido para enviar a generar el reporte..
        /// </summary>
        /// <param name="strFecha"></param>
        /// <returns></returns>
        public string FechaReporteAAAAMMDD(string strFecha)
        {
            
            string[] strSplit;
            string strFechaArmada = string.Empty;

            try
            {
                if (strFecha.Trim().Length > 0 && strFecha != "0")
                {
                    strFecha = strFecha.Replace("_", "/");
                    strSplit = strFecha.Split('/');
                    strFechaArmada = string.Join("/", strSplit[2], strSplit[1], strSplit[0]);
                }              
                
            }
            catch (Exception)
            {
                strFechaArmada = string.Empty;
            }

            return strFechaArmada;

        }

        #endregion

        public ServicioProcedimiento(IRepositorioProcedimiento repositorio, IRepositorioCategoriaAtencion repositorioCategoriaAtencion, IRepositorioTipoPaciente repositorioTipoPaciente)
        {
            _repositorio = repositorio;
            _repositorioCategoriaAtencion = repositorioCategoriaAtencion;
            _repositorioTipoPaciente = repositorioTipoPaciente;
            //_repositorioPaciente = repositorioPaciente;
        }

        public bool Actualizar(Procedimiento modelo, out string error)
        {
            
            bool blnRetorno = true;

            try
            {

                //modelo.IdPaciente = modelo.objPaciente.IdPaciente;
                //_repositorioPaciente.Actualizar(modelo.objPaciente, out error);
                //if (error.Trim().Length > 0)
                //{
                //    throw new ArgumentException(error);
                //}

                _repositorio.Actualizar(modelo, out error);

                if (error.Trim().Length > 0)
                {
                    throw new ArgumentException(error);
                }

            }
            catch (Exception ex)
            {
                blnRetorno = false;
                error = string.Concat("Ocurrio error en Procedimiento_Actualizar: ", ex.Message);
            }      

            return blnRetorno;
        }

        public bool Eliminar(Procedimiento modelo, out string error)
        {
            throw new NotImplementedException();
        }

        public bool Insertar(Procedimiento modelo, out string error)
        {

            Paciente objPaciente = new Paciente();
            bool blnRetorno = true;

            try
            {
                //objPaciente = _repositorioPaciente.ObtenerPorTipoDocumento(modelo.objPaciente.IdTipoDocumento, modelo.objPaciente.Documento);
                ////Si encuentra al paciente lo actualiza.
                //if (objPaciente != null)
                //{
                //    modelo.IdPaciente = objPaciente.IdPaciente;
                //    modelo.objPaciente.IdPaciente = objPaciente.IdPaciente;
                //    _repositorioPaciente.Actualizar(modelo.objPaciente, out error);
                //    if (error.Trim().Length > 0)
                //    {
                //        throw new ArgumentException(error);
                //    }
                //}
                //else
                //{
                //    //Si no encuentra al paciente lo inserta.
                //    _repositorioPaciente.Insertar(modelo.objPaciente, out error);
                //    if (error.Trim().Length > 0)
                //    {
                //        throw new ArgumentException(error);
                //    }
                //    objPaciente = _repositorioPaciente.ObtenerPorTipoDocumento(modelo.objPaciente.IdTipoDocumento, modelo.objPaciente.Documento);
                //    modelo.IdPaciente = objPaciente.IdPaciente;
                //    modelo.objPaciente = objPaciente;
                //}

                _repositorio.Insertar(modelo, out error);

                if (error.Trim().Length > 0)
                {
                    throw new ArgumentException(error);
                }

            }
            catch (Exception ex)
            {
                blnRetorno = false;
                error = string.Concat("Ocurrio error en Procedimiento_Insertar: ", ex.Message);
            }
            finally
            {
                objPaciente = null;
            }

            return blnRetorno;

        }

        public IEnumerable<Procedimiento> ObtenerLista()
        {
            return _repositorio.ObtenerLista();
        }

        public Procedimiento ObtenerPorId(int Id)
        {
            return _repositorio.ObtenerPorId(Id);
        }

        public IEnumerable<Procedimiento> ReporteAtenciones(int IdTipoDocumentoPaciente, int IdCategoriaAtencion, int IdTipoPaciente, string strFechaInicial, string strFechaFinal, int IdProcedimiento, int IdZonaArea, int IdUbicacion)
        { 
            return _repositorio.ReporteAtenciones(IdTipoDocumentoPaciente, IdCategoriaAtencion,IdTipoPaciente, FechaReporteAAAAMMDD(strFechaInicial), FechaReporteAAAAMMDD(strFechaFinal), IdProcedimiento, IdZonaArea, IdUbicacion);

        }

        /// <summary>
        /// RDSH: Obtiene la categoria de la atencion para centro medico.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerCategoriaAtencion()
        {
            return _repositorioCategoriaAtencion.ObtenerLista().Where(x => x.IdEstado == 1).Select(x => new TipoGeneral { Id = x.IdCategoriaAtencion, Nombre = x.Nombre });
        }

        /// <summary>
        /// RDSH: Obtiene la lista de tipos de paciente para centro medico.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerTipoPaciente()
        {
            return _repositorioTipoPaciente.ObtenerLista().Where(x => x.IdEstado == 1).Select(x => new TipoGeneral { Id = x.IdTipoPaciente, Nombre = x.Nombre });
        }

        //public string Crear(Procedimiento modelo)
        //{
        //   return _repositorio.Crear(modelo);
        //}

        //public Procedimiento Obtener(int id)
        //{
        //    var procedimiento = _repositorio.ObtenerJoin(id);
        //    return procedimiento;
        //}

        //public IEnumerable<Procedimiento> ObtenerTodos()
        //{
        //    return _repositorio.ObtenerListaJoin();
        //}

        //public IEnumerable<Paciente> ObtenerPacientes()
        //{
        //    return _repositorioPaciente.ObtenerLista();
        //}
    }
}
