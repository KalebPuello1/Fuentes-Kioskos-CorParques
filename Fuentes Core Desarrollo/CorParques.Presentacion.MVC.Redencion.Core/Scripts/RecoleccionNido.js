

var paso = "";
var adelante = true;
var cantidadpasos = 0;
var contador = 0;

$(function () {
    asignarSelect2();
    if (!PostBackGuardar()) {
        return false;
    }
    $("#btnFin").css('display', 'none');
    $(".loader-wrapper").css("display", "none");
    if (localStorage.TabActive == "System.Int32[]") {
        localStorage.TabActive = null;
    }
    if (localStorage.TabActive != null) {
        $("#" + localStorage.TabActive).addClass('active');
    }
    localStorage.TabActive = null;
    contador = 0;
    $("#myTab li").each(function (index) {
        contador++;
    })

    if (contador == 1) {
        $("#cierreRecoleccion").val(1);
    }

    $("#wizard1").smartWizard({
        onLeaveStep: function (obj, context) {
            var sectionValidate = $(".stepContainer>div:visible");
            if (!validarFormulario(sectionValidate.attr("id") + " *"))
                return false;

            if (context.fromStep > context.toStep) {
                adelante = false;
            } else {
                adelante = true;
            }

            if (paso == "Base" && adelante) {
                if (!ValidarBase()) {
                    return false;
                } else {
                    paso = "Corte";
                    adelante = true;
                }
            }
            else if (paso == "Corte" && adelante) {
                if (!ValidarCorte()) {
                    return false;
                } else {
                    paso = "Voucher";
                    adelante = true;
                }
            }

            if (context.toStep === 1 && PasoValido('objRecoleccionAuxliar_MostrarBase')) {
                paso = "Base";
            } else if (context.toStep === 1 && PasoValido('objRecoleccionAuxliar_MostrarCorte')) {
                paso = "Corte";
            } else if (context.toStep === 2 && PasoValido('objRecoleccionAuxliar_MostrarCorte')) {
                paso = "Corte";
            } else if (context.toStep === 3 && cantidadpasos === 4) {
                paso = "Voucher";
            } else if (context.toStep === 3 && cantidadpasos === 3) {
                paso = "Documentos";
            } else if (context.toStep === 4) {
                paso = "Documentos";
            } else if (context.toStep === 2 && cantidadpasos === 3) {
                paso = "Voucher";
            }

            return true;
        },
        onFinish: Finalizar
    });
    Inicializar();    
});

function GuardarRecoleccion() {
    $("#frmRecoleccion").submit();
}

//Esta funcion se ejecuta cuando el usuario haga clic en el boton de finalizar sobre el wizard.
function Finalizar() {
    Detalle();
    contador = 0;
    $("#myTab li").each(function (index) {
        contador++;
    })

    if (contador == 1) {
        $("#cierreRecoleccion").val(1);
        var IdApertura = $("#IdApertura").val();
        $("#IdPunto").val($("#IdPunto").val());
        //$(".buttonFinish").addClass("btn btn-success").attr('data-toggle', 'modal').attr('data-target', '#myModal');
        iniciarProceso();
        GuardarRecoleccion();
        //abrirModal("myModal");
        //EjecutarLogin($("#txtUsuario").val());
    }
    else {
        $("#cierreRecoleccion").val(0);
        $("#IdPunto").val($("#IdPunto").val());
        iniciarProceso();
        GuardarRecoleccion();
    }
    contador = 0;
}

function SuccessDetalle() {
    if (data.Mensaje === "OK") {
        
        $(".buttonFinish").addClass("btn btn-success").attr('data-toggle', 'modal').attr('data-target', '#myModal');
    }
    else {

    }
}

