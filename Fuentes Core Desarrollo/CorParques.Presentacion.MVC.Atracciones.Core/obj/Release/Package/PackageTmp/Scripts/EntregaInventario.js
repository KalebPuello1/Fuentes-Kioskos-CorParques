var listaBrazalete = [];
var DetalleInventario;
var isListSupervidor = false;
var NombreSupervisor;
var NombreTaquillero;
var RetornaLogin = false;

$(".Numero:input[type=text]").mask("0000", { reverse: true });

$("#IdSupervisor").select2({
    placeholder: "* Seleccione el supervisor"
});

$("#IdTaquillero").select2({
    placeholder: "* Seleccione el taquillero"
});


function SumarTotal(ctr) {


    var split = ctr.id.split('_');
    var id = split[1];
    var idPunto = split[2];
    var Denominacion = $("#Denominacion_" + id).html().trim();
    var ctrValor;
    if (ctr.value == "") {
        ctrValor = 0;
    } else {
        ctrValor = ctr.value;
    }
    var Campo = parseInt(Denominacion) * parseInt(ctrValor);
    var Valorcampo = $("#Total_" + id + "_" + idPunto).html().trim();
    
    if (Campo == 0) {
        $("#Total_" + id + "_" + idPunto).html("");
    } else {
        $("#Total_" + id + "_" + idPunto).html(FormatoMoneda(Campo));
    }
    $("#TotalNido_" + id + "_" + idPunto).val(Campo);
    var Total = RemoverFormatoMoneda($("#Total_" + idPunto).html().trim());
    if (Valorcampo == "") {
        var TotalValor = 0
    } else {
        var TotalValor = RemoverFormatoMoneda(Valorcampo);
    }
    Total = Total - TotalValor;
    Total = parseInt(Total) + parseInt(Campo);
    $("#Total_" + idPunto).html(FormatoMoneda(Total));

}

function placeholdercero(ctr) {
    if (ctr.value == "0") {
        ctr.value = ""
    }

}

function InitPartialCreate(dataGrupos) {
    
    tagsAutocomplete($('#tagsPuntos'), $('#puntosAutocomplete'), $("#hdPuntos"), dataGrupos, false)

}

//@Eventos
$("#btnAceptarAlistamiento").click(function () {

    if (validarerror("frmInventarioOperativo")) {
        if (validarFormulario("frmInventarioOperativo")) {

            var _inventario = ObtenerObjeto("frmInventarioOperativo");
            listaBrazalete = [];

            $.each($(".Brazaletes"), function (i, item) {
                if ($(item).val().length > 0) {
                    var id = $(item).data("id");
                    var CodSap = $(item).data("valor");
                    var _obj = { "Id": id, "Cantidad": $(item).val(), "CodigoSap": CodSap };
                    listaBrazalete.push(_obj);
                }
            });

            if (listaBrazalete.length > 0)
                _inventario.push({ name: 'Brazaletes', value: listaBrazalete })

            EjecutarAjax(urlBase + "Apertura/ObtenerDetalleInventario", "GET", _inventario, "printPartialModal", {
                title: "Detalle ", hidesave: true, modalLarge: true
            });
        }
    } else {
        MostrarMensaje("Importante", "Hay inconsistencias en el formulario, revise los campos demarcados con color rojo.", "error");
    }
});

$("#btnCancelarAlistamiento").click(function () {
     MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "Cancelar", "");
});

$("#btnAceptarAlistamientoPuntos").click(function () {
    

    if (validarerror("frmInventarioOperativoPuntos")) {
        if (validarFormulario("frmInventarioOperativoPuntos")) {

            NombreTaquillero = $("#IdTaquillero").find('option:selected').text();
            ListaDetalle();
        }
    } else {
        MostrarMensaje("Importante", "Hay inconsistencias en el formulario, revise los campos demarcados con color rojo.", "error");
    }
});

$("#btnCancelarAlistamientoPuntos").click(function () { 
     MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "CancelarAlistamientoPunto", "");
});

$("#frmInventarioOperativo #IdSupervisor").change(function () {

    var idSupervisor = $(this).val();
    NombreSupervisor = $(this).find('option:selected').text();
    if (idSupervisor.length > 0) {
        RespuestaValidSupervidor(idSupervisor);
    }

});
//@Fin Eventos

