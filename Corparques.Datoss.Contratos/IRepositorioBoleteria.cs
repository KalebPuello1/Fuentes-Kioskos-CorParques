using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioBoleteria : IRepositorioBase<Boleteria>
	{
        string InsertarBoleteriaApp(int tipoBoleta, int producto, double precio);

        // RDSH: Inserta una boleta.
        bool Insertar(Boleteria modelo, out string error);

        // RDSH: Actualiza la tabla de boleteria.
        bool Actualizar(Boleteria modelo, out string error);

        // RDSH: Retorna un objeto boleteria por su consecutivo (codigo de barras).
        Boleteria ObtenerPorConsecutivo(string strConsecutivo);

        ConsultaMovimientoBoletaControl ConsultaMovimientoBoletaControl(string CodBarrasBoletaControl);
        
        int NumeroUsosBoleteria(string strConsecutivo);
        string ObtenerSaldoTarjetaRecargable(string strConsecutivo);

        string CambiarBoleta(List<Boleteria> modelo);
        //string Cambioboleta(string codigo1, string codigo2);

        //RDSH: Retorna un objeto boleteria para el ajuste de medio de pago bono regalo.
        Boleteria ConsultarBonoRegalo(string strConsecutivo);
        //----------------------------------------------------------------------------------------------------------------------------------------------------
        IEnumerable<ReporteBoleteria> ConsultarInventarioDia();
        IEnumerable<RegistroFallasInvOperacion> ConsultarInventarioProdDia(int producto);
        //----------------------------------------------------------------------------------------------------------------------------------------------------
        string InsertarBoleteriaExterna(int tipoBoleta, int producto, double precio, DateTime fechaUso, int Origen, int usuarioCreacion);
        string RegistroRolloImpresionLinea(Producto producto);
        string Cambioboleta(string codigo1);
        Producto CambioboletaDato(string codigo1);
        string UpdateEstadoCambioboleta(string codigo1);
    }
}
