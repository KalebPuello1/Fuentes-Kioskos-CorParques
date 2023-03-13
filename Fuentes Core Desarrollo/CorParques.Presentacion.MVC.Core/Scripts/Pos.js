//Propiedades
var ListaTodosProductosSAP;
var ListaConveniosCodigoBarras;
var CodigoBarrasConvenioPistoleadedo = "";
var listDetalleConvenio = [];
var lista = [];
var listaDetallePago = [];
var optKey = false;
$("#CodBarra").prop("type", "password");
var optKeyRecarga = false;
$("#CodTarjeta").prop("type", "password");
var _tmpBanderaMesa = null;
var _tmpIdPedido = null;
var tmpLista;
var TipoPrincipal = 'Alimentos';
var lstProductosCompra = []; //Lista para mostrar los productos comprados.
var tmpListaAyD;
var tmpListaSouvenir;
var tmpListaServicios;
var tmpListaPasaporte;
var _opcionSeleccionada;
var index;
var TieneCodigoReservaParqueadero = false;
var inicializadointerval = false;
var inicializadointervalPark = false;
var inicializadointervalResPark = false;
var inicializadointervalConvenio = false;
var iniIntervalPagoNomina = false;
var empleadoNominaSeleccionado;
var parametros;
var productoPorAdd;
var transaccionRedebanHabilitada = document.getElementById("inpuntTransaccionRedebanHabilitada").value;
var resultadoTransaccionRedebanExitosa = false;
var inicializadointerval = false;
var numeroFacturaEliminar = "";
var numeroReciboEliminar = "";
var indexMedioPagoEliminar = 0;
var IdFranquiciaRedeban = 0;
var _tarjetafanclien = 0;
var numeroPasaportesDia = 0;
var tarjetaVencida = false;
var listProductosCortesia = [];
var usuarioCortesia = "";
var IdDetalleCortesia = 0;
var esMejoraCortesia = false;

//FIDELIZACION
Webcam.set({
    width: 160,
    height: 120,
    crop_width: 160,
    crop_height: 120,

    image_format: 'png',
    jpeg_quality: 50
});
Webcam.attach('#my_camera');
loadCalendar();


$("#btnCaptura").click(function () {
    Webcam.snap(function (data_uri) {
        $("#snapshotFan").show();
        $("#snapshotFan").attr("src", data_uri);
        $("#my_camera").hide();
        $("#btnNuevaCaptura").show();
        $("#btnCaptura").hide();
    });

});
$("#btnNuevaCaptura").click(function () {
    $("#snapshotFan").hide();
    $("#snapshotFan").attr("src", "");
    $("#my_camera").show();
    $("#btnCaptura").show();
    $("#btnNuevaCaptura").hide();

});
$("#txtBirthday").datetimepicker({
    format: 'YYYY-MM-DD',
    maxDate: moment().subtract(1, 'y')

});

$("#txtDoc").change(function () {
    EjecutarAjax(urlBase + "Fidelizacion/Buscar", "GET", { doc: $(this).val() }, "successPintarDatosClienteFan", null);
});

function successPintarDatosClienteFan(data) {
    if (data.rta) {
        if (data.obj.Consecutivo !== null) {
            if (data.obj.Consecutivo.indexOf("renovar|") >= 0 && data.obj.Consecutivo.replace("renovar|", "") === $("#TRecargable").val()) {
                $("#txtName").val(data.obj.Nombre);
                $("#txtMail").val(data.obj.Correo);
                $("#txtPhone").val(data.obj.Telefono);
                $("#txtBirthday").val(data.obj.FechaNacimiento);
                $("#txtGenderFan").val(data.obj.Genero);
                $("#txtAddress").val(data.obj.Direccion);
                $("#txtName").focus();
                $("#hdrenova").val("1");
                $("#ValorCompraRecargaFan").val("");
                $("#ValorCompraRecargaFan").prop("disabled", true);
                if (data.obj.FotoTexto != "") {
                    $("#snapshotFan").show();
                    $("#snapshotFan").attr("src", data.obj.FotoTexto);
                    $("#my_camera").hide();
                    $("#btnNuevaCaptura").show();
                    $("#btnCaptura").hide();
                }
                MostrarMensajeRedireccion("Importante", "Este cliente ya cuenta con una tarjeta asociada, pero su contrato fan ha vencido, en este proceso solo se renovará el contrato FAN", null, "warning");
            } else {
                $("#txtDoc").val("");
                MostrarMensajeRedireccion("Importante", "Este cliente ya cuenta con una tarjeta asociada, para poder activarla como fan por favor bloquear la que se encuentra activa en el momento", null, "warning");

            }

        } else {
            $("#txtName").val(data.obj.Nombre);
            $("#txtMail").val(data.obj.Correo);
            $("#txtPhone").val(data.obj.Telefono);
            $("#txtBirthday").val(data.obj.FechaNacimiento);
            $("#txtGenderFan").val(data.obj.Genero);
            $("#txtAddress").val(data.obj.Direccion);
            $("#txtName").focus();
            $("#ValorCompraRecargaFan").prop("disabled", false);
            $("#hdrenova").val("");
            if (data.obj.FotoTexto != "") {
                $("#snapshotFan").show();
                $("#snapshotFan").attr("src", data.obj.FotoTexto);
                $("#my_camera").hide();
                $("#btnNuevaCaptura").show();
                $("#btnCaptura").hide();
            }
        }

    }
}

//FIDELIZACION

$(".TipoPago:input[type=text]").mask("000.000.000", { reverse: true });
$(".Numerico").mask("000000000000");

$("#selectPosConvenioSAP").select2({
    placeholder: "Seleccione el convenio"
});

//DANR: adicion eventos tarjeta recargable
$("#btnCancelPos").click(function () {
    cerrarModal("modalPOS");
})

$("#btnSavePos").click(function () {
    //falta validar documento q no exista o con tarjetas bloqueadas
    if ($("#DocClienteTR").val() == "") {
        $("#modalPOS .msjErrorModal #div_message_error #lbl_message_error").html("El campo Documento es obligatorio");
        $("#modalPOS .msjErrorModal #div_message_error").show();
    } else if ($("#NombreCliente").val() == "") {
        $("#modalPOS .msjErrorModal #div_message_error #lbl_message_error").html("El campo Nombre es obligatorio");
        $("#modalPOS .msjErrorModal #div_message_error").show();
    } else if ($("#CorreoCliente").val() == "") {
        $("#modalPOS .msjErrorModal #div_message_error #lbl_message_error").html("El campo Correo es obligatorio");
        $("#modalPOS .msjErrorModal #div_message_error").show();
    } else if (!validarEmail($("#CorreoCliente").val())) {
        $("#modalPOS .msjErrorModal #div_message_error #lbl_message_error").html("El correo ingresado es invalido");
        $("#modalPOS .msjErrorModal #div_message_error").show();
    } else if ($("#txtGender option:selected").val() === "") {
        $("#modalPOS .msjErrorModal #div_message_error #lbl_message_error").html("Seleccione un genero valido");
        $("#modalPOS .msjErrorModal #div_message_error").show();
    } else if (parseInt($("#ValorCompraRecarga").val().replace(/\./g, "")) > 1000000) {
        MostrarMensaje("Error", "El valor de la recarga no debe superar $1.000.000", "error");
    } else {
        EjecutarAjax(urlBase + "Pos/ValidarDocumento", "GET", { doc: $("#DocClienteTR").val() }, "AgregarTarjetaYRecarga", null);
    }

})
function LimpiarFan() {
    $("#TRecargable").val("");
    $("#txtName").val("");
    $("#txtMail").val("");
    $("#txtPhone").val("");
    $("#txtBirthday").val("");
    $("#txtAddress").val("");
    $("#txtGenderFan").val("");
    $("#txtDoc").val("");
    $("#ValorCompraRecargaFan").val("");
    $("#ValorCompraRecargaFan").prop("disabled", false);
    $("#snapshotFan").attr("src", "");
    $("#snapshotFan").hide();
    $("#my_camera").show();
    $("#btnCaptura").show();
    $("#btnNuevaCaptura").hide();
    $("#hdrenova").val("");

    $(".otherField").hide();
}
function LimpiarRepo() {
    $("#DocClienteRepo").val("");
    $("#TRecargableRepo").val("");
    $("#hdIdProdRepo").val("");
}
function LimpiarCortesia() {
    $("#NumTarjeta").val("");
    $("#txtCodigo").val("");    

    $("#Detallecortesia").html("");

    $(".otherField").hide();
    listProductosCortesia = [];
    tablaBodyCortesias = "";
    IdDetalleCortesia = 0;
    esMejoraCortesia = false;
    usuarioCortesia = "";
    
}
$("#btnCancelFan").click(function () {
    cerrarModal("modalFAN");
    LimpiarFan();
})
$("#btnSaveRepo").click(function () {

    if (validarFormulario("frmRepo")) {

        EjecutarAjax(urlBase + "Pos/ValidarDocumento", "GET", { doc: $("#DocClienteRepo").val() }, "AgregarTarjetaYRepo", null);
    }
})
$("#btnCancelCortesia").click(function () {
    cerrarModal("modalRedencionCortesia");
    LimpiarCortesia();
})

$("#btnSaveCortesia").click(function () {
    cerrarModal("modalRedencionCortesia");    
})

var contador = 0;
$("#btnSaveFan").click(function () {
    $("#btnSaveFan").hide();
    _tarjetafanclien = $("#TRecargable").val();
    if (validarFormulario("frmFan")) {
        if ($("#snapshotFan").prop("src").replace('#', '') === window.location.href.replace('#', '')) {
            MostrarMensajeRedireccion("Importante", "La foto es obligatoria, por favor tomela para poder guardar la informacion", null, "warning");
            $("#btnSaveFan").show();
        } else if (parseInt($("#ValorCompraRecargaFan").val().replace(/\./g, "")) > 1000000) {
            MostrarMensaje("Error", "El valor de la recarga no debe superar $1.000.000", "error");
            $("#btnSaveFan").show();
        } else if (parseInt($(this).val().split('/')[0]) > 31 || parseInt($(this).val().split('/')[1]) > 12 || parseInt($(this).val().split('/')[2]) < 1800) {
            MostrarMensaje("Error", "La fecha ingresada no es valida", "error");
            $("#btnSaveFan").show();
        }
        else {
            if ($("#hdrenova").val() === "1") {
                var ProductoSeleccionado = ObtenerProducto(tmpListaServicios, $("#hdIdProd").val());
                ProductoSeleccionado[0].Cantidad = 1;
                ProductoSeleccionado[0].DataExtension = $("#TRecargable").val();
                ProductoSeleccionado[0].PrecioTotal = ProductoSeleccionado[0].Precio;
                lstProductosCompra.push(ProductoSeleccionado[0]);


                ActualizarTablaCompras();
                MostrarcambioApp();
                cerrarModal("modalFAN");
                $("#TRecargable").val("");
                $("#btnSaveFan").show();
            }
            else {
                $.ajax({
                    url: 'Pos/ObtenerBrazalete',
                    ContentType: 'application/json; charset-uft8',
                    dataType: 'json',
                    type: 'get',
                    data: { CodBarra: $("#TRecargable").val() },
                    success: function (r) {
                        if (r.IdProducto > 0) {
                            var ProductoSeleccionado = ObtenerProducto(tmpListaServicios, $("#hdIdProd").val());
                            ProductoSeleccionado[0].Cantidad = 1;
                            ProductoSeleccionado[0].DataExtension = $("#TRecargable").val();
                            ProductoSeleccionado[0].PrecioTotal = ProductoSeleccionado[0].Precio;
                            lstProductosCompra.push(ProductoSeleccionado[0]);

                            var productoPorAdd = r;
                            if (ValidarRecargaAdicionproducto(productoPorAdd.CodigoSap)) {
                                productoPorAdd.Cantidad = 1;
                                productoPorAdd.PrecioTotal = productoPorAdd.Precio;
                                productoPorAdd.DataExtension = "1" + $("#txtDoc").val() + "|2" + $("#txtName").val() + "|3" + $("#txtMail").val() + "|4" + $("#txtGenderFan option:selected").val() + "|5" + $("#txtPhone").val() + "|6" + $("#txtBirthday").val() + "|7" + $("#txtAddress").val() + "|8" + $("#snapshotFan").attr("src").replace("data:image/png;base64,", "");
                                lstProductosCompra.push(productoPorAdd);
                                ////adicionar precio tarjeta
                                //var r = ObtenerproductoPorCodigoSap(parametros.CodSapPrecioTarjeta);
                                //r.Cantidad = 1;
                                //r.Entregado = true;
                                //r.PrecioTotal = r.Precio;
                                //r.DataExtension = productoPorAdd.ConseutivoDetalleProducto;
                                //lstProductosCompra.push(r);

                                var precioRecarga = parseInt($("#ValorCompraRecargaFan").val().replace(/\./g, ""));
                                if (precioRecarga > 0) {
                                    var r = ObtenerproductoPorCodigoSap(parametros.CodSapRecargaTarjeta.Valor);
                                    r.Precio = precioRecarga;
                                    r.Cantidad = 1;
                                    r.Entregado = true;
                                    r.PrecioTotal = r.Precio;
                                    r.DataExtension = productoPorAdd.ConseutivoDetalleProducto;
                                    lstProductosCompra.push(r);
                                }

                                ActualizarTablaCompras();
                                MostrarcambioApp();
                                cerrarModal("modalFAN");
                                $("#TRecargable").val("");
                            }
                        }
                        else {
                            MostrarMensajeRedireccion("Importante", r.MensajeValidacion, null, "warning");

                        }

                        $("#btnSaveFan").show();
                    },
                    error: function (a, b, c) {

                        MostrarMensajeRedireccion("Error", a.responseText, null, "error");

                        $("#btnSaveFan").show();
                    }
                });
            }
        }


    }
    else {
        $("#btnSaveFan").show();
    }

})

