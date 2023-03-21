using CorParques.Negocio.Entidades;
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
    /// <summary>
    /// Manuel Ochoa - 08/05/2017 - Formulario windows para la operacion de parqueadero
    /// </summary>
    public partial class FormControlPrqueadero : Form
    {
        public ControlParqueadero _ingreso { get; set; }
        public ControlParqueadero _salida { get; set; }
        public Usuario _usuario;

        public FormControlPrqueadero()
        {
            InitializeComponent();
            groupBoxSalida.Enabled = false;
            groupBoxSalida.Text = "Salida";
            geta();
        }

        private async void geta()
        {
            var listaTipoVehiculo = await FormBase<List<TipoVehiculo>>.GetAsync<List<TipoVehiculo>>("TypeVehiclePerParking/GetTypes");
            
            comboBoxTipoVehiculoSalida.DataSource = listaTipoVehiculo.OrderBy(M => M.Id).ToList();
            comboBoxTipoVehiculoSalida.DisplayMember = "Nombre";
            comboBoxTipoVehiculoSalida.ValueMember = "Id";
            comboBoxTipoVehiculoSalida.SelectedValue = -1;
            comboBoxTipoVehiculoSalida.Refresh();

            _usuario = new Usuario() { Id = 1, NombreUsuario= "bryantellez" };

        }

        private bool ValidarSalida()
        {
            bool isValid = true;
            try
            {
                if (textBoxValor.Text == string.Empty)
                {
                    isValid = false;
                }

                if (comboBoxTarifa.SelectedIndex == -1)
                {
                    isValid = false;
                }
            }
            catch (Exception ex)
            {
                isValid = false;
            }

            return isValid;
        }

        private void textBoxTicket_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxTicket_TextChanged(object sender, EventArgs e)
        {
            TextBox txtValidate = (TextBox)sender;
            if (System.Text.RegularExpressions.Regex.IsMatch(txtValidate.Text, "[^0-9]"))
            {
                txtValidate.Text = txtValidate.Text.Remove(txtValidate.Text.Length - 1);
            }
        }
        
        //No usar pageload para que sea asincornica
        private void FormControlPrqueadero_Load(object sender, EventArgs e)
        {
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxTicket.Text = textBoxTicket.Text.Trim();
                textBoxPlacaBusqueda.Text = textBoxPlacaBusqueda.Text.Trim();

                if (!string.IsNullOrEmpty(textBoxTicket.Text))
                {
                    int idControl = 0;
                    if (int.TryParse(textBoxTicket.Text, out idControl))
                    {
                        this.BuscarPorId(idControl);
                    }
                }
                else if (!string.IsNullOrEmpty(textBoxPlacaBusqueda.Text))
                {
                    this.BuscarPorPlaca(textBoxPlacaBusqueda.Text);
                }
                else
                {
                    MessageBox.Show("Datos incompletos", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void BuscarPorId(int Id)
        {
            var objRespControlParqueadero = await FormBase<ControlParqueadero>.GetAsync<ControlParqueadero>($"ControlParqueadero/GetIngresoById/{Id}");
            if (objRespControlParqueadero != null && !string.IsNullOrEmpty(objRespControlParqueadero.Placa))
            {
                _salida = objRespControlParqueadero;
                this.MontarIngresoParaSalida();
            }
            else
            {
                MessageBox.Show("No se encontro registro", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private async void BuscarPorPlaca(string Placa)
        {
            var objRespControlParqueadero = await FormBase<ControlParqueadero>.GetAsync<ControlParqueadero>($"ControlParqueadero/GetIngresoByPlaca/{Placa}");
            if (objRespControlParqueadero != null && !string.IsNullOrEmpty(objRespControlParqueadero.Placa))
            {
                _salida = objRespControlParqueadero;
                this.MontarIngresoParaSalida();
            }
            else
            {
                MessageBox.Show("No se encontro registro", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private async void MontarIngresoParaSalida()
        {
            var listaTrifa = await FormBase<List<TarifasParqueadero>>.GetAsync<List<TarifasParqueadero>>($"TarifasParqueadero/ObtenerTarifaPorIdTipoVehiculo/{_salida.IdTipoVehiculo}");
            textBoxPlacaSalida.Text = _salida.Placa;
            comboBoxTipoVehiculoSalida.SelectedValue = _salida.IdTipoVehiculo;
            textBoxFechaHoraIngresoSalida.Text = _salida.FechaHoraIngreso.ToString("dd/MM/yyyy HH:mm");
            textBoxUsuarioIngresoSalida.Text = _salida.CodUsuarioIngreso.ToString();
            comboBoxTarifa.DataSource = listaTrifa.OrderBy(M => M.Id).ToList();
            comboBoxTarifa.DisplayMember = "Nombre";
            comboBoxTarifa.ValueMember = "Id";
            comboBoxTarifa.SelectedValue = -1;
            comboBoxTarifa.Refresh();
            //comboBoxTarifa
            //textBoxValor
            textBoxFechaHoraSalida.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            textBoxUsurioSalida.Text = _usuario.NombreUsuario;

            groupBoxSalida.Enabled = true;
            groupBoxSalida.Text = "Salida - Ticket: " + _salida.Id.ToString();
            textBoxTicket.Text = string.Empty;
            textBoxPlacaBusqueda.Text = string.Empty;
        }

        private void buttonCancelarIngreso_Click(object sender, EventArgs e)
        {
            try
            {
                _salida = null;
                groupBoxSalida.Enabled = false;
                groupBoxSalida.Text = "Salida";
                textBoxPlacaSalida.Text = string.Empty;
                comboBoxTipoVehiculoSalida.SelectedValue = -1;
                textBoxFechaHoraIngresoSalida.Text = string.Empty;
                textBoxUsuarioIngresoSalida.Text = string.Empty;
                comboBoxTarifa.DataSource = null;
                comboBoxTarifa.Refresh();
                textBoxValor.Text = string.Empty;
                textBoxFechaHoraSalida.Text = string.Empty;
                textBoxUsurioSalida.Text = string.Empty;
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void comboBoxTarifa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (groupBoxSalida.Enabled)
                {
                    if (!string.IsNullOrEmpty(comboBoxTarifa.SelectedValue.ToString()))
                    {
                        //_salida.IdTarifa = int.Parse(comboBoxTarifa.SelectedValue.ToString());
                        Calcular();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void Calcular()
        {
            //var objRespControlParqueadero = await FormBase<ControlParqueadero>.GetAsync<ControlParqueadero>($"ControlParqueadero/CalcularSalida/{_salida}");
            var objRespControlParqueadero = await FormBase<ControlParqueadero>.PostAsync<ControlParqueadero>("ControlParqueadero/CalcularSalida", _salida);
            if (objRespControlParqueadero != null && !string.IsNullOrEmpty(objRespControlParqueadero.Placa))
            {
                textBoxFechaHoraSalida.Text = objRespControlParqueadero.FechaHoraSalida.Value.ToString("dd/MM/yyyy HH:mm");
                //textBoxValor.Text = string.Format("{0:#0.00}", objRespControlParqueadero.ValorPago.Value);
                _salida = objRespControlParqueadero;
            }
        }

        private void buttonSalida_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidarSalida())
                {
                    _salida.CodUsuarioSalida = _usuario.Id;
                    this.RegistrarSalida();
                }
                else
                {
                    MessageBox.Show("Datos incompletos", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void RegistrarSalida()
        {
            var objRespControlParqueadero = await FormBase<ControlParqueadero>.PutAsync("ControlParqueadero/Update", _salida);
            if (objRespControlParqueadero)
            {
                MessageBox.Show("Salida registrada con exito", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
