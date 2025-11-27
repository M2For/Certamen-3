using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoPatentar
{
    public partial class FormResumen21 : Form
    {
        private RegistroMarca registroActual = new RegistroMarca();

        private const string ArchivoSolicitantes = "solicitantes.txt";
        private const string ArchivoIdentificaciones = "identificaciones.txt";
        private const string ArchivoClasificaciones = "clasificaciones.txt";

        public FormResumen21()
        {
            InitializeComponent();
            ConfigurarDataGridView();

            CargarDatosAgregados();
            MostrarResumen();
        }



        private void ConfigurarDataGridView()
        {
            if (dgvHistorialClases.Columns.Count == 0)
            {
                dgvHistorialClases.Columns.Add("colClaseNumero", "Clase Niza");
                dgvHistorialClases.Columns.Add("colDescripcion", "Productos / Servicios");
                dgvHistorialClases.Columns["colClaseNumero"].Width = 100;
                dgvHistorialClases.Columns["colDescripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void CargarDatosAgregados()
        {
        

            if (File.Exists(ArchivoSolicitantes))
            {
                string primeraLinea = File.ReadAllLines(ArchivoSolicitantes).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(primeraLinea))
                {
                    registroActual.DatosSolicitante = Solicitante.DesdeTexto(primeraLinea);
                }
            }

            if (File.Exists(ArchivoIdentificaciones))
            {
                string primeraLinea = File.ReadAllLines(ArchivoIdentificaciones).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(primeraLinea))
                {
                    registroActual.DatosMarca = Identificacion.DesdeTexto(primeraLinea);
                }
            }

            if (File.Exists(ArchivoClasificaciones))
            {
                string[] lineas = File.ReadAllLines(ArchivoClasificaciones);
                foreach (string linea in lineas)
                {
                    if (!string.IsNullOrWhiteSpace(linea))
                    {
                        Clasificacion c = Clasificacion.DesdeTexto(linea);
                        if (c != null) registroActual.ClasesDeNiza.Add(c);
                    }
                }
            }
        }

        private void MostrarResumen()
        {

            if (registroActual.DatosMarca != null)
            {
                txtNombreMarca.Text = registroActual.DatosMarca.NombreMarca;
            }
            else
            {
                txtNombreMarca.Text = "N/A";
            }

            dgvHistorialClases.Rows.Clear();

            foreach (var clase in registroActual.ClasesDeNiza)
            {
                int index = dgvHistorialClases.Rows.Add();
                dgvHistorialClases.Rows[index].Cells["colClaseNumero"].Value = clase.ClaseNumero.ToString();
                dgvHistorialClases.Rows[index].Cells["colDescripcion"].Value = clase.Descripcion;
            }

            decimal costo = registroActual.CalcularCostoTotal();
            txtMontoTotal.Text = $"${costo:N2}";
        }

 


        private void btnFinalizar_Click(object sender, EventArgs e)
        {
             if (registroActual.DatosSolicitante != null)
         {
             decimal costo = registroActual.CalcularCostoTotal();
             MessageBox.Show(
                 $"Registro de marca completado para {registroActual.DatosMarca.NombreMarca} por un costo total de ${costo:N2}. Se ha enviado tu solicitud, estaremos trabajando en proceso tu proceso patentificación,{registroActual.DatosSolicitante.Nombre}.",
                 "Registro Finalizado, ",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Information);

             Application.Exit();
         }
         else
         {
             MessageBox.Show("Por favor agregue sus datos en la ventana Datos del solicitante'.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);

         }
        }
    }
}
