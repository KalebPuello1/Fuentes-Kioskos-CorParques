$(function () {

    //envio de notificacion
    $("#txtDoc").change(function () { 
        if($(this).val()!== "")
            EjecutarAjax(urlBase + "BloqueoTarjeta/ObtenerTarjeta", "GET", { doc: $(this).val() }, "successSearch", null)

    });
    $("#btnGuardar").click(function () {
        EjecutarAjax(urlBase + "BloqueoTarjeta/BloquearTarjeta?Consecutivo=" + $("#hdConsecutivo").val(), "GET", null, "successSave", null)

    });
});


function successSave(data) {
    if (data) {
        MostrarMensaje("Correcto", "La tarjeta ha sido bloqueada", "success");
        Limpiar();
    }
    else
        MostrarMensaje("Error", "Se ha presentado un error al guardar la informacion. Por favor intentelo mas tarde", "error");
}

function Limpiar() {
    $("#txtName").val("");
    $("#txtMail").val("");
    $("#txtPhone").val("");
    $("#txtBirthday").val("");
    $("#txtGender").val("");
    $("#txtDoc").val("");
    $("#txtDate").val("");
    $("#hdConsecutivo").val("");
    $("#txtReload").val("");
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
        $("#txtBirthday").val(moment(data.obj.FechaNacimiento).format("YYYY-MM-DD"));
        $("#txtGender").val(data.obj.Genero);
        $("#txtDate").val(data.obj.Fecha);
        $("#hdConsecutivo").val(data.obj.Consecutivo);
        $("#txtReload").val(FormatoMoneda(data.obj.Recarga));
        if (data.obj.Consecutivo == null)
            $("#btnGuardar").hide();
        else
            $("#btnGuardar").show();
        $("#txtName").focus();
        if (data.obj.FotoTexto != "")
        {
            $("#snapshot").show();
            $("#snapshot").attr("src", data.obj.FotoTexto);
        }
        $(".otherField").show();
    } else {
        $(".otherField").hide();
        MostrarMensaje("Error", data.msj, "error");
    }
}
