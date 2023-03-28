var ObservacionesTaq;
var listatraslados = [];
var itemsexcel=""

$(function () {

    $.each(pedidos, function (i, v) {
        itemsexcel += v.CodSapPedido + "|";
    });

    $("#IdUsuario").change(function () {

        EjecutarAjax(urlBase + "SalidaPedidos/ObtenerUsuario", "GET", { usuario: $(this).val() }, "SucessChange", null);

    });
    $(".filtro").keyup(function () {
        itemsexcel = []
        var f = $(this);
        $("#tbTrasladoInventario tbody tr.principal").hide();
        $.each($("#tbTrasladoInventario tbody tr.principal"), function (i, v) {
            $.each($(v).find("td."+$(f).data("id")), function (j, va) {
                if ($(va).children().length == 0) {
                    if ($(va).html().toLowerCase().indexOf($(f).val().toLowerCase()) >= 0) {
                        $(v).show();
                        itemsexcel+=$(v).find(".lnkDisable").data("id").toString()+"|";
                        return false;
                    }
                }
            });
        });

    });

    InicializarMateriales();

});
function success(rta) {
    if (rta.Correcto) {
        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "Salidapedidos", "success");
    }
    else {
        MostrarMensaje("Fallo al guardar", rta.Mensaje);
    }
}


function SucessChange(data) {

    if (data !== "") {

        $("#NombreUsuario").html(data.Nombre);
        $("#hdIdUsuario").val(data.IdUsuario);
    } else {
        $("#NombreUsuario").html("");
        $("#hdIdUsuario").val("");
        $("#IdUsuario").val("");
        MostrarMensaje("Importante", "El usuario no existe.", "error");
    }
}


function InicializarMateriales() {
    
    $(".lnkDisable").click(function () {
        RealizarSalida($(this).data("id"));
    });


    $("#txtSearchMateri").keyup(function () {
        itemsexcel = []
        $("#tbTrasladoInventario tbody tr.principal").hide();
        $.each($("#tbTrasladoInventario tbody tr.principal"), function (i, v) {
            $.each($(v).find("td.principal"), function (j, va) {
                if ($(va).children().length == 0) {
                    if ($(va).html().toLowerCase().indexOf($("#txtSearchMateri").val().toLowerCase()) >= 0) {
                        $(v).show();
                        itemsexcel+=$(v).find(".lnkDisable").data("id").toString()+"|";
                        return false;
                    }
                }
            });
        });
    });


}

function Excel()
{
    EjecutarAjax(urlBase + "SalidaPedidos/Excel", "GET", { pedidos: JSON.stringify(pedidos), lista: itemsexcel.substring(0, itemsexcel.length-1) }, "cargarTabla", null);
}
function cargarTabla(datos, params) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante", datos);
        }
        else {
            window.location = urlBase + 'SalidaPedidos/Download?Data=' + datos;
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
}

function RealizarSalida(CodSapPedido) {

    if ($("#hdIdUsuario").val() === "") {
        MostrarMensaje("Importante", "Debe seleccionar un usuario a quien entregar el material.", "error");
        return false;
    }

    var valido = false;
    listatraslados = [];
    $.each(pedidos, function (i, v) {
        if (v.CodSapPedido == CodSapPedido) {
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
                objTrasladoInventario.Id = v.IdProducto;
                objTrasladoInventario.IdPuntoOrigen = $("#IdPuntoOrigen").val();
                objTrasladoInventario.IdPuntoDestino = $("#IdPuntoDestino").val();
                objTrasladoInventario.idUsuario = $("#hdIdUsuario").val();
                objTrasladoInventario.Cantidad = v.Cantidad;
                objTrasladoInventario.UnidadMedida = v.Cliente;
                objTrasladoInventario.Pedido = CodSapPedido;
                listatraslados.push(objTrasladoInventario);
            } else {
                MostrarMensaje("Importante", "No cuenta con el inventario suficiente del material '" + v.Producto + "' para realizar la entrega del pedido.", "error");
                return false;
            }
        }
    });
    if(valido)
        MostrarConfirm("Importante!", "¿Está seguro de realizar la entrega del pedido a " + $("#NombreUsuario").text() +"? ", "GuardarAlistamiento", "");
    
}


function GuardarAlistamiento() {
    EjecutarAjax(urlBase + "SalidaPedidos/Guardar", "POST", JSON.stringify({ modelo: listatraslados }), "success", null);
  
}


