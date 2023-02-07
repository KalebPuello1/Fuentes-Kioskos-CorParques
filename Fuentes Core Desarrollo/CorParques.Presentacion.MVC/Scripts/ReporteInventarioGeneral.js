/// <reference path="D:\TFS\Corparques\SRC\Softtek.CorParques\CorParques.Presentacion.MVC\Vendors/jquery/dist/jquery.min.js" />
$(function () {

    loadCalendar();
    setNumeric();
    
    $("#idPunto").select2({
        placeholder: "* Seleccione el Almacen"
    });

    $("#idMaterial").select2({
        placeholder: "* Seleccione el Material"
    });

    $("#DDL_CB").select2({
        placeholder: "* Seleccione"
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
        var idPunto = $("#idPunto").val();
        var idMaterial = $("#idMaterial").val();
        var DDL_CB = $("#DDL_CB").val();
        
        
        var objeto = new Object();
       
        if (fechaInicial != ""){
            fechaInicial = fechaInicial.substring(6, 10) + "-" + fechaInicial.substring(3, 5) + "-" + fechaInicial.substring(0, 2);
        }
        if (fechaFinal != "") {
            fechaFinal = fechaFinal.substring(6, 10) + "-" + fechaFinal.substring(3, 5) + "-" + fechaFinal.substring(0, 2);
        }

        objeto.fechaInicial = fechaInicial;
        objeto.fechaFinal = fechaFinal;
        objeto.fechaFinal = fechaFinal;
        objeto.idMaterial = isNaN(idMaterial) ? 0 : parseInt(idMaterial);
        //objeto.idPunto = isNaN(idPunto) ? 0 : parseInt(idPunto);
        objeto.Almacen = idPunto == "" ? "0" : idPunto;
        objeto.CB = DDL_CB;
        if (validarFormulario("listView")) {
            EjecutarAjax(urlBase + "ReporteInventarioGeneral/GenerarArchivo", "GET", objeto, "cargarTabla", null);
        }
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteInventarioGeneral";
    });
    

});

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante",  datos);
        }
        else {
            window.location = urlBase + 'ReporteInventarioGeneral/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}


