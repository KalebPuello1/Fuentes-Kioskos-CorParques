//Powered by RDSH.

$(function () {

    $("#div_message_error").hide();
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "Convenio/GetPartial", "GET", null, "printPartialModal", { title: "Crear convenio", modalLarge: true, DatePicker: true });
    });
    setEventEdit();


    //@ Author: EDSP 
    //@ Description:  eventos actualizar precios convenio 

    //Evento tipo productos actualizar precio convenios//
    $("#ddlTipoProductoPrecio").change(function () {
        var CodsapProducto = $(this).val();
        var lista = "<option value=''> Seleccione...</option>";

        if (CodsapProducto.length > 0) {
            $.each(Productos, function (i, item) {
                if (item.CodSapTipoProducto == CodsapProducto)
                    lista = lista + "<option value='" + item.CodSapProducto + "'>" + item.Nombre + "</option>";
            });
        }

        $("#ddlProducto").html(lista);


    });



    //Evento check todos
    $("#chxTodos").click(function () {
        if ($(this).is(":checked")) {
            $("#ddlTipoProductoPrecio").removeClass("errorValidate");
            QuitarTooltip();
            $("#ddlTipoProductoPrecio").val("");
            $("#ddlProducto").val("");
            $("#ddlTipoProductoPrecio").prop("disabled", true);
            $("#ddlProducto").prop("disabled", true);

        } else {
            $("#ddlTipoProductoPrecio").prop("disabled", false);
            $("#ddlProducto").prop("disabled", false);
        }
    });

    //Evento cancelar actualizar convenio

    $("#btnCancelarActualizar").click(function () {
        MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "Cancelar", "");
    });


    //Evento aceptar 

    $("#btnAceptarActualizar").click(function () {

        if (ValidarCamposActualizarPrecios()) 
            MostrarConfirm("Importante!", "¿Está seguro de actualizar los precios de los convenios? ", "ActualizarPrecios", "");
    });

    //////////////////////////////////////////// Exclusión convenio ////////////////////////
    
    //Boton cancelar
    $("#btnCancelarExclusion").click(function () {
        MostrarConfirm("Importante!", "¿Está seguro de cancelar la operación? ", "CancelarExclusion", "");
    });

    $("#btnAdicionar").click(function () {
        if (validarFormulario("frmExclusion")) 
            MostrarConfirm("Importante!", "¿Está seguro de agregar una exclusión de convenio? ", "AdicionarExclusion", "");
    });

    //@FIN EDSP
    
    

});
//Ajuste Buscador GALD
function BuscadorEditar() {
    $("#txtSearch").keyup(function () {
        $("#tbDetail tbody tr").hide();
        $.each($("#tbDetail tbody tr"), function (i, v) {
            $.each($(v).find("td>select >option:selected"), function (j, va) {
                if ($(va).html().toLowerCase().indexOf($("#txtSearch").val().toLowerCase()) >= 0) {
                    $(v).show();
                    return false;
                }                
            });
        });
    });
}

//@Evento cancelar 
function Cancelar() {
    window.location = urlBase + "Convenio/ActualizarPreciosConvenios";
}

//@Cancelar exclusión
function CancelarExclusion() {
    window.location = urlBase + "Convenio/ExclusionesConvenio";
}

//@Eliminar exclusión
function eliminarExclusion(id) {
    MostrarConfirm("Importante!", "¿Está seguro de eliminar la exclusión? ", "EliminarExclusionControl", id);
}

//@Evento validar campos
//Description: Se debe seleccionar el tipo de producto o el check de todos para continuar
function ValidarCamposActualizarPrecios() {
    debugger;

    var validarfrm = true;
    if (!$("#chxTodos").is(':checked')) {
        validarfrm = validarFormulario("frmActualizarPrecios");
    }

    return validarfrm;
}

//@Actualizar precios convenios 
function ActualizarPrecios() {

    var elemento = new Object();

    elemento.Todos = $("#chxTodos").is(':checked');
    elemento.CodSapTipoProducto = $("#ddlTipoProductoPrecio").val();
    elemento.CodSapProducto = $("#ddlProducto").val();
    EjecutarAjaxJson(urlBase + "Convenio/ActualizarPrecio", "POST", { modelo: elemento }, "successActualizarPrecios", null);
}

//@Adicionar exclusión de fecha 
function AdicionarExclusion() {
    var elemento = new Object();
    elemento.strFechaInicio = $("#strFechaInicio").val();
    elemento.strFechaFin = $("#strFechaFin").val();
    elemento.IdConvenio = idConvenio;

    EjecutarAjaxJson(urlBase + "Convenio/AgregarExclusion", "POST", { modelo: elemento }, "successInsertExclusion", null);
}
//@Eliminar exclusión
function EliminarExclusionControl(id) {
    debugger;
    EjecutarAjax(urlBase + "Convenio/DeshabilitarExclusion", "GET", { IdExclusion: id }, "successEliminarExclusion", null);
}

