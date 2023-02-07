//iniciarProceso();

//finalizarProceso();
var ubicacion = location.href
$("document").ready(() => {
    
    var numDatos = 0;
    var cambio = 0;

    var numId = $("#noEditable").length;
    console.log(numId)

    $("#ddlEmpleado").select2({
        placeholder: "* Seleccione cliente"
    })

    var Clientes = "";
    var pintarHtml = "<div></div>"
    $("#limpiar").click(e => {
        location.reload();
        $("#bodyCliente").html('')
        Clientes = ''
        pintarHtml = ''
    })

    /*$("#ddlEmpleado").click(e => {
    if (cambio == 0) {*/
        $("#ddlEmpleado").change(e => {
            //$("#ddlEmpleado").click(e => {
            $("#tablaCliente").css("display", "block")
            //EjecutarAjax(urlBase + "CrearUsuarioExterno/verClientes", "GET", { cliente: "dato" }, "CambioclienteSuccess", null);
            var datoGeneral = $("#ddlEmpleado").val()
            CambioclienteSuccess(datoGeneral)
            //$("#ddlEmpleado").val(0)
            $("#ddlEmpleado").select2({
                placeholder: "* Seleccione cliente"
            })
        })
    /*}
    })*/

    function CambioclienteSuccess(dato) {
        var documento = dato.split("-")[0]
        //pintarHtml += `<div class='box' id=${numDatos} onclick=captura(${numDatos},${documento})>` + `${dato}` + "</div>"
        //pintarHtml += `<div class='box' onclick=captura(${numDatos},${documento})>` + `${dato}` + `<div id='inp_${numDatos}'></div>` + "</div>"


        //pintarHtml += `<div id='ctr_${numDatos}' class='box'>` + `${dato}` + `<div id='inp_${numDatos}'></div>` + "</div>"

        //
        var documentoCliente = dato.split("-")
        console.log(documentoCliente[0])

        //Clientes += `${dato}` + "|"


        console.log(pintarHtml)
        console.log(dato)
        //pintaHtml(pintarHtml)

        var docu = documentoCliente[0]
        $.ajax({
            url: `${ubicacion}/verPedidoDocumentoCliente`,
            method: "POST",
            data: { docCliente: documentoCliente[0] },
            success: (data) => {
                console.log(data)
                var dataPedido = data.pedidosCliente
                if (!isNaN(dataPedido)) {
                    pintarHtml += `<div id='ctr_${numDatos}'>` + `${dato}` + `<div id='inp_${numDatos}'></div>` + "</div>"
                    pintaHtml(pintarHtml)
                    Clientes += `${dato}` + "|"
                    MostrarMensaje("Importante", "Agrego el cliente exitosamente", "success")
                    /*var datPinta = `<div class='box' onclick='GuardarPedidosAsignados(${docu}, ${dataPedido})'>${dataPedido}</div>`*/
                    //var datPinta = `<div class='box' onclick='GuardarPedidosAsignados(${docu}, ${dataPedido})'>Subir logo empresa</div>`
                    //var datPinta = `<input class='box' id='logo_${numDatos}' type="file" /> <button style="display:inline" onclick='GuardarPedidosAsignados(${docu}, ${dataPedido},${numDatos},${numDatos})'>Subir logo</button><img height="60" width="60" alt='logoEmpresa' id='Vlogo_${numDatos}'>`
                    //var datPinta = `<input class='box' id='logo_${numDatos}' type="file" /> <input type="button" style="display:inline" 'button_${numDatos}' onclick='GuardarPedidosAsignados(${docu}, ${dataPedido},${numDatos},${numDatos})' value="Subir logo"/><img height="60" width="60" alt='logoEmpresa' id='Vlogo_${numDatos}'>`
                    var datPinta = `<input class='box' id='logo_${numDatos}' type="file" /> <button type="button" style="display:inline" id='button_${numDatos}' onclick='GuardarPedidosAsignados(${docu}, ${dataPedido},${numDatos},${numDatos})'>Subir logo</button><img height="60" width="60" alt='logoEmpresa' id='Vlogo_${numDatos}'>`
                    console.log(`#logo_${numDatos}`)
                    console.log($(`#logo_${numDatos}`).val())

                } else {
                    //var datPinta = `<div class='box'>${dataPedido}</div>`
                    ///$(`#ctr_${numDatos}`).remove()
                    //$("#bodyCliente"). en caso de que no funcione directamente el id
                    numDatos = numDatos - 1
                    //Clientes = $("#bodyCliente").html()
                    //pintarHtml = ''
                    datPinta = ''
                    MostrarMensaje("Importante",dataPedido,"error")  
                }
                //var datPinta = `<div>${data.pedidosCliente}</div>`

                /*$(`#inp_${numDatos}`).append(datPinta)*/
                $(`#inp_${numDatos}`).append(datPinta)
                numDatos = numDatos + 1
            },
            error: (err) => {
                console.log("error")
            }
        })
        //numDatos = numDatos + 1
    }

    /*$(`#logo_${numDatos}`).change(e => {
    
    })*/
   
   


    function pintaHtml(pintarHtmll) {
        //$("#bodyCliente").html("<tr>" + `${pintarHtml}` + "</tr>")
       // $("#bodyCliente").html(`${pintarHtmll}`)
        $("#bodyCliente").html(`${pintarHtmll}`)
    }

    $("#crearUsuario").click(e => {

        iniciarProceso()
        if ($("#nombre").val().length > 0 && $("#usuario").val().length > 0 && $("#Apellido").val().length > 0 && $("#correo").val().length > 0 && $("#correo").val().includes("@")) {
            console.log($("#usuario").val())
            console.log($("#nombre").val())
            var r = $("#usuario").val()
            console.log($("#IdEmpleado").val())

            /*EjecutarAjax(`${ubicacion}/Insert`, "GET", {
                Nombres: $("#nombre").val(),
                usuario: $("#usuario").val(),
                apellido: $("#Apellido").val(),
                //empresa: $("#empresa").val(),
                cliente: Clientes,
                correo: $("#correo").val()
            }, "cargarTabla", null);*/

            $.ajax({
                url: `${ubicacion}/Insert`,
                method: 'POST',
                data: {
                    Nombres: $("#nombre").val(),
                    usuario: $("#usuario").val(),
                    apellido: $("#Apellido").val(),
                    //empresa: $("#empresa").val(),
                    cliente: Clientes,
                    correo: $("#correo").val()
                },
                success: (e) => {
                    console.log(e)
                    cargarTabla(e)
                },
                error: (e) => {
                    console.log("Error")
                    MostrarMensaje("Importante", "Algo genero un erro, no proceso la tarea", "error")
                    location.reload
                }

            })


            console.log($("#usuario").val())
            /*$("#bodyCliente").html('')
            $("#nombre").val('')
            $("#usuario").val('')
            $("#Apellido").val('')
            $("#correo").val('')
            $("#ddlEmpleado").val(0)
            Clientes = ''
            pintarHtml = ''*/
           // location.reload();
        } else {
            if (!$("#correo").val().includes("@")) {
                $("#correo").css("border-color","red")
            }
            else {
                $("#correo").css("border-color", "green")
            }
            if ($("#nombre").val().length < 1) {
                $("#nombre").css("border-color", "red")
            } else {
                $("#nombre").css("border-color", "green")
            }
            if ($("#usuario").val().length < 1) {
                $("#usuario").css("border-color", "red")
            } else {
                $("#usuario").css("border-color", "green")
            }
            if ($("#Apellido").val().length < 1) {
                $("#Apellido").css("border-color", "red")
            } else {
                $("#Apellido").css("border-color", "green")
            }
            MostrarMensaje("Importante","Verifique que los campos esten bien diligenciados","error")
        }
        finalizarProceso();
    })

})

