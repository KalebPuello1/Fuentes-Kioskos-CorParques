//@Variables globales

var _listaProductos = [];
var inicializadointerval = false;
var cantidadBoletasControl = 1;
var proceso = true;
var proceso2 = true;
var TipoProductos = [];
var CantidadMostrar = 1;


//@Init
$(function () {

    $('#CodBarra1').keyup(function (e) {

        //if (e.which != 13)
        //    return false;
        var letters = /^[A-Za-z0-9]+$/;
        if (!e.key.match(letters)) {
            this.value = this.value + String.fromCharCode(45)
            return false;
        }
        if (!inicializadointerval) {
            inicializadointerval = true;
            var refreshIntervalId = setInterval(function () { ConsultarCodBarra1(); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);

        }
        
        //if (e.which == 13 && $(this).val().length > 0) {
       
        //    return false;
        //}
    });

   

    $("#CodBarra2").keyup(function (e) {
        var letters = /^[A-Za-z0-9]+$/;
        if (!e.key.match(letters)) {
            this.value = this.value + String.fromCharCode(45)
            return false;
        }
        if (!inicializadointerval) {
            inicializadointerval = true;
            var refreshIntervalId = setInterval(function () { ConsultarCodBarra2(); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);

        }
    });

    $("#btnDescargar").prop("disabled", true);
   

});
//@Fin 

//@Inicio Eventos
$("#btnDescargar").click(function () {

    if (validarFormulario("frmDescargueBoleta")) {
        MostrarConfirm("Importante!", "¿Está seguro de descargar estos productos? ", "DescargaProducto", "");
    }
});

$("#btnCancelar").click(function () {
    MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "Cancelar", "");
});

//@Fin Eventos

//@Inicio funciones 

function ConsultarCodBarra1() {
    var obj = $("#CodBarra1");
    if (obj.val().length > 0) {
        ValidarBoleta(obj.val());
    }
}

function ConsultarCodBarra2() {
    var obj = $("#CodBarra2");
    var _cod1 = $("#CodBarra1").val();

    if (_cod1.length == 0) {
        MostrarMensaje("Importante", "No ha digitado un código de inicio", "");
        return;
    }

    if (obj.val().length > 0) 
        ValidarBoleta2(obj.val());
    else
    {
        cantidadBoletasControl = 1;
        MostrarLabelCantidadBtaCtrol(cantidadBoletasControl);
        ActualizarCantidades();
    }

    //Limpiar texbox de productos
    $.each($(".CodBarraInicio"), function (i, item) {
        $(item).val("");
    })

    $.each($(".CodBarraFin"), function (a, b) {
        $(b).val("");
    });
    
}

function ConsultarProd1(id, cantidad, Codigo) {
    
    id = id.toString();
    Codigo = $("#id_" + id).val();
    if (Codigo.length > 0 && id.length > 0 && cantidad > 0 && proceso) {
        proceso = false;
        var codFinal = $("#id2_" + id).val();
        if (codFinal.length > 0)
            $("#id2_" + id).val("");

        EjecutarAjax(urlBase + "DescargueBoleta/BuscarBoleta", "GET", { CodBarraInicio: Codigo, CodBarraFinal: Codigo, Codproducto: id }, "successBuscarProd", cantidad);
    }
    
}

function ConsultarProd2(id, cantidad, Codigo) {
    
    id = id.toString();
    Codigo = $("#id2_" + id).val();
    if (Codigo.length > 0 && id.length > 0 && cantidad > 0 && proceso2) {
        proceso2 = false;
        var codInicial = $("#id_" + id).val();
        if (codInicial.length == 0) {
           
            $("#id2_" + id).val("");
            MostrarMensaje("Importante", "No ha digitado un código de inicio", "");
            
        } else {
            
            var cod1 = $("#id_" + id).val();
            EjecutarAjax(urlBase + "DescargueBoleta/BuscarBoleta", "GET", { CodBarraInicio: cod1, CodBarraFinal: Codigo, Codproducto: id }, "successBuscarProd2", cantidad);
        }
        
    }
}

function successBuscarProd2(rta, cantidad) {
    
    var cantidadTotal = parseInt(cantidad) * parseInt(cantidadBoletasControl);
    if (rta.Mensaje.length > 0) {
        MostrarMensaje("Importante", rta.Mensaje, "");
        $("#id2_" + rta.CodSapProducto).val("");
    } else {
        if (rta.Cantidad != cantidadTotal) {
            MostrarMensaje("Importante", "No coincide la cantidad de boleta del producto", "");
            $("#id2_" + rta.CodSapProducto).val("");
        }
    }

    proceso2 = true;
}




function successBuscarProd(rta, cantidad) {
    var cantidadTotal = parseInt(cantidad) * parseInt(cantidadBoletasControl);
    if (rta.Mensaje.length > 0) {
        MostrarMensaje("Importante", rta.Mensaje, "");
        $("#id_" + rta.CodSapProducto).val("");
    }
    else {
        //if (rta.Cantidad != cantidadTotal) {
        //    MostrarMensaje("Importante", "No coincide la cantidad de boleta del producto", "");
        //    $("#id_" + rta.CodSapProducto).val("");
        //}
    }

    proceso = true;
}

function ValidarBoleta(cod) {
    iniciarProceso() //
    EjecutarAjax(urlBase + "DescargueBoleta/ObtenerProductos", "GET", { CodBarra: cod }, "successProd", null,funcErrorView);
}

function ValidarBoleta2(cod) {
    var codBarra1 = $("#CodBarra1").val();
    var CodBarra2 = cod;
    var CodSap = 0;

    EjecutarAjax(urlBase + "DescargueBoleta/BuscarBoleta", "GET", { CodBarraInicio: codBarra1, CodBarraFinal: CodBarra2, Codproducto: CodSap }, "successBusqueda", null,funcErrorView);

}

var funcErrorView = function (err,data) {
    MostrarMensajeRedireccion("Error", "Se ha presentado un error al realizar la redencion de boleta control. por favor intente de nuevo.", "DescargueBoleta", "error");

}
function successBusqueda(rta) {
    if (rta.Mensaje.length > 0) {
        if (rta.CodSapProducto == "0")
            $("#CodBarra2").val("");
        cantidadBoletasControl = 1;
        MostrarLabelCantidadBtaCtrol(cantidadBoletasControl);
        MostrarMensaje("Importante", rta.Mensaje , "");
    } else {
        cantidadBoletasControl = rta.Cantidad;
        MostrarLabelCantidadBtaCtrol(cantidadBoletasControl);
      
    }
    ActualizarCantidades();
}

function MostrarLabelCantidadBtaCtrol(cantidad) {
    $("#cantidadBoletaCtrol").html("Cantidad boleta control : " + cantidad);
}

function ActualizarCantidades() {
    $.each($(".cantidad"), function (i, item) {
        var cantidadOriginal = $(item).data("original");
        var CodSap = $(item).data("id");
        var cantidadFinal = parseInt(cantidadOriginal) * cantidadBoletasControl;

        ActivarRequied(CodSap, cantidadFinal);
        SetCantidadProducto(CodSap, cantidadFinal);
        $(item).html(cantidadFinal);
        ValidarMostrarProductos();
    });
}

function ActivarRequied(CodSap, cantidad) {
    $.each($(".CodBarraFin"), function (i, item) {
        var cod = $(item).data("id");
        
        if (CodSap == cod) {
            if(cantidad == 1)
                $(item).removeClass("required");
            else
                $(item).addClass("required");
        }

    });

}

function SetCantidadProducto(CodSap, Cantidad) {
    $.each(_listaProductos, function (i, item) {
        if (item.CodigoSap == CodSap) {
            item.Cantidad = Cantidad;
            return false;
        }

    });
}

function successProd(rta) {
    if (rta.length > 0) {
        TipoProductos = [];
        CantidadMostrar = 1;
        $("#listproductos").html(rta);
        $("#CodBarra2").prop('disabled', false);
        $("#btnDescargar").prop("disabled", false);
    }
    finalizarProceso(); //
}

function Cancelar() {
    window.location = urlBase + "DescargueBoleta";
}

function DescargaProducto() {

    

    if (_listaProductos.length > 0) {
        SetEventCodBarra();
        iniciarProceso()
        EjecutarAjax(urlBase + "DescargueBoleta/ImprimirCodBarras", "POST", JSON.stringify({
                        _listprod: _listaProductos, CodBarraInicio: $("#CodBarra1").val(), CodBarraFinal: $("#CodBarra2").val()
        }), "sucessCodBarra", null, funcErrorView);


    }
}

function sucessCodBarra(rta) {
    //iniciarProceso()
    if (rta.Correcto) {
        finalizarProceso()
        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "DescargueBoleta", "success");
    } else {
        MostrarMensaje("", rta.Mensaje);
    }
}