function successInsertExclusion(rta) {
    if (rta.Correcto) {
        //MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "Convenio/ExclusionesConvenio", "success");
        EjecutarAjax(urlBase + "Convenio/ObtenerExclusion", "GET", { IdConvenio: idConvenio }, "MostrarExclusiones", null);
    }
    else
        MostrarMensaje("Importante", rta.Mensaje, "error");
}

function successActualizarPrecios(rta) {
    if (rta.Correcto)
        MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "Convenio/ActualizarPreciosConvenios", "success");
    else
        MostrarMensaje("Importante", rta.Mensaje, "error");
}

function successEliminarExclusion(rta) {
    if (rta.Correcto) {
        //EjecutarAjax(urlBase + "Convenio/ObtenerExclusion", "GET", { IdConvenio: idConvenio }, "MostrarExclusiones", null);
        EjecutarAjax(urlBase + "Convenio/ObtenerExclusion", "GET", { IdConvenio: idConvenio }, "MostrarExclusiones", null);
    }
    else
        MostrarMensaje("Importante", rta.Mensaje, "error");
}



function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "Convenio/Obtener", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar convenio", modalLarge: true, DatePicker: true });
    });

    $(".lnkDisable").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea inactivar esta autorización?", "InactivarAutorizacion", $(this).data("id"));
    });

    $(".lnkDetail").click(function () {
        EjecutarAjax(urlBase + "Convenio/Detalle", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Detalle convenio", modalLarge: true, hidesave: "S", showreturn: "S" });
    });

    $(".lnkExclusion").click(function () {
        var nombre = $(this).data("convenio");
        EjecutarAjax(urlBase + "Convenio/Exclusion", "GET", { id: $(this).data("id") }, "printPartialModal", { modalLarge: true, title: "Exclusión fechas - " + nombre, hidesave: true, modalLarge: false, hideCustom: true, showreturn: "S" });
    });

    $(".lnkProductos").click(function () {
        var nombre = $(this).data("convenio");
        EjecutarAjax(urlBase + "Convenio/ProductoConvenio", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Cortesia cumpleaños - " + nombre, modalLarge: true, hidesave: true , hideCustom: true});
    });

}

//Busca los productos por codigo sap tipo producto.
function BuscarProductos(elemento) {
    debugger;
    if ($(elemento).val() !== "") {
        var result = $.grep(Productos, function (e) { return e.CodSapTipoProducto == $(elemento).val(); });
        CargarProductos(result, elemento, $(elemento).val());
    } else {
        $(elemento).parent().parent().find("select[name='ListaDetalle.CodSapProducto']").html('<option value="">Seleccione...</option>').val('')
    }

}

//EDSP Buscar valor producto : 27/12/2017
function EventoPorcentaje(elemento) {
    var porcentaje = $(elemento).val();
    var registro = $(elemento).parent().parent();

    if (porcentaje.length > 0) {


        var CodSapProducto = $(registro).find("select[name='ListaDetalle.CodSapProducto']").val();

        //Validacion si el campo porcentaje unicamente tiene '.'
        if (porcentaje == ".") {
            $(elemento).val("");
            $(registro).find("input[name='ListaDetalle.Valor']").val("");
            return;
        }


        //Validaciones campo
        if (porcentaje < 1 || porcentaje > 100) {
            $(elemento).val("");
            $(registro).find("input[name='ListaDetalle.Valor']").val("");
            return;
        }

        if (CodSapProducto.length == 0) {
            $(elemento).val("");
            $(registro).find("input[name='ListaDetalle.Valor']").val("");
            return;
        }

        //Validar si selecciono Todos 

        var tipoProd = $(registro).find("select[name='ListaDetalle.CodSapProducto']").val();                      
        if (tipoProd.indexOf('Todos') >= 0) {
            $(registro).find("input[name='ListaDetalle.Valor']").val("0");
            return;
        }

        var objProducto = $.grep(Productos, function (e) { return e.CodSapProducto == CodSapProducto });

        if (typeof objProducto !== 'undefined' && objProducto.length > 0)
            $(registro).find("input[name='ListaDetalle.Valor']").val(CalcularValorPorcentaje(objProducto[0].Precio,porcentaje));
        else
            $(registro).find("input[name='ListaDetalle.Valor']").val("");
    } else 
        $(registro).find("input[name='ListaDetalle.Valor']").val("");
}

//EDSP: Evento ddl producto : 27/12/2017
function CalcularPorcentaje(elemento) {
    if ($(elemento).length > 0) {
        var registro = $(elemento).parent().parent();
        var CodSapProducto = $(elemento).val();
        var Porcentaje = $(registro).find("input[name='ListaDetalle.Porcentaje']");
        if (Porcentaje.length > 0) {
            if ($(Porcentaje).val().length > 0) {
                if (_crear == true && CodSapProducto.indexOf('Todos') >= 0)
                    $(registro).find("input[name='ListaDetalle.Valor']").val("0");
                else {
                    var objProducto = $.grep(Productos, function (e) { return e.CodSapProducto == CodSapProducto });
                    $(registro).find("input[name='ListaDetalle.Valor']").val(CalcularValorPorcentaje(objProducto[0].Precio, Porcentaje.val()));
                }
            }
            else
                $(registro).find("input[name='ListaDetalle.Valor']").val("");
        }
        else
            $(registro).find("input[name='ListaDetalle.Valor']").val("");
    }
}

//EDSP : 27/12/2017 calcular valor con porcentaje
function CalcularValorPorcentaje(valor, porcentaje) {

    var base = 50;
    var _valor = valor - ((valor * porcentaje) / 100);
    var retorno = 0;

    var ValorEntero = parseInt(_valor);
    var componente = (ValorEntero / base);

    if (isFloat(componente))
        retorno = (parseInt(componente) * base);
    else
        retorno = ValorEntero;
    
    return retorno;
}


//EDSP Validacion tipo de dato : 28/12/2017
function isInteger(x) { return typeof x === "number" && isFinite(x) && Math.floor(x) === x; }
function isFloat(x) { return !!(x % 1); }
//FIN  EDSP Validacion tipo de dato : 28/12/2017


//Carga los productos devueltos en la consulta.
function CargarProductos(data, elemento, tipoProd) {
    //Se remueven todos los items de la lista de productos y se deja solo la opcion selecione.
    $(elemento).parent().parent().find("select[name='ListaDetalle.CodSapProducto']").html('<option value="">Seleccione...</option>').val('');

    if (_crear == true && data.length > 1)
        listitems += '<option value=Todos_' + tipoProd + '>' + 'Todos' + '</option>';

    //Se agregan los items que vienen en la variable data.
    var listitems;
    $.each(data, function (key, value) {
        listitems += '<option value=' + value.CodSapProducto + '>' + value.Nombre + '</option>';
    });
    
    $(elemento).parent().parent().find("select[name='ListaDetalle.CodSapProducto']").append(listitems);
}

//RDSH: Setea el evento change del combo tipo producto.
function setEventChange() {

    //$("#CodSapTipoProducto").change(function () {
    //    BuscarProductos($(this).val());
    //});


    $("#btnAddDetail").click(function () {
        debugger;
        var count = $("#tbDetail tbody tr").length + 1;
        var listitems = "";
        $.each(TiposProductos, function (key, value) {
            listitems += '<option value=' + value.CodSAP + '>' + value.Nombre + '</option>';
        });
        //@
        //Author: EDSP
        //date: 27/12/2017 
        //Description: Se adiciona un control para recibir el porcentaje de descuento por producto
        //@//
        var fila = "<tr>" +
                        "<td><select style='width:160px;' class='form-control required' name='ListaDetalle.CodSapTipoProducto' onchange='BuscarProductos(this)'><option value=''>Seleccione...</option>" + listitems + "</select></td>" +
                        "<td><select style='width:300px;' class='form-control required productos' name='ListaDetalle.CodSapProducto' onchange='CalcularPorcentaje(this)'><option value=''>Seleccione...</option></select></td>" +
                        "<td><input type='text' name='ListaDetalle.Porcentaje' maxlength='5' class='form-control required' onkeypress=' return validateFloatKeyPress(this, event);' onkeyup='EventoPorcentaje(this);' /></td>" +
                        "<td><input type='text' name='ListaDetalle.Valor' maxlength='10' class='form-control required' onkeypress='return EsNumero(this);' readonly /></td>" +
                        "<td><input type='text' name='ListaDetalle.Cantidad' maxlength='5' class='form-control required Cantidad' onkeypress='return EsNumero(this);' onchange='ValidarCantidad()' /></td>" +
                        "<td><input type='text' name='ListaDetalle.CantidadxDia' maxlength='5' class='form-control required CantidadxDia' onkeypress='return EsNumero(this);' onchange='ValidarCantidad()'/></td>" +
                        "<td><a href='javascript:void(0)' onclick='removeRow(this)'><i class='fa fa-trash'></i></a></td>" +
                        "</tr>";
        $("#tbDetail tbody").append(fila);
    });

    //Oculta el boton guardar que va a base de datos.
    $("#btnSaveGeneric").hide();

    //Muestra el guardar que arma el detalle del pedido.
    $("#btnSaveCustomizable").show();

    //Ejecuta el armado del objeto convenio detalle y el guardado a base de datos.
    $('#btnSaveCustomizable').unbind('click');
    $("#btnSaveCustomizable").click(function () {
        if (validarFormulario("modalCRUD .modal-body")) {
            Guardar();
        }
    });
    // 
}

//Elimina una fila.
function removeRow(element) {
    var tbody = $(element).parent().parent().parent();
    $(element).parent().parent().remove();

}

//Establece el nombre de la propiedad del detalle convenio.
function setName(table, property, func) {
    var countRows = 0;
    var NombreCampo = "";

    $.each($("#" + table + " tbody tr"), function (i, tr) {
        var countColumns = 0;
        $.each($(tr).find("td"), function (j, td) {
            var input = $(td).find("input");

            var select = $(td).find("select");

            if (input.length > 0) {
                NombreCampo = $(input[0]).attr("name");
                var split = NombreCampo.split('.');
                $(input[0]).attr("name", split[0] + "[" + countRows + "]." + split[0]);
                //$(input[0]).attr("name", property + "[" + countRows + "]." + window[func](countColumns));
            }

            if (select.length > 0) {
                NombreCampo = $(select[0]).attr("name");
                var split = NombreCampo.split('.');
                $(select[0]).attr("name", split[0] + "[" + countRows + "]." + split[0]);
                //$(select[0]).attr("name", property + "[" + countRows + "]." + window[func](countColumns));
            }
            countColumns++;
        });
        countRows++;
    });
}

function getColumnNameDetail(position) {
    var name = "";
    switch (position) {

        case 0:
            name = "CodSapTipoProducto";
            break;
        case 1:
            name = "CodSapProducto";
            break;
        case 2:
            name = "Valor";
            break;
        case 3:
            name = "Cantidad";
            break;
        case 4:
            name = "CantidadxDia";
            break;
        case 5:
            name = "CantidadInicial";
            break;
    }
    return name;
}

//RDSH: Guarda en base de datos
function Guardar() {
    if (ValidacionesGuardar()) {
        GenerarDetalleConvenio("tbDetail", "ListaDetalle");
        var objConvenio = ObtenerObjeto("modalCRUD .modal-body form");
        //EDSP: Se actualzia el valor a float para el porcentaje
        var objConvenio = ConvertirVariablesFloat(objConvenio);
        EjecutarAjaxJson(urlBase + "Convenio/Insert", "POST", objConvenio, "successInsertConvenio", null);
    }
}

//RDSH: Insercion existosa.
function successInsertConvenio(rta) {

    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Convenio/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        MostrarMensaje("Importante", "Su operación fue exitosa.", "success");
    }
    else {
        MostrarMensaje("Importante", rta.Mensaje, "error");
        //$("#lbl_message_error").html(rta.Mensaje);
        //$("#div_message_error").show();
    }
}

