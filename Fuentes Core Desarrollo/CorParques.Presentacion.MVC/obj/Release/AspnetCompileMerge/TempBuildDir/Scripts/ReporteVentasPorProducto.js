var _Modelo;
var consulta;

$(function () {
    $('#datetimepickerIni').datetimepicker({ format: 'DD/MM/YYYY' });
    $('#datetimepickerFin').datetimepicker({
        useCurrent: false,
        format: 'DD/MM/YYYY'
    });
    $("#datetimepickerIni").on("dp.change", function (e) {
        $('#datetimepickerFin').data("DateTimePicker").minDate(e.date);
        if ($('#txtFechaFinal')[0].disabled) {
            $("#txtFechaFinal").val($("#txtFechaInicial").val()).change();
        }
        if ($("#txtFechaInicial").val() != "" && $("#txtFechaFinal").val() != "") {
            $('#datetimepicker1').prop('disabled', true);
            $('#datetimepicker2').prop('disabled', true);
        }
        else {
            $('#datetimepicker1').prop('disabled', false);
            $('#datetimepicker2').prop('disabled', false);
        }

        
    });
    $("#datetimepickerFin").on("dp.change", function (e) {
        $('#datetimepickerIni').data("DateTimePicker").maxDate(e.date);
    });
    
    asignarSelect2();
    setEventEdit()
    InhabilitaEscritura()
    $("#btnBuscar").click(function () {
        if (validarFormulario("frmBusqueda")) {
            $("#FechaInicial").val($("#txtFechaInicial").val());
            $("#FechaFinal").val($("#txtFechaFinal").val());
            $("#CodigoPunto").val($("#DDL_Punto").val());
            $("#CodigoProducto").val($("#Producto").val());
            $("#CentroBeneficio").val($("#DDL_CentroBeneficio").val());
            //$('#frmBusqueda').submit();
            var objeto = ObtenerObjeto("frmBusqueda");
            EjecutarAjax(urlBase + "ReporteVentasPorProducto/ObtenerDatosReporte", "GET", objeto, "GenerarReporte", null);
        }        
        
    });

    consulta = $("#consulta").val();
    if (consulta == '1' && document.getElementById('frmList') == null) {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }

    $("#btnExport").click(function () {
        EjecutarAjax(urlBase + "ReporteVentasPorProducto/ExportarExcel", "GET", { Filtros: $("#Filtros").val() }, "GenerarReporte", null);
    });

    $("#btnLimpiar").click(function () {
        LimpiarControles();
    });
});

function InhabilitaEscritura() {
    $("#datetimepickerIni, #datetimepickerFin").on("keypress", function (e) {
        e.preventDefault();
    });
    $("#datetimepickerIni, #datetimepickerFin").on("keydown", function (e) {
        e.preventDefault();
    });
}

function InhabilitarCopiarPegarCortar(control) {
    $("#" + control).bind("cut copy paste", function (e) {
        e.preventDefault();
    });
}

function GenerarReporte(data) {
    if (data.length > 0) {
        if (data.indexOf("Error") >= 0) {
            mostrarAlerta("Error al generar reporte: " + data);
        }
        else {
            window.location = urlBase + 'ReporteVentasPorProducto/Download?Data=' + data;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}

function setEventEdit() {
    EstablecerToolTipIconos();    
}

function LimpiarControles() {
    $("#txtFechaInicial").val("");
    $("#txtFechaFinal").val("");   
    $("#datetimepickerIni").val("").change();
    $("#datetimepickerFin").val("").change();    
    $("#DDL_Punto").val("").trigger('change');
    $("#DDL_CentroBeneficio").val("").trigger('change');
    $("#Producto").val("").trigger('change');
    $('#txtFechaFinal').prop('disabled', false);    
    $("#datatable-responsive_wrapper").hide();
    $("#btnExport").hide();
}
