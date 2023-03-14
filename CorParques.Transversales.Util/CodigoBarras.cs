using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using Zen.Barcode;
using System.IO;


namespace CorParques.Transversales.Util
{
    public static class CodigoBarras
    {

        #region Funciones

        public static string GenerarCodigoDeBarras(string strCodigoBarras, int intAlto, int intAncho)
        {

            string strResultado = string.Empty;
            string strPathTemp = string.Empty;

            Code128BarcodeDraw BarCode = BarcodeDrawFactory.Code128WithChecksum;
            MemoryStream stream = new MemoryStream();
            
            try
            {
                
                Image img = BarCode.Draw(strCodigoBarras, intAlto, intAncho);
                img.Save(stream, ImageFormat.Png);
                strPathTemp = string.Concat(Path.GetTempPath(), strCodigoBarras, ".Jpeg");
                Image CodigoBarras = System.Drawing.Image.FromStream(stream);
                CodigoBarras.Save(strPathTemp, ImageFormat.Jpeg);
                strResultado = strPathTemp;             

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "CodigoBarras_GenerarCodigoDeBarras");
                strResultado = string.Concat("Error en GenerarCodigoDeBarras_CodigoBarras: ", ex.Message);
            }
            finally
            {                
                BarCode = null;
                stream = null;
            }

            return strResultado;
        }

        #endregion
                
    }
}
