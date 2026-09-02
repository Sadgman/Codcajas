using FastReport;
using FastReport.Barcode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cajas
{
    public partial class crear : Form
    {
        private byte[] selectedLogoBytes;

        // Cadena de conexión actualizada a la base de datos remota
        private readonly string connString = @"Data Source=10.0.0.102;Initial Catalog=codcajas;User ID=aregalado;Password=@@Fq*2026#Ar;TrustServerCertificate=True;Encrypt=False;";

        public crear()
        {
            InitializeComponent();
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                dlg.Title = "Seleccionar logo";
                if (dlg.ShowDialog() != DialogResult.OK) return;

                // Leer bytes sin bloquear el archivo original
                selectedLogoBytes = File.ReadAllBytes(dlg.FileName);

                // Mostrar en PictureBox
                using (var ms = new MemoryStream(selectedLogoBytes))
                {
                    pictureBox1.Image = Image.FromStream(ms);
                }
            }
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                string descripcion = txtDescripcion.Text.Trim();
                string nombre = txtNombre.Text.Trim();
                string abreviatura = txtAbreviatura.Text.Trim();
                string unidadDeMedida = txtUnidad.Text.Trim();
                string medidaDeCapacidad = txtMedida.Text.Trim();
                string barcode = txtBarcode.Text.Trim();

                if (selectedLogoBytes == null)
                {
                    MessageBox.Show("Selecciona una imagen para el logo", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (int.TryParse(tbId.Text.Trim(), out int id) && id > 0)
                {
                    UpdateProducto(connString, id, descripcion, nombre, abreviatura, unidadDeMedida, medidaDeCapacidad, barcode, selectedLogoBytes);
                    MessageBox.Show("Producto actualizado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    InsertProducto(connString, descripcion, nombre, abreviatura, unidadDeMedida, medidaDeCapacidad, barcode, selectedLogoBytes);
                    MessageBox.Show("Producto insertado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión o inserción: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertProducto(
            string connString,
            string descripcion,
            string nombre,
            string abreviatura,
            string unidadDeMedida,
            string medidaDeCapacidad,
            string barcode,
            byte[] logoBytes)
        {
            using (var conn = new SqlConnection(connString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    INSERT INTO producto
                        (descripcion, nombre, abreviatura, unidadDeMedida, medidaDeCapacidad, barcode, logo)
                    VALUES
                        (@descripcion, @nombre, @abreviatura, @unidadDeMedida, @medidaDeCapacidad, @barcode, @logo)";

                cmd.Parameters.Add("@descripcion", SqlDbType.VarChar, 255).Value = string.IsNullOrEmpty(descripcion) ? (object)DBNull.Value : descripcion;
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 255).Value = string.IsNullOrEmpty(nombre) ? (object)DBNull.Value : nombre;

                string abrevValida = abreviatura;
                cmd.Parameters.Add("@abreviatura", SqlDbType.VarChar, 255).Value = string.IsNullOrEmpty(abrevValida) ? (object)DBNull.Value : abrevValida;

                cmd.Parameters.Add("@unidadDeMedida", SqlDbType.VarChar, 255).Value = string.IsNullOrEmpty(unidadDeMedida) ? (object)DBNull.Value : unidadDeMedida;
                cmd.Parameters.Add("@medidaDeCapacidad", SqlDbType.VarChar, 255).Value = string.IsNullOrEmpty(medidaDeCapacidad) ? (object)DBNull.Value : medidaDeCapacidad;
                cmd.Parameters.Add("@barcode", SqlDbType.VarChar, 255).Value = string.IsNullOrEmpty(barcode) ? (object)DBNull.Value : barcode;
                cmd.Parameters.Add("@logo", SqlDbType.VarBinary, -1).Value = (object)logoBytes ?? DBNull.Value;

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateProducto(
            string connString,
            int id,
            string descripcion,
            string nombre,
            string abreviatura,
            string unidadDeMedida,
            string medidaDeCapacidad,
            string barcode,
            byte[] logoBytes)
        {
            using (var conn = new SqlConnection(connString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    UPDATE producto
                    SET descripcion = @descripcion,
                        nombre = @nombre,
                        abreviatura = @abreviatura,
                        unidadDeMedida = @unidadDeMedida,
                        medidaDeCapacidad = @medidaDeCapacidad,
                        barcode = @barcode,
                        logo = @logo
                    WHERE id = @id";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@descripcion", SqlDbType.VarChar, 255).Value = string.IsNullOrEmpty(descripcion) ? (object)DBNull.Value : descripcion;
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 255).Value = string.IsNullOrEmpty(nombre) ? (object)DBNull.Value : nombre;

                string abrevValida = abreviatura;
                cmd.Parameters.Add("@abreviatura", SqlDbType.VarChar, 255).Value = string.IsNullOrEmpty(abrevValida) ? (object)DBNull.Value : abrevValida;

                cmd.Parameters.Add("@unidadDeMedida", SqlDbType.VarChar, 255).Value = string.IsNullOrEmpty(unidadDeMedida) ? (object)DBNull.Value : unidadDeMedida;
                cmd.Parameters.Add("@medidaDeCapacidad", SqlDbType.VarChar, 255).Value = string.IsNullOrEmpty(medidaDeCapacidad) ? (object)DBNull.Value : medidaDeCapacidad;
                cmd.Parameters.Add("@barcode", SqlDbType.VarChar, 255).Value = string.IsNullOrEmpty(barcode) ? (object)DBNull.Value : barcode;
                cmd.Parameters.Add("@logo", SqlDbType.VarBinary, -1).Value = (object)logoBytes ?? DBNull.Value;

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        private void crear_Load(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            seleccionar select = new seleccionar();
            if (select.ShowDialog() == DialogResult.OK && select.SelectedProduct != null)
            {
                var p = select.SelectedProduct;
                ShowProductReport(p);
            }
        }
        private void ShowProductReport(seleccionar.Product p)
        {
            tbId.Text = p.Id.ToString();
            txtDescripcion.Text = p.Descripcion.ToString();
            txtNombre.Text = p.Nombre.ToString();
            txtAbreviatura.Text = p.Abreviatura.ToString();
            txtUnidad.Text = p.UnidadDeMedida.ToString();
            txtMedida.Text = p.MedidaDeCapacidad.ToString();
            txtBarcode.Text = p.Barcode.ToString();
            pictureBox1.Image = p.LogoBytes != null ? Image.FromStream(new MemoryStream(p.LogoBytes)) : null;
            selectedLogoBytes = p.LogoBytes;


        }
    }
}