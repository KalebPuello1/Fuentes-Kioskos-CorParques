
<reference path="jquery.signalR-2.2.2.min.js" />
$(function () {
   

    chat.client.verMensajePunto = function (mensajes) {
        var esPunto = false;
        var cantidad = 0;
        
        if (mensajes != null) {
            //se valida que hayan mensajes del punto al que se pertenece y la cantidad de mensajes
            $.each(mensajes, function (i, v) {
                
                if (v.IdPunto == puntoInfo.Id) {
                    esPunto = true;
                    if(v.IdEstado == estadoActivo){
                        cantidad++;
                    }
                    //break;
                }
            });

            //se valida si el punto existe, de lo contrario, no debe hacer nada
            if(!esPunto){
                return false;
            }
            var html = '<a href="javascript:;" class="dropdown-toggle info-number" data-toggle="dropdown" aria-expanded="false"><i class="fa fa-envelope-o"></i>';

            if (cantidad > 0) {
                html += '<span id="countpendingview" class="badge bg-green">'+cantidad+'</span>';
            }
            html += '</a>';
            html += '<ul id="menu1" class="dropdown-menu list-unstyled msg_list" role="menu">';

            //empieza a armar el cuerpo del html
            $.each(mensajes, function (i, v) {
                if(v.IdPunto == puntoInfo.Id)
                {
                    html += '<li class="' + (v.IdEstado==estadoActivo?'notificacionNew':'') + '" data-prioritario="' + v.Prioritario + '">';
                    html += '<a onclick="viewNotification(this)" data-titulo="'+v.Asunto+'" data-contenido="'+v.Contenido+'" data-id="'+v.Id+'">';
                    html += '<span><b>'+v.Asunto+'</b><span class="time">'+v.ListaGrupos+'</span></span>';
                    html += '<span class="message">'+v.Contenido.Length > 50 ? v.Contenido.substring(0, 50) + "..." : v.Contenido+'</span>';

                    html += '</a></li>';                    
                }
            });
            html += '</ul>';
            $("#menuNotificacion").html(html);
            
            validateNotificationPriority();
            //insertar html
        }
    }
    chat.client.clienteCerrarSesion = function (idusuario, punto) {
        if (punto.toString() == puntoInfo.Id.toString()) {
            if (idusuario.toString() == usuarioInfo.id.toString()) {
                MostrarMensaje("Importante", "Su sesión fue cerrada desde la administración del sistema. por favor inicie sesión nuevamente.", "warning", "cerrarSesion");
            }
        }
    };
    $.connection.hub.start({ transport: ['webSockets', 'serverSentEvents', 'longPolling'], jsonp: true }).done(
        function () {
            console.log('conexion Hub iniciada SignalR2');
        });
});


/*
public int Id { get; set; }
public string Asunto { get; set; }
public string Contenido { get; set; }
public int IdPunto { get; set; }
public int IdEstado { get; set; }
public int UsuarioCreacion { get; set; }
public DateTime FechaCreacion { get; set; }
public bool Prioritario { get; set; }
public int? UsuarioModificacion { get; set; }
public string ListaGrupos { get; set; } 
public DateTime? FechaModificacion { get; set; }
*/
