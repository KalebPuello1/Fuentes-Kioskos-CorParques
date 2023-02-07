var local = location.href


$("document").ready(e => {


    $("#BtnBuscarBoletas").click(e => {

        var CampoConsecutivo = document.getElementById("Consecutivo").value;

        if (CampoConsecutivo == "") {
            MostrarMensaje("Importante", "El campo n° consecutivo vacío", "error");
        } else {

            iniciarProceso();
            var consecutivo = $("#Consecutivo").val()
            console.log(consecutivo)
            $.ajax({
                url: `${local}/ObtenerProducto`,
                method: "POST",
                data: { consecutivo: consecutivo },
                success: (e) => {
                    console.log(e)

                    if (e.hasOwnProperty("existe")) {
                        finalizarProceso();
                        MostrarMensaje("Importante", e.existe, "error")
                    } else {
                        $("#listView").html(e)
                        finalizarProceso();
                    }

                    $('#datetimepickerIni').datetimepicker({ format: 'YYYY-MM-DD' });
                    $('#datetimepickerfin').datetimepicker({ format: 'YYYY-MM-DD' });

                },
                error: (e) => {
                    console.log(e)
                }
            })
            console.log(consecutivo)


        }

    });


    $('#CodigoFactura').keyup(function (e) {


        var letters = /^[A-Za-z0-9]+$/;
        if (!e.key.match(letters)) {
            this.value = this.value.replace(e.key, '') + String.fromCharCode(45)
            return false;
        }
    });


    $("#BtnBuscarFactura").click(e => {

        var CampoCodFactura = document.getElementById("CodigoFactura").value;



        if (CampoCodFactura == "") {
            MostrarMensaje("Importante", "El campo código de factura vacío", "error");
        } else {

            iniciarProceso();
            $("#TxtFechaActualR").val("");
            var CodBarra = $("#CodigoFactura").val()
            $.ajax({
                url: `${local}/ObtenerFacturaReagendamiento`,
                method: "POST",
                data: { CodBarra: CodBarra },
                success: (e) => {
                    console.log(e)
                    if (e.hasOwnProperty("existe")) {
                        finalizarProceso();
                        MostrarMensaje("Importante", e.existe, "error")
                    } else {
                        $("#listView").html(e)
                        finalizarProceso();
                    }

                    $('#datetimepickerIni').datetimepicker({ format: 'YYYY-MM-DD' });
                    $('#datetimepickerFin').datetimepicker({ format: 'YYYY-MM-DD' });

                },
                error: (e) => {
                    console.log(e)
                }
            })
            console.log(CodBarra)

        }

    });





    function respuesta(dato) {
        console.log(dato)
    }

    function printPartiall(data, values) {
        $(values.div).html(data);
        window[values.func]();
    }

    function setEventEdit() {
        EstablecerToolTipIconos();
        var rdato = 0;
        $(".lnkEdit").click(function () {
            rdato = $(this).data("id")
        });

        $(".lnkDisable").click(function () {
            var dato = confirm("Esta seguro de enviar este codigo QR?")
        });
    }




});

function ModificarFecha() {

    var Pasaporte = $("#Consecutivo").val()

    var FechaActualR = $("#TxtFechaActualR").val();

    var FechaNuevaR = $("#TxtFechaNuevaR").val();

    console.log(Pasaporte, FechaActualR, FechaNuevaR);

    if (Pasaporte.length > 0 && FechaActualR.length > 0 && FechaNuevaR.length > 0) {

        $.ajax({
            url: `${local}/ModificarFecha`,
            method: "POST",
            data: { pasaportesR: Pasaporte, fechaactualR: FechaActualR, fechanuevaR: FechaNuevaR },
            success: (e) => {
                console.log(e.dato)
                if (!e.hasOwnProperty("error")) {
                    MostrarMensaje("Importante", e.dato, "success")
                    $("#TxtFechaNuevaR").val("")
                    var producto = e.ProductoModificado
                    var fi = e.fi
                    $("#TxtFechaActualR").html("")
                    console.log(producto)
                    console.log(e.fi)
                    var pintar =
                        $("#TxtFechaActualR").val(fi);
                    $("#TxtFechaActualR").append(pintar)

                } else {
                    MostrarMensaje("Importante", e.dato, "error")
                }
            },
            error: (e) => {
                console.log(e)
            }
        });
    } else {
        MostrarMensaje("Importante", "Por favor diligenciar todos los campos", "error")
    }

}

function ModificacionMasiva(checkbox) {

    if (checkbox.checked) {
        $(".ChxReagendar").attr('checked', true);
    }
    else {
        $(".ChxReagendar").attr('checked', false);

    }

}

function ModificacionEspecifica() {

    var codsapBonolluvia = 60000101;
    var codsapBonolluviaFiesta = 60000102;
    var IdPBonolluvia = 8726;
    var IdPBonolluviaFiesta = 8727;

    var ids = $('.ChxReagendar:checked').map(function () {
        return this.id;
    }).get();
    console.log(JSON.stringify(ids.join()));


    var nomess = $('.ChxReagendar:checked').map(function () {
        return this.name;
    }).get();
    console.log(JSON.stringify(nomess.join()));

    nomess.forEach((item) => {
        console.log(item);

        if (item == codsapBonolluvia || item == codsapBonolluviaFiesta) {

            alert("Factura contiene bono lluvia");
            $(".ChxReagendar").attr('checked', true);

        }
    });
}

function CancelarReagendamiento() {
    MostrarConfirm("Importante!", "¿Está seguro de cancelar la modificación? ", "Cancelar", "error");
}

function Cancelar() {
    window.location = urlBase + "Reagendamiento";

}

function ModificarFechaProductosFactura() {

    var CampoModificar = document.getElementById("TxtFechaNuevaR").value;


    if (CampoModificar == "") {
        MostrarMensaje("Importante", "No se ha seleccionado una fecha", "error");
    } else {

        var ids = $('.ChxReagendar:checked').map(function () {
            return this.id;
        }).get();
        console.log(JSON.stringify(ids.join()));

        ids.forEach((item) => {
            console.log(item);


            var Pasaporte = item;

            var FechaActualR = $("#TxtFechaActualR").val();

            var FechaNuevaR = $("#TxtFechaNuevaR").val();



            console.log(Pasaporte, FechaActualR, FechaNuevaR);

            if (Pasaporte.length > 0 && FechaActualR.length > 0 && FechaNuevaR.length > 0) {

                $.ajax({
                    url: `${local}/ModificarFecha`,
                    method: "POST",
                    data: { pasaportesR: Pasaporte, fechaactualR: FechaActualR, fechanuevaR: FechaNuevaR },
                    success: (e) => {
                        console.log(e.dato)
                        if (!e.hasOwnProperty("error")) {
                            MostrarMensaje("Importante", e.dato, "success")
                            $("#TxtFechaNuevaR").val("")
                            var producto = e.ProductoModificado
                            var fi = e.fi
                            $("#TxtFechaActualR").html("")
                            console.log(producto)
                            console.log(e.fi)
                            var pintar =
                                $("#TxtFechaActualR").val(fi);
                            $("#TxtFechaActualR").append(pintar)

                        } else {
                            MostrarMensaje("Importante", e.dato, "error")
                        }
                    },
                    error: (e) => {
                        console.log(e)
                    }
                });
            } else {
                MostrarMensaje("Importante", "Por favor diligenciar todos los campos", "error")
            }
        });
    }

}
