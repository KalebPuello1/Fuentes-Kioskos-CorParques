/// <reference path="D:\TFS\Corparques\SRC\Softtek.CorParques\CorParques.Presentacion.MVC\Vendors/jquery/dist/jquery.min.js" />
$(function () {

    loadCalendar();
    setNumeric();
    MostrarVistaPrevia();
    
    $("#idTipIngreso").select2({
        placeholder: "* Seleccione..."
    });

    $("#TipConsnumo").select2({
        placeholder: "* Seleccione..."
    });

    $("#TipNovedad").select2({
        placeholder: "* Seleccione..."
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
        var fechaInicial = $("#FInicial").val();
        var fechaFinal = $("#FFinnal").val();
        var idTipIngreso = $("#idTipIngreso").val();

        var TipConsnumo = $("#TipConsnumo").val();
        var TipNovedad = $("#TipNovedad").val();
        
        var objeto = new Object();
       
        if (fechaInicial != ""){
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

        objeto.fechaInicial = fechaInicial == "" ? "null" : fechaInicial;
        objeto.fechaFinal = fechaFinal == "" ? "null" : fechaFinal;
        objeto.TipNovedad = TipNovedad == "" ? "null" : TipNovedad;
        objeto.TipConsnumo = TipConsnumo == "" ? "null" : TipConsnumo;

        objeto.idTipIngreso = idTipIngreso;
        if (validarFormulario("listView")) {
            EjecutarAjax(urlBase + "ReporteFlujoCajasTaq/GenerarArchivo", "GET", objeto, "cargarTabla", null);
        }
    });

    $("#btnGrid").click(function () {

        var fechaInicial = $("#FInicial").val();
        var fechaFinal = $("#FFinnal").val();
        var idTipIngreso = $("#idTipIngreso").val();

        var TipConsnumo = $("#TipConsnumo").val();
        var TipNovedad = $("#TipNovedad").val();

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

        objeto.fechaInicial = fechaInicial == "" ? "null" : fechaInicial;
        objeto.fechaFinal = fechaFinal == "" ? "null" : fechaFinal;
        objeto.TipNovedad = TipNovedad == "" ? "null" : TipNovedad;
        objeto.TipConsnumo = TipConsnumo == "" ? "null" : TipConsnumo;

        objeto.idTipIngreso = idTipIngreso;
        if (validarFormulario("listView")) {
            EjecutarAjax(urlBase + "ReporteFlujoCajasTaq/VistaPrevia", "GET", objeto, "VistaPrevia", null);
        }
    });


    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteFlujoCajasTaq";
    });
    

});

function VistaPrevia(datos) {
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

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante",  datos);
        }
        else if (datos == "")
        {   
            MostrarMensaje("Importante", "No hay datos en la consulta.");
        }
        else {
            window.location = urlBase + 'ReporteFlujoCajasTaq/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
    //limpiar datos de input
    //$("#FInicial").val("");
    //$("#FFinnal").val("");
    //$("#idTipIngreso").val(null).trigger("change");
    //$("#TipConsnumo").val(null).trigger("change");
    //$("#TipNovedad").val(null).trigger("change");
    //$('#datetimepickerIni').data("DateTimePicker").clear();
    //$('#datetimepickerFin').data("DateTimePicker").clear();

}

function MostrarVistaPrevia() {

    var FechaActual = ObtenerFechaActual();

    if ($("#FInicial").val() === FechaActual && $("#FFinnal").val() === FechaActual) {
        $("#btnGrid").show();
    } else {
        $("#btnGrid").hide();
    }

}