$(function () {
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "Usuarios/GetPartial", "GET", null, "printPartialModal", {
            title: "Crear Usuario", url: urlBase + "Usuarios/Insert", metod: "GET", func: "successInsertUser", func2: "setEventTreView", param: $('#hdListaPuntos'),  modalLarge: true
        });
    });
    setEventEdit();
});



function successInsertUser(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Usuarios/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("El usuario fue creado con éxito.");
    } else {        
        $("#NombreUsuario").addClass("errorValidate");
        $("#NombreUsuario").attr("data-mensajeerror", "El usuario ya existe");
        mostrarTooltip();
    }
}


function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "Usuarios/GetById", "GET", { id: $(this).data("id") }, "printPartialModal", {
            title: "Editar Usuario", url: urlBase + "Usuarios/Update", metod: "GET", func: "successUpdate", func2: "setEventTreView", param: $('#hdListaPuntos'), modalLarge: true
        });
    });
    $(".lnkEditPerfilUsuario").click(function () {
        EjecutarAjax(urlBase + "Usuarios/GetByIdPerfilUsuario", "GET", { id: $(this).data("id") }, "printPartialModal", {
            title: "Editar Perfil Usuario", url: urlBase + "Usuarios/UpdatePerfilUsuario", metod: "GET", func: "successUpdatePerfilUsuario", func2: "setEventTreView", param: $('#hdListaPerfiles'), modalLarge: true
        });
    });
    $(".lnkDisable").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea desactivar este usuario?", "Delete", $(this).data("id"));
        //if (confirm("Está seguro que desea eliminar este usuario?"))
        //    EjecutarAjax(urlBase + "Usuarios/Delete", "GET", { id: $(this).data("id") }, "successDelete", null);
    });
    $(".lnkDesbloquear").click(function () {
        MostrarConfirm("Importante", "¿Está seguro de desbloquear este usuario?", "Desbloqueo", $(this).data("id"));
        //if (confirm("Está seguro de desbloquear este usuario?"))
        //    EjecutarAjax(urlBase + "Usuarios/Desbloqueo", "GET", { id: $(this).data("id") }, "successDesbloqueo", null);
    });
    $(".lnkResetpsw").click(function () {
        MostrarConfirm("Importante", "¿Está seguro de resetear la clave del core?", "Resetpwd", $(this).data("id"));
        //if (confirm("Está seguro de resetear la clave del core?"))
        //    EjecutarAjax(urlBase + "Usuarios/Reseteopwd2", "GET", { id: $(this).data("id") }, "successReset", null);
    });  

}


function ValidarSegregacion(id) {

    MostrarConfirm("Importante", "Este perfil tiene conflicto, desea continuar?", "ContinuarSegregacion",id);
}

function ContinuarSegregacion(rta) {
    mostrarAlerta("Importante", "Este perfil tiene conflicto, desea continuar?" + rta);
}





function Delete(Id) {
    EjecutarAjax(urlBase + "Usuarios/Delete", "GET", { id: Id }, "successDelete", null);
}

function successDelete(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Usuarios/GetList", "GET", null, "printPartial", {
            div: "#listView", func: "setEventEdit"
        });
        mostrarAlerta("El usuario fue desactivado con éxito.");
    }
}

function Desbloqueo(Id) {
    EjecutarAjax(urlBase + "Usuarios/Desbloqueo", "GET", { id: Id }, "successDesbloqueo", null);
}

function successDesbloqueo(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Usuarios/GetList", "GET", null, "printPartial", {
            div: "#listView", func: "setEventEdit"
        });
        mostrarAlerta("El usuario fue desbloqueado con éxito.");
    }
}

function successReset(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Usuarios/GetList", "GET", null, "printPartial", {
            div: "#listView", func: "setEventEdit"
        });
        mostrarAlerta("La clave fue reseteada con éxito.");
    }
}

function Resetpwd(Id) {
    EjecutarAjax(urlBase + "Usuarios/Reseteopwd2", "GET", { id: Id }, "successReset", null);
}

function successUpdate(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Usuarios/GetList", "GET", null, "printPartial", {
            div: "#listView", func: "setEventEdit"
        });
        cerrarModal("modalCRUD");
        mostrarAlerta("El usuario fue modificado con éxito!.");  
    }
}

function successUpdatePerfilUsuario(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Usuarios/GetListPerfilUsuario", "GET", null, "printPartial", {
            div: "#listView", func: "setEventEdit"
        });
        cerrarModal("modalCRUD");
        mostrarAlerta("El perfil fue modificado con éxito!.");
    }
}
//@Inicializar funciones -

//@Eventos

function InitPartialCreate(dataperfil, dataPuntos) {

    //$("#grpPassword").hide();
    //$("#chbx").click(function () {
        
    //    if ($(this).is(':checked'))
    //        {
    //            $("#grpPassword").show();
    //            $("#Password").addClass("password");
                
    //        }
    //    else
    //        {
    //            $("#grpPassword").hide();
    //            $("#Password").removeClass("password");
    //        }
    //});

    setAutocompleteCategory();
    tagsAutocomplete($('#tagsPerfiles'), $('#perfilesAutocomplete'), $("#hdListPerfiles"), dataperfil, false)
    //tagsAutocomplete($('#tagsPuntos'), $('#puntosAutocomplete'), $("#hdListPuntos"), dataPuntos, true)

}

//@Fin eventos


function AsignarEventoEmpleado()
{
    $("#ddlEmpleado").change(function () {
        var datos = $(this).find("option:selected").val().split(' ');
        var nombre = "";
        var apellido = "";
        if (datos.length > 3)
            nombre = datos[1] + " " + datos[2];
        else {
            nombre = datos[1];
        }
        if (datos.length > 4)
            apellido = datos[3] + " " + datos[4];
        else if (datos.length > 3) {
            apellido = datos[3];
        } else {
            apellido = datos[2];
        }
        $("#Nombre").val(nombre);
        $("#Apellido").val(apellido);
        $("#IdEmpleado").val($(this).find("option:selected").data("id"));
    });
}