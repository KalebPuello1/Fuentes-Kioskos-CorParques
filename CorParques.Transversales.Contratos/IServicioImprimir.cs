using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Transversales;
using CorParques.Negocio.Entidades;

namespace CorParques.Transversales.Contratos
{
    public interface IServicioImprimir
    {
        /// <summary>
        /// RDSH: Para generar impresión de cortesías.
        /// </summary>
        /// <param name="objTicket"></param>
        /// <returns></returns>
        string ImprimirTicketCortesias(TicketImprimir objTicket);

        /// <summary>
        /// RDSH: Genera impresión de alistamiento recoleccion.
        /// </summary>
        /// <param name="objTicket"></param>
        /// <returns></returns>
        string ImprimirTicketRecoleccion(TicketImprimir objTicket, bool blnAlistamiento, bool blnDataTable);
        
        /// <summary>
        /// GALD: Para generar impresíon de apertura.
        /// </summary>
        /// <param name="objTicket"></param>
        /// <returns></returns>
        string ImprimirTicketApertura(TicketImprimir objTicket);

        /// <summary>
        ///  RDSH: Impresion de varios tickets en un solo recibo.
        /// </summary>
        /// <param name="objTicket"></param>
        /// <returns></returns>
        string ImprimirTicketMasivo(TicketImprimir objTicket);


        /// <summary>
        /// GALD: Para generar impresíon de apertura..
        /// </summary>
        /// <param name="objTicket"></param>
        /// <returns></returns>
        string ImprimirTicketArqueo(TicketImprimir objTicket);

        /// <summary>
        /// RDSH: Genera impresión de elementos para el cierre de punto.
        /// </summary>
        /// <param name="objTicket"></param>
        /// <returns></returns>
        string ImprimirTicketCierrePunto(TicketImprimir objTicket);

        /// <summary>
        /// GALD: Para generar impresíon de apertura a taquillero.
        /// </summary>
        /// <param name="objTicket"></param>
        /// <returns></returns>
        string ImprimirTicketAperturaTaquillero(TicketImprimir objTicket);

        /// <summary>
        /// RDSH: Genera la impresion de las boletas de adicion de pedido.
        /// </summary>
        /// <param name="objTicket"></param>
        /// <returns></returns>
        string ImprimirAdicionPedido(TicketImprimir objTicket);

    }
}
