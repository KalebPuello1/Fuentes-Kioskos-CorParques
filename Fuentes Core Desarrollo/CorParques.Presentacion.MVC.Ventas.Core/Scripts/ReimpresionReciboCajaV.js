//$("#fechaI").datetimepicker({ format: 'YYYY-MM-DD' })
//$("#fechaF").datetimepicker({ format: 'YYYY-MM-DD' })

var local = location.href
var FechaI = $("#fechaI");
var FechaF = $("#fechaF");
$("#datetimepickerIni").datetimepicker({ format: 'YYYY-MM-DD' })
$("#datetimepickerFin").datetimepicker({ format: 'YYYY-MM-DD' })
$("document").ready(() => {

    /*var ft = $('#example').DataTable({
        bFilter: false,// bInfo: false,
        "language": {
            "search": "Buscar:",
            "paginate": {
                "previous": "Anterior",
                "next": "Siguiente",
            }
        },
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "Todos"]],
        "pageLength": 10,
        //"dom": 'Bfrtip',

    });*/

    $("#enviar").click(() => {
        //EjecutarAjax(`${local}/Details`, "GET", { datoI: FechaI.val(), datoF: FechaF.val() }, "printPartial", { div: "#listDato", func: "setEventEdit"})
        EjecutarAjax(`${local}/Details`, "GET", { datoI: FechaI.val(), datoF: FechaF.val() }, "printPartial", { div: "#listDato", func: "setEventEdit" })
        console.log(`${local}/Details`)
    })
})


function printPartial(data, values) {
    $(values.div).html(data);
    
    if (!values.table) {
        
        $(values.div).find("table").DataTable();

        $(values.div).find("table").on('draw.dt', function () {
            setEventEdit()
        });
        
    }
    console.log("pasooo print")
    window[values.func]();


}

function setEventEdit() {
    EstablecerToolTipIconos();
    console.log("pasooo edit")
    var rdato = 0;
    $(".linkDesacarga").click(function () {
        rdato = $(this).data("id")
        //EjecutarAjax(urlBase + `${local}/Reimprimir`, "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Dato Codigo Fecha Abierta", url: urlBase + "CodigoFechaAbierta/editar", metod: "PUT", func: "successUpdateDestrezas", modalLarge: true });
        EjecutarAjax(`${local}/Reimprimir`, "GET", { numPedido: $(this).data("id") }, "cargar", null);
        //EjecutarAjax(urlBase + "CodigoFechaAbierta/Obtener", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar punto CodigoFechaAbierta", url: urlBase + "CodigoFechaAbierta/editar", metod: "PUT", func: "successUpdateDestrezas", modalLarge: true });
        //EjecutarAjax(urlBase + "CodigoFechaAbierta/Obtener", "GET", { id: $(this).data("id") }, "printPartialModal", { div: "#listView", func: "setEventEdit" });
    });

    $(".lnkDisable").click(function () {
        /*  MostrarConfirm("Importante", "¿Está seguro que desea inactivar este registro?", "EliminarDestreza", $(this).data("id"));
          //if (confirm("Está seguro que desea eliminar esta destreza?"))
          //    EjecutarAjax(urlBase + "Destrezas/Delete", "GET", { id: $(this).data("id") }, "successDeleteDestrezas", null);
        */
        var dato = confirm("Esta seguro de enviar este codigo QR?")
        //EjecutarAjax(urlBase + "CodigoFechaAbierta/EnviarCorreo", "GET", { id: $(this).data("id"), lol: "Successfully" }, "printPartialModal", { title: "Enviar Codigo Fecha Abierta", url: urlBase + "CodigoFechaAbierta/editar", metod: "PUT", func: "successUpdateDestrezas", modalLarge: true });
        EjecutarAjax(urlBase + "CodigoFechaAbierta/EnviarCorreo", "GET", { id: $(this).data("id"), lol: "Successfully" }, "printPartialModall", { div: "#listView", func: "setEventEdit" });
        successUpdateDestrezas()
    });
}

var cargar = (e) => {
    console.log(" ------> ")
}