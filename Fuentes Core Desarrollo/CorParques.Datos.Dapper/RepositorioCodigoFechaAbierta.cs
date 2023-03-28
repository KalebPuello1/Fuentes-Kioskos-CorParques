
using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Contratos;
using CorParques.Transversales.Util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
//using System.Windows.Forms;

namespace CorParques.Datos.Dapper
{
    public class RepositorioCodigoFechaAbierta :/* RepositorioBase<Pedidos>, */ IRepositorioCodigoFechaAbierta
    {
        #region Propiedades 
        public string rol = "";
        private readonly SqlConnection _cnn;
        #endregion

        #region Constructor
        IEnvioMails EnviarQR;
        public RepositorioCodigoFechaAbierta(IEnvioMails Enviaqr)
        {
            EnviarQR = Enviaqr;
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        #endregion

        #region Metodos
        //CodigoFactura -> es bool
        public IEnumerable<CodigoFechaAbierta> CodigoFacturaa(int c)
        {
            IEnumerable<CodigoFechaAbierta> _lis = new List<CodigoFechaAbierta>();

            try
            {
                _lis = _cnn.Query<CodigoFechaAbierta>("SP_PedidosBoletaControl",
                    commandType: CommandType.StoredProcedure,
                    param: new
                    {
                        CodSapPedido = 100,
                        Redencion = true
                    });
            }
            catch
            {
                throw;
            }
            return _lis;
        }

        public bool Actualizar(CodigoFechaAbierta modelo, out string error)
        {
            try
            {
                error = _cnn.Query<string>("UPDATE TB_PedidosBoletaControl SET Redencion = " + modelo.Redencion.ToString() + "WHERE IdPedidoBoletaControl = " + modelo.IdPedidoBoletaControl.ToString()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error update/CodigoFechaAbierta", ex.Message);
            }
            return string.IsNullOrEmpty(error);
        }

        public IEnumerable<CodigoFechaAbierta> VerFacturas(string IdDestreza, int? IdAtraccion)
        {
            var _list = _cnn.Query<CodigoFechaAbierta>("SP_PedidosBoletaControl",
                param: new { CodSapPedido = IdDestreza, Redencion = IdAtraccion },
                        commandType: CommandType.StoredProcedure).ToList();

            return _list;
        }

        public CodigoFechaAbierta ObtenerPorId(int Id)
        {
            var dato = _cnn.Query<CodigoFechaAbierta>("SELECT * FROM TB_PedidosBoletaControl WHERE IdPedidoBoletaControl = " + Id).FirstOrDefault();
            return dato;
        }

        public IEnumerable<CodigoFechaAbierta> ObtenerTodos(string CodSap)
        {
            var _list = _cnn.Query<CodigoFechaAbierta>("SELECT * FROM TB_PedidosBoletaControl WHERE CodSapPedido = " + CodSap).ToList();
            return _list;
        }

        public string EnviarQRCorreo(string correo, string pathQR)
        {
            List<string> imgQR = new List<string>();
            imgQR.Add(pathQR);
            string nom = new FileInfo(pathQR).Name;
            //bool env = EnviarQR.EnviarCorreo(correo,$"CodigoQR {nom}","Gracias por ser parte de nuestra familia",System.Net.Mail.MailPriority.High,imgQR);
            bool env = EnviarCorreo(correo, $"CodigoQR {nom}", "Gracias por ser parte de nuestra familia", System.Net.Mail.MailPriority.High, imgQR);
            string envio = "";
            if (env)
            {
                envio = "envio exitoso";
            }
            else
            {
                envio = "envio no exitoso";
            }
            return envio;
        }
        public string EnviarCorreoCodConfirmacion(string correo, string CodConfirma)
        {
            //bool env = EnviarQR.EnviarCorreo(correo,$"CodigoQR {nom}","Gracias por ser parte de nuestra familia",System.Net.Mail.MailPriority.High,imgQR);
            bool env = EnviarCorreo(correo, $"Código confirmación ", $"Gracias por ser parte de nuestra familia, el codigo de confirmacion es {CodConfirma}", System.Net.Mail.MailPriority.High, null);
            string envio = "";
            if (env)
            {
                envio = "envio exitoso";
            }
            else
            {
                envio = "envio no exitoso";
            }
            return envio;
        }


        public IEnumerable<CodigoFechaAbierta> ObtenerListaPaginada(int pagina, int registrosPorPagina, string filtro, string columnaOrden)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CodigoFechaAbierta> ObtenerLista()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CodigoFechaAbierta> ObtenerLista(string filtro)
        {
            throw new NotImplementedException();
        }

        public CodigoFechaAbierta Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public int Insertar(ref CodigoFechaAbierta modelo)
        {
            throw new NotImplementedException();
        }

        public bool Actualizar(ref CodigoFechaAbierta modelo)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(CodigoFechaAbierta modelo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CodigoFechaAbierta> StoreProcedure(string nombre, object Parametros)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CodigoFechaAbierta> ExecuteQuery(string query, object Parametros)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CortesiaPunto> test(int IdDestreza, int IdAtraccion)
        {
            var lista = _cnn.Query<CortesiaPunto>("SP_ObtenerCortesiaPunto",
            null, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return lista;
        }

        #endregion

        #region test
        public bool EnviarCorreo(string sTo, string sSubject, string sMensaje, MailPriority mpPriority, List<string> attachmentList)
        {
            /*      if (true)
                  {
                      try
                      {
                          SmtpMail oMail = new SmtpMail("TryIt");

                          // Your Hotmail email address
                          oMail.From = "liveid@hotmail.com";
                          // Set recipient email address
                          oMail.To = "support@emailarchitect.net";

                          // Set email subject
                          oMail.Subject = "test email from hotmail account";
                          // Set email body
                          oMail.TextBody = "this is a test email sent from c# project with hotmail.";

                          // Hotmail SMTP server address
                          SmtpServer oServer = new SmtpServer("smtp.office365.com");

                          // Hotmail user authentication should use your
                          // email address as the user name.
                          oServer.User = "liveid@hotmail.com";

                          // If you got authentication error, try to create an app password instead of your user password.
                          // https://support.microsoft.com/en-us/account-billing/using-app-passwords-with-apps-that-don-t-support-two-step-verification-5896ed9b-4263-e681-128a-a6f2979a7944
                          oServer.Password = "your password or app password";

                          // Set 587 port, if you want to use 25 port, please change 587 to 25
                          oServer.Port = 587;

                          // detect SSL/TLS connection automatically
                          oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

                          Console.WriteLine("start to send email over SSL...");

                          SmtpClient oSmtp = new SmtpClient();
                          oSmtp.SendMail(oServer, oMail);

                          Console.WriteLine("email was sent successfully!");
                      }
                      catch (Exception ep)
                      {
                          Console.WriteLine("failed to send email with the following error:");
                          Console.WriteLine(ep.Message);
                      }
                  }*/
            /*
                        if (true)
                        {
                            var MailFromm = ConfigurationManager.AppSettings["MailFrom"].ToString();
                            var fromAddress = new MailAddress((string)MailFromm, ConfigurationManager.AppSettings["Mask"].ToString());
                            var toAddress = new MailAddress("itrujillo@corparques.co", "To me");
                            var fromPassword = ConfigurationManager.AppSettings["MailPass"].ToString();
                            const string subject = "test";
                            const string body = "Hey now!!";
                            //SmtpServer oServer = new SmtpServer("smtp.office365.com");

                            var smtp = new SmtpClient
                            {
                                //Host = "smtp.gmail.com",
                                Host = "smtp.office365.com",
                                Port = 25,
                                UseDefaultCredentials = false,
                                EnableSsl = true,
                                DeliveryMethod = SmtpDeliveryMethod.Network,
                                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                                Timeout = 700000,
                                TargetName = "STARTTLS/smtp.office365.com"
                            };
                            smtp.EnableSsl = true;
                            using (var message = new MailMessage(fromAddress, toAddress)
                            {
                                Subject = subject,
                                Body = body
                            })
                            {
                                try
                                {
                                    smtp.SendAsyncCancel();
                                    smtp.Send(message);
                                    smtp.SendMailAsync(message);
                                    smtp.SendAsyncCancel();
                                    smtp.Send(message);
                                }
                                catch (IOException)
                                {
                                    smtp.Send(message);
                                    smtp.SendMailAsync(message);
                                    smtp.SendAsyncCancel();
                                    smtp.Send(message);
                                }
                            }
                        } */

            var ms = new MemoryStream();

            var mail = new MailMessage();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                      | SecurityProtocolType.Tls11
                                      | SecurityProtocolType.Tls12;

            var smtpServer = new SmtpClient(ConfigurationManager.AppSettings["Smtp"].ToString()); ;
            var MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
            var MailPass = ConfigurationManager.AppSettings["MailPass"].ToString();
            var Port = ConfigurationManager.AppSettings["Port"].ToString();
            mail.From = new MailAddress((string)MailFrom, ConfigurationManager.AppSettings["Mask"].ToString());
            mail.To.Add(sTo);
            mail.Subject = sSubject;
            string tbody = sMensaje;
            mail.Body = tbody;
            mail.IsBodyHtml = true;
            mail.Priority = mpPriority;

            if (attachmentList != null)
            {
                foreach (var cln in attachmentList)
                {
                    ms = new MemoryStream(File.ReadAllBytes(cln));

                    try
                    {
                        mail.Attachments.Add(new Attachment(ms, new FileInfo(cln).Name));
                    }
                    catch (Exception) { }
                }
            }
            smtpServer.Port = int.Parse(Port);
            smtpServer.EnableSsl = true;
            smtpServer.Credentials = new System.Net.NetworkCredential(MailFrom, MailPass);

            try
            {
                //smtpServer.

                //System.Text.Encoding e = System.Text.Encoding.Convert("dfdfd");
                //Convert
                mail.BodyEncoding = System.Text.Encoding.ASCII;
                //smtpServer.Send(mail);

                smtpServer.Send(mail);
                smtpServer.Dispose();
                ms.Dispose();
                ms.Close();
                return true;
                MessageBox.Show("Entro al correo");
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public IEnumerable<CodigoFechaAbierta> VerPedidosClienteExterno(string cod)
        {
            IEnumerable<CodigoFechaAbierta> dato;
            dato = _cnn.Query<CodigoFechaAbierta>("SP_ObtenerPedidoBoletaControl", commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    @codpedido = cod
                }
                ).ToList();

            return dato;
        }

        public string UpdatePedidosClienteExterno(string pedido, int cantidad)
        {
            string dato;
            try
            {
                _cnn.Query("SP_UpdatePedidoBoletaControl", commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    @codpedido = pedido,
                    @cantidad = cantidad
                    //@codSapProducto = codSapProducto
                }
                );
                dato = "Exitoso";
                return dato;
            }
            catch (Exception e)
            {
                dato = "Fallo";
                return dato;
            }
        }

        public string InsertarAsignacionBoleta(IEnumerable<CodigoFechaAbierta> FechaAbierta)
        {
            var correo = FechaAbierta.First().Correo;
            var nombre = FechaAbierta.First().NombreCliente;
            var aventurita = FechaAbierta.First().NombreEnviado;
            var idCliente = FechaAbierta.First().IdSAPCliente;
            var pedido = FechaAbierta.First().CodSapPedido;
            List<CodigoFechaAbierta> listaTemporal = null;
            var pedidos = "";
            try
            {
                pedidos = _cnn.Query<string>("SP_InsertarAsigancionBoleta", commandType: System.Data.CommandType.StoredProcedure,
                          param: new
                          {
                              @Correo = correo,
                              @Nombre = nombre,
                              @IdSAPCliente = idCliente,
                              @Pedido = pedido,
                              @NombreAventurita = aventurita
                          }).First();



                /*if (listaTemporal == null)
                {
                    listaTemporal = new List<CodigoFechaAbierta>();
                    var datoLista = getCodigosBoletaControl(FechaAbierta.ToList()); 
                    listaTemporal = datoLista.ListaCodigoFechaAbierta;

                }*/
                getCodigosBoletaControl(FechaAbierta.ToList());

                foreach (var item in FechaAbierta)
                {
                    /*if (listaTemporal != null)
                    {
                        if (listaTemporal.Exists(x => x.CodSapPedido == item.CodSapPedido))
                        {
                            item.CodBarrasBoletaControl = listaTemporal.Where(x => x.CodSapPedido == item.CodSapPedido && item.CodBarrasBoletaControl != "00").First().CodBarrasBoletaControl;
                            listaTemporal.Where(x => x.CodSapPedido == item.CodSapPedido && item.CodBarrasBoletaControl != "00").First().CodBarrasBoletaControl = "00";

                        }
                        else
                        {
                            //listaTemporal.Add(getCodigosBoletaControl(item.CodSapPedido, item.CantEnviar));
                            //Devolver algo o no?
                        }
                    }*/

                    _cnn.Query("SP_InsertarAsigancionBoletaDetalle", commandType: System.Data.CommandType.StoredProcedure,
                    param: new
                    {
                        @idAsignacion = pedidos,
                        @BoletaControl = item.CodBarrasBoletaControl,
                        @Boleteria = item.Consecutivo
                        //@Pedido = pedido
                    });
                }
                listaTemporal = null;
            }
            catch (Exception e)
            {
                pedidos = "fallo";
            }
            return pedidos;
        }
        public List<int> boletasControl(List<CodigoFechaAbierta> codigos)
        {
            List<int> lista = new List<int>();
            List<int> lPrueba = new List<int>();

            foreach (var item in codigos)
            {
                lista.Add(int.Parse(item.CodSapPedido));
            }

            lista = lista.GroupBy(x => x)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();
            lPrueba = lista.GroupBy(x => x)
                .Select(x => x.Key)
                .ToList();

            return lista;
        }
        //Aca debo dividir los pedidos y buscar su boleta cntrol
        public List<CodigoFechaAbierta> getCodigosBoletaControl(List<CodigoFechaAbierta> codigos)
        {
            List<CodigoFechaAbierta> cod = new List<CodigoFechaAbierta>();
            CodigoFechaAbierta Retornacod = new CodigoFechaAbierta();

            var Numeropedido = codigos.First().CodSapPedido;
            var cantidad = codigos.Count();
            try
            {
                //Usar o no?
                //List<int> ver = boletasControl(codigos);

                var codBoleta = _cnn.Query<string>("SP_ObtenerCodigosBoletaControl", commandType: System.Data.CommandType.StoredProcedure, param: new
                {
                    @codPedido = Numeropedido,
                    @cantidad = cantidad
                }).ToList();
                /* var i = 0;
                 while (i < codigos)
                 {
                     cod.CodSapPedido = Numeropedido;
                     cod.CodBarrasBoletaControl = codBoleta;
                     Retornacod.ListaCodigoFechaAbierta.Add(cod);
                     i++;
                 }*/
                cod = codigos;
                foreach (var item in codBoleta)
                {
                    var boletaControl = item.Split('-')[0];
                    var cantidadEnviar = item.Split('-')[1];
                    //foreach (var it in cod.Where(x => x.CodBarrasBoletaControl == null).Take(int.Parse(cantidadEnviar)))
                    foreach (var it in codigos.Where(x => x.CodBarrasBoletaControl == null).Take(int.Parse(cantidadEnviar)))
                    {
                        it.CodBarrasBoletaControl = boletaControl;
                    }
                }

                return cod;
            }
            catch (Exception e)
            {
                cod.First().Nombre = "No existe";
                return cod;
            }
        }
        public string CorreoAsigancionBoleta(string pedido, int cantidad, string codSapProducto)
        {
            //EnviarQR.EnviarCorreo();
            return "";
        }

        public string UpdateFechaBoletas(string consecutivoGenerado, string codsap, string Valor, DateTime? FechaInicial, DateTime? FechaFinal)
        {
            var pedidos = _cnn.Query("SP_updateBoletaAsignada", commandType: System.Data.CommandType.StoredProcedure,
                param: new
                {
                    @consecutivo = consecutivoGenerado,
                    @FechaInicial = FechaInicial,
                    @FechaFinal = FechaFinal,
                    @codsap = codsap,
                    @Valor = Valor
                });
            return "";
        }

        public string InsertarBoleteriaCodigosExternos(CodigoFechaAbierta codigo)
        {
            codigo.Consecutivo = "";
            var dato = _cnn.Query<string>("SP_InsertarBoleteriaCodigosVirtuales", commandType: System.Data.CommandType.StoredProcedure, param: new
            {
                @IdProducto = codigo.IdProducto,
                @Consecutivo = codigo.Consecutivo,
                //@IdSolicitudBoleteria = 0,
                @IdEstado = 1,
                @Valor = codigo.Valor,
                //@CodigoSapConvenio = "",
                @CodigoVenta = codigo.CodSapPedido,
                //@FechaImpresion = ,
                @FechaUsoInicial = codigo.FechaInicial,
                @FechaUsoFinal = codigo.FechaFinal,
                @FechaInicioEvento = codigo.FechaInicial,
                @FechaFinEvento = codigo.FechaFinal,
                @IdUsuarioCreacion = 1,
                @Saldo = codigo.Valor,
                @EsMedioPago = 1,
                //@Posicion INT = NULL,
                //@IdBoleteria 
            }).Last();
            return dato;
        }
        public string ObteneridCliente(string numeroPedido)
        {
            var dato = _cnn.Query<string>("SP_ObtenerInfoCliente", commandType: System.Data.CommandType.StoredProcedure, param: new
            {
                @NumPedido = numeroPedido
            }).First();
            return dato;
        }

        public string ObtenerPedidosPorUsuario(string idUsuario, string Usuario)
        {
            try
            {
                /*var dato = _cnn.Query<string>("SP_ObtenerPedidosPorUsuario", commandType: System.Data.CommandType.StoredProcedure, param: new
                {
                    @idUsuario = idUsuario,
                    @NomUsuario = Usuario
                }).First();*/
                var datoo = "";
                var dato = _cnn.Query<string>("SP_ObtenerPedidosPorUsuario", commandType: System.Data.CommandType.StoredProcedure, param: new
                {
                    @idUsuario = idUsuario,
                    @NomUsuario = Usuario
                });
                foreach (var item in dato)
                {
                    if (dato.Count() > 1)
                    {
                        datoo += item + "|";
                    }
                    else
                    {
                        datoo += item;
                    }
                }
                return datoo;
            }
            catch (Exception e)
            {
                var dato = "No existen pedidos para mostrar";
                return dato;
            }
        }
        public IEnumerable<CodigoFechaAbierta> VerPedidosClienteExternoMultiple(string pedido)
        {
            IEnumerable<CodigoFechaAbierta> dato = null;
            try
            {
                dato = _cnn.Query<CodigoFechaAbierta>("SP_ObtenerPedidoBoletaControl", commandType: System.Data.CommandType.StoredProcedure,
                    param: new
                    {
                        @codpedido = pedido
                    }
                    ).ToList();
                //CodigoFechaAbierta codigo = new CodigoFechaAbierta();
                //codigo.ListaCodigoFechaAbierta = 
                return dato;
            }
            catch (Exception e)
            {
                return dato;
            }
        }
        public string updatePedidoUsuarioExterno(string pedido, int idUsuario, string IdSap)
        {
            try
            {
                //var dato = _cnn.Update($"update tb_usuarioExterno set pedido = '{pedido}' where idusuario = 12 and Usuario = '{idUsuario}'");
                _cnn.Query("SP_InsertarDetalleUsuarioExterno", commandType: System.Data.CommandType.StoredProcedure, param: new
                {
                    @IdUsuario = idUsuario,
                    @IdSapCliente = IdSap,
                    @Pedido = pedido
                });
                return "Exitoso";
            }
            catch (Exception e)
            {
                return "fallo";
            }
        }
        public string verPedidosPorIdCliente(string IdCliente)
        {
            var dato = "";
            try
            {
                dato = _cnn.Query<string>("SP_ObtenerPedidoPorCliente", commandType: System.Data.CommandType.StoredProcedure,
                    param: new
                    {
                        @IdCliente = IdCliente
                    }
                    ).First();
                return dato;
            }
            catch (Exception e)
            {
                dato = "Este cliente no tiene pedidos habilitados";
                return dato;
            }
        }
        public string EnviarUsuario(CodigoFechaAbierta datosAventurita)
        {
            var mensaje = "";
            mensaje += "Su nombre de usuario es: " + datosAventurita.usuario.NombreUsuario + " - Password: " + datosAventurita.usuario.Password + " debe cambiar su contraseña.  - " +
                //"Esta es la ruta para poder ingresar :" + "http://8.242.211.181/Admin/CuentaExterna/Login";
                "Esta es la ruta para poder ingresar :" + "http://localhost:62696/CuentaExterna/Login";
            EnviarCorreo(datosAventurita.usuario.Correo, "prueba", mensaje, MailPriority.High, null);
            return "";
        }
        public string EnviarUsuarioProductos(CodigoFechaAbierta datosAventurita)
        {
            try
            {
                List<string> listaArchivos = new List<string>();
                listaArchivos.Add(datosAventurita.RutaPDF);
                EnviarCorreo(datosAventurita.Correo, "Mundo Aventura", "Te adjuntamos tus productos en codigos", MailPriority.High, listaArchivos);
                return "";
            }
            catch (Exception)
            {
                return "fallo";
            }
        }

        public MemoryStream intentoImgBD(string rtaLogo, string IdClienteSap)
        {

            //&byte[] img = File.ReadAllBytes("D:/Usuarios/User/itrujillo/Pictures/Screenshots/imgpru.png");
            //byte[] img = File.ReadAllBytes("D:/Usuarios/User/itrujillo/Pictures/Screenshots/ffff.png");
            byte[] img = File.ReadAllBytes(rtaLogo);
            /*using System.Drawing.Imaging.Image img = new Image.FromStream();*/

            _cnn.Query("SP_InsertarImagenCliente", commandType: System.Data.CommandType.StoredProcedure, param: new
            {
                @img = img,
                @IdClienteSap = IdClienteSap
            });

            byte[] imgCargar = _cnn.Query<byte[]>("SP_ObetenerImagenCliente", commandType: System.Data.CommandType.StoredProcedure, param: new
            {
                @IdClienteSap = IdClienteSap
                //}).First();
            }).Last();

            MemoryStream guarda = new MemoryStream(imgCargar, 0, imgCargar.Length);
            guarda.Write(imgCargar, 0, imgCargar.Length);
            /*MemoryStream guarda = new MemoryStream(img, 0, img.Length);
            guarda.Write(img, 0, img.Length);*/
            return guarda;
        }
        public MemoryStream ObtenerLogoCliente(string IdClienteSap)
        {
            ReportePDF Rpdf = new ReportePDF();
            try
            {
                Rpdf.repositorio("inicio repositorio logo ");
                //&byte[] img = File.ReadAllBytes("D:/Usuarios/User/itrujillo/Pictures/Screenshots/imgpru.png");
                //byte[] img = File.ReadAllBytes("D:/Usuarios/User/itrujillo/Pictures/Screenshots/ffff.png");
                //byte[] img = File.ReadAllBytes(rtaLogo);
                /*using System.Drawing.Imaging.Image img = new Image.FromStream();*/

                /*_cnn.Query("SP_InsertarImagenCliente", commandType: System.Data.CommandType.StoredProcedure, param: new
                {
                    @img = img,
                    @IdClienteSap = IdClienteSap
                });*/
                Rpdf.repositorio("empezo repositorio logo ");
                byte[] imgCargar = _cnn.Query<byte[]>("SP_ObetenerImagenCliente", commandType: System.Data.CommandType.StoredProcedure, param: new
                {
                    @IdClienteSap = IdClienteSap
                    //}).First();
                }).Last();

                Rpdf.repositorio("consiguio repositorio logo ");

                MemoryStream guarda = new MemoryStream(imgCargar, 0, imgCargar.Length);
                guarda.Write(imgCargar, 0, imgCargar.Length);
                /*MemoryStream guarda = new MemoryStream(img, 0, img.Length);
                guarda.Write(img, 0, img.Length);*/
                Rpdf.repositorio("finalizo repositorio logo ");
                return guarda;
            }
            catch (Exception e)
            {
                Rpdf.repositorio("fallo repositorio");
                throw e;
            }
        }

        #endregion
    }
}



