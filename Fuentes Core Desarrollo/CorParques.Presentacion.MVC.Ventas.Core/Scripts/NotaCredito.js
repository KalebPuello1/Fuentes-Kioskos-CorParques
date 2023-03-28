var notaCredito;
var FacturaAnulada = false;
var IdSupervisor = 0;

$(".Numero:input[type=text]").mask("0000000000000", { reverse: true });
$(".precioProducto:input[type=text]").mask("0.000.000.000.000", { reverse: true });


$('#txtNumeroFactura').keypress(function (e) {
    var letters = /^[A-Za-z0-9]+$/;
    if (!e.key.match(letters)) {
        this.value = this.value + String.fromCharCode(45)
        return false;
    }
    if (e.which == 13) {

        if (validarObligatorios("frmNotaCredito")) {
            ConsultarProductos($.trim($("#txtNumeroFactura").val()));
        }
        return false;   
    }
    
});


$("#btnAceptarNotaCredito").click(function () {


    var procesoEnvio = true;
    

        var codigoFactura = $("#txtNumeroFactura").val();
        codigoFactura = $.trim(codigoFactura);

        if (notaCredito == undefined) {
            if (validarFormulario("frmNotaCredito"))
                ConsultarProductos(codigoFactura);
            procesoEnvio = false;
        } else {
            if (notaCredito.DetalleFactura.length > 0) {

                if (notaCredito.DetalleFactura[0].CodigoFactura != codigoFactura) {
                    ConsultarProductos(codigoFactura);
                    procesoEnvio = false;
                }
            }
        }

        if (procesoEnvio) {

            var modifico = false;

            $.each($(".chxGrupo"), function (i, item) {
                
                if ($(item).is(":checked"))
                    modifico = true;
            })

            if (!modifico) {
                MostrarMensaje("Mensaje", "Seleccione uno o varios productos!");
            } else {

                if (ValidarCampos()) {
                    MostrarConfirm('Importante!', '¿Está seguro de guardar la Nota crédito? ', 'CrearfrmNotaCredito');

                } else {
                MostrarMensaje("Importante", "Hay inconsistencias en el formulario, revise los campos demarcados con color rojo.", "error");
                }
            }


                //$.each(notaCredito.DetalleFactura, function (i, item) {
                //    if (item.NotaCredito) {
                //        modifico = 1;
                //    }
                //})
                //if (modifico == 0) {
                //    MostrarMensaje("Mensaje", "No se encontró cambio en la factura!");

                //} else {

                //    notaCredito.Observacion = $("#Observacion").val();
                //    notaCredito.IdSupervisor = IdSupervisor;
                //    CrearNotaCredito();
            //}

        }

});

function CrearfrmNotaCredito() {

    if (notaCredito != undefined) {

        $.each($(".chxGrupo"), function (i, item) {
            if ($(item).is(":checked")) {
                var id = $(item).data("id");

                $.each(notaCredito.DetalleFactura, function (i2, item2) {
                    if (item2.IdDetalleFactura == id) {
                        item2.NotaCredito = true;
                        item2.PrecioNotaCredito = $("#Id_" + id).val().replace(/\./g, '');
                    }
                });
            }
        });

        notaCredito.Observacion = $("#Observacion").val();
        notaCredito.IdSupervisor = IdSupervisor;
        CrearNotaCredito();
    }  
}

function ValidarCampos() {

    var result = true;
    $.each($(".chxGrupo"), function (i, item) {
        if ($(item).is(":checked")) {
            var id = $(item).data("id");
            if ($("#Id_" + id).hasClass('errorValidate')) {
                result = false
            }
        }
    });

    return result;
}

$("#btnCancelarNotaCredito").click(function () {
    MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "Cancelar", "");
});


function Cancelar() {
    IdSupervisor = 0;
    window.location = urlBase + "Home/Index";
}

