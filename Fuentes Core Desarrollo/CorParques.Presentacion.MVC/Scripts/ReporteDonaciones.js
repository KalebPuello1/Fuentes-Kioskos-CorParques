/// <reference path="D:\TFS\Corparques\SRC\Softtek.CorParques\CorParques.Presentacion.MVC\Vendors/jquery/dist/jquery.min.js" />
$(function () {

    loadCalendar();
    setNumeric();
    
    $("#Producto").select2({
        placeholder: "* Seleccione un producto"
    });
    $("#Punto").select2({
        placeholder: "* Seleccione un punto"
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
        var FechaInicial = $("#FInicial").val();
        var FechaFinal = $("#FFinnal").val();
        var Punto= $("#Punto").val();
        var Producto = $("#Producto").val();
        var objeto = new Object();
       
        if (FechaInicial != "") {
            FechaInicial = FechaInicial.substring(6, 10) + "-" + FechaInicial.substring(3, 5) + "-" + FechaInicial.substring(0, 2);
        }
        else {
            FechaInicial = "0";
        }

        if (FechaFinal != "") {
            FechaFinal = FechaFinal.substring(6, 10) + "-" + FechaFinal.substring(3, 5) + "-" + FechaFinal.substring(0, 2);
        }
        else {
            FechaFinal = "0";
        }

        objeto.fechaInicial = FechaInicial == "" ? "0" : FechaInicial;;
        objeto.fechaFinal = FechaFinal == "" ? "0" : FechaFinal;
        objeto.IdProducto = Producto == "" ? "null" : Producto;
        objeto.IdPunto = Punto == "" ? "null" : Punto;
        if (validarFormulario("listView")) {
            EjecutarAjax(urlBase + "ReporteDonaciones/GenerarReporte", "GET", objeto, "cargarTabla", null);
        }
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteDonaciones";
    });
    

});

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante", datos);
        }
        else {
            window.location = urlBase + 'ReporteDonaciones/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}


