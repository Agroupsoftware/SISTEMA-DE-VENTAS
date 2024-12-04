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
            Usuario usuarios = new Usuario
            {
                MdiParent = this, // Establecer el formulario principal como contenedor
                StartPosition = FormStartPosition.Manual,
                Location = new Point(0, this.MainMenuStrip != null ? this.MainMenuStrip.Height : 0) // Posicionarlo debajo del menú
            };

            usuarios.Show();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cliente clientes = new Cliente
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
            Inventary inventario = new Inventary
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
            Proveedore proveedores = new Proveedore
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
