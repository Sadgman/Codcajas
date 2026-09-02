// C#
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using FastReport;
using System.IO;
using System.Drawing;

namespace cajas
{



    public partial class seleccionar : Form
    {
        public seleccionar()
        {
            InitializeComponent();
        }
        private CancellationTokenSource _cts;
        //private readonly string connString = @"Server=(localdb)\MSSQLLocalDB;Database=codcajas;Integrated Security=true;";
        private readonly string connString = @"Data Source=10.0.0.102;Initial Catalog=codcajas;User ID=aregalado;Password=@@Fq*2026#Ar;TrustServerCertificate=True;Encrypt=False;";

        //private readonly string connString = @"Server=FAQUINASERVER;Database=dbsisflex_farquina;User Id=aregalado;Password=@@Fq*2026#Ar;TrustServerCertificate=True;";
        //private readonly string connString = @"Server=10.0.0.102;Database=dbsisflex_farquina;User Id=aregalado;Password=@@Fq*2026#Ar;TrustServerCertificate=True;";
        public Product SelectedProduct { get; set; }

        public class Product
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public string Abreviatura { get; set; }
            public string UnidadDeMedida { get; set; }
            public string MedidaDeCapacidad { get; set; }
            public string Barcode { get; set; }
            public byte[] LogoBytes { get; set; }
        }
        private async void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            string term = txtSearch.Text.Trim();

            // debounce corto
            try
            {
                await Task.Delay(250, token);
            }
            catch (TaskCanceledException) { return; }

            if (string.IsNullOrEmpty(term))
            {
                lstSuggestions.Visible = false;
                return;
            }

            List<string> items;
            try
            {
                items = await GetSuggestionsAsync(term, token);
            }
            catch (OperationCanceledException) { return; }
            catch (Exception ex)
            {
                items = new List<string>();
            }

            // Actualizar UI en hilo principal
            if (!token.IsCancellationRequested)
            {
                lstSuggestions.BeginUpdate();
                lstSuggestions.Items.Clear();
                foreach (var it in items) lstSuggestions.Items.Add(it);
                lstSuggestions.EndUpdate();
                lstSuggestions.Visible = items.Count > 0;
            }
        }

        private async void LstSuggestions_Click(object sender, EventArgs e)
        {
            if (lstSuggestions.SelectedItem != null)
            {
                string productName = lstSuggestions.SelectedItem.ToString();
                txtSearch.Text = productName;
                lstSuggestions.Visible = false;

                try
                {
                    // Obtener producto completo
                    var product = await GetProductByNameAsync(productName);
                    if (product != null)
                    {
                        SelectedProduct = product;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar el producto: " + ex.Message);
                }
            }
        }

        private async Task<Product> GetProductByNameAsync(string nombre)
        {
            string sql = @"SELECT id, nombre, descripcion, abreviatura, unidadDeMedida, 
                                  medidaDeCapacidad, barcode, logo
                           FROM producto
                           WHERE nombre = @nombre";

            using (var cn = new SqlConnection(connString))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@nombre", nombre);
                await cn.OpenAsync();
                using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    if (await rdr.ReadAsync())
                    {
                        return new Product
                        {
                            Id = rdr.GetInt32(0),
                            Nombre = rdr.GetString(1),
                            Descripcion = rdr.IsDBNull(2) ? null : rdr.GetString(2),
                            Abreviatura = rdr.IsDBNull(3) ? null : rdr.GetString(3),
                            UnidadDeMedida = rdr.IsDBNull(4) ? null : rdr.GetString(4),
                            MedidaDeCapacidad = rdr.IsDBNull(5) ? null : rdr.GetString(5),
                            Barcode = rdr.IsDBNull(6) ? null : rdr.GetString(6),
                            LogoBytes = rdr.IsDBNull(7) ? null : (byte[])rdr.GetValue(7)
                        };
                    }
                }
            }
            return null;
        }

        private async Task<List<string>> GetSuggestionsAsync(string term, CancellationToken token)
        {
            var results = new List<string>();

            string sql = @"SELECT nombre FROM producto
                       WHERE nombre LIKE '%' + @p + '%'
                       ORDER BY nombre;";

            using (var cn = new SqlConnection(connString))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@p", term);
                await cn.OpenAsync(token);
                using (var rdr = await cmd.ExecuteReaderAsync(token))
                {
                    while (await rdr.ReadAsync(token))
                    {
                        results.Add(rdr.GetString(0));
                    }
                }
            }

            return results;
        }

        private void BtnSeleccionar_Click(object sender, EventArgs e)
        {
            if (lstSuggestions.SelectedItem != null)
            {
                LstSuggestions_Click(null, null);
            }
            else
            {
                MessageBox.Show("Por favor selecciona un producto de la lista");
            }
        }

        private void seleccionar_Load(object sender, EventArgs e)
        {
        }
    }
}