using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cajas
{
    public partial class cambiarFecha : Form
    {
        public cambiarFecha()
        {
            InitializeComponent();
        }

        private void cbautomatica_CheckedChanged(object sender, EventArgs e)
        {

            if (cbautomatica.Checked)
            {
                btnGrabar.Enabled = false;
                tbFecha.Enabled = false;
                Form1.fecha = DateTime.Now.ToString("ddMMyy");
            }
            else
            {
                btnGrabar.Enabled = true;
                tbFecha.Enabled = true;
                Form1.fecha = tbFecha.Text;
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            Form1.fecha = tbFecha.Text;
            MessageBox.Show("Fecha cambiada a: " + Form1.fecha, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cambiarFecha_Load(object sender, EventArgs e)
        {
            if(Form1.fecha == DateTime.Now.ToString("ddMMyy"))
            {
                cbautomatica.Checked = true;
                tbFecha.Enabled = false;
                btnGrabar.Enabled = false;
            }
            else
            {
                cbautomatica.Checked = false;
                tbFecha.Enabled = true;
                btnGrabar.Enabled = true;
                tbFecha.Text = Form1.fecha;
            }
        }
    }
}
