//Powered by RDSH.

$(function () {

    asignarEventoCalendario();

    $("#btnMostrarBitacora").click(function () {
        $('#myModalBitacora').modal('show');
    });
    
});

function asignarEventoCalendario() {
    $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
        EjecutarAjax(urlBase + "DashBoard/CargarDashBoard", "GET", { FechaInicial: picker.startDate.format('DD-MM-YYYY'), FechaFinal: picker.endDate.format('DD-MM-YYYY') }, "successLoadDashboard", null);
        //console.log("apply event fired, start/end dates are " + picker.startDate.format('DD-MM-YYYY') + " to " + picker.endDate.format('DD-MM-YYYY'));
    });
}

function successLoadDashboard(data) {
    $("#listView").html(data);
    loadCalendar();
    //init_daterangepicker();
    LoadDashboard();
    ValidarBitacora();
    //asignarEventoCalendario();
}

//Genera grafico donas.
function GenerarGraficoDona(Valores, NombreDona) {

    var Colores = ["#3498DB", "#00A8B4", "#9B59B6", "#9CC2CB", "#E74C3C", "#FFC300", "#FF00FF", "#34495E", "#FF8C00", "#00FF7F"];

    var ArregloProductos = [];
    var ArregloValores = [];
    var ArregloColores = [];
    var Contador = 0;

    $.each(Valores, function (i, v) {
        ArregloProductos.push([v.Producto]);
        ArregloValores.push([v.Cantidad]);
        ArregloColores.push(Colores[Contador]);
        Contador = Contador + 1;
    });

    if (typeof (Chart) === 'undefined') { return; }

    console.log('GenerarGraficoDona ' + NombreDona);

    //if ($('.canvasDoughnut').length) {
    if ($('#' + NombreDona).length) {
        var chart_doughnut_settings = {
            type: 'doughnut',
            tooltipFillColor: "rgba(51, 51, 51, 0.55)",
            data: {
                labels: ArregloProductos,
                datasets: [{
                    data: ArregloValores,
                    backgroundColor: ArregloColores,
                    hoverBackgroundColor: []
                }]
            },
            options: {
                legend: false,
                responsive: false
            }
        }

        //$('.canvasDoughnut').each(function () {
        $('#' + NombreDona).each(function () {
            var chart_element = $(this);
            var chart_doughnut = new Chart(chart_element, chart_doughnut_settings);

        });

    }

}


//Genera grafico de barras horizontal.
function GenerarBarras(Valores, NombreBarras, Etiqueta) {

    var ArregloEtiquetas = [];
    var ArregloValores = [];

    $.each(Valores, function (i, v) {
        ArregloEtiquetas.push([v.Producto]);
        ArregloValores.push(v.Cantidad);
    });

    var div_Barras = $("#" + NombreBarras);

    var horizontalBarChartData = {
        labels: ArregloEtiquetas,
        datasets: [{
            label: Etiqueta,
            borderWidth: 1,
            data: ArregloValores
        }]

    };

    var myBarChart = new Chart(div_Barras, {
        type: 'horizontalBar',
        data: horizontalBarChartData
    });

}

//Grafico para representar las ventas por hora.
function GraficoVentasPorHora(Valores) {

    var ArregloEtiquetas = [];
    var ArregloValores = [];

    $.each(Valores, function (i, v) {
        ArregloEtiquetas.push(Formato12Horas(v.Hora));
        ArregloValores.push(v.Total);
    });

    var data = {
        labels: ArregloEtiquetas,
        datasets: [
          {
              label: "Ventas $",
              lineTension: 0,
              backgroundColor: "rgba(143,199,232,0.2)",
              borderColor: "rgba(108,108,108,1)",
              borderWidth: 1,
              pointBackgroundColor: "#535353",
              data: ArregloValores
          }
        ]
    };

    var myChart = new Chart($("#ventas_por_hora"), {
        type: 'line',
        data: data,
        options: {
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        return FormatoMoneda(tooltipItem.yLabel);
                    }
                }
            },
            animation: false,
            legend: { display: false },
            maintainAspectRatio: false,
            responsive: true,
            responsiveAnimationDuration: 0,
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true,
                        callback: function (value, index, values) {
                            return FormatoMoneda(value);
                        }
                    }
                }]
            }
        }
    });
}

function CerrarBitacora() {
    cerrarModal("myModalBitacora");
}

function ValidarBitacora()
{
    if ($("#hdf_Bitacora").val() == "S") {
        $("#div_boton_Bitacora").show();
        if ($("#hdf_TieneObservaciones").val() == "1") {
            $("#btnMostrarBitacora").removeClass("btn-default");
            $("#btnMostrarBitacora").addClass("btn-success");
        } else {
            $("#btnMostrarBitacora").removeClass("btn-success");
            $("#btnMostrarBitacora").addClass("btn-default");
        }
    } else {
        $("#div_boton_Bitacora").hide();
    }
}