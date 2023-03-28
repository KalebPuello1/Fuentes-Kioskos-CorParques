$(function () {
    asignarSelect2();
    $(".moneda:input[type=text]").mask("000.000.000", { reverse: true });

    $("#txtValor").on("keyup", function (e) {
        var key = e.keyCode || e.charCode;
        // si la tecla es cero y el primer carácter es un cero
        if (key == 48 && this.value[0] == "0") {
            // se eliminan los ceros a la izquierda
            this.value = this.value.replace(/^0+/, '');
        }
    });

    $('#btnOK').click(function () {
            if ($("#ObservacionRec").val() != "" && $("#txtValor").val() != "" && $("#ddlUsuarios").val() != "") {
                $('#txtPassword').removeClass("required").removeClass("errorValidate").removeAttr('data-mensajeerror');
                $('#btnOK').attr('data-toggle', 'modal').attr('data-target', '#modalCRUD');
            }
            else {
                validarFormulario("frmRecambio");                
                $("#txtValor").val() == "" ? $('#txtValor').addClass("required").addClass("errorValidate").addClass('data-mensajeerror') : $('#txtValor').removeClass("required").removeClass("errorValidate").removeAttr('data-mensajeerror');
                $('#btnOK').removeAttr('data-toggle');
            }
    });
    
    InhabilitarCopiarPegarCortar('txtValor')

    $("#btnAceptarLogin").click(function () {
        if ($('#txtPassword').val() == "") {
            $('#txtPassword').attr("data-mensajeerror", "Este campo es obligatorio");
            $('#txtPassword').addClass("errorValidate");
        };
        //if ($('#ObservacionAprov').val() == "") {
        //    $('#ObservacionAprov').attr("data-mensajeerror", "Este campo es obligatorio");
        //    $('#ObservacionAprov').addClass("errorValidate");
        //};
        mostrarTooltip()
        if ($('#txtPassword').val() != "") {
            //&& $('#ObservacionAprov').val() != "") {
            var usuario = $("#txtUsuario").val();
            var clave = $('#txtPassword').val();
            ValidaClave(usuario, clave)
        }
    });

    $('#btnCancelar').click(function () {
        LimpiarControles();
    });

    $('#btnCancelarLogin').click(function () {
        cerrarModal("modalCRUD");
        $("#txtPassword").val("");
        $("#ObservacionAprov").val("");
    });
})

function SuccessRecambio(data) {

    if (data.Correcto) {
        if (data.Mensaje === "OK") {            
            MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "Recambio/Index", "success");
            $("#txtValor").val("");
            $("#txtObservaciones").val("");
            $("#ObservacionRec").val("");
            $("#ObservacionAprov").val("");
            $("#ddlUsuarios").val("").trigger('change');
            $("#txtUser").val("");
            $("#txtPassword").val("");
            $("#lblError").hide();
            $('#txtValor').removeClass("required").removeClass("errorValidate").removeAttr('data-mensajeerror');
            $('#ObservacionRec').removeClass("required").removeClass("errorValidate").removeAttr('data-mensajeerror');
            $('#ddlUsuarios').removeClass("required").removeClass("errorValidate").removeAttr('data-mensajeerror');
            return;
        }
        return;
    }
    $("#txtValor").val("");
    $("#txtObservaciones").val("");
    $("#ObservacionRec").val("");
    $("#ObservacionAprov").val("");
    $("#ddlUsuarios").val("").trigger('change');
    $("#txtUser").val("");
    $("#txtPassword").val("");
    $("#lblError").hide();
}

function ValidaClave(usuario, clave) {    
    if (validarFormulario("modalCRUD .modal-body")) {
        EjecutarAjax(urlBase + "Recambio/ValidaClave", "POST", JSON.stringify({ usuario: usuario, clave: clave }), "SuccessLogin", null);
    }
}

function OnChangeEvent(dropDownElement) {
    var selectedValue = dropDownElement.options[dropDownElement.selectedIndex].value;
    var usuario = dropDownElement.options[dropDownElement.selectedIndex].label
    $("#txtUsuario").val(selectedValue);
    document.getElementById("lblUsuario").innerText = usuario;
}

function CancelLogin() {
    cerrarModal("modalCRUD");
    $("#txtUser").val("");
    $("#txtPassword").val("");
    $("#lblError").hide();
    $('#ObservacionAprov').val("");
}

function SuccessLogin(data) {    
    if (data.Correcto) {
        if (data.Mensaje === "OK") {
            iniciarProceso();
            GuardarRecambio();
            cerrarModal("modalCRUD");
            return;
        }
        abrirModal("modalCRUD");
        return;
    }
    else {
        abrirModal("modalCRUD");
        if (data.Mensaje === "Usuario o contraseña incorrectos") {
            MostrarMensaje("Importante", "Contraseña incorrecta", "error");
        }
        return;
    }
    $("#txtUser").val("");
    $("#txtPassword").val("");
    $("#lblError").html(data.Mensaje);
    $("#lblError").show();
    $('#ObservacionRec').val("");
    $('#ObservacionAprov').val("");
}

function GuardarRecambio() {
    var cantidad = $("#txtValor").val().replace(".", "").replace(".", "");
    var observacionRec = $("#ObservacionRec").val();
    var observacionAprov = $("#ObservacionAprov").val();
    var idusuario = $("#ddlUsuarios").val();
    EjecutarAjax(urlBase + "Recambio/Insertar", "POST", JSON.stringify({ idusuario: idusuario, cantidad: cantidad, observacionRec: observacionRec, observacionAprov: observacionAprov }), "SuccessRecambio", null);
}

function LimpiarControles() {
    $("#txtValor").val("");
    $("#txtObservaciones").val("");
    $("#ObservacionRec").val("");
    $("#ObservacionAprov").val("");
    $("#ddlUsuarios").val("").trigger('change');
    $('#txtValor').removeClass("required").removeClass("errorValidate").removeAttr('data-mensajeerror');
}