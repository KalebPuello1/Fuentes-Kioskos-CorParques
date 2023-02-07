$(function () {

    loadCalendar();
    setNumeric();


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
        var FechaInicial = $("#FechaInicial").val();
        var FechaFinal = $("#FechaFinal").val();


        var objeto = new Object();

        if (FechaInicial != "") {
            FechaInicial = FechaInicial.substring(6, 10) + "-" + FechaInicial.substring(3, 5) + "-" + FechaInicial.substring(0, 2);
        }
        if (FechaFinal != "") {
            FechaFinal = FechaFinal.substring(6, 10) + "-" + FechaFinal.substring(3, 5) + "-" + FechaFinal.substring(0, 2);
        }

        objeto.FechaInicial = FechaInicial;
        objeto.FechaFinal = FechaFinal;

        if (validarFormulario("listView")) {
            EjecutarAjax(urlBase + "ReporteVentasPorConvenio/ObtenerReporte", "GET", objeto, "cargarTabla", null);
        }
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteVentasPorConvenio";
    });


});

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante", datos);
        }
        else {
            window.location = urlBase + 'ReporteVentasPorConvenio/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }

}


