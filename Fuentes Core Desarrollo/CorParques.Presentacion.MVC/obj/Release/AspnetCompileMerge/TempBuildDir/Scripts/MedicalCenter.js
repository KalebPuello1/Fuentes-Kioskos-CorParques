var EraEmpleado = false;

$(function () {
    $("#div_message_error").hide();
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "MedicalCenter/GetPartial", "GET", null, "printPartialModal", { title: "Crear registro paciente", url: urlBase + "MedicalCenter/Insert", metod: "GET", func: "successInsert", modalLarge: true, DatePicker: true, TimePicker : true});
    });
    setEventEdit();    
});

function setEventEdit() {
    EstablecerToolTipIconos();    
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "MedicalCenter/GetById", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar registro paciente", url: urlBase + "MedicalCenter/Update", metod: "GET", func: "successUpdate", modalLarge: true, DatePicker: true, TimePicker: true });
    });

    $(".lnkDetail").click(function () {
        EjecutarAjax(urlBase + "MedicalCenter/Detalle", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Detalle registro paciente", url: "", metod: "", func: "", hidesave: "Y", showreturn: "Y", modalLarge: true, print: "Y" });
    });    
}

function successInsert(rta) {

    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "MedicalCenter/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
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
        EjecutarAjax(urlBase + "MedicalCenter/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Edición exitosa.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}

function ValidarEdad()
{
    var edad = $("#EdadPaciente").val();
    var meses = $("#Meses").val();
    var edadcalculo = 0;

    if (parseInt(meses) > 0)
    {
        edadcalculo = (parseInt(meses) / 12);
    }
    if (parseInt(edadcalculo) > 0)
    {
        if (edad != '') {
            edad = parseInt(edad) + parseInt(edadcalculo);
        }
        else {
            edad = edadcalculo;
        }
    }
    

    $("#IdTipoDocumentoAcudiente").removeClass("required");
    $("#DocumentoAcudiente").removeClass("required");
    $("#NombreAcudiente").removeClass("required");
    $("#TelefonoAcudiente").removeClass("required");
    $("#IdTipoDocumentoAcudiente").removeClass("errorValidate");
    $("#DocumentoAcudiente").removeClass("errorValidate");
    $("#NombreAcudiente").removeClass("errorValidate");
    $("#TelefonoAcudiente").removeClass("errorValidate"); 
    $("#IdTipoDocumentoAcudiente").attr("data-mensajeerror", "");
    $("#DocumentoAcudiente").attr("data-mensajeerror", "");
    $("#NombreAcudiente").attr("data-mensajeerror", "");
    $("#TelefonoAcudiente").attr("data-mensajeerror", "");

    if (edad != '')
    {
        if (parseInt(edad) < 18)
        {
            $("#IdTipoDocumentoAcudiente").addClass("required");
            $("#DocumentoAcudiente").addClass("required");
            $("#NombreAcudiente").addClass("required");
            $("#TelefonoAcudiente").addClass("required");
        }        
    }

}

//Setea el evento change de tipo paciente y de empleado.
function EventoTipoPaciente(){

    $("#IdTipoPaciente").change(function () {
        if ($(this).val() == "1") {
            EraEmpleado = true;
            $("#div_ListaEmpleados").show();
            $("#DatosEmpleado").addClass("required");
            LimpiarControlesPaciente();
            BloquearControles(true, true);
        } else {
            $("#div_ListaEmpleados").hide();
            $("#DatosEmpleado").removeClass("required");
            if (EraEmpleado)
            {
                LimpiarControlesPaciente();
                BloquearControles(false, true);
                EraEmpleado = false;
            }
        }

    });

    $("#DatosEmpleado").change(function () {
        var Empleado = $(this).val();
        var datosSplit = "";
        if (Empleado.length > 0) {
            datosSplit = Empleado.split('_');

            if (datosSplit.length > 0) {
                $("#IdTipoDocumentoPaciente").val("1");
                $("#DocumentoPaciente").val(datosSplit[0]);
                $("#NombrePaciente").val(datosSplit[1]);
                $("#ApellidoPaciente").val(datosSplit[2]);
                //BloquearControles(true, true);
            }
        } else {
            LimpiarControlesPaciente();
            BloquearControles(false, true);
        }
    });

}

//Inicializa el formulacio de creacion y de edicion.
function Inicializar() {
    if ($("#IdTipoPaciente").val() == "1") {
        BloquearControles(true, false);        
    } else {        
        $("#div_ListaEmpleados").hide();
        $("#DatosEmpleado").removeClass("required");
        if ($("#IdTipoPaciente").val() !== "")
        {
            $("#IdTipoPaciente option[value='1']").remove();
        }        
    }

    if ($('input[name="Traslado"]:checked').val() == "Si") {
        $("#div_TieneEps").show();
    }

    if ($('input[name="Eps"]:checked').val() == "Si") {
        $("#div_NombreEps").show();
        $("#NombreEps").addClass("required");
    }

}

//Bloquea o desbloquea los controles de paciente segun la seleccion del tipo del paciente.
function BloquearControles(blnBloquear, blnCreacion)
{
    if (blnBloquear) {           
        $("#DocumentoPaciente").attr('readonly', true);
        $("#NombrePaciente").attr('readonly', true);
        $("#ApellidoPaciente").attr('readonly', true);
    }
    else {
        $("#IdTipoDocumentoPaciente").removeAttr("readonly");
        $("#DocumentoPaciente").removeAttr("readonly");
        $("#NombrePaciente").removeAttr("readonly");
        $("#ApellidoPaciente").removeAttr("readonly");
    }
}

//Limpia los controles de tipo de paciente.
function LimpiarControlesPaciente()
{
    $("#IdTipoDocumentoPaciente").val("");
    $("#DocumentoPaciente").val("");
    $("#NombrePaciente").val("");
    $("#ApellidoPaciente").val("");
}

//Si marcan Si en el radio button de "¿Tiene EPS?", muestra un text box para digitar el nombre de la EPS
function EstablecerNombreEPS() {

    $('input[type=radio][name=Eps]').on('change', function () {
        switch ($(this).val()) {
            case 'Si':
                $("#div_NombreEps").show();
                $("#NombreEps").addClass("required");
                break;
            case 'No':
                $("#NombreEps").val("");
                $("#NombreEps").removeClass("required");
                $("#div_NombreEps").hide();
                break;
        }
    });

}

//Si marcan Si en radio button Traslado, aparece la opcion de Tiene EPS
function EstablecerTieneEPS() {

    $('input[type=radio][name=Traslado]').on('change', function () {
        switch ($(this).val()) {
            case 'Si':
                $("#div_TieneEps").show();
                break;
            case 'No':
                //Toca dejar no en Tiene EPS
                //$('#Eps').prop('checked', true);
                $('input:radio[name=Eps]').val(['No']);
                $("#div_TieneEps").hide();
                $("#NombreEps").val("");
                $("#NombreEps").removeClass("required");
                $("#div_NombreEps").hide();
                break;
        }
    });

}

//function OrdenDescendente()
//{
//    var table = $('#datatable-responsive').DataTable();
//    // Sort by column 1 and then re-draw
//    table
//        .order([1, 'desc'])
//        .draw();
//}