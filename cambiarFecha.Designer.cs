namespace cajas
{
    partial class cambiarFecha
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
            this.TxtFecha = new System.Windows.Forms.Label();
            this.tbFecha = new System.Windows.Forms.TextBox();
            this.cbautomatica = new System.Windows.Forms.CheckBox();
            this.btnGrabar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TxtFecha
            // 
            this.TxtFecha.AutoSize = true;
            this.TxtFecha.Location = new System.Drawing.Point(46, 97);
            this.TxtFecha.Name = "TxtFecha";
            this.TxtFecha.Size = new System.Drawing.Size(37, 13);
            this.TxtFecha.TabIndex = 0;
            this.TxtFecha.Text = "Fecha";
            // 
            // tbFecha
            // 
            this.tbFecha.Enabled = false;
            this.tbFecha.Location = new System.Drawing.Point(108, 94);
            this.tbFecha.MaxLength = 6;
            this.tbFecha.Name = "tbFecha";
            this.tbFecha.Size = new System.Drawing.Size(100, 20);
            this.tbFecha.TabIndex = 1;
            // 
            // cbautomatica
            // 
            this.cbautomatica.AutoSize = true;
            this.cbautomatica.Checked = true;
            this.cbautomatica.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbautomatica.Location = new System.Drawing.Point(224, 97);
            this.cbautomatica.Name = "cbautomatica";
            this.cbautomatica.Size = new System.Drawing.Size(78, 17);
            this.cbautomatica.TabIndex = 2;
            this.cbautomatica.Text = "automatica";
            this.cbautomatica.UseVisualStyleBackColor = true;
            this.cbautomatica.CheckedChanged += new System.EventHandler(this.cbautomatica_CheckedChanged);
            // 
            // btnGrabar
            // 
            this.btnGrabar.Enabled = false;
            this.btnGrabar.Location = new System.Drawing.Point(120, 165);
            this.btnGrabar.Name = "btnGrabar";
            this.btnGrabar.Size = new System.Drawing.Size(75, 23);
            this.btnGrabar.TabIndex = 3;
            this.btnGrabar.Text = "Grabar";
            this.btnGrabar.UseVisualStyleBackColor = true;
            this.btnGrabar.Click += new System.EventHandler(this.btnGrabar_Click);
            // 
            // cambiarFecha
            // 
            this.AcceptButton = this.btnGrabar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 234);
            this.Controls.Add(this.btnGrabar);
            this.Controls.Add(this.cbautomatica);
            this.Controls.Add(this.tbFecha);
            this.Controls.Add(this.TxtFecha);
            this.Name = "cambiarFecha";
            this.Text = "cambiarFecha";
            this.Load += new System.EventHandler(this.cambiarFecha_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TxtFecha;
        private System.Windows.Forms.TextBox tbFecha;
        private System.Windows.Forms.CheckBox cbautomatica;
        private System.Windows.Forms.Button btnGrabar;
    }
}