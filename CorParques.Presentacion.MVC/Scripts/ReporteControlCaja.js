/// <reference path="D:\TFS\Corparques\SRC\Softtek.CorParques\CorParques.Presentacion.MVC\Vendors/jquery/dist/jquery.min.js" />
$(function () {

    loadCalendar();
    setNumeric();
    
    $("#idRecolector").select2({
        placeholder: "* .. Seleccione .."
    });



    $("#idTBoleteria").select2({
        placeholder: "* .. Seleccione .."
    });

    $("#idMPago").select2({
        placeholder: "* .. Seleccione .."
    });

    $("#idSupervisor").select2({
        placeholder: "* .. Seleccione .."
    });

    $("#idTaquillero").select2({
        placeholder: "* .. Seleccione .."
    });

    $("#idPerfil").select2({
        placeholder: "* .. Seleccione .."
    });
    

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




    $("#btnConsultar").click(function () {
        if (validarFormulario("listView *")) {
            var fechaInicial = $("#FInicial").val();
            var fechaFinal = $("#FFinnal").val();
            var idAtraccion = $("#idPunto").val();

            var idPerfil = $("#idPerfil").val();
            var idTaquillero = $("#idTaquillero").val();
            var idSupervisor = $("#idSupervisor").val();
            var idMPago = $("#idMPago").val();
            var idRecolector = $("#idRecolector").val();
            var idTBoleteria = $("#idTBoleteria").val();


            var objeto = new Object();

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

            objeto.fechaInicial = fechaInicial == "" ? "null" : fechaInicial;
            objeto.fechaFinal = fechaFinal == "" ? "null" : fechaFinal;
            objeto.idPerfil = idPerfil;
            objeto.idTaquillero = idTaquillero;
            objeto.idSupervisor = idSupervisor;
            objeto.idMPago = idMPago
            objeto.idRecolector = idRecolector;
            objeto.idTBoleteria = idTBoleteria;

            EjecutarAjax(urlBase + "ReporteControlCaja/GenerarArchivo", "GET", objeto, "cargarTabla", null);
        }
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteControlCaja";
    });
    

});

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante",  datos);
        }
        else {
            window.location = urlBase + 'ReporteControlCaja/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}


