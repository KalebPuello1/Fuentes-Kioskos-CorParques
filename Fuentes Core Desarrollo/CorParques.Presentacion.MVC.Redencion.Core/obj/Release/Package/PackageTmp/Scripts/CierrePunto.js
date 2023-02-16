$(function () {

    var DetalleInventario;

    $("#btnSave").click(function () {
        if (!validarFormulario("frmcierreelementos *")) {            
            return false;
        }
        GuardarCierre();
    });
    $("#btnCancelar").click(function () {
        MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "Cancelar", "");
    });

});


function Cancelar() {

    window.location = urlBase + "CierrePunto/Index";
    
}


function GuardarCierre() {

    
    //$("#frmcierreelementos").submit();
    EjecutarAjax(urlBase + "CierrePunto/GuardarCierrePuntos", "GET", ObtenerObjeto("frmcierreelementos *"), "SucessSave", null);
    //EjecutarAjax(urlBase + "Apertura/Insert", "POST", element, "successInsertApertura", null);    

}


function SucessSave(rta) {
    if (rta.Correcto) {
        MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", "Home/Index", "success");
        //EjecutarAjax(urlBase + "Apertura/EditAperturaBase", "GET", null, "successfull", null);
        //mostrarMensaje("Importante", "Operación realizada con éxito.");
    }
    else {
        mostrarMensaje("Error al guardar", rta.Mensaje);
    }
}