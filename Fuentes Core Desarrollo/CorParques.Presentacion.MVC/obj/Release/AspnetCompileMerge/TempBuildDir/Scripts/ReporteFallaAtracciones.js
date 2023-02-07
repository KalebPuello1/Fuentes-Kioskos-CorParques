/// <reference path="D:\TFS\Corparques\SRC\Softtek.CorParques\CorParques.Presentacion.MVC\Vendors/jquery/dist/jquery.min.js" />
$(function () {

    loadCalendar();
    MostrarVistaPrevia();
    setNumeric();
    
    $("#idPunto").select2({
        placeholder: "* Seleccione el área"
    });

    $("#idArea").select2({
        placeholder: "* Seleccione el área"
    });

    $('#datetimepickerIni').datetimepicker({ format: 'DD/MM/YYYY' });
    $('#datetimepickerFin').datetimepicker({
        useCurrent: false,
        format: 'DD/MM/YYYY'
    });
    $("#datetimepickerIni").on("dp.change", function (e) {
        $('#datetimepickerFin').data("DateTimePicker").minDate(e.date);
        MostrarVistaPrevia();
    });
    $("#datetimepickerFin").on("dp.change", function (e) {
        $('#datetimepickerIni').data("DateTimePicker").maxDate(e.date);
        MostrarVistaPrevia();
    });




    $("#btnConsultar").click(function () {

        if (!validarFormulario("frmReporteFalla")) {
            return;
        }

        var fechaInicial = $("#FInicial").val();
        var fechaFinal = $("#FFinnal").val();
        var idAtraccion = $("#idPunto").val();
        var idArea = $("#idArea").val();
        var objeto = new Object();
       
        if (fechaInicial != ""){
            fechaInicial = fechaInicial.substring(6, 10) + "-" + fechaInicial.substring(3, 5) + "-" + fechaInicial.substring(0, 2);
        }
        else {
            fechaInicial = "0";
        }

        if (fechaFinal != "") {
            fechaFinal = fechaFinal.substring(6, 10) + "-" + fechaFinal.substring(3, 5) + "-" + fechaFinal.substring(0, 2);
        }
        else {
            fechaFinal = "0";
        }

        objeto.fechaInicial = fechaInicial == "" ? "0" : fechaInicial;
        objeto.fechaFinal = fechaFinal == "" ? "0" : fechaFinal;
        objeto.idArea = idArea;
        objeto.idAtraccion = idAtraccion;

        EjecutarAjax(urlBase + "ReporteFallaAtracciones/GenerarArchivo", "GET", objeto, "cargarTabla", null);
    });

    $("#btnGrid").click(function () {
        if (!validarFormulario("frmReporteFalla")) {
            return;
        }

        var fechaInicial = $("#FInicial").val();
        var fechaFinal = $("#FFinnal").val();
        var idAtraccion = $("#idPunto").val();
        var idArea = $("#idArea").val();
        var objeto = new Object();

        if (fechaInicial != "") {
            fechaInicial = fechaInicial.substring(6, 10) + "-" + fechaInicial.substring(3, 5) + "-" + fechaInicial.substring(0, 2);
        }
        else {
            fechaInicial = "0";
        }

        if (fechaFinal != "") {
            fechaFinal = fechaFinal.substring(6, 10) + "-" + fechaFinal.substring(3, 5) + "-" + fechaFinal.substring(0, 2);
        }
        else {
            fechaFinal = "0";
        }

        objeto.fechaInicial = fechaInicial == "" ? "0" : fechaInicial;
        objeto.fechaFinal = fechaFinal == "" ? "0" : fechaFinal;
        objeto.idArea = idArea;
        objeto.idAtraccion = idAtraccion;

        EjecutarAjax(urlBase + "ReporteFallaAtracciones/VistaPrevia", "GET", objeto, "VistaPrevia", null);
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteFallaAtracciones";
    });
    

});

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante",  datos);
        }
        else {
            window.location = urlBase + 'ReporteFallaAtracciones/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}

function VistaPrevia(datos) {
    $("#div_grilla").html("");
    if (datos != null) {
        if (datos.length > 0) {
            $("#div_grilla").html(datos);
            $("#div_tabla").find("table").DataTable();
            //$(".dataTables_filter").hide();
        } else {
            MostrarMensaje("Importante", "No hay información para mostrar.");
        }
    } else {
        MostrarMensaje("Importante", "No hay información para mostrar.");
    }

}

function MostrarVistaPrevia() {

    var FechaActual = ObtenerFechaActual();

    if ($("#FInicial").val() === FechaActual && $("#FFinnal").val() === FechaActual) {
        $("#btnGrid").show();
    } else {
        $("#btnGrid").hide();
    }

}

