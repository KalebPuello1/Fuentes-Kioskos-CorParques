using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CorParques.Presentacion.WinForms
{
    public partial class FormTeclado : Form
    {
        #region VARIABLES DE CLASE
        public int it_shfbot = 0;
        public string it_swtshf = string.Empty;
        public string it_tecvar = string.Empty;
        public string it_txtstr = string.Empty;
        public TextBox objTextbox;

        //Clases.cl_extfns ob_extfns = new Clases.cl_extfns();
        #endregion

        #region INICIO FORMULARIO
        /* CARGA DE COMPONENTES */

        public FormTeclado(ref TextBox _objTextbox)
        {
            InitializeComponent();
            objTextbox = _objTextbox;
        }

        /* CARGA INICIAL FORMULARIO */
        private void fm_teclsp_Load(object sender, EventArgs e)
        {
            // DATOS DEL SISTEMA
            //ob_extfns.fn_crgsys("fm_teclsp");

            // INICIALIZAR VARIABLES
            it_shfbot = 0;
            it_swtshf = "N";
            it_tecvar = string.Empty;
            //Clases.cl_varglo.gb_tecstr = string.Empty;

            // INICIALIZAR TEC
            mt_cesoff();
            //lb_texstr.Text = string.Empty;
        }
        #endregion

        #region CONTROLES FORMULARIO
        /* BT ESCAPE */
        private void bt_esctec_Click(object sender, EventArgs e)
        {
            // INICIALIZAR VARIABLES
            it_shfbot = 0;
            it_swtshf = "N";
            it_tecvar = string.Empty;
            //Clases.cl_varglo.gb_tecstr = string.Empty;

            // INICIALIZAR TEC
            //lb_texstr.Text = string.Empty;

            // CERRAR TECLADO
            this.Dispose();
            this.Close();
        }

        /* BT F2 */
        private void bt_f02tec_Click(object sender, EventArgs e)
        {
            #region VARIABLES LOCALES
            //fm_dasysp ob_dasysp = new fm_dasysp();
            #endregion

            // DATOS SISTEMA
            //ob_dasysp.ShowDialog();
        }

        /* BT @ */
        private void bt_arrtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "@";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT 1 */
        private void bt_unotec_Click(object sender, EventArgs e)
        {
            it_tecvar = "1";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT 2 */
        private void bt_dostec_Click(object sender, EventArgs e)
        {
            it_tecvar = "2";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT 3 */
        private void bt_trstec_Click(object sender, EventArgs e)
        {
            it_tecvar = "3";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT 4 */
        private void bt_ctrtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "4";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT 5 */
        private void bt_cnctec_Click(object sender, EventArgs e)
        {
            it_tecvar = "5";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT 6 */
        private void bt_seitec_Click(object sender, EventArgs e)
        {
            it_tecvar = "6";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT 7 */
        private void bt_sietec_Click(object sender, EventArgs e)
        {
            it_tecvar = "7";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT 8 */
        private void bt_ochtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "8";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT 9 */
        private void bt_nuetec_Click(object sender, EventArgs e)
        {
            it_tecvar = "9";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT 0 */
        private void bt_certec_Click(object sender, EventArgs e)
        {
            it_tecvar = "0";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT = */
        private void bt_equtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "=";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT + */
        private void bt_plutec_Click(object sender, EventArgs e)
        {
            it_tecvar = "+";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT - */
        private void bt_mintec_Click(object sender, EventArgs e)
        {
            it_tecvar = "-";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT _ */
        private void bt_unltec_Click(object sender, EventArgs e)
        {
            it_tecvar = "_";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT BACK SPACE */
        private void bt_eratec_Click(object sender, EventArgs e)
        {
            #region VARIABLES LOCALES
            int lc_lenstr = 0;
            string lc_varstr = string.Empty;
            #endregion

            lc_lenstr = objTextbox.Text.Length;
            if (lc_lenstr > 0)
            {
                lc_varstr = objTextbox.Text.Substring(0, (lc_lenstr - 1));
                objTextbox.Text = lc_varstr.TrimStart();
            }            
        }

        /* BT Q */
        private void bt_qqqtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "Q";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT W */
        private void bt_wwwtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "W";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT E */
        private void bt_eeetec_Click(object sender, EventArgs e)
        {
            it_tecvar = "E";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT R */
        private void bt_rrrtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "R";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT T */
        private void bt_ttttec_Click(object sender, EventArgs e)
        {
            it_tecvar = "T";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT Y */
        private void bt_yyytec_Click(object sender, EventArgs e)
        {
            it_tecvar = "Y";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT U */
        private void bt_uuutec_Click(object sender, EventArgs e)
        {
            it_tecvar = "U";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT I */
        private void bt_iiitec_Click(object sender, EventArgs e)
        {
            it_tecvar = "I";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT O */
        private void bt_oootec_Click(object sender, EventArgs e)
        {
            it_tecvar = "O";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT P */
        private void bt_ppptec_Click(object sender, EventArgs e)
        {
            it_tecvar = "P";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT * */
        private void bt_atrtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "*";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT " */
        private void bt_comtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "''";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT \ */
        private void bt_bshtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "'\'";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT A */
        private void bt_aaatec_Click(object sender, EventArgs e)
        {
            it_tecvar = "A";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT S */
        private void bt_ssstec_Click(object sender, EventArgs e)
        {
            it_tecvar = "S";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT D */
        private void bt_dddtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "D";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT F */
        private void bt_ffftec_Click(object sender, EventArgs e)
        {
            it_tecvar = "F";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT G */
        private void bt_gggtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "G";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT H */
        private void bt_hhhtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "H";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT J */
        private void bt_jjjtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "J";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT K */
        private void bt_kkktec_Click(object sender, EventArgs e)
        {
            it_tecvar = "L";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT L */
        private void bt_llltec_Click(object sender, EventArgs e)
        {
            it_tecvar = "L";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT ; */
        private void bt_pyctec_Click(object sender, EventArgs e)
        {
            it_tecvar = ";";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT ' */
        private void bt_costec_Click(object sender, EventArgs e)
        {
            it_tecvar = "'";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT ENTER */
        private void bt_enttec_Click(object sender, EventArgs e)
        {

            it_tecvar = "\r\n";
            objTextbox.Text = objTextbox.Text + it_tecvar;

        }

        /* BT SHIFT 1 */
        private void bt_sh1tec_Click(object sender, EventArgs e)
        {
            // VALIDAR ONN/OFF CAR ESP
            it_shfbot = 0;
            if (it_swtshf == "N")
            {
                // ACTIVAR CAR ESP
                it_swtshf = "S";
                bt_sh1tec.Text = "Shift (ACT)";
                bt_sh2tec.Text = "Shift (ACT)";
                mt_cesonn();
            }
            else
            {
                // DESACTIVAR CAR ESP
                it_swtshf = "N";
                bt_sh1tec.Text = "Shift (DES)";
                bt_sh2tec.Text = "Shift (DES)";
                mt_cesoff();
            }
        }

        /* BT Z */
        private void bt_zzztec_Click(object sender, EventArgs e)
        {
            it_tecvar = "Z";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT X */
        private void bt_xxxtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "X";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT C */
        private void bt_ccctec_Click(object sender, EventArgs e)
        {
            it_tecvar = "C";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT V */
        private void bt_vvvtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "V";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT B */
        private void bt_bbbtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "B";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT N */
        private void bt_nnntec_Click(object sender, EventArgs e)
        {
            it_tecvar = "N";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT M */
        private void bt_mmmtec_Click(object sender, EventArgs e)
        {
            it_tecvar = "M";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT , */
        private void bt_coatec_Click(object sender, EventArgs e)
        {
            it_tecvar = ",";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT . */
        private void bt_ptttec_Click(object sender, EventArgs e)
        {
            it_tecvar = ".";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT / */
        private void bt_shatec_Click(object sender, EventArgs e)
        {
            it_tecvar = "/";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT SHIFT 2 */
        private void bt_sh2tec_Click(object sender, EventArgs e)
        {
            // VALIDAR ONN/OFF CAR ESP
            it_shfbot = 1;
            if (it_swtshf == "N")
            {
                // ACTIVAR CAR ESP
                it_swtshf = "S";
                bt_sh1tec.Text = "Shift (ACT)";
                bt_sh2tec.Text = "Shift (ACT)";
                mt_cesonn();
            }
            else
            {
                // DESACTIVAR CAR ESP
                it_swtshf = "N";
                bt_sh1tec.Text = "Shift (DES)";
                bt_sh2tec.Text = "Shift (DES)";
                mt_cesoff();
            }
        }

        /* BT SPACE */
        private void bt_spatec_Click(object sender, EventArgs e)
        {
            it_tecvar = " ";
            objTextbox.Text = objTextbox.Text + it_tecvar;
        }

        /* BT VER DATOS DEL SISTEMA */
        private void lb_sysppl_Click(object sender, EventArgs e)
        {
            // DATOS SISTEMA
            bt_f02tec.PerformClick();
        }
        #endregion

        #region MÉTODOS FORMULARIO
        /* ACTIVAR CAR ESP */
        public void mt_cesonn()
        {
            bt_unotec.Enabled = false;
            bt_dostec.Enabled = false;
            bt_trstec.Enabled = false;
            bt_ctrtec.Enabled = false;
            bt_cnctec.Enabled = false;
            bt_seitec.Enabled = false;
            bt_sietec.Enabled = false;
            bt_ochtec.Enabled = false;
            bt_nuetec.Enabled = false;
            bt_certec.Enabled = false;

            bt_equtec.Enabled = true;
            bt_plutec.Enabled = true;
            bt_mintec.Enabled = true;
            bt_unltec.Enabled = true;
            bt_atrtec.Enabled = true;
            bt_comtec.Enabled = true;
            bt_bshtec.Enabled = true;
            bt_pyctec.Enabled = true;
            bt_costec.Enabled = true;
            bt_coatec.Enabled = true;
            bt_ptttec.Enabled = true;
            bt_shatec.Enabled = true;
        }

        /*DESACTIVAR CAR ESP */
        public void mt_cesoff()
        {
            bt_unotec.Enabled = true;
            bt_dostec.Enabled = true;
            bt_trstec.Enabled = true;
            bt_ctrtec.Enabled = true;
            bt_cnctec.Enabled = true;
            bt_seitec.Enabled = true;
            bt_sietec.Enabled = true;
            bt_ochtec.Enabled = true;
            bt_nuetec.Enabled = true;
            bt_certec.Enabled = true;

            bt_equtec.Enabled = false;
            bt_plutec.Enabled = false;
            bt_mintec.Enabled = false;
            bt_unltec.Enabled = false;
            bt_atrtec.Enabled = false;
            bt_comtec.Enabled = false;
            bt_bshtec.Enabled = false;
            bt_pyctec.Enabled = false;
            bt_costec.Enabled = false;
            bt_coatec.Enabled = false;
            bt_ptttec.Enabled = false;
            bt_shatec.Enabled = false;
        }

        private void btn_Cerrar_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
        #endregion    
    }
}
