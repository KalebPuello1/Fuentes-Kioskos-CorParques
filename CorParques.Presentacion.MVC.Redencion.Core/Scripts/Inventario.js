﻿var ObservacionesTaq;
var listatraslados = [];
var arregloObsevaciones = "";
var arregloNombres = "";
var arregloDiferencia = "";
var arregloTeorico = "";
var arregloCamtidadDisponible = "";
var arregloTipoMovimiento = "";


$(function () {

    $(".Numero:input[type=text]").mask("0000000000000", { reverse: true });

    $("#IdPuntoOrigen").select2({
        placeholder: "* Seleccione punto"
    });

    $("#IdPuntoDestino").select2({
        placeholder: "* Seleccione punto"
    });

    $("#IdPuntoOrigen").change(function () {

        EjecutarAjax(urlBase + "Inventario/ObtenerMaterialesxPunto", "GET", { IdPunto: $(this).val() }, "SucessChange", null);

    });

    $("#IdUsuario").select2({
        placeholder: "* Seleccione usuario"
    });

    $("#btnCancel").click(function () {
        //var element = ObtenerObjeto("frmtest");
        Cancel();

    });

    $("#btnSave").click(function () {

        //validacion de que taquilla origen y destino no sean iguales

        $("#btnSave").attr("disabled", true)
        $("#btnSave").css("display", "none")

        if ($("#IdPuntoOrigen").val() === $("#IdPuntoDestino").val()) {
            MostrarMensaje("Importante", "El punto de origen y destino no pueden ser iguales.", "error");
            return false;
        }

        var $tablaDatos = $("#tbTrasladoInventario");
        if ($tablaDatos.find("tbody").find("tr").length >= 1) {
            $("#IdMateriales").removeClass("required");
            $("#CantidadMaterial").removeClass("required");
        }

        if (!validarFormulario("frmTransladoInventario *")) {
            return false;
        }

        if ($tablaDatos.find("tbody").find("tr").length == 0) {
            MostrarMensaje("", "No se han agregado materiales", "error");
            return false;
        }

        MostrarConfirm("Importante!", "¿Está seguro de realizar el traslado? ", "SolicitarLogin", "");

        //Colocar linea porque marlon esta bravo

        $("#IdMateriales").addClass("required");
        $("#CantidadMaterial").addClass("required");
    });

    $("#btnCancelInventario").click(function () {
        //var element = ObtenerObjeto("frmtest");
        CancelInventario();

    });

    function CancelInvFis() {
        MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "CancelarNuevo", "InventarioFisico");
    }

    $("#btnSaveInventario").click(function () {

        if (!validarFormulario("frmInventarioFisico *")) {
            return false;
        }

        var long = $($("tbody").children()).length 
       
       for (var i = 0; i < long; i++)
       {
           //Estas condicionales se deben corregir
           if ($($("tbody").children()[i]).children()[11].children[0].innerText != "0,00") {
               if (i == 0 && $($("tbody").children()[i]).children()[11].children[0].innerText != "0,00") {
                   arregloObsevaciones = "," +$("#" + $($("tbody").children()[i]).children()[14].children[0].id).val();
                   arregloNombres = "," + $($("tbody").children()[i]).children()[7].innerText;
                   arregloDiferencia = ";" + $($("tbody").children()[i]).children()[11].children[0].innerText
                   arregloTeorico = "," + $($("tbody").children()[i]).children()[8].innerText
                   arregloCamtidadDisponible = "," + $("#" + $($("tbody").children()[i]).children()[10].children[0].id).val()
                   arregloTipoMovimiento = "," + $($("tbody").children()[i]).children()[12].children[0].innerText
               }
               else {
                   //Estas condicionales se deben asignar
                   if (i != 0 && $($("tbody").children()[i]).children()[11].children[0].innerText != "0,00") {
                       arregloObsevaciones += "," + $("#" + $($("tbody").children()[i]).children()[14].children[0].id).val();
                       arregloNombres += "," + $($("tbody").children()[i]).children()[7].innerText;
                       arregloDiferencia += ";" + $($("tbody").children()[i]).children()[11].children[0].innerText
                       arregloTeorico += "," + $($("tbody").children()[i]).children()[8].innerText
                       arregloCamtidadDisponible += "," + $("#" + $($("tbody").children()[i]).children()[10].children[0].id).val()
                       arregloTipoMovimiento += "," + $($("tbody").children()[i]).children()[12].children[0].innerText
                   }
               }
           }
        }
        MostrarConfirm("Importante!", "¿Esta seguro de realizar el Inventario Físico?", "SolicitarLoginAjuste", "");
         
            
        //Aqui va a venir el methodo para el correo y el ajax 
        
    });

    $("#txtSearch").keyup(function () {
        $("#tbInventarioFisico tbody tr").hide();
        $.each($("#tbInventarioFisico tbody tr"), function (i, v) {
            $.each($(v).find("td"), function (j, va) {
                if ($(va).children().length == 0) {
                    if ($(va).html().toLowerCase().indexOf($("#txtSearch").val().toLowerCase()) >= 0) {
                        $(v).show();
                        return false;
                    }
                }
            });
        });
    });

    $(".decimal").keypress(function () {
        return validateFloatKeyPress(this, event);
    });


});

