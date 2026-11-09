using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bagay_Week05_Work04
{
    public partial class ProductForm : Form
    {
        private List<Product> products = new List<Product>();
        public ProductForm()
        {
            InitializeComponent();
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
            txtCost.TextChanged += Pricing_TextChanged;
            txtPrice.TextChanged += Pricing_TextChanged;
            txtCost.KeyPress += NumericOnly_KeyPress;
            txtPrice.KeyPress += NumericOnly_KeyPress;
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            cmbCategory.Items.AddRange(new string[]
                {
                    "Electronics",
                    "Clothing",
                    "Food & Beverages",
                    "Home & Appliances",
                    "School Supplies",
                    "Others"
                });

            cmbCategory.SelectedIndex = -1;

            txtBarcode.Text = GenerateBarcode();
            lblBarcodeStatus.Text = "Barcode generated automatically.";
        }

        private void Pricing_TextChanged(object sender, EventArgs e)
        {
            decimal cost;
            decimal price;

            if (decimal.TryParse(txtCost.Text, out cost) &&
                decimal.TryParse(txtPrice.Text, out price) &&
                cost > 0)
            {
                decimal markup = ((price - cost) / cost) * 100;

                txtMarkup.Text = markup.ToString("0.00");
            }
            else
            {
                txtMarkup.Clear();
            }
        }

        private bool ValidateProductInputs()
        {
            if (string.IsNullOrWhiteSpace(txtBarcode.Text))
            {
                MessageBox.Show("Barcode is required.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtBarcode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter the product name.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Please enter the product description.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtDescription.Focus();
                return false;
            }

            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a product category.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbCategory.Focus();
                return false;
            }

            if (!rdoLocal.Checked && !rdoAbroad.Checked)
            {
                MessageBox.Show("Please select the product origin.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                grpOrigin.Focus();
                return false;
            }

            decimal cost;

            if (!decimal.TryParse(txtCost.Text, out cost) || cost <= 0)
            {
                MessageBox.Show("Please enter a valid cost greater than zero.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCost.Focus();
                return false;
            }

            decimal price;

            if (!decimal.TryParse(txtPrice.Text, out price) || price < 0)
            {
                MessageBox.Show("Please enter a valid price that is zero or greater.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPrice.Focus();
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateProductInputs())
            {
                return;
            }

            decimal cost = decimal.Parse(txtCost.Text);
            decimal price = decimal.Parse(txtPrice.Text);

            decimal markup = ((price - cost) / cost) * 100;

            Product newProduct = new Product()
            {
                Barcode = txtBarcode.Text,
                Name = txtName.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                Category = cmbCategory.SelectedItem.ToString(),
                Origin = rdoLocal.Checked ? "Local" : "Abroad",
                IsActive = chkActive.Checked,
                Cost = cost,
                Price = price,
                Markup = markup
            };

            products.Add(newProduct);

            MessageBox.Show(
                "Product saved successfully!\n\n" +
                "Barcode: " + newProduct.Barcode +
                "\nName: " + newProduct.Name +
                "\nCategory: " + newProduct.Category +
                "\nOrigin: " + newProduct.Origin +
                "\nCost: " + newProduct.Cost.ToString("N2") +
                "\nPrice: " + newProduct.Price.ToString("N2") +
                "\nMarkup: " + newProduct.Markup.ToString("N2") + "%",
                "Product Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            ClearProductForm();
        }

        private void ClearProductForm()
        {
            txtBarcode.Text = GenerateBarcode();

            txtName.Clear();
            txtDescription.Clear();

            cmbCategory.SelectedIndex = -1;

            rdoLocal.Checked = true;
            chkActive.Checked = true;

            txtCost.Clear();
            txtPrice.Clear();
            txtMarkup.Clear();

            lblBarcodeStatus.Text = "Barcode generated automatically.";

            txtName.Focus();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to clear all fields?",
                "Confirm Clear",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                ClearProductForm();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void NumericOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            if (char.IsDigit(e.KeyChar))
            {
                return;
            }

            char decimalSeparator =
                System.Globalization.CultureInfo.CurrentCulture
                .NumberFormat.NumberDecimalSeparator[0];

            if (e.KeyChar == decimalSeparator &&
                !textBox.Text.Contains(decimalSeparator.ToString()))
            {
                return;
            }

            e.Handled = true;
        }

        private string GenerateBarcode()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
    }
}
