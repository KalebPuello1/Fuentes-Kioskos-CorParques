var _Modelo;
var consulta;

$(function () {

    loadCalendar();

    $('#datetimepickerIni').datetimepicker({ format: 'YYYY-MM-DD' });

    $("#btnConsultar").click(function () {
        if (validarFormulario("listView")) {
            //EjecutarAjax(urlBase + "ReporteRecoleccion/GenerarArchivo/", "GET", { Fecha: $("#Fecha").val(), Taquillero: $("#IdTaquillero").val(),FormaRecoleccion: $("#IdFormaRecoleccion") }, "cargarTabla", null);
            EjecutarAjax(urlBase + "ReporteRecoleccion/GenerarArchivo", "GET", { Fecha: $("#txtFecha").val(), IdTaquillero: $("#DDL_Taquillero").val() == "" ? "null" : $("#DDL_Taquillero").val(), IdSupervisor: $("#DDL_Supervisor").val() == "" ? "null" : $("#DDL_Supervisor").val() }, "cargarTabla", null);
        }
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteRecoleccion";
    });


});


function cargarTabla(datos) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante", datos);
        }
        else {
            window.location = urlBase + 'ReporteRecoleccion/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}


/*function InhabilitaEscritura() {
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
}*/

function GenerarReporte(data) {
    if (data.length > 0) {
        if (data.indexOf("Error") >= 0) {
            mostrarAlerta("Error al generar reporte: " + data);
        }
        else {
            window.location = urlBase + 'ReporteRecoleccion/Download?Data=' + data;
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
    EjecutarAjax(urlBase + "ReporteRecoleccion/Index", "GET", { modelo: null }, "retorno", null);
}

function retorno() {

}