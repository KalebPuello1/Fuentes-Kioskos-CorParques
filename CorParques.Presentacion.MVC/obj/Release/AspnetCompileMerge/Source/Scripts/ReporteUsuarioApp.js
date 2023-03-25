/// <reference path="D:\TFS\Corparques\SRC\Softtek.CorParques\CorParques.Presentacion.MVC\Vendors/jquery/dist/jquery.min.js" />
var locacion = location.href
console.log(`${locacion}/GenerarReporte`)
$(function () {

    loadCalendar();
    setNumeric();

    $("#Correoelectronico").select({
        placeholder: "* Ingrese correo electronico"
    });
    
    $("#btnConsultar").click(function () {
        var Correoelectronico = $("#Correoelectronico").val();

       Correoelectronico = Correoelectronico == "" ? "null" : Correoelectronico;

        if (validarFormulario("listView")) {
            //EjecutarAjax(urlBase + "ReporteUsuarioApp/GenerarReporte", "POST", { Correoelectronico: Correoelectronico}, "cargarTabla", null);
            EjecutarAjax(`${locacion}/GenerarReporte`, "GET", { Correoelectronico: Correoelectronico }, "cargarTabla", null);
        }
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteUsuarioApp";
    });
    

});

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante", "No hay información para exportar.","error");
        }
        else {
            window.location = urlBase + 'ReporteUsuarioApp/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}


