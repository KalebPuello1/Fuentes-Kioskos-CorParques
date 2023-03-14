$(function () {
    $("#btnSubir").on("click", function (e) {
        EjecutarAjax(urlBase + "BitacoraDia/Asignar", "GET", ObtenerObjeto("frmBitacora"), "successUpdate", null);
    });

    setNumeric();

});
function successUpdate(data) {
    mostrarAlerta(data.Elemento.Mensaje == "" ? "Bitacora Actualizada" : data.Elemento.Mensaje);
}
