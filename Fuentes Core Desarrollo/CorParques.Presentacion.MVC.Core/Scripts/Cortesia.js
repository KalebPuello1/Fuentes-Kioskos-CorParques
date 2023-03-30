var objUsuario;
var listProductos = [];
var inicializadointerval = false;
var IdDetalleCortesia = 0;
$(".Numerico").mask("00000000000000");

//Consultar cortesias disponibles para usuarios visitantes

$(function () {
    $("#btnCancelar").click(function () {
        MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "Cancelar", "");
    });

    $("#btnAceptar").click(function () {
        if ($("#btnAceptar").val() == "Buscar") {
            if (validarFormulario("frmCortesias")) {
                var _cedula = $("#DocVisitante").val();
                var _cedulaEjecutivo = $("#DocEjecutivo").val();
                var _numTarjeta = $("#NumTarjeta").val();
                var _correoApp = $("#CorreoApp").val();
                if ((_cedula.length === 0 || !_cedula.trim()) && (_numTarjeta.length === 0 || !_numTarjeta.trim()) && (_correoApp.length === 0 || !_correoApp.trim()) && (_cedulaEjecutivo.length === 0 || !_cedulaEjecutivo.trim()))
                {
                    MostrarMensaje("Importante", "Hay inconsistencias en el formulario, debe diligenciar un campo", "error");
                }
                else if ((_cedula.length != 0 && _numTarjeta.length != 0) || (_cedula.length != 0 && _correoApp.length != 0) || (_numTarjeta.length != 0 && _correoApp.length != 0) || (_cedulaEjecutivo.length != 0 && _numTarjeta.length != 0) || (_cedulaEjecutivo.length != 0 && _correoApp.length != 0) || (_cedulaEjecutivo.length != 0 && _cedula.length != 0))
                {
                    MostrarMensaje("Importante", "Hay inconsistencias en el formulario, debe diligenciar solo un campo", "error");
                }

                else {
                    EjecutarAjax(urlBase + "Cortesia/ObtenerCortesiaUsuarioVisitante", "GET", { documento: _cedula, numTarjeta: _numTarjeta, correoApp: _correoApp, documentoEjecutivo: _cedulaEjecutivo }, "RespuestaConsulta", _cedula);
                }

            }
        } else {
            if (listProductos.length == 0) {
                MostrarMensaje("Importante", "Debe seleccionar un producto", "warning");
            } else {
                if ($("#btnHabilitarImpresionLinea").val() == "Activar Imp en línea") {                    
                    EjecutarAjax(urlBase + "Cortesia/GuardarCortesiaUsuarioVisitante", "POST", JSON.stringify({ usuarioVisitante: objUsuario, productos: listProductos, IdDetalle: IdDetalleCortesia }), "RespuestaGuardarCortesia", null);
                }
                else {
                    EjecutarAjax(urlBase + "Cortesia/GuardarCortesiaUsuarioVisitanteImpresionLinea", "POST", JSON.stringify({ usuarioVisitante: objUsuario, productos: listProductos }), "RespuestaGuardarCortesia", null);                    
                }
                
            }
        }
    });

});

