$(function () {
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "TipoBrazalete/GetPartial", "GET", null, "printPartialModal", { title: "Crear Tipo de Brazalete", url: urlBase + "TipoBrazalete/ActualizarTipoBrazalete", metod: "GET", func: "successInsert", Table: "S", modalLarge: true });
    });
    setEventEdit();
});
function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "TipoBrazalete/Obtener", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar tipo de brazalete", url: urlBase + "TipoBrazalete/ActualizarTipoBrazalete", metod: "GET", func: "successUpdate", Table: "S", modalLarge: true });

    });
    $(".lnkDisable").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea inactivar este tipo de brazalete?", "InactivarTipoBrazalete", $(this).data("id"));
        //if (confirm("Está seguro que desea eliminar este tipo de brazalete?"))
        //    EjecutarAjax(urlBase + "TipoBrazalete/Delete", "GET", { id: $(this).data("id") }, "successDelete", null);
    });
}
function successInsert(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "TipoBrazalete/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Operación realizada con éxito.");
    }
}
function successDelete(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "TipoBrazalete/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        mostrarAlerta("Registro inactivado con éxito.");
    }
}
function successUpdate(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "TipoBrazalete/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Edición exitosa.");
    }
}

function selectcheck(ctrl) {

    var split = "";
    var Items = "";
    var textControl = "";
    var splitText = "";

    textControl = document.getElementById(ctrl.value);

    if ($("#hdf_atraccionesSeleccionadas").val().length > 0) {
        split = $("#hdf_atraccionesSeleccionadas").val().split(',');
    }

    if (!ctrl.checked) {
        if (split.length > 0) {
            for (var I = 0; I <= (split.length - 1) ; I++) {
                splitText = split[I].split('|');
                if (splitText[0] != ctrl.value) {
                    if (Items == "") {
                        Items = split[I];
                    }
                    else {
                        Items = Items + ',' + split[I]
                    }
                }
            }
        }
        textControl.readOnly = true;
        textControl.value = "";
    }
    else {
        if (split.length > 0) {
            textControl.readOnly = false;
            Items = $("#hdf_atraccionesSeleccionadas").val() + ',' + ctrl.value + '|0'
        }
        else {
            textControl.readOnly = false;
            Items = ctrl.value + '|0';
        }

    }

    $("#hdf_atraccionesSeleccionadas").val(Items);

    //var checkedVals = $('.flat:checkbox:checked').map(function () {
    //    return this.value;
    //}).get();
    //$("#hdf_atraccionesSeleccionadas").val(checkedVals.join(","))
}

function AddControl(ctrl) {

    var split = "";
    var splitText = "";
    var Items = "";
    

    if ($("#hdf_atraccionesSeleccionadas").val().length > 0) {
        split = $("#hdf_atraccionesSeleccionadas").val().split(',');
    }

    if (ctrl.value.length > 0) {
        if (split.length > 0) {
            for (var I = 0; I <= (split.length - 1) ; I++) {
                splitText = split[I].split('|');
                if (splitText[0] == ctrl.id) {
                    if (Items == "") {
                        Items = splitText[0] + '|' + ctrl.value;
                    }
                    else {
                        Items = Items + ',' + splitText[0] + '|' + ctrl.value;
                    }
                }
                else {
                    if (Items == "") {
                        Items = splitText[0] + '|' + splitText[1];
                    }
                    else {
                        Items = Items + ',' + splitText[0] + '|' + splitText[1];
                    }
                }
            }
        }
    }
    

    $("#hdf_atraccionesSeleccionadas").val(Items);

    //var checkedVals = $('.flat:checkbox:checked').map(function () {
    //    return this.value;
    //}).get();
    //$("#hdf_atraccionesSeleccionadas").val(checkedVals.join(","))
}

function InactivarTipoBrazalete(Id) {
    EjecutarAjax(urlBase + "TipoBrazalete/Delete", "GET", { id: Id }, "successDelete", null);
}




