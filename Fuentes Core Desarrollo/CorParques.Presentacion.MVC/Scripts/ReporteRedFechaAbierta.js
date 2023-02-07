
$(function () {
    var ubicacion = location.href;
    loadCalendar();
    setNumeric();
    
    $("#SapTipoProducto").select2({
        placeholder: "* Seleccione el área"
    });

    $("#SapAsesor").select2({
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
        var fechaFinal = $("#FFinnal").val();
        var SapTipoProducto = $("#SapTipoProducto").val();
        var SapAsesor = $("#SapAsesor").val();
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

        /* objeto.fechaInicial = fechaInicial == "" ? "0" : fechaInicial;
        objeto.fechaFinal = fechaFinal == "" ? "0" : fechaFinal;
        objeto.SapTipoProducto = SapTipoProducto == "" ? "null" : SapTipoProducto;
        objeto.SapAsesor = SapAsesor == "" ? "null" : SapAsesor;*/

        objeto.fechaInicial = "12-01-2019";
        objeto.fechaFinal = "12-10-2020";
        objeto.SapTipoProducto = SapTipoProducto == "" ? "null" : SapTipoProducto;
        objeto.SapAsesor = SapAsesor == "" ? "null" : SapAsesor;

        if (validarFormulario("listView")) {
          EjecutarAjax(urlBase + "ReporteRedFechaAbierta/GenerarArchivo", "GET", { fechaInicial: fechaInicial == "" ? "0" : fechaInicial, fechaFinal: fechaFinal == "" ? "0" : fechaFinal, SapTipoProducto: SapTipoProducto == "" ? "null" : SapTipoProducto, SapAsesor: SapAsesor == "" ? "null" : SapAsesor}, "cargarTabla", null);
        }
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteRedFechaAbierta";
    });
    

});

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante", datos);
        }
        else {
            MostrarMensaje("Importante", "Descarga realizada con exito");
            window.location = urlBase + 'ReporteRedFechaAbierta/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}


