var HoraInicio;
var HoraFin;
var IdReserva;
$(function () {

    setEventEdit();    

    $('#btnSaveGeneric').click(function () {
        
        var _Reserva = new Object();
        _Reserva.Consecutivo = $("#Consecutivo").val();
        _Reserva.HoraInicio = $("#HoraInicio").val();
        _Reserva.HoraFin = $("#HoraFin").val();
        _Reserva.FechaReserva = $("#FechaReserva").val();
        _Reserva.IdTicket = 0;
        _Reserva.IdPunto = $("#IdPunto").val();
        _Reserva.Capacidad = $("#Capacidad").val();
        
        EjecutarAjax(urlBase + "ReservaSkycoaster/InsertaReserva", "POST", JSON.stringify(_Reserva), "successInsert", _Reserva.IdPunto);
        $("#diverror").hide();
        $(".alert-notification").hide();
    });


    $("#ddlPunto").change(function () {
        
        EjecutarAjax(urlBase + "ReservaSkycoaster/ObtenerLista", "GET", { id: $(this).val() }, "printPartial", { div: "#listView", func: "setEventEdit" });
    });

});

function successInsert(rta, id) {
    
    $("#diverror").hide();
    $(".alert-notification").hide();

    EjecutarAjax(urlBase + "ReservaSkycoaster/ObtenerLista", "GET", { id: id }, "printPartial",
                {
                    div: "#listView", func: "setEventEdit"
                });

    if (rta.Correcto) {
        cerrarModal("modalCRUD");
        //MostrarMensajeRedireccion("Importante", "Su operación fue exitosa.", "ReservaSkycoaster/Index?id=" + id, "success");
        MostrarMensaje("Importante", "Su operación fue exitosa.", "success");
    }
    else {
        cerrarModal("modalCRUD");
        MostrarMensaje("Importante", rta.Mensaje, "info");
    }
}

function setEventEdit() {
    EstablecerToolTipIconos();
    $('.reserva').click(function () {
        
        HoraInicio = $(this).data("horainicio");
        HoraFin = $(this).data("horafin");
        var IdPunto = $(this).data("punto");
        var capacidad = $(this).data("capacidad");

        EjecutarAjax(urlBase + "ReservaSkycoaster/ReservaModal", "GET", { HoraInicio: HoraInicio, HoraFin: HoraFin, IdPunto: IdPunto, Capacidad : capacidad }, "printPartialModal", {
            title: "Detalle Reserva", hidesave: false, modalLarge: false, OcultarCierre: true
        });
        
    });

    $(".liberar").click(function () {

        HoraInicio = $(this).data("horainicio");
        HoraFin = $(this).data("horafin");
        var IdPunto = $(this).data("punto");
        var obj = { HInicio: HoraInicio, Hfinal: HoraFin, IdPunto: IdPunto };
        MostrarConfirm("Importante!", "Desea liberar esta reserva", "Liberar", obj);


    });


   

    $(".Cerrar").click(function () {
        
        HoraInicio = $(this).data("horainicio");
        HoraFin = $(this).data("horafin");
        var IdPunto = $(this).data("punto");
        var capacidad = $(this).data("capacidad");

        var obj = { HInicio: HoraInicio, Hfinal: HoraFin, IdPunto: IdPunto, Capacidad: capacidad };
        MostrarConfirm("Importante!", "Desea cerrar esta reserva", "CerrarReserva", obj);

    });
}


function Liberar(objeto) {

    var _Reserva = new Object();
    _Reserva.HoraInicio = objeto.HInicio;
    _Reserva.HoraFin = objeto.Hfinal;
    _Reserva.IdPunto = objeto.IdPunto;
    
    EjecutarAjax(urlBase + "ReservaSkycoaster/LiberarReserva", "POST", JSON.stringify(_Reserva), "successProceso", _Reserva.IdPunto);

    

}

function CerrarReserva(objeto) {
    
    var _Reserva = new Object();
    _Reserva.HoraInicio = objeto.HInicio;
    _Reserva.HoraFin = objeto.Hfinal;
    _Reserva.IdPunto = objeto.IdPunto;
    _Reserva.IdTicket = 0
    _Reserva.Capacidad = objeto.Capacidad;
    
    EjecutarAjax(urlBase + "ReservaSkycoaster/CerrarReserva", "POST", JSON.stringify(_Reserva), "successProceso", _Reserva.IdPunto);

}



function successProceso(rta, id) {
    
    $("#diverror").hide();
    $(".alert-notification").hide();
    if (rta.Correcto) {
        cerrarModal("modalCRUD");
        EjecutarAjax(urlBase + "ReservaSkycoaster/ObtenerLista", "GET", { id: id }, "printPartial",
                {
                    div: "#listView", func: "setEventEdit"
                });
        MostrarMensaje("Importante", "Su operación fue exitosa.", "success");
    }
    else {
        cerrarModal("modalCRUD");
        MostrarMensaje("Importante", rta.Mensaje, "info");
    }
}
