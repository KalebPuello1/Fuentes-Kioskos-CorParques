$(function () {
    if (getCookie(".ASPXAUTH") === "" && document.location.href.indexOf("Login")<0) {
        document.location = urlBase;
        return false;
    }
    setNumeric();
    $(".mascaraMoneda").change(function () {
        formatoMoneda(this);
    });

    $("#btnSaveGeneric").click(function () {

        var dataurl = $(this).data("url");

        if (dataurl != null && dataurl.length > 0)
            if (validarFormulario("modalCRUD .modal-body")) {
                EjecutarAjax($(this).data("url"), $(this).data("metod"), ObtenerObjeto("modalCRUD .modal-body form"), $(this).data("function"));
            }
    });
    $('[data-toggle="popover"]').popover({
        html: true
    });
    
    validateNotificationPriority();
    ValidarEstadoPantalla();
});

function asignarSelect2() {
    $(".setSelect2").select2();
}
var objectNotification;

function viewNotification(element) {


    $("#modalNotificacion .modal-title").html($(element).data("titulo"));
    $("#modalNotificacion .modal-body").html($(element).data("contenido"));
    $("#btnOkViewNotification").off("click");
    objectNotification = element;
    $("#btnOkViewNotification").click(function () {
        setViewNotification(objectNotification);
    });
    abrirModal("modalNotificacion");
}

function setViewNotification(element) {
    $.ajax({
        url: urlBase + "Home/setView",
        type: "GET",
        data: { id: $(element).data("id") },
        contentType: "application/json"
    });
    if ($(element).parent().hasClass("notificacionNew")) {
        $(element).parent().removeClass("notificacionNew");
        var contador = parseInt($("#countpendingview").html()) - 1;
        $("#countpendingview").html(contador == 0 ? "" : contador.toString());
    }
    var refreshIntervalNotification = setInterval(function () { validateNotificationPriority(); clearInterval(refreshIntervalNotification); }, 1000)
}
function validateNotificationPriority() {
    $.each($(".notificacionNew"), function (i, v) {
        if ($(v).data("prioritario") === "True" || $(v).data("prioritario") === true) {
            viewNotification($(v).find("a:first"));
            return false;
        }
    });
}

function setAutocompleteCategory() {

    $.widget("custom.catcomplete", $.ui.autocomplete, {
        _create: function () {
            this._super();
            this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
        },
        _renderMenu: function (ul, items) {
            var that = this,
              currentCategory = "";
            $.each(items, function (index, item) {
                var li;
                if (item.category != currentCategory) {
                    ul.append("<li class='ui-autocomplete-category'>" + item.category + "</li>");
                    currentCategory = item.category;
                }
                li = that._renderItemData(ul, item);
                if (item.category) {
                    li.attr("aria-label", item.category + " : " + item.label);
                }
            });
        }
    });
}
//NMSR Validaciones arbol
function setEventTreView(hdListSelect) {
    $(".minus-treeview").click(function () {
        $(this).parent().find("ul").css("display", "none");
        $(this).css("display", "none");
        $(this).parent().find(".plus-treeview").css("display", "inline");
    });
    $(".plus-treeview").click(function () {
        $(this).parent().find("ul").css("display", "");
        $(this).css("display", "none");
        $(this).parent().find(".minus-treeview").css("display", "inline");
    });
    $("input[type=checkbox]").click(function () {
        if ($(this).next().is("ul")) {
            $(this).next().find("input[type=checkbox]").prop("checked", $(this).prop("checked"))
        }

        $chkPadre = $(this).parent().parent().parent().children("input[type=checkbox]");

        //Coloca como chequeado al nodo padre del nodo actual
        if ($(this).prop("checked")) {
            SelectParent($chkPadre, $(this));
        }
        else {
            //Si es el último en chequearse de ese nivel debe desmarcar a su check padre
            if (validaCheckNivelActual($chkPadre, $(this).attr("class").split(" ")[0]))
                SelectParent($chkPadre, $(this));
        }
        //Limia el hidden
        $(hdListSelect.selector).val("");
        ////Recorre todos los checkbox contenidos dentro del arbol y que se encuentre en estado chequeado s
        $("div .checkbox").find("input[type=checkbox]:checked").each(function (i, item) {
            var _value = $(hdListSelect.selector).val();
            if (_value.length > 0)
                _value += ",";
            _value += item.value;
            //Agrega al hidden cada id correspondiente al valor del checkbox seleccionado y separado por ","
            $(hdListSelect.selector).val(_value);
            //alert($(hdListSelect.selector).val())
        });

    });
}