function AgregarTarjetaYRepo(data) {
    if (data !== "") {
        $("#modalRepo .msjErrorModal #div_message_error #lbl_message_error").html(data);
        $("#modalRepo .msjErrorModal #div_message_error").show();
    } else {
        $.ajax({
            url: 'Pos/ObtenerBrazalete',
            ContentType: 'application/json; charset-uft8',
            dataType: 'json',
            type: 'get',
            data: { CodBarra: $("#TRecargableRepo").val() },
            success: function (r) {
                if (r.IdProducto > 0) {
                    var ProductoSeleccionado = ObtenerProducto(tmpListaServicios, $("#hdIdProdRepo").val());
                    ProductoSeleccionado[0].Cantidad = 1;
                    ProductoSeleccionado[0].PrecioTotal = ProductoSeleccionado[0].Precio;
                    lstProductosCompra.push(ProductoSeleccionado[0]);

                    var productoPorAdd = r;
                    if (ValidarRecargaAdicionproducto(productoPorAdd.CodigoSap)) {
                        productoPorAdd.Cantidad = 1;
                        productoPorAdd.PrecioTotal = productoPorAdd.Precio;
                        productoPorAdd.DataExtension = "1" + $("#DocClienteRepo").val();
                        lstProductosCompra.push(productoPorAdd);

                        ActualizarTablaCompras();
                        MostrarcambioApp();
                        cerrarModal("modalRepo");
                    }
                }
                else {
                    MostrarMensajeRedireccion("Importante", r.MensajeValidacion, null, "warning");

                }
            },
            error: function (a, b, c) {

                MostrarMensajeRedireccion("Error", a.responseText, null, "error");
            }
        });

    }
}
function AgregarTarjetaYRecarga(data) {
    if (data !== "") {
        $("#modalPOS .msjErrorModal #div_message_error #lbl_message_error").html(data);
        $("#modalPOS .msjErrorModal #div_message_error").show();
    } else {
        if (ValidarRecargaAdicionproducto(productoPorAdd.CodigoSap)) {
            productoPorAdd.Cantidad = 1;
            productoPorAdd.PrecioTotal = productoPorAdd.Precio;
            productoPorAdd.DataExtension = "1" + $("#DocClienteTR").val() + "|2" + $("#NombreCliente").val() + "|3" + $("#CorreoCliente").val() + "|4" + $("#txtGenderFan option:selected").val();
            lstProductosCompra.push(productoPorAdd);
            //adicionar precio tarjeta
            var r = ObtenerproductoPorCodigoSap(parametros.CodSapPrecioTarjeta.Valor);
            r.Cantidad = 1;
            r.Entregado = true;
            r.PrecioTotal = r.Precio;
            r.DataExtension = productoPorAdd.ConseutivoDetalleProducto;
            lstProductosCompra.push(r);

            var precioRecarga = parseInt($("#ValorCompraRecarga").val().replace(/\./g, ""));
            if (precioRecarga > 0) {
                var r = ObtenerproductoPorCodigoSap(parametros.CodSapRecargaTarjeta.Valor);
                r.Precio = precioRecarga;
                r.Cantidad = 1;
                r.Entregado = true;
                r.PrecioTotal = r.Precio;
                r.DataExtension = productoPorAdd.ConseutivoDetalleProducto;
                lstProductosCompra.push(r);
            }
            ActualizarTablaCompras();
            MostrarcambioApp();

        }
        //$("#ValorCompraRecarga").val("");
        $("#DocClienteTR").val("");
        $("#modalPOS .msjErrorModal #div_message_error #lbl_message_error").html("");
        $("#modalPOS .msjErrorModal #div_message_error").hide();
        cerrarModal("modalPOS");
    }
}

$.each($(".tipoProducto"), function (i, item) {
    if ($(item).attr('id').indexOf(TipoPrincipal) === -1)
        $(item).hide();
});

//Ocultar franquicia 
$("#colFranquicia").hide();
$("#colReferencia").hide();

function MostrarContenedorProducto(id) {
    //Limpiar Buscador

    $.each($(".txtBuscador"), function (i, item) {
        $(item).val("");
    });

    $("#txtBuscador").val("");
    //Muestra todos productos y no tiene encuenta el filtro - se comprate parcial souvenir y alimentos
    $.each($(".ProductoGrilla"), function (i, v) {
        $(v).show();
    })

    $.each($(".tipoProducto"), function (i, item) {

        if ($(item).attr('id').indexOf(id) == -1)
            $(item).hide();
        else
            $(item).show();
    });
}

$(".linkProductos").click(function () {
    var id = $(this).data("id");
    _opcionSeleccionada = id;
    MostrarContenedorProducto(id);
});
//CupoDebito
$("#btnAdicionarCupoDebito").click(function () {
    AdicionarCupo();
});
//Fin Cupo Debito
//Tiquete Ingreso
$("#listPasaporte").select2({
    placeholder: "Seleccione el pasaporte Uso"
});

$("#AdicionarTiquete").click(function () {
    if (validarObligatorios("frmTiqueteIngreso")) {

        var idPasaporte = $("#listPasaporte").val();

        $.ajax({
            url: 'Pos/ObtenerPasaporte/',
            ContentType: 'application/json; charset-uft8',
            dataType: 'json',
            type: 'get',
            data: { IdPasaporte: idPasaporte },
            success: function (r) {
                AdicionarTiquete(r);
            },
            error: function (a, b, c) {
                alert(" Error " + a.responseText);
            }
        })
    }
});
//Fin Tiquete ingreso

//Atracciones
$("#listAtracciones").select2({
    placeholder: "Seleccione la atracción"
});
//Fin Atracciones

//Parqueadero
$("#btnCalculaPagoParqueadero").click(function () {
    calculaPagoParqueadero();
});

