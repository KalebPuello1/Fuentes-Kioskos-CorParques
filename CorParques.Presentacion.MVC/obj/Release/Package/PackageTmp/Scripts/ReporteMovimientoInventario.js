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
    $("#IdZonaOrigen").select2({
        placeholder: "* Seleccione la zona"
    });
    $(".mesaselect").select2({
        placeholder: "* Seleccione la mesa"
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
        /////
        objeto.CodSapAlmacenOrigen = IdPuntoOrigen
        /////
        objeto.CodSapAlmacenDestino = IdPuntoDestino
        ////
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


    $("#btnConsultarRest").click(function () {
        var fechaInicial = $("#FInicial").val();
        var fechaFinal = $("#FFinal").val();
        var IdResponsable = $("#IdResponsable").val();
        var IdZonaOrigen = $("#IdZonaOrigen").val();
        var IdMesa = $("#IdPuntoOrigen").val();
        var CodAlmacen = $("#IdPuntoDestino").val();

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
        objeto.Id_Vendedor = isNaN(IdResponsable) ? 0 : parseInt(IdResponsable);
        objeto.IdZona = isNaN(IdZonaOrigen) ? 0 : parseInt(IdZonaOrigen);
        objeto.Id_Mesa = isNaN(IdMesa) ? 0 : parseInt(IdMesa);
        objeto.CodSapAlmacenOrigen = CodAlmacen;
       
      
        if (validarFormulario("listView")) {
            EjecutarAjax(urlBase + "ReportePedidoRestaurante/GenerarArchivo", "GET", objeto, "cargarTablaRest", null);
        }
    });
    $("#btnLimpiarRest").click(function () {
        window.location = urlBase + "ReporteMovimientoInventario";
    });


})
function listarMesaporZona() {
    var IdZonaOrigen = $("#IdZonaOrigen").val();

    if (IdZonaOrigen !== null && IdZonaOrigen !== '') {


        EjecutarAjaxJson(urlBase + "PedidoA/ListarMesasZona", "post", {
            IdZona: IdZonaOrigen
        }, "successListarMesaZona", null);
    }
    else {
        var select = document.getElementById("IdPuntoOrigen");
        document.getElementById("IdPuntoOrigen").innerHTML = ""

        var option1 = document.createElement("option");
        option1.text = "*Seleccione..."; option1.value = "";
        option1.value = "";
        select.add(option1);
    }

}
function successListarMesaZona(rta) {
    if (rta !== null) {

        var select = document.getElementById("IdPuntoOrigen");
        document.getElementById("IdPuntoOrigen").innerHTML = ""

        var option1 = document.createElement("option");
        option1.value = "";
        option1.text = "*Seleccione...";
        select.add(option1);

        $.each(rta, function (i, item) {

            var option2 = document.createElement("option");
            option2.text = item.Nombre;
            option2.value = item.Id;
            select.add(option2);
        });


    }
    else {

    }

}
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
function cargarTablaRest(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante", datos);
        }
        else {
            window.location = urlBase + 'ReportePedidoRestaurante/Download?Data=' + datos;
            MostrarMensaje("Importante", "La descarga fue satisfactoria", "success")
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}
