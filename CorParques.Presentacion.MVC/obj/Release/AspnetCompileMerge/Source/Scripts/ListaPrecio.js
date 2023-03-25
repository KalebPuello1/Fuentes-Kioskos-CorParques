/// <reference path="General.js" />

$(function () {
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "ListaPrecio/GetPartial", "GET", null, "printPartialModal", { title: "Crear Lista de Precios", url: urlBase + "ListaPrecio/Insert", metod: "GET", func: "successInsertListaPrecios", func2: "loadCalendar" });
    });
    setEventEdit();
    setEventDelete();
});

function setEventEdit() {
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "ListaPrecio/GetById", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar Lista de Precios", url: urlBase + "ListaPrecio/Update", metod: "GET", func: "successUpdateListaPrecios", func2: "loadCalendar" });
    });
}

function setEventDelete() {
    $(".lnkDelete").click(function () {
        if (confirm("¿Está seguro que desea eliminar esta lista de precios?"))
        {
            EjecutarAjax(urlBase + "ListaPrecio/Delete", "GET", { id: $(this).data("id") }, "successDeleteListaPrecios", null);
        }        
    });
}

function successInsertListaPrecios(rta) {
    
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "ListaPrecio/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("La lista de precios fue creado con éxito.");
    }
}

function successUpdateListaPrecios(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "ListaPrecio/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("La lista de precios fue modificado con éxito.")
    }
}

function successDeleteListaPrecios() {
    EjecutarAjax(urlBase + "ListaPrecio/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
    mostrarAlerta("La lista de precios fue eliminado con éxito.")
}
