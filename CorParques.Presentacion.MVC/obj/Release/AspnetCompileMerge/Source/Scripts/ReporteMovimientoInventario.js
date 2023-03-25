/// <reference path="C:\Users\epulido\Documents\Proyectos\Corparques\SRC\Trunk\Softtek.CorParques\CorParques.Presentacion.MVC\Vendors/jquery/dist/jquery.min.js" />

$(function () {

    loadCalendar();
    setNumeric();

    $("#idUnidadMedida").select2({
        placeholder: "* Seleccione la unidad"
    });

    $("#idMaterial").select2({
        placeholder: "* Seleccione el Material"
    });

    $("#IdPuntoOrigen").select2({
        placeholder: "* Seleccione el almacen de origen"
    });

    $("#IdPuntoDestino").select2({
        placeholder: "* Seleccione el almacen de destino"
    });

    $("#IdResponsable").select2({
        placeholder: "* Seleccione el responsable"
    });

    $("#IdTipoMovimiento").select2({
        placeholder: "* Seleccione  tipo movimiento"
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
        var CodMaterial = $("#idMaterial").val();
        var IdTipoMovimiento = $("#IdTipoMovimiento").val();
        var IdPuntoOrigen = $("#IdPuntoOrigen").val();
        var IdPuntoDestino = $("#IdPuntoDestino").val();
        var IdResponsable = $("#IdResponsable").val();
        var UnidadMedida = $("#idUnidadMedida").val();

        var objeto = new Object();

        if (fechaInicial != "") {
            fechaInicial = fechaInicial.substring(6, 10) + "-" + fechaInicial.substring(3, 5) + "-" + fechaInicial.substring(0, 2);
        }

        if (fechaFinal != "") {
            fechaFinal = fechaFinal.substring(6, 10) + "-" + fechaFinal.substring(3, 5) + "-" + fechaFinal.substring(0, 2);
        }

        objeto.FechaInicial = fechaInicial;
        objeto.FechaFinal = fechaFinal;
        debugger;
        objeto.IdTipoMovimiento = isNaN(IdTipoMovimiento) ? 0 : parseInt(IdTipoMovimiento);
        objeto.CodigoMaterial = CodMaterial;
        objeto.UnidadMedida = UnidadMedida;
        objeto.IdPuntoOrigen = isNaN(IdPuntoOrigen) ? 0 : parseInt(IdPuntoOrigen);
        objeto.PuntoDestino = isNaN(IdPuntoDestino) ? 0 : parseInt(IdPuntoDestino);
        objeto.IdPersonaResponsable = isNaN(IdResponsable) ? 0 : parseInt(IdResponsable);
        if (validarFormulario("listView")) {
            EjecutarAjax(urlBase + "ReporteMovimientoInventario/GenerarArchivo", "GET", objeto, "cargarTabla", null);
        }
    });



    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteMovimientoInventario";
    });


})

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante", datos);
        }
        else {
            window.location = urlBase + 'ReporteMovimientoInventario/Download?Data=' + datos;
            MostrarMensaje("Importante","La descarga fue satisfactoria","success")
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}

