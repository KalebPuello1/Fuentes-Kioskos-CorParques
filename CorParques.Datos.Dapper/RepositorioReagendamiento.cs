using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corparques.Datos.Contratos
{
    public class RepositorioReagendamiento : IRepositorioReagendamiento
    {
        private readonly SqlConnection _cnn;
        public RepositorioReagendamiento()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        public string ModificarFecha(CambioFechaBoleta producto)
        {
            try
            {
                var ini = producto.FechaUsoInicial + " 00:00:00.000";
                var fin = producto.FechaUsoFinal + " 23:59:00.000";

                _cnn.Query<string>($"update tb_boleteria set fechaUsoInicial = '{ini}', FechaUsoFinal = '{fin}'," +
                $"FechaInicioEvento = '{ini}', FechaFinEvento = '{fin}' where Consecutivo = {producto.Consecutivo}");

                return "Exitoso";
            }
            catch (Exception)
            {
                return "fallo";
            }
        }

        public Boleteria ObtenerProducto(string consecutivo)
        {
            Boleteria producto = new Boleteria();
            Producto objCodSap = new Producto();

            try
            {
                producto = _cnn.Query<Boleteria>($"SELECT * FROM TB_Boleteria WHERE Consecutivo = '{consecutivo}'").First();
                producto.CantidadPasaportes = _cnn.Query<int>($"select  count (*) from TB_Boleteria where Consecutivo = '{consecutivo}'").First();
                producto.UsosPasaportes = _cnn.Query<int>($"select count (*) from TB_RegistroUsoBoleteria where consecutivo = '{consecutivo}'").First();
                producto.ReagendamientoPasaportes = _cnn.Query<int>($"select count (*) from TB_DetalleReagendamientoFecha where consecutivo = '{consecutivo}'").First();

                return producto;
            }
            catch (Exception)
            {
                producto.NombreProducto = "No existe";
                return producto;
            }
        }

        public Reagendamiento ObtenerFacturaReagendamiento(string CodBarra)
        {

            var _rta = new Reagendamiento();

            try
            {
                var rta = _cnn.QueryMultiple("SP_ObtenerFacturaReagendamiento", new { CodigoFactura = CodBarra }, commandType: System.Data.CommandType.StoredProcedure);

                //Listas
                _rta.Factura = rta.Read<Factura>();
                _rta.DetalleFactura = rta.Read<DetalleFactura>();
                _rta.MedioPagoFactura = rta.Read<MedioPagoFactura>();
                _rta.Boleteria = rta.Read<Boleteria>();

                return _rta;
            }
            catch (Exception e)
            {
                _rta.NombreProducto = "No existe";
                return _rta;
            }
        }

        public Boleteria ObtenerDetalleReagendamientoFecha(string consecutivo)
        {
            var Consecutivo = consecutivo;
            Boleteria producto = new Boleteria();
            try
            {
                producto = _cnn.Query<Boleteria>($"SELECT * FROM TB_DetalleReagendamientoFecha WHERE Consecutivo = '{Consecutivo}'").Last();
                producto.NombreProducto = "existe";
                return producto;
            }
            catch (Exception e)
            {
                producto.NombreProducto = "No existe";
                return producto;
            }
        }
        public string InsertarDetalleReagendamientoFecha(Reagendamiento reagendamiento)
        {
            string producto;
            var FI = reagendamiento.fechaInicial;
            var FA = reagendamiento.fechaFinal;
            var FUI = reagendamiento.fechaInicial;
            var FUA = reagendamiento.fechaFinal;
            //string consecutivo = reagendamiento.Consecutivo;
            string consecutivo = reagendamiento.Consecutivo;
            var IdUsuario = reagendamiento.idUsuarioLogueado;
            var Observacion = "";
            try
            {
                _cnn.Query("SP_InsertarDetalleReagendamientoFecha", commandType: System.Data.CommandType.StoredProcedure, param: new
                {
                    @fechaInicial = FI,
                    @fechaFinal = FA,
                    @fechaInicialAnterior = FUI,
                    @fechaFinalAnterior = FUA,
                    //@Consecutivo = consecutivo,
                    @Consecutivo = consecutivo,
                    @IdUsuario = IdUsuario,
                    @Observacion = Observacion

                });
                producto = "Exitoso";
                return producto;
            }
            catch (Exception e)
            {
                producto = "fallo";
                return producto;
            }
        }
    }
}