function Cancelar() {
    window.location = urlBase + "Cortesia/Index";
}
function ActivarImpresion() {
    if ($("#btnHabilitarImpresionLinea").val() == "Activar Imp en línea") {
        $("#btnHabilitarImpresionLinea").val("Inactivar Imp en línea");
        $("#txtCodigoImpresion").show();
        $("#txtCodigo").hide();
        listProductos = [];
        ActualizarTablaCompras();
    }
    else {
        $("#btnHabilitarImpresionLinea").val("Activar Imp en línea");
        $("#txtCodigoImpresion").hide();
        $("#txtCodigo").show();
        listProductos = [];
        ActualizarTablaCompras();
    }
}
function RespuestaConsulta(rta, doc) {
    console.log(rta);
    console.log(rta.Elemento);
    console.log(rta.Elemento.NumeroDocumento);
    console.log(doc);
    if (rta.Elemento.NumeroDocumento != null || rta.Elemento.TipoDocumento != null ) {
        objUsuario = rta.Elemento;
        $("#DocVisitante").val("");
        $("#DocVisitante").hide();
        $("#DocEjecutivo").val("");
        $("#DocEjecutivo").hide();
        $("#NumTarjeta").val("");
        $("#NumTarjeta").hide();
        $("#CorreoApp").val("");
        $("#CorreoApp").hide();
        $("#MostrarDatos").show();
        if (rta.Elemento.Apellidos != null) {
            $("#NombreVisitante").html(rta.Elemento.Nombres + ' ' + rta.Elemento.Apellidos);
        }
        else {
            $("#NombreVisitante").html(rta.Elemento.Nombres );
        }
       
        $("#CedulaVisitante").html(rta.Elemento.NumeroDocumento);
        $("#CorreoVisitante").html(rta.Elemento.Correo);
        //$("#TelefonoVisitante").html(rta.Telefono);
        $("#lblCantidadCortesias").html(rta.Elemento.Cantidad);
        $("#txtNumdocumentoV").val(rta.Elemento.NumeroDocumento);
        $("#txtNumTarjetaFAN").val(rta.Elemento.NumTarjetaFAN);
        $("#txtTipoCortesiaV").val(rta.Elemento.IdTipoCortesia);
        $("#btnAceptar").val("Aceptar");
        if (rta.Elemento.IdTipoCortesia == 1 || rta.Elemento.IdTipoCortesia == 5 || rta.Elemento.IdTipoCortesia == 2) {
            EjecutarAjax(urlBase + "Cortesia/ObtenerDetalleCortesia", "GET", { documento: rta.Elemento.NumeroDocumento, IdTipoCortesia: rta.Elemento.IdTipoCortesia, numeroTarjetaFAN: rta.Elemento.NumTarjetaFAN }, "RespuestaConsultaDetalle", null);
        }
       
       //$("#DocVisitante").removeClass("required");
    }
    else {
        objUsuario = null;
        $("#DocVisitante").show();
        $("#DocEjecutivo").show();
        $("#NumTarjeta").show();
        $("#CorreoApp").show();
        $("#MostrarDatos").hide();
        $("#NombreVisitante").val("");
        $("#lblCantidadCortesias").val("");
        $("#btnAceptar").val("Buscar");
        $("#DocVisitante").val("");
        $("#DocEjecutivo").val("");
        $("#NumTarjeta").val("");
        $("#CorreoApp").val("");
        // $("#DocVisitante").addClass("required");
        MostrarMensaje("Mensaje", "El usuario visitante no se encuentra disponible para redimir una cortesía");
    }
}

