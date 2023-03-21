using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;
using CorParques.Transversales.Contratos;
using CorParques.Transversales.Util;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioNotificacion : IServicioNotificacion
    {

        private readonly IRepositorioNotificacion _repositorio;

        public ServicioNotificacion(IRepositorioNotificacion repositorio, IRepositorioUsuario repositorioUsuario, IEnvioMails mail)
        {
            _repositorio = repositorio;
        }

        public bool CambiarEstado(int id, int idUsuario)
        {
            var notificacion = _repositorio.Obtener(id);
            notificacion.IdEstado = (int)Enumerador.Estados.Inactivo;
            notificacion.UsuarioModificacion = idUsuario;
            notificacion.FechaModificacion = DateTime.Now;
            return _repositorio.Actualizar(ref notificacion);
        }

        public bool Enviar(Notificacion modelo)
        {
            try
            {
                var puntos = _repositorio.ObtenerPuntos(modelo.ListaGrupos);
                var registrosInsercion = puntos.Select(x => new Notificacion { Asunto = modelo.Asunto, Contenido = modelo.Contenido, FechaCreacion = DateTime.Now, IdEstado = (int)Enumerador.Estados.Activo, IdPunto = x, UsuarioCreacion = modelo.UsuarioCreacion, Prioritario=modelo.Prioritario });
                foreach (var item in registrosInsercion)
                {
                    var elemento = item;
                    _repositorio.Insertar(ref elemento);
                    item.Id = elemento.Id;
                }
                return true;

            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public IEnumerable<Notificacion> Obtener(int id)
        {
            return _repositorio.ObtenerNotificacionesXPunto(id);
        }

        public IEnumerable<Notificacion> ObtenerNotificaciones()
        {
            return _repositorio.ObtenerNotificaciones();
        }

    }
}
