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
    public partial class Usuarios : MetroFramework.Forms.MetroForm
    {
        public Usuarios()
        {
            InitializeComponent();

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

        private void Usuarios_Load(object sender, EventArgs e)
        {
            CargarDatosUsuarios();
        }
        private void CargarDatosUsuarios()
        {
            // Cadena de conexión con autenticación de Windows
            string connectionString = "Server=JEFFERSON\\SQLEXPRESS;Database=WOLFSFITNESSMARKET;Integrated Security=True;";


            // Consulta SQL para seleccionar los datos de la tabla usuario
            string query = "SELECT * FROM Usuarios";

            // Crear la conexión y el adaptador de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Abrir la conexión
                    connection.Open();

                    // Crear un adaptador de datos con la consulta
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                    // Crear y llenar un DataTable con los datos
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Asignar el DataTable al DataSource del DataGridView
                    guna2DataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    MessageBox.Show("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Método para validar que todos los campos estén llenos
        private bool ValidarCampos()
        {
            // Validamos que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(guna2TextBox1.Text) ||
                string.IsNullOrWhiteSpace(guna2TextBox2.Text) ||
                string.IsNullOrWhiteSpace(guna2TextBox3.Text) ||
                guna2ComboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        // Método para limpiar los campos
        private void LimpiarCampos()
        {
            guna2TextBox1.Clear();
            guna2TextBox2.Clear();
            guna2TextBox3.Clear();
            guna2ComboBox1.SelectedIndex = -1; // Desmarca la opción seleccionada en el ComboBox
        }



        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // Cadena de conexión
            string connectionString = "Server=JEFFERSON\\SQLEXPRESS;Database=WOLFSFITNESSMARKET;Integrated Security=True;";

            // Variables para capturar los datos del usuario
            string nombre = guna2TextBox1.Text;
            string correo = guna2TextBox2.Text;
            string contrasena = guna2TextBox3.Text;
            string rol = guna2ComboBox1.SelectedItem?.ToString();  // Usamos el operador null-conditional para evitar null referencia

            // Validar que todos los campos estén llenos
            if (ValidarCampos())
            {
                // Verificar si el rol no es nulo
                if (string.IsNullOrEmpty(rol))
                {
                    MessageBox.Show("Por favor, seleccione un rol.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Consulta SQL para insertar un nuevo usuario
                string query = "INSERT INTO Usuarios (Nombre, Correo, Contrasena, Rol) VALUES (@Nombre, @Correo, @Contrasena, @Rol)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Agregar los parámetros de la consulta
                            command.Parameters.AddWithValue("@Nombre", nombre);
                            command.Parameters.AddWithValue("@Correo", correo);
                            command.Parameters.AddWithValue("@Contrasena", contrasena);
                            command.Parameters.AddWithValue("@Rol", rol);

                            // Ejecutar la consulta
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Usuario guardado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LimpiarCampos();
                                CargarDatosUsuarios();
                            }
                            else
                            {
                                MessageBox.Show("No se pudo guardar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado un usuario
            if (string.IsNullOrWhiteSpace(guna2TextBox5.Text))  // Suponiendo que guna2TextBox5 contiene el ID del usuario
            {
                MessageBox.Show("Por favor, seleccione un usuario para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirmar la eliminación
            DialogResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar este usuario?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                // Cadena de conexión
                string connectionString = "Server=JEFFERSON\\SQLEXPRESS;Database=WOLFSFITNESSMARKET;Integrated Security=True;";

                // Obtener el ID del usuario seleccionado
                int usuarioId = Convert.ToInt32(guna2TextBox5.Text);

                // Consulta SQL para eliminar el usuario
                string query = "DELETE FROM Usuarios WHERE UsuarioID = @UsuarioID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Agregar el parámetro del ID
                            command.Parameters.AddWithValue("@UsuarioID", usuarioId);

                            // Ejecutar la consulta
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Usuario eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                CargarDatosUsuarios();
                                LimpiarCampos();
                            }
                            else
                            {
                                MessageBox.Show("No se pudo eliminar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void guna2DataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificamos que se haya hecho clic en una fila válida (no en los encabezados de columna)
            if (e.RowIndex >= 0)
            {
                // Obtener la fila seleccionada
                DataGridViewRow selectedRow = guna2DataGridView1.Rows[e.RowIndex];

                // Asignar los valores de las celdas de la fila a los TextBox
                guna2TextBox5.Text = selectedRow.Cells["UsuarioID"].Value.ToString();  // Ajusta el nombre de la columna
            }
        }

        private void Usuarios_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string searchTerm = guna2TextBox4.Text.Trim();

            // Si el cuadro de texto está vacío, mostrar todos los usuarios
            if (string.IsNullOrEmpty(searchTerm))
            {
                CargarDatosUsuarios();  // Cargar todos los datos de nuevo
                return;
            }

            // Cadena de conexión
            string connectionString = "Server=JEFFERSON\\SQLEXPRESS;Database=WOLFSFITNESSMARKET;Integrated Security=True;";

            // Consulta SQL para buscar solo por UsuarioID
            string query = "SELECT * FROM Usuarios WHERE UsuarioID = @SearchTerm";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Verificar si el término de búsqueda es un número (ID)
                        if (int.TryParse(searchTerm, out int usuarioId))
                        {
                            // Si es un número, lo buscamos por ID
                            command.Parameters.AddWithValue("@SearchTerm", usuarioId);  // Buscar por ID

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
                            // Si no es un número, mostrar un mensaje de error
                            MessageBox.Show("Por favor, ingresa un ID válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            guna2DataGridView1.DataSource = null;  // Limpiar la vista del DataGridView si no es un ID válido
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al realizar la búsqueda: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {


        }

        private void guna2DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Verificamos que la celda modificada no sea una de las cabeceras o celdas vacías
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Obtener los valores de la fila y columna editada
                int usuarioId = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["UsuarioID"].Value);
                string nombre = guna2DataGridView1.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                string correo = guna2DataGridView1.Rows[e.RowIndex].Cells["Correo"].Value.ToString();
                string contrasena = guna2DataGridView1.Rows[e.RowIndex].Cells["Contrasena"].Value.ToString();
                string rol = guna2DataGridView1.Rows[e.RowIndex].Cells["Rol"].Value.ToString();

                // Mostrar un mensaje de confirmación
                DialogResult result = MessageBox.Show("¿Estás seguro de que deseas actualizar este usuario?",
                                                      "Confirmar actualización",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);

                // Si el usuario hace clic en 'Sí', proceder con la actualización
                if (result == DialogResult.Yes)
                {
                    // Cadena de conexión
                    string connectionString = "Server=JEFFERSON\\SQLEXPRESS;Database=WOLFSFITNESSMARKET;Integrated Security=True;";

                    // Consulta SQL para actualizar el usuario
                    string query = "UPDATE Usuarios SET Nombre = @Nombre, Correo = @Correo, Contrasena = @Contrasena, Rol = @Rol WHERE UsuarioID = @UsuarioID";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                // Agregar los parámetros de la consulta
                                command.Parameters.AddWithValue("@UsuarioID", usuarioId);
                                command.Parameters.AddWithValue("@Nombre", nombre);
                                command.Parameters.AddWithValue("@Correo", correo);
                                command.Parameters.AddWithValue("@Contrasena", contrasena);
                                command.Parameters.AddWithValue("@Rol", rol);

                                // Ejecutar la consulta
                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Usuario actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    CargarDatosUsuarios();  // Recargar los datos actualizados
                                }
                                else
                                {
                                    MessageBox.Show("No se pudo actualizar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ocurrió un error al actualizar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    // Si el usuario hace clic en 'No', deshacer el cambio en la celda
                    guna2DataGridView1.CancelEdit();
                    CargarDatosUsuarios();

                }

            }

        }

        private void guna2TextBox4_Click(object sender, EventArgs e)
        {
            guna2TextBox4.Clear();
        }
    }
}
