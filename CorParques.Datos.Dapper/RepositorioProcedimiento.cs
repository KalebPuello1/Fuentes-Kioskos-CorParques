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
    public class RepositorioProcedimiento : IRepositorioProcedimiento
    {


        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioProcedimiento()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion
        
        #region Metodos

        /// <summary>
        /// RDSH: Actualiza un procedimiento.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(Procedimiento modelo, out string error)
        {
            error = _cnn.Query<string>("SP_ActualizarProcedimiento", param: new
            {
                IdProcedimiento = modelo.IdProcedimiento,                
                IdCentroMedico = modelo.IdCentroMedico,                
                Causa = modelo.Causa,
                Sintomas = modelo.Sintomas,
                Alergias = modelo.Alergias,
                Tratamiento = modelo.Tratamiento,
                Recomendaciones = modelo.Recomendaciones,
                NombreMedico = modelo.NombreMedico,
                IdEstado = modelo.IdEstado,                
                IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                FechaModificacion = modelo.FechaModificacion,
                IdCategoriaAtencion = modelo.IdCategoriaAtencion,
                IdTipoPaciente = modelo.IdTipoPaciente,
                Triage = modelo.Triage,
                IdTipoDocumentoPaciente = modelo.IdTipoDocumentoPaciente,
                DocumentoPaciente = modelo.DocumentoPaciente,
                NombrePaciente = modelo.NombrePaciente,
                ApellidoPaciente = modelo.ApellidoPaciente,
                EdadPaciente = modelo.EdadPaciente,
                DireccionPaciente = modelo.DireccionPaciente,
                TelefonoPaciente = modelo.TelefonoPaciente,
                CorreoPaciente = modelo.CorreoPaciente,
                IdTipoDocumentoAcudiente = modelo.IdTipoDocumentoAcudiente,
                DocumentoAcudiente = modelo.DocumentoAcudiente,
                NombreAcudiente = modelo.NombreAcudiente,
                FechaIncidente = modelo.FechaIncidente,
                TelefonoAcudiente = modelo.TelefonoAcudiente,
                Meses = modelo.Meses,
                Traslado = modelo.Traslado,
                Eps = modelo.Eps,
                NombreEps = modelo.NombreEps
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            return string.IsNullOrEmpty(error);
        }

        public bool Eliminar(Procedimiento modelo, out string error)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserta un procedimiento.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(Procedimiento modelo, out string error)
        {

            error = _cnn.Query<string>("SP_InsertarProcedimiento", param: new
            {
                
                IdCentroMedico = modelo.IdCentroMedico,                
                Causa = modelo.Causa,
                Sintomas = modelo.Sintomas,
                Alergias = modelo.Alergias,
                Tratamiento = modelo.Tratamiento,
                Recomendaciones = modelo.Recomendaciones,
                NombreMedico = modelo.NombreMedico,
                IdEstado = modelo.IdEstado,
                IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                FechaCreacion = modelo.FechaCreacion ,
                IdCategoriaAtencion = modelo.IdCategoriaAtencion,
                IdTipoPaciente = modelo.IdTipoPaciente,
                Triage = modelo.Triage,
                IdTipoDocumentoPaciente = modelo.IdTipoDocumentoPaciente,
                DocumentoPaciente = modelo.DocumentoPaciente,
                NombrePaciente = modelo.NombrePaciente,
                ApellidoPaciente = modelo.ApellidoPaciente,
                EdadPaciente = modelo.EdadPaciente,
                DireccionPaciente = modelo.DireccionPaciente,
                TelefonoPaciente = modelo.TelefonoPaciente,
                CorreoPaciente = modelo.CorreoPaciente,
                IdTipoDocumentoAcudiente = modelo.IdTipoDocumentoAcudiente,
                DocumentoAcudiente = modelo.DocumentoAcudiente,
                NombreAcudiente = modelo.NombreAcudiente,
                FechaIncidente = modelo.FechaIncidente,
                TelefonoAcudiente = modelo.TelefonoAcudiente,
                Meses = modelo.Meses,
                Traslado = modelo.Traslado,
                Eps = modelo.Eps,
                NombreEps = modelo.NombreEps
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            return string.IsNullOrEmpty(error);

        }

        /// <summary>
        /// RDSH: Obtiene la lista de procedimientos con sus pacientes.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Procedimiento> ObtenerLista()
        {
            //return _cnn.Query<Procedimiento, Paciente, Procedimiento>("SP_ObtenerProcedimientoPaciente", (PRO, PA) => { PRO.objPaciente = PA; return PRO; }, null, commandType: System.Data.CommandType.StoredProcedure, splitOn: "IdPaciente").ToList();
            return _cnn.Query<Procedimiento>("SP_ObtenerProcedimientoPaciente",null, commandType: System.Data.CommandType.StoredProcedure).ToList();

        }

        /// <summary>
        /// RDSH: Obtiene un procedimiento para editar.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Procedimiento ObtenerPorId(int Id)
        {
            return _cnn.Get<Procedimiento>(Id);
        }

        /// <summary>
        /// RDSH: Genera el reporte de atencion en centro médico.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        public IEnumerable<Procedimiento> ReporteAtenciones(int IdTipoDocumentoPaciente, int IdCategoriaAtencion, int IdTipoPaciente, string FechaInicial, string FechaFinal, int IdProcedimiento, int IdZonaArea, int IdUbicacion)
        {

            try
            {
                return _cnn.Query<Procedimiento>("SP_ReporteCentroMedico", param: new
                {
                    IdTipoDocumentoPaciente = IdTipoDocumentoPaciente,
                    IdCategoriaAtencion = IdCategoriaAtencion,
                    IdTipoPaciente = IdTipoPaciente,
                    FechaInicialParam = FechaInicial,
                    FechaFinalParam = FechaFinal,
                    IdProcedimiento = IdProcedimiento,
                    IdZonaArea = IdZonaArea,
                    IdUbicacion = IdUbicacion
                }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            
        }

        #endregion

        #region CodigoAnterior
        //public Procedimiento ObtenerJoin(int id)
        //{
        //    throw new NotImplementedException();
        //    //return _cnn.Query<Procedimiento, Paciente, Procedimiento>("SP_GetMedicalCenterById",
        //    //   (a, b) => { a.Paciente = b; return a; }, param: new { Id = id }, commandType: System.Data.CommandType.StoredProcedure).Single();
        //}

        //public IEnumerable<Procedimiento> ObtenerListaJoin()
        //{
        //    throw new NotImplementedException();
        //    //return _cnn.Query<Procedimiento, Paciente, Procedimiento>("SP_GetAllMedicalCenter",
        //    //    (a, b) => { a.Paciente = b; return a; }, commandType: System.Data.CommandType.StoredProcedure).ToList();
        //}

        //public string Crear(Procedimiento modelo)
        //{
        //    throw new NotImplementedException();
        //    //return _cnn.Query<string>("SP_InsUpdProcedure", param: new
        //    //{
        //    //    Cedula = modelo.Paciente.Cedula,
        //    //    Codigo = modelo.Id,
        //    //    Nombre = modelo.Paciente.Nombre,
        //    //    Apellido = modelo.Paciente.Apellido,
        //    //    Acudiente = modelo.Paciente.Acudiente,
        //    //    Telpaciente = modelo.Paciente.TelPaciente,
        //    //    telacudiente = modelo.Paciente.TelAcudiente,
        //    //    Correo = modelo.Paciente.Correo,
        //    //    Causa = modelo.Causa,
        //    //    Sintoma = modelo.Sintomas,
        //    //    Alergia = modelo.Alergias,
        //    //    Tratamiento = modelo.Tratamiento,
        //    //    Recomendacion = modelo.Recomendaciones,
        //    //    Estado = modelo.Estado,
        //    //    Creado = modelo.Creado,
        //    //    Modificado = modelo.Modificado
        //    //}, commandType: System.Data.CommandType.StoredProcedure).Single();
        //}
        #endregion

    }
}
