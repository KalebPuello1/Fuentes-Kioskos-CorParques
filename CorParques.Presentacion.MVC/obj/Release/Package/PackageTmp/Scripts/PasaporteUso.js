$(function () {
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "PasaporteUso/GetPartial", "GET", null, "printPartialModal", { title: "Crear Pasaporte por uso", url: urlBase + "PasaporteUso/ActualizarPasaporteUso", metod: "GET", func: "successInsert", Table: "S", modalLarge: true });
    });
    setEventEdit();
});
function setEventEdit() {
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "PasaporteUso/Obtener", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar Pasporte", url: urlBase + "PasaporteUso/ActualizarPasaporteUso", metod: "GET", func: "successUpdate", Table: "S", modalLarge: true });

    });
    $(".lnkDelete").click(function () {
        if (confirm("Está seguro que desea desactivar este pasaporte?"))
            EjecutarAjax(urlBase + "PasaporteUso/Delete", "GET", { id: $(this).data("id") }, "successDelete", null);
    });
}
function successInsert(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "PasaporteUso/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("El pasaporte fue creado con éxito.");
    }    
}
function successDelete(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "PasaporteUso/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        mostrarAlerta("El pasaporte fue desactivado con éxito.");
    }
}
function successUpdate(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "PasaporteUso/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("El pasaporte fue modificado con éxito.");
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
        textControl.classList.remove('required');
        textControl.readOnly = true;
        textControl.value = "";
    }
    else {
        if (split.length > 0) {
            textControl.readOnly = false;
            textControl.classList.add('required');
            textControl.value = "1";
            Items = $("#hdf_atraccionesSeleccionadas").val() + ',' + ctrl.value + '|1'
        }
        else {
            textControl.classList.add('required');
            textControl.readOnly = false;
            textControl.value = "1";
            Items = ctrl.value + '|1';
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

function ValidaNUsos(ctrl) {
   
    
    if (ctrl.value.length == 0)
    {
        
        if (!ctrl.readOnly)
         {
            ctrl.value = "1";
        }
    }
    else {
        if (parseInt(ctrl.value) <= 0)
        {
            ctrl.value = "1";
        }
    }
   

}




