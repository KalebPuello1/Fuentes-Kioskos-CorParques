var inicializadointerval = false;

$(function () {

    $('#CodBoleta').keyup(function () {


        if (!inicializadointerval) {
            inicializadointerval = true;
            var refreshIntervalId = setInterval(function () { ConsultarCodBarra1(); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);

        }
    });

    $("#CodBoleta2").keyup(function () {
        if (!inicializadointerval) {
            inicializadointerval = true;
            var refreshIntervalId = setInterval(function () { ConsultarCodBarra2(); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);

        }
    });


});


$("#btnCancelar").click(function () {
    MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "Cancelar", "");
});

$("#btnAsignar").click(function () {
    if (validarFormulario("frmCambioBoleta")) {
        MostrarConfirm("Importante!", "¿Está seguro de hacer el cambio de la boleta? ", "CambioBoleta", "");
    }
})

function CambioBoleta() {
    var cod1 = $("#CodBoleta").val();
    var cod2 = $("#CodBoleta2").val();
    EjecutarAjax(urlBase + "AsignacionBoleta/AsignarBoleta", "GET", { codigo1: cod1.toString(), codigo2: cod2 .toString()}, "sucessAsignacion", null);
}

function sucessAsignacion(rta) {
    if (rta.Correcto) {
        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "AsignacionBoleta", "success");
    } else {
        MostrarMensaje("", rta.Mensaje);
    }
}

function Cancelar() {
    window.location = urlBase + "AsignacionBoleta";
}

function ConsultarCodBarra1() {
    var cod1 = $("#CodBoleta").val();
    var cod2 = $("#CodBoleta2").val();

    if (cod1.toString().length > 0) {
        $("#CodBoleta2").val("");
        if (cod1.toString().length > 0) {
            EjecutarAjax(urlBase + "AsignacionBoleta/ObtenerBoleta", "GET", { Codigo: cod1.toString() }, "successCod1", null);
        }
    }
}

function ConsultarCodBarra2() {
    var cod1 = $("#CodBoleta").val();
    var cod2 = $("#CodBoleta2").val();

    if (cod2.toString().length > 0) {
        EjecutarAjax(urlBase + "AsignacionBoleta/ObtenerBoletas", "GET", { Codigo1: cod1.toString(), Codigo2 : cod2.toString() }, "successCod2", null);
    }
}

function successCod1(rta) {
    
    if (rta != null && rta.length > 0) {
        $("#CodBoleta").val("");
        MostrarMensaje("Importante", rta, "");

    } else {
        $("#CodBoleta2").prop("disabled", false);
    }
}

function successCod2(rta) {
    
    if (rta != null && rta.length > 0) {
        $("#CodBoleta2").val("");
        $("#btnAsignar").prop("disabled", true);
        MostrarMensaje("Importante", rta, "");

    } else {
        $("#btnAsignar").prop("disabled", false);
    }
}