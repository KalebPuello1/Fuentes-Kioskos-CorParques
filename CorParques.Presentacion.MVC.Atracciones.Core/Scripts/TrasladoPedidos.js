﻿var ObservacionesTaq;
var listatraslados = [];

$(function () {

    $(".Numero:input[type=text]").mask("0000000000000", { reverse: true });

    $("#IdPuntoOrigen").select2({
        placeholder: "* Seleccione punto"
    });

    $("#IdPuntoDestino").select2({
        placeholder: "* Seleccione punto"
    });

    $("#IdPuntoOrigen").change(function () {

        EjecutarAjax(urlBase + "TrasladoPedidos/ObtenerMaterialesxPuntoAjax", "GET", { IdPunto: $(this).val() }, "SucessChange", null);

    });
    $("#IdUsuario").select2({
        placeholder: "* Seleccione usuario"
    });


    $("#btnSave").click(function () {

        //validacion de que taquilla origen y destino no sean iguales
        
        
        
        if ($("#IdPuntoOrigen").val() === $("#IdPuntoDestino").val()) {
            MostrarMensaje("Importante", "El punto de origen y destino no pueden ser iguales.", "error");
            return false;
        }

        var $tablaDatos = $("#tbTrasladoInventario");
        if ($tablaDatos.find("tbody").find("tr").length >= 1)
        {
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
        MostrarConfirm("Importante!", "¿Esta seguro de realizar el Inventario Físico?", "SolicitarLoginAjuste", "");

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
    InicializarMateriales()

});

function ObtenerIdSupervisor(id) {    
    GuardarInventario(id);
}

function InicializarMateriales() {
    
    $(".lnkDisable").click(function () {
        RealizarTraslado($(this).data("id"));
    });


    $("#txtSearchMateri").keyup(function () {
        $("#tbTrasladoInventario tbody tr.principal").hide();
        $.each($("#tbTrasladoInventario tbody tr.principal"), function (i, v) {
            $.each($(v).find("td.principal"), function (j, va) {
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

function RealizarTraslado(CodSapPedido) {

    if (!validarFormulario("frmTransladoInventario *")) {
        return false;
    }

    if ($("#IdPuntoOrigen").val() === $("#IdPuntoDestino").val()) {
        MostrarMensaje("Importante", "El punto de origen y destino no pueden ser iguales.", "error");
        return false;
    }

    var valido = false;
    listatraslados = [];
    $.each(pedidos, function (i, v) {
        if (v.CodigoVenta == CodSapPedido && v.IdProducto === 1) {
            valido = false;
            var nombre;
            $.each(materiales, function (j, m) {
                if (m.CodigoSap == v.CodigoSap) {
                    nombre = m.Nombre;
                    if (m.CantidadDisponible >= v.Cantidad)
                        valido = true;
                }
            })
            if (valido) {
                var objTrasladoInventario = new Object();
                objTrasladoInventario.CodSapMaterial = v.CodigoSap;
                objTrasladoInventario.IdPuntoOrigen = $("#IdPuntoOrigen").val();
                objTrasladoInventario.IdPuntoDestino = $("#IdPuntoDestino").val();
                objTrasladoInventario.idUsuario = $("#IdUsuario").val();
                objTrasladoInventario.Cantidad = v.Cantidad;
                objTrasladoInventario.UnidadMedida = v.NombreCliente;
                objTrasladoInventario.Pedido = CodSapPedido;
                listatraslados.push(objTrasladoInventario);
            } else {
                MostrarMensaje("Importante", "No cuenta con el inventario suficiente del material '" + v.Nombre + "' para realizar el traslado.", "error");
                return false;
            }
        }
    });
    if(valido)
        MostrarConfirm("Importante!", "¿Está seguro de realizar el traslado? ", "SolicitarLogin", "");
    
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
    EjecutarAjax(urlBase + "Cuenta/ObtenerLoginCormfirmacion", "GET", { Mensaje : MensajeConfirm }, "printPartialModal", { title: "Confirmación inventario", hidesave: true, modalLarge: false });
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
                item += '"' + $(data).attr("name") + '":"' + encodeURI($(data).val()) + '",';
        });
        item = item.substring(0, item.length - 1) + "},";
        _obj += item;
    });
    _obj = _obj.substring(0, _obj.length - 1) + "]";
    _obj = JSON.parse(_obj);


    EjecutarAjaxJson(urlBase + "Inventario/GuardarInventario", "POST", { modelo: _obj }, "SuccessInventarioFisico", null);

}

function GuardarAlistamiento() {
    EjecutarAjax(urlBase + "TrasladoPedidos/Guardar", "POST", JSON.stringify({ modelo: listatraslados }), "success", null);
  
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

    if(data)
        materiales = data.Elemento;
    
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

    var ValorDeferencia=0;
    if (ctr.value!="")
        ValorDeferencia = parseFloat(ctr.value.toString().replace(',', '.')) - parseFloat($(ctr).data("id").toString().replace(',', '.'));
    var contador = $(ctr).data("contador");
    $('#CodSapMotivo_' + contador).html("");
    var item = '<option value=' + "" + '>' + "Seleccione..." + '</option>';
    $('#CodSapMotivo_' + contador).append(item);
    if (ValorDeferencia > 0) {
        $("#CodSapMotivo_" + contador).addClass("required");
        $("#Observaciones_" + contador).addClass("required");
        $("#Movimiento_" + contador).html("Entrada por Ajuste");
        $("#CodSapAjuste_" + contador).val("C");
        if (listaMotivos != null) {
            var listitems;
            $.each(listaMotivos, function (key, value) {
                if (value.CodSapAjuste == "C")
                    {
                    listitems += '<option value=' + value.CodSapMotivo + '>' + value.Descripcion + '</option>';
                }
            });
        }
        $('#CodSapMotivo_' + contador).append(listitems);
    }
    if (ValorDeferencia < 0) {
        $("#CodSapMotivo_" + contador).addClass("required");
        $("#Observaciones_" + contador).addClass("required");
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
    if (ValorDeferencia.toString()!="")
        ValorDeferencia = parseFloat(ValorDeferencia.toString().replace(',', '.')).toFixed(2)

    $("#Diferencia_" + contador).html(ValorDeferencia.toString().replace('.', ','));
    
}