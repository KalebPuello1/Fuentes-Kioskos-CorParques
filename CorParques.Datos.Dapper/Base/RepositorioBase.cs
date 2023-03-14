using CorParques.Datos.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace CorParques.Datos.Dapper
{
    public class RepositorioBase<T> : IRepositorioBase<T> where T : class
    {
        public readonly SqlConnection _cnn;

        public RepositorioBase()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        public bool Actualizar(ref T modelo)
        {
            try
            {

            return _cnn.Update(modelo) > 0;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool Eliminar(T modelo)
        {
            return _cnn.Delete<T>(modelo) > 0;
        }

        public IEnumerable<T> ExecuteQuery(string query, object Parametros)
        {
            return _cnn.Query<T>(query, Parametros);
        }

        public int Insertar(ref T modelo)
        {
            return _cnn.Insert<int>(modelo);
            
        }

        public T Obtener(int id)
        {
            return _cnn.Get<T>(id);
        }

        public IEnumerable<T> ObtenerLista()
        {
            try
            {

            return _cnn.GetList<T>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }   


        public IEnumerable<T> ObtenerLista(string filtro)
        {
            return _cnn.GetList<T>(filtro);
        }

        public IEnumerable<T> ObtenerListaPaginada(int pagina,int registrosPorPagina,string filtro, string columnaOrden)
        {
            return _cnn.GetListPaged<T>(pagina, registrosPorPagina,filtro,columnaOrden);
        }

        public IEnumerable<T> StoreProcedure(string nombre, object Parametros)
        {
            return _cnn.Query<T>(nombre, param: Parametros, commandType: System.Data.CommandType.StoredProcedure);
        
        }

        //private string GetWhere(Expression<Func<T, bool>> where) {
        //    string wheretest = "WHERE";

        //    var filtros = new Queue<Expression>(new[] { where.Body });
        //    a(filtros);
        //    foreach (var prop in where.GetType().GetProperties())
        //    {
        //        string propName = prop.Name;
        //        if (prop.CustomAttributes != null)
        //            if (prop.CustomAttributes.Count() > 0)
        //                if (prop.CustomAttributes.FirstOrDefault(x => x.AttributeType.Name.Equals("ColumnAttribute")) != null)
        //                    propName = prop.CustomAttributes.FirstOrDefault(x => x.AttributeType.Name.Equals("ColumnAttribute")).ConstructorArguments[0].Value.ToString();
        //        object value = prop.GetValue(where, null);
        //        bool valido = false;
        //        if (value != null)
        //            switch (value.GetType().Name)
        //            {
        //                case "Guid":
        //                    valido = (Guid)value != new Guid();
        //                    break;

        //                case "String":
        //                    valido = !string.IsNullOrEmpty((string)value);
        //                    break;

        //                case "DateTime":
        //                    valido = (DateTime)value != null;
        //                    break;
        //            }
        //        if (valido)
        //            wheretest += string.Format(" {0} = '{1}'", (wheretest != "WHERE" ? "AND " : "") + propName, value.ToString());
        //    }
        //    return wheretest;
        //}

        //private void a(Queue<Expression> where)
        //{
        //    var ss = where;
        //    foreach (var item in where)
        //    {
        //        var a = item.CanReduce;
        //    }
        //}
    }
}
