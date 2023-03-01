var objUsuario;
var listProductos = [];
var inicializadointerval = false;
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
                var _numTarjeta = $("#NumTarjeta").val();
                var _correoApp = $("#CorreoApp").val();
                if ( (_cedula.length === 0 || !_cedula.trim()) && (_numTarjeta.length === 0 || !_numTarjeta.trim()) && (_correoApp.length === 0 || !_correoApp.trim()))
                {
                    MostrarMensaje("Importante", "Hay inconsistencias en el formulario, debe diligenciar un campo", "error");
                }
                else if ((_cedula.length != 0 && _numTarjeta.length != 0) || (_cedula.length != 0 && _correoApp.length != 0) || (_numTarjeta.length != 0 && _correoApp.length != 0) )
                {
                    MostrarMensaje("Importante", "Hay inconsistencias en el formulario, debe diligenciar solo un campo", "error");
                }

                else {
                    EjecutarAjax(urlBase + "Cortesia/ObtenerCortesiaUsuarioVisitante", "GET", { documento: _cedula, numTarjeta: _numTarjeta, correoApp: _correoApp }, "RespuestaConsulta", _cedula);
                }

            }
        } else {
            if (listProductos.length == 0) {
                MostrarMensaje("Importante", "Debe seleccionar un producto", "warning");
            } else {
                EjecutarAjax(urlBase + "Cortesia/GuardarCortesiaUsuarioVisitante", "POST", JSON.stringify({ usuarioVisitante: objUsuario, productos: listProductos }), "RespuestaGuardarCortesia", null);
            }
        }
    });

});

function Cancelar() {
    window.location = urlBase + "Cortesia/Index";
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
        $("#NumTarjeta").val("");
        $("#NumTarjeta").hide();
        $("#CorreoApp").val("");
        $("#CorreoApp").hide();
        $("#MostrarDatos").show();
        $("#NombreVisitante").html(rta.Elemento.Nombres + ' ' + rta.Elemento.Apellidos);
        $("#lblCantidadCortesias").html(rta.Elemento.Cantidad);
        $("#btnAceptar").val("Aceptar");
       //$("#DocVisitante").removeClass("required");
    }
    else {
        objUsuario = null;
        $("#DocVisitante").show();
        $("#NumTarjeta").show();
        $("#CorreoApp").show();
        $("#MostrarDatos").hide();
        $("#NombreVisitante").val("");
        $("#lblCantidadCortesias").val("");
        $("#btnAceptar").val("Buscar");
        $("#DocVisitante").val("");
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

function ConsultarBoleta() {
    var obj = $("#txtCodigo");
    if (obj.length > 0) {
        EjecutarAjax(urlBase + "Cortesia/ObtenerProducto", "GET", { CodBarra: obj.val() }, "successObtenerProducto", null);
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
        MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", "Cortesia/Index", "success");
    }
    else {
        MostrarMensaje("Importante", rta.Elemento, "error");
    }
}