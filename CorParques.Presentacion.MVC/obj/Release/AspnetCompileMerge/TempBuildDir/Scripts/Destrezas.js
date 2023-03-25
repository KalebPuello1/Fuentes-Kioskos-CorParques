//Powered by RDSH.
$(function () {
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "Destrezas/GetPartial", "GET", null, "printPartialModal", { title: "Crear destreza", url: urlBase + "Puntos/Insert", metod: "GET", func: "successInsertDestrezas", modalLarge: true });
    });
    setEventEdit();
});

function setFileLoad(prev) {
    //fileDropZone($("#dropzoneElement"), "/Destrezas/saveFile", "Destrezas/RemoveFile", "image/*", $("#Imagen"), prev, "/Images/Destrezas/");
    fileDropZone($("#dropzoneElement"), urlBase + "Destrezas/saveFile", "Destrezas/RemoveFile", "image/*", $("#Imagen"), prev, urlBase + "Images/Destrezas/");
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
            $("#DescripcionCierre").val("");
        }
    });
}
function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkEdit").click(function () {        
        EjecutarAjax(urlBase + "Destrezas/GetById", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar punto destreza", url: urlBase + "Destrezas/Update", metod: "GET", func: "successUpdateDestrezas", modalLarge: true });       
    });
    $(".lnkDisable").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea inactivar este registro?", "EliminarDestreza", $(this).data("id"));
        //if (confirm("Está seguro que desea eliminar esta destreza?"))
        //    EjecutarAjax(urlBase + "Destrezas/Delete", "GET", { id: $(this).data("id") }, "successDeleteDestrezas", null);
    });
}
function successInsertDestrezas(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Destrezas/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Su operación fue exitosa.");
    }
}
function successDeleteDestrezas(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Destrezas/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        mostrarAlerta("Registro inactivado con éxito.");
    }
}
function successUpdateDestrezas(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Destrezas/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Edición exitosa.");
    }
}

function MostrarMotivoCierreEdicion() {

    var IdEstado = $("#EstadoId").val();
    
    $("#DescripcionCierre").parent().parent().hide();
    $("#DescripcionCierre").removeClass("required");
    if (IdEstado == "4") {
        $("#DescripcionCierre").parent().parent().show();
        $("#DescripcionCierre").addClass("required");
    }

}

function EliminarDestreza(Id)
{
    EjecutarAjax(urlBase + "Destrezas/Delete", "GET", { id: Id }, "successDeleteDestrezas", null);
}
