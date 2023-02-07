$(function () {
    $("#div_message_error").hide();
    $("#lnkAdd").click(function () {        
        EjecutarAjax(urlBase + "ConvenioParqueadero/GetPartial", "GET", null, "printPartialModal", { title: "Crear autorización parqueadero", url: urlBase + "ConvenioParqueadero/Insert", metod: "GET", func: "successInsert", DatePicker: true });
    });
    setEventEdit();
});
function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "ConvenioParqueadero/Obtener", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar autorización parqueadero", url: urlBase + "ConvenioParqueadero/Update", metod: "GET", func: "successUpdate", DatePicker: true });
    });

    $(".lnkDisable").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea inactivar esta autorización?", "InactivarAutorizacion", $(this).data("id"));
        //if (confirm("¿Está seguro que desea inactivar esta autorización?"))
        //    EjecutarAjax(urlBase + "ConvenioParqueadero/UpdateEstado", "GET", { id: $(this).data("id") }, "successDisable", null);
    });

    $(".lnkDetail").click(function () {
        EjecutarAjax(urlBase + "ConvenioParqueadero/Detalle", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Detalle autorización parqueadero", hidesave: "S", showreturn: "S" });
    });
}

function setEventChange() {
    $("#IdTipoConvenioParqueadero").change(function () {
        if ($(this).val() == "1") {
            //Si el tipo de convenio es para empleado se muestra el combo para seleccionar al empleado.
            $("#div_ListaEmpleados").show();
            $("#DatosEmpleado").addClass("required");
            $("#div_DatosPersona").hide();
            $("#Documento").removeClass("required");
            $("#Nombre").removeClass("required");
            $("#Apellido").removeClass("required");
        }
        else {
            //Si no es para empleado se muestran los demas campos para capturar la informacion del autorizado.
            $("#DatosEmpleado").removeClass("required");
            $("#div_ListaEmpleados").hide();
            $("#div_DatosPersona").show();
            $("#Documento").addClass("required");
            $("#Nombre").addClass("required");
            $("#Apellido").addClass("required");
        }
        $("#ListaEmpleados").val("");
    });

    $("#btn_AddVehicle").click(function () {
        AgregarVehiculo();
    });
}

function setAreaVisibility() {        
    if ($("#IdTipoConvenioParqueadero").val() == "1") {
        $("#div_DatosPersona").hide();
        $("#div_ListaEmpleados").show();
    } else {
        $("#DatosEmpleado").removeClass("required");
        $("#div_ListaEmpleados").hide();        
    }
}

function SetEdit()
{
    $("#DatosEmpleado").removeClass("required");
    $("#div_ListaEmpleados").hide();
    if ($("#IdTipoConvenioParqueadero").val() == "1") {
        $("#Documento").prop("readonly", true);
        $("#Nombre").prop("readonly", true);
        $("#Apellido").prop("readonly", true);
    } else {
        $("#IdTipoConvenioParqueadero option[value='1']").remove();
    }

}

function successInsert(rta) {

    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "ConvenioParqueadero/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Su operación fue exitosa.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}

function successUpdate(rta) {
    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "ConvenioParqueadero/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Edición exitosa.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}

function successDisable(rta) {
    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "ConvenioParqueadero/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("La autorización fue inactivada con éxito.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}

function InactivarAutorizacion(Id) {
    EjecutarAjax(urlBase + "ConvenioParqueadero/UpdateEstado", "GET", { id: Id }, "successDisable", null);
}

function AgregarVehiculo()
{
    var bnlFlag = false;
    var linkEliminar = "<a class='lnkDeleteVehicle' data-id='@' href='javascript:void(0)'><b class='fa fa-times-circle'></b></a>";
    var Placa = "";
    var TipoVehiculo = "";
    var IdTipoVehiculo = 0;

    if ($("#IdTipoVehiculo").val() == "") {
        $("#IdTipoVehiculo").attr("data-mensajeerror", "Este campo es obligatorio");
        $("#IdTipoVehiculo").addClass("errorValidate");
        bnlFlag = true;
    }
    else {        
        $("#IdTipoVehiculo").attr("data-mensajeerror", "");
        $("#IdTipoVehiculo").removeClass("errorValidate");
    }

    if ($("#Placa").val() == "") {        
        $("#Placa").attr("data-mensajeerror", "Este campo es obligatorio");
        $("#Placa").addClass("errorValidate");
        bnlFlag = true;
    }
    else {
        $("#Placa").attr("data-mensajeerror", "");
        $("#Placa").removeClass("errorValidate");

        if ($("#Placa").val().length < 4) {
            $("#Placa").attr("data-mensajeerror", "Minimo 4 caracteres");
            $("#Placa").addClass("errorValidate");
            bnlFlag = true;
        }
        else {
            $("#Placa").attr("data-mensajeerror", "");
            $("#Placa").removeClass("errorValidate");
        }
    }



    if (!bnlFlag) {
        QuitarTooltip();
        IdTipoVehiculo = $("#IdTipoVehiculo").val();
        TipoVehiculo = $("#IdTipoVehiculo option:selected").text();
        Placa = $("#Placa").val().toUpperCase();
        linkEliminar = linkEliminar.replace("@", Placa);
        if (!ValidarSiPlacaExiste(Placa)) {
            $('#tb_Vehiculos > tbody:last-child').append("<tr id=tr_" + Placa + "><td class='IdTipoVehiculo' data-id='" + IdTipoVehiculo + "'> " + TipoVehiculo + " </td><td class='placas'> " + Placa + " </td><td>" + linkEliminar + "</td></tr>");
            $("#IdTipoVehiculo").val("");
            $("#Placa").val("");
            RecorrerVehiculos();
        }
        else {
            $("#Placa").focus();
            MostrarMensaje("Importante", "La placa digitada ya existe.", "error");
        }
    }
    else {
        mostrarTooltip();
    }

    SetDeleteVehicle();

}
function EliminarVehiculo(Placa)
{
    $("#tr_" + Placa).remove();
    RecorrerVehiculos();
}

function ValidarSiPlacaExiste(Placa)
{
    var blnFlag = false;

    $.each($(".placas"), function (i, v) {
        if ($(v).html().trim() == Placa) {
            blnFlag = true;
            return false;
        }
    });       
    return blnFlag;
}

function RecorrerVehiculos()
{
    var Placas = "";
    var IdTipoVehiculo = "";

    $.each($(".placas"), function (i, v) {
        if (Placas == "") {
            Placas = $(v).html().trim();
        } else {
            Placas = Placas + ',' + $(v).html().trim();
        }     
    });

    $.each($(".IdTipoVehiculo"), function (i, v) {
        if (IdTipoVehiculo == "") {
            IdTipoVehiculo = $(v).data("id");
        } else {
            IdTipoVehiculo = IdTipoVehiculo + ',' + $(v).data("id");
        }
    });

    $("#ListaIdTipoVehiculo").val(IdTipoVehiculo);
    $("#ListaPlacas").val(Placas);

}

function SetDeleteVehicle()
{
    $(".lnkDeleteVehicle").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea remover este vehículo?", "EliminarVehiculo", $(this).data("id"));
    });
}