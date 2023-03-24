$(function () {

    Webcam.set({
        width: 320,
        height: 240,
        crop_width: 320,
        crop_height: 240,

        image_format: 'png',
        jpeg_quality: 50
    });
    Webcam.attach('#my_camera');
    loadCalendar();

    //envio de notificacion
    $("#txtDoc").change(function () { 
        if ($(this).val() !== "")
            EjecutarAjax(urlBase + "Fidelizacion/Buscar", "GET", { doc: $(this).val() }, "successSearch", null);

    });
    $("#btnGuardar").click(function () {
        if (validarFormulario("frmFidel")) {
            if ($("#snapshot").prop("src") === "")
            {
                MostrarMensajeRedireccion("Importante", "La foto es obligatoria, por favor tomela para poder guardar la informacion", null, "warning");
            }
            else {
                EjecutarAjax(urlBase + "Fidelizacion/Guardar", "POST", JSON.stringify({ modelo: CrearObjeto() }), "successSave", null)
            }
        }

    });
     $("#btnCaptura").click(function () {
         Webcam.snap(function (data_uri) {
             $("#snapshot").show();
             $("#snapshot").attr("src", data_uri);
             $("#my_camera").hide();
             $("#btnNuevaCaptura").show();
             $("#btnCaptura").hide();
         });

     });
     $("#btnNuevaCaptura").click(function () {
        $("#snapshot").hide();
        $("#snapshot").attr("src", "#");
        $("#my_camera").show();
        $("#btnCaptura").show();
        $("#btnNuevaCaptura").hide();

    });
    var doc = getUrlParameter('doc');
    if (doc !== undefined) {
        $("#txtDoc").val(doc);
        EjecutarAjax(urlBase + "Fidelizacion/Buscar", "GET", { doc: doc }, "successSearch", null);
    }
});

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
};
function CrearObjeto(){
    return {
        Documento:$("#txtDoc").val(),
        Correo:$("#txtMail").val(),
        Nombre:$("#txtName").val(),
        Telefono:$("#txtPhone").val(),
        FechaNacimiento:$("#txtBirthday").val(),
        Genero: $("#txtGender").val(),
        Direccion: $("#txtAddress").val(),
        FotoTexto: ($("#snapshot").attr("src")==="#"?"":$("#snapshot").attr("src"))


    }
}

function successSave(data) {
    if (data) {
        MostrarMensaje("Correcto", "La informacion del cliente se ha guardado con éxito", "success");
        Limpiar();
    }
    else
        MostrarMensaje("Error", "Se ha presentado un error al guardar la informacion. Por favor intentelo mas tarde", "Error");
}

function Limpiar() {
    $("#txtName").val("");
    $("#txtMail").val("");
    $("#txtPhone").val("");
    $("#txtBirthday").val("");
    $("#txtAddress").val("");
    $("#txtGender").val("");
    $("#txtDoc").val("");
    $("#snapshot").hide();
    $("#snapshot").attr("src", "#");
    $("#my_camera").show();
    $("#btnCaptura").show();
    $("#btnNuevaCaptura").hide();

    $(".otherField").hide();
}

function successSearch(data) {
    if (data.rta) {
        $("#txtName").val(data.obj.Nombre);
        $("#txtMail").val(data.obj.Correo);
        $("#txtPhone").val(data.obj.Telefono);
        $("#txtBirthday").val(data.obj.FechaNacimiento);
        $("#txtGender").val(data.obj.Genero);
        $("#txtAddress").val(data.obj.Direccion);
        $("#txtName").focus();
        if (data.obj.FotoTexto != "")
        {
            $("#snapshot").show();
            $("#snapshot").attr("src", data.obj.FotoTexto);
            $("#my_camera").hide();
            $("#btnNuevaCaptura").show();
            $("#btnCaptura").hide();
        }
        $(".otherField").show();
    } else {
        $(".otherField").hide();
        MostrarMensaje("Error", data.msj, "error");
    }
}