//@Inicio funciones 

function Cancelar() {
    window.location = urlBase + "Apertura/EntregaInventario";
}

function CancelarAlistamientoPunto() {
    window.location = urlBase + "Apertura/EntregaInventarioPuntos";
}

function SuccessValidSuper(r) {

    if (r != null && r.length > 0) {
        isListSupervidor = true;
        $.each(r, function (i, item) {
            $.each($(".Brazaletes"), function (a, b) {
                var id = $(this).data("id");
                if (id == item.IdBrazalete) {
                    $(this).val(item.CantidadFinal);
                }
                $(this).attr("readonly", true);
            });
        });
    } else {
        isListSupervidor = false;
        $.each($(".Brazaletes"), function (i, item) {
            $(this).attr("readonly", false);
            $(this).val("");
        });
    }

}

function RespuestaValidSupervidor(idSupervisor) {

    //Cambio para ejecutar el Ajax general
    EjecutarAjax(urlBase + "Apertura/ObtenerBrazaletesPorSupervisor", "GET", { IdSupervisor: idSupervisor }, "SuccessValidSuper", null);
    
    
    //$.ajax({
    //    url: urlBase + 'Apertura/ObtenerBrazaletesPorSupervisor',
    //    ContentType: 'application/json; charset-uft8',
    //    dataType: 'JSON',
    //    async: false,
    //    type: 'GET',
    //    data: {
    //        IdSupervisor: idSupervisor
    //    },
    //    success: function (r) {
    //        
    //        if (r != null && r.length > 0) {
    //            isListSupervidor = true;
    //            $.each(r, function (i, item) {
    //                $.each($(".Brazaletes"), function (a, b) {
    //                    var id = $(this).data("id");
    //                    if (id == item.IdBrazalete) {
    //                        $(this).val(item.CantidadFinal);
    //                    }
    //                    $(this).attr("readonly", true);
    //                });
    //            });
    //        } else {
    //            isListSupervidor = false;
    //            $.each($(".Brazaletes"), function (i, item) {
    //                $(this).attr("readonly", false);
    //                $(this).val("");
    //            });
    //        }
    //    },
    //    error: function (a, b, c) {
    //        alert(" Error " + a.responseText);
    //    }
    //});

}

function AlistarListaApertura() {
    if (DetalleInventario != null) {

        //Denominacion moneda
        $.each($(".ValorSupervisor"), function (i, item) {
            var denominacion = $(item).data("id").split("|");
            var idDenominacion = denominacion[0]; //Primer caracter Id denominacion
            var idApertura = denominacion[1]; // Segundo caractes Id Apertura           
            var _obj = ObtenerObjetoAperturaBase(idApertura, idDenominacion);
            if (_obj != null) {
                _obj.CantidadSupervisor = $(item).val();
                _obj.TotalSupervisor = $(item).val().length > 0 ? parseInt(ObtenerTotalSupervisor(idDenominacion, $(item).val())) : 0;
            } else {
                if (parseInt($(item).val()) > 0) {
                    var _objBase = {
                        IdAperturaBase: 0,
                        CantidadNido: 0,
                        TotalNido: 0,
                        TotalSupervisor: 0,
                        IdTipoDenominacion: idDenominacion,
                        IdApertura: idApertura,
                        CantidadSupervisor: $(item).val(),
                        TotalSupervisor: $(item).val().length > 0 ? parseInt(ObtenerTotalSupervisor(idDenominacion, $(item).val())) : 0,
                        CantidadPunto: 0,
                        TotalPunto: 0
                    };
                    $.each(DetalleInventario.Apertura, function (i, item) {
                        if (item.Id == idApertura) {
                            item.AperturaBase.push(_objBase);
                        }
                    });

                }
            }

        });

        //Elementos 
        $.each($(".Elementos"), function (i, item) {
            
            var elementos = $(item).data("id").split("|");
            var idElemento = elementos[0]; //Primer caracter Id elemento
            var idApertura = elementos[1]; // Segundo caractes Id Apertura
            var _obj = ObtenerObjetoAperturaElemento(idApertura, idElemento);
            _obj.ValidSupervisor = $(this).is(':checked');
        });

        //Lista de brazaletes
        if (!isListSupervidor)
            DetalleInventario.Brazaletes = listaBrazalete;

    }


    EjecutarAjax(urlBase + "Cuenta/ObtenerLogin", "GET", null, "printPartialModal", {
        title: "Confirmación supervisor", hidesave: true, modalLarge: false
    });
}


