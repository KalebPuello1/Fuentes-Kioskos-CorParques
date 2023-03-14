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
using CorParques.Transversales.Util;

namespace CorParques.Datos.Dapper
{
    public class RepositorioOperacion : IRepositorioOperacion
    {
        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor
        public RepositorioOperacion()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        #endregion

        public IEnumerable<Operaciones> ObtenerOrdenes(string Punto, string NumDocumento)
        {
            //var lista = _cnn.GetList<Operaciones>().Where(x => x.Procesado == false).ToList();
            var lista = _cnn.Query<Operaciones>("SP_ObtenerOrdenes",
                param: new
                {
                    Punto = Punto,
                    NumDocumento = NumDocumento
                }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return lista;
        }

        public Operaciones ObtenerOrdenPorNumeroOrden(long NumeroOrden)
        {
            var orden = _cnn.GetList<Operaciones>().Where(x => x.NumeroOrden == NumeroOrden).First();
            return orden;
        }


        /// <summary>
        /// RDSH: Retorna las operaciones por numero de orden.
        /// </summary>
        /// <param name="intNumeroOrden"></param>
        /// <returns></returns>
        public IEnumerable<Operaciones> ObtenerOperacionesPorOrden(long intNumeroOrden)
        {

            try
            {
                var lista = _cnn.Query<Operaciones>("SP_ObtenerOperacionesPorOrden",
                                                        param: new
                                                        {
                                                            NumeroOrden = intNumeroOrden
                                                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                return lista;
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioOperacion_ObtenerOperacionesPorOrden");
                return null;
            }
        }
    }
}
