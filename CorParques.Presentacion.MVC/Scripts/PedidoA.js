var HoraInicio;
var HoraFin;
var IdReserva;

//Propiedades
var _idmesa;
var _idtemp;

var _idProductocarga = 0;
var _idDetalleAcompa = 0;
var _idDetalle = 0;
var _contemp;
var _IdTipoProd = 1;
var _IdPanel = 1;
var _idDetalletemp;
var _IdPedido = 0;
var _Contador = 0;
var Banderasave;
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
var _IdProducto;
var lstAcompanamientos = [];

$(function () {

    setEventEdit();


    $("#btnSaveGeneric").click(function () {

        var myObjGeneral = {};
        myObjGeneral.IdProducto = _IdProducto;
        myObjGeneral.Consecutivo = _Contador;
        myObjGeneral.Observaciones = $("#TxtDescripcion").val();
        myObjGeneral.TipoEntrega = 0;
        $('#IdTipoEntregacheck:checked').each(function () {
            myObjGeneral.TipoEntrega = $("#IdTipoEntregacheck").val();
        });
        lstAcompanamientos.push(myObjGeneral);

        $('.checkAcomp:checked').each(function () {
            var myObj = {};
            myObj.IdProducto = _IdProducto;
            myObj.IdAcomp = $(this).attr("id");
            myObj.Consecutivo = _Contador;
            myObj.Nombre = $(this).val();
            myObj.Observaciones = $("#TxtDescripcion").val();
            lstAcompanamientos.push(myObj);
        });

        ActualizarTablaCompras2();

        mostrarAlerta("Información", "Producto agregado a pedido.", "success");
        cerrarModal("modalCRUD");
    });

    $("#ddlPunto").change(function () {

        EjecutarAjax(urlBase + "ReservaSkycoaster/ObtenerLista", "GET", { id: $(this).val() }, "printPartial", { div: "#listView", func: "setEventEdit" });
    });

    //$("#btnImprimirPreFactura").css("display", "none");
    //if (lstProductosCompra == 0) {
    $("#btnImprimirPreFactura").prop("disabled", true);
    //}
});

$(".productos .ProductoGrilla").click(function (e) {
    crearListaBusqueda($(this).data("id"));
});
$(".productos .ProductoGrillaMesas").click(function (e) {
    crearListaBusqueda($(this).data("id"));
});

