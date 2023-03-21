using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using CorParques.Transversales.Util;

namespace CorParques.Datos.Dapper
{

	public class RepositorioTipoProducto : RepositorioBase<TipoProducto>,  IRepositorioTipoProducto
	{

        /// <summary>
        /// RDSH: Retorna la lista de tipo de producto.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoProducto> ObtenerListaTipoProduto()
        {

            List<TipoProducto> objListaTipoProducto = null;

            try
            {
                objListaTipoProducto = _cnn.Query<TipoProducto>("SP_ObtenerTipoProducto", null, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ObtenerListaTipoProduto_RepositorioConvenio");
            }

            return objListaTipoProducto;
        }

    }
}
