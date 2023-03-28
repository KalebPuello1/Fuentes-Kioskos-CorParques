var idPunto;
var id;
$(function () {
    setEventEdit();
    asignarSelect2();

    var FechaInicial = $("#hdf_FechaInicial").val();
    var FechaFinal = $("#hdf_FechaFinal").val();
    var FechaIni;
    var FechaFin;

    if (FechaInicial == "") {
        FechaIni = moment();
    }
    else {
        var fec = FechaInicial.split('/');
        var newFecha = new Date(fec[2], fec[1] - 1, fec[0]);
        FechaIni = newFecha;
    }

    if (FechaFinal == "") {
        FechaFin = moment();
    } else {
        var fec = FechaFinal.split('/');
        var newFecha = new Date(fec[2], fec[1] - 1, fec[0]);
        FechaFin = newFecha;
    }

    if ($("#hdf_MostrarFechas").val() == "S") {
        $('#FechaInicial').datetimepicker({ format: 'DD/MM/YYYY', maxDate: FechaFin });
        $('#FechaFinal').datetimepicker({ format: 'DD/MM/YYYY', minDate: FechaIni, maxDate: FechaFin });
        
        $("#FechaInicial").on("dp.change", function (e) {

            $('#FechaFinal').data("DateTimePicker").minDate(e.date);
            $(this).data("DateTimePicker").hide();
            var decrementDay = moment(new Date(e.date));
            var dateSelected = new Date(decrementDay);
            var FechaInicial = (dateSelected.getDate()) + '/' + (dateSelected.getMonth() + 1) + '/' + (dateSelected.getFullYear());
            var fechaValidar;

            var FechaFinal = $('#FechaFinal').val();
            if (FechaFinal != "") {
                var fec = FechaFinal.split('/');
                var newFecha = new Date(fec[2], fec[1] - 1, fec[0]);
                fechaValidar = newFecha.getDate() + '/' + (newFecha.getMonth() + 1) + '/' + (newFecha.getFullYear());
                if (fechaValidar == FechaInicial) {
                    $('#HoraInicial').prop('disabled', false);
                    $('#HoraFinal').prop('disabled', false);
                } else {
                    $('#HoraInicial').prop('disabled', true);
                    $('#HoraFinal').prop('disabled', true);
                }
            }
        });

        $("#FechaFinal").on("dp.change", function (e) {
            $('#FechaInicial').data("DateTimePicker").maxDate(e.date);
            $(this).data("DateTimePicker").hide();
            var decrementDay = moment(new Date(e.date));
            var dateSelected = new Date(decrementDay);
            var FechaFinal = (dateSelected.getDate()) + '/' + (dateSelected.getMonth() + 1) + '/' + (dateSelected.getFullYear());
            var fechaValidar;

            var FechaInicial = $('#FechaInicial').val();
            if (FechaInicial != "") {
                var fec = FechaInicial.split('/');
                var newFecha = new Date(fec[2], fec[1] - 1, fec[0]);
                fechaValidar = newFecha.getDate() + '/' + (newFecha.getMonth() + 1) + '/' + (newFecha.getFullYear());
                if (fechaValidar == FechaFinal) {
                    $('#HoraInicial').prop('disabled', false);
                    $('#HoraFinal').prop('disabled', false);
                } else {
                    $('#HoraInicial').prop('disabled', true);
                    $('#HoraFinal').prop('disabled', true);
                }
            }
        });

    }

    $('#horaInicial').datetimepicker({
        format: 'LT'
    });

    $('#horaFinal').datetimepicker({
        format: 'LT',
        useCurrent: false
    });

    $("#horaInicial").on("dp.change", function (e) {
        $('#horaFinal').data("DateTimePicker").minDate(e.date);
    });

    $("#horaFinal").on("dp.change", function (e) {
        $('#horaInicial').data("DateTimePicker").maxDate(e.date);
    });

    //ValidaFacturas();
    InhabilitaEscritura();

    if ($("#Validado").val() != "") {
        if (document.getElementById('datatable-responsive') == null) {
            MostrarMensaje("Importante", "La búsqueda no retornó información", "info");
        }
    }

    if ($("#CodPunto").val() != null) {
        $("#DDL_Punto").val($("#CodPunto").val()).trigger('change.select2');
    }

    //$("#CodFinalFactura").on("blur", function () {
    //    ValidaFacturas();
    //});

    //$("#CodInicialFactura").on("blur", function () {
    //    if ($("#CodInicialFactura").val() != "") {
    //        $("#btnBuscar").attr('disabled', false);
    //    }
    //});

    //$("#CodInicialFactura, #CodFinalFactura").on("keypress", function (e) {
    //    $("#btnBuscar").attr('disabled', false);
    //}).on("keydown", function (e) {
    //    $("#btnBuscar").attr('disabled', false);
    //});

    $("#btnBuscar").click(function () {
        idPunto = $("#DDL_Punto").val();
        $("#CodPunto").val(idPunto);
        $("#Validado").val(1);
        if (ValidaFacturas()) {
            $('#frmBusqueda').submit();
        }
        //$("#CodPunto").val("");
        //$("#DDL_Punto").val("")
    });

    $("#btnPrintAll").click(function () {
        MostrarConfirm("Importante!", "¿Está seguro que desea imprimir todas las facturas? ", "PrintAll", "");
    });

    $("#btnLimpiar").click(function () {
        LimpiarControles();
    });

    //$("#frmBusqueda").find(':input').each(function () {
    //    var elemento = this;
    //    //InhabilitarCopiarPegarCortar(elemento.id)
    //});

    if (document.getElementById("frmList") != null && document.getElementById("frmList").elements.length == 12) {
        $("#btnPrintAll").css('display', 'none');
    }
    
    //RDSH: Establece las fechas luego del submit.
    if ($("#hdf_MostrarFechas").val() == "S")
    {
        EstablecerFechas();
    }    
});

