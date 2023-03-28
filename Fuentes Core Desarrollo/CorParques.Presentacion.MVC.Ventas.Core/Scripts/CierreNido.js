//Powered by RDSH
var paso = "";
var adelante = true;
var cantidadpasos = 0;
var ObservacionesRecoleccion;
var strObservacionesSupervisor = "";

$(function () {

    $("#DDL_Punto").change(function () {
        ConsultarRecoleccion();
    });
    asignarSelect2();    
    Inicializar();
});

//Se ejecuta al cambiar la seleccion en el combo taquillero.
function ConsultarRecoleccion() {

    var IdPunto = "";
    var IdUsuario = "";
    IdPunto = $("#DDL_Punto").val();    

    if (IdPunto != "")
    {
        var objSplit = IdPunto.split('|');
        IdPunto = objSplit[0];
        IdUsuario = objSplit[1];
        EjecutarAjax(urlBase + "CierreNido/BuscarRecoleccion", "GET", {IdPunto : IdPunto , IdSupervisor: parseInt(IdUsuario) }, "CargarPagina", null);
    
    } else {
        $('#ContenidoWizard').html("");
    }
}

//Muestra la pagina con el wizard de recoleccion.
function CargarPagina(data) {
    $('#ContenidoWizard').html(data);
    Inicializar();
}

//Guarda la informacion de la recoleccion.
function GuardarRecoleccion() {
    BloquearControles();
    $("#frmRecoleccion").submit();
}

//Esta funcion se ejecuta cuando el usuario haga clic en el boton de finalizar sobre el wizard.
function Finalizar() {

    var blnFlag = true;

    if (!ValidarBase()) {
        blnFlag = false;
        MostrarMensaje("Importante", "Revise las cantidades digitadas en la pestaña: Efectivo.", "error");
    }

    var sectionValidate = $(".stepContainer>div:visible");
    if (!validarFormulario(sectionValidate.attr("id") + " *"))
    {        
        blnFlag = false;
    }
        

    if (blnFlag) {
        MostrarConfirm("Importante", "¿Realmente desea guardar la información ingresada?", "MostrarResumen");
    }
}


function Total(txt, prefijoDeno, prefijoTotal, claseTotal) {
    var total = 0;
    var indice = "0";
    indice = txt.id.split('_')[1];

    if (txt.value.trim() != "") {
        total = (parseInt($("#" + prefijoDeno + indice).html().trim()) * parseInt(txt.value.trim()));
    }
    $("#" + prefijoTotal + '_' + indice).html(FormatoMoneda(total));
    Totalizar(claseTotal, prefijoTotal);
    CrearDetalleEfectivo(indice, txt.value.trim());
}

function Totalizar(claseTotal, prefijoTotal) {
    var Total = 0;

    $("." + claseTotal).each(function (index, element) {
        if ($("#" + element.id).html().trim().length > 0) {
            Total = Total + parseInt(RemoverFormatoMoneda($("#" + element.id).html().trim()));
        }
    });

    $("#div_" + prefijoTotal).html(FormatoMoneda(Total));
    //ColorTotal(prefijoTotal);

}

//Pasa la seleccion del check box a la entidad.
function SeleccionarCheckBox(chk, clase) {
    if (chk.checked) {
        $("#" + chk.id).attr("value", "true");
    } else {
        $("#" + chk.id).attr("value", "false");
    }
    CrearDetalleVoucherDocumentos(chk);
    TotalizarCheckBox(chk, clase);
}

function Validaciones(clasevalores, prefijoTotal) {

    var indice = "0";
    var blnResultado = true;

    $("." + clasevalores).each(function (index, element) {
        if ($("#" + element.id).val().trim().length > 0) {
            if (parseInt($("#" + element.id).val().trim()) > 0) {
                indice = element.id.split('_')[1];

                if ($("#" + prefijoTotal + indice).html().trim().length <= 0) {
                    blnResultado = false;
                    return false;
                } else if (parseInt($("#" + prefijoTotal + indice).html().trim()) <= 0) {
                    blnResultado = false;
                    return false;
                }
            }
        }
        else {
            blnResultado = false;
            return false;
        }
    });

    return blnResultado;

}

function ValidarBase() {

    var ValorBase = 0;
    var MaximoBase = 0;

    //if ($("#div_Total").html() !== "" && $("#objRecoleccionAuxliar_MaximoCierre").val() !== "") {
    //    ValorBase = RemoverFormatoMoneda($("#div_Total").html());
    //    MaximoBase = parseInt($("#objRecoleccionAuxliar_MaximoCierre").val());

    //    if (ValorBase > MaximoBase) {
    //        return false;
    //    }
    //}
    //else {
    //    return false;
    //}

    return true;

}

function ColorTotal(prefijoTotal) {
    var bnlCambiarColor = false;

    if (prefijoTotal.indexOf("Corte") >= 0) {
        bnlCambiarColor = ValidarCorte();
    }
    else {
        bnlCambiarColor = ValidarBase();
    }

    if (!bnlCambiarColor) {
        $("#div_" + prefijoTotal).css('color', 'red');
    }
    else {
        $("#div_" + prefijoTotal).css('color', 'green');
    }
}