//NMSR Validaciones arbol
function SelectParent(checkboxSelect, actual) {
    if (checkboxSelect && checkboxSelect.length == 0)
        return;
    checkboxSelect.prop("checked", $(actual).prop("checked"));
    SelectParent(checkboxSelect.parent().parent().parent().children("input[type=checkbox]"), checkboxSelect);
}

//NMSR Valida si el check que recibe es el último chequeado en el nivel donde este se encuentre
function validaCheckNivelActual(chek, clase) {
    var ultimoChekeado = true;
    chek.next().find("." + clase).each(function (i, v) {
        if ($(v).prop("checked")) {
            ultimoChekeado = false;
            return false;
        }
    });
    return ultimoChekeado;
}


var Math1 = {
    min: function (values) {
        if (values.length == 0) {
            return NaN;
        } else if (values.length == 1) {
            var val = values.pop();
            if (typeof val == "number") {
                return val;
            } else {
                return NaN;
            }
        } else {
            var ss = values;
            var val = ss.pop();
            return Math.min(val, this.min(values))
        }
    },
    max: function (values) {
        if (values.length == 0) {
            return NaN;
        } else if (values.length == 1) {
            var val = values.pop();
            if (typeof val == "number") {
                return val;
            } else {
                return NaN;
            }
        } else {
            var ss = values;
            var val = ss.pop();
            return Math.min(val, this.max(values))
        }
    }
}
function setNumeric() {
    $(".numerico").keypress(function () {
        return EsNumero(event);
    });
}
function loadCalendar() {
    $(".calendario").datetimepicker({
        format: 'DD/MM/YYYY'
    });
    $(".CalendarDateTime").datetimepicker({
        format: 'DD/MM/YYYY hh:mm A'
    });
    $(".calendario7dias").datetimepicker({
        format: 'DD/MM/YYYY',
        minDate: moment().subtract(1, 'd'),
        maxDate: moment().add(7, 'd')

    });

    $(".calendario5dias").datetimepicker({
        format: 'DD/MM/YYYY',
        minDate: moment().subtract(1, 'd'),
        maxDate: moment().add(5, 'd')

    });
    $(".calendario15dias").datetimepicker({
        format: 'DD/MM/YYYY',
        minDate: moment().subtract(1, 'd'),
        maxDate: moment().add(15, 'd')

    });
}
function voidFunction() {

}
function formatNumber(num) {
    var numero = num.toString().replace('.', ',');
    return numero.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
}