function PrintAll() {
    $("#frmList").submit();
    //MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", "Reimpresion/Index", "success");
}

function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkDetail").click(function () {
        EjecutarAjax(urlBase + "Reimpresion/Detalle", "GET", { id: $(this).data("id") }, "printPartialModal", {
            title: "Detalle Reimpresion", hidesave: true, modalLarge: false
        });
    });

    $(".lnkPrint").click(function () {
        id = $(this).data("id");
        PintById(id);
        //MostrarConfirm("Importante!", "¿Está seguro que desea imprimir la factura? ", "PintById", "");
    });
}

function InhabilitaEscritura() {
    $("#FechaInicial, #FechaFinal, #HoraInicial, #HoraFinal").on("keypress", function (e) {
        e.preventDefault();
    });
    $("#FechaInicial, #FechaFinal, #HoraInicial, #HoraFinal").on("keydown", function (e) {
        e.preventDefault();
    });
}

function PintById(Id) {
    EjecutarAjax(urlBase + "Reimpresion/Imprimir", "GET", { id: Id }, "successPrint", null);
    //MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", "Reimpresion/Index", "success");
}

function ValidaFacturas() {
    var codInicial = $("#CodInicialFactura").val();
    var codFinal = $("#CodFinalFactura").val();

    var flag = true;
    if ($("#CodInicialFactura").val() == "" && $("#CodFinalFactura").val() == "") {
        flag = true;
    } else {
        if ($("#CodInicialFactura").val() != "" || $("#CodFinalFactura").val() != "") {
            if ($("#CodFinalFactura").val() != "" && $("#CodInicialFactura").val() == "") {
                MostrarMensaje("Importante", "Ingrese el código factura inicial", "error");
                flag = false;
            } else if ($("#CodInicialFactura").val() != "" && $("#CodFinalFactura").val() == "") {
                MostrarMensaje("Importante", "Ingrese el código factura final", "error");
                flag = false;
            } else {
                if (parseFloat(codInicial) <= parseFloat(codFinal)) {
                    $("#DDL_Punto").attr('disabled', true);
                    $("#btnBuscar").attr('disabled', false);
                }
                else {
                    $("#DDL_Punto").attr('disabled', true);
                    $("#btnBuscar").attr('disabled', true);
                    MostrarMensaje("Importante", "El código factura final debe ser mayor al código factura inicial", "error");
                    flag = false;
                }
            }
        }
        else {
            $("#DDL_Punto").attr('disabled', false);
            $("#btnBuscar").attr('disabled', false);
            flag = true;
        }
    }

    if ($('#FechaInicial').val() != "" && $('#FechaFinal').val() == "") {
        MostrarMensaje("Importante", "Debe seleccionar la fecha final", "error");
        flag = false;
    } else if ($('#FechaInicial').val() == "" && $('#FechaFinal').val() != "") {
        MostrarMensaje("Importante", "Debe seleccionar la fecha inicial", "error");
        flag = false;
    } else if ($('#HoraInicial').val() != "" && $('#HoraFinal').val() == "") {
        MostrarMensaje("Importante", "Debe seleccionar la hora final", "error");
        flag = false;
    } else if ($('#HoraInicial').val() == "" && $('#HoraFinal').val() != "") {
        MostrarMensaje("Importante", "Debe seleccionar la hora inicial", "error");
        flag = false;
    } else if ($('#FechaInicial').val() == "" && $('#FechaFinal').val() == "" && $("#HoraInicial").val() == "" && $("#HoraFinal").val() == "" && $("#CodInicialFactura").val() == "" && $("#CodFinalFactura").val() == "" && $("#CodPunto").val() == "" && $('#CodBrazalete').val() == "") {
        MostrarMensaje("Importante", "Debe seleccionar por lo menos un filtro de busqueda.", "error");
        flag = false;
    }

    return flag;
}