function Detalle() {
    if (document.getElementById('datatable-base') != null) {
        var rowsBase = document.getElementById('datatable-base').getElementsByTagName('tbody')[0].getElementsByTagName('tr');
        for (i = 0; i < rowsBase.length; i++) {
            var cantidadBase = $("#ValorSupervisor_" + i).val();
            var totalBase = parseInt($("#D_" + i)[0].textContent.replace("$", "").replace(".", "").replace(".", "").replace(",", ".").replace(",", ".").replace(",", ".")) * cantidadBase;
            $('#CantidadBase_' + i).val(cantidadBase)
            $('#TotalBase_' + i).val(totalBase)
            //FormatMoney($('#TotalBase_' + i)[0]);
            //FormatMoney($("#DD_" + i)[0]);
            //FormatMoney($("#DDt_" + i)[0]);
        }
    }

    if (document.getElementById('datatable-corte') != null) {
        var rowsCorte = document.getElementById('datatable-corte').getElementsByTagName('tbody')[0].getElementsByTagName('tr');
        for (i = 0; i < rowsCorte.length; i++) {
            var cantidadCorte = $("#ValorCorteSupervisor_" + i).val();
            var totalCorte = parseInt($("#DCorte_" + i)[0].textContent.replace("$", "").replace(".", "").replace(".", "").replace(",", ".").replace(",", ".").replace(",", ".")) * cantidadCorte;
            $('#CantidadCorte_' + i).val(cantidadCorte)
            $('#TotalCorte_' + i).val(totalCorte)
            //FormatMoney($('#TotalCorte_' + i)[0]);
            //FormatMoney($("#DDCorte_" + i)[0]);
            //FormatMoney($("#DDCortet_" + i)[0]);
        }
    }

    if (document.getElementById('datatable-voucher') != null) {
        var rowsVoucher = document.getElementById('datatable-voucher').getElementsByTagName('tbody')[0].getElementsByTagName('tr');
        for (i = 0; i < rowsVoucher.length; i++) {
            var chkVoucher = $("#chkvs_" + i)[0].checked;
            $('#chkvc_' + i).prop('checked', chkVoucher);
            $("#chkvc_" + i).val(chkVoucher);//tellez
        }
    }
    if (document.getElementById('datatable-documentos') != null) {
        var rowsDocumentos = document.getElementById('datatable-documentos').getElementsByTagName('tbody')[0].getElementsByTagName('tr');
        for (i = 0; i < rowsDocumentos.length; i++) {
            var chkDocumento = $("#chkds_" + i)[0].checked;
            $('#chkdc_' + i).prop('checked', chkDocumento);
            $("#chkdc_" + i).val(chkDocumento);//tellez
        }
    }
    if (document.getElementById('datatable-novedades') != null) {
        var rowsDocumentos = document.getElementById('datatable-novedades').getElementsByTagName('tbody')[0].getElementsByTagName('tr');
        for (i = 0; i < rowsDocumentos.length; i++) {
            var chknovedad = $("#chkns_" + i)[0].checked;
            $('#chknc_' + i).prop('checked', chknovedad);
            $("#chknc_" + i).val(chknovedad);//tellez
        }
    }
}



function Total(txt, prefijoDeno, prefijoTotal, claseTotal) {
    var total = 0;
    var indice = "0";
    indice = txt.id.split('_')[1];

    if (txt.value.trim() != "") {
        total = (parseInt($("#" + prefijoDeno + indice).html().trim().replace("$", "").replace(".", "").replace(".", "").replace(",", ".").replace(",", ".").replace(",", ".")) * parseInt(txt.value.trim()));
    }
    $("#" + prefijoTotal + '_' + indice).html(total);
    FormatMoney($("#" + prefijoTotal + '_' + indice)[0]);
    Totalizar(claseTotal, prefijoTotal);
}

function Totalizar(claseTotal, prefijoTotal) {
    var Total = 0;

    $("." + claseTotal).each(function (index, element) {
        if ($("#" + element.id).html().trim().length > 0) {
            //Total = Total + parseInt($("#" + element.id).html().trim());
            Total = Total + parseInt(RemoverFormatoMoneda($("#" + element.id).html().trim()));
        }
    });

    $("#div_" + prefijoTotal).html(FormatoMoneda(Total));
    ColorTotal(prefijoTotal);

}

//Pasa la seleccion del check box a la entidad.
function SeleccionarCheckBox(chk) {
    if (chk.checked) {
        $("#" + chk.id).attr("value", "true");
    } else {
        $("#" + chk.id).attr("value", "false");
    }
    if (chk.className == "voucher") {
        TotalVoucher();
    }
    if (chk.className == "documentos") {
        TotalDocumentos();
    }
    if (chk.className == "Novedad") {
        TotalNovedad();
    }
}