$("#inputCodigoParqueadero").keypress(function (evt) {
    if (window.event) {
        var charCode = event.keyCode;
    } else if (evt.onkeypress.arguments[0].charCode) {
        var charCode = evt.onkeypress.arguments[0].charCode;
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    if (!inicializadointervalPark) {
        inicializadointervalPark = true;
        var refreshIntervalIdPark = setInterval(function () { calculaPagoParqueadero(); clearInterval(refreshIntervalIdPark); }, 300);
    }
});

$("#btnAdicionarPagoParqueadero").click(function () {
    AdicionarPagoParqueadero();
});

$("#btnReimprimirParqueadero").click(function () {
    PagoReimprimirParqueadero();
});
//Fin Parqueadero
$(".productos .ProductoGrilla").click(function (e) {
    crearListaBusqueda($(this).data("id"));
});
$(".productos .ProductoGrillaMesas").click(function (e) {
    crearListaBusqueda($(this).data("id"));
});

function crearListaBusqueda(id) {
    _tmpBanderaMesa = null;
    _tmpIdPedido = null;
    var _list;
    if (_opcionSeleccionada == $("#Atracciones")[0].id) {
        _list = tmpListaAyD;
    } else if (_opcionSeleccionada == $("#Souvenir")[0].id) {
        _list = tmpListaSouvenir;
    } else if (_opcionSeleccionada == $("#Servicios")[0].id) {
        _list = tmpListaServicios;
    } else if (_opcionSeleccionada == $("#Boleteria")[0].id) {
        _list = tmpListaPasaporte;
    } else {
        _list = tmpLista;
    }
    if (_opcionSeleccionada == $("#Mesas")[0].id) {
        _tmpBanderaMesa = id;
        EjecutarAjaxJson(urlBase + "PedidoA/ListarProductosMesa", "post", {
            IdMesa: id
        }, "successListarProdMesa", null);

    }
    else {
        AgregarProductoACompra(id, _list);
    }

}

function successListarProdMesa(rta) {
    if (rta !== null) {
        lstProductosCompra = [];
        var lista = tmpLista;
        $.each(rta.listaProductos, function (i, item) {
            _tmpIdPedido = item.Id_Pedido;
            AgregarProductoACompra(item.Id_Producto, lista);
        });
    } else {

    }
}

$(".EditarCod").click(function () {
    $("#CodBarra").val("");
    $("#CodBarra").focus();
    if (optKey) {
        $("#CodBarra").prop("type", "password");
        optKey = false;
    }
    else {
        $("#CodBarra").prop("type", "text");
        optKey = true;
    }
});

$(".EditarRecarga").click(function () {
    $("#CodTarjeta").val("");
    $("#CodTarjeta").focus();
    if (optKeyRecarga) {
        $("#CodTarjeta").prop("type", "password");
        optKeyRecarga = false;
    }
    else {

        $("#CodTarjeta").prop("type", "text");
        optKeyRecarga = true;
    }
});

$('#CodBarra').keypress(function (e) {
    if (!inicializadointerval && !optKey) {
        inicializadointerval = true;
        var refreshIntervalId = setInterval(function () { ConsultarBrazalete(); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);
    } else {
        if (e.which == 13 && $(this).val().length > 0 && optKey) {
            ConsultarBrazalete(); return false;
        }
    }
});

function ConsultarBrazalete() {
    var obj = $("#CodBarra");
    if (obj.length > 0) {
        EjecutarAjax(urlBase + "Pos/ObtenerBrazalete", "GET", { CodBarra: obj.val() }, "successObtenerBrazalete", null);
        obj.val("");
    }
}

$("#DocClienteTR").change(function () {
    EjecutarAjax(urlBase + "Fidelizacion/Buscar", "GET", { doc: $(this).val() }, "successPintarDatosCliente", null);
});

function successPintarDatosCliente(data) {
    if (data.rta) {
        if (data.obj.Consecutivo !== null) {
            $("#DocClienteTR").val("");
            MostrarMensajeRedireccion("Importante", "Este cliente ya cuenta con una tarjeta asociada, para poder activarla por favor bloquear la que se encuentra activa en el momento", null, "warning");
        } else {
            $("#NombreCliente").val(data.obj.Nombre);
            $("#CorreoCliente").val(data.obj.Correo);
            $("#txtGender").val(data.obj.Genero);
        }

    }
}

function limpiarModalPOS() {
    $("#DocClienteTR").val("");
    $("#ValorCompraRecarga").val("");
    $("#NombreCliente").val("");
    $("#CorreoCliente").val("");
    $("#txtGender").val("");
}

function successObtenerBrazalete(rta) {

    if (rta.IdProducto > 0) {
        var obj = BuscarIdBrazalete(rta.IdDetalleProducto);
        if (obj === null) {
            if (rta.DataExtension === "TR") {
                if (contingencia === 1) {
                    MostrarMensaje("Importante", "El producto no puede ser vendido durante la contingencia.", "warning");
                } else {
                    limpiarModalPOS();
                    abrirModal("modalPOS");
                    productoPorAdd = rta;
                }
            } else {
                if (ValidarRecargaAdicionproducto(rta.CodigoSap)) {
                    var cant = 0
                    $.each(lstProductosCompra, function (i, v) {
                        if (v.IdProducto === rta.IdProducto)
                            cant += v.Cantidad;
                    })
                    if (cant < rta.Cantidad) {
                        rta.Cantidad = 1;
                        rta.PrecioTotal = rta.Precio;
                        lstProductosCompra.push(rta);
                        ActualizarTablaCompras();
                        MostrarcambioApp();
                    } else {
                        MostrarMensaje("Importante", "No cuenta con inventario para realizar esta venta.", "warning");
                    }
                }
            }
        } else
            MostrarMensaje("Importante", "El brazalete ya fue agregado en la lista de productos.", "warning");
    } else {

        MostrarMensaje("Importante", rta.MensajeValidacion, "warning");
    }
}
//Buscar 
function BuscarIdBrazalete(id) {
    var objReturn = null;

    $.each(lstProductosCompra, function (i, item) {
        if ($.trim(item.IdDetalleProducto) == $.trim(id)) {
            objReturn = item;
            return false;
        }
    });

    return objReturn;
}

//@ Inicio Propina
$("#chxPropina").click(function () {
    if ($(this).is(':checked')) {
        if (lstProductosCompra != null && lstProductosCompra.length > 0) {
            $("#txtPropina").prop("disabled", false);
            MostrarPropina();
            $("#txtPropina").focus();
        }
    } else {
        $("#txtPropina").val("0");
        $("#txtPropina").prop("disabled", true);
    }
    MostrarTotal();
    MostrarcambioApp();
});

$("#txtPropina").change(function () {
    MostrarTotal();
    MostrarcambioApp();
});

function ObtenerPropina() {
    var valorProp = 0;
    var total = ObtenerTotal();
    valorProp = parseInt((parseInt(total) * parseInt(ValorPropina.Valor)) / 100);
    return valorProp;
}

function MostrarPropina() {
    if ($("#chxPropina").is(":checked"))
        $("#txtPropina").val(EnMascarar(ObtenerPropina()));
}

function AdicionarPropinaProducto() {
    if ($("#chxPropina").is(":checked")) {
        if ($("#txtPropina").val().length > 0) {
            if (parseInt($("#txtPropina").val()) !== 0) {
                var r = ObtenerproductoPorCodigoSap(parametros.CodSapPropina.Valor);//PROPINA Cambioquitar
                r.Precio = $("#txtPropina").val().replace(".", "").replace(".", "");
                r.Cantidad = 1;
                r.PrecioTotal = r.Precio;
                lista.push(r);
            }
        }
    }
}
//@ Fin propina

function MostrarTotal() {
    var total = ObtenerTotal();
}
function EnMascarar(valor) {
    var val = $.trim(valor);
    var fn = val;
    if (val.length > 3)
        fn = val.substring(0, val.length - 3) + "." + val.substring(val.length - 3, val.length);

    if (val.length > 6)
        fn = fn.substring(0, fn.length - 7) + "." + fn.substring(fn.length - 7, fn.length);

    return fn;
}

function ObtenerTotal() {
    var total = 0;
    var impuestoTotal = 0;
    $.each(lstProductosCompra, function (i, item) {
        var precioItem = parseInt(item.PrecioTotal == "" ? 0 : item.PrecioTotal);
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

    $("#total").html("<h1 style='font-weight: bold;'> $" + EnMascarar(total) + "</h1>");
    $("#TotalImpuestos").html(EnMascarar(impuestoTotal));
    $("#TotalBase").html(EnMascarar(total - impuestoTotal));

    return parseInt(total);
}

function Limpiar() {
    $("#total").html("<h1 style='font-weight: bold;'>$0</h1>");
    //$("#TotalIva").html("$0");
    $("#dvProductos").html("");
    $("#cambio").html("Cambio: $0");
    $("#TotalPagado").html("$0");
    $("#TotalBase").html("$0");
    $("#TotalImpuestos").html("$0");
    $("#txtDonante").val("0");
    $("#divDonante").hide();
    lista = [];
    listaDetallePago = [];
    lstProductosCompra = [];
    listDetalleConvenio = [];

    $.each($(".TipoPago:input[type=text]"), function (i, item) {
        $(this).val("");
    });

    //Limpiar caja buscador
    $.each($(".txtBuscador"), function (i, item) {
        $(item).val("");
    })
    //Mostrar todos los productos
    $.each($(".ProductoGrilla"), function (i, v) {
        $(v).show()
    });

    //Limpiar medios de pago 

    $("#selectMedioPago").val("");
    $("#selectFranquicia").val("");
    $("#inputReferencia").val("");
    $("#inputValorPago").val("");

    selectMedioPagoChange();


    //Posicionar al inicio no al final 
    $(this).scrollTop(0);

    $("#tableBodyPagos").html("");
    $("#txtPropina").val("0");
    $("#txtPropina").prop("disabled", true);
    $("#chxPropina").prop("checked", false);

    optKey = false;
    $("#CodBarra").prop("type", "password");
    optKeyRecarga = false;
    $("#CodTarjeta").prop("type", "password");
    $("#div_print").html("");

    TieneCodigoReservaParqueadero = false;
    $("#inputCodigoReservaParqueadero").removeAttr("disabled");
    CodigoBarrasConvenioPistoleadedo = "";

    $("#divCodigoReservaParqueadero").hide();
    $("#divCodigoPagoParqueadero").hide();


    if (contingencia == 0) {
        $("#selectPosConvenioSAP").removeAttr("disabled");
        var $selectPosConvenioSAP = $("#selectPosConvenioSAP").select2();

        $selectPosConvenioSAP.val('').trigger("change");
        $("#selectPosConvenioSAP").select2({
            placeholder: "Seleccione el convenio"
        });
    } else {
        $("#selectPosConvenioSAP").attr("disabled", "disabled");
    }
    $("#inputConvenioCodBarrasSAP").val("");
    if (contingencia == 0)
        $("#inputConvenioCodBarrasSAP").removeAttr("disabled");
    else {
        $("#inputConvenioCodBarrasSAP").attr("disabled", "disabled");
    }
    $("#CodTarjeta").val("");
    $("#ValorRecargaTR").val("");
    $("#snapshot").attr("src", "#").hide();

    inicializadointerval = false;
    inicializadointervalPark = false;
    inicializadointervalResPark = false;
    inicializadointervalConvenio = false;
    iniIntervalPagoNomina = false;
    reiniciarMediosPago();
    EjecutarAjax(urlBase + "Pos/ObtenerUltimaFacturaPunto", "GET", "", "successObtenerFactura", null);

    var select22 = document.getElementById("selectMedioPago");
    for (var i = 0; i < select22.length; ++i) {
        if (select22.options[i].value == '14') {
            select22.remove(i);
        }
    }

    EjecutarAjaxJson(urlBase + "Pos/ActualizarMesas", "GET", "", "successActualizarMesas", { div: "#Mesas" });
}

function successActualizarMesas(data, values) {
    if (isNaN(data)) {
        $(values.div).html(data);
        $(".productos .ProductoGrillaMesas").click(function (e) {
            crearListaBusqueda($(this).data("id"));
        });

    } else {

    }
}

function successObtenerFactura(rta) {
    if (rta.CodigoFactura != null && rta.CodigoFactura.length > 0)
        $("#lblConsecutivo").html(" <br />  Última venta del punto: " + rta.CodigoFactura);
}

$("#btnCancelar").click(function () {
    Limpiar();
});

function AdicionarDonacion() {
    var codSap = $("#txtDonante").data("codigo");
    var listado = tmpListaServicios;
    AgregarProductoACompra(codSap, listado);
}

$("#btnPagar").click(function () {
    $("#btnPagar").attr("disabled", true);

    var sum = 0;
    var total = ObtenerTotal();
    var cambio = 0;
    lista = [];

    //Retirar enmarcado de obligatorios 
    $("#selectMedioPago, #inputValorPago").attr("data-mensajeerror", "");
    $("#selectMedioPago, #inputValorPago").removeClass("errorValidate");
    QuitarTooltip();

    //Valida que el usuario por lo menos seleccione un producto
    if (lstProductosCompra.length == 0) {
        MostrarMensajeRedireccion("Importante", "Debe seleccionar al menos un producto.", null, "warning");
        $("#btnPagar").attr("disabled", false);
        return false;
    }

    //Valida que el Valor sea > 0


    //Valida que no que ningun campo de cantidad este con una advertencia

    var cantidadCampo = false;
    $.each($(".evtCambiar"), function (key, element) {
        if ($(element).hasClass('errorValidate'))
            cantidadCampo = true;
    });

    if (cantidadCampo) {
        MostrarMensajeRedireccion("Importante", "Cantidad para el convenio superada", null, "warning");
        $("#btnPagar").attr("disabled", false);
        return false;
    }

    //Sumatoria de la cantidad de dinero recibido en taquilla
    if (listaDetallePago.length > 0) {
        $.each(listaDetallePago, function (i, item) {
            sum += item.Valor;
        });
    }

    if (sum >= total) {

        if (sum > total) {
            var hayPagoEfectivo = false;
            if (listaDetallePago.length > 0) {
                $.each(listaDetallePago, function (i, item) {
                    if (item.IdMedioPago == 1 || item.IdMedioPago == "1") {
                        hayPagoEfectivo = true;
                    }
                });
            }
            if (!hayPagoEfectivo) {
                MostrarMensajeRedireccion("Importante", "Solo se puede dar cambio con pago en efectivo", null, "warning");
                $("#btnPagar").attr("disabled", false);
                return false;
            }
        }

        //EDSP 26/01/2018 - Cambio evitar que hagan avances
        if (hayPagoEfectivo) {
            var efectivo = 0;
            $.each(listaDetallePago, function (a, b) {
                if (b.IdMedioPago == 1 || b.IdMedioPago == "1") {
                    efectivo += b.Valor;
                }
            });

            var _cambio = parseInt(sum - total);
            if (efectivo < _cambio) {
                MostrarMensajeRedireccion("Importante", "El cambio no puede ser superior al efectivo recibido", null, "warning");
                $("#btnPagar").attr("disabled", false);
                return false;
            }

        }

        if (!ValidarDonante()) {
            $("#btnPagar").attr("disabled", false);
            return false;
        }

        cambio = parseInt(sum - total);
        if ($("#divDonarCambio").css("display") !== "none" && $("#cbDonacion").prop("checked") === true && cambio > 0) {
            for (var i = 0; i < (cambio / 50); i++) {
                AdicionarDonacion();
            }
            $("#btnPagar").click();
            return false;

        }

        $("#cambio").html("Cambio: $" + EnMascarar(cambio));

        var CopiaProducto = JSON.parse(JSON.stringify(lstProductosCompra))

        $.each(CopiaProducto, function (i, item) {
            for (i = 0; i < item.Cantidad; i++) {
                lista.push(item);
            }
        });

        $.each(lista, function (i, item) {
            item.Cantidad = 1;
        });

        var _list = JSON.parse(JSON.stringify(lista))
        lista = [];
        lista = _list;

        //Si aplica adicionara propina    
        AdicionarPropinaProducto();
        var _aplicaSINO = false;

        $.each(lista, function (a, obj) {
            if (!(ValidaListaSINO(obj))) {
                _aplicaSINO = true;
            }
            else {
                var aplicaEntregado = false;
                var list = parametros.CodSapTipoProdDescargue.Valor.split(',');
                $.each(list, function (i, item) {
                    if (item == obj.CodSapTipoProducto)
                        aplicaEntregado = true;
                    else if (obj.CodigoSap == parametros.CodSapRecargaTarjeta.Valor)
                        aplicaEntregado = true;
                });
                if (aplicaEntregado) {
                    obj.Entregado = true;
                }
            }

        });

        //Mostrar si o no - 23/02/2018 : Si está en contigencia no muestra mensaje de descarga pasa directamente 
        if (!_aplicaSINO || contingencia == 1)
            ConfirmarPago()
        else
            MostrarConfirmSINO("Importante!", "Desea descargar los productos seleccionados", "MostrarModalProductos", "CancelarDescargoProductos", "");
        $("#btnPagar").attr("disabled", false);
    } else {
        MostrarMensajeRedireccion("Importante", "El valor ingresado es menor al valor a pagar.", null, "warning");
        $("#btnPagar").attr("disabled", false);
    }
});
function ValidarDonante() {
    if ($("#divDonante").css("display") !== "none") {
        if ($("#txtDonante").val() === "" || $("#txtDonante").val() === "0") {
            MostrarMensajeRedireccion("Importante", "Por favor ingrese el numero de documento del donante", null, "warning");
            return false;
        }
        var mp = false;
        $.each(listaDetallePago, function (a, b) {
            if (b.IdMedioPago == 1 || b.IdMedioPago == "1" //||
                //b.IdMedioPago == 2 || b.IdMedioPago == "2" ||
                //b.IdMedioPago == 3 || b.IdMedioPago == "3" ||
                //b.IdMedioPago == 9 || b.IdMedioPago == "9"
            ) {
                mp = true;
            }
        });
        if (!mp)
            MostrarMensajeRedireccion("Importante", "Las donaciones solo son permitidas con medio de pago en efectivo", null, "warning");
        return mp;
    }

    return true;
}
//Muestra modal de los productos para el descargue
function MostrarModalProductos() {
    EjecutarAjaxJson(urlBase + "Pos/ObtenerDetalleEntrega", "POST", { listaProductos: lista }, "printPartialModal", {
        title: "Descargar producto ", hidesave: true, modalLarge: false, OcultarCierre: true
    });
}

//Método que invoca el evento cancelar del formulario descargue de productos
function CancelarDescargoProductos() {
    $.each(lista, function (i, item) {
        item.Entregado = false;
    });
    ConfirmarPago();
}

//Método que invoca el evento aceptar del formulario descargue productos
function AceptarDescargoProductos() {
    if (!ValidarCampos()) {
        MostrarMensaje("Importante", "Hay inconsistencias en el formulario, revise los campos demarcados con color rojo.", "error");
    } else {
        $.each($(".ChxDescarga"), function (i, item) {

            if ($(item).is(':checked')) {
                var id = $(item).data("id");
                var cantidad = $.trim($("#txt_" + id).val());
                setDescargueProducto(cantidad, id);
            }
        });
        ConfirmarPago();
    }
}

//Método que actualiza la lista con los productos descargados
function setDescargueProducto(cantidad, idProducto) {
    var count = 0;
    var idprod = parseInt(idProducto);
    var Valor = cantidad.length > 0 ? parseInt(cantidad) : 0;

    $.each(lista, function (i, item) {
        if (item.IdProducto == idprod) {
            if (count < Valor)
                item.Entregado = true;
            else
                item.Entregado = false;
            count++;
        }
    });
}

//Eventos check descargue producto EDSP
function SetEventDescargueProducto() {
    $(".Cantidad").keyup(function () {

        var id = $(this).data("id");
        if ($.trim($(this).val()).length > 0 && $("#chx_" + id).is(":checked")) {
            $(this).attr("data-mensajeerror", "");
            $(this).removeClass("errorValidate");
            QuitarTooltip();

            var CantidadMaxima = $(this).data("max");
            var cantidad = parseInt($.trim($(this).val()));
            if (cantidad == 0) {
                $(this).val(CantidadMaxima);
            } else {
                if (cantidad > CantidadMaxima) {
                    $(this).attr("data-mensajeerror", "La cantidad no puede ser superior a " + CantidadMaxima);
                    $(this).addClass("errorValidate");
                    mostrarTooltip();
                }
            }
        }
    });

    //Evento check Descargue producto
    $(".ChxDescarga").click(function () {
        var id = $(this).data("id");
        var obj = $("#txt_" + id);
        var CantidadMaxima = $(obj).data("max");
        var cantidad = parseInt($.trim($(obj).val()));

        if ($(this).is(':checked')) {
            QuitarTooltip();
            $(obj).removeClass("errorValidate");
            if (cantidad == 0) {
                $(obj).val(CantidadMaxima);
            } else {
                if (cantidad > CantidadMaxima) {
                    $(obj).attr("data-mensajeerror", "La cantidad no puede ser superior a " + CantidadMaxima);
                    $(obj).addClass("errorValidate");
                    mostrarTooltip();
                }
            }
        } else {
            $(obj).val(CantidadMaxima);
            QuitarTooltip();
            $(obj).removeClass("errorValidate");
        }
    });
}

// Validar campos descarga producto
function ValidarCampos() {
    var result = true;
    $.each($(".ChxDescarga"), function (i, item) {
        if ($(item).is(":checked")) {
            var id = $(item).data("id");
            if ($("#txt_" + id).hasClass('errorValidate')) {
                result = false
            }
        }
    });
    return result;
}

function ConfirmarPago() {
    cerrarModal('modalCRUD');
    $.each(lista, function (i, item) {
        if (ValidaListaSINO(item)) {
            var aplicaEntregado = false;
            var list = parametros.CodSapTipoProdDescargue.Valor.split(',');
            $.each(list, function (i, value) {
                if (value == item.CodSapTipoProducto)
                    aplicaEntregado = true;
                else if (item.CodigoSap == parametros.CodSapRecargaTarjeta.Valor)
                    aplicaEntregado = true;
            });
            if (aplicaEntregado) {
                item.Entregado = true;
            }
        }
    })
    var CodSapConvenioVal = listDetalleConvenioSAPGetCodSapConvenio();
    var idConvenio = listDetalleConvenioSAPGetIdConvenio();
    var numTarjeta = $("#NumTarjeta").val();
    if (CodSapConvenioVal == "") {
        EjecutarAjaxJson(urlBase + "Pos/PagarCompra", "post", {
            IdPedido: _tmpIdPedido,
            ListaProductos: lista,
            MediosPago: listaDetallePago,
            Donante: ($("#divDonante").css("display") !== "none" ? $("#txtDonante").val() : "")
        }, "successPagarPos", null);

    } else {        
        EjecutarAjaxJson(urlBase + "Pos/PagarCompra", "post", {
            IdPedido: _tmpIdPedido,
            ListaProductos: lista,
            MediosPago: listaDetallePago,
            CodSapConvenio: CodSapConvenioVal,
            ConsecutivoConvenio: CodigoBarrasConvenioPistoleadedo,
            IdConvenio: idConvenio,
            productosCortesia: listProductosCortesia,
            numTarjeta: numTarjeta,
            usuarioCortesia: usuarioCortesia,
            IdDetalleCortesia: IdDetalleCortesia,
            Donante: ($("#divDonante").css("display") !== "none" ? $("#txtDonante").val() : "")
        }, "successPagarPos", null);
    }
}
function AgregarCortesiasTarjetaFan() {
    var bandera = false;

$.each(lstProductosCompra, function (i) {    
        if (lstProductosCompra[i].CodigoSap === parametros.CodSapClienteFan.Valor ) {
            bandera = true;                                    
        } 
    if (lstProductosCompra[i].DataExtension.split('|').length > 1 && bandera==true) {
        var DataExtension = lstProductosC0ompra[i].DataExtension.split('|');        
        EjecutarAjaxJson(urlBase + "Pos/InsertarCortesias", "post", {
            Documento: DataExtension[0].substring(1),
            Nombre: DataExtension[1].substring(1),
            Correo: DataExtension[2].substring(1),
            Genero: DataExtension[3].substring(1),
            Telefono: DataExtension[4].substring(1),
            FechaCumple: DataExtension[5].substring(1),
            Direccion: DataExtension[6].substring(1),
            CodTarjetaFAN: lstProductosCompra[i].ConseutivoDetalleProducto,
            Foto: DataExtension[7].substring(1)
            }, "successagregarCortesias", null);
    }

});
    //if (bandera == true) {
    //    var DataExtension = "1" + $("#txtDoc").val() + "|2" + $("#txtName").val() + "|3" + $("#txtMail").val() + "|4" + $("#txtGenderFan option:selected").val() + "|5" + $("#txtPhone").val() + "|6" + $("#txtBirthday").val() + "|7" + $("#txtAddress").val() + "|8" + $("#snapshotFan").attr("src").replace("data:image/png;base64,", "");

    //    EjecutarAjaxJson(urlBase + "Pos/InsertarCortesias", "post", {
    //        Documento: $("#txtDoc").val(),
    //        Nombre: $("#txtName").val(),
    //        Correo: $("#txtMail").val(),
    //        Genero: $("#txtGenderFan option:selected").val(),
    //        Telefono: $("#txtPhone").val(),
    //        FechaCumple: $("#txtBirthday").val(),
    //        Direccion: $("#txtAddress").val(),
    //        CodTarjetaFAN: _tarjetafanclien,
    //        Foto: $("#snapshotFan").attr("src").replace("data:image/png;base64,", "")
    //    }, "successagregarCortesias", null);

    //}
}

function successagregarCortesias(rta) {
    var mensaje = rta;
}
function successPagarPos(rta) {
    console.log(rta.hasOwnProperty("impresionLinea"))
    //if (isNaN(rta)) {
    if (rta.hasOwnProperty("impresionLinea")) {
        if (rta.impresionLinea.includes("excede las boletas existentes")) {
            MostrarMensaje("Importante", rta.impresionLinea, "error");
        }
        else if (rta.impresionLinea != null) {
            MostrarMensaje("Importante", rta.impresionLinea, "success");
            AgregarCortesiasTarjetaFan();
            Limpiar();
            LimpiarCortesia();
        }
        else {
            AgregarCortesiasTarjetaFan();
            Limpiar();
            LimpiarCortesia();
        }
    }
    else {
        if (isNaN(rta)) {
            if (rta.indexOf('frmLogin') > -1)
                window.location.href = "Home";
            else {
                Limpiar();
                LimpiarCortesia();
                MostrarMensajeHTML("Importante", rta, "error");
            }
        } else {
            AgregarCortesiasTarjetaFan();
            Limpiar();
            LimpiarCortesia();
        }
    }
    
}

function MostrarcambioApp() {
    var sum = 0;
    var total = ObtenerTotal();
    var cambio = 0;

    if (listaDetallePago.length > 0) {
        $.each(listaDetallePago, function (i, item) {
            sum += item.Valor;
        });
    }

    if (sum >= total) {
        cambio = parseInt(sum - total);
        $("#cambio").html("Cambio: $" + EnMascarar(cambio));
    } else {
        $("#cambio").html("<div style='color: red;'> Cambio: <b> - $ " + EnMascarar(parseInt(sum - total) * -1) + "</b></div>");
    }
}

//Cupo debito
$("#linkCupoDebito").click(function () {
    EjecutarAjax(urlBase + "Pos/ObtenerCupoDebito", "GET", null, "printPartialModal", { title: "Cupo débito", hidesave: true, modalLarge: false });
});

function AdicionarCupo() {
    var valorcupo = $("#ValorCupoDebito");

    valorcupo.removeClass("errorValidate");
    var valorcupo = valorcupo.val();
    if (valorcupo == "") {
        $("#ValorCupoDebito").attr("data-mensajeerror", "Este campo es obligatorio");
        $("#ValorCupoDebito").addClass("errorValidate");
        return;
    } else {
        if (ValidarRecargaAdicionproducto(parametros.CodSapCupoDebito.Valor)) {
            var r = ObtenerproductoPorCodigoSap(parametros.CodSapCupoDebito.Valor); // CUPO DÉBITO Cambioquitar
            r.Precio = $("#ValorCupoDebito").val().replace(".", "").replace(".", "");
            r.Cantidad = 1;
            r.PrecioTotal = r.Precio;//
            lstProductosCompra.push(r);
            ActualizarTablaCompras();
            MostrarcambioApp();
        }
        $("#ValorCupoDebito").val("");
    }
}
//Fin cupo debito
// Inicio Atraccion 
$("#linkAtraccion").click(function () {
    EjecutarAjax(urlBase + "Pos/ObtenerTiqueteAtraccion", "GET", null, "printPartialModal", { title: "Tiquete atracción", hidesave: true, modalLarge: false });
});

function AdicionarTiqueteAtraccion(Atraccion) {
    var r = ListaTodosProductosSAPGetById(18);//TIQUETE ATRACCION Cambioquitar
    r.Precio = Atraccion.Valor;
    r.Nombre = Atraccion.Nombre;
    r.IdDetalleProducto = Atraccion.Id;
    r.Cantidad = 1;
    r.PrecioTotal = r.Precio;
    lstProductosCompra.push(r);
    ActualizarTablaCompras();
    MostrarcambioApp();
    var $lt = $("#listAtracciones").select2();
    $lt.val('').trigger("change");
    cerrarModal("modalCRUD");
}
//Fin Atraccion 
function AdicionarTiquete(Pasaporte) {
    var precio = parseInt(Pasaporte.Precio);
    var r = ListaTodosProductosSAPGetById(4);//PASAPORTE USO Cambioquitar
    r.Precio = precio;
    r.Nombre = Pasaporte.Nombre;
    r.IdDetalleProducto = Pasaporte.Id;
    r.Cantidad = 1;
    r.PrecioTotal = r.Precio;
    lstProductosCompra.push(r);
    ActualizarTablaCompras();
    MostrarcambioApp();
    var $lt = $("#listPasaporte").select2();
    $lt.val('').trigger("change");
    cerrarModal("modalCRUD");
}
//Fin 

/**Pagos*************************************/
$(function () {
    $("#btnDetallePago").click(function () {
        EjecutarAjax(urlBase + "Pos/DetallePagos", "GET", null, "succesLoadMediosPago", { title: "Detalle del Pago", url: "", metod: "GET", func: "loadCalendar", func2: "loadCalendar", hidesave: true });
    });
});

function succesLoadMediosPago(data, obj) {
    $("#btnCancelGeneric").show();
    $("#btnSaveGeneric").show();
    $("#btnVolverGeneric").hide();
    $("#btnImprimirGeneric").hide();
    $("#btnSaveGeneric").data("url", obj.url);
    $("#btnSaveGeneric").data("metod", obj.metod);
    $("#btnSaveGeneric").data("function", obj.func);
    $("#btnCancelGeneric").click(function () {
        cerrarModal('modalCRUD');
    });
    $("#btnVolverGeneric").click(function () {
        cerrarModal('modalCRUD');
    });

    $("#modalCRUD .modal-dialog").removeClass("modal-lg");
    //$("#modalCRUD .modal-dialog").addClass("modal-lg");
    $("#modalCRUD .modal-title").html(obj.title);
    $("#modalCRUD .modal-body").html(data);

    $("#btnCancelGeneric").hide();
    $("#btnSaveGeneric").hide();
    $("#btnVolverGeneric").hide();

    setNumeric();
    abrirModal("modalCRUD");

    var total = 0;
    var sHTML = '';
    if (listaDetallePago.length > 0) {
        $.each(listaDetallePago, function (i, item) {
            total += item.Valor;
            sHTML += '<tr>';
            sHTML += '     <td>' + item.DescMedioPago + '</td>';
            sHTML += '     <td>' + item.NumReferencia + '</td>';
            sHTML += '     <td align="right">' + item.Valor + '</td>';
            sHTML += '     <td align="center"><a data-id="' + i + '" class="evtEliminarMedioPago"><span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
            sHTML += '</tr>';

        });
    }
    $("#tableBodyPagos").append(sHTML);
    //$("#spanTotalMedios").html("Total: $" + total);
    event.preventDefault();
}

//Cambioquitar
function selectMedioPagoChange() {
    var IdMedioPago = $("#selectMedioPago").val();

    if (IdMedioPago == parametros.IdMedioPagoEfectivo || IdMedioPago == "") {
        $("#colReferencia").fadeOut("fast");
    } else {
        $("#colReferencia").fadeIn("fast");
    }

    //if (IdMedioPago == parametros.IdMedioPagoTarjetaDebito || IdMedioPago == parametros.IdMedioPagoTarjetaCredito) {
    //    $("#colFranquicia").fadeIn("fast");
    //} else {
    //    $("#colFranquicia").fadeOut("fast");
    //}

    if ((IdMedioPago == parametros.IdMedioPagoTarjetaDebito || IdMedioPago == parametros.IdMedioPagoTarjetaCredito || IdMedioPago == "14")) {
        if (transaccionRedebanHabilitada == "0" || IdMedioPago == "14") {
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
    }


    if (IdMedioPago == parametros.IdMedioPagoTarjetaRecargable) {
        $("#inputValorPago").val($("#total h1").html().replace("$", ""));
    }
    else {
        $("#inputValorPago").val("");
    }
    $("#selectFranquicia").val("");
    $("#inputReferencia").val("");
}

function AgregarOpcionTarjetaManual() {
    var select = document.getElementById("selectMedioPago");
    var banderase = 0;
    for (var i = 0; i < select.length; ++i) {
        if (select.options[i].value == '14') {
            banderase = 1;
        }
    }
    if (banderase == 0) {
        var option1 = document.createElement("option");
        option1.value = "14";
        option1.text = "Tarjetas Opcion Manual";
        select.add(option1);
    }
}
//Valida que se selecciono medio de pago
//Valida que hay valor y # referencia
function AgregarPago() {
    var resp = true;
    var _documento = "";
    var franquiciaTarjeta = "";
    var _numeroRerenciaTarjeta = "";
    var numeroRecibo = "";
    var IdMedioPago = $("#selectMedioPago").val();
    QuitarTooltip();

    $("#selectMedioPago").removeClass("errorValidate");
    $("#inputValorPago").removeClass("errorValidate");

    //Validacion transaccion redeban activa para obligatoriedad de los campos
    if (IdMedioPago == "2" || IdMedioPago == "14") {

        if (transaccionRedebanHabilitada == "0" || IdMedioPago == "14") {

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
        resp = false;
    }


    if ($("#inputValorPago").val() == "" || $("#inputValorPago").val() == "0") {
        $("#inputValorPago").attr("data-mensajeerror", "Este campo es obligatorio");
        $("#inputValorPago").addClass("errorValidate");
        resp = false;
    }

    if (!resp) {
        mostrarTooltip();
        MostrarMensaje("Importante", "Por favor seleccione un medio de pago", "error");
        return false;
    }

    if (IdMedioPago == parametros.IdMedioPagoBonoRegalo) {

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
            || $("#inputReferencia").val() != "" || ($("#inputReferencia").val() == "" && IdMedioPago == "2" && transaccionRedebanHabilitada == "1")) {

            //Cambioquitar
            if ((IdMedioPago == parametros.IdMedioPagoTarjetaDebito || IdMedioPago == parametros.IdMedioPagoTarjetaCredito || IdMedioPago == "14") && ((transaccionRedebanHabilitada == "0") || IdMedioPago == "14")) {
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

            if ((IdMedioPago == parametros.IdMedioPagoTarjetaDebito || IdMedioPago == "14") && ((transaccionRedebanHabilitada == "0") || IdMedioPago == "14")) {
                if (listaDetallePago.length > 0) {
                    $.each(listaDetallePago, function (i, item) {
                        if (item.IdMedioPago == parametros.IdMedioPagoTarjetaDebito || item.IdMedioPago == parametros.IdMedioPagoTarjetaDebito.toString() || IdMedioPago == "14") {
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
            if (IdMedioPago == "2" && transaccionRedebanHabilitada == "1") {
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
                            console.log(respuesta);
                            if (respuesta.MensajeRespuesta != "OK" && respuesta.CodigoRespuesta != "00") {
                                resultadoTransaccionRedebanExitosa = false;

                                AgregarOpcionTarjetaManual();

                                MostrarMensajeHTML("Importante", respuesta.MensajeRespuesta.toString(), "error");
                                return false;
                            }
                            else {
                                resultadoTransaccionRedebanExitosa = true;
                                _numeroRerenciaTarjeta = respuesta.NumeroAprobacion;
                                franquiciaTarjeta = respuesta.Franquicia;
                                numeroRecibo = respuesta.NumeroRecibo;
                                IdFranquiciaRedeban = respuesta.IdFranquicia;
                                if (_numeroRerenciaTarjeta == "" || _numeroRerenciaTarjeta == null || numeroRecibo == null || numeroRecibo == "") {

                                    AgregarOpcionTarjetaManual();
                                }
                                MostrarMensajeHTML("Importante", "Transacción exitosa. <br /> Conexión Aprobada", "success");
                            }

                        }
                    },
                    error: function (error) {

                        AgregarOpcionTarjetaManual();


                        MostrarMensajeHTML("Importante", "Error conectando con Redeban. <br /> Por favor intente nuevamente", "error");
                        console.log(error)
                        return false;
                    }
                });
            }

            if (!hayPagoEfectivoAdd) {
                if (transaccionRedebanHabilitada == "1" && IdMedioPago == "2" && resultadoTransaccionRedebanExitosa == true) {
                    console.log("1");
                    var ObjectPago = new Object();
                    ObjectPago.IdMedioPago = parseInt($("#selectMedioPago").val());
                    ObjectPago.DescMedioPago = $("#selectMedioPago option:selected").text();
                    ObjectPago.NumeroRecibo = numeroRecibo;
                    ObjectPago.NumReferencia = _numeroRerenciaTarjeta;
                    ObjectPago.IdFranquicia = IdFranquiciaRedeban;
                    ObjectPago.DescFranquicia = franquiciaTarjeta;
                    ObjectPago.Valor = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                    if (IdMedioPago == parametros.IdMedioPagoDescuentoNomina)
                        ObjectPago.NombreMedioPago = empleadoNominaSeleccionado.Nombres + ' ' + empleadoNominaSeleccionado.Apellidos;
                    listaDetallePago.push(ObjectPago);
                }
                else if ((IdMedioPago == "2" && transaccionRedebanHabilitada == "0") || IdMedioPago == "14") {
                    console.log("2");
                    var ObjectPago = new Object();
                    ObjectPago.IdMedioPago = parseInt(2);
                    ObjectPago.DescMedioPago = $("#selectMedioPago option:selected").text();
                    ObjectPago.NumReferencia = _documento;
                    ObjectPago.NumeroRecibo = "N/A";
                    ObjectPago.IdFranquicia = $("#selectFranquicia").val();
                    ObjectPago.DescFranquicia = ($("#selectFranquicia").val() == "" ? "" : $("#selectFranquicia option:selected").text());
                    ObjectPago.Valor = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                    if (IdMedioPago == parametros.IdMedioPagoDescuentoNomina)
                        ObjectPago.NombreMedioPago = empleadoNominaSeleccionado.Nombres + ' ' + empleadoNominaSeleccionado.Apellidos;
                    listaDetallePago.push(ObjectPago);
                }

                else if (IdMedioPago != "2") {
                    console.log("3");
                    var ObjectPago = new Object();
                    ObjectPago.IdMedioPago = parseInt($("#selectMedioPago").val());
                    ObjectPago.DescMedioPago = $("#selectMedioPago option:selected").text();
                    ObjectPago.NumReferencia = _documento;
                    ObjectPago.NumeroRecibo = "N/A";
                    ObjectPago.IdFranquicia = $("#selectFranquicia").val();
                    ObjectPago.DescFranquicia = ($("#selectFranquicia").val() == "" ? "" : $("#selectFranquicia option:selected").text());
                    ObjectPago.Valor = parseInt($("#inputValorPago").val().replace(".", "").replace(".", ""));
                    if (IdMedioPago == parametros.IdMedioPagoDescuentoNomina)
                        ObjectPago.NombreMedioPago = empleadoNominaSeleccionado.Nombres + ' ' + empleadoNominaSeleccionado.Apellidos;
                    listaDetallePago.push(ObjectPago);
                }
            }

            var sHTML = '';
            $("#tableBodyPagos").html(sHTML);
            if (listaDetallePago.length > 0) {
                $.each(listaDetallePago, function (i, item) {
                    var valorArgumentosFuncion = i + ",\'" + item.NumeroRecibo + "\',\'" + item.NumReferencia + "\'";
                    total += item.Valor;
                    sHTML += '<tr>';
                    sHTML += '     <td>' + item.DescMedioPago + '</td>';
                    sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" && item.IdMedioPago != "2" ? "N/A" : item.NumeroRecibo) + '</td>';
                    sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" && item.IdMedioPago != "2" ? "N/A" : item.NumReferencia) + '</td>';
                    sHTML += '     <td>' + (item.DescMedioPago != "Tarjetas" && item.IdMedioPago != "2" ? "N/A" : item.DescFranquicia) + '</td>';
                    sHTML += '     <td align="right">' + item.Valor + '</td>';
                    sHTML += '     <td align="center"><a onclick="evtEliminarMedioPago(' + valorArgumentosFuncion + ' );"><span class="fa fa-trash-o IconosPos" aria-hidden="true"></span></a></td>';
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
            $("#tableBodyPagos").append(sHTML);

            $("#selectMedioPago").val("");
            $("#selectFranquicia").val("");
            $("#inputReferencia").val("");
            $("#inputValorPago").val("");

            selectMedioPagoChange();
            calculaTotal();


        } else {

            if (($("#inputReferencia").val() == "" && IdMedioPago != "1")) {

                if (transaccionRedebanHabilitada == "1" && IdMedioPago == "2") {
                }
                else {
                    $("#inputReferencia").attr("data-mensajeerror", "Este campo es obligatorio");
                    $("#inputReferencia").addClass("errorValidate");
                }
            }
        }

        //finalizarProceso();

    }
    // event.preventDefault();
}

//function iniciarProceso() {
//    $(".loader-wrapper").css("display", "block");
//    //Reinicia el contador de finalizar sesion cada vez que se haga una peticion ajax.
//    ReiniciarControlSesion();
//}

//function finalizarProceso() {
//    $(".loader-wrapper").css("display", "none");
//}

//function ReiniciarControlSesion() {
//    $('#control').val("0");
//}

//Permite la eliminación de un medio de pago de tarjeta por redeban
function CorfimarSupervisorRedeban() {
    $('#myModalConfirmarSupervisorRedeban').modal('show');
}

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
                    if (listaDetallePago.length == 1)
                        listaDetallePago = [];
                    else {
                        $.each(listaDetallePago, function (i) {
                            if (i == indexMedioPagoEliminar) {
                                listaDetallePago.splice(i, 1);
                                return false;
                            }
                        });
                    }

                    calculaTotal();
                    var sHTML = '';
                    $("#tableBodyPagos").html(sHTML);
                    if (listaDetallePago.length > 0) {
                        $.each(listaDetallePago, function (i, item) {
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


function LimpiarControles() {
    $('#HoraInicial').val("");
    $("#DDL_Punto").attr('disabled', false);
    $("#btnBuscar").attr('disabled', false);
}


function evtEliminarMedioPago(indexMedioPago, numeroRecibo, numeroReferencia) {

    numeroFacturaEliminar = numeroReferencia;
    numeroReciboEliminar = numeroRecibo;
    indexMedioPagoEliminar = indexMedioPago;

    if (listaDetallePago[indexMedioPago].IdMedioPago === 9) {
        $("#divNombreCliente").html("");
        $("#snapshot").attr("src", "");
        $("#snapshot").hide();
    }

    if (listaDetallePago[indexMedioPago].IdMedioPago === 2 && transaccionRedebanHabilitada == "1") {
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

    calculaTotal();
    var sHTML = '';
    $("#tableBodyPagos").html(sHTML);
    if (listaDetallePago.length > 0) {
        $.each(listaDetallePago, function (i, item) {
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
    }
    $("#tableBodyPagos").append(sHTML);

    indexMedioPagoEliminar = 0;

}

function calculaTotal() {
    var total = 0;
    if (listaDetallePago.length > 0) {
        $.each(listaDetallePago, function (i, item) {
            total += item.Valor;
        });
    }

    $("#TotalPagado").html("$" + EnMascarar(total));
    MostrarcambioApp();
}
/*******************************************/
/*Pago de parqueadero**/
$("#linkPagoParqueadero").click(function () {
    EjecutarAjax(urlBase + "Pos/PagoParqueadero", "GET", null, "printPartialModal", { title: "Pago de Parqueadero", hidesave: true, modalLarge: false });
});

function calculaPagoParqueadero() {
    if (($("#inputCodigoParqueadero").val() == "" || $("#inputCodigoParqueadero").val() == "0" || $("#inputCodigoParqueadero").val().length <= 3)) {
        inicializadointervalPark = false;
        $("#inputCodigoParqueadero").val("");
    }
    else {
        if ($("#inputCodigoParqueadero").val().length >= 10) {
            var strCodBarras = $("#inputCodigoParqueadero").val();
            var idDepurado = strCodBarras.substring(2, strCodBarras.length - 1);
            _idParqueadero = parseInt(idDepurado);
            var yaFueAgregado = false;
            if (lstProductosCompra.length > 0) {
                $.each(lstProductosCompra, function (i, item) {
                    if (_idParqueadero == item.IdDetalleProducto) {
                        yaFueAgregado = true;
                    }
                });
            }

            if (yaFueAgregado) {
                MostrarMensajeRedireccion("Importante", "El pago ya fue agregado en la lista de productos.", null, "warning");
                $("#inputCodigoParqueadero").val("");
                inicializadointervalPark = false;
            } else {
                EjecutarAjax(urlBase + "Pos/PagoParqueadero", "POST", JSON.stringify({ idParqueadero: _idParqueadero }), "SuccessParqueaderoPago", null);
            }
        } else {
            $("#inputCodigoParqueadero").val("");
            inicializadointervalPark = false;
        }
    }
}

function SuccessParqueaderoPago(data) {
    if (data.Correcto) {
        var objControlParqueadero = JSON.parse(data.Elemento);
        $("#IdControlPagoParquedero").val(objControlParqueadero.Id);
        $("#ValorPagoParquedero").val(objControlParqueadero.ValorPago);
        AdicionarPagoParqueadero();
    }
    else {
        $("#IdControlPagoParquedero").val("");
        $("#ValorPagoParquedero").val("");
        MostrarMensajeRedireccion("Importante", data.Mensaje, null, "warning");
    }
    $("#inputCodigoParqueadero").val("");
    inicializadointervalPark = false;
}
function AdicionarPagoParqueadero() {
    var valorcupo = $("#ValorPagoParquedero");
    var valorcupo = valorcupo.val();
    if (valorcupo == "") {
        mostrarAlerta("No hay Valor registrado.");
        return;
    } else {
        $("#inputCodigoReservaParqueadero").val("");
        $("#inputCodigoReservaParqueadero").removeAttr("disabled");

        var r = ObtenerproductoPorCodigoSap(parametros.CodSapProdParqueadero.Valor);//PARQUEADERO Cambioquitar
        if (TieneCodigoReservaParqueadero)
            r.Precio = 0;
        else
            r.Precio = $("#ValorPagoParquedero").val().replace(".", "").replace(".", "");

        r.IdDetalleProducto = $("#IdControlPagoParquedero").val();
        r.Cantidad = 1;
        r.PrecioTotal = r.Precio;
        lstProductosCompra.push(r);
        ActualizarTablaCompras();
        MostrarcambioApp();

        cerrarModal("modalCRUD");
    }
}

function PagoReimprimirParqueadero() {
    var valorcupo = $("#placaReimprimir");

    valorcupo.removeClass("errorValidate");
    var valorcupo = valorcupo.val();
    if (valorcupo == "") {
        $("#placaReimprimir").attr("data-mensaje-error", "Este campo es obligatorio");
        $("#placaReimprimir").addClass("errorValidate");
        return;
    } else {
        var r = ObtenerproductoPorCodigoSap(parametros.CodSapProdReimpresionPq.Valor); // REIMPRESION PARQUEADERO Cambioquitar
        r.Precio = $("#placaReimprimir").val().replace(".", "").replace(".", "");
        r.Cantidad = 1;
        r.PrecioTotal = r.Precio;
        lstProductosCompra.push(r);
        ActualizarTablaCompras();
        MostrarcambioApp();
        $("#placaReimprimir").prop('disabled', true);
    }
}
/*******************************************/
//Ocultar y mostrar productos
$(function () {
    $(".txtBuscador").keyup(function () {
        var texto = $(this).val();
        if ((texto.length <= 3) && (isInt(texto)))
            texto = "# " + texto;

        $.each($(".ProductoGrilla"), function (i, v) {
            if ($(v).html().toLowerCase().indexOf(texto.toLowerCase()) > -1)
                $(v).show()
            else
                $(v).hide()
        })
    });
});

//fin 
function isInt(value) {
    var er = /^-?[0-9]+$/;
    return er.test(value);
}

function ValidaProductoAgregadoConvenio(indiceValidar, itemValidar) {
    var agregado = false;
    try {
        if (lstProductosCompra.length > 0) {
            $.each(lstProductosCompra, function (i, item) {
                if (!(i === indiceValidar)) {
                    if (item.CodigoSap == itemValidar.CodigoSap && item.CodSapTipoProducto == itemValidar.CodSapTipoProducto) {
                        if (item.EsConvenio == null) {
                            /*agregado = false;*/
                        } else {
                            if (item.EsConvenio) {
                                agregado = true;
                                return true;
                            }
                        }
                    }
                }
            });
        }
    }
    catch (err) {
    }

    return agregado;
}

function ActualizarListaDePrecios() {      
    if (lstProductosCompra.length > 0) {
        $.each(lstProductosCompra, function (i, item) {
            var tieneprecioConvenio = false;

            var objDetalleConvenio = listDetalleConvenioSAPGetByCodSap(item.CodSapTipoProducto, item.CodigoSap)

            var objProducto = ListaTodosProductosSAPGetByCodSap(item.CodSapTipoProducto, item.CodigoSap)
            if (objDetalleConvenio != null) {

                var objProductoNm = ListaTodosProductosSAPGetByCodSap(item.CodSapTipoProducto, item.CodigoSap)
                if (objProductoNm != null) {

                    ////Cambioquitar
                    if (item.CodigoSap == parametros.CodSapCupoDebito.Valor) {
                        //Si es cupo debitoo no debe montar el precio
                    } else if (item.CodigoSap == parametros.CodSapProdParqueadero.Valor) {
                        //Si es Parqueadero no debe montar el precio pues esta en 0
                    } else {

                        if (objDetalleConvenio.IdConvenioDetalle == -2) {
                            if (ValidaProductoAgregadoConvenio(i, item)) {
                                /* Si el producto IdConvenioDetalle == -2 ya fue agregado como convenio no lo debe permitir agregar mas */
                                /* Los otros productos que se agreguen deben ir por preio full */
                            } else {
                                item.Nombre = objProductoNm.Nombre + ' ***';
                                item.Precio = objDetalleConvenio.Valor;
                                item.PrecioTotal = (item.Precio * item.Cantidad);
                                item.EsConvenio = true;
                                tieneprecioConvenio = true;
                            }
                        } else {
                            item.Nombre = objProductoNm.Nombre + ' **';
                            if (item.esMayor && numeroPasaportesDia < 1) {
                                item.Precio = objDetalleConvenio.Valor;
                            }
                            else if (item.CodSapTipoProducto != "2000" && item.CodSapTipoProducto != "2005") {
                                item.Precio = objDetalleConvenio.Valor;
                            }
                            else if (!esMejoraCortesia && (item.CodSapTipoProducto == "2000" || item.CodSapTipoProducto == "2005")) {
                                item.Precio = objProductoNm.Precio;
                            }
                            else if (item.Consecutivo == 0 ){                            
                                item.Precio = objDetalleConvenio.Valor;
                            }
                            else {                                
                                item.Precio = objProducto.Precio;
                            }
                            //item.Precio = objDetalleConvenio.Valor;
                            item.PrecioTotal = (item.Precio * item.Cantidad);
                            item.EsConvenio = true;
                            tieneprecioConvenio = true;
                        }

                    }
                }
            }

            if (!tieneprecioConvenio) {
                //var objProducto = ListaTodosProductosSAPGetByCodSap(item.CodSapTipoProducto, item.CodigoSap)
                if (objProducto != null) {
                    item.EsConvenio = false;
                    //Cambioquitar
                    if (item.CodigoSap == parametros.CodSapCupoDebito.Valor || item.CodigoSap == parametros.CodSapRecargaTarjeta.Valor) {
                        //Si es cupo debito o recarga de tarjeta no debe montar el precio
                    } else if (item.CodigoSap == parametros.CodSapProdParqueadero.Valor) {
                        //Si es Parqueadero no debe montar el precio pues esta en 0
                    } else {
                        item.Nombre = objProducto.Nombre;
                        item.Precio = objProducto.Precio;
                        item.PrecioTotal = (item.Precio * item.Cantidad);
                    }

                }
            }

        });
    }

}

function listDetalleConvenioSAPGetByCodSap(CodSapTipoProducto, CodigoSapProducto) {
    var returnObject = null;
    $.each(listDetalleConvenio, function (i, item) {
        if (item.CodSapTipoProducto == CodSapTipoProducto && item.CodSapProducto == CodigoSapProducto) {
            returnObject = JSON.parse(JSON.stringify(item));
        }
    });
    return returnObject;
}

function ListaTodosProductosSAPGetByCodSap(CodSapTipoProducto, CodigoSapProducto) {
    var returnObject = null;
    $.each(ListaTodosProductosSAP, function (i, item) {
        if (item.CodSapTipoProducto == CodSapTipoProducto && item.CodigoSap == CodigoSapProducto) {
            returnObject = JSON.parse(JSON.stringify(item));
        }
    });
    return returnObject;
}

//Se obtiene el nombre con el id del convenio EDSP: 03/01/2018
function listDetalleConvenioSAPGetNombreConvenio() {
    var IdConvenio = 0;
    var NombreConvenio = "";
    $.each(listDetalleConvenio, function (i, item) {
        IdConvenio = item.IdConvenio;
        return false;
    });
    $.each(ListaConveniosCodigoBarras, function (i, item) {
        if (item.IdConvenio == IdConvenio) {
            NombreConvenio = item.Nombre;
            return false;
        }
    });
    return NombreConvenio;
}

function listDetalleConvenioSAPGetCodSapConvenio() {
    var CodSapConvenio = "";
    $.each(listDetalleConvenio, function (i, item) {
        CodSapConvenio = item.CodSapConvenio;
        return false;
    });
    return CodSapConvenio;
}

function listDetalleConvenioSAPGetIdConvenio() {
    var idConvenio = 0;
    $.each(listDetalleConvenio, function (i, item) {
        idConvenio = item.IdConvenio;
        return false;
    });
    return idConvenio;
}

function ListaTodosProductosSAPGetById(Id) {
    var returnObject = null;
    $.each(ListaTodosProductosSAP, function (i, item) {
        if (item.IdProducto == Id) {
            returnObject = JSON.parse(JSON.stringify(item));
        }
    });
    return returnObject;
}

function ObtenerproductoPorCodigoSap(CodSap) {
    var returnObject = null;
    $.each(ListaTodosProductosSAP, function (i, item) {
        if (item.CodigoSap == CodSap) {
            returnObject = JSON.parse(JSON.stringify(item));
        }
    });
    return returnObject;
}

//RDSH: Actualiza la tabla de los productos que el cliente ha comprado.
function ActualizarTablaCompras() {
    //Aqui determino el pasaporte con mayor valor    
    var mayorPrecio = 0;
    var ContI = 0;
    var ContMayor = 0;
    var encontrado = false;
    $.each(lstProductosCompra, function () {
        //var IdProducto = item.IdDetalleProducto == null || item.IdDetalleProducto === undefined ? "0" : item.IdDetalleProducto;
        if (lstProductosCompra[ContI].Precio > mayorPrecio && CodigoBarrasConvenioPistoleadedo != ""
            && (lstProductosCompra[ContI].CodSapTipoProducto == "2000" || lstProductosCompra[ContI].CodSapTipoProducto == "2005")) {
            mayorPrecio = lstProductosCompra[ContI].Precio;
            ContMayor = ContI;
            encontrado = true;
        }
        lstProductosCompra[ContI].esMayor = false;
        ContI++;
    });
    if (encontrado) {
        lstProductosCompra[ContMayor].esMayor = true;
    }
    ActualizarListaDePrecios();
    MostrarPropina();

    MostrarProductos();

    
}

//RDSH: Funcion para agregar un producto a la lista de compras.
function AgregarProductoACompra(IdProducto, tmpList) {

    index = IdProducto;
    if (tmpList != null) {

        if (lstProductosCompra.length > 0) {
            //Si la lista de productos tiene algun producto, se busca si ese producto que se esta agregando ya existe en la lista de compras.
            //Si ya existe, aumenta la cantidad en 1.
            var Producto = ObtenerProducto(lstProductosCompra, IdProducto);
            if (Producto.length > 0) {
                if (Producto[0].CodigoSap === parametros.CodSapClienteFan.Valor && contingencia === 1) {
                    MostrarMensajeRedireccion("Importante", "No se puede vender este producto en contingencia", null, "warning");
                    return false;
                }
            }
            //if (Producto.length > 0 && Producto[0].CodigoSap === parametros.CodSapClienteFan.Valor) {
            //    MostrarMensajeRedireccion("Importante", "Solo puede agregar una tarjeta FAN por factura", null, "warning");
            //    return false;
            //}
            if (Producto.length > 0 && !validaProductoAgrupaCantidad(Producto[0].CodSapTipoProducto, Producto[0].CodigoSap)) {
                ActualizarCantidadProducto(IdProducto, (Producto[0].Cantidad + 1))
            }
            else {
                var existeDonacion = false;
                var prodDonacionNuevo = false;
                var ProductoSeleccionado = ObtenerProducto(tmpList, IdProducto);
                var prodDonacion = $("#inputCodSapProductosDonaciones").val().split(',')
                for (var i = 0; i < prodDonacion.length; i++) {
                    if (ObtenerProductoCodSap(lstProductosCompra, prodDonacion[i]).length > 0) {
                        existeDonacion = true;
                    }
                    if (ProductoSeleccionado[0].CodigoSap === prodDonacion[i]) {
                        prodDonacionNuevo = true;
                    }

                }
                //if (existeDonacion || prodDonacionNuevo) {
                //    if (existeDonacion === prodDonacionNuevo) {
                //        ProductoSeleccionado[0].Cantidad = 1;
                //        ProductoSeleccionado[0].PrecioTotal = ProductoSeleccionado[0].Precio;
                //        lstProductosCompra.push(ProductoSeleccionado[0]);
                //    } else {
                //        MostrarMensajeRedireccion("Importante", "No es posible mezclar productos de donación con productos de otra categoría", null, "warning");
                //    }
                //} 
                //else {

                if (ValidarRecargaAdicionproducto(ProductoSeleccionado[0].CodigoSap)) {
                    if (ProductoSeleccionado[0].CodigoSap == parametros.CodSapClienteFan.Valor) {
                        MostrarModalClienteFan(IdProducto);
                    }
                    else if (ProductoSeleccionado[0].CodigoSap == parametros.CodSapReposicionTarjeta.Valor) {
                        MostrarModalReposicion(IdProducto);
                    }
                    else {
                        ProductoSeleccionado[0].Cantidad = 1;
                        ProductoSeleccionado[0].PrecioTotal = ProductoSeleccionado[0].Precio;
                        lstProductosCompra.push(ProductoSeleccionado[0]);
                    }
                }

                //}

            }
        }
        else {
            var ProductoSeleccionado = ObtenerProducto(tmpList, IdProducto);
            if (ProductoSeleccionado[0].CodigoSap === parametros.CodSapClienteFan.Valor && contingencia === 1) {
                MostrarMensajeRedireccion("Importante", "No se puede vender este producto en contingencia", null, "warning");
                return false;
            }
            if (ProductoSeleccionado[0].CodigoSap == parametros.CodSapClienteFan.Valor) {
                MostrarModalClienteFan(IdProducto);
            }
            else if (ProductoSeleccionado[0].CodigoSap == parametros.CodSapReposicionTarjeta.Valor) {
                MostrarModalReposicion(IdProducto);
            }
            else {
                ProductoSeleccionado[0].Cantidad = 1;
                ProductoSeleccionado[0].PrecioTotal = ProductoSeleccionado[0].Precio;
                lstProductosCompra.push(ProductoSeleccionado[0]);
            }
        }

        ActualizarTablaCompras();
        MostrarcambioApp();
    }
}

function ValidarRecargaAdicionproducto(ProductoSeleccionado) {
    var prodrecarga = ProductoSeleccionado == parametros.CodSapRecargaTarjeta.Valor;
    var existeprodLista = ObtenerProductoCodSap(lstProductosCompra, parametros.CodSapRecargaTarjeta.Valor).length > 0;
    var listrecarga = lstProductosCompra.length > 0;
    //if ((listrecarga && !existeprodLista && prodrecarga) || (listrecarga && existeprodLista && !prodrecarga)){
    //    MostrarMensajeRedireccion("Importante", "No es posible mezclar recargas de tarjetas con otros productos", null, "warning");
    //    return false;
    //}
    //else 
    return true;
}

function MostrarModalClienteFan(idproducto) {    
    $("#hdIdProd").val(idproducto);
    LimpiarFan();
    abrirModal("modalFAN");
}

function MostrarModalReposicion(idproducto) {
    LimpiarRepo();
    $("#hdIdProdRepo").val(idproducto);
    abrirModal("modalRepo");
}


// Object { foo: "foo", bar: "bar" } 

//RDSH: Obtiene un producto por su id, busca en el array que se le envie en el parametro Arreglo.
function ObtenerProducto(Arreglo, IdProducto) {
    var result = null;
    $.each(Arreglo, function (i, item) {
        if (item.IdProducto == IdProducto) {
            result = JSON.parse(JSON.stringify(item));
        }
    });
    if (result !== null)
        return [result]
    return [];
}
function ObtenerProductoCodSap(Arreglo, IdProducto) {
    var result = null;
    $.each(Arreglo, function (i, item) {
        if (item.CodigoSap == IdProducto) {
            result = JSON.parse(JSON.stringify(item));
        }
    });
    if (result !== null)
        return [result]
    return [];
}
//RDSH: Actualiza la cantidad y el precio total de un producto cuando este existe en el la coleccion de productos.
function ActualizarCantidadProducto(IdProducto, Cantidad) {
    var index = -1;
    lstProductosCompra.some(function (el, i) {
        if (el.IdProducto == IdProducto) {
            index = i;
            lstProductosCompra[index].Cantidad = Cantidad;
            lstProductosCompra[index].PrecioTotal = (lstProductosCompra[index].Precio * Cantidad);
            return true;
        }
    });
}

/*Este metodo valida si un producto debe agrupar cantidades en el POS.
Primero Valida por Producto, despues por linea
*/
function validaProductoAgrupaCantidad(CodSapTipoProducto, CodigoSapProducto) {
    var retvalidaProductoAgrupaCantidad = false;

    var arrayCodSapProductoAgrupaCantidad = $("#inputCodSapProductoAgrupaCantidad").val().split(",");
    if (arrayCodSapProductoAgrupaCantidad.length > 0) {
        for (i = 0; i < arrayCodSapProductoAgrupaCantidad.length; i++) {
            if (CodigoSapProducto === arrayCodSapProductoAgrupaCantidad[i]) {
                retvalidaProductoAgrupaCantidad = true;
            }
        }
    }

    if (!retvalidaProductoAgrupaCantidad) {
        var arrayCodSapTipoProductoAgrupaCantidad = $("#inputCodSapTipoProductoAgrupaCantidad").val().split(",");
        if (arrayCodSapTipoProductoAgrupaCantidad.length > 0) {
            for (i = 0; i < arrayCodSapTipoProductoAgrupaCantidad.length; i++) {
                if (CodSapTipoProducto === arrayCodSapTipoProductoAgrupaCantidad[i]) {
                    retvalidaProductoAgrupaCantidad = true;
                }
            }
        }
    }
    return retvalidaProductoAgrupaCantidad;
}

//Valida los productos que no aplican para el descargue
//Cambioquitar
function ValidaListaSINO(codigo) {
    var result = false;
    //if ((codigo == '2000')
    //    || (codigo == '2020')
    //    || (codigo == '2035')
    //    || (codigo == '2005')
    //    || (codigo == '2010')
    //    || (codigo == '2015')
    //    || (codigo == '2030')
    //    || (codigo == '2040')) {
    //    result = true;
    //}

    var list = parametros.CodSapTipoProdDescargue.Valor.split(',');
    $.each(list, function (i, item) {
        if (item == codigo.CodSapTipoProducto)
            result = true;
        else if (codigo.CodigoSap == parametros.CodSapRecargaTarjeta.Valor)
            result = true;
    });
    if (parametros.AplicaDescargue)
        result = true;

    return result;
}

//Enmascarar precio de productos en la grilla de productos 
$.each($(".precioPos"), function (idObj, objetoProducto) {
    var precio = $(objetoProducto).html();
    var _precioMascara = EnMascarar(parseInt($.trim(precio)));
    $(objetoProducto).html("$ " + _precioMascara);
})

/* INI - Convenios */
$("#selectPosConvenioSAP").change(function () {
    if ($("#selectPosConvenioSAP").val() == "") {
        $("#inputConvenioCodBarrasSAP").removeAttr("disabled");
        listDetalleConvenio = [];
        ActualizarTablaCompras();
    } else {
        EjecutarAjax(urlBase + "Pos/ObtenerDetalleConvenio", "GET", { IdConvenio: $("#selectPosConvenioSAP").val() }, "successObtenerDetalleConvenio", null);

        //Aqui llamo la ventana emrgente para redimir las cortesias 
        var prueba = $("#selectPosConvenioSAP").val();
        if ($("#selectPosConvenioSAP").val() == parametros.MejoraUbin.Valor) {
            esMejoraCortesia = true;
            MostrarModalRedencionCortesia();
            ActualizarTablaCortesias();
        }        

    }
});

function successObtenerDetalleConvenio(rta) {
    
    if (rta.length > 0) {
        listDetalleConvenio = rta;
        ActualizarTablaCompras();
        $("#inputConvenioCodBarrasSAP").attr("disabled", "disabled");

    } else {
        listDetalleConvenio = [];
        ActualizarTablaCompras();
        //$("#inputConvenioCodBarrasSAP").attr("disabled", false);
    }
}

$("#inputCancelselectPosConvenioSAP").click(function (evt) {
    if ($("#inputConvenioCodBarrasSAP").val() == "") {
        var $selectPosConvenioSAP = $("#selectPosConvenioSAP").select2();
        $selectPosConvenioSAP.val('').trigger("change");
        $("#selectPosConvenioSAP").select2({
            placeholder: "Seleccione el convenio"
        });
        $("#inputConvenioCodBarrasSAP").removeAttr("disabled");
    }
});

$("#inputConvenioCodBarrasSAP").keypress(function (evt) {
    if (window.event) {
        var charCode = event.keyCode;
    } else if (evt.onkeypress.arguments[0].charCode) {
        var charCode = evt.onkeypress.arguments[0].charCode;
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    if (!inicializadointervalConvenio) {
        inicializadointervalConvenio = true;
        numeroPasaportesDia = 0;
        tarjetaVencida = false;
        var refreshIntervalIdConvenio = setInterval(function () { fnValidaCodSapConvenio(); clearInterval(refreshIntervalIdConvenio); }, 300);
    }
});

function fnValidaCodSapConvenio() {
    if ($("#inputConvenioCodBarrasSAP").val() == "") {
        inicializadointervalConvenio = false;
        CodigoBarrasConvenioPistoleadedo = "";
    } else {
        CodigoBarrasConvenioPistoleadedo = $("#inputConvenioCodBarrasSAP").val();
        EjecutarAjax(urlBase + "Pos/ConsultarVencimientoTarjeta", "GET", { Tarjeta: $("#inputConvenioCodBarrasSAP").val() }, "successObtenerVencimientoTarjeta", null);
        EjecutarAjax(urlBase + "Pos/ObtenerDetallesConsecutivoConvenioDia", "GET", { Consecutivo: $("#inputConvenioCodBarrasSAP").val() }, "successObtenerDetalleConsecutivoDia", null);
        EjecutarAjax(urlBase + "Pos/ObtenerDetalleConvenioByConsec", "GET", { Consecutivo: $("#inputConvenioCodBarrasSAP").val() }, "successObtenerDetalleConvenioCodigoSAP", null);
    }
}

function successObtenerDetalleConvenioCodigoSAP(rta) {
    debugger;
    if (rta.resultado.length > 0 && tarjetaVencida == false) {
        listDetalleConvenio = rta.resultado;
        ActualizarTablaCompras();
        var nombreConvenio = listDetalleConvenioSAPGetNombreConvenio();
        $("#inputConvenioCodBarrasSAP").val(nombreConvenio);
        $("#inputConvenioCodBarrasSAP").attr("disabled", "disabled");
        $("#selectPosConvenioSAP").attr("disabled", "disabled");

        var listaProductosObsequio = "";
        $.each(listDetalleConvenio, function (i, item) {
            if (item.IdConvenioDetalle == -2) {
                var objProductoNm = ListaTodosProductosSAPGetByCodSap(item.CodSapTipoProducto, item.CodSapProducto);
                if (objProductoNm != null) {
                    listaProductosObsequio = listaProductosObsequio + objProductoNm.Nombre + " - " + item.Cantidad + " " + "<br />";
                }
            }
        });
        //Muestra popup si aplica sortsia de cumpleanos
        if (listaProductosObsequio != "") {
            MostrarMensajeHTML("Importante", "Cliente por ser fecha especial reclama obsequio: <br /> " + listaProductosObsequio, "success");
        }

    } else {
        listDetalleConvenio = [];
        ActualizarTablaCompras();
        if (rta.fan !== null) {
            if (rta.fan.NombreProducto.substring(0, 6) === "error:") {
                MostrarMensajeRedireccion("Importante", rta.fan.NombreProducto.replace("error:", "").split('|')[0], null, "warning");
            } else {
                MostrarMensajeRedireccion("Importante", "Convenio inválido", null, "warning");
            }
        } else {
            MostrarMensajeRedireccion("Importante", "Convenio inválido", null, "warning");
        }
        $("#inputConvenioCodBarrasSAP").val("");
        inicializadointervalConvenio = false;
        CodigoBarrasConvenioPistoleadedo = "";
    }
    $("#selectMedioPago option").show();
    if (rta.empleado) {
        if (listaDetallePago.length > 0) {
            MostrarMensajeRedireccion("Importante", "Empleado sin autorizacion para pagos diferentes a descuento de nomina", null, "warning");
        }
        for (j = 0; listaDetallePago.length; j++) {
            evtEliminarMedioPago(j);
        }
        $.each($("#selectMedioPago option"), function (i, v) {
            if ($(this).val() !== "6") {
                $(this).hide();
            }
        });
    }
}

function successObtenerDetalleConsecutivoDia(rta) {
    if (rta.resultado != null) {
        listaComprasDia = rta.resultado;
        $.each(listaComprasDia, function (i, item) {
            if (item.CodSapTipoProducto == "2000" || item.CodSapTipoProducto == "2005") {
                numeroPasaportesDia++;
            }
        });
        if (numeroPasaportesDia > 0) {
            ActualizarListaDePrecios();
            MostrarProductos();
        }
    }
}

function successObtenerVencimientoTarjeta(rta) {
    if (rta.resultado != null) {
        var anio = (rta.resultado.substring(6, 10)) * 1;
        var dia = (rta.resultado.substring(3, 5)) * 1;
        var mes = ((rta.resultado.substring(0, 2)) * 1) - 1;
        var vencimiento = new Date(anio, mes, dia);
        var hoy = new Date();
        if (vencimiento < hoy) {
            MostrarMensajeRedireccion("Importante", "La tarjeta FAN esta vencida", null, "warning");
            tarjetaVencida = true;
        }
    }
    return;
}

$("#inputCancelConvenioCodBarrasSAP").click(function (evt) {
    if ($("#selectPosConvenioSAP").val() == "") {
        listDetalleConvenio = [];
        ActualizarTablaCompras();

        $("#inputConvenioCodBarrasSAP").val("");
        $("#inputConvenioCodBarrasSAP").removeAttr("disabled");
        $("#selectPosConvenioSAP").removeAttr("disabled");
        inicializadointervalConvenio = false;
        CodigoBarrasConvenioPistoleadedo = "";
    }
});
/* FIN - Convenios */

/* INI - Reserva Parqueadero */
$("#btnNoTieneReservaParqueadero").click(function () {
    $("#divCodigoReservaParqueadero").hide();
    $("#divCodigoPagoParqueadero").fadeIn();
    $("#inputCodigoReservaParqueadero").val("");
    $("#inputCodigoParqueadero").focus();
    TieneCodigoReservaParqueadero = false;
    $("#inputCodigoReservaParqueadero").removeAttr("disabled");
});

$("#btnSiTieneReservaParqueadero").click(function () {
    var attr = $("#inputCodigoReservaParqueadero").attr('disabled');
    if (typeof attr === typeof undefined) {
        $("#inputCodigoReservaParqueadero").val("");
        $("#divCodigoReservaParqueadero").fadeIn();
        $("#divCodigoPagoParqueadero").fadeIn();
        $("#inputCodigoReservaParqueadero").focus();
    }
});

$("#inputCodigoReservaParqueadero").keypress(function (e) {
    if (window.event) {
        var charCode = event.keyCode;

    } else if (evt.onkeypress.arguments[0].charCode) {
        var charCode = evt.onkeypress.arguments[0].charCode;
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    if (!inicializadointervalResPark) {
        inicializadointervalResPark = true;
        var refreshIntervalIdResPark = setInterval(function () { fnValidaReservaParqueadero(); clearInterval(refreshIntervalIdResPark); }, 300);
    }

});

function fnValidaReservaParqueadero() {
    if ($("#inputCodigoReservaParqueadero").val().length > 0) {
        EjecutarAjax(urlBase + "Pos/ValidaReservaParqueadero", "GET", { CodigoBarrasBoletaControl: $("#inputCodigoReservaParqueadero").val() }, "successValidaReservaParqueadero", null);
        return false;
    } else {
        inicializadointervalResPark = false;
    }
}

function successValidaReservaParqueadero(rta) {
    if (rta == "1") {
        TieneCodigoReservaParqueadero = true;
        $("#inputCodigoReservaParqueadero").attr("disabled", "disabled");
        $("#inputCodigoParqueadero").focus();
    } else {
        TieneCodigoReservaParqueadero = false;
        $("#inputCodigoReservaParqueadero").removeAttr("disabled");
        MostrarMensajeRedireccion("Importante", rta, null, "warning");
        $("#inputCodigoReservaParqueadero").val("");
        inicializadointervalResPark = false;
    }
}
/* FIN - Reserva Parqueadero */

$("#selectMedioPago").change(function () {
    empleadoNominaSeleccionado = new Object();
    iniIntervalPagoNomina = false;
    $("#inputReferencia").val("");
    $("#inputReferencia").removeAttr("disabled");
});

//Cambioquitar
//$("#inputReferencia").keypress(function (evt) {
//    if ($("#selectMedioPago").val() == parametros.IdMedioPagoDescuentoNomina) {
//        if (window.event) {
//            var charCode = event.keyCode;

//        } else if (evt.onkeypress.arguments[0].charCode) {
//            var charCode = evt.onkeypress.arguments[0].charCode;
//        }

//        if (charCode > 31 && (charCode < 48 || charCode > 57))
//            return false;

//        if (!iniIntervalPagoNomina) {
//            iniIntervalPagoNomina = true;
//            var refreshIntervalPagoNomina = setInterval(function () { fnValidaPagoNomina(); clearInterval(refreshIntervalPagoNomina); }, 300);
//        }
//    } else {
//        empleadoNominaSeleccionado = new Object();
//        iniIntervalPagoNomina = false;
//    }

//});

function fnValidaPagoNomina() {
    if ($("#inputReferencia").val() == "") {
        iniIntervalPagoNomina = false;
    } else {
        EjecutarAjax(urlBase + "Pos/ObtenerEmpleadoPorConsecutivo", "GET", { Consecutivo: $("#inputReferencia").val() }, "successfnValidaPagoNomina", null);
    }
}

function successfnValidaPagoNomina(rtaEmpleado) {
    if (rtaEmpleado.IdEstructuraEmpleado > 0) {
        if (rtaEmpleado.CupoRestante > 0) {
            empleadoNominaSeleccionado = rtaEmpleado;
            $("#inputReferencia").val(rtaEmpleado.Documento);
            $("#inputReferencia").attr("disabled", "disabled");
            iniIntervalPagoNomina = false;
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
        iniIntervalPagoNomina = false;
        MostrarMensajeRedireccion("Importante", "Registro Invalido", null, "warning");
    }
}

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
function reiniciarMediosPago() {
    $("#selectMedioPago option").show();
}
//Cambioquitar
function CalcularTotalBonoRegalo(referencia) {
    var _contador = 0;
    if (listaDetallePago.length > 0) {
        $.each(listaDetallePago, function (i, item) {
            if ((item.IdMedioPago == parametros.IdMedioPagoBonoRegalo && item.referencia == referencia) ||
                (item.IdMedioPago == parametros.IdMedioPagoBonoRegalo.toString() && item.referencia == referencia))
                _contador = _contador + item.Valor;
        });
    }

    return _contador;
}
/*DANR: Validacion APP *************************************/
function ValidarPagoApp(Valor, Consecutivo) {
    var result = "";
    $.ajax({
        url: urlBase + "Pos/ObtenerCupoAPP",
        type: 'GET',
        data: { Consecutivo: Consecutivo },
        ContentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (rta) {
            if (rta.Msj != "") {
                MostrarMensajeRedireccion("Importante", rta.Msj, null, "warning");
            }
            else {
                if (parseInt(rta.Saldo) >= Valor) {
                    result = rta.Consecutivo;
                    $("#inputReferencia").val(rta.Mail);
                } else {
                    $("#inputReferencia").val("");
                    $("#inputReferencia").removeAttr("disabled");
                    MostrarMensajeRedireccion("Importante", "El Cliente no cuenta con el monto solicitado a descontar. Saldo: " + FormatoMoneda(rta.Saldo), null, "warning");
                }
            }
        }
    });

    return result;
}


/*DANR: Validacion tarjeta recargable *************************************/
function ValidarTarjetaRecargable(Valor, Consecutivo) {
    var result = "";
    $.ajax({
        url: urlBase + "Pos/ObtenerCupoTarjetaRecargable",
        type: 'GET',
        data: { Consecutivo: Consecutivo },
        ContentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (rta) {
            $("#snapshot").hide();
            $("#divNombreCliente").html("");
            if (rta.IdEstado === 16) {
                MostrarMensajeRedireccion("Importante", "La tarjeta se enuentra bloqueada", null, "warning");
            }
            else if (new Date(moment(rta.FechaFinEvento).format("YYYY-MM-DD HH:mm:ss")) < new Date()) {
                MostrarMensajeRedireccion("Importante", "La tarjeta ha caducado, su fecha de vencimiento fue el " + moment(rta.FechaFinEvento).format("YYYY-MM-DD"), null, "warning");
            }
            else {
                if (rta.NombreProducto !== null) {
                    if (rta.NombreProducto.substring(0, 6) === "error:" && rta.NombreProducto.split('|')[0] !== "error:El convenio FAN ha vencido. Renueve su tarjeta FAN.") {
                        MostrarMensajeRedireccion("Importante", rta.NombreProducto.replace("error:", "").split('|')[0], null, "warning");
                    } else {
                        var nombre = rta.NombreProducto.split('|')[0];
                        var foto = rta.NombreProducto.split('|')[1];
                        if (rta.NombreProducto.substring(0, 6) === "error:") {
                            nombre = rta.NombreProducto.split('|')[1];
                            foto = rta.NombreProducto.split('|')[2];
                        }
                        $("#divNombreCliente").html(nombre);
                        $("#snapshot").attr("src", foto);
                        $("#snapshot").show();
                    }
                }

                if (rta.IdBoleteria > 0) {
                    if (rta.NombreProducto !== null) {

                        if (rta.NombreProducto.substring(0, 6) !== "error:") {
                            if (parseInt(rta.Saldo) >= Valor) {//parseInt(Valor.replace(/\./g, "").trim())) {
                                result = rta.Consecutivo;

                            } else {
                                $("#inputReferencia").val("");
                                $("#inputReferencia").removeAttr("disabled");
                                MostrarMensajeRedireccion("Importante", "La tarjeta no cuenta con el monto solicitado a descontar. Saldo: " + FormatoMoneda(rta.Saldo), null, "warning");
                            }
                        } else {
                            if (rta.NombreProducto.split('|')[0] === "error:El convenio FAN ha vencido. Renueve su tarjeta FAN.") {
                                if (parseInt(rta.Saldo) >= Valor) {//parseInt(Valor.replace(/\./g, "").trim())) {
                                    result = rta.Consecutivo;

                                } else {
                                    $("#inputReferencia").val("");
                                    $("#inputReferencia").removeAttr("disabled");
                                    MostrarMensajeRedireccion("Importante", "La tarjeta no cuenta con el monto solicitado a descontar. Saldo: " + FormatoMoneda(rta.Saldo), null, "warning");
                                }
                            }
                            else
                                MostrarMensajeRedireccion("Importante", rta.NombreProducto.replace("error:", "").split('|')[0], null, "warning");
                        }
                    } else {
                        if (parseInt(rta.Saldo) >= Valor) {//parseInt(Valor.replace(/\./g, "").trim())) {
                            result = rta.Consecutivo;

                        } else {
                            $("#inputReferencia").val("");
                            $("#inputReferencia").removeAttr("disabled");
                            MostrarMensajeRedireccion("Importante", "La tarjeta no cuenta con el monto solicitado a descontar. Saldo: " + FormatoMoneda(rta.Saldo), null, "warning");
                        }
                    }
                } else {
                    $("#inputReferencia").val("");
                    $("#inputReferencia").removeAttr("disabled");
                    MostrarMensajeRedireccion("Importante", "Registro Invalido", null, "warning");
                }
            }
        }
    });

    return result;
}

/**********************************************************************/

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

function ValidarCantidadConvenio() {
    var ret = true;
    $.each(lstProductosCompra, function (key, data) {

        var elemento = $("#prod_compra_" + data.IdProducto + "");

        if (listDetalleConvenio.length > 0) {
            QuitarTooltip();
            $(elemento).removeClass("errorValidate");

            $.each(listDetalleConvenio, function (i, item) {
                if (item.CodSapProducto == data.CodigoSap) {

                    //Cortesia fechas especiales
                    if (item.IdConvenioDetalle == -2) {
                        if (item.Cantidad < data.Cantidad) {
                            $(elemento).attr("data-mensajeerror", "La cantidad no puede ser superior a " + item.Cantidad);
                            $(elemento).addClass("errorValidate");
                            ret = false;
                        }

                    } else {
                        if (item.CantidadxDia < data.Cantidad) {
                            $(elemento).attr("data-mensajeerror", "La cantidad no puede ser superior a " + item.CantidadxDia);
                            $(elemento).addClass("errorValidate");
                            ret = false;
                        }

                    }
                }
            });
        }

    });

    if (!ret)
        mostrarTooltip();
}

//Funcionalidad de recarga tarjeta recargable
//*******************************************************************

$("#cbDonacion").change(function () {
    $("#divDonante").hide();
    if ($(this).prop("checked") === true) {
        $("#txtDonante").val("");
        $("#divDonante").show();
    }
});

$('#btnAdicionarRecarga').click(function () {
    if ($("#ValorRecargaTR").val() === "" || $("#CodTarjeta").val() === "") {
        MostrarMensaje("Error", "El campo Cod. Tarjeta y Valor son obligatorios", "error");
    } else if (parseInt($("#ValorRecargaTR").val().replace(/\./g, "")) > 1000000) {
        MostrarMensaje("Error", "El valor de la recarga no debe superar $1.000.000", "error");
    } else {
        var obj = $("#CodTarjeta");
        if (obj.length > 0) {
            EjecutarAjax(urlBase + "Pos/ObtenerTarjetaRecargable", "GET", { CodBarra: obj.val(), accion: 1 }, "successObtenerTarjetaRecargable", null);
        }
    }
});
obtenerDocumentoCedulaDonante();
function obtenerDocumentoCedula() {
    $("#DocClienteTR").attr("maxlength", 50);
    $("#DocClienteTR").removeClass("numerico");
    $('#DocClienteTR').keypress(function (e) {
        if (!inicializadointerval && !$("#DocClienteTR").hasClass("numerico")) {
            inicializadointerval = true;
            var refreshIntervalId = setInterval(function () { extraerDocumento($('#DocClienteTR').val(), $('#DocClienteTR')); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);
        }
    });
}
obtenerDocumentoCedulaRepo();
function obtenerDocumentoCedulaRepo() {
    $("#DocClienteRepo").attr("maxlength", 50);
    $("#DocClienteRepo").removeClass("numerico");
    $('#DocClienteRepo').keypress(function (e) {
        if (!inicializadointerval && !$("#DocClienteRepo").hasClass("numerico")) {
            inicializadointerval = true;
            var refreshIntervalId = setInterval(function () { extraerDocumento($('#DocClienteRepo').val(), $('#DocClienteRepo')); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);
        }
    });

}
function obtenerDocumentoCedulaDonante() {
    $("#txtDonante").attr("maxlength", 200);
    $("#txtDonante").removeClass("numerico");
    $('#txtDonante').keypress(function (e) {
        if (!inicializadointerval && !$("#txtDonante").hasClass("numerico")) {
            inicializadointerval = true;
            var refreshIntervalId = setInterval(function () { extraerDocumento($('#txtDonante').val(), $('#txtDonante')); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);
        }
    });
}

function extraerDocumento(texto, campo) {
    var txt = texto;
    var cedula = texto;
    if (txt.length >= 30) {
        txt = texto.split('?')[1].substring(0, 30);
        var fin = 0;
        for (var i = 0; i < txt.length; i++) {
            if (!int_try_parse(txt.charAt(i))) {
                fin = i;
                break;
            }
        }
        cedula = parseFloat(txt.substring(fin - 10, fin));
    }
    campo.val(cedula);
}
obtenerDocumentoCedula();
var int_try_parse = function (val) {
    default_val = false;
    try {
        //validate this object is not null
        if (val != null) {
            var rta = parseInt(val);

            default_val = !isNaN(rta);
            //convert to string

        }
    }
    catch (err) {
        console.log(err);
    }
    //this is not a number
    return default_val;
}
$(".EditarDoc").click(function () {
    $("#DocClienteTR").val("");
    if ($("#DocClienteTR").hasClass("numerico")) {
        $("#DocClienteTR").attr("maxlength", 50);
        $("#DocClienteTR").removeClass("numerico")
        obtenerDocumentoCedula();
    } else {
        $("#DocClienteTR").off("keypress");
        $("#DocClienteTR").attr("maxlength", 15);
        $("#DocClienteTR").addClass("numerico");
    }
});
$(".EditarDocRepo").click(function () {
    $("#DocClienteRepo").val("");
    if ($("#DocClienteRepo").hasClass("numerico")) {
        $("#DocClienteRepo").attr("maxlength", 50);
        $("#DocClienteRepo").removeClass("numerico")
        obtenerDocumentoCedulaRepo();
    } else {
        $("#DocClienteRepo").off("keypress");
        $("#DocClienteRepo").attr("maxlength", 15);
        $("#DocClienteRepo").addClass("numerico");
    }
});

$(".EditarDon").click(function () {
    $("#txtDonante").val("");
    if ($("#txtDonante").hasClass("numerico")) {
        $("#txtDonante").attr("maxlength", 50);
        $("#txtDonante").removeClass("numerico");
        obtenerDocumentoCedulaDonante();
    } else {
        $("#txtDonante").off("keypress");
        $("#txtDonante").attr("maxlength", 15);
        $("#txtDonante").addClass("numerico");
    }
});


function successObtenerTarjetaRecargable(rta) {

    if (rta.IdProducto > 0) {
        var obj = BuscarIdBrazalete(rta.IdDetalleProducto);
        if (obj === null) {
            if (ValidarRecargaAdicionproducto(parametros.CodSapRecargaTarjeta.Valor)) {
                var r = ObtenerproductoPorCodigoSap(parametros.CodSapRecargaTarjeta.Valor);
                r.Precio = $("#ValorRecargaTR").val().replace(".", "").replace(".", "");
                r.Cantidad = 1;
                r.PrecioTotal = r.Precio;
                r.Entregado = true;
                r.DataExtension = rta.ConseutivoDetalleProducto;
                lstProductosCompra.push(r);
                ActualizarTablaCompras();
                MostrarcambioApp();
            }


        } else
            MostrarMensaje("Importante", "Ya fue agregada una recarga a esta tarjeta, si desea un valor diferente de recarga, elimine el producto de la lista y agreguelo con el nuevo saldo a recargar.", "warning");

        $("#ValorRecargaTR").val("");
        $("#CodTarjeta").val("");
    } else {

        $("#ValorRecargaTR").val("");
        $("#CodTarjeta").val("");
        MostrarMensaje("Importante", rta.MensajeValidacion, "warning");
    }
}


function MostrarModalRedencionCortesia() {    
    abrirModal("modalRedencionCortesia");
}

$('#NumTarjeta').keypress(function (e) {    
    //if (validarFormulario("frmCortesias")) {
    var _numTarjeta = $("#NumTarjeta").val();
    
    EjecutarAjax(urlBase + "Cortesia/ObtenerCortesiaUsuarioVisitante", "GET", { numTarjeta: _numTarjeta }, "RespuestaConsulta");
    //}
    
});

function RespuestaConsulta(rta, doc) {    
    
    if (rta.Elemento.NumeroDocumento != null || rta.Elemento.TipoDocumento != null) {
        usuarioCortesia = rta.Elemento;
        
        if (rta.Elemento.Apellidos != null) {
            $("#NombreVisitante").html(rta.Elemento.Nombres + ' ' + rta.Elemento.Apellidos);
        }
        else {
            $("#NombreVisitante").html(rta.Elemento.Nombres);
        }

        $("#CedulaVisitante").html(rta.Elemento.NumeroDocumento);
        
        $("#lblCantidadCortesias").html(rta.Elemento.Cantidad);
        $("#txtNumdocumentoV").val(rta.Elemento.NumeroDocumento);
        $("#txtNumTarjetaFAN").val(rta.Elemento.NumTarjetaFAN);
        $("#txtTipoCortesiaV").val(rta.Elemento.IdTipoCortesia);
        /*$("#btnAceptar").val("Aceptar");*/
        if (rta.Elemento.IdTipoCortesia == 1 || rta.Elemento.IdTipoCortesia == 5 || rta.Elemento.IdTipoCortesia == 2) {                        
            EjecutarAjax(urlBase + "Pos/ObtenerDetalleCortesia", "GET", { documento: rta.Elemento.NumeroDocumento, IdTipoCortesia: rta.Elemento.IdTipoCortesia, numeroTarjetaFAN: rta.Elemento.NumTarjetaFAN }, "RespuestaConsultaDetalle", null);
        }
        
    }
    else {
        usuarioCortesia = null;

        MostrarMensaje("Mensaje", "El usuario visitante no se encuentra disponible para redimir una cortesía");
    }
}

function RespuestaConsultaDetalle(data) {
    if (isNaN(data)) {
        $("#Detallecortesia").html(data);        
    }
}

$('#txtCodigo').keypress(function (e) {
    if (!inicializadointerval) {
        inicializadointerval = true;
        var refreshIntervalId = setInterval(function () { ConsultarBoleta(); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);
    }
});

function ConsultarBoleta() {
    var obj = $("#txtCodigo");
    var numdocument = $("#txtNumdocumentoV");
    var IDtipoCorteV = $("#txtTipoCortesiaV");
    var numerotarjeta = $("#txtNumTarjetaFAN");
    if (obj.length > 0) {        
        EjecutarAjax(urlBase + "Pos/ObtenerProductoCortesia", "POST", JSON.stringify({ CodBarra: obj.val(), Documento: numdocument.val(), numtarjeta: numerotarjeta.val(), productos: listProductosCortesia, IdTipoCortesia: IDtipoCorteV.val(), impresionLinea: 0, IdDetalle: IdDetalleCortesia }), "successObtenerProducto", null);
        obj.val("");
    }
}

function successObtenerProducto(rta) {

    if (rta.IdProducto > 0) {
        //var obj = BuscarIdBrazalete(rta.IdDetalleProducto);
        var obj = BuscarIdBrazaleteCortesia(rta.IdDetalleProducto);
        if (obj === null) {
            rta.Cantidad = 1;
            rta.PrecioTotal = rta.Precio;
            listProductosCortesia.push(rta);            
            ActualizarTablaCortesias();
        } else
            MostrarMensaje("Importante", "La boleta ya fue agregado en la lista de productos.", "warning");
    } else {

        MostrarMensaje("Importante", rta.MensajeValidacion, "warning");
    }
}


//Actualiza la tabla de cortesias a redimir
function ActualizarTablaCortesias() {

    if (listProductosCortesia.length > 0) {
        var tablaHeadCortesias = "<div class='row x_panel'> <table class='table table-striped jambo_table' width='100%'>";
        tablaHeadCortesias += "<thead>" + "<th>Nombre</th>"
            + "<th></th>"
            + "</thead>";
        var tablaBodyCortesias = "<tbody>";

        $.each(listProductosCortesia, function (i, item) {

            tablaBodyCortesias += "<tr>"
                + "<td style='vertical-align: middle;'>" + (item.ConseutivoDetalleProducto == null ? item.Nombre : item.Nombre + " " + item.ConseutivoDetalleProducto) + "</td>"
                + "<td style='vertical-align: middle;'><a data-id ='" + item.IdProducto + "' class='evtEliminarCortesia' id='" + item.IdDetalleProducto + "' href='javascript:void(0)'><span class='fa fa-trash-o IconosPos' aria-hidden='true' ></span></a></td></tr>";

        });

        var footerCortesias = "</tbody></table></div>";
        $("#listProductos").html(tablaHeadCortesias + tablaBodyCortesias + footerCortesias);



        $(".evtEliminarCortesia").click(function () {

            var id = $(this).data('id');
            var idDetalle = $(this).attr("id");

            if (listProductosCortesia.length == 1)
                listProductosCortesia = [];
            else {

                if (idDetalle != '0') {

                    $.each(listProductosCortesia, function (i) {
                        if (listProductosCortesia[i].IdDetalleProducto == idDetalle) {
                            listProductosCortesia.splice(i, 1);
                            return false;
                        }
                    });
                }
            }            
            ActualizarTablaCortesias();
        });

    } else {
        $("#listProductos").html("");
    }
}

function Seleccionar(idDetalle) {
    IdDetalleCortesia = idDetalle;
}

function MostrarProductos() {
    if (lstProductosCompra.length > 0) {
        var tablaHead = "<div class='row x_panel'> <table class='table table-striped jambo_table' width='100%'>";
        tablaHead += "<thead>" + "<th>Nombre</th>"
            + "<th>Valor</th>"
            + "<th>Cantidad</th>"
            + "<th>Total</th>"
            + "<th></th>"
            + "</thead>";
        var tablaBody = "<tbody>";
        var UltimoIdProducto = "";
        var disabledCantidad = "";
        var hiddenCantidad = "";
        $.each(lstProductosCompra, function (i, item) {
            var IdProducto = item.IdDetalleProducto == null || item.IdDetalleProducto === undefined ? "0" : item.IdDetalleProducto;
            UltimoIdProducto = item.IdProducto;
            disabledCantidad = "";
            if (validaProductoAgrupaCantidad(item.CodSapTipoProducto, item.CodigoSap)) {
                disabledCantidad = "disabled";
            }
            if (item.CtgProducto == 1) {
                disabledCantidad = "disabled";
                hiddenCantidad = "hidden";
            }
            tablaBody += "<tr>"
                //+ "<td style='vertical-align: middle;'>" + (item.ConseutivoDetalleProducto == null ? item.Nombre : item.Nombre + " " + item.ConseutivoDetalleProducto) + "</td>"
                + "<td style='vertical-align: middle;'>" + item.Nombre + "</td>"

                + "<td style='vertical-align: middle;'>" + EnMascarar(item.Precio) + "</td>"
                + "<td style='vertical-align: middle;' width='20px'>" + "<input data-id ='" + item.IdProducto + "' type='text' style='text-align: center; padding-top: 0; padding-bottom: 0; height: 22px;' class='form-control evtCambiar' id='prod_compra_" + item.IdProducto + "'" + "value='" + item.Cantidad + "' maxlength='3' autofocus onkeypress='return EsNumero(this);' " + disabledCantidad + " />" + "</td>"
                + "<td style='vertical-align: middle;' width='100px'>" + EnMascarar(item.PrecioTotal) + "</td>"
                + "<td style='vertical-align: middle;'><a data-id ='" + item.IdProducto + "' class='evtEliminar' id='" + IdProducto + "' " + hiddenCantidad + "><span class='fa fa-trash-o IconosPos' aria-hidden='true'  ></span></a></td></tr>";

        });

        var footer = "</tbody></table></div>";
        $("#dvProductos").html(tablaHead + tablaBody + footer);
        MostrarTotal();
        ValidarCantidadConvenio();

        var searchInput = $("#prod_compra_" + UltimoIdProducto);
        if (!searchInput.prop("disabled")) {
            // Multiply by 2 to ensure the cursor always ends up at the end;
            // Opera sometimes sees a carriage return as 2 characters.
            var strLength = searchInput.val().length * 2;
            searchInput.focus();
            searchInput[0].setSelectionRange(strLength, strLength);
        }

        $(".evtCambiar").change(function () {

            var id = $(this).data('id');
            var elemento = $(this);
            index = id;
            if (isNaN($(this).val()) || $(this).val() == "" || $(this).val() == " ") {
                $(this).val("1");
            }
            var NuevaCantidad = parseInt($(this).val());
            if (NuevaCantidad <= 0) {
                NuevaCantidad = 1;
            }

            $.each(lstProductosCompra, function (i) {
                if (lstProductosCompra[i].IdProducto === id) {
                    lstProductosCompra[i].Cantidad = NuevaCantidad;
                    lstProductosCompra[i].PrecioTotal = (lstProductosCompra[i].Precio * NuevaCantidad);
                    return false;
                }
            });

            ActualizarTablaCompras();
            MostrarcambioApp();
        });

        $(".evtEliminar").click(function () {
            var id = $(this).data('id');
            var idDetalle = $(this).attr("id");

            if (lstProductosCompra.length == 1)
                lstProductosCompra = [];
            else {

                if (idDetalle != '0') {

                    $.each(lstProductosCompra, function (i) {
                        if (lstProductosCompra[i].IdDetalleProducto == idDetalle && lstProductosCompra[i].IdProducto == id) {
                            if (lstProductosCompra[i].CodigoSap == parametros.CodSapTarjetaRecargable.Valor) {
                                var idtest = lstProductosCompra[i].ConseutivoDetalleProducto;
                                lstProductosCompra.splice(i, 1);
                                var lstFinalProdu = $.grep(lstProductosCompra, function (e) {
                                    return e.DataExtension != idtest
                                });
                                lstProductosCompra = lstFinalProdu;
                                return false;
                                //$.each(lstProductosCompra, function (j, v) {
                                //    if (lstProductosCompra[i].ConseutivoDetalleProducto === v.DataExtension) {

                                //        lstProductosCompra.splice(i, 1);
                                //        lstProductosCompra.splice(j - 1, 1);
                                //    }
                                //});
                            } else {
                                lstProductosCompra.splice(i, 1);
                            }
                            return false;
                        }
                    });
                } else {

                    $.each(lstProductosCompra, function (i) {
                        if (lstProductosCompra[i].IdProducto === id) {
                            if (lstProductosCompra[i].CodigoSap === parametros.CodSapPrecioTarjeta.Valor || lstProductosCompra[i].CodigoSap === parametros.CodSapClienteFan.Valor || lstProductosCompra[i].CodigoSap === parametros.CodSapReposicionTarjeta.Valor) {
                                var idtest = lstProductosCompra[i].DataExtension;
                                lstProductosCompra.splice(i, 1);
                                var lstFinalProdu = $.grep(lstProductosCompra, function (e) {
                                    return (e.DataExtension != idtest && e.ConseutivoDetalleProducto != idtest)
                                });
                                lstProductosCompra = lstFinalProdu;
                            } else {
                                lstProductosCompra.splice(i, 1);
                            }

                            return false;
                        }
                    });
                }
            }
            ActualizarTablaCompras();
            MostrarcambioApp();
        });

        //DANR: 22-01-2019 -- Adicion campo donante
        var mostrarDonante = false;
        $.each(lstProductosCompra, function (i, v) {
            if ($("#inputCodSapProductosDonaciones").val().indexOf(v.CodigoSap) >= 0)
                mostrarDonante = true;
        });
        if (mostrarDonante) {
            if ($("#cbDonacion").prop("checked") && $("#txtDonante").val() !== "") {
            } else {
                $("#cbDonacion").prop("checked", true);
                $("#divDonarCambio").hide();
                $("#txtDonante").val("");
                $("#divDonarProd").show();
                $("#divDonante").show();
            }
        }
        else {
            $("#divDonarCambio").show();
            $("#divDonarProd").hide();
            $("#cbDonacion").prop("checked", false);
            $("#divDonante").hide();
        }
        //fin DANR: 22-01-2019 -- Adicion campo donante

        MostrarcambioApp();
    } else {
        /*Limpiar();*/
        $("#dvProductos").html("");
    }
}

function BuscarIdBrazaleteCortesia(id) {
    var objReturn = null;

    $.each(listProductosCortesia, function (i, item) {
        if ($.trim(item.IdDetalleProducto) == $.trim(id)) {
            objReturn = item;
            return false;
        }
    });

    return objReturn;
}