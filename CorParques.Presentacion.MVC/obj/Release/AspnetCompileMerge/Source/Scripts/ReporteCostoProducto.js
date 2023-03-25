/// <reference path="D:\TFS\Corparques\SRC\Softtek.CorParques\CorParques.Presentacion.MVC\Vendors/jquery/dist/jquery.min.js" />
$(function () {

    loadCalendar();
    setNumeric();
    
    $("#CodSap").select2({
        placeholder: "* Seleccione el área"
    });

    //$("#CodSapNombre").select2({
    //    placeholder: "* Seleccione el área"
    //});

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

        if (validarFormulario("listView")) {
            var fechaInicial = $("#FInicial").val();
            var fechaFinal = $("#FFinnal").val();
            //var CodSapNombre = $("#CodSapNombre").val();
            var CodSap = $("#CodSap").val();
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

            objeto.fechaInicial = fechaInicial == "" ? "nul" : fechaInicial;;
            objeto.fechaFinal = fechaFinal == "" ? "null" : fechaFinal;
            //objeto.CodSapNombre = CodSapNombre == "" ? "null" : CodSapNombre;
            objeto.CodSap = CodSap == "" ? "null" : CodSap;

            EjecutarAjax(urlBase + "ReporteCostoProducto/GenerarArchivo", "GET", objeto, "cargarTabla", null);
        }
        
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteCostoProducto";
    });
    

});

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            mostrarAlerta("Error al generar reporte: " + datos);
        }
        else {
            window.location = urlBase + 'ReporteCostoProducto/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}


