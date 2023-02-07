using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    /// <summary>
    /// Obtener lista diccionario - TB_MenuPerfil, TB_PerfilUsuario,TB_PuntoUsuario, TB_Franquicias
    /// </summary>
    public class DiccionarioContigencia
    {
        public IEnumerable<MenuPerfil> ListMenuPerfil { get; set; }
        public IEnumerable<PuntoUsuario> ListPtoUsuario { get; set; }
        public IEnumerable<PerfilUsuario> ListPerfilUsuario { get; set; }
        public IEnumerable<Franquicia> ListFranquicia { get; set; }
        public IEnumerable<CampanaDonacion> ListCampanaDocacion { get; set; }
        public IEnumerable<ProductoCampanaDocanion> ListProductoCampanaDocanion { get; set; }

    }

    public class CampanaDonacion {
        public int IdCampanaDonacion { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public string Texto { get; set; }
    }
    public class ProductoCampanaDocanion
    {
        public int IdCampanaDonacion { get; set; }
        public string CodSapProducto { get; set; }
    }

}
