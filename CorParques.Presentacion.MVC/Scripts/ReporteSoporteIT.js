var _Modelo;
var consulta;

$(function () {

    loadCalendar();

    $('#datetimepickerIni').datetimepicker({ format: 'YYYY-MM-DD' });

    $("#btnConsultar").click(function () {
       
            EjecutarAjax(urlBase + "ReporteSoporteIT/ConsultarFecha", "GET", { Fecha: $("#txtFecha").val() }, "printPartial", { div: "#listView", func: "setEventEdit" });
    });

    $("#btnLimpiar").click(function () {
        window.location = urlBase + "ReporteSoporteIT";
    });


});


function setEventEdit() {
    EstablecerToolTipIconos();
}

function LimpiarControles() {
    //$("#txtFechaInicial").val("");
    //$("#txtFechaFinal").val("");    
    //$("#DDL_Punto").val("").trigger('change');
    //$("#DDL_Taquillero").val("").trigger('change');
    //$("#DDL_MetodoPago").val("").trigger('change');
    //$("#DDL_Franquicia").val("").trigger('change');
    EjecutarAjax(urlBase + "ReporteRecoleccion/Index", "GET", { modelo: null }, "retorno", null);
}

function retorno() {

}