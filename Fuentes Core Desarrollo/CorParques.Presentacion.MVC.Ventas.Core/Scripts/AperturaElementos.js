var listaElementos = [];
var listaElementosConsultados = [];

$(function () {
    /**Funciones de la vista PARCIAL**/
    loadCalendar();

    $("#lnkAddElemento").click(function () {
        limpiarControlesAdmElementos();
        abrirModal("modalAgergarElemento");
    });

    $("#btnCancelAgergarElemento").click(function () {
        limpiarControlesAdmElementos();
        cerrarModal('modalAgergarElemento');
    });

    $("#selectPuntoAgergarElemento").change(function () {
        CargarDatosAperturaElementos();
        $("#selectPuntoAgergarElemento").removeClass("errorValidate");
        $("#selectPuntoAgergarElemento").attr("data-mensajeerror", "");

        if ($("#selectPuntoAgergarElemento").val() == "") {
            $("#lnkAddElemento").hide();
        } else {
            $("#lnkAddElemento").show();
        }
    });


    $("#fechaAperturaElementos").on('dp.change', function (e) {
        $("#fechaAperturaElementos").removeClass("errorValidate");
        $("#fechaAperturaElementos").attr("data-mensajeerror", "");
        listaElementos = [];
        listaElementosConsultados = [];
        listaElementosToTable();

        EjecutarAjax(urlBase + "Apertura/PuntosxFechaConApertura", "GET", { Fecha: $(this).val() }, "successPuntosxFechaConApertura", null);
    });

    $("#selectTipoElementoAgergarElemento").change(function () {
        switch ($("#selectTipoElementoAgergarElemento").val()) {
            case "":
            case "300":
            case "301":
                $("#inputCodigoBarrasAgergarElemento").addClass("required");
                $("#ateriskCodigoBarras").show();
                break;
            default:
                $("#inputCodigoBarrasAgergarElemento").removeClass("required");
                $("#ateriskCodigoBarras").hide();
        }
    });

    $("#btnSaveAgergarElemento").click(function () {
        if (validarFormulario("formAgergarElemento *")) {
            var existeCodigoBarras = false;
            if (listaElementos.length > 0) {
                $.each(listaElementos, function (i, item) {
                    switch (item.IdElemento) {
                        case "":
                        case 300:
                        case 301:
                            if ((item.CodigoBarras === null ? "" : item.CodigoBarras) == $("#inputCodigoBarrasAgergarElemento").val()) {
                                existeCodigoBarras = true;
                            }
                            break;
                        default:
                    }
                });
            }

            if (listaElementosConsultados.length > 0) {
                $.each(listaElementosConsultados, function (i, item) {
                    switch (item.IdElemento) {
                        case "":
                        case 300:
                        case 301:
                            if ((item.CodigoBarras === null ? "" : item.CodigoBarras) == $("#inputCodigoBarrasAgergarElemento").val()) {
                                existeCodigoBarras = true;
                            }
                            break;
                        default:
                    }
                });
            }

            if (existeCodigoBarras) {
                $("#inputCodigoBarrasAgergarElemento").attr("data-mensajeerror", "El código de barras ya fue registrado.");
                $("#inputCodigoBarrasAgergarElemento").addClass("errorValidate");

                mostrarTooltip();
                MostrarMensaje("Importante", "Hay inconsistencias en el formulario, revise los campos demarcados con color rojo.", "error");
            } else {
                var objElemento = new Object();
                objElemento.Id = -1;
                objElemento.IdElemento = parseInt($("#selectTipoElementoAgergarElemento").val());
                objElemento.Elemento = new Object();
                objElemento.Elemento.Id = objElemento.IdElemento;
                objElemento.Elemento.Nombre = $("#selectTipoElementoAgergarElemento option:selected").text();
                objElemento.CodigoBarras = $("#inputCodigoBarrasAgergarElemento").val();
                objElemento.Observacion = $("#inputObservacionAgergarElemento").val();

                listaElementos.push(objElemento);
                listaElementosToTable();
                cerrarModal('modalAgergarElemento');
                limpiarControlesAdmElementos();
            }
        } else {
            return false;
        }
    });

    /**Funciones de la vista principal*********/
    $("#btnEnviarAperturaElemento").click(function () {
        
        var isValidForm = true;
        $("#selectPuntoAgergarElemento").removeClass("errorValidate");
        $("#selectPuntoAgergarElemento").attr("data-mensajeerror", "");

        $("#tableAgregarElementos").removeClass("errorValidate");
        $("#tableAgregarElementos").attr("data-mensajeerror", "");

        if ($("#selectPuntoAgergarElemento").val() == "" || $("#selectPuntoAgergarElemento").val() == "0") {
            $("#selectPuntoAgergarElemento").attr("data-mensajeerror", "Este campo es obligatorio.");
            $("#selectPuntoAgergarElemento").addClass("errorValidate");
            isValidForm = false;
        }

        if (listaElementos.length == 0) {
            $("#tableAgregarElementos").attr("data-mensajeerror", "Debe agregar al menos un elemento.");
            $("#tableAgregarElementos").addClass("errorValidate");
            isValidForm = false;
        }

        if ($("#fechaAperturaElementos").val() == "" || $("#fechaAperturaElementos").val() == "0") {
            $("#fechaAperturaElementos").attr("data-mensajeerror", "Este campo es obligatorio.");
            $("#fechaAperturaElementos").addClass("errorValidate");
            isValidForm = false;
        }

        if (isValidForm) {
            MostrarConfirm("Importante!", "¿Está seguro de guardar el alistamiento?", "ConfirmEnviarApertura", null);
            
        } else {
            mostrarTooltip();
            MostrarMensaje("Importante", "Hay inconsistencias en el formulario, revise los campos demarcados con color rojo.", "error");
            return false;
        }
    });

    $("#btnCancelarAperturaElemento").click(function () {
        MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación?", "ConfirmCancelarAperturaElementos", null);
    });
});