function EsNumero(evt) {

    if (window.event) {
        var charCode = event.keyCode;

    } else if (evt.onkeypress.arguments[0].charCode) {
        var charCode = evt.onkeypress.arguments[0].charCode;
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}


function EsTexto(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (!((charCode == 35) || (charCode == 8) || (charCode == 32) || (charCode == 45) || (charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122)))
        return false;

    return true;
}



function ObtenerObjeto(formulario) {
    return $("#" + formulario).serializeArray();
}

function mostrarAlerta(titulo, descripcion) {

    $(".alert-notification h4").html(titulo)
    $(".alert-notification p").html(descripcion)
    $(".alert-notification").show("fade", {}, 500, callbackShow);
    setTimeout(function () {
        $(".alert-notification").hide("fade", {}, 1000, callbackHide);
    }, 5000);
}
function callbackHide(div) {
    setTimeout(function () {
        $("#" + div).hide().fadeIn();
    }, 1000);
};
function callbackShow(div) {
    setTimeout(function () {
        $("#" + div).hide().fadeIn();
    }, 1000);
};
function fileDropZone(element, urlSave, urlRemove, types, hidden, prev, url) {
    $(element).dropzone({
        url: urlSave,
        maxFilesize: 2,
        uploadMultiple: false,
        addRemoveLinks: true,
        dictRemoveFile: "X",
        acceptedFiles: types,
        removedfile: function (file) {
            EjecutarAjax(urlBase + urlRemove, "GET", { name: file.name }, "voidFunction", null);
            file.previewTemplate.remove();
        },
        success: function () {
            $(hidden).val($(this)[0].files[0].name);
        },
        init: function () {
            if (prev) {
                if ($(hidden).val() !== "") {
                    var mockFile = { name: url + $(hidden).val(), width: '100%' };
                    this.emit("addedfile", mockFile);
                    this.emit("thumbnail", mockFile, url + $(hidden).val());
                    $('.dz-image').last().find('img').attr({ width: '100%', height: '100%' });

                }

            }
        }
    });
}

function ReplaceCharacter(element,characterOld,characterNew)
{
    var letters = /^[A-Za-z0-9]+$/;
    if (!characterOld.match(letters)) {
        element = element.replace(characterOld, '') + characterNew
        return false;
    }
    return true;
}

function tagsAutocomplete(inputTags, autocomplete, hidden, data, category) {
    
    inputTags.tagsInput({
        width: 'auto',
        onRemoveTag: function (item) {
            var id;
            $.each(data, function (i, v) {
                if (v.label === item)
                    id = v.value;
            });
            var seleccionados = hidden.val().split(',')
            if ($.inArray(id, seleccionados)) {
                var datafinal = $.grep(seleccionados, function (value) {
                    return value.toString() != id.toString();
                });
            }
            hidden.val(datafinal.toString())
        }
    });
    if (autocomplete.next().hasClass("required")) {
        $('.tagsinput').addClass("required");
    }
    $('.tagsinput').keypress(function () { return false; })
    if (category) {
        autocomplete.catcomplete({
            source: data,
            focus: function (event, ui) {
                autocomplete.val(ui.item.label);
                event.preventDefault();
            },
            select: function (event, ui) {
                if (!inputTags.tagExist(ui.item.label)) {
                    inputTags.addTag(ui.item.label)
                    hidden.val((hidden.val().length > 0 ? (hidden.val() + ",") : "") + ui.item.value);
                }
                $(this).val("");
                return false;
            }
        });
    } else {
        autocomplete.autocomplete({
            source: data,
            focus: function (event, ui) {
                autocomplete.val(ui.item.label);
                event.preventDefault();
            },
            select: function (event, ui) {
                if (!inputTags.tagExist(ui.item.label)) {
                    inputTags.addTag(ui.item.label)
                    hidden.val((hidden.val().length > 0 ? (hidden.val() + ",") : "") + ui.item.value);
                }
                $(this).val("");
                return false;
            }
        });
    }
    InhabilitarCopiarPegarCortarClase();
}

function printPartialModal(data, obj) {
    $("#btnCancelGeneric").show();
    $("#btnSaveGeneric").show();
    $("#btnVolverGeneric").hide();
    $("#btnImprimirGeneric").hide();
    $("#btnSaveGeneric").data("url", obj.url);
    $("#btnSaveGeneric").data("metod", obj.metod);
    $("#btnSaveGeneric").data("function", obj.func);
    $("#btnCancelGeneric").click(function () {
        cerrarModal('modalCRUD');
    });
    $("#btnVolverGeneric").click(function () {
        cerrarModal('modalCRUD');
    });
    $("#btnImprimirGeneric").click(function () {

        $("#div_print").show();
        var mode = "popup";
        var close = true;
        var extraCss = "";
        var print = "#div_print";
        var keepAttr = [];
        keepAttr.push("class");
        keepAttr.push("id");
        keepAttr.push("style");

        var headElements = '<meta charset="utf-8" />,<meta http-equiv="X-UA-Compatible" content="IE=edge"/>';
        var options = { mode: mode, popClose: close, extraCss: extraCss, retainAttr: keepAttr, extraHead: headElements };

        $(print).printArea(options);
        setTimeout(function () { $("#div_print").hide(); }, 200);

    });

    $("#modalCRUD .modal-dialog").removeClass("modal-lg");
    if (obj.modalLarge)
        $("#modalCRUD .modal-dialog").addClass("modal-lg");
    $("#modalCRUD .modal-title").html(obj.title);
    $("#modalCRUD .modal-body").html(data);
    if (obj.hidesave) {
        $("#btnCancelGeneric").hide();
        $("#btnSaveGeneric").hide();
    }
    if (obj.showreturn) {
        $("#btnVolverGeneric").show();
    }
    if (obj.print) {
        $("#btnImprimirGeneric").show();
    }
    if (obj.Table) {
        $("#datatable-responsive_1").DataTable();
        $("#modalCRUD .modal-dialog").css("width", 940);
    }

    if (obj.func2) {
        window[obj.func2](obj.param);
    }

    if (obj.DatePicker) {
        loadCalendar();
    }

    if (obj.TimePicker) {
        setTimePicker();
    }

    if (obj.OcultarCierre)
        $("#modalCRUD .close").hide();
    else
        $("#modalCRUD .close").show();

    if (obj.TextoBotonGuardar)
        $("#btnSaveGeneric").html(obj.TextoBotonGuardar);

    setNumeric();
    abrirModal("modalCRUD");    
}

function printPartial(data, values) {
    $(values.div).html(data);

    if (!values.table) {
        $(values.div).find("table").DataTable();
        $(values.div).find("table").on('draw.dt', function () {
            setEventEdit()
        });
    }
    window[values.func]();

 
}


function EjecutarAjax(url, type, values, funcionSuccess, parameter, loader, funcError) {
    if (funcError === null) {
        funcError = function (jqXHR, exception) {
            $("#divError strong").text("Error!");
            if (jqXHR.status === 0) {
                $("#divError i").text(" No cuenta con conexion a internet.");
            } else if (jqXHR.status === 404) {
                $("#divError i").text(" 404. No encuentra el recurso solicitado. '" + url + "'");
            } else if (jqXHR.status === 500) {
                $("#divError i").text(" 500. Error interno del servidor. Por favor comuníquese con el administrador del sistema");
            } else if (exception === 'parsererror') {
                $("#divError i").text(" Error al convertir el objeto en JSON");
            } else if (exception === 'timeout') {
                $("#divError i").text(" Tiempo de espera agotado. Por favor comuníquese con el administrador del sistema");
            } else if (exception === 'abort') {
                $("#divError i").text(" Petición AJAX abortada. Por favor comuníquese con el administrador del sistema");
            } else {
                $("#divError i").text(" Error inesperado (" + jqXHR.responseText + "). Por favor comuníquese con el administrador del sistema");
            }
            if (!loader)
                finalizarProceso();
            EjecutarAjaxJson(url, type, JSON.parse(values), funcionSuccess, parameter);
        };
    }

    if(!loader)
        iniciarProceso();
    $.ajaxSetup({ cache: false });
    $.ajax({
        url: url,
        type: type,
        data: values,
        contentType: "application/json",
        success: function (data) {
            window[funcionSuccess](data, parameter);
            if (!loader)
                finalizarProceso();
        },
        error: funcError,
        complete: function (data) {

        }
    });
}


function EjecutarAjaxJson(url, type, values, funcionSuccess, parameter) {
    iniciarProceso();
    $.ajaxSetup({ cache: false });
 
    $.ajax({
        ContentType: "application/json",
        url: url,
        type: type,
        data: values,
        success: function (data) {
            window[funcionSuccess](data, parameter);
            finalizarProceso()
        },
        error: function (jqXHR, exception) {
            $("#divError strong").text("Error!");
            if (jqXHR.status === 0) {
                mostrarAlerta("divError", " No cuenta con conexion a internet.");
            } else if (jqXHR.status === 404) {
                mostrarAlerta("divError", " 404. No encuentra el recurso solicitado. '" + url + "'");
            } else if (jqXHR.status === 500) {
                mostrarAlerta("divError", " 500. Error interno del servidor. Por favor comuníquese con el administrador del sistema");
            } else if (exception === 'parsererror') {
                mostrarAlerta("divError", " Error al convertir el objeto en JSON");
            } else if (exception === 'timeout') {
                mostrarAlerta("divError", " Tiempo de espera agotado. Por favor comuníquese con el administrador del sistema");
            } else if (exception === 'abort') {
                mostrarAlerta("divError", " Petición AJAX abortada. Por favor comuníquese con el administrador del sistema");
            } else {
                mostrarAlerta("divError", " Error inesperado (" + jqXHR.responseText + "). Por favor comuníquese con el administrador del sistema");
            }
            finalizarProceso();
        }
    });
}

function abrirModal(nombre) {
    $("#" + nombre).modal({
        backdrop: "static",
        keyboard: false
    });
}

function iniciarProceso() {
    $(".loader-wrapper").css("display", "block");
    //Reinicia el contador de finalizar sesion cada vez que se haga una peticion ajax.
    ReiniciarControlSesion();
}

function finalizarProceso() {
    $(".loader-wrapper").css("display", "none");
}

function cerrarModal(nombre) {
    $("#" + nombre).modal("hide");
    $("body").removeClass("modal-open");
    $(".modal-backdrop").remove();
}
function formatoMoneda(input) {
    var num = input.value.replace(/\./g, '');
    if (!isNaN(num)) {
        num = num.toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1.');
        num = num.split('').reverse().join('').replace(/^[\.]/, '');
        input.value = num;
    }

    else {
        alert('Solo se permiten numeros');
        input.value = input.value.replace(/[^\d\.]*/g, '');
    }
}

function validarEmail(email) {
    expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return expr.test(email);
}

function validarRangos(formulario) {
    var correcto = true;
    var rangos = $("#" + formulario).find(".rango");
    $.each(rangos, function (index, value) {

        if ($(value).data("valor") === "min") {
            if (parseFloat($(value).val()) >= parseFloat($("#" + $(value).data("comparar")).val())) {
                $(this).attr("data-mensajeerror", $(this).attr("data-mensaje"));
                $(this).addClass("errorValidate");
                correcto = false;
            }
        }
        else if ($(value).data("valor") === "max") {
            if (parseFloat($(value).val()) <= parseFloat($("#" + $(value).data("comparar")).val())) {
                $(this).attr("data-mensajeerror", $(this).attr("data-mensaje"));
                $(this).addClass("errorValidate");
                correcto = false;
            }
        }

        if ($(value).data("valor") === "minTime") {
            if (parseFloat($(value).val().replace(":", "")) >= parseFloat($("#" + $(value).data("comparar")).val().replace(":", ""))) {
                $(this).attr("data-mensajeerror", $(this).attr("data-mensaje"));
                $(this).addClass("errorValidate");
                correcto = false;
            }
        }
        else if ($(value).data("valor") === "maxTime") {
            if (parseFloat($(value).val().replace(":", "")) <= parseFloat($("#" + $(value).data("comparar")).val().replace(":", ""))) {
                $(this).attr("data-mensajeerror", $(this).attr("data-mensaje"));
                $(this).addClass("errorValidate");
                correcto = false;
            }
        }
    });
    return correcto;
}

function validarpassword(formulario) {
    var requeridos = $("#" + formulario).find(".password");
    var correcto = true;
    var registros = 0;
    $.each(requeridos, function (index, value) {

        $(this).removeClass("errorValidate");
        var grupo = $(this)[0].tagName;
        var tipo = $(this)[0].type;
        if ($(this).val() === "") {
            $(this).attr("data-mensajeerror", "Este campo es obligatorio");
            $(this).addClass("errorValidate");
            correcto = false;
        }

        var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])([A-Za-z\d$@$!%*?&]|[^ ]){8,15}$/;
        if (!(regex.test($(this).val()))) {
            var mensaje = "La contraseña debe contener <br/> -Minimo 8 caracteres y maximo 15 <br/> -Al menos una letra mayúscula <br/> -Al menos una letra minúscula <br/> -Al menos un valor numerico <br/> -Al menos un caracter especial";
            $(this).attr("data-mensajeerror", mensaje);
            $(this).addClass("errorValidate");
            correcto = false;
        }

    });
    return correcto;
}
function validarObligatorios(formulario) {
    var requeridos = $("#" + formulario).find(".required");
    var correcto = true;
    var registros = 0;
    $("#" + formulario + " .errorValidate").removeClass("errorValidate");
    $.each(requeridos, function (index, value) {

        $(this).removeClass("errorValidate");
        var grupo = $(this)[0].tagName;
        var tipo = $(this)[0].type;
        switch (grupo) {
            case "SELECT":
                if ($(this).val() === "") {
                    if ($(this).hasClass("setSelect2")) {
                        $(this).next().attr("data-mensajeerror", "Este campo es obligatorio");
                        $(this).next().addClass("errorValidate");
                        $(this).next().find(".select2-selection").addClass("errorValidate");
                    }
                    else {
                        $(this).attr("data-mensajeerror", "Este campo es obligatorio");
                        $(this).addClass("errorValidate");
                    }
                    correcto = false;
                }
                break;
            case "INPUT":
                switch (tipo) {
                    case "text":
                        if ($(this).val() === "") {
                            $(this).attr("data-mensajeerror", "Este campo es obligatorio");
                            $(this).addClass("errorValidate");
                            correcto = false;
                        }
                        break;
                    case "password":
                        if ($(this).val() === "") {
                            $(this).attr("data-mensajeerror", "Este campo es obligatorio");
                            $(this).addClass("errorValidate");
                            correcto = false;
                        }
                        break
                        //case "radio":
                        //    if ($(this).parent().find("[name=" + $(this)[0].name + "]:checked").length === 0) {
                        //        if ($.inArray($(this).data("nombre"), errores) === -1)
                        //            errores[registros] = $(this).data("nombre");
                        //    }
                        //    break;
                        //case "check":
                        //    if (!$(this).prop("checked"))
                        //        errores[registros] = $(this).data("nombre");
                        //    break;

                        //case "hidden":
                        //    if ($(this).val() === "")
                        //        errores[registros] = $(this).data("nombre");
                        //    break;
                }
                break;
            case "TEXTAREA":
                if ($(this).val() === "") {
                    $(this).attr("data-mensajeerror", "Este campo es obligatorio");
                    $(this).addClass("errorValidate");
                    correcto = false;
                }
                break;
            case "DIV":
                if ($(this).children().length <= 2) {
                    $(this).attr("data-mensajeerror", "Este campo es obligatorio");
                    $(this).addClass("errorValidate");
                }
                break;
        }
    });

    //Check-group
    var _listCheck = $("#" + formulario).find(".grpCheckBox");
    if (_listCheck.length > 0) {

        var obj = _listCheck.children().find("input:checkbox");
        if (obj.length > 0) {
            if (_listCheck.children().find("input:checkbox:checked").length == 0) {
                correcto = false;
                $(this).addClass("errorValidate");
            }
        }
    }


    return correcto;
}

