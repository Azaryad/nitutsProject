using System;
using System.Windows.Forms;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// הטופס הראשי — חלון יחיד שמחליף תוכן בתוכו.
    /// כל המסכים במערכת הם UserControl שנטענים לתוך panelMain.
    /// המסך הראשון הוא מסך הכניסה (LoginPanel).
    /// </summary>
    public partial class mainForm : Form
    {
        // הפניה סטטית לטופס הראשי — כדי שכל מסך יוכל לנווט
        private static mainForm instance;

        // טיימר דדליין: בודק כל דקה אם נסיעה כלשהי חצתה את סף 6 השעות לפני האיסוף ללא שיבוץ.
        // Forms.Timer פועם על ה-UI thread, ולכן בטוח לגעת ברשימות שבזיכרון, ב-DB וב-MessageBox.
        // הערה: פועל רק כשהאפליקציה פתוחה (אין מתזמן מערכת-הפעלה).
        private readonly System.Windows.Forms.Timer deadlineTimer = new System.Windows.Forms.Timer();

        public mainForm()
        {
            InitializeComponent();
            instance = this;
            UiTheme.Apply(this);
            deadlineTimer.Interval = 60_000;   // every minute
            deadlineTimer.Tick += deadlineTimer_Tick;
            deadlineTimer.Start();
            showPanel(new LoginPanel());
        }

        private void deadlineTimer_Tick(object sender, EventArgs e)
        {
            DispatchService.EscalateOverdueTrips();   // notifies the dispatcher per escalated trip
        }

        /// <summary>
        /// החלפת המסך הנוכחי במסך חדש. זו הדרך היחידה לנווט בין מסכים.
        /// שימוש: mainForm.showPanel(new MyPanel());
        /// </summary>
        public static void showPanel(UserControl panel)
        {
            instance.panelMain.Controls.Clear();
            panel.Dock = DockStyle.Fill;
            instance.panelMain.Controls.Add(panel);
        }
    }
}
