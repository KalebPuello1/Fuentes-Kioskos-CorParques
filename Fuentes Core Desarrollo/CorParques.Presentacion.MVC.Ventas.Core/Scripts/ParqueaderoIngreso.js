var inicializadointervalSalidaPark = false;



$(function () {
    
    $("#lnkSalir").click(function () {
        EjecutarAjax(urlBase + "Parqueadero/Salir", "GET", null, "printPartialModal", { title: "Salida de Parqueadero", url: urlBase + "ListaPrecio/Insert", metod: "GET", func: "successInsertListaPrecios", func2: "loadCalendar", hidesave: true });
        $("#divParqueaderoSalidaSuccess").hide();
        $("#TextoParqueaderoSalidaSuccess").text("");

        $("#divParqueaderoSalidaDanger").hide();
        $("#TextoParqueaderoSalidaDanger").text("");
    });

    setEventSelect();

    $('#textBoxPlaca').mask('AAAAAAAA', {
        'translation': {
            A: { pattern: /[A-Za-z0-9]/ }
        }
    });
});

function setEventSelect() {
    $('#bodyTipoVehiculos').on('click', 'tr', function () {
        cleanSuccess();
        $("#hiddenIdTipoVehiculo").val($(this).attr("id"));
        $(this).addClass("success");
    });

   
}

function cleanSuccess() {
    $("#hiddenIdTipoVehiculo").val("");
    $('#bodyTipoVehiculos > tr').each(function () {
        if ($(this).hasClass("success"))
            $(this).removeClass("success")
    });
}

$("#btnCancelar").click(function () {
    cleanSuccess();
    $("#textBoxPlaca").val("");
    $("#tabletipovehiculos").removeClass("errorValidate");
    $("#tabletipovehiculos").attr("data-mensajeerror", "");

    $("#textBoxPlaca").removeClass("errorValidate");
    $("#textBoxPlaca").attr("data-mensajeerror", "");
});


$("#btnEnviarIngreso").click(function () {
    $("#tabletipovehiculos").removeClass("errorValidate");
    $("#tabletipovehiculos").attr("data-mensajeerror", "");

    $("#textBoxPlaca").removeClass("errorValidate");
    $("#textBoxPlaca").attr("data-mensajeerror", "");
    var isValidForm = true;

    if ($("#textBoxPlaca").val() == "" || $("#textBoxPlaca").val() == " ") {
        $("#textBoxPlaca").attr("data-mensajeerror", "Este campo es obligatorio");
        $("#textBoxPlaca").addClass("errorValidate");
        isValidForm = false;
    } else if ($("#textBoxPlaca").val().length < 4) {
        $("#textBoxPlaca").attr("data-mensajeerror", "Minimo 4 caracteres");
        $("#textBoxPlaca").addClass("errorValidate");
        isValidForm = false;
    }

    if ($("#hiddenIdTipoVehiculo").val() == "" || $("#hiddenIdTipoVehiculo").val() == "0") {
        $("#tabletipovehiculos").attr("data-mensajeerror", "Este campo es obligatorio");
        $("#tabletipovehiculos").addClass("errorValidate");
        isValidForm = false;
    }

    
    if (isValidForm) {
        var _Ingreso = new Object();
        _Ingreso.Placa = $("#textBoxPlaca").val();
        _Ingreso.IdTipoVehiculo = $("#hiddenIdTipoVehiculo").val();

        mostrarAlerta('Estado', 'Enviando...');
        EjecutarAjax(urlBase + "Parqueadero/Ingresar", "POST", JSON.stringify(_Ingreso), "SuccessParqueaderoIngresar", null);
    } else {
        mostrarTooltip();
        MostrarMensaje("Importante", "Hay inconsistencias en el formulario, revise los campos demarcados con color rojo.", "error");
        return false;
    }
});

function SuccessParqueaderoIngresar(data) {
    if (data.Correcto) {
        cleanSuccess();
        $("#textBoxPlaca").val("");
        mostrarAlerta('Estado', 'Vehiculo ingresado con exito.');
        EjecutarAjax(urlBase + "Parqueadero/GetPartial", "GET", null, "printPartialParqueaderoIngresar", { div: "#listView", func: "setEventSelect" });
    }
    else {
        mostrarAlerta('Estado', data.Mensaje);
    }
}

function printPartialParqueaderoIngresar(data, values) {
    $(values.div).html(data);
    window[values.func]();
}

