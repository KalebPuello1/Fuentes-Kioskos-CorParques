using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{

	public class RepositorioMateriales : RepositorioBase<Materiales>,  IRepositorioMateriales
	{

        public IEnumerable<Materiales> ObtenerMaterialesxPuntos(int IdPunto, DateTime? Fecha)
        {
            Fecha = (Fecha == null) ? System.DateTime.Now : Fecha;
            
            //GALD utilizar para pruebas desde los fuentes
            return _cnn.Query<Materiales>("SP_MaterialesDisponiblesxPunto", param: new { IdPunto = IdPunto, Fecha = Fecha }, commandType: System.Data.CommandType.StoredProcedure).ToList();

        }

        public IEnumerable<Materiales> ObtenerTodos()
        {
            return _cnn.Query<Materiales>("SP_MaterialesTodos", null, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