function ObtenerIdSupervisor(id) {    
    GuardarInventario(id);
}

function InicializarMateriales() {

    $(".lnkDisable").click(function () {
        RemoverMaterial($(this).data("id"));
    });

    $("#IdMateriales").select2({
        placeholder: "* Seleccione material"
    });

    $("#btnAgregar").click(function () {
        if (!validarFormulario("frmTransladoInventario *")) {
            return false;
        }
        AdicionarMaterial();
    });

    $("#txtSearchMateri").keyup(function () {
        $("#tbTrasladoInventario tbody tr").hide();
        $.each($("#tbTrasladoInventario tbody tr"), function (i, v) {
            $.each($(v).find("td"), function (j, va) {
                if ($(va).children().length == 0) {
                    if ($(va).html().toLowerCase().indexOf($("#txtSearchMateri").val().toLowerCase()) >= 0) {
                        $(v).show();
                        return false;
                    }
                }
            });
        });
    });


}

function RemoverMaterial(CodSapMaterial) {

    debugger;
    split = $("#ListMateriales").val().split(';');
    ListaMaterialesRemove = ""
    var Adicionar = true;

    if (split.length > 0) {
        for (var I = 0; I <= (split.length - 1); I++) {
            splitCount = split[I].split('|');
            if (splitCount.length > 0) {
                if (CodSapMaterial != splitCount[0]) {
                    ListaMaterialesRemove = ListaMaterialesRemove + split[I] + ";";
                }
            }
        }
    }

    if (Adicionar) {
        var _obj = ObtenerObjeto("frmTransladoInventario *")
        EjecutarAjax(urlBase + "Inventario/AdicionarMaterial", "GET", { modelo: _obj, IdPunto: $("#IdPuntoOrigen").val(), ListMateriales: ListaMaterialesRemove }, "SucessAdd", null);
        $("#ListMateriales").val(ListaMaterialesRemove);
    }
}


