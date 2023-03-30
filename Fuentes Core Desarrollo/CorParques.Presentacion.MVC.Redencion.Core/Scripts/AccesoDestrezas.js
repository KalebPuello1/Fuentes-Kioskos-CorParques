//Powered by RDSH
//Acceso a destrezas.
var inicializadointerval = false;
var LecturaCuposDebito = []; //Aqui se va a almacenar la información de los cupos debito para hacer la acumulacion de los mismos.
var ContadorNumeroDeAccesosDia = 0;

$(function () {
    $("#div_message_error").hide();
    Inicializar();

});

function Inicializar() {

    if (ValidacionOperativaPunto()) {

        InhabilitarCopiarPegarCortar("txt_CodigoBarras");
        $("#txt_CodigoBarras").keyup(function () {
            if (!inicializadointerval) {
                inicializadointerval = true;
                var refreshIntervalId = setInterval(function () { ValidarCodigoDeBarras(); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);
            }
        });

        $('#txt_CodigoBarras').focusout(function () {
            setTimeout(function () { $('#txt_CodigoBarras').focus() }, 500);
        });

        ContadorNumeroDeAccesosDia = parseInt($("#hdf_NumeroAccesosDia").val());
        EstablecerAccesosDia(ContadorNumeroDeAccesosDia);
    }

}

function ValidacionOperativaPunto() {

    var Mensaje = "";
    var blnResultado = true;

    Mensaje = $("#hdf_ValidacionOperativaPunto").val();

    if (Mensaje !== '') {
        if (Mensaje.indexOf("[H]") >= 0) {
            blnResultado = false;
            MostrarMensajeRedireccion("Importante", Mensaje.replace("[H]", ""), "Home/Index", "error");
        //} else {
        //    blnResultado = false;
        //    MostrarMensajeRedireccion("Importante", Mensaje, "AuxiliarPunto/Index", "error");
        } else if (Mensaje.indexOf("[O]") >= 0) {
            MostrarMensaje("Punto listo para operar", Mensaje.replace("[O]", ""), "info");
        }
    }

    return blnResultado;

}

///Valida si el codigo de barras tiene acceso a la Destrezas.
function ValidarCodigoDeBarras() {
    var strCodigoBarras = "";
    var lngValorAcumulado = 0;
    var lngValorConvenio = 0;

    strCodigoBarras = $("#txt_CodigoBarras").val();
    if (strCodigoBarras != '') {
        lngValorAcumulado = ObtenerAcumuladoCupoDebito(strCodigoBarras);
        lngValorConvenio = ObtenerAcumuladoConvenio(strCodigoBarras);
        if (ValidarSiExisteCupoDebito(strCodigoBarras)) {
            if (lngValorAcumulado > 0) {
                lngValorAcumulado = ObtenerAcumuladoCupoDebitoTransferencia();
                //Si existe ese cupo debito, quiere decir que hay que transferir el total de los cupos debito leidos al cupo debito que fue leido dos veces.
                EjecutarAjax(urlBase + "Destrezas/TransferirSaldo", "GET", { CodigoACargar: strCodigoBarras, CodigoDeBarrasDescargar: RetornarCodigosDeBarraDescargar(strCodigoBarras), ValorAcumulado: lngValorAcumulado }, "RespuestaTranferencia", null);
            }
            LimpiarCuposDebito();
        } else {
            if (!AnularIngresoCupoDebito(strCodigoBarras))
            {
                EjecutarAjax(urlBase + "Destrezas/ValidarAccesoDestreza", "GET", { CodigoDeBarras: strCodigoBarras, ValorAcumulado: lngValorAcumulado, ValorAcumuladoConvenio: lngValorConvenio, CodigoDeBarrasDescargar: RetornarCodigosDeBarraDescargar(strCodigoBarras) }, "RespuestaValidacion", null);
            }            
        }
    } else {
        //Limpia cupo debito.
        LimpiarCuposDebito();
    }
    $("#txt_CodigoBarras").val("");
}

//Respuesta que se da al validar el acceso del codigo de barras.
function RespuestaValidacion(data) {

    var blnValido = data.Valido;
    var strCodigoMensaje = data.CodigoMensaje;
    var strMensajeAcceso = data.MensajeAcceso;
    $("#snapshot").hide()
    if (data.fotoTexto != "") {
        $("#snapshot").attr("src", data.fotoTexto);
        $("#snapshot").show()
    }
    $("#div_Mensaje").html("");
    if (blnValido) {
        //CON ACCESO
        $("#div_NoAcceso").hide();
        $("#div_Acceso").show();
        $("#div_Mensaje").show();
        if (strCodigoMensaje == 'BR5') {
            //Brazalete tiene acceso. 
            if (data.MensajeAcceso !== '')
            {
                $("#div_Mensaje").html(data.MensajeAcceso + ' : ' + data.IngresosDisponibles);
            }            
            //Limpia cupo debito.
            LimpiarCuposDebito();
        }
        else {

            //Si no es brazalete es cupo debito.            
            $("#div_Mensaje").html(data.MensajeAcceso + ' : ' + FormatoMoneda(data.SaldoActual));
            //Limpia cupo debito.
            LimpiarCuposDebito();
        }
        //Aumenta la cantidad de ingresos exitosos del dia.
        ContadorNumeroDeAccesosDia = ContadorNumeroDeAccesosDia + 1;
        EstablecerAccesosDia(ContadorNumeroDeAccesosDia);
    }
    else {
        //SIN ACCESO
        $("#div_Acceso").hide();
        $("#div_NoAcceso").show();
        $("#div_Mensaje").show();
        if (strCodigoMensaje == 'CB0') {
            //El codigo de barras no existe.
            $("#div_Mensaje").html(data.MensajeAcceso);
            //Limpia cupo debito.
            LimpiarCuposDebito();
        }
        else {
            if (strCodigoMensaje == 'CD1') {
                //Es cupo debito. No ingresa por que la boleta no es del dia.
                $("#div_Mensaje").html(data.MensajeAcceso);
            } else if (strCodigoMensaje == 'CD2') {
                //Es cupo debito. No ingresa por saldo insifuciente.
                $("#div_Mensaje").html(data.MensajeAcceso + '<br  /> ' + 'Saldo Actual: ' + FormatoMoneda(parseFloat(data.SaldoActual) + parseFloat(data.ValorAcumulado)) + ' <br / > ' + 'Recargue: ' + FormatoMoneda(data.ValorRecarga));

                //Agrega el cupo debito al array de cupos debito leidos, para que pueda ser acumulativo.
                AgregarCupoDebito(data.CodigodeBarras, data.SaldoActual, data.ValorDescuentoConvenio);
            } else {
                //Es brazalete
                //Si es brazalete y no tiene acceso por el numero de accesos, se debe mostrar numero accesos en cero.
                if (strCodigoMensaje == 'BR4') {
                    $("#div_Mensaje").html(data.MensajeAcceso + ' <br /> ' + 'Ingresos disponibles: 0');
                } else {
                    $("#div_Mensaje").html(data.MensajeAcceso);
                }
                //Limpia cupo debito.
                LimpiarCuposDebito();
            }

        }
    }
}

//RDSH: Mustra el nuevo saldo del cupo debito.
function RespuestaTranferencia(data) {
    $("#div_Acceso").hide();
    $("#div_NoAcceso").hide();
    $("#div_Mensaje").show();

    $("#div_Mensaje").html('Saldo disponible: ' + FormatoMoneda(data.SaldoActual));

}


///Se agreagan las boletas de cupo debito que no tienen saldo.
function AgregarCupoDebito(strCodigoBarras, lngSaldo, lngValorConvenio) {
    var objCupoDebito = { CodigoBarras: strCodigoBarras, Valor: lngSaldo, ValorConvenio: lngValorConvenio }
    var result = $.grep(LecturaCuposDebito, function (e) { return e.CodigoBarras == strCodigoBarras; });

    if (result.length == 0) {
        LecturaCuposDebito.push(objCupoDebito);
    }

}

///Retorna el valor acumulado de los cupo debito que se han leido.
function ObtenerAcumuladoCupoDebito(strCodigoBarras) {

    var lngValorAcumulado = 0;

    if (LecturaCuposDebito != undefined) {
        if (LecturaCuposDebito.length > 0) {
            $.each(LecturaCuposDebito, function (i) {
                if (LecturaCuposDebito[i].CodigoBarras != strCodigoBarras) {
                    if (parseFloat(LecturaCuposDebito[i].Valor) > 0) {
                        lngValorAcumulado = (parseFloat(lngValorAcumulado) + parseFloat(LecturaCuposDebito[i].Valor));
                    }
                }
            });
        }
    }

    return lngValorAcumulado;
}

///Retorna el valor acumulado de los convenios de los cupo debito que se han leido.
function ObtenerAcumuladoConvenio(strCodigoBarras) {

    var lngValorAcumulado = 0;

    if (LecturaCuposDebito != undefined) {
        if (LecturaCuposDebito.length > 0) {
            $.each(LecturaCuposDebito, function (i) {
                if (LecturaCuposDebito[i].CodigoBarras != strCodigoBarras) {
                    if (parseFloat(LecturaCuposDebito[i].ValorConvenio) > 0) {
                        lngValorAcumulado = (parseFloat(lngValorAcumulado) + parseFloat(LecturaCuposDebito[i].ValorConvenio));
                    }
                }
            });
        }
    }

    return lngValorAcumulado;
}

///Valida si se ha leido dos veces el cupo credito, si lo leyo dos veces debe dejarle el saldo al ultimo cupo credito que leyo dos veces.
function ValidarSiExisteCupoDebito(strCodigoBarras) {

    var Cantidad = LecturaCuposDebito.length;
    var result = $.grep(LecturaCuposDebito, function (e) { return e.CodigoBarras == strCodigoBarras; });
    var Resultado = false;

    if (result.length > 0 && Cantidad > 1) {
        Resultado = true;
    }

    return Resultado;

}

///Limpia el array de cupos credito.
function LimpiarCuposDebito() {
    LecturaCuposDebito.splice(0, LecturaCuposDebito.length);
}


///Retorna los codigos de barras separados con coma.
function RetornarCodigosDeBarraDescargar(strCodigoDeBarrasACargar) {

    var CodigosBarras = "";

    if (LecturaCuposDebito != undefined) {
        if (LecturaCuposDebito.length > 0) {
            $.each(LecturaCuposDebito, function (i) {
                if (strCodigoDeBarrasACargar !== LecturaCuposDebito[i].CodigoBarras) {
                    if (CodigosBarras == "") {
                        CodigosBarras = LecturaCuposDebito[i].CodigoBarras;
                    }
                    else {
                        CodigosBarras = CodigosBarras + ',' + LecturaCuposDebito[i].CodigoBarras;
                    }

                }
            });
        }
    }

    if (CodigosBarras == "") {
        CodigosBarras = "N";
    }

    return CodigosBarras;
}

//Establece el valor de los accesos a la atraccion durante el dia.
function EstablecerAccesosDia(NumeroAccesosDia) {
    $("#div_AccesosDia").html(NumeroAccesosDia);
}

//Retorna el acumulado para la transferencia de saldo.
function ObtenerAcumuladoCupoDebitoTransferencia() {

    var lngValorAcumulado = 0;

    if (LecturaCuposDebito != undefined) {
        if (LecturaCuposDebito.length > 0) {
            $.each(LecturaCuposDebito, function (i) {
                if (parseFloat(LecturaCuposDebito[i].Valor) > 0) {
                    lngValorAcumulado = (parseFloat(lngValorAcumulado) + parseFloat(LecturaCuposDebito[i].Valor));
                }
            });
        }
    }

    return lngValorAcumulado;
}

///Si solo han leido un cupo debito anula la transaccion si lo pasan dos veces.
function AnularIngresoCupoDebito(strCodigoBarras)
{
    var Cantidad = LecturaCuposDebito.length;
    var result = $.grep(LecturaCuposDebito, function (e) { return e.CodigoBarras == strCodigoBarras; });
    var Resultado = false;

    if (result.length > 0 && Cantidad == 1) {
        Resultado = true;
        $("#div_Acceso").hide();
        $("#div_NoAcceso").hide();
        $("#div_Mensaje").show();
        $("#div_Mensaje").html('Saldo disponible: ' + FormatoMoneda(result[0].Valor));
        //Limpia cupo debito.
        LimpiarCuposDebito();
    }

    return Resultado;

}