function ConfirmCancelarAperturaElementos()
{
    $("#selectPuntoAgergarElemento").removeClass("errorValidate");
    $("#selectPuntoAgergarElemento").attr("data-mensajeerror", "");

    $("#tableAgregarElementos").removeClass("errorValidate");
    $("#tableAgregarElementos").attr("data-mensajeerror", "");

    $("#selectPuntoAgergarElemento").val("");

    listaElementosConsultados = [];
    listaElementos = [];
    listaElementosToTable();

    if (urlBase == "/")
        window.location = "/";
    else
        window.location = urlBase + "/";
}

function ConfirmEnviarApertura()
{
    EjecutarAjax(urlBase + "Apertura/InsertElementos", "POST", JSON.stringify({ Elementos: listaElementos, IdPunto: $("#selectPuntoAgergarElemento").val(), Fecha: $("#fechaAperturaElementos").val() }), "SuccessbtnEnviarAperturaElemento", null);
}

function successPuntosxFechaConApertura(rta) {
    
    $("#selectPuntoAgergarElemento").html("");
    $("#selectPuntoAgergarElemento").append("<option value>Seleccione...</option>");
    if (rta.length > 0) {
        $.each(rta, function (i, item) {
            $("#selectPuntoAgergarElemento").append("<option value='" + item.Id + "'>" + item.Nombre + "</option>");
        });
    }
}

function CargarDatosAperturaElementos() {
    
    if ($("#selectPuntoAgergarElemento").val() != "" && $("#fechaAperturaElementos").val() != "") {
        EjecutarAjax(urlBase + "Apertura/ElementosPorIdPunto", "POST", JSON.stringify({ IdPunto: $("#selectPuntoAgergarElemento").val(), Fecha: $("#fechaAperturaElementos").val() }), "SuccessObtenerElementosPorIdPunto", null);
    } else {
        listaElementos = [];
        listaElementosConsultados = [];
        listaElementosToTable();
    }
}

function SuccessObtenerElementosPorIdPunto(data) {
    
    if (data.Correcto) {
        listaElementos = [];
        listaElementosConsultados = data.Elemento;
        listaElementosToTable();
    }
    else {

    }
}

function SuccessbtnEnviarAperturaElemento(data) {
    
    if (data.Correcto) {
        /*listaElementos = [];
        listaElementosConsultados = [];
        CargarDatosAperturaElementos();*/
        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", null, "success");

        $("#selectPuntoAgergarElemento").val("");

        listaElementosConsultados = [];
        listaElementos = [];
        listaElementosToTable();
    }
    else {

    }
}

