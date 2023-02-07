
iniciarProceso();
$("document").ready(() => {
    $(".noEditable").attr("disabled", true)
    var stringruta = "";
    var regis = document.getElementById("reg");
    var datoGlobalDescarga = "";
    var descargar = document.getElementById("datar");
    var chek = 0;
    var datCheck;
    var width = window.width;
    var height = window.height;
    var datosPintarFocus;
    var global = "";

    function iniciarProceso() {
        $(".loader-wrapper").css("display", "block");
        $("#div_message_error").hide();
    }

    function finalizarProceso() {
        $(".loader-wrapper").css("display", "none");
    }

    

    
/*    var ft = $('#example').DataTable({
        bFilter: false,// bInfo: false,
        "language": {
            "search" : "Buscar:",
            "paginate": {
                "previous": "Anterior",
                "next": "Siguiente",
            }
        },
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "Todos"]],
        "pageLength": 10,
        //"dom": 'Bfrtip',

    });
*/
    finalizarProceso();

   /* ft.rows({ search: 'applied' }).data().each(function (value, index) {
       arrD.push(value[1])
   });
    */
    //regis.style.display = "none";
    //descargar.style.display = "none";
    var ruta = location.href;
    console.log(`${ruta}/cree`)
//    $("#hide").hide();
    
    //0 -> es 0
    //1 -> es null

    $("#chk").click(e => {
        arrD = []
        console.log(" ** Redencion ** ")
        var checkk = document.getElementById("chk");
        var fcheck = document.getElementById("ch");
        if (chek === 0) {
            chek = 1;
            fcheck.checked = 1;
            console.log(" ** Redencion * " + 0)
            //checkk.classList.toggle("check")
        } else {
            chek = 0;
            fcheck.checked = 0;
            console.log(" ** Redencion * " + 1)
            //checkk.classList.toggle("check")
        }
        console.log(arrD);
        console.log(arrD.length);
    });
    var eleA = document.querySelector("a")

    eleA.addEventListener("click", (e) => {
        e.preventDefault()
        console.log("sirve")
    })

    $("a").map(edit)


//begin data 



  function edit(e) {
     
        $("this").attr("data-id")
    }
    function reden(e) {
        e.preventDefault();
        console.log("this is ff")
    }
    +
    $("a #editar").click((e) => {
        e.preventDefault()
        console.log($("this").attr("data-id"))
        console.log("This is test of editar")
    })


//end data 


    var ff = 0;


    $("#btnn").click(e => {
        console.log(`${ruta}/imgG`)
        console.log($("#imgCliente").val())
        stringruta = $("#imgCliente").val()
        $.ajax({
            url: `${ruta }/img`,
            method: 'GET',
            data: { img: stringruta},
            success: (e) => {
                console.log(e)
            },
            error: (e) => {
                console.log(e)
            }
        })
    })

    $("#busc").click((e) => {
        e.preventDefault();
        var fact = document.getElementById("factura");
        console.log(fact.value.length)
        if (fact.value.length >= 10) {
            var checkk = document.getElementById("chk");
            arrD = []
            ff = 1;
            console.log(fact.value)
            console.log(e.value)

            EjecutarAjax(urlBase + "CodigoFechaAbiertaExternos/BuscarCodigos", "GET", { daT: $("#factura").val(), check: chek }, "printPartiall", { div: "#listView", func: "setEventEdit" });
            //EjecutarAjax(urlBase + "CodigoFechaAbierta/BuscarCodigos", "GET", { daT: $("#factura").val(), check: chek }, "sussesfully", null);
            if (fact.value.length > 0/*&& arrD.length > 0*/) {
                regis.style.display = "block";
            }
            fact.value = "";
            console.log(arrD);
            console.log(arrD.length);
            checkk.checked = 0;
            chek = 0;
            console.log("sended.... 1");

            $(".inputt").prop(disabled, true)

        } else {
            alert("Por favor ingrese los 10 digitos de la factura ");
        }
    });


    $("#reg").click((e) => {
        e.preventDefault();
        iniciarProceso();
        ff = 0;
        console.log(arrD)
        $("#hide").show();
        console.log("Vieww correo.....");
        console.log(`${ruta}/exportarCod`);

        $.ajax({

            url: `${ruta}/exportarCod`,
            method: 'POST',
            data: { l: "lol"},
            success: (ee) => {
                /*console.log(ee)
                datoGlobalDescarga = ee;*/
                //descargar.href = `/${datoGlobalDescarga}`
                //descargar.style.display = "block"
                /*//console.log(datoGlobalDescarga)
                window.location = urlBase + 'CodigoFechaAbierta/Download?Data=' + ee*/
                console.log($("#put").find("tr").val())

                $("tr").find("td").each(function (i, td) {
                    //console.log(td[i])
                    console.log(td)
                    if ($(this).find("input").val() != undefined && $(this).find("input").val() != "") {
                        console.log(" ---> " + $(this).find("input").val())
                        $(this).find("input").val("")

                    }

                    $("#example").serializeArray();
                    console.log($("#example").serializeArray())
                    console.log($("tr").find("td").serializeArray())
                    //console.log(td.html())
                    $(this).find("td").html()
                    console.log(td.innerHTML)
                    console.log($("tr").text())
                    console.log($(td).text())
                    console.log($(td).children().first().val())
                    console.log($(td).children().val())
                    console.log($(td).val())
                    var r = $("tr").text()
                    console.log(r.trim())
                    console.log($("tr").find("td").children().first().val())
                })
                $("tr").find("td").children().first().val()
                console.log($("tr").find("td").children().first().val())

                

                finalizarProceso();
            },
            error: (e) => {
                console.log(e)
            }
        })

        /*var f = $("#formEnv").serialize()

        console.log($("#formEnv").serialize())
        $("#formEnv").serializeArray()
        console.log($("#formEnv").serializeArray())*/
        //$("#formEnv").submit()

       
        
        //regis.style.display = "none";
    });

    $("#datar").click(e => {
        console.log("Eliminando....")
        setTimeout(() => {
            $.ajax({
                url: `${ruta}/Eliminar`,
                method: 'POST',
                //data: { l: "lol" },
                success: (ee) => {
                    console.log("fff")
                    console.log(ee)
                },
                error: (e) => {
                    console.log(e)
                }
            })
        },200)
        descargar.style.display = "none";
        regis.style.display = "block";
        console.log("Eliminado")
    })

    $("#activa").click(e => {
        $.ajax({
            url: `${ruta}/enviarMail`,
            method: 'GET',
            //data: { id: "248281" },
            success: (ee) => {
                console.log(ee)
            },
            error: (e) => {
                console.log(e)
            }
        })
    })

    $("#save").click((e) => {
        e.preventDefault();
        console.log("saved data....");
        var dat = $("#MeCorr").val();
        console.log(dat);
        $("#MeCorr").val("");
    });

    $("#corr").click((e) => {
        e.preventDefault();
        $("#hide").hide();
        console.log("sended ah correo.... ");
    });
    arrD = []
    /*ft.rows({ search: 'applied' }).data().each(function (value, index) {
        arrD.push(value[1])
    });*/
});

