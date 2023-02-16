var user = 0;
var userhistorico = "";
var inicializadointerval = false;
$(function () {
    //$('#txtUser').bind("cut copy paste", function (e) {
    //    e.preventDefault();
    //});
    $("#txtUser").focus();
    $(this).click(function () {
        $("#txtUser").focus();
    });
    $("#frmLogin").submit(function () {
        return validarFormulario("frmLogin");
    });
    $("#txtUser").keyup(function () {
        if (!inicializadointerval) {
            inicializadointerval = true;
            var refreshIntervalId = setInterval(function () { $("#frmLogin").submit(); inicializadointerval = false; clearInterval(refreshIntervalId); }, 300);
            
        }
        
    });

});
