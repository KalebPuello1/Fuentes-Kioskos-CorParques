$(function () {
    $("#div_message_error").hide();
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "CentroMedico/GetPartial", "GET", null, "printPartialModal", { title: "Crear Ubicación", url: urlBase + "CentroMedico/Insert", metod: "GET", func: "successInsert" });
    });
    setEventEdit();
});
function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "CentroMedico/Obtener", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar Ubicación", url: urlBase + "CentroMedico/Update", metod: "GET", func: "successUpdate" });
    });

    $(".lnkDelete").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea eliminar este registro?", "InactivarUbicacion", $(this).data("id"));
        //if (confirm("¿Está seguro que desea inactivar este registro?"))
        //    EjecutarAjax(urlBase + "CentroMedico/UpdateEstado", "GET", { id: $(this).data("id") }, "successUpdate", null);
    });
}

function InactivarUbicacion(Id)
{
    EjecutarAjax(urlBase + "CentroMedico/UpdateEstado", "GET", { id: Id }, "successDelete", null);
}

function successInsert(rta) {

    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "CentroMedico/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
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
        EjecutarAjax(urlBase + "CentroMedico/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Edición exitosa.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}

function successDelete(rta) {
    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "CentroMedico/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("La ubicación fue eliminada con éxito.");
    }
    else {
        MostrarMensaje("Importante", rta.Mensaje, "error");
    }
}