//Powered by RDSH.

$(function () {

    loadCalendar();    
    //MostrarVistaPrevia();
      

    $('#datetimepickerIni').datetimepicker({ format: 'DD/MM/YYYY' });
    $('#datetimepickerFin').datetimepicker({
        useCurrent: false,
        format: 'DD/MM/YYYY'
    });
    $("#datetimepickerIni").on("dp.change", function (e) {
        $('#datetimepickerFin').data("DateTimePicker").minDate(e.date);
//        MostrarVistaPrevia();
    });
    $("#datetimepickerFin").on("dp.change", function (e) {
        $('#datetimepickerIni').data("DateTimePicker").maxDate(e.date);
        //MostrarVistaPrevia();
    });

    $("#btnConsultar").click(function () {

        if (!validarFormulario("frmReporteNotificaciones")) {
            return;
        } else {
            $("#datetimepickerIni").attr("data-mensajeerror", "");
            $("#datetimepickerFin").attr("data-mensajeerror", "");
        }

        var FechaInicial = $("#txtFechaInicial").val();
        var FechaFinal = $("#txtFechaFinal").val();        
        
        var objeto = new Object();

        if (FechaInicial != "") {
            FechaInicial = FechaInicial.substring(6, 10) + "-" + FechaInicial.substring(3, 5) + "-" + FechaInicial.substring(0, 2);
        }
        else {
            FechaInicial = "null";
        }

        if (FechaFinal != "") {
            FechaFinal = FechaFinal.substring(6, 10) + "-" + FechaFinal.substring(3, 5) + "-" + FechaFinal.substring(0, 2);
        }
        else {
            FechaFinal = "null";
        }

        objeto.FechaInicial = FechaInicial == "" ? "null" : FechaInicial;
        objeto.FechaFinal = FechaFinal == "" ? "null" : FechaFinal;        

        EjecutarAjax(urlBase + "ReporteNotificaciones/GenerarArchivo", "GET", objeto, "cargarTabla", null);
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteNotificaciones";
    });

    //RDSH: Inicio Evento del boton btnGrid - Vista previa
    $("#btnGrid").click(function () {

        if (!validarFormulario("frmReporteNotificaciones")) {
            return;
        } else {
            $("#datetimepickerIni").attr("data-mensajeerror", "");
            $("#datetimepickerFin").attr("data-mensajeerror", "");
        }

        var FechaInicial = $("#txtFechaInicial").val();
        var FechaFinal = $("#txtFechaFinal").val();

        var objeto = new Object();

        if (FechaInicial != "") {
            FechaInicial = FechaInicial.substring(6, 10) + "-" + FechaInicial.substring(3, 5) + "-" + FechaInicial.substring(0, 2);
        }
        else {
            FechaInicial = "null";
        }

        if (FechaFinal != "") {
            FechaFinal = FechaFinal.substring(6, 10) + "-" + FechaFinal.substring(3, 5) + "-" + FechaFinal.substring(0, 2);
        }
        else {
            FechaFinal = "null";
        }

        objeto.FechaInicial = FechaInicial == "" ? "null" : FechaInicial;
        objeto.FechaFinal = FechaFinal == "" ? "null" : FechaFinal;      

        EjecutarAjax(urlBase + "ReporteNotificaciones/VistaPrevia", "GET", objeto, "VistaPrevia", null);
    });

    //RDSH: Fin Evento del boton btnGrid - Vista previa

});

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante", datos);
        }
        else {
            window.location = urlBase + 'ReporteVentasPorHora/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}

//RDSH: Muestra la grilla de vista previa.
function VistaPrevia(datos)
{
    $("#div_grilla").html("");
    if (datos != null) {
        if (datos.length > 0) {
            $("#div_grilla").html(datos);
            $("#div_tabla").find("table").DataTable();
            //$(".dataTables_filter").hide();
        } else {            
            MostrarMensaje("Importante", "No hay información para mostrar.");
        }
    } else {        
        MostrarMensaje("Importante", "No hay información para mostrar.");
    }

}

//RDSH: Valida que la fecha del calendario seleccionada sea igual al dia actual.
function MostrarVistaPrevia()
{    
    var FechaActual = ObtenerFechaActual();

    if ($("#txtFechaInicial").val() === FechaActual && $("#txtFechaFinal").val() === FechaActual) {
        $("#btnGrid").show();
    } else {
        $("#btnGrid").hide();
    }

}
