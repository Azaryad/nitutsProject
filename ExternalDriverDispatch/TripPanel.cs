using System;
using System.Data;
using System.Windows.Forms;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// Trip management screen (full CRUD). Lifecycle actions (offer / confirm / requeue)
    /// are exposed as dedicated verb buttons (state machine).
    /// </summary>
    public partial class TripPanel : UserControl
    {
        private Trip selected;

        public TripPanel()
        {
            InitializeComponent();
            UiTheme.Apply(this);
            refreshRegionCombo();
            foreach (VehicleType vt in Enum.GetValues(typeof(VehicleType)))
                comboBox_vehicleType.Items.Add(VehicleTypeHelper.ToDisplay(vt));
            comboBox_vehicleType.SelectedIndex = 0;
            foreach (TripStatus st in Enum.GetValues(typeof(TripStatus)))
                comboBox_status.Items.Add(TripStatusHelper.ToDisplay(st));
            comboBox_status.SelectedIndex = 0;
            loadGrid();
        }

        private void refreshRegionCombo()
        {
            comboBox_region.Items.Clear();
            // the combo index matches the index in Program.Regions
            foreach (Region r in Program.Regions)
                comboBox_region.Items.Add(r.getName());
            if (comboBox_region.Items.Count > 0)
                comboBox_region.SelectedIndex = 0;
        }

        private void loadGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Booking", typeof(string));
            dt.Columns.Add("Region", typeof(string));
            dt.Columns.Add("Origin", typeof(string));
            dt.Columns.Add("Destination", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("Pax", typeof(int));
            dt.Columns.Add("Vehicle", typeof(string));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("Status", typeof(string));

            foreach (Trip t in Program.Trips)
            {
                string regionName = t.getRegion() != null ? t.getRegion().getName() : "";
                dt.Rows.Add(t.getId(), t.getExternalBookingId(), regionName,
                    t.getPickupCity(), t.getDropoffCity(), t.getPickupTime().ToString("dd/MM/yyyy HH:mm"),
                    t.getNumPassengers(), VehicleTypeHelper.ToDisplay(t.getVehicleType()),
                    t.getPriceToDriver(), TripStatusHelper.ToDisplay(t.getStatus()));
            }

            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
            selected = Trip.seekTrip(id);
            if (selected != null)
            {
                textBox_id.Text = selected.getId().ToString();
                textBox_bookingId.Text = selected.getExternalBookingId();
                comboBox_region.SelectedIndex = Program.Regions.IndexOf(selected.getRegion());
                textBox_pickupCity.Text = selected.getPickupCity();
                textBox_pickupAddress.Text = selected.getPickupAddress();
                textBox_dropoffCity.Text = selected.getDropoffCity();
                textBox_dropoffAddress.Text = selected.getDropoffAddress();
                dateTimePicker_pickup.Value = selected.getPickupTime();
                textBox_passengers.Text = selected.getNumPassengers().ToString();
                comboBox_vehicleType.Text = VehicleTypeHelper.ToDisplay(selected.getVehicleType());
                textBox_price.Text = selected.getPriceToDriver().ToString("0.00");
                comboBox_status.Text = TripStatusHelper.ToDisplay(selected.getStatus());
            }
        }

        private bool readInputs(out Region region, out int passengers, out decimal price)
        {
            region = null; passengers = 0; price = 0m;

            if (string.IsNullOrWhiteSpace(textBox_bookingId.Text))
            {
                MessageBox.Show("Please enter a booking ID", "Error", MessageBoxButtons.OK); return false;
            }
            if (comboBox_region.SelectedIndex < 0 || comboBox_region.SelectedIndex >= Program.Regions.Count)
            {
                MessageBox.Show("Please select a region", "Error", MessageBoxButtons.OK); return false;
            }
            region = Program.Regions[comboBox_region.SelectedIndex];

            if (string.IsNullOrWhiteSpace(textBox_pickupCity.Text) ||
                string.IsNullOrWhiteSpace(textBox_pickupAddress.Text) ||
                string.IsNullOrWhiteSpace(textBox_dropoffCity.Text) ||
                string.IsNullOrWhiteSpace(textBox_dropoffAddress.Text))
            {
                MessageBox.Show("Please fill in pickup and dropoff city and address", "Error", MessageBoxButtons.OK); return false;
            }
            if (!int.TryParse(textBox_passengers.Text, out passengers) || passengers <= 0)
            {
                MessageBox.Show("Number of passengers is invalid", "Error", MessageBoxButtons.OK); return false;
            }
            if (!decimal.TryParse(textBox_price.Text, out price) || price < 0)
            {
                MessageBox.Show("Price is invalid", "Error", MessageBoxButtons.OK); return false;
            }
            return true;
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            Region region; int passengers; decimal price;
            if (!readInputs(out region, out passengers, out price)) return;

            int id = Trip.getNextTripId();
            VehicleType vt = VehicleTypeHelper.FromDisplay(comboBox_vehicleType.Text);
            TripStatus st = TripStatusHelper.FromDisplay(comboBox_status.Text);
            new Trip(id, textBox_bookingId.Text, textBox_pickupAddress.Text, textBox_dropoffAddress.Text,
                textBox_pickupCity.Text, textBox_dropoffCity.Text, dateTimePicker_pickup.Value, passengers,
                vt, price, st, DateTime.Now, region, true);
            loadGrid();
            clearForm();
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Please select a trip from the list", "Error", MessageBoxButtons.OK); return;
            }
            Region region; int passengers; decimal price;
            if (!readInputs(out region, out passengers, out price)) return;

            selected.setExternalBookingId(textBox_bookingId.Text);
            selected.setRegion(region);
            selected.setPickupCity(textBox_pickupCity.Text);
            selected.setPickupAddress(textBox_pickupAddress.Text);
            selected.setDropoffCity(textBox_dropoffCity.Text);
            selected.setDropoffAddress(textBox_dropoffAddress.Text);
            selected.setPickupTime(dateTimePicker_pickup.Value);
            selected.setNumPassengers(passengers);
            selected.setVehicleType(VehicleTypeHelper.FromDisplay(comboBox_vehicleType.Text));
            selected.setPriceToDriver(price);
            selected.setStatus(TripStatusHelper.FromDisplay(comboBox_status.Text));
            selected.updateTrip();
            loadGrid();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Please select a trip from the list", "Error", MessageBoxButtons.OK); return;
            }
            selected.deleteTrip();
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
            textBox_bookingId.Clear();
            textBox_pickupCity.Clear();
            textBox_pickupAddress.Clear();
            textBox_dropoffCity.Clear();
            textBox_dropoffAddress.Clear();
            textBox_passengers.Clear();
            textBox_price.Clear();
            if (comboBox_region.Items.Count > 0) comboBox_region.SelectedIndex = 0;
            comboBox_vehicleType.SelectedIndex = 0;
            comboBox_status.SelectedIndex = 0;
        }

        // ----- State-machine verb buttons -----

        private void button_offer_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Please select a trip from the list", "Error", MessageBoxButtons.OK); return;
            }
            if (selected.offer()) loadGrid();
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Please select a trip from the list", "Error", MessageBoxButtons.OK); return;
            }
            if (selected.confirm()) loadGrid();
        }

        private void button_requeue_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Please select a trip from the list", "Error", MessageBoxButtons.OK); return;
            }
            if (selected.requeue()) loadGrid();
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new DispatcherHomePanel());
        }
    }
}
