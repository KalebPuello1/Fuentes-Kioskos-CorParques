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
using System.Data;

namespace CorParques.Datos.Dapper
{
    public class RepositorioReporteUsuarioApp : IRepositorioReporteUsuarioApp
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;
        public IEnumerable<ReporteUsuarioApp>[] listaUsuarioApp = new IEnumerable<ReporteUsuarioApp>[3];
        #endregion

        #region Constructor

        public RepositorioReporteUsuarioApp()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        #endregion

        public IEnumerable<ReporteUsuarioApp>[] ObtenerReporte(string Correoelectronico)
        {
            try
            {                
                
                    var rta =  _cnn.QueryMultiple(
                "SP_ObtenerCompraAppUsuario",
                param: new
                {
                    Usuario =(Correoelectronico)
                },
                commandType: System.Data.CommandType.StoredProcedure);
                ///var datos = rta.Read<ReporteUsuarioApp>().ToList();
          ///      if (datos.Count() > 0)
                    listaUsuarioApp[0]= (rta.Read<ReporteUsuarioApp>().ToList());
                    listaUsuarioApp[1]=  (rta.Read<ReporteUsuarioApp>().ToList());
                    listaUsuarioApp[2]= (rta.Read<ReporteUsuarioApp>().ToList());
               
                
            }
            catch (Exception ex)
            {
               // return listaUsuarioApp;
                throw new ArgumentException(string.Concat("Se presentó un error al realizar la consulta: ", ex.Message));
            }
            return listaUsuarioApp;
        }
        
    }

}
