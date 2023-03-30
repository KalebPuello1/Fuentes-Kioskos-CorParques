$("#ddlPedido").select2({
    placeholder: "* Seleccione el pedido"
});

$("#ddlPunto").select2({
    placeholder: "* Seleccione el punto"
});

$(function () {
    var lisProductosPedido = [];
    var tablaMostrar = [];

    $("#ddlPedido").change(function () {
        LimpiarTabla();
        var CodSap = $(this).val();
        MostrarTabla(CodSap);
    });

    $("#btnCancelar").click(function () {
        MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "Cancelar", "");
    });

    $("#btnAceptar").click(function () {

        if (validarFormulario("formDescargueMasivo")) {
            MostrarConfirm("Importante!", "¿Está seguro de hacer el descargue ", "Descargue", "");
        }
    });

});


function MostrarTabla(codSap) {
    debugger;
    if (tablaMostrar.length > 0 && BuscarPedido(codSap) > 0) {
        var tablaHead = "<div class='row x_panel'> <table class='table table-striped jambo_table' width='100%'>";
        tablaHead += "<thead>" + "<th>Producto</th>"
                                + "<th>Cantidad</th>"
                                + "</thead>";
        var tablaBody = "<tbody>";

        $.each(tablaMostrar, function (i, item) {
          if  ($.trim(item.CodigoVenta) == $.trim(codSap)){
                tablaBody += "<tr>"
                                + "<td style='vertical-align: middle;'>" + item.Nombre + "</td>"
                                + "<td style='vertical-align: middle;'>" + item.Cantidad + "</td>"
                            + "</tr>";
            }
        });

        var footer = "</tbody></table>";

        $("#vwTabla").html(tablaHead + tablaBody + footer);

    }
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
    debugger;
    var idPunto = $("#ddlPunto").val();
    var cod = $("#ddlPedido").val();
    var lista = ObtenerListaProductos(cod);
    debugger;
    EjecutarAjaxJson(urlBase + "DescargueMaterialMasivo/DescargarProductos", "POST", {
        productosPedido: lista, IdPuntoDescargue: idPunto
    }, "sucessDescargue", null);
}

function ObtenerListaProductos(CodSapPedido) {
    return $.grep(lisProductosPedido, function (element, index) {
        return element.CodigoVenta == CodSapPedido;
    });
}

function sucessDescargue(rta) {
    if (rta.Correcto) {
        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "DescargueMaterialMasivo", "success");
    } else {
        MostrarMensaje("", rta.Mensaje);
    }
}