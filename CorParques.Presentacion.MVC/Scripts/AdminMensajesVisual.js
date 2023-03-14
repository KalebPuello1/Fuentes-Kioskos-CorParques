$(function () {
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "AdminMensajesVisual/GetPartial", "GET", null, "printPartialModal", { title: "Crear mensaje", url: urlBase + "AdminMensajesVisual/Insert", metod: "GET", func: "successInsertParameter" });
    });
    setEventEdit();
});
function setEventEdit() {
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "AdminMensajesVisual/GetById", "GET", { Codigo: $(this).data("id") }, "printPartialModal", { title: "Editar mensaje", url: urlBase + "AdminMensajesVisual/Update", metod: "GET", func: "successUpdateParameter" });
    });
    $(".lnkDelete").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea eliminar este registro?", "InactivarParametro", $(this).data("id"));
        //if (confirm("Está seguro que desea eliminar este parámetro?"))
        //    EjecutarAjax(urlBase + "Parameters/Delete", "GET", { id: $(this).data("id") }, "successDeleteParameter", null);
    });
}
function successInsertParameter(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "AdminMensajesVisual/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("El registro fue creado con éxito.");
    }
}
function successDeleteParameter(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "AdminMensajesVisual/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        mostrarAlerta("El registro fue eliminado con éxito.");
    }
}
function successUpdateParameter(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "AdminMensajesVisual/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("El registro fue modificado con éxito.")
    }
}
function InactivarParametro(Codigo) {
    EjecutarAjax(urlBase + "AdminMensajesVisual/Delete", "GET", { Codigo: Codigo }, "successDeleteParameter", null);
}
function SuccessMensajetexto(rta, tipocortesia) {
    if (rta === undefined) {
    }
    else {
        if (!rta.Correcto) {
            Dropzone.forElement("#dropzoneElements").removeAllFiles(true);
            MostrarMensaje("Importante", rta.Mensaje, "error");
        }
        else {
            EjecutarAjax(urlBase + "Cortesia/MensajeTexto", "GET", null, "printPartial", { func: "BotonCancelarUsuario" });
            cerrarModal("modalCRUD");
            if (rta.Mensaje !== undefined && rta.Mensaje !== null && rta.Mensaje !== "") {
                MostrarMensaje("Importante", rta.Mensaje, "warning");


            }
            else {
                mostrarAlerta("Se agrego la imagen con éxito.");
            }

        }
    }
}
