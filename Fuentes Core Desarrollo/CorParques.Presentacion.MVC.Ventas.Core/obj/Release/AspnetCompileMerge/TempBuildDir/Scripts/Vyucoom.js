var ruta = location.href
var parametros; //listaDetallePago
var listaDetallePago = [];
var lstProductosCompra = [];
var da = 0
var ingFunction = 0;
var objj = new Object();
var transaccionRedebanHabilitadaa = document.getElementById("inpuntTransaccionRedebanHabilitada").value;
var resultadoTransaccionRedebanExitosa = false;
var inicializadointerval = false;
var pintarTotal = 0;
var ii = 0;
var datoEnviarPagar = 0;
var ValorCostoComparar = 0;
var Seanulo = 0;
var nuevoDato = 0;

//var Cliente = $("#Scliente")
var Cliente = $("#NomClientePed")
var nuevoCliente = document.getElementById("NomCliente")
var IdenCliente = document.getElementById("IdeCliente")
var Mpago = $("#selectMedioPago")
var CodPedido = $("#CodPedido")
var Pago = $("#inputValorPago")
var modificarPrecio = $("#precio")

//
var nuevoClientee = "";
var identClientee = "";
var identidadCliente = "";
//

/// Variable para identificar si el cliente existe 
var clienteNoExiste = 0;
///

///

var pedidoNoExiste = 0;

///


//
var franquiciaTarjeta = "";
var numeroRerenciaTarjeta = "";
var numeroRecibo = "";
var numeroFactura = "";
//
var numeroFacturaEliminar = "";
var numeroReciboEliminar = "";

objj.lol = "this data for test"
objj.ele = "relol ";
//console.log(obj)
///

console.log(transaccionRedebanHabilitadaa)

var envCliente;
var envForPago;
var envCodPedido;



$("#Scliente").select2({
    placeholder: "* Seleccione la unidad"
});
$("#selectMedioPago").select2({
    placeholder: "* Seleccione la unidad"
});

console.log(`${ruta}/InsertDatos`)
$("#tableBodyPagos").html("");
$("document").ready(() => {



    $("#btnCancelar").click((e) => {
        $.ajax({
            url: `${ruta}/reiniciarDat`,
            method: 'GET',
            success: () => {
                console.log("Se reiniciaron los datos completamente")
                //listaDetallePago = [];
            },
            error: () => {
                console.log("No Se reiniciaron los datos completamente")
            }
        })
        e.preventDefault();
        console.log("Cancelando pago...")
        ValorCostoComparar = 0;
        nuevCliente = document.getElementById("NueCliente")
        nuevCliente.style.display = "none"
        CodPedido.val("")
        $("#NomClientePed").val("")
        $("#IdApagar").val("")
        pedidoNoExiste = 0
        clienteNoExiste = 0;
        nuevoCliente.value = "";
        listaDetallePago = [];
        da = "";
        $("#tableBodyPagos").html("");
        calculaTotal();
        
    })

    $("#IdApagar").mask("000.000.000", {reverse: true})
    $("#inputValorPago").mask("000.000.000", { reverse: true });

    $("#btnPagar").click(() => {
        if ($("#Scliente option:selected").text() == " --Seleccione cliente-- ") {
            MostrarMensaje("Importante", "Por favor seleccione un cliente", "error")
        } else {
            console.log(Mpago.val() + "  ----- f" + $("#selectMedioPago option:selected").text())
            if ($("#selectMedioPago option:selected").text() == " --Seleccione Forma Pago-- ") {
                MostrarMensaje("Importante", "Por favor seleccione metodo de pago", "error")
            } else {
                /* if (Pago.val() <= 0) {
                    MostrarMensaje("Importante", "Por favor ingrese el monto a pagar", "error")
                } else {*/
                if (ingFunction == 0) {
                    MostrarMensaje("Importante", "Por favor agregue los productos", "error")
                } else {
                    console.log(Cliente.val())
                    //EjecutarAjax(`${ruta}/InsertDatos`, "GET", { dato: CodPedido.val(), cliente: Cliente.val() == "22222-SIN REGISTROS CLIENTE" ? identidadCliente : Cliente.val(), mpago: $("#selectMedioPago option:selected").text(), pago: Pago.val(), cambio: da }, "lol", null)

                    $.ajax({
                        url: `${ruta}/MostrarDato`,
                        method: "POST",
                        data: { medioPago: listaDetallePago },
                        success: (e) => {
                            console.log(e)
                        },
                        error: (e) => {
                            console.log(e)
                        }
                    })
                    var PesosCuadre = 0;
                    $.each(listaDetallePago, (i, item) => {
                        PesosCuadre += item.Valor
                    })
                    if (PesosCuadre == datoEnviarPagar) {
                        console.log("Si esta el cuadre " + PesosCuadre + " -- " + datoEnviarPagar)
                    }
                    console.log(listaDetallePago)
                    console.log(envCliente + " * " + envForPago)
                    console.log(datoEnviarPagar + " $$$$ *********** ")
                    //EjecutarAjax(`${ruta}/InsertDatos`, "GET", { dato: CodPedido.val(), cliente: Cliente.val() == "22222-SIN REGISTROS CLIENTE" ? identidadCliente : Cliente.val(), mpago: $("#selectMedioPago option:selected").text(), pago: datoEnviarPagar, cambio: da, medioPago: listaDetallePago, exist: clienteNoExiste}, "lol", null)
                    EjecutarAjax(`${ruta}/InsertDatos`, "GET", { dato: CodPedido.val(), cliente: Cliente.val() == "22222-SIN REGISTROS CLIENTE" ? identidadCliente : Cliente.val(), mpago: $("#selectMedioPago option:selected").text(), pago: datoEnviarPagar, cambio: da, medioPago: listaDetallePago, exist: clienteNoExiste}, "lol", null)
                    console.log(listaDetallePago)
                    clienteNoExiste = 0;
                    pedidoNoExiste = 0
                    $("#NomClientePed").val("")

                    ingFunction = 0;
                    $("#CodPedido").val("")
                    ValorCostoComparar = 0;
                    listaDetallePago = []
                    $("#tableBodyPagos").html("");
                    //nuevoCliente.style.display = "none"
                    var nuev = document.getElementById("NueCliente")
                    nuev.style.display = "none";
                    nuevoCliente.value = "";
                    //IdenCliente.style.display = "none"
                    IdenCliente.value = "";
                    calculaTotal()
                   
                    
                }
                // }
            }
        }

        $("#Scliente").val("0")
        $('#Scliente').change();
        $("#selectMedioPago").val('0')
        $('#selectMedioPago').change();

    })

    $("#agregarCliente").click((e) => {
        e.preventDefault()
        identClientee = IdenCliente.value;
        nuevoClientee = nuevoCliente.value;
        console.log(identClientee + "-" + nuevoClientee)
        identidadCliente = identClientee + "-" + nuevoClientee
        document.getElementById("agregarCliente").style.display = "none"
        document.getElementById("IdApagar").setAttribute("disabled","true")
    })
 



    $("#Scliente").change(() => {
        console.log(Cliente.val() + " Se realizo un cambio ffffffff ")
        if (Cliente.val() == "22222-SIN REGISTROS CLIENTE" && nuevoClientee == "") {
            nuevCliente = document.getElementById("NueCliente")
            nuevCliente.style.display = "block"
            var Clientee = document.getElementById("Clientee")
            //Clientee.style.display = "none"

            clienteNoExiste = 1;
            //nuevoDato = 1;
        } else {
            nuevCliente = document.getElementById("NueCliente")
            nuevCliente.style.display = "none"
            IdenCliente.value = "";
            nuevoCliente.value = "";
            clienteNoExiste = 0;
        }
    })


    $("#CodPedido").change(() => {
        $("#tableBodyPagos").html("");
        $("#precio").mask("000.000.000", { reverse: true });
        $("#inputValorPago").mask("000.000.000", { reverse: true });
        console.log(Cliente.val() + " Se realizo un cambio")
           
            var mos = document.getElementById("Mostrar")
            mos.style.display = "none";
            console.log("##############")
            var inputDato = $("#inputValorPago")
            if (CodPedido.val().length >= 4) {
                if (nuevoDato == 1) {
                    nuevoClientee = nuevoCliente.value;
                } else {
                    $.ajax({
                        url: `${ruta}/BuscarPedido`,
                        method: "POST",
                        data: { dato: CodPedido.val() },
                        success: (e) => {
                            if (e.Valor == "Pedido_no_existe") {
                                ValorCostoComparar = 0
                                datoEnviarPagar = 0
                                inputDato.val("");
                                mos.style.display = "none";
                                console.log(e)
                                $("#precio").mask("000.000.000", { reverse: true });
                                pedidoNoExiste = 1
                                console.log(e.NombCliente)
                                //var pagar = document.getElementById("IdApagar")
                                var pagar = $("#IdApagar")
                                //pagar.value = ""
                                pagar.val("")
                                pagar.removeAttr("disabled")
                                nuevoClientee = ""
                                if (e.NombCliente == "22222-SIN REGISTROS CLIENTE" && nuevoClientee == "") {
                                    $("#NomClientePed").val("22222-SIN REGISTROS CLIENTE")
                                    console.log(" !!!! $$$$ ###### !!!!!  ")
                                    nuevCliente = document.getElementById("NueCliente")
                                    nuevCliente.style.display = "block"
                                    var Clientee = document.getElementById("Clientee")
                                    //Clientee.style.display = "none"
                                    clienteNoExiste = 1;
                                    //nuevoDato = 1;
                                    document.getElementById("agregarCliente").style.display = "block"
                                } else {
                                    nuevCliente = document.getElementById("NueCliente")
                                    nuevCliente.style.display = "none"
                                    IdenCliente.value = "";
                                    nuevoCliente.value = "";
                                    clienteNoExiste = 0;
                                }
                            } else {
                                nuevCliente = document.getElementById("NueCliente")
                                nuevCliente.style.display = "none"
                                $("#NomClientePed").val("")
                                pedidoNoExiste = 0
                                clienteNoExiste = 0;
                                console.log(e.Valor.lengt + " ***** ")
                                if (e.Valor.length == 6) {
                                    console.log(e)
                                    modificarPrecio.text(`$${e.Valor}`)
                                    inputDato.val(`${e.Valor}`)
                                    /*$("#inputValorPago").mask("000.000.000", { reverse: true });*/
                                    ValorCostoComparar = e.Valor;
                                    $("#inputValorPago").mask("000.000.000");
                                    $("#precio").mask("000.000.000,00");
                                    $("#inputValorPago").mask("000.000.000", { reverse: true });
                                    console.log(e.NombCliente)
                                    $("#NomClientePed").val(e.NombCliente)
                                    
                                    ///// Verificar
                                    datoEnviarPagar = e.Valor;
                                }
                                else {
                                    /*$("#inputValorPago").mask("000.000.000", { reverse: true });*/
                                    console.log(e)
                                    modificarPrecio.text(`$${e.Valor}`)
                                    ValorCostoComparar = e.Valor;
                                    inputDato.val(`${e.Valor}`)
                                    $("#inputValorPago").mask("000.000.000");
                                    $("#precio").mask("000.000.000", { reverse: true });
                                    $("#inputValorPago").mask("000.000.000", { reverse: true });
                                    console.log(e.NombCliente)
                                    $("#NomClientePed").val(e.NombCliente)

                                    ///// Verificar
                                    datoEnviarPagar = e.Valor;
                                }
                            }
                        },
                        error: (e) => {
                            console.log(e)
                        }
                    })
                }
            } //Va mensaje de error, ingrese los numeros de la factura
            else {
                MostrarMensaje("importante", "Por favor inserte el numero de su factura mayor a 5", "error")
            }
        
    })
    //
  

})

