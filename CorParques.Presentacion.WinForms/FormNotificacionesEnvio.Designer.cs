namespace CorParques.Presentacion.WinForms
{
    partial class FormNotificacionesEnvio
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
            this.grb_Grupos = new System.Windows.Forms.GroupBox();
            this.lbxGrupos = new System.Windows.Forms.ListBox();
            this.Correo = new System.Windows.Forms.GroupBox();
            this.btnAdicionarCorreo = new System.Windows.Forms.Button();
            this.lbl_Correo_Adicional = new System.Windows.Forms.Label();
            this.txtMail = new System.Windows.Forms.TextBox();
            this.lbxMails = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.grb_Mensaje = new System.Windows.Forms.GroupBox();
            this.txtContenido = new System.Windows.Forms.TextBox();
            this.grb_Enviar = new System.Windows.Forms.GroupBox();
            this.lbl_Texto = new System.Windows.Forms.Label();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.grb_Prioridad = new System.Windows.Forms.GroupBox();
            this.radBaja = new System.Windows.Forms.RadioButton();
            this.radAlta = new System.Windows.Forms.RadioButton();
            this.radNormal = new System.Windows.Forms.RadioButton();
            this.grb_Grupo_Asunto = new System.Windows.Forms.GroupBox();
            this.txtAsunto = new System.Windows.Forms.TextBox();
            this.splc_ContenedorPrincipal = new System.Windows.Forms.SplitContainer();
            this.spl_ContenedorSecundario = new System.Windows.Forms.SplitContainer();
            this.grb_Grupos.SuspendLayout();
            this.Correo.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grb_Mensaje.SuspendLayout();
            this.grb_Enviar.SuspendLayout();
            this.grb_Prioridad.SuspendLayout();
            this.grb_Grupo_Asunto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splc_ContenedorPrincipal)).BeginInit();
            this.splc_ContenedorPrincipal.Panel1.SuspendLayout();
            this.splc_ContenedorPrincipal.Panel2.SuspendLayout();
            this.splc_ContenedorPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spl_ContenedorSecundario)).BeginInit();
            this.spl_ContenedorSecundario.Panel1.SuspendLayout();
            this.spl_ContenedorSecundario.Panel2.SuspendLayout();
            this.spl_ContenedorSecundario.SuspendLayout();
            this.SuspendLayout();
            // 
            // grb_Grupos
            // 
            this.grb_Grupos.Controls.Add(this.lbxGrupos);
            this.grb_Grupos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grb_Grupos.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grb_Grupos.Location = new System.Drawing.Point(0, 0);
            this.grb_Grupos.Name = "grb_Grupos";
            this.grb_Grupos.Size = new System.Drawing.Size(347, 234);
            this.grb_Grupos.TabIndex = 0;
            this.grb_Grupos.TabStop = false;
            this.grb_Grupos.Text = "Grupos";
            // 
            // lbxGrupos
            // 
            this.lbxGrupos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxGrupos.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbxGrupos.FormattingEnabled = true;
            this.lbxGrupos.ItemHeight = 14;
            this.lbxGrupos.Location = new System.Drawing.Point(3, 20);
            this.lbxGrupos.Name = "lbxGrupos";
            this.lbxGrupos.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbxGrupos.Size = new System.Drawing.Size(341, 211);
            this.lbxGrupos.TabIndex = 0;
            // 
            // Correo
            // 
            this.Correo.Controls.Add(this.btnAdicionarCorreo);
            this.Correo.Controls.Add(this.lbl_Correo_Adicional);
            this.Correo.Controls.Add(this.txtMail);
            this.Correo.Controls.Add(this.lbxMails);
            this.Correo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Correo.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Correo.Location = new System.Drawing.Point(0, 0);
            this.Correo.Name = "Correo";
            this.Correo.Size = new System.Drawing.Size(347, 224);
            this.Correo.TabIndex = 0;
            this.Correo.TabStop = false;
            this.Correo.Text = "Correos Adicionales";
            // 
            // btnAdicionarCorreo
            // 
            this.btnAdicionarCorreo.Location = new System.Drawing.Point(274, 57);
            this.btnAdicionarCorreo.Name = "btnAdicionarCorreo";
            this.btnAdicionarCorreo.Size = new System.Drawing.Size(67, 24);
            this.btnAdicionarCorreo.TabIndex = 2;
            this.btnAdicionarCorreo.Text = "+";
            this.btnAdicionarCorreo.UseVisualStyleBackColor = true;
            this.btnAdicionarCorreo.Click += new System.EventHandler(this.btnAdicionarCorreo_Click);
            // 
            // lbl_Correo_Adicional
            // 
            this.lbl_Correo_Adicional.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Correo_Adicional.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Correo_Adicional.Location = new System.Drawing.Point(1, 20);
            this.lbl_Correo_Adicional.Name = "lbl_Correo_Adicional";
            this.lbl_Correo_Adicional.Size = new System.Drawing.Size(340, 37);
            this.lbl_Correo_Adicional.TabIndex = 0;
            this.lbl_Correo_Adicional.Text = "Digite el correo al que desea enviar la notificación y presione la tecla Enter pa" +
    "ra adicionarlo a la lista o use el botón +";
            // 
            // txtMail
            // 
            this.txtMail.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMail.Location = new System.Drawing.Point(6, 59);
            this.txtMail.MaxLength = 100;
            this.txtMail.Name = "txtMail";
            this.txtMail.Size = new System.Drawing.Size(263, 22);
            this.txtMail.TabIndex = 1;
            this.txtMail.TextChanged += new System.EventHandler(this.txtMail_TextChanged);
            this.txtMail.Enter += new System.EventHandler(this.txtMail_Enter);
            this.txtMail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMail_KeyPress);
            this.txtMail.Leave += new System.EventHandler(this.txtMail_Leave);
            // 
            // lbxMails
            // 
            this.lbxMails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbxMails.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbxMails.FormattingEnabled = true;
            this.lbxMails.ItemHeight = 14;
            this.lbxMails.Location = new System.Drawing.Point(4, 86);
            this.lbxMails.Name = "lbxMails";
            this.lbxMails.Size = new System.Drawing.Size(340, 130);
            this.lbxMails.TabIndex = 3;
            this.lbxMails.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lbxMails_KeyPress);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.grb_Mensaje);
            this.groupBox3.Controls.Add(this.grb_Enviar);
            this.groupBox3.Controls.Add(this.grb_Prioridad);
            this.groupBox3.Controls.Add(this.grb_Grupo_Asunto);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(433, 462);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Contenido de la notificación";
            // 
            // grb_Mensaje
            // 
            this.grb_Mensaje.Controls.Add(this.txtContenido);
            this.grb_Mensaje.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grb_Mensaje.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grb_Mensaje.Location = new System.Drawing.Point(3, 127);
            this.grb_Mensaje.Name = "grb_Mensaje";
            this.grb_Mensaje.Size = new System.Drawing.Size(427, 248);
            this.grb_Mensaje.TabIndex = 15;
            this.grb_Mensaje.TabStop = false;
            this.grb_Mensaje.Text = "Mensaje:";
            // 
            // txtContenido
            // 
            this.txtContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContenido.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContenido.Location = new System.Drawing.Point(3, 18);
            this.txtContenido.MaxLength = 3000;
            this.txtContenido.Multiline = true;
            this.txtContenido.Name = "txtContenido";
            this.txtContenido.Size = new System.Drawing.Size(421, 227);
            this.txtContenido.TabIndex = 7;
            this.txtContenido.TextChanged += new System.EventHandler(this.txtContenido_TextChanged);
            this.txtContenido.Enter += new System.EventHandler(this.txtContenido_Enter);
            this.txtContenido.Leave += new System.EventHandler(this.txtContenido_Leave);
            // 
            // grb_Enviar
            // 
            this.grb_Enviar.Controls.Add(this.lbl_Texto);
            this.grb_Enviar.Controls.Add(this.btnEnviar);
            this.grb_Enviar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grb_Enviar.Location = new System.Drawing.Point(3, 375);
            this.grb_Enviar.Name = "grb_Enviar";
            this.grb_Enviar.Size = new System.Drawing.Size(427, 84);
            this.grb_Enviar.TabIndex = 14;
            this.grb_Enviar.TabStop = false;
            // 
            // lbl_Texto
            // 
            this.lbl_Texto.AutoSize = true;
            this.lbl_Texto.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Texto.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lbl_Texto.Location = new System.Drawing.Point(3, 34);
            this.lbl_Texto.Name = "lbl_Texto";
            this.lbl_Texto.Size = new System.Drawing.Size(125, 24);
            this.lbl_Texto.TabIndex = 9;
            this.lbl_Texto.Text = "Cargando...";
            // 
            // btnEnviar
            // 
            this.btnEnviar.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEnviar.Enabled = false;
            this.btnEnviar.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnviar.Location = new System.Drawing.Point(295, 20);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(129, 61);
            this.btnEnviar.TabIndex = 8;
            this.btnEnviar.Text = "Enviar";
            this.btnEnviar.UseVisualStyleBackColor = true;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // grb_Prioridad
            // 
            this.grb_Prioridad.Controls.Add(this.radBaja);
            this.grb_Prioridad.Controls.Add(this.radAlta);
            this.grb_Prioridad.Controls.Add(this.radNormal);
            this.grb_Prioridad.Dock = System.Windows.Forms.DockStyle.Top;
            this.grb_Prioridad.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grb_Prioridad.Location = new System.Drawing.Point(3, 69);
            this.grb_Prioridad.Name = "grb_Prioridad";
            this.grb_Prioridad.Size = new System.Drawing.Size(427, 58);
            this.grb_Prioridad.TabIndex = 12;
            this.grb_Prioridad.TabStop = false;
            this.grb_Prioridad.Text = "Prioridad:";
            // 
            // radBaja
            // 
            this.radBaja.AutoSize = true;
            this.radBaja.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radBaja.Location = new System.Drawing.Point(318, 21);
            this.radBaja.Name = "radBaja";
            this.radBaja.Size = new System.Drawing.Size(57, 23);
            this.radBaja.TabIndex = 5;
            this.radBaja.TabStop = true;
            this.radBaja.Text = "Baja";
            this.radBaja.UseVisualStyleBackColor = true;
            this.radBaja.CheckedChanged += new System.EventHandler(this.radBaja_CheckedChanged);
            // 
            // radAlta
            // 
            this.radAlta.AutoSize = true;
            this.radAlta.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radAlta.Location = new System.Drawing.Point(28, 21);
            this.radAlta.Name = "radAlta";
            this.radAlta.Size = new System.Drawing.Size(55, 23);
            this.radAlta.TabIndex = 3;
            this.radAlta.TabStop = true;
            this.radAlta.Text = "Alta";
            this.radAlta.UseVisualStyleBackColor = true;
            this.radAlta.CheckedChanged += new System.EventHandler(this.radAlta_CheckedChanged);
            // 
            // radNormal
            // 
            this.radNormal.AutoSize = true;
            this.radNormal.Checked = true;
            this.radNormal.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radNormal.Location = new System.Drawing.Point(161, 21);
            this.radNormal.Name = "radNormal";
            this.radNormal.Size = new System.Drawing.Size(79, 23);
            this.radNormal.TabIndex = 4;
            this.radNormal.TabStop = true;
            this.radNormal.Text = "Normal";
            this.radNormal.UseVisualStyleBackColor = true;
            this.radNormal.CheckedChanged += new System.EventHandler(this.radNormal_CheckedChanged);
            // 
            // grb_Grupo_Asunto
            // 
            this.grb_Grupo_Asunto.Controls.Add(this.txtAsunto);
            this.grb_Grupo_Asunto.Dock = System.Windows.Forms.DockStyle.Top;
            this.grb_Grupo_Asunto.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grb_Grupo_Asunto.Location = new System.Drawing.Point(3, 20);
            this.grb_Grupo_Asunto.Name = "grb_Grupo_Asunto";
            this.grb_Grupo_Asunto.Size = new System.Drawing.Size(427, 49);
            this.grb_Grupo_Asunto.TabIndex = 10;
            this.grb_Grupo_Asunto.TabStop = false;
            this.grb_Grupo_Asunto.Text = "Asunto:";
            // 
            // txtAsunto
            // 
            this.txtAsunto.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAsunto.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAsunto.Location = new System.Drawing.Point(6, 21);
            this.txtAsunto.MaxLength = 100;
            this.txtAsunto.Name = "txtAsunto";
            this.txtAsunto.Size = new System.Drawing.Size(412, 22);
            this.txtAsunto.TabIndex = 1;
            this.txtAsunto.TextChanged += new System.EventHandler(this.txtAsunto_TextChanged);
            this.txtAsunto.Enter += new System.EventHandler(this.txtAsunto_Enter);
            this.txtAsunto.Leave += new System.EventHandler(this.txtAsunto_Leave);
            // 
            // splc_ContenedorPrincipal
            // 
            this.splc_ContenedorPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splc_ContenedorPrincipal.Location = new System.Drawing.Point(0, 0);
            this.splc_ContenedorPrincipal.Name = "splc_ContenedorPrincipal";
            // 
            // splc_ContenedorPrincipal.Panel1
            // 
            this.splc_ContenedorPrincipal.Panel1.Controls.Add(this.spl_ContenedorSecundario);
            // 
            // splc_ContenedorPrincipal.Panel2
            // 
            this.splc_ContenedorPrincipal.Panel2.Controls.Add(this.groupBox3);
            this.splc_ContenedorPrincipal.Size = new System.Drawing.Size(784, 462);
            this.splc_ContenedorPrincipal.SplitterDistance = 347;
            this.splc_ContenedorPrincipal.TabIndex = 5;
            // 
            // spl_ContenedorSecundario
            // 
            this.spl_ContenedorSecundario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spl_ContenedorSecundario.Location = new System.Drawing.Point(0, 0);
            this.spl_ContenedorSecundario.Name = "spl_ContenedorSecundario";
            this.spl_ContenedorSecundario.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spl_ContenedorSecundario.Panel1
            // 
            this.spl_ContenedorSecundario.Panel1.Controls.Add(this.grb_Grupos);
            // 
            // spl_ContenedorSecundario.Panel2
            // 
            this.spl_ContenedorSecundario.Panel2.Controls.Add(this.Correo);
            this.spl_ContenedorSecundario.Size = new System.Drawing.Size(347, 462);
            this.spl_ContenedorSecundario.SplitterDistance = 234;
            this.spl_ContenedorSecundario.TabIndex = 0;
            // 
            // FormNotificacionesEnvio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(784, 462);
            this.Controls.Add(this.splc_ContenedorPrincipal);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FormNotificacionesEnvio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Envio de notificaciones";
            this.Load += new System.EventHandler(this.FormNotificacionesEnvio_Load);
            this.grb_Grupos.ResumeLayout(false);
            this.Correo.ResumeLayout(false);
            this.Correo.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.grb_Mensaje.ResumeLayout(false);
            this.grb_Mensaje.PerformLayout();
            this.grb_Enviar.ResumeLayout(false);
            this.grb_Enviar.PerformLayout();
            this.grb_Prioridad.ResumeLayout(false);
            this.grb_Prioridad.PerformLayout();
            this.grb_Grupo_Asunto.ResumeLayout(false);
            this.grb_Grupo_Asunto.PerformLayout();
            this.splc_ContenedorPrincipal.Panel1.ResumeLayout(false);
            this.splc_ContenedorPrincipal.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splc_ContenedorPrincipal)).EndInit();
            this.splc_ContenedorPrincipal.ResumeLayout(false);
            this.spl_ContenedorSecundario.Panel1.ResumeLayout(false);
            this.spl_ContenedorSecundario.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spl_ContenedorSecundario)).EndInit();
            this.spl_ContenedorSecundario.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grb_Grupos;
        private System.Windows.Forms.GroupBox Correo;
        private System.Windows.Forms.ListBox lbxMails;
        private System.Windows.Forms.ListBox lbxGrupos;
        private System.Windows.Forms.TextBox txtMail;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.RadioButton radBaja;
        private System.Windows.Forms.RadioButton radNormal;
        private System.Windows.Forms.RadioButton radAlta;
        private System.Windows.Forms.TextBox txtAsunto;
        private System.Windows.Forms.Label lbl_Correo_Adicional;
        private System.Windows.Forms.SplitContainer splc_ContenedorPrincipal;
        private System.Windows.Forms.SplitContainer spl_ContenedorSecundario;
        private System.Windows.Forms.Button btnAdicionarCorreo;
        private System.Windows.Forms.Label lbl_Texto;
        private System.Windows.Forms.GroupBox grb_Mensaje;
        private System.Windows.Forms.TextBox txtContenido;
        private System.Windows.Forms.GroupBox grb_Enviar;
        private System.Windows.Forms.GroupBox grb_Prioridad;
        private System.Windows.Forms.GroupBox grb_Grupo_Asunto;
    }
}