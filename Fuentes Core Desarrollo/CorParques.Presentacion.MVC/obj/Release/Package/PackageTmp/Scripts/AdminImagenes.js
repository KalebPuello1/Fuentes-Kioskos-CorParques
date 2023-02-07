$(function () {
    setEventEdit();
});

function setFileLoad(prev) {

    fileDropZone($("#dropzoneElement"), urlBase + "AdminImagenes/SaveFile", "AdminImagenes/RemoveFile", "image/*", $("#NombreImagen"), prev, urlBase + "Images/Productos/");

}

function setEventEdit() {
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "Productos/GetById", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar producto", url: urlBase + "Productos/Update", metod: "GET", func: "successUpdate" });
    });
}

function successUpdate(rta) {

    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Productos/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("El producto fue modificado con éxito.")
    }
}
function listarPantallasXcategoria() {

    var IdCategoria = $("#IdCategoria").val();

    if (IdCategoria !== null && IdCategoria !== '') {


        EjecutarAjaxJson(urlBase + "AdminImagenes/ListarVisualPantallas", "post", {
            IdCategoria: IdCategoria
        }, "successPantallasXcategoria", null);
    }
    else {
        var select = document.getElementById("IdCodPantalla");
        document.getElementById("IdCodPantalla").innerHTML = ""

        var option1 = document.createElement("option");
        option1.text = "*Seleccione..."; option1.value = "";
        option1.value = "";
        select.add(option1);

        ValidarPantalla();
    }

}

function GuardarPantallasXcategoria() {
    modelo = new Object();
    modelo.Id = $("#Id").val();
    modelo.NombreImagen = $("#NombreImagen").val();
    modelo.Frecuencia = $("#Frecuencia").val();
    modelo.CodPantalla = $("#CodPantalla").val();

    /*if (modelo.NombreImagen !== null && modelo.NombreImagen !== '') {*/

    EjecutarAjaxJson(urlBase + "AdminImagenes/Update", "post", {
        modelo: modelo
    }, "successUpdatePantallasXcategoria", null);

   /* }*/

}

function LimpiarPantallasXcategoria() {
    window.location = urlBase + "AdminImagenes/Index";

}

function successUpdatePantallasXcategoria(rta) {
    if (rta !== null) {

        window.location = urlBase + "AdminImagenes/Index";
    }
    else {

    }

    ValidarPantalla();
}
function successPantallasXcategoria(rta) {
    if (rta !== null) {

        var select = document.getElementById("IdCodPantalla");
        document.getElementById("IdCodPantalla").innerHTML = ""

        var option1 = document.createElement("option");
        option1.value = "";
        option1.text = "*Seleccione...";
        select.add(option1);

        $.each(rta, function (i, item) {

            var option2 = document.createElement("option");
            option2.text = item.CodPantalla;
            option2.value = item.CodPantalla;
            select.add(option2);
        });


    }
    else {

    }

    ValidarPantalla();
}

function ValidarPantalla() {
    var IdCodPantalla = $("#IdCodPantalla").val();
    if (IdCodPantalla !== null && IdCodPantalla !== '') {

        EjecutarAjaxJson(urlBase + "AdminImagenes/ObtenerImagenAdminXCodpantalla", "post", {
            CodPantalla: IdCodPantalla
        }, "successImagenAdmin", null);


    }
    else {
        div = document.getElementById('DetalleContent');
        div.style.display = 'none';
    }


}
function setEventStatus() {
    $("#EstadoId").change(function () {
        $("#DescripcionCierre").parent().parent().hide();
        $("#DescripcionCierre").removeClass("required");
        if ($(this).val() == "4") {
            $("#DescripcionCierre").parent().parent().show();
            $("#DescripcionCierre").addClass("required");
        }
    });
}
function successImagenAdmin(rta) {
    //if (rta !== null) {
    //    //div = document.getElementById('divcontenidoimg');
    //    //div.style.display = 'block';


    //    //$("#txtFrecuencia").val(rta.Frecuencia);
    //    //$("#Imagen").val(rta.NombreImagen);
    //    //var prueba = $("#Imagen").val();
    //    //setFileLoad(true);

    //}

    if (isNaN(rta)) {
        div = document.getElementById('DetalleContent');
        div.style.display = 'block';
        $("#DetalleContent").html(rta);

    }
}

function SuccessUsuarioImagenArchivo(rta, tipocortesia) {
    if (rta === undefined) {
    }
    else {
        if (!rta.Correcto) {
            Dropzone.forElement("#dropzoneElements").removeAllFiles(true);
            MostrarMensaje("Importante", rta.Mensaje, "error");
        }
        else {
            EjecutarAjax(urlBase + "Cortesia/ImagenArchivo", "GET", null, "printPartial", { func: "BotonCancelarUsuario" });
            cerrarModal("modalCRUD");
            if (rta.Mensaje !== undefined && rta.Mensaje !== null && rta.Mensaje !== "") {
                MostrarMensaje("Importante", rta.Mensaje, "warning");
              

            }
            else {
                mostrarAlerta("Se agrego la imagen con éxito.");
            }

        }
    }
}