function lol(f) {
    console.log(f)
    MostrarMensaje("Importante", f, "success")
}



function AgregarPagoo() {

    envCliente = $("#selectMedioPago option:selected").text()
    envForPago = $("#Scliente option:selected").text()
    //var envCodPedido = $("#").val();

        console.log(datoEnviarPagar)

        var resp = true;
        var _documento = "";
        /* var franquiciaTarjeta = "";
         var numeroRerenciaTarjeta = "";
         var numeroRecibo = ""; */
        var IdMedioPago = $("#selectMedioPago").val();

    console.log(IdMedioPago)


       QuitarTooltip();

        //SPago -> medio de pago 
        $("#selectMedioPago").removeClass("errorValidate");
        $("#inputValorPago").removeClass("errorValidate");
        console.log("pasoooooo por agregar pago")
        //Validacion transaccion redeban activa para obligatoriedad de los campos
        if (IdMedioPago == "2") {

            if (transaccionRedebanHabilitadaa == "0") {

                _documento = $("#inputReferencia").val();
                $("#selectFranquicia").removeClass("errorValidate");
                $("#inputReferencia").removeClass("errorValidate");
            }
            else {
                _documento = $("#inputReferencia").val();
            }
        }
        else {
            _documento = $("#inputReferencia").val();
            $("#selectFranquicia").removeClass("errorValidate");
            $("#inputReferencia").removeClass("errorValidate");
        }


        if ($.trim(IdMedioPago).length == 0) {
            $("#selectMedioPago").attr("data-mensajeerror", "Este campo es obligatorio");
            $("#selectMedioPago").addClass("errorValidate");
            console.log("entro y esta mal")
            resp = false;
        }


        if ($("#inputValorPago").val() == "" || $("#inputValorPago").val() == "0") {
            $("#inputValorPago").attr("data-mensajeerror", "Este campo es obligatorio");
            $("#inputValorPago").addClass("errorValidate");
            console.log("entro y esta mallll")
            resp = false;
        }
    console.log($("#inputValorPago").val() + " --- " + $("#inputValorPago").val() + " --- " + resp)
        if (!resp) {
            mostrarTooltip();
            MostrarMensaje("Importante", "Por favor seleccione un medio de pago", "error");
            return false;
        }

        if (IdMedioPago == parametros.IdMedioPagoBonoRegalo) {

            console.log("medio de pago regalo")
            var _referenciaBono = $("#inputReferencia").val();
            var _valorBono = $("#inputValorPago").val();

            if (!ValidarBonoRegalo(_referenciaBono, parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""))))
                return false;
        }

        var documento = $("#inputReferencia").val();

        if ($("#selectMedioPago").val() != ""
            && $("#inputValorPago").val() != ""
            && $("#inputValorPago").val() != "0") {

            if (($("#inputReferencia").val() == "" && IdMedioPago == parametros.IdMedioPagoEfectivo)
                || $("#inputReferencia").val() != "" || ($("#inputReferencia").val() == "" && IdMedioPago == "2" && transaccionRedebanHabilitadaa == "1")) {

                //Cambioquitar
                if ((IdMedioPago == parametros.IdMedioPagoTarjetaDebito || IdMedioPago == parametros.IdMedioPagoTarjetaCredito) && (transaccionRedebanHabilitadaa == "0")) {
                    if ($("#selectFranquicia").val() == "") {
                        $("#selectFranquicia").attr("data-mensajeerror", "Este campo es obligatorio");
                        $("#selectFranquicia").addClass("errorValidate");
                        // alert("error obligatorio");
                        return false;
                    }
                }



                //Al agregar un medio de pago Nomina, si ya hay un medio de pago no lo debe agregar. Cambioquitar
                if (IdMedioPago == parametros.IdMedioPagoDescuentoNomina) {
                    if (listaDetallePago.length > 0) {
                        MostrarMensaje("Importante", "Solo puede haber una forma de pago por Nomina.", "error");
                        return false;
                    } else {
                        var documento = ValidarCupoEmpleado($("#inputValorPago").val(), documento);
                        if (documento.length == 0)
                            return false;
                        else
                            _documento = $("#inputReferencia").val();
                    }
                }

                //Al agregar un medio de pago diferente a Nomina y ya hay medios de pago Nomina, no lo debe dejar. Cambioquitar
                var restringirPago = false;
                if (listaDetallePago.length > 0) {
                    $.each(listaDetallePago, function (i, item) {
                        if (item.IdMedioPago == parametros.IdMedioPagoDescuentoNomina || item.IdMedioPago == parametros.IdMedioPagoDescuentoNomina.toString()) {
                            restringirPago = true;
                            return false;
                        }
                    });
                }

                if (restringirPago) {
                    MostrarMensaje("Importante", "Solo puede haber una forma de pago por Nomina.", "error");
                    return false;
                }

                if (IdMedioPago == "6") {
                    var validaObtenerTotal = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                    if (empleadoNominaSeleccionado.CupoRestante < validaObtenerTotal) {
                        MostrarMensajeHTML("Importante", "No cuenta con cupo suficiente para pago por Nomina. <br /> Cupo Restante: " + empleadoNominaSeleccionado.CupoRestante, "error");
                        return false;
                    }
                }

                var hayPagoEfectivoAdd = false;
                if (IdMedioPago == parametros.IdMedioPagoEfectivo) {
                    if (listaDetallePago.length > 0) {
                        $.each(listaDetallePago, function (i, item) {
                            if (item.IdMedioPago == 1 || item.IdMedioPago == "1") {
                                hayPagoEfectivoAdd = true;
                                var newValor = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                                item.Valor += newValor;
                            }
                        });
                    }
                }
                //Cambioquitar
                if (IdMedioPago == parametros.IdMedioPagoBonoRegalo) {
                    if (listaDetallePago.length > 0) {
                        $.each(listaDetallePago, function (i, item) {
                            if ((item.IdMedioPago == parametros.IdMedioPagoBonoRegalo && item.referencia == parseInt($("#inputValorPago").val())) ||
                                (item.IdMedioPago == parametros.IdMedioPagoBonoRegalo.toString() && item.referencia == parseInt($("#inputValorPago").val()))) {
                                hayPagoEfectivoAdd = true;
                                var newValor = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                                item.Valor += newValor;
                            }
                        });
                    }
                }
                var newValor = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                var valortotal = newValor;
                if (IdMedioPago == parametros.IdMedioPagoTarjetaRecargable) {
                    if (listaDetallePago.length > 0) {
                        $.each(listaDetallePago, function (i, item) {
                            if ((item.IdMedioPago == 9 || item.IdMedioPago == "9") && item.NumReferencia === _documento) {
                                hayPagoEfectivoAdd = true;
                                item.Valor += newValor;
                                valortotal = item.Valor;
                            }
                        });
                    }
                }

                //Ajustes para validar misma franquicia y misma referencia
                var MismaFranquicia = false;

                if (IdMedioPago == parametros.IdMedioPagoTarjetaDebito && (transaccionRedebanHabilitadaa == "0")) {
                    if (listaDetallePago.length > 0) {
                        $.each(listaDetallePago, function (i, item) {
                            if (item.IdMedioPago == parametros.IdMedioPagoTarjetaDebito || item.IdMedioPago == parametros.IdMedioPagoTarjetaDebito.toString()) {
                                if (item.IdFranquicia == $("#selectFranquicia").val()) {
                                    if (item.NumReferencia == $("#inputReferencia").val()) {
                                        MismaFranquicia = true
                                    }
                                }
                            }
                        });
                    }
                }

                //DANR: Validacion tarjeta recargable
                /*********************************************************************/
                if (IdMedioPago == parametros.IdMedioPagoTarjetaRecargable) {
                    var documento = ValidarTarjetaRecargable(valortotal, documento);
                    if (documento.length == 0)
                        return false;
                    else
                        _documento = $("#inputReferencia").val();

                }
                /********************************************************************/


                //DANR: Validacion pago app
                /*********************************************************************/
                if (IdMedioPago == parametros.IdMedioPagoAPP) {
                    var documento = ValidarPagoApp(valortotal, documento);
                    if (documento.length == 0)
                        return false;
                    else
                        _documento = $("#inputReferencia").val();

                }

                /*********************************************************************/
                if (MismaFranquicia) {
                    MostrarMensaje("Importante", "No se puede referenciar dos tarjetas con la misma franquicia y mismo número de referencia.", "error");
                    return false;
                }

                //Transacción con Redeban
                /*********************************************************************/
                if (IdMedioPago == "2" && transaccionRedebanHabilitadaa == "1") {
                    var validaObtenerTotal = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                    //Envio de solicitud a redeban
                    urlaction = urlBase + "Redeban/IniciarTransaccionRedeban";
                    $.ajax({
                        url: urlaction,
                        ContentType: 'application/json; charset-uft8',
                        dataType: 'JSON',
                        type: 'GET',
                        data: {
                            montoPago: validaObtenerTotal
                        },
                        async: false,
                        success: function (respuesta) {
                            if (respuesta != null) {
                                if (respuesta.MensajeRespuesta != "OK" && respuesta.CodigoRespuesta != "00") {
                                    resultadoTransaccionRedebanExitosa = false;
                                    MostrarMensajeHTML("Importante", respuesta.MensajeRespuesta.toString(), "error");
                                    return false;
                                }
                                else {
                                    console.log(respuesta);
                                    resultadoTransaccionRedebanExitosa = true;
                                    numeroRerenciaTarjeta = respuesta.NumeroAprobacion;
                                    franquiciaTarjeta = respuesta.Franquicia;
                                    numeroRecibo = respuesta.NumeroRecibo;
                                    //NumeroFactura
                                    numeroFactura = respuesta.NumeroFactura;
                                    MostrarMensajeHTML("Importante", "Transacción exitosa. <br /> Conexión Aprobada", "success");
                                }
                            }
                        },
                        error: function (error) {
                            MostrarMensajeHTML("Importante", "Error conectando con Redeban. <br /> Por favor intente nuevamente", "error");
                            console.log(error)
                            return false;
                        }
                    });
                }

                if (!hayPagoEfectivoAdd) {
                    if (transaccionRedebanHabilitadaa == "1" && IdMedioPago == "2" && resultadoTransaccionRedebanExitosa == true) {
                        console.log("1 ---------------------------- D");
                        var ObjectPago = new Object();
                        ObjectPago.IdMedioPago = parseInt($("#selectMedioPago").val());
                        console.log(parseInt($("#selectMedioPago").val()) + "----------------- fff")
                        //ObjectPago.DescMedioPago = $("#SPago option:selected").text(); Mpago.text()
                        //ObjectPago.DescMedioPago = Mpago.text()
                        ObjectPago.DescMedioPago = $("#selectMedioPago option:selected").text();
                        console.log(Mpago.text())
                        //ObjectPago.NumeroRecibo = numeroRecibo;
                        ObjectPago.NumeroRecibo = numeroRecibo;
                        ObjectPago.NumReferencia = numeroRerenciaTarjeta;
                        ObjectPago.IdFranquicia = $("#selectFranquicia").val();
                        ObjectPago.DescFranquicia = franquiciaTarjeta;
                        ObjectPago.Valor = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                        //ObjectPago.Valor = "1212212"
                        if (IdMedioPago == parametros.IdMedioPagoDescuentoNomina)
                            ObjectPago.NombreMedioPago = empleadoNominaSeleccionado.Nombres + ' ' + empleadoNominaSeleccionado.Apellidos;
                        listaDetallePago.push(ObjectPago);
                    }
                    else if (IdMedioPago == "2" && transaccionRedebanHabilitadaa == "0") {
                        console.log("2");
                        var ObjectPago = new Object();
                        ObjectPago.IdMedioPago = parseInt($("#selectMedioPago").val());
                        //ObjectPago.DescMedioPago = $("#selectMedioPago option:selected").text();
                        ObjectPago.DescMedioPago = $("#selectMedioPago option:selected").text();
                        console.log($("#selectMedioPago option:selected").text())
                        ObjectPago.NumReferencia = _documento;
                        ObjectPago.NumeroRecibo = "N/A";
                        ObjectPago.IdFranquicia = $("#selectFranquicia").val();
                        ObjectPago.DescFranquicia = ($("#selectFranquicia").val() == "" ? "" : $("#selectFranquicia option:selected").text());
                        ObjectPago.Valor = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                        //ObjectPago.Valor = "1212212"
                        if (IdMedioPago == parametros.IdMedioPagoDescuentoNomina)
                            ObjectPago.NombreMedioPago = empleadoNominaSeleccionado.Nombres + ' ' + empleadoNominaSeleccionado.Apellidos;
                        listaDetallePago.push(ObjectPago);
                    }

                    else if (IdMedioPago != 2) {
                        console.log("3");
                        var ObjectPago = new Object();
                        ObjectPago.IdMedioPago = parseInt($("#selectMedioPago").val());
                        //ObjectPago.DescMedioPago = "TESTED EFECTIVO";
                        ObjectPago.DescMedioPago = $("#selectMedioPago option:selected").text();
                        ObjectPago.NumReferencia = _documento == "" ? "N/A" : _documento;
                        //ObjectPago.NumReferencia = "1212212";
                        ObjectPago.NumeroRecibo = "N/A";
                        //ObjectPago.NumeroRecibo = ObjectPago.Valor = "1212212";
                        ObjectPago.IdFranquicia = $("#selectFranquicia").val();
                        ObjectPago.DescFranquicia = ($("#selectFranquicia").val() == "" ? "" : $("#selectFranquicia option:selected").text());
                        ObjectPago.Valor = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                        //ObjectPago.Valor = $("#inputValorPago").val()
                        ObjectPago.PrecioTotal = "23432"
                        if (IdMedioPago == parametros.IdMedioPagoDescuentoNomina)
                            ObjectPago.NombreMedioPago = empleadoNominaSeleccionado.Nombres + ' ' + empleadoNominaSeleccionado.Apellidos;
                        listaDetallePago.push(ObjectPago);
                    }
                }



//                enviarDato(Pago.val(), CodPedido.val())
  

                if (nuevoCliente.value != "") {
                    enviarDatoo(Pago.val())
                } else {
                    enviarDato(Pago.val(), CodPedido.val())
                }


                console.log(ValorCostoComparar)





                var total = 0;
                var sHTML = '';
                $("#tableBodyPagos").html(sHTML);
                if (listaDetallePago.length > 0) {
                    $.each(listaDetallePago, function (i, item) {
                        var valorArgumentosFuncion = i + ",\'" + item.NumeroRecibo + "\',\'" + item.NumReferencia + "\'";
                        total += item.Valor;
                        sHTML += '<tr>';
                        sHTML += '     <td>' + item.DescMedioPago + '</td>';
                        sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? item.NumeroRecibo : item.NumeroRecibo) + '</td>';
                        sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.NumReferencia) + '</td>';
                        sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.DescFranquicia) + '</td>';
                        sHTML += '     <td align="right" id="Valor">' + item.Valor + '</td>';
                        //sHTML += '     <td align="center"><a onclick="evtEliminarMedioPagoo(' + valorArgumentosFuncion + ' );"><span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
                        //sHTML += '     <td align="right"><a data-id="' + i + '" class="evtEliminarMedioPago"><span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
                        sHTML += '     <td align="right"><a data-id="' + i + '" class="evtEliminarMedioPago"> <span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
                        sHTML += '</tr>';

                    });

                    //if (IdMedioPago == "2" && resultadoTransaccionRedebanExitosa == true) {
                    //    $.each(listaDetallePago, function (i, item) {
                    //        console.log(item.DescMedioPago);
                    //        total += item.Valor;
                    //        sHTML += '<tr>';
                    //        sHTML += '     <td>' + item.DescMedioPago + '</td>';
                    //        /*  sHTML += '     <td>' + NumeroRecibo.toString() + '</td>';*/
                    //        sHTML += '     <td>' + "000034" + '</td>';
                    //        sHTML += '     <td>' + NumeroRerenciaTarjeta.toString() + '</td>';
                    //        sHTML += '     <td>' + FranquiciaTarjeta.toString() + '</td>';
                    //        sHTML += '     <td align="right">' + item.Valor + '</td>';
                    //        sHTML += '     <td align="center"><a onclick="evtEliminarMedioPago(' + i + ');"><span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
                    //        sHTML += '</tr>';

                    //    });
                    //}
                    //else {
                    //    $.each(listaDetallePago, function (i, item) {
                    //        total += item.Valor;
                    //        sHTML += '<tr>';
                    //        sHTML += '     <td>' + item.DescMedioPago + '</td>';
                    //        sHTML += '     <td>' + "N/A" + '</td>';
                    //        sHTML += '     <td>' + item.NumReferencia + '</td>';
                    //        sHTML += '     <td>' + item.DescFranquicia + '</td>';
                    //        sHTML += '     <td align="right">' + item.Valor + '</td>';
                    //        sHTML += '     <td align="center"><a onclick="evtEliminarMedioPago(' + i + ');"><span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
                    //        sHTML += '</tr>';

                    //    });
                    //}
                }

                //rEGRESAR
                //$("#tableBodyPagos").append(sHTML);



                $("#Valor").mask("000.000.000", { reverse: true });
                //Deberia estar sin comentar
                //$("#SPago").val("");


                $("#selectFranquicia").val("");
                $("#inputReferencia").val("");
                //$("#inputValorPago").val("");

                selectMedioPagoChange();


                //calculaTotal();


            } else {

                if (($("#inputReferencia").val() == "" && IdMedioPago != "1")) {

                    if (transaccionRedebanHabilitadaa == "1" && IdMedioPago == "2") {
                    }
                    else {
                        $("#inputReferencia").attr("data-mensajeerror", "Este campo es obligatorio");
                        $("#inputReferencia").addClass("errorValidate");
                    }
                }
            }

            //finalizarProceso();

        }

       // $("#inputValorPago").val("")

        var resp = true;
        var _documento = "";
        /* var franquiciaTarjeta = "";
         var numeroRerenciaTarjeta = "";
         var numeroRecibo = ""; */
        var IdMedioPago = $("#selectMedioPago").val();



        QuitarTooltip();
        //SPago -> medio de pago 
        $("#selectMedioPago").removeClass("errorValidate");
        $("#inputValorPago").removeClass("errorValidate");
        console.log("pasoooooo por agregar pago")
        //Validacion transaccion redeban activa para obligatoriedad de los campos
        if (IdMedioPago == "2") {

            if (transaccionRedebanHabilitadaa == "0") {

                _documento = $("#inputReferencia").val();
                $("#selectFranquicia").removeClass("errorValidate");
                $("#inputReferencia").removeClass("errorValidate");
            }
            else {
                _documento = $("#inputReferencia").val();
            }
        }
        else {
            _documento = $("#inputReferencia").val();
            $("#selectFranquicia").removeClass("errorValidate");
            $("#inputReferencia").removeClass("errorValidate");
        }


        if ($.trim(IdMedioPago).length == 0) {
            $("#selectMedioPago").attr("data-mensajeerror", "Este campo es obligatorio");
            $("#selectMedioPago").addClass("errorValidate");
            console.log(resp + "*********" + "gran error")
            resp = false;
        }


        if ($("#inputValorPago").val() == "" || $("#inputValorPago").val() == "0") {
            $("#inputValorPago").attr("data-mensajeerror", "Este campo es obligatorio");
            $("#inputValorPago").addClass("errorValidate");
            console.log(resp + "*********" + "gran error fff " + $("#inputValorPago").val() + " * " + $("#inputValorPago").val())
            resp = false;
        }
    $("#inputValorPago").val("")
    console.log(resp + "*********")
        if (!resp) {
            mostrarTooltip();
            MostrarMensaje("Importante", "Por favor seleccione un medio de pago", "error");
            return false;
        }

        if (IdMedioPago == parametros.IdMedioPagoBonoRegalo) {

            console.log("medio de pago regalo")
            var _referenciaBono = $("#inputReferencia").val();
            var _valorBono = $("#inputValorPago").val();

            if (!ValidarBonoRegalo(_referenciaBono, parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""))))
                return false;
        }

        var documento = $("#inputReferencia").val();

        if ($("#selectMedioPago").val() != ""
            && $("#inputValorPago").val() != ""
            && $("#inputValorPago").val() != "0") {

            if (($("#inputReferencia").val() == "" && IdMedioPago == parametros.IdMedioPagoEfectivo)
                || $("#inputReferencia").val() != "" || ($("#inputReferencia").val() == "" && IdMedioPago == "2" && transaccionRedebanHabilitadaa == "1")) {

                //Cambioquitar
                if ((IdMedioPago == parametros.IdMedioPagoTarjetaDebito || IdMedioPago == parametros.IdMedioPagoTarjetaCredito) && (transaccionRedebanHabilitadaa == "0")) {
                    if ($("#selectFranquicia").val() == "") {
                        $("#selectFranquicia").attr("data-mensajeerror", "Este campo es obligatorio");
                        $("#selectFranquicia").addClass("errorValidate");
                        // alert("error obligatorio");
                        return false;
                    }
                }



                //Al agregar un medio de pago Nomina, si ya hay un medio de pago no lo debe agregar. Cambioquitar
                if (IdMedioPago == parametros.IdMedioPagoDescuentoNomina) {
                    if (listaDetallePago.length > 0) {
                        MostrarMensaje("Importante", "Solo puede haber una forma de pago por Nomina.", "error");
                        return false;
                    } else {
                        var documento = ValidarCupoEmpleado($("#inputValorPago").val(), documento);
                        if (documento.length == 0)
                            return false;
                        else
                            _documento = $("#inputReferencia").val();
                    }
                }

                //Al agregar un medio de pago diferente a Nomina y ya hay medios de pago Nomina, no lo debe dejar. Cambioquitar
                var restringirPago = false;
                if (listaDetallePago.length > 0) {
                    $.each(listaDetallePago, function (i, item) {
                        if (item.IdMedioPago == parametros.IdMedioPagoDescuentoNomina || item.IdMedioPago == parametros.IdMedioPagoDescuentoNomina.toString()) {
                            restringirPago = true;
                            return false;
                        }
                    });
                }

                if (restringirPago) {
                    MostrarMensaje("Importante", "Solo puede haber una forma de pago por Nomina.", "error");
                    return false;
                }

                if (IdMedioPago == "6") {
                    var validaObtenerTotal = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                    if (empleadoNominaSeleccionado.CupoRestante < validaObtenerTotal) {
                        MostrarMensajeHTML("Importante", "No cuenta con cupo suficiente para pago por Nomina. <br /> Cupo Restante: " + empleadoNominaSeleccionado.CupoRestante, "error");
                        return false;
                    }
                }

                var hayPagoEfectivoAdd = false;
                if (IdMedioPago == parametros.IdMedioPagoEfectivo) {
                    if (listaDetallePago.length > 0) {
                        $.each(listaDetallePago, function (i, item) {
                            if (item.IdMedioPago == 1 || item.IdMedioPago == "1") {
                                hayPagoEfectivoAdd = true;
                                var newValor = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                                item.Valor += newValor;
                            }
                        });
                    }
                }
                //Cambioquitar
                if (IdMedioPago == parametros.IdMedioPagoBonoRegalo) {
                    if (listaDetallePago.length > 0) {
                        $.each(listaDetallePago, function (i, item) {
                            if ((item.IdMedioPago == parametros.IdMedioPagoBonoRegalo && item.referencia == parseInt($("#inputValorPago").val())) ||
                                (item.IdMedioPago == parametros.IdMedioPagoBonoRegalo.toString() && item.referencia == parseInt($("#inputValorPago").val()))) {
                                hayPagoEfectivoAdd = true;
                                var newValor = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                                item.Valor += newValor;
                            }
                        });
                    }
                }
                var newValor = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                var valortotal = newValor;
                if (IdMedioPago == parametros.IdMedioPagoTarjetaRecargable) {
                    if (listaDetallePago.length > 0) {
                        $.each(listaDetallePago, function (i, item) {
                            if ((item.IdMedioPago == 9 || item.IdMedioPago == "9") && item.NumReferencia === _documento) {
                                hayPagoEfectivoAdd = true;
                                item.Valor += newValor;
                                valortotal = item.Valor;
                            }
                        });
                    }
                }

                //Ajustes para validar misma franquicia y misma referencia
                var MismaFranquicia = false;

                if (IdMedioPago == parametros.IdMedioPagoTarjetaDebito && (transaccionRedebanHabilitadaa == "0")) {
                    if (listaDetallePago.length > 0) {
                        $.each(listaDetallePago, function (i, item) {
                            if (item.IdMedioPago == parametros.IdMedioPagoTarjetaDebito || item.IdMedioPago == parametros.IdMedioPagoTarjetaDebito.toString()) {
                                if (item.IdFranquicia == $("#selectFranquicia").val()) {
                                    if (item.NumReferencia == $("#inputReferencia").val()) {
                                        MismaFranquicia = true
                                    }
                                }
                            }
                        });
                    }
                }

                //DANR: Validacion tarjeta recargable
                /*********************************************************************/
                if (IdMedioPago == parametros.IdMedioPagoTarjetaRecargable) {
                    var documento = ValidarTarjetaRecargable(valortotal, documento);
                    if (documento.length == 0)
                        return false;
                    else
                        _documento = $("#inputReferencia").val();

                }
                /********************************************************************/


                //DANR: Validacion pago app
                /*********************************************************************/
                if (IdMedioPago == parametros.IdMedioPagoAPP) {
                    var documento = ValidarPagoApp(valortotal, documento);
                    if (documento.length == 0)
                        return false;
                    else
                        _documento = $("#inputReferencia").val();

                }

                /*********************************************************************/
                if (MismaFranquicia) {
                    MostrarMensaje("Importante", "No se puede referenciar dos tarjetas con la misma franquicia y mismo número de referencia.", "error");
                    return false;
                }

                //Transacción con Redeban
                /*********************************************************************/
                if (IdMedioPago == "2" && transaccionRedebanHabilitadaa == "1") {
                    var validaObtenerTotal = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                    //Envio de solicitud a redeban
                    urlaction = urlBase + "Redeban/IniciarTransaccionRedeban";
                    $.ajax({
                        url: urlaction,
                        ContentType: 'application/json; charset-uft8',
                        dataType: 'JSON',
                        type: 'GET',
                        data: {
                            montoPago: validaObtenerTotal
                        },
                        async: false,
                        success: function (respuesta) {
                            if (respuesta != null) {
                                if (respuesta.MensajeRespuesta != "OK" && respuesta.CodigoRespuesta != "00") {
                                    resultadoTransaccionRedebanExitosa = false;
                                    MostrarMensajeHTML("Importante", respuesta.MensajeRespuesta.toString(), "error");
                                    return false;
                                }
                                else {
                                    console.log(respuesta);
                                    resultadoTransaccionRedebanExitosa = true;
                                    numeroRerenciaTarjeta = respuesta.NumeroAprobacion;
                                    franquiciaTarjeta = respuesta.Franquicia;
                                    numeroRecibo = respuesta.NumeroRecibo;
                                    //NumeroFactura
                                    numeroFactura = respuesta.NumeroFactura;
                                    MostrarMensajeHTML("Importante", "Transacción exitosa. <br /> Conexión Aprobada", "success");
                                }
                            }
                        },
                        error: function (error) {
                            MostrarMensajeHTML("Importante", "Error conectando con Redeban. <br /> Por favor intente nuevamente", "error");
                            console.log(error)
                            return false;
                        }
                    });
                }

                if (!hayPagoEfectivoAdd) {
                    if (transaccionRedebanHabilitadaa == "1" && IdMedioPago == "2" && resultadoTransaccionRedebanExitosa == true) {
                        console.log("1");
                        var ObjectPago = new Object();
                        ObjectPago.IdMedioPago = parseInt($("#selectMedioPago").val());
                        //ObjectPago.DescMedioPago = $("#SPago option:selected").text(); Mpago.text()
                        ObjectPago.DescMedioPago = Mpago.text()
                        console.log(Mpago.text())
                        //ObjectPago.NumeroRecibo = numeroRecibo;
                        ObjectPago.NumeroRecibo = numeroRecibo + "Test necesario";
                        ObjectPago.NumReferencia = numeroRerenciaTarjeta;
                        ObjectPago.IdFranquicia = $("#selectFranquicia").val();
                        ObjectPago.DescFranquicia = franquiciaTarjeta;
                        ObjectPago.Valor = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                        //ObjectPago.Valor = "1212212"
                        if (IdMedioPago == parametros.IdMedioPagoDescuentoNomina)
                            ObjectPago.NombreMedioPago = empleadoNominaSeleccionado.Nombres + ' ' + empleadoNominaSeleccionado.Apellidos;
                        listaDetallePago.push(ObjectPago);
                    }
                    else if (IdMedioPago == "2" && transaccionRedebanHabilitadaa == "0") {
                        console.log("2");
                        var ObjectPago = new Object();
                        console.log("3 ------------------------- D ");
                        ObjectPago.IdMedioPago = parseInt($("#selectMedioPago").val());
                        ObjectPago.DescMedioPago = $("#selectMedioPago option:selected").text();
                        console.log($("#selectMedioPago option:selected").text())
                        ObjectPago.NumReferencia = _documento;
                        ObjectPago.NumeroRecibo = "N/A";
                        ObjectPago.IdFranquicia = $("#selectFranquicia").val();
                        ObjectPago.DescFranquicia = ($("#selectFranquicia").val() == "" ? "" : $("#selectFranquicia option:selected").text());
                        ObjectPago.Valor = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                        //ObjectPago.Valor = "1212212"
                        if (IdMedioPago == parametros.IdMedioPagoDescuentoNomina)
                            ObjectPago.NombreMedioPago = empleadoNominaSeleccionado.Nombres + ' ' + empleadoNominaSeleccionado.Apellidos;
                        listaDetallePago.push(ObjectPago);
                    }

                    else if (IdMedioPago != 2) {
                        console.log("3 ******  ");
                        var ObjectPago = new Object();
                        ObjectPago.IdMedioPago = parseInt($("#selectMedioPago").val());
                        //ObjectPago.DescMedioPago = "TESTED EFECTIVO";
                        ObjectPago.DescMedioPago = $("#selectMedioPago option:selected").text();
                        ObjectPago.NumReferencia = _documento == "" ? "N/A" : _documento;
                        //ObjectPago.NumReferencia = "1212212";
                        ObjectPago.NumeroRecibo = "N/A";
                        //ObjectPago.NumeroRecibo = ObjectPago.Valor = "1212212";
                        ObjectPago.IdFranquicia = $("#selectFranquicia").val();
                        ObjectPago.DescFranquicia = ($("#selectFranquicia").val() == "" ? "" : $("#selectFranquicia option:selected").text());
                        ObjectPago.Valor = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                        //ObjectPago.Valor = $("#inputValorPago").val()
                        ObjectPago.PrecioTotal = "23432"
                        if (IdMedioPago == parametros.IdMedioPagoDescuentoNomina)
                            ObjectPago.NombreMedioPago = empleadoNominaSeleccionado.Nombres + ' ' + empleadoNominaSeleccionado.Apellidos;
                        listaDetallePago.push(ObjectPago);
                    }
                }



                /*
                 *Aqui va la lista de pagos  
                 
                 
                 listaDetallePago
                    
                 enviarDato(Pago.val(), CodPedido.val())
                 
                 
                 */
               // enviarDato(Pago.val(), CodPedido.val())

                ////////











                var total = 0;
                var sHTML = '';
                $("#tableBodyPagos").html(sHTML);
                if (listaDetallePago.length > 0) {
                    $.each(listaDetallePago, function (i, item) {
                        var valorArgumentosFuncion = i + ",\'" + item.NumeroRecibo + "\',\'" + item.NumReferencia + "\'";
                        total += item.Valor;
                        sHTML += '<tr>';
                        sHTML += '     <td>' + item.DescMedioPago + '</td>';
                        sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? item.NumeroRecibo : item.NumeroRecibo) + '</td>';
                        sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.NumReferencia) + '</td>';
                        sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.DescFranquicia) + '</td>';
                        sHTML += '     <td align="right" id="Valor">' + item.Valor + '</td>';
                        //sHTML += '     <td align="center"><a onclick="evtEliminarMedioPagoo(' + valorArgumentosFuncion + ' );"><span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
                        //sHTML += '     <td align="right"><a data-id="' + i + '" class="evtEliminarMedioPago"><span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
                        sHTML += '     <td align="right"><a data-id="' + i + '" class="evtEliminarMedioPago"> <span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
                        sHTML += '</tr>';

                    });

                    //if (IdMedioPago == "2" && resultadoTransaccionRedebanExitosa == true) {
                    //    $.each(listaDetallePago, function (i, item) {
                    //        console.log(item.DescMedioPago);
                    //        total += item.Valor;
                    //        sHTML += '<tr>';
                    //        sHTML += '     <td>' + item.DescMedioPago + '</td>';
                    //        /*  sHTML += '     <td>' + NumeroRecibo.toString() + '</td>';*/
                    //        sHTML += '     <td>' + "000034" + '</td>';
                    //        sHTML += '     <td>' + NumeroRerenciaTarjeta.toString() + '</td>';
                    //        sHTML += '     <td>' + FranquiciaTarjeta.toString() + '</td>';
                    //        sHTML += '     <td align="right">' + item.Valor + '</td>';
                    //        sHTML += '     <td align="center"><a onclick="evtEliminarMedioPago(' + i + ');"><span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
                    //        sHTML += '</tr>';

                    //    });
                    //}
                    //else {
                    //    $.each(listaDetallePago, function (i, item) {
                    //        total += item.Valor;
                    //        sHTML += '<tr>';
                    //        sHTML += '     <td>' + item.DescMedioPago + '</td>';
                    //        sHTML += '     <td>' + "N/A" + '</td>';
                    //        sHTML += '     <td>' + item.NumReferencia + '</td>';
                    //        sHTML += '     <td>' + item.DescFranquicia + '</td>';
                    //        sHTML += '     <td align="right">' + item.Valor + '</td>';
                    //        sHTML += '     <td align="center"><a onclick="evtEliminarMedioPago(' + i + ');"><span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
                    //        sHTML += '</tr>';

                    //    });
                    //}
                }

                //rEGRESAR
                //$("#tableBodyPagos").append(sHTML);



                $("#Valor").mask("000.000.000", { reverse: true });
                //Deberia estar sin comentar
                //$("#SPago").val("");


                $("#selectFranquicia").val("");
                $("#inputReferencia").val("");
                //$("#inputValorPago").val("");

                selectMedioPagoChange();


                //calculaTotal();


            } else {

                if (($("#inputReferencia").val() == "" && IdMedioPago != "1")) {

                    if (transaccionRedebanHabilitadaa == "1" && IdMedioPago == "2") {
                    }
                    else {
                        $("#inputReferencia").attr("data-mensajeerror", "Este campo es obligatorio");
                        $("#inputReferencia").addClass("errorValidate");
                    }
                }
            }

            //finalizarProceso();

        }

        $("#inputValorPago").val("")
    
}

