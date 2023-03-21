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

	public class ServicioCierrePunto : IServicioCierrePuntos
	{

        private readonly IRepositorioCierrePunto _repositorio;

        public ServicioCierrePunto(IRepositorioCierrePunto repositorio)
        {
            _repositorio = repositorio;
        }


        #region Constructor

        public bool Actualizar(CierreElementos modelo)
        {
            var item = Obtener(modelo.Id);
            modelo.FechaCreacion = item.FechaCreacion;       
            return _repositorio.Actualizar(ref modelo);
        }

        public bool ActualizarCierre(IEnumerable<CierreElementos> modelo)
        {
            foreach (var item in modelo)
            {
                item.IdEstadoSupervisor = (item.IdEstadoSupervisor == 0) ? null : item.IdEstadoSupervisor;
                item.IdEstadoNido = (item.IdEstadoNido == 0) ? null : item.IdEstadoNido;
                item.IdUsuarioSupervisor = (item.IdUsuarioSupervisor == 0) ? null : item.IdUsuarioSupervisor;
                item.IdUsuarioNido = (item.IdUsuarioSupervisor == 0) ? null : item.IdUsuarioNido;
                
                if (item.Id > 0)
                {
                    if (!Actualizar(item))
                    {
                        return false;
                    }                    
                }
                else
                {
                    Crear(item);
                    
                }
            }
            return true;
        }

        public CierreElementos Crear(CierreElementos modelo)
        {
            modelo.FechaCreacion = DateTime.Now;
            var resul =  _repositorio.Insertar(ref modelo);
            return _repositorio.Obtener(resul);
        }

        public CierreElementos Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        public IEnumerable<CierreElementos> ObtenerElementosCierre(int IdPunto)
        {
            return _repositorio.ObtenerElementosCierre(IdPunto);
            
        }

        public IEnumerable<CierreElementos> ObtenerElementosCierreNido(int Usuario)
        {
            return _repositorio.ObtenerElementosCierreNido(Usuario);
        }

        public IEnumerable<CierreElementos> ObtenerElementosCierreSupervisor(int Usuario)
        {
            return _repositorio.ObtenerElementosCierreSupervisor(Usuario);
        }

        public IEnumerable<CierreElementos> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Actualiza un cierre de elementos para supervisor o para nido.
        /// </summary>
        /// <param name="intTipo"></param>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool ActualizarMasivo(int intTipo, IEnumerable<CierreElementos> modelo, out string error)
        {
            bool blnResultado = true;
            error = string.Empty;

            try
            {
                _repositorio.ActualizarMasivo(intTipo, modelo, out error);
                if (error.Trim().Length > 0)
                {
                    throw new ArgumentException(error.Trim());
                }
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en ActualizarMasivo_ServicioCierrePunto: ", ex.Message);
                blnResultado = false;
            }

            return blnResultado;
        }

        #endregion

    }
}
