﻿using System;
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
    public partial class Clientes : MetroFramework.Forms.MetroForm
    {
        public Clientes()
        {
            InitializeComponent();
        }

        private void Clientes_Load(object sender, EventArgs e)
        {
            CargarDatosClientes();
        }

        private void CargarDatosClientes()
        {
            // Cadena de conexión con autenticación de Windows
            string connectionString = "Server=DESKTOP-GM5B0SU;Database=WOLFSFITNESSMARKET;Integrated Security=True;";

            // Consulta SQL para seleccionar los datos de la tabla Clientes
            string query = "SELECT * FROM Clientes";

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
                string.IsNullOrWhiteSpace(guna2TextBox4.Text)) // Asegúrate de que todos los campos estén completos
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


        private void guna2DataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Obtener la fila seleccionada
                DataGridViewRow selectedRow = guna2DataGridView1.Rows[e.RowIndex];

                // Asignar los valores de las celdas de la fila a los TextBox
                guna2TextBox6.Text = selectedRow.Cells["ClienteID"].Value.ToString();  // Ajusta el nombre de la columna
                guna2TextBox1.Text = selectedRow.Cells["Nombre"].Value.ToString();  // Ajusta el nombre de la columna
                guna2TextBox2.Text = selectedRow.Cells["Direccion"].Value.ToString();  // Ajusta el nombre de la columna
                guna2TextBox3.Text = selectedRow.Cells["Telefono"].Value.ToString();  // Ajusta el nombre de la columna
                guna2TextBox4.Text = selectedRow.Cells["Correo"].Value.ToString();  // Ajusta el nombre de la columna
            }

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string searchTerm = guna2TextBox5.Text.Trim();

            // Si el cuadro de texto está vacío, mostrar todos los clientes
            if (string.IsNullOrEmpty(searchTerm))
            {
                CargarDatosClientes();  // Cargar todos los datos de nuevo
                return;
            }

            // Cadena de conexión
            string connectionString = "Server=DESKTOP-GM5B0SU;Database=WOLFSFITNESSMARKET;Integrated Security=True;";

            // Consulta SQL para buscar solo por ClienteID
            string query = "SELECT * FROM Clientes WHERE ClienteID = @SearchTerm";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Verificar si el término de búsqueda es un número (ID del cliente)
                        if (int.TryParse(searchTerm, out int clienteId))
                        {
                            // Si es un número, lo buscamos por ClienteID
                            command.Parameters.AddWithValue("@SearchTerm", clienteId);  // Buscar por ClienteID

                            // Ejecutar la consulta y llenar el DataTable
                            SqlDataAdapter adapter = new SqlDataAdapter(command);
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Verificar si se encontraron resultados
                            if (dataTable.Rows.Count > 0)
                            {
                                // Asignar los resultados al DataGridView
                                guna2DataGridView1.DataSource = dataTable;
                            }
                            else
                            {
                                // Si no hay resultados, mostrar un mensaje opcional o limpiar el DataGridView
                                MessageBox.Show("No se encontraron resultados.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                guna2DataGridView1.DataSource = null;  // Limpiar la vista del DataGridView si no hay resultados
                            }
                        }
                        else
                        {
                            // Si no es un número, buscar por nombre del cliente
                            string queryByName = "SELECT * FROM Clientes WHERE Nombre LIKE @SearchTerm";
                            command.CommandText = queryByName;
                            command.Parameters.Clear();  // Limpiar los parámetros previos
                            command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");  // Buscar por nombre

                            // Ejecutar la consulta y llenar el DataTable
                            SqlDataAdapter adapter = new SqlDataAdapter(command);
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Verificar si se encontraron resultados
                            if (dataTable.Rows.Count > 0)
                            {
                                // Asignar los resultados al DataGridView
                                guna2DataGridView1.DataSource = dataTable;
                            }
                            else
                            {
                                // Si no hay resultados, mostrar un mensaje opcional o limpiar el DataGridView
                                MessageBox.Show("No se encontraron resultados.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                guna2DataGridView1.DataSource = null;  // Limpiar la vista del DataGridView si no hay resultados
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al realizar la búsqueda: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado un cliente
            if (string.IsNullOrWhiteSpace(guna2TextBox6.Text)) // Suponiendo que guna2TextBox5 contiene el ID del cliente
            {
                MessageBox.Show("Por favor, seleccione un cliente para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirmar la eliminación
            DialogResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar este cliente?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                // Cadena de conexión
                string connectionString = "Server=DESKTOP-GM5B0SU;Database=WOLFSFITNESSMARKET;Integrated Security=True;";

                // Obtener el ID del cliente seleccionado
                int clienteId = Convert.ToInt32(guna2TextBox6.Text);

                // Consulta SQL para eliminar el cliente
                string query = "DELETE FROM Clientes WHERE ClienteID = @ClienteID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Agregar el parámetro del ID
                            command.Parameters.AddWithValue("@ClienteID", clienteId);

                            // Ejecutar la consulta
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Cliente eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                CargarDatosClientes();
                                LimpiarCampos();
                            }
                            else
                            {
                                MessageBox.Show("No se pudo eliminar el cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            // Cadena de conexión
            string connectionString = "Server=DESKTOP-GM5B0SU;Database=WOLFSFITNESSMARKET;Integrated Security=True;";

            // Variables para capturar los datos del cliente
            string nombre = guna2TextBox1.Text;
            string direccion = guna2TextBox2.Text;
            string telefono = guna2TextBox3.Text;
            string correo = guna2TextBox4.Text;

            // Validar que todos los campos estén llenos
            if (ValidarCampos())
            {
                // Consulta SQL para insertar un nuevo cliente
                string query = "INSERT INTO Clientes (Nombre, Direccion, Telefono, Correo) VALUES (@Nombre, @Direccion, @Telefono, @Correo)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Agregar los parámetros de la consulta
                            command.Parameters.AddWithValue("@Nombre", nombre);
                            command.Parameters.AddWithValue("@Direccion", direccion);
                            command.Parameters.AddWithValue("@Telefono", telefono);
                            command.Parameters.AddWithValue("@Correo", correo);

                            // Ejecutar la consulta
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Cliente guardado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LimpiarCampos();
                                CargarDatosClientes();
                            }
                            else
                            {
                                MessageBox.Show("No se pudo guardar el cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Obtener los valores de la fila y columna editada
                int clienteId = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["ClienteID"].Value);
                string nombre = guna2DataGridView1.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                string direccion = guna2DataGridView1.Rows[e.RowIndex].Cells["Direccion"].Value.ToString();
                string telefono = guna2DataGridView1.Rows[e.RowIndex].Cells["Telefono"].Value.ToString();
                string correo = guna2DataGridView1.Rows[e.RowIndex].Cells["Correo"].Value.ToString();

                // Mostrar un mensaje de confirmación
                DialogResult result = MessageBox.Show("¿Estás seguro de que deseas actualizar este cliente?",
                                                      "Confirmar actualización",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Cadena de conexión
                    string connectionString = "Server=DESKTOP-GM5B0SU;Database=WOLFSFITNESSMARKET;Integrated Security=True;";

                    // Consulta SQL para actualizar el cliente
                    string query = "UPDATE Clientes SET Nombre = @Nombre, Direccion = @Direccion, Telefono = @Telefono, Correo = @Correo WHERE ClienteID = @ClienteID";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                // Agregar los parámetros de la consulta
                                command.Parameters.AddWithValue("@ClienteID", clienteId);
                                command.Parameters.AddWithValue("@Nombre", nombre);
                                command.Parameters.AddWithValue("@Direccion", direccion);
                                command.Parameters.AddWithValue("@Telefono", telefono);
                                command.Parameters.AddWithValue("@Correo", correo);

                                // Ejecutar la consulta
                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Cliente actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    CargarDatosClientes();
                                }
                                else
                                {
                                    MessageBox.Show("No se pudo actualizar el cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    
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
                    // Si el usuario hace clic en 'No', deshacer el cambio en la celda
                    guna2DataGridView1.CancelEdit();
                    CargarDatosClientes();

                }
            }
        }
    }
}