function AdicionarMaterial() {


    CantidadDisponible = $("#IdMateriales option:selected").data("id");
    split = $("#ListMateriales").val().split(';');
    ListaMateriales = $("#ListMateriales").val();
    var MaterialAdd = $("#IdMateriales").val() + "|" + $("#CantidadMaterial").val();

    if (parseFloat(CantidadDisponible.toString().replace(',', '.')) < parseFloat($("#CantidadMaterial").val().toString().replace(',', '.'))) {
        MostrarMensaje("", "La cantidad es superior a la cantidad disponible. Cantidad disponible es " + CantidadDisponible + ".", "error");
        $("#CantidadMaterial").val("");
        return;
    }

    var Adicionar = true;

    //if (split.length > 0) {
    //    for (var I = 0; I <= (split.length - 1) ; I++) {
    //        splitCount = split[I].split('|');
    //        if (splitCount.length > 0) {
    //            if ($("#IdMateriales").val() == splitCount[0]) {
    //                Adicionar = false;
    //                MostrarMensaje("", "El material ya fue agregado.", "error");
    //                return;
    //            }
    //        }
    //    }
    //}

    if (listatraslados.length > 0) {
        $.each(listatraslados, function (i, item) {
            if (item.CodigoSap == $("#IdMateriales").val()) {
                Adicionar = false;
                MostrarMensaje("", "El material ya fue agregado.", "error");
                return;
            }
        });
    }


    if (Adicionar) {
        var _obj = ObtenerObjeto("frmTransladoInventario *")
        //ListaMateriales = ListaMateriales + MaterialAdd + ";";
        //EjecutarAjax(urlBase + "Inventario/AdicionarMaterial", "GET", { modelo: _obj, IdPunto: $("#IdPuntoOrigen").val(), ListMateriales: ListaMateriales }, "SucessAdd", null);
        //$("#ListMateriales").val(ListaMateriales);
        var objTrasladoInventario = new Object();
        objTrasladoInventario.CodSapMaterial = $("#IdMateriales option:selected").data("codsap");
        objTrasladoInventario.Nombre = $("#IdMateriales option:selected").data("nombre");
        objTrasladoInventario.UnidadMedida = $("#IdMateriales option:selected").data("unidad");
        objTrasladoInventario.CantidadDisponible = $("#IdMateriales option:selected").data("id");
        objTrasladoInventario.Cantidad = $("#CantidadMaterial").val();
        listatraslados.push(objTrasladoInventario);
        loadArrayToTable();


    }
}

function loadArrayToTable() {
    var sHtml = '';
    $("#bodyTableTrasladoInventario").html(sHtml);

    if (listatraslados.length > 0) {
        $.each(listatraslados, function (i, item) {
            sHtml += '<tr>';
            sHtml += '<td>' + item.CodSapMaterial + '</td>';
            sHtml += '<td>' + item.Nombre + '</td>';
            sHtml += '<td>' + item.CantidadDisponible + '</td>';
            sHtml += '<td>' + item.UnidadMedida + '</td>';
            sHtml += '<td>' + item.Cantidad + '</td>';
            sHtml += '<td style="text-align:center; width:50px"> <a class="lnkDelete" onclick="evtEliminarElementoPorIndice(' + (i) + ');" href="javascript:void(0)" title="Eliminar"><b class="fa fa-trash-o"></b></a>  </td>';
            sHtml += '</tr>';
        });
    }
    $("#bodyTableTrasladoInventario").append(sHtml);

    limpiarControlesAgregar();
}

function limpiarControlesAgregar() {

    var $lt1 = $("#IdMateriales").select2();
    $lt1.val('').trigger("change");
    $("#IdMateriales").select2({
        placeholder: "* Seleccione material"
    });

    $("#CantidadMaterial").val("");

}


function evtEliminarElementoPorIndice(indexElemento) {

    if (listatraslados.length > 0) {
        $.each(listatraslados, function (i, item) {
            if (i == indexElemento) {
                listatraslados.splice(i, 1);
            }
        });
    }
    loadArrayToTable();
}


function SolicitarLoginAjuste() {

    EjecutarAjax(urlBase + "Cuenta/ObtenerLoginSupervisor", "GET", null, "printPartialModal", {
        title: "Confirmación supervisor", hidesave: true, modalLarge: false, OcultarCierre: true
    });

}

function SolicitarLogin()
{
    MensajeConfirm = "¿Está de acuerdo con el inventario que está recibiendo?";
    EjecutarAjax(urlBase + "Cuenta/ObtenerLoginCormfirmacion", "GET", { Mensaje: MensajeConfirm }, "printPartialModal", { title: "Confirmación inventario", hidesave: true, modalLarge: false });
}