//RDSH: Valida que los campos de cantidad sean mayores a cantidad por transaccion.
function ValidarCantidad() {
    var blnResultado = false;
    var objCantidadxDia;
    $("#hdf_Productos").val("N");
    QuitarTooltip();
    $(".Cantidad").each(function (index, element) {
        $(element).removeClass("errorValidate");
        $(element).attr("data-mensajeerror", "");

        objCantidadxDia = $(element).parent().parent().find("input[name*='CantidadxDia']");
        if (element.value !== "" && objCantidadxDia.val() !== "") {
            if (parseInt(element.value) < parseInt(objCantidadxDia.val())) {
                $(element).addClass("errorValidate");
                $(element).attr("data-mensajeerror", "Cantidad debe ser mayor o igual que Cant X Tran");
                blnResultado = true;
            }
        }
        $("#hdf_Productos").val("S");
    });

    if (blnResultado) {
        mostrarTooltip();
        $("#hdf_Validaciones").val("S");        
    } else {
        $(".tooltipError").hide();
        $("#hdf_Validaciones").val("N");
    }

}

//RDSH: Valida cantidades y que haya un producto en el detalle del convenio.
function ValidacionesGuardar() {
    debugger;
    var blnGuardar = true;
    var Contador = 0;
    
    ValidarCantidad();
    if ($("#hdf_Validaciones").val() == "S") {
        blnGuardar = false;
        MostrarMensaje("Importante", "Hay inconsistencias en el formulario, revise los campos demarcados con color rojo.", "error");
    }
    else if ($("#hdf_Productos").val() == "N") {
        blnGuardar = false;
        MostrarMensaje("Importante", "El convenio debe tener mínimo un producto.", "error");
        return;
    }
        
    $(".productos").each(function (index, element) {
        Contador = 0;
        $(element).removeClass("errorValidate");
        $(element).attr("data-mensajeerror", "");

        var objCodSapProducto = $(element).parent().parent().find("select[name*='CodSapProducto']");
        $(".productos").each(function (index, elementovalidar) {
            var objCodSapProductovalidar = $(elementovalidar).parent().parent().find("select[name*='CodSapProducto']");
            if (objCodSapProducto.val() === objCodSapProductovalidar.val())
            {
                Contador = Contador + 1
            }
            if (Contador > 1)
            {
                $(objCodSapProductovalidar).addClass("errorValidate");
                $(objCodSapProductovalidar).attr("data-mensajeerror", "Producto repetido.");
                blnGuardar = false;
            }
        });        

    });

    if (!blnGuardar) {        
        $("#hdf_Validaciones").val("S");
        MostrarMensaje("Importante", "Existen productos repetidos, revise los campos demarcados con color rojo.", "error");
        mostrarTooltip();
    } else {
        $(".tooltipError").hide();
        $("#hdf_Validaciones").val("N");
        QuitarTooltip();
    }

    return blnGuardar;

}