function crearListaBusqueda(id) {

    var _list;
    _list = tmpLista;
    _IdProducto = id;
    AgregarProductoACompra(id, _list);

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

function AgregarProductoACompra(IdProducto, tmpList) {
    _Contador++;
    index = IdProducto;
    if (tmpList != null) {

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
            ProductoSeleccionado[0].Consecutivo = _Contador;
            ProductoSeleccionado[0].Cantidad = 1;
            ProductoSeleccionado[0].Entregado = false;
            ProductoSeleccionado[0].PrecioTotal = ProductoSeleccionado[0].Precio;
            lstProductosCompra.push(ProductoSeleccionado[0]);
        }


        ActualizarTablaCompras2();
        acompa = "prueba";
        var idmesa = $("#IdPuntoOrigen").val();
        EjecutarAjax(urlBase + "PedidoA/Acompa", "GET", { IdProducto: IdProducto, IdMesa: idmesa }, "printPartialModalPedidosA", {
            title: "Detalle Pedido", hidesave: false, modalLarge: false, OcultarCierre: true
        });

    }
}

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
function MostrarContenedorProducto22(id) {
    //Limpiar Buscador
    $.each($(".txtBuscadorMesas"), function (i, item) {
        $(item).val("");
    });

    $("#txtBuscadorMesas").val("");
    //Muestra todos productos y no tiene encuenta el filtro - se comprate parcial souvenir y alimentos
    $.each($(".ProductoGrillaMesas"), function (i, v) {
        $(v).show();
    })

    $.each($(".tipoProducto"), function (i, item) {

        if ($(item).attr('id').indexOf(id) == -1)
            $(item).hide();
        else
            $(item).show();
    });
}


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
$(function () {
    $(".txtBuscadorMesas").keyup(function () {
        var texto = $(this).val();
        if ((texto.length <= 3) && (isInt(texto)))
            texto = "# " + texto;

        $.each($(".ProductoGrillaMesas"), function (i, v) {
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
function EliminarProductoTabla(data, obj) {

    if (data < 3) {

        if (lstProductosCompra.length == 1) {
            lstProductosCompra = [];
            lstAcompanamientos = [];
        }
        else {

            if (_idDetalletemp != '0') {

                $.each(lstProductosCompra, function (i) {
                    if (lstProductosCompra[i].IdDetalleProducto == _idDetalletemp && lstProductosCompra[i].IdProducto == _idtemp) {
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
                            var lstFinalAcompa = lstAcompanamientos;

                            $.each(lstFinalAcompa, function (j) {
                                if (lstFinalAcompa[j].IdProducto === _idtemp) {
                                    lstAcompanamientos.splice(j, 1);
                                }
                            });
                        }

                        return false;
                    }

                });
            } else {
                let lstFinalAcompa = [];
                lstFinalAcompa = lstAcompanamientos;
                $.each(lstProductosCompra, function (i) {
                    if (lstProductosCompra[i].IdProducto === _idtemp && lstProductosCompra[i].Consecutivo === _contemp) {
                        if (lstProductosCompra[i].CodigoSap === parametros.CodSapPrecioTarjeta.Valor || lstProductosCompra[i].CodigoSap === parametros.CodSapClienteFan.Valor || lstProductosCompra[i].CodigoSap === parametros.CodSapReposicionTarjeta.Valor) {
                            var idtest = lstProductosCompra[i].DataExtension;
                            lstProductosCompra.splice(i, 1);
                            var lstFinalProdu = $.grep(lstProductosCompra, function (e) {
                                return (e.DataExtension != idtest && e.ConseutivoDetalleProducto != idtest)
                            });
                            lstProductosCompra = lstFinalProdu;
                        } else {
                            lstProductosCompra.splice(i, 1);
                            let k = 0;
                            while (k < lstAcompanamientos.length) {
                                var item = lstAcompanamientos[k];

                                if (item.IdProducto === _idtemp && item.Consecutivo === _contemp) {
                                    lstAcompanamientos.splice(k, 1);
                                } else {
                                    k += 1;
                                }

                            }



                        }

                        return false;
                    }
                });
            }
        }
        ActualizarTablaCompras2();
    }
    else {
        MostrarMensajeRedireccion("Importante", "El producto no se puede eliminar porque se encuentra en un estado despachado o entregado", null, "warning");
    }

}




function printPartialModalPedidosA(data, obj) {

    if (data != 1) {

        $("#btnCancelGeneric").show();
        $("#btnSaveGeneric").show();
        $("#btnVolverGeneric").hide();
        $("#btnImprimirGeneric").hide();

        $("#btnCancelGeneric").click(function () {
            cerrarModal('modalCRUD');
        });
        $("#btnVolverGeneric").click(function () {
            cerrarModal('modalCRUD');
        });

        $("#btnImprimirGeneric").click(function () {

            $("#div_print").show();
            var mode = "popup";
            var close = true;
            var extraCss = "";
            var print = "#div_print";
            var keepAttr = [];
            keepAttr.push("class");
            keepAttr.push("id");
            keepAttr.push("style");

            var headElements = '<meta charset="utf-8" />,<meta http-equiv="X-UA-Compatible" content="IE=edge"/>';
            var options = { mode: mode, popClose: close, extraCss: extraCss, retainAttr: keepAttr, extraHead: headElements };

            $(print).printArea(options);
            setTimeout(function () { $("#div_print").hide(); }, 200);

        });

        $("#modalCRUD .modal-dialog").removeClass("modal-lg");
        if (obj.modalLarge)
            $("#modalCRUD .modal-dialog").addClass("modal-lg");
        $("#modalCRUD .modal-title").html(obj.title);
        $("#modalCRUD .modal-body").html(data);
        //$('input.Acompa1').on('change', function () {

        //    var cantidadMaxima = $("#MaxAcompa").val();
        //    // Cogemos el elemento actual
        //    var elemento = this;
        //    var contador = 0;

        //    // Recorremos todos los checkbox para contar los que estan seleccionados
        //    $(".Acompa1").each(function () {
        //        if ($(this).is(":checked"))
        //            contador++;
        //    });



        //    // Comprovamos si supera la cantidad máxima indicada
        //    if (contador > cantidadMaxima) {


        //        // Desmarcamos el ultimo elemento
        //        $(elemento).prop('checked', false);
        //        contador--;
        //        MostrarMensaje("Importante", "Has seleccionado mas acompañamientos que los indicados.", "error");
        //    }

        //});
        $('input.checkAcomp').on('change', function () {
            var idacompa = $(this).attr("maxlength");

            var cantidadMaxima = $("#MaxAcompa-" + idacompa).val();
            // Cogemos el elemento actual
            var elemento = this;
            var contador = 0;

            // Recorremos todos los checkbox para contar los que estan seleccionados
            $(".Acompa-" + idacompa).each(function () {
                if ($(this).is(":checked"))
                    contador++;
            });



            // Comprovamos si supera la cantidad máxima indicada
            if (contador > cantidadMaxima) {


                // Desmarcamos el ultimo elemento
                $(elemento).prop('checked', false);
                contador--;
                MostrarMensaje("Importante", "Has seleccionado mas acompañamientos que los indicados.", "error");
            }

        });
        //$('input.Acompa2').on('change', function () {
        //    var cantidadMaxima = $("#MaxBebidas").val();
        //    // Cogemos el elemento actual
        //    var elemento = this;
        //    var contador = 0;

        //    // Recorremos todos los checkbox para contar los que estan seleccionados
        //    $(".Acompa2").each(function () {
        //        if ($(this).is(":checked"))
        //            contador++;
        //    });

        //    // Comprovamos si supera la cantidad máxima indicada
        //    if (contador > cantidadMaxima) {

        //        // Desmarcamos el ultimo elemento
        //        $(elemento).prop('checked', false);
        //        contador--;
        //        MostrarMensaje("Importante", "Has seleccionado mas bebidas que las indicadas.", "error");
        //    }

        //});
        if (obj.hidesave) {
            $("#btnCancelGeneric").hide();
            $("#btnSaveGeneric").hide();
        }
        if (obj.showreturn) {
            $("#btnVolverGeneric").show();
        }
        if (obj.print) {
            $("#btnImprimirGeneric").show();
        }
        if (obj.Table) {
            $("#datatable-responsive_1").DataTable();
            $("#modalCRUD .modal-dialog").css("width", 940);
        }

        if (obj.func2) {
            window[obj.func2](obj.param);
        }

        if (obj.DatePicker) {
            loadCalendar();
        }

        if (obj.TimePicker) {
            setTimePicker();
        }

        if (obj.OcultarCierre)
            $("#modalCRUD .close").hide();
        else
            $("#modalCRUD .close").show();

        if (obj.TextoBotonGuardar)
            $("#btnSaveGeneric").html(obj.TextoBotonGuardar);


        setNumeric();
        abrirModal("modalCRUD");
    }
    else {
        mostrarAlerta("Información", "Producto agregado a pedido.", "success");
    }
}



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
function ObtenerAcompanamiento(Arreglo, IdDetallePedido) {
    var result = null;
    $.each(Arreglo, function (i, item) {
        if (item.IdDetallePedido == IdDetallePedido) {
            result = JSON.parse(JSON.stringify(item));
        }
    });
    if (result !== null)
        return [result]
    return null;
}
function ObtenerAcompanamientos(Arreglo, IdProducto, IdConsecutivo) {
    var result = null;
    $.each(Arreglo, function (i, item) {
        if (item.IdProducto == IdProducto && item.Consecutivo == IdConsecutivo) {
            if (result == null) {
                result = item.Nombre;
            }
            else {
                result = result + "," + item.Nombre;
            }
        }
    });
    if (result !== null)
        return [result]
    return [];
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

function verDiasSeleccionados() {

    var diasSeleccionados = new Array();



    $('.checkAcomp:checked').each(function () {
        var myObj = {};
        myObj.IdProducto = _IdProducto;
        myObj.IdAcomp = $(this).attr("id");
        myObj.Consecutivo = _Contador;
        myObj.Nombre = $(this).val();
        lstAcompanamientos.push(myObj);
    });
    ActualizarTablaCompras2();
    MostrarMensaje("Importante", "Producto agregado a pedido.", "success");
}
$("#btnAnularPedido").click(function () {
    $("#btnAnularPedido").attr("disabled", true);
    if (_IdPedido > 0) {
        ConfirmaAnulacion();
    }
    else {
        MostrarMensajeRedireccion("Importante", "No se puede anular el pedido porque no se ha guardado", null, "warning");
        $("#btnAnularPedido").attr("disabled", false);
        return false;
    }
});

$("#btnCerrarMesa").click(function () {

    $("#btnCerrarMesa").attr("disabled", true);
    var bandera;
    $('.evtCambiarcheck').each(function () {

        if ($(this).is(':checked')) {
            /* bandera = true;*/
        } else {
            bandera = false;
        }
    });
    if (bandera == false) {
        MostrarMensajeRedireccion("Importante", "No se puede cerrar la mesa porque se encuentran productos sin entregar", null, "warning");
        $("#btnCerrarMesa").attr("disabled", false);
        return false;
    }
    if (_IdPedido > 0) {
        ConfirmarCierre();
    }
    else {
        MostrarMensajeRedireccion("Importante", "No se puede cerrar la mesa porque el pedido no se ha guardado", null, "warning");
        $("#btnCerrarMesa").attr("disabled", false);
        return false;
    }

});
$("#btnSavePedidoP").click(function () {
    $("#btnSavePedidoP").attr("disabled", true);
    //Valida que el usuario por lo menos seleccione un producto
    if (lstProductosCompra.length == 0) {
        MostrarMensajeRedireccion("Importante", "Debe seleccionar al menos un producto.", null, "warning");
        $("#btnSavePedidoP").attr("disabled", false);
        return false;
    }
    lista = [];
    Lista2 = [];
    var CopiaProducto = JSON.parse(JSON.stringify(lstProductosCompra));
    var CopiaAcomp = JSON.parse(JSON.stringify(lstAcompanamientos));

    $.each(CopiaProducto, function (i, item) {
        for (i = 0; i < item.Cantidad; i++) {
            lista.push(item);
        }
    });
    $.each(CopiaAcomp, function (j, item2) {
        for (j = 0; j < item2.Cantidad; j++) {
            Lista2.push(item2);
        }
    });
    // enable btn impresion
    $("#btnImprimirPreFactura").prop("disabled", false);
    ConfirmarPago()
});
function successPagarPos(data, values) {
    if (isNaN(data)) {
        //$(values.div).html(data);
        //finalizarProceso();
        _IdPedido = data.Elemento;

        var idmesa = $("#IdPuntoOrigen").val();
        EjecutarAjaxJson(urlBase + "PedidoA/ListarProductosMesa", "post", {
            IdMesa: idmesa
        }, "successListarProdMesa", null);

        EjecutarAjaxJson(urlBase + "PedidoA/ActualizarMesas", "GET", {

        }, "successActualizarMesas", { div: "#Mesas" });


    } else {

    }
}
function successActualizarMesas(data, values) {
    if (isNaN(data)) {
        $(values.div).html(data);
        finalizarProceso();


        mostrarAlerta("Información", "Pedido guardado", "success");
        $('.MesasBusq').click(function () {
            _idmesa = $(this).data("id");
            var nombremesa = $(this).data("nombre");
            $("#TabPedido").addClass('active');
            $("#TabMesas").removeClass('active');
            $("#home").addClass('active in');
            $("#menu2").removeClass('active in');

            $("#btnSaveAPedido").hide();
            $("#ContenedorCarta").show();
            $("#ConFactura").show();
            $("#ConMesa").show();
            $('#IdPuntoOrigen').prepend("<option value='" + _idmesa + "' >" + nombremesa + "</option>");
            $("#IdPuntoOrigen").val(_idmesa);
            //$("#IdPuntoOrigen option[value='" + idmesa + "']").attr("selected", true);
            $("#IdPuntoOrigen").prop("disabled", true);
            Banderasave = 1;


            EjecutarAjaxJson(urlBase + "PedidoA/ListarProductosMesa", "post", {
                IdMesa: _idmesa
            }, "successListarProdMesa", null);


        });
        $("#btnSavePedidoP").attr("disabled", false);
    } else {

    }
}

function successCierreMesa(rta) {
    if (rta !== null) {
        /* if (rta == "NO") {*/
        //MostrarMensajeRedireccion("Importante", "No se puede cerrar la mesa porque no se ha cancelado el pedido en caja", null, "warning");
        //$("#btnCerrarMesa").attr("disabled", false);
        //return false;
        window.location = urlBase + "PedidoA";
    }
    else {

    }

}
function successListarMesaZona(rta) {
    if (rta !== null) {

        var select = document.getElementById("IdPuntoOrigen");
        document.getElementById("IdPuntoOrigen").innerHTML = ""

        var option1 = document.createElement("option");
        option1.value = "";
        option1.text = "*Seleccione...";
        select.add(option1);

        $.each(rta, function (i, item) {

            var option2 = document.createElement("option");
            option2.text = item.Nombre;
            option2.value = item.Id;
            select.add(option2);
        });


    }
    else {

    }

}
function successAnulacionPedido(rta) {
    if (rta !== null) {
        if (rta == "SI") {
            window.location = urlBase + "PedidoA";

        }
        else {
            MostrarMensajeRedireccion("Importante", "No se puedo anular el pedido", null, "warning");
            $("#btnAnularPedido").attr("disabled", false);
            return false;
        }

    }
    else {

    }

}

function TipoProdResta(id) {
    _IdTipoProd = id;
    ActulizarPedidos(1);
    $("#TabPedidoAraza").addClass('active');
    $("#TabPedidoPreparacion").removeClass('active');
    $("#TabPedidoPreparados").removeClass('active');
    $("#home2").addClass('active in');
    $("#menu22").removeClass('active in');
    $("#menu33").removeClass('active in');
}
function ActulizarPedidos(id) {

    //$(values.div).html(data);
    //finalizarProceso();
    var nombrediv;
    _IdPanel = id;
    if (id === 1) {
        nombrediv = "#home2";
    }
    if (id === 2) {
        nombrediv = "#menu22";
    }
    if (id === 3) {
        nombrediv = "#menu33";
    }

    EjecutarAjaxJson(urlBase + "PedidoCocina/ActualizarMesasCocina", "GET", { IdEstado: id, IdTipoProd: _IdTipoProd }, "successActualizarMesasCocina", { div: nombrediv });



}
function successActualizarMesasCocina(data, values) {
    if (isNaN(data)) {
        if (data === "Alerta") {
            MostrarMensaje("Importante", "El producto ha sido cancelado ", "error");
        }
        else {
            $(values.div).html(data);
            $(".evtActualizaProdPedido").click(function () {
                var EstadoProducto = $(this).data('con');
                var idDetallePedido = $(this).attr("id");
                EjecutarAjaxJson(urlBase + "PedidoCocina/ActualizarEstadoProducto", "GET", { IdDetallePedido: idDetallePedido, IdEstadoProd: EstadoProducto, IdEstado: _IdPanel, IdTipoProd: _IdTipoProd }, "successActualizarMesasCocina", { div: values.div });
                //if (confirm("Está seguro que desea eliminar este usuario?"))
                //    EjecutarAjax(urlBase + "Usuarios/Delete", "GET", { id: $(this).data("id") }, "successDelete", null);
            });

        }
        finalizarProceso();


    } else {

    }
}

$(".evtActualizaProdPedido").click(function () {
    var EstadoProducto = $(this).data('con');
    var idDetallePedido = $(this).attr("id");
    EjecutarAjaxJson(urlBase + "PedidoCocina/ActualizarEstadoProducto", "GET", { IdDetallePedido: idDetallePedido, IdEstadoProd: EstadoProducto, IdEstado: 1, IdTipoProd: 1 }, "successActualizarMesasCocina", { div: "#home2" });
    //if (confirm("Está seguro que desea eliminar este usuario?"))
    //    EjecutarAjax(urlBase + "Usuarios/Delete", "GET", { id: $(this).data("id") }, "successDelete", null);
});



function successListarProdMesa(rta) {
    if (rta !== null) {
        _Contador = 0;
        lstAcompanamientos = [];
        lstProductosCompra = [];
        var nombreclientePe = "";
        $.each(rta.listaProductos, function (i, item) {

            if (nombreclientePe === "") {
                nombreclientePe = item.NombreCliente;
            }
            _Contador++;
            var ProductoSeleccionado2 = ObtenerProducto(tmpLista, item.Id_Producto);
            _IdPedido = item.Id_Pedido;
            ProductoSeleccionado2[0].Consecutivo = _Contador;
            ProductoSeleccionado2[0].Cantidad = 1;
            ProductoSeleccionado2[0].IdDetallePedido = item.IdDetallePedido;
            if (item.Entregado === 1) {
                ProductoSeleccionado2[0].Entregado = true;
            }
            else {
                ProductoSeleccionado2[0].Entregado = false;
            }


            lstProductosCompra.push(ProductoSeleccionado2[0]);


            $.each(rta.listaAcompa, function (j, item2) {
                if (item2.IdDetallePedido == item.IdDetallePedido) {
                    var myObj = {};
                    myObj.IdProducto = item2.Id_Producto;
                    myObj.IdAcomp = item2.Id_Acomp;
                    myObj.Consecutivo = _Contador;
                    myObj.Nombre = item2.Nombre;
                    myObj.Observaciones = "";
                    lstAcompanamientos.push(myObj);
                }
            });
        });
        $("#inputNombreCliente").val(nombreclientePe);

        ActualizarTablaCompras2();

        finalizarProceso();
        mostrarAlerta("Información", "Mesa seleccionada", "success");
    } else {

    }
}

function successListarProdFactura(rta) {
    if (rta !== null) {
        _Contador = 0;
        lstAcompanamientos = [];
        lstProductosCompra = [];
        $.each(rta.listaProductos, function (i, item) {

            _Contador++;
            var ProductoSeleccionado2 = ObtenerProducto(tmpLista, item.Id_Producto);
            _IdPedido = item.Id_Pedido;
            ProductoSeleccionado2[0].Consecutivo = _Contador;
            ProductoSeleccionado2[0].Cantidad = 1;

            lstProductosCompra.push(ProductoSeleccionado2[0]);


            $.each(rta.listaAcompa, function (j, item2) {
                if (item2.IdDetallePedido == item.IdDetallePedido) {
                    var myObj = {};
                    myObj.IdProducto = item2.Id_Producto;
                    myObj.IdAcomp = item2.Id_Acomp;
                    myObj.Consecutivo = _Contador;
                    myObj.Nombre = item2.Nombre;
                    myObj.Observaciones = "";
                    lstAcompanamientos.push(myObj);
                }
            });
        });

        ActualizarTablaCompras2();

        finalizarProceso();
        mostrarAlerta("Información", "Productos cargados", "success");
    } else {

    }
}
function ConfirmaAnulacion() {
    var idmesa = $("#IdPuntoOrigen").val();

    EjecutarAjaxJson(urlBase + "PedidoA/AnularPedido", "post", {
        IdPedido: _IdPedido
    }, "successAnulacionPedido", null);


}
function ConfirmarCierre() {
    var idmesa = $("#IdPuntoOrigen").val();

    EjecutarAjaxJson(urlBase + "PedidoA/CerrarMesa", "post", {
        IdPedido: _IdPedido
    }, "successCierreMesa", null);


}

function listarMesaporZona() {
    var IdZonaOrigen = $("#IdZonaOrigen").val();

    if (IdZonaOrigen !== null && IdZonaOrigen !== '') {


        EjecutarAjaxJson(urlBase + "PedidoA/ListarMesasZona", "post", {
            IdZona: IdZonaOrigen
        }, "successListarMesaZona", null);
    }
    else {
        var select = document.getElementById("IdPuntoOrigen");
        document.getElementById("IdPuntoOrigen").innerHTML = ""

        var option1 = document.createElement("option");
        option1.text = "*Seleccione..."; option1.value = "";
        option1.value = "";
        select.add(option1);
    }

}

function ConfirmarPago() {
    cerrarModal('modalCRUD');
    var idmesa = $("#IdPuntoOrigen").val();
    var NombreCCli = $("#inputNombreCliente").val();

    EjecutarAjaxJson(urlBase + "PedidoA/PagarCompra", "POST", {
        ListaProductos: lista,
        ListaAcompa: lstAcompanamientos,
        Mesa: idmesa,
        NombreCliente: NombreCCli
    }, "successPagarPos", { div: "#Mesas" });


}
function DeleteProductoCarga(Id) {

    var id = _idProductocarga;
    var con = _idDetalleAcompa;
    var idDetalle = _idDetalle;
    _idtemp = id;
    _contemp = con;
    _idDetalletemp = idDetalle;
    if (_IdPedido > 0) {
        EjecutarAjax(urlBase + "PedidoA/ValidarEstadoPedido", "GET", { IdPedido: _IdPedido }, "EliminarProductoTabla", null);
    }
    else {
        if (lstProductosCompra.length == 1) {
            lstProductosCompra = [];
            lstAcompanamientos = [];
        }
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
                            var lstFinalAcompa = lstAcompanamientos;

                            $.each(lstFinalAcompa, function (j) {
                                if (lstFinalAcompa[j].IdProducto === id) {
                                    lstAcompanamientos.splice(j, 1);
                                }
                            });
                        }
                        return false;
                    }
                });
            } else {
                let lstFinalAcompa = [];
                lstFinalAcompa = lstAcompanamientos;
                $.each(lstProductosCompra, function (i) {
                    if (lstProductosCompra[i].IdProducto === id && lstProductosCompra[i].Consecutivo === con) {
                        if (lstProductosCompra[i].CodigoSap === parametros.CodSapPrecioTarjeta.Valor || lstProductosCompra[i].CodigoSap === parametros.CodSapClienteFan.Valor || lstProductosCompra[i].CodigoSap === parametros.CodSapReposicionTarjeta.Valor) {
                            var idtest = lstProductosCompra[i].DataExtension;
                            lstProductosCompra.splice(i, 1);
                            var lstFinalProdu = $.grep(lstProductosCompra, function (e) {
                                return (e.DataExtension != idtest && e.ConseutivoDetalleProducto != idtest)
                            });
                            lstProductosCompra = lstFinalProdu;
                        } else {
                            lstProductosCompra.splice(i, 1);
                            let k = 0;
                            while (k < lstAcompanamientos.length) {
                                var item = lstAcompanamientos[k];

                                if (item.IdProducto === id && item.Consecutivo === con) {
                                    lstAcompanamientos.splice(k, 1);
                                } else {
                                    k += 1;
                                }

                            }



                        }

                        return false;
                    }
                });
            }
        }
        ActualizarTablaCompras2();
    }


}

