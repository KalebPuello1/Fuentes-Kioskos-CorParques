var _Mantenimiento;
var _Orden;
var HoraInicioOrden;
var HoraInicio;
var HoraFin;

$(function () {
    setEventEdit();
    asignarSelect2();
});

function OnChangeEvent(dropDownElement) {
    var IdPunto = dropDownElement.options[dropDownElement.selectedIndex].value;
    var punto = dropDownElement.options[dropDownElement.selectedIndex].label
    EjecutarAjax(urlBase + "OrdenMantenimiento/ListarOrdenes", "GET", { IdPunto: IdPunto }, "printPartial", { div: "#listView", func: "setEventEdit" });
}

function setEventEdit() {
    $(".procesar").click(function () {
        var CodPunto = parseInt($("#DDL_Punto").val());        
        EjecutarAjax(urlBase + "OrdenMantenimiento/BuscarOrdenes", "GET", { NumeroOrden: $(this).data("id"), CodPunto: CodPunto }, "printPartialWizard", { div: "#listView", func: "setEventEdit" });
    });
}

function GuardarRegistro(paso) {
    _Mantenimiento = new Object();
    var flag = false;
    var Paso = paso //$('.js-title-step')[0].innerText.split(' ')[1]
    if (document.getElementById('aprobado-' + Paso) != null) {
        var aprobado = $("#aprobado-" + Paso)[0].checked;
        if (!aprobado && $("#Obsrv-" + Paso).val() == "") {
            $("#Obsrv-" + Paso).addClass("required").addClass("errorValidate").addClass('data-mensajeerror');
            if (!validarFormulario("frmWizard")) {
                return false;
            }
        }
        $('.obs').removeClass("required").removeClass("errorValidate").removeAttr('data-mensajeerror');
        _Mantenimiento.Procesado = aprobado;
    }
    HoraFin = new Date();
    _Mantenimiento.NumeroOrden = $("#NumeroOrden").val();
    _Mantenimiento.NumeroOperacion = $("#NumeroOperacion-" + Paso).val(); // $("#NumeroOperacion").val();
    _Mantenimiento.Observacion = $("#Obsrv-" + Paso).val();
    _Mantenimiento.Aprobado = aprobado;
    //_Mantenimiento.HoraInicio = localStorage.HoraInicio;
    //_Mantenimiento.HoraFin = createDateObj(HoraFin);
    _Mantenimiento.CodSapPunto = $("#CodSapPunto").val();
    _Mantenimiento.Nombre = $("#Nombre-" + Paso).val();
    _Mantenimiento.Punto = $("#Punto-" + Paso).val();
    _Mantenimiento.IdOrdenMantenimiento = $("#IdOrdenMantenimiento-" + Paso).val();
    EjecutarAjax(urlBase + "OrdenMantenimiento/InsertOrdenMantenimiento", "POST", JSON.stringify(_Mantenimiento), "successCreateOrdenMantenimiento", null);
    return true;
}

function createDateObj(date) {
    //
    var day = date.getDate();
    var month = date.getMonth() + 1;
    var year = date.getFullYear();
    var hour = date.getHours();
    var minute = date.getMinutes();
    var second = date.getSeconds();
    var millisec = date.getMilliseconds();
    var fecha = year + "/" + month + "/" + day + " " + hour + ":" + minute + ":" + second;
    //var jsDate = Date.UTC(year, month, day, hour, minute, second, millisec);
    return fecha;
}

function Insertar(Paso) {
    _Orden = new Object();
    if ($("#ObsrvAprov-" + Paso).val() == "") {
        $("#ObsrvAprov-" + Paso).addClass("required").addClass("errorValidate").addClass('data-mensajeerror');
        if (!validarFormulario("frmWizard")) {
            return false;
        }
    }
    else {
        HoraFin = new Date();
        var aprobado = document.getElementsByName("o5");
        if (aprobado[0].checked == true) {
            chk = 1;
        } else if (aprobado[1].checked == true) {
            chk = 0;
        }
        _Orden.NumeroOrden = parseInt($("#NumeroOrdenFin").val());
        _Orden.Aprobado = chk;
        _Orden.Observacion = $("#ObsrvAprov-" + Paso).val();
        _Orden.Procesado = 1;
        _Orden.CodSapPunto = $("#CodSapPuntoFin").val();
        _Orden.IdOperaciones = $("#IdOperaciones").val();
        _Orden.HoraInicio = localStorage.HoraInicioOrden;
        _Orden.HoraFin = createDateObj(HoraFin);
        EjecutarAjax(urlBase + "OrdenMantenimiento/InsertOrden", "POST", JSON.stringify(_Orden), "successCreateOrden", null);
        return true;
    }
}

function successCreateOrdenMantenimiento(rta) {
    if (rta.Correcto) {
        //cerrarModal("modalOrdenes");
        //MostrarMensajeRedireccion("Importante", "Operación realizada con éxito", "/OrdenMantenimiento/Index", "success");
    }
}

function successCreateOrden(rta) {
    if (rta.Correcto) {
        cerrarModal("modalOrdenes");
        MensajeRedireccionAceptar("Importante", "Operación realizada con éxito", "OrdenMantenimiento/Index", "success");
    }
}

function MensajeRedireccionAceptar(Titulo, Mensaje, UrlRedireccion, Tipo) {
    var Type = "";

    if (Tipo != undefined) {
        Type = Tipo;
    }

    swal({
        title: Titulo,
        text: Mensaje,
        showCancelButton: false,
        closeOnConfirm: true,
        type: Type,
        allowOutsideClick: false,
        allowEscapeKey: false
    }).then(function () {
        if (UrlRedireccion != null) {
            if (urlBase.length > 1) {
                window.location = urlBase + UrlRedireccion;
            } else if (UrlRedireccion.substr(0, 1) == "/") {
                window.location = UrlRedireccion;
            } else {
                window.location = "/" + UrlRedireccion;
            }
        }

    }).catch(swal.noop);
}



function MensajeRedireccionCerrar(Titulo, Mensaje, UrlRedireccion, Tipo) {
    var Type = "";
    if (Tipo != undefined) {
        Type = Tipo;
    }
    swal({
        title: Titulo,
        text: Mensaje,
        showCancelButton: true,
        closeOnConfirm: true,
        type: Type,
        allowOutsideClick: false,
        allowEscapeKey: false
    }).then(function () {
        if (UrlRedireccion != null) {
            if (urlBase.length > 1) {
                window.location = urlBase + UrlRedireccion;
            } else if (UrlRedireccion.substr(0, 1) == "/") {
                window.location = UrlRedireccion;
            } else {
                window.location = "/" + UrlRedireccion;
            }
        }
    }).catch(swal.noop);
}

function printPartialWizard(data, values) {
    $(values.div).html(data);
    window[values.func]();
}





