/// <reference path="D:\TFS\Corparques\SRC\Softtek.CorParques\CorParques.Presentacion.MVC\Vendors/jquery/dist/jquery.min.js" />
$(function () {

    loadCalendar();
    setNumeric();
    MostrarVistaPrevia();

    $("#DDL_Punto").select2({
        placeholder: "* Seleccione..."
    });

    $("#DDL_TipoProducto").select2({
        placeholder: "* Seleccione..."
    });

    $(".dateTime").datetimepicker({
        format: 'HH:mm'
    });

    $('#datetimepickerIni').datetimepicker({ format: 'DD/MM/YYYY' });
    $('#datetimepickerFin').datetimepicker({
        useCurrent: false,
        format: 'DD/MM/YYYY'
    });
    $("#datetimepickerIni").on("dp.change", function (e) {
        $('#datetimepickerFin').data("DateTimePicker").minDate(e.date);
        MostrarVistaPrevia();
    });
    $("#datetimepickerFin").on("dp.change", function (e) {
        $('#datetimepickerIni').data("DateTimePicker").maxDate(e.date);
        MostrarVistaPrevia();
    });

    $("#btnConsultar").click(function () {


        if (validarFormulario("listView")) {

            var FechaInicial = $("#txtFechaInicial").val();
            var FechaFinal = $("#txtFechaFinal").val();
            var IdPunto = $("#DDL_Punto").val();
            var IdTipoProducto = $("#DDL_TipoProducto").val();


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
            objeto.IdPunto = IdPunto;
            objeto.TipoProducto = IdTipoProducto;

            EjecutarAjax(urlBase + "ReportePasajerosAtracciones/GenerarArchivo", "GET", objeto, "cargarTabla", null);
        }
    });


    //RDSH: Inicio Evento del boton btnGrid - Vista previa
    $("#btnGrid").click(function () {
        if (validarFormulario("listView")) {

            var FechaInicial = $("#txtFechaInicial").val();
            var FechaFinal = $("#txtFechaFinal").val();
            var IdPunto = $("#DDL_Punto").val();
            var IdTipoProducto = $("#DDL_TipoProducto").val();


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
            objeto.IdPunto = IdPunto;
            objeto.TipoProducto = IdTipoProducto;

            EjecutarAjax(urlBase + "ReportePasajerosAtracciones/VistaPrevia", "GET", objeto, "VistaPrevia", null);
        }
    });


    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReportePasajerosAtracciones";
    });
});

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante", datos);
        }
        else {
            window.location = urlBase + 'ReportePasajerosAtracciones/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}


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

function MostrarVistaPrevia()
{    
    var FechaActual = ObtenerFechaActual();

    if ($("#txtFechaInicial").val() === FechaActual && $("#txtFechaFinal").val() === FechaActual) {
        $("#btnGrid").show();
    } else {
        $("#btnGrid").hide();
    }

}