function GuardarInventario(id) {
    
    $(".IdSupervisor").each(function (index, element) {
        element.value = id;
    });
    var _obj="[";
    $.each($("#frmInventarioFisico #tbInventarioFisico tbody tr"), function (i, v) {
        var item="{";
        $.each($(v).find("input"), function (j, data) {
            if ($(data).attr("name")!==undefined)
                item += '"' + $(data).attr("name") + '":"' + $(data).val().toString().replace('.',',') + '",';
        });
        $.each($(v).find("select"), function (j, data) {
            if ($(data).attr("name") !== undefined)
                item += '"' + $(data).attr("name") + '":"' + $(data).val() + '",';
        });
        $.each($(v).find("textarea"), function (j, data) {
            if ($(data).attr("name") !== undefined)
                item += '"' + $(data).attr("name") + '":"' + encodeURI($(data).val() == "" ? "Ajuste por conteo" : $(data).val()) + '",';
        });
        item = item.substring(0, item.length - 1) + "},";
        _obj += item;
    });
    _obj = _obj.substring(0, _obj.length - 1) + "]";
    _obj = JSON.parse(_obj);

    EjecutarAjaxJson(urlBase + "Inventario/GuardarInventario", "POST", { modelo: _obj }, "SuccessInventarioFisico", null);

}

function GuardarAlistamiento() {


    //$(".PuntoOrigen").each(function (index, element) {
    //    element.value = $("#IdPuntoOrigen").val();
    //});

    //$(".PuntoDestino").each(function (index, element) {
    //    element.value = $("#IdPuntoDestino").val();
    //});

    //$(".idUsuario").each(function (index, element) {
    //    element.value = $("#IdUsuario").val();
    //});

    //$(".cantidad").each(function (index, element) {
    //    element.value = element.value.toString().replace(',', '.');
    //});

    if (listatraslados.length > 0) {
        $.each(listatraslados, function (i, item) {
            item.IdPuntoOrigen = $("#IdPuntoOrigen").val();
            item.IdPuntoDestino = $("#IdPuntoDestino").val();
            item.idUsuario = $("#IdUsuario").val();
            item.Cantidad = item.Cantidad.toString().replace('.', ',');
        });

    }
    //var _obj = ObtenerObjeto("frmTransladoInventario *")
    //var _obj = JSON.parse(listatraslados);
    //EjecutarAjax(urlBase + "Inventario/Guardar", "GET", listatraslados, "success", null);
    EjecutarAjax(urlBase + "Inventario/Guardar", "POST", JSON.stringify({ modelo: listatraslados }), "success", null);

}


function Cancel() {
    MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "CancelarNuevo", "");
}

function CancelInventario() {
    MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "CancelarNuevoInventario", "");
}

function CancelarNuevoInventario() {

    //EjecutarAjax(urlBase + "Inventario/PruebaInventario", "GET", null, "SucessChange", null);
    window.location = urlBase + "Inventario/inventarioFisico";
}

function CancelarNuevo() {

    //EjecutarAjax(urlBase + "Inventario/PruebaInventario", "GET", null, "SucessChange", null);
    window.location = urlBase + "Inventario";
}

function EstablecerFormatoMoneda() {
    $(".formato_moneda").each(function (index, element) {
        element.innerText = FormatoMoneda(element.innerText);
    });
}


function success(rta) {
    if (rta.Correcto) {
        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "Inventario", "success");
        //EjecutarAjax(urlBase + "Apertura/EditAperturaBase", "GET", null, "successfull", null);
        //MostrarMensaje("Importante", "Operación realizada con éxito.");
    }
    else {
        MostrarMensaje("Fallo al guardar", rta.Mensaje);
    }
}

function SuccessInventarioFisico(rta) {
    if (rta.Correcto) {
        if (arregloObsevaciones != "" && arregloNombres != "" && arregloDiferencia != "") {
            $.ajax({
                method: 'GET',
                url: urlBase + 'Inventario/CreaYenviaMail',
                async: false,
                data: { Observaciones: arregloObsevaciones, Nombres: arregloNombres, Diferencia: arregloDiferencia, ArregloTeorico: arregloTeorico, ArregloCamtidadDisponible: arregloCamtidadDisponible, ArregloTipoMovimiento: arregloTipoMovimiento },
                success: (e) => {
                    arregloObsevaciones = "";
                    arregloNombres = "";
                    arregloDiferencia = "";
                    arregloTeorico = "";
                    arregloCamtidadDisponible = "";
                    arregloTipoMovimiento = "";
                    console.log(e)
                },
                error: (e) => {
                    arregloObsevaciones = "";
                    arregloNombres = "";
                    arregloDiferencia = "";
                    arregloTeorico = "";
                    arregloCamtidadDisponible = "";
                    arregloTipoMovimiento = "";
                    console.log(e)
                }
            })
        }

        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "Home", "success");
    }
    else {

        MostrarMensaje("Fallo al guardar", rta.Mensaje);

    }
}