function ValidarExprRegulares(formulario) {
    //valida la estructura del Email
    var _listEmail = $("#" + formulario).find(".email");
    var correcto = true;

    $.each(_listEmail, function (i, item) {

        $(this).removeClass("errorValidate");
        var grupo = $(this)[0].tagName;
        var tipo = $(this)[0].type;

        switch (grupo) {
            case "INPUT":
                switch (tipo) {
                    case "text":
                        if ($(this).val().length > 0) {
                            //
                            if (!validarEmail($(this).val())) {
                                $(this).attr("data-mensajeerror", "El Email es inválido");
                                $(this).addClass("errorValidate");
                                correcto = false;
                            }
                        } else {
                            if (this.classList.contains("required")) {

                                $(this).attr("data-mensajeerror", "Este campo es obligatorio");
                                $(this).addClass("errorValidate");
                                correcto = false;
                            }
                        }
                        break;
                }
                break;
        }

    });
    //validacion estructura Email  fin

    return correcto;
}

function validarFormulario(formulario) {
    QuitarTooltip()
    var respuestas = [];
    respuestas.push(validarObligatorios(formulario));
    respuestas.push(validarpassword(formulario));
    respuestas.push(validarRangos(formulario));
    respuestas.push(ValidarExprRegulares(formulario));
    mostrarTooltip();

    if ($.inArray(false, respuestas) !== -1) {
        MostrarMensaje("Importante", "Hay inconsistencias en el formulario, revise los campos demarcados con color rojo.", "error");
    }
    return $.inArray(false, respuestas) === -1;
}

