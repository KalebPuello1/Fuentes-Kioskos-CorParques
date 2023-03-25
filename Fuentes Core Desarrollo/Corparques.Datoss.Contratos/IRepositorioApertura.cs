using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioApertura : IRepositorioBase<Apertura>
	{
        IEnumerable<Puntos> ObtenerPuntosSinApertura(DateTime? Fecha);
        IEnumerable<Puntos> ObtenerPuntosConApertura(DateTime? Fecha);

        IEnumerable<Puntos> ObtenerPuntosParaAperturaElementos();        
        IEnumerable<Puntos> ObtenerPuntosAperturaEnProceso();

        IEnumerable<TipoGeneral> ObtenerTipoElementos();

        IEnumerable<AperturaElementos> ObtenerElementosPorIdApertura(int IdApertura);

        int InsertarAperturaElemento(AperturaElementos modelo);
        bool EliminarElementoPorIdAperturaElemento(AperturaElementos modelo);
        int InsertarAperturaElementoHeader(AperturaElementosHeader modelo);
        int ObtieneIdAperturaElementoHeader(int _Punto, DateTime _Fecha);

        int ObtenerIdAperturaPorPunto(int IdPunto);

        int ActualizarValidSupervisor(int IdElemento, bool CheckSupervisor);
        int ActualizarValidTaquilla(int IdElemento, bool CheckTaquilla);

        IEnumerable<AperturaElementos> ObtenerElementosPorIdPunto(int _IdPunto, DateTime _Fecha);
        AperturaElementosHeader ObtenerAperturaElementoHeader(int id);
        string ActualizarAperturaElementoHeader(AperturaElementosHeader aperturaElementosHeader);

        IEnumerable<Puntos> ObtenerPuntosConElementosReabastecimiento();

        //RDSH: Valida si taquillero tiene una apertura activa.
        string ValidaPermiteAnularUsuario(int intIdUsuario);

        /// RDSH: Retorna los brazaletes que tiene asignado un supervisor o taquillero para poder realizar la impresion
        /// de reabastecimiento.
        IEnumerable<CierreBrazalete> ObtenerBoleteriaAsignada(int intIdUsuario, bool blnEsSupervisor);
    }
}