/*function evtEliminarMedioPagoo(indexMedioPago, numeroRecibo, numeroReferencia) {

    numeroFacturaEliminar = numeroReferencia;
    numeroReciboEliminar = numeroRecibo;
    indexMedioPagoEliminar = indexMedioPago;
    *//*$.ajax({
        url: ''
    })*//*
    if (listaDetallePago[indexMedioPago].IdMedioPago === 9) {
        $("#divNombreCliente").html("");
        $("#snapshot").attr("src", "");
        $("#snapshot").hide();
    }

    if (listaDetallePago[indexMedioPago].IdMedioPago === 2 && transaccionRedebanHabilitadaa == "1") {
        MostrarConfirm("Importante!", "¿Está seguro de eliminar el pago electrónico? ", "CorfimarSupervisorRedeban");
        return false;
    }


    if (listaDetallePago.length == 1)
        listaDetallePago = [];
    else {
        $.each(listaDetallePago, function (i) {
            if (i == indexMedioPago) {
                listaDetallePago.splice(i, 1);
                return false;
            }
        });
    }

    //calculaTotal();
    var total = 0;
    var sHTML = '';
    $("#tableBodyPagos").html(sHTML);
    if (listaDetallePago.length > 0) {
        $.each(listaDetallePago, function (i, item) {
            console.log("---------------- **** " + i + " -- " + item)
            var valorArgumentosFuncion = i + ",\'" + item.NumeroRecibo + "\',\'" + item.NumReferencia + "\'";
            total += item.Valor;
            sHTML += '<tr>';
            sHTML += '     <td>' + item.DescMedioPago + '</td>';
            sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? item.NumeroRecibo : item.NumeroRecibo) + '</td>';
            sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.NumReferencia) + '</td>';
            sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.DescFranquicia) + '</td>';
            sHTML += '     <td align="right" id="Valor">' + item.Valor + '</td>';
            //sHTML += '     <td align="center"><a onclick="evtEliminarMedioPago(' + valorArgumentosFuncion + '  );"><span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
            //sHTML += '     <td align="right"><a data-id="' + i + '" class="evtEliminarMedioPago"><span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
            sHTML += '     <td align="right"><a data-id="' + i + '" class="evtEliminarMedioPago"><span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
            sHTML += '</tr>';

        });
        $("#Valor").mask("000.000.000", { reverse: true });
    }
    $("#tableBodyPagos").append(sHTML);

    indexMedioPagoEliminar = 0;

}
*/