/*function iniciarProceso() {
    $(".loader-wrapper").css("display", "block");
    $("#div_message_error").hide();
}

function finalizarProceso() {
    $(".loader-wrapper").css("display", "none");
}
*/
function SetEventCodBarra() {
    //setear código de barras de los brazaletes
    //SET Código Inicio
    $.each($(".CodBarraInicio"), function (i, item) {
        var idProd = $(item).data("id");
        var valor = $(item).val();
        $.each(_listaProductos, function (a, b) {
            if (b.CodigoSap == idProd)
                b.CodBarraInicio = valor;
        });
    })

    //SET Código FIN 

    $.each($(".CodBarraFin"), function (i, item) {
        var idProd = $(item).data("id");

        $.each(_listaProductos, function (a, b) {
            if (b.CodigoSap == idProd)
                b.CodBarraFin = $(item).val();
        });
    })


}

//Mostrar los productos que se pueden pistolear
function MostrarProductos() {
    $.each(_listaProductos, function (i, item) {
        if (BuscarTipoProductos(item.CodSapTipoProducto)) {
            
            $("#id_" + item.CodigoSap).show();
            $("#id_" + item.CodigoSap).addClass("required");
            $("#id2_" + item.CodigoSap).show();
            if (ObtenerCantidadProducto(item.CodigoSap) > 1)
                $("#id2_" + item.CodigoSap).addClass("required");
        }
        else {
            $("#id_" + item.CodigoSap).removeClass("required");
            $("#id_" + item.CodigoSap).hide();
            $("#id2_" + item.CodigoSap).removeClass("required");
            $("#id2_" + item.CodigoSap).hide();
        }
    });
}


function BuscarTipoProductos(TipoProducto) {
    var result = false;
    $.each(TipoProductos, function (i, item) {
        if (item == TipoProducto)
            result = true;
            return false; //break
    });
    return result;
}

//Validar que la cantidad de las boletas control superen la cantidad
// si es asi obliga al usuario pistolear todos los productos

function ValidarMostrarProductos() {
    if (parseInt(cantidadBoletasControl) >= parseInt(CantidadMostrar)) {
        
        $.each(_listaProductos, function (i, item) {
            $("#id_" + item.CodigoSap).show();
            $("#id_" + item.CodigoSap).addClass("required");
            $("#id2_" + item.CodigoSap).show();
            $("#id2_" +item.CodigoSap).addClass("required");
        })
    }
    else {
        MostrarProductos();
    }
}

//Buscar producto
function ObtenerCantidadProducto(CodSap) {
    var cantidad = 0;
    $.each(_listaProductos, function (i, item) {
        if (item.CodigoSap.toString() == CodSap)
            cantidad = item.Cantidad;
        return false;
    });

    return cantidad;
}

//@Fin funciones 

