using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

    public interface IServicioBoleteria : IServicioBase<Boleteria>
    {
        string InsertarBrazalete(int tipoBoleta, int producto, double precio);

        // RDSH: Inserta una boleta.
        bool Insertar(Boleteria modelo, out string error);

        // RDSH: Actualiza la tabla de boleteria.
        bool Actualizar(Boleteria modelo, out string error);

        // RDSH: Retorna un objeto boleteria por su consecutivo (codigo de barras).
        Boleteria ObtenerPorConsecutivo(string strConsecutivo);

        ConsultaMovimientoBoletaControl ConsultaMovimientoBoletaControl(string CodBarrasBoletaControl);

        
        int NumeroUsosBoleteria(string strConsecutivo);
        string ObtenerSaldoTarjetaRecargable(string strConsecutivo);

        //EDSP: Bloquea la boleta

        bool BloquearBoleta(Boleteria modelo);

        Boleteria ObtenerBoleta(string CodBarra);

        string CambiarBoleta(List<Boleteria> modelo);

        //RDSH: Retorna un objeto boleteria para el ajuste de medio de pago bono regalo.
        Boleteria ConsultarBonoRegalo(string strConsecutivo);
        IEnumerable<ReporteBoleteria> ConsultarInventarioDia();
        IEnumerable<RegistroFallasInvOperacion> ConsultarInventarioProdDia(int producto);
        string Cambioboleta(string codigo1, string codigo2);
        Producto CambioboletaDato(string codigo1, string codigo2);
        string UpdateEstadoCambioboleta(string codigo1);
    }
}