function calculaTotal() {
    var total = 0;

    console.log(datoEnviarPagar)
    console.log(listaDetallePago)
    
    
    if (listaDetallePago.length > 0) {
      /*  if (Seanulo == 1) {
            console.log(listaDetallePago)
            console.log(total + " ... Se anulo ...")
            listaDetallePago = []
            Seanulo = 0;
            total = 0;
        } else if (Seanulo == 2) {
            $.each(listaDetallePago, function (i) {
                if (i == indexMedioPagoEliminar) {
                    listaDetallePago.splice(i, 1);
                    datoEnviarPagar = 0;
                    console.log(listaDetallePago)
                    console.log(total + " ... Se anulo ...")
                    Seanulo = 0;
                    total = 0;
                    return false;
                }
            });
        }*/
        $.each(listaDetallePago, function (i, item) {
            total += item.Valor;
            console.log(listaDetallePago)
            console.log(total)
        });
    }
    console.log(datoEnviarPagar)
    console.log(listaDetallePago)
    console.log(total + " ++++++ ")
    console.log("$" + EnMascarar(total))
    if (nuevoCliente.value != "") {
        $("#TotalPagado").html("$" + total);
        console.log(total + " ++++++ $$$$$ ")
    } else {
        $("#TotalPagado").html("$" + EnMascarar(total));
    }
    MostrarcambioApp();
}


