using Datos;
using Entidades;
using System;
using System.Data;
using System.Windows.Forms;

namespace Vista
{
    public partial class ClienteForm : Syncfusion.Windows.Forms.Office2010Form
    {
        public ClienteForm()
        {
            InitializeComponent();
        }
        string tipoOperacion;

        DataTable dt = new DataTable();
        ClienteDB ClienteDB = new ClienteDB();
        Cliente user = new Cliente();
        private void HabilitarControles()
        {
            IdentidadTextBox.Enabled = true;
            NombreTextBox.Enabled = true;
            TelefonoTextBox.Enabled = true;
            CorreoTextBox.Enabled = true;
            DireccionTextBox1.Enabled = true;
            FechaNacimientoDateTimePicker.Enabled = true;
            EstaActivoCheckBox.Enabled = true;
            GuardarButton.Enabled = true;
            CancelarButton.Enabled = true;
            ModificarButton.Enabled = false;
        }

        private void DeshabilitarControles()
        {
            IdentidadTextBox.Enabled = false;
            NombreTextBox.Enabled = false;
            TelefonoTextBox.Enabled = false;
            CorreoTextBox.Enabled = false;
            DireccionTextBox1.Enabled = false;
            FechaNacimientoDateTimePicker.Enabled = false;
            EstaActivoCheckBox.Enabled = false;
            GuardarButton.Enabled = false;
            CancelarButton.Enabled = false;
            ModificarButton.Enabled = true;
        }

        private void LimpiarControles()
        {
            IdentidadTextBox.Clear();
            NombreTextBox.Clear();
            TelefonoTextBox.Clear();
            CorreoTextBox.Clear();
            DireccionTextBox1.Clear();
            FechaNacimientoDateTimePicker.Value = DateTime.Now;
            EstaActivoCheckBox.Checked = false;

        }

        private void NuevoButton_Click(object sender, EventArgs e)
        {

            IdentidadTextBox.Focus();
            HabilitarControles();
            tipoOperacion = "Nuevo";

        }

        private void ModificarButton_Click(object sender, EventArgs e)
        {
            tipoOperacion = "Modificar";
            if (ClienteDataGridView.SelectedRows.Count > 0)
            {
                IdentidadTextBox.Text = ClienteDataGridView.CurrentRow.Cells["Identidad"].Value.ToString();
                NombreTextBox.Text = ClienteDataGridView.CurrentRow.Cells["Nombre"].Value.ToString();
                TelefonoTextBox.Text = ClienteDataGridView.CurrentRow.Cells["Telefono"].Value.ToString();
                CorreoTextBox.Text = ClienteDataGridView.CurrentRow.Cells["Correo"].Value.ToString();
                DireccionTextBox1.Text = ClienteDataGridView.CurrentRow.Cells["Direccion"].Value.ToString();
                // FechaNacimientoDateTimePicker.Text = ClienteDataGridView.CurrentRow.Cells["FechaNacimiento"].Value.ToString();
                EstaActivoCheckBox.Checked = Convert.ToBoolean(ClienteDataGridView.CurrentRow.Cells["EstaActivo"].Value);

                HabilitarControles();
            }
            else
            {
                MessageBox.Show("Debe seleccionar un registro");
            }



        }

        private void GuardarButton_Click(object sender, EventArgs e)
        {
            if (tipoOperacion == "Nuevo")
            {
                if (string.IsNullOrEmpty(IdentidadTextBox.Text))
                {
                    errorProvider1.SetError(IdentidadTextBox, "Ingrese un numero de Identidad");
                    IdentidadTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();
                if (string.IsNullOrEmpty(NombreTextBox.Text))
                {
                    errorProvider1.SetError(NombreTextBox, "Ingrese un nombre");
                    NombreTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();
                if (string.IsNullOrEmpty(TelefonoTextBox.Text))
                {
                    errorProvider1.SetError(TelefonoTextBox, "Ingrese un numero de telefono");
                    TelefonoTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();
                if (string.IsNullOrEmpty(CorreoTextBox.Text))
                {
                    errorProvider1.SetError(CorreoTextBox, "Ingrese un correo");
                    CorreoTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();
                if (string.IsNullOrEmpty(DireccionTextBox1.Text))
                {
                    errorProvider1.SetError(DireccionTextBox1, "Ingrese una Direccion");
                    TelefonoTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();
                if (string.IsNullOrEmpty(FechaNacimientoDateTimePicker.Text))
                {
                    errorProvider1.SetError(FechaNacimientoDateTimePicker, "Ingrese un numero de telefono");
                    FechaNacimientoDateTimePicker.Focus();
                    return;
                }
                errorProvider1.Clear();


                user.Identidad = IdentidadTextBox.Text;
                user.Nombre = NombreTextBox.Text;
                user.Telefono = TelefonoTextBox.Text;
                user.Correo = CorreoTextBox.Text;
                user.Direccion = DireccionTextBox1.Text;
                user.FechaNacimiento = FechaNacimientoDateTimePicker.Value;
                user.EstaActivo = EstaActivoCheckBox.Checked;



                //Insertar en la base de datos

                bool inserto = ClienteDB.Insertar(user);

                if (inserto)
                {
                    LimpiarControles();
                    DeshabilitarControles();
                    TraerCliente();
                    MessageBox.Show("Registro Guardado");
                }
                else
                {
                    MessageBox.Show("No se pudo guardar el registro");
                }
            }
            else if (tipoOperacion == "Modificar")
            {
                user.Identidad = IdentidadTextBox.Text;
                user.Nombre = NombreTextBox.Text;
                user.Telefono = TelefonoTextBox.Text;
                user.Correo = CorreoTextBox.Text;
                user.Direccion = DireccionTextBox1.Text;
                user.FechaNacimiento = FechaNacimientoDateTimePicker.Value;
                user.EstaActivo = EstaActivoCheckBox.Checked;



                bool modifico = ClienteDB.Editar(user);
                if (modifico)
                {
                    LimpiarControles();
                    DeshabilitarControles();
                    TraerCliente();
                    MessageBox.Show("Registro actualizado correctamente");
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el registro");
                }

            }

        }

        private void EliminarButton_Click(object sender, EventArgs e)
        {
            if (ClienteDataGridView.SelectedRows.Count > 0)
            {
                DialogResult resultado = MessageBox.Show("Esta seguro de eliminar el registro", "Advertencia", MessageBoxButtons.YesNo);

                if (resultado == DialogResult.Yes)
                {
                    bool elimino = ClienteDB.Eliminar(ClienteDataGridView.CurrentRow.Cells["Identidad"].Value.ToString());

                    if (elimino)
                    {
                        LimpiarControles();
                        DeshabilitarControles();
                        TraerCliente();
                        MessageBox.Show("Registro Eliminado");
                    }
                    else
                    { MessageBox.Show("No se pudo eliminar el registro"); }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un registro");
            }

        }

        private void CancelarButton_Click(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LimpiarControles();
        }

        private void TraerCliente()
        {
            dt = ClienteDB.DevolverCliente();

            ClienteDataGridView.DataSource = dt;

        }

        private void ClienteForm_Load(object sender, EventArgs e)
        {
            DeshabilitarControles();
            TraerCliente();
        }
    }
}
