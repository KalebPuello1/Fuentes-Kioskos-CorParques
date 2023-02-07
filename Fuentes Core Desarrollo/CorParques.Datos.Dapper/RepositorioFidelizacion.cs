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

    public class RepositorioFidelizacion : RepositorioBase<ClienteFideliacion>, IRepositorioFidelizacion
    {
        public ClienteFideliacion ObtenerClienteTarjeta(string doc) {

            var rta = _cnn.Query<ClienteFideliacion>("SP_ObtenerClienteTarjeta", param: new
            {
                Documento = doc
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return rta;
        }
        public ClienteFideliacion ObtenerTarjetaSaldoPuntos(string consecutivo) {

            var rta = _cnn.Query<ClienteFideliacion>("SP_ObtenerTarjetaSaldoPuntos", param: new
            {
                Consecutivo = consecutivo
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return rta;
        }

        public IEnumerable<TipoGeneral> ObtenerProductosRedimibles(int puntos)
        {

            var rta = _cnn.Query<TipoGeneral>("SP_ObtenerProductosRedimibles", param: new
            {
                Puntos = puntos
            }, commandType: System.Data.CommandType.StoredProcedure);
            return rta;
        }
        public bool BloquearTarjeta(string consecutivo, int usuario, int punto) {
            var rta = _cnn.Query<bool>("SP_BloquearTarjeta", param: new
            {
                Consecutivo = consecutivo,
                Usuario = usuario,
                Punto = punto
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return rta;
        }

        public bool RedimirProduto(string consecutivo, int producto, int usuario, int punto)
        {
            var rta = _cnn.Query<bool>("SP_RedimirProducto", param: new
            {
                Consecutivo = consecutivo,
                Producto = producto,
                Usuario = usuario,
                Punto = punto
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return rta;
        }
    }
}
