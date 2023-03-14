$(function () {
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "TypeVehiclePerParking/GetPartial", "GET", null, "printPartialModal", { title: "Crear relación Tipo vehículo por parqueadero", url: urlBase + "TypeVehiclePerParking/Insert", metod: "GET", func: "successInsertTypeVehiclePerParking" });
    });
    setEventEdit();
});
function setEventEdit()
{
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "TypeVehiclePerParking/GetById", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar parámetro", url: urlBase + "TypeVehiclePerParking/Update", metod: "GET", func: "successUpdateTypeVehiclePerParking" });
    });
    $(".lnkDelete").click(function () {
        if (confirm("¿Está seguro que desea eliminar este parámetro?"))
            EjecutarAjax(urlBase + "TypeVehiclePerParking/Delete", "GET", { id: $(this).data("id") }, "successDeleteTypeVehiclePerParking", null);
    });
}
function successInsertTypeVehiclePerParking(rta)
{
    if (rta.Correcto)
    {
        EjecutarAjax(urlBase + "TypeVehiclePerParking/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("La relación tipo vehículo parqueadero fue creada con éxito.");
    }
}
function successDeleteTypeVehiclePerParking(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "TypeVehiclePerParking/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        mostrarAlerta("La relación tipo vehiculo parqueadero fue eliminada con éxito.");
    }
}
function successUpdateTypeVehiclePerParking(rta)
{
    if (rta.Correcto)
    {
        EjecutarAjax(urlBase + "TypeVehiclePerParking/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("La relacion tipo vehiculo parqueadero fue modificada con éxito.")
    }
}