//RDSH: Inicializa la edicion de un pedido.
function InicializarEdicion() {
    var listitems = "";
    var listProductos = "";
    var fila = "";

    $.each(ListaDetalle, function (key, value) {
        listitems = "";
        listProductos = "";
        fila = "";

        $.each(TiposProductos, function (key, value2) {
            if (value.CodSapTipoProducto === value2.CodSAP)
                listitems += '<option selected value=' + value2.CodSAP + '>' + value2.Nombre + '</option>';
            else
                listitems += '<option value=' + value2.CodSAP + '>' + value2.Nombre + '</option>';
        });

        var resultado = $.grep(Productos, function (e) { return e.CodSapTipoProducto == value.CodSapTipoProducto; });

        $.each(resultado, function (key, value2) {
            if (value.CodSapProducto === value2.CodSapProducto)
                listProductos += '<option selected value=' + value2.CodSapProducto + '>' + value2.Nombre + '</option>';
            else
                listProductos += '<option value=' + value2.CodSapProducto + '>' + value2.Nombre + '</option>';
        });

        //@ EDSP: 29/12/2017
        //  Description: Se adiciona el porcentaje 
        //@

        fila = "<tr>" +
                "<td style='display:none;'><input type='hidden' name='ListaDetalle.IdConvenioDetalle' maxlength='5' class='form-control' value='" + value.IdConvenioDetalle + "' /></td>" +
                "<td style='display:none;'><input type='hidden' name='ListaDetalle.CantidadDisponible' maxlength='5' class='form-control' value='" + value.CantidadDisponible + "' /></td>" +
                "<td><select style='width:160px;' class='form-control required' name='ListaDetalle.CodSapTipoProducto' onchange='BuscarProductos(this)'><option value=''>Seleccione...</option>" + listitems + "</select></td>" +
                "<td><select style='width:300px;' class='form-control required productos' name='ListaDetalle.CodSapProducto'><option value=''>Seleccione...</option>" + listProductos + "</select></td>" +
                "<td><input type='text' name='ListaDetalle.Porcentaje' maxlength='5' class='form-control required' onkeypress='return validateFloatKeyPress(this, event);' onkeyup='EventoPorcentaje(this);' value ='" + (value.Porcentaje == 0? "" : value.Porcentaje) + "' /></td>" +
                "<td><input type='text' name='ListaDetalle.Valor' maxlength='10' class='form-control required' onkeypress='return EsNumero(this);' value='" + value.Valor + "' readonly /></td>" +
                "<td><input type='text' name='ListaDetalle.Cantidad' maxlength='5' class='form-control required Cantidad' onkeypress='return EsNumero(this);' onchange='ValidarCantidad()' value='" + value.Cantidad + "'/></td>" +
                "<td><input type='text' name='ListaDetalle.CantidadxDia' maxlength='5' class='form-control required CantidadxDia' onkeypress='return EsNumero(this);' onchange='ValidarCantidad()' value='" + value.CantidadxDia + "' /></td>" +
                "<td>" + "<input type='hidden' name='ListaDetalle.CantidadInicial' maxlength='5' class='form-control'  value='" + value.CantidadInicial + "'/>" + "<a href='javascript:void(0)' onclick='removeRow(this)'><i class='fa fa-trash'></i></a></td>" +
                "</tr>";
        $("#tbDetail tbody").append(fila);

    });


    $("#btnAddDetail").click(function () {
        var count = $("#tbDetail tbody tr").length + 1;
        var listitems = "";
        $.each(TiposProductos, function (key, value) {
            listitems += '<option value=' + value.CodSAP + '>' + value.Nombre + '</option>';
        });
        var fila = "<tr>" +
                        "<td><select style='width:160px;' class='form-control required' name='ListaDetalle.CodSapTipoProducto' onchange='BuscarProductos(this)'><option value=''>Seleccione...</option>" + listitems + "</select></td>" +
                        "<td><select style='width:300px;' class='form-control required productos' name='ListaDetalle.CodSapProducto'><option value=''>Seleccione...</option></select></td>" +
                        "<td><input type='text' name='ListaDetalle.Porcentaje' maxlength='5' class='form-control required' onkeypress='return validateFloatKeyPress(this, event);' onkeyup='EventoPorcentaje(this);' /></td>" +
                        "<td><input type='text' name='ListaDetalle.Valor' maxlength='10' class='form-control required' onkeypress='return EsNumero(this);' /></td>" +
                        "<td><input type='text' name='ListaDetalle.Cantidad' maxlength='5' class='form-control required Cantidad' onkeypress='return EsNumero(this);' onchange='ValidarCantidad()' /></td>" +
                        "<td><input type='text' name='ListaDetalle.CantidadxDia' maxlength='5' class='form-control required CantidadxDia' onkeypress='return EsNumero(this);' onchange='ValidarCantidad()'/></td>" +
                        "<td><a href='javascript:void(0)' onclick='removeRow(this)'><i class='fa fa-trash'></i></a></td>" +
                        "</tr>";
        $("#tbDetail tbody").append(fila);
    });

    //Oculta el boton guardar que va a base de datos.
    $("#btnSaveGeneric").hide();

    //Muestra el guardar que arma el detalle del pedido.
    $("#btnSaveCustomizable").show();

    //Ejecuta el armado del objeto convenio detalle y el guardado a base de datos.
    $('#btnSaveCustomizable').unbind('click');
    $("#btnSaveCustomizable").click(function () {
        if (validarFormulario("modalCRUD .modal-body")) {
            GuardarEdicion();
        }
    });
}

