//Powered by RDSH
var inicializadointerval = false;
var blnAgregarEmpleadoGlobal = true;
$(function () {

    Inicializar();

});


function Inicializar()
{
    $("#div_message_error").hide();

    $("#txt_DocumentoEmpleado").keyup(function () {
        if (!inicializadointerval) {
            inicializadointerval = true;
            var refreshIntervalId = setInterval(function () { BuscarDatosEmpleado(blnAgregarEmpleadoGlobal); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);
        }
    });

    InhabilitarCopiarPegarCortar("txt_DocumentoEmpleado");

    $("#btn_Agregar").click(function () {
        MostrarLectorDeCodigo(true);
    });

    $("#btn_Remover").click(function () {
        MostrarLectorDeCodigo(false);
    });

}

function BuscarDatosEmpleado(blnAgregar)
{
    $("#div_message_error").hide();
    var strDocumento = "";
    var TituloVentana = "Asociar auxiliar";
    var FuncionEjecutar = "AuxiliarPunto/Insert";
    var FuncionSuccess = "successInsertAuxiliar";

    strDocumento = $("#txt_DocumentoEmpleado").val();   
    cerrarModal("myModal");
    
    if (blnAgregar === false)
    {
        TituloVentana = "Retirar auxiliar";
        FuncionEjecutar = "AuxiliarPunto/Update";
        FuncionSuccess = "successUpdateAuxiliar";
    }

    if (strDocumento !== "")
    {
        EjecutarAjax(urlBase + "AuxiliarPunto/BuscarDatosAuxiliar", "GET", { Documento: strDocumento, Agregar: blnAgregar }, "printPartialModal", { title: TituloVentana, url: urlBase + FuncionEjecutar, metod: "GET", func: FuncionSuccess, TextoBotonGuardar:"Aceptar" });
    }
    $("#txt_DocumentoEmpleado").val('');
}

function successInsertAuxiliar(rta) {

    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "AuxiliarPunto/GetList", "GET", null, "printPartial", { div: "#listView", func: "Inicializar", table: 'N'});
        cerrarModal("modalCRUD");
        mostrarAlerta("Su operación fue exitosa.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}


function successUpdateAuxiliar(rta) {
    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "AuxiliarPunto/GetList", "GET", null, "printPartial", { div: "#listView", func: "Inicializar", table: 'N' });
        cerrarModal("modalCRUD");
        mostrarAlerta("Auxiliar retirado exitosamente.");
    }
    else {
        $("#lbl_message_error").html(rta.Mensaje);
        $("#div_message_error").show();
    }
}

function EmpleadoNoEncontrado()
{    
    MostrarMensaje('Importante', 'Empleado no encontrado', 'error');
}

function EmpleadoSinUbicaciones() {
    MostrarMensaje('Importante', 'Este auxiliar no se encuentra registrado.', 'error');
}

function MensajeNoHayUbicaciones()
{
    MostrarMensaje('Importante', 'La atracción no tiene ubicaciones definidas.', 'error');
}

function setEventEdit()
{

}

function MostrarLectorDeCodigo(blnAgregar)
{
    if (blnAgregar === true) {
        blnAgregarEmpleadoGlobal = true;
        $('#myModalLabel').html('<b>Agregar auxiliar</b>');
    }
    else {
        blnAgregarEmpleadoGlobal = false;
        $('#myModalLabel').html('<b>Retirar auxiliar</b>');
    }
    $('#myModal').modal('show');
    
    setTimeout(function () { $('#txt_DocumentoEmpleado').focus() }, 500);
    

    $('#txt_DocumentoEmpleado').focusout(function () {
        if (($('#myModal').is(':visible'))) {
            setTimeout(function () { $('#txt_DocumentoEmpleado').focus() }, 500);
        }        
    });
}

