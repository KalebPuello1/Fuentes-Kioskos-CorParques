
var aplicaCheck = false;
var idCheck = 0;

$(function () {
    $("#div_message_error").hide();
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "Cargos/GetPartial", "GET", null, "printPartialModal", {
            title: "Crear Cargo", url: urlBase + "Cargos/Insert",
            metod: "GET", func: "successInsert", func2: "setEventTreView", param: $('#hdListaPerfiles')
        });
    });

    //$(".perfil").change(function () {

    //    if (!$(this).prop('checked')) {
    //        var id = str.substring($(this).id.indexOf("_"), $(this).id.lenth);
    //        ValidarPerfil(id);
    //    }

    //});



    setEventEdit();
});
function setEventEdit() {
    $(".lnkEditar").click(function () {
        EjecutarAjax(urlBase + "Cargos/Obtener", "GET", { IdCargo: $(this).data("id") }, "printPartialModal", {
            title: "Editar Cargo", url: urlBase + "Cargos/Update",
            metod: "GET", func: "successUpdate", func2: "setEventTreView", param: $('#hdListaPerfiles')
        });
    });    
}




function successInsert(rta) {

    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Cargos/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Operación realizada con éxito.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}

function CheckSeleccionados() {
    var seleccionado = '';


    $('.perfil').each(function (index, element) {
        if ($(this).is(':checked')) {
            if (seleccionado == '') {
                seleccionado = $(this).val();
            } else {
                seleccionado = seleccionado + ',' + $(this).val();
            }
        }
    });

    return seleccionado;

}

function successUpdate(rta) {
    if (rta.Correcto) {

        if (rta.Mensaje.length > 0) {
            MostrarConfirm("Importante",rta.Mensaje + " ¿desea continuar?", "continuarPerfil", rta.Elemento);            
        } else {
            EjecutarAjax(urlBase + "Cargos/GetList", "GET", null, "printPartial", {
                div: "#listView", func: "setEventEdit"
            });
            cerrarModal("modalCRUD");
            mostrarAlerta("Operación realizada con éxito.")
        }

    }
    else {
        cerrarModal("modalCRUD");
        mostrarAlerta("Operación sin exito.");
    }
}

function ValidarPerfil(Id) {

    var seleccionado = CheckSeleccionados();

    EjecutarAjax(urlBase + "Cargos/ValidarPerfil", "GET", { idPerfil: Id, hdListaPerfiles: seleccionado }, "successValidarPerfil", null);

}

function successValidarPerfil(rta) {
    if (!rta.Correcto) {
        MostrarConfirm("Importante", "El perfil presenta conflito con los siguientes perfile " + rta.Mensaje + " ¿desea continuar?", "continuarPerfil", rta.Elemento);
    } else {
        aplicaCheck = true;
        $("#chkPerfil_" + rta.Elemento).prop('checked', true);
    }
}

function continuarPerfil(id) {
    EjecutarAjax(urlBase + "Cargos/UpdateEmail", "GET", { idCargo: id, hdListaPerfiles: $('#hdListaPerfiles').val() }, "successUpdate", null);
}



