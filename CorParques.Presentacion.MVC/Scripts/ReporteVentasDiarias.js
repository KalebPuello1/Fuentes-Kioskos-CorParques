var _Modelo;
var consulta;

$(function () {
    $('#datetimepickerIni').datetimepicker({ format: 'DD/MM/YYYY' });
    $('#datetimepickerFin').datetimepicker({
        useCurrent: false,
        format: 'DD/MM/YYYY'
    });
    $("#datetimepickerIni").on("dp.change", function (e) {
        $('#datetimepickerFin').data("DateTimePicker").minDate(e.date);
    });
    $("#datetimepickerFin").on("dp.change", function (e) {
        $('#datetimepickerIni').data("DateTimePicker").maxDate(e.date);
    });
    asignarSelect2();
    setEventEdit()
    InhabilitaEscritura()
    $("#btnBuscar").click(function () {
        if (validarFormulario("divform")) {
            var fechaInicial = $("#txtFechaInicial").val();
            var fechaFinal = $("#txtFechaFinal").val();
            if (fechaInicial != "") {
                fechaInicial = fechaInicial.substring(6, 10) + "-" + fechaInicial.substring(3, 5) + "-" + fechaInicial.substring(0, 2);
            }
            else {
                fechaInicial = "null";
            }

            if (fechaFinal != "") {
                fechaFinal = fechaFinal.substring(6, 10) + "-" + fechaFinal.substring(3, 5) + "-" + fechaFinal.substring(0, 2);
            }
            else {
                fechaFinal = "null";
            }
            var objeto = new Object();
            objeto.fechaInicial = fechaInicial == "" ? "null" : fechaInicial;
            objeto.fechaFinal = fechaFinal == "" ? "null" : fechaFinal;
            objeto.IdPunto = $("#DDL_Punto").val();
            objeto.idTaquillero = $("#DDL_Taquillero").val();
            objeto.IdFormaPago = $("#DDL_MetodoPago").val();
            objeto.IdFranquicia = $("#DDL_Franquicia").val();
            objeto.CentroBeneficio = $("#DDL_Convenio").val();

            EjecutarAjax(urlBase + "ReporteVentasDirectas/GenerarArchivo", "GET", objeto, "cargarTabla", null);

        }
    });

   
    $("#btnExportarReporteDiario").click(function () {
        if (validarFormulario("divform")) {
            debugger;
            var fecha = $("#txtFechaInicial").val();
            if (fecha != "") {
                fecha = fecha.substring(6, 10) + "-" + fecha.substring(3, 5) + "-" + fecha.substring(0, 2);
            }
            var objeto = new Object();
            objeto.fechaInicial = fecha == "" ? "null" : fecha;
            EjecutarAjax(urlBase + "ReporteVentasDiarias/GenerarArchivoVentasDiarias", "GET", objeto, "cargarTabla", null);
        }
       
    });

    $("#btnExportarInventario").click(function () {
        EjecutarAjax(urlBase + "ReporteVentasDiarias/GenerarArchivoInventario", "GET", null, "cargarTabla", null);

    });


});
function cargarTabla(datos) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante", datos);
        }
        else {
            window.location = urlBase + 'ReporteVentasDirectas/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}


function InhabilitaEscritura() {
    $("#datetimepickerIni, #datetimepickerFin").on("keypress", function (e) {
        e.preventDefault();
    });
    $("#datetimepickerIni, #datetimepickerFin").on("keydown", function (e) {
        e.preventDefault();
    });
}

function InhabilitarCopiarPegarCortar(control) {
    $("#" + control).bind("cut copy paste", function (e) {
        e.preventDefault();
    });
}

function GenerarReporte(data) {
    if (data.length > 0) {
        if (data.indexOf("Error") >= 0) {
            mostrarAlerta("Error al generar reporte: " + data);
        }
        else {
            window.location = urlBase + 'ReporteVentasDirectas/Download?Data=' + data;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}

function setEventEdit() {
    EstablecerToolTipIconos();    
}

function LimpiarControles() {
    //$("#txtFechaInicial").val("");
    //$("#txtFechaFinal").val("");    
    //$("#DDL_Punto").val("").trigger('change');
    //$("#DDL_Taquillero").val("").trigger('change');
    //$("#DDL_MetodoPago").val("").trigger('change');
    //$("#DDL_Franquicia").val("").trigger('change');
    EjecutarAjax(urlBase + "ReporteVentasDirectas/Index", "GET", { modelo: null }, "retorno", null);
}

function retorno() {

}
