$(function () {

    //envio de notificacion
    $("#txtCod").change(function () { 
        if($(this).val()!== "")
            EjecutarAjax(urlBase + "RedencionPuntos/ObtenerTarjetaPuntos", "GET", { consecutivo: $(this).val() }, "successSearch", null)

    });
    $("#btnGuardar").click(function () {
        EjecutarAjax(urlBase + "RedencionPuntos/RedimirProducto?codProducto=" + $("#ddlProductos option:selected").val() + "&consecutivo=" + $("#txtCod").val(), "GET", null, "successSave", null)

    });
});


function successSave(data) {
    if (data) {
        MostrarMensaje("Correcto", "El producto fue redimido con exito, tiene (" + (parseInt($("#txtPuntos").val()) - parseInt($("#ddlProductos option:selected").text().split('-')[1].trim())) + ") puntos disponibles", "success");
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
    $("#txtConsecutivo").val("");
    $("#txtPuntos").val("");
    $("#txtCod").val("");
    $("#snapshot").hide();
    $("#snapshot").attr("src", "#");
    $("#my_camera").show();
    $("#btnCaptura").show();
    $("#btnNuevaCaptura").hide();

    $(".otherField").hide();
}

function successSearch(data) {
    if (data.rta) {
        $("#txtName").val(data.obj.Cliente.Nombre);
        $("#txtMail").val(data.obj.Cliente.Correo);
        $("#txtPhone").val(data.obj.Cliente.Telefono);
        $("#txtBirthday").val(data.obj.Cliente.FechaNacimiento);
        $("#txtGender").val(data.obj.Cliente.Genero);
        $("#hdConsecutivo").val(data.obj.Cliente.Consecutivo);
        $("#txtPuntos").val(data.obj.Cliente.Puntos);
        if (data.obj.Cliente.Consecutivo === null)
            $("#btnGuardar").hide();
        else
            $("#btnGuardar").show();
        $("#txtName").focus();
        if (data.obj.Cliente.FotoTexto !== "")
        {
            $("#snapshot").show();
            $("#snapshot").attr("src", data.obj.Cliente.FotoTexto);
        }
        var options = null;
        $.each(data.obj.Productos, function (i, v) {
            options += "<option value='" + v.Id + "'>" + v.Nombre + " - " + v.CodSAP + "</option>";
        });
        $("#ddlProductos").html(options);

        $(".otherField").show();
    } else {
        $(".otherField").hide();
        MostrarMensaje("Error", data.msj, "error");
    }
}