function ProductosNotaCredito(id) {
    if (notaCredito != undefined) {
        $.each(notaCredito.DetalleFactura, function (i, item) {
            if (item.IdDetalleFactura == id) {
                item.NotaCredito = true;
                item.PrecioNotaCredito = $("#Id_" + id).val().replace('.', '');
            }
        });

        $("#tr_" + id).hide()

    }
}
//cambiooquitar:
function CrearNotaCredito() {
    iniciarProceso();
    
    $.ajax({
        url: urlBase + 'NotaCredito/GuardarNotaCredito',
        ContentType: 'application/json; charset-uft8',
            dataType: 'JSON',
                async: false,
        type: 'POST',
            data: {
                modeloNotaCredito: notaCredito
        },
            success: function (r) {
                finalizarProceso();
            if (r.Correcto) {
                MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", "NotaCredito/Index", "success");
                } else {
                MostrarMensaje("Mensaje", "Error comuniqueselo al administrador");
                }
                },
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
            finalizarProceso();
        },
        complete: function (data) {

        }
    })
   

}

//cambiooquitar:
function ConsultarProductos(factura) {
    iniciarProceso();
    $.ajaxSetup({ cache: false });

    $.ajax({
        url: urlBase + 'NotaCredito/DetalleFactura',
        contenType: 'application/json; charset-utf8',
        Type: 'GET',
        data: { codigoFactura: factura },
        success: function (resp) {
            
            finalizarProceso();
            $("#listView").html(resp);
            if (resp == null || $.trim(resp).length == 0) {
                $("#btnAceptarNotaCredito").val("Buscar");
                notaCredito = undefined;
             
                MostrarMensaje("Mensaje", "No se encontro la factura!");
            } else {
                if (FacturaAnulada === true) 
                    MostrarMensajeRedireccion("Importante", "La factura consultada esta anulada.", "NotaCredito/Index", "warning");
                else
                    $("#btnAceptarNotaCredito").val("Guardar");
            }
        },
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
            finalizarProceso();
        },
        complete: function (data) {

        }

    });
}


function SetEvent() {

    $(".lnkDelete").click(function () {
        var id = $(this).data("id");
        
        if (!$("#Id_" + id).hasClass('errorValidate')) {
            MostrarConfirm('Importante!', '¿Está seguro de eliminar este producto? ', 'ProductosNotaCredito', id);
            //ProductosNotaCredito(id);
        }
    });

     $(".precioProducto").keyup(function() {

         var id = $(this).data("id");

         if ($.trim($(this).val()).length > 0 && $("#chx_" + id).is(":checked")) {            
             $(this).attr("data-mensajeerror", "");
             $(this).removeClass("errorValidate");
             QuitarTooltip();

             var precioFactura = $(this).data("precio");
             var precio = parseInt(ReplaceAll($(this).val(), '.', ''));
             if (precio == 0) {
                 $(this).val(EnMascarar(precioFactura));
             } else {
                 if (precio > precioFactura) {
                     $(this).attr("data-mensajeerror", "El valor no puede ser superior al costo del producto");
                     $(this).addClass("errorValidate");
                     mostrarTooltip();
                 }
             }
         }
            
     });

     $(".chxGrupo").click(function () {

         var id = $(this).data("id");
         var obj = $("#Id_" +id);

         if ($(this).is(':checked')) {

             QuitarTooltip();
             $(obj).removeClass("errorValidate");

             var precioFactura = $(obj).data("precio");
             var precio = parseInt(ReplaceAll($(obj).val(), '.', ''));
             if (precio == 0) {
                 $(obj).val(EnMascarar(precioFactura));
             } else {
                 if (precio > precioFactura) {
                     $(obj).attr("data-mensajeerror", "El valor no puede ser superior al costo del producto");
                     $(obj).addClass("errorValidate");
                     mostrarTooltip();
                 }
             }
         } else {
             QuitarTooltip();
             $(obj).removeClass("errorValidate");
        }
    });

     

     function EnMascarar(valor) {
         var val = $.trim(valor);
         var fn = val;
         if (val.length > 3)
             fn = val.substring(0, val.length - 3) + "." + val.substring(val.length - 3, val.length);

         if (val.length > 6)
             fn = fn.substring(0, fn.length - 7) + "." + fn.substring(fn.length - 7, fn.length);

         return fn;
     }

}

function CancelarLogin() {

    cerrarModal('modalCRUD');
    window.location = urlBase + "Home/Index";
}

