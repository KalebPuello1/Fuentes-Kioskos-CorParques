//Powered by RDSH

$(function () {

    $("#IdIndicador").change(function () {
        ConsultarPlaneacion();
    });

    $('#FechaTexto').on('dp.change', function (e) {
        ConsultarPlaneacion();
    });

})

function Inicializar() {
        

    $(".decimales").keypress(function () {
        return validateFloatKeyPress(this, event);
    });

    $("#btnAceptar").click(function () {
        PreGuardar();
    });

    $("#btnCancelar").click(function () {
        PreCancelar()
    });
}


//Se valida la informacion digitada y se solicita confirmacion de usuario para guardar.
function PreGuardar() {
    if (validarFormulario("frmPlaneacion *")) {
        QuitarTooltip();
        MostrarConfirm("Importante", "¿Realmente desea guardar la información registrada?", "GuardarPlaneacion");
    }
}

//Rutina de guardado de la informacion.
function GuardarPlaneacion() {
    
    var _obj = ObtenerObjeto("frmCreacionPlaneacion *")

    EjecutarAjax(urlBase + "Planeacion/Insertar", "GET", _obj, "InsercionPlaneacion", null);
    
        //EjecutarAjax(urlBase + "Planeacion/Actualizar", "GET", ObtenerObjeto("frmCreacionPlaneacion *"), "InsercionPlaneacion", null);
}

//Resultado insercion.
function InsercionPlaneacion(rta) {
    if (rta.Correcto) {
        MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", "Planeacion", "success");
    }
    else {
        MostrarMensaje("Importante", rta.Mensaje, "error");
    }
}

//Cancela la planeacion.
function PreCancelar() {
    MostrarConfirm("Importante", "¿Realmente desea cancelar?", "RedireccionCancelar");
}

function RedireccionCancelar() {
    window.location = urlBase + "Planeacion";
}



function ConsultarPlaneacion() {
    var intIdIndicador = $("#IdIndicador").val();
    var strFecha = $("#FechaTexto").val();

    if (intIdIndicador != '' && strFecha != '') {
        EjecutarAjax(urlBase + "Planeacion/ConsultarPlaneacion", "GET", { IdIndicador: intIdIndicador, Fecha: strFecha }, "CargarPlaneacion", null);
    }
    else {
        $('#divMostrarPlaneacion').html("");
    }
}

function CargarPlaneacion(data) {

    if (data != '') {
        $('#divMostrarPlaneacion').html(data);
        Inicializar();
    } else {
        $('#divMostrarPlaneacion').html("");
    }
}