using Datos;
using Entidades;
using System;
using System.Windows.Forms;

namespace Vista
{
    public partial class LoginFrom : Form
    {
        public LoginFrom()
        {


            InitializeComponent();

        }

        private void CancelarButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AceptarButton_Click(object sender, EventArgs e)
        {
            if (UsuarioTtxtBox.Text == string.Empty)
            {
                errorProvider1.SetError(UsuarioTtxtBox, "ingrese un usuario");
                UsuarioTtxtBox.Focus();
                return;
            }
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(UsuarioTtxtBox.Text))
            {
                errorProvider1.SetError(ContraeñaTextBox, "ingrese una contraseña");
                ContraeñaTextBox.Focus();

                return;
            }
            errorProvider1.Clear();
            // validar en la basede datos

            Login login = new Login(UsuarioTtxtBox.Text, ContraeñaTextBox.Text);
            Usuario usuario = new Usuario();
            UsuarioDB usuarioDB = new UsuarioDB();

            usuario = usuarioDB.Autenticar(login);

            if (usuario != null)
            {
                if (usuario.EstaActivo)
                {
                    // Mostrar el menu 
                    Menu menuFormulario = new Menu();
                    this.Hide();
                    menuFormulario.Show();

                }
                else
                {
                    MessageBox.Show("El usuario no esta activo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }


            }
            else
            {
                MessageBox.Show("Datos de usuario incorrecto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }

        private void MostrarContraseñaButton_Click(object sender, EventArgs e)
        {
            if (ContraeñaTextBox.PasswordChar == '*')
            {
                ContraeñaTextBox.PasswordChar = '\0';
            }
            else
            {
                ContraeñaTextBox.PasswordChar = '*';
            }
        }
    }
}
