
$(function () {

    //SetParametroTabla();

    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "Solicitudes/GetPartial", "GET", null, "printPartialModal", { title: "Crear solicitud de impresión", url: urlBase + "Solicitudes/Insert", metod: "GET", func: "successInsert", modalLarge: true, DatePicker: true });
    });
    setEventEdit();

    var table = $('#datatable-responsive').DataTable();
    //table.order([0, 'desc']).draw()
    table.order([0, 'asc']).draw()
});


$(".Imprimir").click(function () {

    var id = $(this).data("id");
    var CodigoVenta = $(this).data("codigoventa");
    var IdBoletaControl = $(this).data("boletacontrol");
    GestionarCentroImpresion({
        IdSolicitud: id, CodVenta: CodigoVenta, boletaControl: IdBoletaControl, procesar: false
    });

    //MostrarConfirm('Importante!', '¿Está seguro de continuar con el proceso de asignación de consecutivos? ', "GestionarCentroImpresion", { IdSolicitud: id, CodVenta: CodigoVenta, boletaControl: IdBoletaControl });
});

$(".Detalle").click(function () {
    
    var id = $(this).data("id");
    var IdBoletaControl = $(this).data("boletacontrol");
    GestionarCentroImpresion({ IdSolicitud: id, CodVenta: "", boletaControl: IdBoletaControl });
});

$(".Eliminar").click(function () {

    var id = $(this).data("id");
    var CodigoVenta = $(this).data("codigoventa");
    var IdBoletaControl = $(this).data("boletacontrol");

    MostrarConfirm('Importante!', '¿Está seguro de eliminar esta solicitud? ', 'EliminarSolicitud', {
        IdSolicitud: id, CodVenta: CodigoVenta, boletaControl: IdBoletaControl
    });
});




function GestionarCentroImpresion(obj)
{
    EjecutarAjax(urlBase + "CentroImpresion/GestionarCentroImpresion", "GET", obj, "sucessProcess", obj);
}

function EliminarSolicitud(obj) {
    EjecutarAjax(urlBase + "CentroImpresion/EliminarSolicitud", "GET", obj, "sucessProcessEliminar", null);
}

function sucessProcess(rta, obj) {
    if (rta.Correcto) {
        //EjecutarAjax(urlBase + "CentroImpresion/GetList", "GET", null, "printPartial", { div: "#listView", func: "successGestion" });
        var Mensaje = "<div style='text-align:left'><div class='row'>"+
        "<div class='col-md-6 col-sx-12'><strong>Producto: </strong></div>" + "<div class='col-md-6 col-sx-12'>" + rta.Elemento[0].NombreProducto + "</div></div>" +
        "<div class='row'><div class='col-md-6 col-sx-12'><strong>Tipo producto: </strong></div><div class='col-md-6 col-sx-12'>" + rta.Elemento[0].NombreTipoProducto + "</div></div>" +
        "<div class='row'><div class='col-md-6 col-sx-12'><strong>Fecha uso inicio:</strong></div><div class='col-md-6 col-sx-12'>" + rta.Elemento[0].FechaUsoInicialDDMMYYY + "</div></div>" +
        "<div class='row'><div class='col-md-6 col-sx-12'><strong>Fecha uso fin:</strong> </div><div class='col-md-6 col-sx-12'>" + rta.Elemento[0].FechaUsoFinalDDMMYYY + "</div></div>" +
        "<div class='row'><div class='col-md-6 col-sx-12'><strong>Cantidad: </strong></div><div class='col-md-6 col-sx-12'>" + rta.Elemento[0].Cantidad + "</div></div>" +
        "<div class='row'><div class='col-md-6 col-sx-12'><strong>Consecutivo inicio:</strong></div><div class='col-md-6 col-sx-12'>" + rta.Elemento[0].Consecutivo + "</div></div>" +
        "<div class='row'><div class='col-md-6 col-sx-12'><strong>Consecutivo fin: </strong></div><div class='col-md-6 col-sx-12'>" + rta.Elemento[rta.Elemento.length - 1].Consecutivo + "</div></div></div>";
        
        
        //MostrarMensaje("Detalle", Mensaje);
        obj.procesar = true;
        //MostrarConfirm('Detalle', Mensaje, 'sucessProcess2', obj, true);
        MostrarConfirm('Detalle', Mensaje, 'respsucessProcess2', obj, true);

    } else {
        MostrarMensaje("Importante", "No fue posible procesar la solicitud. Por favor intentelo más tarde.", "warning");
    }
}

