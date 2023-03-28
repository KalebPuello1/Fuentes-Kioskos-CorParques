$(function () {
    $(".lnkView").click(function () {
        var id = $(this).data("id");
        //EjecutarAjax(urlBase + "Groups/Detail", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Detalle Grupo", hidesave: "Y", showreturn: "Y" });
        EjecutarAjax(urlBase + "InventarioOperacion/ObtenerDetalle", "GET", { IdProducto: id }, "printPartialModal", { title: "Detalle", hidesave: "Y", showreturn: "Y", modalLarge: true });

        //EjecutarAjax(urlBase + "InventarioOperacion/ObtenerDetalle", "GET", { IdProducto: id }, "SucessGet", null);

    });
});

function SucessGet(data)
{
    
}