//Esto se debe descomentar 
function MostrarcambioApp() {
    var sum = 0;
    var total = ObtenerTotal();
    var cambio = 0;

    console.log(listaDetallePago)
    console.log(total)

    ////

    if (Seanulo == 1) {
        console.log(listaDetallePago)
        console.log(total + " ... Se anulo ...")
        listaDetallePago = []
        
        total = 0;
        sum = 0;
        
        //Valor a descomentar
        Seanulo = 0;
        console.log(ValorCostoComparar + " ---- " + sum)
        ValorCostoComparar = 0;
        
    } else if (Seanulo == 2) {
        $.each(listaDetallePago, function (i) {
            if (i == indexMedioPagoEliminar) {
                listaDetallePago.splice(i, 1);
                datoEnviarPagar = 0;
                console.log(listaDetallePago)
                console.log(total + " ... Se anulo ...")
                Seanulo = 0;
                total = 0;
                
                sum = 0;
                return false;
            }
        });
        //Valor a descomentar
        Seanulo = 0;
        console.log(ValorCostoComparar + " ---- " + sum)
        ValorCostoComparar = 0;
    }

    ////
    if (listaDetallePago.length > 0) {
        $.each(listaDetallePago, function (i, item) {
            sum += item.Valor;
            //datoEnviarPagar += item.Valor;
            pintarTotal = sum
        });
    }

    if (sum >= total) {
        //cambio = parseInt(sum - total);
        console.log(ValorCostoComparar)
        
        cambio = sum - ValorCostoComparar;
        console.log(sum + "  ---- " + total + " QQQñññ " + cambio + " ......  " + ValorCostoComparar)
        console.log(sum + "  ---- " + total + " sssssQQQ " + (sum - ValorCostoComparar) + "ffff")
        if (clienteNoExiste != 1)
        {
            if (pedidoNoExiste != 1) {
                if (cambio < 0) {
                    console.log(sum + "  ---- " + total + " QQQaaaaa ")
                    $("#cambio").html("<p style='color:red'> Cambio: $" + EnMascarar(cambio) + "</p>");
                    $("#total").html("<p>Total abonado<h1 style='font-weight: bold;'> $" + EnMascarar(sum) + "</h1></p>");
                } else {
                    console.log(sum + "  ---- " + total + " QQQvvvv ")
                    $("#cambio").html("Cambio: $" + EnMascarar(cambio));
                    $("#total").html("<p>Total abonado<h1 style='font-weight: bold;'> $" + EnMascarar(sum) + "</h1></p>");
                }
            } else {
                //$("#cambio").html("Cambio: $" + EnMascarar(0));
                console.log(sum + "  ---- " + total + " QQQwwww ")
                $("#cambio").html("Cambio: $" + EnMascarar(cambio));
               // $("#total").html("<p>Total abonado<h1 style='font-weight: bold;'> $" + EnMascarar(cambio) + "</h1></p>");
                //$("#total").html("<p>Total abonado<h1 style='font-weight: bold;'> $" + EnMascarar(ValorCostoComparar) + "</h1></p>");
                $("#total").html("<p>Total abonado<h1 style='font-weight: bold;'> $" + EnMascarar(sum) + "</h1></p>");
            }
        } else {
            if (pedidoNoExiste != 1) {
                if (cambio < 0) {

                    console.log(sum + "  ---- " + total + " QQQdddd ")
                    $("#cambio").html("<p style='color:red'> Cambio: $" + EnMascarar(cambio) + "</p>");
                    $("#total").html("<p>Total abonado<h1 style='font-weight: bold;'> $" + EnMascarar(sum) + "</h1></p>");
                } else {
                    $("#cambio").html("Cambio: $" + EnMascarar(0));
                    $("#total").html("<p>Total abonado<h1 style='font-weight: bold;'> $" + EnMascarar(sum) + "</h1></p>");
                }
            } else {
                //$("#cambio").html("Cambio: $" + EnMascarar(0));
                $("#cambio").html("Cambio: $" + EnMascarar(cambio));
                //$("#total").html("<p>Total abonado<h1 style='font-weight: bold;'> $" + EnMascarar(cambio) + "</h1></p>");
                //$("#total").html("<p>Total abonado<h1 style='font-weight: bold;'> $" + EnMascarar(ValorCostoComparar) + "</h1></p>");
                console.log(sum + "  ---- " + total + " QQQf ")

                $("#total").html("<p>Total abonado<h1 style='font-weight: bold;'> $" + EnMascarar(sum) + "</h1></p>");

            }
        }
        pintarTotal = sum;



        ///Volver a descomentar
        /*if (pedidoNoExiste == 1) {
            datoEnviarPagar = sum;
        }*/

        if (ii == 1)
        {
            $("#cambio").html("Cambio: $" + EnMascarar(0));
            $("#total").html("<p>Total abonado<h1 style='font-weight: bold;'> $" + EnMascarar(0) + "</h1></p>");
        }

    } else {
        console.log(sum + "  ---- " + total + " QQQ ")
        $("#cambio").html("<div style='color: red;'> Cambio: <b> - $ " + EnMascarar(parseInt(sum - total) * -1) + "</b></div>");
    }
}


