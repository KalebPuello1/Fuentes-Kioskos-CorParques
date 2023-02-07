//Registrar usuarios visitantes
$(function () {
    Dropzone.autoDiscover = false;    
    $("#dropzoneElements").dropzone({
        url: urlBase + "Cortesia/GuardarArchivoTemporal",
        maxFilesize: 2,
        uploadMultiple: false,
        addRemoveLinks: true,
        dictRemoveFile: "X",
        renameFilename: function (file) {
            nombreArchivo = new Date().getTime() + "_" + file;
            return nombreArchivo;
        },
        acceptedFiles: ".pdf,.jpeg,.jpg,.png",
        removedfile: function (file) {
            EjecutarAjax(urlBase + "Cortesia/RemoverArchivoTemporal", "GET", { name: nombreArchivo }, "voidFunction", null);
            file.previewTemplate.remove();
            $("#Imagen").val("");
            nombreArchivo = "";
        },
        success: function () {
            $("#dropzoneElements").removeAttr("style");
            $("#Imagen").val(nombreArchivo);
            $("#Archivo").val(nombreArchivo);
        }
    });
});

function Cancelar() {
    var nombre = $("#Imagen").val();
    if (nombre.length > 0)
        EjecutarAjax(urlBase + "Cortesia/RemoverArchivoTemporal", "GET", { name: nombre }, "voidFunction", null);
    window.location = urlBase + "Cortesia/Index";
}

function SuccessUsuarioArchivo(rta) {
    if (rta === undefined) {
    }
    else
    {
        if (!rta.Correcto) {
            Dropzone.forElement("#dropzoneElements").removeAllFiles(true);
            MostrarMensaje("Importante", rta.Mensaje, "error");
        }
        else {
            EjecutarAjax(urlBase + "Cortesia/ObtenerListaUsuarioVisitante", "GET", null, "printPartial", { div: "#listView", func: "BotonCancelarUsuario" });
            cerrarModal("modalCRUD");
            mostrarAlerta("Se agrego la cortesia con éxito.");
        }
    }
}

function BotonCrearUsuario() {
        cerrarModal("modalCRUD");
    EjecutarAjax(urlBase + "Cortesia/CrearUsuarioModal", "GET", null, "printPartialModal", {
        title: "Adicionar Cortesia", url: null,
        metod: "GET", func: null, modalLarge: true, hidesave: true
    })
        cerrarModal("modalCRUD");
}

function BotonCancelarUsuario() {
    cerrarModal("modalCRUD");
}

function BotonGuardarUsuario() {

    if (validarFormulario("modalCRUD .modal-body")) {
        iniciarProceso();
        var formData = ObtenerObjeto("modalCRUD .modal-body form");
        $.ajaxSetup({ cache: false });
        var data = formData;
        $.ajax({
            ContentType: "application/json",
            url: urlBase + 'Cortesia/InsertarUsuarioVisitante',
            type: 'GET',
            data: data,
            success: function (answer) {
                finalizarProceso()
                SuccessUsuarioArchivo(answer)
            },
            error: function (jqXHR, exception) {
                MostrarMensaje("Importante", "Intente nuevamente ó contáctese con el Administrador", "error");
                finalizarProceso();
            }
        });
    }
}
 