function GuardarEdicion() {
    
    if (ValidacionesGuardar()) {
        GenerarDetalleConvenio("tbDetail", "ListaDetalle");
        var objConvenio = ObtenerObjeto("modalCRUD .modal-body form");
        objConvenio = ConvertirVariablesFloat(objConvenio);
        EjecutarAjaxJson(urlBase + "Convenio/Update", "POST", objConvenio, "successUpdateConvenio", null);
    }
}

//RDSH: Carga la propiedad lista detalle para poder enviar el modelo a guardar.
function GenerarDetalleConvenio(table, nombrepropiedad) {
    debugger;
    var countRows = 0;
    var NombreCampo = "";

    $.each($("#" + table + " tbody tr"), function (i, tr) {
        var countColumns = 0;
        $.each($(tr).find("td"), function (j, td) {
            var input = $(td).find("input");

            var select = $(td).find("select");

            if (input.length > 0) {
                NombreCampo = $(input[0]).attr("name");
                var split = NombreCampo.split('.');
                $(input[0]).attr("name", nombrepropiedad + "[" + countRows + "]." + split[(split.length - 1)]);
                //$(input[0]).attr("name", property + "[" + countRows + "]." + window[func](countColumns));
            }

            if (select.length > 0) {
                NombreCampo = $(select[0]).attr("name");
                var split = NombreCampo.split('.');
                $(select[0]).attr("name", nombrepropiedad + "[" + countRows + "]." + split[(split.length - 1)]);
                //$(select[0]).attr("name", property + "[" + countRows + "]." + window[func](countColumns));
            }
            countColumns++;
        });
        countRows++;
    });
}