function ObtenerObjetoAperturaBase(IdApertura, IdTipoDenominacion) {
    var rta;

    $.each(DetalleInventario.Apertura, function (i, item) {
        if (item.Id == IdApertura) {
            $.each(item.AperturaBase, function (a, item2) {
                if (item2.IdTipoDenominacion == IdTipoDenominacion)
                    rta = item2;
            });
        }
    });

    return rta;
}

function ObtenerObjetoAperturaElemento(IdApertura, Id) {
    var rta;

    $.each(DetalleInventario.Apertura, function (i, item) {
        if (item.Id == IdApertura) {
            $.each(item.AperturaElemento, function (a, item2) {
                if (item2.Id == Id)
                    rta = item2;
            });
        }
    });

    return rta;
}

function ObtenerObjetoAperturaBrazalete(IdApertura, IdTipoBrazalete) {
    var rta;

    $.each(DetalleInventario.Apertura, function (i, item) {
        if (item.Id == IdApertura) {
            $.each(item.AperturaBrazalete, function (a, item2) {
                if (item2.IdBrazalete == IdTipoBrazalete)
                    rta = item2;
            });
        }
    });

    return rta;
}

function ObtenerTotalSupervisor(id, cantidad) {

    var total = 0;

    $.each(DetalleInventario.TipoDenomicacionMoneda, function (i, item) {
        if (item.IdTipoDenominacion == id)
            total = parseInt(item.Denominacion) * parseInt(cantidad);
    })

    return total;
}

function Login(password, observaciones) {

    //Setear la observacion del supervisor 
    


    //Dependiendo del formulario envia el id del supervidor o del taquillero 
    var _idUsuario = 0;

    if ($("#frmInventarioOperativo").length > 0) {

        $.each(DetalleInventario.Apertura, function (i, item) {
            item.ObservacionSupervisor = observaciones;
        });

        _idUsuario = DetalleInventario.IdSupervisor;
    }

    if ($("#frmInventarioOperativoPuntos").length > 0) {

        _idUsuario = $("#IdTaquillero").val();
        
        $.each(DetalleInventario.Apertura, function (i, item) {
            item.IdTaquillero = _idUsuario;
        });

        $.each(DetalleInventario.Apertura, function (i, item) {
            item.ObservacionPunto = observaciones;
        });

    }


    EjecutarAjax(urlBase + "Cuenta/ValidarPassword", "GET", {
        idUsuario: _idUsuario, password: password
    }, "respuestaLogin", null);

}

function CancelarLogin() {

    if ($("#frmInventarioOperativo").length > 0) {
        //Nido Supervisor mostrar detalle
        RetornaLogin = true;
        $("#btnAceptarAlistamiento").click();

    }            
    else if ($("#frmInventarioOperativoPuntos").length > 0)
        //Supervisor - taquillero , se cierra
        cerrarModal('modalCRUD');
}

function respuestaLogin(data) {
    if (data.Correcto) {

        if ($("#frmInventarioOperativo").length > 0)
            urlaction = urlBase + "Apertura/GuardarInformacionInventario";
        else if ($("#frmInventarioOperativoPuntos").length > 0)
            urlaction = urlBase + 'Apertura/ActualizarInformacionInventario';
        //

        $.ajax({
            url: urlaction,
            ContentType: 'application/json; charset-uft8',
            dataType: 'JSON',
            async: false,
            type: 'POST',
            data: {
                modeloInventario: DetalleInventario
            },
            success: function (r) {
                RespuestaActualizarInventario(r);
            },
            error: function (a, b, c) {
                alert(" Error " + a.responseText);
            }
        })

        cerrarModal("modalCRUD");
    } else {
        MostrarMensaje("Mensaje", "Contraseña incorrecta");
    }
}

function RespuestaActualizarInventario(data) {
    if (data.Correcto) {

        var urlaction = "";

        if ($("#frmInventarioOperativo").length > 0)
            urlaction = "Apertura/EntregaInventario";
        else if ($("#frmInventarioOperativoPuntos").length > 0)
            urlaction = 'Home/Index';

        //Limpiar();
        MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", urlaction, "success");

    }
    else {
        MostrarMensaje("Mensaje", "Error comuniqueselo al administrador");
    }
}

