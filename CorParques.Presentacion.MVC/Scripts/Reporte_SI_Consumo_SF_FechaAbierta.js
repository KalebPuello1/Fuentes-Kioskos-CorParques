console.log("Buenaaa --> Reporte_SI_Consumo_SF_FechaAbierta ")

$('#datetimepickerIni').datetimepicker({ format: 'YYYY-MM-DD' });
$('#datetimepickerfin').datetimepicker({ format: 'YYYY-MM-DD' });

var fInicial = $("#FInicial")
var fFinal = $("#FFinal")
var nPedido = $("#NumPedido")

var BASE_DIR = location.href

$("document").ready(() => {
    console.log(`${BASE_DIR}/Details`)
    $("#exportar").click((e) => {
        iniciarProceso();
        console.log(nPedido.val() + " " + fInicial.val() + " " + fFinal.val() + " . " + $("#NumPedido").val())
        e.preventDefault()
        console.log("lolando")
        $.ajax({
            url: `${BASE_DIR}/Details`,
            method: "GET",
            data: { fechaI: fInicial.val(), fechaF: fFinal.val(), Npedido: nPedido.val() == "" ? "null" : nPedido.val(), redencion: "0"},
            //data: { fechaI: '2018-03-03', fechaF: '2018-12-03', Npedido: "1000000009000010", redencion: "0"},
            success: (ee) => {
                if (ee != "No hay datos para exportar en estas fecha") {
                    if (ee.length > 0) {
                        Window, location = `${BASE_DIR}/download?dato=${ee}`;
                        finalizarProceso();
                        MostrarMensaje("Importante", `Descarga exitosa ${ee}`, "success");
                    }
                } else {
                    finalizarProceso();
                    MostrarMensaje("Importante", ee, "error");
                }
            },
            error: (e) => {
                console.log(e)
            }
        })
    })
})

function iniciarProceso() {
    $(".loader-wrapper").css("display", "block");
    $("#div_message_error").hide();
}

function finalizarProceso() {
    $(".loader-wrapper").css("display", "none");
}


/*if (ee.length > 0) {
    Window, location = `${BASE_DIR}/download?dato=${e}`
} else {
    ostrarMensaje("Importante", "NO HAY DATOS PARA EXPORTAR", "error");
}*/
