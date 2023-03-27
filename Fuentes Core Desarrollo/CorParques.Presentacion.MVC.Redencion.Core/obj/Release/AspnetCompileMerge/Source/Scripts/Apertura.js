var dataPuntos = [];

$(function () {
    Inicializar();
});
function successInsertApertura(rta) {
    if (rta.Correcto) {
        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "Apertura", "success");
        //EjecutarAjax(urlBase + "Apertura/EditAperturaBase", "GET", null, "successfull", null);
        //MostrarMensaje("Importante", "Operación realizada con éxito.");
    }
    else {
        MostrarMensaje("Fallo al guardar", rta.Mensaje);
    }
}

function successUpdateApertura(rta) {
    if (rta.Correcto) {
        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "Apertura/EditAperturaBase", "success");
        //EjecutarAjax(urlBase + "Apertura/EditAperturaBase", "GET", null, "successfull", null);
        //MostrarMensaje("Importante", "Operación realizada con éxito.");
    }
    else {
        MostrarMensaje("Fallo al guardar", rta.Mensaje);
    }
}

function setInsertEdit(data) {
 
    $("#IdPunto").html("");
    $("#IdPunto").append("<option value>Seleccione...</option>");
    if (data.length > 0) {
        $.each(data, function (i, item) {
            $("#IdPunto").append("<option value='" + item.value + "'>" + item.label + "</option>");
        });
    }

    EjecutarAjax(urlBase + "Apertura/ObtenerAperturaBase", "GET", { IdPunto: 0, Fecha: $("#FechaString").val() }, "successfull", null);
    //$("#listViewContenido").html("");

}

function successfull(data) {
    $("#listViewContenido").html(data);
    //$("#listViewContenido").html($(data).find("#listViewContenido"));
    //$("#IdPunto").value = data.IdPunto;
    //$("#id").value(data.Id);
    //$("#idPuntoOrigen").value(data.IdPuntoCreado);
    //$("#listView").html(data);
}

function Limpiar() {
    var requeridos = $("#frmtest").find(".limpiar");
    var correcto = true;
    var registros = 0;
    $.each(requeridos, function (index, value) {

        $(value).val("");
        $(value).html("");

    });
    $("#tagsPuntos").importTags('');
}

function Cancelar() {
    window.location = urlBase + "Apertura/EditAperturaBase";
}

function CancelarNuevo() {
    window.location = urlBase + "Apertura";
}

function Cancel() {
    MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "CancelarNuevo", "");
}


function Confirmar() {
    if (!validarFormulario("frmtest *")) {
        return false;
    }
    if (ValidaCantidades('frmtest')) {
        MostrarConfirm("Importante!", "¿Está seguro de guardar el alistamiento? ", "GuardarAlistamiento", "");
    }
    else {
        MostrarMensaje("Importante", "Digite un valor diferente a cero");
    }
}

function EditarAlistamiento() {

    EjecutarAjax(urlBase + "Apertura/Update", "GET", ObtenerObjeto("frmEditApertura *"), "successUpdateApertura", null);
    //EjecutarAjax(urlBase + "Apertura/Insert", "POST", element, "successInsertApertura", null);

}

function GuardarAlistamiento() {

    //$("#frmtest").submit();

    EjecutarAjax(urlBase + "Apertura/Insert", "GET", ObtenerObjeto("frmtest *"), "successInsertApertura", null);
    //EjecutarAjax(urlBase + "Apertura/Insert", "POST", element, "successInsertApertura", null);

}

function SumarTotal(ctr) {


    var split = ctr.id.split('_');
    var id = split[1];
    var Denominacion = $("#Denominacion_" + id).html().trim();
    var ctrValor;
    if (ctr.value == "") {
        ctrValor = 0;
    } else {
        ctrValor = ctr.value;
    }
    var Campo = parseInt(Denominacion) * parseInt(ctrValor);
    var Valorcampo = $("#Total_" + id).html().trim();
    
    if (Campo == 0) {
        $("#Total_" + id).html("");
    } else {
        $("#Total_" + id).html(FormatoMoneda(Campo));
    }
    $("#TotalNido_" + id).val(Campo);
    var Total = RemoverFormatoMoneda($("#Total").html().trim());
    if (Valorcampo == "") {
        var TotalValor = 0
    } else {
        var TotalValor = RemoverFormatoMoneda(Valorcampo);
    }
    Total = Total - TotalValor;
    Total = parseInt(Total) + parseInt(Campo);
    $("#Total").html(FormatoMoneda(Total));

}

