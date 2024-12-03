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
    public partial class Logofijocs :Form
    {
        public Logofijocs()
        {
            InitializeComponent();

            // Hacer que el formulario hijo no se pueda mover ni redimensionar
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ControlBox = false; // Elimina los botones de minimizar, maximizar y cerrar
            this.MaximizeBox = false; // Deshabilita la opción de maximizar
            this.MinimizeBox = false; // Deshabilita la opción de minimizar
        }
        // Sobrecargar el evento WndProc para bloquear la capacidad de mover el formulario
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
        private void Logofijocs_Load(object sender, EventArgs e)
        {

        }
        
    }
}

