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
    public partial class Inventario : MetroFramework.Forms.MetroForm
    {
        public Inventario()
        {
            InitializeComponent();
        }

        private void Inventario_Load(object sender, EventArgs e)
        {
            CargarDatosInventario();
        }

        private void CargarDatosInventario()
        {
            string connectionString = "Server=DESKTOP-GM5B0SU;Database=WOLFSFITNESSMARKET;Integrated Security=True;";
            string query = "SELECT * FROM Inventario";

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
                string.IsNullOrWhiteSpace(guna2TextBox4.Text) ||
                 string.IsNullOrWhiteSpace(guna2TextBox5.Text) ||
                string.IsNullOrWhiteSpace(guna2TextBox6.Text))
  
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
            guna2TextBox5.Clear();
            guna2TextBox6.Clear();
        }

        private void guna2TextBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            string connectionString = "Server=DESKTOP-GM5B0SU;Database=WOLFSFITNESSMARKET;Integrated Security=True;";

            if (ValidarCampos())
            {
                string query = "INSERT INTO Inventario (Nombre, Descripcion, PrecioCosto, PrecioVenta, StockActual, StockMinimo) " +
                               "VALUES (@Nombre, @Descripcion, @PrecioCosto, @PrecioVenta, @StockActual, @StockMinimo)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Nombre", guna2TextBox1.Text);
                            command.Parameters.AddWithValue("@Descripcion", guna2TextBox2.Text);
                            command.Parameters.AddWithValue("@PrecioCosto", decimal.Parse(guna2TextBox3.Text));
                            command.Parameters.AddWithValue("@PrecioVenta", decimal.Parse(guna2TextBox4.Text));
                            command.Parameters.AddWithValue("@StockActual", int.Parse(guna2TextBox5.Text));
                            command.Parameters.AddWithValue("@StockMinimo", int.Parse(guna2TextBox6.Text));

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Producto guardado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LimpiarCampos();
                                CargarDatosInventario();
                            }
                            else
                            {
                                MessageBox.Show("No se pudo guardar el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void guna2DataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = guna2DataGridView1.Rows[e.RowIndex];

                guna2TextBox8.Text = selectedRow.Cells["ProductoID"].Value.ToString();
                guna2TextBox1.Text = selectedRow.Cells["Nombre"].Value.ToString();
                guna2TextBox2.Text = selectedRow.Cells["Descripcion"].Value.ToString();
                guna2TextBox3.Text = selectedRow.Cells["PrecioCosto"].Value.ToString();
                guna2TextBox4.Text = selectedRow.Cells["PrecioVenta"].Value.ToString();
                guna2TextBox5.Text = selectedRow.Cells["StockActual"].Value.ToString();
                guna2TextBox6.Text = selectedRow.Cells["StockMinimo"].Value.ToString();
            }

        }

        private void guna2DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            /// Verificar si el cambio ocurrió en una fila válida y una columna válida
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Obtener los valores de las celdas modificadas en la fila
                int productoId = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["ProductoID"].Value);
                string nombre = guna2DataGridView1.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                string descripcion = guna2DataGridView1.Rows[e.RowIndex].Cells["Descripcion"].Value.ToString();
                decimal precioCosto = Convert.ToDecimal(guna2DataGridView1.Rows[e.RowIndex].Cells["PrecioCosto"].Value);
                decimal precioVenta = Convert.ToDecimal(guna2DataGridView1.Rows[e.RowIndex].Cells["PrecioVenta"].Value);
                int stockActual = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["StockActual"].Value);
                int stockMinimo = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["StockMinimo"].Value);

                // Confirmación de actualización
                DialogResult result = MessageBox.Show("¿Estás seguro de que deseas actualizar este producto?",
                                                      "Confirmar actualización",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Cadena de conexión a la base de datos
                    string connectionString = "Server=DESKTOP-GM5B0SU;Database=WOLFSFITNESSMARKET;Integrated Security=True;";
                    // Consulta de actualización SQL
                    string query = "UPDATE Inventario SET Nombre = @Nombre, Descripcion = @Descripcion, PrecioCosto = @PrecioCosto, " +
                                   "PrecioVenta = @PrecioVenta, StockActual = @StockActual, StockMinimo = @StockMinimo WHERE ProductoID = @ProductoID";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                // Asignar valores a los parámetros de la consulta
                                command.Parameters.AddWithValue("@ProductoID", productoId);
                                command.Parameters.AddWithValue("@Nombre", nombre);
                                command.Parameters.AddWithValue("@Descripcion", descripcion);
                                command.Parameters.AddWithValue("@PrecioCosto", precioCosto);
                                command.Parameters.AddWithValue("@PrecioVenta", precioVenta);
                                command.Parameters.AddWithValue("@StockActual", stockActual);
                                command.Parameters.AddWithValue("@StockMinimo", stockMinimo);

                                // Ejecutar la consulta de actualización
                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Producto actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    CargarDatosInventario();  // Recargar datos de inventario
                                }
                                else
                                {
                                    MessageBox.Show("No se pudo actualizar el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    CargarDatosInventario();
                }
            }

        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(guna2TextBox6.Text))
            {
                MessageBox.Show("Por favor, seleccione un producto para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar este producto?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string connectionString = "Server=DESKTOP-GM5B0SU;Database=WOLFSFITNESSMARKET;Integrated Security=True;";
                int productoId = Convert.ToInt32(guna2TextBox8.Text);
                string query = "DELETE FROM Inventario WHERE ProductoID = @ProductoID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ProductoID", productoId);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Producto eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                CargarDatosInventario();
                                LimpiarCampos();
                            }
                            else
                            {
                                MessageBox.Show("No se pudo eliminar el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string searchTerm = guna2TextBox7.Text.Trim();

            // Si el cuadro de texto está vacío, mostrar todos los productos
            if (string.IsNullOrEmpty(searchTerm))
            {
                CargarDatosInventario();  // Cargar todos los datos de nuevo
                return;
            }

            // Cadena de conexión
            string connectionString = "Server=DESKTOP-GM5B0SU;Database=WOLFSFITNESSMARKET;Integrated Security=True;";

            // Consulta SQL para buscar solo por ProductoID
            string query = "SELECT * FROM Inventario WHERE ProductoID = @SearchTerm";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Verificar si el término de búsqueda es un número (ID del producto)
                        if (int.TryParse(searchTerm, out int productoId))
                        {
                            // Si es un número, lo buscamos por ProductoID
                            command.Parameters.AddWithValue("@SearchTerm", productoId);  // Buscar por ProductoID

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
                            // Si no es un número, buscar por nombre del producto
                            string queryByName = "SELECT * FROM Inventario WHERE Nombre LIKE @SearchTerm";
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
    }
}
