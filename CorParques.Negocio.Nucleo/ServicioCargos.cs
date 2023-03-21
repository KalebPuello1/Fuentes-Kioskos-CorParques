using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;
using CorParques.Transversales.Contratos;

namespace CorParques.Negocio.Nucleo
{
    /// <summary>
    /// KADM Configuración Cargos
    /// </summary>
	public class ServicioCargos : IServicioCargos
    {
        private readonly IRepositorioCargos _repositorio;
        private readonly IEnvioMails _mails;
        IRepositorioParametros _repositorioParametro;

        #region Constructor

        public ServicioCargos(IRepositorioCargos repositorio, IEnvioMails mails, IRepositorioParametros repositorioParametro)
        {
            _repositorio = repositorio;
            _mails = mails;
            _repositorioParametro = repositorioParametro;
        }

        #endregion

        public bool Insertar(Cargos modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }


        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            return _repositorio.ObtenerListaSimple();
        }


        public IEnumerable<Cargos> ConsultarCargo(Cargos model)
        {
            return _repositorio.ConsultarCargo(model);
        }


        public bool GuardarCargoPerfil(Cargos modelo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cargos> ObtenerTodos()
        {
           return _repositorio.ObtenerLista();
        }

        public Cargos ObtenerxId(int IdCargo)
        {
            return _repositorio.ObtenerxId(IdCargo);
        }

        public Cargos Crear(Cargos modelo)
        {
            throw new NotImplementedException();
        }

        public bool Actualizar(Cargos modelo)
        {
            return _repositorio.Actualizar(modelo);            
        }

        public bool ActualizarEmail(Cargos modelo)
        {
            var body = "Se creado segregacion de funciones.";
            var correo = _repositorioParametro.ObtenerParametroPorNombre("CorreoSegregacion").Valor;
            _mails.EnviarCorreo(correo, "Alerta reabastecimiento", body, System.Net.Mail.MailPriority.Normal, new List<string>());
            return _repositorio.Actualizar(modelo);
        }

        public Cargos Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CargosPerfil> ObtenerCargosPerfil(int idCargo)
        {
            return _repositorio.ObtenerListaCargoPerfil(idCargo);
        }

    }
}
