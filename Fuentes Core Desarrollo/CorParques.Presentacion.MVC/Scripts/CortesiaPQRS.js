//Registrar usuarios visitantes
$(function () {
    Dropzone.autoDiscover = false;
    var nombreArchivo = "";
    $("#btnRegistrarVisitante").click(function () {
        document.getElementById("btnRegistrarVisitante").style.visibility = "hidden";

        EjecutarAjax(urlBase + "CortesiaPQRS/CrearUsuarioModal", "GET", null, "printPartial", { div: "#listView", func: "voidFunction" });
        /*        EjecutarAjax(urlBase + "CortesiaPQRS/CrearUsuarioModal", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });*/
        //EjecutarAjax(urlBase + "CortesiaPQRS/CrearUsuarioModal", "GET", null, "printPartialModal", {
        //    title: "Crear Usuario Visitante", url: urlBase + "CortesiaPQRS/InsertarUsuarioVisitante", metod: "POST",  func: "successInsertUser", modalLarge: true
        //});
    });

    $("#dropzoneElements").dropzone({
        url: urlBase + "CortesiaPQRS/GuardarArchivoTemporal",
        maxFilesize: 2,
        uploadMultiple: false,
        addRemoveLinks: true,
        dictRemoveFile: "X",
        renameFilename: function (file) {
            nombreArchivo = new Date().getTime() + "_" + file;
            return nombreArchivo;
        },
        acceptedFiles: ".txt",
        removedfile: function (file) {
            EjecutarAjax(urlBase + "CortesiaPQRS/RemoverArchivoTemporal", "GET", { name: nombreArchivo }, "voidFunction", null);
            file.previewTemplate.remove();
            $("#Imagen").val("");
            nombreArchivo = "";
        },
        success: function () {
            $("#dropzoneElements").removeAttr("style");
            $("#Imagen").val(nombreArchivo);
        }
    });
});


$("#btnCancelar").click(function () {
    MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "Cancelar", "");
});

$("#btnAceptar").click(function () {

    var documento = $("#Imagen").val();
    documento = typeof document == 'undefined' ? "" : documento;
    var TipoDocumento = $("#TipoDocumento").val();
    var NumeroDocumento = $("#NumeroDocumento").val();
    var Nombres = $("#Nombres").val();
    var Apellidos = $("#Apellidos").val();
    var NumeroTicket = $("#NumeroTicket").val();
    var Cantidad = $("#Cantidad").val();
    var Descripcion = $("#Descripcion").val();

    var valida = true;

    if (documento.length == 0) {
        $('#dropzoneElements').attr('style', 'border: solid #ef0b0b 1px !important');
        valida = false;
    }

    if (NumeroDocumento.length == 0) {
        $("#NumeroDocumento").attr("data-mensajeerror", "Este campo es obligatorio");
        $("#NumeroDocumento").addClass("errorValidate");
        valida = false;
    }

    if (Nombres.length == 0) {
        $("#Nombres").attr("data-mensajeerror", "Este campo es obligatorio");
        $("#Nombres").addClass("errorValidate");
        valida = false;
    }

    if (Apellidos.length == 0) {
        $("#Apellidos").attr("data-mensajeerror", "Este campo es obligatorio");
        $("#Apellidos").addClass("errorValidate");
        valida = false;
    }

    if (NumeroTicket.length == 0) {
        $("#NumeroTicket").attr("data-mensajeerror", "Este campo es obligatorio");
        $("#NumeroTicket").addClass("errorValidate");
        valida = false;
    }

    if (Cantidad.length == 0) {
        $("#Cantidad").attr("data-mensajeerror", "Este campo es obligatorio");
        $("#Cantidad").addClass("errorValidate");
        valida = false;
    }

    if (Descripcion.length == 0) {
        $("#Descripcion").attr("data-mensajeerror", "Este campo es obligatorio");
        $("#Descripcion").addClass("errorValidate");
        valida = false;
    }

    if (valida)
        MostrarConfirm("Importante!", "¿Está seguro de registrar este usuario?", "InsertarUsuarioVisitante", {
            TipoDocumento: TipoDocumento, NumeroDocumento: NumeroDocumento, Nombres: Nombres, Apellidos: Apellidos, NumeroTicket: NumeroTicket, Cantidad: Cantidad, Descripcion: Descripcion, Archivo: documento
        });
    else
        MostrarMensaje("Importante", "Hay inconsistencias en el formulario, revise los campos demarcados con color rojo.", "error");

});


