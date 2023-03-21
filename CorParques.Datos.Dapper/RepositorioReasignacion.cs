using Corparques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{
    public class RepositorioReasignacion : IRepositorioReagendamiento
    {
        SqlConnection _cnn;
        public RepositorioReasignacion(SqlConnection cnn)
        {
            SqlConnection _cnn = cnn;
        }
        public string ModificarFecha(Boleteria producto)
        {
            try
            {
                _cnn.Query($"Update TB_Boleteria set FechaUsoInicial = {producto.FechaUsoInicial}, FechaUsoFinal = {producto.FechaUsoFinal}, FechaInicioEvento = {producto.FechaInicioEvento}, FechaFinEvento = {producto.FechaFinEvento} WHERE Consecutivo = {producto.Consecutivo}");
                return "Satisfactorio";
            }
            catch (Exception e)
            {
                return "Fallo la modificacion de fecha";
            }
        }

        public string ModificarFecha(CambioFechaBoleta producto)
        {
            throw new NotImplementedException();
        }

        public Boleteria ObtenerProducto(string consecutivo)
        {
            try
            {
                var boleta = _cnn.Query<Boleteria>("", commandType: System.Data.CommandType.StoredProcedure, param: new
                {
                    @Consecutivo = consecutivo
                }).First();
                return boleta;
            }
            catch (Exception)
            {
                Boleteria fallo = new Boleteria();
                fallo.NombreProducto = "Este consecutivo no se encuentra registrado en la boleteria";
                return fallo;
            }
        }
    }
}
