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
    public partial class FormMDI : Form
    {

        #region Constructor

        public FormMDI()
        {
            InitializeComponent();
        }

        #endregion

        #region Eventos

        private void centroMedicoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCentroMedico objFormCentroMedico = new FormCentroMedico();
            objFormCentroMedico.MdiParent = this;
            objFormCentroMedico.WindowState = FormWindowState.Maximized; 
            objFormCentroMedico.Show();
        }

        private void controlParqueaderoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormControlPrqueadero objFormControlPrqueadero = new FormControlPrqueadero();
            objFormControlPrqueadero.MdiParent = this;
            //objFormControlPrqueadero.WindowState = FormWindowState.Maximized;
            objFormControlPrqueadero.Show();
        }

        private void ingresoParqueaderoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormParqueaderoIngreso objFOrm = new FormParqueaderoIngreso();
            objFOrm.MdiParent = this;
            //objFormControlPrqueadero.WindowState = FormWindowState.Maximized;
            objFOrm.Show();
        }
        

        private void notificacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNotificacionesEnvio objFormNotificacionesEnvio = new FormNotificacionesEnvio();
            objFormNotificacionesEnvio.MdiParent = this;
            objFormNotificacionesEnvio.WindowState = FormWindowState.Maximized;
            objFormNotificacionesEnvio.Show();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void FormMDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("¿Realmente desea salir de la aplicación?", "Mundo Aventura", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        #endregion
        
    }
}