function Limpiar() {
    listaBrazalete = [];
    DetalleInventario = 'undefined';
    $.each($(".Brazaletes"), function (i, item) {
        $(item).val("");
    });

    $("#hdPuntos").val("");

    var $lt = $("#IdSupervisor").select2();
    $lt.val('').trigger("change");

    $("#tagsPuntos").importTags('');
    $("#Observacion").val("");
    NombreSupervisor = "";

}

function ListaDetalle() {
    if (DetalleInventario != null) {

        //Denominacion moneda
        $.each($(".ValorSupervisor"), function (i, item) {
            var denominacion = $(item).data("id").split("|");
            var idDenominacion = denominacion[0]; //Primer caracter Id denominacion
            var idApertura = denominacion[1]; // Segundo caractes Id Apertura           
            var _obj = ObtenerObjetoAperturaBase(idApertura, idDenominacion);
            if (_obj != null) {
                _obj.CantidadPunto = $(item).val();
                _obj.TotalPunto = $(item).val().length > 0 ? parseInt(ObtenerTotalSupervisor(idDenominacion, $(item).val())) : 0;
            } else {
                
                if (parseInt($(item).val()) > 0) {
                    var _objBase = {
                        IdAperturaBase: 0,
                        CantidadNido: 0,
                        CantidadSupervisor: 0,
                        TotalNido: 0,
                        TotalSupervisor: 0,
                        IdTipoDenominacion: idDenominacion,
                        IdApertura: idApertura,
                        CantidadPunto: $(item).val(),
                        TotalPunto: $(item).val().length > 0 ? parseInt(ObtenerTotalSupervisor(idDenominacion, $(item).val())) : 0
                    };
                    DetalleInventario.Apertura[0].AperturaBase.push(_objBase);
                }

            }

        });

        //Elementos 
        $.each($(".Elementos"), function (i, item) {
            
            var elementos = $(item).data("id").split("|");
            var idElemento = elementos[0]; //Primer caracter Id elemento
            var idApertura = elementos[1]; // Segundo caractes Id Apertura
            var _obj = ObtenerObjetoAperturaElemento(idApertura, idElemento);
            _obj.ValidTaquilla = $(this).is(':checked');
        });

        //Brazalete
        $.each($(".Brazaletes"), function (i, item) {
            var denominacion = $(item).data("id").split("|");
            var idTipoBrazalete = denominacion[0]; //Primer caracter Id denominacion
            var idApertura = denominacion[1]; // Segundo caractes Id Apertura           
            var _obj = ObtenerObjetoAperturaBrazalete(idApertura, idTipoBrazalete);
            if (_obj != null) {
                _obj.BrazaleteDetalle.IdTaquillero = $("#IdTaquillero").val();
                _obj.BrazaleteDetalle.Cantidad = $(item).val();
                _obj.BrazaleteDetalle.CodigoSap =  $(item).data("codigosap")
            }
        });

    }

    
    EjecutarAjaxJson(urlBase + "Apertura/ObtenerDetalleInventarioPuntos", "POST", { modelo: DetalleInventario }, "printPartialModal", {
        title: "Confirmación supervisor", hidesave: true, modalLarge: false
    });
}

function ValidateCount(ctrl) {
   $(ctrl).attr("data-mensajeerror", "");
    var cantmax = $(ctrl).data("value");
    $(ctrl).removeClass("errorValidate");
    $(ctrl).removeClass("errorBrazalete"); 
    if (parseInt(cantmax) < parseInt($(ctrl).val())) {
        $(ctrl).attr("data-mensajeerror", "No se puede asigar mas brazaletes de los existentes");
        $(ctrl).addClass("errorValidate");
        $(ctrl).addClass("errorBrazalete");
        mostrarTooltip();
        return false;
    }
    return true;
}

function validarerror(formulario) {
    var requeridos = $("#" + formulario).find(".errorBrazalete");
    var correcto = true;
    $.each(requeridos, function (index, value) {
        correcto = false;
    });
    return correcto;
}


//@Fin Funciones