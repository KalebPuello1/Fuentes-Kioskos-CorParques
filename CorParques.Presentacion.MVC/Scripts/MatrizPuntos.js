//Powered by RDSH
$(function () {
    $("#div_message_error").hide();
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "MatrizPuntos/GetPartial", "GET", null, "printPartialModal", { title: "Crear relacion producto puntos", url: urlBase + "MatrizPuntos/Insert", metod: "GET", func: "successInsert" });
    });
    setEventEdit();
});
function setEventEdit() {
    EstablecerToolTipIconos();
  
    $(".lnkDisable").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea eliminar este registro?", "EliminarProducto", $(this).data("id"));
    });
}
function successInsert(rta) {

    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "MatrizPuntos/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Su operación fue exitosa.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}
function successDelete(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "MatrizPuntos/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        MostrarMensaje("Importante", "Registro eliminado con éxito.", "success");
    } else {
        MostrarMensaje("Importante", "No fue posible eliminar el registro.", "error");
    }

}
function EliminarProducto(Id) {
    EjecutarAjax(urlBase + "MatrizPuntos/Delete", "GET", { id: Id }, "successDelete", null);
}
