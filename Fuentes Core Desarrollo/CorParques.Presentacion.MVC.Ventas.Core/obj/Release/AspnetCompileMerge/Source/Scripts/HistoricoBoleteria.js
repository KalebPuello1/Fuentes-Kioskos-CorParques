var idBoleta = 0;

$(function () {

    $('#codBarra').keypress(function (e) {
        
        if (e.which == 13 && $(this).val().length > 0) {
            

            EjecutarAjax(urlBase + "HistoricoBoleteria/ObtenerBoleta", "GET", { consecutivo: $(this).val() }, "printPartial", { div: "#vwDetalleBoleta", func: "SetEdit" });

            //EjecutarAjax(urlBase + "Pos/ObtenerBrazalete", "GET", { CodBarra: $(this).val() }, "successObtenerBrazalete", null);
            //$(this).val("");
            return false;
        }
    });



    SetEdit();
});

function SetEdit() {
    $("#btnBloquear").click(function () {
        MostrarConfirm("Importante!", "¿Está seguro de cancelar esta boleta? ", "Cancelar", "");

    })
}


function Cancelar() {
    EjecutarAjax(urlBase + "HistoricoBoleteria/BloquearBoleta", "GET", { IdBoleta: idBoleta }, "CancelarBoleta", null);
}

function CancelarBoleta(rta) {
    if (rta.Correcto) {
        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "HistoricoBoleteria/Index", "success");
        //EjecutarAjax(urlBase + "Apertura/EditAperturaBase", "GET", null, "successfull", null);
        //MostrarMensaje("Importante", "Operación realizada con éxito.");
    }
    else {
        MostrarMensaje("Fallo al guardar", rta.Mensaje);
    }
}