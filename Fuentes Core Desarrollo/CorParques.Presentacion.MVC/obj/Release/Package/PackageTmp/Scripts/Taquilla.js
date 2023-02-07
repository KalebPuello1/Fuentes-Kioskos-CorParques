$(function () {
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "Taquilla/GetPartial", "GET", null, "printPartialModal", { title: "Crear Punto de Taquilla", url: urlBase + "Taquilla/Insert", metod: "GET", func: "successInsertParking", modalLarge: true });
    });
    setEventEdit();
});

function setFileLoad(prev) {
    //fileDropZone($("#dropzoneElement"), "/Taquilla/saveFile", "Taquilla/RemoveFile", "image/*", $("#Imagen"), prev, "/Images/Parking/");
    fileDropZone($("#dropzoneElement"), urlBase + "Taquilla/saveFile", "Taquilla/RemoveFile", "image/*", $("#Imagen"), prev, urlBase + "Images/Parking/");
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
        EjecutarAjax(urlBase + "Taquilla/GetById", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar Punto de taquilla", url: urlBase + "Taquilla/Update", metod: "GET", func: "successUpdateParking", modalLarge: true });
    });
    $(".lnkDisable").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea inactivar este registro?", "EliminarTaquilla", $(this).data("id"));
        //if (confirm("Está seguro que desea inactivar este registro?"))
        //    EjecutarAjax(urlBase + "Taquilla/Delete", "GET", { id: $(this).data("id") }, "successDeleteParking", null);
    });
}
function successInsertParking(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Taquilla/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Su operación fue exitosa.");
    }
}
function successDeleteParking(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Taquilla/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        mostrarAlerta("Registro inactivado con éxito.");
    }
}
function successUpdateParking(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Taquilla/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Edición exitosa.")
    }
}

function EliminarTaquilla(Id) {
    EjecutarAjax(urlBase + "Taquilla/Delete", "GET", { id: Id }, "successDeleteParking", null);
}