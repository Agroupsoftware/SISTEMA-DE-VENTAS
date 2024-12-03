using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WOLFSFITNESSMARKET
{
    public partial class Proveedores : MetroFramework.Forms.MetroForm
    {
        public Proveedores()
        {
            InitializeComponent();
        }

        private void Proveedores_Load(object sender, EventArgs e)
        {
            CargarDatosProveedores();
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTCLIENT = 0x1;


            // Ignorar cualquier intento de mover el formulario
            if (m.Msg == WM_NCHITTEST)
            {
                m.Result = (IntPtr)HTCLIENT;  // Establecer que el área activa es el cliente, no la barra de título
            }
            else
            {
                base.WndProc(ref m); // Llamar al procesamiento estándar para otros mensajes
            }
        }

        private void CargarDatosProveedores()
        {
            string connectionString = "Server=JEFFERSON\\SQLEXPRESS;Database=WOLFSFITNESSMARKET;Integrated Security=True;";
            string query = "SELECT * FROM Proveedores";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    guna2DataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(guna2TextBox1.Text) ||
                string.IsNullOrWhiteSpace(guna2TextBox2.Text) ||
                string.IsNullOrWhiteSpace(guna2TextBox3.Text) ||
                string.IsNullOrWhiteSpace(guna2TextBox4.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void LimpiarCampos()
        {
            guna2TextBox1.Clear();
            guna2TextBox2.Clear();
            guna2TextBox3.Clear();
            guna2TextBox4.Clear();
        }


        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            string connectionString = "Server=JEFFERSON\\SQLEXPRESS;Database=WOLFSFITNESSMARKET;Integrated Security=True;";

            if (ValidarCampos())
            {
                string query = "INSERT INTO Proveedores (Nombre, Direccion, Telefono, Correo) VALUES (@Nombre, @Direccion, @Telefono, @Correo)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Nombre", guna2TextBox1.Text);
                            command.Parameters.AddWithValue("@Direccion", guna2TextBox2.Text);
                            command.Parameters.AddWithValue("@Telefono", guna2TextBox3.Text);
                            command.Parameters.AddWithValue("@Correo", guna2TextBox4.Text);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Proveedor guardado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LimpiarCampos();
                                CargarDatosProveedores();
                            }
                            else
                            {
                                MessageBox.Show("No se pudo guardar el proveedor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(guna2TextBox6.Text))
            {
                MessageBox.Show("Por favor, seleccione un proveedor para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar este proveedor?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string connectionString = "Server=JEFFERSON\\SQLEXPRESS;Database=WOLFSFITNESSMARKET;Integrated Security=True;";
                int proveedorId = Convert.ToInt32(guna2TextBox6.Text);
                string query = "DELETE FROM Proveedores WHERE ProveedorID = @ProveedorID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ProveedorID", proveedorId);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Proveedor eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                CargarDatosProveedores();
                                LimpiarCampos();
                            }
                            else
                            {
                                MessageBox.Show("No se pudo eliminar el proveedor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {

            string searchTerm = guna2TextBox5.Text.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                CargarDatosProveedores();
                return;
            }

            string connectionString = "Server=JEFFERSON\\SQLEXPRESS;Database=WOLFSFITNESSMARKET;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    if (int.TryParse(searchTerm, out int proveedorId))
                    {
                        command.CommandText = "SELECT * FROM Proveedores WHERE ProveedorID = @SearchTerm";
                        command.Parameters.AddWithValue("@SearchTerm", proveedorId);
                    }
                    else
                    {
                        command.CommandText = "SELECT * FROM Proveedores WHERE Nombre LIKE @SearchTerm";
                        command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        guna2DataGridView1.DataSource = dataTable;
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron resultados.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        guna2DataGridView1.DataSource = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al realizar la búsqueda: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void guna2DataGridView1_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int proveedorId = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["ProveedorID"].Value);
                string nombre = guna2DataGridView1.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                string direccion = guna2DataGridView1.Rows[e.RowIndex].Cells["Direccion"].Value.ToString();
                string telefono = guna2DataGridView1.Rows[e.RowIndex].Cells["Telefono"].Value.ToString();
                string correo = guna2DataGridView1.Rows[e.RowIndex].Cells["Correo"].Value.ToString();

                DialogResult result = MessageBox.Show("¿Estás seguro de que deseas actualizar este proveedor?",
                                                      "Confirmar actualización",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string connectionString = "Server=JEFFERSON\\SQLEXPRESS;Database=WOLFSFITNESSMARKET;Integrated Security=True;";
                    string query = "UPDATE Proveedores SET Nombre = @Nombre, Direccion = @Direccion, Telefono = @Telefono, Correo = @Correo WHERE ProveedorID = @ProveedorID";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@ProveedorID", proveedorId);
                                command.Parameters.AddWithValue("@Nombre", nombre);
                                command.Parameters.AddWithValue("@Direccion", direccion);
                                command.Parameters.AddWithValue("@Telefono", telefono);
                                command.Parameters.AddWithValue("@Correo", correo);

                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Proveedor actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    CargarDatosProveedores();
                                }
                                else
                                {
                                    MessageBox.Show("No se pudo actualizar el proveedor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    CargarDatosProveedores();
                }
            }
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Obtener la fila seleccionada
                DataGridViewRow selectedRow = guna2DataGridView1.Rows[e.RowIndex];

                // Asignar los valores de las celdas de la fila a los TextBox
                guna2TextBox6.Text = selectedRow.Cells["ProveedorID"].Value.ToString();  // Ajusta el nombre de la columna
            
            }
        }
    }
}