function successUpdateConvenio(rta) {
    $("#div_message_error").hide();
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "Convenio/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
        cerrarModal("modalCRUD");
        MostrarMensaje("Importante", "Edición exitosa.", "success");
    }
    else {
        MostrarMensaje("Importante", rta.Mensaje, "error");
        //$("#lbl_message_error").html(rta.Mensaje);
        //$("#div_message_error").show();
    }
}

//RDSH: Inicializa la pantalla de detalle
//EDSP: Se adiciona en el detalle el porcentaje de descuento : 29/12/2017
function InicializarDetalle() {
    var listitems = "";
    var listProductos = "";
    var fila = "";

    $.each(ListaDetalle, function (key, value) {
        listitems = "";
        listProductos = "";
        fila = "";

        $.each(TiposProductos, function (key, value2) {
            if (value.CodSapTipoProducto === value2.CodSAP)
                listitems += '<option selected value=' + value2.CodSAP + '>' + value2.Nombre + '</option>';
            else
                listitems += '<option value=' + value2.CodSAP + '>' + value2.Nombre + '</option>';
        });

        var resultado = $.grep(Productos, function (e) { return e.CodSapTipoProducto == value.CodSapTipoProducto; });

        $.each(resultado, function (key, value2) {
            if (value.CodSapProducto === value2.CodSapProducto)
                listProductos += '<option selected value=' + value2.CodSapProducto + '>' + value2.Nombre + '</option>';
            else
                listProductos += '<option value=' + value2.CodSapProducto + '>' + value2.Nombre + '</option>';
        });

        fila = "<tr>" +
                "<td style='display:none;'><input type='hidden' disabled name='ListaDetalle.IdConvenioDetalle' maxlength='5' class='form-control' value='" + value.IdConvenioDetalle + "' /></td>" +
                "<td style='display:none;'><input type='hidden' disabled name='ListaDetalle.CantidadDisponible' maxlength='5' class='form-control' value='" + value.CantidadDisponible + "' /></td>" +
                "<td><select style='width:160px;' disabled class='form-control required' name='ListaDetalle.CodSapTipoProducto' onchange='BuscarProductos(this)'><option value=''>Seleccione...</option>" + listitems + "</select></td>" +
                "<td><select style='width:300px;' disabled class='form-control required productos' name='ListaDetalle.CodSapProducto'><option value=''>Seleccione...</option>" + listProductos + "</select></td>" +
                "<td><input type='text' disabled name='ListaDetalle.Porcentaje' maxlength='5' class='form-control required' onkeypress='return validateFloatKeyPress(this, event);' onkeyup='EventoPorcentaje(this);' value ='" + (value.Porcentaje == 0 ? "" : value.Porcentaje) + "' /></td>" +
                "<td><input type='text' disabled name='ListaDetalle.Valor' maxlength='10' class='form-control required' onkeypress='return EsNumero(this);' value='" + value.Valor + "' /></td>" +
                "<td><input type='text' disabled name='ListaDetalle.Cantidad' maxlength='5' class='form-control required Cantidad' onkeypress='return EsNumero(this);' onchange='ValidarCantidad()' value='" + value.Cantidad + "'/></td>" +
                "<td><input type='text' disabled name='ListaDetalle.CantidadxDia' maxlength='5' class='form-control required CantidadxDia' onkeypress='return EsNumero(this);' onchange='ValidarCantidad()' value='" + value.CantidadxDia + "' /></td>" +
                "</tr>";
        $("#tbDetail tbody").append(fila);

    });
      
    //Oculta el boton guardar que va a base de datos.
    $("#btnSaveGeneric").hide();

    //Oculta el guardar personalizable.
    $("#btnSaveCustomizable").hide();
    
}

