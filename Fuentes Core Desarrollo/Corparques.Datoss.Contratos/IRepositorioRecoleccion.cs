using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

    public interface IRepositorioRecoleccion
    {

        //RDSH: Inserta una recoleccion.
        bool Insertar(Recoleccion modelo, out string error, out int IdRecoleccion);

        //RDSH: Actualiza una recoleccion.
        bool Actualizar(Recoleccion modelo, out string error);

        //RDSH: Elimina una recoleccion.
        bool Eliminar(Recoleccion modelo, out string error);

        //RDSH: Retorna un objeto Recoleccion para la edicion.
        Recoleccion ObtenerPorId(int Id);

        //RDSH: Retorna un objeto Recoleccion por id usuario y por punto para su edicion.
        Recoleccion ObtenerRecoleccionActiva(int IdUsuario, int IdPunto, bool Cierre, int IdEstado);

        // RDSH: Retorna los documentos pendientes de recolección.
        IEnumerable<MediosPagoFactura> ObtenerDocumentosRecoleccion(int IdUsuario);

        //RDSH: Retorna si se muestra la recoleccion base, corte y los topes para cada una de estas.
        RecoleccionAuxliar ObtenerTopesRecoleccion(int intIdUsuario, int intIdPunto);
        /// <summary>
        /// Obtiene las notificaciones para recoleccion.
        /// </summary>
        /// <returns></returns>
        IEnumerable<NotificacionAlerta> ObtenerNotificaciones();

        //RDSH: Retorna la cantidad maxima de dinero que se puede recolectar para cierre.
        RecoleccionAuxliar ObtenerTopesCierreTaquilla(int intIdUsuario, int intIdPunto);

        /// RDSH: Retorna la cantidad de brazaletes restantes.
        IEnumerable<CierreBrazalete> ObtenerBrazaletesRestantes(int intIdUsuario, int intIdPunto);

        //RDSH: Consulta los puntos que tienen recoleccion segun el estado y si la recoleccion es de cierre o no.
        IEnumerable<TipoGeneralValor> ObtenerPuntosRecoleccion(int intIdEstado, bool blnCierre);

        //RDSH: Consulta los taquilleros que tienen alistamiento de cierre o de recoleccion, esto para el proceso de entrega a supervisor.
        IEnumerable<TipoGeneral> ObtenerTaquillerosConRecoleccion(int intIdEstado, bool blnCierre);

        //RDSH: Retorna las novedades pendientes de recolección.
        ///TOCA CAMBIAR MediosPagoFactura POR LA ENTIDAD QUE SE CREE PARA LA TABLA [TB_NovedadArqueo]
        IEnumerable<NovedadArqueo> ObtenerNovedadesRecoleccion(int IdUsuario);

        //RDSH: Borrado del detalle de las recolecciones al editar una recoleccion y desmarcar los voucher, documentos, novedad.
        bool EliminarDetalleRecoleccion(int intIdRecoleccion, int intIdTipoRecoleccion);

        /// RDSH: Retorna las recolecciones activas.
        IEnumerable<Recoleccion> ObtenerRecoleccionesActivas(int intIdUsuario, int intIdPunto, bool blnCierre, int IdEstado);

        string ObtenerNumBoletaImpresionEnLinea();

        //Se agrego para impresion en linea
        int ControlBoleteria(int idBoleta);

        string ModificarControlBoleteria(int idBoleta, int NumBoletasRestantes);

        Producto ValidarImpresionEnLinea(int idBoleteria);

        IEnumerable<Producto> NumBoletasControlBoleteria();

        IEnumerable<Producto> VerPasaportesCodigoPedido(string codigoPedido);

    }
}