function successPrint(data) {
    if (data.Correcto) {
        MostrarMensaje("Importante", "Operación realizada con éxito.", "success");
    }
    else {
        MostrarMensaje("Falló al reimprimir", data.Mensaje);
    }
}

function successPrintAll(data) {
    if (data.Correcto) {
        MostrarMensaje("Importante", "Operación realizada con éxito.", "success");
    }
    else {
        MostrarMensaje("Falló al reimprimir", rta.Mensaje);
    }
}

function LimpiarControles() {
    $('#HoraInicial').val("");
    $('#HoraFinal').val("");
    $('#FechaInicial').val("");
    $('#FechaFinal').val("");
    $('#CodInicialFactura').val("");
    $('#CodBrazalete').val("");
    $('#CodPunto').val("");
    $("#DDL_Punto").val("");
    $("#DDL_Punto").val("").trigger('change');
    $('#HoraInicial').prop('disabled', false);
    $('#HoraFinal').prop('disabled', false);
    $("#CodFinalFactura").val("");
    $("#DDL_Punto").attr('disabled', false);
    $("#btnBuscar").attr('disabled', false);
}

//RDSH: Establece las fechas luego de la consulta de la informacion.
function EstablecerFechas() {
    var FechaInicial = $("#hdf_FechaInicial").val();
    var FechaFinal = $("#hdf_FechaFinal").val();

    if (FechaInicial != null)
        $('#FechaInicial').data("DateTimePicker").date(FechaInicial);
    if (FechaFinal != null)
        $('#FechaFinal').data("DateTimePicker").date(FechaFinal);

    if (FechaInicial != FechaFinal) {
        $('#HoraInicial').prop('disabled', true);
        $('#HoraFinal').prop('disabled', true);
    } else {
        $('#HoraInicial').prop('disabled', false);
        $('#HoraFinal').prop('disabled', false);
    }

}


