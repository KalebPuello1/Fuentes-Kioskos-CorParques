$("#ddlPedido").select2({
    placeholder: "* Seleccione el pedido"
});

$("#ddlPunto").select2({
    placeholder: "* Seleccione el punto"
});

$(function () {
    var lisProductosPedido = [];
    var tablaMostrar = [];

    //evento change pedidos
    $("#ddlPedido").change(function () {
        LimpiarTabla();
        var CodSap = $(this).val();
        MostrarTabla(CodSap);
    });

    //Evento cancelar
    $("#btnCancelar").click(function () {
        MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "Cancelar", "");
    });

    //Evento aceptar
    $("#btnAceptar").click(function () {

        if (validarFormulario("formDescargueMasivo")) {
            if (ValidarCheck())
                MostrarConfirm("Importante!", "¿Está seguro de hacer el descargue? ", "Descargue", "");
            else
                MostrarMensaje("Importante", "Debe seleccionar mínimo un producto para descargar", "error");
        }
    });

    //Ocultar seccion de puntos
    $("#puntos").hide();

});

function ValidarCheck() {
    var cont = 0;
    $.each($(".chx"), function (i, item) {
        if ($(item).is(":checked"))
            cont++;
    });
    return cont > 0;
}

function setEventMasivoBoleta() {
    //Ckeck seleccionar todos

    $('#chxTodos').unbind('click');
    $("#chxTodos").click(function () {
        if ($("#chxTodos").is(':checked')) {

            $.each($(".chx"), function (i, item) {
                $(this).prop('checked', true);
            });

        } else {
            $.each($(".chx"), function (i, item) {
                $(this).prop('checked', false);
            });
        }
    });
}

function MostrarTabla(codSap) {
    if (tablaMostrar.length > 0 && BuscarPedido(codSap) > 0) {
        $("#puntos").show();
        var tablaHead = "<div class='row x_panel'> <table class='table table-striped jambo_table' width='100%'>";
        tablaHead += "<thead>"
                                + "<th>Código SAP</th>"
                                + "<th>Producto</th>"
                                + "<th>Cantidad</th>"
                                + "<th><input type='checkbox' id='chxTodos' /></th>"
                                + "</thead>";
        var tablaBody = "<tbody>";

        $.each(tablaMostrar, function (i, item) {
          if  ($.trim(item.CodigoVenta) == $.trim(codSap)){
              tablaBody += "<tr>"
                                + "<td style='vertical-align: middle;'>" + item.CodigoSap + "</td>"
                                + "<td style='vertical-align: middle;'>" + item.Nombre + "</td>"
                                + "<td style='vertical-align: middle;'>" + item.Cantidad + "</td>"
                                + "<td style='vertical-align: middle;'><input type='checkbox' class='chx' data-id='" + item.IdSolicitudBoleteria + "' /></td>"
                            + "</tr>";
            }
        });

        var footer = "</tbody></table>";

        $("#vwTabla").html(tablaHead + tablaBody + footer);
        setEventMasivoBoleta();

    } else
        $("#puntos").hide();
}

function LimpiarTabla() {
    $("#vwTabla").html("");
}

function BuscarPedido(CodSap) {
    var cont = 0;
    $.each(tablaMostrar, function (i, item) {
        if ($.trim(item.CodigoVenta) == $.trim(CodSap))
            cont = cont + 1;
    });

    return cont;
}

function Cancelar() {
    window.location = urlBase + "DescargueMaterialMasivo/Index";
}

function Descargue() {
    var idPunto = $("#ddlPunto").val();
    var cod = $("#ddlPedido").val();
    //var lista = ObtenerListaProductos(cod);
    //var listaSolicitud = ObtenerListaFiltrarSolicitud(lista);
    var ids = ObtenerIdsSolicitudes();

    EjecutarAjaxJson(urlBase + "DescargueMaterialMasivo/DescargarProductos", "POST", {
        codigoPedido: cod, IdPuntoDescargue: idPunto, IdSolicitudes : ids
    }, "sucessDescargue", null);


    //EjecutarAjaxJson(urlBase + "DescargueMaterialMasivo/DescargarProductos", "POST", {
    //    productosPedido: listaSolicitud, IdPuntoDescargue: idPunto
    //}, "sucessDescargue", null);
}

function ObtenerListaProductos(CodSapPedido) {
    return $.grep(lisProductosPedido, function (element, index) {
            return element.CodigoVenta == CodSapPedido;
    });
}

function ObtenerListaFiltrarSolicitud(lista) {
    var resp = [];
    $.each($(".chx:checked"), function (i, item) {
        $.each(lista, function (i2, item2) {

            if (item2.IdSolicitudBoleteria == $(item).data("id"))
                resp.push(item2);
        });
    });

    return resp;
}

function ObtenerIdsSolicitudes() {
    var ids = "";
    $.each($(".chx:checked"), function (i, item) {
        if (ids.length > 0)
            ids = ids + "," + $(item).data("id");
        else
            ids = ids + $(item).data("id");
    });

    return ids;
}

function sucessDescargue(rta) {
    if (rta.Correcto) {
        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "DescargueMaterialMasivo", "success");
    } else {
        MostrarMensaje("", rta.Mensaje);
    }
}