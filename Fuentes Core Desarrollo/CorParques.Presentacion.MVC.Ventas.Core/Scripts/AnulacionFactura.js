var listaFacturaAnular = [];
var listaAnularFinal = [];
var IdSupervisorLogueado = 0;
$(function () {

    //$("#datatable-responsive_1").DataTable();

    $('#datatable-Anulaciones').DataTable({
        "fnDrawCallback": function (oSettings) {
            setEventSelecccionar();
        }
    });

    setEventSelecccionar();
    

    $("#btnAnularFacturas").click(function () {

        //Limpiar lista para evitar duplicados EDSP - 23-01-2018
        listaAnularFinal = [];

        $(".textbox-observacion-anulacion").removeClass("errorValidate");
        $(".textbox-observacion-anulacion").attr("data-mensajeerror", "");        

        if (listaFacturaAnular.length > 0) {
            var isValid = true;
            
            listaFacturaAnular.forEach(function (item) {
                var objInputObs = $("#observaciones_" + item, $("#datatable-Anulaciones").DataTable().rows().nodes());

                if (objInputObs.val() == "" || objInputObs.val() == " ") {
                    
                    objInputObs.attr("data-mensajeerror", "Este campo es obligatorio");
                    objInputObs.addClass("errorValidate");
                    isValid = false;
                } else {
                    
                    listaAnularFinal.push(item + "|" + objInputObs.val().replace("|", ""));
                }
            });

            if (isValid) {
                MostrarConfirm("Importante!", "¿Está seguro de anular " + listaFacturaAnular.length + " factura(s) seleccionadas ? ", "EventoAnularFacturas", "");
            } else {
                mostrarTooltip();
                MostrarMensaje("Importante", "Hay inconsistencias en el formulario, revise los campos demarcados con color rojo.", "error");
            }
        }
    });

    $("#btnTerminarAnularFacturas").click(function () {
        if (urlBase == "/")
            window.location = "/";
        else
            window.location = urlBase + "/";
    });

    EjecutarAjax(urlBase + "Cuenta/ObtenerLoginSupervisor", "GET", null, "printPartialModal", {
        title: "Confirmación supervisor", hidesave: true, modalLarge: false, OcultarCierre: true
    });
    
});
function ObtenerIdSupervisor(id) {
    IdSupervisorLogueado = id;
}

function EventoAnularFacturas() {
    EjecutarAjaxJson(urlBase + "Anulacion/Anular", "POST", { IdsAnular: listaAnularFinal, IdSupervisor: IdSupervisorLogueado }, "SuccessAnularFactura", null);
}

function SuccessAnularFactura(data) {
    if (data.Correcto) {
        listaFacturaAnular = [];
        listaAnularFinal = [];

        EjecutarAjax(urlBase + "Anulacion/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventSelecccionar" });
        cerrarModal("modalCRUD");

        MostrarMensajeRedireccion("", 'Anulación Exitosa', null, "success");

        //mostrarAlerta('Estado', 'Anulación Exitosa.');
        //window.location = "/Anulacion";
    } else {
        //mostrarAlerta('Estado', data.Mensaje);
        //EDSP : Valida si retorna el objeto de lo contrario muestra el mensaje del objeto

        if (data.Mensaje) {
            MostrarMensaje("", data.Mensaje, "error");
        } else {
            MostrarMensaje("", data, "error");
        }
    }
}

function setEventSelecccionar() {

    $(".checkbox-seleccionar-anulacion").unbind();
    $(".checkbox-seleccionar-anulacion").click(function () {

        var intIdFactura = parseInt($(this).data("id"));
        if (this.checked) {
            listaFacturaAnular.push(intIdFactura);
            $("#observaciones_" + intIdFactura).removeAttr("disabled");

        } else {
            $("#observaciones_" + intIdFactura).attr("disabled", "disabled");
            $("#observaciones_" + intIdFactura).val("");
            $("#observaciones_" + intIdFactura).removeClass("errorValidate");
            $("#observaciones_" + intIdFactura).attr("data-mensajeerror", "");

            $.each(listaFacturaAnular, function (i) {
                if (listaFacturaAnular[i] === intIdFactura) {
                    listaFacturaAnular.splice(i, 1);
                    return false;
                }
            });
        }

    });
}

function CancelarLogin() {

    cerrarModal('modalCRUD');
    window.location = urlBase + "Home/Index";
}


