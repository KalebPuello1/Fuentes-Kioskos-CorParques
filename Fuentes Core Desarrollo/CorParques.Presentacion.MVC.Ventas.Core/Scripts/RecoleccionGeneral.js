//Powered by RDSH
var paso = "";
var adelante = true;
var cantidadpasos = 0;
var inicializadointerval = false;

$(function () {

    $("#btnCancelRecoleccion").click(function () {
        //var element = ObtenerObjeto("frmtest");
        Cancel();

    });

    $("#btnCancelarLogin").click(function () {
        $("#txt_DocumentoEmpleado").val("");
        $("#Password").val("");
        $('#divPassword').hide();
        cerrarModal("myModal");
    });

    $("#btnAceptarLogin").click(function () {

        if ($("#txt_DocumentoEmpleado").val().length <= 0 || $("#Password").val() <= 0)  {
            MostrarMensaje("Importante", "Debe digitar usuario y contraseña", "error");
        } else {
            iniciarProceso();
            EjecutarAjax(urlBase + "Recoleccion/LoginSupervisor", "POST", JSON.stringify({ user: $("#txt_DocumentoEmpleado").val(), pwd: $("#Password").val() }), "SuccessLogin", null, true);
            
        }
 
    });

    $("#btnLimpiar").click(function () {

        $("#txt_DocumentoEmpleado").val("");
        $("#Password").val("");
        $('#divPassword').hide();

    });

    $("#txt_DocumentoEmpleado").focus();

    $("#btnSaveRecoleccion").click(function () {
        if (ValidarTopes()) {
            MostrarConfirm("Importante!", "¿Está seguro de realizar la recolección? ", "CorfimarSupervisor");
            return false;
        }
    });

    $(this).click(function () {
        setearfocus();
    });
    $(this).keydown(function () {
        setearfocus();
    });

    $("#txt_DocumentoEmpleado").keyup(function () {
        if (!inicializadointerval) {
            inicializadointerval = true;
            var refreshIntervalId = setInterval(function () { EjecutarLogin(); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);

        }

    });

    $('#txt_DocumentoEmpleado').focusout(function () {
        if (($('#myModal').is(':visible'))) {
            setTimeout(function () { $('#txt_DocumentoEmpleado').focus() }, 500);
        }
    });

    $(".RecoleccionValor:input[type=text]").mask("000.000.000", { reverse: true });

    $(".expandirDocumentos").click(function () {
        if ($('#tdExpandirDocumentos').is(':visible')) {
            $("#iconoPlusDocumento").removeProp("fa fa-minus-square");
            $("#iconoPlusDocumento").addClass("fa fa-plus-square");
            $("#tdExpandirDocumentos").hide();            
        } else {
            $("#tdExpandirDocumentos").show();
            $("#iconoPlusDocumento").removeProp("fa fa-plus-square");
            $("#iconoPlusDocumento").addClass("fa fa-minus-square");
            $("#divTotaldocumentos").html("");
            TotalizarCheckBoxInicial("documentos");
        }
        
    });

    $(".expandirVoucher").click(function () {
        if ($('#tdExpandirVoucher').is(':visible')) {
            $("#iconoPlusVoucher").removeProp("fa fa-minus-square");
            $("#iconoPlusVoucher").addClass("fa fa-plus-square");
            $("#tdExpandirVoucher").hide();
        } else {
            $("#tdExpandirVoucher").show();
            $("#iconoPlusVoucher").removeProp("fa fa-plus-square");
            $("#iconoPlusVoucher").addClass("fa fa-minus-square");            
        }

    });

    $(this).keydown(function () {
        setearfocus();
    });

    if (!PostBackGuardar()) {
        return false;
    }

    if (ExisteCierre()) {
        $("#wizard1").hide();
        MostrarMensajeRedireccion("Importante", "No es posible realizar mas alistamientos de recolección porque existe un alistamiento de cierre.", "Home/Index", "error");
        return false;
    }   

});

function EjecutarLogin() {

    if (($('#myModal').is(':visible'))) {
        $('#divPassword').show();
        $("#Password").focus();
    }

}

function SuccessLogin(rta){

    if (rta.Correcto) {
        $("#IdUsuarioSupervisor").val(rta.Elemento.Id);
        GuardarRecoleccion();
    } else {
        $("#txt_DocumentoEmpleado").val("");
        $("#Password").val("");
        $('#divPassword').hide();
        MostrarMensaje("Importante", rta.Mensaje, "error");
        finalizarProceso();
    }

}

function setearfocus() {
    if (($('#myModal').is(':visible')) && $("#txt_DocumentoEmpleado").val().length < 13) {        
        $("#txt_DocumentoEmpleado").focus();
    } else {
        if (($('#myModal').is(':visible'))) {
            $('#divPassword').show();
            $("#Password").focus();
        }
    }
}

function ValidateCount(ctrl) {
    $(ctrl).attr("data-mensajeerror", "");
    var cantmax = $(ctrl).data("value");
    $(ctrl).removeClass("errorValidate");
    $(ctrl).removeClass("errorBrazalete");
    if (parseInt(cantmax) < parseInt($(ctrl).val())) {
        $(ctrl).attr("data-mensajeerror", "No se puede retornar mas boletas de las que tiene en caja");
        $(ctrl).addClass("errorValidate");
        $(ctrl).addClass("errorBrazalete");
        mostrarTooltip();
        return false;
    }
    return true;
}

function successGuardarRecoleccion(rta) {
    if (rta.Correcto) {
        if (rta.Mensaje.length > 0) {            
            MostrarMensajeRedireccion("Importante", rta.Mensaje, "Home/Index", "success");
        } else {
            MostrarMensajeRedireccion("Importante", "Recolección de taquilla generada con éxito.", "Home/Index", "success");
        }

    }
    else {
        MostrarMensaje(rta.Mensaje);
        $("#txt_DocumentoEmpleado").val("");
        $("#Password").val("");
        $('#divPassword').hide();
        MostrarMensaje("Importante", rta.Mensaje, "error");
    }
    finalizarProceso();
}

function GuardarRecoleccion() {
    
    var elemento = ObtenerObjeto("frmRecoleccion *");

    EjecutarAjaxJson(urlBase + "Recoleccion/InsertarRecoleccion", "POST", ObtenerObjeto("frmRecoleccion *"), "successGuardarRecoleccion", null, false);
    //$("#frmRecoleccion").submit();
}

//Esta funcion se ejecuta cuando el usuario haga clic en el boton de finalizar sobre el wizard.
function Finalizar() {

    var blnFlag = true;

    if (PasoValido('objRecoleccionAuxliar_MostrarBase')) {
        if (!ValidarBase()) {
            blnFlag = false;
            MostrarMensaje("Importante", "Revise las cantidades digitadas en la preparación para la base.", "error");
        }
    }

    if (PasoValido('objRecoleccionAuxliar_MostrarCorte')) {
        if (!ValidarCorte() && blnFlag) {
            blnFlag = false;
            MostrarMensaje("Importante", "Revise las cantidades digitadas en la preparación para el corte.", "error");
        }
    }
    if (PasoValido('objRecoleccionAuxliar_MostrarCorte')) {
        if (!ValidarCorte() && blnFlag) {
            blnFlag = false;
            MostrarMensaje("Importante", "Revise las cantidades digitadas en la preparación para el corte.", "error");
        }
    }
    if ($(".txtBoleteriareturn.errorValidate").length > 0)
    {
        blnFlag = false;
        MostrarMensaje("Importante", "Revise las cantidades digitadas en la boleteria.", "error");
    }


    //RDSH: Se remueve validación de total de la recolección.
    //if (ValidarTotalRecoleccion())
    //{
    //    blnFlag = false;
    //    MostrarMensaje("Importante", "No se puede realizar una recolección que supere el total de las ventas en efectivo para hoy.", "error");
    //}

    var sectionValidate = $(".stepContainer>div:visible");
    if (!validarFormulario(sectionValidate.attr("id") + " *")) {
        blnFlag = false;
    }

    if (blnFlag) {
        MostrarConfirm("Importante", "¿Realmente desea guardar la información ingresada?", "GuardarRecoleccion");
    }
}

function CorfimarSupervisor() {
    $('#myModal').modal('show');
}


function Total(txt, prefijoDeno, prefijoTotal, claseTotal) {
    var total = 0;
    var indice = "0";
    indice = txt.id.split('_')[1];

    if (txt.value.trim() != "") {
        total = (parseInt($("#" + prefijoDeno + indice).html().trim()) * parseInt(txt.value.trim()));
    }
    $("#" + prefijoTotal + '_' + indice).html(FormatoMoneda(total));
    Totalizar(claseTotal, prefijoTotal);
    //Valida si hace obligatorio el campo de sobres para billetes o monedas.
    SolicitarNumeroSobre(txt, claseTotal);
}

function Totalizar(claseTotal, prefijoTotal) {
    var Total = 0;

    $("." + claseTotal).each(function (index, element) {
        if ($("#" + element.id).html().trim().length > 0) {
            Total = Total + parseInt(RemoverFormatoMoneda($("#" + element.id).html().trim()));
        }
    });

    if (Total > 0) {
        $("#div_" + prefijoTotal).html(FormatoMoneda(Total));
        ColorTotal(prefijoTotal);
    } else {
        $("#div_" + prefijoTotal).html("");
    }
}

//Pasa la seleccion del check box a la entidad.
function SeleccionarCheckBox(chk, clase) {
    if (chk.checked) {
        $("#" + chk.id).attr("value", "true");
    } else {
        $("#" + chk.id).attr("value", "false");
    }
    TotalizarCheckBox(chk, clase);
    SolicitarNumeroSobreCheck(clase);
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

    var ValorBase = 0;
    var MaximoBase = 0;

    if (PasoValido('objRecoleccionAuxliar_MostrarBase')) {
        if ($("#div_Total").html() !== "" && $("#objRecoleccionAuxliar_MaximoBase").val() !== "") {
            ValorBase = RemoverFormatoMoneda($("#div_Total").html());
            MaximoBase = parseInt($("#objRecoleccionAuxliar_MaximoBase").val());

            if (ValorBase != MaximoBase) {
                return false;
            }
        }
        else {
            return false;
        }
    }

    return true;
    //return $("#div_Total").html() !== "";
}

function ValidarCorte() {

    var ValorCorte = 0;
    var MaximoCorte = 0;

    if (PasoValido('objRecoleccionAuxliar_MostrarCorte')) {
        if ($("#div_TotalCorte").html() !== "" && $("#objRecoleccionAuxliar_MaximoCorte").val() !== "") {
            MaximoCorte = parseInt($("#objRecoleccionAuxliar_MaximoCorte").val());
            ValorCorte = RemoverFormatoMoneda($("#div_TotalCorte").html());

            //if (ValorCorte < MaximoCorte || ValidarTotalRecoleccion()) {
            if (ValorCorte < MaximoCorte) {
                return false;
            }
        }
        else {
            if (!PasoValido('objRecoleccionAuxliar_MostrarBase')) {
                return false;
            }
        }
    }

    return true;

    //return $("#div_TotalCorte").html() !== "";
    //MaximoCorte

}

function ValidarTopes() {

    var ValorCaja = ($("#objRecoleccionAuxliar_TotalVentasDia").val() - $("#objRecoleccionAuxliar_ValorCortesRealizados").val());
    var MaximoCorte = 0;
    if (PasoValido('objRecoleccionAuxliar_MostrarBase')) {
        if ($("#ValorRecoleccionBase").val().replaceAll(".","") != $("#objRecoleccionAuxliar_MaximoBase").val()) {
            MostrarMensaje("Importante", "Revise las cantidades digitadas en la preparación para la base.", "error");
            return false;
        }
    }
    if (PasoValido('objRecoleccionAuxliar_MostrarCorte')) {
        if ($("#ValorRecoleccionCorte").val().replaceAll(".", "", )  > ValorCaja) {
            MostrarMensaje("Importante", "Revise las cantidades digitadas en la preparación para el corte.", "error");
            return false;
        }
    }


    return true;

    //return $("#div_TotalCorte").html() !== "";
    //MaximoCorte

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
    //if (!PasoValido('objRecoleccionAuxliar_MostrarBase') && !PasoValido('objRecoleccionAuxliar_MostrarCorte')) {
    //    $(".actionBar").hide();
    //    MostrarMensajeRedireccion("Importante", "No ha llegado a ningún tope para realizar recolección", "/Home/Index", "error");        
    //} else {
    //    $(".actionBar").show();
    //}


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


    $(".buttonFinish").addClass("btn btn-success");
    $(".buttonNext").addClass("btn btn-primary");
    $(".buttonPrevious").addClass("btn btn-info");

    $("#div_MaximoBase").html(FormatoMoneda($("#div_MaximoBase").html()));
    $("#div_MaximoCorte").html(FormatoMoneda($("#div_MaximoCorte").html()));
    $("#div_TotalVentasDiaB").html(FormatoMoneda($("#div_TotalVentasDiaB").html()));
    $("#div_TotalVentasDiaC").html(FormatoMoneda($("#div_TotalVentasDiaC").html()));

    if (parseInt($("#IdRecoleccion").val()) > 0) {
        InicializarEdicion();
    }

    EstablecerFormatoMoneda();
    SetInhabilitarCopiarPegarCortar();
    TotalizarCheckBoxInicial('voucher');
    TotalizarCheckBoxInicial('documentos');
    TotalizarCheckBoxInicial('novedad');
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

function ValidarTotalRecoleccion() {
    var ValorBase = 0;
    var ValorCorte = 0;
    var TotalVentasDia = 0;
    var TotalRecoleccion = 0;
    var TotalCortesRealizados = 0;

    if (PasoValido('objRecoleccionAuxliar_MostrarBase')) {
        ValorBase = RemoverFormatoMoneda($("#div_Total").html());
    }

    if (PasoValido('objRecoleccionAuxliar_MostrarCorte')) {
        ValorCorte = RemoverFormatoMoneda($("#div_TotalCorte").html());
    }

    TotalRecoleccion = ValorBase + ValorCorte;
    TotalVentasDia = $("#objRecoleccionAuxliar_TotalVentasDia").val();
    TotalCortesRealizados = $("#objRecoleccionAuxliar_ValorCortesRealizados").val();

    //Si el total de recoleccion entre base y corte o solo corte es mayor que las ventas realizadas en el dia menos el valor de los cortes realizados
    //No permite guardar la recoleccion.
    return TotalRecoleccion > (TotalVentasDia - TotalCortesRealizados);

}

function InicializarEdicion() {
    $(".convalorbase").each(function (index, element) {
        Total(element, 'D_', 'Total', 'Total_Base')
    });

    $(".convalorcorte").each(function (index, element) {
        Total(element, 'DCorte_', 'TotalCorte', 'TotalCorte');
    });

}

//Formatea la columa de denominacion.
function EstablecerFormatoMoneda() {
    $(".formato_moneda").each(function (index, element) {
        element.innerText = FormatoMoneda(element.innerText);
    });
}

///Inhabilita las opciones de copiar pegar y cortar dentro de un text box
function SetInhabilitarCopiarPegarCortar() {
    $(".cantidadbase").each(function (index, element) {
        InhabilitarCopiarPegarCortar(element.id);
    });

    $(".cantidadcorte").each(function (index, element) {
        InhabilitarCopiarPegarCortar(element.id);
    });
}
function TopeMaximoRecoleccionCorte() {
    var TotalVentas = parseInt($("#objRecoleccionAuxliar_TotalVentasDia").val())
    var Base = parseInt($("#objRecoleccionAuxliar_MaximoBase").val())
    var MaximoRecoleccionCorte = 0;

    MaximoRecoleccionCorte = (TotalVentas - Base);

    return FormatoMoneda(MaximoRecoleccionCorte);

}


//Valida si hay alguna opcion para setear el wizard y evitar error de jquery.
function HayAlgunaOpcion() {
    var blnRetorno = false;

    if (PasoValido('objRecoleccionAuxliar_MostrarBase') || PasoValido('objRecoleccionAuxliar_MostrarCorte')) {
        blnRetorno = true;
    }

    return blnRetorno;

}

//Establece el campo sobre como obligatorio o no para la base y el corte.
function SolicitarNumeroSobre(txt, opcion) {
    var TipoDenominacion = "";
    TipoDenominacion = $("#" + txt.id).attr("data-TipoDenominacion");
    var blnHacerObligatorio = false;

    if (opcion.indexOf("Base") >= 0) {
        $(".cantidadbase").each(function (index, element) {
            if (parseInt($("#" + element.id).val().trim()) > 0 && $("#" + element.id).attr("data-TipoDenominacion") == TipoDenominacion) {
                blnHacerObligatorio = true;
            }
        });

        if (TipoDenominacion.indexOf("Bille") >= 0 && blnHacerObligatorio) {
            $("#SobreBilletesBase").removeClass("required");
            $("#SobreBilletesBase").addClass("required");
        } else if (TipoDenominacion.indexOf("Mone") >= 0 && blnHacerObligatorio) {
            $("#SobreMonedasBase").removeClass("required");
            $("#SobreMonedasBase").addClass("required");
        } else if (TipoDenominacion.indexOf("Bille") >= 0 && !blnHacerObligatorio) {
            $("#SobreBilletesBase").removeClass("required");
        } else if (TipoDenominacion.indexOf("Mone") >= 0 && !blnHacerObligatorio) {
            $("#SobreMonedasBase").removeClass("required");
        }
    }
    else {
        $(".cantidadcorte").each(function (index, element) {
            if (parseInt($("#" + element.id).val().trim()) > 0 && $("#" + element.id).attr("data-TipoDenominacion") == TipoDenominacion) {
                blnHacerObligatorio = true;
            }
        });

        if (TipoDenominacion.indexOf("Bille") >= 0 && blnHacerObligatorio) {
            $("#SobreBilletesCorte").removeClass("required");
            $("#SobreBilletesCorte").addClass("required");
        } else if (TipoDenominacion.indexOf("Mone") >= 0 && blnHacerObligatorio) {
            $("#SobreMonedasCorte").removeClass("required");
            $("#SobreMonedasCorte").addClass("required");
        } else if (TipoDenominacion.indexOf("Bille") >= 0 && !blnHacerObligatorio) {
            $("#SobreBilletesCorte").removeClass("required");
        } else if (TipoDenominacion.indexOf("Mone") >= 0 && !blnHacerObligatorio) {
            $("#SobreMonedasCorte").removeClass("required");
        }

    }
}

//Establece el campo sobre como obligatorio o no para voucher, documentos, novedades.
function SolicitarNumeroSobreCheck(clase) {
    var blnHacerObligatorio = false;

    $("." + clase).each(function (index, element) {
        if (element.checked) {
            blnHacerObligatorio = true;
        }
    });

    if (clase == "voucher" && blnHacerObligatorio) {
        $("#SobreVoucher").removeClass("required");
        $("#SobreVoucher").addClass("required");
    } else if (clase == "voucher" && !blnHacerObligatorio) {
        $("#SobreVoucher").removeClass("required");
    } else if (clase == "documentos" && blnHacerObligatorio) {
        $("#SobreDocumentos").removeClass("required");
        $("#SobreDocumentos").addClass("required");
    } else if (clase == "documentos" && !blnHacerObligatorio) {
        $("#SobreDocumentos").removeClass("required");
    } else if (clase == "novedad" && blnHacerObligatorio) {
        $("#SobreNovedad").removeClass("required");
        $("#SobreNovedad").addClass("required");
    } else if (clase == "novedad" && !blnHacerObligatorio) {
        $("#SobreNovedad").removeClass("required");
    }

}

function ExisteCierre() {
    var blnRetorno = false;
    var ValorExisteCierre = $("#objRecoleccionAuxliar_ExisteCierre").val();

    if (ValorExisteCierre === "True") {
        blnRetorno = true;
    }

    return blnRetorno;

}

///Valida que el numero de sobre sea diferente para monedas y para documentos.
function ValidarNumeroSobre(blnBase) {

    var strValorSobreMonedas = "";
    var strValorSobreBilletes = "";
    var bnlRetorno = false;

    if (blnBase) {
        strValorSobreMonedas = $("#SobreMonedasBase").val();
        strValorSobreBilletes = $("#SobreBilletesBase").val();
    }
    else {
        strValorSobreMonedas = $("#SobreMonedasCorte").val();
        strValorSobreBilletes = $("#SobreBilletesCorte").val();
    }

    if (strValorSobreMonedas.trim().length > 0 || strValorSobreBilletes.trim().length > 0) {
        if (strValorSobreMonedas.trim() === strValorSobreBilletes.trim()) {
            bnlRetorno = true;
        }
    }

    return bnlRetorno;
}

///Funcion para marcar o desmarcar todos los check box de una columna.
function SeleccionarTodos(Control, clase) {    
    $("." + clase).each(function (index, element) {
        element.checked = Control.checked;
        TotalizarCheckBox(element, clase);
    });

    SolicitarNumeroSobreCheck(clase);

}

///Funcion para totalizar voucher, documentos y novedades según la selección realizada.
function TotalizarCheckBox(checkbox, clase) {
    var Total = 0;
    var indice = $("#" + checkbox.id).attr("data-indice");
    var TotalAcumulado = $("#divTotal" + clase).html().trim();

    if (TotalAcumulado.trim().length > 0)
        Total = parseInt(RemoverFormatoMoneda(TotalAcumulado));


    if (checkbox.checked) {
        Total = Total + parseInt(RemoverFormatoMoneda($("#valor" + clase + '_' + indice).html().trim()));
    }
    else {
        Total = Total - parseInt(RemoverFormatoMoneda($("#valor" + clase + '_' + indice).html().trim()));
    }

    if (Total > 0) {
        $("#divTotal" + clase).html(FormatoMoneda(Total));

    } else {
        $("#divTotal" + clase).html("");
    }
}

///Como los check box inicial marcados al cargar la pagina, se debe totalizar al cargar la pagina.
function TotalizarCheckBoxInicial(clase) {

    $("." + clase).each(function (index, element) {
        TotalizarCheckBox(element, clase);
    });

}