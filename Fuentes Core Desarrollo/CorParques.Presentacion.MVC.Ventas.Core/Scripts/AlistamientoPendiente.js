
$(function () {
        if (document.getElementById('datatable-responsive').innerText.substring(17, (document.getElementById('datatable-responsive').innerText.length-1)) == "No existen datos disponibles") {
            MostrarMensaje("Importante", "No existen alistamientos pendientes", "info");
        }
});