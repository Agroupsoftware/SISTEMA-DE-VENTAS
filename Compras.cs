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
using System.Configuration;


namespace WOLFSFITNESSMARKET
{

    public partial class Compras : Form
    {

        public Compras()
        {
            InitializeComponent();
        }
        public class DetalleCompra
        {
            public int ProductoID { get; set; }
            public string Nombre { get; set; }
            public int Cantidad { get; set; }
            public decimal PrecioUnitario { get; set; }
            public decimal Subtotal => Cantidad * PrecioUnitario;
        }


        // Variables globales
        private List<DetalleCompra> detalleCompras1 = new List<DetalleCompra>();
        private decimal totalCompra = 0;
        private string connectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        private void Compras_Load(object sender, EventArgs e)
        {

            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Estás seguro de que quieres cerrar?", "Confirmar cierre", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

           
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                int productoID;
                if (!int.TryParse(txtProductoID.Text, out productoID))
                {
                    MessageBox.Show("ID de producto inválido.");
                    return;
                }

                int cantidad;
                if (!int.TryParse(txtCantidad.Text, out cantidad))
                {
                    MessageBox.Show("Cantidad inválida.");
                    return;
                }

                decimal precioUnitario;
                if (!decimal.TryParse(txtPrecioUnitario.Text, out precioUnitario))
                {
                    MessageBox.Show("Precio unitario inválido.");
                    return;
                }

                string nombreProducto = ObtenerNombreProducto(productoID);

                if (nombreProducto == null)
                {
                    MessageBox.Show("El producto no existe.");
                    return;
                }

                var detalle = new DetalleCompra
                {
                    ProductoID = productoID,
                    Nombre = nombreProducto,
                    Cantidad = cantidad,
                    PrecioUnitario = precioUnitario
                };

                detalleCompras1.Add(detalle);
                totalCompra += detalle.Subtotal;


                ActualizarDataGrid();
                lblTotal.Text = totalCompra.ToString("C2");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el producto: " + ex.Message);
            }
        }
        private string ObtenerNombreProducto(int productoID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Nombre FROM Inventario WHERE ProductoID = @ProductoID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductoID", productoID);
                return cmd.ExecuteScalar()?.ToString();
            }
        }
        private void ActualizarDataGrid()
        {
            dgvDetalleCompras.DataSource = null;  // Limpia el DataSource para evitar problemas de duplicado
            dgvDetalleCompras.DataSource = detalleCompras1;  // Asigna la lista correcta
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

            if (dgvDetalleCompras.CurrentRow != null)
            {
                var detalle = (DetalleCompra)dgvDetalleCompras.CurrentRow.DataBoundItem;
                detalleCompras1.Remove(detalle);
                totalCompra -= detalle.Subtotal;

                ActualizarDataGrid();
                lblTotal.Text = totalCompra.ToString("C2");
            }
        }
        private void LimpiarFormulario()
        {
            txtProveedor.Clear();
            txtUsuario.Clear();
            cmbTipoPago.SelectedIndex = -1;
            lblTotal.Text = "0.00";

            detalleCompras1.Clear();
            ActualizarDataGrid();
            totalCompra = 0;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtProveedor.Text) || string.IsNullOrEmpty(txtUsuario.Text) || detalleCompras1.Count == 0)
            {
                MessageBox.Show("Por favor, complete todos los campos antes de finalizar la compra.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string queryCompra = "INSERT INTO Compras (ProveedorID, UsuarioID, FechaCompra, TotalCompra, TipoPago, Estado) OUTPUT INSERTED.CompraID VALUES (@ProveedorID, @UsuarioID, GETDATE(), @TotalCompra, @TipoPago, 'Pendiente')";
                    SqlCommand cmdCompra = new SqlCommand(queryCompra, conn, transaction);
                    cmdCompra.Parameters.AddWithValue("@ProveedorID", int.Parse(txtProveedor.Text));
                    cmdCompra.Parameters.AddWithValue("@UsuarioID", int.Parse(txtUsuario.Text));
                    cmdCompra.Parameters.AddWithValue("@TotalCompra", totalCompra);
                    cmdCompra.Parameters.AddWithValue("@TipoPago", cmbTipoPago.SelectedItem.ToString());

                    int compraID = (int)cmdCompra.ExecuteScalar();

                    foreach (var detalle in detalleCompras1)
                    {
                        string queryDetalle = "INSERT INTO DetalleCompras (CompraID, ProductoID, Cantidad, PrecioUnitario) VALUES (@CompraID, @ProductoID, @Cantidad, @PrecioUnitario)";
                        SqlCommand cmdDetalle = new SqlCommand(queryDetalle, conn, transaction);
                        cmdDetalle.Parameters.AddWithValue("@CompraID", compraID);
                        cmdDetalle.Parameters.AddWithValue("@ProductoID", detalle.ProductoID);
                        cmdDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                        cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);

                        cmdDetalle.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Compra registrada exitosamente.");
                    LimpiarFormulario();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error al guardar la compra: " + ex.Message);
                }
            }
            }
    }
}
