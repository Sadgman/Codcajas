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
    public partial class login : Form
    {
        public string password { get; set; }
        public string proviene { get; set; }
        public login(string password, string proviene)
        {
            InitializeComponent();
            this.password = password;
            this.proviene = proviene;
        }

        private void Iniciar_Click(object sender, EventArgs e)
        {
            if(TxtContra.Text == this.password)
            {
                if(this.proviene == "crear")
                {
                    crear form = new crear();
                    form.Show();
                }else if(this.proviene == "cambiarFecha")
                {
                    cambiarFecha form = new cambiarFecha();
                    form.Show();
                }
                this.Close();
            }
        }

        private void chmostrar_CheckedChanged(object sender, EventArgs e)
        {
            if(chmostrar.Checked)
            {
                TxtContra.PasswordChar = '\0';
            }
            else
            {
                TxtContra.PasswordChar = '*';
            }
        }
    }
}