function mostrarTooltip() {
    $(".errorValidate:first").focus();
    $(".errorValidate").mouseover(function () {
        if ($(this).attr("data-mensajeerror") !== undefined) {
            if ($(this).attr("data-mensajeerror").length > 0) {
                $(this).parent().append("<div class='tooltipError'>" + $(this).attr("data-mensajeerror") + "</div>");
                if ($(this).parent().prop("tagName") == "TD") {
                    $('.tooltipError').css('right', 'auto');
                    $('.tooltipError').css('top', 'auto');
                }
            }
        }
    });
    $(".errorValidate").mouseout(function () {
        $(this).parent().find(".tooltipError").remove();
    });
}
function QuitarTooltip() {
    $(".errorValidate").off("mouseover");
    $(".errorValidate").off("mouseout");

}

function EstablecerToolTipIconos() {
    var Add = $("#lnkAdd");
    var Edit = $(".lnkEdit");
    var Delete = $(".lnkDelete");
    var Detail = $(".lnkDetail");
    var Disable = $(".lnkDisable");
    var Invalidate = $(".lnkInvalidate");
    var GenerarCortesia = $(".lnkGenerarCortesia");

    if (Add != null) {
        Add.attr("title", "Adicionar");
    }
    if (Edit != null) {
        Edit.attr("title", "Editar");
    }
    if (Delete != null) {
        Delete.attr("title", "Eliminar");
    }
    if (Detail != null) {
        Detail.attr("title", "Detalle");
    }
    if (Disable != null) {
        Disable.attr("title", "Inactivar");
    }
    if (Invalidate != null) {
        Invalidate.attr("title", "Anular");
    }
    if (GenerarCortesia != null) {
        GenerarCortesia.attr("title", "Imprimir cortesia");
    }
}