function SeleccionarTodos(Control, clase) {
    var totalVou = 0;
    $("#div_Total" + clase).html("");
    $("." + clase).each(function (index, element) {
        element.checked = Control.checked;
        if (element.checked) {
            var idname = "#valor" + clase + "_" + element.id.split('_')[1];
            totalVou += parseInt(RemoverFormatoMoneda($(idname).html()));
            $("#" + element.id).attr("value", "true");
        }
        else {
            $("#" + element.id).attr("value", "false");
        }
    });
    if (totalVou != 0) {
        $("#div_Total" + clase).html(FormatoMoneda(totalVou));
    }
}

function TotalNovedad() {
    var totalVou = 0;
    $("#div_TotalNovedad").html("");
    $(".Novedad").each(function (index, element) {
        if (element.checked) {
            var idname = "#valorNovedad_" + element.id.split('_')[1];
            totalVou += parseInt(RemoverFormatoMoneda($(idname).html()));
        }
    });
    if (totalVou != 0) {
        $("#div_TotalNovedad").html(FormatoMoneda(totalVou));
    }
}

function TotalVoucher() {
    var totalVou = 0;
    $("#div_Totalvoucher").html("");
    $(".voucher").each(function (index, element) {
        if (element.checked) {
            var idname = "#valorvoucher_" + element.id.split('_')[1];
            totalVou += parseInt(RemoverFormatoMoneda($(idname).html()));
        }
    });
    if (totalVou != 0) {
        $("#div_Totalvoucher").html(FormatoMoneda(totalVou));
    }
}

function TotalDocumentos() {
    var totalDoc = 0;
    $("#div_Totaldocumentos").html("");
    $(".documentos").each(function (index, element) {
        if (element.checked) {
            var idname = "#valordocumentos_" + element.id.split('_')[1];
            totalDoc += parseInt(RemoverFormatoMoneda($(idname).html()));
        }
    });
    if (totalDoc != 0) {
        $("#div_Totaldocumentos").html(FormatoMoneda(totalDoc));
    }
}

function Validaciones(clasevalores, prefijoTotal) {

    var indice = "0";
    var blnResultado = true;

    $("." + clasevalores).each(function (index, element) {
        if ($("#" + element.id).val().trim().length > 0) {
            if (parseInt($("#" + element.id).val().trim()) > 0) {
                indice = element.id.split('_')[1];

                if ($("#" + prefijoTotal + indice).html().trim().length <= 0) {
                    blnResultado = false;
                    return false;
                } else if (parseInt($("#" + prefijoTotal + indice).html().trim()) <= 0) {
                    blnResultado = false;
                    return false;
                }
            }
        }
        else {
            blnResultado = false;
            return false;
        }
    });

    return blnResultado;

}

function ValidarBase() {

    var base = RemoverFormatoMoneda($("#div_TotalSupervisorBase").html());
    var ValActual = RemoverFormatoMoneda($("#div_TotalSupervisor").html());

    if (base != ValActual) {
        return false;
    }
    else {
        return true;
    }
}

function ValidarCorte() {

    var corteSup = RemoverFormatoMoneda($("#div_TotalSupervisorCorte").html());
    var ValActual = RemoverFormatoMoneda($("#div_TotalCorteSupervisor").html());

    if (corteSup > ValActual) {
        return false;
    }
    else {
        return true;
    }


}

function ColorTotal(prefijoTotal) {
    var bnlCambiarColor = false;

    if (prefijoTotal.indexOf("Corte") >= 0) {
        bnlCambiarColor = ValidarCorte();
    }
    else {
        bnlCambiarColor = ValidarBase();
    }

    if (!bnlCambiarColor) {
        $("#div_" + prefijoTotal).css('color', 'red');
    }
    else {
        $("#div_" + prefijoTotal).css('color', 'green');
    }
}

function PasoValido(obj) {

    return $("#" + obj).val() == "True";

}

