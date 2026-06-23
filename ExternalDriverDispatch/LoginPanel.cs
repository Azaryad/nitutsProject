using System;
using System.Windows.Forms;

namespace ExternalDriverDispatch
{
    /// <summary>
    /// Dispatcher login screen.
    ///
    /// Note: authentication is not a functional requirement — it is an NFR, and the class
    /// diagram has no credential-bearing entity (email + password). This is therefore a
    /// placeholder login for demo purposes only: a fixed dev password. It can later be
    /// replaced by a real authentication source.
    /// </summary>
    public partial class LoginPanel : UserControl
    {
        // temporary dev password (demo only)
        private const string DEV_PASSWORD = "1234";

        public LoginPanel()
        {
            InitializeComponent();
            UiTheme.Apply(this);
        }

        private void enter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_user.Text))
            {
                MessageBox.Show("Please enter a username", "Error", MessageBoxButtons.OK);
                return;
            }
            if (!textBox_password.Text.Equals(DEV_PASSWORD))
            {
                MessageBox.Show("Incorrect password", "Error", MessageBoxButtons.OK);
                return;
            }
            // The dispatcher is the only human actor — go to the dispatch board (routine flow)
            mainForm.showPanel(new DispatchBoardPanel());
        }

        // Dev shortcut to bypass the login screen during development/demo
        private void devEnter_Click(object sender, EventArgs e)
        {
            mainForm.showPanel(new DispatchBoardPanel());
        }
    }
}
