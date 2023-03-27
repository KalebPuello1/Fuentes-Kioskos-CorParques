var inicializadointerval = false;
var lista = [];

$(function () {
    $(".Numerico").mask("000000000000");

    $('#CodBarraProducto').keyup(function (e) {


        var letters = /^[A-Za-z0-9]+$/;
        if (!e.key.match(letters)) {
            this.value = this.value.replace(e.key,'') + String.fromCharCode(45)
            return false;
        }
        if (!inicializadointerval) {
            inicializadointerval = true;
            var refreshIntervalId = setInterval(function () { ConsultarCodBarra(); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);

        }
    });
})

$("#btnCancelar").click(function () {
    MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "Cancelar", "");
});

function Cancelar() {
    window.location = urlBase + "DescargueMaterial";
}


function ConsultarCodBarra() {
    var obj = $("#CodBarraProducto");
    if (obj.val().length > 0) {
        ValidarBoleta(obj.val());
    }
}

function ValidarBoleta(cod) {
    EjecutarAjax(urlBase + "DescargueMaterial/ObtenerProductos", "GET", { CodBarra: cod }, "successProd", null);
}

function successProd(rta) {
    if (rta.length > 0) {
        $("#listproductos").html(rta);
        $("#btnDescargar").prop('disabled', false);
    } else {
        $("#btnDescargar").prop('disabled', true);
    }
}

$("#btnDescargar").click(function () {
    AceptarDescargoProductos();
})

function SetEventDescargueProducto() {
    //ValidarCantidad

    $(".Cantidad").keyup(function () {

        var id = $(this).data("id");

        if ($.trim($(this).val()).length > 0 && $("#chx_" + id).is(":checked")) {
            $(this).attr("data-mensajeerror", "");
            $(this).removeClass("errorValidate");
            QuitarTooltip();

            var CantidadMaxima = $(this).data("max");
            var cantidad = parseInt($.trim($(this).val()));
            if (cantidad == 0) {
                $(this).val(CantidadMaxima);
            } else {
                if (cantidad > CantidadMaxima) {
                    $(this).attr("data-mensajeerror", "La cantidad no puede ser superior a " + CantidadMaxima);
                    $(this).addClass("errorValidate");
                    mostrarTooltip();
                }
            }
        }

    });

    //Evento check Descargue producto
    $(".ChxDescarga").click(function () {

        var id = $(this).data("id");
        var obj = $("#txt_" + id);
        var CantidadMaxima = $(obj).data("max");
        var cantidad = parseInt($.trim($(obj).val()));

        if ($(this).is(':checked')) {

            QuitarTooltip();
            $(obj).removeClass("errorValidate");

            if (cantidad == 0) {
                $(obj).val(CantidadMaxima);
            } else {
                if (cantidad > CantidadMaxima) {
                    $(obj).attr("data-mensajeerror", "La cantidad no puede ser superior a " + CantidadMaxima);
                    $(obj).addClass("errorValidate");
                    mostrarTooltip();
                }
            }
        } else {
            $(obj).val(CantidadMaxima);
            QuitarTooltip();
            $(obj).removeClass("errorValidate");
        }
    });

}


// Validar campos descarga producto
function ValidarCampos() {

    var result = true;
    $.each($(".ChxDescarga"), function (i, item) {
        if ($(item).is(":checked")) {
            var id = $(item).data("id");
            if ($("#txt_" + id).hasClass('errorValidate')) {
                result = false
            } else {
                if ($("#txt_" + id).val().length == 0) {
                    $("#txt_" + id).attr("data-mensajeerror", "Campo obligatorio");
                    $("#txt_" + id).addClass("errorValidate");
                    result = false;
                }
            }
        }
    });

    return result;
}


//Método que invoca el evento aceptar del formulario descargue productos
function AceptarDescargoProductos() {

    if ($(".ChxDescarga:checked").length == 0) {
        MostrarMensaje("Importante", "Mínimo debe seleccionar un producto para la descarga", "error");
        return;
    }

    if (!ValidarCampos()) {
        MostrarMensaje("Importante", "Hay inconsistencias en el formulario, revise los campos demarcados con color rojo.", "error");

    } else {
        $.each($(".ChxDescarga"), function (i, item) {

            if ($(item).is(':checked')) {
                var id = $(item).data("id");
                var cantidad = $.trim($("#txt_" + id).val());
                setDescargueProducto(cantidad, id);
            }
        });
        ConfirmarPago();
    }
}


function setDescargueProducto(cantidad, idProducto) {

    var count = 0;
    var idprod = parseInt(idProducto);
    var Valor = cantidad.length > 0 ? parseInt(cantidad) : 0;

    $.each(lista, function (i, item) {

        if (item.IdProducto == idprod) {
            if (count < Valor)
                item.Entregado = true;
            else
                item.Entregado = false;
            count++;
        }
    });

}

function ConfirmarPago() {
    debugger;
    EjecutarAjaxJson(urlBase + "DescargueMaterial/Descargue", "POST", { Productos: lista }, "SuccessDescargue", null);
}

function SuccessDescargue(rta) {
    if (rta.Correcto) {
        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "DescargueMaterial", "success");
    } else {
        MostrarMensaje("", rta.Mensaje);
    }
}
