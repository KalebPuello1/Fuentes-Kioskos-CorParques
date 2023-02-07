/// <reference path="D:\TFS\Corparques\SRC\Softtek.CorParques\CorParques.Presentacion.MVC\Vendors/jquery/dist/jquery.min.js" />
$(function () {

    loadCalendar();
    setNumeric();

    $("#idMedioPago").select2({
        placeholder: "* Seleccione el área"
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

        var fechaInicial = $("#FInicial").val();
        var fechaFinal = $("#FFinal").val();
        var txtConsecutivo = $("#txtConsecutivo").val();
        var txtCliente = $("#txtCliente").val();
        var idMedioPago = $("#idMedioPago").val();

        var objeto = new Object();

        if (fechaInicial != "") {
            fechaInicial = fechaInicial.substring(6, 10) + "-" + fechaInicial.substring(3, 5) + "-" + fechaInicial.substring(0, 2);
        }
        else {
            fechaInicial = "nulo";
        }

        if (fechaFinal != "") {
            fechaFinal = fechaFinal.substring(6, 10) + "-" + fechaFinal.substring(3, 5) + "-" + fechaFinal.substring(0, 2);
        }
        else {
            fechaFinal = "nulo";
        }

        //valida si solo se selecciona la fecha inicial, para establecerla como fecha unica de consulta
        if (fechaInicial != "nulo" && fechaFinal == "nulo")
        {
            fechaFinal = fechaInicial;
        }

        objeto._FechaInicial = fechaInicial == "" ? "nulo" : fechaInicial;
        objeto._FechaFinal = fechaFinal == "" ? "nulo" : fechaFinal;
        objeto._Consecutivo = txtConsecutivo == "" ? "nulo" : txtConsecutivo;
        objeto._Cliente = txtCliente == "" ? "nulo" : txtCliente.replace(/\./g, '_');
        objeto._FormaPago = "nulo";
        objeto._Entidad = "nulo";

        //if (validarFormulario("listView")) {
            EjecutarAjax(urlBase + "ReporteRecaudosVentas/GenerarArchivo", "GET", objeto, "cargarTabla", null);
        //}
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteRecaudosVentas";
    });


});

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante", datos);
        }
        else {
            window.location = urlBase + 'ReporteRecaudosVentas/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}