function sucessProcessEliminar(rta) {
    if (rta.Correcto) {
        //EjecutarAjax(urlBase + "CentroImpresion/GetList", "GET", null, "printPartial", { div: "#listView", func: "successGestion" });
        //MostrarMensaje("Mensaje", "Operación realizada con éxito");
        MostrarMensajeRedireccion("Importante", "Operación realizada con éxito", "CentroImpresion/Index", "success");
    }
}

function sucessProcess2(obj) {
    EjecutarAjax(urlBase + "CentroImpresion/GestionarCentroImpresion", "GET", obj, "respsucessProcess2", null);
}

function respsucessProcess2(rta) {
     //if (rta.Correcto) {
    //EjecutarAjax(urlBase + "CentroImpresion/GetList", "GET", null, "printPartial", { div: "#listView", func: "successGestion" });
    MostrarMensajeRedireccion("Importante", "Operación realizada con éxito", "CentroImpresion/Index", "success");
        //MostrarMensaje("Mensaje", "Operación realizada con éxito");
    //}
}



function successGestion() {

    setEventEdit();
     var table = $('#datatable-responsive').DataTable();
     table.order([0, 'desc']).draw()
}

function setEventEdit() {


$(".MascaraTipoMoneda:input[type=text]").mask("000.000.000", { reverse: true });
$(".MascaraNumero").mask("0000000000");

$("#Cantidad").keyup(function () {
    var _value = $(this).val().toString();
    if ($.trim(_value).length == 1) {
        if (_value == "0")
            $(this).val("");
    }
});


        $("#ddlTipoProducto").change(function () {
            var CodSap = $(this).val();
  
            if (CodSap == "2020") {
                $("#divColValor").fadeIn();
                $("#strvalor").addClass("required");
                $("#strvalor").val("");
            } else {
                $("#divColValor").fadeOut("fast");
                $("#strvalor").removeClass("required");
                $("#strvalor").val("");
            }

            if (CodSap.length > 0) {
                $.ajaxSetup({ cache: false });
                $.ajax({
                    url: urlBase + "Solicitudes/ObtenerProductosporTipo",
                    type: "GET",
                    dataType: "JSON",
                    data: {
                        CodSapTipoProducto: CodSap
                    },
                    contentType: "application/json; charset-uft8",
                    success: MostrarProductos,
                    error: function (jqXHR, exception) {
                        $("#divError strong").text("Error!");
                        if (jqXHR.status === 0) {
                            mostrarAlerta("divError", " No cuenta con conexion a internet.");
                        } else if (jqXHR.status === 404) {
                            mostrarAlerta("divError", " 404. No encuentra el recurso solicitado. '" + url + "'");
                        } else if (jqXHR.status === 500) {
                            mostrarAlerta("divError", " 500. Error interno del servidor. Por favor comuníquese con el administrador del sistema");
                        } else if (exception === 'parsererror') {
                            mostrarAlerta("divError", " Error al convertir el objeto en JSON");
                        } else if (exception === 'timeout') {
                            mostrarAlerta("divError", " Tiempo de espera agotado. Por favor comuníquese con el administrador del sistema");
                        } else if (exception === 'abort') {
                            mostrarAlerta("divError", " Petición AJAX abortada. Por favor comuníquese con el administrador del sistema");
                        } else {
                            mostrarAlerta("divError", " Error inesperado (" + jqXHR.responseText + "). Por favor comuníquese con el administrador del sistema");
                        }
                    }

                })

                //EjecutarAjax(urlBase + "Solicitudes/ObtenerProductosporTipo", "GET",
                //    {
                //    CodSapTipoProducto: CodSap
                //}, "MostrarProductos", null);
            }
            else
                $("#IdProducto").html("<option value=''>Seleccione</option>");
        });

    }

function MostrarProductos(rta){
    var lista = "<option value=''>Seleccione...</option>";
    if (rta != null && rta.length > 0) {
        $.each(rta, function (i, item) {
            if (item.IdEstado === 1) {
                lista += "<option value='" + item.IdProducto + "'>" + item.Nombre + "</option>";
            }
        });                
    }
    $("#IdProducto").html(lista);
}

function successInsert(rta) {

    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Solicitudes/GetList", "GET", null, "printPartial", { div: "#listView", func: "successGestion" });
        cerrarModal("modalCRUD");
        mostrarAlerta("La solicitud fue creada con éxito.");
    }
}



function SetParametroTabla(){
    //Ordenar tabla de forma descente
var table = $('#datatable-responsive').DataTable();
 
// Sort by columns 1 and 2 and redraw
table
    .order( [ 0, 'desc' ], [ 1, 'asc' ] )
    .draw();

}