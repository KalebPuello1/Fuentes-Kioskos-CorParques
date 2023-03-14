using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{

    public class RepositorioUsuarioVisitante : RepositorioBase<UsuarioVisitante>, IRepositorioUsuarioVisitante
    {
        #region Declaraciones

        private readonly SqlConnection _conn;
        public string RutaSoporteCorreos = ConfigurationManager.AppSettings["RutaSoporteCorreos"].ToString();

        #endregion

        #region Constructor

        public RepositorioUsuarioVisitante()
        {
            _conn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion
        public int InsertarUsuarioVisitante(UsuarioVisitanteViewModel usuarioVisitante, int mesesCortesia)
        {
            try
            {


                UsuarioVisitante usuario = new UsuarioVisitante();
                Cortesia cortesia = new Cortesia();
                int IdUsuario = 0;
                usuario.Activo = usuarioVisitante.Activo;
                usuario.Apellidos = usuarioVisitante.Apellidos;
                usuario.Correo = usuarioVisitante.Correo;
                usuario.FechaActualizacion = DateTime.Now;
                usuario.FechaCreacion = usuarioVisitante.FechaCreacion;

                usuario.Nombres = usuarioVisitante.Nombres;
                usuario.NumeroDocumento = usuarioVisitante.NumeroDocumento;
                usuario.Telefono = usuarioVisitante.Telefono;
                usuario.TipoDocumento = usuarioVisitante.TipoDocumento;

                cortesia.Activo = usuarioVisitante.Activo;
                cortesia.Cantidad = usuarioVisitante.Cantidad;
                cortesia.Descripcion = usuarioVisitante.Descripcion;
                cortesia.FechaCreacion = usuarioVisitante.FechaCreacion;
                cortesia.FechaInicial = DateTime.Now;



                if (usuarioVisitante.IdPlazo == 1)
                {
                    mesesCortesia = 1;

                }
                if (usuarioVisitante.IdPlazo == 2)
                {
                    mesesCortesia = 2;
                }
                if (usuarioVisitante.IdPlazo == 3)
                {
                    mesesCortesia = 3;
                }

                cortesia.FechaFinal = mesesCortesia == 0 ? DateTime.Now.AddMonths(1) : DateTime.Now.AddMonths(mesesCortesia);
                cortesia.IdTipoCortesia = usuarioVisitante.IdTipoCortesia;
                cortesia.IdUsuarioCreacion = usuarioVisitante.IdUsuarioCreacion;

                cortesia.NumeroTicket = usuarioVisitante.NumeroTicket;
                cortesia.Aprobacion = usuarioVisitante.Aprobacion;
                cortesia.IdPlazo = usuarioVisitante.IdPlazo;
                cortesia.IdComplejidad = usuarioVisitante.IdComplejidad;
                cortesia.IdRedencion = usuarioVisitante.IdRedencion;
                if (usuarioVisitante.Archivo != null)
                {
                    cortesia.RutaSoporte = RutaSoporteCorreos + usuarioVisitante.Archivo;
                }
                else
                {
                    cortesia.RutaSoporte = null;
                }

                if (cortesia.IdTipoCortesia == 0)
                {
                    cortesia.IdTipoCortesia = 1;
                }

                if (usuarioVisitante.BanderaFan == 1 || usuarioVisitante.BanderaFan == 2 || usuarioVisitante.BanderaFan == 3)
                {
                    cortesia.FechaInicial = usuarioVisitante.FechaInicialFan;
                    cortesia.FechaFinal = usuarioVisitante.FechaFinalFan;
                    cortesia.NumTarjetaFAN = usuarioVisitante.NumTarjetaFAN;
                    cortesia.Observacion = usuarioVisitante.DescripcionBeneficioFAN;
                }
                if (usuarioVisitante.IdTipoCortesia != 3)
                {


                    var var = _cnn.GetList<UsuarioVisitante>().Where(x => x.NumeroDocumento == usuario.NumeroDocumento).ToList();
                    if (var != null && var.Count > 0)
                    {
                        usuario.Id = var.FirstOrDefault().Id;
                        _cnn.Update(usuario);
                        cortesia.IdUsuarioVisitante = var[0].Id;
                        IdUsuario = usuario.Id;
                    }
                    else
                    {
                        IdUsuario = _cnn.Insert<int>(usuario);
                        cortesia.IdUsuarioVisitante = IdUsuario;

                    }
                }
                else
                {
                    cortesia.IdUsuarioCreacion = null;
                    cortesia.IdUsuarioVisitante = null;
                    cortesia.CorreoAPP = usuario.Correo;
                    IdUsuario = 1;
                }


                int IdCortesia = _cnn.Insert<int>(cortesia);


                foreach (var item in usuarioVisitante.listaProductosAgregar)
                {
                    if (IdCortesia > 0)
                    {
                        var resp33 = _cnn.Query<int>(sql: "SP_AgregarDetalleCortesia", param: new
                        {
                            IdCortesia = IdCortesia,
                            CodigoSap = item.CodigoSap,
                            Cantidad = item.Cantidad
                        }, commandType: CommandType.StoredProcedure
                  );
                    }
                }
                return IdUsuario;
            }
            catch (Exception e)
            {

                throw;
            }
        }

    }
}