function ActualizarTablaCompras2() {
    //ActualizarListaDePrecios();


    if (lstProductosCompra.length > 0) {
        var tablaHead = " <table id='datatable-responsive' class='table table-striped jambo_table' width='100%'>";
        tablaHead += "<thead>" + "<th>Nombre</th>"
            + "<th>Descripción</th>"
            + "<th>Valor</th>"
            + "<th></th>"
            + "</thead>";
        var tablaBody = "<tbody>";
        var UltimoIdProducto = "";
        var disabledCantidad = "";
        var TotalPedidoP = 0;
        $.each(lstProductosCompra, function (i, item) {
            var IdProducto = item.IdDetalleProducto == null || item.IdDetalleProducto === undefined ? "0" : item.IdDetalleProducto;
            UltimoIdProducto = item.IdProducto;
            disabledCantidad = "";
            TotalPedidoP = TotalPedidoP + item.Precio;
            //if (validaProductoAgrupaCantidad(item.CodSapTipoProducto, item.CodigoSap)) {
            //    disabledCantidad = "disabled";
            //}
            var NombreAcom = ObtenerAcompanamientos(lstAcompanamientos, item.IdProducto, item.Consecutivo);
            var banderacheck = "";
            if (item.Entregado == true) {
                banderacheck = "checked";
            }

            tablaBody += "<tr>"
                //+ "<td style='vertical-align: middle;'>" + (item.ConseutivoDetalleProducto == null ? item.Nombre : item.Nombre + " " + item.ConseutivoDetalleProducto) + "</td>"
                + "<td style='vertical-align: middle;'>" + item.Nombre + "</td>"
                + "<td style='vertical-align: middle;'>" + NombreAcom + "</td>"
                + "<td style='vertical-align: middle;'>" + EnMascarar(item.Precio) + "</td>"
                + "<td style='vertical-align: middle;'> <input type='checkbox' class='evtCambiarcheck'  value='" + item.IdProducto + "' id='" + item.Consecutivo + "' " + banderacheck + "/> <a data-id ='" + item.IdProducto + "' data-con ='" + item.Consecutivo + "' class='evtEliminar' id='" + IdProducto + "'><span class='fa fa-trash-o IconosPos' aria-hidden='true' ></span></a></td></tr>";

        });

        var footer = "</tbody></table>";
        $("#dvProductos").html(tablaHead + tablaBody + footer);
        $("#totalPedido").html("<h1 style='font-weight: bold;'> $" + EnMascarar(TotalPedidoP) + "</h1>");
        //MostrarTotal();
        //ValidarCantidadConvenio();

        //var searchInput = $("#prod_compra_" + UltimoIdProducto);
        //if (!searchInput.prop("disabled")) {
        //    // Multiply by 2 to ensure the cursor always ends up at the end;
        //    // Opera sometimes sees a carriage return as 2 characters.
        //    var strLength = searchInput.val().length * 2;
        //    searchInput.focus();
        //    searchInput[0].setSelectionRange(strLength, strLength);
        //}

        $(".evtCambiarcheck").change(function () {
            var bandera;
            if ($(this).is(':checked')) {
                bandera = true;
            } else {
                bandera = false;
            }
            var id = parseInt($(this).attr("id"));

            $.each(lstProductosCompra, function (i) {
                if (lstProductosCompra[i].Consecutivo === id) {
                    lstProductosCompra[i].Entregado = bandera;
                    return false;
                }
            });



        });



        $(".evtEliminar").click(function () {
            _idProductocarga = $(this).data('id');
            _idDetalleAcompa = $(this).data('con');
            _idDetalle = $(this).attr("id");
            MostrarConfirm("Importante", "¿Está seguro que desea eliminar este producto?", "DeleteProductoCarga", $(this).data("id"));
            //if (confirm("Está seguro que desea eliminar este usuario?"))
            //    EjecutarAjax(urlBase + "Usuarios/Delete", "GET", { id: $(this).data("id") }, "successDelete", null);
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


    } else {
        /*Limpiar();*/
        $("#dvProductos").html("");
    }
}

function ActualizarTablaCompras() {
    //ActualizarListaDePrecios();


    if (lstProductosCompra.length > 0) {
        var tablaHead = "<table id='datatable-responsive' class='table table-striped jambo_table' width='100%'>";
        tablaHead += "<thead>" + "<th>Nombre</th>"
            + "<th>Valor</th>"
            + "<th>Cantidad</th>"
            + "<th>Total</th>"
            + "<th></th>"
            + "</thead>";
        var tablaBody = "<tbody>";
        var UltimoIdProducto = "";
        var disabledCantidad = "";
        $.each(lstProductosCompra, function (i, item) {
            var IdProducto = item.IdDetalleProducto == null || item.IdDetalleProducto === undefined ? "0" : item.IdDetalleProducto;
            UltimoIdProducto = item.IdProducto;
            disabledCantidad = "";
            //if (validaProductoAgrupaCantidad(item.CodSapTipoProducto, item.CodigoSap)) {
            //    disabledCantidad = "disabled";
            //}

            tablaBody += "<tr>"
                //+ "<td style='vertical-align: middle;'>" + (item.ConseutivoDetalleProducto == null ? item.Nombre : item.Nombre + " " + item.ConseutivoDetalleProducto) + "</td>"
                + "<td style='vertical-align: middle;'>" + item.Nombre + "</td>"

                + "<td style='vertical-align: middle;'>" + EnMascarar(item.Precio) + "</td>"
                + "<td style='vertical-align: middle;' width='20px'>" + "<input data-id ='" + item.IdProducto + "' type='text' style='text-align: center; padding-top: 0; padding-bottom: 0; height: 22px;' class='form-control evtCambiar' id='prod_compra_" + item.IdProducto + "'" + "value='" + item.Cantidad + "' maxlength='3'  onkeypress='return EsNumero(this);' " + disabledCantidad + " />" + "</td>"
                + "<td style='vertical-align: middle;' width='100px'>" + EnMascarar(item.PrecioTotal) + "</td>"
                + "<td style='vertical-align: middle;'><a data-id ='" + item.IdProducto + "' class='evtEliminar' id='" + IdProducto + "'><span class='fa fa-trash-o IconosPos' aria-hidden='true' ></span></a></td></tr>";

        });

        var footer = "</tbody></table>";
        $("#dvProductos").html(tablaHead + tablaBody + footer);
        //MostrarTotal();
        //ValidarCantidadConvenio();

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

            //$.each(lstProductosCompra, function (i) {
            //    if (lstProductosCompra[i].IdProducto === id) {
            //        lstProductosCompra[i].Cantidad = NuevaCantidad;
            //        lstProductosCompra[i].PrecioTotal = (lstProductosCompra[i].Precio * NuevaCantidad);
            //        return false;
            //    }
            //});

            ActualizarTablaCompras();

        });
        $(".evtEliminar").click(function () {
            _idProductocarga = $(this).data('id');
            _idDetalleAcompa = $(this).attr("id");
            MostrarConfirm("Importante", "¿Está seguro que desea desactivar este usuario?", "DeleteProductoCarga", $(this).data("id"));
            //if (confirm("Está seguro que desea eliminar este usuario?"))
            //    EjecutarAjax(urlBase + "Usuarios/Delete", "GET", { id: $(this).data("id") }, "successDelete", null);
        });

        function DeleteProductoCarga(Id) {
            var id = _idProductocarga;
            var idDetalle = _idDetalleAcompa;

            if (lstProductosCompra.length == 1) {
                lstProductosCompra = [];
                lstAcompanamientos = [];
            }
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
                                var lstFinalAcompa = lstAcompanamientos;

                                $.each(lstFinalAcompa, function (j) {
                                    if (lstFinalAcompa[j].IdProducto === id) {
                                        lstAcompanamientos.splice(j, 1);
                                    }
                                });
                            }
                            return false;
                        }
                    });
                } else {
                    let lstFinalAcompa = [];
                    lstFinalAcompa = lstAcompanamientos;
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
                                let k = 0;
                                while (k < lstAcompanamientos.length) {
                                    var item = lstAcompanamientos[k];

                                    if (item.IdProducto === id) {
                                        lstAcompanamientos.splice(k, 1);
                                    } else {
                                        k += 1;
                                    }

                                }



                            }

                            return false;
                        }
                    });
                }
            }
            ActualizarTablaCompras();

        }

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


    } else {
        /*Limpiar();*/
        $("#dvProductos").html("");
    }
}