function ObtenerTotal() {

    var total = 0;
    var impuestoTotal = 0;
    //borrar
    var sum = 0;
    //

    console.log(listaDetallePago)
    console.log(total)

    $.each(lstProductosCompra, function (i, item) {
        var precioItem = parseInt(item.PrecioTotal == "" ? 0 : item.PrecioTotal);
        //

        console.log(listaDetallePago)
        console.log(total)

        sum += item.Valor;
        //
        total += precioItem;
        var Base = Math.round((100 * precioItem) / (100 + item.PorcentajeImpuesto));
        impuestoTotal = impuestoTotal + (precioItem - Base);
    });

    //Propina
    if ($("#chxPropina").is(":checked")) {
        if ($("#txtPropina").val().length > 0) {
            total = parseInt(total) + parseInt($("#txtPropina").val().replace(".", "").replace(".", ""));
        }
    }
    //fin propina

    //$("#total").html("<h1 style='font-weight: bold;'> $" + EnMascarar(total) + "</h1>");

    $("#TotalImpuestos").html(EnMascarar(impuestoTotal));
    //$("#TotalBase").html(EnMascarar(total - impuestoTotal));
    return parseInt(total);
}

function EnMascarar(valor) {
    var val = $.trim(valor);
    var fn = val;
    console.log(val)
    console.log(valor)
    console.log(fn)
    if (val.length > 3)
        fn = val.substring(0, val.length - 3) + "." + val.substring(val.length - 3, val.length);
        console.log(fn)
    if (val.length > 6)
        fn = fn.substring(0, fn.length - 7) + "." + fn.substring(fn.length - 7, fn.length);
        console.log(fn)
   
    return fn;
}

const enviarDato = (pago, CodPedido) => {
    var total = 0;
    var sHTML = '';
    var ProductoSeleccionado = [{}]
    $("#tableBodyPagos").html("");


    ///DEVOLVER PARA NO DAÑAR
    ///listaDetallePago = []
    //



    $.ajax({
        url: `${ruta}/verDiferencias`,
        method: "POST",
        data: { dato: pago, Pedido: CodPedido },
        success: (e) => {
            console.log(e)
            var obj = new Object();
            obj.DescMedioPago = $("#selectMedioPago option:selected").text();

            da = e.diferencia
            

            if (listaDetallePago.length > 0) {
                $.each(listaDetallePago, function (i, item) {
                    console.log("---------------- **** " + i + " -- " + item + "ver pagoooo")
                    var valorArgumentosFuncion = i + ",\'" + item.NumeroRecibo + "\',\'" + item.NumReferencia + "\'";
                    // ---> var valorArgumentosFuncion = i + ",\'" + item.NumeroRecibo + "\',\'" + item.NumeroFactura + "\'";
                    //total += item.Valor;
                    total += item.pagoCli;
                    sHTML += '<tr>';
                    sHTML += '     <td>' + item.DescMedioPago + '</td>';
                    sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? item.NumeroRecibo : item.NumeroRecibo) + '</td>';
                    //sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.numeroRerenciaTarjeta) + '</td>';
                    sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? item.NumReferencia : item.NumReferencia) + '</td>';
                    sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.DescFranquicia) + '</td>';
                    //sHTML += '     <td align="right" id="Valor">' + total + '</td>';
                    //sHTML += '     <td align="right" id="Valor">' + item.pagoCli + '</td>';
                    sHTML += '     <td align="right" id="Valor">' + item.Valor + '</td>';
                    //sHTML += '     <td align="right" id="Valor">' + item.pagoCli + '</td>';
                    //sHTML += '     <td align="right"><a onclick="evtEliminarMedioPago(' + valorArgumentosFuncion + '  );">f<span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
                    sHTML += '     <td align="right"><a onclick="evtEliminarMedioPagoo(' + valorArgumentosFuncion + '  );"><i class="fa fa-trash-o IconosPos" aria-hidden="true"></i></a></td>';
                    sHTML += '</tr>';
                });
            }


            /*var sHTML = '';
            $("#tableBodyPagos").html(sHTML);
            if (listaDetallePago.length > 0) {
                $.each(listaDetallePago, function (i, item) {
                    console.log("---------------- **** " + i + " -- " + item)
                    var valorArgumentosFuncion = i + ",\'" + item.NumeroRecibo + "\',\'" + item.NumReferencia + "\'";
                    total += item.Valor;
                    sHTML += '<tr>';
                    sHTML += '     <td>' + item.DescMedioPago + '</td>';
                    sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.NumeroRecibo) + '</td>';
                    sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.NumReferencia) + '</td>';
                    sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.DescFranquicia) + '</td>';
                    sHTML += '     <td align="right">' + item.Valor + '</td>';
                    sHTML += '     <td align="center"><a onclick="evtEliminarMedioPagoo(' + valorArgumentosFuncion + '  );"><span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
                    sHTML += '</tr>';

                });
            }*/

            $("#tableBodyPagos").append(sHTML);



            //
            calculaTotal();
            //
            $("#cambioo").mask("000.000.000", { reverse: true });
            $("#TotalPagadoo").mask("000.000.000", { reverse: true });
            $("#Totall").mask("000.000.000", { reverse: true });

            ingFunction++;

            //$("#tableBodyPagos").append(sHTML);


            $("#Valor").mask("000.000.000", { reverse: true });

            ///fffffff

        },
        error: (e) => {
            console.log(e)
        }
    })
    //
    //
    return { listaDetallePago, ProductoSeleccionado }
}

