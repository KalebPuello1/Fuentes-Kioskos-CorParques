//Powered by RDSH
$(function () {
    $("#div_message_error").hide();
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "ResolucionFactura/GetPartial", "GET", null, "printPartialModal", { title: "Crear resolucion factura", url: urlBase + "ResolucionFactura/Insert", metod: "GET", func: "successInsert" });
    });

    $("#FechaInicio").mask("99/99/9999");
    $("#FechaFinal").mask("99/99/9999");

    setEventEdit();
});
function setEventEdit() {
    EstablecerToolTipIconos();

    
    $(".lnkDisable").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea eliminar este registro?", "EliminarResolucion", $(this).data("id"));
    });

    $(".lnkApproved").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea aprobar este registro?", "AprobarResolucion", $(this).data("id"));
    });
}

function cambioPunto() {
    $("#IdPunto").change(function () {
        $("#Prefijo").val($("#IdPunto option:selected").data("pref"));

    });
}
function cambiofecha()
{
    $("#FechaInicio").on('dp.change', function (e) {
        var fechaF = e.date._d;
        fechaF = fechaF.setMonth(fechaF.getMonth() + parseInt($("#hdMeses").val()));
        $("#FechaFinal").val(formatDate(e.date._d))
    });
    
}
function formatDate(date) {
    month = '' + (date.getMonth() + 1),
        day = '' + date.getDate(),
        year = date.getFullYear();

    if (month.length < 2) 
        month = '0' + month;
    if (day.length < 2) 
        day = '0' + day;

    return [day, month, year].join('/');
}
function successInsert(rta) {

    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "ResolucionFactura/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Su operación fue exitosa.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}
function successDelete(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "ResolucionFactura/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        MostrarMensaje("Importante", "Registro eliminado con éxito.", "success");
    } else {
        MostrarMensaje("Importante", "No fue posible eliminar el registro.", "error");
    }

}
function successApproved(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "ResolucionFactura/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        MostrarMensaje("Importante", "La resolucion fue aprobada con éxito.", "success");
    } else {
        MostrarMensaje("Importante", "No fue posible aprobar la resolucion.", "error");
    }

}
function EliminarResolucion(Id) {
    EjecutarAjax(urlBase + "ResolucionFactura/Delete", "GET", { id: Id }, "successDelete", null);
}
function AprobarResolucion(Id) {
    EjecutarAjax(urlBase + "ResolucionFactura/Update", "GET", { id: Id }, "successApproved", null);
}
