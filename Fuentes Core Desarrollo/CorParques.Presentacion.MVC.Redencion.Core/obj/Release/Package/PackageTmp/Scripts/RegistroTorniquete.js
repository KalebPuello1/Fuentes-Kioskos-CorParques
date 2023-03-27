//Powered by RDSH

$(function () {

    Inicializar();

});


function Inicializar()
{
    $("#btnAceptar").click(function () {
        if (validarFormulario("frmTorniquete"))
        {
            MostrarConfirm("Importante", "¿Esta seguro que desea realizar el registro del torniquete? Esta acción retirará a todos los auxiliares asociados a la atracción.", "GuardarCierreTorniquete", "")
        }       

    });

}

function GuardarCierreTorniquete()
{
    var IdRegistroTorniquete = $("#IdRegistroTorniquete").val();
    var obj = ObtenerObjeto("frmTorniquete");

    if (IdRegistroTorniquete == 0) {
        EjecutarAjax(urlBase + "RegistroTorniquete/Insert", "GET", obj, "successRegistroTorniquete", null);
    }
    else {
        EjecutarAjax(urlBase + "RegistroTorniquete/Update", "GET", obj, "successRegistroTorniquete", null);
    }    

}

function successRegistroTorniquete()
{
    MostrarMensajeRedireccion("Importante", "Registro de torniquete guardado exitosamente.", "Home/Index", "success");
}