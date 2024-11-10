using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WOLFSFITNESSMARKET
{
    public partial class Menu : MetroFramework.Forms.MetroForm
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Usuarios formUsuarios = new Usuarios();
            formUsuarios.Show();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            Clientes formclientes = new Clientes();
            formclientes.Show();

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Proveedores formProveedores = new Proveedores();
            formProveedores.Show();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Inventario formInventario = new Inventario();
            formInventario.Show();
        }
    }
}
