var dataPuntos = [];
$(function () {
    $("#lnkAdd").click(function () {
        //EjecutarAjax(urlBase + "Ubicaciones/GetPartial", "GET", null, "printPartialModal", { title: "Crear Ubicación Punto", url: urlBase + "Ubicaciones/Insert", metod: "GET", func: "successInsert", modalLarge: true });
        EjecutarAjax(urlBase + "Ubicaciones/GetPartial", "GET", null, "printPartialModal", {
            title: "Crear Ubicación Punto", hidesave: false, modalLarge: false
        });
    });
    setEventEdit();
    $('#btnSaveGeneric').click(function () {
        ($('#RequiereAuxiliar')[0].checked) ? $("#RequiereAuxiliar").val(true) : $("#RequiereAuxiliar").val(false);
        (document.getElementById('frmCreate') != null) ? CrearUbicacion() : EditarUbicacion();
    });
});

function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkEdit").click(function () {
       // EjecutarAjax(urlBase + "Ubicaciones/GetById", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar Ubicación Punto", url: urlBase + "Ubicaciones/Update", metod: "GET", func: "successUpdateUbicacionPunto", modalLarge: true });
        EjecutarAjax(urlBase + "Ubicaciones/GetById", "GET", { id: $(this).data("id") }, "printPartialModal", {
            title: "Editar Ubicación Punto", hidesave: false, modalLarge: false
        });
    });
    $(".lnkDisable").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea inactivar este registro?", "EliminarUbicacionPunto", $(this).data("id"));
    });
}

function successInsert(rta) {
    if (rta.Correcto) {
        cerrarModal("modalCRUD");
    }
}

function successDelete(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Ubicaciones/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        MostrarMensaje("Importante", "Registro inactivado con éxito.", "success");
    } else {
        MostrarMensaje("Importante", "No fue posible inactivar el registro.", "error");
    }
    
}

function EliminarUbicacionPunto(Id) {
    EjecutarAjax(urlBase + "Ubicaciones/Delete", "GET", { id: Id }, "successDelete", null);    
}

function OnChangeEvent(dropDownElement) {
    var selectedValue = dropDownElement.options[dropDownElement.selectedIndex].value;
    $('#' + dropDownElement.id).val(selectedValue);
}

function CrearUbicacion() {    
    if (validarFormulario("frmCreate"))
    {
        var _Ubicacion;

        _Ubicacion = new Object();
        _Ubicacion.Nombre = $("#Nombre").val();
        _Ubicacion.IdPunto = $("#IdPunto").val();
        _Ubicacion.IdEstado = $("#IdEstado").val();
        _Ubicacion.RequiereAuxiliar = $("#RequiereAuxiliar").val();

        EjecutarAjax(urlBase + "Ubicaciones/Insert", "POST", JSON.stringify(_Ubicacion), "successCreateUbicacionPunto", null);

        //
        //var Puntos = $("#hdListPuntos").val().split(',')
        //for (index = 0; index < Puntos.length; ++index) {
        //    _Ubicacion = new Object();                        
        //    _Ubicacion.Nombre = $("#Nombre").val();
        //    _Ubicacion.IdPunto = Puntos[index];
        //    _Ubicacion.IdEstado = $("#IdEstado").val();
        //    _Ubicacion.RequiereAuxiliar = $("#RequiereAuxiliar").val();
        //    EjecutarAjax(urlBase + "Ubicaciones/Insert", "POST", JSON.stringify(_Ubicacion), "successCreateUbicacionPunto", null);
        //}
        
    }   
}

function successCreateUbicacionPunto(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Ubicaciones/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        MostrarMensaje("Importante", "Su operación fue exitosa.", "success");
    }
    else {
        //MostrarMensajeRedireccion("Importante", rta.Mensaje, "/Ubicaciones/Index", "success");
        MostrarMensaje("Importante", rta.Mensaje, "error");
    }
}

function EditarUbicacion() {
    if (validarFormulario("frmEdit")) {
        var _Ubicacion = new Object();
        _Ubicacion.IdUbicacionPunto = $("#IdUbicacionPunto").val();
        _Ubicacion.IdUsuarioCreacion = $("#IdUsuarioCreacion").val();
        _Ubicacion.FechaCreacion = $("#FechaCreacion").val();
        _Ubicacion.Nombre = $("#Nombre").val();
        _Ubicacion.IdPunto = $("#IdPunto").val();
        _Ubicacion.IdEstado = $("#IdEstado").val();
        _Ubicacion.RequiereAuxiliar = $("#RequiereAuxiliar").val();
        EjecutarAjax(urlBase + "Ubicaciones/Update", "POST", JSON.stringify(_Ubicacion), "successUpdateUbicacionPunto", null);
    }
}

function successUpdateUbicacionPunto(rta) {
    if (rta.Correcto) {
        cerrarModal("modalCRUD");
        EjecutarAjax(urlBase + "Ubicaciones/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        MostrarMensaje("Importante", "Edición exitosa.", "success");
    }
    else {
        //MostrarMensajeRedireccion("Importante", rta.Mensaje, "/Ubicaciones/Index", "success");
        MostrarMensaje("Importante", rta.Mensaje, "error");
    }
}