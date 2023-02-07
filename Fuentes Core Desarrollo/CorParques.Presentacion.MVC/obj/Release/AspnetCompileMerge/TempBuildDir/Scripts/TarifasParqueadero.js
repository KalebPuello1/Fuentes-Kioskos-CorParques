$(function () {
    $("#div_message_error").hide();
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "TarifasParqueadero/GetPartial", "GET", null, "printPartialModal", { title: "Crear tarifa parqueadero", url: urlBase + "TarifasParqueadero/Insert", metod: "GET", func: "successInsert" });
    });
    setEventEdit();    
});
function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "TarifasParqueadero/Obtener", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar tarifa parqueadero", url: urlBase + "TarifasParqueadero/Update", metod: "GET", func: "successUpdate" });
    });

    $(".lnkDisable").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea inactivar esta tarifa parqueadero?", "InactivarTarifaParqueadero", $(this).data("id"));
        //if (confirm("¿Está seguro que desea inactivar esta tarifa parqueadero?"))
        //    EjecutarAjax(urlBase + "TarifasParqueadero/UpdateEstado", "GET", { id: $(this).data("id") }, "successDisable", null);
    });

    $(".lnkDetail").click(function () {
        EjecutarAjax(urlBase + "TarifasParqueadero/Detalle", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Detalle tarifa parqueadero", hidesave: "S", showreturn: "S" });
    });
    //EsteblecerFormatoMoneda();
}
function EsteblecerFormatoMoneda()
{
    QuitarFormatoMoneda();
    $(".formato_moneda").each(function (index, element) {
        element.innerText = FormatoMoneda(parseInt(element.innerText));
    });
}
function successInsert(rta) {

    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "TarifasParqueadero/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Su operación fue exitosa.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}

function successUpdate(rta) {
    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "TarifasParqueadero/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Edición exitosa.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}

function successDisable(rta) {
    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "TarifasParqueadero/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("La tarifa fue inactivada con éxito.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}

function InactivarTarifaParqueadero(Id)
{
    EjecutarAjax(urlBase + "TarifasParqueadero/UpdateEstado", "GET", { id: Id}, "successDisable", null);
}
