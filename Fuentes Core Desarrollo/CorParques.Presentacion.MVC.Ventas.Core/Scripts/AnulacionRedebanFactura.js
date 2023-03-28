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
                console.log(item);
                var objInputObs = $("#observaciones_" + item, $("#datatable-Anulaciones").DataTable().rows().nodes());
                console.log(objInputObs.val());
                if (objInputObs.val() == "" || objInputObs.val() == " ") {

                    objInputObs.attr("data-mensajeerror", "Este campo es obligatorio");
                    objInputObs.addClass("errorValidate");
                    isValid = false;
                } else {
                    listaAnularFinal.push(item.replaceAll("_", "|") + "|" + objInputObs.val().replace("|", ""));
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
    EjecutarAjaxJson(urlBase + "AnulacionRedeban/Anular", "POST", { IdsAnular: listaAnularFinal, IdSupervisor: IdSupervisorLogueado }, "SuccessAnularFactura", null);
}

function SuccessAnularFactura(data) {
    if (data.Correcto) {
        listaFacturaAnular = [];
        listaAnularFinal = [];
        EjecutarAjax(urlBase + "AnulacionRedeban/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventSelecccionar" });
        cerrarModal("modalCRUD");
        MostrarMensajeRedireccion("", 'Anulación Exitosa', null, "success");

    } else {
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
        var numReferencia = $(this).data("id");
        if (this.checked) {
            listaFacturaAnular = [];
            listaFacturaAnular.push(numReferencia);
            var groupInput = $("input:text[name='redeban']");
            $(groupInput).prop("disabled", true);
            $("#observaciones_" + numReferencia).removeAttr("disabled");
            var groupCheckbox = $("input:checkbox[name='redeban']");
            $(groupCheckbox).prop("checked", false);
            document.getElementById(numReferencia).checked = true;

        } else {
            $(numReferencia).removeAttr("checked");
            $("#observaciones_" + numReferencia).attr("disabled", "disabled");
            $("#observaciones_" + numReferencia).val("");
            $("#observaciones_" + numReferencia).removeClass("errorValidate");
            $("#observaciones_" + numReferencia).attr("data-mensajeerror", "");

            $.each(listaFacturaAnular, function (i) {
                if (listaFacturaAnular[i] === numReferencia) {
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