function ReiniciarControlSesion() {
    $('#control').val("0");
}

function MostrarMensaje(Titulo, Mensaje, Tipo, funcion, parametro) {
    var Type = "";

    if (Tipo != undefined) {
        Type = Tipo;
    }

    swal({
        title: Titulo,
        text: Mensaje,
        type: Type,
        allowOutsideClick: false,
        allowEscapeKey: false
    }).then(function () {
        
        if (funcion != null) {
            window[funcion](parametro);
        }
    }).catch(swal.noop);

    
}

function MostrarMensajeHTML(Titulo, Mensaje, Tipo, funcion, parametro) {
    var Type = "";

    if (Tipo != undefined) {
        Type = Tipo;
    }

    swal({
        title: Titulo,
        html: Mensaje,
        type: Type,
        allowOutsideClick: false,
        allowEscapeKey: false
    }).then(function () {

        if (funcion != null) {
            window[funcion](parametro);
        }
    }).catch(swal.noop);


}


function CambioClave() {
    EjecutarAjax(urlBase + "Cuenta/CambioClave", "POST", null, "printPartialModal", { title: "Cambio de contraseña", url: urlBase + "Cuenta/ActualizarCambioClave", metod: "GET", func: "SuccessCambioClave" });
}

function SuccessCambioClave(rta) {

    if (rta.Correcto) {
        cerrarModal("modalCRUD");
        mostrarAlerta("Información", rta.Mensaje, "success");
        return;
    }
    MostrarMensaje("Atención", rta.Mensaje, "");
}


