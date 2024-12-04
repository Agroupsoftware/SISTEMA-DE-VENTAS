using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace WOLFSFITNESSMARKET
{
    public partial class Usuario : Form
    {
        public Usuario()
        {
            InitializeComponent();
        }

        private void Usuario_Load(object sender, EventArgs e)
        {
            CargarDatosUsuarios();
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

        private void CargarDatosUsuarios()
        {
            // Cadena de conexión con autenticación de Windows
            string connectionString =@"Server=DESKTOP-5G47S2B\SQLEXPRESS;Database=WOLFSFITNESSMARKET;Integrated Security=True;";


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
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    MessageBox.Show("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidarCampos()
        {
            // Validamos que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        // Método para limpiar los campos
        private void LimpiarCampos()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            comboBox1.SelectedIndex = -1; // Desmarca la opción seleccionada en el ComboBox
        }


        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Estás seguro de que quieres cerrar?", "Confirmar cierre", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            // Cadena de conexión
            string connectionString =@"Server=DESKTOP-5G47S2B\SQLEXPRESS;Database=WOLFSFITNESSMARKET;Integrated Security=True;";

            // Variables para capturar los datos del usuario
            string nombre = textBox2.Text;
            string correo = textBox3.Text;
            string contrasena = textBox4.Text;
            string rol = comboBox1.SelectedItem?.ToString();  // Usamos el operador null-conditional para evitar null referencia

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

        private void button2_Click(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado un usuario
            if (string.IsNullOrWhiteSpace(textBox1.Text))  // Suponiendo que guna2TextBox5 contiene el ID del usuario
            {
                MessageBox.Show("Por favor, seleccione un usuario para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirmar la eliminación
            DialogResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar este usuario?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                // Cadena de conexión
                string connectionString =@"Server=DESKTOP-5G47S2B\SQLEXPRESS;Database=WOLFSFITNESSMARKET;Integrated Security=True;";

                // Obtener el ID del usuario seleccionado
                int usuarioId = Convert.ToInt32(textBox1.Text);

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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificamos que se haya hecho clic en una fila válida (no en los encabezados de columna)
            if (e.RowIndex >= 0)
            {
                // Obtener la fila seleccionada
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // Asignar los valores de las celdas de la fila a los TextBox
                textBox1.Text = selectedRow.Cells["UsuarioID"].Value.ToString();  // Ajusta el nombre de la columna
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Verificamos que la celda modificada no sea una de las cabeceras o celdas vacías
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Obtener los valores de la fila y columna editada
                int usuarioId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["UsuarioID"].Value);
                string nombre = dataGridView1.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                string correo = dataGridView1.Rows[e.RowIndex].Cells["Correo"].Value.ToString();
                string contrasena = dataGridView1.Rows[e.RowIndex].Cells["Contrasena"].Value.ToString();
                string rol = dataGridView1.Rows[e.RowIndex].Cells["Rol"].Value.ToString();

                // Mostrar un mensaje de confirmación
                DialogResult result = MessageBox.Show("¿Estás seguro de que deseas actualizar este usuario?",
                                                      "Confirmar actualización",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);

                // Si el usuario hace clic en 'Sí', proceder con la actualización
                if (result == DialogResult.Yes)
                {
                    // Cadena de conexión
                    string connectionString =@"Server=DESKTOP-5G47S2B\SQLEXPRESS;Database=WOLFSFITNESSMARKET;Integrated Security=True;";

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
                    dataGridView1.CancelEdit();
                    CargarDatosUsuarios();

                }

            }

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            // Cadena de conexión a tu base de datos
            string connectionString =@"Server=DESKTOP-5G47S2B\SQLEXPRESS;Database=WOLFSFITNESSMARKET;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Consulta SQL dinámica
                string query = @"
        SELECT *
        FROM Usuarios
        WHERE 
            (CAST(UsuarioID AS NVARCHAR) LIKE '%' + @Busqueda + '%') OR
            (Nombre LIKE '%' + @Busqueda + '%') OR
            (Correo LIKE '%' + @Busqueda + '%') OR
            (contrasena LIKE '%' + @Busqueda + '%') OR
            (Rol LIKE '%' + @Busqueda + '%') OR
            (CONVERT(NVARCHAR, FechaCreacion, 120) LIKE '%' + @Busqueda + '%');
        ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Parámetro para la búsqueda
                    command.Parameters.AddWithValue("@Busqueda", textBox6.Text);

                    // Abrir la conexión
                    connection.Open();

                    // Ejecutar el comando y llenar el DataTable con los resultados
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable results = new DataTable();
                    adapter.Fill(results);

                    // Mostrar los resultados en un DataGridView (ajusta el nombre del control según tu diseño)
                    dataGridView1.DataSource = results;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            CargarDatosUsuarios();
        }
    }
}
