$(function () {
    $("#div_message_error").hide();
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "CargueBrazalete/GetPartial", "GET", null, "printPartialModal", { title: "Cargar Boleteria", url: urlBase + "CargueBrazalete/Insert", metod: "GET", func: "successInsertCargueBrazalate", DatePicker: true });
    });
    setEventEdit();    
});
function setEventEdit() {
    EstablecerToolTipIconos();  
    $(".lnkInvalidate").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea anular este cargue de boleteria?", "InactivarCargueBoleterias", $(this).data("id"));        
    });

}
function successInsertCargueBrazalate(rta) {

    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "CargueBrazalete/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Operación realizada con éxito.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }    
}
function successAnularCargueBoleteria(rta) {

    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "CargueBrazalete/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        mostrarAlerta("El cargue de boleteria fue anulado con éxito.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }    
}

function InactivarCargueBoleterias(Id) {
    EjecutarAjax(urlBase + "CargueBrazalete/Update", "GET", { id: Id }, "successAnularCargueBoleteria", null);
}
