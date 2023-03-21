$(function () {
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "GestionMantenimiento/GetPartial", "GET", null, "printPartialModal", { title: "Crear mantenimiento", url: urlBase + "GestionMantenimiento/ActualizarMantenimiento", metod: "GET", func: "successInsert" });
    });
    setEventEdit();
});
function setEventEdit() {
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "GestionMantenimiento/Obtener", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar Mantenimiento", url: urlBase + "GestionMantenimiento/ActualizarMantenimiento", metod: "GET", func: "successUpdate" });
        
    });
    $(".lnkDelete").click(function () {
        if (confirm("Está seguro que desea eliminar este tipo de brazalete?"))
            EjecutarAjax(urlBase + "GestionMantenimiento/Delete", "GET", { id: $(this).data("id") }, "successDelete", null);
    });
}
function successInsert(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "GestionMantenimiento/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("El Mantenimiento fue generado con éxito.");
    }
}
function successDelete(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "TipoBrazalete/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        mostrarAlerta("El tipo de brazalete fue eliminado con éxito.");
    }
}
function successUpdate(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "GestionMantenimiento/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("El mantenimiento fue modificado con éxito.");
    }
}

function selectcheck(ctrl) {
    
    var checkedVals = $('.flat:checkbox:checked').map(function () {
        return this.value;
    }).get();
    $("#hdf_atraccionesSeleccionadas").val(checkedVals.join(","))
}


function selectChanged(item) {
    EjecutarAjax(urlBase + "GestionMantenimiento/GetPartialMantenimiento", "GET", { id: item.value }, "printPartial", { div: "#listMantenimiento" });
    //alert("Item " + item.text + " (valor es: " + item.value + ")");
}

