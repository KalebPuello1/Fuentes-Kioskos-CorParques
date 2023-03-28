$(function () {

    //envio de notificacion
    $("#btnEnviar").on("click", function (e) {
        if (!validarFormulario("formularioNot *")) {
            return false;
        }
        if ($("#hdGrupos").val() == "") {
            mostrarAlerta('No se han asignado destinatarios para el envio de la notificación.');
            return false;
        }
        var objeto = ObtenerObjeto("formularioNot *");
        $.each(objeto, function(i,v){
            if(v.name==="Prioritario" && v.value==="on")
                v.value="true";
        });
        EjecutarAjax(urlBase + "Notificacion/EnviarNotificacion", "GET", objeto, "successSendMail", null)





    });
});
function successSendMail(data) {
    mostrarAlerta(data.Correcto ? data.Elemento : data.Mensaje);
    $("#gruposAtracciones").importTags('')
    $("#txtAsunto").val("");
    $("#txtContenido").val("");
    $("#chbxPrioritario").prop("checked", false);
    finalizarProceso();
}
function InitPartialCreate(dataGrupos) {

    tagsAutocomplete($('#gruposAtracciones'), $('#gruposAutocomplete'), $("#hdGrupos"), dataGrupos, false)

}
