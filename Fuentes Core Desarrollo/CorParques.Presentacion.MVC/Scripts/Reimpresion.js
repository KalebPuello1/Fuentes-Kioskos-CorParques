var idPunto;
$(function () {
    setEventEdit();
    asignarSelect2();

    $('#datatable-responsive_filter').addClass('hidden');
    $('#fechaInicial').datetimepicker({ format: 'DD/MM/YYYY' });

    $('#fechaFinal').datetimepicker({ useCurrent: false, format: 'DD/MM/YYYY' });

    $('#HoraInicial').prop('disabled', false);

    $("#fechaInicial").on("dp.change", function (e) {
        $('#fechaFinal').data("DateTimePicker").minDate(e.date);
        $(this).data("DateTimePicker").hide();
        var decrementDay = moment(new Date(e.date));
        var dateSelected = new Date(decrementDay);
        var fechaInicial = (dateSelected.getDate()) + '/' + (dateSelected.getMonth() + 1) + '/' + (dateSelected.getFullYear());
        var fechaValidar;

        var fechaFinal = $('#FechaFinal').val();
        if (fechaFinal != "") {
            var fec = fechaFinal.split('/');
            var newFecha = new Date(fec[2], fec[1] - 1, fec[0]);
            fechaValidar = newFecha.getDate() + '/' + (newFecha.getMonth() + 1) + '/' + (newFecha.getFullYear());
            if (fechaValidar == fechaInicial) {
                $('#HoraInicial').prop('disabled', false);
                $('#HoraFinal').prop('disabled', false);
            } else {
                $('#HoraInicial').prop('disabled', true);
                $('#HoraFinal').prop('disabled', true);
            }
        }
    });

    $("#fechaFinal").on("dp.change", function (e) {
        $('#fechaInicial').data("DateTimePicker").maxDate(e.date);
        $(this).data("DateTimePicker").hide();
        var decrementDay = moment(new Date(e.date));
        var dateSelected = new Date(decrementDay);
        var fechaFinal = (dateSelected.getDate()) + '/' + (dateSelected.getMonth() + 1) + '/' + (dateSelected.getFullYear());
        var fechaValidar;

        var fechaInicial = $('#FechaInicial').val();
        if (fechaInicial != "") {
            var fec = fechaInicial.split('/');
            var newFecha = new Date(fec[2], fec[1] - 1, fec[0]);
            fechaValidar = newFecha.getDate() + '/' + (newFecha.getMonth() + 1) + '/' + (newFecha.getFullYear());
            if (fechaValidar == fechaFinal) {
                $('#HoraInicial').prop('disabled', false);
                $('#HoraFinal').prop('disabled', false);
            } else {
                $('#HoraInicial').prop('disabled', true);
                $('#HoraFinal').prop('disabled', true);
            }
        }
    });

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

    ValidaFacturas();

    ValidaFechas();

    InhabilitaEscritura();

    if ($("#Validado").val() != "") {
        if (document.getElementById('datatable-responsive') == null) {
            MostrarMensaje("Importante", "La búsqueda no retornó información", "info");
        }
    }

    //if ($("#CodPunto").val() != null) {
    //    $("#DDL_Punto").val($("#CodPunto").val());
    //}

    $("#CodFinalFactura").on("blur", function () {
        ValidaFacturas();
    });

    $("#CodInicialFactura").on("blur", function () {
        if ($("#CodInicialFactura").val() != "") {
            $("#btnBuscar").attr('disabled', false);
        }
    });

    $("#CodInicialFactura, #CodFinalFactura").on("keypress", function (e) {
        $("#btnBuscar").attr('disabled', false);
    }).on("keydown", function (e) {
        $("#btnBuscar").attr('disabled', false);
    });

    $("#btnBuscar").click(function () {
        idPunto = $("#DDL_Punto").val();
        $("#CodPunto").val(idPunto);
        $("#Validado").val(1);
        if (ValidaFacturas()) {           
            $('#frmBusqueda').submit();            
        }
    });

    

    $("#btnPrintAll").click(function () {
        $("#frmList").submit();
        MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", "Reimpresion/Index", "success");
    });

    $("#btnLimpiar").click(function () {
        LimpiarControles();
    });

    $("#frmBusqueda").find(':input').each(function () {
        var elemento = this;
        InhabilitarCopiarPegarCortar(elemento.id)
    });
});

function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkDetail").click(function () {
        EjecutarAjax(urlBase + "Reimpresion/Detalle", "GET", { id: $(this).data("id") }, "printPartialModal", {
            title: "Detalle Reimpresion", hidesave: true, modalLarge: false
        });
    });

    $(".lnkPrint").click(function () {
        EjecutarAjax(urlBase + "Reimpresion/Imprimir", "GET", { id: $(this).data("id") }, "successPrint", null);
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


function ValidaFechas() {
    if ($("#FechaInicial").val() != "") {
        valuesStart = $("#FechaInicial").val().split("/");
        valuesEnd = $("#FechaFinal").val().split("/");

        var dateStart = new Date(valuesStart[2], (valuesStart[1] - 1), valuesStart[0]);
        var dateEnd = new Date(valuesEnd[2], (valuesEnd[1] - 1), valuesEnd[0]);
        if (dateStart >= dateEnd) {
            $('#HoraInicial').attr('disabled', false);
            $('#HoraFinal').attr('disabled', false);
        }
        else {
            $('#HoraInicial').attr('disabled', true);
            $('#HoraFinal').attr('disabled', true);
        }
    }    
}

function ValidaFacturas() {
    
    var codInicial = $("#CodInicialFactura").val();
    var codFinal = $("#CodFinalFactura").val();
    var flag = true;
    if ($("#CodInicialFactura").val() == "" && $("#CodFinalFactura").val() == "" && $("#FechaInicial").val() != "") {
        flag = true;
        ValidaFechas();
    } else {
        if ($("#CodFinalFactura").val() != "") {
            
            if ($("#CodFinalFactura").val() != "" && $("#CodInicialFactura").val() == "") {
                //MostrarMensaje("Importante", "Ingrese el código factura inicial", "error");
                //flag = false;
            } else {
                if (parseInt(codInicial) < parseInt(codFinal)) {
                    $("#DDL_Punto").attr('disabled', true);
                    $("#btnBuscar").attr('disabled', false);
                }
                else {
                    $("#DDL_Punto").attr('disabled', true);
                    //$("#btnBuscar").attr('disabled', true);
                    //MostrarMensaje("Importante", "El código factura final debe ser mayor al código factura inicial", "error");
                    //flag = false;
                }
            }
        }
        else {
            $("#DDL_Punto").attr('disabled', false);
            $("#btnBuscar").attr('disabled', false);
            flag = true;
        }
    }
    return flag;
}

function successPrint(data) {
    if (data.Correcto) {
        MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", "Reimpresion/Index", "success");
    }
    else {
        MostrarMensaje("Falló al reimprimir", rta.Mensaje);
    }
}

//function MostrarMensajeRedireccion(Titulo, Mensaje, UrlRedireccion, Tipo) {

//    var Type = "";

//    if (Tipo != undefined) {
//        Type = Tipo;
//    }
//    swal({
//        title: Titulo,
//        text: Mensaje,
//        showCancelButton: false,
//        closeOnConfirm: true,
//        type: Type
//    },
//    function (isConfirm) {
//        if (isConfirm) {
//            window.location = UrlRedireccion;
//        }
//    });
//}

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

function InhabilitarCopiarPegarCortar(control) {
    $("#" + control).bind("cut copy paste", function (e) {
        e.preventDefault();
    });
}


