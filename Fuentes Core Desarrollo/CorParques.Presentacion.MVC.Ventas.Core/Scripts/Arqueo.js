var ObservacionesTaq;

$(function () {

    $(".Numero:input[type=text]").mask("000.000.000", { reverse: true });

    $("#IdTaquillero").select2({
        placeholder: "* Seleccione el taquillero"
    });

    $("#IdTaquillero").change(function () {

        EjecutarAjax(urlBase + "Arqueo/ObtenerArqueo", "GET", { IdUsuario: $(this).val() }, "SucessChange", null);

    });

    $("#btnCancel").click(function () {
        //var element = ObtenerObjeto("frmtest");
        Cancel();

    });

    $("#btnSave").click(function () {
        if (!validarFormulario("frmArqueo *")) {
            return false;
        }
        //var element = ObtenerObjeto("frmtest");
        //MostrarConfirm("Importante!", "¿Está seguro de guardar el arqueo? ", "SolicitarLogin", "");
        SolicitarLogin();

    });


});


function SolicitarLogin()
{
    EjecutarAjax(urlBase + "Cuenta/ObtenerLogin", "GET", null, "printPartialModal", { title: "Confirmación taquillero", hidesave: true, modalLarge: false });
}

function GuardarAlistamiento() {
    
    var requeridos = $("#frmArqueo").find(".Valor");
    var Valores = "";
    var negativo = false;
    $.each(requeridos, function (index, value) {

        negativo = false;
        var valor = $(value).html().trim();
        if (valor.length <= 0) {
            valor = FormatoMoneda(0);
        } 
        if (valor.indexOf('-') != -1) {
            negativo = true;
        }
        valor = RemoverFormatoMoneda(valor);

        if (negativo)
        {
            valor = valor * -1;
        }
        var split = value.id.split('_');
        var tipo = split[1];

        if (Valores === "") {
            var ultimoVal = ReplaceAll($("#Cantidad_" + tipo).val(), ".", "");
            if (ultimoVal == "") {
                ultimoVal = "0";
            }
            Valores = tipo + "," + valor + "," + ultimoVal;
        } else {
            var ultimoValor = ReplaceAll($("#Cantidad_" + tipo).val(), ".", "");
            if (ultimoValor == "") {
                ultimoValor = "0";
            }
            Valores = Valores + "|" + tipo + "," + valor + "," + ultimoValor;
        }

    });
    requeridos = $("#frmArqueo").find(".ValorBrazalete");
    negativo = false;
    $.each(requeridos, function (index, value) {

        negativo = false;
        var valor = $(value).html().trim();
        if (valor.length <= 0) {
            valor = FormatoMoneda(0);
        }
        if (valor.indexOf('-') != -1) {
            negativo = true;
        }
        valor = RemoverFormatoMoneda(valor);
        
        if (negativo) {
            valor = valor * -1;
        }
        var split = value.id.split('_');
        var tipo = split[1];

        if (Valores === "") {
            var ultimoVal = ReplaceAll($("#CantidadBrazalete_" + tipo).val(), ".", "");
            if (ultimoVal == "") {
                ultimoVal = "0";
            }
            Valores = tipo + "," + valor + "," + ultimoVal;
        } else {
            var ultimoValor = ReplaceAll($("#CantidadBrazalete_" + tipo).val(), ".", "");
            if (ultimoValor == "") {
                ultimoValor = "0";
            }
            Valores = Valores + "|" + tipo + "," + valor + "," + ultimoValor + "," + $(value).data("codsap");
        }

    });

    EjecutarAjax(urlBase + "Arqueo/Insert", "GET", { TipoNovedad: Valores, IdTaquillero: $("#IdTaquillero").val(), Observaciones: ObservacionesTaq }, "successArqueo", null);

}


function Cancel() {
    MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "CancelarNuevo", "");
}

function CancelarNuevo() {
    window.location = urlBase + "Arqueo";
}

function EstablecerFormatoMoneda() {
    $(".formato_moneda").each(function (index, element) {
        element.innerText = FormatoMoneda(element.innerText);
    });
}


function successArqueo(rta) {
    if (rta.Correcto) {
        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "Arqueo", "success");
        //EjecutarAjax(urlBase + "Apertura/EditAperturaBase", "GET", null, "successfull", null);
        //MostrarMensaje("Importante", "Operación realizada con éxito.");
    }
    else {
        MostrarMensaje("Fallo al guardar", rta.Mensaje);
    }
}

function SucessChange(data) {

    $("#listView").html(data);
    EstablecerFormatoMoneda();
   
}

function Total(ctr) {

    var split = ctr.id.split('_');
    var id = split[1];
    var montoCaja = $(ctr).data("id");
    var ctrValor;
    if (ctr.value == "") {
        ctrValor = 0;
    } else {
        ctrValor = ctr.value.replace(/\./g, "");
    }
    var Valor = parseInt(ctrValor) - parseInt(montoCaja);
    if (Valor == 0) {
        $(ctr).parent().next().children().html("");
    } else {
        $(ctr).parent().next().children().html(($(ctr).hasClass("mon") ? FormatoMoneda(Valor) : Valor));
    }        
}


function Login(password, observaciones) {

    var _idUsuario = $("#IdTaquillero").val();
    ObservacionesTaq = observaciones;

    EjecutarAjax(urlBase + "Cuenta/ValidarPassword", "GET", {
        idUsuario: _idUsuario, password: password
    }, "respuestaLogin", null);

}

function CancelarLogin() {

        cerrarModal('modalCRUD');
}

function respuestaLogin(data) {
    if (data.Correcto) {

        GuardarAlistamiento();

        cerrarModal("modalCRUD");
    } else {
        MostrarMensaje("Mensaje", "Contraseña incorrecta");
    }
}