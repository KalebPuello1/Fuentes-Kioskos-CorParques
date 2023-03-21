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

	public class ServicioConvenio : IServicioConvenio
	{

		private readonly IRepositorioConvenio _repositorio;

        #region Constructor

        public ServicioConvenio (IRepositorioConvenio repositorio)
		{

			_repositorio = repositorio;
		}


        #endregion

        #region Metodos

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
                modelo.ListaDetalle.Where(x => x.CantidadDisponible == null).ToList().ForEach(x => x.CantidadDisponible = 0);
                modelo.ListaDetalle.ToList().ForEach(x => x.CantidadDisponible = CalcularCantidadDisponible(x.Cantidad, x.CantidadInicial, x.CantidadDisponible));

                return _repositorio.Actualizar(modelo, out error);
            }
            catch (Exception ex)
            {
                error = "Ocurrio un error actualizando el convenio.";
                Utilidades.RegistrarError(ex, "Actualizar_ServicioConvenio");
                return string.IsNullOrEmpty(error);
            }            
          
        }
        

        /// <summary>
        /// RDSH: Inserta un convenio. Retorna el id del convenio en la propiedad error
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(Convenio modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }

        /// <summary>
        /// EDSP: Actualizar precios convenios
        /// </summary>
        public string ActualizarPreciosConvenios(ActualizarPrecios modelo)
        {
            return _repositorio.ActualizarPreciosConvenios(modelo);
        }


        /// <summary>
        /// RDSH: Retorna la información de los pedidos.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Convenio> ObtenerListaConvenios()
        {
            return _repositorio.ObtenerListaConvenios();
        }

        /// <summary>
        ///RDSH: Retorna la informacion de un convenio para su edicion.
        ///EDSP: Se envia el id del convenio para obtener el detalle del convenio,metodo obtener convenio 29/12/2017
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public Convenio ObtenerPorId(int Id)
        {
            Convenio objConvenio;
            objConvenio = _repositorio.Obtener(Id);
            objConvenio.ListaDetalle = _repositorio.ObtenerDetalleConvenio(objConvenio.IdConvenio);
            return objConvenio;
        }

        /// <summary>
        /// RDSH: Calcula la cantidad disponible.
        /// </summary>
        /// <param name="intCantidad"></param>
        /// <param name="intCantidadInicial"></param>
        /// <param name="intCantidadDisponible"></param>
        /// <returns></returns>
        private int CalcularCantidadDisponible(int? Cantidad, int intCantidadInicial, int? CantidadDisponible)
        {
            int intRetornoCantidadDisponible = 0;
            int intCantidadDisponible = 0;
            int intCantidad = 0;

            try
            {
                if (Cantidad.HasValue)
                    intCantidad = Cantidad.Value;

                if (CantidadDisponible.HasValue)
                    intCantidadDisponible = CantidadDisponible.Value;

                if (intCantidadDisponible <= 0)
                {
                    intRetornoCantidadDisponible = intCantidad;
                }
                else if (intCantidad > intCantidadInicial)
                {
                    intRetornoCantidadDisponible = (intCantidad - intCantidadInicial) + intCantidadDisponible;
                }
                else if (intCantidad < intCantidadInicial)
                {
                    if (intCantidad < intCantidadDisponible)
                    {
                        intRetornoCantidadDisponible = intCantidad;
                    }
                    else
                    {
                        intRetornoCantidadDisponible = (intCantidadInicial - intCantidad);
                        intRetornoCantidadDisponible = intCantidadDisponible - intRetornoCantidadDisponible;
                    }
                }
                else
                {
                    intRetornoCantidadDisponible = intCantidad;
                }
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "CalcularCantidadDisponible_ServicioConvenio");
            }

            return intRetornoCantidadDisponible;

        }

        /// <summary>
        /// EDSP: insertar exclusión convenio
        /// </summary>

        public string InsertarExclusionConvenio(ExclusionConvenio modelo)
        {
            return _repositorio.InsertarExclusionConvenio(modelo);
        }

        /// <summary>
        /// EDSP: Deshabilita exclusión del convenio
        /// </summary>
        public int DeshabilitarExclusion(ExclusionConvenio modelo)
        {
            var obj = _repositorio.ObtenerExclusion(modelo.Id);
            obj.IdEstado = modelo.IdEstado;
            obj.IdUsuarioModificacion = modelo.IdUsuarioModificacion;
            obj.FechaModificacion = DateTime.Now;

            return _repositorio.DeshabilitarExclusion(obj);
        }

        /// <summary>
        /// EDSP: Obtener todos las exclusiones del convenio
        /// </summary>
        public IEnumerable<ExclusionConvenio> ObtenerExclusionesConvenio()
        {
            return _repositorio.ObtenerExclusionesConvenio();
        }

        /// <summary>
        /// EDSP: Obtener exclusión por id convenio
        /// </summary>
        public IEnumerable<ExclusionConvenio> ObtenerExclusionesPorIdConvenio(int IdConvenio)
        {
            return _repositorio.ObtenerExclusionesPorIdConvenio(IdConvenio);
        }

        /// <summary>
        /// EDSP: Obtener productos por id convenio
        /// </summary>
        public IEnumerable<ConvenioProducto> ObtenerProductoConvenio(int IdConvenio)
        {
            return _repositorio.ObtenerProductoConvenio(IdConvenio);
        }

        public string ActualizarProductosConvenio(IEnumerable<ConvenioProducto> modelo)
        {
            return _repositorio.ActualizarProductosConvenio(modelo);
        }

        #endregion

        #region Metodos no implementados

        public bool Actualizar(Convenio modelo)
        {
            throw new NotImplementedException();
        }

        public Convenio Crear(Convenio modelo)
        {
            throw new NotImplementedException();
        }
        
        public IEnumerable<Convenio> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        public Convenio Obtener(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
