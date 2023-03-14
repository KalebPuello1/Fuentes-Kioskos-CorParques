$(function () {
    $("#div_message_error").hide();
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "Perfil/GetPartial", "GET", null, "printPartialModal", {
            title: "Crear Perfil", url: urlBase + "Perfil/Insert",
            metod: "GET", func: "successInsert", func2: "setEventTreView", param: $('#hdListaMenus'), param: $('#hdIdPerfilConflicto')
        });
    });
    setEventEdit();
});
function setEventEdit() {
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "Perfil/Obtener", "GET", { id: $(this).data("id") }, "printPartialModal", {
            title: "Editar Perfil", url: urlBase + "Perfil/Update", metod: "GET",
            func: "successUpdate", func2: "setEventTreView", param: $('#hdListaMenus')
        });
    });

    $(".lnkDelete").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea desactivar este perfil?", "Delete", $(this).data("id"));
        //if (confirm("¿Está seguro que desea inactivar esta Perfil?"))
            //EjecutarAjax(urlBase + "Perfil/UpdateEstado", "GET", { id: $(this).data("id") }, "successUpdate", null);
    });


    $(".lnkSegregacion").click(function () {
        EjecutarAjax(urlBase + "Perfil/PerfilActivos", "GET", { IdPerfil: $(this).data("id") }, "printPartialModal", {
            title: "Segregación de funciones", url: urlBase + "Perfil/UpdateSegregacion", metod: "GET",
            func: "successSegregacion", func2: "setEventTreView", param: $('#hdIdPerfilConflicto')
        });
    });
   

    //$(".lnkSegregacion").click(function () {
    //    EjecutarAjax(urlBase + "Perfil/PerfilActivos", "GET", { IdPerfil: $(this).data("id") }, "printPartialModal", {
    //        title: "Editar Perfil", url: urlBase + "Perfil/Update", metod: "GET", func: "successUpdate", modalLarge: true
    //    });
    //});
}

function Delete(Id) {
    EjecutarAjax(urlBase + "Perfil/UpdateEstado", "GET", { id: Id }, "successUpdate", null);
}

function successInsert(rta) {

    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Perfil/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Operación realizada con éxito.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}



function successUpdate(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Perfil/GetList", "GET", null, "printPartial", {
            div: "#listView", func: "setEventEdit"
        });
        cerrarModal("modalCRUD");
        mostrarAlerta("Operación realizada con éxito.")
    }
    else {
        cerrarModal("modalCRUD");
        mostrarAlerta("Operación sin exito.");
    }
}

function successSegregacion(rta) {
    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Perfil/GetList", "GET", null, "printPartial", {div: "#listView", func: "setEventEdit"
        });
        cerrarModal("modalCRUD");
        mostrarAlerta("Operación realizada con éxito!!.")
    }
    else {
        cerrarModal("modalCRUD");
        mostrarAlerta("Operación sin exito!!.")
        $("#div_message_error").show();
    }

}

