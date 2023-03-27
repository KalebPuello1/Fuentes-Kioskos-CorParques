//Powered by RDSH

$(function () {

    Inicializar();

});

function Inicializar()
{
    $(".txtBuscador").keyup(function () {
        var texto = $(this).val();
        //if ((texto.length <= 3) && (isInt(texto)))
        //    texto = "# " + texto;

        $.each($(".ProductoGrilla_Generic"), function (i, v) {
            //if ($(v).html().toLowerCase().indexOf(texto.toLowerCase()) >= 0)
            if (MostrarProducto($(v).html(), texto))
                $(v).show();
            else
                $(v).hide();
        })
    });

    $(".productos .ProductoGrilla_Generic").click(function (e) {
        var id = $(this).data("id");
        var NombreProducto = ObtenerNombreProducto(id);

        MostrarConfirm("Importante", "¿Está seguro que desea entregar el premio: " + NombreProducto + "?", "DescargarPremio", id);

    });

    $("#btnLimpiar").click(function () {
        $(".txtBuscador").val("");
        $.each($(".ProductoGrilla_Generic"), function (i, v) {
             $(v).show();          
        })
    });

}

function MostrarProducto(buscaren, valorbuscado)
{
    var posicioninicial = 0;
    var posicionfinal = 0;
    var textoencontrado = "";
    var blnRetorno = false;

    posicioninicial = buscaren.indexOf('<label');
    posicionfinal = buscaren.indexOf('</label>');
    textoencontrado = buscaren.substring(posicioninicial + 7, posicionfinal);
    posicioninicial = textoencontrado.indexOf('>');
    posicionfinal = textoencontrado.length;
    textoencontrado = textoencontrado.substring(posicioninicial + 1, posicionfinal);

    if (textoencontrado.length > 0)
    {
        if (textoencontrado.toLowerCase().indexOf(valorbuscado.toLowerCase()) >= 0)
        {
            blnRetorno = true;
        }
    }

    return blnRetorno;

}

function ObtenerNombreProducto(id)
{

    return $("#Producto_" + id).text();

}

function DescargarPremio(id)
{
    var strCodigoSapProducto = "";
    strCodigoSapProducto = $("#hdf_" + id).val();

    EjecutarAjax(urlBase + "EntregaPremios/EntregarPremio", "GET", { IdProducto: id, CodigoSap: strCodigoSapProducto}, "RespuestaDescargaPremio", null);
}

function RespuestaDescargaPremio(rta)
{
    if (rta.Correcto) {
        MostrarMensaje("Importante", "Premio descargado exitosamente.", "success");
    }
    else {
        MostrarMensaje("Importante", rta.Mensaje, "error");
    }
}