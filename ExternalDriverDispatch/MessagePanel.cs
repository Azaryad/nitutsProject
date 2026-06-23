using System;
using System.Data;
using System.Windows.Forms;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// Messages screen — the WhatsApp conversation audit trail (read-mostly).
    /// Rows are produced by the dispatch flow (outbound offer texts + inbound replies);
    /// this screen lists them and allows deletion. No add/edit form — messages are not
    /// authored by hand.
    /// </summary>
    public partial class MessagePanel : UserControl
    {
        private Message selected;

        public MessagePanel()
        {
            InitializeComponent();
            UiTheme.Apply(this);
            loadGrid();
        }

        private void loadGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("Driver", typeof(string));
            dt.Columns.Add("Direction", typeof(string));
            dt.Columns.Add("Offer", typeof(string));
            dt.Columns.Add("Body", typeof(string));
            dt.Columns.Add("WA Id", typeof(string));

            foreach (Message m in Program.Messages)
            {
                string driver = m.getDriver() != null ? m.getDriver().getName() : "";
                string offer = m.getOffer() != null ? ("#" + m.getOffer().getId()) : "";
                dt.Rows.Add(m.getId(), m.getTimestamp().ToString("dd/MM HH:mm:ss"), driver,
                    MessageDirectionHelper.ToDisplay(m.getDirection()), offer, m.getBody(),
                    m.getWaMessageId() ?? "");
            }
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
            selected = Message.seekMessage(id);
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            loadGrid();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Please select a message from the list", "Error", MessageBoxButtons.OK); return;
            }
            selected.deleteMessage();
            selected = null;
            loadGrid();
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new DispatcherHomePanel());
        }
    }
}
