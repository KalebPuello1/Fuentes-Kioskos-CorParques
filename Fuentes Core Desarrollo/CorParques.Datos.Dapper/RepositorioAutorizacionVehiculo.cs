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

	public class RepositorioAutorizacionVehiculo : RepositorioBase<AutorizacionVehiculo>,  IRepositorioAutorizacionVehiculo
	{

        #region Metodos

        public IEnumerable<AutorizacionVehiculo> ObtenerPorIdConvenioParqueadero(int IdConvenioParqueadero)
        {
            var objAutorizacionVehiculo = _cnn.Query<AutorizacionVehiculo>("SP_ObtenerAutorizacionVehiculo", param: new
            {
                IdConvenioParqueadero = IdConvenioParqueadero
            }, commandType: System.Data.CommandType.StoredProcedure).ToList();

            return objAutorizacionVehiculo;
        }


        #endregion

    }
}
