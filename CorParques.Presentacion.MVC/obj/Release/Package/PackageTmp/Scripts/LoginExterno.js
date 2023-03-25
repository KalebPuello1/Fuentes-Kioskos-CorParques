var user = 0;
var userhistorico = "";
var inicializadointerval = false;
var ubicacion = location.href;
var ruta = `${ubicacion.slice(0, -6)}`
$(function () {
    $("#btnCambioClaveExterno").click(function () {
        if (validarFormulario("modalCRUD .modal-body")) {
    
            EjecutarAjax(`${ruta}/ActualizarCambioClave`, "GET", ObtenerObjeto("modalCRUD .modal-body *"), "SuccessChangePwdExterno", null);
            //EjecutarAjax(`${ubicacion}/ActualizarCambioClave`, "GET", ObtenerObjeto("modalCRUD .modal-body *"), "SuccessChangePwdExterno", null);
        }
    });

    $("#btnCancelar").click(function () {
        CancelLogin();
    });

    $("#frmLogin").submit(function () {
        return false;
    });
    $("#btnSave").click(function () {
        EjecutarLogin();
    });

    $("#txtUser").keypress(function (event) {
        if (event.which == 13) {
            $("#txtPwd").focus();
        }
    });

    $("#txtPwd").keypress(function (event) {
        if (event.which == 13) {
            EjecutarLogin();
        }
    });

});

console.log(`${ruta}/LoginExterna`)
/*console.log(`${ubicacion.slice(0, -6)}/`)
var ruta = `${ubicacion.slice(0, -6)}/`*/
function EjecutarLogin() {

    $("#lblError").hide();
    $("#Cambiopwd").hide();
    $("#Changepwd").removeClass("password");
    $("#ConfirChangepwd").removeClass("required");

    //EjecutarAjax("/CuentaExterna/LoginExterna", "POST", JSON.stringify({ user1: $("#txtUser").val(), pwd: $("#txtPwd").val() }), "SuccessLogin", null);
    EjecutarAjax(`${ruta}/LoginExterna`, "POST", JSON.stringify({ user1: $("#txtUser").val(), pwd: $("#txtPwd").val() }), "SuccessLogin", null);
    //EjecutarAjax(`${ubicacion}/LoginExterna`, "POST", JSON.stringify({ user1: $("#txtUser").val(), pwd: $("#txtPwd").val() }), "SuccessLogin", null);

}

function SuccessChangePwdExterno(data) {
    if (data.Correcto) {
        window.location = data.Elemento;
    }
    else {
        $("#lblErrorChangePwd").html(data.Mensaje);
        $("#lblErrorChangePwd").show();
    }
}

function SuccessLogin(data) {

    if (data.Correcto) {
        iniciarProceso();
        if (data.Mensaje === "OK") {
            cerrarModal("modalCRUD");
            window.location = data.Elemento;
            return;
        }
        if (data.Mensaje === "Cambiopwd") {
            $("#Changepwd").addClass("password");
            $("#ConfirChangepwd").addClass("required");
            abrirModal("modalCRUD");
            return;
        }
        abrirModal("modalCRUD");
        return;
    } else {
        //if (data.Mensaje.indexOf("[M]") >= 0) {
            //MostrarMensaje("Importante!", data.Mensaje.replace("[M]", ""), "error");
            MostrarMensaje("Importante!", data.Mensaje, "error");
        //}
    }
    cerrarModal("modalCRUD");
    //$("#txtUser").val("");    
    $("#Password2").val("");
    $("#Changepwd").val("");
    $("#ConfirChangepwd").val("");

    /*if (data.Mensaje.indexOf("[M]") >= 0) {
        $("#lblError").html("El punto se encuentra en mantenimiento");
        $("#lblError").show();
    } else {
        $("#lblError").html(data.Mensaje);
        $("#lblError").show();
    }*/

}

function CancelLogin() {

    cerrarModal("modalCRUD");
    $("#txtUser").val("");
    $("#Password2").val("");
    $("#Changepwd").val("");
    $("#ConfirChangepwd").val("");
    $("#lblError").hide();
    $("#Cambiopwd").hide();
    $("#Changepwd").removeClass("password");
    $("#ConfirChangepwd").removeClass("required");

}

function MostrarCambioPwd() {

    $("#Cambiopwd").show();
    $("#Changepwd").addClass("password");
    $("#ConfirChangepwd").addClass("required");

}


