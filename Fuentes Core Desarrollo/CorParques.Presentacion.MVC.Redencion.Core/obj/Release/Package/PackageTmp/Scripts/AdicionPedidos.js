//Powered by RDSH.
$(function () {

    $("#txtCodigoPedido").focusout(function () {
        if ($(this).val() != '') {
            ConsultarInformacionPedido($(this).val());
        }
    });
});

//Llamado por ajax al controlador de adicion pedido.
function ConsultarInformacionPedido(Codigo) {
    EjecutarAjax(urlBase + "AdicionPedidos/DetallePedido", "GET", { CodigoSapPedido: Codigo }, "CargarPagina", null);
}

//Muestra la pagina con la informacion del pedido.
function CargarPagina(data) {
    if (data != '') {
        $('#ListaPedidos').html(data);
        //$('#ListaPedidos').find("table").DataTable();
        AsignarEventosBotones();
        //BloquearCopiarCortarPegar();
        setEventEdit();
    } else {
        $('#ListaPedidos').html("");
        MostrarMensaje("Importante", "Código de pedido inválido.", "warning");
    }
}

function setEventEdit()
{
}

//Asigna el evento guardar y cancelar.
function AsignarEventosBotones()
{
    $("#btnCancelar").click(function () {
        MostrarConfirm("Importante", "¿Realmente desea cancelar este proceso?", "RedireccionCancelar", null);        
    });

    $("#btnGuardar").click(function () {
        if (ValidacionesAdicionPedidos()) {
            MostrarConfirm("Importante", "¿Realmente desea guardar la información registrada?", "Guardar", null);
        } else {
            MostrarMensaje("Importante", 'Hay inconsistencias en el formulario, revise los campo demarcados en color rojo.', "error");
        }
    });

    $(".textinicial").change(function () {
        if ($(this).val() != '') {
            var consecutivo = $(this).attr("data-contador");
            var txtconsecutivofinal = $("#consecutivofinal_" + consecutivo).val();
            var txtconsecutivoinicial = $(this).val();

            if (txtconsecutivoinicial != "" && txtconsecutivofinal != "")
            {
                ValidarRangoConsecutivos(txtconsecutivoinicial, txtconsecutivofinal, consecutivo);
            }

        }
    });

    $(".textfinal").change(function () {
        if ($(this).val() != '') {
            var consecutivo = $(this).attr("data-contador");
            var txtconsecutivoinicial = $("#consecutivoinicial_" + consecutivo).val();
            var txtconsecutivofinal = $(this).val();

            if (txtconsecutivoinicial != "" && txtconsecutivofinal != "") {
                ValidarRangoConsecutivos(txtconsecutivoinicial, txtconsecutivofinal, consecutivo);
            }
        }
    });

}

//Cancelar proceso de adicion de pedidos.
function RedireccionCancelar() {
    window.location = urlBase + "AdicionPedidos";
}

//RDSH: Validacion de datos registrados, retorna false si hay alguna validacion de consecutivos pendiente.
function ValidacionesAdicionPedidos()
{
      
    var blnResultado = true;

    $(".validadores").each(function (index, element) {
        if (element.value == "S")
        {            
            blnResultado = false;
            return;
        }
    });

    if (blnResultado)
    {
        if (!validarFormulario("frm_AdicionarPedido *")) {
            blnResultado = false;
        }
    }

    return blnResultado;
}

//RDSH: Valida en base de datos si el rango de consecutivos se puede usar.
function ValidarRangoConsecutivos(consecutivo_inicial, consecutivo_final, fila)
{
    var CantidadRango = $("#cantidad_" + fila).val();
    var intIdProducto = $("#IdProducto_" + fila).val();
    EjecutarAjax(urlBase + "AdicionPedidos/ValidarRangoConsecutivos", "GET", { ConsecutivoInicial: consecutivo_inicial, ConsecutivoFinal: consecutivo_final, Cantidad: CantidadRango, IdProducto: intIdProducto}, "ResultadoValidacion", fila);
}

//RDSH: Aqui se sabe si los consecutivos se pueden usar.
function ResultadoValidacion(data, fila)
{
    QuitarTooltip();
    $("#consecutivoinicial_" + fila).removeClass("required");
    $("#consecutivoinicial_" + fila).removeClass("errorValidate");
    $("#consecutivoinicial_" + fila).attr("data-mensajeerror", "");

    $("#consecutivofinal_" + fila).removeClass("required");
    $("#consecutivofinal_" + fila).removeClass("errorValidate");
    $("#consecutivofinal_" + fila).attr("data-mensajeerror", "");
    $("#Validacion_" + fila).val('N');
    if (data != '') {
        if (data.indexOf("Error") >= 0)
        {
            data = "Rango de consecutivos inválido.";
        }
        $("#consecutivoinicial_" + fila).addClass("required");
        $("#consecutivoinicial_" + fila).addClass("errorValidate");
        $("#consecutivoinicial_" + fila).attr("data-mensajeerror", data);

        $("#consecutivofinal_" + fila).addClass("required");
        $("#consecutivofinal_" + fila).addClass("errorValidate");
        $("#consecutivofinal_" + fila).attr("data-mensajeerror", data);
        mostrarTooltip();
        $("#Validacion_" + fila).val('S');
    } else {
        $(".tooltipError").hide();
        ValidarTraslapeConsecutivos(fila);
    }
}