function SucessAdd(data) {

    $("#listView").html(data);
    InicializarMateriales();

}

function SucessChange(data) {

    $("#listView").html(data);
    InicializarMateriales();
    $("#ListMateriales").val("");

}

function Login(password) {

    var _idUsuario = $("#IdUsuario").val();

    EjecutarAjax(urlBase + "Cuenta/ValidarPassword", "GET", {
        idUsuario: _idUsuario, password: password
    }, "respuestaLogin", null);

}


function respuestaLogin(data) {
    if (data.Correcto) {

        GuardarAlistamiento();

        cerrarModal("modalCRUD");
    } else {
        MostrarMensaje("Contraseña incorrecta", "");
        $("#txtPassword").val("");
    }
}

function CancelarLogin() {

    cerrarModal('modalCRUD');
}

function Diferencia(ctr) {

    var ValorDeferencia = 0;
    if (ctr.value != "")
        ValorDeferencia = parseFloat(ctr.value.toString().replace(',', '.')) - parseFloat($(ctr).data("id").toString().replace(',', '.'));

    var contador = $(ctr).data("contador");
    $('#CodSapMotivo_' + contador).html("");
    var item = '<option value=' + "" + '>' + "Seleccione..." + '</option>';
    $('#CodSapMotivo_' + contador).append(item);
    if (ValorDeferencia > 0) {
        $("#CodSapMotivo_" + contador).addClass("required");
        /*$("#Observaciones_" + contador).addClass("required");*/
        $("#Observaciones_" + contador).removeClass("required");
        $("#Movimiento_" + contador).html("Entrada por Ajuste");
        $("#CodSapAjuste_" + contador).val("C");
        if (listaMotivos != null) {
            var listitems;
            $.each(listaMotivos, function (key, value) {
                if (value.CodSapAjuste == "C") {
                    listitems += '<option value=' + value.CodSapMotivo + '>' + value.Descripcion + '</option>';
                }
            });
        }
        $('#CodSapMotivo_' + contador).append(listitems);
    }
    if (ValorDeferencia < 0) {
        $("#CodSapMotivo_" + contador).addClass("required");
        /*$("#Observaciones_" + contador).addClass("required");*/
        $("#Observaciones_" + contador).removeClass("required");
        $("#Movimiento_" + contador).html("Salida por Ajuste");
        $("#CodSapAjuste_" + contador).val("B");
        if (listaMotivos != null) {
            var listitems;
            $.each(listaMotivos, function (key, value) {
                if (value.CodSapAjuste == "B") {
                    listitems += '<option value=' + value.CodSapMotivo + '>' + value.Descripcion + '</option>';
                }
            });
        }
        $('#CodSapMotivo_' + contador).append(listitems);
    }
    if (ValorDeferencia == 0) {
        $("#CodSapMotivo_" + contador).removeClass("required");
        $("#Observaciones_" + contador).removeClass("required");

        $("#Movimiento_" + contador).html("0");
        $("#CodSapAjuste_" + contador).val("0");
    }

    if (ValorDeferencia >= 0) {
        $("#Cantidad_" + contador).val(ValorDeferencia);
    } else {
        $("#Cantidad_" + contador).val(ValorDeferencia * (-1));
    }

    var Valor = ValorDeferencia * $("#Costohidden_" + contador).val();

    if (Valor.toString() != "")
        Valor = parseFloat(Valor.toString().replace(',', '.')).toFixed(2).toString().replace('.', ',')

    $("#Costo_" + contador).html(Valor);
    if (ValorDeferencia.toString() != "")
        ValorDeferencia = parseFloat(ValorDeferencia.toString().replace(',', '.')).toFixed(2)

    $("#Diferencia_" + contador).html(ValorDeferencia.toString().replace('.', ','));

}
