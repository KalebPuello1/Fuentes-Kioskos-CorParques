/// <reference path="D:\TFS\Corparques\SRC\Softtek.CorParques\CorParques.Presentacion.MVC.Core\Vendors/jquery/dist/jquery-1.11.1.min.js" />

//carga una vez la pagina cargue el front
$(function () {
    Inicializar();
});

//metodo usado como costructor del .js
function Inicializar()
{
    setTimePicker();

    //boton Guardar
    $("#btnSalvarRegistro").click(function () {
        if (!validarFormulario("frmRegistroFallas *")) {
            return false;
        }
        else{
            //var d = new Date();
            //var mm = d.getMinutes();
            //var hh = d.getHours();

            //if (mm < 10) {
            //    mm = "0" + mm;
            //}

            //if (hh < 10) {
            //    hh = "0" + hh;
            //}

            //var tim = hh + ":" + mm;

            //if ($("#HoraInicio").val() > tim) {
            if ($("#HoraInicio").val() > $("#horaRespuesta").val()) {
                var tm = $("#HoraInicio").val();
                $("#HoraInicio").val("");
                validarFormulario("frmRegistroFallas *");
                $("#HoraInicio").val(tm);
                $("#HoraInicio").attr("data-mensajeerror", "Hora incorrecta");
                //MostrarMensaje("Importante", "La hora no debe ser superior a la hora actual " + tim, "error");
                MostrarMensaje("Importante", "La hora no debe ser superior a la hora actual " + $("#horaRespuesta").val(), "error");
                return false;
            }

            if ($("#horaLlegadaTec").val() > $("#horaRespuesta").val() && $("#horaLlegadaTec").val() < $("#HoraInicio").val()) {
                var tm = $("#horaLlegadaTec").val();
                $("#horaLlegadaTec").val("");
                validarFormulario("frmRegistroFallas *");
                $("#horaLlegadaTec").val(tm);
                $("#horaLlegadaTec").attr("data-mensajeerror", "Hora incorrecta");
                MostrarMensaje("Importante", "La hora no está en el rango de hora de la falla y solución", "error");
                return false;
            }

            MostrarConfirm("Importante!", "¿Está seguro de guardar el registro? ", "aceptarConfirmacion", "");
        }
    });

    //boton Cancelar
    $("#btnCancelarRegistro").click(function () {
        MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "cancelarConfirmacion", "");
    });

    //caja de texto que consulta el usuario
    $("#txtTecnico").change(function () {
        var usuario = $(this).val();
        $("#txtTecnico").val("");
        EjecutarAjax(urlBase + "RegistroFallas/consultarUsuario", "GET", {nombreUsuario : usuario}, "mostrarNombre", usuario);
    });

    //se aplica el efecto de busqueda en la lista desplegable
    $("#idPunto").select2({
        placeholder: "* Seleccione el punto"
    });
    $("#idArea").select2({
        placeholder: "* Seleccione el área"
    });
    $("#idOrdenFalla").select2({
        placeholder: "* Seleccione el orden falla"
    });
}


function mostrarNombre(datos, params)
{
    if (datos == "") {
        MostrarMensaje("Error", "El usuario no tiene un nombre asociado.", "error");
    }
    else {
        $("#hdnTecnico").val(params);
        $("#txtTecnico").val(datos);
    }
}

//llamado desde boton cancelar
function cancelarConfirmacion() {
    window.location = urlBase + "RegistroFallas";
}

//llamado desde boton guardar
function aceptarConfirmacion() {
    var objeto = $("#frmRegistroFallas").serializeArray();
    
    EjecutarAjax(urlBase + "RegistroFallas/Salvar", "GET", objeto, "mostrarMensajeRegistroFallos", null);
}

//datos {titulo, cuerpo}
function mostrarMensajeRegistroFallos(datos)
{
    if (datos.Elemento) {
        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "RegistroFallas", "success");
    }
    else {
        MostrarMensaje("Error", "Se ha presentado un inconveniente, intentelo nuevamente.", "error");
    }
    
}
