
/// <reference path="D:\TFS\Corparques\SRC\Softtek.CorParques\CorParques.Presentacion.MVC\Vendors/jquery/dist/jquery.min.js" />
$(function () {

    loadCalendar();
    setNumeric();

    $(".dateTime").datetimepicker({
        format: 'HH:mm',
        useCurrent: false
    });
    
    $("#idEps").select2({
        placeholder: "* Seleccione..."
    });

    $("#idTipEps").select2({
        placeholder: "* Seleccione..."
    });

    $('#datetimepickerIni').datetimepicker({ format: 'DD/MM/YYYY' });
    $('#datetimepickerFin').datetimepicker({ useCurrent: false,  format: 'DD/MM/YYYY' });
    $("#datetimepickerIni").on("dp.change", function (e) {
        $('#datetimepickerFin').data("DateTimePicker").minDate(e.date);
    });
    $("#datetimepickerFin").on("dp.change", function (e) {
        $('#datetimepickerIni').data("DateTimePicker").maxDate(e.date);
    });




    $("#btnConsultar").click(function () {

        if (!validarFormulario("frmReservaEspacios")) {
            return;
        }

        var fechaInicial = $("#FInicial").val();
        var fechaFinal = $("#FFinnal").val();
        var txtHoraInicial = $("#txtHoraInicial").val();
        var txtHoraFinal = $("#txtHoraFinal").val();
        var idEps = $("#idEps").val();
        var idTipEps = $("#idTipEps").val();
        var txtNPedido = $("#txtNPedido").val();

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

        objeto.fechaInicialGet = fechaInicial == "" ? "null" : fechaInicial;
        objeto.fechaFinalGet = fechaFinal == "" ? "null" : fechaFinal;
        //objeto.horaIniGet = txtHoraInicial == "" ? "null" : txtHoraInicial.replace(":", "");
        //objeto.horaFinGet = txtHoraFinal == "" ? "null" : txtHoraFinal.replace(":", "");
        objeto.idEspGet = idEps;
        objeto.idTipEpsGet = idTipEps;
        //objeto.txtNPedidoGet = txtNPedido;

        EjecutarAjax(urlBase + "ReporteReservaEspacio/GenerarArchivo", "GET", objeto, "cargarTabla", null);
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteReservaEspacio";
    });
    

});

function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante",  datos);
        }
        else {
            window.location = urlBase + 'ReporteReservaEspacio/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}