//RDSH: Valida que un rango de consecutivos para un producto no este siendo usado en la pantalla.
function ValidarTraslapeConsecutivos(fila)
{
    var intIdProducto = $("#IdProducto_" + fila).val();
    var consecutivo_inicial = $("#consecutivoinicial_" + fila).val();
    var consecutivo_final = $("#consecutivofinal_" + fila).val();
    var filaeach = 0;
    var consecutivo_inicial_each = "";
    var consecutivo_final_each = "";
    var intIdProducto_each = 0;

    consecutivo_inicial = RetornarIdBoleteriaDesdeConsecutivo(consecutivo_inicial);
    consecutivo_final = RetornarIdBoleteriaDesdeConsecutivo(consecutivo_final);   

    QuitarTooltip();
    $("#consecutivoinicial_" + fila).removeClass("required");
    $("#consecutivoinicial_" + fila).removeClass("errorValidate");
    $("#consecutivoinicial_" + fila).attr("data-mensajeerror", "");

    $("#consecutivofinal_" + fila).removeClass("required");
    $("#consecutivofinal_" + fila).removeClass("errorValidate");
    $("#consecutivofinal_" + fila).attr("data-mensajeerror", "");
    $("#Validacion_" + fila).val('N');

    $(".validartraslape").each(function (index, element) {
        filaeach = $("#" + element.id).attr("data-contador")
        intIdProducto_each = $("#IdProducto_" + filaeach).val();

        if (filaeach != fila && intIdProducto == intIdProducto_each)
        {
            consecutivo_inicial_each = RetornarIdBoleteriaDesdeConsecutivo($("#consecutivoinicial_" + filaeach).val());
            consecutivo_final_each = RetornarIdBoleteriaDesdeConsecutivo($("#consecutivofinal_" + filaeach).val());

            if (consecutivo_inicial_each != "" && consecutivo_final_each != "")
            {
                if (ValidarTraslape(consecutivo_inicial, consecutivo_final, consecutivo_inicial_each, consecutivo_final_each)) {
                    $("#consecutivoinicial_" + fila).addClass("required");
                    $("#consecutivoinicial_" + fila).addClass("errorValidate");
                    $("#consecutivoinicial_" + fila).attr("data-mensajeerror", "Rango de consecutivos inválido.");

                    $("#consecutivofinal_" + fila).addClass("required");
                    $("#consecutivofinal_" + fila).addClass("errorValidate");
                    $("#consecutivofinal_" + fila).attr("data-mensajeerror", "Rango de consecutivos inválido.");
                    mostrarTooltip();
                    $("#Validacion_" + fila).val('S');
                } else {
                    $(".tooltipError").hide();
                }
            }            
        }        
    });    

    

}

//RDSH: Guarda en base de datos la información registrada.
function Guardar() {
    var objAdicionPedido = ObtenerObjeto("frm_AdicionarPedido *");
    EjecutarAjax(urlBase + "AdicionPedidos/Guardar", "GET", objAdicionPedido, "ActualizacionOk", null);
}

//RDSH: Muestra mensaje de confirmación de guardado de la información.
function ActualizacionOk(rta) {
    if (rta == '') {
        MostrarMensajeRedireccion("Importante", "Operación realizada con éxito.", "Home/Index", "success");
    }
    else {
        MostrarMensaje("Importante", rta, "error");
    }
}


//RDHS: Remueve los el consecutivo de codigo de barras y retorna el id de la boleteria.
function RetornarIdBoleteriaDesdeConsecutivo(consecutivo)
{
    //Quita los dos consecutivos iniciales y remueve los ceros de la izquierda.
    consecutivo = consecutivo.substring(2, consecutivo.length).replace(/^0+/, '')    

    //Quita el digito de verificacion del codigo de barras.
    consecutivo = consecutivo.substring(0, consecutivo.length - 1);   

    return consecutivo;
}

//RDSH: Valida si ConsecutivoInicial1 y ConsecutivoFinal1 se traslapan con ConsecutivoInicial2 y ConsecutivoFinal2
function ValidarTraslape(ConsecutivoInicial1, ConsecutivoFinal1, ConsecutivoInicial2, ConsecutivoFinal2)
{
    var blnTraslape = false;

    if (parseFloat(ConsecutivoInicial2) >= parseFloat(ConsecutivoInicial1) && parseFloat(ConsecutivoInicial2) <= parseFloat(ConsecutivoFinal1))
    {
        blnTraslape = true;
    }

    if (parseFloat(ConsecutivoFinal2) >= parseFloat(ConsecutivoInicial1) && parseFloat(ConsecutivoFinal2) <= parseFloat(ConsecutivoFinal1) && blnTraslape == false)
    {
        blnTraslape = true;
    }

    return blnTraslape;

}

//RDSH: Solo se permite pistolear los codigos de barras.
function BloquearCopiarCortarPegar()
{
    $(".required").each(function (index, element) {
        InhabilitarCopiarPegarCortar(element.id);
    });
}