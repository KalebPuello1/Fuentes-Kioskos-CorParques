var listaAjustes = [];

$("#CodSapMaterial").select2({
    placeholder: "* Seleccione el material"
});

$("#IdPunto").select2({
    placeholder: "* eleccione el punto"
});

$("#CodSapTipoAjuste").select2({
    placeholder: "* Seleccione tipo de ajuste"
});

$("#CodSapMotivo").select2({
    placeholder: "* Seleccione motivo"
});

$(".decimal").keypress(function () {
    return validateFloatKeyPress(this, event);
});

$("#Cantidad").val("");

$("#btnAgregarAjusteInventario").click(function () {
    if (validarFormulario("formAjusteInvntario")) {
        var objAjusteInventario = new Object();
        objAjusteInventario.Id = 0;
        objAjusteInventario.CodSapMaterial = $("#CodSapMaterial").val();
        objAjusteInventario.Material = new Object();
        objAjusteInventario.Material.CodigoSap = $("#CodSapMaterial").val();
        objAjusteInventario.Material.Nombre = $("#CodSapMaterial option:selected").text()
        objAjusteInventario.Cantidad = parseFloat(ReplaceAll($("#Cantidad").val(), ',', '.'));
        objAjusteInventario.CodSapAlmacen = "";
        objAjusteInventario.IdPunto = parseInt($("#IdPunto").val());
        objAjusteInventario.CodSapTipoAjuste = $("#CodSapTipoAjuste").val();
        objAjusteInventario.DescSapTipoAjuste = $("#CodSapTipoAjuste option:selected").text();
        objAjusteInventario.CodSapMotivo = $("#CodSapMotivo").val();
        objAjusteInventario.DescSapMotivo = $("#CodSapMotivo option:selected").text();
        objAjusteInventario.Observaciones = $("#Observaciones").val();
        objAjusteInventario.Unidad = $("#CodSapMaterial option:selected").data("unidad");
        objAjusteInventario.CostoPromedio = $("#CodSapMaterial option:selected").data("costopromedio");
        listaAjustes.push(objAjusteInventario);
        loadArrayToTable();
    }
    //mostrarTooltip();
});

$("#btnAceptarAjusteInventario").click(function () {
    if (listaAjustes.length > 0) {
        MostrarConfirm("Importante!", "¿Está seguro de guardar los ajustes?", "ConfirmAceptarAjusteInventario", null);
    }
});

function ConfirmAceptarAjusteInventario() {
    EjecutarAjax(urlBase + "Inventario/AjustesGuardar", "POST", JSON.stringify({ modelo: listaAjustes }), "successAceptarAjusteInventario", null);

}

function successAceptarAjusteInventario(rta) {
    MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", null, "success");
    listaAjustes = [];
    loadArrayToTable();
}


$("#btnCancelarAjusteInventario").click(function () {
    MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación?", "ConfirmCancelarAjusteInventario", null);
});

function ConfirmCancelarAjusteInventario() {
    listaAjustes = [];
    loadArrayToTable();
    limpiarControlesAgregar();
}


function loadArrayToTable() {
    var sHtml = '';
    $("#bodyTableAjustesInventario").html(sHtml);

    if (listaAjustes.length > 0) {
        $.each(listaAjustes, function (i, item) {
            sHtml += '<tr id="' + item.Id + '">';
            sHtml += '<td>' + item.CodSapMaterial + '</td>';
            sHtml += '<td>' + item.Material.Nombre + '</td>';
            sHtml += '<td>' + item.Cantidad + '</td>';
            sHtml += '<td>' + item.Unidad + '</td>';
            sHtml += '<td>' + item.DescSapTipoAjuste + '</td>';
            sHtml += '<td>' + item.DescSapMotivo + '</td>';
            sHtml += '<td>' + item.CostoPromedio * item.Cantidad  + '</td>';
            sHtml += '<td style="text-align:center; width:50px"> <a class="lnkDelete" onclick="evtEliminarElementoPorIndice(' + (i) + ');" href="javascript:void(0)" title="Eliminar"><b class="fa fa-trash-o"></b></a>  </td>';
            sHtml += '</tr>';
        });
    }
    $("#bodyTableAjustesInventario").append(sHtml);

    limpiarControlesAgregar();
}

function evtEliminarElementoPorIndice(indexElemento) {
    //$("#tableAjustesInventario").find("[id=" + itemId + "]").remove();
    if (listaAjustes.length > 0) {
        $.each(listaAjustes, function (i, item) {
            if (i == indexElemento) {
                listaAjustes.splice(i, 1);
            }
        });
    }
    loadArrayToTable();
}

