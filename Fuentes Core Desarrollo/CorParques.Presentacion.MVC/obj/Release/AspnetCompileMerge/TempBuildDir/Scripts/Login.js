var user = 0;
var userhistorico = "";
var inicializadointerval = false;
$(function () {
    $("#btnCambioClave").click(function () {
        if (validarFormulario("modalCRUD .modal-body")) {
            EjecutarAjax(urlBase + "Cuenta/ActualizarCambioClave", "GET", ObtenerObjeto("modalCRUD .modal-body *"), "SuccessChangePwd", null);
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

function EjecutarLogin() {

    $("#lblError").hide();
    $("#Cambiopwd").hide();
    $("#Changepwd").removeClass("password");
    $("#ConfirChangepwd").removeClass("required");

    EjecutarAjax(urlBase + "Cuenta/Login", "POST", JSON.stringify({ user1: $("#txtUser").val(), pwd: $("#txtPwd").val() }), "SuccessLogin", null);


}

function SuccessChangePwd(data) {
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
        if (data.Mensaje.indexOf("[M]") >= 0) {
            MostrarMensaje("Importante!", data.Mensaje.replace("[M]", ""), "error");
        }
    }
    cerrarModal("modalCRUD");
    //$("#txtUser").val("");    
    $("#Password2").val("");
    $("#Changepwd").val("");
    $("#ConfirChangepwd").val("");

    if (data.Mensaje.indexOf("[M]") >= 0) {
        $("#lblError").html("El punto se encuentra en mantenimiento");
        $("#lblError").show();
    } else {
        $("#lblError").html(data.Mensaje);
        $("#lblError").show();
    }

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


