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
    public class RepositorioProductosApp : RepositorioBase<Producto>, IRepositorioProductosApp
    {
        public bool Insertar(Producto producto)
        {

            string strList = string.Empty;
            string strListPuntos = string.Empty;

            var sql = _cnn.Query<bool>("sec.SP_Actualizar", new
            {

                Nombre = producto.Nombre,
                CodSap = producto.CodigoSap,
                Precio = producto.Precio,

            }, commandType: System.Data.CommandType.StoredProcedure).Single();

            return sql;
        }


        public bool Crear(Producto producto)
        {
            string strList = string.Empty;
            string strListPuntos = string.Empty;

            var sql = _cnn.Query<string>("SP_InsertarProductosApp", new
            {
                Id = producto.IdProducto,
                CodSap = producto.CodigoSap,
                Name = producto.Nombre,
                Description = producto.Descricpion,
                Price = producto.Precio,
                Photo = string.Concat(producto.CodigoSap,".png"),
                ProductType = producto.IdTipoProducto,
                Active = 1,
                DateActive = System.DateTime.Now,
                Araza = producto.Araza,

            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            return string.IsNullOrWhiteSpace(sql);
        }




        public bool Inactivar( int IdProducto, int activo)
        {
           string error = string.Empty;

            try
            {
                error = _cnn.Query<string>("SP_InactivarProductosApp", param: new
                {
                    IdProducto = IdProducto,
                    Activo = activo
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {              
                error = string.Concat("Error inesperado en Actualizar_RepositorioCortesiaPunto: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);

        }

        public IEnumerable<Producto> Obtener()
        {
            var sql = _cnn.Query<Producto>("SP_ConsultarProductosAppCore", commandType: System.Data.CommandType.StoredProcedure).ToList();

            return sql;
        }

        public Producto ObtenerId(int IdProducto)
        {
            return _cnn.GetList<Producto>().Where(x => x.IdProducto==IdProducto).FirstOrDefault();
        }
    }
}
