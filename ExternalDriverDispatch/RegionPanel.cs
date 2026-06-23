using System;
using System.Data;
using System.Windows.Forms;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// Region management screen (full CRUD): list, view/edit, add, update, delete.
    /// </summary>
    public partial class RegionPanel : UserControl
    {
        private Region selected;  // the region currently selected in the list

        public RegionPanel()
        {
            InitializeComponent();
            UiTheme.Apply(this);
            loadGrid();
        }

        private void loadGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Country", typeof(string));
            dt.Columns.Add("City", typeof(string));
            dt.Columns.Add("Created", typeof(string));

            foreach (Region r in Program.Regions)
                dt.Rows.Add(r.getId(), r.getName(), r.getCountry(), r.getCity(),
                    r.getCreatedAt().ToString("dd/MM/yyyy"));

            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
            // Control already has a 'Region' property — call the entity type fully-qualified
            selected = ExternalDriverDispatch.Region.seekRegion(id);
            if (selected != null)
            {
                textBox_id.Text = selected.getId().ToString();
                textBox_name.Text = selected.getName();
                textBox_country.Text = selected.getCountry();
                textBox_city.Text = selected.getCity();
            }
        }

        private bool validateInputs()
        {
            if (string.IsNullOrWhiteSpace(textBox_name.Text))
            {
                MessageBox.Show("Please enter a region name", "Error", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBox_country.Text))
            {
                MessageBox.Show("Please enter a country", "Error", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBox_city.Text))
            {
                MessageBox.Show("Please enter a city", "Error", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            if (!validateInputs()) return;
            int id = ExternalDriverDispatch.Region.getNextRegionId();
            new Region(id, textBox_name.Text, textBox_country.Text, textBox_city.Text, DateTime.Now, true);
            loadGrid();
            clearForm();
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Please select a region from the list", "Error", MessageBoxButtons.OK);
                return;
            }
            if (!validateInputs()) return;
            selected.setName(textBox_name.Text);
            selected.setCountry(textBox_country.Text);
            selected.setCity(textBox_city.Text);
            selected.updateRegion();
            loadGrid();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Please select a region from the list", "Error", MessageBoxButtons.OK);
                return;
            }
            selected.deleteRegion();
            loadGrid();
            clearForm();
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            clearForm();
        }

        private void clearForm()
        {
            selected = null;
            textBox_id.Clear();
            textBox_name.Clear();
            textBox_country.Clear();
            textBox_city.Clear();
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new DispatcherHomePanel());
        }
    }
}
