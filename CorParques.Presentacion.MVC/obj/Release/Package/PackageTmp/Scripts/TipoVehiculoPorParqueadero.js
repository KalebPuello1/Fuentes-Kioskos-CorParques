$(function () {
    $("#div_message_error").hide();
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "TipoVehiculoPorParqueadero/GetPartial", "GET", null, "printPartialModal", { title: "Crear disponibilidad parqueadero", url: urlBase + "TipoVehiculoPorParqueadero/Insert", metod: "GET", func: "successInsert" });
    });
    setEventEdit();    
});
function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "TipoVehiculoPorParqueadero/Obtener", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar disponibilidad parqueadero", url: urlBase + "TipoVehiculoPorParqueadero/Update", metod: "GET", func: "successUpdate" });
    });

    $(".lnkDisable").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea inactivar esta disponibilidad?", "InactivarDisponibilidad", $(this).data("id"));
        //if (confirm("¿Está seguro que desea inactivar esta disponibilidad?"))
        //    EjecutarAjax(urlBase + "TipoVehiculoPorParqueadero/UpdateEstado", "GET", { id: $(this).data("id") }, "successDisable", null);
    });
}
function successInsert(rta) {

    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "TipoVehiculoPorParqueadero/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
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
        EjecutarAjax(urlBase + "TipoVehiculoPorParqueadero/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
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
        EjecutarAjax(urlBase + "TipoVehiculoPorParqueadero/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("La disponibilidad fue inactivada con éxito.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}
function InactivarDisponibilidad(Id)
{
    EjecutarAjax(urlBase + "TipoVehiculoPorParqueadero/UpdateEstado", "GET", { id: Id }, "successDisable", null);
}