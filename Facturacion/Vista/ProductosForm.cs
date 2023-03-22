using Datos;
using Entidades;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vista
{
    public partial class ProductosForm : Syncfusion.Windows.Forms.Office2010Form
    {
        public ProductosForm()
        {
            InitializeComponent();
        }

        string operacion;
        Producto producto = new Producto();
        ProductoDB productoDB = new ProductoDB();

        private void NuevoButton_Click(object sender, EventArgs e)
        {
            operacion = "Nuevo";
            HabilitarControles();

        }
        private void CancelarButton_Click(object sender, EventArgs e)
        {
            DeshabilitarControles();
        }
        private void HabilitarControles()
        {
            CodigoTextBox.Enabled = true;
            DescripcionTextBox.Enabled = true;
            ExistenciaTextBox.Enabled = true;
            PrecioTextBox.Enabled = true;
            AdjuntarImagenBbutton.Enabled = true;
            GuardarButton.Enabled = true;
            CancelarButton.Enabled = true;
            NuevoButton.Enabled = false;
        }
        private void LimpiarControles()
        {
            CodigoTextBox.Clear();
            DescripcionTextBox.Clear();
            ExistenciaTextBox.Clear();
            PrecioTextBox.Clear();
            EstaActivoCheckBox.Checked = false;
            ImagenPictureBox.Image = null;
        }
        private void DeshabilitarControles()
        {
            CodigoTextBox.Enabled = false;
            DescripcionTextBox.Enabled = false;
            ExistenciaTextBox.Enabled = false;
            PrecioTextBox.Enabled = false;
            AdjuntarImagenBbutton.Enabled = false;
            GuardarButton.Enabled = false;
            CancelarButton.Enabled = false;
            NuevoButton.Enabled = true;
        }

        private void Modificarutton_Click(object sender, EventArgs e)
        {
            operacion = "Modificar";
            if (ProductoDataGridView.SelectedRows.Count > 0)
            {
                CodigoTextBox.Text = ProductoDataGridView.CurrentRow.Cells["Codigo"].Value.ToString();
                DescripcionTextBox.Text = ProductoDataGridView.CurrentRow.Cells["Descripcion"].Value.ToString();
                ExistenciaTextBox.Text = ProductoDataGridView.CurrentRow.Cells["Existencia"].Value.ToString();
                PrecioTextBox.Text = ProductoDataGridView.CurrentRow.Cells["Precio"].Value.ToString();
                EstaActivoCheckBox.Checked = Convert.ToBoolean(ProductoDataGridView.CurrentRow.Cells["EstaActivo"].Value);


                byte[] img = productoDB.DevolverFoto(ProductoDataGridView.CurrentRow.Cells["Codigo"].Value.ToString());
                if (img.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(img);
                    ImagenPictureBox.Image = System.Drawing.Bitmap.FromStream(ms);
                }
                HabilitarControles();
                CodigoTextBox.ReadOnly = true;
            }

            else
            {
                MessageBox.Show("Debe seleccionar un registro");
            }
        }

        private void GuardarButton_Click(object sender, EventArgs e)
        {
            producto = new Producto();
            producto.Codigo = CodigoTextBox.Text;
            producto.Descripcion = DescripcionTextBox.Text;
            producto.Precio = Convert.ToDecimal(PrecioTextBox.Text);
            producto.Existencia = Convert.ToInt32(ExistenciaTextBox.Text);
            producto.EstaActivo = EstaActivoCheckBox.Checked;

            if (ImagenPictureBox.Image != null)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                ImagenPictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                producto.Foto = ms.GetBuffer();
            }

            if (operacion == "Nuevo")
            {
                if (string.IsNullOrEmpty(CodigoTextBox.Text))
                {
                    errorProvider1.SetError(CodigoTextBox, "Ingrese un código");
                    CodigoTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(DescripcionTextBox.Text))
                {
                    errorProvider1.SetError(DescripcionTextBox, "Ingrese una descripción");
                    DescripcionTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(ExistenciaTextBox.Text))
                {
                    errorProvider1.SetError(ExistenciaTextBox, "Ingrese una existencia");
                    ExistenciaTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(PrecioTextBox.Text))
                {
                    errorProvider1.SetError(PrecioTextBox, "Ingrese un precio");
                    PrecioTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                bool inserto = productoDB.Insertar(producto);
                if (inserto)
                {
                    DeshabilitarControles();
                    LimpiarControles();
                    TraerProducto();
                    MessageBox.Show("Registro guardado con exito", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo guardar el registro", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (operacion == "Modificar")
            {
                bool modifico = productoDB.Editar(producto);
                if (modifico)
                {
                    CodigoTextBox.ReadOnly = false;
                    DeshabilitarControles();
                    LimpiarControles();
                    TraerProducto();
                    MessageBox.Show("Registro actualizado con exito", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se puro actualizar el registro", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void ExistenciaTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {

            }
            else
            {
                e.Handled = true;
            }
        }

        private void PrecioTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '.') && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void AdjuntarImagenBbutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult resultado = dialog.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                ImagenPictureBox.Image = Image.FromFile(dialog.FileName);
            }
        }

        private void ProductosForm_Load(object sender, EventArgs e)
        {
            TraerProducto();
        }

        private void TraerProducto()
        {
            ProductoDataGridView.DataSource = productoDB.DevolverProducto();
        }

        private void EliminarButton_Click(object sender, EventArgs e)
        {
            if (ProductoDataGridView.SelectedRows.Count > 0)
            {
                DialogResult resultado = MessageBox.Show("Esta seguro de eliminar el registro", "Advertencia", MessageBoxButtons.YesNo);

                if (resultado == DialogResult.Yes)
                {
                    bool elimino = productoDB.Eliminar(ProductoDataGridView.CurrentRow.Cells["Codido"].Value.ToString());

                    if (elimino)
                    {
                        LimpiarControles();
                        DeshabilitarControles();
                        TraerProducto();
                        MessageBox.Show("Registro Eliminado");
                    }
                    else
                    { MessageBox.Show("No se pudo eliminar el registro"); }
                }
            }
        }
    }
}
