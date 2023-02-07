$(function () {

   

    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "Atracciones/GetPartial", "GET", null, "printPartialModal", { title: "Crear Punto", url: urlBase + "Atracciones/Insert", metod: "GET", func: "successInsertAttraction", modalLarge: true });
    });
    setEventEdit();
});

function setFileLoad(prev)
{
    fileDropZone($("#dropzoneElement"), urlBase + "Atracciones/saveFile", "Productos/RemoveFile", "image/*", $("#Imagen"), prev, urlBase + "Images/Attractions/");
    $(".dateTime").datetimepicker({
        format: 'HH:mm',
    });
}
function setEventStatus()
{
    $("#EstadoId").change(function () {
        $("#DescripcionCierre").parent().parent().hide();
        $("#DescripcionCierre").removeClass("required");
        if ($(this).val() == "4")
        {
            $("#DescripcionCierre").parent().parent().show();
            $("#DescripcionCierre").addClass("required");
            $("#DescripcionCierre").val("");
        }
    });
}
    function setEventEdit()
{
        EstablecerToolTipIconos();
        $(".lnkEdit").click(function () {
            EjecutarAjax(urlBase + "Atracciones/GetById", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar Atracciones", url: urlBase + "Atracciones/Update", metod: "GET", func: "successUpdateAttraction", modalLarge: true });
        });
        $(".lnkDisable").click(function () {
            MostrarConfirm("Importante", "¿Está seguro que desea inactivar este registro?", "InactivarAtraccion", $(this).data("id"));
            //if (confirm("Está seguro que desea eliminar esta atracción?"))
            //    EjecutarAjax(urlBase + "Atracciones/Delete", "GET", { id: $(this).data("id") }, "successDeleteAttraction", null);
        });

       
    }
    
    function InactivarAtraccion(id) {        
        EjecutarAjax(urlBase + "Atracciones/Delete", "GET", { id: id }, "successDeleteAttraction", null);
    }

    function successInsertAttraction(rta)
    {
        if (rta.Correcto)
        {
            EjecutarAjax(urlBase + "Atracciones/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
            cerrarModal("modalCRUD");
            mostrarAlerta("El punto fue creado con éxito.");
        }
    }
    function successDeleteAttraction(rta) {
        if (rta.Correcto) {
            EjecutarAjax(urlBase + "Atracciones/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
            mostrarAlerta("Su operación fue exitosa.");
        }
    }
    function successUpdateAttraction(rta)
    {
        if (rta.Correcto)
        {
            EjecutarAjax(urlBase + "Atracciones/GetList", "GET", null, "printPartial", { div: "#listView", func: "setEventEdit" });
            cerrarModal("modalCRUD");
            mostrarAlerta("Su operación fue exitosa.")
        }
    }

    function MostrarMotivoCierreEdicion() {

        var IdEstado = $("#EstadoId").val();

        $("#DescripcionCierre").parent().parent().hide();
        $("#DescripcionCierre").removeClass("required");
        if (IdEstado == "4") {
            $("#DescripcionCierre").parent().parent().show();
            $("#DescripcionCierre").addClass("required");
        }

    }
