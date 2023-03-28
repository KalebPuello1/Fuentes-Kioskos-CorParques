$(function () {

    $('#textBoxPlaca').mask('AAAAAAAA', {
        'translation': {
            A: { pattern: /[A-Za-z0-9]/ }
        }
    });
});


$("#btnEnviarIngreso").click(function () {

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

    
    if (isValidForm) {
        var _Ingreso = new Object();
        _Ingreso.Placa = $("#textBoxPlaca").val();

        EjecutarAjax(urlBase + "Reimpresion/ParqueaderoReImprimir", "POST", JSON.stringify(_Ingreso), "SuccessParqueaderoReimprimir", null);
    } else {
        mostrarTooltip();
        MostrarMensaje("Importante", "Hay inconsistencias en el formulario, revise los campos demarcados con color rojo.", "error");
        return false;
    }
});


function SuccessParqueaderoReimprimir(data) {
    if (data.Correcto) {
        $("#textBoxPlaca").val("");
        MostrarMensaje("Importante", "Reimpresión generada con éxito", "success");
    }
    else {
        MostrarMensaje("Importante", data.Mensaje, "error");
    }
}
