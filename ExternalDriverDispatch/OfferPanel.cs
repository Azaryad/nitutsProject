using System;
using System.Data;
using System.Windows.Forms;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// Offer management screen (full CRUD). Lifecycle actions (accept / reject) are
    /// exposed as dedicated verb buttons (state machine).
    /// </summary>
    public partial class OfferPanel : UserControl
    {
        private Offer selected;

        public OfferPanel()
        {
            InitializeComponent();
            UiTheme.Apply(this);
            refreshTripCombo();
            refreshDriverCombo();
            foreach (OfferStatus st in Enum.GetValues(typeof(OfferStatus)))
                comboBox_status.Items.Add(OfferStatusHelper.ToDisplay(st));
            comboBox_status.SelectedIndex = 0;
            loadGrid();
        }

        private void refreshTripCombo()
        {
            comboBox_trip.Items.Clear();
            foreach (Trip t in Program.Trips)
                comboBox_trip.Items.Add(t.getId() + " - " + t.getExternalBookingId());
            if (comboBox_trip.Items.Count > 0) comboBox_trip.SelectedIndex = 0;
        }

        private void refreshDriverCombo()
        {
            comboBox_driver.Items.Clear();
            foreach (ExternalDriver d in Program.ExternalDrivers)
                comboBox_driver.Items.Add(d.getId() + " - " + d.getName());
            if (comboBox_driver.Items.Count > 0) comboBox_driver.SelectedIndex = 0;
        }

        private void loadGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Trip", typeof(string));
            dt.Columns.Add("Driver", typeof(string));
            dt.Columns.Add("Sent", typeof(string));
            dt.Columns.Add("Expires", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Rank", typeof(int));

            foreach (Offer o in Program.Offers)
            {
                string tripLabel = o.getTrip() != null ? o.getTrip().getExternalBookingId() : "";
                string driverLabel = o.getDriver() != null ? o.getDriver().getName() : "";
                dt.Rows.Add(o.getId(), tripLabel, driverLabel,
                    o.getSentAt().ToString("dd/MM HH:mm"), o.getExpiresAt().ToString("dd/MM HH:mm"),
                    OfferStatusHelper.ToDisplay(o.getStatus()), o.getRankPosition());
            }

            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
            selected = Offer.seekOffer(id);
            if (selected != null)
            {
                textBox_id.Text = selected.getId().ToString();
                comboBox_trip.SelectedIndex = Program.Trips.IndexOf(selected.getTrip());
                comboBox_driver.SelectedIndex = Program.ExternalDrivers.IndexOf(selected.getDriver());
                dateTimePicker_sent.Value = selected.getSentAt();
                dateTimePicker_expires.Value = selected.getExpiresAt();
                comboBox_status.Text = OfferStatusHelper.ToDisplay(selected.getStatus());
                textBox_rank.Text = selected.getRankPosition().ToString();
                textBox_reply.Text = selected.getDriverReplyText() ?? "";
                textBox_ai.Text = selected.getAiInterpretation() ?? "";
            }
        }

        private bool readInputs(out Trip trip, out ExternalDriver driver, out int rank)
        {
            trip = null; driver = null; rank = 0;

            if (comboBox_trip.SelectedIndex < 0 || comboBox_trip.SelectedIndex >= Program.Trips.Count)
            {
                MessageBox.Show("Please select a trip", "Error", MessageBoxButtons.OK); return false;
            }
            trip = Program.Trips[comboBox_trip.SelectedIndex];

            if (comboBox_driver.SelectedIndex < 0 || comboBox_driver.SelectedIndex >= Program.ExternalDrivers.Count)
            {
                MessageBox.Show("Please select a driver", "Error", MessageBoxButtons.OK); return false;
            }
            driver = Program.ExternalDrivers[comboBox_driver.SelectedIndex];

            if (!int.TryParse(textBox_rank.Text, out rank) || rank <= 0)
            {
                MessageBox.Show("Rank is invalid", "Error", MessageBoxButtons.OK); return false;
            }
            return true;
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            Trip trip; ExternalDriver driver; int rank;
            if (!readInputs(out trip, out driver, out rank)) return;

            int id = Offer.getNextOfferId();
            OfferStatus st = OfferStatusHelper.FromDisplay(comboBox_status.Text);
            string reply = string.IsNullOrWhiteSpace(textBox_reply.Text) ? null : textBox_reply.Text;
            string ai = string.IsNullOrWhiteSpace(textBox_ai.Text) ? null : textBox_ai.Text;
            new Offer(id, trip, driver, dateTimePicker_sent.Value, dateTimePicker_expires.Value,
                st, reply, ai, rank, true);
            loadGrid();
            clearForm();
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Please select an offer from the list", "Error", MessageBoxButtons.OK); return;
            }
            Trip trip; ExternalDriver driver; int rank;
            if (!readInputs(out trip, out driver, out rank)) return;

            selected.setTrip(trip);
            selected.setDriver(driver);
            selected.setSentAt(dateTimePicker_sent.Value);
            selected.setExpiresAt(dateTimePicker_expires.Value);
            selected.setStatus(OfferStatusHelper.FromDisplay(comboBox_status.Text));
            selected.setRankPosition(rank);
            selected.setDriverReplyText(string.IsNullOrWhiteSpace(textBox_reply.Text) ? null : textBox_reply.Text);
            selected.setAiInterpretation(string.IsNullOrWhiteSpace(textBox_ai.Text) ? null : textBox_ai.Text);
            selected.updateOffer();
            loadGrid();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Please select an offer from the list", "Error", MessageBoxButtons.OK); return;
            }
            selected.deleteOffer();
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
            textBox_rank.Clear();
            textBox_reply.Clear();
            textBox_ai.Clear();
            if (comboBox_trip.Items.Count > 0) comboBox_trip.SelectedIndex = 0;
            if (comboBox_driver.Items.Count > 0) comboBox_driver.SelectedIndex = 0;
            comboBox_status.SelectedIndex = 0;
        }

        // ----- State-machine verb buttons -----

        private void button_accept_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Please select an offer from the list", "Error", MessageBoxButtons.OK); return;
            }
            if (selected.accept()) loadGrid();
        }

        private void button_reject_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Please select an offer from the list", "Error", MessageBoxButtons.OK); return;
            }
            if (selected.reject()) loadGrid();
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new DispatcherHomePanel());
        }
    }
}
