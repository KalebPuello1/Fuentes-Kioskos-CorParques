//Powered by RDSH
var blnEstaEditando = false;
var blnConsultarPedido = true;

$(function () {

    $('#txtFechaReserva').on('dp.change', function (e) {
        ConsultarReservas($(this).val());
    });

    setEventEdit();
});

//Consulta las reservas que se han hecho en la fecha seleccionada.
function ConsultarReservas(strFecha) {
    if (strFecha != "") {
        EjecutarAjax(urlBase + "ReservaEspacios/BuscarReservas", "GET", { FechaReserva: FormatoFecha(strFecha) }, "CargarPagina", null);
    } else {
        $('#ListaReservas').html("");
    }
}

//Muestra la pagina con las reservas que se han realizado.
function CargarPagina(data) {
    if (data != '') {
        $('#ListaReservas').html(data);
        $('#ListaReservas').find("table").DataTable();
        setEventEdit();
    } else {
        $('#ListaReservas').html("");
        MostrarMensaje("Importante", "No se encontraron reservas en la fecha seleccionada.", "warning");
    }

}

//Recibe fecha en formato dd/mm/yyyy, retorna fecha en formato yyyy-mm-dd
function FormatoFecha(strFecha) {
    var strFechaRetorno;

    strFechaRetorno = strFecha.split('/');

    if (strFechaRetorno.length == 3) {
        return strFechaRetorno[2] + '-' + strFechaRetorno[1] + '-' + strFechaRetorno[0];
    } else {
        return "";
    }

}

function Inicializar() {
    loadCalendar();
    setTimePicker();
    $("[name=IdTipoReserva]").first().addClass("seleccionado");
    $("#IdTipoReserva").val($("[name=IdTipoReserva]").first().attr("data-id"));

    $("#IdTipoEspacio").change(function () {
        ConsultarEspacios($(this).val());
    });

    $("#btnGuardar").click(function () {
        PreGuardar();
    });

    $("#btnCancelar").click(function () {
        CancelarReserva();
    });


    $("#CodigoSapPedido").focusout(function () {
        if ($(this).val() == '')
        {
            blnConsultarPedido = true;
            OcultarYLimpiarControles(true);
        }
    });

    $("#CodigoSapPedido").on('change', function (e) {
        if (blnConsultarPedido)
            ConsultarPedido($(this).val());
        else
            blnConsultarPedido = true;
    });

    $("#CodigoSapPedido").keypress(function (event) {
        if (event.which == 13) {
            if ($(this).val() == '') {
                blnConsultarPedido = true;
                OcultarYLimpiarControles(true);
            }
            else {
                blnConsultarPedido = false;
                ConsultarPedido($(this).val());
            }
        }
    });

    $("#container_codigosap").on('keydown', '#CodigoSapPedido', function (e) {
        var keyCode = e.keyCode || e.which;

        if (keyCode == 9) {
            if (blnConsultarPedido == true)
            {
                e.preventDefault();
                blnConsultarPedido = false;
                ConsultarPedido($(this).val());
            }                       
        }
    });

    $('#FechaReserva').on('dp.change', function (e) {
        if ($(this).val() == undefined || $(this).val() == '')
            ValidarFechaReservaConsultaPedido(false);
        else
            ValidarFechaReservaConsultaPedido(true);

        $("#hdf_TienePedido").val('0');
        $("#DetallePedido").html('');
        $("#CodigoSapPedido").val('');
        OcultarYLimpiarControles(true);
        blnConsultarPedido = true;
    });

}

//Consulta los espacios por tipo de espacio.
function ConsultarEspacios(Id) {
    if (Id != '') {
        EjecutarAjax(urlBase + "ReservaEspacios/ConsultarEspacios", "GET", { IdTipoEspacio: Id }, "CargarEspacios", null);
    } else {
        $('#IdEspacio').find('option').remove().end().append('<option value="">Seleccione...</option>').val('');
    }
}

//Carga el combo de espacios.
function CargarEspacios(data) {
    //Se remueven todos los items de la lista de ubicacion y se deja solo la opcion todas.
    $('#IdEspacio').find('option').remove().end().append('<option value="">Seleccione...</option>').val('');

    //Se agregan los items que vienen en la variable data.
    var listitems = "";
    if ($("#IdReservaEspacios").val() == "0") {
        $('#IdEspacio').append('<option value="0">Todos</option>');
    }
    $.each(data, function (key, value) {
        listitems += '<option value=' + value.Id + '>' + value.Nombre + '</option>';
    });
    $('#IdEspacio').append(listitems);
}

