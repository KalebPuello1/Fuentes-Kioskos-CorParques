/// <reference path="D:\TFS\Corparques\SRC\Softtek.CorParques\CorParques.Presentacion.MVC\Vendors/jquery/dist/jquery.min.js" />
$(function () {

    loadCalendar();
    
    $('#datetimepickerIni').datetimepicker({ format: 'YYYY-MM-DD' });
   
    $("#btnConsultar").click(function () {
       
        if (validarFormulario("listView")) {
            EjecutarAjax(urlBase + "ReporteControlBoleteria/GenerarArchivo/", "GET",  { Fecha: $("#Fecha").val(), usuario : $("#idTaquillero").val() }, "cargarTabla", null);
        }
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteControlBoleteria";
    });
    

});

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante",  datos);
        }
        else {
            window.location = urlBase + 'ReporteControlBoleteria/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}


