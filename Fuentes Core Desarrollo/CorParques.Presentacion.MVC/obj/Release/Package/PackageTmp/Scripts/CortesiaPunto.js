//Powered by RDSH
$(function () {
    $("#div_message_error").hide();
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "CortesiaPunto/GetPartial", "GET", null, "printPartialModal", { title: "Crear cortesia por destreza", url: urlBase + "CortesiaPunto/Insert", metod: "GET", func: "successInsert" });
    });
    setEventEdit();    
});
function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "CortesiaPunto/Obtener", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar cortesia por destreza", url: urlBase + "CortesiaPunto/Update", metod: "GET", func: "successUpdate" });
    });

    $(".lnkDisable").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea inactivar este registro?", "InactivarCortesiaPunto", $(this).data("id"));

        //if (confirm("¿Está seguro que desea inactivar este registro?"))
        //    EjecutarAjax(urlBase + "CortesiaPunto/UpdateEstado", "GET", { id: $(this).data("id") }, "successDisable", null);
    });

    $(".lnkDetail").click(function () {
        EjecutarAjax(urlBase + "CortesiaPunto/Detalle", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Detalle cortesia por destreza", hidesave: "S", showreturn: "S" });
    });
}
function successInsert(rta) {

    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "CortesiaPunto/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
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
        EjecutarAjax(urlBase + "CortesiaPunto/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
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
        EjecutarAjax(urlBase + "CortesiaPunto/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Registro inactivado con éxito.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}

function InactivarCortesiaPunto(Id)
{
    EjecutarAjax(urlBase + "CortesiaPunto/UpdateEstado", "GET", { id: Id}, "successDisable", null);
}