function Inicializar() {
    localStorage.TabActive = null;
    if (document.getElementById('datatable-base') != null) {
        var rowsBase = document.getElementById('datatable-base').getElementsByTagName('tbody')[0].getElementsByTagName('tr');
        for (i = 0; i < rowsBase.length; i++) {
            FormatMoney($("#D_" + i)[0]);
            FormatMoney($("#Dt_" + i)[0]);
        }
    }

    if (document.getElementById('datatable-corte') != null) {
        var rowsCorte = document.getElementById('datatable-corte').getElementsByTagName('tbody')[0].getElementsByTagName('tr');
        for (i = 0; i < rowsCorte.length; i++) {
            FormatMoney($("#DCorte_" + i)[0]);
            FormatMoney($("#DCortet_" + i)[0]);
        }
    }

    if (PasoValido('objRecoleccionAuxliar_MostrarBase')) {
        paso = "Base";
    } else if (PasoValido('objRecoleccionAuxliar_MostrarCorte')) {
        paso = "Corte"
    }
    if (PasoValido('objRecoleccionAuxliar_MostrarBase') && PasoValido('objRecoleccionAuxliar_MostrarCorte')) {
        cantidadpasos = 4;
    } else {
        cantidadpasos = 3;
    }
    $(".buttonFinish").addClass("btn btn-success");//.attr('data-toggle', 'modal').attr('data-target', '#myModal');
    $(".buttonNext").addClass("btn btn-primary")
    $(".buttonPrevious").addClass("btn btn-info");

    $("#btnValidarClave").click(function () {
        var usuario = $("#txtUsuario").val();
        var clave = $('#Password2').val()
        var observacion = $('#ObservacionSupervisor').val();
        ValidaClave(usuario, clave, observacion)
    });

    $("#btnOk").click(function () {
        
        EjecutarLogin($("#txtUsuario").val());
    });

    $(".paso").click(function () {
        
        var id = $(this).attr('id')
        var Usuario = $("#Usuario").val();
        var IdUsuarioSupervisor = $("#IdUsuarioSupervisor").val();
        $(".loader-wrapper").css("display", "block");
        localStorage.TabActive = id;
        $("#IdPunto").val();
        //document.location = document.location.origin + "/core/RecoleccionNido/ObtenerDatosRecoleccion?IdSupervisor=" + IdUsuarioSupervisor + "&IdPunto=" + $("#IdPunto").val() + "&IdRecoleccion=" + id;
        document.location = urlBase + "RecoleccionNido/ObtenerDatosRecoleccion?IdSupervisor=" + IdUsuarioSupervisor + "&IdPunto=" + $("#IdPunto").val() + "&IdRecoleccion=" + id;
    });

    $("#btnAceptarLogin").click(function () {
        if ($('#txtPassword').val() == "") {
            $('#txtPassword').attr("data-mensajeerror", "Este campo es obligatorio");
            $('#txtPassword').addClass("errorValidate");
        };
        if ($('#txtObservaciones').val() == "") {
            $('#txtObservaciones').attr("data-mensajeerror", "Este campo es obligatorio");
            $('#txtObservaciones').addClass("errorValidate");
        };
        mostrarTooltip()
        if ($('#txtPassword').val() != "" && $('#txtObservaciones').val() != "") {
            var usuario = $("#txtUsuario").val();
            var clave = $('#txtPassword').val()
            var observacion = $('#txtObservaciones').val();
            ValidaClave(usuario, clave, observacion)
        }
    });

    $("[id*=ValorSupervisor_]").each(function (i, v) {
        Total(v, 'D_', 'TotalSupervisor', 'Total_BaseSupervisor');
    });

    var div_TotalSupervisorBase = 0;
    $("[id*=Dt_]").each(function (i, v) {
        div_TotalSupervisorBase += RemoverFormatoMoneda($(v).html());
        $("#div_TotalSupervisorBase").html(FormatoMoneda(div_TotalSupervisorBase));
    });

    var div_TotalSupervisorCorte = 0;
    $("[id*=DCortet_]").each(function (i, v) {
        div_TotalSupervisorCorte += RemoverFormatoMoneda($(v).html());
        $("#div_TotalSupervisorCorte").html(FormatoMoneda(div_TotalSupervisorCorte));
    });

}

function setEventEdit() {

}