function sussesfully(rta) {

    if (!rta.Correcto)
    {
        MostrarMensaje("Fecha abierta ", "No existen datos ", "error")
    }
   
}

function setEventEdit() {
    EstablecerToolTipIconos();
    var rdato = 0;
    $(".lnkEdit").click(function () {
        //successUpdateDestrezas
        rdato = $(this).data("id")
        EjecutarAjax(urlBase + "CodigoFechaAbierta/Obtener", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Dato Codigo Fecha Abierta", url: urlBase + "CodigoFechaAbierta/editar", metod: "PUT", func: "successUpdateDestrezas", modalLarge: true });
        //EjecutarAjax(urlBase + "CodigoFechaAbierta/Obtener", "GET", { id: $(this).data("id") }, "printPartialModal", { title: "Editar punto CodigoFechaAbierta", url: urlBase + "CodigoFechaAbierta/editar", metod: "PUT", func: "successUpdateDestrezas", modalLarge: true });
        //EjecutarAjax(urlBase + "CodigoFechaAbierta/Obtener", "GET", { id: $(this).data("id") }, "printPartialModal", { div: "#listView", func: "setEventEdit" });
    });

    $(".lnkDisable").click(function () {
      /*  MostrarConfirm("Importante", "¿Está seguro que desea inactivar este registro?", "EliminarDestreza", $(this).data("id"));
        //if (confirm("Está seguro que desea eliminar esta destreza?"))
        //    EjecutarAjax(urlBase + "Destrezas/Delete", "GET", { id: $(this).data("id") }, "successDeleteDestrezas", null);
      */
        var dato = confirm("Esta seguro de enviar este codigo QR?")
        //EjecutarAjax(urlBase + "CodigoFechaAbierta/EnviarCorreo", "GET", { id: $(this).data("id"), lol: "Successfully" }, "printPartialModal", { title: "Enviar Codigo Fecha Abierta", url: urlBase + "CodigoFechaAbierta/editar", metod: "PUT", func: "successUpdateDestrezas", modalLarge: true });
        EjecutarAjax(urlBase + "CodigoFechaAbierta/EnviarCorreo", "GET", { id: $(this).data("id"), lol: "Successfully" }, "printPartialModall", { div: "#listView", func: "setEventEdit" });
        successUpdateDestrezas()
    });
}

function sendMail() {
   console.log("relolando")
}

function printPartialModall(data, obj) {
    $("#btnCancelGeneric").show();
    $("#btnSaveGeneric").show();
    $("#btnVolverGeneric").hide();
    $("#btnImprimirGeneric").hide();
    $("#btnSaveGeneric").data("url", obj.url);
    $("#btnSaveGeneric").data("metod", obj.metod);
    $("#btnSaveGeneric").data("function", obj.func);
    $("#btnCancelGeneric").click(function () {
        console.log("paso para cerrar la modal ")
        cerrarModal('modalCRUD');
    });
    $("#btnVolverGeneric").click(function () {
        cerrarModal('modalCRUD');
    });
    $("#btnSaveGeneric").click(function () {
        console.log("paso por guardar la modal ")
        $.ajax({
            url: urlBase + "CodigoFechaAbierta/EnviandoCorreo",
            method: 'GET',
            data: { 'qr': $(this).data("id")},
            success: (e) => {
                console.log(e)
            },
            error: (e) => {
                console.log(e)
            }
        })
        cerrarModal('modalCRUD');
    });
    $("#btnImprimirGeneric").click(function () {

        $("#div_print").show();
        var mode = "popup";
        var close = true;
        var extraCss = "";
        var print = "#div_print";
        var keepAttr = [];
        keepAttr.push("class");
        keepAttr.push("id");
        keepAttr.push("style");

        var headElements = '<meta charset="utf-8" />,<meta http-equiv="X-UA-Compatible" content="IE=edge"/>';
        var options = { mode: mode, popClose: close, extraCss: extraCss, retainAttr: keepAttr, extraHead: headElements };

        $(print).printArea(options);
        setTimeout(function () { $("#div_print").hide(); }, 200);

    });

    $("#modalCRUD .modal-dialog").removeClass("modal-lg");
    if (obj.modalLarge)
        $("#modalCRUD .modal-dialog").addClass("modal-lg");
    $("#modalCRUD .modal-title").html(obj.title);
    $("#modalCRUD .modal-body").html(data);
    if (obj.hidesave) {
        $("#btnCancelGeneric").hide();
        $("#btnSaveGeneric").hide();
    }
    if (obj.hideSaveGeneric) {
        $("#btnSaveGeneric").hide();
    }
    if (obj.showreturn) {
        $("#btnVolverGeneric").show();
    }
    if (obj.print) {
        $("#btnImprimirGeneric").show();
    }
    /*if (obj.Table) {
       // $("#datatable-responsive_1").DataTable();
        $("#modalCRUD .modal-dialog").css("width", 940);
    }*/

    if (obj.func2) {
        window[obj.func2](obj.param);
    }

    if (obj.DatePicker) {
        loadCalendar();
    }
    if (obj.TimePicker) {
        setTimePicker();
    }
    //}

    if (obj.hideCustom) {
        $("#btnSaveCustomizable").hide();
    }
    setNumeric();
    abrirModal("modalCRUD");
}

//Modificar
function successUpdateDestrezas(rta) {
    //if (rta.Correcto) {
 /*   if (rta.F) {
        EjecutarAjax(urlBase + "CodigoFechaAbierta/ObtenerTodo", "POST", null, "printPartial", { div: "#listView", func: "sendMail" });
        cerrarModal("modalCRUD");
        mostrarAlerta("Edición exitosa.");
    }*/
}

function verProductosPedido(dato) {
    global = dato
    /*$.ajax({
        url: urlBase + "CodigoFechaAbiertaExternos/getPedido",
        method: 'GET',
        data: { 'cod': dato},
        success: (e) => {
            console.log(e)
        },
        error: (e) => {
            console.log(e)
        }
    })*/
    EjecutarAjax(urlBase + "CodigoFechaAbiertaExternos/getPedido", "GET", { 'cod': dato }, "printPartiall", { div: "#listView", func: "setEventEdit" });
    //location.reload();
    console.log(dato)
}

//printPartial
//data: que va a pintar -- values: donde va pintar
function printPartiall(data, values) {
    //values.div: div.id -> values
    $(values.div).html(data);

    
    //$(values.div).find("table").DataTable();
    /*$(values.div).find("table").on('draw.dt', function () {
        //
        setEventEdit()
    });*/
    window[values.func]();
    $(".noEditable").attr("disabled", true)
}

//setEventEdit

function verGlobal() {
    return global
}
function enviar() {
    $("#envia").submit()
    console.log("Paso")
}

function enviaDato(e) {
    iniciarProceso();
}
