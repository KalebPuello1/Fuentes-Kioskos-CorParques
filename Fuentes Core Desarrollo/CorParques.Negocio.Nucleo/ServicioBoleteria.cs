using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;

namespace CorParques.Negocio.Nucleo
{

    public class ServicioBoleteria : IServicioBoleteria
    {

        private readonly IRepositorioBoleteria _repositorio;

        #region Constructor
        public ServicioBoleteria(IRepositorioBoleteria repositorio)
        {

            _repositorio = repositorio;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Actualiza la tabla de boleteria.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(Boleteria modelo, out string error)
        {
            return _repositorio.Actualizar(modelo, out error);
        }

        /// <summary>
        /// RDSH: Inserta una boleta.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(Boleteria modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }
        /// <summary>
        /// RDSH: Inserta una boleta.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public string InsertarBrazalete(int tipoBoleta, int producto, double precio)
        {
            return _repositorio.InsertarBoleteriaApp(tipoBoleta, producto, precio);
        }

        /// <summary>
        /// RDSH: Retorna un objeto boleteria por su consecutivo (codigo de barras).
        /// </summary>
        /// <param name="strConsecutivo"></param>
        /// <returns></returns>
        public Boleteria ObtenerPorConsecutivo(string strConsecutivo)
        {
            return _repositorio.ObtenerPorConsecutivo(strConsecutivo);
        }
        public ConsultaMovimientoBoletaControl ConsultaMovimientoBoletaControl(string CodBarrasBoletaControl)
        {
            return _repositorio.ConsultaMovimientoBoletaControl(CodBarrasBoletaControl);
        }
        
        /// <summary>
        /// DANR: Retorna numero de usos de boleteria en el dia.
        /// </summary>
        /// <param name="strConsecutivo"></param>
        /// <returns></returns>
        public int NumeroUsosBoleteria(string strConsecutivo)
        {
            return _repositorio.NumeroUsosBoleteria(strConsecutivo);
        }


        /// <summary>
        /// DANR: Retorna saldo tarjeta recargable.
        /// </summary>
        /// <param name="strConsecutivo"></param>
        /// <returns></returns>
        public string ObtenerSaldoTarjetaRecargable(string strConsecutivo)
        {
            return _repositorio.ObtenerSaldoTarjetaRecargable(strConsecutivo);
        }
        /// <summary>
        /// RDSH: Retorna una boleta por su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boleteria Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        public bool BloquearBoleta(Boleteria modelo)
        {
            var _modelo = Obtener(modelo.IdBoleteria);
            _modelo.IdUsuarioModificacion = modelo.IdUsuarioModificacion;
            _modelo.IdUsuarioBloqueo = modelo.IdUsuarioModificacion;
            _modelo.PuntoBloqueo = modelo.PuntoBloqueo;
            _modelo.FechaBloqueo = DateTime.Now;
            _modelo.IdEstado = modelo.IdEstado;

            var rta = _repositorio.Actualizar(ref _modelo);
            return rta;
        }

        public Boleteria ObtenerBoleta(string CodBarra)
        {
            var rta = _repositorio.ObtenerLista($"WHERE Consecutivo = '{CodBarra}'");

            if (rta != null && rta.Count() > 0)
            {
                var codSap = rta.First().CodigoVenta;
                var idProducto = rta.First().IdProducto;
                var fecha = _repositorio.ExecuteQuery(" select top 1 FechaFinEvento from TB_SolicitudBoleteria where CodigoVenta = '" + codSap + "' and IdProducto= " + idProducto + ";", null);
                if (fecha != null && fecha.Count() > 0)
                    rta.First().FechaFinEvento = fecha.First().FechaFinEvento;
            }
            return rta != null && rta.Count() > 0 ? rta.First() : null;
        }

        public string CambiarBoleta(List<Boleteria> modelo)
        {
            return _repositorio.CambiarBoleta(modelo);
        }

        /// <summary>
        /// RDSH: Retorna un objeto boleteria para el ajuste de medio de pago bono regalo.
        /// </summary>
        /// <param name="strConsecutivo"></param>
        /// <returns></returns>
        public Boleteria ConsultarBonoRegalo(string strConsecutivo)
        {
            return _repositorio.ConsultarBonoRegalo(strConsecutivo);
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------
        public IEnumerable<ReporteBoleteria> ConsultarInventarioDia()
        {
            return _repositorio.ConsultarInventarioDia();
        }


        public IEnumerable<RegistroFallasInvOperacion> ConsultarInventarioProdDia(int producto)
        {
            return _repositorio.ConsultarInventarioProdDia(producto);
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------
        public string Cambioboleta(string codigo1, string codigo2)
        {
            return _repositorio.Cambioboleta(codigo1);
        }

        public Producto CambioboletaDato(string codigo1, string codigo2)
        {
            return _repositorio.CambioboletaDato(codigo1);
        }

        public string UpdateEstadoCambioboleta(string codigo1)
        {
            return _repositorio.UpdateEstadoCambioboleta(codigo1);
        }
        #endregion

        #region Metodos no implementados

        public bool Actualizar(Boleteria modelo)
        {
            throw new NotImplementedException();
        }

        public Boleteria Crear(Boleteria modelo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Boleteria> ObtenerTodos()
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}
