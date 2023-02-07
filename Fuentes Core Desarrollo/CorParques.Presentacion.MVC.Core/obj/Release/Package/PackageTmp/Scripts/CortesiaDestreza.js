$(function () {
    $("#div_message_error").hide();   
    setEventEdit();    
});
function setEventEdit() {
    EstablecerToolTipIconos();
    
    $(".lnkGenerarCortesia").click(function () {

        var NombreAtraccion = $("#div_NombreAtraccion_" + $(this).data("id")).html();
        MostrarConfirm("Importante", "¿Está seguro que desea entregar la cortesía de " + NombreAtraccion + "?", "AceptarEntrega", $(this).data("id"));
        //if (confirm("¿Desea generar esta cortesia?"))
        //    EjecutarAjax(urlBase + "CortesiaDestreza/GenerarCortesia", "GET", { IdAtraccion: $(this).data("id") }, "successGenerarCortesia", null);
    });
}


function successGenerarCortesia(rta) {    
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "CortesiaDestreza/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        MostrarMensaje("Importante", "Cortesía generada con éxito.", "success");
    }
    else {        
        mostrarAlerta("Fallo al generar la cortesía", rta.Mensaje);
    }
}

function AceptarEntrega(Id)
{
    EjecutarAjax(urlBase + "CortesiaDestreza/GenerarCortesia", "GET", { IdAtraccion: Id }, "successGenerarCortesia", null);
}