const enviarDatoo = (pago) => {
    var total = 0;
    var sHTML = '';
    var ProductoSeleccionado = [{}]
    $("#tableBodyPagos").html("");


    var pagar = $("#IdApagar");
    //var sin puntos 
    var sinPuntosDinero = pagar.val().replace(".", "").replace(".", "");
    console.log(sinPuntosDinero)
    ///DEVOLVER PARA NO DAÑAR
    ///listaDetallePago = []
    //

    

    ValorCostoComparar = pagar.val().replace(".","").replace(".","")

    ///


    datoEnviarPagar = pagar.val().replace(".","").replace(".","")
    console.log(datoEnviarPagar + " $$$$ " + pagar.val())
    $.ajax({
        url: `${ruta}/verDiferenciass`,
        method: "POST",
        //data: { dato: pago, Pedido: pagar.val()},
        data: { dato: pago, Pedido: sinPuntosDinero},
        success: (e) => {
            console.log(e)
            var obj = new Object();
            obj.DescMedioPago = $("#selectMedioPago option:selected").text();

            da = e.diferencia


            if (listaDetallePago.length > 0) {
                $.each(listaDetallePago, function (i, item) {
                    console.log("---------------- **** " + i + " -- " + item + "ver pagoooo")
                    var valorArgumentosFuncion = i + ",\'" + item.NumeroRecibo + "\',\'" + item.NumReferencia + "\'";
                    // ---> var valorArgumentosFuncion = i + ",\'" + item.NumeroRecibo + "\',\'" + item.NumeroFactura + "\'";
                    //total += item.Valor;
                    total += item.pagoCli;
                    sHTML += '<tr>';
                    sHTML += '     <td>' + item.DescMedioPago + '</td>';
                    sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? item.NumeroRecibo : item.NumeroRecibo) + '</td>';
                    //sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.numeroRerenciaTarjeta) + '</td>';
                    sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? item.NumReferencia : item.NumReferencia) + '</td>';
                    sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.DescFranquicia) + '</td>';
                    //sHTML += '     <td align="right" id="Valor">' + total + '</td>';
                    //sHTML += '     <td align="right" id="Valor">' + item.pagoCli + '</td>';
                    sHTML += '     <td align="right" id="Valor">' + item.Valor + '</td>';
                    //sHTML += '     <td align="right" id="Valor">' + item.pagoCli + '</td>';
                    //sHTML += '     <td align="right"><a onclick="evtEliminarMedioPago(' + valorArgumentosFuncion + '  );">f<span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
                    sHTML += '     <td align="right"><a onclick="evtEliminarMedioPagoo(' + valorArgumentosFuncion + '  );"><i class="fa fa-trash-o IconosPos" aria-hidden="true"></i></a></td>';
                    sHTML += '</tr>';
                });
            }


            /*var sHTML = '';
            $("#tableBodyPagos").html(sHTML);
            if (listaDetallePago.length > 0) {
                $.each(listaDetallePago, function (i, item) {
                    console.log("---------------- **** " + i + " -- " + item)
                    var valorArgumentosFuncion = i + ",\'" + item.NumeroRecibo + "\',\'" + item.NumReferencia + "\'";
                    total += item.Valor;
                    sHTML += '<tr>';
                    sHTML += '     <td>' + item.DescMedioPago + '</td>';
                    sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.NumeroRecibo) + '</td>';
                    sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.NumReferencia) + '</td>';
                    sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.DescFranquicia) + '</td>';
                    sHTML += '     <td align="right">' + item.Valor + '</td>';
                    sHTML += '     <td align="center"><a onclick="evtEliminarMedioPagoo(' + valorArgumentosFuncion + '  );"><span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
                    sHTML += '</tr>';

                });
            }*/

            $("#tableBodyPagos").append(sHTML);



            //
            calculaTotal();
            //
            $("#cambioo").mask("000.000.000", { reverse: true });
            $("#TotalPagadoo").mask("000.000.000", { reverse: true });
            $("#Totall").mask("000.000.000", { reverse: true });

            ingFunction++;

            //$("#tableBodyPagos").append(sHTML);


            $("#Valor").mask("000.000.000", { reverse: true });

            ///fffffff

        },
        error: (e) => {
            console.log(e)
        }
    })
    //
    //
    return { listaDetallePago, ProductoSeleccionado }
}


function evtEliminarMedioPagoo(indexMedioPago, numeroRecibo, numeroReferencia) {

    numeroFacturaEliminar = numeroReferencia;
    numeroReciboEliminar = numeroRecibo;
    indexMedioPagoEliminar = indexMedioPago;
    console.log("Eliminando -> " + numeroFacturaEliminar + " ** " + numeroReciboEliminar + " ### " + indexMedioPagoEliminar)
  /* $.ajax({
        url: `${ruta}/reiniciarDat`,
        method: 'GET',
        success: () => {
            console.log("Se reiniciaron los datos completamente")
            //listaDetallePago = [];
        },
        error: () => {
            console.log("No Se reiniciaron los datos completamente")
        }
    })*/

    if (listaDetallePago[indexMedioPago].IdMedioPago === 9) {
        $("#divNombreCliente").html("");
        $("#snapshot").attr("src", "");
        $("#snapshot").hide();
    }
    console.log(listaDetallePago + " -----------------> " + " paso ")
    console.log(listaDetallePago )
    
    if (listaDetallePago[indexMedioPago].IdMedioPago === 2 && transaccionRedebanHabilitadaa == "0") {
        MostrarConfirm("Importante!", "¿Está seguro de eliminar el pago electrónico? ", "CorfimarSupervisorRedeban");
        return false;
    }


    if (listaDetallePago.length == 1)
        listaDetallePago = [];
    else {
        $.each(listaDetallePago, function (i) {
            if (i == indexMedioPago) {
                listaDetallePago.splice(i, 1);
                console.log("Eliminando -> " +  + " ** " + numeroReciboEliminar + " ### :::::::: " + indexMedioPagoEliminar)
                console.log("Eliminando -> " + listaDetallePago[0])
                console.log(listaDetallePago[0])
                return false;
            }
        });
    }
   // ii = 1;
    calculaTotal();

    ii = 0;
    var sHTML = '';
    $("#tableBodyPagos").html(sHTML);
   /* if (listaDetallePago.length > 0) {
        $.each(listaDetallePago, function (i, item) {
            console.log("---------------- **** " + i + " -- " + item)
            var valorArgumentosFuncion = i + ",\'" + item.NumeroRecibo + "\',\'" + item.NumReferencia + "\'";
            total += item.Valor;
            sHTML += '<tr>';
            sHTML += '     <td>' + item.DescMedioPago + '</td>';
            sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.NumeroRecibo) + '</td>';
            sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.NumReferencia) + '</td>';
            sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.DescFranquicia) + '</td>';
            sHTML += '     <td align="right">' + item.Valor + '</td>';
            sHTML += '     <td align="center"><a onclick="evtEliminarMedioPago(' + valorArgumentosFuncion + '  );"><span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
            sHTML += '</tr>';

        });
    }*/

    //
    var sHTML = '';
    $("#tableBodyPagos").html(sHTML);
    if (listaDetallePago.length > 0) {
        $.each(listaDetallePago, function (i, item) {
            console.log("---------------- **** " + i + " -- " + item + "ver pagoooo")
            var valorArgumentosFuncion = i + ",\'" + item.NumeroRecibo + "\',\'" + item.NumReferencia + "\'";
            // ---> var valorArgumentosFuncion = i + ",\'" + item.NumeroRecibo + "\',\'" + item.NumeroFactura + "\'";
            //total += item.Valor;
            total += item.pagoCli;
            sHTML += '<tr>';
            sHTML += '     <td>' + item.DescMedioPago + '</td>';
            sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? item.NumeroRecibo : item.NumeroRecibo) + '</td>';
            //sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.numeroRerenciaTarjeta) + '</td>';
            sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? item.NumReferencia : item.NumReferencia) + '</td>';
            sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.DescFranquicia) + '</td>';
            //sHTML += '     <td align="right" id="Valor">' + total + '</td>';
            //sHTML += '     <td align="right" id="Valor">' + item.pagoCli + '</td>';
            sHTML += '     <td align="right" id="Valor">' + item.Valor + '</td>';
            //sHTML += '     <td align="right" id="Valor">' + item.pagoCli + '</td>';
            //sHTML += '     <td align="right"><a onclick="evtEliminarMedioPago(' + valorArgumentosFuncion + '  );">f<span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
            sHTML += '     <td align="right"><a onclick="evtEliminarMedioPagoo(' + valorArgumentosFuncion + '  );"><i class="fa fa-trash-o IconosPos" aria-hidden="true"></i></a></td>';
            sHTML += '</tr>';
        });
    }

    //
    $("#tableBodyPagos").html(sHTML);
    indexMedioPagoEliminar = 0;
    //CodPedido.val("")

}

function CorfimarSupervisorRedeban() {
    $('#myModalConfirmarSupervisorRedeban').modal('show');
}

