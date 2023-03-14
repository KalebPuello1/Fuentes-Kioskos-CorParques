namespace CorParques.Presentacion.WinForms
{
    partial class FormParqueaderoIngreso
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
            this.groupBoxIngreso = new System.Windows.Forms.GroupBox();
            this.buttonIngresar = new System.Windows.Forms.Button();
            this.labelPlaca = new System.Windows.Forms.Label();
            this.textBoxPlacaIngreso = new System.Windows.Forms.TextBox();
            this.listViewTipoVehiculo = new System.Windows.Forms.ListView();
            this.groupBoxIngreso.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxIngreso
            // 
            this.groupBoxIngreso.Controls.Add(this.listViewTipoVehiculo);
            this.groupBoxIngreso.Controls.Add(this.buttonIngresar);
            this.groupBoxIngreso.Controls.Add(this.labelPlaca);
            this.groupBoxIngreso.Controls.Add(this.textBoxPlacaIngreso);
            this.groupBoxIngreso.Location = new System.Drawing.Point(12, 12);
            this.groupBoxIngreso.Name = "groupBoxIngreso";
            this.groupBoxIngreso.Size = new System.Drawing.Size(460, 338);
            this.groupBoxIngreso.TabIndex = 1;
            this.groupBoxIngreso.TabStop = false;
            // 
            // buttonIngresar
            // 
            this.buttonIngresar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonIngresar.ForeColor = System.Drawing.Color.Green;
            this.buttonIngresar.Location = new System.Drawing.Point(18, 157);
            this.buttonIngresar.Name = "buttonIngresar";
            this.buttonIngresar.Size = new System.Drawing.Size(121, 44);
            this.buttonIngresar.TabIndex = 4;
            this.buttonIngresar.Text = "Ingresar";
            this.buttonIngresar.UseVisualStyleBackColor = true;
            
            // 
            // labelPlaca
            // 
            this.labelPlaca.AutoSize = true;
            this.labelPlaca.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlaca.Location = new System.Drawing.Point(57, 91);
            this.labelPlaca.Name = "labelPlaca";
            this.labelPlaca.Size = new System.Drawing.Size(43, 16);
            this.labelPlaca.TabIndex = 1;
            this.labelPlaca.Text = "Placa";
            // 
            // textBoxPlacaIngreso
            // 
            this.textBoxPlacaIngreso.BackColor = System.Drawing.Color.Yellow;
            this.textBoxPlacaIngreso.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxPlacaIngreso.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPlacaIngreso.Location = new System.Drawing.Point(18, 110);
            this.textBoxPlacaIngreso.MaxLength = 10;
            this.textBoxPlacaIngreso.Name = "textBoxPlacaIngreso";
            this.textBoxPlacaIngreso.Size = new System.Drawing.Size(121, 26);
            this.textBoxPlacaIngreso.TabIndex = 0;
            this.textBoxPlacaIngreso.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // listViewTipoVehiculo
            // 
            this.listViewTipoVehiculo.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewTipoVehiculo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewTipoVehiculo.FullRowSelect = true;
            this.listViewTipoVehiculo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewTipoVehiculo.HideSelection = false;
            this.listViewTipoVehiculo.Location = new System.Drawing.Point(178, 19);
            this.listViewTipoVehiculo.MultiSelect = false;
            this.listViewTipoVehiculo.Name = "listViewTipoVehiculo";
            this.listViewTipoVehiculo.Size = new System.Drawing.Size(254, 313);
            this.listViewTipoVehiculo.TabIndex = 14;
            this.listViewTipoVehiculo.UseCompatibleStateImageBehavior = false;
            this.listViewTipoVehiculo.View = System.Windows.Forms.View.List;
            // 
            // FormParqueaderoIngreso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 362);
            this.Controls.Add(this.groupBoxIngreso);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "FormParqueaderoIngreso";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ingreso al Parqueadero";
            this.groupBoxIngreso.ResumeLayout(false);
            this.groupBoxIngreso.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxIngreso;
        private System.Windows.Forms.Button buttonIngresar;
        private System.Windows.Forms.Label labelPlaca;
        private System.Windows.Forms.TextBox textBoxPlacaIngreso;
        private System.Windows.Forms.ListView listViewTipoVehiculo;
    }
}