function MostrarConfirm(Titulo, Mensaje, FuncionAceptar, value) {

    swal({
        title: Titulo,
        text: Mensaje,
        showCancelButton: true,
        closeOnConfirm: true,
        allowOutsideClick: false,
        allowEscapeKey: false
    }).then(function () {
        window[FuncionAceptar](value);
    }).catch(swal.noop);

}

function MostrarConfirmSINO(Titulo, Mensaje, FuncionAceptar, FuncionCancelar, value) {

    swal({
        title: Titulo,
        text: Mensaje,
        showCancelButton: true,
        confirmButtonText: "SI",
        cancelButtonText: "NO",
        closeOnConfirm: false,
        closeOnCancel: false

    }).then(function () {
        window[FuncionAceptar](value);
    }, function (dismiss) {
        window[FuncionCancelar](value);
    }).catch(swal.noop);

}

function FormatoMoneda(Valor) {
    var decimals = 0;
    var decimal_sep = ",";
    var thousands_sep = ".";
    var n = Valor,
    c = isNaN(decimals) ? 2 : Math.abs(decimals), //if decimal is zero we must take it, it means user does not want to show any decimal
    d = decimal_sep || '.', //if no decimal separator is passed we use the dot as default decimal separator (we MUST use a decimal separator)

    t = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep, //if you don't want to use a thousands separator you can pass empty string as thousands_sep value

    sign = (n < 0) ? '-' : '',
    i = parseInt(n = Math.abs(n).toFixed(c)) + '',

    j = ((j = i.length) > 3) ? j % 3 : 0;

    return sign + '$ ' + (j ? i.substr(0, j) + t : '') + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : '');
}

