namespace CorParques.Presentacion.WinForms
{
    partial class FormMain
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
            this.btn_Notificaciones = new System.Windows.Forms.Button();
            this.btn_Centro_Medico = new System.Windows.Forms.Button();
            this.btn_Control_Parqueadero = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_Notificaciones
            // 
            this.btn_Notificaciones.Location = new System.Drawing.Point(21, 23);
            this.btn_Notificaciones.Name = "btn_Notificaciones";
            this.btn_Notificaciones.Size = new System.Drawing.Size(168, 75);
            this.btn_Notificaciones.TabIndex = 0;
            this.btn_Notificaciones.Text = "Notificaciones";
            this.btn_Notificaciones.UseVisualStyleBackColor = true;
            this.btn_Notificaciones.Click += new System.EventHandler(this.btn_Notificaciones_Click);
            // 
            // btn_Centro_Medico
            // 
            this.btn_Centro_Medico.Location = new System.Drawing.Point(209, 23);
            this.btn_Centro_Medico.Name = "btn_Centro_Medico";
            this.btn_Centro_Medico.Size = new System.Drawing.Size(168, 75);
            this.btn_Centro_Medico.TabIndex = 1;
            this.btn_Centro_Medico.Text = "Centro Medico";
            this.btn_Centro_Medico.UseVisualStyleBackColor = true;
            this.btn_Centro_Medico.Click += new System.EventHandler(this.btn_Centro_Medico_Click);
            // 
            // btn_Control_Parqueadero
            // 
            this.btn_Control_Parqueadero.Location = new System.Drawing.Point(21, 118);
            this.btn_Control_Parqueadero.Name = "btn_Control_Parqueadero";
            this.btn_Control_Parqueadero.Size = new System.Drawing.Size(168, 75);
            this.btn_Control_Parqueadero.TabIndex = 2;
            this.btn_Control_Parqueadero.Text = "Control Parqueadero";
            this.btn_Control_Parqueadero.UseVisualStyleBackColor = true;
            this.btn_Control_Parqueadero.Click += new System.EventHandler(this.btn_Control_Parqueadero_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 235);
            this.Controls.Add(this.btn_Control_Parqueadero);
            this.Controls.Add(this.btn_Centro_Medico);
            this.Controls.Add(this.btn_Notificaciones);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Formulario principal";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Notificaciones;
        private System.Windows.Forms.Button btn_Centro_Medico;
        private System.Windows.Forms.Button btn_Control_Parqueadero;
    }
}