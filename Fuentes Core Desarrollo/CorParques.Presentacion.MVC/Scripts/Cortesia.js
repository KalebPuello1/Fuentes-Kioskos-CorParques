//Registrar usuarios visitantes


$(function () {
   
    $(".validarnumero").keydown(function (event) {
        //alert(event.keyCode);
        if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && event.keyCode !== 190 && event.keyCode !== 110 && event.keyCode !== 8 && event.keyCode !== 9) {
            return false;
        }
        return true;
    });



    $('#datatable-responsive11').DataTable({
        "paging": false,
        "ordering": true,
        "scrollCollapse": false,
        "searching": true
    });
    $('#datatable-responsive11').on('draw.dt', function () {
        setEventEdit()
    });


    Dropzone.autoDiscover = false;    
    $("#dropzoneElements").dropzone({
        url: urlBase + "Cortesia/GuardarArchivoTemporal",
        maxFilesize: 2,
        uploadMultiple: false,
        addRemoveLinks: true,
        dictRemoveFile: "X",
        renameFilename: function (file) {
            nombreArchivo = new Date().getTime() + "_" + file;
            return nombreArchivo;
        },
        acceptedFiles: ".pdf,.jpeg,.jpg,.png",
        removedfile: function (file) {
            EjecutarAjax(urlBase + "Cortesia/RemoverArchivoTemporal", "GET", { name: nombreArchivo }, "voidFunction", null);
            file.previewTemplate.remove();
            $("#Imagen").val("");
            nombreArchivo = "";
        },
        success: function () {
            $("#dropzoneElements").removeAttr("style");
            $("#Imagen").val(nombreArchivo);
            $("#Archivo").val(nombreArchivo);
        }
    });
});

function validarnumeros(evt) {

    // code is the decimal ASCII representation of the pressed key.
    var code = (evt.which) ? evt.which : evt.keyCode;

    if (code == 8) { // backspace.
        return true;
    } else if (code >= 48 && code <= 57) { // is a number.
        return true;
    } else { // other keys.
        return false;
    }
}

function Cancelar() {
    var nombre = $("#Imagen").val();
    if (nombre.length > 0)
        EjecutarAjax(urlBase + "Cortesia/RemoverArchivoTemporal", "GET", { name: nombre }, "voidFunction", null);
    window.location = urlBase + "Cortesia/Index";
}

function SuccessUsuarioArchivo(rta,tipocortesia) {
    if (rta === undefined) {
    }
    else
    {
        if (!rta.Correcto) {
            Dropzone.forElement("#dropzoneElements").removeAllFiles(true);
            MostrarMensaje("Importante", rta.Mensaje, "error");
        }
        else {
            EjecutarAjax(urlBase + "Cortesia/ObtenerListaUsuarioVisitante", "GET", { Idtipocortesia: tipocortesia }, "printPartial", { func: "BotonCancelarUsuario" });
            cerrarModal("modalCRUD");
            if (rta.Mensaje !== undefined && rta.Mensaje !== null && rta.Mensaje !== "") {
                MostrarMensaje("Importante", rta.Mensaje, "warning");
                mostrarAlerta("Se agrego la cortesia con éxito.");
              
            }
            else {
                mostrarAlerta("Se agrego la cortesia con éxito.");
            }
           
        }
    }
}



$(".lnkAprobarCortesia").click(function () {
    var IdCortesia = $(this).data("id");
    MostrarConfirm("Importante", "¿Está seguro que desea autorizar esta Cortesía?", "ConfirmarAutoricacionCortesia", IdCortesia);
});

function ConfirmarAutoricacionCortesia(IdCortesia) {
    EjecutarAjax(urlBase + "Cortesia/AprobacionCortesia", "GET", { IdCortesia: IdCortesia }, "RespuestaAprobacionCortesia", null);
}
function RespuestaAprobacionCortesia(data) {
    if (isNaN(data)) {
        $("#listView").html(data);
        
    }
}
function BotonCrearUsuario(IDBandera) {
    var titulo;

    if (IDBandera == 1) {
        titulo = "Adicionar Cortesia Visitante";
    }
    else {
        titulo = "Adicionar Cortesia Ejecutivo";
    }
        cerrarModal("modalCRUD");
    EjecutarAjax(urlBase + "Cortesia/CrearUsuarioModal", "GET", { IdTipoCortesia: IDBandera }, "printPartialModal", {
        title: titulo, url: null,
        metod: "GET", func: null, modalLarge: true, hidesave: true
    })
        cerrarModal("modalCRUD");
}

function SumarTotalCortesias(num) {
    var sumatoria = 0;
    $('.inputNumCortesias').each(function () {
        var cant = $(this).val();
        if (cant.length > 0 && cant.trim()) {
            sumatoria = sumatoria + parseInt(cant);
        }

    });
    //var valr = $("#Cantidad").val();
    //var total = parseInt(valr) + parseInt(num);  
    $("#Cantidad").val(sumatoria);
}

function BotonCancelarUsuario() {
    cerrarModal("modalCRUD");
}

function BotonGuardarUsuario() {
   

    if (validarFormulario("modalCRUD .modal-body")) {

        var NumCortesias = $("#Cantidad").val();
        if (NumCortesias == 0) {
            return MostrarMensaje("Importante", "Debe agregar mínimo un producto para la cortesía", "warning");
        }
        iniciarProceso();

        
        var listProductosAgregar = [];

        $('.inputNumCortesias').each(function () {
            var cant = $(this).val();
            if (cant.length > 0 && cant.trim()) {
                var myObj = {};
                myObj.Cantidad = $(this).val();
                myObj.CodigoSap = $(this).attr("id");
                listProductosAgregar.push(myObj);
            }
           
        });

        var formData = ObtenerObjeto("modalCRUD .modal-body form");

        $.ajaxSetup({ cache: false });
        var data = formData;
        var myObj2 = {};
        myObj2.Archivo = formData[0].value;
        myObj2.IdTipoCortesia = formData[1].value;
        myObj2.TipoDocumento = formData[2].value;
        myObj2.NumeroDocumento = formData[3].value;
        myObj2.Nombres = formData[4].value;
        myObj2.Apellidos = formData[5].value;
        myObj2.Correo = formData[6].value;       
        myObj2.Telefono = formData[7].value;
        myObj2.NumeroTicket = formData[8].value;
        myObj2.Cantidad = formData[9].value;
        myObj2.IdPlazo = formData[10].value;
        myObj2.IdComplejidad = formData[11].value;
        myObj2.IdRedencion = formData[12].value;
        myObj2.Descripcion = formData[14].value;

        $.ajax({
            ContentType: 'application/json; charset-uft8',
            dataType: 'JSON',
            url: urlBase + 'Cortesia/InsertarUsuarioVisitante',
            type: 'POST',
            data: {
                usuario: myObj2,
                listaProductosAgregar: listProductosAgregar
            },
            success: function (answer) {
                finalizarProceso();
                SuccessUsuarioArchivo(answer, myObj2.IdTipoCortesia);
              
            },
            error: function (jqXHR, exception) {
                MostrarMensaje("Importante", "Intente nuevamente ó contáctese con el Administrador", "error");
                finalizarProceso();
            }
        });

      


    }
}


 
