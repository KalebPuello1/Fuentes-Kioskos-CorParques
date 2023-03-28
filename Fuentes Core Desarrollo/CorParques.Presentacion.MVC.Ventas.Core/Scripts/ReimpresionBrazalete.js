var listaFacturaReimprimir = [];
var IdSupervisorLogueado = 0;
$(function () {
    

    $('#datatable-Reimpresion').DataTable({
        "fnDrawCallback": function (oSettings) {
            setEventSelecccionar();
        }
    });

    setEventSelecccionar();


    $("#btnReimprimir").click(function () {

        if (listaFacturaReimprimir.length > 0) {
            MostrarConfirm("Importante!", "¿Está seguro de reimprimir " + listaFacturaReimprimir.length + " brazaletes(s) seleccionadas ? ", "EventoReimprimirFacturas", "");

        }
    });

    EjecutarAjax(urlBase + "Cuenta/ObtenerLoginSupervisor", "GET", null, "printPartialModal", {
        title: "Confirmación supervisor", hidesave: true, modalLarge: false, OcultarCierre: true
    });

});
function ObtenerIdSupervisor(id) {
    IdSupervisorLogueado = id;
}

function EventoReimprimirFacturas() {
    EjecutarAjaxJson(urlBase + "BrazaleteReimpresion/Reimprimir", "POST", { IdsReimprimir: listaFacturaReimprimir, IdSupervisor: IdSupervisorLogueado }, "SuccessReimprimirFactura", null);
}

function SuccessReimprimirFactura(data) {
    if (data.Correcto) {
        listaFacturaReimprimir = [];

        EjecutarAjax(urlBase + "BrazaleteReimpresion/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventSelecccionar" });
        cerrarModal("modalCRUD");

        MostrarMensajeRedireccion("", 'Impresión Exitosa', null, "success");
    } else {

        if (data.Mensaje) {
            MostrarMensaje("", data.Mensaje, "error");
        } else {
            MostrarMensaje("", data, "error");
        }
    }
}

function setEventSelecccionar() {

    $(".checkbox-seleccionar-reimpresion").unbind();
    $(".checkbox-seleccionar-reimpresion").click(function () {

        var intIdFactura = $(this).data("id");
        if (this.checked) {
            listaFacturaReimprimir.push(intIdFactura);

        } else {

            $.each(listaFacturaReimprimir, function (i) {
                if (listaFacturaReimprimir[i] == intIdFactura) {
                    listaFacturaReimprimir.splice(i, 1);
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