function FormatMoney(input) {
    var x;
    var flag = true;
    String.prototype.reverse = function () {
        return this.split("").reverse().join("");
    }
    if (input.value == undefined) {
        x = input.textContent.trim().replace("$", "").replace(".", "").replace(".", "").replace(",", ".").replace(",", ".").replace(",", ".");
        flag = false;
    } else {
        x = input.value;
    }
    x = x.replace(/,/g, "");
    x = x.reverse();
    x = x.replace(/.../g, function (e) {
        return e + ",";
    });
    x = x.reverse();
    x = x.replace(/^,/, "");
    if (flag) {
        input.value = "$" + x.replace(",", ".").replace(",", ".").replace(",", ".").replace(",", ".").replace(",", ".");
    }
    else {
        input.textContent = "$" + x.replace(",", ".").replace(",", ".").replace(",", ".").replace(",", ".").replace(",", ".");
    }

}

function PostBackGuardar() {
    var Resultado = $("#div_ResultadoRecoleccion").html();
    if (Resultado != undefined) {
        if (Resultado.indexOf("Error") >= 0) {
            MostrarMensajeRedireccion("Importante", Resultado, "Home/Index", "error");
        }
        else {
            MostrarMensajeRedireccion("Importante", Resultado, "Home/Index", "success");
        }
        return false;
    }
    return true;
}

function OnChangeEvent(dropDownElement) {
    var selectedValue = dropDownElement.options[dropDownElement.selectedIndex].value;
    var IdPunto = "";
    var IdUsuario = "";

    var objSplit = selectedValue.split('|');
    IdPunto = objSplit[0];
    IdUsuario = objSplit[1];
    $("#IdPunto").val(IdPunto);
    $(".loader-wrapper").css("display", "block");
    //document.location = document.location.origin + "/core/RecoleccionNido/ObtenerDatosRecoleccion?IdSupervisor=" + IdUsuario + "&IdPunto=" + IdPunto + "&IdRecoleccion=0";
    document.location = urlBase + "RecoleccionNido/ObtenerDatosRecoleccion?IdSupervisor=" + IdUsuario + "&IdPunto=" + IdPunto + "&IdRecoleccion=0";
    $(".loader-wrapper").css("display", "none");
}


function ValidaClave(usuario, clave, observacion) {
    if (validarFormulario("modalCRUD .modal-body")) {
        EjecutarAjax(urlBase + "RecoleccionNido/ValidaClave", "POST", JSON.stringify({ usuario: usuario, clave: clave, observacion: observacion }), "SuccessLogin", null);
    }
}

function EjecutarLogin(usuario) {
    $("#lblError").hide();
    $("#Cambiopwd").hide();
    $("#Password2").val("");
    $('#Observaciones').val("");
    EjecutarAjax(urlBase + "Cuenta/Login", "POST", JSON.stringify({ user1: usuario }), "SuccessLogin", null);

}

function SuccessLogin(data) {

    if (data.Correcto) {
        if (data.Mensaje === "OK") {
            iniciarProceso();
            GuardarRecoleccion();
            cerrarModal("modalCRUD");
            cerrarModal("myModal");
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
    $("#Password2").val("");
    $("#lblError").html(data.Mensaje);
    $("#lblError").show();
    $('#ObservacionTaquillero').val("");
    $('#ObservacionSupervisor').val("");
}

function CancelLogin() {
    cerrarModal("modalCRUD");
    $("#txtUser").val("");
    $("#Password2").val("");
    $("#lblError").hide();
    $('#ObservacionTaquillero').val("");
    $('#ObservacionSupervisor').val("");
    MostrarMensajeRedireccion("Importante", "Desea cancelar la recolección", "RecoleccionNido/Index", "success");
}

//function SuccessLogin(data) {

//        if (data.Correcto) {
//            MostrarMensajeRedireccion("Importante", Resultado, "RecoleccionNido/Index", "success");
//        }
//        else {

//        }
//}

function Confirmacion(Titulo, Mensaje) {
    swal({
        title: Titulo,
        text: Mensaje,
        showCancelButton: true,
        closeOnConfirm: true
    }, function (isConfirm) {
        if (isConfirm) {
            GuardarRecoleccion();
            cerrarModal("modalCRUD");
            cerrarModal("myModal");
            $('#ObservacionTaquillero').val("");
            $('#ObservacionSupervisor').val("");
        }
    });
}
