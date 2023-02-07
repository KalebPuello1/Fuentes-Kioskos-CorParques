/// <reference path="D:\TFS\Corparques\SRC\Softtek.CorParques\CorParques.Presentacion.MVC\Vendors/jquery/dist/jquery.min.js" />
$(function () {

    loadCalendar();
    setNumeric();

    $("#selectPunto").select2({
        placeholder: "* Seleccione..."
    });

    $("#selectTipoBoleta").select2({
        placeholder: "* Seleccione..."
    });

    $("#ddlTipoVenta").select2({
        placeholder: "* Seleccione..."
    });

    $("#selectCliente").select2({
        placeholder: "* Seleccione..."
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
        if (validarFormulario("listview *")) {
            var FechaInicial = $("#txtFechaInicial").val();
            var FechaFinal = $("#txtFechaFinal").val();
            var CodigoPunto = $("#selectPunto").val();            
            var tipoVenta = $("#ddlTipoVenta").val();
            var NombreTipoBoleta = $("#selectTipoBoleta").val();
            var NombreCliente = $("#selectCliente").val();
            var objeto = new Object();

            objeto.FechaInicial = FechaInicial;
            objeto.FechaFinal = FechaFinal;
            objeto.CodigoPunto = CodigoPunto;            
            objeto.TipoVenta = tipoVenta;
            objeto.NombreTipoBoleta = NombreTipoBoleta;
            objeto.NombreCliente = NombreCliente;

            EjecutarAjax(urlBase + "ReporteDestrezas/GenerarArchivo", "GET", objeto, "cargarTabla", null);
        }
        else {
            $("#datetimepickerIni").attr("data-mensajeerror", "");
            $("#datetimepickerFin").attr("data-mensajeerror", "");
        }
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteDestrezas";
    });


});

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante", datos);
        }
        else {
            window.location = urlBase + 'ReporteDestrezas/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}
