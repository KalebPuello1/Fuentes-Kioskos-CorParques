using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace CorParques.Datos.Dapper
{
    public class RepositorioPaciente : IRepositorioPaciente
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioPaciente()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion

        /// <summary>
        /// RDSH: Actualiza un paciente.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(Paciente modelo, out string error)
        {
            error = _cnn.Query<string>("SP_ActualizarPaciente", param: new
            {
                IdPaciente = modelo.IdPaciente,
                IdTipoDocumento = modelo.IdTipoDocumento,
                Documento = modelo.Documento,
                Nombre = modelo.Nombre,
                Apellido = modelo.Apellido,
                Acudiente = modelo.Acudiente,
                TelefonoPaciente = modelo.TelefonoPaciente,
                TelefonoAcudiente = modelo.TelefonoAcudiente,
                Correo = modelo.Correo,
                IdEstado = modelo.IdEstado,
                IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                FechaModificacion = modelo.FechaModificacion
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: Borrado logico de un paciente.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Eliminar(Paciente modelo, out string error)
        {
            error = _cnn.Query<string>("SP_EliminarPaciente", param: new
            {
                IdPaciente = modelo.IdPaciente,                
                IdEstado = modelo.IdEstado,
                IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                FechaModificacion = modelo.FechaModificacion
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: Insercion de un paciente.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(Paciente modelo, out string error)
        {
            error = _cnn.Query<string>("SP_InsertarPaciente", param: new
            {
                IdTipoDocumento = modelo.IdTipoDocumento,
                Documento = modelo.Documento,
                Nombre = modelo.Nombre,
                Apellido = modelo.Apellido,
                Acudiente = modelo.Acudiente,
                TelefonoPaciente = modelo.TelefonoPaciente,
                TelefonoAcudiente = modelo.TelefonoAcudiente,
                Correo = modelo.Correo,
                IdEstado = modelo.IdEstado,
                IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                FechaCreacion = modelo.FechaCreacion
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            return string.IsNullOrEmpty(error);
        }

        public IEnumerable<Paciente> ObtenerLista()
        {
            return _cnn.GetList<Paciente>();
        }

        /// <summary>
        /// RDSH: Retorna un paciente por id paciente.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Paciente ObtenerPorId(int Id)
        {
            return _cnn.Get<Paciente>(Id);
        }

        /// <summary>
        /// RDSH: Retorna un paciente por tipo y numero de documento.
        /// </summary>
        /// <param name="IdTipoDocumento"></param>
        /// <param name="Documento"></param>
        /// <returns></returns>
        public Paciente ObtenerPorTipoDocumento(int IdTipoDocumento, string Documento)
        {
            return _cnn.GetList<Paciente>().Where(x => x.IdTipoDocumento == IdTipoDocumento && x.Documento == Documento).FirstOrDefault();
        }

        /// <summary>
        /// RDSH: Lista de pacientes para cargar combo box.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            return _cnn.GetList<Paciente>().Select(x => new TipoGeneral { Id = x.IdPaciente, Nombre = string.Concat(x.Nombre, ' ', x.Apellido) });
        }
     
    }
}
