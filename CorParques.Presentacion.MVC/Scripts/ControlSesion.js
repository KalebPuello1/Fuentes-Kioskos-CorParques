var sysdate1 = new Date();
var ticktack;
var Contador = 0;
var MinRefresh =parseInt(document.getElementsByTagName("BODY")[0].attributes["data-time"].value);
var minuto = sysdate1.getMinutes();

$(function () {
    $("body").change(function () {
        //ReiniciarControlSesion();
        EjecutarAjax(urlBase + "Cuenta/ActualizarCookie", "GET", null, "succesChangeCookie", null, true);
    });
    
});
function succesChangeCookie() {

}

function stop() {
    clearTimeout(ticktack);
}

function real_time() {
    var sysdate = new Date();
    var midate = " ";
    var h = sysdate.getHours();
    var m = sysdate.getMinutes();
    var s = sysdate.getSeconds();                    

    if (s <= 9) s = "0" + s;
    if (m <= 9) m = "0" + m;
    if (h <= 9) h = "0" + h;

    var day = sysdate.getDate();
    var month = sysdate.getMonth();
    var year = sysdate.getYear();
    if (year < 1900) year = 1900 + sysdate.getYear();
    month += 1;
    if (month < 10) month = '0' + month;
    if (day < 10) day = '0' + day;

    var hidden = $('#control').val();
    if (hidden != "" && hidden != undefined) {
        Contador = parseInt(hidden);
    }

    if (m != minuto)
    {        
        Contador = Contador + 1;
        minuto = m;        
        if (Contador > MinRefresh)
        {
            Contador = 0;
            $('#frmLogOut').submit();
            MostrarMensajeRedireccion("Importante", "Su sesíón a caducado, por favor inicie sesión nuevamente", "Cuenta/Login", "warning");
        }
        $('#control').val(Contador);
    }

    midate += day + "/" + month + "/" + year + " " + h + ":" + m + ":" + s;
    document.getElementById('date_complete').innerHTML = midate;
    ticktack = setTimeout("real_time()", 1000);
}
