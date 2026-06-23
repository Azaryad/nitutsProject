using System;
using System.Data;
using System.Windows.Forms;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// External-driver management screen (full CRUD).
    /// </summary>
    public partial class ExternalDriverPanel : UserControl
    {
        private ExternalDriver selected;

        public ExternalDriverPanel()
        {
            InitializeComponent();
            UiTheme.Apply(this);
            foreach (VehicleType vt in Enum.GetValues(typeof(VehicleType)))
                comboBox_vehicleType.Items.Add(VehicleTypeHelper.ToDisplay(vt));
            comboBox_vehicleType.SelectedIndex = 0;
            loadGrid();
        }

        private void loadGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Code", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns.Add("City", typeof(string));
            dt.Columns.Add("Vehicle", typeof(string));
            dt.Columns.Add("Active", typeof(string));

            foreach (ExternalDriver d in Program.ExternalDrivers)
                dt.Rows.Add(d.getId(), d.getDrivercode(), d.getName(), d.getPhone(), d.getHomeCity(),
                    VehicleTypeHelper.ToDisplay(d.getVehicleType()), d.getActive() ? "Yes" : "No");

            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
            selected = ExternalDriver.seekExternalDriver(id);
            if (selected != null)
            {
                textBox_id.Text = selected.getId().ToString();
                textBox_drivercode.Text = selected.getDrivercode();
                textBox_name.Text = selected.getName();
                textBox_phone.Text = selected.getPhone();
                textBox_homeCity.Text = selected.getHomeCity();
                comboBox_vehicleType.Text = VehicleTypeHelper.ToDisplay(selected.getVehicleType());
                checkBox_shabbat.Checked = selected.getWorksShabbat();
                checkBox_nights.Checked = selected.getWorksNights();
                checkBox_long.Checked = selected.getWorksLongDistance();
                checkBox_active.Checked = selected.getActive();
            }
        }

        private bool validateInputs()
        {
            if (string.IsNullOrWhiteSpace(textBox_drivercode.Text))
            {
                MessageBox.Show("Please enter a driver code", "Error", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBox_name.Text))
            {
                MessageBox.Show("Please enter a driver name", "Error", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBox_phone.Text))
            {
                MessageBox.Show("Please enter a phone number", "Error", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBox_homeCity.Text))
            {
                MessageBox.Show("Please enter a home city", "Error", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            if (!validateInputs()) return;
            int id = ExternalDriver.getNextExternalDriverId();
            VehicleType vt = VehicleTypeHelper.FromDisplay(comboBox_vehicleType.Text);
            new ExternalDriver(id, textBox_drivercode.Text, textBox_name.Text, textBox_phone.Text,
                textBox_homeCity.Text, vt, checkBox_shabbat.Checked, checkBox_nights.Checked,
                checkBox_long.Checked, checkBox_active.Checked, true);
            loadGrid();
            clearForm();
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Please select a driver from the list", "Error", MessageBoxButtons.OK);
                return;
            }
            if (!validateInputs()) return;
            selected.setDrivercode(textBox_drivercode.Text);
            selected.setName(textBox_name.Text);
            selected.setPhone(textBox_phone.Text);
            selected.setHomeCity(textBox_homeCity.Text);
            selected.setVehicleType(VehicleTypeHelper.FromDisplay(comboBox_vehicleType.Text));
            selected.setWorksShabbat(checkBox_shabbat.Checked);
            selected.setWorksNights(checkBox_nights.Checked);
            selected.setWorksLongDistance(checkBox_long.Checked);
            selected.setActive(checkBox_active.Checked);
            selected.updateExternalDriver();
            loadGrid();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Please select a driver from the list", "Error", MessageBoxButtons.OK);
                return;
            }
            selected.deleteExternalDriver();
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
            textBox_drivercode.Clear();
            textBox_name.Clear();
            textBox_phone.Clear();
            textBox_homeCity.Clear();
            comboBox_vehicleType.SelectedIndex = 0;
            checkBox_shabbat.Checked = false;
            checkBox_nights.Checked = false;
            checkBox_long.Checked = false;
            checkBox_active.Checked = true;
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new DispatcherHomePanel());
        }
    }
}
