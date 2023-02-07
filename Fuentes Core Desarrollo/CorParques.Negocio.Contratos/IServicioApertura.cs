using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

    public interface IServicioApertura : IServicioBase<Apertura>
    {
        IEnumerable<Puntos> ObtenerPuntosSinApertura(DateTime? Fecha = null);
        IEnumerable<Puntos> ObtenerPuntosConApertura(DateTime? Fecha = null);

        IEnumerable<Puntos> ObtenerPuntosParaAperturaElementos();
        IEnumerable<Puntos> ObtenerPuntosAperturaEnProceso();

        IEnumerable<TipoGeneral> ObtenerTipoElementos();

        bool InsertElementos(AperturaElementosInsert modelo);
        bool EliminarElementoPorIdAperturaElemento(AperturaElementos modelo);
        IEnumerable<AperturaElementos> ElementosPorIdPunto(int IdPunto, DateTime Fecha);
        bool InsertarAperturaBrazalete(List<AperturaBrazalete> modelo);
        bool ActualizarAperturaBrazalete(List<AperturaBrazalete> modelo);
        bool ActualizarValidSupervisorElemento(List<AperturaElementos> _listElementos);
        bool ActualizarValidTaquillaElemento(List<AperturaElementos> modelo);
        AperturaBrazalete ObtenerAperturaBrazalete(int id);
        AperturaBrazalete ObtenerAperturaBrazalete(int idSupervisor, int idBrazalete);
        List<AperturaBrazalete> ObtenerListaAperturaBrazalete(int IdSupervisor);
        bool InsertarAperturaBrazaleteDetalle(AperturaBrazaleteDetalle modelo);
        bool ActualizarTotalBrazalete(AperturaBrazalete modelo);
        IEnumerable<Apertura> ObtenerListaApertura(int IdPunto, int IdEstado);
        IEnumerable<Apertura> ObtenerAperturasTaquillero(int IdTaquillero, int IdEstado);

        AperturaElementosHeader ObtenerAperturaElementoHeader(int id);
        string ActualizarAperturaElementoHeader(AperturaElementosHeader aperturaElementosHeader);

        IEnumerable<Puntos> ObtenerPuntosConElementosReabastecimiento();

        /// RDSH: Retorna los brazaletes que tiene asignado un supervisor o taquillero para poder realizar la impresion
        /// de reabastecimiento.
        IEnumerable<CierreBrazalete> ObtenerBoleteriaAsignada(int intIdUsuario, bool blnEsSupervisor);

    }
}