function cargarTabla(dato) {
    console.log(dato)
    if (dato.Correcto) {
        $("#bodyCliente").html('')
        $("#nombre").val('')
        $("#usuario").val('')
        $("#Apellido").val('')
        $("#correo").val('')
        $("#ddlEmpleado").val(0)
        Clientes = ''
        pintarHtml = ''
        MostrarMensaje("Importante", dato.Mensaje, "success")
        location.reload();
    } else {
        /*$("#bodyCliente").html('')
        $("#nombre").val('')
        $("#usuario").val('')
        $("#Apellido").val('')
        $("#correo").val('')
        $("#ddlEmpleado").val(0)
        Clientes = ''
        pintarHtml = ''*/
        MostrarMensaje("Importante", dato.Mensaje, "error")
    }
    
}

/*function cargarTablaa(dato) {
    console.log(dato)
}*/

function captura(dato, cliente){
    console.log(dato, cliente)
}

function GuardarPedidosAsignados(idCliente, codPedido, idPintar, idLogo)
{
    const clickBoton = document.getElementById(`button_${idLogo}`);
    clickBoton.preventdefault;
    const logoSub = document.getElementById(`logo_${idLogo}`)
    console.log(`logo_${idLogo}`)

    console.log("Imprimio logo")

    console.log(idCliente, codPedido, idPintar, idLogo)
    var logo = document.querySelector(`#logo_${idLogo}`)
    var verImagen = document.getElementById(`Vlogo_${idPintar}`)
    var ver = document.querySelector(`#crearUsuario`)

    var arLogo = logo.files
    
    if (!arLogo || !arLogo.length) {
        verImagen.src = ""
        return 
    }
    console.log(arLogo)

    var rutaImg = verImagen.ImageUrl
    console.log(rutaImg)

    priArchivo = arLogo[0]

    var urlArchivo = URL.createObjectURL(priArchivo)

    verImagen.src = urlArchivo

    var archivoLogo = logo.files
    console.log(archivoLogo)
    console.log(priArchivo)
    console.log(logo)
    console.log(urlArchivo)


    var reader = new FileReader();
    var fileData = new FormData();
    for (var i = 0; i < arLogo.length; i++)
    {
        fileData.append(arLogo[i].name, arLogo[i])
    }
    fileData.append("idCliente", idCliente);
    fileData.append("codPedido", codPedido);
    console.log(fileData)

    $.ajax({
        url: `${ubicacion}/guardaImg`,
        method: 'POST',
        //data: { url: urlArchivo, Cliente : idCliente},
        data: fileData,
        //data: url,
        contentType: false,
        processData: false,
        success: (e) => {
            clickBoton.style.display = "none"
            logoSub.style.display = "none"
            console.log("dato")
        },
        error: (e) => {
            console.log("error")
        }
    })


}

