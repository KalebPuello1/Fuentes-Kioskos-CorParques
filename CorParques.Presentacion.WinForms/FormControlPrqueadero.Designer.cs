namespace CorParques.Presentacion.WinForms
{
    partial class FormControlPrqueadero
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxSalida = new System.Windows.Forms.GroupBox();
            this.buttonCancelarIngreso = new System.Windows.Forms.Button();
            this.buttonSalida = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxUsurioSalida = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxFechaHoraSalida = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxValor = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBoxTarifa = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxUsuarioIngresoSalida = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxFechaHoraIngresoSalida = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxTipoVehiculoSalida = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxPlacaSalida = new System.Windows.Forms.TextBox();
            this.groupBoxBuscar = new System.Windows.Forms.GroupBox();
            this.buttonBuscar = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxTicket = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxPlacaBusqueda = new System.Windows.Forms.TextBox();
            this.groupBoxSalida.SuspendLayout();
            this.groupBoxBuscar.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSalida
            // 
            this.groupBoxSalida.Controls.Add(this.buttonCancelarIngreso);
            this.groupBoxSalida.Controls.Add(this.buttonSalida);
            this.groupBoxSalida.Controls.Add(this.label11);
            this.groupBoxSalida.Controls.Add(this.textBoxUsurioSalida);
            this.groupBoxSalida.Controls.Add(this.label12);
            this.groupBoxSalida.Controls.Add(this.textBoxFechaHoraSalida);
            this.groupBoxSalida.Controls.Add(this.label10);
            this.groupBoxSalida.Controls.Add(this.textBoxValor);
            this.groupBoxSalida.Controls.Add(this.label9);
            this.groupBoxSalida.Controls.Add(this.comboBoxTarifa);
            this.groupBoxSalida.Controls.Add(this.label3);
            this.groupBoxSalida.Controls.Add(this.textBoxUsuarioIngresoSalida);
            this.groupBoxSalida.Controls.Add(this.label4);
            this.groupBoxSalida.Controls.Add(this.textBoxFechaHoraIngresoSalida);
            this.groupBoxSalida.Controls.Add(this.label5);
            this.groupBoxSalida.Controls.Add(this.comboBoxTipoVehiculoSalida);
            this.groupBoxSalida.Controls.Add(this.label6);
            this.groupBoxSalida.Controls.Add(this.textBoxPlacaSalida);
            this.groupBoxSalida.Location = new System.Drawing.Point(22, 106);
            this.groupBoxSalida.Name = "groupBoxSalida";
            this.groupBoxSalida.Size = new System.Drawing.Size(759, 188);
            this.groupBoxSalida.TabIndex = 9;
            this.groupBoxSalida.TabStop = false;
            this.groupBoxSalida.Text = "Salida";
            // 
            // buttonCancelarIngreso
            // 
            this.buttonCancelarIngreso.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancelarIngreso.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.buttonCancelarIngreso.Location = new System.Drawing.Point(658, 109);
            this.buttonCancelarIngreso.Name = "buttonCancelarIngreso";
            this.buttonCancelarIngreso.Size = new System.Drawing.Size(89, 39);
            this.buttonCancelarIngreso.TabIndex = 10;
            this.buttonCancelarIngreso.Text = "Cancelar";
            this.buttonCancelarIngreso.UseVisualStyleBackColor = true;
            this.buttonCancelarIngreso.Click += new System.EventHandler(this.buttonCancelarIngreso_Click);
            // 
            // buttonSalida
            // 
            this.buttonSalida.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSalida.ForeColor = System.Drawing.Color.Green;
            this.buttonSalida.Location = new System.Drawing.Point(658, 47);
            this.buttonSalida.Name = "buttonSalida";
            this.buttonSalida.Size = new System.Drawing.Size(89, 55);
            this.buttonSalida.TabIndex = 9;
            this.buttonSalida.Text = "Registrar Salida";
            this.buttonSalida.UseVisualStyleBackColor = true;
            this.buttonSalida.Click += new System.EventHandler(this.buttonSalida_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(487, 100);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(97, 16);
            this.label11.TabIndex = 15;
            this.label11.Text = "Usuario Salida";
            // 
            // textBoxUsurioSalida
            // 
            this.textBoxUsurioSalida.Enabled = false;
            this.textBoxUsurioSalida.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxUsurioSalida.Location = new System.Drawing.Point(490, 119);
            this.textBoxUsurioSalida.Name = "textBoxUsurioSalida";
            this.textBoxUsurioSalida.Size = new System.Drawing.Size(153, 26);
            this.textBoxUsurioSalida.TabIndex = 14;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(304, 101);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(150, 16);
            this.label12.TabIndex = 13;
            this.label12.Text = "Fecha y Hora de Salida";
            // 
            // textBoxFechaHoraSalida
            // 
            this.textBoxFechaHoraSalida.Enabled = false;
            this.textBoxFechaHoraSalida.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxFechaHoraSalida.Location = new System.Drawing.Point(307, 120);
            this.textBoxFechaHoraSalida.Name = "textBoxFechaHoraSalida";
            this.textBoxFechaHoraSalida.Size = new System.Drawing.Size(153, 26);
            this.textBoxFechaHoraSalida.TabIndex = 12;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(154, 101);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 16);
            this.label10.TabIndex = 11;
            this.label10.Text = "Valor";
            // 
            // textBoxValor
            // 
            this.textBoxValor.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxValor.Enabled = false;
            this.textBoxValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxValor.Location = new System.Drawing.Point(157, 120);
            this.textBoxValor.Name = "textBoxValor";
            this.textBoxValor.Size = new System.Drawing.Size(121, 26);
            this.textBoxValor.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(8, 101);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 16);
            this.label9.TabIndex = 9;
            this.label9.Text = "Tarifa";
            // 
            // comboBoxTarifa
            // 
            this.comboBoxTarifa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTarifa.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxTarifa.FormattingEnabled = true;
            this.comboBoxTarifa.Location = new System.Drawing.Point(11, 120);
            this.comboBoxTarifa.Name = "comboBoxTarifa";
            this.comboBoxTarifa.Size = new System.Drawing.Size(121, 28);
            this.comboBoxTarifa.TabIndex = 8;
            this.comboBoxTarifa.SelectedIndexChanged += new System.EventHandler(this.comboBoxTarifa_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(486, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Usuario Ingreso";
            // 
            // textBoxUsuarioIngresoSalida
            // 
            this.textBoxUsuarioIngresoSalida.Enabled = false;
            this.textBoxUsuarioIngresoSalida.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxUsuarioIngresoSalida.Location = new System.Drawing.Point(489, 50);
            this.textBoxUsuarioIngresoSalida.Name = "textBoxUsuarioIngresoSalida";
            this.textBoxUsuarioIngresoSalida.Size = new System.Drawing.Size(153, 26);
            this.textBoxUsuarioIngresoSalida.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(303, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Fecha y Hora de Ingreso";
            // 
            // textBoxFechaHoraIngresoSalida
            // 
            this.textBoxFechaHoraIngresoSalida.Enabled = false;
            this.textBoxFechaHoraIngresoSalida.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxFechaHoraIngresoSalida.Location = new System.Drawing.Point(306, 51);
            this.textBoxFechaHoraIngresoSalida.Name = "textBoxFechaHoraIngresoSalida";
            this.textBoxFechaHoraIngresoSalida.Size = new System.Drawing.Size(153, 26);
            this.textBoxFechaHoraIngresoSalida.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(153, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "Tipo de Vehiculo";
            // 
            // comboBoxTipoVehiculoSalida
            // 
            this.comboBoxTipoVehiculoSalida.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTipoVehiculoSalida.Enabled = false;
            this.comboBoxTipoVehiculoSalida.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxTipoVehiculoSalida.FormattingEnabled = true;
            this.comboBoxTipoVehiculoSalida.Location = new System.Drawing.Point(156, 51);
            this.comboBoxTipoVehiculoSalida.Name = "comboBoxTipoVehiculoSalida";
            this.comboBoxTipoVehiculoSalida.Size = new System.Drawing.Size(121, 28);
            this.comboBoxTipoVehiculoSalida.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(7, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 16);
            this.label6.TabIndex = 1;
            this.label6.Text = "Placa";
            // 
            // textBoxPlacaSalida
            // 
            this.textBoxPlacaSalida.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxPlacaSalida.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxPlacaSalida.Enabled = false;
            this.textBoxPlacaSalida.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPlacaSalida.Location = new System.Drawing.Point(10, 51);
            this.textBoxPlacaSalida.Name = "textBoxPlacaSalida";
            this.textBoxPlacaSalida.Size = new System.Drawing.Size(121, 26);
            this.textBoxPlacaSalida.TabIndex = 0;
            this.textBoxPlacaSalida.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBoxBuscar
            // 
            this.groupBoxBuscar.Controls.Add(this.buttonBuscar);
            this.groupBoxBuscar.Controls.Add(this.label8);
            this.groupBoxBuscar.Controls.Add(this.textBoxTicket);
            this.groupBoxBuscar.Controls.Add(this.label7);
            this.groupBoxBuscar.Controls.Add(this.textBoxPlacaBusqueda);
            this.groupBoxBuscar.Location = new System.Drawing.Point(12, 12);
            this.groupBoxBuscar.Name = "groupBoxBuscar";
            this.groupBoxBuscar.Size = new System.Drawing.Size(408, 88);
            this.groupBoxBuscar.TabIndex = 10;
            this.groupBoxBuscar.TabStop = false;
            this.groupBoxBuscar.Text = "Busqueda";
            // 
            // buttonBuscar
            // 
            this.buttonBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBuscar.Location = new System.Drawing.Point(306, 44);
            this.buttonBuscar.Name = "buttonBuscar";
            this.buttonBuscar.Size = new System.Drawing.Size(75, 26);
            this.buttonBuscar.TabIndex = 6;
            this.buttonBuscar.Text = "Buscar";
            this.buttonBuscar.UseVisualStyleBackColor = true;
            this.buttonBuscar.Click += new System.EventHandler(this.buttonBuscar_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(7, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 16);
            this.label8.TabIndex = 5;
            this.label8.Text = "Por Ticket";
            // 
            // textBoxTicket
            // 
            this.textBoxTicket.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxTicket.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTicket.Location = new System.Drawing.Point(10, 44);
            this.textBoxTicket.Name = "textBoxTicket";
            this.textBoxTicket.Size = new System.Drawing.Size(121, 26);
            this.textBoxTicket.TabIndex = 5;
            this.textBoxTicket.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxTicket.TextChanged += new System.EventHandler(this.textBoxTicket_TextChanged);
            this.textBoxTicket.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxTicket_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(153, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 16);
            this.label7.TabIndex = 3;
            this.label7.Text = "Por Placa";
            // 
            // textBoxPlacaBusqueda
            // 
            this.textBoxPlacaBusqueda.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxPlacaBusqueda.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxPlacaBusqueda.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPlacaBusqueda.Location = new System.Drawing.Point(156, 44);
            this.textBoxPlacaBusqueda.Name = "textBoxPlacaBusqueda";
            this.textBoxPlacaBusqueda.Size = new System.Drawing.Size(121, 26);
            this.textBoxPlacaBusqueda.TabIndex = 5;
            this.textBoxPlacaBusqueda.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FormControlPrqueadero
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 558);
            this.Controls.Add(this.groupBoxBuscar);
            this.Controls.Add(this.groupBoxSalida);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "FormControlPrqueadero";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormControlPrqueadero";
            this.Load += new System.EventHandler(this.FormControlPrqueadero_Load);
            this.groupBoxSalida.ResumeLayout(false);
            this.groupBoxSalida.PerformLayout();
            this.groupBoxBuscar.ResumeLayout(false);
            this.groupBoxBuscar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxSalida;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxUsuarioIngresoSalida;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxFechaHoraIngresoSalida;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxTipoVehiculoSalida;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxPlacaSalida;
        private System.Windows.Forms.GroupBox groupBoxBuscar;
        private System.Windows.Forms.Button buttonBuscar;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxTicket;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxPlacaBusqueda;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxValor;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBoxTarifa;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxUsurioSalida;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxFechaHoraSalida;
        private System.Windows.Forms.Button buttonSalida;
        private System.Windows.Forms.Button buttonCancelarIngreso;
    }
}