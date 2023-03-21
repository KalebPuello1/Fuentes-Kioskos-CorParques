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

    public class ServicioConvenioParqueadero : IServicioConvenioParqueadero
    {

        private readonly IRepositorioConvenioParqueadero _repositorio;
        private readonly IRepositorioAutorizacionVehiculo _repositorioAutorizacionVehiculo;

        #region Constructor

        public ServicioConvenioParqueadero(IRepositorioConvenioParqueadero repositorio, IRepositorioAutorizacionVehiculo repositorioAutorizacionVehiculo)
        {
            _repositorio = repositorio;
            _repositorioAutorizacionVehiculo = repositorioAutorizacionVehiculo;
        }

        #endregion

        #region Metodos

        public bool Actualizar(ConvenioParqueadero modelo, out string error)
        {
            bool blnResultado = true;
            error = string.Empty;
            string[] strSplitTipoVehiculo;
            string[] strSplitPlacas;
            List<AutorizacionVehiculo> objListaAutorizacionVehiculo = new List<AutorizacionVehiculo>();
            AutorizacionVehiculo objAutorizacionVehiculo;

            try
            {

                if (!string.IsNullOrEmpty(modelo.ListaIdTipoVehiculo))
                {
                    strSplitTipoVehiculo = modelo.ListaIdTipoVehiculo.Split(',');
                    strSplitPlacas = modelo.ListaPlacas.Split(',');
                    if (strSplitTipoVehiculo.Length > 0)
                    {
                        for (int Contador = 0; Contador < strSplitTipoVehiculo.Length; Contador++)
                        {
                            objAutorizacionVehiculo = new AutorizacionVehiculo();
                            objAutorizacionVehiculo.IdTipoVehiculo = int.Parse(strSplitTipoVehiculo[Contador].ToString());
                            objAutorizacionVehiculo.Placa = strSplitPlacas[Contador].ToString();
                            objListaAutorizacionVehiculo.Add(objAutorizacionVehiculo);
                            objAutorizacionVehiculo = null;
                        }
                    }
                }
                else
                {
                    objAutorizacionVehiculo = new AutorizacionVehiculo();
                    objAutorizacionVehiculo.IdTipoVehiculo = 0;
                    objAutorizacionVehiculo.Placa = "0";
                    objListaAutorizacionVehiculo.Add(objAutorizacionVehiculo);
                }
                modelo.objAutorizacionVehiculo = objListaAutorizacionVehiculo;
                blnResultado = _repositorio.Actualizar(modelo, out error);
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_ServicioConvenioParqueadero: ", ex.Message);
                blnResultado = false;
            }
            finally
            {
                objAutorizacionVehiculo = null;
            }

            return blnResultado;
            
        }

        public bool Eliminar(ConvenioParqueadero modelo, out string error)
        {
            return _repositorio.Eliminar(modelo, out error);
        }

        public bool Insertar(ConvenioParqueadero modelo, out string error)
        {

            bool blnResultado = true;
            error = string.Empty;
            string[] strSplitTipoVehiculo;
            string[] strSplitPlacas;
            List<AutorizacionVehiculo> objListaAutorizacionVehiculo = new List<AutorizacionVehiculo>();
            AutorizacionVehiculo objAutorizacionVehiculo;

            try
            {
                if (!string.IsNullOrEmpty(modelo.ListaIdTipoVehiculo))
                {
                    strSplitTipoVehiculo = modelo.ListaIdTipoVehiculo.Split(',');
                    strSplitPlacas = modelo.ListaPlacas.Split(',');
                    if (strSplitTipoVehiculo.Length > 0)
                    {
                        for (int Contador = 0; Contador < strSplitTipoVehiculo.Length; Contador++)
                        {
                            objAutorizacionVehiculo = new AutorizacionVehiculo();
                            objAutorizacionVehiculo.IdTipoVehiculo = int.Parse(strSplitTipoVehiculo[Contador].ToString());
                            objAutorizacionVehiculo.Placa = strSplitPlacas[Contador].ToString();
                            objListaAutorizacionVehiculo.Add(objAutorizacionVehiculo);
                            objAutorizacionVehiculo = null;
                        }
                    }
                }
                else
                {
                    objAutorizacionVehiculo = new AutorizacionVehiculo();
                    objAutorizacionVehiculo.IdTipoVehiculo = 0;
                    objAutorizacionVehiculo.Placa = "0";
                    objListaAutorizacionVehiculo.Add(objAutorizacionVehiculo);
                }
                modelo.objAutorizacionVehiculo = objListaAutorizacionVehiculo;
                blnResultado = _repositorio.Insertar(modelo, out error);
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_ServicioConvenioParqueadero: ", ex.Message);
                blnResultado = false;
            }
            finally
            {
                objAutorizacionVehiculo = null;
            }

            return blnResultado;
        }

        public IEnumerable<ConvenioParqueadero> ObtenerLista()
        {
            return _repositorio.ObtenerLista();
        }

        public IEnumerable<EstructuraEmpleado> ObtenerListaEmpleados()
        {
            return _repositorio.ObtenerListaEmpleados();
        }

        /// <summary>
        /// RDSH: Retorno del objeto convenio (header autorizacion) y de los vehiculos relacionados a esta.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ConvenioParqueadero ObtenerPorId(int Id)
        {
            ConvenioParqueadero objConvenioParqueadero;
            objConvenioParqueadero = _repositorio.ObtenerPorId(Id);
            objConvenioParqueadero.objAutorizacionVehiculo = _repositorioAutorizacionVehiculo.ObtenerPorIdConvenioParqueadero(Id);
            return objConvenioParqueadero;
        }

        public ConvenioParqueadero ObtenerPorPlaca(string Placa)
        {
            return _repositorio.ObtenerPorPlaca(Placa);
        }

        #endregion

    }
}
