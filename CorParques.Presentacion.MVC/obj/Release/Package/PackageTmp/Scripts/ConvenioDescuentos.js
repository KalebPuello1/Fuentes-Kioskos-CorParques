$(function () {
    $("#div_message_error").hide();
    $("#lnkAdd").click(function () {
        //EjecutarAjax(urlBase + "ConvenioDescuento/GetPartial", "GET", null, "printPartialModal", { title: "Crear convenio descuento", url: urlBase + "ConvenioDescuento/Insert", metod: "GET", func: "successInsert" });
        EjecutarAjax(urlBase + "ConvenioDescuento/GetPartial", "GET", null, "printPartialModal", { title: "Crear convenio descuento", url: urlBase + "", metod: "GET", func: "", func2: "loadCalendar", hidesave: true });
    });
    setEventEdit();
});


function eventoGuardarBoton() {

    if (!validarFormulario("formCrearConvenioDescuento *")) {
        return false;
    }
    else {
        var _ConvenioDescuento = new Object();
        var _DescunetosProducto = [];
        var _DescunetosLinea = [];

        _ConvenioDescuento.NomConvenioDescuento = $("#NomConvenioDescuento").val();
        _ConvenioDescuento.DescConvenioDescuento = $("#DescConvenioDescuento").val();
        _ConvenioDescuento.CodSap = $("#CodSap").val();
        _ConvenioDescuento.CodInterno = $("#CodInterno").val();
        _ConvenioDescuento.FecVigenciaDesde = $("#FecVigenciaDesde").val();
        _ConvenioDescuento.FecVigenciaHasta = $("#FecVigenciaHasta").val();
        _ConvenioDescuento.IdEstado = $("#IdEstado").val();
        
        var lista_itemConvenioDescuentoProducto = $("#formCrearConvenioDescuento").find(".itemConvenioDescuentoProducto");
        $.each(lista_itemConvenioDescuentoProducto, function (index, value) {
            if (isNaN( $(this).val() )) {
            } else {

                var _DescItemProd = new Object();
                _DescItemProd.Id = parseInt($(this).attr("idItem"));
                _DescItemProd.Valor = parseFloat($(this).val());
                _DescunetosProducto.push(_DescItemProd);
            }
        });

        var lista_itemConvenioDescuentoLinea = $("#formCrearConvenioDescuento").find(".itemConvenioDescuentoLinea");
        $.each(lista_itemConvenioDescuentoLinea, function (index, value) {
            if (isNaN($(this).val())) {
            } else {

                var _DescItemLinea = new Object();
                _DescItemLinea.Id = parseInt($(this).attr("idItem"));
                _DescItemLinea.Valor = parseFloat($(this).val());
                _DescunetosLinea.push(_DescItemLinea);
            }
        });


        _ConvenioDescuento.ListaProductos = _DescunetosProducto;
        _ConvenioDescuento.ListaLineas = _DescunetosLinea;
        EjecutarAjax(urlBase + "ConvenioDescuento/Insert", "POST", JSON.stringify(_ConvenioDescuento), "SuccessDescuentoParqueaderoInsert", null);
    }

}

function SuccessDescuentoParqueaderoInsert(data) {

    $("#div_message_error").hide();
    if (data.Correcto) {
        EjecutarAjax(urlBase + "ConvenioDescuento/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Su operación fue exitosa.");
    }
    else {
        $("#lbl_message_error").html(data.Mensaje);
        $("#div_message_error").show();
    }
}

function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "ConvenioDescuento/Obtener", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar convenio descuento", url: urlBase + "ConvenioDescuento/Update", metod: "GET", func: "successUpdate" });
    });

    $(".lnkDisable").click(function () {
        if (confirm("¿Está seguro que desea inactivar este convenio?"))
            EjecutarAjax(urlBase + "ConvenioDescuento/UpdateEstado", "GET", { id: $(this).data("id") }, "successDisable", null);
    });

    $(".lnkDetail").click(function () {
        EjecutarAjax(urlBase + "ConvenioDescuento/Detalle", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Detalle convenio descuento", hidesave: "S", showreturn: "S" });
    });
}
function successInsert(rta) {

    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "ConvenioDescuento/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Su operación fue exitosa.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}

function successUpdate(rta) {
    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "ConvenioDescuento/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Edición exitosa.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}

function successDisable(rta) {
    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "ConvenioDescuento/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("El convenio fue inactivado con éxito.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}