function limpiarControlesAgregar() {
    var $lt1 = $("#CodSapMaterial").select2();
    $lt1.val('').trigger("change");
    $("#CodSapMaterial").select2({
        placeholder: "Seleccione el material"
    });

    var $lt2 = $("#IdPunto").select2();
    $lt2.val('').trigger("change");
    $("#IdPunto").select2({
        placeholder: "Seleccione el punto"
    });

    $("#CodSapMaterial").val("");
    $("#IdPunto").val("");

    $("#Cantidad").val("");
    $("#Cantidad").removeClass("errorValidate");
    $("#Cantidad").attr("data-mensajeerror", "");

    var $lt3 = $("#CodSapTipoAjuste").select2();
    $lt3.val('').trigger("change");
    $("#CodSapTipoAjuste").select2({
        placeholder: "* Seleccione tipo Ajuste"
    });

    //$("#CodSapTipoAjuste").val("");
    //$("#CodSapTipoAjuste").removeClass("errorValidate");
    //$("#CodSapTipoAjuste").attr("data-mensajeerror", "");

    var $lt4 = $("#CodSapMotivo").select2();
    $lt4.val('').trigger("change");
    $("#CodSapMotivo").select2({
    placeholder: "* Seleccione motivo"
    });

    //$("#CodSapMotivo").val("");
    //$("#CodSapMotivo").removeClass("errorValidate");
    //$("#CodSapMotivo").attr("data-mensajeerror", "");

    $("#Observaciones").val("");
    $("#Observaciones").removeClass("errorValidate");
    $("#Observaciones").attr("data-mensajeerror", "");


}

$("#IdPunto").change(function () {
    if ($("#IdPunto").val() != "") {
        EjecutarAjax(urlBase + "Inventario/ObtenerMaterialesxPuntoAjax", "GET", { IdPunto: $("#IdPunto").val() }, "SuccessCargarMateriales", null);
    }
});

function SuccessCargarMateriales(data) {
    
    if (data.Correcto) {

        $("#CodSapMaterial").html("");
        var sHtml = "";
        if (data.Elemento.length > 0) {
            for (var i = 0; i < data.Elemento.length; i++) {
                sHtml += "<option value='" + data.Elemento[i].CodigoSap + "' data-unidad = '" + data.Elemento[i].Unidad + "' data-CostoPromedio = '" + data.Elemento[i].CostoPromedio +  "'>" + data.Elemento[i].Nombre + "  </option>";
            }
        }

        $("#CodSapMaterial").html("<option value>Seleccione...</option>" + sHtml);

        var $CodSapMaterial = $("#CodSapMaterial").select2();
        $CodSapMaterial.val('').trigger("change");

        $("#CodSapMaterial").select2({
            placeholder: "Seleccione el material"
        });

    } else {
        //mostrarAlerta('Estado', data.Mensaje);
        MostrarMensaje("", data.Mensaje, "error");
    }
}



$("#CodSapTipoAjuste").change(function () {
    if ($("#CodSapTipoAjuste").val() != "") {
        EjecutarAjax(urlBase + "Inventario/ObtenerMotivosxTipoAjuste", "GET", { CodSapAjuste: $("#CodSapTipoAjuste").val() }, "SuccessCargarMotivos", null);
    }
});

function SuccessCargarMotivos(data) {
    
    if (data.Correcto) {

        $("#CodSapMotivo").html("");
        var sHtml = "";
        if (data.Elemento.length > 0) {
            for (var i = 0; i < data.Elemento.length; i++) {
                sHtml += "<option value='" + data.Elemento[i].CodSapMotivo + "'>" + data.Elemento[i].Descripcion + "</option>";
            }
        }

        $("#CodSapMotivo").html("<option value>Seleccione...</option>" + sHtml);

    } else {
        //mostrarAlerta('Estado', data.Mensaje);
        MostrarMensaje("", data.Mensaje, "error");
    }
}

$("#txtSearchInv").keyup(function () {

    
    $("#tableAjustesInventario tbody tr").hide();
    $.each($("#tableAjustesInventario tbody tr"), function (i, v) {
        $.each($(v).find("td"), function (j, va) {
            if ($(va).children().length == 0) {
                if ($(va).html().toLowerCase().indexOf($("#txtSearchInv").val().toLowerCase()) >= 0) {
                    $(v).show();
                    return false;
                }
            }
        });
    });
});