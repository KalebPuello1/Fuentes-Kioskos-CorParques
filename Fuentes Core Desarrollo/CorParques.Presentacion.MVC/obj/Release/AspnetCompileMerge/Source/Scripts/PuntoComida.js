$(function () {
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "PuntoComida/GetPartial", "GET", null, "printPartialModal", { title: "Crear Punto Comida", url: urlBase + "PuntoComida/Insert", metod: "GET", func: "successInsert", modalLarge: true });
    });
    setEventEdit();
});

function setFileLoad(prev) {
    //fileDropZone($("#dropzoneElement"), "/PuntoComida/saveFile", "Puntos/RemoveFile", "image/*", $("#Imagen"), prev, "/Images/Attractions/");
    fileDropZone($("#dropzoneElement"), urlBase + "PuntoComida/saveFile", "PuntoComida/RemoveFile", "image/*", $("#Imagen"), prev, urlBase + "Images/Attractions/");
    $(".dateTime").datetimepicker({
        format: 'HH:mm',
    });
}
function setEventStatus() {
    $("#EstadoId").change(function () {
        $("#DescripcionCierre").parent().parent().hide();
        $("#DescripcionCierre").removeClass("required");
        if ($(this).val() == "4") {
            $("#DescripcionCierre").parent().parent().show();
            $("#DescripcionCierre").addClass("required");
        }
    });
}
function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "PuntoComida/GetById", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar Punto Comida", url: urlBase + "PuntoComida/Update", metod: "GET", func: "successUpdatePunto", modalLarge: true });
    });
    $(".lnkDisable").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea inactivar este registro?", "EliminarPuntoComida", $(this).data("id"));
        //if (confirm("Está seguro que desea eliminar este punto de atención?"))
        //    EjecutarAjax(urlBase + "PuntoComida/Delete", "GET", { id: $(this).data("id") }, "successDelete", null);
    });
}
function successInsert(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "PuntoComida/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Su operación fue exitosa.");
    }
}
function successDelete(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "PuntoComida/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        mostrarAlerta("Registro inactivado con éxito.");
    }
}
function successUpdatePunto(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "PuntoComida/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Edición exitosa.")
    }
}

function EliminarPuntoComida(Id) {
    EjecutarAjax(urlBase + "PuntoComida/Delete", "GET", { id: Id }, "successDelete", null);    
}