//function successInsert(rta, id) {

//    $("#diverror").hide();
//    $(".alert-notification").hide();

//    EjecutarAjax(urlBase + "ReservaSkycoaster/ObtenerLista", "GET", { id: id }, "printPartial",
//        {
//            div: "#listView", func: "setEventEdit"
//        });

//    if (rta.Correcto) {
//        cerrarModal("modalCRUD");
//        //MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "ReservaSkycoaster/Index?id=" + id, "success");
//        MostrarMensaje("Importante", "Su operación fue exitosa.", "success");
//    }
//    else {
//        cerrarModal("modalCRUD");
//        MostrarMensaje("Importante", rta.Mensaje, "info");
//    }
//}

$("#btnSavePedidoP").click(function () {
    Banderasave = 0;
})
$("#btnNuvMesa").click(function () {
    if (Banderasave == 0) {
        //$("#ConMesa").hide();
        //$("#btnSaveAPedido").show();
        //$("#ContenedorCarta").hide();
        //$("#ConFactura").hide();
        //$("#IdPuntoOrigen").prop("disabled", false);
        Banderasave = 1;
        _IdPedido = 0;
        lstAcompanamientos = [];
        lstProductosCompra = [];
        window.location = urlBase + "PedidoA";
    }
    else {
        MostrarMensaje("Importante", "Guarde el pedido actual antes de abrir otra mesa.", "error");
    }

})
$("#btnCancelarMesa").click(function () {
    //$("#ConMesa").hide();
    //$("#btnSaveAPedido").show();
    //$("#ContenedorCarta").hide();
    //$("#ConFactura").hide();
    //$("#IdPuntoOrigen").prop("disabled", false);
    //Banderasave = 1;
    window.location = urlBase + "PedidoA";
})