//Confirma que se desea guardar la reserva.
function PreGuardar() {
    if (validarFormulario("frm_CrearReserva *")) {
        if (!ValidarHoras() && ValidarSiHayPedido()) {
            QuitarTooltip();
            MostrarConfirm("Importante", "¿Realmente desea guardar la información registrada?", "GuardarReserva");
        }
    }
}

//Guarda la reserva de espacios.
function GuardarReserva() {
    EjecutarAjax(urlBase + "ReservaEspacios/Insertar", "GET", ObtenerObjeto("frm_CrearReserva *"), "InsercionReservaRespuesta", null);
}

//Respuesta de la ejecución del guardado de la reserva.
function InsercionReservaRespuesta(rta) {
    if (rta.Correcto) {
        if (rta.Elemento.length > 0) {
            MostrarMensajeRedireccion("Importante", rta.Elemento, "ReservaEspacios/Crear", "success");
        } else {
            MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", "ReservaEspacios/Crear", "success");
        }
    }
    else {
        MostrarMensaje("Importante", rta.Mensaje, "error");
    }
}

//Cancelar creacion de la reserva de espacios.
function CancelarReserva() {
    MostrarConfirm("Importante", "¿Realmente desea cancelar?", "RedireccionCancelar");
}

function RedireccionCancelar() {
    window.location = urlBase + "ReservaEspacios/Crear";
}

//Consulta el pedido digitado en el input CodigoSapPedido
function ConsultarPedido(strPedido) {

    var FechaReserva = $("#FechaReserva").val();

    if (FechaReserva == undefined || FechaReserva == '') {
        $("#hdf_TienePedido").val('0');
        $("#DetallePedido").html('');
        ValidarFechaReservaConsultaPedido(false);
    }
    else {
        ValidarFechaReservaConsultaPedido(true);
        if (strPedido !== '') {
            EjecutarAjax(urlBase + "ReservaEspacios/ConsultarDetallePedido", "GET", { CodigoSap: strPedido, Fecha: FechaReserva }, "CargarDetallePedido", null);
        } else {
            $("#hdf_TienePedido").val('0');
            $("#DetallePedido").html('');
        }
    }


}

//Carga el detalle del pedido en pantalla.
function CargarDetallePedido(data) {
    if (data != '') {
        $("#DetallePedido").html(data);
        $("#hdf_TienePedido").val('1');
        OcultarYLimpiarControles(false);
        CargarNombreCliente();
    } else {
        $("#hdf_TienePedido").val('0');
        $("#DetallePedido").html('');
        OcultarYLimpiarControles(true);
        $("#CodigoSapPedido").focus();
        MostrarMensaje("Importante", "No se encontró información relacionada al pedido digitado.", "warning");
    }

}

function setEventEdit() {
    $(".lnkDetail").click(function () {
        EjecutarAjax(urlBase + "ReservaEspacios/Detalle", "GET", { NumeroReserva: $(this).data("id") }, "printPartialModal", { title: "Detalle reserva", url: "", metod: "", func: "", hidesave: "Y", showreturn: "Y", modalLarge: true });
    });
}

//Valida que la hora inicial sea mayor que la hora final.
function ValidarHoras() {

    var strHoraInicial = $("#HoraInicio").val();
    var strHoraFin = $("#HoraFin").val();
    var strFecha = $("#FechaReserva").val();
    var strFechaRetorno;
    var blnRetorno = false;

    //strFechaRetorno = strFecha.split('/');
    //strFecha = strFechaRetorno[2] + '/' + strFechaRetorno[1] + '/' + strFechaRetorno[0];

    if (validate_fechaMayorQueHoras(strFecha + ' ' + strHoraFin, strFecha + ' ' + strHoraInicial)) {
        $("#HoraInicio").attr("data-mensajeerror", "Hora inicial debe ser menor que la hora final");
        $("#HoraInicio").addClass("errorValidate");
        $("#HoraFin").attr("data-mensajeerror", "Hora final debe ser mayor que la hora inicial");
        $("#HoraFin").addClass("errorValidate");
        mostrarTooltip();
        blnRetorno = true;
    } else {
        $("#HoraInicio").attr("data-mensajeerror", "");
        $("#HoraInicio").removeClass("errorValidate");
        $("#HoraFin").attr("data-mensajeerror", "");
        $("#HoraFin").removeClass("errorValidate");
        QuitarTooltip();
    }

    return blnRetorno;
}

//-----*****Edicion

