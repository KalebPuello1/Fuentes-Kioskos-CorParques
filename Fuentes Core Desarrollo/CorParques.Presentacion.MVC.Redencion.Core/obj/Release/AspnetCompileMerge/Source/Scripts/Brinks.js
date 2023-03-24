
$(".Moneda:input[type=text]").mask("000.000.000", { reverse: true });


if (typeof _cantidad != 'undefined') {
    $("#Valor").keyup(function () {

        var valor = $(this).val();

        if (valor == 0 || valor == '0') {
            $("#Valor").val("");
            return false;
        }

        if (valor.length > 0) {

            $(this).attr("data-mensajeerror", "");
            $(this).removeClass("errorValidate");
            QuitarTooltip();

            var cantidadMaxima = _cantidad;
            var precio = parseInt(ReplaceAll($(this).val(), '.', ''));
            if (precio == 0) {
                $(this).val(EnMascarar(precioFactura));
            } else {
                if (precio > cantidadMaxima) {
                    //$(this).attr("data-mensajeerror", "El valor no puede superar la cantidad disponible en caja ("+cantidadMaxima+")");
                    $(this).attr("data-mensajeerror", "Confirme Valor");
                    $(this).addClass("errorValidate");
                    mostrarTooltip();
                }
            }
        }
    });
}


//@Inicio Eventos
    $("#btnCancelarBrinks").click(function () {
        MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "Cancelar", "");
    });

    $("#btnAceptarBrinks").click(function () {

        var validar1 = true;
        validar1 = validarFormulario("frmBrinks");

        var validar2 = ValidarCampos();

        if (validar1) {
            if (validar2) {
                MostrarConfirm("Importante!", "¿Está seguro de guardar el documento Brinks? ", "GuardarBrinks", "");
            }
            else {
                MostrarMensaje("Importante", "Hay inconsistencias en el formulario, revise los campos demarcados con color rojo.", "error");
            }
        } 
    });

//@Fin Eventos

//@Inicio Métodos
    function Cancelar() {
        window.location = urlBase + "Brinks";
    }

    function GuardarBrinks() {
        var obj = ObtenerObjeto("frmBrinks");
        $.each(obj, function (i, item) {
            
            if (item.name == "Valor") {
                item.value = ReplaceAll(item.value, ".", "");
            }
        });
        EjecutarAjax(urlBase + "Brinks/Insertar", "GET", obj, "successInsertBrinks", null);

    }

    function successInsertBrinks(rta) {
        if (rta.Correcto) 
            MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "Brinks", "success");
        else 
            MostrarMensaje("Error al guardar comuníquese con el administrador", rta.Mensaje);
        
    }

    function ValidarCampos() {

        var result = true;        

        if (parseInt(ReplaceAll($("#Valor").val(),".","")) > parseInt(_cantidad)) {
            result = false
            $("#Valor").attr("data-mensajeerror", "Verifique valores en la caja con su supervisor");
            $("#Valor").addClass("errorValidate");
            mostrarTooltip();
        }

        return result;
    }
    

//@Fin Métodos
