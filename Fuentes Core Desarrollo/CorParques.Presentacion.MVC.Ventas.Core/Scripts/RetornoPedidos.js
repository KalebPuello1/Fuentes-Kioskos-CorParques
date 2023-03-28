
$(function () {


    $("#IdPedido").change(function () {

        EjecutarAjax(urlBase + "RetornoPedidos/ObtenerPedido", "GET", { codigo: $(this).val() }, "SucessChange", null);

    });
    $("#btnCreate").click(function () {
        if (validarFormulario("frmTransladoInventario *"))
        {
            if ($("#hdIdPedido").val() == "")
                MostrarMensaje("Importante", "Por favor digite un numero de pedido.", "error");
            else {
                var obj = { CodSapPedido: $("#hdIdPedido").val(), Motivo: $("#ddlMotivo option:selected").val(), Observacion: $("#txtObser").val() }
                EjecutarAjax(urlBase + "RetornoPedidos/Crear", "GET", obj, "success", null);
            }
        }
    });
    $(".lnkDisable").click(function () {

        MostrarConfirm("Importante!", "¿Está seguro de eliminar esta solicitud?", "EliminarSolicitud", $(this).data("id"));
    });

    $(".lnkUpdate").click(function () {

        MostrarConfirm("Importante!", "¿Está seguro que desea recibir este material?", "RecibirSolicitud", $(this).data("id"));
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

});
function success(rta) {
    if (rta.Correcto) {
        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "RetornoPedidos"+(window.location.href.indexOf("Pedidos/Ingreso")>=0?"/Ingreso":""), "success");
    }
    else {
        MostrarMensaje("Fallo al guardar", rta.Mensaje);
    }
}


function SucessChange(data) {
    if (data.length>0) {
            
        $("#hdIdPedido").val(data[0].CodSapPedido);
        $("#tdNCli").html(data[0].Cliente);
        $("#tdNAs").html(data[0].Asesor);
        $("#tdFec").html(data[0].FechaUso);
        var prd = "<table>"
        $.each(data,function(i,v){
            prd +="<tr><td>"+v.Producto+"</td><td>"+v.Cantidad+"</td></tr>" 
        });
        prd += "<table>"
        $("#tdProd").html(prd);
        $(".divPedido").show()
    } else {

        $(".divPedido").hide()
        $("#hdIdPedido").val("");
        $("#IdPedido").html("");
        $("#tdNCli").html("");
        $("#tdNAs").html("");
        $("#tdFec").html("");
        $("#tdProd").html("");
        MostrarMensaje("Importante", "El pedido no existe o no puede ser retornado.", "error");
    }
}



function EliminarSolicitud(CodSapPedido) {

    var obj = { CodSapPedido: CodSapPedido.toString(), Motivo: "1", Observacion: "" }

    EjecutarAjax(urlBase + "RetornoPedidos/Eliminar", "GET", obj, "success", null);
}


function RecibirSolicitud(CodSapPedido) {

    var obj = { CodSapPedido: CodSapPedido.toString(), Motivo: "1", Observacion: "" }

    EjecutarAjax(urlBase + "RetornoPedidos/Recibir", "GET", obj, "success", null);
}





