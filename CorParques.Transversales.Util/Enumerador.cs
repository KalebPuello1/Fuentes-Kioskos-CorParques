using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Transversales.Util
{
    public static class Enumerador
    {

        public enum ModulosAplicacion : int
        {
            TipoTarifaParqueadero = 1,
            TarifasParqueadero = 2,
            GrupoNotificacion = 3,
            TipoBrazalete = 4,
            GestionMantenimiento = 5,
            CargueBrazaletes = 6,
            Atracciones = 7,
            CentroMedico = 8,
            ConvenioParqueadero = 9,
            Puntos = 10,
            Perfil = 11,
            CortesiasPorPunto = 12,
            ConvenioDescuento = 13,
            CierreElementos = 14,
            Ubicaciones = 15
        }


        public enum Estados : int
        {
            Activo = 1,
            Inactivo = 2,
            Anulado = 3,
            Cerrado = 4,
            Mantenimiento = 5,
            Abierto = 6,
            EnProceso = 7,
            Entregado = 8,
            EntregadoSupervisor = 9,
            EntregadoNido = 10,
            ElementoEntregado = 11,
            ElementoNoEntregado = 12,
            ElementoAgotado= 13,
            ElementoDefectuoso= 14,
            Procesado = 15,
            Bloqueado = 16
        }
        //Cambioquitar:
        public enum TiposPuntos : int
        {
            Atraccion = 1,
            Taquilla = 3,
            Comida = 4,
            Destreza = 5,
            Parqueadero = 6,
            Almacenes = 7
        }

        //Cambioquitar: revisar
        public enum LineaProducto : int
        {
            //Souvenir = 2,
            //Brazaletes = 7,
            //Bonos = 8,
            //Parqueadero = 10,
            //PasaporteUso = 11,
            //AtraccionesNegocio = 12,
            Propina = 13,
            Brazaletes= 1,
            Pasaporte= 2,
            Atracciones= 3,
            Destrezas= 4,
            Cupo= 5,
            AyB= 6,
            Souvenires= 7,
            Servicios= 8,
            PremiosDestrezas = 9

        }

        //Cambioquitar: Se  comentarea para usar en TB_Parametros GALD
        //public static class CodigosSapTipoProducto
        //{
        //    public static string Brazaletes = "2000";
        //    public static string Pasaporte = "2005";
        //    public static string Atracciones = "2010";
        //    public static string Destrezas = "2015";
        //    public static string Cupo = "2020";
        //    public static string AyB = "2025";
        //    public static string Souvenires = "2030";
        //    public static string Servicios = "2035";
        //    public static string PremiosDestrezas = "2040";
        //    public static string Propina = "2000";
        //}

        //Cambioquitar: Se  comentarea para usar en TB_Parametros GALD
        //public static class CodigosSapProducto
        //{
        //    public static string Propina = "60000096";
        //}

        public enum TipoRecibo : int
        {
            UnaColumna = 0,
            DosColumna = 1,
            TresColumna = 2,
            CuatroColumna = 3
        }


        //Cambioquitar:
        public enum Perfiles : int
        {
            Administrador = 1
        }

        public enum TipoRecoleccion : int
        {
            Base = 1,
            Corte = 2,
            Voucher = 3,
            Documentos = 4,
            CierreTaquilla = 5,
            Novedad = 6
        }
        public enum Alfabeto : int
        {
            zzzzzz,
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            H,
            I,
            J,
            K,
            L,
            M,
            N,
            O,
            P,
            Q,
            R,
            S,
            T,
            U,
            V,
            W,
            X,
            Y,
            Z,
            AA,
            AB,
            AC,
            AD,
            AE,
            AF,
            AG,
            AH,
            AI,
            AJ,
            AK,
            AL,
            AM,
            AN,
            AO,
            AP,
            AQ,
            AR,
            AS,
            AT,
            AU,
            AV,
            AW,
            AX,
            AY,
            AZ
        }

        public enum MediosPago : int
        {
             Efectivo = 1
            ,TarjetaDebito = 2
            ,TarjetaCredito = 3
            ,Bonosregalo = 4
            ,ChequesSodexo = 5
            ,DescuentoNomina = 6
            ,DocumentoBrinks = 7
            ,DocumentoPayU = 8
            ,TarjetaRecargable = 9
            ,APP = 10
        }

        public enum TipoNovedadArqueo : int
        {
            Efectivo = 1,
        	Voucher = 2,
            Documentos = 3
        }
        public enum  SerieCodigoBarras : int
        {
            Factura = 1,
            CupoDebito = 2,
            PasaporteUso = 3
        }
        public enum TipoDenominacion : int
        {
            Billete = 1,
            Moneda = 2
        }

        public enum OrigenBrazalete : int
        {
            App = 1,
            Pagina = 2,
            ImpresionLinea = 3
        }

        public static class RedebanValues
        {
            public const double BaseDevolucion = 0;
            public const double ImpuestoConsumo = 0;
            public const double Iva = 0;
            public const string CodigoCajero = "0";
            public const double Propina = 0;
            public const bool PropinaHabilitada = false;
            public const string OperacionPago = "0";
            public const string OperacionAnulacion = "01";
            public const string CodigoErrorConfiguracion = "99";
            public const string CodigoCompraAprobada = "00";
            public const string CodigoCompraDeclinada = "01";
            public const string CodigoCompraPinIncorrecto = "02";
            public const string CodigoCompraEntidadNoResponde = "04";
            public const string ClaveSupervisorAnulacion = "0000";
            public const string ClaveAdministradorAnulacion = "000000";
            public const string EventoInicio = "INICIO";
            public const string EventoInicioAnulacion = "INICIO_ANULACION";
            public const string EventoFinalizacion = "FIN";
            public const string EventoFinalizacionAnulacion = "FIN_ANULACION";
        }
    }
}
