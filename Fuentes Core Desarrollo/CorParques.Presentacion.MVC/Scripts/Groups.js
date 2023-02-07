/// <reference path="General.js" />

$(function () {
    $("#div_message_error").hide();
    $("#lnkAdd").click(function () {
               
        EjecutarAjax(urlBase + "Groups/GetPartial", "GET", null, "printPartialModal", { title: "Crear Grupo", url: urlBase + "Groups/Insert", metod: "GET", func: "successInsertGroup", modalLarge: true });        

    });
    setEventEdit();
});

function setEventEdit() {
    EstablecerToolTipIconos();
   
    $(".lnkDetail").click(function () {
        EjecutarAjax(urlBase + "Groups/Detail", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Detalle Grupo", hidesave: "Y", showreturn: "Y" });
    });
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "Groups/GetById", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar Grupo", url: urlBase + "Groups/Update", metod: "GET", func: "successUpdateGroup", modalLarge: true });
    });
    $(".lnkDisable").click(function () {
        MostrarConfirm("Importante", "¿Está seguro de desactivar el grupo?", "desabilitargrupo", $(this).data("id"));

        //if (confirm("Está seguro de desactivar el grupo?"))
        //    EjecutarAjax(urlBase + "Groups/UpdateStatus", "GET", { id: $(this).data("id") }, "successDeleteGroup", null);
    });
}

function desabilitargrupo(id) {

    EjecutarAjax(urlBase + "Groups/UpdateStatus", "GET", { id: id }, "successDeleteGroup", null);

}

function successInsertGroup(rta) {
    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Groups/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Su operación fue exitosa.");
    } else {
        $("#lbl_message_error").html("El grupo ya existe.");
        $("#div_message_error").show();
    }

}


function successUpdateGroup(rta) {
    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Groups/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Edición Exitosa.")
    } else {
        $("#lbl_message_error").html("El grupo ya existe.");
        $("#div_message_error").show();
    }
}

function successDeleteGroup() {
    EjecutarAjax(urlBase + "Groups/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
    mostrarAlerta("El grupo fue desactivado con éxito.")
}

function InitPartialCreate(dataPuntos) {

    setAutocompleteCategory()
    tagsAutocomplete($('#tagsPuntos'), $('#puntosAutocomplete'), $("#hdf_UsuariosSeleccionados"), dataPuntos, true)

}

