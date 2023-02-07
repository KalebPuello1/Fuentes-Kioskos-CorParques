
$(function () {
    $("#lnkAdd").click(function () {
        EjecutarAjax(urlBase + "ProductosApp/GetPartial", "GET", null, "printPartialModal", { title: "Crear Producto", url: urlBase + "ProductosApp/Insert", metod: "GET", func: "successInsertUser", modalLarge: true });
    });
    setEventEdit();
});







function setEventEdit() {
    EstablecerToolTipIconos();
    $(".lnkEdit").click(function () {
        EjecutarAjax(urlBase + "ProductosApp/GetById", "GET", { id: $(this).data("id") }, "printPartialModal", {
            title: "Crear Producto", url: urlBase + "ProductosApp/Crear", metod: "GET", func: "successUpdate", modalLarge: true
        });
    });
    $(".lnkDisable").click(function () {
        MostrarConfirm("Importante", "¿Está seguro que desea desactivar este producto?", "Inactivar", $(this).data("id"));
        
    });
    $(".lnkhabilitar").click(function () {
        MostrarConfirm("Importante", "¿Está seguro de habilitar este producto?", "habilitar", $(this).data("id"));
       
    });  
}

function Inactivar(Id) {
    EjecutarAjax(urlBase + "ProductosApp/CambioEstado", "GET", { IdProducto: Id, activo:0 }, "successInactivar", null);
}

function successInactivar(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "ProductosApp/GetList", "GET", null, "printPartial", {
            div: "#listView", func: "setEventEdit"
        });
        mostrarAlerta("El producto fue inactivado con éxito.");
    } else {
        mostrarAlerta("Se ha presentado un error.")
    }

}

function habilitar(Id) {
    EjecutarAjax(urlBase + "ProductosApp/CambioEstado", "GET", { IdProducto: Id, activo: 1 }, "successhabilitar", null);
}

function successhabilitar(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "ProductosApp/GetList", "GET", null, "printPartial", {
            div: "#listView", func: "setEventEdit"
        });
        mostrarAlerta("El producto fue habilitado con éxito.");
    }
    else {
        mostrarAlerta("Se ha presentado un error.")
    }
}



function successUpdate(rta) {
    if (rta.Correcto) {
        EjecutarAjax(urlBase + "ProductosApp/GetList", "GET", null, "printPartial", {
            div: "#listView", func: "setEventEdit"
        });
        cerrarModal("modalCRUD");
        mostrarAlerta("El producto fue creado con éxito.")
    }
    else {
        cerrarModal("modalCRUD");
        mostrarAlerta("El producto ya existe.");
    }
}