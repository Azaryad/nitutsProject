using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// Driver Performance report (Phase 8) — read-only.
    /// Per driver over an optional region + date range: offers received,
    /// accepted / rejected / timed-out counts, acceptance rate, and average
    /// response time (derived from the inbound WhatsApp audit trail).
    ///
    /// All aggregation lives in sp_DriverPerformance (JOIN + GROUP BY); this
    /// panel only gathers the filter values and binds the result set to the
    /// grid. There are no Save/Update/Delete actions — reports are read-only.
    /// DB-only: it calls no external service.
    /// </summary>
    public partial class DriverPerformancePanel : UserControl
    {
        // Parallel to comboRegion's items. Index 0 ("All regions") maps to null.
        private readonly List<int?> regionIds = new List<int?>();

        public DriverPerformancePanel()
        {
            InitializeComponent();
            UiTheme.Apply(this);
            populateRegions();
            loadReport();   // initial run: all regions, all dates
        }

        /// <summary>Fill the region filter from the in-memory list, with an "All regions" sentinel first.</summary>
        private void populateRegions()
        {
            comboRegion.Items.Clear();
            regionIds.Clear();

            comboRegion.Items.Add("All regions");
            regionIds.Add(null);

            foreach (Region r in Program.Regions)
            {
                comboRegion.Items.Add(r.getName());
                regionIds.Add(r.getId());
            }
            comboRegion.SelectedIndex = 0;
        }

        /// <summary>Call the report SP with the current filters and bind the rows.</summary>
        private void loadReport()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXECUTE sp_DriverPerformance @region_id, @from, @to";

            int idx = comboRegion.SelectedIndex;
            int? regionId = (idx >= 0 && idx < regionIds.Count) ? regionIds[idx] : null;

            // Unchecked DateTimePickers (ShowCheckBox) mean "no bound" -> NULL, matching the SP defaults.
            cmd.Parameters.AddWithValue("@region_id", (object)regionId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@from", dtpFrom.Checked ? (object)dtpFrom.Value.Date : DBNull.Value);
            cmd.Parameters.AddWithValue("@to",   dtpTo.Checked   ? (object)dtpTo.Value.Date   : DBNull.Value);

            SQL_CON SC = new SQL_CON();
            SqlDataReader rdr = SC.execute_query(cmd);
            if (rdr == null) return;   // execute_query already showed the error

            DataTable dt = new DataTable();
            dt.Load(rdr);
            rdr.Close();

            dataGridView1.DataSource = dt;
            applyColumnHeaders();
            label_summary.Text = dt.Rows.Count + " driver(s)";
        }

        /// <summary>Friendly headers for the SP's column aliases (logic columns are untouched).</summary>
        private void applyColumnHeaders()
        {
            setHeader("DriverCode", "Code");
            setHeader("DriverName", "Driver");
            setHeader("HomeCity", "Home city");
            setHeader("OffersReceived", "Offers");
            setHeader("Accepted", "Accepted");
            setHeader("Rejected", "Rejected");
            setHeader("TimedOut", "Timed out");
            setHeader("AcceptanceRatePct", "Accept %");
            setHeader("AvgResponseMinutes", "Avg resp (min)");
        }

        private void setHeader(string col, string text)
        {
            if (dataGridView1.Columns.Contains(col))
                dataGridView1.Columns[col].HeaderText = text;
        }

        private void button_generate_Click(object sender, EventArgs e)
        {
            loadReport();
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new DispatcherHomePanel());
        }
    }
}
