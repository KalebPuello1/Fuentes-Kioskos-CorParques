namespace CorParques.Presentacion.WinForms
{
    partial class FormCentroMedico
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
            this.gbxDatos = new System.Windows.Forms.GroupBox();
            this.btnGuardarPaciente = new System.Windows.Forms.Button();
            this.tbxCorreoPaciente = new System.Windows.Forms.TextBox();
            this.tbxTelAcudientePaciente = new System.Windows.Forms.TextBox();
            this.tbxTelPaciente = new System.Windows.Forms.TextBox();
            this.tbxAcudientePaciente = new System.Windows.Forms.TextBox();
            this.tbxApellidoPaciente = new System.Windows.Forms.TextBox();
            this.tbxNombrePaciente = new System.Windows.Forms.TextBox();
            this.tbxCedulaPaciente = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbxPacientes = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbxProcedimeinto = new System.Windows.Forms.GroupBox();
            this.tbxRecomendacionesPaciente = new System.Windows.Forms.TextBox();
            this.tbxTratamientoPaciente = new System.Windows.Forms.TextBox();
            this.tbxAlergiasPaciente = new System.Windows.Forms.TextBox();
            this.tbxSintomasPaciente = new System.Windows.Forms.TextBox();
            this.tbxCausasPaciente = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.gbxDatos.SuspendLayout();
            this.gbxProcedimeinto.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxDatos
            // 
            this.gbxDatos.Controls.Add(this.tbxCorreoPaciente);
            this.gbxDatos.Controls.Add(this.tbxTelAcudientePaciente);
            this.gbxDatos.Controls.Add(this.tbxTelPaciente);
            this.gbxDatos.Controls.Add(this.tbxAcudientePaciente);
            this.gbxDatos.Controls.Add(this.tbxApellidoPaciente);
            this.gbxDatos.Controls.Add(this.tbxNombrePaciente);
            this.gbxDatos.Controls.Add(this.tbxCedulaPaciente);
            this.gbxDatos.Controls.Add(this.label8);
            this.gbxDatos.Controls.Add(this.label7);
            this.gbxDatos.Controls.Add(this.label6);
            this.gbxDatos.Controls.Add(this.label5);
            this.gbxDatos.Controls.Add(this.label4);
            this.gbxDatos.Controls.Add(this.label3);
            this.gbxDatos.Controls.Add(this.label2);
            this.gbxDatos.Controls.Add(this.lbxPacientes);
            this.gbxDatos.Controls.Add(this.label1);
            this.gbxDatos.ForeColor = System.Drawing.Color.Red;
            this.gbxDatos.Location = new System.Drawing.Point(12, 12);
            this.gbxDatos.Name = "gbxDatos";
            this.gbxDatos.Size = new System.Drawing.Size(315, 560);
            this.gbxDatos.TabIndex = 0;
            this.gbxDatos.TabStop = false;
            this.gbxDatos.Text = "Datos del Paciente";
            // 
            // btnGuardarPaciente
            // 
            this.btnGuardarPaciente.ForeColor = System.Drawing.Color.Black;
            this.btnGuardarPaciente.Location = new System.Drawing.Point(9, 524);
            this.btnGuardarPaciente.Name = "btnGuardarPaciente";
            this.btnGuardarPaciente.Size = new System.Drawing.Size(502, 23);
            this.btnGuardarPaciente.TabIndex = 11;
            this.btnGuardarPaciente.Text = "Guardar";
            this.btnGuardarPaciente.UseVisualStyleBackColor = true;
            this.btnGuardarPaciente.Click += new System.EventHandler(this.btnGuardarPaciente_Click);
            // 
            // tbxCorreoPaciente
            // 
            this.tbxCorreoPaciente.Location = new System.Drawing.Point(9, 526);
            this.tbxCorreoPaciente.MaxLength = 50;
            this.tbxCorreoPaciente.Name = "tbxCorreoPaciente";
            this.tbxCorreoPaciente.Size = new System.Drawing.Size(297, 20);
            this.tbxCorreoPaciente.TabIndex = 10;
            // 
            // tbxTelAcudientePaciente
            // 
            this.tbxTelAcudientePaciente.Location = new System.Drawing.Point(9, 482);
            this.tbxTelAcudientePaciente.MaxLength = 50;
            this.tbxTelAcudientePaciente.Name = "tbxTelAcudientePaciente";
            this.tbxTelAcudientePaciente.Size = new System.Drawing.Size(297, 20);
            this.tbxTelAcudientePaciente.TabIndex = 9;
            // 
            // tbxTelPaciente
            // 
            this.tbxTelPaciente.Location = new System.Drawing.Point(9, 438);
            this.tbxTelPaciente.MaxLength = 50;
            this.tbxTelPaciente.Name = "tbxTelPaciente";
            this.tbxTelPaciente.Size = new System.Drawing.Size(297, 20);
            this.tbxTelPaciente.TabIndex = 8;
            // 
            // tbxAcudientePaciente
            // 
            this.tbxAcudientePaciente.Location = new System.Drawing.Point(9, 394);
            this.tbxAcudientePaciente.MaxLength = 50;
            this.tbxAcudientePaciente.Name = "tbxAcudientePaciente";
            this.tbxAcudientePaciente.Size = new System.Drawing.Size(297, 20);
            this.tbxAcudientePaciente.TabIndex = 7;
            // 
            // tbxApellidoPaciente
            // 
            this.tbxApellidoPaciente.Location = new System.Drawing.Point(9, 350);
            this.tbxApellidoPaciente.MaxLength = 50;
            this.tbxApellidoPaciente.Name = "tbxApellidoPaciente";
            this.tbxApellidoPaciente.Size = new System.Drawing.Size(297, 20);
            this.tbxApellidoPaciente.TabIndex = 6;
            // 
            // tbxNombrePaciente
            // 
            this.tbxNombrePaciente.Location = new System.Drawing.Point(9, 305);
            this.tbxNombrePaciente.MaxLength = 50;
            this.tbxNombrePaciente.Name = "tbxNombrePaciente";
            this.tbxNombrePaciente.Size = new System.Drawing.Size(297, 20);
            this.tbxNombrePaciente.TabIndex = 5;
            // 
            // tbxCedulaPaciente
            // 
            this.tbxCedulaPaciente.Location = new System.Drawing.Point(9, 260);
            this.tbxCedulaPaciente.MaxLength = 10;
            this.tbxCedulaPaciente.Name = "tbxCedulaPaciente";
            this.tbxCedulaPaciente.Size = new System.Drawing.Size(297, 20);
            this.tbxCedulaPaciente.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(6, 510);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "E-mail";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(6, 466);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Tel. Acudiente";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(6, 422);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Tel. Paciente";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(6, 378);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Acudiente";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(6, 334);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Apellido";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(6, 289);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Nombre";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(6, 242);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Cédula";
            // 
            // lbxPacientes
            // 
            this.lbxPacientes.FormattingEnabled = true;
            this.lbxPacientes.Location = new System.Drawing.Point(9, 45);
            this.lbxPacientes.Name = "lbxPacientes";
            this.lbxPacientes.Size = new System.Drawing.Size(297, 186);
            this.lbxPacientes.TabIndex = 3;
            this.lbxPacientes.DoubleClick += new System.EventHandler(this.lbxPacientes_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lista de Pacientes";
            // 
            // gbxProcedimeinto
            // 
            this.gbxProcedimeinto.Controls.Add(this.btnGuardarPaciente);
            this.gbxProcedimeinto.Controls.Add(this.tbxRecomendacionesPaciente);
            this.gbxProcedimeinto.Controls.Add(this.tbxTratamientoPaciente);
            this.gbxProcedimeinto.Controls.Add(this.tbxAlergiasPaciente);
            this.gbxProcedimeinto.Controls.Add(this.tbxSintomasPaciente);
            this.gbxProcedimeinto.Controls.Add(this.tbxCausasPaciente);
            this.gbxProcedimeinto.Controls.Add(this.label13);
            this.gbxProcedimeinto.Controls.Add(this.label12);
            this.gbxProcedimeinto.Controls.Add(this.label11);
            this.gbxProcedimeinto.Controls.Add(this.label10);
            this.gbxProcedimeinto.Controls.Add(this.label9);
            this.gbxProcedimeinto.ForeColor = System.Drawing.Color.Red;
            this.gbxProcedimeinto.Location = new System.Drawing.Point(333, 12);
            this.gbxProcedimeinto.Name = "gbxProcedimeinto";
            this.gbxProcedimeinto.Size = new System.Drawing.Size(517, 560);
            this.gbxProcedimeinto.TabIndex = 0;
            this.gbxProcedimeinto.TabStop = false;
            this.gbxProcedimeinto.Text = "Procedimiento";
            // 
            // tbxRecomendacionesPaciente
            // 
            this.tbxRecomendacionesPaciente.Location = new System.Drawing.Point(9, 433);
            this.tbxRecomendacionesPaciente.Multiline = true;
            this.tbxRecomendacionesPaciente.Name = "tbxRecomendacionesPaciente";
            this.tbxRecomendacionesPaciente.Size = new System.Drawing.Size(502, 72);
            this.tbxRecomendacionesPaciente.TabIndex = 15;
            // 
            // tbxTratamientoPaciente
            // 
            this.tbxTratamientoPaciente.Location = new System.Drawing.Point(9, 337);
            this.tbxTratamientoPaciente.Multiline = true;
            this.tbxTratamientoPaciente.Name = "tbxTratamientoPaciente";
            this.tbxTratamientoPaciente.Size = new System.Drawing.Size(502, 72);
            this.tbxTratamientoPaciente.TabIndex = 14;
            // 
            // tbxAlergiasPaciente
            // 
            this.tbxAlergiasPaciente.Location = new System.Drawing.Point(9, 240);
            this.tbxAlergiasPaciente.Multiline = true;
            this.tbxAlergiasPaciente.Name = "tbxAlergiasPaciente";
            this.tbxAlergiasPaciente.Size = new System.Drawing.Size(502, 72);
            this.tbxAlergiasPaciente.TabIndex = 16;
            // 
            // tbxSintomasPaciente
            // 
            this.tbxSintomasPaciente.Location = new System.Drawing.Point(9, 143);
            this.tbxSintomasPaciente.Multiline = true;
            this.tbxSintomasPaciente.Name = "tbxSintomasPaciente";
            this.tbxSintomasPaciente.Size = new System.Drawing.Size(502, 72);
            this.tbxSintomasPaciente.TabIndex = 13;
            // 
            // tbxCausasPaciente
            // 
            this.tbxCausasPaciente.Location = new System.Drawing.Point(9, 45);
            this.tbxCausasPaciente.Multiline = true;
            this.tbxCausasPaciente.Name = "tbxCausasPaciente";
            this.tbxCausasPaciente.Size = new System.Drawing.Size(502, 72);
            this.tbxCausasPaciente.TabIndex = 12;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(6, 417);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(96, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Recomendaciones";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(6, 321);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Tratamiento";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(6, 224);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Alergias";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(6, 127);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Síntomas";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(6, 29);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Causas";
            // 
            // FormCentroMedico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(862, 584);
            this.Controls.Add(this.gbxProcedimeinto);
            this.Controls.Add(this.gbxDatos);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCentroMedico";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Centro Médico";
            this.Load += new System.EventHandler(this.FormCentroMedico_Load);
            this.gbxDatos.ResumeLayout(false);
            this.gbxDatos.PerformLayout();
            this.gbxProcedimeinto.ResumeLayout(false);
            this.gbxProcedimeinto.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxDatos;
        private System.Windows.Forms.GroupBox gbxProcedimeinto;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbxPacientes;
        private System.Windows.Forms.Button btnGuardarPaciente;
        private System.Windows.Forms.TextBox tbxCorreoPaciente;
        private System.Windows.Forms.TextBox tbxTelAcudientePaciente;
        private System.Windows.Forms.TextBox tbxTelPaciente;
        private System.Windows.Forms.TextBox tbxAcudientePaciente;
        private System.Windows.Forms.TextBox tbxApellidoPaciente;
        private System.Windows.Forms.TextBox tbxNombrePaciente;
        private System.Windows.Forms.TextBox tbxCedulaPaciente;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxRecomendacionesPaciente;
        private System.Windows.Forms.TextBox tbxTratamientoPaciente;
        private System.Windows.Forms.TextBox tbxAlergiasPaciente;
        private System.Windows.Forms.TextBox tbxSintomasPaciente;
        private System.Windows.Forms.TextBox tbxCausasPaciente;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
    }
}