
$(function () {
    setEventEdit();
});


function setFileLoad(prev) {
    fileDropZone($("#dropzoneElement"), urlBase + "Productos/SaveFile", "Productos/RemoveFile", "image/*", $("#Imagen"), prev, urlBase + "Images/Productos/");
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
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "Productos/GetById", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar producto", url: urlBase + "Productos/Update", metod: "GET", func: "successUpdate" });
    });
}

function successUpdate(rta) {
    
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Productos/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("El producto fue modificado con éxito.")
    }
}
