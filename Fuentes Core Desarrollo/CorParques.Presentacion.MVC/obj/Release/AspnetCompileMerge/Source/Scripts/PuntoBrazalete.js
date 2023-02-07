/// <reference path="General.js" />

$(function () {
    $("#div_message_error").hide();   
    setEventEdit();
});

function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkEdit").unbind("click");

    $(".lnkDetail").click(function () {
        EjecutarAjax(urlBase + "Groups/Detail", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Detalle Grupo", hidesave: "Y", showreturn: "Y" });
    });

    $(".lnkEdit").unbind("click");
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "PuntoBrazalete/GetById", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar punto", url: null, metod: "GET", func: null, modalLarge: true, hideSaveGeneric: true });
        //Muestra el guardar que arma el detalle del pedido.
        $("#btnSaveCustomizable").show();

        //Ejecuta el armado del objeto convenio detalle y el guardado a base de datos.
        $('#btnSaveCustomizable').unbind('click');
        $("#btnSaveCustomizable").click(function () {

            //válidar que tenga al menos un brazalete
            var existeCheckeado = false;
            var ids = [];

            $.each($("[name=grpoBrazaletes]"), function (a, b) {
                if ($(this).is(":checked")) {
                    existeCheckeado = true;
                    ids.push($(this).data("id"));
                }
            });

            if (!existeCheckeado) {
                MostrarMensaje("Importante", "Debe seleccionar por lo menos un brazalete para asociar al punto.", "error");
            } else {
                EjecutarAjaxJson(urlBase + "PuntoBrazalete/Update", "POST", { modelo: { Id: $("#Id").val(), BrazaletesAsociados: ids }}, "successUpdatePuntoBrazalete");
            }

        });
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


function successUpdatePuntoBrazalete(rta) {
    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "PuntoBrazalete/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
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