function Cancelar() {
    var nombre = $("#Imagen").val();
    if (nombre.length > 0)
        EjecutarAjax(urlBase + "CortesiaPQRS/RemoverArchivoTemporal", "GET", { name: nombre }, "voidFunction", null);
    window.location = urlBase + "CortesiaPQRS/Index";
}

function InsertarUsuarioVisitante(obj) {
    console.log(obj);
    debugger;
    EjecutarAjax(urlBase + "CortesiaPQRS/InsertarUsuarioVisitante", "GET", { TipoDocumento: obj.TipoDocumento, NumeroDocumento: obj.NumeroDocumento, Nombres: obj.Nombres, Apellidos: obj.Apellidos, NumeroTicket: obj.NumeroTicket, Cantidad: obj.Cantidad, Descripcion: obj.Descripcion, Archivo: obj.Archivo }, "SuccessUsuarioArchivo", null);
}



function SuccessUsuarioArchivo(rta) {
    if (rta.length > 0) {
        Dropzone.forElement("#dropzoneElements").removeAllFiles(true);
        MostrarMensaje("Importante", rta, "error");
    }
    else
        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "CortesiaPQRS/Index", "success");
}


//function successInsertUser(rta) {
//    console.log(rta);
//    if (rta.Correcto) {
//        EjecutarAjax(urlBase + "CortesiaPQRS/ObtenerListaUsuarioVisitante", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
//        cerrarModal("modalCRUD");
//        mostrarAlerta("El usuario visitante fue creado con éxito.");
//        window.location = urlBase + "CortesiaPQRS/Index";
//    } else {
//        $("#NumeroDocumento").addClass("errorValidate");
//        $("#NumeroDocumento").attr("data-mensajeerror", "El usuario visitante ya existe");
//        mostrarTooltip();
//    }
//}

//function mostrarTooltip() {
//    $(".errorValidate:first").focus();
//    $(".errorValidate").mouseover(function () {
//        if ($(this).attr("data-mensajeerror") !== undefined) {
//            if ($(this).attr("data-mensajeerror").length > 0) {
//                $(this).parent().append("<div class='tooltipError'>" + $(this).attr("data-mensajeerror") + "</div>");
//                if ($(this).parent().prop("tagName") == "TD") {
//                    $('.tooltipError').css('right', 'auto');
//                    $('.tooltipError').css('top', 'auto');
//                }
//            }
//        }
//    });
//    $(".errorValidate").mouseout(function () {
//        $(this).parent().find(".tooltipError").remove();
//    });
//}

//function QuitarTooltip() {
//    $(".errorValidate").off("mouseover");
//    $(".errorValidate").off("mouseout");

//}

//function setEventEdit() {
//    EstablecerToolTipIconos();
//}

//function EstablecerToolTipIconos() {
//    var Add = $("#lnkAdd");
//    var Edit = $(".lnkEdit");
//    var Delete = $(".lnkDelete");
//    var Detail = $(".lnkDetail");
//    var Disable = $(".lnkDisable");
//    var Invalidate = $(".lnkInvalidate");
//    var GenerarCortesia = $(".lnkGenerarCortesia");

//    if (Add != null) {
//        Add.attr("title", "Adicionar");
//    }
//    if (Edit != null) {
//        Edit.attr("title", "Editar");
//    }
//    if (Delete != null) {
//        Delete.attr("title", "Eliminar");
//    }
//    if (Detail != null) {
//        Detail.attr("title", "Detalle");
//    }
//    if (Disable != null) {
//        Disable.attr("title", "Inactivar");
//    }
//    if (Invalidate != null) {
//        Invalidate.attr("title", "Anular");
//    }
//    if (GenerarCortesia != null) {
//        GenerarCortesia.attr("title", "Imprimir cortesia");
//    }
//}

////Listar usuarios visitantes
//$(function () {
//    $("#btnConsultarVisitante").click(function () {
//        EjecutarAjax(urlBase + "CortesiaPQRS/ObtenerListaUsuarioVisitante", "GET", null, "printPartialModal", {
//            title: "Listar Usuario Visitante", url: urlBase + "CortesiaPQRS/ObtenerListaUsuarioVisitante", metod: "GET", func: "successGetUser", modalLarge: true, hidesave: true
//        });
//    });
//    setEventEdit();
//});


//function successGetUser(rta) {
//}