$("#btnSaveAPedido").click(function () {
    var mesaSele = $("#IdPuntoOrigen").val();
    if (mesaSele !== null && mesaSele !== '') {
        $("#btnSaveAPedido").hide();
        $("#ContenedorCarta").show();
        $("#ConFactura").show();
        $("#ConMesa").show();
        $("#DivZonaOrigen").hide();
        $("#IdPuntoOrigen").prop("disabled", true);
    }
    else {
        MostrarMensaje("Importante", "Seleccione una mesa para abrir.", "error");
    }


})

function setEventEdit() {
    EstablecerToolTipIconos();
    $('.Acompa').click(function () {

        acompa = "prueba";


        EjecutarAjax(urlBase + "PedidoA/Acompa", "GET", { acompa: acompa }, "printPartialModal", {
            title: "Detalle Acompañamientos", hidesave: false, modalLarge: false, OcultarCierre: true
        });

    });

    $('.MesasBusq').click(function () {
        _idmesa = $(this).data("id");
        var nombremesa = $(this).data("nombre");

        $("#TabPedido").addClass('active');
        $("#TabMesas").removeClass('active');
        $("#home").addClass('active in');
        $("#menu2").removeClass('active in');

        $("#btnSaveAPedido").hide();
        $("#DivZonaOrigen").hide();
        $("#ContenedorCarta").show();
        $("#ConFactura").show();
        $("#ConMesa").show();
        $('#IdPuntoOrigen').prepend("<option value='" + _idmesa + "' >" + nombremesa + "</option>");
        $("#IdPuntoOrigen").val(_idmesa);
        //$("#IdPuntoOrigen option[value='" + idmesa + "']").attr("selected", true);
        $("#IdPuntoOrigen").prop("disabled", true);
        Banderasave = 1;


        EjecutarAjaxJson(urlBase + "PedidoA/ListarProductosMesa", "post", {
            IdMesa: _idmesa
        }, "successListarProdMesa", null);


    });
    $('.conusultaFactura').click(function () {
        var idFactura = $("#inputFacturaProd").val();
        if (idFactura !== '' && idFactura !== null) {
            EjecutarAjaxJson(urlBase + "PedidoA/ListarProductosFactura", "post", {
                IdFactura: idFactura
            }, "successListarProdFactura", null);
        }
        else {
            MostrarMensaje("Importante", "Seleccione una factura para abrir.", "error");
        }


    });

    $(".liberar").click(function () {

        HoraInicio = $(this).data("horainicio");
        HoraFin = $(this).data("horafin");
        var IdPunto = $(this).data("punto");
        var obj = { HInicio: HoraInicio, Hfinal: HoraFin, IdPunto: IdPunto };
        MostrarConfirm("Importante!", "Desea liberar esta reserva", "Liberar", obj);


    });




    $(".Cerrar").click(function () {

        HoraInicio = $(this).data("horainicio");
        HoraFin = $(this).data("horafin");
        var IdPunto = $(this).data("punto");
        var capacidad = $(this).data("capacidad");

        var obj = { HInicio: HoraInicio, Hfinal: HoraFin, IdPunto: IdPunto, Capacidad: capacidad };
        MostrarConfirm("Importante!", "Desea cerrar esta reserva", "CerrarReserva", obj);

    });

    $('.MesasBusquedaConNombresClien').click(function () {
        _idmesa = $(this).data("id");

        $("#TabPedido").addClass('active');
        $("#TabMesas").removeClass('active');
        $("#home").addClass('active in');
        $("#menu2").removeClass('active in');

        $("#btnSaveAPedido").hide();
        $("#ContenedorCarta").show();
        $("#ConFactura").show();
        $("#ConMesa").show();
        $('#IdPuntoOrigen').prepend("<option value='" + _idmesa + "' >Mesa " + _idmesa + "</option>");
        $("#IdPuntoOrigen").val(_idmesa);
        //$("#IdPuntoOrigen option[value='" + idmesa + "']").attr("selected", true);
        $("#IdPuntoOrigen").prop("disabled", true);
        Banderasave = 1;


        EjecutarAjaxJson(urlBase + "PedidoA/ListarProductosMesa", "post", {
            IdMesa: _idmesa
        }, "successListarProdMesa", null);


    });

}