function RemoverFormatoMoneda(Valor) {
    var retorno = 0;
    retorno = ReplaceAll(Valor, '-', '');
    retorno = ReplaceAll(retorno, '$', '');
    retorno = ReplaceAll(retorno, '.', '');
    retorno = ReplaceAll(retorno, ',', '');

    return parseInt(retorno.trim());
}

function MostrarMensajeRedireccion(Titulo, Mensaje, UrlRedireccion, Tipo) {

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
        if (funcion != null)
        {
            window[funcion](parameter);
        }
    }).catch(swal.noop);


}

function ReplaceAll(str, find, replace) {
    return str.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}
function escapeRegExp(str) {
    return str.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}

//RDSH: Recotna la fecha actual.
function ObtenerFechaActual() {
    var sysdate = new Date();
    var midate = " ";
    var h = sysdate.getHours();
    var m = sysdate.getMinutes();
    var s = sysdate.getSeconds();

    if (s <= 9) s = "0" + s;
    if (m <= 9) m = "0" + m;
    if (h <= 9) h = "0" + h;

    var day = sysdate.getDate();
    var month = sysdate.getMonth();
    var year = sysdate.getYear();

    if (year < 1900) year = 1900 + sysdate.getYear();
    month += 1;
    if (month < 10) month = '0' + month;
    if (day < 10) day = '0' + day;

    midate += year + "-" + month + "-" + day + " " + h + ":" + m + ":" + s;

    return midate;
}

//RDSH: Inhabilita las opciones de copiar pegar y cortar dentro de un text box
function InhabilitarCopiarPegarCortar(control) {
    $("#" + control).bind("cut copy paste", function (e) {
        e.preventDefault();
    });
}

//GALD: Inhabilita las opciones de copiar pegar y cortar dentro de un text box que contenga la clase 
function InhabilitarCopiarPegarCortarClase() {
    
    $(".tags").each(function (index, element) {
        
        InhabilitarCopiarPegarCortar(element.id + "_tagsinput");        
    });
}

function setTimePicker() {
    $(".dateTime").datetimepicker({
        format: 'HH:mm',
    });
}
//RDSH: Permite numeros y caractes especiales.
function NumeroYCaracterEspecial(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31 && (charCode < 33 || charCode > 57))
        return false;

    return true;
}

//Funcion para validar letras y numeros.
function EsAlfaNumerico(inputtxt) {

    if ((event.charCode >= 48 && event.charCode <= 57) || // 0-9
           (event.charCode >= 65 && event.charCode <= 90) || // A-Z
           (event.charCode >= 97 && event.charCode <= 122))  // a-z
        return true;

    return false;
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for(var i = 0; i <ca.length; i++) {
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

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays*24*60*60*1000));
    var expires = "expires="+ d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function ValidarEstadoPantalla() {
    var IdEstado = getCookie("EstadoPantalla");
    if (IdEstado.length > 0) {
        if (IdEstado == "1" && typeof $BODY != 'undefined') {
            if ($BODY.hasClass('nav-md')) {
                $SIDEBAR_MENU.find('li.active ul').hide();
                $SIDEBAR_MENU.find('li.active').addClass('active-sm').removeClass('active');
                $BODY.toggleClass('nav-md nav-sm');
            }
        }
    }
}

function validateFloatKeyPress(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = el.value.split(',');
    if (charCode != 44 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    //just one dot (thanks ddlab)
    if (number.length > 1 && charCode == 46) {
        return false;
    }
    //get the carat position
    var caratPos = getSelectionStart(el);
    var dotPos = el.value.indexOf(",");
    if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
        return false;
    }
    return true;
}

//thanks: http://javascript.nwbox.com/cursor_position/
function getSelectionStart(o) {
    if (o.createTextRange) {
        var r = document.selection.createRange().duplicate()
        r.moveEnd('character', o.value.length)
        if (r.text == '') return o.value.length
        return o.value.lastIndexOf(r.text)
    } else return o.selectionStart
}