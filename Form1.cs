using FastReport;
using FastReport.Barcode;
using FastReport.Export.Image;
using FastReport.Preview;
using FastReport.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cajas
{
    public partial class Form1 : Form
    {
        public static string fecha = DateTime.Now.ToString("ddMMyy");
        private Report reporteActual; // Guardar referencia para imprimir desde la UI

        public Form1()
        {
            InitializeComponent();
        }

        private void crearCodigoDeCajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            login form = new login("Fq.1234567", "crear");
            form.Show();
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            seleccionar form = new seleccionar();
            if (form.ShowDialog() == DialogResult.OK && form.SelectedProduct != null)
            {
                var p = form.SelectedProduct;
                ShowProductReport(p);
            }
        }

        public void ImprimirConSeleccionDeImpresora(Report report, float anchoMm, float altoMm)
        {
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                ImageExport imageExport = new ImageExport();
                imageExport.ImageFormat = ImageExportFormat.Png;
                imageExport.Resolution = 300;
                imageExport.SeparateFiles = false;

                using (MemoryStream ms = new MemoryStream())
                {
                    report.Export(imageExport, ms);
                    ms.Position = 0;

                    Image imagenPagina = Image.FromStream(ms);

                    PrintDocument printDoc = new PrintDocument();
                    printDoc.PrinterSettings = printDialog.PrinterSettings;
                    printDoc.PrintController = new StandardPrintController();

                    int anchoCentesimas = (int)(anchoMm / 25.4f * 100);
                    int altoCentesimas = (int)(altoMm / 25.4f * 100);

                    PaperSize tamañoPersonalizado = new PaperSize("EtiquetaPersonalizada", anchoCentesimas, altoCentesimas);
                    printDoc.DefaultPageSettings.PaperSize = tamañoPersonalizado;
                    printDoc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

                    printDoc.PrintPage += (s, e) =>
                    {
                        e.Graphics.DrawImage(imagenPagina, new Rectangle(0, 0, e.PageBounds.Width, e.PageBounds.Height));
                    };

                    printDoc.Print();

                    imagenPagina.Dispose();
                    printDoc.Dispose();
                }
            }

            printDialog.Dispose();
        }

        private void ShowProductReport(seleccionar.Product p)
        {
            var dt = new DataTable("producto");
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("nombre", typeof(string));
            dt.Columns.Add("descripcion", typeof(string));
            dt.Columns.Add("abreviatura", typeof(string));
            dt.Columns.Add("unidadDeMedida", typeof(string));
            dt.Columns.Add("medidaDeCapacidad", typeof(string));
            dt.Columns.Add("barcode", typeof(string));
            dt.Columns.Add("logo", typeof(byte[]));
            dt.Columns.Add("fechaFormato", typeof(string));

            var row = dt.NewRow();
            row["id"] = p.Id;
            row["nombre"] = p.Nombre ?? "";
            row["descripcion"] = p.Descripcion ?? "";
            row["abreviatura"] = p.Abreviatura ?? "";
            row["unidadDeMedida"] = p.UnidadDeMedida ?? "";
            row["medidaDeCapacidad"] = p.MedidaDeCapacidad ?? "";
            row["barcode"] = p.Barcode ?? "";
            row["logo"] = p.LogoBytes != null ? (object)p.LogoBytes : DBNull.Value;
            row["fechaFormato"] = Form1.fecha;
            dt.Rows.Add(row);

            var ds = new DataSet();
            ds.Tables.Add(dt);

            try
            {
                if (reporteActual != null)
                {
                    reporteActual.Dispose();
                }

                reporteActual = new Report();
                var page = new ReportPage();

                page.PaperHeight = 89f;
                page.PaperWidth = 100f;
                //page.LeftMargin = 0;
                //page.RightMargin = 0;
                //page.TopMargin = 0;
                //page.BottomMargin = 0;

                reporteActual.Pages.Add(page);

                var detail = new DataBand();
                detail.Height = 89f;
                page.Bands.Add(detail);

                // Logo
                var logoPicture = new PictureObject();
                logoPicture.Bounds = new RectangleF(-13, -25, 90, 90);
                logoPicture.DataColumn = "producto.logo";
                detail.Objects.Add(logoPicture);

                // Descripción
                var descField = new TextObject();
                string descripcion = p.Descripcion ?? "";
                MessageBox.Show(descripcion.Length.ToString());
                if (descripcion.Length == 5)
                {
                    descField.Bounds = new RectangleF(90, -30, 244, 97);
                    descField.Font = new Font("Arial", 40, FontStyle.Bold);

                }
                else if (descripcion.Length == 15)
                {
                    descField.Bounds = new RectangleF(70, -30, 244, 97);
                    descField.Font = new Font("Arial", 30, FontStyle.Bold);

                }
                else if (descripcion.Length == 26)
                {
                    descField.Bounds = new RectangleF(90, -30, 244, 97);
                    descField.Font = new Font("Arial", 15, FontStyle.Bold);
                }else if (descripcion.Length > 13)
                {
                    descField.Bounds = new RectangleF(70, -30, 244, 97);
                    descField.Font = new Font("Arial", 20, FontStyle.Bold);
                }
                else
                {
                    descField.Bounds = new RectangleF(90, -30, 244, 97);
                    descField.Font = new Font("Arial", 25, FontStyle.Bold);
                }
                descField.VertAlign = FastReport.VertAlign.Center;
                descField.HorzAlign = FastReport.HorzAlign.Center;
                descField.Text = "[producto.descripcion]";
                detail.Objects.Add(descField);

                // Lote no
                var loteno = new TextObject();
                loteno.Bounds = new RectangleF(165, 90, 250, 15);
                loteno.Font = new Font("Arial", 10, FontStyle.Bold);
                loteno.Text = "Lote No:";
                detail.Objects.Add(loteno);

                // Abreviatura + Fecha
                var abrevField = new TextObject();
                abrevField.Bounds = new RectangleF(120, 110, 250, 35);
                abrevField.Font = new Font("Arial", 25, FontStyle.Bold);
                abrevField.Text = "[producto.abreviatura][producto.fechaFormato]";
                detail.Objects.Add(abrevField);

                // Unidad de Medida
                var unitField = new TextObject();
                unitField.Bounds = new RectangleF(2, 110, 160, 60);
                unitField.Font = new Font("Arial", 15, FontStyle.Bold);
                unitField.Text = "[producto.unidadDeMedida]\nCAJA [producto.medidaDeCapacidad]/1";
                detail.Objects.Add(unitField);

                // Código de Barras
                var barcodeField = new BarcodeObject();
                string codigoLimpio = !string.IsNullOrEmpty(p.Barcode) ? p.Barcode.Trim() : "2050001107213";

                if (codigoLimpio.All(char.IsDigit) && codigoLimpio.Length == 13)
                {
                    barcodeField.Barcode = new BarcodeEAN13();
                    barcodeField.Bounds = new RectangleF(-4, 200, 150, 70);
                }
                else if (codigoLimpio.Length == 14 && codigoLimpio.All(char.IsDigit))
                {
                    var itf14 = new BarcodeITF14();
                    itf14.CalcCheckSum = false;

                    barcodeField.Barcode = itf14;
                    barcodeField.AutoSize = false;
                    barcodeField.Bounds = new RectangleF(-15, 190, 150, 70);
                }
                else
                {
                    barcodeField.Barcode = new BarcodeEAN13();
                    codigoLimpio = "2050001107213";
                    barcodeField.Bounds = new RectangleF(-4, 200, 150, 70);
                }

                barcodeField.Text = codigoLimpio;
                barcodeField.ShowText = true;
                detail.Objects.Add(barcodeField);

                // Textos FARQUINA
                var farmoquimicanacional = new TextObject();
                farmoquimicanacional.Bounds = new RectangleF(155, 200, 250, 15);
                farmoquimicanacional.Font = new Font("Arial", 6, FontStyle.Bold);
                farmoquimicanacional.Text = "FARMOQUIMICA NACIONAL, S.R.L";
                detail.Objects.Add(farmoquimicanacional);

                var farquina = new TextObject();
                farquina.Bounds = new RectangleF(148, 210, 250, 45);
                farquina.Font = new Font("Arial", 20, FontStyle.Bold);
                farquina.Text = "FARQUINA";
                detail.Objects.Add(farquina);

                reporteActual.RegisterData(ds, "producto");
                reporteActual.Prepare();

                ActualizarVistaPrevia(reporteActual);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el reporte: " + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void ActualizarVistaPrevia(Report report)
        {
            ImageExport imageExport = new ImageExport();
            imageExport.ImageFormat = ImageExportFormat.Png;

            imageExport.Resolution = 96;
            imageExport.SeparateFiles = false;

            using (MemoryStream ms = new MemoryStream())
            {
                report.Export(imageExport, ms);
                ms.Position = 0;

                if (picVistaPrevia.Image != null)
                {
                    picVistaPrevia.Image.Dispose();
                }

                picVistaPrevia.Image = Image.FromStream(ms);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (reporteActual != null && reporteActual.PreparedPages.Count > 0)
            {
                ImprimirConSeleccionDeImpresora(reporteActual, 100, 89f);
            }
            else
            {
                MessageBox.Show("Seleccione un producto primero para generar la vista previa.");
            }
        }

        private void cambiarFechaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            login form = new login("Fq.123", "cambiarFecha");
            form.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            picVistaPrevia.SizeMode = PictureBoxSizeMode.Zoom;
            picVistaPrevia.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }
    }
}