function setEventExclusion() {

    $("#btnCancelarExclusion").click(function () {
        LimpiarFechas();       
    });
    $("#btnAdicionar").click(function () {
        if (validarFormulario("frmExclusion"))
            AdicionarExclusion();
            //MostrarConfirm("Importante!", "¿Está seguro de agregar una exclusión de convenio? ", "AdicionarExclusion", "");
    });
}


function MostrarExclusiones(rta) {
    //Si tiene datos 
    debugger;
    if (rta.Correcto) {
        var _html = "<table class='table table-striped jambo_table' width='100%'>";
        _html += "<thead><tr><th>Fecha Inicio</th><th>Fecha Fin</th><th></th></tr></thead>";
        _html += "<tbody>";
        $.each(rta.Elemento, function (i, item) {
            _html += "<tr>";
                _html += "<td>" + item.strFechaInicio + "</td>";
                _html += "<td>" + item.strFechaFin + "</td>";
                _html += "<td> <a href='javascript:void(0)' onclick='eliminarExclusion(" + item.Id + ")'><i class='fa fa-trash' style='font-size: 18px;'></i></a></td>";
            _html += "</tr>";
        });

        _html += "</tbody>";
        _html += " </table>";

        $("#tblConvenios").html(_html);
        LimpiarFechas();
    } else {
        $("#tblConvenios").html("");
    }
}

function LimpiarFechas() {
    $('#strFechaInicio').data("DateTimePicker").clear();
    $('#strFechaFin').data("DateTimePicker").clear();
    $("#strFechaInicio").removeClass("errorValidate");
    $('#strFechaFin').removeClass("errorValidate");
}

//@ EDSP: Iniciar evento vista parcial productos convenio para fechas especiales 

