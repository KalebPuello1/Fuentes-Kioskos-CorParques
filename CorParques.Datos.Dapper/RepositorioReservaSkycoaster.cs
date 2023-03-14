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
    public class RepositorioReservaSkycoaster : IRepositorioReservaSkycoaster
    {
        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioReservaSkycoaster()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion

        #region Metodos

        public IEnumerable<ReservaSkycoaster> ObtenerListaReserva()
        {
            var fechaActual = DateTime.Today.ToString();
            var lista = _cnn.GetList<ReservaSkycoaster>(conditions: $"WHERE CONVERT(VARCHAR,FechaReserva,103) = '{DateTime.Now.ToString("dd/MM/yyyy")}'");
            return lista;
        }

        public bool Insertar(ReservaSkycoaster modelo, out string error)
        {
            error = _cnn.Query<string>("InsertarReserva", param: new
            {
                IdTicket = modelo.IdTicket,
                HoraInicio = modelo.HoraInicio.Substring(0, 2) + ":" + modelo.HoraInicio.Substring(2, 2),
                HoraFin = modelo.HoraFin.Substring(0, 2) + ":" + modelo.HoraFin.Substring(2, 2),
                FechaCreacion = DateTime.Now,
                Consecutivo = modelo.Consecutivo,
                IdPunto = modelo.IdPunto,
                Capacidad = modelo.Capacidad,
                IdUsuarioCreacion = modelo.IdUsuarioCreacion
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }

        public int ObtenerReservaHora(string horaInicio)
        {
            var fechaActual = DateTime.Today;
            var resp = _cnn.Query<int>("SP_RetornarIdReserva", param: new { Fecha = fechaActual, HoraInicio = horaInicio }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return resp.FirstOrDefault();
        }

        public bool LiberarReserva(ReservaSkycoaster modelo, out string error)
        {
            error = _cnn.Query<string>("LiberarReserva", param: new
            {
               
                IdPunto = modelo.IdPunto,
                HoraInicio = modelo.HoraInicio.Substring(0, 2) + ":" + modelo.HoraInicio.Substring(2, 2),
                IdUsuarioModificacion = modelo.IdUsuarioModificacion
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }


        public bool CerrarReserva(ReservaSkycoaster modelo, out string error)
        {
            error = _cnn.Query<string>("CerrarReserva", param: new
            {
                IdTicket = modelo.IdTicket,
                HoraInicio = modelo.HoraInicio.Substring(0, 2) + ":" + modelo.HoraInicio.Substring(2, 2),
                HoraFin = modelo.HoraFin.Substring(0, 2) + ":" + modelo.HoraFin.Substring(2, 2),
                FechaCreacion = DateTime.Now,
                Consecutivo = modelo.Consecutivo,
                IdPunto = modelo.IdPunto,
                Capacidad = modelo.Capacidad,
                IdUsuarioCreacion = modelo.IdUsuarioCreacion

            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }

        #endregion

    }
}