///////

$(function () {

    $("#btnCancelarLoginRedeban").click(function () {
        $("#txt_DocumentoEmpleado").val("");
        $("#Password").val("");
        $('#divPassword').hide();
        cerrarModal("myModalConfirmarSupervisorRedeban");
    });

    $("#btnAceptarLoginRedeban").click(function () {

        if ($("#txt_DocumentoEmpleado").val().length <= 0 || $("#Password").val() <= 0) {
            MostrarMensaje("Importante", "Debe digitar usuario y contraseña", "error");
        } else {
            EjecutarAjax(urlBase + "Redeban/LoginSupervisor", "POST", JSON.stringify({ user: $("#txt_DocumentoEmpleado").val(), pwd: $("#Password").val() }), "SuccessLogin", null);

        }
    });

    $("#btnLimpiarRedeban").click(function () {

        $("#txt_DocumentoEmpleado").val("");
        $("#Password").val("");
        $('#divPassword').hide();

    });

    $("#txt_DocumentoEmpleado").focus();

    $(this).click(function () {
        setearfocus();
    });
    $(this).keydown(function () {
        setearfocus();
    });

    $("#txt_DocumentoEmpleado").keyup(function () {
        if (!inicializadointerval) {
            inicializadointerval = true;
            var refreshIntervalId = setInterval(function () { EjecutarLogin(); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);

        }
    });

    $('#txt_DocumentoEmpleado').focusout(function () {
        if (($('#myModalConfirmarSupervisorRedeban').is(':visible'))) {
            setTimeout(function () { $('#txt_DocumentoEmpleado').focus() }, 500);
        }
    });


    function EjecutarLogin() {

        if (($('#myModalConfirmarSupervisorRedeban').is(':visible'))) {
            $('#divPassword').show();
            $("#Password").focus();
        }

    }


    function setearfocus() {
        if (($('#myModalConfirmarSupervisorRedeban').is(':visible')) && $("#txt_DocumentoEmpleado").val().length < 13) {
            $("#txt_DocumentoEmpleado").focus();
        } else {
            if (($('#myModalConfirmarSupervisorRedeban').is(':visible'))) {
                $('#divPassword').show();
                $("#Password").focus();
            }
        }
    }

});


function SuccessLogin(rta) {

    if (rta.Correcto) {
        $("#IdUsuarioSupervisor").val(rta.Elemento.Id);
        IniciarTransaccionAnulacion();
    } else {
        $("#txt_DocumentoEmpleado").val("");
        $("#Password").val("");
        $('#divPassword').hide();
        MostrarMensaje("Importante", rta.Mensaje, "error");
    }

}


//
function IniciarTransaccionAnulacion() {
    urlaction = urlBase + "Redeban/IniciarAnulacionRedeban";
    $.ajax({
        url: urlaction,
        ContentType: 'application/json; charset-uft8',
        dataType: 'JSON',
        type: 'GET',
        data: {
            numeroRecibo: numeroReciboEliminar,
            numeroFactura: numeroFacturaEliminar
        },
        async: false,
        success: function (respuesta) {
            if (respuesta != null) {
                if (respuesta.MensajeRespuesta != "OK" && respuesta.CodigoRespuesta != "00") {
                    cerrarModal("myModalConfirmarSupervisorRedeban");
                    MostrarMensajeHTML("Importante", respuesta.MensajeRespuesta.toString(), "error");
                    
                    return false;
                }
                else {
                    if (listaDetallePago.length == 1) {
                        listaDetallePago = [];
                        datoEnviarPagar = 0;
                        console.log(listaDetallePago)
                        console.log(respuesta)
                        console.log(ValorCostoComparar + " ******  " + respuesta.Monto + " >>>> " + respuesta.Monto)
                        //ValorCostoComparar = respuesta.Monto - ValorCostoComparar;
                        console.log(ValorCostoComparar + " ******  " + respuesta.Monto)
                        //ValorCostoComparar = 0;
                        Seanulo = 1;
                        //pedidoNoExiste = 1;
                    } else {
                        console.log(listaDetallePago)
                        $.each(listaDetallePago, function (i) {
                            if (i == indexMedioPagoEliminar) {
                                listaDetallePago.splice(i, 1);
                                datoEnviarPagar = 0;
                                console.log(listaDetallePago)
                                //pedidoNoExiste = 1; Valor
                                //ValorCostoComparar = 0
                                console.log(respuesta)
                                console.log(ValorCostoComparar + " ******  " + respuesta.Monto + " >>>> " + respuesta.Monto)
                                //ValorCostoComparar = respuesta.Monto - ValorCostoComparar;
                                console.log(ValorCostoComparar + " ******  " + respuesta.Monto)
                                
                                Seanulo = 2;
                                return false;
                            }
                        });
                    }

                    //lol
                    console.log(ValorCostoComparar + " ---- " )
                    console.log(listaDetallePago +  " verificar y ver el dato")
                    calculaTotal();

                    var sHTML = '';
                    $("#tableBodyPagos").html(sHTML);
                    if (listaDetallePago.length > 0) {
                        $.each(listaDetallePago, function (i, item) {
                            console.log("---------------- **** " + i + " -- " + item )
                            var valorArgumentosFuncion = i + ",\'" + item.NumeroRecibo + "\',\'" + item.NumReferencia + "\'";
                            total += item.Valor;
                            sHTML += '<tr>';
                            sHTML += '     <td>' + item.DescMedioPago + '</td>';
                            sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.NumeroRecibo) + '</td>';
                            sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.NumReferencia) + '</td>';
                            sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" ? "N/A" : item.DescFranquicia) + '</td>';
                            sHTML += '     <td align="right">' + item.Valor + '</td>';
                            sHTML += '     <td align="center"><a onclick="evtEliminarMedioPagoo(' + valorArgumentosFuncion + '  );"><span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
                            sHTML += '</tr>';

                        });
                    }
                    $("#tableBodyPagos").append(sHTML);

                    indexMedioPagoEliminar = 0;
                    resultadoTransaccionRedebanExitosa = true;
                    cerrarModal("myModalConfirmarSupervisorRedeban");
                    MostrarMensajeHTML("Importante", "Transacción eliminada con exito. <br /> Conexión Aprobada", "success");
                }

            }
        },
        error: function (error) {
            MostrarMensajeHTML("Importante", "Error conectando con Redeban. <br /> Por favor intente nuevamente", "error");
            console.log(error)
            return false;
        }
    });
}

///

function selectMedioPagoChange() {
    var IdMedioPago = $("#selectMedioPago").val();

    if (IdMedioPago == parametros.IdMedioPagoEfectivo || IdMedioPago == "") {
        $("#colReferencia").fadeOut("fast");
    } else {
        $("#colReferencia").fadeIn("fast");
    }

    if (IdMedioPago == parametros.IdMedioPagoTarjetaDebito || IdMedioPago == parametros.IdMedioPagoTarjetaCredito) {
        $("#colFranquicia").fadeIn("fast");
    } else {
        $("#colFranquicia").fadeOut("fast");
    }

    if ((IdMedioPago == parametros.IdMedioPagoTarjetaDebito || IdMedioPago == parametros.IdMedioPagoTarjetaCredito)) {
        if (transaccionRedebanHabilitadaa == "0") {
            $("#inputReferencia").fadeIn("fast");
            $("#NumRelacionado").fadeIn("fast");
            $("#colFranquicia").fadeIn("fast");
        }
        else {
            $("#inputReferencia").fadeOut("fast");
            $("#NumRelacionado").fadeOut("fast");
            $("#colFranquicia").fadeOut("fast");
        }

    } else {
        $("#colFranquicia").fadeOut("fast");
    }/**/


    if (IdMedioPago == parametros.IdMedioPagoTarjetaRecargable) {
        $("#inputValorPago").val($("#total h1").html().replace("$", ""));
    }
    else {
        // $("#inputValorPago").val("");
    }
    $("#selectFranquicia").val("");
    $("#inputReferencia").val("");
}

//


////

function ValidarBonoRegalo(codigo, valor) {

    var result = true;
    $.ajax({
        url: urlBase + "Pos/ConsultarBonoRegalo",
        type: 'GET',
        data: { codigo: codigo },
        ContentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (rta) {
            if (!rta.respuesta) {
                $("#inputReferencia").val("");
                MostrarMensaje("Importante", "El bono regalo no esta disponible!", "warning");
                result = false;
            } else {
                var _ttal = CalcularTotalBonoRegalo($("#inputReferencia").val()) + valor;
                if (_ttal > parseInt(rta.objeto.Saldo)) {
                    $("#inputValorPago").attr("data-mensajeerror", "El valor es superior a " + EnMascarar(rta.objeto.Saldo));
                    $("#inputValorPago").addClass("errorValidate");
                    mostrarTooltip();
                    result = false;
                }
            }
        }
    });

    return result;
}

function ValidarCupoEmpleado(Valor, Consecutivo) {
    var result = "";
    $.ajax({
        url: urlBase + "Pos/ObtenerEmpleadoPorConsecutivo",
        type: 'GET',
        data: { Consecutivo: Consecutivo },
        ContentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (rta) {
            if (rta.IdEstructuraEmpleado > 0) {
                if (parseInt(rta.CupoRestante) >= parseInt(Valor.trim())) {
                    empleadoNominaSeleccionado = rta;
                    result = rta.Documento;

                } else {
                    empleadoNominaSeleccionado = new Object();
                    $("#inputReferencia").val("");
                    $("#inputReferencia").removeAttr("disabled");
                    iniIntervalPagoNomina = false;
                    MostrarMensajeRedireccion("Importante", "El registro no tiene cupo", null, "warning");
                }
            } else {
                empleadoNominaSeleccionado = new Object();
                $("#inputReferencia").val("");
                $("#inputReferencia").removeAttr("disabled");
                MostrarMensajeRedireccion("Importante", "Registro Invalido", null, "warning");
            }
        }
    });

    return result;
}

$("#precio").mask("000.000.000");
$("#inputValorPago").mask("000.000.000", {reverse : true});