function limpiarControlesAdmElementos() {
    $("#selectTipoElementoAgergarElemento").val('');
    $("#inputCodigoBarrasAgergarElemento").val('');
    $("#inputObservacionAgergarElemento").val('');

    $("#selectTipoElementoAgergarElemento").attr("data-mensajeerror", "");
    $("#selectTipoElementoAgergarElemento").removeClass("errorValidate");

    $("#inputCodigoBarrasAgergarElemento").attr("data-mensajeerror", "");
    $("#inputCodigoBarrasAgergarElemento").removeClass("errorValidate");

    $("#inputObservacionAgergarElemento").attr("data-mensajeerror", "");
    $("#inputObservacionAgergarElemento").removeClass("errorValidate");

    $("#tableAgregarElementos").removeClass("errorValidate");
    $("#tableAgregarElementos").attr("data-mensajeerror", "");
}

function listaElementosToTable() {
    var sHtml = '';
    limpiarControlesAdmElementos();
    $("#bodyElementosApertura").html(sHtml);

    if (listaElementosConsultados.length > 0) {
        $.each(listaElementosConsultados, function (i, item) {
            sHtml += '<tr id="' + item.Id + '">';
            sHtml += '<td>' + item.Elemento.Nombre + '</td>';
            sHtml += '<td>' + (item.CodigoBarras === null ? "" : item.CodigoBarras) + '</td>';
            sHtml += '<td>' + item.Observacion + '</td>';
            sHtml += '<td style="text-align:center; width:50px"> <a class="lnkDelete" onclick="evtEliminarElementoPorId(' + (item.Id) + ');" href="javascript:void(0)" title="Eliminar"><b class="fa fa-trash-o"></b></a>  </td>';
            sHtml += '</tr>';
        });
    }

    if (listaElementos.length > 0) {
        $.each(listaElementos, function (i, item) {
            if (item.Id == -1) {
                sHtml += '<tr id="' + item.Id + '">';
                sHtml += '<td>' + item.Elemento.Nombre + '</td>';
                sHtml += '<td>' + item.CodigoBarras + '</td>';
                sHtml += '<td>' + item.Observacion + '</td>';
                sHtml += '<td style="text-align:center; width:50px"> <a class="lnkDelete" onclick="evtEliminarElemento(' + (i) + ');" href="javascript:void(0)" title="Eliminar"><b class="fa fa-trash-o"></b></a>  </td>';
                sHtml += '</tr>';
            }
        });
    }
    $("#bodyElementosApertura").append(sHtml);
}


function evtEliminarElemento(indexElemento) {
    MostrarConfirm("Importante!", "¿Está seguro que desea eliminar este registro?", "FinalEliminarElementoPorIndex", indexElemento);
}

function FinalEliminarElementoPorIndex(indexElemento)
{
    if (listaElementos.length == 1) {
        listaElementos = [];
    } else {
        $.each(listaElementos, function (i) {
            if (i == indexElemento) {
                listaElementos.splice(i, 1);
                return false;
            }
        });
    }
    listaElementosToTable();
}

function evtEliminarElementoPorId(IdAperturaElemento) {
    
    MostrarConfirm("Importante!", "¿Está seguro que desea eliminar este registro?", "FinalEliminarElementoPorId", IdAperturaElemento);
}

function FinalEliminarElementoPorId(IdAperturaElemento) {
    
    var objElemento = new Object();
    objElemento.Id = IdAperturaElemento;

    //EjecutarAjax(urlBase + "Apertura/EliminarElementoPorIdAperturaElemento", "POST", JSON.stringify({ objModel: objElemento }), "SuccesEliminarElementoPorId", null);
    $.each(listaElementosConsultados, function (i) {
        if (listaElementosConsultados[i].Id == IdAperturaElemento) {
            listaElementos.push(listaElementosConsultados[i]);
            listaElementosConsultados.splice(i, 1);
            return false;
        }
    });

    listaElementosToTable();
}

function SuccesEliminarElementoPorId(data) {
    
    if (data.Correcto) {
        CargarDatosAperturaElementos();
        /*mostrarAlerta('Elemento eliminado con exito.');*/
    } else {
        //mostrarAlerta('Estado', data.Mensaje);
        MostrarMensaje("", data.Mensaje, "error");
    }
}