//function Liberar(objeto) {

//    var _Reserva = new Object();
//    _Reserva.HoraInicio = objeto.HInicio;
//    _Reserva.HoraFin = objeto.Hfinal;
//    _Reserva.IdPunto = objeto.IdPunto;

//    EjecutarAjax(urlBase + "ReservaSkycoaster/LiberarReserva", "POST", JSON.stringify(_Reserva), "successProceso", _Reserva.IdPunto);



//}

function successProceso(rta, id) {

    $("#diverror").hide();
    $(".alert-notification").hide();
    if (rta.Correcto) {
        cerrarModal("modalCRUD");
        EjecutarAjax(urlBase + "ReservaSkycoaster/ObtenerLista", "GET", { id: id }, "printPartial",
            {
                div: "#listView", func: "setEventEdit"
            });
        MostrarMensaje("Importante", "Su operación fue exitosa.", "success");
    }
    else {
        cerrarModal("modalCRUD");
        MostrarMensaje("Importante", rta.Mensaje, "info");
    }
}

//funcion Boton imprimir prefactura
$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    var target = $(e.target).attr("href");
    if (target == "#menu22" || target == "#menu2") {
        $("#btnImprimirPreFactura").css("display", "inline-block");
        if (lstProductosCompra.length <= 0) {
            $("#btnImprimirPreFactura").prop("disabled", true);
        }
    }
    else {
        $("#btnImprimirPreFactura").css("display", "none");
    }
});

$('#btnImprimirPreFactura').click(function () {
    console.log('Mesa', $('#IdPuntoOrigen').val());
    //console.log('Total', document.getElementById("totalPedido").innerText);
    var total = document.getElementById("totalPedido").innerText;
    if (lstProductosCompra.length > 0) {
        //var lst_productos = JSON.stringify(lstProductosCompra);
        EjecutarAjaxJson(urlBase + "PedidoA/ImprimirPreFactura", "post",
            { ListaProductos: lstProductosCompra, IdMesa: _idmesa, Total: total },
            "successImpresionPreFactura", null);
    }
});

function successImpresionPreFactura(response) {
    if (response) {
        $("#btnImprimirPreFactura").prop("disabled", true);
        MostrarMensaje("Impresion Correcta", "La prefactura se ha impreso", "success");
    }
    else {
        $("#btnImprimirPreFactura").prop("disabled", false);
        MostrarMensaje("Impresion incorrecta", "La prefactura no se imprimio, intentelo de nuevo", "error");
    }
}