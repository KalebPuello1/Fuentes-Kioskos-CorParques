$(function () {
    
    Dropzone.autoDiscover = false;
    var nombreArchivo = "";
    //setFileLoad(true);

    //var myDropzone = new Dropzone("#dropzoneElements", { url: "/ConvenioConsecutivo/saveFile" });

    $("#dropzoneElements").dropzone({
        url: urlBase + "ConvenioConsecutivo/saveFile",
        maxFilesize: 2,
        uploadMultiple: false,
        addRemoveLinks: true,
        dictRemoveFile: "X",
        renameFilename: function (file) {
            nombreArchivo = new Date().getTime()+ "_" + file;
            return nombreArchivo;
        },
        acceptedFiles: ".xlsx",
        removedfile: function (file) {
            EjecutarAjax(urlBase + "ConvenioConsecutivo/RemoveFile", "GET", { name: nombreArchivo }, "voidFunction", null);
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

//function setFileLoad(prev) {
//    fileDropZone($("#dropzoneElements"), "/ConvenioConsecutivo/saveFile", "ConvenioConsecutivo/RemoveFile", ".xlsx", $("#Imagen"), prev, "/Archivos/Temp/");
//}

$("#btnCancelar").click(function () {
    MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "Cancelar", "");
});

$("#btnAceptar").click(function () {
    debugger;
    var documento = $("#Imagen").val();
    documento = typeof document == 'undefined' ? "" : documento;
    var Convenio = $("#ddlConvenio").val();
    var valida = true;

    if (documento.length == 0) {
        $('#dropzoneElements').attr('style', 'border: solid #ef0b0b 1px !important');
        valida = false;
    }

    if (Convenio.length == 0) {
        $("#ddlConvenio").attr("data-mensajeerror", "Este campo es obligatorio");
        $("#ddlConvenio").addClass("errorValidate");
        valida = false;
    }
    

    if (valida)
        MostrarConfirm("Importante!", "¿Está seguro de subir este documento? ", "SubirArchivo", { doc: documento, Conv: Convenio });
    else
        MostrarMensaje("Importante", "Hay inconsistencias en el formulario, revise los campos demarcados con color rojo.", "error");
    
});


function Cancelar() {
    var nombre = $("#Imagen").val();
    if (nombre.length > 0)
        EjecutarAjax(urlBase + "ConvenioConsecutivo/RemoveFile", "GET", { name: nombre }, "voidFunction", null);
    window.location = urlBase + "ConvenioConsecutivo/Index";
}

function SubirArchivo(obj) {

    EjecutarAjax(urlBase + "ConvenioConsecutivo/SubirArchivo", "GET", { archivo: obj.doc, convenio: obj.Conv }, "SuccessArchivo", null);
}

function SuccessArchivo(rta) {
    if (rta.length > 0) {
        Dropzone.forElement("#dropzoneElements").removeAllFiles(true);
        MostrarMensaje("Importante", rta, "error");
    }
    else
        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "ConvenioConsecutivo", "success");
}