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

namespace CorParques.Datos.Dapper
{
    public class RepositorioRecambio : IRepositorioRecambio
    {
        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioRecambio()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion

        #region Metodos

        
        public bool InsertarRecambio(Recambio modelo, out string error, out int IdRecambio)
        {

            IdRecambio = 0;

            try
            {
                error = _cnn.Query<string>("SP_InsertarRecambio", param: new
                {
                    IdEstado = modelo.IdEstado,
                    IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                    FechaCreacion = DateTime.Now,
                    Valor = modelo.Valor,
                    ObservacionRecambio = modelo.ObservacionRecambio,
                    ObservacionAprovado = modelo.ObservacionAprovado,
                    IdUsuarioAsignacion = modelo.IdUsuarioAsignacion

                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                if (error.Trim().Length > 0)
                {
                    int.TryParse(error, out IdRecambio);
                    if (IdRecambio > 0)
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
        #endregion
    }
}
