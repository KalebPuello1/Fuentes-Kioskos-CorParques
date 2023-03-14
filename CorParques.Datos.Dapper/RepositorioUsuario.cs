using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{
    public class RepositorioUsuario : RepositorioBase<Usuario>, IRepositorioUsuario
    {
        public bool ActualizarUsuario(Usuario usuario)
        {

            string strList = string.Empty;
            string strListPuntos = string.Empty;
            if (usuario.ListaPerfiles != null && usuario.ListaPerfiles.Any())
            {
                var _listID = (from x in usuario.ListaPerfiles select x.Id).ToList();
                strList = string.Join(",", _listID);
            }

            if (usuario.ListaPuntos != null && usuario.ListaPuntos.Any())
            {
                var _listID = (from x in usuario.ListaPuntos select x.Id).ToList();
                strListPuntos = string.Join(",", _listID);
            }

            var sql = _cnn.Query<bool>("sec.SP_ActualizarUsuario", new
            {
                IdUsuario = usuario.Id,
                Nombre= usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo,
                IdUsuarioModificacion= usuario.IdUsuarioModificacion,
                listaPefiles = strList,
                IdEstado = usuario.IdEstado,
                listaPuntos = strListPuntos
                //Password = usuario.Password
            }, commandType: System.Data.CommandType.StoredProcedure).Single();

            return sql;
        }

        public bool ActualizarPerfilUsuario(Usuario usuario)
        {

            string strList = string.Empty;
            
            if (usuario.ListaPerfiles != null && usuario.ListaPerfiles.Any())
            {
                var _listID = (from x in usuario.ListaPerfiles select x.Id).ToList();
                strList = string.Join(",", _listID);
            }

           
            var sql = _cnn.Query<bool>("sec.SP_ActualizarUsuarioPerfil", new
            {
                IdUsuario = usuario.Id,                
                listaPefiles = strList
               
                //Password = usuario.Password
            }, commandType: System.Data.CommandType.StoredProcedure).Single();

            return sql;
        }

        public Usuario GetUserPassword(string user, string pwd, int Punto)
        {
            try
            {
                var rta = _cnn.QueryMultiple("SP_GetUserByUsrPwd", new { User = user, pwd = pwd, Punto = Punto }, commandType: System.Data.CommandType.StoredProcedure);
                var usuario = rta.Read<Usuario>().FirstOrDefault();
                if (usuario != null)
                {
                    usuario.ListaPerfiles = rta.Read<TipoGeneral>().ToList();
                    usuario.ListaMenu = rta.Read<Menu>().ToList();
                    usuario.ListaPuntos = rta.Read<Puntos>().ToList();

                }
                return usuario;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public Usuario GetUserPassword2(string user, string pwd)
        {
            try
            {
                var rta = _cnn.Query<Usuario>("SP_RetornaUsuarioPWD2", param: new { User = user, pwd = pwd}, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                return rta;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public SaldoApp GetSaldoApp(string qr)
        {
            try
            {
                var rta = _cnn.Query<SaldoApp>("SP_RetornaSaldoApp", param: new { qr = qr }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                return rta;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public int InsertarUsuarioPerfil(Usuario usuario)
        {
            string strList = string.Empty;
            string strListPuntos = string.Empty;

            var var = _cnn.GetList<Usuario>().Where(x => x.NombreUsuario == usuario.NombreUsuario).ToList();
            if(var != null && var.Count > 0)
            {
                return 0;
            } 

            if (usuario.ListaPerfiles.Any())
            {
                var _listID = (from x in usuario.ListaPerfiles select x.Id).ToList();
                strList = string.Join(",", _listID);
            }

            if (usuario.ListaPuntos.Any())
            {
                var _listID = (from x in usuario.ListaPuntos select x.Id).ToList();
                strListPuntos = string.Join(",", _listID);
            }


            var sql = _cnn.Query<int>("sec.SP_InsertarUsuario", new {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Usuario = usuario.NombreUsuario,
                Password = usuario.Password,
                Correo = usuario.Correo,
                IdEstado = usuario.IdEstado,
                IdUsuarioCreacion = usuario.IdUsuarioCreacion,
                listaPefiles = strList,
                listaPuntos = strListPuntos,
                Password2 = usuario.Password2,
                CambioPassword = usuario.CambioPassword,
                IdEmpleado = usuario.IdEmpleado
            }, commandType: System.Data.CommandType.StoredProcedure).Single();

            return sql;
        }

        public IEnumerable<TipoGeneral> ObtenerUsuarioSimple(string filtro)
        {
            try
            {
                var rta = _cnn.Query<TipoGeneral>("SP_ObtenerUsuarioSimpleRptControlCaja", 
                    param: new { filtro = filtro}, 
                    commandType: System.Data.CommandType.StoredProcedure);
                return rta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Usuario> ObtenerUsuariosPorPefil(string IdsPerfil)
        {
            IEnumerable<Usuario> _lisUsuario = new List<Usuario>();

           try
            {
                _lisUsuario = this._cnn.Query<Usuario>("SP_ObtenerUsuariosPorPerfiles", 
                                                        commandType: System.Data.CommandType.StoredProcedure,
                                                        param: new
                                                        {
                                                            IdsPerfiles = IdsPerfil
                                                        });
            }
            catch
            {
                throw;
            }
            return _lisUsuario;
        }

        public IEnumerable<Usuario> ObtenerUsuariosSinAperturaPorPefil(string IdsPerfil, int Tipo)
        {
            IEnumerable<Usuario> _lisUsuario = new List<Usuario>();

            try
            {
                _lisUsuario = this._cnn.Query<Usuario>("SP_UsuariosAperturaPorPerfiles",
                                                        commandType: System.Data.CommandType.StoredProcedure,
                                                        param: new
                                                        {
                                                            IdsPerfiles = IdsPerfil,
                                                            Tipo = Tipo
                                                        });
            }
            catch
            {
                throw;
            }
            return _lisUsuario;
        }

        public IEnumerable<Perfil> ObtenerPerfilxUsuario(int idUsuario)
        {

            IEnumerable<Perfil> perfiles = new List<Perfil>();

            try
            {
                perfiles = this._cnn.Query<Perfil>("sec.SP_ConsultarPerfilxIdUsuario",
                                                        commandType: System.Data.CommandType.StoredProcedure,
                                                        param: new
                                                        {
                                                            Idusuario = idUsuario
                                                        });
            }
            catch(Exception e)
            {
                throw;
            }
            return perfiles;
           

        }
        public string InsertarUsuarioExterno(Usuario usuario)
        {
            try
            {
                var sql = _cnn.Query<string>("SP_InsertarUsuarioExterno", new
                {
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Usuario = usuario.NombreUsuario,
                    Password = usuario.Password,
                    Correo = usuario.Correo,
                    IdEstado = usuario.IdEstado,
                    IdUsuarioCreacion = usuario.IdUsuarioCreacion,
                    //listaPefiles = strList,
                    //listaPuntos = strListPuntos,
                    Password2 = usuario.Password2,
                    CambioPassword = usuario.CambioPassword,
                    //IdEmpleado = usuario.IdEmpleado,
                    //IdSAPCliente = 32321,
                    //Pedido = "232323",
                    ImagenCliente = "nada"
                }, commandType: System.Data.CommandType.StoredProcedure).Single();

                //repositorioCodigoFecha.updatePedidoUsuarioExterno();
                //return sql;
                return sql;
            }
            catch (Exception e)
            {
                //var sql = e.Message -- Usuario ya existe;
                var sql = "Usuario ya existe";
                return sql;
            }
        }
        public Usuario GetUserPasswordExterno(string user, string pwd, int Punto)
        {
            try
            {
                var rta = _cnn.QueryMultiple("SP_GetUserByUsrPwdExterno", new { User = user, pwd = pwd, Punto = Punto }, commandType: System.Data.CommandType.StoredProcedure);
                var usuario = rta.Read<Usuario>().FirstOrDefault();
                if (usuario != null)
                {
                    usuario.ListaPerfiles = rta.Read<TipoGeneral>().ToList();
                    usuario.ListaMenu = rta.Read<Menu>().ToList();
                    usuario.ListaPuntos = rta.Read<Puntos>().ToList();

                }
                return usuario;
            }
            catch (Exception ex)
            {

                throw;
            }

        }   
        public bool ActualizarExterno(ref UsuarioExterno usuario)
            {
                //return _cnn.Update(usuario);      
                try
            {
                //return _cnn.Update(usuario) > 0;
                //_cnn.Query("SP_ActualizarCambioClaveExternos", commandType: System.Data.CommandType.StoredProcedure, param: new
                //{
                //    Apellido = usuario.Apellido,
                //    Nombre = usuario.Nombre,
                //    IdUsuarioCreacion = usuario.IdUsuarioCreacion,
                //    Password = usuario.Password,
                //    Password2 = usuario.Password2,
                //    NombreUsuario = usuario.NombreUsuario
                //});
                _cnn.Query($"update TB_UsuarioExterno set Password = '{usuario.Password}', CambioPassword = '{usuario.CambioPassword}' WHERE Password = '{usuario.Password2}' AND Usuario = '{usuario.NombreUsuario}'");
                return true;
            }
            catch(Exception e)
            {
                return false;
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
                    @Pedido = pedido,
                });
                return "Exitoso";
            }
            catch (Exception e)
            {
                return "fallo";
            }
        }
    }
}