function placeholdercero(ctr) {
    if (ctr.value == "0") {
        ctr.value = ""
    }

}


function InitPartialCreate(dataPuntos) {

    $("#tagsPuntos").importTags('');
    $("#hdListPuntos").val('');
    setAutocompleteCategory()
    tagsAutocomplete($('#tagsPuntos'), $('#puntosAutocomplete'), $("#hdListPuntos"), dataPuntos, true)

}

function CambioFechaSuccess(rta) {
    dataPuntos = rta;
    InitPartialCreate(dataPuntos);
}

function EstablecerFormatoMoneda() {
    $(".formato_moneda").each(function (index, element) {
        element.innerText = FormatoMoneda(element.innerText);
    });
}
function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function loadCalendarmin() {
    var crearcalendario = getCookie('CalendarioCrear');
    var editarcalendario = getCookie('CalendarioEditar');
    if (crearcalendario !== "") {
        $(".CalendarioCrear").val(crearcalendario);
        if ($(".CalendarioCrear").length)
            EjecutarAjax(urlBase + "Apertura/PuntosxFecha", "GET", { Fecha: $(".CalendarioCrear").val() }, "CambioFechaSuccess", null);
    }
    if (editarcalendario !== "") {
        $(".CalendarioEditar").val(editarcalendario);
        if ($(".CalendarioEditar").length)
            EjecutarAjax(urlBase + "Apertura/PuntosEditxFecha", "GET", { Fecha: $(".CalendarioEditar").val() }, "setInsertEdit", null);
    }

    $(".CalendarioCrear").on('dp.change', function (e) {
        if ($(this).val() !== "") {
            document.cookie = "CalendarioCrear=" + $(this).val();
            EjecutarAjax(urlBase + "Apertura/PuntosxFecha", "GET", { Fecha: $(this).val() }, "CambioFechaSuccess", null);
        }
    })
    $(".CalendarioEditar").on('dp.change', function (e) {
        if ($(this).val() !== "") {
            document.cookie = "CalendarioEditar=" + $(this).val();
            EjecutarAjax(urlBase + "Apertura/PuntosEditxFecha", "GET", { Fecha: $(this).val() }, "setInsertEdit", null);
        }
    })
}


function Inicializar() {
    loadCalendar();
    loadCalendarmin();
    EstablecerFormatoMoneda();

    var Total = RemoverFormatoMoneda($("#Total").html().trim());
    $("#Total").html(FormatoMoneda(Total));

    $("#div_message_error").hide();
    $("#btnSaveTest").click(function () {
        //var element = ObtenerObjeto("frmtest");
        Confirmar();
    });
    $("#btnCancel").click(function () {
        //var element = ObtenerObjeto("frmtest");
        Cancel();

    });

    $("#btnEdtiSave").click(function () {
        if (!validarFormulario("frmEditApertura *")) {
            return false;
        }
        if (ValidaCantidades('frmEditApertura')) {
            MostrarConfirm("Importante!", "¿Está seguro de modificar el alistamiento? ", "EditarAlistamiento", "");
        }
        else {
            MostrarMensaje("Importante", "Digite un valor diferente a cero");
        }
    });
    $("#btnEdtiCancel").click(function () {
        //var element = ObtenerObjeto("frmtest");
        MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "Cancelar", "");

    });

    $("#IdPunto").select2({
        placeholder: "Seleccione el punto"
    });

    $("#IdPunto").change(function () {

        EjecutarAjax(urlBase + "Apertura/ObtenerAperturaBase", "GET", { IdPunto: $(this).val(), Fecha: $("#FechaString").val() }, "successfull", null);

    });


}

function ValidaCantidades(form) {
    var valido = false;
    var objeto = $("#" + form).serializeArray();
    objeto.forEach(function (obj) {
        if (obj.name.indexOf('Cantidad') > 0 && obj.value > 0) {
            valido = true;
        }
    });
    return valido;
}