function PasoValido(obj) {

    return $("#" + obj).val() == "True";

}

function Inicializar() {

    $("#wizard1").smartWizard({
        onLeaveStep: function (obj, context) {
            var sectionValidate = $(".stepContainer>div:visible");
            if (!validarFormulario(sectionValidate.attr("id") + " *"))
            {                
                return false;
            }
                

            if (context.fromStep > context.toStep) {
                adelante = false;
            } else {
                adelante = true;
            }

            if (context.fromStep == 1 && context.toStep != 1) {
                if (!ValidarBase()) {
                    return false;
                }
            }

            return true;
        },
        onFinish: Finalizar
    });


    $(".buttonFinish").addClass("btn btn-success");
    $(".buttonNext").addClass("btn btn-primary");
    $(".buttonPrevious").addClass("btn btn-info");

    $("#div_MaximoBase").html(FormatoMoneda($("#div_MaximoBase").html()));
    $("#div_MaximoCorte").html(FormatoMoneda($("#div_MaximoCorte").html()));
    $("#div_TotalVentasDiaB").html(FormatoMoneda($("#div_TotalVentasDiaB").html()));
    $("#div_TotalVentasDiaC").html(FormatoMoneda($("#div_TotalVentasDiaC").html()));

    if (parseInt($("#IdRecoleccion").val()) > 0) {
        InicializarEdicion();
    }
    $("#btn_AceptarResumen").click(function () {
        MostrarLogin();
    });

    SetInhabilitarCopiarPegarCortar();
    EstablecerFormatoMoneda();
    TotalizarCheckBoxInicial('voucher');
    TotalizarCheckBoxInicial('documentos');
    TotalizarCheckBoxInicial('novedad');

    //Toma el id del punto seleccionado en el drop down list.

    var IdPunto = "";    
    IdPunto = $("#DDL_Punto").val();
    var objSplit = IdPunto.split('|');
    IdPunto = objSplit[0];


    $("#IdPuntoCierre").val(IdPunto);

}

function PostBackGuardar() {
    var Resultado = $("#div_ResultadoRecoleccion").html();
    if (Resultado != undefined) {
        if (Resultado.indexOf("Error") >= 0) {
            MostrarMensajeRedireccion("Importante", Resultado, "Home/Index", "error");
        }
        else {
            MostrarMensajeRedireccion("Importante", Resultado, "Home/Index", "success");
        }
        return false;
    }
    return true;
}

function ValidarTotalRecoleccion() {
    var ValorBase = 0;
    var ValorCorte = 0;
    var TotalVentasDia = 0;
    var TotalRecoleccion = 0;
    var TotalCortesRealizados = 0;

    if (PasoValido('objRecoleccionAuxliar_MostrarBase')) {
        ValorBase = RemoverFormatoMoneda($("#div_Total").html());
    }

    if (PasoValido('objRecoleccionAuxliar_MostrarCorte')) {
        ValorCorte = RemoverFormatoMoneda($("#div_TotalCorte").html());
    }

    TotalRecoleccion = ValorBase + ValorCorte;
    TotalVentasDia = $("#objRecoleccionAuxliar_TotalVentasDia").val();
    TotalCortesRealizados = $("#objRecoleccionAuxliar_ValorCortesRealizados").val();

    //Si el total de recoleccion entre base y corte o solo corte es mayor que las ventas realizadas en el dia menos el valor de los cortes realizados
    //No permite guardar la recoleccion.
    return TotalRecoleccion > (TotalVentasDia - TotalCortesRealizados);

}

function InicializarEdicion() {
    $(".convalorbase").each(function (index, element) {
        Total(element, 'D_', 'Total', 'Total_Base')
    });

}

function MostrarLogin()
{
    CerrarResumen();
    EjecutarAjax(urlBase + "CierreNido/ObtenerLogin", "GET", null, "printPartialModal", {title: "Confirmación supervisor", hidesave: true, modalLarge: false});
}

function Login(password, observaciones) {

    var IdUsuario = 0;

    IdUsuario = RetornarIdUsuario();
    strObservacionesSupervisor = observaciones;
    $("#Observaciones").val(strObservacionesSupervisor);
    EjecutarAjax(urlBase + "Cuenta/ValidarPassword", "GET", {
        idUsuario: IdUsuario, password: password
    }, "respuestaLogin", null);

}

function respuestaLogin(data) {

    urlaction = urlBase + "CierreNido/GuardarObservaciones";
    if (data.Correcto) {
        CrearObjetoObservacion();
        $.ajax({
            url: urlaction,
            ContentType: 'application/json; charset-uft8',
            dataType: 'JSON',
            async: false,
            type: 'POST',
            data: { objObservacionRecoleccion: ObservacionesRecoleccion},
            success: function (r) {
                GuardarRecoleccion();
            },
            error: function (a, b, c) {
                MostrarMensaje("Importante", "No fue posible guardar la información, por favor intentelo nuevamente.", "error");
            }

        })
        cerrarModal("modalCRUD");
    } else {
        MostrarMensaje("Importante", "Contraseña incorrecta", "error");
    }

}

