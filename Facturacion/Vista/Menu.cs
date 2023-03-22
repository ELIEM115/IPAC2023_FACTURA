using System.Windows.Forms;

namespace Vista
{
    public partial class Menu : Form
    {
        public Menu()

        {

            InitializeComponent();
        }

        private void UsuariosToolStripButton_Click(object sender, System.EventArgs e)
        {
            UsuariosFrom userFrom = new UsuariosFrom();
            userFrom.MdiParent = this;
            userFrom.Show();
        }

        private void ProductosToolStripButton_Click(object sender, System.EventArgs e)
        {
            ProductosForm productosForm = new ProductosForm();
            productosForm.MdiParent = this;
            productosForm.Show();
        }



        private void ClientesToolStripButton_Click(object sender, System.EventArgs e)
        {
            ClienteForm clienteForm = new ClienteForm();
            clienteForm.MdiParent = this;
            clienteForm.Show();

        }
    }
}
