namespace cajas
{
    partial class login
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
            this.label1 = new System.Windows.Forms.Label();
            this.TxtContra = new System.Windows.Forms.TextBox();
            this.Iniciar = new System.Windows.Forms.Button();
            this.chmostrar = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Contraseña";
            // 
            // TxtContra
            // 
            this.TxtContra.Location = new System.Drawing.Point(180, 100);
            this.TxtContra.Name = "TxtContra";
            this.TxtContra.PasswordChar = '*';
            this.TxtContra.Size = new System.Drawing.Size(152, 20);
            this.TxtContra.TabIndex = 1;
            // 
            // Iniciar
            // 
            this.Iniciar.Location = new System.Drawing.Point(114, 177);
            this.Iniciar.Name = "Iniciar";
            this.Iniciar.Size = new System.Drawing.Size(170, 23);
            this.Iniciar.TabIndex = 2;
            this.Iniciar.Text = "Iniciar";
            this.Iniciar.UseVisualStyleBackColor = true;
            this.Iniciar.Click += new System.EventHandler(this.Iniciar_Click);
            // 
            // chmostrar
            // 
            this.chmostrar.AutoSize = true;
            this.chmostrar.Location = new System.Drawing.Point(348, 102);
            this.chmostrar.Name = "chmostrar";
            this.chmostrar.Size = new System.Drawing.Size(61, 17);
            this.chmostrar.TabIndex = 3;
            this.chmostrar.Text = "Mostrar";
            this.chmostrar.UseVisualStyleBackColor = true;
            this.chmostrar.CheckedChanged += new System.EventHandler(this.chmostrar_CheckedChanged);
            // 
            // login
            // 
            this.AcceptButton = this.Iniciar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 252);
            this.Controls.Add(this.chmostrar);
            this.Controls.Add(this.Iniciar);
            this.Controls.Add(this.TxtContra);
            this.Controls.Add(this.label1);
            this.Name = "login";
            this.Text = "login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtContra;
        private System.Windows.Forms.Button Iniciar;
        private System.Windows.Forms.CheckBox chmostrar;
    }
}