//RDSH: Crea el objeto observaciones para enviar a guardar.
function CrearObjetoObservacion()
{    
    var IntIdRecoleccion = 0;
    IntIdRecoleccion = parseInt($("#IdRecoleccion").val());
    ObservacionesRecoleccion = { IdObservacionRecoleccion: 0, IdRecoleccion: IntIdRecoleccion, IdUsuarioCreacion: 0, FechaCreacion: ObtenerFechaActual(), Observacion: strObservacionesSupervisor };
    strObservacionesSupervisor = "";
}

//Oculta los controles del formulario mientras hace el envio de la información.
function BloquearControles()
{
    $("#DDL_Punto").attr("disabled", "disabled");
    $(".actionBar").hide();
}

//Retorna el id del usuario para la validacion de login.
function RetornarIdUsuario()
{

    var IdPunto = "";
    var IdUsuario = "";
    IdPunto = $("#DDL_Punto").val();    

    if (IdPunto != "") {
        var objSplit = IdPunto.split('|');
        IdUsuario = objSplit[1];
    }

    return IdUsuario;
}

function MostrarResumen() {
    $('#myModal').modal('show');
    TotalizarResumen();
}

function TotalizarResumen() {
    $(".convalorbaseresumen").each(function (index, element) {
        Total(element, 'DR_', 'TotalResumen', 'Total_BaseResumen')
    });
}

//Busca el txt de detalle efectivo que comienza con DEF_ para asignarle el valor que digito el nido en el detalle de efectivo.
function CrearDetalleEfectivo(indice, valor)
{
    $("#DEF_" + indice).val(valor);
}

//Busca el txt de detalle boleteria que comienza con DBO_ para asignarle el valor que digito el nido en el detalle de boleteria.
function CreaDetalleBoleteria(txt)
{
    var indice = "0";
    indice = txt.id.split('_')[1];
    $("#DBO_" + indice).val(txt.value.trim());
}

//Actualiza el estado del check box de detalle para voucher, documentos y novedades.
function CrearDetalleVoucherDocumentos(checkbox)
{
    var indice = "0";
    indice = checkbox.id.split('_')[1];

    switch (checkbox.id.split('_')[0]) {
        case "chkv":
            $("#chkvd_" + indice).prop("checked", checkbox.checked);
            break;
        case "chkd":
            $("#chkdd_" + indice).prop("checked", checkbox.checked);
            break;
        case "chkn":
            $("#chknd_" + indice).prop("checked", checkbox.checked);
            break;
    }
}

//Formatea las columnas con el formato de moneda.
function EstablecerFormatoMoneda() {
    $(".formato_moneda").each(function (index, element) {
        element.innerText = FormatoMoneda(element.innerText);
    });
}

///Inhabilita las opciones de copiar pegar y cortar dentro de un text box
function SetInhabilitarCopiarPegarCortar() {
    $(".cantidadbase").each(function (index, element) {
        InhabilitarCopiarPegarCortar(element.id);
    });
    $(".boleteria").each(function (index, element) {
        InhabilitarCopiarPegarCortar(element.id);
    });
}
//Cierra popup de resumen.
function CerrarResumen() {
    cerrarModal("myModal");
}

function CancelarLogin() {
    cerrarModal('modalCRUD');
}


///Funcion para marcar o desmarcar todos los check box de una columna.
function SeleccionarTodos(Control, clase) {

    $("#div_Total" + clase).html("");
    $("#div_Totalr" + clase).html("");
    $("." + clase).each(function (index, element) {
        element.checked = Control.checked;
        TotalizarCheckBox(element, clase, Control.checked);
        CrearDetalleVoucherDocumentos(element);
    });

}

///Funcion para totalizar voucher, documentos y novedades según la selección realizada.
function TotalizarCheckBox(checkbox, clase) {
    var Total = 0;
    var indice = $("#" + checkbox.id).attr("data-indice");
    var TotalAcumulado = $("#div_Total" + clase).html().trim();


    if (TotalAcumulado.trim().length > 0)
        Total = parseInt(RemoverFormatoMoneda(TotalAcumulado));


    if (checkbox.checked) {
        Total = Total + parseInt(RemoverFormatoMoneda($("#valor" + clase + '_' + indice).html().trim()));
    }
    else {
        if (Total > 0)
            Total = Total - parseInt(RemoverFormatoMoneda($("#valor" + clase + '_' + indice).html().trim()));
    }

    if (Total > 0) {
        $("#div_Total" + clase).html(FormatoMoneda(Total));
        $("#div_Totalr" + clase).html(FormatoMoneda(Total));

    } else {
        $("#div_Total" + clase).html("");
        $("#div_Totalr" + clase).html("");
    }
}

///Como los check box inicial marcados al cargar la pagina, se debe totalizar al cargar la pagina.
function TotalizarCheckBoxInicial(clase) {


    $("." + clase + ":checked").each(function (index, element) {
        TotalizarCheckBox(element, clase);
    });

}
