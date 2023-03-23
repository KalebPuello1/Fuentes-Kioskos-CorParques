var user = 0;
var userhistorico = "";
var inicializadointerval = false;

document.getElementById("FrmLoginManual").style.display = "none";

$(function () {
    //InhabilitarCopiarPegarCortar("txtUser");
    $("#btnValidarSegundaClave").click(function () {
        if (validarFormulario("modalCRUD .modal-body")) {
            EjecutarAjax(urlBase + "Cuenta/ValidaContrasena2", "POST", JSON.stringify({ Pwd2: $("#Password2").val(), ChangePwd2: $("#Changepwd").val(), ConfirmPwd2: $("#ConfirChangepwd").val() }), "SuccessLogin", null);
        }
    });

    $("#btnCancelar").click(function () {
        CancelLogin();
    });

    $("#txtUser").focus();
    $(this).click(function () {
        setearfocus();
    });
    $(this).keydown(function () {
        setearfocus();
    });

    $("#frmLogin").submit(function () {
        return false;
    });
    $("#txtUser").keyup(function () {
        if (!inicializadointerval) {
            inicializadointerval = true;
            var refreshIntervalId = setInterval(function () { EjecutarLogin(); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);

        }

    });

    $("#Password2").keypress(function (event) {
        if (event.which == 13) {
            if (validarFormulario("modalCRUD .modal-body")) {
                EjecutarAjax(urlBase + "Cuenta/ValidaContrasena2", "POST", JSON.stringify({ Pwd2: $("#Password2").val(), ChangePwd2: $("#Changepwd").val(), ConfirmPwd2: $("#ConfirChangepwd").val() }), "SuccessLogin", null);
            }
        }
    });
});

function EjecutarLogin() {

    $("#lblError").hide();
    $("#lblErrorM").hide();
    $("#Cambiopwd").hide();
    $("#Changepwd").removeClass("password");
    $("#ConfirChangepwd").removeClass("required");

    EjecutarAjax(urlBase + "Cuenta/Login", "POST", JSON.stringify({ user1: $("#txtUser").val() }), "SuccessLogin", null);
    //if (($('#modalCRUD').is(':visible'))) {
    //    $("#Password2").focus();
    //}

}


function setearfocus() {
    if (!($('#modalCRUD').is(':visible'))) {
        $("#txtUser").focus();
    } else {
        if (!($('#Cambiopwd').is(':visible'))) {
            $("#Password2").focus();
        }
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
            $("#Cambiopwd").show();
            $("#Changepwd").addClass("password");
            $("#ConfirChangepwd").addClass("required");
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
    $("#txtUser").val("");
    $("#Password2").val("");
    $("#Changepwd").val("");
    $("#ConfirChangepwd").val("");

    if (data.Mensaje.indexOf("[M]") >= 0) {
        $("#lblError").html("El punto se encuentra en mantenimiento");
        $("#lblError").show();

        $("#lblErrorM").html("El punto se encuentra en mantenimiento");
        $("#lblErrorM").show();
    } else {
        $("#lblError").html(data.Mensaje);
        $("#lblError").show();

        $("#lblErrorM").html(data.Mensaje);
        $("#lblErrorM").show();

    }


}

function CancelLogin() {

    cerrarModal("modalCRUD");
    $("#txtUser").val("");
    $("#Password2").val("");
    $("#Changepwd").val("");
    $("#ConfirChangepwd").val("");
    $("#lblError").hide();
    $("#lblErrorM").hide();
    $("#Cambiopwd").hide();
    $("#Changepwd").removeClass("password");
    $("#ConfirChangepwd").removeClass("required");

}

function MostrarCambioPwd() {

    $("#Cambiopwd").show();
    $("#Changepwd").addClass("password");
    $("#ConfirChangepwd").addClass("required");

}

//****************************************************************************************************************************************************************************************************************

$("#BtnIngresarManual").click(function () {

    document.getElementById("FrmLoginManual").style.display = "block";
    document.getElementById("frmLogin").style.display = "none";
    document.getElementById("IniManual").style.display = "none";
    document.getElementById("lblError").style.display = "block  ";

});

$("#btnValidarSegundaClaveM").click(function () {

    var EjecutarAjaxUsuario = EjecutarAjax(urlBase + "Cuenta/Login", "POST", JSON.stringify({ user1: $("#txtUserM").val() }), "SuccessLogin", null);
    var u = document.getElementById("txtUserM").value;


    if (u == "") {
        alert("¡ CAMPO N° DE USUARIO VACIO !");
    } else {
        EjecutarAjaxUsuario
    }
});

$("#txtUserM").keypress(function (event) {

    if (event.which == 13) {

        var EjecutarAjaxUsuario = EjecutarAjax(urlBase + "Cuenta/Login", "POST", JSON.stringify({ user1: $("#txtUserM").val() }), "SuccessLogin", null);
        var u = document.getElementById("txtUserM").value;


        if (u == "") {
            alert("¡ CAMPO N° DE USUARIO VACIO !");
        } else {
            EjecutarAjaxUsuario
        }

    }
});

$("#btnCancelarM").click(function () {
    CancelManual()
});

function CancelManual() {

    document.getElementById("FrmLoginManual").style.display = "none";
    document.getElementById("frmLogin").style.display = "block";
    document.getElementById("IniManual").style.display = "block";

    CancelLogin();
}

