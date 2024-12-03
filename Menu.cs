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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();

          
          

            // Crear y mostrar el formulario hijo
            Logofijocs hijo = new Logofijocs            {
                MdiParent = this, // Establecer el formulario principal como contenedor
                StartPosition = FormStartPosition.Manual,
                Location = new Point(0, this.MainMenuStrip != null ? this.MainMenuStrip.Height : 0) // Posicionarlo debajo del menú
            };

            hijo.Show();
        }
        

        private void Form131_Load(object sender, EventArgs e)
        {
           
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Usuarios usuarios = new Usuarios
            {
                MdiParent = this, // Establecer el formulario principal como contenedor
                StartPosition = FormStartPosition.Manual,
                Location = new Point(0, this.MainMenuStrip != null ? this.MainMenuStrip.Height : 0) // Posicionarlo debajo del menú
            };

            usuarios.Show();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clientes clientes = new Clientes
            {
                MdiParent = this, // Establecer el formulario principal como contenedor
                StartPosition = FormStartPosition.Manual,
                Location = new Point(0, this.MainMenuStrip != null ? this.MainMenuStrip.Height : 0) // Posicionarlo debajo del menú
            };

            clientes.Show();
        }

        private void inventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Crear y mostrar el formulario hijo
            Inventario inventario = new Inventario
            {
                MdiParent = this, // Establecer el formulario principal como contenedor
                StartPosition = FormStartPosition.Manual,
                Location = new Point(0, this.MainMenuStrip != null ? this.MainMenuStrip.Height : 0) // Posicionarlo debajo del menú
            };

            inventario.Show();
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Crear y mostrar el formulario hijo
            Proveedores proveedores = new Proveedores
            {
                MdiParent = this, // Establecer el formulario principal como contenedor
                StartPosition = FormStartPosition.Manual,
                Location = new Point(0, this.MainMenuStrip != null ? this.MainMenuStrip.Height : 0) // Posicionarlo debajo del menú
            };

            proveedores.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
