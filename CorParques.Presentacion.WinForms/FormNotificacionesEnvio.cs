using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Threading;

namespace CorParques.Presentacion.WinForms
{
    public partial class FormNotificacionesEnvio : Form
    {

        #region Declaraciones

        int prioridad = 0;
        FormTeclado objFormTeclado;

        #endregion

        #region Constructor

        public FormNotificacionesEnvio()
        {
            InitializeComponent();            
        }

        #endregion

        #region Metodos

        private async void ObtenerGrupos()
        {
            try
            {
                var x = await FormBase<List<Grupo>>.GetAsync<List<Grupo>>("Grupo/ObtenerGruposActivos");
                //x.Add(new Grupo() { Id = 0, Nombre = "Ninguno" });
                lbxGrupos.DataSource = x.OrderBy(M => M.Id).ToList();
                lbxGrupos.DisplayMember = "Nombre";
                lbxGrupos.ValueMember = "Id";
                //lbxGrupos.SelectedValue = 0;
                lbxGrupos.Refresh();
                btnEnviar.Enabled = true;                
                lbl_Texto.Visible = false;                
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Concat("Ocurrio un error en ObtenerGrupos: ", ex.Message), "Notificaciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private async Task<object> enviarCorreo(string[] grupo, string[] correo)
        {
            var x = await FormBase<object>.PostAsync<object>("Mail/Send", new Correo { });
            bool valor = bool.Parse(x.ToString());
            if (valor)
                return true;
            else
                return false;
        }

        private async void EnviarCorreo()
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;
                lbl_Texto.Text = "Enviando ...";
                lbl_Texto.Visible = true;
                btnEnviar.Enabled = false;

                int[] grupo = null;
                int intContador = 0;
                string[] correo = null;

                if (lbxGrupos.SelectedItems.Count > 0)
                {
                    grupo = new int[lbxGrupos.SelectedItems.Count];

                    foreach (Grupo objGrupo in lbxGrupos.SelectedItems)
                    {
                        grupo[intContador] = objGrupo.Id;
                        intContador += 1;
                    }
                }

                if (lbxMails.Items.Count > 0)
                {
                    correo = new string[lbxMails.Items.Count];
                    for (int i = 0; i < lbxMails.Items.Count; i++)
                        correo[i] = lbxMails.Items[i].ToString();
                }

                Notificacion _notificacion = new Notificacion()
                {
                    //Creado = 1,
                    //FechaModificado = DateTime.Now,
                    //Estado = "C",
                    //Modificado = 0,
                    //FechaCreado = DateTime.Now,
                    //Grupos = grupo,
                    //Asunto = txtAsunto.Text.Trim(),
                    //Contenido = txtContenido.Text.Trim(),
                    //Prioridad = prioridad,
                    //Status = "S",
                    //OtrosCorreos = correo
                };

                var resultado = await FormBase<Notificacion>.PostAsync<Notificacion>("Notificacion/Set", _notificacion);
                //if (!string.IsNullOrEmpty(resultado.Repuestas))
                //{
                //    throw new ArgumentException(resultado.Repuestas);
                //}
                //else
                //{
                //    this.Cursor = Cursors.Default;
                //    lbl_Texto.Visible = false;
                //    MessageBox.Show("Notificación enviada exitosamente.", "Notificaciones", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}


            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                lbl_Texto.Visible = false;
                MessageBox.Show(string.Concat("Ocurrio un error en EnviarCorreo: \r\n", ex.Message), "Notificaciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnEnviar.Enabled = true;
            }   
        }

        private bool EmailValido(string strEmail)
        {

            System.Text.RegularExpressions.Regex ValidarEMail = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
            bool blnEmailValido = false;

            try
            {
                blnEmailValido = ValidarEMail.IsMatch(strEmail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Concat("Ocurrio un error en EmailValido: ", ex.Message), "Notificaciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return blnEmailValido;
        }

        private bool Validaciones()
        {

            bool blnValidaciones = false;
            string strValidaciones = "";

            try
            {

                if (lbxGrupos.SelectedItems.Count == 0 && lbxMails.Items.Count == 0)
                {
                    strValidaciones = "- Debe seleccionar un grupo o bien debe agregar un correo para enviar la notificación.";                                        
                }
                if (txtAsunto.Text.Trim().Length == 0)
                {
                    if (strValidaciones.Trim().Length > 0)
                    {
                        strValidaciones = string.Concat(strValidaciones, "\r\n", "- Debe digitar el asunto de la notificación.");
                    }
                    else
                    {
                        strValidaciones = "- Debe digitar el asunto de la notificación.";
                    }
                }               

                if (txtContenido.Text.Trim().Length == 0)
                {
                    if (strValidaciones.Trim().Length > 0)
                    {
                        strValidaciones = string.Concat(strValidaciones, "\r\n", "- Debe digitar el mensaje de la notificación.");
                    }
                    else
                    {
                        strValidaciones = "- Debe digitar el mensaje de la notificación.";
                    }
                }


                if (strValidaciones.Trim().Length > 0)
                {
                    MessageBox.Show(string.Concat("Revise lo siguiente: \r\n\r ", strValidaciones), "Notificaciones", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    blnValidaciones = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Concat("Ocurrio un error en Validaciones: ", ex.Message), "Notificaciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return blnValidaciones;

        }

        private void AdicionarCorreo()
        {

            if (!EmailValido(txtMail.Text.Trim()))
            {
                txtMail.Focus(); 
                MessageBox.Show("Debe digitar un email valido.", "Notificaciones", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            lbxMails.Items.Add(txtMail.Text.Trim());
            lbxMails.Refresh();
            txtMail.Text = string.Empty;

        }

        private void LimpiarPantalla()
        {
            txtAsunto.Text = string.Empty;
            txtContenido.Text = string.Empty;
            txtMail.Text = string.Empty;  
            lbxMails.Items.Clear();
            lbxGrupos.ClearSelected();          
        }

        #endregion

        #region Eventos

        private void FormNotificacionesEnvio_Load(object sender, EventArgs e)
        {            
            ObtenerGrupos();            
        }

        private void txtMail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (string.IsNullOrEmpty(txtMail.Text.Trim()))
                {
                    e.Handled = false;
                    return;
                }
                AdicionarCorreo();
            }
        }

        private void lbxMails_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                if (MessageBox.Show("¿Realmente desea remover este correo?", "Notificaciones", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    lbxMails.Items.Remove(lbxMails.SelectedItem);
                }
            }
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {

            if (!Validaciones())
            {
                if (MessageBox.Show("¿Realmente desea enviar esta notificación?", "Notificaciones", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    EnviarCorreo();
                    LimpiarPantalla();
                }
            }            
        }

        private void radAlta_CheckedChanged(object sender, EventArgs e)
        {            
            prioridad = 2;
        }

        private void radNormal_CheckedChanged(object sender, EventArgs e)
        {
    
            prioridad = 0;
        }

        private void radBaja_CheckedChanged(object sender, EventArgs e)
        {     
            prioridad = 1;

        }

        private void btnAdicionarCorreo_Click(object sender, EventArgs e)
        {
            AdicionarCorreo();
        }

        #endregion

        private void txtAsunto_Enter(object sender, EventArgs e)
        {
            objFormTeclado = new FormTeclado(ref txtAsunto);
            objFormTeclado.StartPosition = FormStartPosition.CenterScreen;
            objFormTeclado.Show();
        }

        private void txtContenido_Enter(object sender, EventArgs e)
        {
            objFormTeclado = new FormTeclado(ref txtContenido);
            objFormTeclado.StartPosition = FormStartPosition.CenterScreen;
            objFormTeclado.Show();
        }

        private void txtMail_Enter(object sender, EventArgs e)
        {
            objFormTeclado = new FormTeclado(ref txtMail);
            objFormTeclado.StartPosition = FormStartPosition.CenterScreen;
            objFormTeclado.Show();
        }

        private void txtAsunto_Leave(object sender, EventArgs e)
        {
            objFormTeclado.Close();
        }

        private void txtContenido_Leave(object sender, EventArgs e)
        {
            objFormTeclado.Close();
        }

        private void txtMail_Leave(object sender, EventArgs e)
        {
            objFormTeclado.Close();
        }

        private void txtAsunto_TextChanged(object sender, EventArgs e)
        {
            txtAsunto.Select(txtAsunto.Text.Length, 0);
        }

        private void txtContenido_TextChanged(object sender, EventArgs e)
        {
            txtContenido.Select(txtContenido.Text.Length, 0);
        }

        private void txtMail_TextChanged(object sender, EventArgs e)
        {
            txtMail.Select(txtMail.Text.Length, 0);
        }
    }
}