function SetEventosProductos() {
    $("#btnCancelProd").click(function() {
        cerrarModal('modalCRUD');
    });
    debugger;
    //Si existe productos al convenio se precargan
    if (productosConvenio) {
        if (productosConvenio.length > 0) {

            var listitems = "";
            var listProductos = "";
            var fila = "";

            $.each(productosConvenio, function (key, value) {
                listitems = "";
                listProductos = "";
                fila = "";

                var codSapProducto = value.CodSapProducto;
                var codSapTipoProducto = "";

                $.each(Productos, function(i, obj){
                    if (obj.CodigoSap === codSapProducto)
                        codSapTipoProducto = obj.CodSapTipoProducto;
                });


                $.each(TiposProductos, function (key, value2) {
                    if (value2.CodSAP === codSapTipoProducto)
                        listitems += '<option selected value=' + value2.CodSAP + '>' + value2.Nombre + '</option>';
                    else
                        listitems += '<option value=' + value2.CodSAP + '>' + value2.Nombre + '</option>';
                });

                var resultado = $.grep(Productos, function (e) { return e.CodSapTipoProducto == codSapTipoProducto; });

                $.each(resultado, function (key, value2) {
                    if (value.CodSapProducto === value2.CodigoSap)
                        listProductos += '<option selected value=' + value2.CodigoSap + '>' + value2.Nombre + '</option>';
                    else
                        listProductos += '<option value=' + value2.CodigoSap + '>' + value2.Nombre + '</option>';
                });

                var fila = "<tr>" +
                       "<td><select style='width:160px;' class='form-control required TipProd' onchange='BuscarProductoConvenio(this);' ><option value=''>Seleccione...</option>" + listitems + "</select></td>" +
                       "<td><select style='width:300px;' class='form-control required prod' ><option value=''>Seleccione...</option>" + listProductos + "</select></td>" +
                       "<td><input type='text' maxlength='5' class='form-control required Cantidad' onkeypress='return EsNumero(this);' onkeyup='EventoNumero(this);' value='"+ value.Cantidad + "' /></td>" +
                       "<td><a href='javascript:void(0)' onclick='removeRow(this)'><i class='fa fa-trash'></i></a></td>" +
                       "</tr>";


                $("#tbDetail tbody").append(fila);

            });
        }
    }

    $("#btnAddProd").click(function () {
       var listitems = "";
        $.each(TiposProductos, function (key, value) {
            listitems += '<option value=' + value.CodSAP + '>' +value.Nombre + '</option>';
            });
        var fila = "<tr>" +
                        "<td><select style='width:160px;' class='form-control required TipProd' onchange='BuscarProductoConvenio(this);' ><option value=''>Seleccione...</option>" + listitems + "</select></td>" +
                        "<td><select style='width:300px;' class='form-control required prod' ><option value=''>Seleccione...</option></select></td>" +
                        "<td><input type='text' maxlength='5' class='form-control required Cantidad' onkeypress='return EsNumero(this);' onkeyup='EventoNumero(this);' /></td>" +
                        "<td><a href='javascript:void(0)' onclick='removeRow(this)'><i class='fa fa-trash'></i></a></td>" +
                        "</tr>";
        $("#tbDetail tbody").append(fila);
    });

    $("#btnSave").click(function() {
        
        if (validarFormulario("modalCRUD .modal-body")) {
            GuardarProductoConvenio();
        }
    });
}

//@Buscar productos por tipo de producto - convenip producto
function BuscarProductoConvenio(elemento) {

    $(elemento).parent().parent().find(".prod").html('<option value="">Seleccione...</option>').val('');

    if ($(elemento).val() !== "") {
        var result = $.grep(Productos, function (e) { return e.CodSapTipoProducto == $(elemento).val(); });
        if (result.length > 0) {

            var listitems;
            $.each(result, function (key, value) {
                listitems += '<option value=' + value.CodigoSap + '>' + value.Nombre + '</option>';
            });

            $(elemento).parent().parent().find(".prod").append(listitems);

        }
    }

}

//@ Evento numero
function EventoNumero(elemento) {
    var valor = $(elemento).val();
    if (valor.length > 0) {
        if ($(elemento).val() == 0) {
            $(elemento).val("");
        }
    }
}

function GuardarProductoConvenio() {

    //No es necesario puede haber equivocación

    //if ($(".prod").length == 0) {
    //    MostrarMensaje("Importante", "El convenio debe tener mínimo un producto.", "error");
    //    return;
    //}

    debugger;
    var repetidos = false;

    $.each($(".prod"), function (i, item) {

        $(item).removeClass("errorValidate");
        $(item).attr("data-mensajeerror", "");
        var _codSap = $(item).val();
        var cont = 0;
        $.each($(".prod"), function (a, b) {

            if ($(b).val() == _codSap) {
                cont++;
                if (cont == 2) {
                    $(b).addClass("errorValidate");
                    $(b).attr("data-mensajeerror", "Producto repetido.");
                    repetidos = true;
                }
            }
        });
    });

    if (repetidos) {
        MostrarMensaje("Importante", "Existen productos repetidos, revise los campos demarcados con color rojo.", "error");
        mostrarTooltip();
    } else {
        $(".tooltipError").hide();
        QuitarTooltip();

        //Crear objeto
        var listaProd = [];
        $.each($(".Cantidad"), function (key, element) {
            debugger;
            var obj = new Object();
            obj.IdConvenio = IdConvenio;
            obj.CodSapProducto = $(element).parent().parent().find(".prod").val();
            obj.Cantidad = $(element).val();

            listaProd.push(obj);
        });

        if (listaProd.length == 0) {
            //Elimnacion de todos los productos 
            var obj = new Object();
            obj.IdConvenio = IdConvenio;
            listaProd.push(obj);
        }


        debugger;
        EjecutarAjaxJson(urlBase + "Convenio/ActualizarProductosConvenio", "POST", { modelo: listaProd }, "successInsertConvenio", null);

    }
}

//@EDSP: Convierte los valores del campo a porcentaje a variable de tipo float
function ConvertirVariablesFloat(lista) {
    $.each(lista, function (i, item) {
        if (item.name.indexOf("Porcentaje") >= 0) {
            var data = item.value.split('.')
            if (data.length > 1) {
                if (data[1].length > 0) {
                    item.value = item.value.replace('.', ',');
                } else {
                    item.value = item.value.replace('.', '');
                }
            }
        }
    });

    return lista;
}

