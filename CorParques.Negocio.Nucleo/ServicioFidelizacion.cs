using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;
using CorParques.Transversales.Util;
using CorParques.Transversales.Contratos;
using System.Configuration;

namespace CorParques.Negocio.Nucleo
{

    public class ServicioFidelizacion : IServicioFidelizacion
    {

        private readonly IRepositorioFidelizacion _repositorio;
        private readonly IEnvioMails _mails;
        #region Constructor

        public ServicioFidelizacion(IRepositorioFidelizacion repositorio, IEnvioMails mails)
        {
            _repositorio = repositorio;
            _mails = mails;
        }
        #endregion

        #region Metodos
        public bool Actualizar(ClienteFideliacion modelo)
        {
            var cliente = Obtener(modelo.Documento);
            
            if (modelo.Foto == null)
                modelo.Foto = cliente.Foto;
            if (_repositorio.Actualizar(ref modelo))
            {
                _mails.correoFidelizacion(modelo.FechaNacimiento, modelo.Nombre, modelo.Correo, modelo.Telefono, new List<string> { $"{System.AppDomain.CurrentDomain.BaseDirectory}\\AdjuntosCorreo\\contrato.pdf" });
                
                return true; 
            }
            else
                return false;
        }
        public bool BloquearTarjeta(string consecutivo, int usuario, int punto)
        {
            return _repositorio.BloquearTarjeta(consecutivo,usuario,punto);
        }

        public bool RedimirProduto(string consecutivo, int producto, int usuario, int punto)
        {
            return _repositorio.RedimirProduto(consecutivo, producto, usuario, punto);
        }
        public ClienteFideliacion Crear(ClienteFideliacion modelo)
        {
            throw new NotImplementedException();
        }
        public ClienteFideliacion Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public ClienteFideliacion Obtener(string id)
        {
            return _repositorio.ObtenerClienteTarjeta(id);
        } 
        public ClienteFideliacion ObtenerClienteTarjeta(string doc)
        {
            return _repositorio.ObtenerClienteTarjeta(doc);
        }
        public ClienteFideliacion ObtenerTarjetaSaldoPuntos(string consecutivo)
        {
            return _repositorio.ObtenerTarjetaSaldoPuntos(consecutivo);

        }
        public IEnumerable<TipoGeneral> ObtenerProductosRedimibles(int puntos)
        {
            return _repositorio.ObtenerProductosRedimibles(puntos);
        }

        public IEnumerable<ClienteFideliacion> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        


        
        #endregion


        
    }
}