function InicializarPreEdicion(Edicion) {
    $("#txtNumeroReserva").keypress(function (event) {
        if (event.which == 13) {
            if (!blnEstaEditando)
                ObtenerReserva($(this).val(), Edicion);
        }
    });

    $("#btnNuevaBusqueda").click(function () {
        $("#txtNumeroReserva").val("");
        $('#DatosReserva').html("");
        $('#btnNuevaBusqueda').hide();
        $("#txtNumeroReserva").attr('readonly', false);
        $("#txtNumeroReserva").focus();
        blnEstaEditando = false;
    });

}

function ObtenerReserva(intNumeroReserva, Edicion) {
    $('#btnNuevaBusqueda').hide();
    if (intNumeroReserva != "") {
        EjecutarAjax(urlBase + "ReservaEspacios/ObtenerReservaEdicion", "GET", { NumeroReserva: intNumeroReserva, Editar: Edicion }, "CargarPaginaEdicion", null);
    } else {
        $('#DatosReserva').html("");
    }
}

function CargarPaginaEdicion(data) {
    if (data != '') {
        $('#DatosReserva').html(data);
        $("#txtNumeroReserva").attr('readonly', true);
        $('#btnNuevaBusqueda').show();
        blnEstaEditando = true;
        InicializarEdicion();
    } else {
        $('#DatosReserva').html("");
        MostrarMensaje("Importante", "No se encontró alguna reserva con el número ingresado.", "warning");
    }

}

function InicializarEdicion() {

    if ($("#Editable").val() == "True" && $("#hdf_Accion").val() == "Editar") {
        loadCalendar();
        setTimePicker();
        setNumeric();
        AsignarClickTipoReserva();       

        $("#IdTipoEspacio").change(function () {
            ConsultarEspacios($(this).val());
        });

        $("#btnGuardar").click(function () {
            PreGuardarEdicion();
        });

        $("#btnCancelar").click(function () {
            CancelarEdicionReserva();
        });

        $("#CodigoSapPedido").focusout(function () {
            if ($(this).val() == '') {
                blnConsultarPedido = true;
                OcultarYLimpiarControles(true);
            }
        });

        $("#CodigoSapPedido").on('change', function (e) {
            if (blnConsultarPedido)
                ConsultarPedido($(this).val());
            else
                blnConsultarPedido = true;
        });

        $("#CodigoSapPedido").keypress(function (event) {
            if (event.which == 13) {
                if ($(this).val() == '') {
                    blnConsultarPedido = true;
                    OcultarYLimpiarControles(true);
                }
                else {
                    blnConsultarPedido = false;
                    ConsultarPedido($(this).val());
                }                
            }
        });

        $("#container_codigosap").on('keydown', '#CodigoSapPedido', function (e) {
            var keyCode = e.keyCode || e.which;

            if (keyCode == 9) {
                if (blnConsultarPedido == true) {
                    e.preventDefault();
                    blnConsultarPedido = false;
                    ConsultarPedido($(this).val());
                }
            }
        });

        $('#FechaReserva').on('dp.change', function (e) {
            if ($(this).val() == undefined || $(this).val() == '')
                ValidarFechaReservaConsultaPedido(false);
            else
                ValidarFechaReservaConsultaPedido(true);

            $("#hdf_TienePedido").val('0');
            $("#DetallePedido").html('');            
            $("#CodigoSapPedido").val('');
            OcultarYLimpiarControles(true);
            blnConsultarPedido = true;
        });

        $("#NombrePersona").focus();

    } else {
        if ($("#hdf_Accion").val() == "Editar") {
            ModoSoloLectura();
            MostrarMensaje("Importante", "Esta reserva no se puede editar porque la fecha de reserva es menor a la fecha actual, podrá verla en modo solo lectura.", "info")
            $("#Editable").focus();
        }
        else if ($("#Editable").val() == "False") {
            ModoSoloLectura();
            MostrarMensaje("Importante", "Esta reserva no se puede eliminar porque la fecha de reserva es menor a la fecha actual.", "info")
            $("#Editable").focus();
        } else {
            //Inicializa pantalla de eliminar.
            ModoSoloLectura();
            $("#NombrePersona").focus();

            $("#btnEliminar").click(function () {
                PreEliminar();
            });

            $("#btnCancelarEliminar").click(function () {
                MostrarConfirm("Importante", "¿Realmente desea cancelar el proceso de eliminación de la reserva?", "RedireccionCancelarEliminacion");
            });
        }
    }

    OcultarYLimpiarControles(false);
    $(".redondo").each(function (i, item) {
        if ($("#IdTipoReserva").val() === $("#" + item.id).attr("data-id"))
        {
            $("#" + item.id).addClass("seleccionado");
            return false;
        }        
    });

}

