using System;
using System.Text.RegularExpressions;
using CorParques.Negocio.Entidades;
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
    public partial class FormCentroMedico : Form
    {
        int intPaciente;
        bool bolRespuesta;
        bool bolRespuesta1;
        bool bolRespuesta2;
        string strExpresion;
        
        #region Constructor
        public FormCentroMedico()
        {
            InitializeComponent();
        }
        #endregion
        
        #region Controles
        private void FormCentroMedico_Load(object sender, EventArgs e)
        {
            LimpiarPaciente();
            LimpiarProcedimiento();
            listarPaciente();
        }
          
        private async void btnGuardarPaciente_Click(object sender, EventArgs e)
        {
            
            Procedimiento procedimiento = new Procedimiento();

            bolRespuesta1 = ValidarPaciente();
            bolRespuesta2 = ValidarProcedimiento();

            if (bolRespuesta1 && bolRespuesta2)
            {
                procedimiento = ProcedimientoPam();
                //RDSH procedimiento.Paciente = PacientePam();

                var rta = await GuardarInformacion(procedimiento);
                //var rta = string.Empty;

                if (string.IsNullOrEmpty(rta))
                {
                    MessageBox.Show("Datos guardados con éxtio", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarPaciente();
                    LimpiarProcedimiento();
                    listarPaciente();
                    return;
                }
                else
                {
                    MessageBox.Show("Error en proceso, datos NO guardados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LimpiarPaciente();
                    LimpiarProcedimiento();
                    return;
                }
            }
        }

        private void lbxPacientes_DoubleClick(object sender, EventArgs e)
        {
            Paciente _Paciente = new Paciente();

            intPaciente = int.Parse(lbxPacientes.SelectedValue.ToString());
            _Paciente = ConsultarPaciente(intPaciente);
            PacienteMap(_Paciente);
        }
        #endregion

        #region Método

        private Paciente ConsultarPaciente(int codPaciente)
        {
            Paciente _Paciente = new Paciente();

            return _Paciente;
        }

        private async Task<string> GuardarInformacion(Procedimiento procedimiento)
        {
            var rta =  await FormBase<string>.PostAsync<Procedimiento>("MedicalCenter/Insert", procedimiento);
            return rta;
        }

        private void LimpiarPaciente()
        {
            tbxCedulaPaciente.Text = string.Empty;
            tbxNombrePaciente.Text = string.Empty;
            tbxApellidoPaciente.Text = string.Empty;
            tbxAcudientePaciente.Text = string.Empty;
            tbxTelPaciente.Text = string.Empty;
            tbxTelAcudientePaciente.Text = string.Empty;
            tbxCorreoPaciente.Text = string.Empty;
        }

        private void LimpiarProcedimiento()
        {
            tbxCausasPaciente.Text = string.Empty;
            tbxSintomasPaciente.Text = string.Empty;
            tbxAlergiasPaciente.Text = string.Empty;
            tbxTratamientoPaciente.Text = string.Empty;
            tbxRecomendacionesPaciente.Text = string.Empty;
        }

        private async void listarPaciente()
        {
            string strRespuesta;
            
            strRespuesta = string.Empty;
            var varLista = await FormBase<List<Paciente>>.GetAsync<List<Paciente>>("Paciente/GetAll");
            //RDSH varLista.Add(new Paciente() { Id = 0, Nombre = "Ninguno", Apellido = "Ninguno" });

            lbxPacientes.DataSource = varLista.OrderBy(M => M.IdPaciente).ToList();
            lbxPacientes.DisplayMember = "Nombre";
            lbxPacientes.DisplayMember = "Apellido";
            lbxPacientes.ValueMember = "Id";
            lbxPacientes.SelectedValue = 0;
            lbxPacientes.Refresh();
            
        }

        private void PacienteMap(Paciente _Paciente)
        {
            //RDSH
            //tbxCedulaPaciente.Text = Convert.ToString(_Paciente.Cedula);
            //tbxNombrePaciente.Text = _Paciente.Nombre;
            //tbxApellidoPaciente.Text = _Paciente.Apellido;
            //tbxAcudientePaciente.Text = _Paciente.Acudiente;
            //tbxTelPaciente.Text = Convert.ToString(_Paciente.TelPaciente);
            //tbxTelAcudientePaciente.Text = Convert.ToString(_Paciente.TelAcudiente);
            //tbxCorreoPaciente.Text = _Paciente.Correo;

            tbxCausasPaciente.Focus();
        }

        private Paciente PacientePam()
        {
            Paciente _Paciente = new Paciente();
            
            //RDSH
            //_Paciente.Cedula = Convert.ToInt32(tbxCedulaPaciente.Text);
            //_Paciente.Nombre = tbxNombrePaciente.Text;
            //_Paciente.Apellido = tbxApellidoPaciente.Text;
            //_Paciente.Acudiente = tbxAcudientePaciente.Text;
            //_Paciente.TelPaciente = Convert.ToInt32(tbxTelPaciente.Text);
            //_Paciente.TelAcudiente = Convert.ToInt32(tbxTelAcudientePaciente.Text);
            //_Paciente.Correo = (tbxCorreoPaciente.Text != string.Empty) ? tbxCorreoPaciente.Text : "N/A";
            //_Paciente.Creado = 1;
            //_Paciente.Modificado = 1;
            //_Paciente.FechaCreado = DateTime.Now;
            //_Paciente.FechaModificado = DateTime.Now;

            return _Paciente;
        }

        private Procedimiento ProcedimientoPam()
        {
            Procedimiento _Procedimiento = new Procedimiento();
            _Procedimiento.Causa = tbxCausasPaciente.Text;
            _Procedimiento.Sintomas = tbxSintomasPaciente.Text;
            _Procedimiento.Alergias = (tbxAlergiasPaciente.Text != string.Empty) ? tbxAlergiasPaciente.Text : "N/A";
            _Procedimiento.Tratamiento = tbxTratamientoPaciente.Text;
            _Procedimiento.Recomendaciones = (tbxRecomendacionesPaciente.Text != string.Empty) ? tbxRecomendacionesPaciente.Text : "N/A";
            //RDSH
            //_Procedimiento.FechaCreado = DateTime.Now;
            //_Procedimiento.FechaModificado = DateTime.Now;
            //_Procedimiento.Estado = "1";

            return _Procedimiento;
        }

        private bool ValidarPaciente()
        {
            bolRespuesta = true;
            strExpresion = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

            if (string.IsNullOrEmpty(tbxCedulaPaciente.Text))
            {
                MessageBox.Show("Debe ingrese cédula del paciente para continuar", "Prevención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return bolRespuesta = false;
            }

            if (string.IsNullOrEmpty(tbxNombrePaciente.Text))
            {
                MessageBox.Show("Debe ingrese nombre del paciente para continuar", "Prevención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return bolRespuesta = false;
            }

            if (string.IsNullOrEmpty(tbxApellidoPaciente.Text))
            {
                MessageBox.Show("Debe ingrese apellido del paciente para continuar", "Prevención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return bolRespuesta = false;
            }

            if (string.IsNullOrEmpty(tbxAcudientePaciente.Text))
            {
                MessageBox.Show("Debe ingrese acudiente del paciente para continuar", "Prevención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return bolRespuesta = false;
            }

            if (string.IsNullOrEmpty(tbxTelPaciente.Text))
            {
                MessageBox.Show("Debe ingrese teléfono del paciente para continuar", "Prevención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return bolRespuesta = false;
            }

            if (tbxCorreoPaciente.Text != string.Empty)
            {
                if (!Regex.IsMatch(tbxCorreoPaciente.Text, strExpresion))
                {
                    MessageBox.Show("Debe ingresar un correo válido para continuar", "Prevención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return bolRespuesta = false;
                }
            }

            return bolRespuesta;
        }

        private bool ValidarProcedimiento()
        {
            bolRespuesta = true;

            if (string.IsNullOrEmpty(tbxCausasPaciente.Text))
            {
                MessageBox.Show("Debe ingresar las causas del paciente para continuar", "Prevención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return bolRespuesta = false;
            }

            if (string.IsNullOrEmpty(tbxSintomasPaciente.Text))
            {
                MessageBox.Show("Debe ingresar los sintomas del paciente para continuar", "Prevención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return bolRespuesta = false;
            }

            if (string.IsNullOrEmpty(tbxTratamientoPaciente.Text))
            {
                MessageBox.Show("Debe ingresar el tratamiento del paciente para continuar", "Prevención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return bolRespuesta = false;
            }

            return bolRespuesta;
        }
        #endregion
    }
}
