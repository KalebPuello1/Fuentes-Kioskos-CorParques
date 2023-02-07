$(function () {
    $("#IdProcedimiento").val("");
    loadCalendar();
    $("#btnGenerarReporte").click(function () {
        if (ValidarCampos())
        {
            EjecutarAjax(urlBase + "ReporteCentroMedico/GenerarArchivo", "GET", ObtenerObjeto("div_principal form"), "GenerarReporte", null);
        }        
    });

    $("#IdZonaArea").change(function () {
        BuscarUbicaciones($(this).val());
    });

});

function GenerarReporte(data) {
    if (data.length > 0) {
        if (data.indexOf("Error") >= 0) {
            mostrarAlerta("Error al generar reporte: " + data);
        }
        else {
            window.location = urlBase + '/ReporteCentroMedico/Download?Data=' + data;
        }                
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");        
    }

}

function ValidarCampos()
{
    var IdProcedimiento = $("#IdProcedimiento").val();
    var IdTipoDocumentoPaciente = $("#IdTipoDocumentoPaciente").val();
    var IdCategoriaAtencion = $("#IdCategoriaAtencion").val();
    var IdTipoPaciente = $("#IdTipoPaciente").val();
    var FechaIncidenteDDMMAAAA = $("#FechaIncidenteDDMMAAAA").val();
    var FechaIncidenteFinalDDMMAAAA = $("#FechaIncidenteFinalDDMMAAAA").val();
    var IdZonaArea = $("#IdZonaArea").val();
    var IdCentroMedico = $("#IdCentroMedico").val();


    if (IdProcedimiento.length == 0 && IdTipoDocumentoPaciente.length == 0 && IdCategoriaAtencion.length == 0 &&
        IdTipoPaciente.length == 0 && FechaIncidenteDDMMAAAA.length == 0 && FechaIncidenteFinalDDMMAAAA.length == 0 &&
        IdZonaArea.length == 0 && IdCentroMedico.length == 0) {
        MostrarMensaje("Importante", "Debe seleccionar al menos un filtro para la busqueda");
        return false;
    }
    else {
        if (FechaIncidenteDDMMAAAA.length > 0 && FechaIncidenteFinalDDMMAAAA.length == 0) {            
            MostrarMensaje("Importante", "Debe selecionar la fecha final");
            return false;
        }
        else {
            if (FechaIncidenteFinalDDMMAAAA.length > 0 && FechaIncidenteDDMMAAAA.length == 0) {
                MostrarMensaje("Importante", "Debe selecionar la fecha inicial");
                return false;
            }
            else {
                if (FechaIncidenteDDMMAAAA.length > 0 && FechaIncidenteFinalDDMMAAAA.length > 0)
                {
                    if (validate_fechaMayorQue(FechaIncidenteFinalDDMMAAAA, FechaIncidenteDDMMAAAA)) {
                        MostrarMensaje("Importante", "Fecha inicial debe ser menor o igual a la fecha final");
                        return false;
                    }
                }                
            }
        }
    }



    return true;

}

function BuscarUbicaciones(IdZonaArea)
{
    if (IdZonaArea.length == 0) {
        EjecutarAjax(urlBase + "ReporteCentroMedico/BuscarUbicaciones", "GET", { Id: 0 }, "CargarUbicaciones", null);
    } else {
        EjecutarAjax(urlBase + "ReporteCentroMedico/BuscarUbicaciones", "GET", { Id: IdZonaArea }, "CargarUbicaciones", null);
    }
}

function CargarUbicaciones(data)
{
    //Se remueven todos los items de la lista de ubicacion y se deja solo la opcion todas.
    $('#IdCentroMedico').find('option').remove().end().append('<option value="">Todas</option>').val('');
    
    //Se agregan los items que vienen en la variable data.
    var listitems;
    $.each(data, function (key, value) {        
        listitems += '<option value=' + value.Id + '>' + value.Nombre + '</option>';
    });
    $('#IdCentroMedico').append(listitems);        
}