$('#txtCodigo').keypress(function (e) {
    if (!inicializadointerval) {
        inicializadointerval = true;
        var refreshIntervalId = setInterval(function () { ConsultarBoleta(); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);
    }
});

$('#txtCodigoImpresion').keypress(function (e) {
    if (!inicializadointerval) {
        inicializadointerval = true;
        var refreshIntervalId = setInterval(function () { ConsultarBoletaImpresionEnLinea(); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);
    }
});

function RespuestaConsultaDetalle(data) {
    if (isNaN(data)) {
        $("#Detallecortesia").html(data);
        finalizarProceso();
    } 
}

function MostrarConfirmCampoEntrada() {
    swal({
        title: 'Correo para enviar QR',
        html: '<input type="email" class="form-control nDiscount_fixed" autocomplete="off" style="height: 34px;" value="">',
        showCancelButton: true,
        confirmButtonText: 'Submit',
        preConfirm: function (value) {
            return new Promise(function (resolve, reject) {

                if ($(".nDiscount_fixed").val() === "") {
                    swal.showValidationError(
                        'please enter a refund amount.'
                    )
                    swal.getConfirmButton().disabled = false;
                    swal.getCancelButton().disabled = false;
                }
                else {
                    swal.resetValidationError();
                    swal.getConfirmButton().disabled = true;
                    swal.getCancelButton().disabled = true;
                    var emailRegex = /^[-\w.%+]{1,64}@(?:[A-Z0-9-]{1,63}\.){1,125}[A-Z]{2,63}$/i;
                    //Se muestra un texto a modo de ejemplo, luego va a ser un icono
                    if (emailRegex.test($(".nDiscount_fixed").val())) {
                        EjecutarAjax(urlBase + "Cortesia/EnviarCortesiasporCorreoQR", "POST", JSON.stringify({ correo: $(".nDiscount_fixed").val(), productos: listProductos }), "RespuestaGuardarQR", null);

                    } else {
                        swal.showValidationError(
                            'email incorrecto.'
                        )
                        swal.getConfirmButton().disabled = false;
                        swal.getCancelButton().disabled = false;
                        return false;
                    }


                    resolve()
                }


            })
        }
    }).then(function (result) {
        var swalrebateamount = parseFloat($('.nDiscount_fixed').val()).toFixed(2);

        if (swalrebateamount != "" && swalrebateamount != "0") {
            //some code
        }
    }).catch(swal.noop)
}

function ConsultarBoleta() {
    var obj = $("#txtCodigo");
    var numdocument = $("#txtNumdocumentoV"); 
    var IDtipoCorteV = $("#txtTipoCortesiaV");
    var numerotarjeta = $("#txtNumTarjetaFAN");
    if (obj.length > 0) {        
        EjecutarAjax(urlBase + "Cortesia/ObtenerProducto", "POST", JSON.stringify({ CodBarra: obj.val(), Documento: numdocument.val(), numtarjeta: numerotarjeta.val(), productos: listProductos, IdTipoCortesia: IDtipoCorteV.val(), impresionLinea: 0, IdDetalle: IdDetalleCortesia }), "successObtenerProducto", null);
        obj.val("");
    }
}

function ConsultarBoletaImpresionEnLinea() {
    var obj = $("#txtCodigoImpresion");
    var numdocument = $("#txtNumdocumentoV");
    var IDtipoCorteV = $("#txtTipoCortesiaV");
    var numerotarjeta = $("#txtNumTarjetaFAN");
    if (obj.length > 0) {
        EjecutarAjax(urlBase + "Cortesia/ObtenerProducto", "POST", JSON.stringify({ CodBarra: obj.val(), Documento: numdocument.val(), numtarjeta: numerotarjeta.val(), productos: listProductos, IdTipoCortesia: IDtipoCorteV.val(), impresionLinea: 1 }), "successObtenerProductoImpresion", null);
        obj.val("");
    }
}

function successObtenerProducto(rta) {

    if (rta.IdProducto > 0) {
        var obj = BuscarIdBrazalete(rta.IdDetalleProducto);
        if (obj === null) {
            rta.Cantidad = 1;
            rta.PrecioTotal = rta.Precio;
            listProductos.push(rta);
            ActualizarTablaCompras();
        } else
            MostrarMensaje("Importante", "La boleta ya fue agregado en la lista de productos.", "warning");
    } else {

        MostrarMensaje("Importante", rta.MensajeValidacion, "warning");
    }
}
function successObtenerProductoImpresion(rta) {

    if (rta.IdProducto > 0) {

            rta.Cantidad = 1;
            rta.PrecioTotal = rta.Precio;
            listProductos.push(rta);
            ActualizarTablaCompras();
    } else {

        MostrarMensaje("Importante", rta.MensajeValidacion, "warning");
    }
}
//Buscar 
function BuscarIdBrazalete(id) {
    var objReturn = null;

    $.each(listProductos, function (i, item) {
        if ($.trim(item.IdDetalleProducto) == $.trim(id)) {
            objReturn = item;
            return false;
        }
    });

    return objReturn;
}

function ActualizarTablaCompras() {

    if (listProductos.length > 0) {
        var tablaHead = "<div class='row x_panel'> <table class='table table-striped jambo_table' width='100%'>";
        tablaHead += "<thead>" + "<th>Nombre</th>"
            + "<th></th>"
            + "</thead>";
        var tablaBody = "<tbody>";

        $.each(listProductos, function (i, item) {

            tablaBody += "<tr>"
                + "<td style='vertical-align: middle;'>" + (item.ConseutivoDetalleProducto == null ? item.Nombre : item.Nombre + " " + item.ConseutivoDetalleProducto) + "</td>"
                + "<td style='vertical-align: middle;'><a data-id ='" + item.IdProducto + "' class='evtEliminar' id='" + item.IdDetalleProducto + "' href='javascript:void(0)'><span class='fa fa-trash-o IconosPos' aria-hidden='true' ></span></a></td></tr>";

        });

        var footer = "</tbody></table></div>";
        $("#listProductos").html(tablaHead + tablaBody + footer);



        $(".evtEliminar").click(function () {

            var id = $(this).data('id');
            var idDetalle = $(this).attr("id");

            if (listProductos.length == 1)
                listProductos = [];
            else {

                if (idDetalle != '0') {

                    $.each(listProductos, function (i) {
                        if (listProductos[i].IdDetalleProducto == idDetalle) {
                            listProductos.splice(i, 1);
                            return false;
                        }
                    });
                }
            }
            ActualizarTablaCompras();
        });

    } else {
        $("#listProductos").html("");
    }
}

function RespuestaGuardarCortesia(rta) {
    if (rta.Correcto) {
        MostrarConfirmSINO("Importante!", "¿Desea enviar los QR de la cortesía mediante correo electronico ?", "ConfirmarAutorizacionCortesia", "CancelarDescargoProductos", "");
       /* MostrarConfirm("Importante", "¿Desea enviar los QR de la cortesía mediante correo electronico ?", "ConfirmarAutoricacionCortesia", 1);*/
       /* EjecutarAjax(urlBase + "Cortesia/EnviarCortesiasporCorreoQR", "POST", JSON.stringify({ usuarioVisitante: objUsuario, productos: listProductos }), "RespuestaGuardarCortesia", null)*/;
       /* MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", "Cortesia/Index", "success");*/
    }
    else {
        MostrarMensaje("Importante", "Error al generar impresión en linea", "error");
    }
}

function ConfirmarAutorizacionCortesia(id) {
    MostrarConfirmCampoEntrada();
}
function CancelarDescargoProductos(id) {
    MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", "Cortesia/Index", "success");
}
function RespuestaGuardarQR(rta) {
    MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", "Cortesia/Index", "success");
}

function Seleccionar(idDetalle) {
    IdDetalleCortesia = idDetalle;    
}