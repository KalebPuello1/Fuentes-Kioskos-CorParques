/// <reference path="D:\TFS\Corparques\SRC\Softtek.CorParques\CorParques.Presentacion.MVC\Vendors/jquery/dist/jquery.min.js" />
$(function () {

    loadCalendar();
    setNumeric();
    
    $("#cliente").select2({
        placeholder: "* Seleccione el cliente"
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
        var fechaFinal = $("#FFinnal").val();
        var cliente = $("#cliente").val();
        var pedido = $("#pedido").val();
        var factura = $("#factura").val();
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

        objeto.fechaInicial = fechaInicial == "" ? "0" : fechaInicial;;
        objeto.fechaFinal = fechaFinal == "" ? "0" : fechaFinal;
        objeto.cliente = cliente == "" ? "null" : cliente;
        objeto.pedido = pedido == "" ? "null" : pedido;
        objeto.factura = factura == "" ? "null" : factura;
        if (validarFormulario("listView")) {
            EjecutarAjax(urlBase + "ReporteAprovechamientoFA/GenerarReporte", "GET", objeto, "cargarTabla", null);
        }
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteAprovechamientoFA";
    });
    

});

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante", datos);
        }
        else {
            window.location = urlBase + 'ReporteAprovechamientoFA/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}


