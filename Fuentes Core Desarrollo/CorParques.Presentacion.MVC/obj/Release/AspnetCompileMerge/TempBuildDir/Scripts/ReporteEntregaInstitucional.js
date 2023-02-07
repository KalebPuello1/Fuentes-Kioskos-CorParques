/// <reference path="D:\TFS\Corparques\SRC\Softtek.CorParques\CorParques.Presentacion.MVC\Vendors/jquery/dist/jquery.min.js" />
$(function () {

    
    $('#datetimepickerIni').datetimepicker({ format: 'YYYY-MM-DD' });
    $('#datetimepickerFin').datetimepicker({ format: 'YYYY-MM-DD' });
   
    $("#btnConsultar").click(function () {
       
        if (validarFormulario("listView")) {
            EjecutarAjax(urlBase + "ReporteEntregaInstitucional/GenerarArchivo/", "GET", { fechaEntrega: $("#FEntrega").val(), fechaUso: $("#FUso").val() == "" ? "null" : $("#FUso").val(), pedido: $("#Pedido").val() == "" ? "null" : $("#Pedido").val(), asesot: $("#idAsesor").val(), cliente: $("#idCliente").val(), producto: $("#idProducto").val(), }, "cargarTabla", null);
        }
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteEntregaInstitucional";
    });
    

});

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante",  datos);
        }
        else {
            window.location = urlBase + 'ReporteEntregaInstitucional/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}


