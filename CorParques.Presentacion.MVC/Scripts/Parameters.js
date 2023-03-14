$(function () {
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "Parameters/GetPartial", "GET", null, "printPartialModal", { title: "Crear parámetro", url: urlBase + "Parameters/Insert", metod: "GET", func: "successInsertParameter" });
    });
    setEventEdit();
});
function setEventEdit()
{
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "Parameters/GetById", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar parámetro", url: urlBase + "Parameters/Update", metod: "GET", func: "successUpdateParameter" });
    });
    $(".lnkDelete").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea eliminar este parámetro?", "InactivarParametro", $(this).data("id"));
        //if (confirm("Está seguro que desea eliminar este parámetro?"))
        //    EjecutarAjax(urlBase + "Parameters/Delete", "GET", { id: $(this).data("id") }, "successDeleteParameter", null);
    });
}
function successInsertParameter(rta)
{
    if (rta.Correcto)
    {
        EjecutarAjax(urlBase + "Parameters/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("El parámetro global fue creado con éxito.");
    }
}
function successDeleteParameter(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Parameters/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        mostrarAlerta("El parámetro global fue eliminado con éxito.");
    }
}
function successUpdateParameter(rta)
{
    if (rta.Correcto)
    {
        EjecutarAjax(urlBase + "Parameters/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("El parámetro global fue modificado con éxito.")
    }
}
function InactivarParametro(Id) {
    EjecutarAjax(urlBase + "Parameters/Delete", "GET", { id: Id}, "successDeleteParameter", null);
}