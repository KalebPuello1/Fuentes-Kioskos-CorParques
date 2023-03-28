$(function () {


    $("#DDL_Usuario").change(function () {
        ConsultaCierreElementos($(this).val());
    });

    asignarSelect2();


});
//GALD: Se adiciona para cancelar el cierre supervisor
function Cancelar() {

    window.location = urlBase + "CierrePunto/IndexSupervisor";

}


//RDSH: Consulta el alistamiento hecho por el taquillero.
function ConsultaCierreElementos(IdTaquillero)
{
    if (IdTaquillero != "") {

        EjecutarAjax(urlBase + "CierrePunto/ObtenerAperturaElementosUsuario", "GET", { IdUsuario: parseInt(IdTaquillero), Opcion:1 }, "CargarPagina", null);

    } else {
        $('#ContenidoElementos').html("");
    }

}

//RDSH: carga el alismiento del taquillero en la vista parcial _DetalleElementos_Supervisor
function CargarPagina(data) {
    $('#ContenidoElementos').html(data);

    if (data != "") {
        var DetalleInventario;

        $("#btnSave").click(function () {
            if (!validarFormulario("frmcierreelementos *")) {
                return false;
            }
            MostrarConfirm("Importante", "¿Realmente desea guardar la información ingresada?", "MostrarLogin");
        });

        $("#btnCancelar").click(function () {
            MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "Cancelar", "");
        });
    } else {
        MostrarMensaje("Importante!", "El taquillero seleccionado no ha realizado el alistamiento de cierre de punto.", "warning");
    }
    

}

//RDSH: Guarda en base de datos la información registrada.
function GuardarCierre() {

   ActualizarEstado();
   EjecutarAjax(urlBase + "CierrePunto/ActualizarElementosSupervisor", "GET", ObtenerObjeto("frmcierreelementos *"), "ActualizacionOk", null);
    

}

//RDSH: Muestra mensaje de confirmación de guardado de la información.
function ActualizacionOk(rta) {
    if (rta.Correcto) {
        MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", "Home/Index", "success");
        //EjecutarAjax(urlBase + "Apertura/EditAperturaBase", "GET", null, "successfull", null);
        //mostrarMensaje("Importante", "Operación realizada con éxito.");
    }
    else {
        MostrarMensaje("Importante", rta.Mensaje, "error");
    }
}

//RDSH: Muestra la pantalla de login para ingresar la contraseña del taquillero.
function MostrarLogin() {
    EjecutarAjax(urlBase + "CierrePunto/ObtenerLogin", "GET", null, "printPartialModal", { title: "Confirmación taquillero", hidesave: true, modalLarge: false });
}


///RDSH: Valida las credenciales digitadas por parte del taquillero.
function Login(password, observaciones) {

    var IdUsuario = 0;

    IdUsuario = $("#DDL_Usuario").val();
    strObservacionesSupervisor = observaciones;
    $("[name='[0].Observacion']").val(strObservacionesSupervisor);
    EjecutarAjax(urlBase + "Cuenta/ValidarPassword", "GET", {
        idUsuario: IdUsuario, password: password
    }, "respuestaLogin", null);

}

//RDSH: Captura la respuesta de autenticacion.
function respuestaLogin(data) {
        
    if (data.Correcto) {
        GuardarCierre();
        cerrarModal("modalCRUD");
    } else {
        MostrarMensaje("Importante", "Contraseña incorrecta", "error");
    }

}

function CancelarLogin() {
    cerrarModal('modalCRUD');
}

//Actualiza la propiedad estado.
function ActualizarEstado()
{
    var strControl = "";
    var strControlEstado = "";
    var strEstado = "";

    $(".comboestado").each(function (index, element) {
        strControl = "[name='[" + index + "].IdEstadoSupervisor'";
        strControlEstado = "[" + index + "].Estado"
        strEstado = $(strControl).find("option:selected").text();
        //$("input'[name=" + strControlEstado + "]'").val(strEstado);
        $("[name='" + strControlEstado + "']").val(strEstado)
    });
}