//Confirma que se desea guardar los ajustes hechos en la reserva.
function PreGuardarEdicion() {
    if (validarFormulario("frm_EditarReserva *")) {
        if (!ValidarHoras() && ValidarSiHayPedido()) {
            QuitarTooltip();
            MostrarConfirm("Importante", "¿Realmente desea guardar la información registrada?", "GuardarEdicionReserva");
        }
    }
}

function GuardarEdicionReserva() {
    EjecutarAjax(urlBase + "ReservaEspacios/Actualizar", "GET", ObtenerObjeto("frm_EditarReserva *"), "ActualizacionReservaRespuesta", null);
}

//Respuesta de la ejecución del guardado de la reserva.
function ActualizacionReservaRespuesta(rta) {
    if (rta.Correcto) {
        if (rta.Elemento.length > 0) {
            MostrarMensajeRedireccion("Importante", rta.Elemento, "ReservaEspacios/Editar", "success");
        } else {
            MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", "ReservaEspacios/Crear", "success");
        }
    }
    else {
        MostrarMensaje("Importante", rta.Mensaje, "error");
    }
}

function CancelarEdicionReserva() {
    MostrarConfirm("Importante", "¿Realmente desea cancelar la edición de la reserva?", "RedireccionCancelarEdicion");
}

function RedireccionCancelarEdicion() {
    window.location = urlBase + "ReservaEspacios/Editar";
}

//-----***** Fin Edicion


//-----***** Inicio Eliminar

function ModoSoloLectura() {
    $("#FechaReserva").prop("disabled", true);
    $("#HoraInicio").prop("disabled", true);
    $("#HoraFin").prop("disabled", true);
    $("#IdTipoEspacio").prop("disabled", true);
    $("#IdEspacio").prop("disabled", true);
    $("#CodigoSapPedido").prop("disabled", true);
    $("#NombrePersona").prop("disabled", true);
    $("#Observaciones").prop("disabled", true);
    $("[name=IdTipoReserva]").prop("disabled", true);

}

//Confirmacion antes de eliminar.
function PreEliminar() {
    MostrarConfirm("Importante", "¿Realmente desea eliminar la reserva?", "EliminarReserva");
}

//Ejecucion de eliminado.
function EliminarReserva() {
    var intReservaEspacio = $("#IdReservaEspacios").val();
    if (intReservaEspacio != "") {
        EjecutarAjax(urlBase + "ReservaEspacios/EliminarReserva", "GET", { IdReserva: intReservaEspacio }, "RespuestaEliminarReserva", null);
    }

}

function RespuestaEliminarReserva(rta) {
    if (rta.Correcto) {
        MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", "ReservaEspacios/Eliminar", "success");
    }
    else {
        MostrarMensaje("Importante", rta.Mensaje, "error");
    }
}

function RedireccionCancelarEliminacion() {
    window.location = urlBase + "ReservaEspacios/Eliminar";
}


//-----***** Fin Eliminar

//Valida si ya se tiene un pedido valido.
function ValidarSiHayPedido() {
    if ($("#hdf_TienePedido").val() == "0") {
        MostrarMensaje("Importante", "Número de pedido no es valido.", "error");
        return false;
    } else {
        return true;
    }

}

function ValidarFechaReservaConsultaPedido(blnValido) {

    if (!blnValido) {
        $("#FechaReserva").attr("data-mensajeerror", "Debe seleccionar la fecha de la reserva");
        $("#FechaReserva").addClass("errorValidate");
        mostrarTooltip();
    } else {
        $("#FechaReserva").attr("data-mensajeerror", "");
        $("#FechaReserva").removeClass("errorValidate");
        QuitarTooltip();
    }

}

//Oculta todos los controles junto con los botones de guardar y cancelar.
function OcultarYLimpiarControles(blnOcultar)
{
    if (blnOcultar == false) {
        $("#div_campos").show();
        $("#div_botones").show();
        AsignarClickTipoReserva();
    } else {
        $("#div_campos").hide();
        $("#div_botones").hide();
    }    

}

//Asigna el evento click a los circulos de colores de tipo reserva.
function AsignarClickTipoReserva()
{
    //$("[name=IdTipoReserva]").first().prop("checked", true);

    $(".redondo").click(function () {
        $(".redondo").each(function (i, item) {
            $("#" + item.id).removeClass("seleccionado");
        });
        $($(this)).addClass("seleccionado");        
        $("#IdTipoReserva").val($(this).attr("data-id"));
    });

}

//Carga el nombre del cliente del pedido,
function CargarNombreCliente()
{
    $("#NombrePersona").val($("#div_nombrecliente").html());     
}