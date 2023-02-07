var ubicacion = location.href;
var malU = 0
var malD = 0
$("document").ready(() => {
    $('#datetimepickerIni').datetimepicker({ format: 'YYYY-MM-DD' });
    $('#datetimepickerfin').datetimepicker({ format: 'YYYY-MM-DD' });
    $("#area").select2({
        placeholder: "* Seleccione la unidad"
    });
    //Ubicacion del documento en la plataforma
    var ubicacion = location.href;
    var desc = document.getElementById("desc");
    var FInicial = document.getElementById("FInicial");
    var FFinal = document.getElementById("FFinal")
    var NDocumento = $("#NDocumento");
    var Area = $("#area")
    var exporta = document.getElementById("exportar")
    
    function number(numberdato) {
        let field = document.getElementById(numberdato)
        let valueInt = parseInt(field.value)
        if (!Number.isInteger(valueInt)) {
            return false
        } else {
            field.value = valueInt
            return true
        }
    }
    $("#exportar").click((e) => {
        e.preventDefault()
        if (FInicial.value.length != 0 && FFinal.value.length != 0) {
            console.log(FFinal.value + " -- " + FInicial.value + " --- " + NDocumento.val() + " -- " + Area.val())
            console.log(`${ubicacion}/Buscar`)
            EjecutarAjax(`${ubicacion}/Buscar`, "GET", {
                 FInicial: FInicial.value,
                 FFinal: FFinal.value,
                 NDocumento: NDocumento.val() == 0 ? "null" : NDocumento.val(), 
                Area: Area.val() == "Seleccionar empresa...." ? "null" : Area.val()
            }, "cargarTabla", null);

            } else {
            MostrarMensaje("Importante", "Hay inconsistencias en el formulario, revise los campos demarcados con color rojo.", "error");
            //errorValidate
            if (FInicial.value.length == 0 && malU != 1){
                $("#FInicial").toggleClass("errorValidate")
                malU = 1
            } else if (FInicial.value.length != 0 && malU != 0){
                $("#FInicial").toggleClass("errorValidate")
                malU = 0
            }
            
            if (FFinal.value.length == 0 && malD != 1) {
                $("#FFinal").toggleClass("errorValidate")
                malD = 1
            } else if (FFinal.value.length != 0 && malD != 0){
                $("#FFinal").toggleClass("errorValidate")
                malD = 0
            }
            
        }
       
        })
    
    $("#desc").click(e =>
    {
        var f = 1
        if (f == 1) {
            f = 0;
        }
        setTimeout(() => {
            $.ajax({
                url: `${ubicacion}/Descargar`,
                method: "GET",
                data: { f: f },
                success: (ee) => {
                    console.log("Downloading");
                    console.log(ee);
                    },
                error: () => {
                    console.log("Error downloading");
                }
            })
        },200)
        desc.style.display = "none"
        exporta.style.display = "block"
    })
})

function cargarTabla(datos) {
    if (datos.length > 0) {
        if (datos.indexOf("Error") >= 0) {
            MostrarMensaje("Importante", datos);
            if (malU == 1) {
                $("#FInicial").toggleClass("errorValidate")
                malU = 0
            }
            if (malD == 1) {
                $("#FFinal").toggleClass("errorValidate")
                malD = 0
            }
        }
        else {
            window.location = urlBase + 'ConsumoDeEmpleados/Download?Data=' + datos;
            MostrarMensaje("Importante", "La desacarga fue satisfactoria", "success")
            if (malU == 1) {
                $("#FInicial").toggleClass("errorValidate")
                malU = 0
            }
            if (malD == 1) {
                $("#FFinal").toggleClass("errorValidate")
                malD = 0
            }
            return true
        }
    }
    else {
        MostrarMensaje("Importante", "No hay información para exportar.");
    }
    
}


