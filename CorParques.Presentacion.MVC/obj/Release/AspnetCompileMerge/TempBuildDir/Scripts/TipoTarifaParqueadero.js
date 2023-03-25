$(function () {
    $("#div_message_error").hide();
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "TipoTarifaParqueadero/GetPartial", "GET", null, "printPartialModal", { title: "Crear tipo de tarifa parqueadero", url: urlBase + "TipoTarifaParqueadero/Insert", metod: "GET", func: "successInsert" });
    });
    setEventEdit();    
});
function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "TipoTarifaParqueadero/Obtener", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar tipo de tarifa parqueadero", url: urlBase + "TipoTarifaParqueadero/Update", metod: "GET", func: "successUpdate" });
    });

    $(".lnkDisable").click(function () {
        if (confirm("¿Está seguro que desea inactivar este tipo de tarifa?"))
            EjecutarAjax(urlBase + "TipoTarifaParqueadero/UpdateEstado", "GET", { id: $(this).data("id") }, "successDisabled", null);
    });

    $(".lnkDetail").click(function () {
        EjecutarAjax(urlBase + "TipoTarifaParqueadero/Detalle", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Detalle tipo de tarifa parqueadero", hidesave:"S", showreturn:"S" });
    });

}
function successInsert(rta) {

    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "TipoTarifaParqueadero/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
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
        EjecutarAjax(urlBase + "TipoTarifaParqueadero/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Edición exitosa.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }    
}

function successDisabled(rta) {
    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "TipoTarifaParqueadero/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("El tipo de tarifa fue inactivado con éxito.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}