$(function () {


    $("#DDL_Usuario").change(function () {
        ConsultaCierreElementos($(this).val());
    });

    asignarSelect2();


});

//RDSH: Consulta el alistamiento hecho por el supervisor.
function ConsultaCierreElementos(IdSupervisor) {
    if (IdSupervisor != "") {

        EjecutarAjax(urlBase + "CierrePunto/ObtenerAperturaElementosUsuario", "GET", { IdUsuario: parseInt(IdSupervisor), Opcion: 2 }, "CargarPagina", null);

    } else {
        $('#ContenidoElementos').html("");
    }

}

//RDSH: carga el alismiento del supervisor en la vista parcial _DetalleElementos_Nido
function CargarPagina(data) {
    $('#ContenidoElementos').html(data);

    if (data.length > 10) {
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
        MostrarMensaje("Importante!", "El supervisor seleccionado no tiene ningún alistamiento de cierre de punto.", "warning");
    }

}

//RDSH: Guarda en base de datos la información registrada.
function GuardarCierre() {
    ActualizarEstado();
    EjecutarAjax(urlBase + "CierrePunto/ActualizarElementosNido", "GET", ObtenerObjeto("frmcierreelementos *"), "ActualizacionOk", null);
}

//RDSH: Muestra mensaje de confirmación de guardado de la información.
function ActualizacionOk(rta) {
    if (rta.Correcto) {
        MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", "Home/Index", "success");
    }
    else {
        MostrarMensaje("Importante", rta.Mensaje, "error");
    }
}

//RDSH: Muestra la pantalla de login para ingresar la contraseña del taquillero.
function MostrarLogin() {
    EjecutarAjax(urlBase + "CierrePunto/ObtenerLogin", "GET", null, "printPartialModal", { title: "Confirmación supervisor", hidesave: true, modalLarge: false });
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

//GALD: Se adiciona para cancelar el cierre supervisor
function Cancelar() {

    window.location = urlBase + "CierrePunto/IndexNido";

}

function CancelarLogin() {
    cerrarModal('modalCRUD');
}

//Actualiza la propiedad estado.
function ActualizarEstado() {
    var strControl = "";
    var strControlEstado = "";
    var strEstado = "";

    $(".comboestado").each(function (index, element) {
        strControl = "[name='[" + index + "].IdEstadoNido'";
        strControlEstado = "[" + index + "].Estado"
        strEstado = $(strControl).find("option:selected").text();        
        $("[name='" + strControlEstado + "']").val(strEstado)
    });
}
