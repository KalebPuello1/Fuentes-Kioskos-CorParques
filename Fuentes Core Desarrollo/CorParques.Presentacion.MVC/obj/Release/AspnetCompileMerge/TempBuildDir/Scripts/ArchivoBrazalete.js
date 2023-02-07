/// <reference path="General.js" />

$(function () {
    $("#div_message_error").hide();
    setEventEdit();
});

function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkEdit").unbind("click");
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "BrazaleteArchivo/GetById", "GET", { idProducto: $(this).data("id") }, "printPartialModal", { title: $(this).data("nombre"), url: null, metod: "GET", func: null, modalLarge: false, hideSaveGeneric: true });
        //Muestra el guardar que arma el detalle del pedido.
        $("#btnSaveCustomizable").show();

        //Ejecuta el armado del objeto convenio detalle y el guardado a base de datos.
        $('#btnSaveCustomizable').unbind('click');
        $("#btnSaveCustomizable").click(function () {
            if (validarFormulario("modalCRUD .modal-body")) {
                if (window.FormData != typeof undefined) {
                    var fileUpload = $("#idArchivo").get(0);
                    var files = fileUpload.files;
                    var fileData = new FormData();
                    for (var i = 0; i < files.length; i++) {
                        fileData.append(files[i].name, files[i]);
                    }
                    fileData.append('observacion', $("#observacionArchivo").val());
                    fileData.append('idProducto', $("#idProducto").val());

                    iniciarProceso();
                    $.ajax({
                        url: urlBase + 'BrazaleteArchivo/Update',
                        type: "POST",
                        contentType: false,
                        processData: false,
                        data: fileData,
                        success: function (rta) {
                            successUpdatePuntoBrazalete(rta);
                            finalizarProceso();
                        },
                        error: function (err) {
                            alert(err.statusText);
                            finalizarProceso();
                        }
                    });


                } else {
                    alert("FormData no es soportado por el navegador.");
                }
            }
        });
        
    });

    $(".lnkDescargar").unbind("click");
    $(".lnkDescargar").click(function () {
        var nombre = $(this).data("nombre");
        window.location = '/BrazaleteArchivo/Download?nombre=' + nombre;
    });
}


function successUpdatePuntoBrazalete(rta) {
    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "BrazaleteArchivo/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Edición Exitosa.")
    } else {
        $("#lbl_message_error").html("El grupo ya existe.");
        $("#div_message_error").show();
    }
}



