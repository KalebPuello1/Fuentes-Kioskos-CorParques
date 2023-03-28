
function ConsultarBoleta() {
    var codigo = $("#input-codigo").val();
    if (codigo.toString().length > 0) {
        $(".loader-wrapper").css("display", "block");
        EjecutarAjax(urlBase + "Kioscos/ObtenerCodigoManual", "GET", { Codigo: codigo.toString() }, "successCod1", null);
        document.getElementById("input-codigo").value = '';
        document.getElementById("input-codBarras").value = '';
    }
}

function ConsultarCodBarras() {
    var codBarras = $("#input-codBarras").val();
    if (codBarras.toString().length > 0) {
        $(".loader-wrapper").css("display", "block");
        EjecutarAjax(urlBase + "Kioscos/ObtenerCodigoBarras", "GET", { CodBarras: codBarras.toString() }, "successCod1", null);
        document.getElementById("input-codBarras").value = '';
        document.getElementById("input-codigo").value = '';
    }
}

function Imprimir() {
    var codigos = $("#codigos").val();
    $(".loader-wrapper").css("display", "block");
    EjecutarAjax(urlBase + "Kioscos/Imprimir", "GET", { Codigo: codigos.toString() }, "successCodImp", null);
}

function Reimprimir() {
    var codigos = $("#codigos").val();
    $(".loader-wrapper").css("display", "block");
    EjecutarAjax(urlBase + "Kioscos/Reimprimir", "GET", { Codigo: codigos.toString() }, "successCodImp", null);
}

function successCodImp(data) {
    $(".loader-wrapper").css("display", "none");
    if (data.Correcto) {
        $("#modalSuccess").modal('show');
    }
    else {
        var codigos = document.getElementById('codigos');
        codigos.value = data.Elemento.toString();

        var msgError = document.getElementById('msgError');
        msgError.innerHTML = data.Mensaje.toString();
        $("#modalError").modal('show');
    }
}

function successCod1(data) {
    $(".loader-wrapper").css("display", "none");
    if (data.redirectToUrl != null) {
        window.location.href = data.redirectToUrl;
    }
    else {
        //$("#mymodal").modal('show');
        MostrarMensaje("¡Error!", "El codigo ingresado no corresponde a ningun producto.", "error");
    }
}

function RedireccionImprCorrecta() {
    window.location.replace("/Kioscos");
}

function RedireccionImprError() {
    $("#modalError").modal('hide');
    document.getElementById('btn-continuar').disabled = true;
    document.getElementById('btn-supervisor').hidden = false;
    document.getElementById('btn-reimpresion').hidden = true;
    AbrirModalSupervisor();
}

function AbrirModalSupervisor() {
    EjecutarAjax(urlBase + "Cuenta/ObtenerLoginSupervisor", "GET", null, "printPartialModal", {
        title: "Confirmación supervisor", hidesave: true, modalLarge: false, OcultarCierre: true
    });

    setTimeout(() => {
        document.getElementById('btn-continuar').hidden = true;
        document.getElementById('btn-supervisor').hidden = true;
        document.getElementById('btn-reimpresion').hidden = false;
    }, 50);
}

function CancelarLogin() {
    document.getElementById('btn-reimpresion').hidden = true;
    document.getElementById('btn-supervisor').hidden = false;
    cerrarModal('modalCRUD');
}

function ObtenerIdSupervisor(id) {
    IdSupervisorLogueado = id;
}

function OpcionImprimir() {
    var consecutivos = $("#codigos").val();
    abrirModal("modalCorreo");
    var nombre = document.getElementById("input-nombre");  
    var correo = document.getElementById("input-correo");
    var enviar = document.getElementById("EnviarCorreo");
    var tyc = document.getElementById("TyC");
    tyc.onchange = function () {
        enviar.disabled = !this.checked;
    }

    $("#EnviarCorreo").click(function () {
        document.getElementById("EnviarCorreo").disabled = true;
        if (validarFormulario("frm-correoConfirmacion")) {
            if (validarEmail(correo.value)) {
                $(".loader-wrapper").css("display", "block");
                var elemento = new Object();
                elemento.Nombre = nombre.value;
                elemento.Correo = correo.value;
                EjecutarAjax(urlBase + "Kioscos/EnviarCorreo",
                    "POST", JSON.stringify(elemento), "successEnviarCorreo", null);
            }
            else {
                MostrarMensaje("¡Importante!", "El campo correo no es correcto.", "warning");
            }
        }
    });

    $('#BtnAtrasCorreo').click(function () {
        cerrarModal("modalCorreo");
        document.getElementById("input-correo").value = '';
        document.getElementById("input-nombre").value = '';
        document.getElementById("TyC").checked = false;
        document.getElementById("EnviarCorreo").disabled = true;
    });
}

function successEnviarCorreo(data) {
    $(".loader-wrapper").css("display", "none");
    if (data.Correcto) {
        document.getElementById("input-correo").value = '';
        document.getElementById("input-nombre").value = '';
        document.getElementById("TyC").checked = false;
        document.getElementById("EnviarCorreo").disabled = true;
        document.getElementById("codConfirmacion").value = '';
        cerrarModal("modalCorreo");
        abrirModal("modalCodConfirmacion");

    }
    else {
        document.getElementById("input-correo").value = '';
        MostrarMensaje("¡Importante!", "Ocurrió un error al enviar correo por favor vuelve a intentarlo, verifica que los campos esten correctamente diligenciados.", "error");
    }
}

function EnviarCodConfirmacion() {
    var codConfirmacion = document.getElementById("codConfirmacion");
    if (validarFormulario("frm-codConfirmacion")) {
        $(".loader-wrapper").css("display", "block");
        EjecutarAjax(urlBase + "Kioscos/EnviarCodConfirmacion",
            "GET", { codConfirmacion: codConfirmacion.value }, "successCodConfirmacion", null);
    }
}

function successCodConfirmacion(data) {
    $(".loader-wrapper").css("display", "none");
    if (data.Estado) {
        document.getElementById("codConfirmacion").value = '';
        var consecutivos = $("#codigos").val();
        window.location.href = "ImpresionBoleta?Codigo=" + consecutivos;
    }
    else {
        document.getElementById("codConfirmacion").value = '';
        MostrarMensaje("¡Error!", "Código invalido, verifica que el código ingresado sea correcto o vuelve a intentarlo.", "error");
    }
}