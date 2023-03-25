
$(function () {
    setEventEdit();
    setEventEdit2();
});


function setFileLoad(prev) {
    fileDropZone($("#dropzoneElement"), urlBase + "Productos/SaveFile", "Productos/RemoveFile", "image/*", $("#Imagen"), prev, urlBase + "Images/Productos/");
    $(".dateTime").datetimepicker({
        format: 'HH:mm',
    });
}

function InitPartialCreate(dataPuntos) {


    setAutocompleteCategory()
    tagsAutocomplete($('#tagsPuntos'), $('#puntosAutocomplete'), $("#hdListPuntos"), dataPuntosParcial, true)

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
        EjecutarAjax(urlBase + "Productos/GetByIdPuntosEntrega", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar producto", url: urlBase + "Productos/UpdatePuntosEntrega", metod: "GET", func: "successUpdate2" });
    });
}
function setEventEdit2() {
    $(".lnkEdit2").click(function () {
        EjecutarAjax(urlBase + "Productos/GetByIdPuntosEntrega", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar producto", url: urlBase + "Productos/UpdatePuntosEntrega", metod: "GET", func: "successUpdate2" });

    });
}

function successUpdate(rta) {

    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Productos/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("El producto fue modificado con éxito.")
    }
}

function successUpdate2(rta) {

    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Productos/GetListPuntosEntrega", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("El producto fue modificado con éxito.")
    }
}
