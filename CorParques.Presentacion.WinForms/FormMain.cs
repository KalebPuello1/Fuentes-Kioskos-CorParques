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
    public partial class FormMain : Form
    {

        #region Constructor

        public FormMain()
        {
            InitializeComponent();
        }

        #endregion


        #region Eventos

        private void btn_Notificaciones_Click(object sender, EventArgs e)
        {
            FormNotificacionesEnvio objFormNotificacionesEnvio = new FormNotificacionesEnvio();
            objFormNotificacionesEnvio.ShowDialog();

        }

        private void btn_Centro_Medico_Click(object sender, EventArgs e)
        {
            FormCentroMedico objFormCentroMedico = new FormCentroMedico();
            objFormCentroMedico.ShowDialog();
        }

        private void btn_Control_Parqueadero_Click(object sender, EventArgs e)
        {
            FormControlPrqueadero objFormControlPrqueadero = new FormControlPrqueadero();
            objFormControlPrqueadero.ShowDialog();
        }

        #endregion
        
    }
}
