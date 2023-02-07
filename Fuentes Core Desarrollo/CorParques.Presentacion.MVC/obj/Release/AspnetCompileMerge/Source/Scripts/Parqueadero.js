$(function () {
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "Parqueadero/GetPartial", "GET", null, "printPartialModal", { title: "Crear zona común", url: urlBase + "Parqueadero/Insert", metod: "GET", func: "successInsertParking", modalLarge: true });
    });
    setEventEdit();
});

function setFileLoad(prev) {
    //fileDropZone($("#dropzoneElement"), "/Parqueadero/saveFile", "Parqueadero/RemoveFile", "image/*", $("#Imagen"), prev, "/Images/Parking/");
    fileDropZone($("#dropzoneElement"), urlBase + "Parqueadero/saveFile", "Parqueadero/RemoveFile", "image/*", $("#Imagen"), prev, urlBase + "Images/Parking/");
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
        EjecutarAjax(urlBase + "Parqueadero/GetById", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar zona común", url: urlBase + "Parqueadero/Update", metod: "GET", func: "successUpdateParking", modalLarge: true });
    });
    $(".lnkDisable").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea inactivar este registro?", "EliminarParqueadero", $(this).data("id"));
        //if (confirm("Está seguro que desea eliminar este punto de parqueadero?"))
        //    EjecutarAjax(urlBase + "Parqueadero/Delete", "GET", { id: $(this).data("id") }, "successDeleteParking", null);
    });
}
function successInsertParking(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Parqueadero/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Su operación fue exitosa.");
    }
}
function successDeleteParking(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Parqueadero/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        mostrarAlerta("Registro inactivado con éxito.");
    }
}
function successUpdateParking(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Parqueadero/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Edición exitosa.")
    }
}
function EliminarParqueadero(Id) {
    EjecutarAjax(urlBase + "Parqueadero/Delete", "GET", { id: Id }, "successDeleteParking", null);    
}