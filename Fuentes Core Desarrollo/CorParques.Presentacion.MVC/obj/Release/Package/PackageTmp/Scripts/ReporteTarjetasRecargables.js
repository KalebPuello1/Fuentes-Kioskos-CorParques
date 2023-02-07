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
    $('#datetimepickerCompra').datetimepicker({ format: 'DD/MM/YYYY' });
    $('#datetimepickerVencimiento').datetimepicker({ format: 'DD/MM/YYYY' });
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
        var FechaCompra = $("#FCompra").val();
        var FechaVencimiento = $("#FVencimiento").val();
        var Cliente = $("#Cliente").val();
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
        if (FechaCompra != "") {
            FechaCompra = FechaCompra.substring(6, 10) + "-" + FechaCompra.substring(3, 5) + "-" + FechaCompra.substring(0, 2);
        }
        else {
            FechaCompra = "0";
        }
        if (FechaVencimiento != "") {
            FechaVencimiento = FechaVencimiento.substring(6, 10) + "-" + FechaVencimiento.substring(3, 5) + "-" + FechaVencimiento.substring(0, 2);
        }
        else {
            FechaVencimiento = "0";
        }

        objeto.fechaInicial = FechaInicial == "" ? "0" : FechaInicial;;
        objeto.fechaFinal = FechaFinal == "" ? "0" : FechaFinal;
        objeto.fechaCompra = FechaCompra == "" ? "0" : FechaCompra;
        objeto.fechaVencimiento = FechaVencimiento == "" ? "0" : FechaVencimiento;
        objeto.IdProducto = Cliente == "" ? "null" : Cliente;
        if (validarFormulario("listView")) {
            EjecutarAjax(urlBase + "ReporteTarjetasRecargables/GenerarReporte", "GET", objeto, "cargarTabla", null);
        }
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteTarjetasRecargables";
    });
    

});

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante", datos);
        }
        else {
            window.location = urlBase + 'ReporteTarjetasRecargables/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}


