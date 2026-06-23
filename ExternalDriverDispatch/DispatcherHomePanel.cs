using System;
using System.Windows.Forms;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// מסך ניהול הנתונים (CRUD) — אזור משני שאינו חלק מזרימת השגרה.
    /// נגיש מלוח השיבוץ דרך כפתור "ניהול נתונים", ומכאן חוזרים ללוח.
    /// כל כפתור פותח מסך ניהול (CRUD) של ישות במערכת.
    /// </summary>
    public partial class DispatcherHomePanel : UserControl
    {
        public DispatcherHomePanel()
        {
            InitializeComponent();
            UiTheme.Apply(this);
        }

        private void button_regions_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new RegionPanel());
        }

        private void button_drivers_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new ExternalDriverPanel());
        }

        private void button_trips_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new TripPanel());
        }

        private void button_offers_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new OfferPanel());
        }

        private void button_messages_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new MessagePanel());
        }

        private void button_settings_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new SettingsPanel());
        }

        private void button_report_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new DriverPerformancePanel());
        }

        private void button_logout_Click(object sender, EventArgs e)
        {
            // חזרה ללוח השיבוץ (זרימת השגרה)
            mainForm.showPanel(new DispatchBoardPanel());
        }
    }
}
